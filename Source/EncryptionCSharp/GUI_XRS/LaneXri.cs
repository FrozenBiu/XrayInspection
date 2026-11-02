using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using DatabaseConnection;
using Definitions;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x02000126 RID: 294
	public class LaneXri : INotifyPropertyChanged
	{
		// Token: 0x1400008F RID: 143
		// (add) Token: 0x06001C6E RID: 7278 RVA: 0x000B6330 File Offset: 0x000B4530
		// (remove) Token: 0x06001C6F RID: 7279 RVA: 0x000B6368 File Offset: 0x000B4568
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x1700085D RID: 2141
		// (get) Token: 0x06001C70 RID: 7280 RVA: 0x000B63A0 File Offset: 0x000B45A0
		// (set) Token: 0x06001C71 RID: 7281 RVA: 0x000B63B8 File Offset: 0x000B45B8
		public Lane Lane
		{
			get
			{
				return this.lane;
			}
			set
			{
				bool flag = this.lane != value;
				if (flag)
				{
					this.lane = value;
					this.OnPropertyChanged("Lane");
				}
			}
		}

		// Token: 0x1700085E RID: 2142
		// (get) Token: 0x06001C72 RID: 7282 RVA: 0x000B63EC File Offset: 0x000B45EC
		// (set) Token: 0x06001C73 RID: 7283 RVA: 0x000B6404 File Offset: 0x000B4604
		public XRSImage Image
		{
			get
			{
				return this.image;
			}
			set
			{
				bool flag = this.image != value;
				if (flag)
				{
					this.image = value;
					this.OnPropertyChanged("Image");
				}
			}
		}

		// Token: 0x1700085F RID: 2143
		// (get) Token: 0x06001C74 RID: 7284 RVA: 0x000B6438 File Offset: 0x000B4638
		// (set) Token: 0x06001C75 RID: 7285 RVA: 0x000B6450 File Offset: 0x000B4650
		public List<XRSImage> Images
		{
			get
			{
				return this.images;
			}
			set
			{
				bool flag = this.images != value;
				if (flag)
				{
					this.images = value;
					this.OnPropertyChanged("Images");
				}
			}
		}

		// Token: 0x17000860 RID: 2144
		// (get) Token: 0x06001C76 RID: 7286 RVA: 0x000B6484 File Offset: 0x000B4684
		// (set) Token: 0x06001C77 RID: 7287 RVA: 0x000B649C File Offset: 0x000B469C
		public int TabHeight
		{
			get
			{
				return this.tabHeight;
			}
			set
			{
				this.tabHeight = value;
				this.OnPropertyChanged("TabHeight");
			}
		}

		// Token: 0x17000861 RID: 2145
		// (get) Token: 0x06001C78 RID: 7288 RVA: 0x000B64B4 File Offset: 0x000B46B4
		// (set) Token: 0x06001C79 RID: 7289 RVA: 0x000B64CC File Offset: 0x000B46CC
		public bool Active
		{
			get
			{
				return this.active;
			}
			set
			{
				this.active = value;
				this.OnPropertyChanged("Active");
			}
		}

		// Token: 0x17000862 RID: 2146
		// (get) Token: 0x06001C7A RID: 7290 RVA: 0x000B64E4 File Offset: 0x000B46E4
		// (set) Token: 0x06001C7B RID: 7291 RVA: 0x000B64FC File Offset: 0x000B46FC
		public int Group
		{
			get
			{
				return this.group;
			}
			set
			{
				this.group = value;
				this.OnPropertyChanged("Group");
				bool flag = this.Counter != null;
				if (flag)
				{
					this.Counter.ImageGroup = value;
				}
			}
		}

		// Token: 0x17000863 RID: 2147
		// (get) Token: 0x06001C7C RID: 7292 RVA: 0x000B6538 File Offset: 0x000B4738
		// (set) Token: 0x06001C7D RID: 7293 RVA: 0x000B6550 File Offset: 0x000B4750
		public string LaneName
		{
			get
			{
				return this.laneName;
			}
			set
			{
				this.laneName = value;
				this.OnPropertyChanged("LaneName");
			}
		}

		// Token: 0x17000864 RID: 2148
		// (get) Token: 0x06001C7E RID: 7294 RVA: 0x000B6568 File Offset: 0x000B4768
		// (set) Token: 0x06001C7F RID: 7295 RVA: 0x000B6580 File Offset: 0x000B4780
		public string LaneNameCompleteMultiProduct
		{
			get
			{
				return this.laneNameCompleteMultiProduct;
			}
			set
			{
				this.laneNameCompleteMultiProduct = value;
				this.OnPropertyChanged("LaneNameCompleteMultiProduct");
			}
		}

		// Token: 0x17000865 RID: 2149
		// (get) Token: 0x06001C80 RID: 7296 RVA: 0x000B6596 File Offset: 0x000B4796
		// (set) Token: 0x06001C81 RID: 7297 RVA: 0x000B659E File Offset: 0x000B479E
		public XRSImage TempImage { get; set; }

		// Token: 0x17000866 RID: 2150
		// (get) Token: 0x06001C82 RID: 7298 RVA: 0x000B65A7 File Offset: 0x000B47A7
		// (set) Token: 0x06001C83 RID: 7299 RVA: 0x000B65AF File Offset: 0x000B47AF
		public Counters Counter { get; set; }

		// Token: 0x17000867 RID: 2151
		// (get) Token: 0x06001C84 RID: 7300 RVA: 0x000B65B8 File Offset: 0x000B47B8
		// (set) Token: 0x06001C85 RID: 7301 RVA: 0x000B65D0 File Offset: 0x000B47D0
		public BoundLocalizedText StrThroughput
		{
			get
			{
				return this.strThroughput;
			}
			set
			{
				this.strThroughput = value;
				this.OnPropertyChanged("StrThroughput");
			}
		}

		// Token: 0x17000868 RID: 2152
		// (get) Token: 0x06001C86 RID: 7302 RVA: 0x000B65E6 File Offset: 0x000B47E6
		// (set) Token: 0x06001C87 RID: 7303 RVA: 0x000B65EE File Offset: 0x000B47EE
		public BoundLocalizedText StrReject { get; set; }

		// Token: 0x06001C88 RID: 7304 RVA: 0x000B65F8 File Offset: 0x000B47F8
		public LaneXri(Lane lane, ImageDataTransfer imageTransfer, ParameterHandling parHandling, DBConnection dbConnection, AccessRightsManager accessManager, ProductManager productManager, MeanPixelLogger meanPixelLogger, MachineState machineState, StoreImageData imageArchiver, bool isDualLane, TestcardSimService testcardSimService, SetFooterMainWindow setFooterMainWindow, Sensitivity bulksensitvitiy)
		{
			this.lane = lane;
			this.dbConnection = dbConnection;
			this.RegisterLocalizedText();
			this.image = new XRSImage();
			bool flag = !isDualLane;
			if (flag)
			{
				this.Counter = new Counters(imageTransfer, parHandling, dbConnection, accessManager, productManager, meanPixelLogger, machineState, lane, this.group, imageArchiver, testcardSimService, setFooterMainWindow, bulksensitvitiy);
			}
			this.UpdateLaneName();
		}

		// Token: 0x06001C89 RID: 7305 RVA: 0x000B6680 File Offset: 0x000B4880
		private void RegisterLocalizedText()
		{
			this.strThroughputDefault = new BoundLocalizedText("txtThroughput");
			this.strThroughputPerMin = new BoundLocalizedText("txtThroughputPerMin");
			this.strImages = new BoundLocalizedText("txtImages");
			this.StrReject = new BoundLocalizedText("txtReject");
			MachineMode machineMode = MachineMode.Standard;
			bool flag = false;
			this.dbConnection.GetMachineMode(ref machineMode);
			this.dbConnection.GetThroughputCounterPerMin(ref flag);
			bool flag2 = machineMode == MachineMode.Bulk;
			if (flag2)
			{
				this.StrThroughput = this.strImages;
			}
			else
			{
				bool flag3 = flag;
				if (flag3)
				{
					this.StrThroughput = this.strThroughputPerMin;
				}
				else
				{
					this.StrThroughput = this.strThroughputDefault;
				}
			}
		}

		// Token: 0x06001C8A RID: 7306 RVA: 0x000B6729 File Offset: 0x000B4929
		public void resetImagesTemp()
		{
			this.imagesTemp = new List<XRSImage>();
		}

		// Token: 0x06001C8B RID: 7307 RVA: 0x000B6738 File Offset: 0x000B4938
		public void ViewImage(XRSImage image, bool userLoggedIn, bool bulkMode, bool showBigImage)
		{
			bool flag = (this.lane == Lane.All || this.lane == image.Lane) && (this.group == 0 || this.group == (int)image.Group) && !bulkMode;
			if (flag)
			{
				this.Image = image;
			}
			else if (bulkMode)
			{
				if (showBigImage)
				{
					this.imagesTemp.Add(image);
					bool flag2 = this.imagesTemp.Count == 10;
					if (flag2)
					{
						this.Images = this.imagesTemp;
					}
				}
				else
				{
					this.Image = image;
				}
			}
		}

		// Token: 0x06001C8C RID: 7308 RVA: 0x000B67D4 File Offset: 0x000B49D4
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x06001C8D RID: 7309 RVA: 0x000B6804 File Offset: 0x000B4A04
		public void UpdateLaneName()
		{
			switch (this.lane)
			{
			case Lane.All:
				this.LaneName = LocalizationManager.Localize("txtAll");
				break;
			case Lane.Lane1:
				this.LaneName = LocalizationManager.Localize("txtLane") + " 1";
				break;
			case Lane.Lane2:
				this.LaneName = LocalizationManager.Localize("txtLane") + " 2";
				break;
			case Lane.Lane3:
				this.LaneName = LocalizationManager.Localize("txtLane") + " 3";
				break;
			case Lane.Lane4:
				this.LaneName = LocalizationManager.Localize("txtLane") + " 4";
				break;
			}
		}

		// Token: 0x06001C8E RID: 7310 RVA: 0x000B68C4 File Offset: 0x000B4AC4
		public void UpdateThroughputString(MachineMode machineMode, bool troughputPerMin)
		{
			bool flag = machineMode == MachineMode.Bulk;
			if (flag)
			{
				this.StrThroughput = this.strImages;
			}
			else if (troughputPerMin)
			{
				this.StrThroughput = this.strThroughputPerMin;
			}
			else
			{
				this.StrThroughput = this.strThroughputDefault;
			}
		}

		// Token: 0x040012C9 RID: 4809
		private Lane lane;

		// Token: 0x040012CA RID: 4810
		private XRSImage image;

		// Token: 0x040012CB RID: 4811
		private int tabHeight;

		// Token: 0x040012CC RID: 4812
		private bool active;

		// Token: 0x040012CD RID: 4813
		private int group;

		// Token: 0x040012CE RID: 4814
		private string laneName;

		// Token: 0x040012CF RID: 4815
		private string laneNameCompleteMultiProduct;

		// Token: 0x040012D0 RID: 4816
		private DBConnection dbConnection;

		// Token: 0x040012D1 RID: 4817
		private BoundLocalizedText strThroughput;

		// Token: 0x040012D2 RID: 4818
		private BoundLocalizedText strThroughputDefault;

		// Token: 0x040012D3 RID: 4819
		private BoundLocalizedText strThroughputPerMin;

		// Token: 0x040012D4 RID: 4820
		private BoundLocalizedText strImages;

		// Token: 0x040012D6 RID: 4822
		private List<XRSImage> images = new List<XRSImage>();

		// Token: 0x040012DA RID: 4826
		private List<XRSImage> imagesTemp = new List<XRSImage>();
	}
}
