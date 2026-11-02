using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;
using DatabaseConnection;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x02000012 RID: 18
	public class CtrlAIAutotrain : UserControl, IComponentConnector
	{
		// Token: 0x1400000B RID: 11
		// (add) Token: 0x06000187 RID: 391 RVA: 0x000091D4 File Offset: 0x000073D4
		// (remove) Token: 0x06000188 RID: 392 RVA: 0x0000920C File Offset: 0x0000740C
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event EventHandler AutotrainCompleted;

		// Token: 0x06000189 RID: 393 RVA: 0x00009244 File Offset: 0x00007444
		public CtrlAIAutotrain(StoreImageData imageArchiver, AccessRightsManager accessManager, DBConnection dbConnection, ProductManager productManager, AIService aiService, ParameterHandling parHandling, ImageDataTransfer imageTransfer)
		{
			this.InitializeComponent();
			this.accessManager = accessManager;
			List<string> list = new List<string>();
			list.Add(StoreImageData.archiveDirectoryThroughput);
			list.Add(StoreImageData.archiveDirectoryReject);
			string tableName = "";
			dbConnection.GetCurrentProductTableNameAI(ref tableName);
			string archiveDirectoryAnomalyDetection = imageArchiver.GetArchiveDirectoryAnomalyDetection(tableName);
			list.Add(archiveDirectoryAnomalyDetection);
			this.viewModel = new AIAutotrainViewModel(this.imagebox, list.ToArray(), imageArchiver, dbConnection, productManager, aiService, parHandling, imageTransfer);
			this.viewModel.AutotrainCompleted += delegate(object s, EventArgs e)
			{
				EventHandler autotrainCompleted = this.AutotrainCompleted;
				if (autotrainCompleted != null)
				{
					autotrainCompleted(this, EventArgs.Empty);
				}
				Thread thread = new Thread(new ThreadStart(this.CloseSubWindow));
				thread.Start();
			};
			base.DataContext = this.viewModel;
			this.InitializeButtons();
			this.imagebox.ButtonPressed += this.Imagebox_ButtonPressed;
			this.imagebox.MouseDown += this.imagebox_MouseDown;
		}

		// Token: 0x0600018A RID: 394 RVA: 0x00009328 File Offset: 0x00007528
		private void InitializeButtons()
		{
			this.UiElementRightAligned = new Label
			{
				Content = "THiNK XA",
				FontSize = 30.0,
				FontWeight = FontWeights.Bold,
				FontFamily = new FontFamily("Verdana"),
				Foreground = Brushes.DarkGray,
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Right,
				Margin = new Thickness(0.0, 2.0, 20.0, 2.0)
			};
			this.PanelButtons = new DockPanel();
			this.PanelButtons.HorizontalAlignment = HorizontalAlignment.Stretch;
			this.PanelButtons.VerticalAlignment = VerticalAlignment.Stretch;
			Button button = new Button();
			button.Content = "";
			button.Style = (base.FindResource("defaultButtonStyle") as Style);
			button.Visibility = Visibility.Hidden;
			button.Width = 40.0;
			this.PanelButtons.Children.Add(button);
			this.viewModel.btnStartAutotrain.Content = ImageConverter.ToImage("/img/retrain_disabled.png");
			this.viewModel.btnStartAutotrain.Style = (base.FindResource("defaultButtonStyle") as Style);
			this.viewModel.btnStartAutotrain.Command = this.viewModel.StartAutotrainCommand;
			this.viewModel.btnStartAutotrain.Margin = new Thickness(0.0, 2.0, 10.0, 2.0);
			this.viewModel.btnStartAutotrain.IsEnabled = false;
			this.PanelButtons.Children.Add(this.viewModel.btnStartAutotrain);
			DockPanel.SetDock(this.viewModel.btnStartAutotrain, Dock.Left);
		}

		// Token: 0x0600018B RID: 395 RVA: 0x00009514 File Offset: 0x00007714
		private void Button_ScrollFirst(object sender, RoutedEventArgs e)
		{
			bool flag = this.viewModel.Preview.PreviewItems.Count > 0;
			if (flag)
			{
				this.lstImages.ScrollIntoView(this.viewModel.Preview.PreviewItems[0]);
			}
		}

		// Token: 0x0600018C RID: 396 RVA: 0x00009560 File Offset: 0x00007760
		private void Button_ScrollLast(object sender, RoutedEventArgs e)
		{
			bool flag = this.viewModel.Preview.PreviewItems.Count > 0;
			if (flag)
			{
				this.lstImages.ScrollIntoView(this.viewModel.Preview.PreviewItems[this.viewModel.Preview.PreviewItems.Count - 1]);
			}
		}

		// Token: 0x0600018D RID: 397 RVA: 0x000095C2 File Offset: 0x000077C2
		private void Imagebox_ButtonPressed()
		{
			this.ShowFullscreenImage();
		}

		// Token: 0x0600018E RID: 398 RVA: 0x000095C2 File Offset: 0x000077C2
		private void imagebox_MouseDown(object sender, MouseButtonEventArgs e)
		{
			this.ShowFullscreenImage();
		}

		// Token: 0x0600018F RID: 399 RVA: 0x000095CC File Offset: 0x000077CC
		private void ShowFullscreenImage()
		{
			bool flag = !this.bFullscreenImageShown;
			if (flag)
			{
				this.bFullscreenImageShown = true;
				this.ctrlFullscreenImage = new CtrlFullscreenImage(null, false, false, "0", this.accessManager, null, null, this.viewModel);
				this.ctrlFullscreenImage.Unloaded += this.ctrlFullscreenImage_Unloaded;
				Subwindow.Show(this.ctrlFullscreenImage, SubwindowButtons.Back);
			}
		}

		// Token: 0x06000190 RID: 400 RVA: 0x00009638 File Offset: 0x00007838
		private void ctrlFullscreenImage_Unloaded(object sender, RoutedEventArgs e)
		{
			bool flag = this.ctrlFullscreenImage != null;
			if (flag)
			{
				this.ctrlFullscreenImage.Unloaded -= this.ctrlFullscreenImage_Unloaded;
			}
			((XRSImageBoxViewModel)this.imagebox.DataContext).IsXriInfoVisible = false;
			this.imagebox.SetAutoZoomAndBrightness();
			this.bFullscreenImageShown = false;
		}

		// Token: 0x06000191 RID: 401 RVA: 0x00009698 File Offset: 0x00007898
		private void lstBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			IList selectedItems = ((ListBox)sender).SelectedItems;
			this.viewModel.SelectedPrievewItems = selectedItems.Cast<PreviewItem>().ToList<PreviewItem>();
			bool flag = this.viewModel.SelectedPrievewItems.Count > 0;
			if (flag)
			{
				for (int i = 0; i < this.viewModel.SelectedPrievewItems.Count; i++)
				{
					this.viewModel.SelectedPrievewItems[i].IsMarked = true;
					this.viewModel.SelectedPrievewItems[i].LoadXRSImage();
				}
				this.imagebox.ViewXRSImage(this.viewModel.SelectedPrievewItems[this.viewModel.SelectedPrievewItems.Count - 1].XrsImage);
			}
			bool flag2 = this.viewModel.SelectedPrievewItems != null;
			if (flag2)
			{
				bool flag3 = this.viewModel.SelectedPrievewItems.Count > 3 && this.viewModel.SelectedPrievewItems[0].XrsImage.Height > 3000;
				if (flag3)
				{
					this.viewModel.SelectedPrievewItems[0].IsMarked = false;
					((ListBox)sender).SelectedItems.RemoveAt(0);
				}
				this.viewModel.SelectedItemCount = Convert.ToString(this.viewModel.SelectedPrievewItems.Count);
			}
			else
			{
				this.viewModel.SelectedItemCount = "0";
			}
			this.viewModel.UpdateTextAndButtons();
		}

		// Token: 0x06000192 RID: 402 RVA: 0x0000982C File Offset: 0x00007A2C
		private void UserControl_Unloaded(object sender, RoutedEventArgs e)
		{
			this.viewModel.ClosePreview();
			BindingOperations.ClearAllBindings(this);
			base.DataContext = null;
			this.imagebox.ButtonPressed -= this.Imagebox_ButtonPressed;
			this.imagebox.MouseDown -= this.imagebox_MouseDown;
			this.imagebox = null;
			this.viewModel.Preview.FirstItemCreated -= this.viewModel.Preview_FirstItemCreated;
			this.viewModel.CleanUp();
			GC.Collect();
			bool flag = this.viewModel.SelectedPrievewItems != null;
			if (flag)
			{
				this.viewModel.SelectedPrievewItems.Clear();
				this.viewModel.SelectedPrievewItems = null;
			}
			bool flag2 = this.wdSub != null;
			if (flag2)
			{
				this.wdSub.BackButtonClicked -= this.wdSub_BackButtonClicked;
			}
			this.DeregisterLocalizedText();
		}

		// Token: 0x06000193 RID: 403 RVA: 0x0000991E File Offset: 0x00007B1E
		public void DeregisterLocalizedText()
		{
			this.viewModel.DeregisterLocalizedText();
		}

		// Token: 0x06000194 RID: 404 RVA: 0x0000992D File Offset: 0x00007B2D
		public void CloseSubWindow()
		{
			base.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
			{
				bool flag = Window.GetWindow(this) != null;
				if (flag)
				{
					(Window.GetWindow(this) as Subwindow).Close();
				}
			}));
		}

		// Token: 0x06000195 RID: 405 RVA: 0x0000994C File Offset: 0x00007B4C
		private void wdSub_BackButtonClicked()
		{
			bool flag = this.wdSub != null;
			if (flag)
			{
				string message = LocalizationManager.Localize("txtAnomalyDetectionDisableInference");
				bool flag2 = Dialog.Show(LocalizationManager.Localize("txtWarning"), message, DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
				bool flag3 = flag2;
				if (flag3)
				{
					this.viewModel.CancelAutolearn();
					this.wdSub.IgnoreNextBack = false;
					this.wdSub.BackButtonClicked -= this.wdSub_BackButtonClicked;
				}
				else
				{
					this.wdSub.IgnoreNextBack = true;
				}
			}
		}

		// Token: 0x06000196 RID: 406 RVA: 0x000099D8 File Offset: 0x00007BD8
		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			this.wdSub = (Window.GetWindow(this) as Subwindow);
			bool flag = this.wdSub != null;
			if (flag)
			{
				this.wdSub.BackButtonClicked += this.wdSub_BackButtonClicked;
				this.wdSub.btnHome.Content = ImageConverter.ToImage("/img/cancel.png");
				this.wdSub.btnHome.Style = (base.FindResource("cancelButtonStyle") as Style);
			}
			base.Dispatcher.BeginInvoke(new Action(delegate()
			{
				string text = LocalizationManager.Localize("txtPreAIAutotrain");
				string text2 = LocalizationManager.Localize("txtPostAIAutotrain");
				string message = string.Concat(new string[]
				{
					text,
					" ",
					10.ToString(),
					" ",
					text2
				});
				Dialog.Show(LocalizationManager.Localize("txtInfo"), message, DialogStyle.Info, DialogButtons.OK);
			}), DispatcherPriority.ApplicationIdle, new object[0]);
		}

		// Token: 0x06000197 RID: 407 RVA: 0x00009A8C File Offset: 0x00007C8C
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/ai/ctrlaiautotrain.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x06000198 RID: 408 RVA: 0x00009AC4 File Offset: 0x00007CC4
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				((CtrlAIAutotrain)target).Unloaded += this.UserControl_Unloaded;
				((CtrlAIAutotrain)target).Loaded += this.UserControl_Loaded;
				break;
			case 2:
				this.mainGrid = (Grid)target;
				break;
			case 3:
				this.imagebox = (XRSImageBox)target;
				break;
			case 4:
				((Button)target).Click += this.Button_ScrollFirst;
				break;
			case 5:
				this.lstImages = (ListBox)target;
				this.lstImages.SelectionChanged += this.lstBox_OnSelectionChanged;
				break;
			case 6:
				((Button)target).Click += this.Button_ScrollLast;
				break;
			default:
				this._contentLoaded = true;
				break;
			}
		}

		// Token: 0x0400017B RID: 379
		private AIAutotrainViewModel viewModel;

		// Token: 0x0400017C RID: 380
		public DockPanel PanelButtons;

		// Token: 0x0400017D RID: 381
		public UIElement UiElementRightAligned;

		// Token: 0x0400017E RID: 382
		private AccessRightsManager accessManager;

		// Token: 0x0400017F RID: 383
		private CtrlFullscreenImage ctrlFullscreenImage;

		// Token: 0x04000180 RID: 384
		private bool bFullscreenImageShown = false;

		// Token: 0x04000182 RID: 386
		private Subwindow wdSub;

		// Token: 0x04000183 RID: 387
		internal Grid mainGrid;

		// Token: 0x04000184 RID: 388
		internal XRSImageBox imagebox;

		// Token: 0x04000185 RID: 389
		internal ListBox lstImages;

		// Token: 0x04000186 RID: 390
		private bool _contentLoaded;
	}
}
