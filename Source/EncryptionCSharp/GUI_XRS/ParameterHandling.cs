using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using DatabaseConnection;
using Definitions;
using GUI_XRS.Datalogger;

namespace GUI_XRS
{
	// Token: 0x0200024E RID: 590
	public class ParameterHandling
	{
		// Token: 0x17000C0C RID: 3084
		// (get) Token: 0x06002CCD RID: 11469 RVA: 0x0012650D File Offset: 0x0012470D
		// (set) Token: 0x06002CCE RID: 11470 RVA: 0x00126515 File Offset: 0x00124715
		public bool EdgeFilterMode { get; set; }

		// Token: 0x17000C0D RID: 3085
		// (get) Token: 0x06002CCF RID: 11471 RVA: 0x00126520 File Offset: 0x00124720
		// (set) Token: 0x06002CD0 RID: 11472 RVA: 0x00126538 File Offset: 0x00124738
		public bool ChkParServer
		{
			get
			{
				return this.chkParServer;
			}
			set
			{
				this.chkParServer = value;
			}
		}

		// Token: 0x17000C0E RID: 3086
		// (get) Token: 0x06002CD1 RID: 11473 RVA: 0x00126544 File Offset: 0x00124744
		public bool ConnectionEstablished
		{
			get
			{
				return this.parameterTransfer.ConnectionEstablished;
			}
		}

		// Token: 0x140000FD RID: 253
		// (add) Token: 0x06002CD2 RID: 11474 RVA: 0x00126564 File Offset: 0x00124764
		// (remove) Token: 0x06002CD3 RID: 11475 RVA: 0x0012659C File Offset: 0x0012479C
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action Connected;

		// Token: 0x140000FE RID: 254
		// (add) Token: 0x06002CD4 RID: 11476 RVA: 0x001265D4 File Offset: 0x001247D4
		// (remove) Token: 0x06002CD5 RID: 11477 RVA: 0x0012660C File Offset: 0x0012480C
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action Disconnected;

		// Token: 0x140000FF RID: 255
		// (add) Token: 0x06002CD6 RID: 11478 RVA: 0x00126644 File Offset: 0x00124844
		// (remove) Token: 0x06002CD7 RID: 11479 RVA: 0x0012667C File Offset: 0x0012487C
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event EventHandler<ParameterChangeEventArgs> ParameterChanged;

		// Token: 0x14000100 RID: 256
		// (add) Token: 0x06002CD8 RID: 11480 RVA: 0x001266B4 File Offset: 0x001248B4
		// (remove) Token: 0x06002CD9 RID: 11481 RVA: 0x001266EC File Offset: 0x001248EC
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event EventHandler<ParameterChangeEventArgs> BeforeParameterChanged;

		// Token: 0x06002CDA RID: 11482 RVA: 0x00126724 File Offset: 0x00124924
		public ParameterHandling(DBConnection dbConnection, DBDataLogs dbDatalogs)
		{
			string text = "";
			this.dbConnection = dbConnection;
			this.dbDatalogs = dbDatalogs;
			dbConnection.GetIPElinos(ref text);
			bool flag = text != "";
			if (flag)
			{
				this.parameterTransfer = new ParameterDataTransfer(text);
			}
			else
			{
				this.parameterTransfer = new ParameterDataTransfer();
			}
			this.parameterTransfer.Connected += this.parameterTransfer_Connected;
			this.parameterTransfer.Disconnected += this.parameterTransfer_Disconnected;
			this.parameterTransfer.ParameterRead += this.parameterTransfer_ParameterRead;
		}

		// Token: 0x06002CDB RID: 11483 RVA: 0x00126E10 File Offset: 0x00125010
		public void StartConnectionTest()
		{
			new Thread(new ThreadStart(this.GetSignOfLive))
			{
				Name = "CheckParameterServer"
			}.Start();
		}

		// Token: 0x06002CDC RID: 11484 RVA: 0x00126E43 File Offset: 0x00125043
		private void parameterTransfer_Disconnected()
		{
			this.OnDisconnected();
		}

