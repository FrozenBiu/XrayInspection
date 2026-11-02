using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using DatabaseConnection;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x0200004D RID: 77
	public class RecordImagesViewModel : INotifyPropertyChanged
	{
		// Token: 0x17000232 RID: 562
		// (get) Token: 0x06000799 RID: 1945 RVA: 0x00031670 File Offset: 0x0002F870
		// (set) Token: 0x0600079A RID: 1946 RVA: 0x00031688 File Offset: 0x0002F888
		public string RecordText
		{
			get
			{
				return this.recordText;
			}
			set
			{
				this.recordText = value;
				this.OnPropertyChanged("RecordText");
			}
		}

		// Token: 0x1400002B RID: 43
		// (add) Token: 0x0600079B RID: 1947 RVA: 0x000316A0 File Offset: 0x0002F8A0
		// (remove) Token: 0x0600079C RID: 1948 RVA: 0x000316D8 File Offset: 0x0002F8D8
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x1400002C RID: 44
		// (add) Token: 0x0600079D RID: 1949 RVA: 0x00031710 File Offset: 0x0002F910
		// (remove) Token: 0x0600079E RID: 1950 RVA: 0x00031748 File Offset: 0x0002F948
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action MaxImagesSaved;

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x0600079F RID: 1951 RVA: 0x00031780 File Offset: 0x0002F980
		public int NRecords
		{
			get
			{
				return this.nRecords;
			}
		}

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x060007A0 RID: 1952 RVA: 0x00031798 File Offset: 0x0002F998
		// (set) Token: 0x060007A1 RID: 1953 RVA: 0x000317A0 File Offset: 0x0002F9A0
		public VibroViewModel VibroViewModel { get; set; }

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x060007A2 RID: 1954 RVA: 0x000317A9 File Offset: 0x0002F9A9
		// (set) Token: 0x060007A3 RID: 1955 RVA: 0x000317B1 File Offset: 0x0002F9B1
		public BoundLocalizedText StrStartVibro { get; set; }

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x060007A4 RID: 1956 RVA: 0x000317BA File Offset: 0x0002F9BA
		// (set) Token: 0x060007A5 RID: 1957 RVA: 0x000317C2 File Offset: 0x0002F9C2
		public BoundLocalizedText StrStopVibro { get; set; }

		// Token: 0x060007A6 RID: 1958 RVA: 0x000317CC File Offset: 0x0002F9CC
		public RecordImagesViewModel(XRSImageBox imagebox, DBConnection dbConnection, ParameterHandling parHandling, ImageDataTransfer imageTransfer, MachineState machineState, ProductManager productManager, Vibro vibro, AccessRightsManager accessManager)
		{
			RecordImagesViewModel <>4__this = this;
			this.imagebox = imagebox;
			this.parHandling = parHandling;
			this.dbConnection = dbConnection;
			this.imageTransfer = imageTransfer;
			this.machineState = machineState;
			this.productManager = productManager;
			this.oldState = machineState.State;
			machineState.State = MachineStates.ImageAcquiring;
			this.RegisterLocalizedText();
			this.nRecords = this.GetNRecords();
			this.CreateLocalizedHeader(true);
			this.VibroViewModel = new VibroViewModel(vibro, accessManager, parHandling, false);
			this.SetImageProcessingParameters(false);
			parHandling.WriteValue("/filter/acquisition/MaxLines/Value", 2000.ToString(), false);
			Thread thread = new Thread(delegate()
			{
				Thread.Sleep(300);
				imageTransfer.FrameArrived += <>4__this.imageTransfer_FrameArrived;
			});
			thread.Start();
		}

		// Token: 0x060007A7 RID: 1959 RVA: 0x000318A8 File Offset: 0x0002FAA8
		private void RegisterLocalizedText()
		{
			this.preRecordText = LocalizationManager.Localize("txtRecordImagesStarted");
			this.postRecordText = LocalizationManager.Localize("txtImagesSaved");
			this.StrStartVibro = new BoundLocalizedText("msgStartVibro");
			this.StrStopVibro = new BoundLocalizedText("msgStopVibro");
		}

		// Token: 0x060007A8 RID: 1960 RVA: 0x000318F8 File Offset: 0x0002FAF8
		private void DeregisterLocalizedText()
		{
			MainWindow.localizationManager.Deregister(this.StrStartVibro);
			MainWindow.localizationManager.Deregister(this.StrStopVibro);
		}

		// Token: 0x060007A9 RID: 1961 RVA: 0x00031920 File Offset: 0x0002FB20
		private void CreateLocalizedHeader(bool plural)
		{
			string text = string.Concat(new string[]
			{
				this.preRecordText,
				" ",
				this.nRecords.ToString(),
				" ",
				this.postRecordText
			});
			int num = text.IndexOf('(');
			int num2 = text.IndexOf(')');
			bool flag = num >= 0 && num2 > num;
			if (flag)
			{
				if (plural)
				{
					this.RecordText = text.Remove(num, 1).Remove(num2 - 1, 1);
				}
				else
				{
					this.RecordText = text.Remove(num, num2 - num + 1);
				}
			}
			else
			{
				this.RecordText = text;
			}
		}

		// Token: 0x060007AA RID: 1962 RVA: 0x000319CC File Offset: 0x0002FBCC
		public void imageTransfer_FrameArrived(object sender, ReceiveEventArgs e)
		{
			XRSImage xrsimage = e.Image.DeepCopy();
			xrsimage.State = 0;
			xrsimage.ContaminationCount = 0;
			xrsimage.Weight = 0f;
			xrsimage.ObjectCount = 0;
			for (int i = 0; i < xrsimage.Mask.Length; i++)
			{
				xrsimage.Mask[i] = 0;
			}
			bool flag = this.SaveImage(xrsimage);
			this.imagebox.ViewXRSImage(xrsimage);
			this.nRecords++;
			this.CreateLocalizedHeader(this.nRecords != 1);
			bool flag2 = flag;
			if (flag2)
			{
				this.OnMaxImagesSaved();
			}
		}

		// Token: 0x060007AB RID: 1963 RVA: 0x00031A70 File Offset: 0x0002FC70
		public void CleanUp()
		{
			this.imageTransfer.FrameArrived -= this.imageTransfer_FrameArrived;
			this.DeregisterLocalizedText();
			this.VibroViewModel.Vibro.VibroManualModeByGUI = false;
			bool flag = this.machineState.State == MachineStates.ImageAcquiring;
			if (flag)
			{
				this.SetImageProcessingParameters(true);
				this.machineState.State = this.oldState;
			}
		}

		// Token: 0x060007AC RID: 1964 RVA: 0x00031AE0 File Offset: 0x0002FCE0
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x060007AD RID: 1965 RVA: 0x00031B10 File Offset: 0x0002FD10
		protected virtual void OnMaxImagesSaved()
		{
			Action maxImagesSaved = this.MaxImagesSaved;
			bool flag = maxImagesSaved != null;
			if (flag)
			{
				maxImagesSaved();
			}
		}

		// Token: 0x060007AE RID: 1966 RVA: 0x00031B34 File Offset: 0x0002FD34
		private bool SaveImage(XRSImage image)
		{
			string str = "";
			this.dbConnection.GetCurrentProductTableName(ref str);
			string text = StoreImageData.archiveRoot + "\\" + str;
			bool flag = !Directory.Exists(text);
			if (flag)
			{
				Directory.CreateDirectory(text);
			}
			image.SaveToFile(text + "\\" + this.CreateFilename(image) + XRSImage.fileEnding);
			return (from fileName in Directory.GetFiles(text)
			where fileName.EndsWith(".xri")
			select fileName).Count<string>() >= RecordImagesViewModel.maxProductImages;
		}

		// Token: 0x060007AF RID: 1967 RVA: 0x00031BD8 File Offset: 0x0002FDD8
		private string CreateFilename(XRSImage image)
		{
			return UnixTime.ToDateTime(image.Timestamp).AddMilliseconds((double)(10 * image.HundrethSecond)).ToString("yyyy-MM-dd_HH-mm-ss,ff");
		}

		// Token: 0x060007B0 RID: 1968 RVA: 0x00031C14 File Offset: 0x0002FE14
		private int GetNRecords()
		{
			int result = 0;
			string str = "";
			this.dbConnection.GetCurrentProductTableName(ref str);
			string path = StoreImageData.archiveRoot + "\\" + str;
			bool flag = Directory.Exists(path);
			if (flag)
			{
				result = (from fileName in Directory.GetFiles(path)
				where fileName.EndsWith(".xri") && !fileName.EndsWith(LiveModeViewModel.liveImageName)
				select fileName).Count<string>();
			}
			return result;
		}

		// Token: 0x060007B1 RID: 1969 RVA: 0x00031C8C File Offset: 0x0002FE8C
		private void SetImageProcessingParameters(bool resetToBulk)
		{
			if (resetToBulk)
			{
				int minMaxLines = BulkMode.GetMinMaxLines(this.dbConnection);
				this.parHandling.WriteValue("/filter/acquisition/SendImage/Value", "0", false);
				this.parHandling.WriteValue("/filter/acquisition/SetToProductWidth/Value", "0", false);
				this.parHandling.WriteValue("/filter/acquisition/ConnectImages/Value", "0", false);
				this.parHandling.WriteValue("/filter/acquisition/AddedLeftPixel/Value", "0", false);
				this.parHandling.WriteValue("/filter/acquisition/AddedRightPixel/Value", "0", false);
				this.parHandling.WriteValue("/filter/acquisition/AddedFrontLines/Value", "0", false);
				this.parHandling.WriteValue("/filter/acquisition/AddedBackLines/Value", "0", false);
				this.parHandling.WriteValue("/filter/acquisition/MaxLines/Value", minMaxLines.ToString(), false);
				this.parHandling.WriteValue("/filter/acquisition/MinLines/Value", minMaxLines.ToString(), false);
				this.parHandling.WriteValue("/filter/acquisition/ContinuousMode/Value", "1", false);
				this.parHandling.WriteValue("/filter/acquisition/ContinuousModeFiltering/Value", "1", false);
				this.parHandling.WriteValue("/hardware/svt/ValvesEnable/Value", "1", false);
			}
			else
			{
				this.parHandling.WriteValue("/hardware/svt/ValvesEnable/Value", "0", false);
				this.parHandling.WriteValue("/filter/acquisition/SendImage/Value", "1", false);
				this.parHandling.WriteValue("/filter/acquisition/ConnectImages/Value", "1", false);
				this.parHandling.WriteValue("/filter/acquisition/SetToProductWidth/Value", "1", false);
				this.parHandling.WriteValue("/filter/acquisition/ContinuousModeFiltering/Value", "0", false);
				this.parHandling.WriteValue("/filter/acquisition/ContinuousMode/Value", "0", false);
				this.parHandling.WriteValue("/filter/acquisition/MaxLines/Value", "2000", false);
				this.parHandling.WriteValue("/filter/acquisition/MinLines/Value", "1", false);
				this.parHandling.WriteValue("/filter/acquisition/AddedLeftPixel/Value", "5", false);
				this.parHandling.WriteValue("/filter/acquisition/AddedRightPixel/Value", "5", false);
				this.parHandling.WriteValue("/filter/acquisition/AddedFrontLines/Value", "5", false);
				this.parHandling.WriteValue("/filter/acquisition/AddedBackLines/Value", "5", false);
			}
		}

		// Token: 0x04000536 RID: 1334
		private const int maxLines = 2000;

		// Token: 0x04000537 RID: 1335
		public static readonly int maxProductImages = 10;

		// Token: 0x04000538 RID: 1336
		private string recordText;

		// Token: 0x04000539 RID: 1337
		private string preRecordText;

		// Token: 0x0400053A RID: 1338
		private string postRecordText;

		// Token: 0x0400053B RID: 1339
		private ParameterHandling parHandling;

		// Token: 0x0400053C RID: 1340
		private ImageDataTransfer imageTransfer;

		// Token: 0x0400053D RID: 1341
		private DBConnection dbConnection;

		// Token: 0x0400053E RID: 1342
		private MachineState machineState;

		// Token: 0x0400053F RID: 1343
		private MachineStates oldState;

		// Token: 0x04000540 RID: 1344
		private ProductManager productManager;

		// Token: 0x04000541 RID: 1345
		private XRSImageBox imagebox;

		// Token: 0x04000542 RID: 1346
		private int nRecords;
	}
}
