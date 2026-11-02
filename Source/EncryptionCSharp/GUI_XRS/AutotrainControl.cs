using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Threading;
using DatabaseConnection;
using Definitions;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x02000197 RID: 407
	public class AutotrainControl : UserControl, IComponentConnector
	{
		// Token: 0x17000AFD RID: 2813
		// (get) Token: 0x060026AC RID: 9900 RVA: 0x00106F3D File Offset: 0x0010513D
		// (set) Token: 0x060026AD RID: 9901 RVA: 0x00106F45 File Offset: 0x00105145
		public List<Button> Buttons { get; set; }

		// Token: 0x060026AE RID: 9902 RVA: 0x00106F50 File Offset: 0x00105150
		public AutotrainControl(ImageDataTransfer imageTransfer, ParameterHandling parHandling, DBConnection dbConnection, AutolearnType type, MachineState machineState, ProductManager productManager, int learnedGroup, XrayState xrayState, bool noAutoKVAdjustment, bool autokVChangedToDisabled, AccessRightsManager accessManager, TestcardSimService testcardSimService, Sensitivity bulksensitivity)
		{
			this.InitializeComponent();
			this.testcardSimService = testcardSimService;
			this.parHandling = parHandling;
			dbConnection.GetNAutolearn(ref this.nAutolearn);
			dbConnection.GetMachineMode(ref this.machineMode);
			bool flag = this.machineMode == MachineMode.Bulk;
			if (flag)
			{
				parHandling.WriteValue("/filter/acquisition/ContinuousMode/Value", "0");
			}
			this.autotrain = new Autotrain(dbConnection, parHandling, imageTransfer, this.nAutolearn, type, machineState, productManager, learnedGroup, xrayState, noAutoKVAdjustment, autokVChangedToDisabled, testcardSimService, this.imagebox, bulksensitivity);
			this.autotrain.AutotrainFinished += this.autotrain_AutotrainFinished;
			this.autotrain.SendTestcardSimImages += this.autotrain_SendTestcardSimImg;
			this.autotrain.PropertyChanged += this.autotrain_PropertyChanged;
			this.InitializeButtons();
			this.timerBtnAbortVisibility = new DispatcherTimer(new TimeSpan(0, 0, 30), DispatcherPriority.Normal, new EventHandler(this.MakeBtnAbortVisible), base.Dispatcher);
			this.timerBtnAbortVisibility.Stop();
			this.accessManager = accessManager;
			accessManager.StopTimerAutoLogout();
			base.DataContext = this.autotrain;
		}

		// Token: 0x060026AF RID: 9903 RVA: 0x00107090 File Offset: 0x00105290
		private void InitializeButtons()
		{
			this.Buttons = new List<Button>();
			this.btnAbort = new Button();
			this.btnAbort.Content = ImageConverter.ToImage("/img/cancel.png");
			this.btnAbort.Click += this.AbortAndClose;
			this.btnAbort.Style = (base.FindResource("cancelButtonStyle") as Style);
			this.btnAbort.Visibility = ((!this.autotrain.AdjustingKV) ? Visibility.Visible : Visibility.Collapsed);
			this.SetBtnAbortVisibility();
			this.Buttons.Add(this.btnAbort);
		}

		// Token: 0x060026B0 RID: 9904 RVA: 0x00107135 File Offset: 0x00105335
		public void CloseSubWindow()
		{
			this.accessManager.StartTimerAutoLogout();
			this.autotrain.CleanUp();
			Thread.Sleep(1500);
			base.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
			{
				bool flag = Window.GetWindow(this) != null;
				if (flag)
				{
					(Window.GetWindow(this) as Subwindow).Close();
				}
			}));
		}

		// Token: 0x060026B1 RID: 9905 RVA: 0x00107178 File Offset: 0x00105378
		private void AbortAndClose(object sender, RoutedEventArgs e)
		{
			this.accessManager.StartTimerAutoLogout();
			this.autotrain.AbortTrain(true);
			this.autotrainAborted = true;
			Thread thread = new Thread(new ThreadStart(this.CloseSubWindow));
			thread.Start();
		}

		// Token: 0x060026B2 RID: 9906 RVA: 0x001071C0 File Offset: 0x001053C0
		private void AutotrainControl_Unloaded(object sender, RoutedEventArgs e)
		{
			bool flag = !this.autotrainAborted;
			if (flag)
			{
				this.autotrain.AbortTrain(false);
			}
			this.autotrain.AutotrainFinished -= this.autotrain_AutotrainFinished;
			this.autotrain.SendTestcardSimImages -= this.autotrain_SendTestcardSimImg;
			bool flag2 = this.testcardSimService != null;
			if (flag2)
			{
				this.testcardSimService.MonitoringSensIsActive = true;
			}
			this.parHandling.WriteValue(Definition.autoKVInitImagePath, "0");
			this.autotrain.Image = null;
			this.Buttons.Clear();
		}

		// Token: 0x060026B3 RID: 9907 RVA: 0x00107264 File Offset: 0x00105464
		private void autotrain_AutotrainFinished()
		{
			Thread thread = new Thread(new ThreadStart(this.CloseSubWindow));
			thread.Start();
		}

		// Token: 0x060026B4 RID: 9908 RVA: 0x0010728C File Offset: 0x0010548C
		private void autotrain_SendTestcardSimImg()
		{
			Thread thread = new Thread(delegate()
			{
				this.autotrain.initprogBarSendingTestcardSim();
				this.testcardSimService.TestcardSimAfterTrainingSendImg(true, null, false);
			});
			thread.Start();
		}

		// Token: 0x060026B5 RID: 9909 RVA: 0x001072B3 File Offset: 0x001054B3
		private void autotrain_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			this.SetBtnAbortVisibility();
		}

		// Token: 0x060026B6 RID: 9910 RVA: 0x001072C0 File Offset: 0x001054C0
		private void SetBtnAbortVisibility()
		{
			bool flag = this.btnAbort != null;
			if (flag)
			{
				Visibility visibility = this.autotrain.AdjustingKV ? Visibility.Hidden : Visibility.Visible;
				bool flag2 = this.btnAbort.Visibility != visibility;
				if (flag2)
				{
					this.btnAbort.Visibility = visibility;
				}
				bool flag3 = this.timerBtnAbortVisibility != null;
				if (flag3)
				{
					bool flag4 = this.btnAbort.Visibility > Visibility.Visible;
					if (flag4)
					{
						this.timerBtnAbortVisibility.Start();
					}
					else
					{
						this.timerBtnAbortVisibility.Stop();
					}
				}
			}
		}

		// Token: 0x060026B7 RID: 9911 RVA: 0x0010734E File Offset: 0x0010554E
		private void MakeBtnAbortVisible(object sender, EventArgs e)
		{
			this.btnAbort.Visibility = Visibility.Visible;
		}

		// Token: 0x060026B8 RID: 9912 RVA: 0x00107360 File Offset: 0x00105560
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/autotrain/autotraincontrol.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x060026B9 RID: 9913 RVA: 0x00107398 File Offset: 0x00105598
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				((AutotrainControl)target).Unloaded += this.AutotrainControl_Unloaded;
				break;
			case 2:
				this.mainGrid = (Grid)target;
				break;
			case 3:
				this.imagebox = (XRSImageBox)target;
				break;
			default:
				this._contentLoaded = true;
				break;
			}
		}

		// Token: 0x040018CA RID: 6346
		private Autotrain autotrain;

		// Token: 0x040018CB RID: 6347
		private AccessRightsManager accessManager;

		// Token: 0x040018CC RID: 6348
		private int nAutolearn = 5;

		// Token: 0x040018CD RID: 6349
		private bool autotrainAborted;

		// Token: 0x040018CE RID: 6350
		private Button btnAbort;

		// Token: 0x040018CF RID: 6351
		private DispatcherTimer timerBtnAbortVisibility = null;

		// Token: 0x040018D0 RID: 6352
		private TestcardSimService testcardSimService;

		// Token: 0x040018D1 RID: 6353
		private ParameterHandling parHandling;

		// Token: 0x040018D3 RID: 6355
		private MachineMode machineMode;

		// Token: 0x040018D4 RID: 6356
		internal Grid mainGrid;

		// Token: 0x040018D5 RID: 6357
		internal XRSImageBox imagebox;

		// Token: 0x040018D6 RID: 6358
		private bool _contentLoaded;
	}
}