		// Token: 0x06002CDD RID: 11485 RVA: 0x00126E4D File Offset: 0x0012504D
		private void parameterTransfer_Connected()
		{
			this.OnConnected();
		}

		// Token: 0x06002CDE RID: 11486 RVA: 0x00126E58 File Offset: 0x00125058
		protected virtual void OnConnected()
		{
			Action connected = this.Connected;
			bool flag = connected != null;
			if (flag)
			{
				this.Connected();
			}
		}

		// Token: 0x06002CDF RID: 11487 RVA: 0x00126E84 File Offset: 0x00125084
		protected virtual void OnDisconnected()
		{
			Action disconnected = this.Disconnected;
			bool flag = disconnected != null;
			if (flag)
			{
				this.Disconnected();
			}
		}

		// Token: 0x06002CE0 RID: 11488 RVA: 0x00126EAD File Offset: 0x001250AD
		public void Connect()
		{
			this.parameterTransfer.StartConnect();
		}

		// Token: 0x06002CE1 RID: 11489 RVA: 0x00126EBC File Offset: 0x001250BC
		public void BuildTree(ObservableCollection<ParameterItem> items, bool showCheckbox, bool readOnly, Logger logger, List<string> defaultPaths, List<string> defaultValues, List<string> pathBeginningsToIgnore)
		{
			List<ParameterItem> list = new List<ParameterItem>();
			this.showCheckbox = showCheckbox;
			int num = 0;
			lock (this)
			{
				num = this.parameterTransfer.GetTotalEntryNumber();
			}
			bool flag2 = num != 0;
			if (flag2)
			{
				List<string> entries;
				lock (this)
				{
					entries = this.parameterTransfer.GetEntries(num);
				}
				entries.RemoveAll((string path) => !path.EndsWith("/Value"));
				entries.Sort(new ParameterPathComparer());
				for (int i = 0; i < entries.Count; i++)
				{
					bool flag4 = i > 0 && i % 5000 == 0;
					if (flag4)
					{
						Thread.Sleep(500);
					}
					bool flag5 = !this.IsPathToIgnore(entries[i], pathBeginningsToIgnore);
					if (flag5)
					{
						this.Add(entries[i], list, logger, null, readOnly);
					}
				}
			}
			bool flag6 = defaultPaths != null && defaultValues != null;
			if (flag6)
			{
				for (int j = 0; j < defaultPaths.Count; j++)
				{
					bool flag7 = j > 0 && j % 5000 == 0;
					if (flag7)
					{
						Thread.Sleep(500);
					}
					this.Add(defaultPaths[j], list, logger, defaultValues[j], readOnly);
				}
			}
			for (int k = 0; k < list.Count; k++)
			{
				this.AddDispatchedToList(list[k], items);
			}
			list.Clear();
		}

		// Token: 0x06002CE2 RID: 11490 RVA: 0x001270A4 File Offset: 0x001252A4
		public List<string> GetAllEntries()
		{
			List<string> result;
			lock (this)
			{
				List<string> list = new List<string>();
				int totalEntryNumber = this.parameterTransfer.GetTotalEntryNumber();
				bool flag2 = totalEntryNumber != 0;
				if (flag2)
				{
					list = this.parameterTransfer.GetEntries(totalEntryNumber);
				}
				result = list;
			}
			return result;
		}

		// Token: 0x06002CE3 RID: 11491 RVA: 0x00127110 File Offset: 0x00125310
		public void GetSignOfLive()
		{
			while (this.ChkParServer)
			{
				lock (this)
				{
					this.parameterTransfer.GetTotalEntryNumber();
				}
				Thread.Sleep(this.testFramePeriod);
			}
		}

		// Token: 0x06002CE4 RID: 11492 RVA: 0x00127170 File Offset: 0x00125370
		public void CloseConnection()
		{
			this.parameterTransfer.CloseConnection();
			this.ChkParServer = false;
		}

