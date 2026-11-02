using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using DatabaseConnection;
using Definitions;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x02000245 RID: 581
	public class CtrlLogbook : UserControl, IComponentConnector
	{
		// Token: 0x06002C14 RID: 11284 RVA: 0x0012271C File Offset: 0x0012091C
		public CtrlLogbook(Logbook logbook, AccessRightsManager accessManager, DBConnection dbConnection, SetFooterMainWindow setFooterMainWindow)
		{
			this.accessManager = accessManager;
			this.InitializeComponent();
			this.logbookViewModel = new LogbookViewModel(logbook, accessManager, setFooterMainWindow);
			this.InitializeButtons(dbConnection);
			this.logbookViewModel.SetButtons(this.btnDelete, this.btnSave, this.btnFilterContaminations, this.btnFilterErrors, this.btnFilterWarnings, this.btnFilterInfos);
			this.SetControlRights();
			setFooterMainWindow.SetFooterDeactivation();
			base.DataContext = this.logbookViewModel;
		}

		// Token: 0x06002C15 RID: 11285 RVA: 0x001227B8 File Offset: 0x001209B8
		private void InitializeButtons(DBConnection dbConnection)
		{
			this.Buttons = new List<Button>();
			string usbLetter = "";
			dbConnection.GetUsbDriveLetter(ref usbLetter);
			this.btnSave = new ButtonUSB(usbLetter, "/img/saveonusb_small.png", "/img/saveonusbsmall_nousb.png", new RoutedEventHandler(this.logbookViewModel.SaveLogbook));
			this.Buttons.Add(this.btnSave);
			this.btnDelete = new Button();
			this.btnDelete.Content = ImageConverter.ToImage("/img/logbookdelete_small.png");
			this.btnDelete.Click += this.logbookViewModel.DeleteLogbook;
			this.btnDelete.Style = (base.FindResource("defaultButtonStyle") as Style);
			this.Buttons.Add(this.btnDelete);
			Button button = new Button();
			button.Content = "";
			button.Style = (base.FindResource("defaultButtonStyle") as Style);
			button.Visibility = Visibility.Hidden;
			button.Width = 20.0;
			this.Buttons.Add(button);
			this.btnFilterContaminations = new Button();
			Image image = ImageConverter.ToImage("/img/reject_orange.png");
			image.Height = 40.0;
			image.Width = 40.0;
			image.Stretch = Stretch.Uniform;
			this.btnFilterContaminations.Content = image;
			this.btnFilterContaminations.Click += this.logbookViewModel.FilterContaminations;
			this.btnFilterContaminations.Style = (base.FindResource("defaultButtonStyle") as Style);
			this.Buttons.Add(this.btnFilterContaminations);
			this.btnFilterErrors = new Button();
			Image image2 = ImageConverter.ToImage("/img/errorLog.png");
			image2.Height = 40.0;
			image2.Width = 40.0;
			image2.Stretch = Stretch.Uniform;
			this.btnFilterErrors.Content = image2;
			this.btnFilterErrors.Click += this.logbookViewModel.FilterErrors;
			this.btnFilterErrors.Style = (base.FindResource("defaultButtonStyle") as Style);
			this.Buttons.Add(this.btnFilterErrors);
			this.btnFilterWarnings = new Button();
			Image image3 = ImageConverter.ToImage("/img/warning_yellow.png");
			image3.Height = 40.0;
			image3.Width = 40.0;
			image3.Stretch = Stretch.Uniform;
			this.btnFilterWarnings.Content = image3;
			this.btnFilterWarnings.Click += this.logbookViewModel.FilterWarnings;
			this.btnFilterWarnings.Style = (base.FindResource("defaultButtonStyle") as Style);
			this.Buttons.Add(this.btnFilterWarnings);
			this.btnFilterInfos = new Button();
			Image image4 = ImageConverter.ToImage("/img/infoLog.png");
			image4.Height = 40.0;
			image4.Width = 40.0;
			image4.Stretch = Stretch.Uniform;
			this.btnFilterInfos.Content = image4;
			this.btnFilterInfos.Click += this.logbookViewModel.FilterInfos;
			this.btnFilterInfos.Style = (base.FindResource("defaultButtonStyle") as Style);
			this.Buttons.Add(this.btnFilterInfos);
		}

		// Token: 0x06002C16 RID: 11286 RVA: 0x00122B38 File Offset: 0x00120D38
		private void SetControlRights()
		{
			this.accessManager.RegisterControl(this.btnDelete, this.rightDeleteLogbook, this.rightDeleteLogbook);
			this.accessManager.RegisterControl(this.btnSave, this.rightSaveLogbook, this.rightSaveLogbook);
			this.accessManager.SetProperties();
		}

		// Token: 0x06002C17 RID: 11287 RVA: 0x00122B90 File Offset: 0x00120D90
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/logbook/ctrllogbook.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x06002C18 RID: 11288 RVA: 0x00122BC6 File Offset: 0x00120DC6
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			this._contentLoaded = true;
		}

		// Token: 0x04001B96 RID: 7062
		private BuiltInPriviledges rightDeleteLogbook = BuiltInPriviledges.Admin;

		// Token: 0x04001B97 RID: 7063
		private BuiltInPriviledges rightSaveLogbook = BuiltInPriviledges.Operator1;

		// Token: 0x04001B98 RID: 7064
		private Button btnDelete;

		// Token: 0x04001B99 RID: 7065
		private ButtonUSB btnSave;

		// Token: 0x04001B9A RID: 7066
		private Button btnFilterContaminations;

		// Token: 0x04001B9B RID: 7067
		private Button btnFilterErrors;

		// Token: 0x04001B9C RID: 7068
		private Button btnFilterWarnings;

		// Token: 0x04001B9D RID: 7069
		private Button btnFilterInfos;

		// Token: 0x04001B9E RID: 7070
		private LogbookViewModel logbookViewModel;

		// Token: 0x04001B9F RID: 7071
		private AccessRightsManager accessManager;

		// Token: 0x04001BA0 RID: 7072
		public List<Button> Buttons;

		// Token: 0x04001BA1 RID: 7073
		private bool _contentLoaded;
	}
}
