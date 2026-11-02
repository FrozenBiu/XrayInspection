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
	// Token: 0x02000017 RID: 23
	public class CtrlAIRestoreListBackups : UserControl, IComponentConnector
	{
		// Token: 0x17000074 RID: 116
		// (get) Token: 0x060001C5 RID: 453 RVA: 0x0000B1C5 File Offset: 0x000093C5
		// (set) Token: 0x060001C6 RID: 454 RVA: 0x0000B1CD File Offset: 0x000093CD
		public List<Button> Buttons { get; set; }

		// Token: 0x060001C7 RID: 455 RVA: 0x0000B1D6 File Offset: 0x000093D6
		public CtrlAIRestoreListBackups(DBConnection dbConnection, ParameterHandling parHandling, bool restoreThinkXA, bool restoreThinkXO, ProductManager productManager)
		{
			this.InitializeComponent();
			this.viewModel = new AIRestoreListBackupsViewModel(dbConnection, parHandling, restoreThinkXA, restoreThinkXO, productManager);
			base.DataContext = this.viewModel;
			this.InitializeButtons();
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x0000B210 File Offset: 0x00009410
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

		// Token: 0x060001C9 RID: 457 RVA: 0x0000B294 File Offset: 0x00009494
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/ai/ctrlairestorelistbackups.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x060001CA RID: 458 RVA: 0x0000B2CC File Offset: 0x000094CC
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

		// Token: 0x0400019F RID: 415
		private AIRestoreListBackupsViewModel viewModel;

		// Token: 0x040001A1 RID: 417
		internal ListBox lstBackups;

		// Token: 0x040001A2 RID: 418
		private bool _contentLoaded;
	}
}
