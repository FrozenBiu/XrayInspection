using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Timers;
using System.Windows;
using System.Windows.Threading;
using DatabaseConnection;
using Definitions;
using GUI_XRS.Datalogger;

namespace GUI_XRS
{
	// Token: 0x0200024F RID: 591
	public class ParameterItem : INotifyPropertyChanged
	{
		// Token: 0x14000101 RID: 257
		// (add) Token: 0x06002D01 RID: 11521 RVA: 0x00127CFC File Offset: 0x00125EFC
		// (remove) Token: 0x06002D02 RID: 11522 RVA: 0x00127D34 File Offset: 0x00125F34
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x17000C0F RID: 3087
		// (get) Token: 0x06002D03 RID: 11523 RVA: 0x00127D69 File Offset: 0x00125F69
		// (set) Token: 0x06002D04 RID: 11524 RVA: 0x00127D71 File Offset: 0x00125F71
		public ObservableCollection<ParameterItem> ChildrenItem { get; set; }

		// Token: 0x17000C10 RID: 3088
		// (get) Token: 0x06002D05 RID: 11525 RVA: 0x00127D7C File Offset: 0x00125F7C
		// (set) Token: 0x06002D06 RID: 11526 RVA: 0x00127D94 File Offset: 0x00125F94
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
				this.shownName = value;
			}
		}

		// Token: 0x17000C11 RID: 3089
		// (get) Token: 0x06002D07 RID: 11527 RVA: 0x00127DA8 File Offset: 0x00125FA8
		// (set) Token: 0x06002D08 RID: 11528 RVA: 0x00127DC0 File Offset: 0x00125FC0
		public string ShownName
		{
			get
			{
				return this.shownName;
			}
			set
			{
				this.shownName = value;
				this.OnPropertyChanged("ShownName");
			}
		}

		// Token: 0x17000C12 RID: 3090
		// (get) Token: 0x06002D09 RID: 11529 RVA: 0x00127DD8 File Offset: 0x00125FD8
		// (set) Token: 0x06002D0A RID: 11530 RVA: 0x00127DF0 File Offset: 0x00125FF0
		public bool ReadAndWrite
		{
			get
			{
				return this.readAndWrite;
			}
			set
			{
				this.readAndWrite = value;
				this.OnPropertyChanged("ReadAndWrite");
			}
		}

		// Token: 0x17000C13 RID: 3091
		// (get) Token: 0x06002D0B RID: 11531 RVA: 0x00127E08 File Offset: 0x00126008
		public Visibility TextBoxVisibility
		{
			get
			{
				bool flag = this.ChildrenItem.Count == 0;
				Visibility result;
				if (flag)
				{
					result = Visibility.Visible;
				}
				else
				{
					result = Visibility.Hidden;
				}
				return result;
			}
		}

		// Token: 0x17000C14 RID: 3092
		// (get) Token: 0x06002D0C RID: 11532 RVA: 0x00127E34 File Offset: 0x00126034
		// (set) Token: 0x06002D0D RID: 11533 RVA: 0x00127E4C File Offset: 0x0012604C
		public bool IsExpanded
		{
			get
			{
				return this.isExpanded;
			}
			set
			{
				this.isExpanded = value;
				this.OnPropertyChanged("IsExpanded");
				bool flag = this.isExpanded && !this.defaultProductParameter;
				if (flag)
				{
					new Thread(new ThreadStart(this.UpdateAll))
					{
						Name = "Update " + this.name + " Parameters"
					}.Start();
				}
				else
				{
					this.timerShouldStop = true;
				}
			}
		}

		// Token: 0x17000C15 RID: 3093
		// (get) Token: 0x06002D0E RID: 11534 RVA: 0x00127EC8 File Offset: 0x001260C8
		public bool IsParameterItem
		{
			get
			{
				return this.ChildrenItem == null || this.ChildrenItem.Count == 0;
			}
		}

		// Token: 0x17000C16 RID: 3094
		// (get) Token: 0x06002D0F RID: 11535 RVA: 0x00127EF4 File Offset: 0x001260F4
		// (set) Token: 0x06002D10 RID: 11536 RVA: 0x00127F0C File Offset: 0x0012610C
		public string Type
		{
			get
			{
				return this.type;
			}
			set
			{
				this.type = value;
				this.OnPropertyChanged("Type");
			}
		}

		// Token: 0x17000C17 RID: 3095
		// (get) Token: 0x06002D11 RID: 11537 RVA: 0x00127F22 File Offset: 0x00126122
		// (set) Token: 0x06002D12 RID: 11538 RVA: 0x00127F2A File Offset: 0x0012612A
		public string Root { get; set; }

		// Token: 0x17000C18 RID: 3096
		// (get) Token: 0x06002D13 RID: 11539 RVA: 0x00127F34 File Offset: 0x00126134
		// (set) Token: 0x06002D14 RID: 11540 RVA: 0x00127F4C File Offset: 0x0012614C
		public string Min
		{
			get
			{
				return this.min;
			}
			set
			{
				this.min = value;
				this.OnPropertyChanged("Min");
			}
		}

		// Token: 0x17000C19 RID: 3097
		// (get) Token: 0x06002D15 RID: 11541 RVA: 0x00127F64 File Offset: 0x00126164
		// (set) Token: 0x06002D16 RID: 11542 RVA: 0x00127F7C File Offset: 0x0012617C
		public string Max
		{
			get
			{
				return this.max;
			}
			set
			{
				this.max = value;
				this.OnPropertyChanged("Max");
			}
		}

		// Token: 0x17000C1A RID: 3098
		// (get) Token: 0x06002D17 RID: 11543 RVA: 0x00127F94 File Offset: 0x00126194
		// (set) Token: 0x06002D18 RID: 11544 RVA: 0x00127FAC File Offset: 0x001261AC
		public string Description
		{
			get
			{
				return this.description;
			}
			set
			{
				this.description = value;
				this.OnPropertyChanged("Description");
			}
		}

		// Token: 0x17000C1B RID: 3099
		// (get) Token: 0x06002D19 RID: 11545 RVA: 0x00127FC4 File Offset: 0x001261C4
		// (set) Token: 0x06002D1A RID: 11546 RVA: 0x00127FDC File Offset: 0x001261DC
		public string StrforVoltageOperatingHours
		{
			get
			{
				return this.strforVoltageOperatingHours;
			}
			set
			{
				this.strforVoltageOperatingHours = value;
				this.OnPropertyChanged("StringforOperatingHours");
			}
		}

		// Token: 0x17000C1C RID: 3100
		// (get) Token: 0x06002D1B RID: 11547 RVA: 0x00127FF4 File Offset: 0x001261F4
		// (set) Token: 0x06002D1C RID: 11548 RVA: 0x0012800C File Offset: 0x0012620C
		public string StrforCurrentOperatingHours
		{
			get
			{
				return this.strforCurrentOperatingHours;
			}
			set
			{
				this.strforCurrentOperatingHours = value;
				this.OnPropertyChanged("StringforOperatingHours");
			}
		}

		// Token: 0x17000C1D RID: 3101
		// (get) Token: 0x06002D1D RID: 11549 RVA: 0x00128024 File Offset: 0x00126224
		// (set) Token: 0x06002D1E RID: 11550 RVA: 0x0012803C File Offset: 0x0012623C
		public string StrforOperatingHours
		{
			get
			{
				return this.strforOperatingHours;
			}
			set
			{
				this.strforOperatingHours = value;
				this.OnPropertyChanged("StringforOperatingHours");
			}
		}

		// Token: 0x17000C1E RID: 3102
		// (get) Token: 0x06002D1F RID: 11551 RVA: 0x00128054 File Offset: 0x00126254
		// (set) Token: 0x06002D20 RID: 11552 RVA: 0x0012806C File Offset: 0x0012626C
		public string StrforOperatingHoursTotal
		{
			get
			{
				return this.strforOperatingHoursTotal;
			}
			set
			{
				this.strforOperatingHoursTotal = value;
				this.OnPropertyChanged("StringforOperatingHoursTotal");
			}
		}

		// Token: 0x17000C1F RID: 3103
		// (get) Token: 0x06002D21 RID: 11553 RVA: 0x00128084 File Offset: 0x00126284
		// (set) Token: 0x06002D22 RID: 11554 RVA: 0x0012809C File Offset: 0x0012629C
		public string StrforDateTimeOperatingHours
		{
			get
			{
				return this.strforDateTimeOperatingHours;
			}
			set
			{
				this.strforDateTimeOperatingHours = value;
				this.OnPropertyChanged("StrforDateTimeOperatingHours");
			}
		}

		// Token: 0x17000C20 RID: 3104
		// (get) Token: 0x06002D23 RID: 11555 RVA: 0x001280B4 File Offset: 0x001262B4
		// (set) Token: 0x06002D24 RID: 11556 RVA: 0x001280CC File Offset: 0x001262CC
		public string Operatinghours
		{
			get
			{
				return this.operatinghours;
			}
			set
			{
				this.operatinghours = value;
			}
		}

		// Token: 0x17000C21 RID: 3105
		// (get) Token: 0x06002D25 RID: 11557 RVA: 0x001280D6 File Offset: 0x001262D6
		// (set) Token: 0x06002D26 RID: 11558 RVA: 0x001280DE File Offset: 0x001262DE
		public Logger Datalogger { get; set; }

		// Token: 0x17000C22 RID: 3106
		// (get) Token: 0x06002D27 RID: 11559 RVA: 0x001280E8 File Offset: 0x001262E8
		// (set) Token: 0x06002D28 RID: 11560 RVA: 0x00128100 File Offset: 0x00126300
		public string Value
		{
			get
			{
				return this.value;
			}
			set
			{
				this.value = value;
				this.OnPropertyChanged("Value");
			}
		}

		// Token: 0x17000C23 RID: 3107
		// (get) Token: 0x06002D29 RID: 11561 RVA: 0x00128118 File Offset: 0x00126318
		// (set) Token: 0x06002D2A RID: 11562 RVA: 0x00128130 File Offset: 0x00126330
		public bool IsChecked
		{
			get
			{
				return this.isChecked;
			}
			set
			{
				bool flag = !this.Datalogger.IsRunning;
				if (flag)
				{
					this.isChecked = value;
					this.OnPropertyChanged("IsChecked");
					bool flag2 = this.isChecked;
					if (flag2)
					{
						bool flag3 = this.Datalogger != null;
						if (flag3)
						{
							this.Datalogger.Paths.Add(this.Root + "/" + this.Name + "/Value");
						}
					}
					else
					{
						this.Datalogger.Paths.Remove(this.Root + "/" + this.Name + "/Value");
					}
				}
				else
				{
					bool flag4 = this.isCheckedResetTimer.Interval != 50.0;
					if (flag4)
					{
						this.isCheckedResetTimer.Elapsed += this.checkedResetTimer_Elapsed;
						this.isCheckedResetTimer.Interval = 50.0;
					}
					this.isCheckedResetTimer.Start();
				}
			}
		}

		// Token: 0x17000C24 RID: 3108
		// (get) Token: 0x06002D2B RID: 11563 RVA: 0x00128238 File Offset: 0x00126438
		// (set) Token: 0x06002D2C RID: 11564 RVA: 0x00128250 File Offset: 0x00126450
		public bool IsCheckedVisible
		{
			get
			{
				return this.isCheckedVisible;
			}
			set
			{
				this.isCheckedVisible = value;
				this.OnPropertyChanged("IsCheckedVisible");
			}
		}

		// Token: 0x17000C25 RID: 3109
		// (get) Token: 0x06002D2D RID: 11565 RVA: 0x00128268 File Offset: 0x00126468
		// (set) Token: 0x06002D2E RID: 11566 RVA: 0x00128280 File Offset: 0x00126480
		public bool IsOperatingHoursVisible
		{
			get
			{
				return this.isOperatingHoursVisible;
			}
			set
			{
				this.isOperatingHoursVisible = value;
				this.OnPropertyChanged("IsOperatingHoursVisible");
			}
		}

		// Token: 0x06002D2F RID: 11567 RVA: 0x00128296 File Offset: 0x00126496
		public ParameterItem(ParameterHandling parHandling) : this(parHandling, false, null, null)
		{
		}

		// Token: 0x06002D30 RID: 11568 RVA: 0x001282A4 File Offset: 0x001264A4
		public ParameterItem(ParameterHandling parHandling, bool defaultProductParameter, DBConnection dbConnection, DBDataLogs dbDatalogs)
		{
			this.parHandling = parHandling;
			this.defaultProductParameter = defaultProductParameter;
			this.ChildrenItem = new ObservableCollection<ParameterItem>();
			bool flag = !defaultProductParameter;
			if (flag)
			{
				this.updateTimer = new System.Timers.Timer();
				this.updateTimer.Interval = 500.0;
				this.updateTimer.Elapsed += this.updateTimer_Elapsed;
			}
			this.serviceMode = (MainWindowViewModel.accessRightsManager.CurrentUser.Rights == BuiltInPriviledges.Service);
			this.isOperatingHoursVisible = (OverviewCommon.OperatingHourVisibility && this.serviceMode);
			bool flag2 = this.isOperatingHoursVisible;
			if (flag2)
			{
				this.generateOperatingHoursTable(dbConnection, dbDatalogs);
			}
		}

		// Token: 0x06002D31 RID: 11569 RVA: 0x00128388 File Offset: 0x00126588
		private void generateOperatingHoursTable(DBConnection dbConnection, DBDataLogs dbDatalogs)
		{
			string[] array = new string[this.operatinghourslength];
			string[] array2 = new string[this.operatinghourslength];
			string[] array3 = new string[this.operatinghourslength];
			string[] array4 = new string[this.operatinghourslength];
			string text = "";
			string text2 = "0";
			string text3 = "0";
			double num = 0.0;
			double num2 = 0.0;
			bool flag = dbConnection != null;
			if (flag)
			{
				int num3 = 0;
				this.xrayoperatinghours = new XRayOperatingHours(dbConnection, this.parHandling, dbDatalogs);
				this.xrayoperatinghours.GetXrayOnTimeForkVmAOverview(array4, array, array2, array3, ref text);
				dbConnection.GetXRayOnTimeInternal(ref text3);
				double.TryParse(text.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out num);
				double.TryParse(text3.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out num2);
				bool flag2 = num2 > 0.0;
				if (flag2)
				{
					text2 = Convert.ToString(Math.Round(num2 - num));
				}
				text = Convert.ToString(Math.Round(num));
				for (int i = 0; i < this.operatinghourslength; i++)
				{
					bool flag3 = i == 0;
					string str;
					if (flag3)
					{
						str = "  ";
					}
					else
					{
						str = "";
					}
					bool flag4 = array[i] != null;
					if (flag4)
					{
						bool flag5 = i + 1 < 104 && array[i + 1] == null;
						string str2;
						if (flag5)
						{
							str2 = "";
						}
						else
						{
							str2 = "\n";
						}
						array4[i] = str + array4[i] + " " + str2;
						array[i] = str + array[i] + " kV" + str2;
						array2[i] = str + array2[i] + " mA" + str2;
						array3[i] = str + array3[i] + " h" + str2;
						num3++;
					}
				}
				bool flag6 = num3 > 0;
				if (flag6)
				{
					this.strforDateTimeOperatingHours = string.Join("  ", array4, 0, num3);
					this.strforVoltageOperatingHours = string.Join("  ", array, 0, num3);
					this.strforCurrentOperatingHours = string.Join("  ", array2, 0, num3);
					this.strforOperatingHours = string.Join("  ", array3, 0, num3);
					this.strforOperatingHoursTotal = string.Concat(new string[]
					{
						"  Initial Time Source: \n  ",
						text2,
						" h\n  Total Time Sesotec: \n  ",
						text,
						" h"
					});
				}
			}
		}

		// Token: 0x06002D32 RID: 11570 RVA: 0x00128616 File Offset: 0x00126816
		private void checkedResetTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			this.isCheckedResetTimer.Stop();
			this.isCheckedResetTimer.Elapsed -= this.checkedResetTimer_Elapsed;
			this.OnPropertyChanged("IsChecked");
		}

		// Token: 0x06002D33 RID: 11571 RVA: 0x00128649 File Offset: 0x00126849
		public void SetIsChecked(bool isChecked)
		{
			this.isChecked = isChecked;
			this.OnPropertyChanged("IsChecked");
		}

		// Token: 0x06002D34 RID: 11572 RVA: 0x0012865F File Offset: 0x0012685F
		private void updateTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			this.UpdateValues();
		}

		// Token: 0x06002D35 RID: 11573 RVA: 0x0012866C File Offset: 0x0012686C
		protected void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x06002D36 RID: 11574 RVA: 0x0012869C File Offset: 0x0012689C
		private void UpdateValues()
		{
			try
			{
				this.updateTimer.Stop();
				bool flag = this.ChildrenItem != null;
				if (flag)
				{
					foreach (ParameterItem parameterItem in this.ChildrenItem)
					{
						bool isParameterItem = parameterItem.IsParameterItem;
						if (isParameterItem)
						{
							bool flag2 = !this.SetSpecialValues(parameterItem);
							if (flag2)
							{
								string str = parameterItem.Root + "/" + parameterItem.Name;
								parameterItem.Value = this.parHandling.ReadPar(str + "/Value");
							}
						}
					}
					bool flag3 = !this.timerShouldStop;
					if (flag3)
					{
						this.updateTimer.Start();
					}
				}
			}
			catch (Exception ex)
			{
				Exception ex3;
				Exception ex2 = ex3;
				Exception ex = ex2;
				bool flag4 = Application.Current != null && !Application.Current.Dispatcher.CheckAccess();
				if (flag4)
				{
					Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
					{
						Dialog.Show("Error Update All Values of the parameters", ex.Message);
					}));
				}
			}
		}

		// Token: 0x06002D37 RID: 11575 RVA: 0x001287DC File Offset: 0x001269DC
		private void UpdateAll()
		{
			try
			{
				bool flag = this.Name != "/";
				if (flag)
				{
					string[] array = new string[this.ChildrenItem.Count];
					bool flag2 = this.ChildrenItem.Count > 0 && this.ChildrenItem[0].IsParameterItem;
					if (flag2)
					{
						this.timerShouldStop = false;
						this.updateTimer.Start();
					}
					for (int i = 0; i < this.ChildrenItem.Count; i++)
					{
						bool isParameterItem = this.ChildrenItem[i].IsParameterItem;
						if (isParameterItem)
						{
							array[i] = this.ChildrenItem[i].Root + "/" + this.ChildrenItem[i].Name;
							bool isParameterItem2 = this.ChildrenItem[i].IsParameterItem;
							if (isParameterItem2)
							{
								bool flag3 = !this.SetSpecialValues(this.ChildrenItem[i]);
								if (flag3)
								{
									this.ChildrenItem[i].Value = this.parHandling.ReadPar(array[i] + "/Value");
								}
								bool flag4 = this.ChildrenItem[i].Type == null;
								if (flag4)
								{
									this.ChildrenItem[i].Type = this.parHandling.ReadPar(array[i] + "/Type");
								}
								bool flag5 = this.ChildrenItem[i].Min == null;
								if (flag5)
								{
									this.ChildrenItem[i].Min = this.parHandling.ReadPar(array[i] + "/Min");
								}
								bool flag6 = this.ChildrenItem[i].Max == null;
								if (flag6)
								{
									this.ChildrenItem[i].Max = this.parHandling.ReadPar(array[i] + "/Max");
								}
								bool flag7 = this.ChildrenItem[i].Description == null;
								if (flag7)
								{
									this.ChildrenItem[i].Description = this.parHandling.ReadPar(array[i] + "/Description");
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Exception ex3;
				Exception ex2 = ex3;
				Exception ex = ex2;
				bool flag8 = Application.Current != null && !Application.Current.Dispatcher.CheckAccess();
				if (flag8)
				{
					Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
					{
						Dialog.Show("Error Update All Parameters", ex.Message);
					}));
				}
				else
				{
					Dialog.Show("Error Update All Parameters", ex.Message);
				}
			}
		}

		// Token: 0x06002D38 RID: 11576 RVA: 0x00128AC0 File Offset: 0x00126CC0
		public void StopTimer()
		{
			this.timerShouldStop = true;
		}

		// Token: 0x06002D39 RID: 11577 RVA: 0x00128ACC File Offset: 0x00126CCC
		private bool SetSpecialValues(ParameterItem item)
		{
			string text = item.Root + "/" + item.Name + "/";
			bool flag = text == Definition.xRayOnTimePath || text == Definition.lastXRayOnTimePath;
			bool result;
			if (flag)
			{
				double num = 0.0;
				bool flag2 = double.TryParse(this.parHandling.ReadPar(text + "Value"), out num);
				if (flag2)
				{
					num = Math.Round(num / 3600.0, 2);
					item.Value = num.ToString("F");
				}
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06002D3A RID: 11578 RVA: 0x00128B74 File Offset: 0x00126D74
		public void ResetTimer()
		{
			bool flag = this.isCheckedResetTimer != null;
			if (flag)
			{
				this.isCheckedResetTimer.Stop();
				this.isCheckedResetTimer.Elapsed -= this.checkedResetTimer_Elapsed;
			}
			bool flag2 = this.updateTimer != null;
			if (flag2)
			{
				this.updateTimer.Stop();
				this.updateTimer.Elapsed -= this.updateTimer_Elapsed;
			}
		}

		// Token: 0x04001C29 RID: 7209
		private System.Timers.Timer isCheckedResetTimer = new System.Timers.Timer();

		// Token: 0x04001C2A RID: 7210
		private string value;

		// Token: 0x04001C2B RID: 7211
		private string min;

		// Token: 0x04001C2C RID: 7212
		private string max;

		// Token: 0x04001C2D RID: 7213
		private string description;

		// Token: 0x04001C2E RID: 7214
		private string type;

		// Token: 0x04001C2F RID: 7215
		private bool isChecked;

		// Token: 0x04001C30 RID: 7216
		private bool isCheckedVisible;

		// Token: 0x04001C31 RID: 7217
		private System.Timers.Timer updateTimer;

		// Token: 0x04001C32 RID: 7218
		private ParameterHandling parHandling;

		// Token: 0x04001C33 RID: 7219
		private string name;

		// Token: 0x04001C34 RID: 7220
		private string shownName;

		// Token: 0x04001C35 RID: 7221
		private bool timerShouldStop = false;

		// Token: 0x04001C36 RID: 7222
		private bool readAndWrite = true;

		// Token: 0x04001C37 RID: 7223
		private bool isExpanded;

		// Token: 0x04001C38 RID: 7224
		private bool defaultProductParameter;

		// Token: 0x04001C39 RID: 7225
		private XRayOperatingHours xrayoperatinghours;

		// Token: 0x04001C3B RID: 7227
		private string strforVoltageOperatingHours;

		// Token: 0x04001C3C RID: 7228
		private string strforCurrentOperatingHours;

		// Token: 0x04001C3D RID: 7229
		private string strforOperatingHours;

		// Token: 0x04001C3E RID: 7230
		private string strforOperatingHoursTotal;

		// Token: 0x04001C3F RID: 7231
		private string strforDateTimeOperatingHours;

		// Token: 0x04001C40 RID: 7232
		private string operatinghours = "xraysource - operatinghours";

		// Token: 0x04001C41 RID: 7233
		private bool isOperatingHoursVisible;

		// Token: 0x04001C42 RID: 7234
		private bool serviceMode;

		// Token: 0x04001C43 RID: 7235
		private int operatinghourslength = 104;
	}
}