		// Token: 0x06002CE5 RID: 11493 RVA: 0x00127188 File Offset: 0x00125388
		protected virtual void OnParameterChanged(string path, string oldValue, string newValue)
		{
			EventHandler<ParameterChangeEventArgs> parameterChanged = this.ParameterChanged;
			bool flag = parameterChanged != null;
			if (flag)
			{
				parameterChanged(this, new ParameterChangeEventArgs(path, oldValue, newValue));
			}
		}

		// Token: 0x06002CE6 RID: 11494 RVA: 0x001271B8 File Offset: 0x001253B8
		protected virtual void OnBeforeParameterChanged(string path, string oldValue, string newValue)
		{
			EventHandler<ParameterChangeEventArgs> beforeParameterChanged = this.BeforeParameterChanged;
			bool flag = beforeParameterChanged != null;
			if (flag)
			{
				beforeParameterChanged(this, new ParameterChangeEventArgs(path, oldValue, newValue));
			}
		}

		// Token: 0x06002CE7 RID: 11495 RVA: 0x001271E8 File Offset: 0x001253E8
		public void Add(string path, List<ParameterItem> list, Logger logger, string value, bool readOnly)
		{
			string[] array = path.Split(new char[]
			{
				'/'
			});
			bool flag = list.Count == 0;
			if (flag)
			{
				list.Add(new ParameterItem(this)
				{
					Name = "/",
					IsExpanded = true,
					ReadAndWrite = !readOnly
				});
			}
			ObservableCollection<ParameterItem> childrenItem = list[0].ChildrenItem;
			for (int i = 1; i < array.Length - 2; i++)
			{
				int num = this.ContainsItem(childrenItem, array[i]);
				bool flag2 = num == -1;
				if (flag2)
				{
					childrenItem.Add(new ParameterItem(this, value != null, this.dbConnection, this.dbDatalogs));
					childrenItem[childrenItem.Count - 1].Name = array[i];
					childrenItem[childrenItem.Count - 1].ReadAndWrite = !readOnly;
					childrenItem = childrenItem[childrenItem.Count - 1].ChildrenItem;
				}
				else
				{
					childrenItem = childrenItem[num].ChildrenItem;
				}
			}
			ParameterItem parameterItem = new ParameterItem(this, value != null, this.dbConnection, this.dbDatalogs);
			parameterItem.Name = array[array.Length - 2];
			parameterItem.Root = "";
			for (int j = 0; j < array.Length - 3; j++)
			{
				ParameterItem parameterItem2 = parameterItem;
				parameterItem2.Root = parameterItem2.Root + "/" + array[j + 1];
			}
			parameterItem.IsCheckedVisible = this.showCheckbox;
			parameterItem.Datalogger = logger;
			parameterItem.ReadAndWrite = !readOnly;
			bool flag3 = this.showCheckbox;
			if (flag3)
			{
				bool flag4 = logger.Paths.Contains(parameterItem.Root + "/" + parameterItem.Name + "/Value");
				if (flag4)
				{
					parameterItem.SetIsChecked(true);
				}
			}
			bool flag5 = value != null;
			if (flag5)
			{
				parameterItem.Value = value;
				parameterItem.Type = "String";
			}
			childrenItem.Add(parameterItem);
		}

		// Token: 0x06002CE8 RID: 11496 RVA: 0x00127404 File Offset: 0x00125604
		private void AddDispatchedToList(ParameterItem item, ObservableCollection<ParameterItem> list)
		{
			bool flag = Application.Current != null;
			if (flag)
			{
				Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Send, new Action(delegate()
				{
					list.Add(item);
				}));
			}
		}

		// Token: 0x06002CE9 RID: 11497 RVA: 0x00127454 File Offset: 0x00125654
		public void SetToReadOnly(string[] paths, ObservableCollection<ParameterItem> list)
		{
			bool flag = list != null && paths != null;
			if (flag)
			{
				List<string> list2 = paths.ToList<string>();
				foreach (ParameterItem parameterItem in list)
				{
					bool isParameterItem = parameterItem.IsParameterItem;
					if (isParameterItem)
					{
						ParameterItem parameterItem2 = parameterItem;
						bool flag2 = list2.Contains(parameterItem2.Root + "/" + parameterItem2.Name + "/");
						if (flag2)
						{
							parameterItem2.ReadAndWrite = false;
						}
					}
					bool flag3 = parameterItem.ChildrenItem != null && parameterItem.ChildrenItem.Count > 0;
					if (flag3)
					{
						this.SetToReadOnly(paths, parameterItem.ChildrenItem);
					}
				}
			}
		}

