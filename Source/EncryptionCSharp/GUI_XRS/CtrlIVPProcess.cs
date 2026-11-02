using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Threading;
using DatabaseConnection;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x0200006A RID: 106
	public class CtrlIVPProcess : UserControl, IComponentConnector
	{
		// Token: 0x060009A0 RID: 2464 RVA: 0x0003B1BC File Offset: 0x000393BC
		public CtrlIVPProcess(AccessRightsManager accessManager, MachineState state, ParameterHandling handling, ImageDataTransfer imageTransfer, DBConnection connection, Backup backup, int productType, string number, MessageSystem messageSystem, ProductManager productManager)
		{
			this.InitializeComponent();
			this.viewModel = new IVPProcessViewModel(this.imagebox, accessManager, state, handling, imageTransfer, connection, backup, productType, number, messageSystem, productManager);
			base.DataContext = this.viewModel;
			this.viewModel.Close += this.ViewModel_Close;
			this.viewModel.certificateReady += this.setImage;
			this.webbrowser.Navigate(new Uri("about:blank"));
			this.viewModel.CheckUserRightforUnlockBin(this.btnUnlockBin);
			this.viewModel.UserLoggedIn = accessManager.CurrentUser;
		}

		// Token: 0x060009A1 RID: 2465 RVA: 0x0003B271 File Offset: 0x00039471
		private void ViewModel_Close()
		{
			base.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
			{
				this.webbrowser.Navigate("about:blank");
				this.webbrowser.Dispose();
				this.viewModel.DeregisterUnlockBinButton(this.btnUnlockBin);
				this.viewModel.Close -= this.ViewModel_Close;
				Window.GetWindow(this).Close();
				BindingOperations.ClearAllBindings(this);
				base.DataContext = null;
				this.viewModel.certificateReady -= this.setImage;
			}));
		}

		// Token: 0x060009A2 RID: 2466 RVA: 0x0003B290 File Offset: 0x00039490
		private void arrowRight_Click(object sender, RoutedEventArgs e)
		{
			bool button1Enabled = this.viewModel.button1Enabled;
			if (button1Enabled)
			{
				this.viewModel.button1Enabled = false;
				this.viewModel.button2Enabled = true;
			}
			else
			{
				bool button2Enabled = this.viewModel.button2Enabled;
				if (button2Enabled)
				{
					this.viewModel.button2Enabled = false;
					this.viewModel.button3Enabled = true;
				}
				else
				{
					bool button4Enabled = this.viewModel.button4Enabled;
					if (button4Enabled)
					{
						this.viewModel.End();
					}
				}
			}
		}

		// Token: 0x060009A3 RID: 2467 RVA: 0x0003B319 File Offset: 0x00039519
		private void arrowLeft_Click(object sender, RoutedEventArgs e)
		{
			this.webbrowser.Navigate("about:blank");
			this.viewModel.backToStart();
		}

		// Token: 0x060009A4 RID: 2468 RVA: 0x0003B33C File Offset: 0x0003953C
		private void TextBox_MouseUp(object sender, MouseButtonEventArgs e)
		{
			bool flag = e.Source is TextBox;
			if (flag)
			{
				this.viewModel.ShowKeyboard();
			}
		}

		// Token: 0x060009A5 RID: 2469 RVA: 0x0003B368 File Offset: 0x00039568
		public void setImage(string path)
		{
			bool flag = File.Exists(path);
			if (flag)
			{
				Stopwatch stopwatch = Stopwatch.StartNew();
				Action <>9__0;
				while (stopwatch.ElapsedMilliseconds < 1500L)
				{
					try
					{
						Dispatcher dispatcher = base.Dispatcher;
						Action callback;
						if ((callback = <>9__0) == null)
						{
							callback = (<>9__0 = delegate()
							{
								this.webbrowser.Navigate(new Uri(path));
							});
						}
						dispatcher.Invoke(callback);
						stopwatch.Stop();
						break;
					}
					catch
					{
					}
				}
			}
		}

		// Token: 0x060009A6 RID: 2470 RVA: 0x0003B404 File Offset: 0x00039604
		private void IVP_Unloaded(object sender, RoutedEventArgs e)
		{
			this.webbrowser.Dispose();
			this.viewModel.certificateReady -= this.setImage;
			this.viewModel.Close -= this.ViewModel_Close;
			BindingOperations.ClearAllBindings(this);
			base.DataContext = null;
			GC.Collect();
		}

		// Token: 0x060009A7 RID: 2471 RVA: 0x0003B463 File Offset: 0x00039663
		private void OkButton_Click(object sender, RoutedEventArgs e)
		{
			this.viewModel.setResult(true);
		}

		// Token: 0x060009A8 RID: 2472 RVA: 0x0003B473 File Offset: 0x00039673
		private void BadButton_Click_1(object sender, RoutedEventArgs e)
		{
			this.viewModel.setResult(false);
		}

		// Token: 0x060009A9 RID: 2473 RVA: 0x0003B484 File Offset: 0x00039684
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/integrated%20validation%20process/ctrlivpprocess.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x060009AA RID: 2474 RVA: 0x0003B4BC File Offset: 0x000396BC
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				((CtrlIVPProcess)target).Unloaded += this.IVP_Unloaded;
				break;
			case 2:
				this.mainGrid = (Grid)target;
				break;
			case 3:
				this.btnUnlockBin = (Button)target;
				break;
			case 4:
				this.imagebox = (XRSImageBox)target;
				break;
			case 5:
				((Button)target).Click += this.OkButton_Click;
				break;
			case 6:
				((Button)target).Click += this.BadButton_Click_1;
				break;
			case 7:
				this.listTestsamples = (ListView)target;
				break;
			case 8:
				((TextBox)target).MouseUp += this.TextBox_MouseUp;
				break;
			case 9:
				this.listProduct = (ListView)target;
				break;
			case 10:
				this.listCustomerInformation = (ListView)target;
				break;
			case 11:
				this.listManufacturerInformation = (ListView)target;
				break;
			case 12:
				this.pdfViewGrid = (Grid)target;
				break;
			case 13:
				this.webbrowser = (WebBrowser)target;
				break;
			case 14:
				this.arrowLeft = (Button)target;
				this.arrowLeft.Click += this.arrowLeft_Click;
				break;
			case 15:
				this.button4 = (Button)target;
				break;
			case 16:
				((Button)target).Click += this.arrowRight_Click;
				break;
			default:
				this._contentLoaded = true;
				break;
			}
		}

		// Token: 0x04000696 RID: 1686
		private IVPProcessViewModel viewModel;

		// Token: 0x04000697 RID: 1687
		internal Grid mainGrid;

		// Token: 0x04000698 RID: 1688
		internal Button btnUnlockBin;

		// Token: 0x04000699 RID: 1689
		internal XRSImageBox imagebox;

		// Token: 0x0400069A RID: 1690
		internal ListView listTestsamples;

		// Token: 0x0400069B RID: 1691
		internal ListView listProduct;

		// Token: 0x0400069C RID: 1692
		internal ListView listCustomerInformation;

		// Token: 0x0400069D RID: 1693
		internal ListView listManufacturerInformation;

		// Token: 0x0400069E RID: 1694
		internal Grid pdfViewGrid;

		// Token: 0x0400069F RID: 1695
		internal WebBrowser webbrowser;

		// Token: 0x040006A0 RID: 1696
		internal Button arrowLeft;

		// Token: 0x040006A1 RID: 1697
		internal Button button4;

		// Token: 0x040006A2 RID: 1698
		private bool _contentLoaded;
	}
}
