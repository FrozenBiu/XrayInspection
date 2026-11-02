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

namespace GUI_XRS
{
	// Token: 0x0200006C RID: 108
	public class CtrlIVPSettings : UserControl, IComponentConnector
	{
		// Token: 0x060009C4 RID: 2500 RVA: 0x0003BFEB File Offset: 0x0003A1EB
		public CtrlIVPSettings(DBConnection dbConnection, ParameterHandling parHandling, ProductManager productManager)
		{
			this.InitializeComponent();
			this.viewModel = new IVPSettingsViewModel(dbConnection, parHandling, productManager);
			base.DataContext = this.viewModel;
			this.InitializeLabel();
		}

		// Token: 0x060009C5 RID: 2501 RVA: 0x0003C020 File Offset: 0x0003A220
		private void TextBox_MouseUp(object sender, MouseButtonEventArgs e)
		{
			bool flag = e.Source is TextBox;
			if (flag)
			{
				this.viewModel.getNumKeyboard();
			}
		}

		// Token: 0x060009C6 RID: 2502 RVA: 0x0003C050 File Offset: 0x0003A250
		private void TextBox_MouseUp_1(object sender, MouseButtonEventArgs e)
		{
			bool flag = e.Source is TextBox;
			if (flag)
			{
				this.viewModel.getNumKeyboardNumber();
			}
		}

		// Token: 0x060009C7 RID: 2503 RVA: 0x0003AF21 File Offset: 0x00039121
		private void IVP_Unloaded(object sender, RoutedEventArgs e)
		{
			BindingOperations.ClearAllBindings(this);
			base.DataContext = null;
			GC.Collect();
		}

		// Token: 0x060009C8 RID: 2504 RVA: 0x0003C080 File Offset: 0x0003A280
		private void InitializeLabel()
		{
			this.UiElementRightAligned = new Label
			{
				Content = this.viewModel.StrSettingsMenu.LocalizedText,
				FontSize = 30.0,
				FontWeight = FontWeights.Bold,
				FontFamily = new FontFamily("Verdana"),
				Foreground = Brushes.DarkGray,
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Right,
				Margin = new Thickness(0.0, 0.0, 20.0, 0.0)
			};
		}

		// Token: 0x060009C9 RID: 2505 RVA: 0x0003C12B File Offset: 0x0003A32B
		private void IVP_Loaded(object sender, RoutedEventArgs e)
		{
			this.viewModel.SetStartTime();
		}

		// Token: 0x060009CA RID: 2506 RVA: 0x0003C13C File Offset: 0x0003A33C
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/integrated%20validation%20process/ctrlivpsettings.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x060009CB RID: 2507 RVA: 0x0003C174 File Offset: 0x0003A374
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		internal Delegate _CreateDelegate(Type delegateType, string handler)
		{
			return Delegate.CreateDelegate(delegateType, this, handler);
		}

		// Token: 0x060009CC RID: 2508 RVA: 0x0003C190 File Offset: 0x0003A390
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				((CtrlIVPSettings)target).Unloaded += this.IVP_Unloaded;
				((CtrlIVPSettings)target).Loaded += this.IVP_Loaded;
				break;
			case 2:
				((TextBox)target).MouseUp += this.TextBox_MouseUp_1;
				break;
			case 3:
				this.btnLowerSpeed = (Button)target;
				break;
			case 4:
				this.txtSpeed = (TextBox)target;
				break;
			case 5:
				this.btnHigherSpeed = (Button)target;
				break;
			case 6:
				((TextBox)target).MouseUp += this.TextBox_MouseUp;
				break;
			default:
				this._contentLoaded = true;
				break;
			}
		}

		// Token: 0x040006AE RID: 1710
		private IVPSettingsViewModel viewModel;

		// Token: 0x040006AF RID: 1711
		public UIElement UiElementRightAligned;

		// Token: 0x040006B0 RID: 1712
		internal Button btnLowerSpeed;

		// Token: 0x040006B1 RID: 1713
		internal TextBox txtSpeed;

		// Token: 0x040006B2 RID: 1714
		internal Button btnHigherSpeed;

		// Token: 0x040006B3 RID: 1715
		private bool _contentLoaded;
	}
}
