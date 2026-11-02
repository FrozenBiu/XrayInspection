using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using DatabaseConnection;
using Definitions;
using SesoPlot;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x0200009F RID: 159
	public class CtrlDualEnergy : UserControl, IComponentConnector
	{
		// Token: 0x170004C0 RID: 1216
		// (get) Token: 0x06000FC6 RID: 4038 RVA: 0x0006201E File Offset: 0x0006021E
		// (set) Token: 0x06000FC7 RID: 4039 RVA: 0x00062026 File Offset: 0x00060226
		public List<Button> Buttons { get; set; }

		// Token: 0x06000FC8 RID: 4040 RVA: 0x00062030 File Offset: 0x00060230
		public CtrlDualEnergy(StoreImageData imageStorer, ImageDataTransfer imageTransfer, ParameterHandling parHandling, ProductManager productManager, DBConnection dbConnection, AccessRightsManager accessmanager, MachineState machineState)
		{
			this.accessmanager = accessmanager;
			this.InitializeComponent();
			this.viewModel = new DualEnergyViewModel(imageStorer, imageTransfer, parHandling, productManager, dbConnection, machineState, this.imagebox);
			base.DataContext = this.viewModel;
			for (int i = 0; i < this.plot.Graphs.Count; i++)
			{
				this.plot.Graphs[i].DataContext = this.viewModel;
			}
			this.InitializeButtons();
		}

		// Token: 0x06000FC9 RID: 4041 RVA: 0x000620C0 File Offset: 0x000602C0
		private void InitializeButtons()
		{
			this.Buttons = new List<Button>();
			bool flag = this.accessmanager.CurrentUser.Rights == BuiltInPriviledges.Service;
			Image image = ImageConverter.ToImage("/img/rectangleDotted.png");
			image.Height = 40.0;
			image.Width = 40.0;
			image.Stretch = Stretch.Uniform;
			this.btnMoveOrSelect = new Button();
			this.btnMoveOrSelect.Content = image;
			this.btnMoveOrSelect.Click += this.ChangeToMoveOrSelect;
			this.btnMoveOrSelect.Style = (base.FindResource("defaultButtonStyle") as Style);
			this.Buttons.Add(this.btnMoveOrSelect);
			Button button = new Button();
			button.Content = ImageConverter.ToImage("/img/saveDB.png");
			button.Style = (base.FindResource("defaultButtonStyle") as Style);
			button.Command = this.viewModel.SaveMCwithNewTolCommand;
			this.Buttons.Add(button);
			bool flag2 = flag;
			if (flag2)
			{
				Button button2 = new Button();
				button2.Content = ImageConverter.ToImage("/img/savePNG.png");
				button2.Style = (base.FindResource("defaultButtonStyle") as Style);
				button2.Click += this.SaveMaterialCurvePNG_Click;
				this.Buttons.Add(button2);
			}
			Button button3 = new Button();
			button3.Content = ImageConverter.ToImage("/img/simulateimage_small.png");
			button3.Style = (base.FindResource("defaultButtonStyle") as Style);
			button3.Click += this.ReloadXRI_Click;
			this.Buttons.Add(button3);
		}

		// Token: 0x06000FCA RID: 4042 RVA: 0x00062280 File Offset: 0x00060480
		private void SaveMaterialCurvePNG_Click(object sender, RoutedEventArgs e)
		{
			bool flag = Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("msgSaveDualMaterialCurvePNG"), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
			if (flag)
			{
				Rect descendantBounds = VisualTreeHelper.GetDescendantBounds(this.chartGrid);
				this.viewModel.SaveMaterialCurveAsPng(this.chartGrid, descendantBounds);
			}
		}

		// Token: 0x06000FCB RID: 4043 RVA: 0x000622D1 File Offset: 0x000604D1
		private void ChangeToMoveOrSelect(object sender, RoutedEventArgs e)
		{
			this.imagebox.ToggleBoxSelectionModeAndButtonImage(this.btnMoveOrSelect);
		}

		// Token: 0x06000FCC RID: 4044 RVA: 0x000622E8 File Offset: 0x000604E8
		private void ReloadXRI_Click(object sender, RoutedEventArgs e)
		{
			bool allowReloadXRI = this.viewModel.AllowReloadXRI;
			if (allowReloadXRI)
			{
				this.viewModel.RequestMaterialCurve();
				this.viewModel.SendLatestImage();
			}
			else
			{
				Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("msgReloadNotPossible"), DialogStyle.Ok, DialogButtons.OK, false);
			}
		}

		// Token: 0x06000FCD RID: 4045 RVA: 0x00062340 File Offset: 0x00060540
		private void UserControl_Unloaded(object sender, RoutedEventArgs e)
		{
			this.viewModel.Image = null;
			this.viewModel.DebugImage = null;
			BindingOperations.ClearAllBindings(this);
			base.DataContext = null;
		}

		// Token: 0x06000FCE RID: 4046 RVA: 0x0006236C File Offset: 0x0006056C
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/dual/ctrldualenergy.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x06000FCF RID: 4047 RVA: 0x000623A4 File Offset: 0x000605A4
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				((CtrlDualEnergy)target).Unloaded += this.UserControl_Unloaded;
				break;
			case 2:
				this.chartGrid = (Grid)target;
				break;
			case 3:
				this.plot = (SesoPlot)target;
				break;
			case 4:
				this.tabCtrl = (TabControl)target;
				break;
			case 5:
				this.imagebox = (XRSImageBox)target;
				break;
			default:
				this._contentLoaded = true;
				break;
			}
		}

		// Token: 0x04000A92 RID: 2706
		private DualEnergyViewModel viewModel;

		// Token: 0x04000A93 RID: 2707
		private AccessRightsManager accessmanager;

		// Token: 0x04000A95 RID: 2709
		private Button btnMoveOrSelect;

		// Token: 0x04000A96 RID: 2710
		internal Grid chartGrid;

		// Token: 0x04000A97 RID: 2711
		internal SesoPlot plot;

		// Token: 0x04000A98 RID: 2712
		internal TabControl tabCtrl;

		// Token: 0x04000A99 RID: 2713
		internal XRSImageBox imagebox;

		// Token: 0x04000A9A RID: 2714
		private bool _contentLoaded;
	}
}
