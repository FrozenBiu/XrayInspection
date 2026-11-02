using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;
using Common;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x02000059 RID: 89
	public class VibroViewModel : INotifyPropertyChanged
	{
		// Token: 0x14000033 RID: 51
		// (add) Token: 0x060008D3 RID: 2259 RVA: 0x00037BD4 File Offset: 0x00035DD4
		// (remove) Token: 0x060008D4 RID: 2260 RVA: 0x00037C0C File Offset: 0x00035E0C
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x17000292 RID: 658
		// (get) Token: 0x060008D5 RID: 2261 RVA: 0x00037C41 File Offset: 0x00035E41
		// (set) Token: 0x060008D6 RID: 2262 RVA: 0x00037C49 File Offset: 0x00035E49
		public Vibro Vibro { get; set; }

		// Token: 0x17000293 RID: 659
		// (get) Token: 0x060008D7 RID: 2263 RVA: 0x00037C52 File Offset: 0x00035E52
		// (set) Token: 0x060008D8 RID: 2264 RVA: 0x00037C5A File Offset: 0x00035E5A
		public RelayCommand ChangeSpeedCommand { get; set; }

		// Token: 0x17000294 RID: 660
		// (get) Token: 0x060008D9 RID: 2265 RVA: 0x00037C63 File Offset: 0x00035E63
		// (set) Token: 0x060008DA RID: 2266 RVA: 0x00037C6B File Offset: 0x00035E6B
		public RelayCommand LowerSpeedCommand { get; set; }

		// Token: 0x17000295 RID: 661
		// (get) Token: 0x060008DB RID: 2267 RVA: 0x00037C74 File Offset: 0x00035E74
		// (set) Token: 0x060008DC RID: 2268 RVA: 0x00037C7C File Offset: 0x00035E7C
		public RelayCommand HigherSpeedCommand { get; set; }

		// Token: 0x17000296 RID: 662
		// (get) Token: 0x060008DD RID: 2269 RVA: 0x00037C88 File Offset: 0x00035E88
		// (set) Token: 0x060008DE RID: 2270 RVA: 0x00037CA0 File Offset: 0x00035EA0
		public bool SpeedEnabled
		{
			get
			{
				return this.speedEnabled;
			}
			set
			{
				this.speedEnabled = value;
				this.OnPropertyChanged("SpeedEnabled");
			}
		}

		// Token: 0x17000297 RID: 663
		// (get) Token: 0x060008DF RID: 2271 RVA: 0x00037CB8 File Offset: 0x00035EB8
		// (set) Token: 0x060008E0 RID: 2272 RVA: 0x00037CD0 File Offset: 0x00035ED0
		public bool ManualModeVisible
		{
			get
			{
				return this.manualModeVisible;
			}
			set
			{
				this.manualModeVisible = value;
				this.OnPropertyChanged("ManualModeVisible");
			}
		}

		// Token: 0x17000298 RID: 664
		// (get) Token: 0x060008E1 RID: 2273 RVA: 0x00037CE6 File Offset: 0x00035EE6
		// (set) Token: 0x060008E2 RID: 2274 RVA: 0x00037CEE File Offset: 0x00035EEE
		public BoundLocalizedText StrVibroSpeed { get; set; }

		// Token: 0x17000299 RID: 665
		// (get) Token: 0x060008E3 RID: 2275 RVA: 0x00037CF7 File Offset: 0x00035EF7
		// (set) Token: 0x060008E4 RID: 2276 RVA: 0x00037CFF File Offset: 0x00035EFF
		public BoundLocalizedText StrVibroInManualMode { get; set; }

		// Token: 0x060008E5 RID: 2277 RVA: 0x00037D08 File Offset: 0x00035F08
		public VibroViewModel(Vibro vibro, AccessRightsManager accessManager, ParameterHandling parHandling) : this(vibro, accessManager, parHandling, true)
		{
		}

		// Token: 0x060008E6 RID: 2278 RVA: 0x00037D18 File Offset: 0x00035F18
		public VibroViewModel(Vibro vibro, AccessRightsManager accessManager, ParameterHandling parHandling, bool startManModeTimer)
		{
			this.Vibro = vibro;
			this.accessManager = accessManager;
			this.parHandling = parHandling;
			this.RegisterLocalizedText();
			this.ChangeSpeedCommand = new RelayCommand(new Action<object>(this.ChangeSpeed));
			this.LowerSpeedCommand = new RelayCommand(new Action<object>(this.LowerSpeed));
			this.HigherSpeedCommand = new RelayCommand(new Action<object>(this.HigherSpeed));
			this.SpeedEnabled = (accessManager.CurrentUser.Rights >= this.rightSpeedEnabled);
			if (startManModeTimer)
			{
				this.SetManModeVisibility(null, new EventArgs());
				this.timerManMode = new DispatcherTimer(new TimeSpan(0, 0, 0, 1, 500), DispatcherPriority.Normal, new EventHandler(this.SetManModeVisibility), Application.Current.Dispatcher);
				this.timerManMode.Start();
			}
		}

		// Token: 0x060008E7 RID: 2279 RVA: 0x00037E18 File Offset: 0x00036018
		private void SetManModeVisibility(object sender, EventArgs e)
		{
			bool connectionEstablished = this.parHandling.ConnectionEstablished;
			if (connectionEstablished)
			{
				this.Vibro.VibroManualModeByGUI = (this.parHandling.ReadPar(Definition.vibroManualModeByGUIPath) == "1");
				bool flag = this.parHandling.ReadPar(XrayState.xrayStarterPath) == "0";
				if (flag)
				{
					this.ManualModeVisible = (this.accessManager.CurrentUser.Rights >= this.rightManualMode);
				}
				else
				{
					this.ManualModeVisible = false;
				}
			}
			else
			{
				this.Vibro.VibroManualModeByGUI = false;
				this.ManualModeVisible = false;
			}
		}

		// Token: 0x060008E8 RID: 2280 RVA: 0x00037EC0 File Offset: 0x000360C0
		private void RegisterLocalizedText()
		{
			this.StrVibroSpeed = new BoundLocalizedText("txtVibroSpeed");
			this.StrVibroInManualMode = new BoundLocalizedText("txtManualMode");
		}

		// Token: 0x060008E9 RID: 2281 RVA: 0x00037EE8 File Offset: 0x000360E8
		public void CleanUp()
		{
			bool flag = this.timerManMode != null;
			if (flag)
			{
				this.timerManMode.Stop();
			}
			MainWindow.localizationManager.Deregister(this.StrVibroSpeed);
			MainWindow.localizationManager.Deregister(this.StrVibroInManualMode);
		}

		// Token: 0x060008EA RID: 2282 RVA: 0x00037F34 File Offset: 0x00036134
		public void ChangeSpeed(object obj)
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtVibroSpeed"), this.Vibro.Speed.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_InputSpeed;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x060008EB RID: 2283 RVA: 0x00037F90 File Offset: 0x00036190
		private void numKeyboard_InputSpeed(object sender, KeyboardInputEventArgs e)
		{
			int speed = 0;
			try
			{
				this.numKeyboard.TextInputKeyboard -= this.numKeyboard_InputSpeed;
				bool flag = int.TryParse(e.Text, out speed);
				if (flag)
				{
					this.Vibro.Speed = speed;
				}
			}
			catch
			{
			}
		}

		// Token: 0x060008EC RID: 2284 RVA: 0x00037FF0 File Offset: 0x000361F0
		private void LowerSpeed(object obj)
		{
			this.Vibro.Speed -= Definition.vibroSpeedStep;
		}

		// Token: 0x060008ED RID: 2285 RVA: 0x0003800B File Offset: 0x0003620B
		private void HigherSpeed(object obj)
		{
			this.Vibro.Speed += Definition.vibroSpeedStep;
		}

		// Token: 0x060008EE RID: 2286 RVA: 0x00038028 File Offset: 0x00036228
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x04000603 RID: 1539
		private BuiltInPriviledges rightSpeedEnabled = BuiltInPriviledges.Operator1;

		// Token: 0x04000604 RID: 1540
		private BuiltInPriviledges rightManualMode = BuiltInPriviledges.Operator1;

		// Token: 0x04000605 RID: 1541
		private NumKeyboard numKeyboard;

		// Token: 0x04000606 RID: 1542
		private bool speedEnabled;

		// Token: 0x04000607 RID: 1543
		private AccessRightsManager accessManager;

		// Token: 0x04000608 RID: 1544
		private ParameterHandling parHandling;

		// Token: 0x04000609 RID: 1545
		private bool manualModeVisible;

		// Token: 0x0400060A RID: 1546
		private DispatcherTimer timerManMode;
	}
}
