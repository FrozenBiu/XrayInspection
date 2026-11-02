using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Markup;
using System.Windows.Threading;
using DatabaseConnection;
using Microsoft.Win32;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x0200022D RID: 557
	public partial class CtrlShowImage : System.Windows.Controls.UserControl
	{
		// Token: 0x17000B67 RID: 2919
		// (get) Token: 0x060029E9 RID: 10729 RVA: 0x00116700 File Offset: 0x00114900
		// (set) Token: 0x060029EA RID: 10730 RVA: 0x00116708 File Offset: 0x00114908
		public List<System.Windows.Controls.Button> Buttons { get; set; }

		// Token: 0x060029EB RID: 10731 RVA: 0x00116711 File Offset: 0x00114911
		public CtrlShowImage(ParameterHandling parHandling, StoreImageData imageArchiver)
		{
			this.Initialize(parHandling, null, imageArchiver, null, null);
		}

		// Token: 0x060029EC RID: 10732 RVA: 0x0011672E File Offset: 0x0011492E
		public CtrlShowImage(XRSImage image, StoreImageData imageArchiver, string timestamp, DBConnection dbConnection)
		{
			this.Initialize(null, image, imageArchiver, timestamp, dbConnection);
		}

		// Token: 0x060029ED RID: 10733 RVA: 0x0011674C File Offset: 0x0011494C
		private void Initialize(ParameterHandling parHandling, XRSImage image, StoreImageData imageArchiver, string timestamp, DBConnection dbConnection)
		{
			this.parHandling = parHandling;
			this.imageArchiver = imageArchiver;
			this.dbConnection = dbConnection;
			this.timestamp = timestamp;
			this.InitializeComponent();
			this.InitializeButtons();
			bool flag = image != null;
			if (flag)
			{
				this.imagebox.ViewXRSImage(image);
			}
		}

		// Token: 0x060029EE RID: 10734 RVA: 0x0011679C File Offset: 0x0011499C
		private void InitializeButtons()
		{
			this.Buttons = new List<System.Windows.Controls.Button>();
			bool flag = this.parHandling != null;
			if (flag)
			{
				System.Windows.Controls.Button button = new System.Windows.Controls.Button();
				button.Content = ImageConverter.ToImage("/img/viewimage_small.png");
				button.Style = (base.FindResource("defaultButtonStyle") as Style);
				button.Click += this.btnShow_Click;
				this.Buttons.Add(button);
			}
			else
			{
				string usbLetter = "";
				this.dbConnection.GetUsbDriveLetter(ref usbLetter);
				ButtonUSB item = new ButtonUSB(usbLetter, "/img/saveonusb_small.png", "/img/saveonusbsmall_nousb.png", new RoutedEventHandler(this.SaveToUSB));
				this.Buttons.Add(item);
			}
		}

		// Token: 0x060029EF RID: 10735 RVA: 0x00116858 File Offset: 0x00114A58
		private void btnShow_Click(object sender, RoutedEventArgs e)
		{
			Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
			bool value = openFileDialog.ShowDialog().Value;
			if (value)
			{
				bool flag = openFileDialog.FileName.EndsWith(XRSImage.fileEnding);
				if (flag)
				{
					XRSImage image = this.imageArchiver.GetImage(openFileDialog.FileName);
					this.imagebox.ViewXRSImage(image);
				}
			}
		}

		// Token: 0x060029F0 RID: 10736 RVA: 0x001168B8 File Offset: 0x00114AB8
		public void SaveToUSB(object sender, RoutedEventArgs e)
		{
			bool flag = this.imagebox.Image != null;
			if (flag)
			{
				USBSave usbsave = new USBSave();
				string usbdriveLetter = "";
				bool saveEdgePixelInPNG = false;
				this.dbConnection.GetUsbDriveLetter(ref usbdriveLetter);
				this.dbConnection.GetSaveEdgePixelInPNG(ref saveEdgePixelInPNG);
				usbsave.SaveToUSB(this.imagebox.Image, usbdriveLetter, saveEdgePixelInPNG, this.timestamp);
			}
		}

		// Token: 0x060029F1 RID: 10737 RVA: 0x00116920 File Offset: 0x00114B20
		private void btnShowAll_Click(object sender, RoutedEventArgs e)
		{
			FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
			bool flag = CtrlShowImage.lastRootFolder != null;
			if (flag)
			{
				folderBrowserDialog.SelectedPath = CtrlShowImage.lastRootFolder;
			}
			bool flag2 = folderBrowserDialog.ShowDialog() == DialogResult.OK;
			if (flag2)
			{
				CtrlShowImage.lastRootFolder = folderBrowserDialog.SelectedPath;
				string[] files = Directory.GetFiles(folderBrowserDialog.SelectedPath);
				this.currentXriFiles = (from file in files
				where file.EndsWith(XRSImage.fileEnding)
				select file).ToArray<string>();
				bool flag3 = this.currentXriFiles.Length != 0;
				if (flag3)
				{
					StoreImageData storeImages = new StoreImageData();
					List<XRSImage> images = new List<XRSImage>();
					for (int i = 0; i < this.currentXriFiles.Length; i++)
					{
						images.Add(storeImages.GetImage(this.currentXriFiles[i]));
					}
					string fileName = null;
					this.showAllRunning = true;
					Thread thread = new Thread(delegate()
					{
						while (this.showAllRunning)
						{
							for (int j = 0; j < images.Count; j++)
							{
								Stopwatch swShow = Stopwatch.StartNew();
								this.imagebox.ViewXRSImage(images[j]);
								swShow.Stop();
								Stopwatch swSave = Stopwatch.StartNew();
								storeImages.ArchiveImage(images[j], XRSImage.RotateDirection.AnticlockwiseInvertXAndLines, 50, 50, ref fileName);
								swSave.Stop();
								bool flag4 = System.Windows.Application.Current != null;
								if (flag4)
								{
									System.Windows.Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
									{
										this.txtShowTime.Text = "Time to show: " + swShow.ElapsedMilliseconds.ToString() + " ms";
										this.txtSaveTime.Text = "Time to save: " + swSave.ElapsedMilliseconds.ToString() + " ms";
									}));
								}
								Thread.Sleep(5);
							}
						}
					});
					thread.Start();
				}
			}
		}

		// Token: 0x060029F2 RID: 10738 RVA: 0x00116A4D File Offset: 0x00114C4D
		private void CtrlShowImage_Unloaded(object sender, RoutedEventArgs e)
		{
			this.showAllRunning = false;
			BindingOperations.ClearAllBindings(this);
			base.DataContext = null;
		}

		// Token: 0x04001A19 RID: 6681
		private static string lastRootFolder;

		// Token: 0x04001A1A RID: 6682
		private string[] currentXriFiles;

		// Token: 0x04001A1B RID: 6683
		private bool showAllRunning = false;

		// Token: 0x04001A1C RID: 6684
		private ParameterHandling parHandling;

		// Token: 0x04001A1D RID: 6685
		private StoreImageData imageArchiver;

		// Token: 0x04001A1E RID: 6686
		private DBConnection dbConnection;

		// Token: 0x04001A1F RID: 6687
		private string timestamp;
	}
}
