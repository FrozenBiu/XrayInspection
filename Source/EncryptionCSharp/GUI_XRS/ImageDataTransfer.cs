using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading;
using Definitions;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x02000100 RID: 256
	public class ImageDataTransfer : Network
	{
		// Token: 0x1400007A RID: 122
		// (add) Token: 0x0600187D RID: 6269 RVA: 0x00092F50 File Offset: 0x00091150
		// (remove) Token: 0x0600187E RID: 6270 RVA: 0x00092F88 File Offset: 0x00091188
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event EventHandler<ReceiveEventArgs> FrameArrived;

		// Token: 0x1400007B RID: 123
		// (add) Token: 0x0600187F RID: 6271 RVA: 0x00092FC0 File Offset: 0x000911C0
		// (remove) Token: 0x06001880 RID: 6272 RVA: 0x00092FF8 File Offset: 0x000911F8
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<string> ImageDataError;

		// Token: 0x06001881 RID: 6273 RVA: 0x0009302D File Offset: 0x0009122D
		public ImageDataTransfer(StoreImageData imageArchiver) : this(Definition.defaultElinosIP, imageArchiver)
		{
		}

		// Token: 0x06001882 RID: 6274 RVA: 0x00093040 File Offset: 0x00091240
		public ImageDataTransfer(string ipAddress, StoreImageData imageArchiver) : base(ipAddress, ImageDataTransfer.imagePort, "ImageDataTransfer")
		{
			this.imageArchiver = imageArchiver;
			base.Connected += this.StartGetImageData;
			base.SetReceiveTimeout(15000);
		}

		// Token: 0x06001883 RID: 6275 RVA: 0x00093098 File Offset: 0x00091298
		private void TestConnection()
		{
			XRSImage image = XRSImage.CreateTestImage();
			while (this.testImageThreadRunning)
			{
				this.SendImage(image);
				Thread.Sleep(this.testFramePeriod);
			}
		}

		// Token: 0x06001884 RID: 6276 RVA: 0x000930D0 File Offset: 0x000912D0
		public void StartConnectionTest()
		{
			new Thread(new ThreadStart(this.TestConnection))
			{
				Name = "TestImageConnection"
			}.Start();
		}

		// Token: 0x06001885 RID: 6277 RVA: 0x00093103 File Offset: 0x00091303
		public void StopConnectionTest()
		{
			this.testImageThreadRunning = false;
		}

		// Token: 0x06001886 RID: 6278 RVA: 0x00093110 File Offset: 0x00091310
		protected virtual void OnFrameArrived(XRSImage image)
		{
			EventHandler<ReceiveEventArgs> frameArrived = this.FrameArrived;
			bool flag = frameArrived != null;
			if (flag)
			{
				frameArrived(this, new ReceiveEventArgs(image));
			}
		}

		// Token: 0x06001887 RID: 6279 RVA: 0x00093140 File Offset: 0x00091340
		public void StartGetImageData()
		{
			new Thread(new ThreadStart(this.GetImageData))
			{
				Name = "GetImageData"
			}.Start();
		}

		// Token: 0x06001888 RID: 6280 RVA: 0x00093174 File Offset: 0x00091374
		public void GetImageData()
		{
			while (base.ConnectionEstablished)
			{
				try
				{
					byte[] array = base.ReceiveBytes(0, 1);
					bool flag = this.recvImage != null && (this.recvImage.Width != BitConverter.ToInt16(this.bufferHeader, 1) || this.recvImage.Height != BitConverter.ToInt16(this.bufferHeader, 3));
					if (flag)
					{
						this.bufferRawImageData = null;
					}
					bool flag2 = array != null && array.Length >= 1;
					if (flag2)
					{
						this.bufferHeader = base.ReceiveBytes(1, ((int)array[0] < XRSImage.versionHeaderLength1000) ? (XRSImage.headerSizeV1 - 1) : (XRSImage.headerSizeV5 - 1));
						bool flag3 = this.bufferHeader != null && this.bufferHeader.Length >= 1;
						if (flag3)
						{
							this.bufferHeader[0] = array[0];
							this.recvImage = new XRSImage(this.bufferHeader);
							bool flag4 = this.recvImage.Width > 0 && this.recvImage.Height > 0;
							if (flag4)
							{
								bool flag5 = this.bufferRawImageData == null;
								if (flag5)
								{
									this.bufferRawImageData = new byte[(int)(this.recvImage.Height * 2 * this.recvImage.Width)];
								}
								this.bufferRawImageData = base.ReceiveBytes(0, (int)(this.recvImage.Height * 2 * this.recvImage.Width));
								this.recvImage.Data = ArrayConverter.ByteArrayToShortArray(this.bufferRawImageData);
								this.recvImage.Mask = base.ReceiveBytes(0, this.recvImage.Mask.Length);
								bool flag6 = this.recvImage.Type != ImageType.ConnectionTest;
								if (flag6)
								{
									this.OnFrameArrived(this.recvImage);
								}
							}
						}
					}
					this.recvImage = null;
				}
				catch (Exception ex)
				{
					this.OnImageDataError(ex.Message);
				}
			}
		}

		// Token: 0x06001889 RID: 6281 RVA: 0x0009338C File Offset: 0x0009158C
		public bool SendImage(string path)
		{
			bool result;
			try
			{
				XRSImage image = this.imageArchiver.GetImage(path);
				bool flag = image.Type == ImageType.TrainAI;
				if (flag)
				{
					image.Type = ImageType.Demo;
				}
				result = this.SendImage(image);
			}
			catch
			{
				result = false;
			}
			return result;
		}

		// Token: 0x0600188A RID: 6282 RVA: 0x000933E0 File Offset: 0x000915E0
		public bool SendImage(XRSImage image)
		{
			bool result;
			lock (this)
			{
				try
				{
					base.SendBytes(image.CreateHeader(image.Version), SocketFlags.None);
					base.SendBytes(ArrayConverter.ShortArrayToByteArray(image.Data), SocketFlags.None);
					bool flag2 = image.Type == ImageType.TestcardSimulation;
					if (flag2)
					{
						base.SendBytes(image.Mask, SocketFlags.None);
					}
					result = true;
				}
				catch
				{
					result = false;
				}
			}
			return result;
		}

		// Token: 0x0600188B RID: 6283 RVA: 0x00093474 File Offset: 0x00091674
		protected virtual void OnImageDataError(string message)
		{
			Action<string> imageDataError = this.ImageDataError;
			bool flag = imageDataError != null;
			if (flag)
			{
				imageDataError(message);
			}
		}

		// Token: 0x04001000 RID: 4096
		private readonly int testFramePeriod = 4000;

		// Token: 0x04001001 RID: 4097
		public static readonly int imagePort = 1347;

		// Token: 0x04001002 RID: 4098
		private byte[] bufferHeader;

		// Token: 0x04001003 RID: 4099
		private XRSImage recvImage;

		// Token: 0x04001004 RID: 4100
		private byte[] bufferRawImageData;

		// Token: 0x04001005 RID: 4101
		private bool testImageThreadRunning = true;

		// Token: 0x04001006 RID: 4102
		private StoreImageData imageArchiver;
	}
}
