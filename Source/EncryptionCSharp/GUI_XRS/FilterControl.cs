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
	// Token: 0x0200022F RID: 559
	public class FilterControl : UserControl, IComponentConnector
	{
		// Token: 0x17000BA9 RID: 2985
		// (get) Token: 0x06002AA5 RID: 10917 RVA: 0x0011BEBD File Offset: 0x0011A0BD
		// (set) Token: 0x06002AA6 RID: 10918 RVA: 0x0011BEC5 File Offset: 0x0011A0C5
		public List<Button> Buttons { get; set; }

		// Token: 0x06002AA7 RID: 10919 RVA: 0x0011BED0 File Offset: 0x0011A0D0
		public FilterControl(ParameterHandling parHandling, ImageDataTransfer imageTransfer, DBConnection dbConnection, AccessRightsManager accessManager, MachineState machineState, ProductManager productManager, int imageGroup, StoreImageData imageArchiver, TestcardSimService testcardsimservice, AuditCheckSettings auditSettings)
		{
			this.InitializeComponent();
			this.InitializeButtons();
			this.accessManager = accessManager;
			this.parHandling = parHandling;
			this.dbConnection = dbConnection;
			this.testcardsimservice = testcardsimservice;
			this.filter = new Filter(this.parHandling, imageTransfer, this.dbConnection, accessManager, machineState, productManager, imageGroup, imageArchiver, true, testcardsimservice, auditSettings);
			base.DataContext = this.filter;
			bool flag = testcardsimservice != null;
			if (flag)
			{
				testcardsimservice.MonitoringSensIsActive = false;
			}
		}

		// Token: 0x06002AA8 RID: 10920 RVA: 0x0001092C File Offset: 0x0000EB2C
		private void InitializeButtons()
		{
		}

		// Token: 0x06002AA9 RID: 10921 RVA: 0x0011BF58 File Offset: 0x0011A158
		private void FilterControl_Unloaded(object sender, RoutedEventArgs e)
		{
			this.filter.DeregisterLocalizedText();
			bool flag = this.testcardsimservice != null;
			if (flag)
			{
				this.testcardsimservice.MonitoringSensIsActive = true;
			}
		}

		// Token: 0x06002AAA RID: 10922 RVA: 0x0001092C File Offset: 0x0000EB2C
		private void CheckBox_Checked(object sender, RoutedEventArgs e)
		{
		}

		// Token: 0x06002AAB RID: 10923 RVA: 0x0011BF8C File Offset: 0x0011A18C
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/filter/filtercontrol.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x06002AAC RID: 10924 RVA: 0x0011BFC4 File Offset: 0x0011A1C4
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
				((FilterControl)target).Unloaded += this.FilterControl_Unloaded;
			}
		}

		// Token: 0x04001A8D RID: 6797
		private ParameterHandling parHandling;

		// Token: 0x04001A8E RID: 6798
		private Filter filter;

		// Token: 0x04001A8F RID: 6799
		private AccessRightsManager accessManager;

		// Token: 0x04001A90 RID: 6800
		private DBConnection dbConnection;

		// Token: 0x04001A91 RID: 6801
		private TestcardSimService testcardsimservice;

		// Token: 0x04001A93 RID: 6803
		private bool _contentLoaded;
	}
}
