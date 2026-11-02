using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using DatabaseConnection;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x0200002D RID: 45
	public class CtrlAutotrainAdjustments : UserControl, IComponentConnector
	{
		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x060005CC RID: 1484 RVA: 0x00027882 File Offset: 0x00025A82
		// (set) Token: 0x060005CD RID: 1485 RVA: 0x0002788A File Offset: 0x00025A8A
		public List<Button> Buttons { get; set; }

		// Token: 0x060005CE RID: 1486 RVA: 0x00027894 File Offset: 0x00025A94
		public CtrlAutotrainAdjustments(ParameterHandling parHandling, ImageDataTransfer imageTransfer, DBConnection dbConnection, AccessRightsManager accessManager, MachineState machineState, ProductManager productManager, int imageGroup, StoreImageData imageArchiver, TestcardSimService testcardSimService, AuditCheckSettings auditSettings)
		{
			this.InitializeComponent();
			this.accessManager = accessManager;
			this.parHandling = parHandling;
			this.dbConnection = dbConnection;
			this.viewModel = new Filter(this.parHandling, imageTransfer, this.dbConnection, accessManager, machineState, productManager, imageGroup, imageArchiver, true, testcardSimService, auditSettings);
			base.DataContext = this.viewModel;
		}

		// Token: 0x060005CF RID: 1487 RVA: 0x000278FA File Offset: 0x00025AFA
		private void CtrlAutotrainAdjustments_Unloaded(object sender, RoutedEventArgs e)
		{
			this.viewModel.DeregisterLocalizedText();
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x0002790C File Offset: 0x00025B0C
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/autotrain/ctrlautotrainadjustments.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x00027944 File Offset: 0x00025B44
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
					this.mainGrid = (Grid)target;
				}
			}
			else
			{
				((CtrlAutotrainAdjustments)target).Unloaded += this.CtrlAutotrainAdjustments_Unloaded;
			}
		}

		// Token: 0x04000435 RID: 1077
		private ParameterHandling parHandling;

		// Token: 0x04000436 RID: 1078
		private AccessRightsManager accessManager;

		// Token: 0x04000437 RID: 1079
		private DBConnection dbConnection;

		// Token: 0x04000439 RID: 1081
		private Filter viewModel;

		// Token: 0x0400043A RID: 1082
		internal Grid mainGrid;

		// Token: 0x0400043B RID: 1083
		private bool _contentLoaded;
	}
}
