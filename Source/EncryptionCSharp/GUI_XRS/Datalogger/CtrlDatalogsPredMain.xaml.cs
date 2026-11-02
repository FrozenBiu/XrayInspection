using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using DatabaseConnection;
using Microsoft.Windows.Controls;

namespace GUI_XRS.Datalogger
{
	// Token: 0x02000264 RID: 612
	public partial class CtrlDatalogsPredMain : UserControl
	{
		// Token: 0x06002EE6 RID: 12006 RVA: 0x00133694 File Offset: 0x00131894
		public CtrlDatalogsPredMain(DBDataLogs dBDataLogs, DBConnection dBConnection, ParameterHandling parHandling)
		{
			this.InitializeComponent();
			this.dBDataLogs = dBDataLogs;
			this.dBConnection = dBConnection;
			this.parHandling = parHandling;
			this.datalogsViewModel = new DatalogsPredMainViewModel(dBDataLogs, dBConnection, parHandling);
			base.DataContext = this.datalogsViewModel;
			this.InitializeButtons();
		}

		// Token: 0x06002EE7 RID: 12007 RVA: 0x001336E8 File Offset: 0x001318E8
		private void InitializeButtons()
		{
			this.PanelButtons = new DockPanel();
			this.PanelButtons.HorizontalAlignment = HorizontalAlignment.Stretch;
			this.PanelButtons.VerticalAlignment = VerticalAlignment.Stretch;
			string usbLetter = "";
			this.dBConnection.GetUsbDriveLetter(ref usbLetter);
			this.datalogsViewModel.btnExportCsvSelection = new ButtonUSB(usbLetter, "/img/saveCSVSelection.png", "/img/saveCSVSelection_disabled.png", new RoutedEventHandler(this.ExportSelectioninCSV), true);
			this.PanelButtons.Children.Add(this.datalogsViewModel.btnExportCsvSelection);
			DockPanel.SetDock(this.datalogsViewModel.btnExportCsvSelection, Dock.Left);
			this.datalogsViewModel.btnExportCsvAll = new ButtonUSB(usbLetter, "/img/saveCSVAll.png", "/img/saveCSVAll_disabled.png", new RoutedEventHandler(this.ExportAllTablesinCSV), true);
			this.PanelButtons.Children.Add(this.datalogsViewModel.btnExportCsvAll);
			DockPanel.SetDock(this.datalogsViewModel.btnExportCsvAll, Dock.Left);
			this.datalogsViewModel.btnExportPng = new ButtonUSB(usbLetter, "/img/savePNG.png", "/img/savePNG_disabled.png", new RoutedEventHandler(this.ExportinPNG), true);
			this.PanelButtons.Children.Add(this.datalogsViewModel.btnExportPng);
			DockPanel.SetDock(this.datalogsViewModel.btnExportPng, Dock.Left);
			Grid element = new Grid();
			this.PanelButtons.Children.Add(element);
			DockPanel.SetDock(element, Dock.Left);
		}

		// Token: 0x06002EE8 RID: 12008 RVA: 0x00133851 File Offset: 0x00131A51
		private void ExportSelectioninCSV(object sender, RoutedEventArgs e)
		{
			this.datalogsViewModel.CreateCsv(false);
		}

		// Token: 0x06002EE9 RID: 12009 RVA: 0x00133861 File Offset: 0x00131A61
		private void ExportAllTablesinCSV(object sender, RoutedEventArgs e)
		{
			this.datalogsViewModel.CreateCsv(true);
		}

		// Token: 0x06002EEA RID: 12010 RVA: 0x00133874 File Offset: 0x00131A74
		private void ExportinPNG(object sender, RoutedEventArgs e)
		{
			Rect descendantBounds = VisualTreeHelper.GetDescendantBounds(this.datalogsTables);
			this.datalogsViewModel.CreatePng(this.datalogsTables, descendantBounds);
		}

		// Token: 0x04001D66 RID: 7526
		private DatalogsPredMainViewModel datalogsViewModel;

		// Token: 0x04001D67 RID: 7527
		private DBDataLogs dBDataLogs;

		// Token: 0x04001D68 RID: 7528
		private DBConnection dBConnection;

		// Token: 0x04001D69 RID: 7529
		private ParameterHandling parHandling;

		// Token: 0x04001D6A RID: 7530
		public DockPanel PanelButtons;
	}
}
