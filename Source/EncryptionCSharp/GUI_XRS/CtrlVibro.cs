using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x02000057 RID: 87
	public class CtrlVibro : UserControl, IComponentConnector
	{
		// Token: 0x060008BC RID: 2236 RVA: 0x00037700 File Offset: 0x00035900
		public CtrlVibro(Vibro vibro, AccessRightsManager accessManager, ParameterHandling parHandling)
		{
			this.InitializeComponent();
			this.accessManager = accessManager;
			this.viewModel = new VibroViewModel(vibro, accessManager, parHandling);
			accessManager.RegisterControl(this.btnHigherSpeed, this.rightHigherSpeed, this.rightHigherSpeed);
			accessManager.RegisterControl(this.btnLowerSpeed, this.rightLowerSpeed, this.rightLowerSpeed);
			accessManager.SetProperties();
			base.DataContext = this.viewModel;
		}

		// Token: 0x060008BD RID: 2237 RVA: 0x0003778D File Offset: 0x0003598D
		private void CtrlVibro_Unloaded(object sender, RoutedEventArgs e)
		{
			this.viewModel.CleanUp();
			this.accessManager.DeregisterControl(this.btnLowerSpeed);
			this.accessManager.DeregisterControl(this.btnHigherSpeed);
		}

		// Token: 0x060008BE RID: 2238 RVA: 0x000377C0 File Offset: 0x000359C0
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/bulk/vibro/ctrlvibro.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x060008BF RID: 2239 RVA: 0x000377F8 File Offset: 0x000359F8
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				((CtrlVibro)target).Unloaded += this.CtrlVibro_Unloaded;
				break;
			case 2:
				this.btnLowerSpeed = (Button)target;
				break;
			case 3:
				this.txtSpeed = (TextBox)target;
				break;
			case 4:
				this.btnHigherSpeed = (Button)target;
				break;
			default:
				this._contentLoaded = true;
				break;
			}
		}

		// Token: 0x040005F2 RID: 1522
		private BuiltInPriviledges rightLowerSpeed = BuiltInPriviledges.Operator1;

		// Token: 0x040005F3 RID: 1523
		private BuiltInPriviledges rightHigherSpeed = BuiltInPriviledges.Operator1;

		// Token: 0x040005F4 RID: 1524
		private VibroViewModel viewModel;

		// Token: 0x040005F5 RID: 1525
		private AccessRightsManager accessManager;

		// Token: 0x040005F6 RID: 1526
		internal Button btnLowerSpeed;

		// Token: 0x040005F7 RID: 1527
		internal TextBox txtSpeed;

		// Token: 0x040005F8 RID: 1528
		internal Button btnHigherSpeed;

		// Token: 0x040005F9 RID: 1529
		private bool _contentLoaded;
	}
}
