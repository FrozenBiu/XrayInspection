using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

namespace GUI_XRS
{
	// Token: 0x0200009D RID: 157
	public class CtrlDocumentsManagement : UserControl, IComponentConnector, IStyleConnector
	{
		// Token: 0x170004BC RID: 1212
		// (get) Token: 0x06000FB7 RID: 4023 RVA: 0x00061D4D File Offset: 0x0005FF4D
		// (set) Token: 0x06000FB8 RID: 4024 RVA: 0x00061D55 File Offset: 0x0005FF55
		public List<Button> Buttons { get; set; }

		// Token: 0x06000FB9 RID: 4025 RVA: 0x00061D5E File Offset: 0x0005FF5E
		public CtrlDocumentsManagement(string documentsPath, string usbPath, AccessRightsManager accessManager)
		{
			this.InitializeComponent();
			this.viewModel = new DocumentsManagementViewModel(documentsPath, usbPath, accessManager);
			this.InitializeButtons();
			base.DataContext = this.viewModel;
		}

		// Token: 0x06000FBA RID: 4026 RVA: 0x00061D94 File Offset: 0x0005FF94
		private void listBoxItem_DoubleClick_LocalDoc(object sender, RoutedEventArgs e)
		{
			bool flag = sender is ListBoxItem;
			if (flag)
			{
				bool flag2 = File.Exists(this.viewModel.SelectedLocalDocument.FilePath);
				if (flag2)
				{
					WdManual wdManual = new WdManual(this.viewModel.SelectedLocalDocument.FilePath);
					wdManual.Show();
				}
			}
		}

		// Token: 0x06000FBB RID: 4027 RVA: 0x00061DEC File Offset: 0x0005FFEC
		private void listBoxItem_DoubleClick_USBDoc(object sender, RoutedEventArgs e)
		{
			string text = "";
			bool flag = sender is ListBoxItem;
			if (flag)
			{
				bool flag2 = File.Exists(this.viewModel.SelectedUSBDocument.FilePath);
				if (flag2)
				{
					text = this.viewModel.SelectedUSBDocument.FilePath.Substring(0, 2) + "\\" + this.viewModel.SelectedUSBDocument.DisplayedName + ".pdf";
				}
				bool flag3 = File.Exists(text);
				if (flag3)
				{
					WdManual wdManual = new WdManual(text);
					wdManual.Show();
				}
			}
		}

		// Token: 0x06000FBC RID: 4028 RVA: 0x00061E80 File Offset: 0x00060080
		private void InitializeButtons()
		{
			this.Buttons = new List<Button>();
			Button button = new Button();
			button.Style = (base.FindResource("defaultButtonStyle") as Style);
			this.Buttons.Add(button);
		}

		// Token: 0x06000FBD RID: 4029 RVA: 0x00061EC4 File Offset: 0x000600C4
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/documentsmenu/ctrldocumentsmanagement.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x06000FBE RID: 4030 RVA: 0x00061EFC File Offset: 0x000600FC
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
				this.ListBoxDocuments = (ListBox)target;
			}
		}

		// Token: 0x06000FBF RID: 4031 RVA: 0x00061F2C File Offset: 0x0006012C
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IStyleConnector.Connect(int connectionId, object target)
		{
			if (connectionId != 2)
			{
				if (connectionId == 3)
				{
					EventSetter eventSetter = new EventSetter();
					eventSetter.Event = Control.MouseDoubleClickEvent;
					eventSetter.Handler = new MouseButtonEventHandler(this.listBoxItem_DoubleClick_USBDoc);
					((Style)target).Setters.Add(eventSetter);
				}
			}
			else
			{
				EventSetter eventSetter = new EventSetter();
				eventSetter.Event = Control.MouseDoubleClickEvent;
				eventSetter.Handler = new MouseButtonEventHandler(this.listBoxItem_DoubleClick_LocalDoc);
				((Style)target).Setters.Add(eventSetter);
			}
		}

		// Token: 0x04000A8B RID: 2699
		private DocumentsManagementViewModel viewModel;

		// Token: 0x04000A8D RID: 2701
		internal ListBox ListBoxDocuments;

		// Token: 0x04000A8E RID: 2702
		private bool _contentLoaded;
	}
}
