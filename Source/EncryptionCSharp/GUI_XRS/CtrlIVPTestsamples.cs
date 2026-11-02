using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using DatabaseConnection;
using Definitions;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x0200006B RID: 107
	public class CtrlIVPTestsamples : UserControl, IComponentConnector
	{
		// Token: 0x060009AC RID: 2476 RVA: 0x0003B6FC File Offset: 0x000398FC
		public CtrlIVPTestsamples(AccessRightsManager accessManager, DBConnection dbConnection)
		{
			this.accessManager = accessManager;
			this.InitializeComponent();
			this.viewModel = new IVPTestsamplesViewModel(accessManager, dbConnection);
			base.DataContext = this.viewModel;
			int num = 0;
			dbConnection.GetAuditCheckType(ref num);
			bool flag = num == 1;
			if (flag)
			{
				string text = "";
				bool flag2 = false;
				dbConnection.GetParameterValue(this.auditContaminationEnabledPath, ref text);
				bool flag3 = text.Equals("1");
				if (flag3)
				{
					flag2 = true;
				}
				else
				{
					dbConnection.GetParameterValue(this.auditCountEnabledPath, ref text);
					bool flag4 = text.Equals("1");
					if (flag4)
					{
						flag2 = true;
					}
					else
					{
						dbConnection.GetParameterValue(this.auditWeightEnabledPath, ref text);
						bool flag5 = text.Equals("1");
						if (flag5)
						{
							flag2 = true;
						}
					}
				}
				bool flag6 = flag2;
				if (flag6)
				{
					this.InitializeButtons();
					this.SetControlRights();
					this.newAuditCheck = true;
				}
				this.InitializeLabel();
			}
		}

		// Token: 0x060009AD RID: 2477 RVA: 0x0003B825 File Offset: 0x00039A25
		private void SetControlRights()
		{
			this.accessManager.RegisterControl(this.importTestsamplesBtn, this.rightImport, this.rightImport);
			this.accessManager.SetProperties();
		}

		// Token: 0x060009AE RID: 2478 RVA: 0x0003B854 File Offset: 0x00039A54
		private void InitializeLabel()
		{
			this.UiElementRightAligned = new Label
			{
				Content = this.viewModel.StrTestContaminations.LocalizedText,
				FontSize = 30.0,
				FontWeight = FontWeights.Bold,
				FontFamily = new FontFamily("Verdana"),
				Foreground = Brushes.DarkGray,
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Right,
				Margin = new Thickness(0.0, 0.0, 20.0, 0.0)
			};
		}

		// Token: 0x060009AF RID: 2479 RVA: 0x0003B900 File Offset: 0x00039B00
		private void InitializeButtons()
		{
			this.dockpanel = new DockPanel();
			this.dockpanel.HorizontalAlignment = HorizontalAlignment.Stretch;
			this.dockpanel.VerticalAlignment = VerticalAlignment.Stretch;
			Grid.SetColumn(this.dockpanel, 4);
			this.importTestsamplesBtn = new Button();
			this.importTestsamplesBtn.Content = ImageConverter.ToImage("/img/importTestballsAuditcheck.png");
			this.importTestsamplesBtn.Click += this.ImportSettings;
			this.importTestsamplesBtn.Style = (base.FindResource("defaultButtonStyle") as Style);
			this.importTestsamplesBtn.Margin = new Thickness(0.0, 0.0, 10.0, 0.0);
			this.importTestsamplesBtn.Visibility = Visibility.Visible;
			this.dockpanel.Children.Add(this.importTestsamplesBtn);
			DockPanel.SetDock(this.importTestsamplesBtn, Dock.Left);
		}

		// Token: 0x060009B0 RID: 2480 RVA: 0x0003B9FC File Offset: 0x00039BFC
		private void TextBox_MouseUp1(object sender, MouseButtonEventArgs e)
		{
			bool flag = e.Source is TextBox;
			if (flag)
			{
				this.viewModel.ShowEditKeyboard(1);
			}
		}

		// Token: 0x060009B1 RID: 2481 RVA: 0x0003BA2C File Offset: 0x00039C2C
		private void TextBox_MouseUp2(object sender, MouseButtonEventArgs e)
		{
			bool flag = e.Source is TextBox;
			if (flag)
			{
				this.viewModel.ShowEditKeyboard(2);
			}
		}

		// Token: 0x060009B2 RID: 2482 RVA: 0x0003BA5C File Offset: 0x00039C5C
		private void TextBox_MouseUp3(object sender, MouseButtonEventArgs e)
		{
			bool flag = e.Source is TextBox;
			if (flag)
			{
				this.viewModel.ShowEditKeyboard(3);
			}
		}

		// Token: 0x060009B3 RID: 2483 RVA: 0x0003BA8C File Offset: 0x00039C8C
		private void TextBox_MouseUp4(object sender, MouseButtonEventArgs e)
		{
			bool flag = e.Source is TextBox;
			if (flag)
			{
				this.viewModel.ShowEditKeyboard(4);
			}
		}

		// Token: 0x060009B4 RID: 2484 RVA: 0x0003BABC File Offset: 0x00039CBC
		private void TextBox_MouseUp5(object sender, MouseButtonEventArgs e)
		{
			bool flag = e.Source is TextBox;
			if (flag)
			{
				this.viewModel.ShowEditKeyboard(5);
			}
		}

		// Token: 0x060009B5 RID: 2485 RVA: 0x0003BAEC File Offset: 0x00039CEC
		private void MouseUp_INNumber1(object sender, MouseButtonEventArgs e)
		{
			bool flag = e.Source is TextBox;
			if (flag)
			{
				this.viewModel.ShowEditKeyboardINNumber(1);
			}
		}

		// Token: 0x060009B6 RID: 2486 RVA: 0x0003BB1C File Offset: 0x00039D1C
		private void MouseUp_INNumber2(object sender, MouseButtonEventArgs e)
		{
			bool flag = e.Source is TextBox;
			if (flag)
			{
				this.viewModel.ShowEditKeyboardINNumber(2);
			}
		}

		// Token: 0x060009B7 RID: 2487 RVA: 0x0003BB4C File Offset: 0x00039D4C
		private void MouseUp_INNumber3(object sender, MouseButtonEventArgs e)
		{
			bool flag = e.Source is TextBox;
			if (flag)
			{
				this.viewModel.ShowEditKeyboardINNumber(3);
			}
		}

		// Token: 0x060009B8 RID: 2488 RVA: 0x0003BB7C File Offset: 0x00039D7C
		private void MouseUp_INNumber4(object sender, MouseButtonEventArgs e)
		{
			bool flag = e.Source is TextBox;
			if (flag)
			{
				this.viewModel.ShowEditKeyboardINNumber(4);
			}
		}

		// Token: 0x060009B9 RID: 2489 RVA: 0x0003BBAC File Offset: 0x00039DAC
		private void MouseUp_INNumber5(object sender, MouseButtonEventArgs e)
		{
			bool flag = e.Source is TextBox;
			if (flag)
			{
				this.viewModel.ShowEditKeyboardINNumber(5);
			}
		}

		// Token: 0x060009BA RID: 2490 RVA: 0x0003BBDC File Offset: 0x00039DDC
		private void MouseUp_Density1(object sender, MouseButtonEventArgs e)
		{
			bool flag = e.Source is TextBox;
			if (flag)
			{
				this.viewModel.ShowEditKeyboardDensity(1);
			}
		}

		// Token: 0x060009BB RID: 2491 RVA: 0x0003BC0C File Offset: 0x00039E0C
		private void MouseUp_Density2(object sender, MouseButtonEventArgs e)
		{
			bool flag = e.Source is TextBox;
			if (flag)
			{
				this.viewModel.ShowEditKeyboardDensity(2);
			}
		}

		// Token: 0x060009BC RID: 2492 RVA: 0x0003BC3C File Offset: 0x00039E3C
		private void MouseUp_Density3(object sender, MouseButtonEventArgs e)
		{
			bool flag = e.Source is TextBox;
			if (flag)
			{
				this.viewModel.ShowEditKeyboardDensity(3);
			}
		}

		// Token: 0x060009BD RID: 2493 RVA: 0x0003BC6C File Offset: 0x00039E6C
		private void MouseUp_Density4(object sender, MouseButtonEventArgs e)
		{
			bool flag = e.Source is TextBox;
			if (flag)
			{
				this.viewModel.ShowEditKeyboardDensity(4);
			}
		}

		// Token: 0x060009BE RID: 2494 RVA: 0x0003BC9C File Offset: 0x00039E9C
		private void MouseUp_Density5(object sender, MouseButtonEventArgs e)
		{
			bool flag = e.Source is TextBox;
			if (flag)
			{
				this.viewModel.ShowEditKeyboardDensity(5);
			}
		}

		// Token: 0x060009BF RID: 2495 RVA: 0x0003BCCC File Offset: 0x00039ECC
		private void IVP_Unloaded(object sender, RoutedEventArgs e)
		{
			bool flag = this.newAuditCheck;
			if (flag)
			{
				this.accessManager.DeregisterControl(this.importTestsamplesBtn);
			}
			BindingOperations.ClearAllBindings(this);
			base.DataContext = null;
			GC.Collect();
		}

		// Token: 0x060009C0 RID: 2496 RVA: 0x0003BD10 File Offset: 0x00039F10
		private void IVP_Loaded(object sender, RoutedEventArgs e)
		{
			bool flag = this.newAuditCheck;
			if (flag)
			{
				Subwindow subwindow = Window.GetWindow(this) as Subwindow;
				subwindow.buttonGrid.Children.Remove(subwindow.buttonPanel);
				subwindow.buttonGrid.Children.Add(this.dockpanel);
			}
		}

		// Token: 0x060009C1 RID: 2497 RVA: 0x0003BD64 File Offset: 0x00039F64
		private void ImportSettings(object sender, RoutedEventArgs e)
		{
			this.viewModel.ImportAuditCheckSettings();
		}

		// Token: 0x060009C2 RID: 2498 RVA: 0x0003BD74 File Offset: 0x00039F74
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/integrated%20validation%20process/ctrlivptestsamples.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x060009C3 RID: 2499 RVA: 0x0003BDAC File Offset: 0x00039FAC
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				((CtrlIVPTestsamples)target).Unloaded += this.IVP_Unloaded;
				((CtrlIVPTestsamples)target).Loaded += this.IVP_Loaded;
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
			case 12:
				((TextBox)target).MouseUp += this.MouseUp_Density1;
				break;
			case 13:
				((TextBox)target).MouseUp += this.MouseUp_Density2;
				break;
			case 14:
				((TextBox)target).MouseUp += this.MouseUp_Density3;
				break;
			case 15:
				((TextBox)target).MouseUp += this.MouseUp_Density4;
				break;
			case 16:
				((TextBox)target).MouseUp += this.MouseUp_Density5;
				break;
			default:
				this._contentLoaded = true;
				break;
			}
		}

		// Token: 0x040006A3 RID: 1699
		private IVPTestsamplesViewModel viewModel;

		// Token: 0x040006A4 RID: 1700
		private DockPanel dockpanel;

		// Token: 0x040006A5 RID: 1701
		private Button importTestsamplesBtn;

		// Token: 0x040006A6 RID: 1702
		private BuiltInPriviledges rightImport = BuiltInPriviledges.Admin;

		// Token: 0x040006A7 RID: 1703
		private AccessRightsManager accessManager;

		// Token: 0x040006A8 RID: 1704
		private bool newAuditCheck = false;

		// Token: 0x040006A9 RID: 1705
		private readonly string auditContaminationEnabledPath = "AuditContaminationEnabled";

		// Token: 0x040006AA RID: 1706
		private readonly string auditWeightEnabledPath = "AuditWeightEnabled";

		// Token: 0x040006AB RID: 1707
		private readonly string auditCountEnabledPath = "AuditCountEnabled";

		// Token: 0x040006AC RID: 1708
		public UIElement UiElementRightAligned;

		// Token: 0x040006AD RID: 1709
		private bool _contentLoaded;
	}
}
