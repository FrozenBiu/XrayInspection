using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Controls;
using Common;
using GUI_XRS.Datalogger;

namespace GUI_XRS
{
	// Token: 0x02000134 RID: 308
	public class ParameterViewModel
	{
		// Token: 0x14000098 RID: 152
		// (add) Token: 0x06001DA5 RID: 7589 RVA: 0x000C1268 File Offset: 0x000BF468
		// (remove) Token: 0x06001DA6 RID: 7590 RVA: 0x000C12A0 File Offset: 0x000BF4A0
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action ItemsCreated;

		// Token: 0x170008AC RID: 2220
		// (get) Token: 0x06001DA7 RID: 7591 RVA: 0x000C12D8 File Offset: 0x000BF4D8
		// (set) Token: 0x06001DA8 RID: 7592 RVA: 0x000C12F0 File Offset: 0x000BF4F0
		public string[] CurrentPaths
		{
			get
			{
				return this.currentPaths;
			}
			set
			{
				this.currentPaths = value;
			}
		}

		// Token: 0x170008AD RID: 2221
		// (get) Token: 0x06001DA9 RID: 7593 RVA: 0x000C12FA File Offset: 0x000BF4FA
		// (set) Token: 0x06001DAA RID: 7594 RVA: 0x000C1302 File Offset: 0x000BF502
		public bool DockNumKeyboard { get; set; }

		// Token: 0x170008AE RID: 2222
		// (get) Token: 0x06001DAB RID: 7595 RVA: 0x000C130C File Offset: 0x000BF50C
		public ObservableCollection<ParameterItem> ParameterItems
		{
			get
			{
				return this.parameterItems;
			}
		}

		// Token: 0x170008AF RID: 2223
		// (get) Token: 0x06001DAC RID: 7596 RVA: 0x000C1324 File Offset: 0x000BF524
		public ObservableCollection<ParameterItem> ParameterOperatingHours
		{
			get
			{
				return this.parameterItemsOperatingHours;
			}
		}

		// Token: 0x170008B0 RID: 2224
		// (get) Token: 0x06001DAD RID: 7597 RVA: 0x000C133C File Offset: 0x000BF53C
		// (set) Token: 0x06001DAE RID: 7598 RVA: 0x000C1344 File Offset: 0x000BF544
		public RelayCommand ShowLicensesCommand { get; set; }

		// Token: 0x06001DAF RID: 7599 RVA: 0x000C1350 File Offset: 0x000BF550
		public ParameterViewModel(ParameterHandling parHandling, string[] currentPaths, bool readOnly, bool showCheckbox, Logger logger, bool showProgress, List<string> defaultPaths, List<string> defaultValues, List<string> pathBeginningsToIgnore)
		{
			this.Initialize(parHandling, currentPaths, readOnly, showCheckbox, logger, showProgress, defaultPaths, defaultValues, pathBeginningsToIgnore);
			this.ShowLicensesCommand = new RelayCommand(new Action<object>(this.ShowLicenses));
		}

		// Token: 0x06001DB0 RID: 7600 RVA: 0x000C13B4 File Offset: 0x000BF5B4
		private void ShowLicenses(object obj)
		{
			CtrlLicensesWindow usrControl = new CtrlLicensesWindow(this.parHandling);
			Subwindow.Show(usrControl);
		}

		// Token: 0x06001DB1 RID: 7601 RVA: 0x000C13D8 File Offset: 0x000BF5D8
		public void Initialize(ParameterHandling parHandling, Logger logger, bool showProgress, List<string> defaultPaths, List<string> defaultValues)
		{
			this.Initialize(parHandling, null, false, false, logger, showProgress, defaultPaths, defaultValues, null);
		}

		// Token: 0x06001DB2 RID: 7602 RVA: 0x000C13F8 File Offset: 0x000BF5F8
		public void Initialize(ParameterHandling parHandling, string[] currentPaths, bool readOnly, bool showCheckbox, Logger logger, bool showProgress, List<string> defaultPaths, List<string> defaultValues, List<string> pathBeginningsToIgnore)
		{
			this.DockNumKeyboard = false;
			bool flag = parHandling != null;
			if (flag)
			{
				this.parHandling = parHandling;
				this.currentPaths = currentPaths;
				this.readOnly = readOnly;
				this.showCheckbox = showCheckbox;
				this.logger = logger;
				this.defaultPaths = defaultPaths;
				this.defaultValues = defaultValues;
				this.showProgress = showProgress;
				this.pathBeginningsToIgnore = pathBeginningsToIgnore;
			}
		}

