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
using GUI_XRS.Integrated_Validation_Process;

namespace GUI_XRS
{
	// Token: 0x02000068 RID: 104
	public class CtrlIVPCertificatesManufacturer : UserControl, IComponentConnector, IStyleConnector
	{
		// Token: 0x06000993 RID: 2451 RVA: 0x0003AE6E File Offset: 0x0003906E
		public CtrlIVPCertificatesManufacturer(DBConnection dBConnection)
		{
			this.InitializeComponent();
			this.viewModel = new IVPCertificatesManufacturerViewModel(dBConnection);
			base.DataContext = this.viewModel;
			this.InitializeLabel();
		}

		// Token: 0x06000994 RID: 2452 RVA: 0x0003AEA0 File Offset: 0x000390A0
		private void TextBox_MouseUp(object sender, MouseButtonEventArgs e)
		{
			bool flag = e.Source is TextBox;
			if (flag)
			{
				Infos infos = (Infos)((TextBox)e.Source).DataContext;
				bool flag2 = infos.Description.Equals(this.viewModel.StrEmail.LocalizedText + ":");
				if (flag2)
				{
					this.viewModel.ShowEditKeyboardEmail(infos);
				}
				else
				{
					this.viewModel.ShowEditKeyboard(infos);
				}
			}
		}

		// Token: 0x06000995 RID: 2453 RVA: 0x0003AF21 File Offset: 0x00039121
		private void IVP_Unloaded(object sender, RoutedEventArgs e)
		{
			BindingOperations.ClearAllBindings(this);
			base.DataContext = null;
			GC.Collect();
		}

		// Token: 0x06000996 RID: 2454 RVA: 0x0003AF3C File Offset: 0x0003913C
		private void InitializeLabel()
		{
			this.UiElementRightAligned = new Label
			{
				Content = "by Sesotec",
				FontSize = 30.0,
				FontWeight = FontWeights.Bold,
				FontFamily = new FontFamily("Verdana"),
				Foreground = Brushes.DarkGray,
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Right,
				Margin = new Thickness(0.0, 0.0, 20.0, 0.0)
			};
		}

		// Token: 0x06000997 RID: 2455 RVA: 0x0003AFDC File Offset: 0x000391DC
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/integrated%20validation%20process/ctrlivpcertificatesmanufacturer.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x06000998 RID: 2456 RVA: 0x0003B014 File Offset: 0x00039214
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
				((CtrlIVPCertificatesManufacturer)target).Unloaded += this.IVP_Unloaded;
			}
		}

		// Token: 0x06000999 RID: 2457 RVA: 0x0003B050 File Offset: 0x00039250
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IStyleConnector.Connect(int connectionId, object target)
		{
			if (connectionId == 2)
			{
				((TextBox)target).MouseUp += this.TextBox_MouseUp;
			}
		}

		// Token: 0x04000691 RID: 1681
		private IVPCertificatesManufacturerViewModel viewModel;

		// Token: 0x04000692 RID: 1682
		public UIElement UiElementRightAligned;

		// Token: 0x04000693 RID: 1683
		private bool _contentLoaded;
	}
}
