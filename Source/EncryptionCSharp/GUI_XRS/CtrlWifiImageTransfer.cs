using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Threading;
using DatabaseConnection;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x020000F8 RID: 248
	public class CtrlWifiImageTransfer : UserControl, IComponentConnector
	{
		// Token: 0x1700072A RID: 1834
		// (get) Token: 0x060017D8 RID: 6104 RVA: 0x0008FD84 File Offset: 0x0008DF84
		// (set) Token: 0x060017D9 RID: 6105 RVA: 0x0008FD8C File Offset: 0x0008DF8C
		public List<Button> Buttons { get; set; }

		// Token: 0x1700072B RID: 1835
		// (get) Token: 0x060017DA RID: 6106 RVA: 0x0008FD95 File Offset: 0x0008DF95
		// (set) Token: 0x060017DB RID: 6107 RVA: 0x0008FD9D File Offset: 0x0008DF9D
		public BoundLocalizedText StrNetwork { get; set; }

		// Token: 0x060017DC RID: 6108 RVA: 0x0008FDA8 File Offset: 0x0008DFA8
		public CtrlWifiImageTransfer(DBConnection dbConnection)
		{
			this.InitializeComponent();
			this.dbConnection = dbConnection;
			this.viewModel = new WifiImageTransferViewModel(dbConnection);
			this.networkpath = dbConnection.GetNetworkPathSavingPng();
			base.DataContext = this.viewModel;
			this.InitializeButtons();
			this.InitializeLocalizedText();
		}

		// Token: 0x060017DD RID: 6109 RVA: 0x0008FE0C File Offset: 0x0008E00C
		private void InitializeButtons()
		{
			this.Dockpanel = new DockPanel();
			this.Dockpanel.HorizontalAlignment = HorizontalAlignment.Stretch;
			this.Dockpanel.VerticalAlignment = VerticalAlignment.Stretch;
			this.viewModel.btnShowDirectories.Content = ImageConverter.ToImage("/img/table.png");
			this.viewModel.btnShowDirectories.Style = (base.FindResource("defaultButtonStyle") as Style);
			this.viewModel.btnShowDirectories.Click += this.ShowNetworkFolderList;
			this.viewModel.btnShowDirectories.Margin = new Thickness(0.0, 2.0, 10.0, 2.0);
			this.Dockpanel.Children.Add(this.viewModel.btnShowDirectories);
			DockPanel.SetDock(this.viewModel.btnShowDirectories, Dock.Left);
			Grid element = new Grid();
			this.Dockpanel.Children.Add(element);
			DockPanel.SetDock(element, Dock.Left);
		}

		// Token: 0x060017DE RID: 6110 RVA: 0x0008FF20 File Offset: 0x0008E120
		private void ShowNetworkFolderList(object sender, RoutedEventArgs e)
		{
			bool fileListVisible = this.viewModel.FileListVisible;
			if (fileListVisible)
			{
				this.viewModel.FileListVisible = false;
				bool flag = this.listTree != null && this.listTree.Items != null;
				if (flag)
				{
					this.listTree.Items.Clear();
				}
			}
			else
			{
				this.viewModel.FileListVisible = true;
				this.bgWorker = new BackgroundWorker();
				this.bgWorker.DoWork += this.bgWorker_DoWork;
				this.bgWorker.RunWorkerCompleted += this.bgWorker_RunWorkerCompleted;
				this.bgWorker.WorkerReportsProgress = true;
				this.bgWorker.RunWorkerAsync();
			}
		}

		// Token: 0x060017DF RID: 6111 RVA: 0x0008FFE0 File Offset: 0x0008E1E0
		private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			bool flag = Application.Current != null;
			if (flag)
			{
				Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
				{
					this.prog = new Progress(LocalizationManager.Localize("txtProcessing") + ".", false);
					this.prog.Show();
				}));
				this.getOverview();
			}
		}

		// Token: 0x060017E0 RID: 6112 RVA: 0x00090020 File Offset: 0x0008E220
		private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			this.listTree.Items.Add(this.dataOverview);
			bool flag = this.prog != null;
			if (flag)
			{
				this.prog.FormFadeOut.Begin();
				this.prog.Close();
				this.prog = null;
			}
			this.bgWorker.DoWork -= this.bgWorker_DoWork;
			this.bgWorker.RunWorkerCompleted -= this.bgWorker_RunWorkerCompleted;
		}

		// Token: 0x060017E1 RID: 6113 RVA: 0x000900A8 File Offset: 0x0008E2A8
		public void getOverview()
		{
			int counter = 1;
			this.dataOverview = new CtrlWifiImageTransfer.Data
			{
				name = this.StrNetwork.LocalizedText,
				topLevel = true
			};
			bool flag = Directory.Exists(this.networkpath);
			if (flag)
			{
				CtrlWifiImageTransfer.Data data = new CtrlWifiImageTransfer.Data
				{
					name = this.networkpath,
					topLevel = false,
					lowerLevel = true
				};
				string[] paths0 = Directory.GetDirectories(this.networkpath, "*", SearchOption.TopDirectoryOnly);
				for (int i = 0; i < paths0.Length; i++)
				{
					string[] paths1 = Directory.GetDirectories(paths0[i], "*", SearchOption.TopDirectoryOnly);
					CtrlWifiImageTransfer.Data data2 = new CtrlWifiImageTransfer.Data
					{
						name = this.cutoffPathForName(paths0[i]),
						topLevel = false,
						lowerLevel = true
					};
					Action <>9__0;
					for (int j = 0; j < paths1.Length; j++)
					{
						string[] directories = Directory.GetDirectories(paths1[j], "*", SearchOption.TopDirectoryOnly);
						CtrlWifiImageTransfer.Data data3 = new CtrlWifiImageTransfer.Data
						{
							name = this.cutoffPathForName(paths1[j]),
							topLevel = false,
							lowerLevel = true
						};
						for (int k = 0; k < directories.Length; k++)
						{
							string[] directories2 = Directory.GetDirectories(directories[k], "*", SearchOption.TopDirectoryOnly);
							CtrlWifiImageTransfer.Data data4 = new CtrlWifiImageTransfer.Data
							{
								name = this.cutoffPathForName(directories[k]),
								topLevel = false,
								lowerLevel = true
							};
							for (int l = 0; l < directories2.Length; l++)
							{
								data4.Items.Add(new CtrlWifiImageTransfer.Data
								{
									name = this.cutoffPathForName(directories2[l]) + this.countFiles(directories2[l]),
									topLevel = false,
									lowerLevel = true
								});
							}
							data3.Items.Add(data4);
						}
						data2.Items.Add(data3);
						Dispatcher dispatcher = Application.Current.Dispatcher;
						DispatcherPriority priority = DispatcherPriority.Background;
						Action method;
						if ((method = <>9__0) == null)
						{
							method = (<>9__0 = delegate()
							{
								bool flag2 = this.prog != null;
								if (flag2)
								{
									int counter;
									bool flag3 = counter >= paths1.Length;
									if (flag3)
									{
										counter = 1;
										this.prog.progBar.Value = 100.0;
									}
									else
									{
										this.prog.progBar.Value = (double)(100 * counter / paths0.Length);
										counter = counter;
										counter++;
									}
								}
							});
						}
						dispatcher.Invoke(priority, method);
					}
					data.Items.Add(data2);
				}
				this.dataOverview.Items.Add(data);
			}
		}

		// Token: 0x060017E2 RID: 6114 RVA: 0x0009036B File Offset: 0x0008E56B
		private void InitializeLocalizedText()
		{
			this.StrNetwork = new BoundLocalizedText("txtExternalDrive");
		}

		// Token: 0x060017E3 RID: 6115 RVA: 0x0009037F File Offset: 0x0008E57F
		private void DeregisterLocalizedText()
		{
			MainWindow.localizationManager.Deregister(this.StrNetwork);
		}

		// Token: 0x060017E4 RID: 6116 RVA: 0x00090394 File Offset: 0x0008E594
		private string cutoffPathForName(string text)
		{
			int num = text.LastIndexOf('\\');
			bool flag = num == -1;
			string result;
			if (flag)
			{
				result = text;
			}
			else
			{
				result = text.Substring(num + 1);
			}
			return result;
		}

		// Token: 0x060017E5 RID: 6117 RVA: 0x000903C4 File Offset: 0x0008E5C4
		private string countFiles(string path)
		{
			long num = 0L;
			try
			{
				bool flag = path.Contains("Reject");
				if (flag)
				{
					num = Directory.EnumerateFiles(path, "*.png").LongCount<string>();
				}
			}
			catch
			{
			}
			bool flag2 = num <= 0L;
			string result;
			if (flag2)
			{
				result = "";
			}
			else
			{
				result = " (" + num.ToString() + ")";
			}
			return result;
		}

		// Token: 0x060017E6 RID: 6118 RVA: 0x0009043C File Offset: 0x0008E63C
		private void CtrlWifiImageTransfer_Unloaded(object sender, RoutedEventArgs e)
		{
			this.viewModel.CleanUp();
			this.DeregisterLocalizedText();
			bool flag = this.listTree != null;
			if (flag)
			{
				this.listTree.Items.Clear();
				this.listTree = null;
			}
			bool flag2 = this.dataOverview != null;
			if (flag2)
			{
				this.dataOverview.Items.Clear();
				this.dataOverview = null;
			}
		}

		// Token: 0x060017E7 RID: 6119 RVA: 0x000904AC File Offset: 0x0008E6AC
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/wifi/ctrlwifiimagetransfer.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x060017E8 RID: 6120 RVA: 0x000904E4 File Offset: 0x0008E6E4
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				((CtrlWifiImageTransfer)target).Unloaded += this.CtrlWifiImageTransfer_Unloaded;
				break;
			case 2:
				this.subGrid = (Grid)target;
				break;
			case 3:
				this.listTree = (TreeView)target;
				break;
			default:
				this._contentLoaded = true;
				break;
			}
		}

		// Token: 0x04000F9A RID: 3994
		public CtrlWifiImageTransfer.Data dataOverview;

		// Token: 0x04000F9B RID: 3995
		private BackgroundWorker bgWorker;

		// Token: 0x04000F9C RID: 3996
		private WifiImageTransferViewModel viewModel;

		// Token: 0x04000F9E RID: 3998
		public DockPanel Dockpanel;

		// Token: 0x04000F9F RID: 3999
		private string networkpath = "";

		// Token: 0x04000FA1 RID: 4001
		private DBConnection dbConnection;

		// Token: 0x04000FA2 RID: 4002
		private Progress prog;

		// Token: 0x04000FA3 RID: 4003
		internal Grid subGrid;

		// Token: 0x04000FA4 RID: 4004
		internal TreeView listTree;

		// Token: 0x04000FA5 RID: 4005
		private bool _contentLoaded;

		// Token: 0x020002E1 RID: 737
		public class Data
		{
			// Token: 0x060035CB RID: 13771 RVA: 0x00152A6D File Offset: 0x00150C6D
			public Data()
			{
				this.Items = new ObservableCollection<CtrlWifiImageTransfer.Data>();
			}

			// Token: 0x1700111E RID: 4382
			// (get) Token: 0x060035CC RID: 13772 RVA: 0x00152A83 File Offset: 0x00150C83
			// (set) Token: 0x060035CD RID: 13773 RVA: 0x00152A8B File Offset: 0x00150C8B
			public string name { get; set; }

			// Token: 0x1700111F RID: 4383
			// (get) Token: 0x060035CE RID: 13774 RVA: 0x00152A94 File Offset: 0x00150C94
			// (set) Token: 0x060035CF RID: 13775 RVA: 0x00152A9C File Offset: 0x00150C9C
			public ObservableCollection<CtrlWifiImageTransfer.Data> Items { get; set; }

			// Token: 0x17001120 RID: 4384
			// (get) Token: 0x060035D0 RID: 13776 RVA: 0x00152AA5 File Offset: 0x00150CA5
			// (set) Token: 0x060035D1 RID: 13777 RVA: 0x00152AAD File Offset: 0x00150CAD
			public bool topLevel { get; set; }

			// Token: 0x17001121 RID: 4385
			// (get) Token: 0x060035D2 RID: 13778 RVA: 0x00152AB6 File Offset: 0x00150CB6
			// (set) Token: 0x060035D3 RID: 13779 RVA: 0x00152ABE File Offset: 0x00150CBE
			public bool lowerLevel { get; set; }
		}
	}
}
