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
	// Token: 0x02000019 RID: 25
	public class CtrlAIRestoreOptions : UserControl, IComponentConnector
	{
		// Token: 0x1400000F RID: 15
		// (add) Token: 0x060001D1 RID: 465 RVA: 0x0000B434 File Offset: 0x00009634
		// (remove) Token: 0x060001D2 RID: 466 RVA: 0x0000B46C File Offset: 0x0000966C
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event EventHandler UpdateColourBtnSelectionMenu;

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x060001D3 RID: 467 RVA: 0x0000B4A1 File Offset: 0x000096A1
		// (set) Token: 0x060001D4 RID: 468 RVA: 0x0000B4A9 File Offset: 0x000096A9
		public List<Button> Buttons { get; set; }

		// Token: 0x060001D5 RID: 469 RVA: 0x0000B4B2 File Offset: 0x000096B2
		public CtrlAIRestoreOptions(DBConnection dbConnection, ParameterHandling parHandling, ProductManager productManager)
		{
			this.InitializeComponent();
			this.viewModel = new AIRestoreOptionsViewModel(dbConnection, parHandling, productManager);
			base.DataContext = this.viewModel;
			this.InitializeButtons();
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x0000B4E8 File Offset: 0x000096E8
		private void InitializeButtons()
		{
			this.Buttons = new List<Button>();
			Button button = new Button();
			button.Content = ImageConverter.ToImage("/img/usb.png");
			button.Style = (base.FindResource("btnStyleNoBorders") as Style);
			button.Visibility = Visibility.Hidden;
			this.Buttons.Add(button);
			this.viewModel.StartUsbSurvey(button);
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x0000B552 File Offset: 0x00009752
		private void CtrAIRestoreOptions_Unloaded(object sender, RoutedEventArgs e)
		{
			EventHandler updateColourBtnSelectionMenu = this.UpdateColourBtnSelectionMenu;
			if (updateColourBtnSelectionMenu != null)
			{
				updateColourBtnSelectionMenu(this, EventArgs.Empty);
			}
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x0000B570 File Offset: 0x00009770
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/ai/ctrlairestoreoptions.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x0000B5A8 File Offset: 0x000097A8
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
				((CtrlAIRestoreOptions)target).Unloaded += this.CtrAIRestoreOptions_Unloaded;
			}
		}

		// Token: 0x040001A7 RID: 423
		private AIRestoreOptionsViewModel viewModel;

		// Token: 0x040001AA RID: 426
		private bool _contentLoaded;
	}
}
