using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using DatabaseConnection;
using Definitions;
using GUI_XRS.Graph;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x0200004E RID: 78
	public class CtrlSVT : UserControl, IComponentConnector
	{
		// Token: 0x1400002D RID: 45
		// (add) Token: 0x060007B3 RID: 1971 RVA: 0x00031EEC File Offset: 0x000300EC
		// (remove) Token: 0x060007B4 RID: 1972 RVA: 0x00031F24 File Offset: 0x00030124
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action GraphPlotInitialized;

		// Token: 0x1400002E RID: 46
		// (add) Token: 0x060007B5 RID: 1973 RVA: 0x00031F5C File Offset: 0x0003015C
		// (remove) Token: 0x060007B6 RID: 1974 RVA: 0x00031F94 File Offset: 0x00030194
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<MachineStates> GraphPlotUnloaded;

		// Token: 0x060007B7 RID: 1975 RVA: 0x00031FCC File Offset: 0x000301CC
		public CtrlSVT(ParameterHandling parHandling, DBConnection dbConnection, MachineState machineState, AccessRightsManager accessManager, ImageDataTransfer imageDataTransfer, DBDataLogs dbDataLogs)
		{
			this.InitializeComponent();
			this.accessManager = accessManager;
			this.parHandling = parHandling;
			this.machineState = machineState;
			this.dBConnection = dbConnection;
			this.imageDataTransfer = imageDataTransfer;
			this.dbDataLogs = dbDataLogs;
			this.viewModel = new SVTViewModel(parHandling, dbConnection, machineState, accessManager, imageDataTransfer, dbDataLogs);
			this.InitializeButtons(accessManager, dbConnection);
			base.DataContext = this.viewModel;
		}

		// Token: 0x060007B8 RID: 1976 RVA: 0x00032040 File Offset: 0x00030240
		private void InitializeButtons(AccessRightsManager accessManager, DBConnection dbConnection)
		{
			MachineMode machineMode = MachineMode.Standard;
			this.PanelButtons = new DockPanel();
			this.PanelButtons.HorizontalAlignment = HorizontalAlignment.Stretch;
			this.PanelButtons.VerticalAlignment = VerticalAlignment.Stretch;
			Grid.SetColumn(this.PanelButtons, 4);
			dbConnection.GetMachineMode(ref machineMode);
			bool flag = machineMode == MachineMode.Bulk;
			if (flag)
			{
				Button button = new Button();
				button.Content = ImageConverter.ToImage("/img/svtplot.png");
				button.Click += this.BtnBulk_Click;
				button.Style = (base.FindResource("defaultButtonStyle") as Style);
				button.Margin = new Thickness(0.0, 0.0, 10.0, 0.0);
				this.PanelButtons.Children.Add(button);
				DockPanel.SetDock(button, Dock.Left);
			}
		}

		// Token: 0x060007B9 RID: 1977 RVA: 0x00032124 File Offset: 0x00030324
		private void BtnBulk_Click(object sender, RoutedEventArgs e)
		{
			this.ShowBulk();
		}

		// Token: 0x060007BA RID: 1978 RVA: 0x00032130 File Offset: 0x00030330
		private void SVT_Loaded(object sender, RoutedEventArgs e)
		{
			Subwindow subwindow = Window.GetWindow(this) as Subwindow;
			subwindow.buttonGrid.Children.Remove(subwindow.buttonPanel);
			subwindow.buttonGrid.Children.Add(this.PanelButtons);
		}

		// Token: 0x060007BB RID: 1979 RVA: 0x00032178 File Offset: 0x00030378
		private void ShowBulk()
		{
			bool flag = this.machineState.State == MachineStates.Reseasoning || this.machineState.State == MachineStates.XrayOnCalibrationNotDone || this.machineState.State == MachineStates.MovingSource;
			if (flag)
			{
				Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtWaitingForCalibration"), DialogStyle.Warning, DialogButtons.OK);
			}
			else
			{
				bool flag2 = this.machineState.State != MachineStates.ViewingPlot;
				if (flag2)
				{
					bool flag3 = this.accessManager.CurrentUser.Rights == BuiltInPriviledges.Service || Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtTurningOffProductInspection"), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
					if (flag3)
					{
						this.stateBeforePlot = this.machineState.State;
						this.OnGraphPlotInitialized();
						this.parHandling.WriteValue(Definition.checkArcsEnable, "0");
						CtrlDetectorPlotSVT ctrlDetectorPlotSVT = new CtrlDetectorPlotSVT(this.parHandling, this.imageDataTransfer, this.accessManager, this.dBConnection, this.machineState, this.dbDataLogs);
						ctrlDetectorPlotSVT.Unloaded += this.plot_Unloaded;
						Subwindow.Show(ctrlDetectorPlotSVT, SubwindowButtons.Back);
					}
				}
			}
		}

		// Token: 0x060007BC RID: 1980 RVA: 0x000322AC File Offset: 0x000304AC
		protected virtual void OnGraphPlotInitialized()
		{
			Action graphPlotInitialized = this.GraphPlotInitialized;
			bool flag = graphPlotInitialized != null;
			if (flag)
			{
				graphPlotInitialized();
			}
		}

		// Token: 0x060007BD RID: 1981 RVA: 0x000322D0 File Offset: 0x000304D0
		protected virtual void OnGraphPlotUnloaded(MachineStates oldState)
		{
			Action<MachineStates> graphPlotUnloaded = this.GraphPlotUnloaded;
			bool flag = graphPlotUnloaded != null;
			if (flag)
			{
				graphPlotUnloaded(oldState);
			}
		}

		// Token: 0x060007BE RID: 1982 RVA: 0x000322F5 File Offset: 0x000304F5
		private void plot_Unloaded(object sender, RoutedEventArgs e)
		{
			this.viewModel.Update();
			this.OnGraphPlotUnloaded(this.stateBeforePlot);
		}

		// Token: 0x060007BF RID: 1983 RVA: 0x00032314 File Offset: 0x00030514
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/bulk/svt/ctrlsvt.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x060007C0 RID: 1984 RVA: 0x0003234C File Offset: 0x0003054C
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			if (connectionId != 1)
			{
				this._contentLoaded = true;
			}
			else
			{
				((CtrlSVT)target).Loaded += this.SVT_Loaded;
			}
		}

		// Token: 0x04000548 RID: 1352
		private SVTViewModel viewModel;

		// Token: 0x04000549 RID: 1353
		private AccessRightsManager accessManager;

		// Token: 0x0400054A RID: 1354
		private ParameterHandling parHandling;

		// Token: 0x0400054B RID: 1355
		public DockPanel PanelButtons;

		// Token: 0x0400054C RID: 1356
		private MachineState machineState;

		// Token: 0x0400054F RID: 1359
		private MachineStates stateBeforePlot;

		// Token: 0x04000550 RID: 1360
		private ImageDataTransfer imageDataTransfer;

		// Token: 0x04000551 RID: 1361
		private DBConnection dBConnection;

		// Token: 0x04000552 RID: 1362
		private DBDataLogs dbDataLogs;

		// Token: 0x04000553 RID: 1363
		private bool _contentLoaded;
	}
}