		// Token: 0x06001DB3 RID: 7603 RVA: 0x000C1460 File Offset: 0x000BF660
		public void StartCreation()
		{
			bool flag = this.showProgress;
			if (flag)
			{
				Progress.ShowStaticProgress(LocalizationManager.Localize("txtProcessing"), false);
			}
			this.bgWorkerBuildTree.DoWork += this.bgWorkerBuildTree_DoWork;
			this.bgWorkerBuildTree.RunWorkerCompleted += this.bgWorkerBuildTree_RunWorkerCompleted;
			this.bgWorkerBuildTree.RunWorkerAsync();
		}

		// Token: 0x06001DB4 RID: 7604 RVA: 0x000C14C5 File Offset: 0x000BF6C5
		public void ExpandAllItems()
		{
			this.ExpandAllItems(this.parameterItems);
		}

		// Token: 0x06001DB5 RID: 7605 RVA: 0x000C14D8 File Offset: 0x000BF6D8
		private void ExpandAllItems(ObservableCollection<ParameterItem> items)
		{
			foreach (ParameterItem parameterItem in items)
			{
				parameterItem.IsExpanded = true;
				bool flag = parameterItem.ChildrenItem != null && parameterItem.ChildrenItem.Count > 0;
				if (flag)
				{
					this.ExpandAllItems(parameterItem.ChildrenItem);
				}
			}
		}

		// Token: 0x06001DB6 RID: 7606 RVA: 0x000C1550 File Offset: 0x000BF750
		private void keyboard_TextInputKeyboard(object sender, KeyboardInputEventArgs e)
		{
			bool flag = this.pItem.Value != e.Text;
			if (flag)
			{
				LogbookManager.AddEntry(SSTEntryCode.ParameterChanged, 0, this.pItem.Root + "/" + this.pItem.Name, this.pItem.Value, e.Text);
			}
			this.pItem.Value = e.Text;
			bool flag2 = sender == this.keyboard;
			if (flag2)
			{
				this.keyboard.TextInputKeyboard -= this.keyboard_TextInputKeyboard;
			}
			else
			{
				bool flag3 = sender == this.numKeyboard;
				if (flag3)
				{
					this.numKeyboard.TextInputKeyboard -= this.keyboard_TextInputKeyboard;
				}
			}
			bool flag4 = this.pItem.Root + "/" + this.pItem.Name + "/Value" != XRayOnProcessing.xrayVoltagePath && this.pItem.Root + "/" + this.pItem.Name + "/Value" != "/hardware/xraysource/VoltageSetpoint/Value" && this.pItem.Root + "/" + this.pItem.Name + "/Value" != XRayOnProcessing.xrayCurrentPath && this.pItem.Root + "/" + this.pItem.Name + "/Value" != "/hardware/xraysource/CurrentSetpoint/Value";
			if (flag4)
			{
				this.parHandling.WriteValue(this.pItem.Root + "/" + this.pItem.Name + "/Value", e.Text);
			}
			else
			{
				this.parHandling.WriteValue(this.pItem.Root + "/" + this.pItem.Name + "/Value", e.Text.Replace(',', '.'));
			}
		}

		// Token: 0x06001DB7 RID: 7607 RVA: 0x000C1758 File Offset: 0x000BF958
		private void bgWorkerBuildTree_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			this.bgWorkerBuildTree.RunWorkerCompleted -= this.bgWorkerBuildTree_RunWorkerCompleted;
			Progress.CloseStaticProgress(false);
			bool flag = this.currentPaths != null;
			if (flag)
			{
				this.parHandling.SetCheckboxVisibility(this.parameterItems, this.showCheckbox);
			}
			this.OnItemsCreated();
			bool flag2 = this.currentPaths != null;
			if (flag2)
			{
				this.ExpandAllItems();
			}
		}

		// Token: 0x06001DB8 RID: 7608 RVA: 0x000C17C7 File Offset: 0x000BF9C7
		internal void ShowProgressBar()
		{
			Progress.ShowStaticProgress(LocalizationManager.Localize("txtProcessing"), false);
		}