		// Token: 0x06002CEA RID: 11498 RVA: 0x00127528 File Offset: 0x00125728
		public void SetCheckboxVisibility(ObservableCollection<ParameterItem> list, bool showCheckbox)
		{
			bool flag = list != null;
			if (flag)
			{
				foreach (ParameterItem parameterItem in list)
				{
					bool isParameterItem = parameterItem.IsParameterItem;
					if (isParameterItem)
					{
						ParameterItem parameterItem2 = parameterItem;
						parameterItem2.IsCheckedVisible = showCheckbox;
					}
					bool flag2 = parameterItem.ChildrenItem != null && parameterItem.ChildrenItem.Count > 0;
					if (flag2)
					{
						this.SetCheckboxVisibility(parameterItem.ChildrenItem, showCheckbox);
					}
				}
			}
		}

		// Token: 0x06002CEB RID: 11499 RVA: 0x0001092C File Offset: 0x0000EB2C
		public void SetIoMappings(ObservableCollection<ParameterItem> list, bool setDescription)
		{
		}

		// Token: 0x06002CEC RID: 11500 RVA: 0x001275C0 File Offset: 0x001257C0
		public void GetCertainItems(ObservableCollection<ParameterItem> list, string[] currentPathes, Logger logger, bool readOnly)
		{
			bool flag = list == null;
			if (flag)
			{
				list = new ObservableCollection<ParameterItem>();
			}
			List<ParameterItem> list2 = new List<ParameterItem>();
			for (int i = 0; i < currentPathes.Length; i++)
			{
				this.Add(currentPathes[i], list2, logger, null, readOnly);
			}
			for (int j = 0; j < list2.Count; j++)
			{
				this.AddDispatchedToList(list2[j], list);
			}
		}

		// Token: 0x06002CED RID: 11501 RVA: 0x00127634 File Offset: 0x00125834
		public string ReadPar(string path)
		{
			string result;
			lock (this)
			{
				string text = "";
				bool flag2 = path != null;
				if (flag2)
				{
					text = this.parameterTransfer.ReadEntry(path);
				}
				result = text;
			}
			return result;
		}

		// Token: 0x06002CEE RID: 11502 RVA: 0x00127690 File Offset: 0x00125890
		public void WriteValue(string path, string value)
		{
			this.WriteValue(path, value, true);
		}

		// Token: 0x06002CEF RID: 11503 RVA: 0x001276A0 File Offset: 0x001258A0
		public void WriteValue(string path, string value, bool reportChange)
		{
			lock (this)
			{
				bool flag2 = path != null && value != null;
				if (flag2)
				{
					string oldValue = this.parameterTransfer.ReadEntry(path);
					this.OnBeforeParameterChanged(path, oldValue, value);
					this.parameterTransfer.WriteEntry(path, value);
					if (reportChange)
					{
						this.OnParameterChanged(path, oldValue, value);
					}
				}
			}
		}

		// Token: 0x06002CF0 RID: 11504 RVA: 0x00127720 File Offset: 0x00125920
		public bool WriteValueAndCheckxTimes(string path, string value, bool reportChange, int xTimesToRepeatAndCheckForSuccess)
		{
			bool result = false;
			for (int i = 0; i < xTimesToRepeatAndCheckForSuccess; i++)
			{
				this.WriteValue(path, value, reportChange);
				string a = this.ReadPar(path);
				bool flag = a == value;
				if (flag)
				{
					result = true;
					break;
				}
				Thread.Sleep(200);
			}
			return result;
		}

		// Token: 0x06002CF1 RID: 11505 RVA: 0x00127780 File Offset: 0x00125980
		public void DisableWriteProtection()
		{
			lock (this)
			{
				this.parameterTransfer.DisableWriteProtection();
			}
		}

