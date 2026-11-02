using System;
using System.IO;
using System.Threading;
using DatabaseConnection;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x0200004B RID: 75
	public class LiveModeViewModel
	{
		// Token: 0x06000783 RID: 1923 RVA: 0x00031120 File Offset: 0x0002F320
		public LiveModeViewModel(DBConnection dbConnection, ParameterHandling parHandling, ImageDataTransfer imgTransfer, XRSImageBox imagebox, ProductManager productManager, StoreImageData imageArchiver)
		{
			LiveModeViewModel <>4__this = this;
			this.dbConnection = dbConnection;
			this.parHandling = parHandling;
			this.imgTransfer = imgTransfer;
			this.imagebox = imagebox;
			this.productManager = productManager;
			this.imageArchiver = imageArchiver;
			Thread thread = new Thread(delegate()
			{
				Thread.Sleep(300);
				imgTransfer.FrameArrived += <>4__this.imgTransfer_FrameArrived;
				<>4__this.ShowExistingLiveImage();
			});
			thread.Start();
		}

		// Token: 0x06000784 RID: 1924 RVA: 0x0003119F File Offset: 0x0002F39F
		public void Record()
		{
			this.parHandling.WriteValue(this.liveModePath, "1", false);
		}

		// Token: 0x06000785 RID: 1925 RVA: 0x000311BA File Offset: 0x0002F3BA
		public void CleanUp()
		{
			this.imgTransfer.FrameArrived -= this.imgTransfer_FrameArrived;
		}

		// Token: 0x06000786 RID: 1926 RVA: 0x000311D8 File Offset: 0x0002F3D8
		private void imgTransfer_FrameArrived(object sender, ReceiveEventArgs e)
		{
			bool flag = e.Image.Type != ImageType.Histogram && e.Image.Type != ImageType.MaterialCurve && e.Image.Type != ImageType.TestcardSimulation;
			if (flag)
			{
				e.Image.ProductName = this.productManager.CurrentLoadedProduct.ProductName;
				uint productNumber;
				uint.TryParse(this.productManager.CurrentLoadedProduct.ProductNumber, out productNumber);
				e.Image.ProductNumber = productNumber;
				this.currentLiveImage = e.Image;
				this.imagebox.ViewXRSImage(e.Image);
				this.SaveLiveImage(e.Image);
			}
		}

		// Token: 0x06000787 RID: 1927 RVA: 0x00031288 File Offset: 0x0002F488
		public void SaveToUSB()
		{
			bool flag = this.imagebox.Image != null;
			if (flag)
			{
				USBSave usbsave = new USBSave();
				string usbdriveLetter = "";
				bool saveEdgePixelInPNG = false;
				string timestamp = UnixTime.ToDateTime(this.imagebox.Image.Timestamp).AddMilliseconds((double)(10 * this.imagebox.Image.HundrethSecond)).ToString("yyyy-MM-dd_HH-mm-ss,ff");
				this.dbConnection.GetUsbDriveLetter(ref usbdriveLetter);
				this.dbConnection.GetSaveEdgePixelInPNG(ref saveEdgePixelInPNG);
				usbsave.SaveToUSB(this.currentLiveImage, usbdriveLetter, saveEdgePixelInPNG, timestamp);
			}
		}

		// Token: 0x06000788 RID: 1928 RVA: 0x00031328 File Offset: 0x0002F528
		private void ShowExistingLiveImage()
		{
			string str = "";
			this.dbConnection.GetCurrentProductTableName(ref str);
			string text = StoreImageData.archiveRoot + "\\" + str;
			bool flag = Directory.Exists(text);
			if (flag)
			{
				string path = text + "\\" + LiveModeViewModel.liveImageName;
				bool flag2 = File.Exists(path);
				if (flag2)
				{
					this.currentLiveImage = this.imageArchiver.GetImage(path);
					this.imagebox.ViewXRSImage(this.currentLiveImage);
				}
			}
		}

		// Token: 0x06000789 RID: 1929 RVA: 0x000313B8 File Offset: 0x0002F5B8
		private void SaveLiveImage(XRSImage liveImage)
		{
			string str = "";
			this.dbConnection.GetCurrentProductTableName(ref str);
			string text = StoreImageData.archiveRoot + "\\" + str;
			bool flag = !Directory.Exists(text);
			if (flag)
			{
				Directory.CreateDirectory(text);
			}
			string path = text + "\\" + LiveModeViewModel.liveImageName;
			liveImage.SaveToFile(path);
		}

		// Token: 0x04000529 RID: 1321
		private readonly string liveModePath = "/application/LiveMode/Value";

		// Token: 0x0400052A RID: 1322
		public static readonly string liveImageName = "LiveImage.xri";

		// Token: 0x0400052B RID: 1323
		private ParameterHandling parHandling;

		// Token: 0x0400052C RID: 1324
		private DBConnection dbConnection;

		// Token: 0x0400052D RID: 1325
		private ImageDataTransfer imgTransfer;

		// Token: 0x0400052E RID: 1326
		private XRSImageBox imagebox;

		// Token: 0x0400052F RID: 1327
		private ProductManager productManager;

		// Token: 0x04000530 RID: 1328
		private XRSImage currentLiveImage;

		// Token: 0x04000531 RID: 1329
		private StoreImageData imageArchiver;
	}
}