		// Token: 0x06001DB9 RID: 7609 RVA: 0x000C17DC File Offset: 0x000BF9DC
		private void bgWorkerBuildTree_DoWork(object sender, DoWorkEventArgs e)
		{
			this.bgWorkerBuildTree.DoWork -= this.bgWorkerBuildTree_DoWork;
			bool flag = this.currentPaths == null;
			if (flag)
			{
				this.parHandling.BuildTree(this.parameterItems, this.showCheckbox, this.readOnly, this.logger, this.defaultPaths, this.defaultValues, this.pathBeginningsToIgnore);
			}
			else
			{
				List<string> list = this.currentPaths.ToList<string>();
				list.Sort(new ParameterPathComparer());
				this.parHandling.GetCertainItems(this.parameterItems, list.ToArray(), this.logger, this.readOnly);
				List<string> list2 = new List<string>();
				list2.Add("/xraysource - operatinghours/");
				this.parHandling.GetCertainItems(this.parameterItemsOperatingHours, list2.ToArray(), this.logger, this.readOnly);
			}
		}

		// Token: 0x06001DBA RID: 7610 RVA: 0x000C18BD File Offset: 0x000BFABD
		internal void StopProgressBar()
		{
			Progress.CloseStaticProgress(false);
		}

		// Token: 0x06001DBB RID: 7611 RVA: 0x000C18C7 File Offset: 0x000BFAC7
		public void StopTimers()
		{
			this.StopTimers(this.parameterItems);
		}

		// Token: 0x06001DBC RID: 7612 RVA: 0x000C18D8 File Offset: 0x000BFAD8
		private void StopTimers(ObservableCollection<ParameterItem> items)
		{
			foreach (ParameterItem parameterItem in items)
			{
				parameterItem.ResetTimer();
				bool flag = parameterItem.ChildrenItem != null && parameterItem.ChildrenItem.Count > 0;
				if (flag)
				{
					this.StopTimers(parameterItem.ChildrenItem);
				}
			}
		}

		// Token: 0x06001DBD RID: 7613 RVA: 0x000C1950 File Offset: 0x000BFB50
		public void ShowEditKeyboard(ParameterItem item)
		{
			this.ShowKeyboard(item);
		}

		// Token: 0x06001DBE RID: 7614 RVA: 0x000C195C File Offset: 0x000BFB5C
		private void ShowKeyboard(ParameterItem item)
		{
			this.pItem = item;
			bool flag = this.pItem.Type == "UInt";
			if (flag)
			{
				bool flag2 = !this.DockNumKeyboard;
				if (flag2)
				{
					this.numKeyboard = new NumKeyboard(this.pItem.Name, this.pItem.Value, NumKeyboard.ConvertType(this.pItem.Type));
				}
				else
				{
					this.numKeyboard = new NumKeyboard(this.pItem.Name, this.pItem.Value, NumKeyboard.ConvertType(this.pItem.Type), Dock.Left);
				}
				this.numKeyboard.TextInputKeyboard += this.keyboard_TextInputKeyboard;
				this.numKeyboard.Show();
			}
			bool flag3 = this.pItem.Type == "String";
			if (flag3)
			{
				this.keyboard = new Keyboard(this.pItem.Name, this.pItem.Value, KeyboardType.Default);
				this.keyboard.TextInputKeyboard += this.keyboard_TextInputKeyboard;
				this.keyboard.Show();
			}
			bool flag4 = this.pItem.Type == "Bool";
			if (flag4)
			{
				bool flag5 = !this.DockNumKeyboard;
				if (flag5)
				{
					this.numKeyboard = new NumKeyboard(this.pItem.Name, this.pItem.Value, NumKeyboard.ConvertType(this.pItem.Type));
				}
				else
				{
					this.numKeyboard = new NumKeyboard(this.pItem.Name, this.pItem.Value, NumKeyboard.ConvertType(this.pItem.Type), Dock.Left);
				}
				this.numKeyboard.TextInputKeyboard += this.keyboard_TextInputKeyboard;
				this.numKeyboard.Show();
			}
			bool flag6 = this.pItem.Type == "Double" || this.pItem.Type == "Int";
			if (flag6)
			{
				bool flag7 = !this.DockNumKeyboard;
				if (flag7)
				{
					this.numKeyboard = new NumKeyboard(this.pItem.Name, this.pItem.Value, NumKeyboard.ConvertType(this.pItem.Type));
				}
				else
				{
					this.numKeyboard = new NumKeyboard(this.pItem.Name, this.pItem.Value, NumKeyboard.ConvertType(this.pItem.Type), Dock.Left);
				}
				this.numKeyboard.TextInputKeyboard += this.keyboard_TextInputKeyboard;
				this.numKeyboard.Show();
			}
		}

		// Token: 0x06001DBF RID: 7615 RVA: 0x000C1C10 File Offset: 0x000BFE10
		protected virtual void OnItemsCreated()
		{
			Action itemsCreated = this.ItemsCreated;
			bool flag = itemsCreated != null;
			if (flag)
			{
				itemsCreated();
			}
		}

