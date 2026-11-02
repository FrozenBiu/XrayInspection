using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using DatabaseConnection;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x020000AE RID: 174
	public class CtrlAcquisitionSettings : UserControl, IComponentConnector
	{
		// Token: 0x060011BA RID: 4538 RVA: 0x0006B060 File Offset: 0x00069260
		public CtrlAcquisitionSettings(ParameterHandling parHandling, ImageDataTransfer imageTransfer, DBConnection dbConnection, ProductManager productManager, MachineState machineState, StoreImageData imageArchiver, AccessRightsManager accesmanager, TestcardSimService testcardsimservice)
		{
			this.InitializeComponent();
			this.testcardsimservice = testcardsimservice;
			this.productManager = productManager;
			this.viewModel = new CtrlAcquisitionSettingsViewModel(parHandling, imageTransfer, dbConnection, productManager, machineState, imageArchiver, this.imagebox, accesmanager, testcardsimservice);
			base.DataContext = this.viewModel;
			bool flag = testcardsimservice != null;
			if (flag)
			{
				testcardsimservice.MonitoringSensIsActive = false;
			}
		}

		// Token: 0x060011BB RID: 4539 RVA: 0x0006B0CC File Offset: 0x000692CC
		private void UserControl_Unloaded(object sender, RoutedEventArgs e)
		{
			this.viewModel.CurrentImage = null;
			BindingOperations.ClearAllBindings(this);
			this.btnHighSpeedImage.ClearValue(UIElement.IsEnabledProperty);
			this.btnBulkImage.ClearValue(UIElement.IsEnabledProperty);
			this.btnBulkImageObject.ClearValue(UIElement.IsEnabledProperty);
			base.DataContext = null;
			bool flag = this.testcardsimservice != null;
			if (flag)
			{
				this.testcardsimservice.MonitoringSensIsActive = true;
				this.testcardsimservice.CheckAndSetActivationTestcardSim(this.productManager.CurrentLoadedProduct);
			}
		}

		// Token: 0x060011BC RID: 4540 RVA: 0x0006B15C File Offset: 0x0006935C
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/acquisition/ctrlacquisitionsettings.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x060011BD RID: 4541 RVA: 0x0006B194 File Offset: 0x00069394
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				((CtrlAcquisitionSettings)target).Unloaded += this.UserControl_Unloaded;
				break;
			case 2:
				this.imagebox = (XRSImageBox)target;
				break;
			case 3:
				this.btnHighSpeedImage = (Button)target;
				break;
			case 4:
				this.btnBulkImage = (Button)target;
				break;
			case 5:
				this.btnBulkImageObject = (Button)target;
				break;
			case 6:
				this.grdNoElliptic = (Grid)target;
				break;
			default:
				this._contentLoaded = true;
				break;
			}
		}

		// Token: 0x04000BB9 RID: 3001
		private CtrlAcquisitionSettingsViewModel viewModel;

		// Token: 0x04000BBA RID: 3002
		private TestcardSimService testcardsimservice;

		// Token: 0x04000BBB RID: 3003
		private ProductManager productManager;

		// Token: 0x04000BBC RID: 3004
		internal XRSImageBox imagebox;

		// Token: 0x04000BBD RID: 3005
		internal Button btnHighSpeedImage;

		// Token: 0x04000BBE RID: 3006
		internal Button btnBulkImage;

		// Token: 0x04000BBF RID: 3007
		internal Button btnBulkImageObject;

		// Token: 0x04000BC0 RID: 3008
		internal Grid grdNoElliptic;

		// Token: 0x04000BC1 RID: 3009
		private bool _contentLoaded;
	}
}
