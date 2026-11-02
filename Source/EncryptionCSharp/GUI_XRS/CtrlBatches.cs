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
	// Token: 0x0200003B RID: 59
	public class CtrlBatches : UserControl, IComponentConnector
	{
		// Token: 0x06000674 RID: 1652 RVA: 0x0002B440 File Offset: 0x00029640
		public CtrlBatches(DBConnection dbConnection, ProductManager productManager, ImageDataTransfer imageTransfer, bool showNewBatchKeyboard)
		{
			this.InitializeComponent();
			this.viewModel = new BatchesViewModel(dbConnection, productManager, imageTransfer, showNewBatchKeyboard);
			this.InitializeButtons(productManager.CurrentLoadedProduct == productManager.SelectedProduct);
			base.DataContext = this.viewModel;
		}

		// Token: 0x06000675 RID: 1653 RVA: 0x0002B490 File Offset: 0x00029690
		private void InitializeButtons(bool showStartBatchButton)
		{
			this.Buttons = new List<Button>();
			if (showStartBatchButton)
			{
				Button button = new Button();
				button.Style = (base.FindResource("defaultButtonStyle") as Style);
				button.Content = ImageConverter.ToImage("/img/changeproduct_small.png");
				button.Command = this.viewModel.StartBatchCommand;
				this.Buttons.Add(button);
			}
			Button button2 = new Button();
			button2.Style = (base.FindResource("defaultButtonStyle") as Style);
			button2.Content = ImageConverter.ToImage("/img/addproduct.png");
			button2.Command = this.viewModel.NewBatchCommand;
			this.Buttons.Add(button2);
			Button button3 = new Button();
			button3.Style = (base.FindResource("defaultButtonStyle") as Style);
			button3.Content = ImageConverter.ToImage("/img/deleteproduct.png");
			button3.Command = this.viewModel.DeleteBatchCommand;
			this.Buttons.Add(button3);
			Button button4 = new Button();
			button4.Style = (base.FindResource("defaultButtonStyle") as Style);
			button4.Content = ImageConverter.ToImage("/img/editproduct.png");
			button4.Command = this.viewModel.RenameBatchCommand;
			this.Buttons.Add(button4);
		}

		// Token: 0x06000676 RID: 1654 RVA: 0x0002B5EC File Offset: 0x000297EC
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/batches/ctrlbatches.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x06000677 RID: 1655 RVA: 0x0002B622 File Offset: 0x00029822
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			this._contentLoaded = true;
		}

		// Token: 0x04000493 RID: 1171
		private BatchesViewModel viewModel;

		// Token: 0x04000494 RID: 1172
		public List<Button> Buttons;

		// Token: 0x04000495 RID: 1173
		private bool _contentLoaded;
	}
}