		// Token: 0x06001DC0 RID: 7616 RVA: 0x000C1C34 File Offset: 0x000BFE34
		public void SetLocalizedPseudoPath(List<InOut> mappings)
		{
			this.SetLocalizedPseudoPath(mappings, this.parameterItems);
		}

		// Token: 0x06001DC1 RID: 7617 RVA: 0x000C1C48 File Offset: 0x000BFE48
		private void SetLocalizedPseudoPath(List<InOut> mappings, ObservableCollection<ParameterItem> list)
		{
			bool flag = list != null;
			if (flag)
			{
				using (IEnumerator<ParameterItem> enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ParameterItem item = enumerator.Current;
						List<InOut> list2 = (from mapping in mappings
						where mapping.IOPath == item.Name
						select mapping).ToList<InOut>();
						bool flag2 = list2.Count > 0;
						if (flag2)
						{
							string text = LocalizationManager.Localize(list2[0].LocalizationString);
							bool flag3 = text != null;
							if (flag3)
							{
								item.ShownName = text;
							}
							else
							{
								item.ShownName = list2[0].LocalizationString;
							}
						}
						bool flag4 = item.ChildrenItem != null && item.ChildrenItem.Count > 0;
						if (flag4)
						{
							this.SetLocalizedPseudoPath(mappings, item.ChildrenItem);
						}
					}
				}
			}
		}

		// Token: 0x06001DC2 RID: 7618 RVA: 0x000C1D58 File Offset: 0x000BFF58
		internal void ShowWarningCalibration()
		{
			Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtWaitingForCalibration"), DialogStyle.Warning, DialogButtons.OK);
		}

		// Token: 0x06001DC3 RID: 7619 RVA: 0x000C1D77 File Offset: 0x000BFF77
		public void CollapseAllItems()
		{
			this.CollapseAllItems(this.parameterItems);
		}

		// Token: 0x06001DC4 RID: 7620 RVA: 0x000C1D88 File Offset: 0x000BFF88
		private void CollapseAllItems(ObservableCollection<ParameterItem> items)
		{
			foreach (ParameterItem parameterItem in items)
			{
				parameterItem.IsExpanded = false;
				bool flag = parameterItem.ChildrenItem != null && parameterItem.ChildrenItem.Count > 0;
				if (flag)
				{
					this.CollapseAllItems(parameterItem.ChildrenItem);
				}
			}
		}

		// Token: 0x06001DC5 RID: 7621 RVA: 0x000C1E00 File Offset: 0x000C0000
		public void Cleanup()
		{
			bool flag = this.pItem != null;
			if (flag)
			{
				this.pItem.ChildrenItem.Clear();
				this.pItem = null;
			}
			bool flag2 = this.parHandling != null;
			if (flag2)
			{
				this.parHandling.WriteValue("/hardware/detector/dt/MissingLinesReset/Value", "1");
				this.parHandling.WriteValue("/hardware/detector/dt/MissingLinesDisableCalc/Value", "0");
			}
		}

		// Token: 0x04001384 RID: 4996
		private ParameterHandling parHandling;

		// Token: 0x04001385 RID: 4997
		private Keyboard keyboard;

		// Token: 0x04001386 RID: 4998
		private NumKeyboard numKeyboard;

		// Token: 0x04001387 RID: 4999
		private ParameterItem pItem;

		// Token: 0x04001388 RID: 5000
		private ObservableCollection<ParameterItem> parameterItems = new ObservableCollection<ParameterItem>();

		// Token: 0x04001389 RID: 5001
		private ObservableCollection<ParameterItem> parameterItemsOperatingHours = new ObservableCollection<ParameterItem>();

		// Token: 0x0400138A RID: 5002
		private BackgroundWorker bgWorkerBuildTree = new BackgroundWorker();

		// Token: 0x0400138B RID: 5003
		private string[] currentPaths;

		// Token: 0x0400138C RID: 5004
		private bool readOnly;

		// Token: 0x0400138D RID: 5005
		private bool showCheckbox;

		// Token: 0x0400138E RID: 5006
		private bool showProgress;

		// Token: 0x0400138F RID: 5007
		private Logger logger;

		// Token: 0x04001390 RID: 5008
		private List<string> defaultPaths;

		// Token: 0x04001391 RID: 5009
		private List<string> defaultValues;

		// Token: 0x04001392 RID: 5010
		private List<string> pathBeginningsToIgnore;
	}
}
