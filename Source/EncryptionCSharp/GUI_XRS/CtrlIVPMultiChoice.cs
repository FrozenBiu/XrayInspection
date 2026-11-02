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

namespace GUI_XRS
{
	// Token: 0x02000069 RID: 105
	public class CtrlIVPMultiChoice : UserControl, IComponentConnector, IStyleConnector
	{
		// Token: 0x0600099A RID: 2458 RVA: 0x0003B084 File Offset: 0x00039284
		public CtrlIVPMultiChoice(ProductManager productManager, DBConnection dbConnection, ParameterHandling parameterHandling, AccessRightsManager accessManager, MachineState machineState, ImageDataTransfer imageTransfer, Backup backup, MessageSystem messageSystem)
		{
			this.InitializeComponent();
			this.viewModel = new IVPMultiChoice(accessManager, machineState, parameterHandling, imageTransfer, backup, productManager, dbConnection, messageSystem);
			base.DataContext = this.viewModel;
		}

		// Token: 0x0600099B RID: 2459 RVA: 0x0003B0C8 File Offset: 0x000392C8
		private void listBoxItem_DoubleClick(object sender, RoutedEventArgs e)
		{
			bool flag = sender is ListBoxItem;
			if (flag)
			{
				this.viewModel.CheckItem();
			}
		}

		// Token: 0x0600099C RID: 2460 RVA: 0x0003AF21 File Offset: 0x00039121
		private void Multichoice_Unloaded(object sender, RoutedEventArgs e)
		{
			BindingOperations.ClearAllBindings(this);
			base.DataContext = null;
			GC.Collect();
		}

		// Token: 0x0600099D RID: 2461 RVA: 0x0003B0F4 File Offset: 0x000392F4
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/integrated%20validation%20process/ctrlivpmultichoice.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x0600099E RID: 2462 RVA: 0x0003B12C File Offset: 0x0003932C
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
				((CtrlIVPMultiChoice)target).Unloaded += this.Multichoice_Unloaded;
			}
		}

		// Token: 0x0600099F RID: 2463 RVA: 0x0003B168 File Offset: 0x00039368
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IStyleConnector.Connect(int connectionId, object target)
		{
			if (connectionId == 2)
			{
				EventSetter eventSetter = new EventSetter();
				eventSetter.Event = Control.MouseDoubleClickEvent;
				eventSetter.Handler = new MouseButtonEventHandler(this.listBoxItem_DoubleClick);
				((Style)target).Setters.Add(eventSetter);
			}
		}

		// Token: 0x04000694 RID: 1684
		private IVPMultiChoice viewModel;

		// Token: 0x04000695 RID: 1685
		private bool _contentLoaded;
	}
}