		// Token: 0x06002CF2 RID: 11506 RVA: 0x001277C8 File Offset: 0x001259C8
		public void EnableWriteProtection()
		{
			lock (this)
			{
				this.parameterTransfer.EnableWriteProtection();
			}
		}

		// Token: 0x06002CF3 RID: 11507 RVA: 0x00127810 File Offset: 0x00125A10
		public void Reboot()
		{
			lock (this)
			{
				this.parameterTransfer.Reboot();
			}
		}

		// Token: 0x06002CF4 RID: 11508 RVA: 0x00127858 File Offset: 0x00125A58
		private int ContainsItem(ObservableCollection<ParameterItem> list, string item)
		{
			for (int i = 0; i < list.Count; i++)
			{
				bool flag = list[i].Name == item;
				if (flag)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06002CF5 RID: 11509 RVA: 0x001278A0 File Offset: 0x00125AA0
		public List<string> GetPathes(List<string> saveValues)
		{
			List<string> allEntries = this.GetAllEntries();
			List<string> list = new List<string>();
			foreach (string text in allEntries)
			{
				bool flag = this.dbEntries.Contains(text);
				if (flag)
				{
					list.Add(text);
				}
				else
				{
					bool flag2 = (text.StartsWith(ParameterHandling.rejectersParentPath) || text.StartsWith("/filter") || text.StartsWith("/testcardSimulation")) && text.EndsWith("/Value");
					if (flag2)
					{
						bool flag3 = !this.ExcludePath(text);
						if (flag3)
						{
							list.Add(text);
						}
					}
				}
			}
			bool flag4 = saveValues != null;
			if (flag4)
			{
				foreach (string path in list)
				{
					saveValues.Add(this.ReadPar(path));
				}
			}
			return list;
		}

		// Token: 0x06002CF6 RID: 11510 RVA: 0x001279CC File Offset: 0x00125BCC
		public List<string> GetPathesAI()
		{
			List<string> allEntries = this.GetAllEntries();
			List<string> list = new List<string>();
			foreach (string text in allEntries)
			{
				bool flag = (text.StartsWith("/filter/anomalyDetection/inference") || text.StartsWith("/filter/anomalyDetection/train") || text.StartsWith("/filter/objectDetection/inference")) && text.EndsWith("/Value");
				if (flag)
				{
					bool flag2 = !this.ExcludePathAI(text);
					if (flag2)
					{
						list.Add(text);
					}
				}
			}
			return list;
		}

		// Token: 0x06002CF7 RID: 11511 RVA: 0x00127A84 File Offset: 0x00125C84
		public List<string> GetValuesAI(List<string> pathes)
		{
			List<string> list = new List<string>();
			for (int i = 0; i < pathes.Count; i++)
			{
				list.Add(this.ReadPar(pathes[i]));
			}
			return list;
		}

		// Token: 0x06002CF8 RID: 11512 RVA: 0x00127AC8 File Offset: 0x00125CC8
		private bool ExcludePath(string path)
		{
			bool flag = this.dbExcludeFilterEnding != null;
			if (flag)
			{
				foreach (string value in this.dbExcludeFilterEnding)
				{
					bool flag2 = path.EndsWith(value);
					if (flag2)
					{
						return true;
					}
				}
			}
			bool flag3 = this.dbExcludeFilterBeginning != null;
			if (flag3)
			{
				foreach (string value2 in this.dbExcludeFilterBeginning)
				{
					bool flag4 = path.StartsWith(value2);
					if (flag4)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06002CF9 RID: 11513 RVA: 0x00127B64 File Offset: 0x00125D64
		private bool ExcludePathAI(string path)
		{
			bool flag = this.dbExcludeFilterEndingAI != null;
			if (flag)
			{
				foreach (string value in this.dbExcludeFilterEndingAI)
				{
					bool flag2 = path.EndsWith(value);
					if (flag2)
					{
						return true;
					}
				}
				foreach (string value2 in this.dbExcludeFilterEnding)
				{
					bool flag3 = path.EndsWith(value2);
					if (flag3)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06002CFA RID: 11514 RVA: 0x00127BEE File Offset: 0x00125DEE
		public void AddGroupToParameterServer(int group)
		{
			this.WriteValue(Definition.addImageGroupPath, group.ToString(), false);
		}

		// Token: 0x06002CFB RID: 11515 RVA: 0x00127C05 File Offset: 0x00125E05
		public void RemoveGroupFromParameterServer(int group)
		{
			this.WriteValue(Definition.removeImageGroupPath, group.ToString(), false);
		}

		// Token: 0x06002CFC RID: 11516 RVA: 0x00127C1C File Offset: 0x00125E1C
		public void AddRegionToParameterServer(int region)
		{
			this.WriteValue(Definition.addRegionPath, region.ToString(), false);
		}

		// Token: 0x06002CFD RID: 11517 RVA: 0x00127C33 File Offset: 0x00125E33
		public void RemoveRegionFromParameterServer(int region)
		{
			this.WriteValue(Definition.removeRegionPath, region.ToString(), false);
		}

		// Token: 0x06002CFE RID: 11518 RVA: 0x0001092C File Offset: 0x0000EB2C
		private void parameterTransfer_ParameterRead(string parameter, int index, int numParameters)
		{
		}

		// Token: 0x06002CFF RID: 11519 RVA: 0x00127C4C File Offset: 0x00125E4C
		private bool IsPathToIgnore(string path, List<string> pathBeginningsToIgnore)
		{
			bool result = false;
			bool flag = pathBeginningsToIgnore != null;
			if (flag)
			{
				for (int i = 0; i < pathBeginningsToIgnore.Count; i++)
				{
					bool flag2 = path.StartsWith(pathBeginningsToIgnore[i]);
					if (flag2)
					{
						result = true;
						break;
					}
				}
			}
			return result;
		}

		// Token: 0x04001C12 RID: 7186
		private string[] dbEntries = new string[]
		{
			"/hardware/xraysource/CurrentSetpoint/Value",
			"/hardware/xraysource/VoltageSetpoint/Value",
			"/hardware/svt/Enable/Value",
			"/hardware/svt/ValvesEnable/Value",
			"/hardware/svt/HardwareDelay/Value",
			"/hardware/svt/SoftwareDelay/Value",
			"/hardware/svt/HardwareLength/Value",
			"/hardware/svt/SoftwareLength/Value",
			"/hardware/svt/PWMSpeedUpCount/Value",
			"/hardware/svt/WakeUpCheckPeriod/Value",
			"/hardware/svt/WakeUpPulseLength/Value",
			"/hardware/ios/VibroSpeed/Value"
		};

		// Token: 0x04001C13 RID: 7187
		private string[] dbExcludeFilterEnding = new string[]
		{
			"Autolearn/Value",
			"Retrain/Value",
			"Debug/Value",
			"Triggered/Value",
			"MinX/Value",
			"MaxX/Value",
			"MinY/Value",
			"MaxY/Value",
			"RetrainRegion/Value",
			"TriggerLevel/Value",
			"TrainToTrigger/Value",
			"DebugImageType/Value",
			"CurrentWeight/Value",
			"CountedObjects/Value",
			"CurrentObjectSize/Value",
			"CountedContaminations/Value",
			"CurrentHeight/Value",
			"CurrentWidth/Value",
			"ImageDelay/Value",
			"MeanValueLine/Value",
			"MeanValueLineHE/Value",
			"ResetCalibration/Value",
			"VerifyMissingObjects/Value",
			"VerifyInstantTime/Value",
			"BinFull/Value",
			"VerifyMeasuredCorrectedTime/Value",
			"VerifyReset/Value",
			"VerifyTrain/Value",
			"OffsetCalibrationValue/Value",
			"OffsetCalibrationIndex/Value",
			"GainCalibrationValue/Value",
			"GainCalibrationIndex/Value",
			"recognition/NLearn/Value",
			"recognition/LearnedObjectIndex/Value",
			"recognition/CurrentAverage/Value",
			"recognition/CurrentArea/Value",
			"recognition/CurrentObjectPixel/Value",
			"recognition/CurrentWidth/Value",
			"recognition/CurrentHeight/Value",
			"recognition/GroupAdd/Value",
			"recognition/GroupRemove/Value",
			"recognition/LastAddedGroup/Value",
			"recognition/NGroups/Value",
			"recognition/CurrentGradAverage/Value",
			"arc/ImageArcs/Value",
			"arc/SerialArcs/Value",
			"arc/NCriticalArcs/Value",
			"arc/CriticalArcsPeriod/Value",
			"rejecter1/VerifyTest/Value",
			"rejecter2/VerifyTest/Value",
			"rejecter3/VerifyTest/Value",
			"rejecter4/VerifyTest/Value",
			"buzzer/VerifyTest/Value",
			"rejecter1/CorrectedDelay/Value",
			"rejecter2/CorrectedDelay/Value",
			"rejecter3/CorrectedDelay/Value",
			"rejecter4/CorrectedDelay/Value",
			"buzzer/CorrectedDelay/Value",
			"rejecter1/CorrectedLength/Value",
			"rejecter2/CorrectedLength/Value",
			"rejecter3/CorrectedLength/Value",
			"rejecter4/CorrectedLength/Value",
			"buzzer/CorrectedLength/Value",
			"rejecter1/PreRejectTime/Value",
			"rejecter2/PreRejectTime/Value",
			"rejecter3/PreRejectTime/Value",
			"rejecter4/PreRejectTime/Value",
			"buzzer/PreRejectTime/Value",
			"rejecter1/LightBarrier/Value",
			"rejecter2/LightBarrier/Value",
			"rejecter3/LightBarrier/Value",
			"rejecter4/LightBarrier/Value",
			"buzzer/LightBarrier/Value",
			"rejecter1/HLVersion/Value",
			"rejecter1/HLE_ProductsMissing/Value",
			"rejecter1/HLE_SensorBlockedPhotogating/Value",
			"rejecter1/HLE_SensorBlockedRejectCheck/Value",
			"rejecter1/HLCountsPhotogating/Value",
			"rejecter1/HLCountsRejectCheck/Value",
			"rejecter1/HLCountsRejectVerify/Value",
			"rejecter1/HLE_Consistency/Value",
			"rejecter1/HLE_BinMissing/Value",
			"rejecter1/HLSCInOutDelayTrained/Value",
			"rejecter1/HLSCInOutDelayTrain/Value",
			"rejecter1/HLErrorReset/Value",
			"rejecter1/HLBinUnlock/Value",
			"rejecter1/HLBinTimeout/Value",
			"rejecter1/HLEFPusherSignal/Value",
			"rejecter1/HLEFPusherLength/Value",
			"rejecter1/HLE_ProductNotInspected/Value",
			"rejecter2/HLVersion/Value",
			"rejecter2/HLE_ProductsMissing/Value",
			"rejecter2/HLE_SensorBlockedPhotogating/Value",
			"rejecter2/HLE_SensorBlockedRejectCheck/Value",
			"rejecter2/HLCountsPhotogating/Value",
			"rejecter2/HLCountsRejectCheck/Value",
			"rejecter2/HLCountsRejectVerify/Value",
			"rejecter2/HLE_Consistency/Value",
			"rejecter2/HLE_BinMissing/Value",
			"rejecter2/HLSCInOutDelayTrained/Value",
			"rejecter2/HLSCInOutDelayTrain/Value",
			"rejecter2/HLErrorReset/Value",
			"rejecter2/HLBinUnlock/Value",
			"rejecter2/HLBinTimeout/Value",
			"rejecter2/HLEFPusherSignal/Value",
			"rejecter2/HLEFPusherLength/Value",
			"rejecter2/HLE_ProductNotInspected/Value",
			"rejecter1/Output/Value",
			"rejecter2/Output/Value",
			"rejecter3/Output/Value",
			"rejecter4/Output/Value",
			"buzzer/Output/Value",
			"rejecter1/IsBuzzer/Value",
			"rejecter2/IsBuzzer/Value",
			"rejecter3/IsBuzzer/Value",
			"rejecter4/IsBuzzer/Value",
			"buzzer/IsBuzzer/Value",
			"rejecter1/TurnOnIfError/Value",
			"rejecter2/TurnOnIfError/Value",
			"rejecter3/TurnOnIfError/Value",
			"rejecter4/TurnOnIfError/Value",
			"rejecter1/VerifyLowActive/Value",
			"rejecter2/VerifyLowActive/Value",
			"rejecter3/VerifyLowActive/Value",
			"rejecter4/VerifyLowActive/Value",
			"objectBasedFlaps/Enabled/Value",
			"objectBasedFlaps/FlapsCount/Value",
			"objectBasedFlaps/BorderPixel1/Value",
			"objectBasedFlaps/BorderPixel2/Value",
			"objectBasedFlaps/BorderPixel3/Value",
			"objectBasedFlaps/CalcPixToFlapMapping/Value",
			"rejecter1/ObjBasRejEnabled/Value",
			"rejecter2/ObjBasRejEnabled/Value",
			"rejecter3/ObjBasRejEnabled/Value",
			"rejecter4/ObjBasRejEnabled/Value",
			"rejecter1/ObjBasRejPixMin/Value",
			"rejecter2/ObjBasRejPixMin/Value",
			"rejecter3/ObjBasRejPixMin/Value",
			"rejecter4/ObjBasRejPixMin/Value",
			"rejecter1/ObjBasRejPixMax/Value",
			"rejecter2/ObjBasRejPixMax/Value",
			"rejecter3/ObjBasRejPixMax/Value",
			"rejecter4/ObjBasRejPixMax/Value"
		};

		// Token: 0x04001C14 RID: 7188
		private string[] dbExcludeFilterBeginning = new string[]
		{
			"/filter/global/",
			"/filter/regionSelector/",
			"/filter/anomalyDetection",
			"/filter/objectDetection"
		};

		// Token: 0x04001C15 RID: 7189
		private string[] dbExcludeFilterEndingAI = new string[]
		{
			"ModelID/Value",
			"ErrorCode/Value",
			"inference/ProcessingTime/Value",
			"train/FineTuningEnabled/Value",
			"train/TrainingInProgress/Value",
			"train/NumberImagesFineTuning/Value",
			"train/SendImages/Value",
			"train/Finished/Value",
			"train/EstimatedTimeTraining/Value",
			"ModelType/Value"
		};

		// Token: 0x04001C16 RID: 7190
		private readonly int testFramePeriod = 2000;

		// Token: 0x04001C17 RID: 7191
		public static readonly string continuousModePath = "/filter/acquisition/ContinuousMode/Value";

		// Token: 0x04001C18 RID: 7192
		public static readonly string ignoreLeftPixelPath = "/machine/IgnoredLeftPixel/Value";

		// Token: 0x04001C19 RID: 7193
		public static readonly string ignoreRightPixelPath = "/machine/IgnoredRightPixel/Value";

		// Token: 0x04001C1A RID: 7194
		public static readonly string rejectersParentPath = "/hardware/rejecter";

		// Token: 0x04001C1B RID: 7195
		public static readonly string highGainPixelPath = "/hardware/detector/dt/HighGain/Value";

		// Token: 0x04001C1C RID: 7196
		public static readonly string lowGainPixelPath = "/hardware/detector/dt/LowGain/Value";

		// Token: 0x04001C1D RID: 7197
		public static readonly string gainPixelPath = "/hardware/detector/sesotec/Gain/Value";

		// Token: 0x04001C1E RID: 7198
		public static readonly string temperatureDetectorPath = "/hardware/detector/TemperatureInUse/Value";

		// Token: 0x04001C20 RID: 7200
		private bool chkParServer = true;

		// Token: 0x04001C21 RID: 7201
		private bool showCheckbox;

		// Token: 0x04001C22 RID: 7202
		private ParameterDataTransfer parameterTransfer;

		// Token: 0x04001C23 RID: 7203
		private DBConnection dbConnection;

		// Token: 0x04001C24 RID: 7204
		private DBDataLogs dbDatalogs;
	}
}
