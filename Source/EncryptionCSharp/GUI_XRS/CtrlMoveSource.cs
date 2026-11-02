using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Threading;
using DatabaseConnection;

namespace GUI_XRS
{
	// Token: 0x02000158 RID: 344
	public class CtrlMoveSource : UserControl, IComponentConnector
	{
		// Token: 0x060020A8 RID: 8360 RVA: 0x000D5199 File Offset: 0x000D3399
		public CtrlMoveSource(DBConnection dbConnection, ParameterHandling parHandling, MachineState machineState, AccessRightsManager accessManager) : this(dbConnection, parHandling, machineState, accessManager, false)
		{
		}

		// Token: 0x060020A9 RID: 8361 RVA: 0x000D51AC File Offset: 0x000D33AC
		public CtrlMoveSource(DBConnection dbConnection, ParameterHandling parHandling, MachineState machineState, AccessRightsManager accessManager, bool readonlyMode)
		{
			this.machineState = machineState;
			this.InitializeComponent();
			this.viewModel = new MoveSourceViewModel(dbConnection, parHandling, machineState, accessManager, readonlyMode);
			this.viewModel.PropertyChanged += this.viewModel_PropertyChanged;
			base.DataContext = this.viewModel;
			this.InitializeBar(this.viewModel.LengthMetric);
			bool flag = machineState != null;
			if (flag)
			{
				this.surveyingMachineState = true;
				Thread thread = new Thread(new ThreadStart(this.SurveyMachineState));
				thread.Start();
			}
		}

		// Token: 0x060020AA RID: 8362 RVA: 0x000D5244 File Offset: 0x000D3444
		private void InitializeBar(bool lengthMetric)
		{
			double steps = lengthMetric ? 1.0 : 0.04;
			int floatingPointPrecision = lengthMetric ? 0 : 2;
			int offset = lengthMetric ? 0 : 75;
			int numberSteps = lengthMetric ? 50 : 25;
			this.bar.FirstUsedValue = this.viewModel.MinHeight;
			this.bar.LastUsedValue = this.viewModel.MaxHeight;
			this.bar.ReinitializeScale(this.viewModel.MinHeight, this.viewModel.MaxHeight, floatingPointPrecision, steps, offset, numberSteps);
			this.bar.CurrentValue = this.viewModel.ProductHeight;
		}

		// Token: 0x060020AB RID: 8363 RVA: 0x000D52F0 File Offset: 0x000D34F0
		private void viewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			bool flag = e.PropertyName == "SourcePosition";
			if (flag)
			{
				this.bar.CurrentValue = this.viewModel.ProductHeight;
			}
			bool flag2 = e.PropertyName == "LengthMetric";
			if (flag2)
			{
				bool flag3 = this.bar != null;
				if (flag3)
				{
					this.InitializeBar(this.viewModel.LengthMetric);
				}
			}
		}

		// Token: 0x060020AC RID: 8364 RVA: 0x000D5360 File Offset: 0x000D3560
		private void SurveyMachineState()
		{
			bool flag = this.machineState != null;
			if (flag)
			{
				while (this.surveyingMachineState)
				{
					bool flag2 = this.machineState.State != MachineStates.MovingSource;
					if (flag2)
					{
						bool flag3 = Application.Current != null;
						if (flag3)
						{
							Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(delegate()
							{
								Subwindow subwindow = Window.GetWindow(this) as Subwindow;
								bool flag4 = subwindow != null;
								if (flag4)
								{
									Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("txtTurnXrayOn") + ".", DialogStyle.Info);
									subwindow.Close();
								}
							}));
						}
						this.surveyingMachineState = false;
					}
					Thread.Sleep(500);
				}
			}
		}

		// Token: 0x060020AD RID: 8365 RVA: 0x000D53E0 File Offset: 0x000D35E0
		private void CtrlMoveSource_Unloaded(object sender, RoutedEventArgs e)
		{
			this.surveyingMachineState = false;
			this.viewModel.PropertyChanged -= this.viewModel_PropertyChanged;
			this.viewModel.CleanUp();
		}

		// Token: 0x060020AE RID: 8366 RVA: 0x000D540E File Offset: 0x000D360E
		public void TriggerSourceMove(int heightInPixel)
		{
			this.viewModel.SourceMoveEnded += this.viewModel_SourceMoveEnded;
			this.viewModel.MoveSource(heightInPixel, false);
		}

		// Token: 0x060020AF RID: 8367 RVA: 0x000D5438 File Offset: 0x000D3638
		private void viewModel_SourceMoveEnded(bool success)
		{
			this.viewModel.SourceMoveEnded -= this.viewModel_SourceMoveEnded;
			Thread.Sleep(1000);
			bool flag = Application.Current != null;
			if (flag)
			{
				Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
				{
					Subwindow subwindow = Window.GetWindow(this) as Subwindow;
					bool flag2 = subwindow != null;
					if (flag2)
					{
						subwindow.Close();
					}
				}));
			}
		}

		// Token: 0x060020B0 RID: 8368 RVA: 0x000D5498 File Offset: 0x000D3698
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/sideshooter/ctrlmovesource.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x060020B1 RID: 8369 RVA: 0x000D54D0 File Offset: 0x000D36D0
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		internal Delegate _CreateDelegate(Type delegateType, string handler)
		{
			return Delegate.CreateDelegate(delegateType, this, handler);
		}

		// Token: 0x060020B2 RID: 8370 RVA: 0x000D54EC File Offset: 0x000D36EC
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			if (connectionId != 1)
			{
				if (connectionId != 2)
				{
					this._contentLoaded = true;
				}
				else
				{
					this.bar = (CtrlBar)target;
				}
			}
			else
			{
				((CtrlMoveSource)target).Unloaded += this.CtrlMoveSource_Unloaded;
			}
		}

		// Token: 0x04001533 RID: 5427
		private MoveSourceViewModel viewModel;

		// Token: 0x04001534 RID: 5428
		private MachineState machineState;

		// Token: 0x04001535 RID: 5429
		private bool surveyingMachineState;

		// Token: 0x04001536 RID: 5430
		internal CtrlBar bar;

		// Token: 0x04001537 RID: 5431
		private bool _contentLoaded;
	}
}
