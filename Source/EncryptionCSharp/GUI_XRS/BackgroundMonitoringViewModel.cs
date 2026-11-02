using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using Common;
using DatabaseConnection;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x02000081 RID: 129
	internal class BackgroundMonitoringViewModel : MonitoringViewModel, INotifyPropertyChanged
	{
		// Token: 0x14000044 RID: 68
		// (add) Token: 0x06000CBD RID: 3261 RVA: 0x0004B638 File Offset: 0x00049838
		// (remove) Token: 0x06000CBE RID: 3262 RVA: 0x0004B670 File Offset: 0x00049870
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x170003DE RID: 990
		// (get) Token: 0x06000CBF RID: 3263 RVA: 0x0004B6A5 File Offset: 0x000498A5
		// (set) Token: 0x06000CC0 RID: 3264 RVA: 0x0004B6AD File Offset: 0x000498AD
		public RelayCommand CleanUpCommand { get; set; }

		// Token: 0x170003DF RID: 991
		// (get) Token: 0x06000CC1 RID: 3265 RVA: 0x0004B6B8 File Offset: 0x000498B8
		// (set) Token: 0x06000CC2 RID: 3266 RVA: 0x0004B6D0 File Offset: 0x000498D0
		public List<MonitoringItem> Items
		{
			get
			{
				return this.items;
			}
			set
			{
				this.items = value;
				this.OnPropertyChanged("Items");
			}
		}

		// Token: 0x170003E0 RID: 992
		// (get) Token: 0x06000CC3 RID: 3267 RVA: 0x0004B6E6 File Offset: 0x000498E6
		// (set) Token: 0x06000CC4 RID: 3268 RVA: 0x0004B6EE File Offset: 0x000498EE
		public BoundLocalizedText StrTitle { get; set; }

		// Token: 0x170003E1 RID: 993
		// (get) Token: 0x06000CC5 RID: 3269 RVA: 0x0004B6F7 File Offset: 0x000498F7
		// (set) Token: 0x06000CC6 RID: 3270 RVA: 0x0004B6FF File Offset: 0x000498FF
		public BoundLocalizedText StrCurrentVal { get; set; }

		// Token: 0x170003E2 RID: 994
		// (get) Token: 0x06000CC7 RID: 3271 RVA: 0x0004B708 File Offset: 0x00049908
		// (set) Token: 0x06000CC8 RID: 3272 RVA: 0x0004B710 File Offset: 0x00049910
		public BoundLocalizedText StrLimits { get; set; }

		// Token: 0x170003E3 RID: 995
		// (get) Token: 0x06000CC9 RID: 3273 RVA: 0x0004B71C File Offset: 0x0004991C
		// (set) Token: 0x06000CCA RID: 3274 RVA: 0x0004B734 File Offset: 0x00049934
		public bool EnableLimits
		{
			get
			{
				return this.enableLimits;
			}
			set
			{
				this.enableLimits = value;
				this.OnPropertyChanged("EnableLimits");
			}
		}

		// Token: 0x170003E4 RID: 996
		// (get) Token: 0x06000CCB RID: 3275 RVA: 0x0004B74A File Offset: 0x0004994A
		// (set) Token: 0x06000CCC RID: 3276 RVA: 0x0004B752 File Offset: 0x00049952
		public BoundLocalizedText StrBackgroundIndex { get; set; }

		// Token: 0x170003E5 RID: 997
		// (get) Token: 0x06000CCD RID: 3277 RVA: 0x0004B75B File Offset: 0x0004995B
		// (set) Token: 0x06000CCE RID: 3278 RVA: 0x0004B763 File Offset: 0x00049963
		public BoundLocalizedText StrPixelDeltaMax { get; set; }

		// Token: 0x170003E6 RID: 998
		// (get) Token: 0x06000CCF RID: 3279 RVA: 0x0004B76C File Offset: 0x0004996C
		// (set) Token: 0x06000CD0 RID: 3280 RVA: 0x0004B774 File Offset: 0x00049974
		public BoundLocalizedText StrPixelDeltaMaxPosition { get; set; }

		// Token: 0x170003E7 RID: 999
		// (get) Token: 0x06000CD1 RID: 3281 RVA: 0x0004B77D File Offset: 0x0004997D
		// (set) Token: 0x06000CD2 RID: 3282 RVA: 0x0004B785 File Offset: 0x00049985
		public BoundLocalizedText StrBadPixel { get; set; }

		// Token: 0x170003E8 RID: 1000
		// (get) Token: 0x06000CD3 RID: 3283 RVA: 0x0004B78E File Offset: 0x0004998E
		// (set) Token: 0x06000CD4 RID: 3284 RVA: 0x0004B796 File Offset: 0x00049996
		public BoundLocalizedText StrDefectiveModule { get; set; }

		// Token: 0x06000CD5 RID: 3285 RVA: 0x0004B7A0 File Offset: 0x000499A0
		public BackgroundMonitoringViewModel(DBDataLogs dBDataLogs, DBConnection dBConnection, ParameterHandling parameterHandling, bool EnableLimits)
		{
			this.RegisterLocalizedText();
			this.dBDataLogs = dBDataLogs;
			this.dbConnection = dBConnection;
			this.parameterHandling = parameterHandling;
			this.EnableLimits = EnableLimits;
			this.Initialize();
			this.timerUpdateParam2sec = new DispatcherTimer(new TimeSpan(0, 0, 2), DispatcherPriority.Normal, new EventHandler(this.updateViewModelValues), Application.Current.Dispatcher);
			this.CleanUpCommand = new RelayCommand(new Action<object>(this.CleanUp));
		}

		// Token: 0x06000CD6 RID: 3286 RVA: 0x0004B838 File Offset: 0x00049A38
		private void Initialize()
		{
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			List<string> list3 = new List<string>();
			List<string> list4 = new List<string>();
			List<string> source = new List<string>();
			List<string> source2 = new List<string>();
			List<string> source3 = new List<string>();
			List<string> list5 = new List<string>();
			List<string> list6 = new List<string>();
			string limit = "";
			string text = "";
			string text2 = "";
			string text3 = this.parameterHandling.ReadPar(Definition.detectorMonitoringBackgroundIndex);
			string text4 = this.parameterHandling.ReadPar(Definition.detectorMonitoringPixelDeltaMax);
			string text5 = this.parameterHandling.ReadPar(Definition.detectorMonitoringPixelDeltaMaxPosition);
			string currentValue = this.parameterHandling.ReadPar(Definition.detectorMonitoringBadPixel);
			string currentValue2 = this.parameterHandling.ReadPar(Definition.detectorMonitoringWrongLength);
			this.dBDataLogs.MachineInfosGetSerialNumbers(ref text2, ref text);
			string tableName = "BackgroundIndex_" + text.Replace(":", "");
			this.dBDataLogs.BackgroundIndexGetEntries(tableName, ref list, ref list2, ref list3, ref list4, ref source, ref source2, ref source3, ref list5, ref list6);
			bool flag = text3 == "" && list.Count > 0;
			if (flag)
			{
				text3 = source.Last<string>();
			}
			bool flag2 = text4 == "" && text5 == "" && list.Count > 0;
			if (flag2)
			{
				text5 = source2.Last<string>();
				text4 = source3.Last<string>();
			}
			this.dBDataLogs.GetLimitsEntry("BackgroundIndex", ref limit);
			this.items.Add(new MonitoringItem(this.StrBackgroundIndex.LocalizedText, text3, limit, "", this.EnableLimits));
			this.dBDataLogs.GetLimitsEntry("PixelDeltaMax", ref limit);
			this.items.Add(new MonitoringItem(this.StrPixelDeltaMax.LocalizedText, text4, limit, "", this.EnableLimits));
			this.items.Add(new MonitoringItem(this.StrPixelDeltaMaxPosition.LocalizedText, text5, "n.a.", "n.a.", false));
			this.items.Add(new MonitoringItem(this.StrDefectiveModule.LocalizedText, currentValue2, "n.a.", "n.a.", false));
			this.items.Add(new MonitoringItem(this.StrBadPixel.LocalizedText, currentValue, "n.a.", "n.a.", false));
		}

		// Token: 0x06000CD7 RID: 3287 RVA: 0x0004BAA4 File Offset: 0x00049CA4
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x06000CD8 RID: 3288 RVA: 0x0004BAD4 File Offset: 0x00049CD4
		public override void getNumKeyboardNumber(MonitoringItem mItem, int limitNumb)
		{
			this.limitNumber = limitNumb;
			this.selectedItem = mItem;
			bool flag = this.limitNumber == 1;
			string oldValue;
			if (flag)
			{
				oldValue = mItem.Limit;
			}
			else
			{
				oldValue = mItem.Limit2;
			}
			this.numKeyboard = new NumKeyboard(mItem.Name, oldValue, NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.keyboard_TextInputKeyboardNumber;
			this.numKeyboard.Show();
		}

		// Token: 0x06000CD9 RID: 3289 RVA: 0x0004BB50 File Offset: 0x00049D50
		private void keyboard_TextInputKeyboardNumber(object sender, KeyboardInputEventArgs e)
		{
			bool flag = e.Text != null;
			if (flag)
			{
				bool flag2 = this.limitNumber == 1;
				if (flag2)
				{
					this.selectedItem.Limit = e.Text;
					this.SetNewLimit(this.selectedItem);
				}
				else
				{
					this.selectedItem.Limit2 = e.Text;
				}
			}
			this.numKeyboard.TextInputKeyboard -= this.keyboard_TextInputKeyboardNumber;
		}

		// Token: 0x06000CDA RID: 3290 RVA: 0x0004BBC8 File Offset: 0x00049DC8
		public void SetNewLimit(MonitoringItem item)
		{
			bool flag = item.Name.Equals(this.StrBackgroundIndex.LocalizedText);
			if (flag)
			{
				this.dBDataLogs.LimitsAddEntry("DateTimeBackgroundIndex", "BackgroundIndex", item.Limit, DateTime.Now.ToString(new CultureInfo("de-DE")));
			}
			else
			{
				bool flag2 = item.Name.Equals(this.StrPixelDeltaMax.LocalizedText);
				if (flag2)
				{
					this.dBDataLogs.LimitsAddEntry("DateTimePixelDeltaMax", "PixelDeltaMax", item.Limit, DateTime.Now.ToString(new CultureInfo("de-DE")));
				}
			}
		}

		// Token: 0x06000CDB RID: 3291 RVA: 0x0004BC78 File Offset: 0x00049E78
		private void updateViewModelValues(object dummy, EventArgs e)
		{
			bool flag = this.Items != null && this.Items.Count == 5;
			if (flag)
			{
				this.Items[0].CurrentValue = this.parameterHandling.ReadPar(Definition.detectorMonitoringBackgroundIndex);
				this.Items[1].CurrentValue = this.parameterHandling.ReadPar(Definition.detectorMonitoringPixelDeltaMax);
				this.Items[2].CurrentValue = this.parameterHandling.ReadPar(Definition.detectorMonitoringPixelDeltaMaxPosition);
				this.Items[3].CurrentValue = this.parameterHandling.ReadPar(Definition.detectorMonitoringWrongLength);
				this.Items[4].CurrentValue = this.parameterHandling.ReadPar(Definition.detectorMonitoringBadPixel);
			}
		}

		// Token: 0x06000CDC RID: 3292 RVA: 0x0004BD54 File Offset: 0x00049F54
		private void RegisterLocalizedText()
		{
			this.StrTitle = new BoundLocalizedText("txtCalibration");
			this.StrLimits = new BoundLocalizedText("txtLimits");
			this.StrCurrentVal = new BoundLocalizedText("txtCurrentValue");
			this.StrBackgroundIndex = new BoundLocalizedText("txtBackgroundIndex");
			this.StrPixelDeltaMax = new BoundLocalizedText("txtPixelDeltaMax");
			this.StrBadPixel = new BoundLocalizedText("txtBadPixel");
			this.StrPixelDeltaMaxPosition = new BoundLocalizedText("txtPixelDeltaMaxPosition");
			this.StrDefectiveModule = new BoundLocalizedText("txtDefectiveModule");
		}

		// Token: 0x06000CDD RID: 3293 RVA: 0x0004BDEC File Offset: 0x00049FEC
		private void DeregisterLocalizedText()
		{
			MainWindow.localizationManager.Deregister(this.StrLimits);
			MainWindow.localizationManager.Deregister(this.StrCurrentVal);
			MainWindow.localizationManager.Deregister(this.StrTitle);
			MainWindow.localizationManager.Deregister(this.StrBackgroundIndex);
			MainWindow.localizationManager.Deregister(this.StrPixelDeltaMax);
			MainWindow.localizationManager.Deregister(this.StrBadPixel);
			MainWindow.localizationManager.Deregister(this.StrPixelDeltaMaxPosition);
			MainWindow.localizationManager.Deregister(this.StrDefectiveModule);
		}

		// Token: 0x06000CDE RID: 3294 RVA: 0x0001092C File Offset: 0x0000EB2C
		public override void ShowDiagramm()
		{
		}

		// Token: 0x06000CDF RID: 3295 RVA: 0x0004BE84 File Offset: 0x0004A084
		private void CleanUp(object obj)
		{
			this.DeregisterLocalizedText();
			bool flag = this.timerUpdateParam2sec != null;
			if (flag)
			{
				this.timerUpdateParam2sec.Stop();
			}
			bool flag2 = this.Items != null;
			if (flag2)
			{
				this.Items.Clear();
				this.Items = null;
			}
		}

		// Token: 0x0400085C RID: 2140
		private NumKeyboard numKeyboard;

		// Token: 0x0400085D RID: 2141
		private MonitoringItem selectedItem;

		// Token: 0x0400085E RID: 2142
		private int limitNumber;

		// Token: 0x0400085F RID: 2143
		private List<MonitoringItem> items = new List<MonitoringItem>();

		// Token: 0x04000863 RID: 2147
		private DBDataLogs dBDataLogs;

		// Token: 0x04000864 RID: 2148
		private DBConnection dbConnection;

		// Token: 0x04000865 RID: 2149
		private ParameterHandling parameterHandling;

		// Token: 0x04000866 RID: 2150
		private bool enableLimits = false;

		// Token: 0x0400086C RID: 2156
		private DispatcherTimer timerUpdateParam2sec;
	}
}
