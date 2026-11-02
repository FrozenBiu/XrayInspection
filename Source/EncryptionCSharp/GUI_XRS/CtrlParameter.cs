using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using GUI_XRS.Datalogger;

namespace GUI_XRS
{
	// Token: 0x0200024A RID: 586
	public class CtrlParameter : UserControl, IComponentConnector, IStyleConnector
	{
		// Token: 0x17000C01 RID: 3073
		// (get) Token: 0x06002C68 RID: 11368 RVA: 0x001243FC File Offset: 0x001225FC
		// (set) Token: 0x06002C69 RID: 11369 RVA: 0x00124424 File Offset: 0x00122624
		public bool DockNumKeyboard
		{
			get
			{
				return this.viewModel != null && this.viewModel.DockNumKeyboard;
			}
			set
			{
				bool flag = this.viewModel != null;
				if (flag)
				{
					this.viewModel.DockNumKeyboard = value;
				}
			}
		}

		// Token: 0x140000FC RID: 252
		// (add) Token: 0x06002C6A RID: 11370 RVA: 0x0012444C File Offset: 0x0012264C
		// (remove) Token: 0x06002C6B RID: 11371 RVA: 0x00124484 File Offset: 0x00122684
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action ItemsCreated;

		// Token: 0x06002C6C RID: 11372 RVA: 0x001244B9 File Offset: 0x001226B9
		public CtrlParameter() : this(null, null, false, null, null)
		{
		}

		// Token: 0x06002C6D RID: 11373 RVA: 0x001244C8 File Offset: 0x001226C8
		public CtrlParameter(ParameterHandling parHandling, Logger logger, bool showProgress, List<string> paths, List<string> values) : this(parHandling, null, false, false, logger, showProgress, paths, values, null)
		{
		}

		// Token: 0x06002C6E RID: 11374 RVA: 0x001244E8 File Offset: 0x001226E8
		public CtrlParameter(ParameterHandling parHandling, string[] currentPaths, bool readOnly, bool showCheckbox, Logger logger, bool showProgress, List<string> paths, List<string> values, List<string> pathBeginningsToIgnore)
		{
			this.InitializeComponent();
			this.viewModel = new ParameterViewModel(parHandling, currentPaths, readOnly, showCheckbox, logger, showProgress, paths, values, pathBeginningsToIgnore);
			this.viewModel.ItemsCreated += this.viewModel_ItemsCreated;
			base.DataContext = this.viewModel;
			this.licensesButton.Visibility = Visibility.Collapsed;
		}

		// Token: 0x06002C6F RID: 11375 RVA: 0x00124550 File Offset: 0x00122750
		public CtrlParameter(ParameterHandling parHandling, string[] currentPaths, bool readOnly, bool showCheckbox, Logger logger, bool showProgress, List<string> paths, List<string> values, List<string> pathBeginningsToIgnore, bool licensesVisible)
		{
			this.InitializeComponent();
			this.viewModel = new ParameterViewModel(parHandling, currentPaths, readOnly, showCheckbox, logger, showProgress, paths, values, pathBeginningsToIgnore);
			this.viewModel.ItemsCreated += this.viewModel_ItemsCreated;
			base.DataContext = this.viewModel;
			if (licensesVisible)
			{
				this.licensesButton.Visibility = Visibility.Visible;
			}
			else
			{
				this.licensesButton.Visibility = Visibility.Collapsed;
			}
		}

		// Token: 0x06002C70 RID: 11376 RVA: 0x001245D0 File Offset: 0x001227D0
		public CtrlParameter(ParameterHandling parHandling, string[] currentPaths, bool readOnly, bool showCheckbox, Logger logger, bool showProgress, List<string> paths, List<string> values, List<string> pathBeginningsToIgnore, MachineState machineState, bool showEndProgress)
		{
			this.InitializeComponent();
			this.machineState = machineState;
			this.viewModel = new ParameterViewModel(parHandling, currentPaths, readOnly, showCheckbox, logger, showProgress, paths, values, pathBeginningsToIgnore);
			this.viewModel.ItemsCreated += this.viewModel_ItemsCreated;
			base.DataContext = this.viewModel;
			this.licensesButton.Visibility = Visibility.Collapsed;
			this.showEndProgress = showEndProgress;
		}

		// Token: 0x06002C71 RID: 11377 RVA: 0x00124648 File Offset: 0x00122848
		public void Initialize(ParameterHandling parHandling, bool showCheckbox, Logger logger, bool showProgress, List<string> defaultPaths, List<string> defaultValues)
		{
			this.viewModel.Initialize(parHandling, null, false, showCheckbox, logger, showProgress, defaultPaths, defaultValues, null);
		}

		// Token: 0x06002C72 RID: 11378 RVA: 0x00124670 File Offset: 0x00122870
		public void Initialize(ParameterHandling parHandling, string[] currentPaths, bool showCheckbox, Logger logger, bool showProgress, List<string> defaultPaths, List<string> defaultValues, bool readOnly)
		{
			this.viewModel.Initialize(parHandling, currentPaths, readOnly, showCheckbox, logger, showProgress, defaultPaths, defaultValues, null);
		}

		// Token: 0x06002C73 RID: 11379 RVA: 0x00124698 File Offset: 0x00122898
		private void TextBox_MouseUp(object sender, MouseButtonEventArgs e)
		{
			bool flag = e.Source is TextBox;
			if (flag)
			{
				bool flag2 = this.machineState != null;
				if (flag2)
				{
					ParameterItem parameterItem = (ParameterItem)((TextBox)e.Source).DataContext;
					bool flag3 = (parameterItem.Name.Equals("VoltageSetpoint") || parameterItem.Name.Equals("CurrentSetpoint")) && (this.machineState.State == MachineStates.Reseasoning || this.machineState.State == MachineStates.XrayOnCalibrationNotDone);
					if (flag3)
					{
						this.viewModel.ShowWarningCalibration();
					}
					else
					{
						this.viewModel.ShowEditKeyboard(parameterItem);
					}
				}
				else
				{
					ParameterItem item = (ParameterItem)((TextBox)e.Source).DataContext;
					this.viewModel.ShowEditKeyboard(item);
				}
			}
		}

