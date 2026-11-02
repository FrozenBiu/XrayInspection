using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using DatabaseConnection;
using Definitions;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x0200002C RID: 44
	public class CtrlAuditCheckTestsamples : UserControl, IComponentConnector
	{
		// Token: 0x060005BA RID: 1466 RVA: 0x00027264 File Offset: 0x00025464
		public CtrlAuditCheckTestsamples(AccessRightsManager accessManager, DBConnection dbConnection)
		{
			this.InitializeComponent();
			this.accessManager = accessManager;
			this.dbConnection = dbConnection;
			dbConnection.GetIntegratedValidationProcess(ref this.ivpActivated);
			this.viewModel = new AuditCheckTestsamplesViewModel(accessManager, dbConnection);
			base.DataContext = this.viewModel;
			this.InitializeButtons();
			this.SetControlRights();
		}

		// Token: 0x060005BB RID: 1467 RVA: 0x000272D8 File Offset: 0x000254D8
		private void SetControlRights()
		{
			bool flag = this.ivpActivated;
			if (flag)
			{
				this.accessManager.RegisterControl(this.importTestsamplesBtn, this.rightImport, this.rightImport);
				this.accessManager.SetProperties();
			}
		}

		// Token: 0x060005BC RID: 1468 RVA: 0x0002731C File Offset: 0x0002551C
		private void InitializeButtons()
		{
			bool flag = this.ivpActivated;
			if (flag)
			{
				this.dockpanel = new DockPanel();
				this.dockpanel.HorizontalAlignment = HorizontalAlignment.Stretch;
				this.dockpanel.VerticalAlignment = VerticalAlignment.Stretch;
				Grid.SetColumn(this.dockpanel, 4);
				this.importTestsamplesBtn = new Button();
				this.importTestsamplesBtn.Content = ImageConverter.ToImage("/img/importTestballsIVP.png");
				this.importTestsamplesBtn.Click += this.ImportSettings;
				this.importTestsamplesBtn.Style = (base.FindResource("defaultButtonStyle") as Style);
				this.importTestsamplesBtn.Margin = new Thickness(0.0, 0.0, 10.0, 0.0);
				this.importTestsamplesBtn.Visibility = Visibility.Visible;
				this.dockpanel.Children.Add(this.importTestsamplesBtn);
				DockPanel.SetDock(this.importTestsamplesBtn, Dock.Left);
			}
		}

		// Token: 0x060005BD RID: 1469 RVA: 0x00027428 File Offset: 0x00025628
		private void TextBox_MouseUp1(object sender, MouseButtonEventArgs e)
		{
			bool flag = e.Source is TextBox;
			if (flag)
			{
				this.viewModel.ShowEditKeyboard(1);
			}
		}

		// Token: 0x060005BE RID: 1470 RVA: 0x00027458 File Offset: 0x00025658
		private void TextBox_MouseUp2(object sender, MouseButtonEventArgs e)
		{
			bool flag = e.Source is TextBox;
			if (flag)
			{
				this.viewModel.ShowEditKeyboard(2);
			}
		}

		// Token: 0x060005BF RID: 1471 RVA: 0x00027488 File Offset: 0x00025688
		private void TextBox_MouseUp3(object sender, MouseButtonEventArgs e)
		{
			bool flag = e.Source is TextBox;
			if (flag)
			{
				this.viewModel.ShowEditKeyboard(3);
			}
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x000274B8 File Offset: 0x000256B8
		private void TextBox_MouseUp4(object sender, MouseButtonEventArgs e)
		{
			bool flag = e.Source is TextBox;
			if (flag)
			{
				this.viewModel.ShowEditKeyboard(4);
			}
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x000274E8 File Offset: 0x000256E8
		private void TextBox_MouseUp5(object sender, MouseButtonEventArgs e)
		{
			bool flag = e.Source is TextBox;
			if (flag)
			{
				this.viewModel.ShowEditKeyboard(5);
			}
		}

		// Token: 0x060005C2 RID: 1474 RVA: 0x00027518 File Offset: 0x00025718
		private void MouseUp_INNumber1(object sender, MouseButtonEventArgs e)
		{
			bool flag = e.Source is TextBox;
			if (flag)
			{
				this.viewModel.ShowEditKeyboardINNumber(1);
			}
		}

		// Token: 0x060005C3 RID: 1475 RVA: 0x00027548 File Offset: 0x00025748
		private void MouseUp_INNumber2(object sender, MouseButtonEventArgs e)
		{
			bool flag = e.Source is TextBox;
			if (flag)
			{
				this.viewModel.ShowEditKeyboardINNumber(2);
			}
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x00027578 File Offset: 0x00025778
		private void MouseUp_INNumber3(object sender, MouseButtonEventArgs e)
		{
			bool flag = e.Source is TextBox;
			if (flag)
			{
				this.viewModel.ShowEditKeyboardINNumber(3);
			}
		}

		// Token: 0x060005C5 RID: 1477 RVA: 0x000275A8 File Offset: 0x000257A8
		private void MouseUp_INNumber4(object sender, MouseButtonEventArgs e)
		{
			bool flag = e.Source is TextBox;
			if (flag)
			{
				this.viewModel.ShowEditKeyboardINNumber(4);
			}
		}

		// Token: 0x060005C6 RID: 1478 RVA: 0x000275D8 File Offset: 0x000257D8
		private void MouseUp_INNumber5(object sender, MouseButtonEventArgs e)
		{
			bool flag = e.Source is TextBox;
			if (flag)
			{
				this.viewModel.ShowEditKeyboardINNumber(5);
			}
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x00027608 File Offset: 0x00025808
		private void AuditCheck_Unloaded(object sender, RoutedEventArgs e)
		{
			bool flag = this.ivpActivated;
			if (flag)
			{
				this.accessManager.DeregisterControl(this.importTestsamplesBtn);
			}
			BindingOperations.ClearAllBindings(this);
			base.DataContext = null;
			GC.Collect();
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x0002764C File Offset: 0x0002584C
		private void AuditCheck_Loaded(object sender, RoutedEventArgs e)
		{
			Subwindow subwindow = Window.GetWindow(this) as Subwindow;
			bool flag = this.ivpActivated;
			if (flag)
			{
				subwindow.buttonGrid.Children.Remove(subwindow.buttonPanel);
				subwindow.buttonGrid.Children.Add(this.dockpanel);
			}
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x000276A0 File Offset: 0x000258A0
		private void ImportSettings(object sender, RoutedEventArgs e)
		{
			this.viewModel.ImportIVPSettings();
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x000276B0 File Offset: 0x000258B0
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/auditchecknew/ctrlauditchecktestsamples.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x000276E8 File Offset: 0x000258E8
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				((CtrlAuditCheckTestsamples)target).Unloaded += this.AuditCheck_Unloaded;
				((CtrlAuditCheckTestsamples)target).Loaded += this.AuditCheck_Loaded;
				break;
			case 2:
				((TextBox)target).MouseUp += this.TextBox_MouseUp1;
				break;
			case 3:
				((TextBox)target).MouseUp += this.TextBox_MouseUp2;
				break;
			case 4:
				((TextBox)target).MouseUp += this.TextBox_MouseUp3;
				break;
			case 5:
				((TextBox)target).MouseUp += this.TextBox_MouseUp4;
				break;
			case 6:
				((TextBox)target).MouseUp += this.TextBox_MouseUp5;
				break;
			case 7:
				((TextBox)target).MouseUp += this.MouseUp_INNumber1;
				break;
			case 8:
				((TextBox)target).MouseUp += this.MouseUp_INNumber2;
				break;
			case 9:
				((TextBox)target).MouseUp += this.MouseUp_INNumber3;
				break;
			case 10:
				((TextBox)target).MouseUp += this.MouseUp_INNumber4;
				break;
			case 11:
				((TextBox)target).MouseUp += this.MouseUp_INNumber5;
				break;
			default:
				this._contentLoaded = true;
				break;
			}
		}

		// Token: 0x0400042D RID: 1069
		private AuditCheckTestsamplesViewModel viewModel;

		// Token: 0x0400042E RID: 1070
		private DockPanel dockpanel;

		// Token: 0x0400042F RID: 1071
		private Button importTestsamplesBtn;

		// Token: 0x04000430 RID: 1072
		private AccessRightsManager accessManager;

		// Token: 0x04000431 RID: 1073
		private BuiltInPriviledges rightImport = BuiltInPriviledges.Admin;

		// Token: 0x04000432 RID: 1074
		private DBConnection dbConnection;

		// Token: 0x04000433 RID: 1075
		private bool ivpActivated = false;

		// Token: 0x04000434 RID: 1076
		private bool _contentLoaded;
	}
}
