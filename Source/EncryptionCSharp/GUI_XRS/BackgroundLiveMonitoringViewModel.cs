using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;
using Common;
using DatabaseConnection;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x02000080 RID: 128
	internal class BackgroundLiveMonitoringViewModel : MonitoringViewModel, INotifyPropertyChanged
	{
		// Token: 0x14000043 RID: 67
		// (add) Token: 0x06000C9A RID: 3226 RVA: 0x0004AFF4 File Offset: 0x000491F4
		// (remove) Token: 0x06000C9B RID: 3227 RVA: 0x0004B02C File Offset: 0x0004922C
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x170003D4 RID: 980
		// (get) Token: 0x06000C9C RID: 3228 RVA: 0x0004B061 File Offset: 0x00049261
		// (set) Token: 0x06000C9D RID: 3229 RVA: 0x0004B069 File Offset: 0x00049269
		public RelayCommand CleanUpCommand { get; set; }

		// Token: 0x170003D5 RID: 981
		// (get) Token: 0x06000C9E RID: 3230 RVA: 0x0004B074 File Offset: 0x00049274
		// (set) Token: 0x06000C9F RID: 3231 RVA: 0x0004B08C File Offset: 0x0004928C
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

		// Token: 0x170003D6 RID: 982
		// (get) Token: 0x06000CA0 RID: 3232 RVA: 0x0004B0A2 File Offset: 0x000492A2
		// (set) Token: 0x06000CA1 RID: 3233 RVA: 0x0004B0AA File Offset: 0x000492AA
		public BoundLocalizedText StrTitle { get; set; }

		// Token: 0x170003D7 RID: 983
		// (get) Token: 0x06000CA2 RID: 3234 RVA: 0x0004B0B3 File Offset: 0x000492B3
		// (set) Token: 0x06000CA3 RID: 3235 RVA: 0x0004B0BB File Offset: 0x000492BB
		public BoundLocalizedText StrCurrentVal { get; set; }

		// Token: 0x170003D8 RID: 984
		// (get) Token: 0x06000CA4 RID: 3236 RVA: 0x0004B0C4 File Offset: 0x000492C4
		// (set) Token: 0x06000CA5 RID: 3237 RVA: 0x0004B0CC File Offset: 0x000492CC
		public BoundLocalizedText StrLimits { get; set; }

		// Token: 0x170003D9 RID: 985
		// (get) Token: 0x06000CA6 RID: 3238 RVA: 0x0004B0D5 File Offset: 0x000492D5
		// (set) Token: 0x06000CA7 RID: 3239 RVA: 0x0004B0DD File Offset: 0x000492DD
		public BoundLocalizedText StrBackground { get; set; }

		// Token: 0x170003DA RID: 986
		// (get) Token: 0x06000CA8 RID: 3240 RVA: 0x0004B0E8 File Offset: 0x000492E8
		// (set) Token: 0x06000CA9 RID: 3241 RVA: 0x0004B100 File Offset: 0x00049300
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

		// Token: 0x170003DB RID: 987
		// (get) Token: 0x06000CAA RID: 3242 RVA: 0x0004B116 File Offset: 0x00049316
		// (set) Token: 0x06000CAB RID: 3243 RVA: 0x0004B11E File Offset: 0x0004931E
		public BoundLocalizedText StrMarkedPixel { get; set; }

		// Token: 0x170003DC RID: 988
		// (get) Token: 0x06000CAC RID: 3244 RVA: 0x0004B127 File Offset: 0x00049327
		// (set) Token: 0x06000CAD RID: 3245 RVA: 0x0004B12F File Offset: 0x0004932F
		public BoundLocalizedText StrBgMonitoringThreshold { get; set; }

		// Token: 0x170003DD RID: 989
		// (get) Token: 0x06000CAE RID: 3246 RVA: 0x0004B138 File Offset: 0x00049338
		// (set) Token: 0x06000CAF RID: 3247 RVA: 0x0004B140 File Offset: 0x00049340
		public string StrBgMonitoringThresholdCombined { get; set; }

		// Token: 0x06000CB0 RID: 3248 RVA: 0x0004B14C File Offset: 0x0004934C
		public BackgroundLiveMonitoringViewModel(DBDataLogs dBDataLogs, DBConnection dBConnection, ParameterHandling parameterHandling, bool EnableLimits)
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

		// Token: 0x06000CB1 RID: 3249 RVA: 0x0004B1E4 File Offset: 0x000493E4
		private void Initialize()
		{
			string currentValue = this.parameterHandling.ReadPar(Definition.detectorMonitoringPixelCount);
			string limit = this.parameterHandling.ReadPar(Definition.detectorMonitoringPixelCountBorder);
			this.items.Add(new MonitoringItem(this.StrMarkedPixel.LocalizedText, currentValue, limit, "", this.EnableLimits));
			this.items.Add(new MonitoringItem(this.StrBgMonitoringThresholdCombined, "n.a.", Convert.ToString(this.calculateThresholdGrayValue()), "", this.EnableLimits));
		}

		// Token: 0x06000CB2 RID: 3250 RVA: 0x0004B270 File Offset: 0x00049470
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x06000CB3 RID: 3251 RVA: 0x0004B2A0 File Offset: 0x000494A0
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

		// Token: 0x06000CB4 RID: 3252 RVA: 0x0004B31C File Offset: 0x0004951C
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

		// Token: 0x06000CB5 RID: 3253 RVA: 0x0004B394 File Offset: 0x00049594
		public void SetNewLimit(MonitoringItem item)
		{
			bool flag = item.Name.Equals(this.StrMarkedPixel.LocalizedText);
			if (flag)
			{
				this.parameterHandling.WriteValue(Definition.detectorMonitoringPixelCountBorder, item.Limit);
			}
			else
			{
				bool flag2 = item.Name.Equals(this.StrBgMonitoringThresholdCombined);
				if (flag2)
				{
					this.parameterHandling.WriteValue(Definition.detectorMonitoringThreshold, this.calculateThresholdInPercent(item.Limit));
				}
			}
		}

		// Token: 0x06000CB6 RID: 3254 RVA: 0x0004B40C File Offset: 0x0004960C
		private void updateViewModelValues(object dummy, EventArgs e)
		{
			bool flag = this.Items != null && this.Items.Count == 2;
			if (flag)
			{
				this.Items[0].CurrentValue = this.parameterHandling.ReadPar(Definition.detectorMonitoringPixelCount);
			}
		}

		// Token: 0x06000CB7 RID: 3255 RVA: 0x0004B45C File Offset: 0x0004965C
		private int calculateThresholdGrayValue()
		{
			double num = Convert.ToDouble(this.parameterHandling.ReadPar(Definition.detectorMonitoringThreshold));
			return Convert.ToInt32(10000.0 - num * 100.0);
		}

		// Token: 0x06000CB8 RID: 3256 RVA: 0x0004B4A0 File Offset: 0x000496A0
		private string calculateThresholdInPercent(string itemval)
		{
			double value = (10000.0 - Convert.ToDouble(itemval)) / 100.0;
			return Convert.ToString(value);
		}

		// Token: 0x06000CB9 RID: 3257 RVA: 0x0004B4D4 File Offset: 0x000496D4
		private void RegisterLocalizedText()
		{
			this.StrTitle = new BoundLocalizedText("txtBeltContamination");
			this.StrBackground = new BoundLocalizedText("txtBackground");
			this.StrLimits = new BoundLocalizedText("txtLimits");
			this.StrCurrentVal = new BoundLocalizedText("txtCurrentValue");
			this.StrMarkedPixel = new BoundLocalizedText("txtMarkedPixel");
			this.StrBgMonitoringThreshold = new BoundLocalizedText("txtThreshold");
			this.StrBgMonitoringThresholdCombined = this.StrBackground.LocalizedText + " " + this.StrBgMonitoringThreshold.LocalizedText;
		}

		// Token: 0x06000CBA RID: 3258 RVA: 0x0004B570 File Offset: 0x00049770
		private void DeregisterLocalizedText()
		{
			MainWindow.localizationManager.Deregister(this.StrLimits);
			MainWindow.localizationManager.Deregister(this.StrCurrentVal);
			MainWindow.localizationManager.Deregister(this.StrTitle);
			MainWindow.localizationManager.Deregister(this.StrMarkedPixel);
			MainWindow.localizationManager.Deregister(this.StrBgMonitoringThreshold);
			MainWindow.localizationManager.Deregister(this.StrBackground);
		}

		// Token: 0x06000CBB RID: 3259 RVA: 0x0001092C File Offset: 0x0000EB2C
		public override void ShowDiagramm()
		{
		}

		// Token: 0x06000CBC RID: 3260 RVA: 0x0004B5E4 File Offset: 0x000497E4
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

		// Token: 0x0400084A RID: 2122
		private NumKeyboard numKeyboard;

		// Token: 0x0400084B RID: 2123
		private MonitoringItem selectedItem;

		// Token: 0x0400084C RID: 2124
		private int limitNumber;

		// Token: 0x0400084D RID: 2125
		private List<MonitoringItem> items = new List<MonitoringItem>();

		// Token: 0x04000852 RID: 2130
		private DBDataLogs dBDataLogs;

		// Token: 0x04000853 RID: 2131
		private DBConnection dbConnection;

		// Token: 0x04000854 RID: 2132
		private ParameterHandling parameterHandling;

		// Token: 0x04000855 RID: 2133
		private bool enableLimits = false;

		// Token: 0x04000859 RID: 2137
		private DispatcherTimer timerUpdateParam2sec;
	}
}