		// Token: 0x06002C74 RID: 11380 RVA: 0x00124778 File Offset: 0x00122978
		private void TextBox_IsMouseCapturedChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			bool flag = (bool)e.OldValue && !(bool)e.NewValue;
			if (flag)
			{
				bool flag2 = sender is TextBox;
				if (flag2)
				{
					bool flag3 = this.machineState != null;
					if (flag3)
					{
						ParameterItem parameterItem = (ParameterItem)((TextBox)sender).DataContext;
						bool flag4 = (parameterItem.Name.Equals("VoltageSetpoint") || parameterItem.Name.Equals("CurrentSetpoint")) && (this.machineState.State == MachineStates.Reseasoning || this.machineState.State == MachineStates.XrayOnCalibrationNotDone);
						if (flag4)
						{
							this.viewModel.ShowWarningCalibration();
						}
						else
						{
							this.viewModel.ShowEditKeyboard(parameterItem);
						}
					}
					else
					{
						ParameterItem item = (ParameterItem)((TextBox)sender).DataContext;
						this.viewModel.ShowEditKeyboard(item);
					}
				}
			}
		}

		// Token: 0x06002C75 RID: 11381 RVA: 0x00124870 File Offset: 0x00122A70
		protected virtual void OnItemsCreated()
		{
			Action itemsCreated = this.ItemsCreated;
			bool flag = itemsCreated != null;
			if (flag)
			{
				itemsCreated();
			}
		}

		// Token: 0x06002C76 RID: 11382 RVA: 0x00124894 File Offset: 0x00122A94
		private void viewModel_ItemsCreated()
		{
			this.OnItemsCreated();
		}

		// Token: 0x06002C77 RID: 11383 RVA: 0x0012489E File Offset: 0x00122A9E
		public void StartCreation()
		{
			this.viewModel.StartCreation();
		}

		// Token: 0x06002C78 RID: 11384 RVA: 0x001248AD File Offset: 0x00122AAD
		public void SetLocalizedPseudoPath(List<InOut> mapping)
		{
			this.viewModel.SetLocalizedPseudoPath(mapping);
		}

		// Token: 0x06002C79 RID: 11385 RVA: 0x001248C0 File Offset: 0x00122AC0
		private void CtrlParameter_Unloaded(object sender, RoutedEventArgs e)
		{
			bool flag = this.showEndProgress;
			if (flag)
			{
				this.viewModel.ShowProgressBar();
			}
			this.viewModel.StopTimers();
			this.viewModel.CollapseAllItems();
			this.viewModel.ItemsCreated -= this.viewModel_ItemsCreated;
			this.viewModel.ParameterItems.Clear();
			this.viewModel.ParameterOperatingHours.Clear();
			this.viewModel.Cleanup();
			this.treePar.ItemsSource = null;
			this.treePar2.ItemsSource = null;
			bool flag2 = this.showEndProgress;
			if (flag2)
			{
				this.viewModel.StopProgressBar();
			}
			base.DataContext = null;
		}

		// Token: 0x06002C7A RID: 11386 RVA: 0x0012497E File Offset: 0x00122B7E
		private void treePar_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
		{
			this.scrollviewer.ScrollToVerticalOffset(this.scrollviewer.VerticalOffset - (double)e.Delta);
			e.Handled = true;
		}

		// Token: 0x06002C7B RID: 11387 RVA: 0x001249A8 File Offset: 0x00122BA8
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/parameter/ctrlparameter.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x06002C7C RID: 11388 RVA: 0x001249E0 File Offset: 0x00122BE0
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				((CtrlParameter)target).Unloaded += this.CtrlParameter_Unloaded;
				return;
			case 2:
				this.scrollviewer = (ScrollViewer)target;
				return;
			case 3:
				this.mainGrid = (Grid)target;
				return;
			case 4:
				this.treePar = (TreeView)target;
				this.treePar.PreviewMouseWheel += this.treePar_PreviewMouseWheel;
				return;
			case 6:
				this.treePar2 = (TreeView)target;
				return;
			case 7:
				this.licensesButton = (Border)target;
				return;
			}
			this._contentLoaded = true;
		}

		// Token: 0x06002C7D RID: 11389 RVA: 0x00124A98 File Offset: 0x00122C98
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IStyleConnector.Connect(int connectionId, object target)
		{
			if (connectionId == 5)
			{
				((TextBox)target).IsMouseCapturedChanged += this.TextBox_IsMouseCapturedChanged;
				((TextBox)target).MouseUp += this.TextBox_MouseUp;
			}
		}

		// Token: 0x04001BCC RID: 7116
		private ParameterViewModel viewModel;

		// Token: 0x04001BCE RID: 7118
		private MachineState machineState;

		// Token: 0x04001BCF RID: 7119
		private bool showEndProgress;

		// Token: 0x04001BD0 RID: 7120
		internal ScrollViewer scrollviewer;

		// Token: 0x04001BD1 RID: 7121
		internal Grid mainGrid;

		// Token: 0x04001BD2 RID: 7122
		internal TreeView treePar;

		// Token: 0x04001BD3 RID: 7123
		internal TreeView treePar2;

		// Token: 0x04001BD4 RID: 7124
		internal Border licensesButton;

		// Token: 0x04001BD5 RID: 7125
		private bool _contentLoaded;
	}
}
