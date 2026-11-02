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
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x02000018 RID: 24
	public class CtrlAIRestoreListProducts : UserControl, IComponentConnector
	{
		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060001CB RID: 459 RVA: 0x0000B2F9 File Offset: 0x000094F9
		// (set) Token: 0x060001CC RID: 460 RVA: 0x0000B301 File Offset: 0x00009501
		public List<Button> Buttons { get; set; }

		// Token: 0x060001CD RID: 461 RVA: 0x0000B30A File Offset: 0x0000950A
		public CtrlAIRestoreListProducts(string backupPath, List<string> tableNamesAI, ParameterHandling parHandling, DBConnection dbConnection, bool restoreThinkXA, bool restoreThinkXO, ProductManager productManager)
		{
			this.InitializeComponent();
			this.viewModel = new AIRestoreListProductsViewModel(backupPath, tableNamesAI, parHandling, dbConnection, restoreThinkXA, restoreThinkXO, productManager);
			base.DataContext = this.viewModel;
			this.InitializeButtons();
		}

		// Token: 0x060001CE RID: 462 RVA: 0x0000B348 File Offset: 0x00009548
		private void InitializeButtons()
		{
			this.Buttons = new List<Button>();
			Button button = new Button();
			Image image = ImageConverter.ToImage("/img/restore.png");
			image.Height = 45.0;
			image.Stretch = Stretch.Uniform;
			button.Content = image;
			button.Style = (base.FindResource("defaultButtonStyle") as Style);
			button.Command = this.viewModel.DoRestoreCommand;
			this.Buttons.Add(button);
		}

		// Token: 0x060001CF RID: 463 RVA: 0x0000B3CC File Offset: 0x000095CC
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/ai/ctrlairestorelistproducts.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x0000B404 File Offset: 0x00009604
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
				this.lstBackups = (ListBox)target;
			}
		}

		// Token: 0x040001A3 RID: 419
		private AIRestoreListProductsViewModel viewModel;

		// Token: 0x040001A5 RID: 421
		internal ListBox lstBackups;

		// Token: 0x040001A6 RID: 422
		private bool _contentLoaded;
	}
}
