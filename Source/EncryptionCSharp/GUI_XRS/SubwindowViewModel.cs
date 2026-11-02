using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using DatabaseConnection;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x02000255 RID: 597
	public class SubwindowViewModel : INotifyPropertyChanged
	{
		// Token: 0x17000C3D RID: 3133
		// (get) Token: 0x06002DA5 RID: 11685 RVA: 0x0012A2E4 File Offset: 0x001284E4
		// (set) Token: 0x06002DA6 RID: 11686 RVA: 0x0012A2FC File Offset: 0x001284FC
		public Visibility BackVisibility
		{
			get
			{
				return this.backVisibility;
			}
			private set
			{
				bool flag = this.backVisibility != value;
				if (flag)
				{
					this.backVisibility = value;
				}
			}
		}

		// Token: 0x17000C3E RID: 3134
		// (get) Token: 0x06002DA7 RID: 11687 RVA: 0x0012A324 File Offset: 0x00128524
		// (set) Token: 0x06002DA8 RID: 11688 RVA: 0x0012A33C File Offset: 0x0012853C
		public Visibility HomeVisibility
		{
			get
			{
				return this.homeVisibility;
			}
			private set
			{
				bool flag = this.homeVisibility != value;
				if (flag)
				{
					this.homeVisibility = value;
				}
			}
		}

		// Token: 0x17000C3F RID: 3135
		// (get) Token: 0x06002DA9 RID: 11689 RVA: 0x0012A364 File Offset: 0x00128564
		// (set) Token: 0x06002DAA RID: 11690 RVA: 0x0012A37C File Offset: 0x0012857C
		public Visibility ConfirmVisibility
		{
			get
			{
				return this.confirmVisibility;
			}
			set
			{
				bool flag = this.confirmVisibility != value;
				if (flag)
				{
					this.confirmVisibility = value;
					this.OnPropertyChanged("ConfirmVisibility");
				}
			}
		}

		// Token: 0x17000C40 RID: 3136
		// (get) Token: 0x06002DAB RID: 11691 RVA: 0x0012A3B0 File Offset: 0x001285B0
		// (set) Token: 0x06002DAC RID: 11692 RVA: 0x0012A3C8 File Offset: 0x001285C8
		public Visibility CancelVisibility
		{
			get
			{
				return this.cancelVisibility;
			}
			private set
			{
				bool flag = this.cancelVisibility != value;
				if (flag)
				{
					this.cancelVisibility = value;
				}
			}
		}

		// Token: 0x17000C41 RID: 3137
		// (get) Token: 0x06002DAD RID: 11693 RVA: 0x0012A3F0 File Offset: 0x001285F0
		// (set) Token: 0x06002DAE RID: 11694 RVA: 0x0012A408 File Offset: 0x00128608
		public Visibility AutotrainVisibility
		{
			get
			{
				return this.autotrainVisibility;
			}
			private set
			{
				bool flag = this.autotrainVisibility != value;
				if (flag)
				{
					this.autotrainVisibility = value;
				}
			}
		}

		// Token: 0x17000C42 RID: 3138
		// (get) Token: 0x06002DAF RID: 11695 RVA: 0x0012A42F File Offset: 0x0012862F
		// (set) Token: 0x06002DB0 RID: 11696 RVA: 0x0012A437 File Offset: 0x00128637
		public BoundLocalizedText StrAutoEdgeFilterActive { get; set; }

		// Token: 0x17000C43 RID: 3139
		// (get) Token: 0x06002DB1 RID: 11697 RVA: 0x0012A440 File Offset: 0x00128640
		// (set) Token: 0x06002DB2 RID: 11698 RVA: 0x0012A448 File Offset: 0x00128648
		public BoundLocalizedText StrAutoKVActive { get; set; }

		// Token: 0x17000C44 RID: 3140
		// (get) Token: 0x06002DB3 RID: 11699 RVA: 0x0012A451 File Offset: 0x00128651
		// (set) Token: 0x06002DB4 RID: 11700 RVA: 0x0012A459 File Offset: 0x00128659
		public BoundLocalizedText StrXRayVoltage { get; set; }

		// Token: 0x17000C45 RID: 3141
		// (get) Token: 0x06002DB5 RID: 11701 RVA: 0x0012A462 File Offset: 0x00128662
		// (set) Token: 0x06002DB6 RID: 11702 RVA: 0x0012A46A File Offset: 0x0012866A
		public BoundLocalizedText StrXRayCurrent { get; set; }

		// Token: 0x17000C46 RID: 3142
		// (get) Token: 0x06002DB7 RID: 11703 RVA: 0x0012A473 File Offset: 0x00128673
		// (set) Token: 0x06002DB8 RID: 11704 RVA: 0x0012A47B File Offset: 0x0012867B
		public BoundLocalizedText StrEdgeFilterActive { get; set; }

		// Token: 0x17000C47 RID: 3143
		// (get) Token: 0x06002DB9 RID: 11705 RVA: 0x0012A484 File Offset: 0x00128684
		// (set) Token: 0x06002DBA RID: 11706 RVA: 0x0012A48C File Offset: 0x0012868C
		public WindowState WState { get; set; }

		// Token: 0x17000C48 RID: 3144
		// (get) Token: 0x06002DBB RID: 11707 RVA: 0x0012A498 File Offset: 0x00128698
		// (set) Token: 0x06002DBC RID: 11708 RVA: 0x0003C95F File Offset: 0x0003AB5F
		public double WindowWidth
		{
			get
			{
				return Definition.windowWidth;
			}
			set
			{
				Definition.windowWidth = value;
			}
		}

		// Token: 0x17000C49 RID: 3145
		// (get) Token: 0x06002DBD RID: 11709 RVA: 0x0012A4B0 File Offset: 0x001286B0
		// (set) Token: 0x06002DBE RID: 11710 RVA: 0x0003C97F File Offset: 0x0003AB7F
		public double WindowHeight
		{
			get
			{
				return Definition.windowHeight;
			}
			set
			{
				Definition.windowHeight = value;
			}
		}

		// Token: 0x17000C4A RID: 3146
		// (get) Token: 0x06002DBF RID: 11711 RVA: 0x0012A4C8 File Offset: 0x001286C8
		// (set) Token: 0x06002DC0 RID: 11712 RVA: 0x0003C99F File Offset: 0x0003AB9F
		public double WLeft
		{
			get
			{
				return Definition.windowLeft;
			}
			set
			{
				Definition.windowLeft = value;
			}
		}

		// Token: 0x17000C4B RID: 3147
		// (get) Token: 0x06002DC1 RID: 11713 RVA: 0x0012A4E0 File Offset: 0x001286E0
		// (set) Token: 0x06002DC2 RID: 11714 RVA: 0x0003C9BF File Offset: 0x0003ABBF
		public double WTop
		{
			get
			{
				return Definition.windowTop;
			}
			set
			{
				Definition.windowTop = value;
			}
		}

		// Token: 0x17000C4C RID: 3148
		// (get) Token: 0x06002DC3 RID: 11715 RVA: 0x0012A4F8 File Offset: 0x001286F8
		// (set) Token: 0x06002DC4 RID: 11716 RVA: 0x0012A510 File Offset: 0x00128710
		public Brush BrushBackground
		{
			get
			{
				return this.brushBackground;
			}
			set
			{
				this.brushBackground = value;
				this.OnPropertyChanged("BrushBackground");
			}
		}

		// Token: 0x17000C4D RID: 3149
		// (get) Token: 0x06002DC5 RID: 11717 RVA: 0x0012A528 File Offset: 0x00128728
		// (set) Token: 0x06002DC6 RID: 11718 RVA: 0x0012A540 File Offset: 0x00128740
		public string VoltageText
		{
			get
			{
				return this.voltageText;
			}
			set
			{
				this.OnPropertyChanged("VoltageText");
				this.voltageText = value;
			}
		}

		// Token: 0x17000C4E RID: 3150
		// (get) Token: 0x06002DC7 RID: 11719 RVA: 0x0012A558 File Offset: 0x00128758
		// (set) Token: 0x06002DC8 RID: 11720 RVA: 0x0012A570 File Offset: 0x00128770
		public string MAText
		{
			get
			{
				return this.mAText;
			}
			set
			{
				this.OnPropertyChanged("MAText");
				this.mAText = value;
			}
		}

		// Token: 0x17000C4F RID: 3151
		// (get) Token: 0x06002DC9 RID: 11721 RVA: 0x0012A588 File Offset: 0x00128788
		// (set) Token: 0x06002DCA RID: 11722 RVA: 0x0012A5A0 File Offset: 0x001287A0
		public string AutoKVText
		{
			get
			{
				return this.autoKVText;
			}
			set
			{
				this.OnPropertyChanged("AutoKVText");
				this.autoKVText = value;
			}
		}

		// Token: 0x17000C50 RID: 3152
		// (get) Token: 0x06002DCB RID: 11723 RVA: 0x0012A5B6 File Offset: 0x001287B6
		// (set) Token: 0x06002DCC RID: 11724 RVA: 0x0012A5BE File Offset: 0x001287BE
		public bool AutoKVCheckBox { get; set; }

		// Token: 0x17000C51 RID: 3153
		// (get) Token: 0x06002DCD RID: 11725 RVA: 0x0012A5C8 File Offset: 0x001287C8
		// (set) Token: 0x06002DCE RID: 11726 RVA: 0x0012A5E0 File Offset: 0x001287E0
		public bool AutoEdgeFilterEnable
		{
			get
			{
				return this.autoEdgeFilterEnable;
			}
			set
			{
				this.OnPropertyChanged("AutoEdgeFilterEnable");
				this.autoEdgeFilterEnable = value;
			}
		}

		// Token: 0x17000C52 RID: 3154
		// (get) Token: 0x06002DCF RID: 11727 RVA: 0x0012A5F8 File Offset: 0x001287F8
		// (set) Token: 0x06002DD0 RID: 11728 RVA: 0x0012A610 File Offset: 0x00128810
		public bool EdgeActive
		{
			get
			{
				return this.edgeActive;
			}
			set
			{
				this.OnPropertyChanged("Edgeactive");
				this.edgeActive = value;
			}
		}

		// Token: 0x17000C53 RID: 3155
		// (get) Token: 0x06002DD1 RID: 11729 RVA: 0x0012A628 File Offset: 0x00128828
		public Machine Machine
		{
			get
			{
				return Machine.Instance;
			}
		}

		// Token: 0x14000105 RID: 261
		// (add) Token: 0x06002DD2 RID: 11730 RVA: 0x0012A640 File Offset: 0x00128840
		// (remove) Token: 0x06002DD3 RID: 11731 RVA: 0x0012A678 File Offset: 0x00128878
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x06002DD4 RID: 11732 RVA: 0x0012A6B0 File Offset: 0x001288B0
		private void VoltageCurrentChange(object sender, EventArgs e)
		{
			string text = this.parHandling.ReadPar("/hardware/xraysource/Voltage/Value");
			this.VoltageText = text + " kV";
			text = this.parHandling.ReadPar("/hardware/xraysource/Current/Value");
			this.MAText = text + " mA";
			text = this.parHandling.ReadPar("/filter/edge/Enabled/Value");
			this.EdgeActive = (text == "1");
		}

		// Token: 0x06002DD5 RID: 11733 RVA: 0x0012A730 File Offset: 0x00128930
		public SubwindowViewModel(SubwindowButtons buttonOption, bool warningStyle, ParameterHandling parHandling, ProductManager productManager, DBConnection dbConnection)
		{
			this.parHandling = parHandling;
			this.dbConnection = dbConnection;
			this.isConfirButtonDemomode = true;
			bool flag = dbConnection != null;
			if (flag)
			{
				dbConnection.GetDemoMode(ref this.demoMode);
			}
			this.WState = ((Definition.windowHeight > 0.0 && Definition.windowWidth > 0.0) ? WindowState.Normal : WindowState.Maximized);
			this.SetButtonVisibility(buttonOption, productManager, parHandling);
			this.SetBrushBackground(warningStyle);
			bool flag2 = this.AutotrainVisibility == Visibility.Visible;
			if (flag2)
			{
				this.RegisterLocalizedText();
				uint num = 0U;
				bool flag3 = parHandling != null;
				if (flag3)
				{
					string text = parHandling.ReadPar("/hardware/xraysource/Voltage/Value");
					this.voltageText = text + " kV";
					text = parHandling.ReadPar("/hardware/xraysource/Current/Value");
					this.mAText = text + " mA";
					text = parHandling.ReadPar("/filter/edge/autoDetect/Enabled/Value");
					this.autoEdgeFilterEnable = (text == "1");
					text = parHandling.ReadPar("/filter/edge/Enabled/Value");
					this.edgeActive = (text == "1");
					dbConnection.GetAutoKVAdjustment(ref num);
					this.AutoKVCheckBox = false;
					bool flag4 = num == 0U;
					if (flag4)
					{
						this.autoKVText = "";
						this.AutoKVCheckBox = true;
					}
					else
					{
						bool flag5 = num == 1U;
						if (flag5)
						{
							this.autoKVText = "FP";
						}
						else
						{
							bool flag6 = num == 2U;
							if (flag6)
							{
								this.autoKVText = "ECO";
							}
						}
					}
					this.voltcurrtimer.Tick += this.VoltageCurrentChange;
					this.voltcurrtimer.Interval = new TimeSpan(0, 0, 0, 1, 0);
					this.voltcurrtimer.Start();
					this.isConfirButtonDemomode = false;
				}
			}
			this.timerIsMachineCalibrating = new Timer(new TimerCallback(this.CheckMachineState), null, 0, 500);
		}

		// Token: 0x06002DD6 RID: 11734 RVA: 0x0012A9D8 File Offset: 0x00128BD8
		private void SetBrushBackground(bool warningStyle)
		{
			bool flag = !warningStyle;
			if (flag)
			{
				this.BrushBackground = this.standardBrush;
			}
			else
			{
				this.BrushBackground = this.standardBrush;
				this.timerChangeBackground = new DispatcherTimer();
				this.timerChangeBackground.Tick += this.changeBackground_Tick;
				this.timerChangeBackground.Interval = new TimeSpan(0, 0, 1);
				this.timerChangeBackground.Start();
			}
		}

		// Token: 0x06002DD7 RID: 11735 RVA: 0x0012AA54 File Offset: 0x00128C54
		public void CleanUp()
		{
			bool flag = this.timerChangeBackground != null;
			if (flag)
			{
				this.timerChangeBackground.Stop();
				this.timerChangeBackground.Tick -= this.changeBackground_Tick;
			}
			bool flag2 = this.voltcurrtimer != null && this.AutotrainVisibility == Visibility.Visible;
			if (flag2)
			{
				this.voltcurrtimer.Stop();
				this.voltcurrtimer.Tick -= this.VoltageCurrentChange;
			}
			bool flag3 = this.timerIsMachineCalibrating != null;
			if (flag3)
			{
				this.timerIsMachineCalibrating.Dispose();
			}
		}

		// Token: 0x06002DD8 RID: 11736 RVA: 0x0012AAF0 File Offset: 0x00128CF0
		private void CheckMachineState(object obj)
		{
			bool flag = this.parHandling != null;
			if (flag)
			{
				string a = this.parHandling.ReadPar("/machine/MachineState/Value");
				bool flag2 = a == "Inspecting" || (this.demoMode && this.isConfirButtonDemomode);
				if (flag2)
				{
					this.ConfirmVisibility = Visibility.Visible;
				}
				else
				{
					this.ConfirmVisibility = Visibility.Collapsed;
				}
			}
		}

		// Token: 0x06002DD9 RID: 11737 RVA: 0x0012AB5C File Offset: 0x00128D5C
		private bool SetAutotrainVisibility(ProductManager productManager, ParameterHandling parHandling)
		{
			bool flag = false;
			bool flag2 = parHandling != null;
			if (flag2)
			{
				DetectorType detectorType = DetectorType.Unknown;
				this.dbConnection.GetDetectorType(ref detectorType);
				bool flag3 = true;
				this.dbConnection.GetMXFeatures04Det(ref flag3);
				MachineMode machineMode = MachineMode.Standard;
				this.dbConnection.GetMachineMode(ref machineMode);
				string a = "";
				this.dbConnection.GetXRaySourceModelNumber(ref a);
				bool flag4 = false;
				bool flag5 = detectorType == DetectorType.Sesotec;
				if (flag5)
				{
					flag4 = (parHandling.ReadPar(Definition.detectorSesotecPixelSizePath) == "0.4");
				}
				else
				{
					bool flag6 = detectorType == DetectorType.DTNetTDI;
					if (flag6)
					{
						flag4 = (parHandling.ReadPar(Definition.detectorDtPixelSizePath) == "0.4");
					}
				}
				bool flag7 = Machine.Instance.MachineType == MachineType.RayconDPlus && flag4 && machineMode != MachineMode.Bulk;
				if (flag7)
				{
					bool flag8 = productManager.CurrentLoadedProduct.IsSingleProduct || productManager.CurrentLoadedProduct.IsRegionProduct;
					if (flag8)
					{
						flag = true;
					}
					bool flag9 = flag && a == "IXS050SE100P214" && flag3;
					if (flag9)
					{
						flag = false;
					}
				}
			}
			return flag;
		}

		// Token: 0x06002DDA RID: 11738 RVA: 0x0012AC84 File Offset: 0x00128E84
		private void SetButtonVisibility(SubwindowButtons buttonOption, ProductManager productManager, ParameterHandling parHandling)
		{
			switch (buttonOption)
			{
			case SubwindowButtons.None:
				this.BackVisibility = Visibility.Collapsed;
				this.HomeVisibility = Visibility.Collapsed;
				this.ConfirmVisibility = Visibility.Collapsed;
				this.CancelVisibility = Visibility.Collapsed;
				this.AutotrainVisibility = Visibility.Collapsed;
				break;
			case SubwindowButtons.Back:
				this.BackVisibility = Visibility.Visible;
				this.HomeVisibility = Visibility.Collapsed;
				this.ConfirmVisibility = Visibility.Collapsed;
				this.CancelVisibility = Visibility.Collapsed;
				this.AutotrainVisibility = Visibility.Collapsed;
				break;
			case SubwindowButtons.Home:
				this.BackVisibility = Visibility.Collapsed;
				this.HomeVisibility = Visibility.Visible;
				this.ConfirmVisibility = Visibility.Collapsed;
				this.CancelVisibility = Visibility.Collapsed;
				this.AutotrainVisibility = Visibility.Collapsed;
				break;
			case SubwindowButtons.BackAndHome:
				this.BackVisibility = Visibility.Visible;
				this.HomeVisibility = Visibility.Visible;
				this.ConfirmVisibility = Visibility.Collapsed;
				this.CancelVisibility = Visibility.Collapsed;
				this.AutotrainVisibility = Visibility.Collapsed;
				break;
			case SubwindowButtons.ConfirmAndCancel:
				this.BackVisibility = Visibility.Collapsed;
				this.HomeVisibility = Visibility.Collapsed;
				this.ConfirmVisibility = Visibility.Visible;
				this.CancelVisibility = Visibility.Visible;
				this.AutotrainVisibility = Visibility.Collapsed;
				break;
			case SubwindowButtons.Autotrain:
			{
				this.BackVisibility = Visibility.Collapsed;
				this.HomeVisibility = Visibility.Collapsed;
				this.ConfirmVisibility = Visibility.Collapsed;
				this.CancelVisibility = Visibility.Collapsed;
				bool flag = this.SetAutotrainVisibility(productManager, parHandling);
				if (flag)
				{
					this.AutotrainVisibility = Visibility.Visible;
				}
				else
				{
					this.AutotrainVisibility = Visibility.Collapsed;
				}
				break;
			}
			}
		}

		// Token: 0x06002DDB RID: 11739 RVA: 0x0012ADD4 File Offset: 0x00128FD4
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x06002DDC RID: 11740 RVA: 0x0012AE04 File Offset: 0x00129004
		private void RegisterLocalizedText()
		{
			this.StrAutoEdgeFilterActive = new BoundLocalizedText("txtAutoEdgeFilterActive");
			this.StrAutoKVActive = new BoundLocalizedText("txtAutoKVActive");
			this.StrXRayVoltage = new BoundLocalizedText("txtXRayVoltage");
			this.StrXRayCurrent = new BoundLocalizedText("txtXRayCurrent");
			this.StrEdgeFilterActive = new BoundLocalizedText("txtEdgeFilterActive");
		}

		// Token: 0x06002DDD RID: 11741 RVA: 0x0012AE68 File Offset: 0x00129068
		public void DeregisterLocalizedText()
		{
			MainWindow.localizationManager.Deregister(this.StrAutoEdgeFilterActive);
			MainWindow.localizationManager.Deregister(this.StrAutoKVActive);
			MainWindow.localizationManager.Deregister(this.StrXRayVoltage);
			MainWindow.localizationManager.Deregister(this.StrXRayCurrent);
			MainWindow.localizationManager.Deregister(this.StrEdgeFilterActive);
		}

		// Token: 0x06002DDE RID: 11742 RVA: 0x0012AECB File Offset: 0x001290CB
		private void changeBackground_Tick(object sender, EventArgs e)
		{
			this.BrushBackground = ((this.BrushBackground == this.warningBrush) ? this.standardBrush : this.warningBrush);
		}

		// Token: 0x04001C88 RID: 7304
		private LinearGradientBrush standardBrush = new LinearGradientBrush(Color.FromRgb(222, 222, 222), Color.FromRgb(236, 234, 234), new Point(0.0, 0.0), new Point(0.0, 1.0));

		// Token: 0x04001C89 RID: 7305
		private LinearGradientBrush warningBrush = new LinearGradientBrush(Color.FromRgb(237, 35, 0), Color.FromRgb(237, 35, 0), new Point(0.0, 0.0), new Point(0.0, 1.0));

		// Token: 0x04001C8A RID: 7306
		private Brush brushBackground;

		// Token: 0x04001C8B RID: 7307
		private DispatcherTimer timerChangeBackground;

		// Token: 0x04001C8C RID: 7308
		private Visibility backVisibility;

		// Token: 0x04001C8D RID: 7309
		private Visibility homeVisibility;

		// Token: 0x04001C8E RID: 7310
		private Visibility confirmVisibility;

		// Token: 0x04001C8F RID: 7311
		private Visibility cancelVisibility;

		// Token: 0x04001C90 RID: 7312
		private Visibility autotrainVisibility;

		// Token: 0x04001C91 RID: 7313
		private ParameterHandling parHandling;

		// Token: 0x04001C92 RID: 7314
		private DispatcherTimer voltcurrtimer = new DispatcherTimer();

		// Token: 0x04001C99 RID: 7321
		private string voltageText;

		// Token: 0x04001C9A RID: 7322
		private string mAText;

		// Token: 0x04001C9B RID: 7323
		private bool autoEdgeFilterEnable;

		// Token: 0x04001C9C RID: 7324
		private string autoKVText;

		// Token: 0x04001C9D RID: 7325
		private bool edgeActive;

		// Token: 0x04001C9E RID: 7326
		private DBConnection dbConnection;

		// Token: 0x04001C9F RID: 7327
		private bool isConfirButtonDemomode;

		// Token: 0x04001CA1 RID: 7329
		private Timer timerIsMachineCalibrating;

		// Token: 0x04001CA3 RID: 7331
		private bool demoMode;
	}
}
