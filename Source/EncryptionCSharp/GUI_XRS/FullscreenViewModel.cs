using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x020000B7 RID: 183
	public class FullscreenViewModel : INotifyPropertyChanged
	{
		// Token: 0x1700058A RID: 1418
		// (get) Token: 0x06001241 RID: 4673 RVA: 0x0006DD2C File Offset: 0x0006BF2C
		// (set) Token: 0x06001242 RID: 4674 RVA: 0x0006DD44 File Offset: 0x0006BF44
		public Thickness TabBorderThickness
		{
			get
			{
				return this.tabBorderThickness;
			}
			set
			{
				this.tabBorderThickness = value;
				this.OnPropertyChanged("TabBorderThickness");
			}
		}

		// Token: 0x1700058B RID: 1419
		// (get) Token: 0x06001243 RID: 4675 RVA: 0x0006DD5C File Offset: 0x0006BF5C
		// (set) Token: 0x06001244 RID: 4676 RVA: 0x0006DD74 File Offset: 0x0006BF74
		public XRSImage Image
		{
			get
			{
				return this.image;
			}
			set
			{
				this.image = value;
			}
		}

		// Token: 0x1700058C RID: 1420
		// (get) Token: 0x06001245 RID: 4677 RVA: 0x0006DD80 File Offset: 0x0006BF80
		// (set) Token: 0x06001246 RID: 4678 RVA: 0x0006DD98 File Offset: 0x0006BF98
		public RegionImage SelectedRegionImage
		{
			get
			{
				return this.selectedRegionImage;
			}
			set
			{
				this.selectedRegionImage = value;
				this.OnPropertyChanged("SelectedRegionImage");
			}
		}

		// Token: 0x1700058D RID: 1421
		// (get) Token: 0x06001247 RID: 4679 RVA: 0x0006DDAE File Offset: 0x0006BFAE
		// (set) Token: 0x06001248 RID: 4680 RVA: 0x0006DDB6 File Offset: 0x0006BFB6
		public ObservableCollection<RegionImage> RegionImages { get; set; }

		// Token: 0x1400005D RID: 93
		// (add) Token: 0x06001249 RID: 4681 RVA: 0x0006DDC0 File Offset: 0x0006BFC0
		// (remove) Token: 0x0600124A RID: 4682 RVA: 0x0006DDF8 File Offset: 0x0006BFF8
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x0600124B RID: 4683 RVA: 0x0006DE30 File Offset: 0x0006C030
		public FullscreenViewModel(HistoryViewModel historyViewModel, TestcardSimViewModel testcardSimViewModel, AIViewModel aiViewModel, AIAutotrainViewModel aIAutotrainViewModel)
		{
			bool flag = historyViewModel != null;
			if (flag)
			{
				this.historyViewModel = historyViewModel;
				bool flag2 = historyViewModel.SelectedImage != null;
				if (flag2)
				{
					this.image = historyViewModel.SelectedImage.XrsImage;
				}
			}
			else
			{
				bool flag3 = testcardSimViewModel != null;
				if (flag3)
				{
					this.testcardSimViewModel = testcardSimViewModel;
					bool flag4 = testcardSimViewModel.SelectedPrievewItems != null && testcardSimViewModel.SelectedPrievewItems.Count > 0;
					if (flag4)
					{
						PreviewItem previewItem = testcardSimViewModel.SelectedPrievewItems[testcardSimViewModel.SelectedPrievewItems.Count - 1];
						bool flag5 = previewItem != null && previewItem.XrsImage != null;
						if (flag5)
						{
							this.image = previewItem.XrsImage;
						}
					}
				}
				else
				{
					bool flag6 = aiViewModel != null;
					if (flag6)
					{
						this.aiViewModel = aiViewModel;
						bool flag7 = aiViewModel.SelectedPrievewItems != null && aiViewModel.SelectedPrievewItems.Count > 0;
						if (flag7)
						{
							PreviewItem previewItem2 = aiViewModel.SelectedPrievewItems[aiViewModel.SelectedPrievewItems.Count - 1];
							bool flag8 = previewItem2 != null && previewItem2.XrsImage != null;
							if (flag8)
							{
								this.image = previewItem2.XrsImage;
							}
						}
					}
					else
					{
						bool flag9 = aIAutotrainViewModel != null;
						if (flag9)
						{
							this.aIAutotrainViewModel = aIAutotrainViewModel;
							bool flag10 = aIAutotrainViewModel.SelectedPrievewItems != null && aIAutotrainViewModel.SelectedPrievewItems.Count > 0;
							if (flag10)
							{
								PreviewItem previewItem3 = aIAutotrainViewModel.SelectedPrievewItems[aIAutotrainViewModel.SelectedPrievewItems.Count - 1];
								bool flag11 = previewItem3 != null && previewItem3.XrsImage != null;
								if (flag11)
								{
									this.image = previewItem3.XrsImage;
								}
							}
						}
					}
				}
			}
			this.tabBorderThickness = new Thickness(1.0, 1.0, 0.0, 0.0);
			this.RegionImages = new ObservableCollection<RegionImage>();
			new Thread(delegate()
			{
				this.InitializeRegionImages();
			})
			{
				Name = "InitializeRegionImages"
			}.Start();
		}

		// Token: 0x0600124C RID: 4684 RVA: 0x0006E042 File Offset: 0x0006C242
		private void InitializeRegionImages()
		{
			Thread.Sleep(200);
			Application.Current.Dispatcher.BeginInvoke(new Action(delegate()
			{
				bool flag = this.image != null;
				if (flag)
				{
					RegionImage regionImage = new RegionImage(this.image.DeepCopy());
					regionImage.XrsImage.ProductName = LocalizationManager.Localize("txtAll");
					this.RegionImages.Add(regionImage);
					this.SelectedRegionImage = regionImage;
					for (int i = 0; i < this.image.Regions.Length; i++)
					{
						bool flag2 = this.image.Regions[i].Vertex2.X > 0.0 && this.image.Regions[i].Vertex2.Y > 0.0;
						if (flag2)
						{
							XRSImage xrsimage = this.image.DeepCopy(false);
							xrsimage.Data = this.image.Data;
							xrsimage.Mask = this.image.Mask;
							xrsimage.Group = (short)(i + 1);
							xrsimage.ProductName = this.image.Regions[i].Name;
							this.RegionImages.Add(new RegionImage(xrsimage));
						}
					}
					bool flag3 = this.RegionImages.Count == 1;
					if (flag3)
					{
						this.RegionImages[0].TabHeight = 0;
						this.tabBorderThickness = new Thickness(0.0);
					}
				}
			}), new object[0]);
		}

		// Token: 0x0600124D RID: 4685 RVA: 0x0006E072 File Offset: 0x0006C272
		public void Retrain()
		{
			this.historyViewModel.Retrain();
		}

		// Token: 0x0600124E RID: 4686 RVA: 0x0006E081 File Offset: 0x0006C281
		public void RetrainRegion()
		{
			this.historyViewModel.ShowRetrainRegion();
		}

		// Token: 0x0600124F RID: 4687 RVA: 0x0006E081 File Offset: 0x0006C281
		public void RetrainGroup()
		{
			this.historyViewModel.ShowRetrainRegion();
		}

		// Token: 0x06001250 RID: 4688 RVA: 0x0006E090 File Offset: 0x0006C290
		public void CommentXri()
		{
			this.historyViewModel.CommentXri();
		}

		// Token: 0x06001251 RID: 4689 RVA: 0x0006E09F File Offset: 0x0006C29F
		public void SaveToUSB()
		{
			this.historyViewModel.SaveToUSB();
		}

		// Token: 0x06001252 RID: 4690 RVA: 0x0006E0B0 File Offset: 0x0006C2B0
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x04000C17 RID: 3095
		private HistoryViewModel historyViewModel;

		// Token: 0x04000C18 RID: 3096
		private TestcardSimViewModel testcardSimViewModel;

		// Token: 0x04000C19 RID: 3097
		private AIViewModel aiViewModel;

		// Token: 0x04000C1A RID: 3098
		private AIAutotrainViewModel aIAutotrainViewModel;

		// Token: 0x04000C1B RID: 3099
		private XRSImage image;

		// Token: 0x04000C1C RID: 3100
		private RegionImage selectedRegionImage;

		// Token: 0x04000C1D RID: 3101
		private Thickness tabBorderThickness;
	}
}
