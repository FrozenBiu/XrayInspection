using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using Common;
using Definitions;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x020000E8 RID: 232
	public class TestcardSimHistogramViewModel : INotifyPropertyChanged
	{
		// Token: 0x14000070 RID: 112
		// (add) Token: 0x060016A8 RID: 5800 RVA: 0x0008946C File Offset: 0x0008766C
		// (remove) Token: 0x060016A9 RID: 5801 RVA: 0x000894A4 File Offset: 0x000876A4
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x170006D4 RID: 1748
		// (get) Token: 0x060016AA RID: 5802 RVA: 0x000894D9 File Offset: 0x000876D9
		// (set) Token: 0x060016AB RID: 5803 RVA: 0x000894E1 File Offset: 0x000876E1
		public RelayCommand CleanUpCommand { get; set; }

		// Token: 0x170006D5 RID: 1749
		// (get) Token: 0x060016AC RID: 5804 RVA: 0x000894EA File Offset: 0x000876EA
		// (set) Token: 0x060016AD RID: 5805 RVA: 0x000894F2 File Offset: 0x000876F2
		public BoundLocalizedText StrInformation { get; set; }

		// Token: 0x170006D6 RID: 1750
		// (get) Token: 0x060016AE RID: 5806 RVA: 0x000894FB File Offset: 0x000876FB
		// (set) Token: 0x060016AF RID: 5807 RVA: 0x00089503 File Offset: 0x00087703
		public BoundLocalizedText StrHistogram { get; set; }

		// Token: 0x170006D7 RID: 1751
		// (get) Token: 0x060016B0 RID: 5808 RVA: 0x0008950C File Offset: 0x0008770C
		// (set) Token: 0x060016B1 RID: 5809 RVA: 0x00089514 File Offset: 0x00087714
		public BoundLocalizedText StrPixel { get; set; }

		// Token: 0x170006D8 RID: 1752
		// (get) Token: 0x060016B2 RID: 5810 RVA: 0x0008951D File Offset: 0x0008771D
		// (set) Token: 0x060016B3 RID: 5811 RVA: 0x00089525 File Offset: 0x00087725
		public BoundLocalizedText StrObject { get; set; }

		// Token: 0x170006D9 RID: 1753
		// (get) Token: 0x060016B4 RID: 5812 RVA: 0x0008952E File Offset: 0x0008772E
		// (set) Token: 0x060016B5 RID: 5813 RVA: 0x00089536 File Offset: 0x00087736
		public BoundLocalizedText StrBackground { get; set; }

		// Token: 0x170006DA RID: 1754
		// (get) Token: 0x060016B6 RID: 5814 RVA: 0x0008953F File Offset: 0x0008773F
		// (set) Token: 0x060016B7 RID: 5815 RVA: 0x00089547 File Offset: 0x00087747
		public BoundLocalizedText StrMinimum { get; set; }

		// Token: 0x170006DB RID: 1755
		// (get) Token: 0x060016B8 RID: 5816 RVA: 0x00089550 File Offset: 0x00087750
		// (set) Token: 0x060016B9 RID: 5817 RVA: 0x00089558 File Offset: 0x00087758
		public BoundLocalizedText StrMaximum { get; set; }

		// Token: 0x170006DC RID: 1756
		// (get) Token: 0x060016BA RID: 5818 RVA: 0x00089561 File Offset: 0x00087761
		// (set) Token: 0x060016BB RID: 5819 RVA: 0x00089569 File Offset: 0x00087769
		public BoundLocalizedText StrDeviation { get; set; }

		// Token: 0x170006DD RID: 1757
		// (get) Token: 0x060016BC RID: 5820 RVA: 0x00089572 File Offset: 0x00087772
		// (set) Token: 0x060016BD RID: 5821 RVA: 0x0008957A File Offset: 0x0008777A
		public BoundLocalizedText StrMeanValue { get; set; }

		// Token: 0x170006DE RID: 1758
		// (get) Token: 0x060016BE RID: 5822 RVA: 0x00089583 File Offset: 0x00087783
		// (set) Token: 0x060016BF RID: 5823 RVA: 0x0008958B File Offset: 0x0008778B
		public string StrObjectPixel { get; set; }

		// Token: 0x170006DF RID: 1759
		// (get) Token: 0x060016C0 RID: 5824 RVA: 0x00089594 File Offset: 0x00087794
		// (set) Token: 0x060016C1 RID: 5825 RVA: 0x0008959C File Offset: 0x0008779C
		public string StrBackgroundPixel { get; set; }

		// Token: 0x170006E0 RID: 1760
		// (get) Token: 0x060016C2 RID: 5826 RVA: 0x000895A8 File Offset: 0x000877A8
		// (set) Token: 0x060016C3 RID: 5827 RVA: 0x000895C0 File Offset: 0x000877C0
		public List<Point> PlotPoints
		{
			get
			{
				return this.plotPoints;
			}
			set
			{
				this.plotPoints = value;
				this.OnPropertyChanged("PlotPoints");
			}
		}

		// Token: 0x170006E1 RID: 1761
		// (get) Token: 0x060016C4 RID: 5828 RVA: 0x000895D8 File Offset: 0x000877D8
		// (set) Token: 0x060016C5 RID: 5829 RVA: 0x000895F0 File Offset: 0x000877F0
		public List<Point> BackgroundLevel
		{
			get
			{
				return this.backgroundLevel;
			}
			set
			{
				this.backgroundLevel = value;
				this.OnPropertyChanged("BackgroundLevel");
			}
		}

		// Token: 0x170006E2 RID: 1762
		// (get) Token: 0x060016C6 RID: 5830 RVA: 0x00089608 File Offset: 0x00087808
		// (set) Token: 0x060016C7 RID: 5831 RVA: 0x00089620 File Offset: 0x00087820
		public int MaxY
		{
			get
			{
				return this.maxY;
			}
			set
			{
				this.maxY = value;
				this.OnPropertyChanged("MaxY");
			}
		}

		// Token: 0x170006E3 RID: 1763
		// (get) Token: 0x060016C8 RID: 5832 RVA: 0x00089638 File Offset: 0x00087838
		// (set) Token: 0x060016C9 RID: 5833 RVA: 0x00089650 File Offset: 0x00087850
		public string PixelCount
		{
			get
			{
				return this.pixelCount;
			}
			set
			{
				bool flag = this.pixelCount != value;
				if (flag)
				{
					this.pixelCount = value;
					this.OnPropertyChanged("PixelCount");
				}
			}
		}

		// Token: 0x170006E4 RID: 1764
		// (get) Token: 0x060016CA RID: 5834 RVA: 0x00089684 File Offset: 0x00087884
		// (set) Token: 0x060016CB RID: 5835 RVA: 0x0008969C File Offset: 0x0008789C
		public string ObjectPixelCount
		{
			get
			{
				return this.objectPixelCount;
			}
			set
			{
				bool flag = this.objectPixelCount != value;
				if (flag)
				{
					this.objectPixelCount = value;
					this.OnPropertyChanged("ObjectPixelCount");
				}
			}
		}

		// Token: 0x170006E5 RID: 1765
		// (get) Token: 0x060016CC RID: 5836 RVA: 0x000896D0 File Offset: 0x000878D0
		// (set) Token: 0x060016CD RID: 5837 RVA: 0x000896E8 File Offset: 0x000878E8
		public string BackgroundPixelCount
		{
			get
			{
				return this.backgroundPixelCount;
			}
			set
			{
				bool flag = this.backgroundPixelCount != value;
				if (flag)
				{
					this.backgroundPixelCount = value;
					this.OnPropertyChanged("BackgroundPixelCount");
				}
			}
		}

		// Token: 0x170006E6 RID: 1766
		// (get) Token: 0x060016CE RID: 5838 RVA: 0x0008971C File Offset: 0x0008791C
		// (set) Token: 0x060016CF RID: 5839 RVA: 0x00089734 File Offset: 0x00087934
		public string MinGreyvalue
		{
			get
			{
				return this.minGreyvalue;
			}
			set
			{
				bool flag = this.minGreyvalue != value;
				if (flag)
				{
					this.minGreyvalue = value;
					this.OnPropertyChanged("MinGreyvalue");
				}
			}
		}

		// Token: 0x170006E7 RID: 1767
		// (get) Token: 0x060016D0 RID: 5840 RVA: 0x00089768 File Offset: 0x00087968
		// (set) Token: 0x060016D1 RID: 5841 RVA: 0x00089780 File Offset: 0x00087980
		public string MaxGreyvalue
		{
			get
			{
				return this.maxGreyvalue;
			}
			set
			{
				bool flag = this.maxGreyvalue != value;
				if (flag)
				{
					this.maxGreyvalue = value;
					this.OnPropertyChanged("MaxGreyvalue");
				}
			}
		}

		// Token: 0x170006E8 RID: 1768
		// (get) Token: 0x060016D2 RID: 5842 RVA: 0x000897B4 File Offset: 0x000879B4
		// (set) Token: 0x060016D3 RID: 5843 RVA: 0x000897CC File Offset: 0x000879CC
		public string MeanGreyvalue
		{
			get
			{
				return this.meanGreyvalue;
			}
			set
			{
				bool flag = this.meanGreyvalue != value;
				if (flag)
				{
					this.meanGreyvalue = value;
					this.OnPropertyChanged("MeanGreyvalue");
				}
			}
		}

		// Token: 0x170006E9 RID: 1769
		// (get) Token: 0x060016D4 RID: 5844 RVA: 0x00089800 File Offset: 0x00087A00
		// (set) Token: 0x060016D5 RID: 5845 RVA: 0x00089818 File Offset: 0x00087A18
		public string StandardDeviation
		{
			get
			{
				return this.standardDeviation;
			}
			set
			{
				bool flag = this.standardDeviation != value;
				if (flag)
				{
					this.standardDeviation = value;
					this.OnPropertyChanged("StandardDeviation");
				}
			}
		}

		// Token: 0x060016D6 RID: 5846 RVA: 0x0008984B File Offset: 0x00087A4B
		private void bgDrawGraphs_DoWork(object sender, DoWorkEventArgs e)
		{
			this.progressCounter = 0;
			this.SetHistogramInfo(this.previewitemforimg);
			this.drawHistogramWithSelectedImg(this.previewitemforimg);
			this.SetBackgroundLine();
		}

		// Token: 0x060016D7 RID: 5847 RVA: 0x00089878 File Offset: 0x00087A78
		private void bgDrawGraphs_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			this.progress.FormFadeOut.Begin();
			this.bgDrawGraphs.DoWork -= this.bgDrawGraphs_DoWork;
			this.bgDrawGraphs.ProgressChanged -= this.bgDrawGraphs_ProgressChanged;
			this.bgDrawGraphs.RunWorkerCompleted -= this.bgDrawGraphs_RunWorkerCompleted;
		}

		// Token: 0x060016D8 RID: 5848 RVA: 0x000898DF File Offset: 0x00087ADF
		private void bgDrawGraphs_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			this.progress.progBar.Value = (double)e.ProgressPercentage;
		}

		// Token: 0x060016D9 RID: 5849 RVA: 0x000898FC File Offset: 0x00087AFC
		public TestcardSimHistogramViewModel(List<PreviewItem> previewitems, ParameterHandling parHandling)
		{
			this.parHandling = parHandling;
			this.previewitemforimg = previewitems;
			this.RegisterLocalizedText();
			this.progress = new Progress(LocalizationManager.Localize("menuItemHistogram") + " " + LocalizationManager.Localize("msgLoadingApplication"), false);
			this.progress.Show();
			this.bgDrawGraphs = new BackgroundWorker();
			this.bgDrawGraphs.WorkerReportsProgress = true;
			this.bgDrawGraphs.WorkerSupportsCancellation = true;
			this.bgDrawGraphs.DoWork += this.bgDrawGraphs_DoWork;
			this.bgDrawGraphs.ProgressChanged += this.bgDrawGraphs_ProgressChanged;
			this.bgDrawGraphs.RunWorkerCompleted += this.bgDrawGraphs_RunWorkerCompleted;
			this.bgDrawGraphs.RunWorkerAsync();
			this.RegisterLocalizedText();
			this.CleanUpCommand = new RelayCommand(new Action<object>(this.CleanUp));
		}

		// Token: 0x060016DA RID: 5850 RVA: 0x000899F4 File Offset: 0x00087BF4
		private void drawHistogramWithSelectedImg(List<PreviewItem> previewitems)
		{
			int[] array = new int[11000];
			this.plotPoints = new List<Point>();
			Point item = default(Point);
			for (int i = 0; i < previewitems.Count; i++)
			{
				for (int j = 0; j < (int)(previewitems[i].XrsImage.Width * previewitems[i].XrsImage.Height); j++)
				{
					bool flag = previewitems[i].XrsImage.Data[j] < 11000;
					if (flag)
					{
						array[(int)previewitems[i].XrsImage.Data[j]]++;
					}
				}
				this.progressCounter++;
				this.bgDrawGraphs.ReportProgress(100 * this.progressCounter / (2 * this.previewitemforimg.Count + 1));
			}
			for (int k = 0; k < 11000; k++)
			{
				item.X = (double)k;
				item.Y = (double)array[k];
				this.plotPoints.Add(item);
			}
			this.MaxY = array.Max() + 100;
			this.PlotPoints = this.plotPoints;
		}

		// Token: 0x060016DB RID: 5851 RVA: 0x00089B44 File Offset: 0x00087D44
		private void SetBackgroundLine()
		{
			this.backgroundLevel = new List<Point>();
			int num = 10000 - 100 * Convert.ToInt32(this.parHandling.ReadPar(Definition.backroundNoisePath));
			int num2 = this.maxY / 20;
			for (int i = 0; i < this.maxY; i += num2)
			{
				this.backgroundLevel.Add(new Point((double)num, (double)i));
			}
			this.BackgroundLevel = this.backgroundLevel;
			this.bgDrawGraphs.ReportProgress(100);
		}

		// Token: 0x060016DC RID: 5852 RVA: 0x00089BD0 File Offset: 0x00087DD0
		private void SetHistogramInfo(List<PreviewItem> previewitems)
		{
			int num = 0;
			int num2 = 0;
			List<int> list = new List<int>();
			double num3 = 0.0;
			int num4 = 10000 - 100 * Convert.ToInt32(this.parHandling.ReadPar(Definition.backroundNoisePath));
			for (int i = 0; i < previewitems.Count; i++)
			{
				int num5 = (int)(previewitems[i].XrsImage.Height * previewitems[i].XrsImage.Width);
				for (int j = 0; j < num5; j++)
				{
					bool flag = (int)previewitems[i].XrsImage.Data[j] <= num4;
					if (flag)
					{
						list.Add((int)previewitems[i].XrsImage.Data[j]);
						num2++;
					}
				}
				num += num5;
				this.progressCounter++;
				this.bgDrawGraphs.ReportProgress(100 * this.progressCounter / (2 * this.previewitemforimg.Count + 1));
			}
			int num6 = Convert.ToInt32(list.Average());
			for (int k = 0; k < list.Count; k++)
			{
				int num7 = list[k] - num6;
				num3 += (double)(num7 * num7);
			}
			int value = Convert.ToInt32(Math.Sqrt(num3 / (double)list.Count));
			this.PixelCount = Convert.ToString(num);
			this.ObjectPixelCount = Convert.ToString(num2);
			this.BackgroundPixelCount = Convert.ToString(num - num2);
			this.MinGreyvalue = Convert.ToString(list.Min());
			this.MaxGreyvalue = Convert.ToString(list.Max());
			this.MeanGreyvalue = Convert.ToString(Convert.ToInt32(list.Average()));
			this.StandardDeviation = Convert.ToString(value);
		}

		// Token: 0x060016DD RID: 5853 RVA: 0x00089DC4 File Offset: 0x00087FC4
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x060016DE RID: 5854 RVA: 0x00089DF4 File Offset: 0x00087FF4
		private void RegisterLocalizedText()
		{
			this.StrHistogram = new BoundLocalizedText("menuItemHistogram");
			this.StrInformation = new BoundLocalizedText("txtInfo");
			this.StrPixel = new BoundLocalizedText("txtPixel");
			this.StrObject = new BoundLocalizedText("txtObject");
			this.StrBackground = new BoundLocalizedText("txtBackground");
			this.StrMinimum = new BoundLocalizedText("txtMinimum");
			this.StrMaximum = new BoundLocalizedText("txtMaximum");
			this.StrMeanValue = new BoundLocalizedText("txtMeanValue");
			this.StrDeviation = new BoundLocalizedText("txtStDeviation");
			this.StrObjectPixel = this.StrObject.LocalizedText + " " + this.StrPixel.LocalizedText;
			this.StrBackgroundPixel = this.StrBackground.LocalizedText + " " + this.StrPixel.LocalizedText;
		}

		// Token: 0x060016DF RID: 5855 RVA: 0x00089EEC File Offset: 0x000880EC
		public void DeregisterLocalizedText()
		{
			MainWindow.localizationManager.Deregister(this.StrHistogram);
			MainWindow.localizationManager.Deregister(this.StrInformation);
			MainWindow.localizationManager.Deregister(this.StrPixel);
			MainWindow.localizationManager.Deregister(this.StrObject);
			MainWindow.localizationManager.Deregister(this.StrBackground);
			MainWindow.localizationManager.Deregister(this.StrMinimum);
			MainWindow.localizationManager.Deregister(this.StrMaximum);
			MainWindow.localizationManager.Deregister(this.StrMeanValue);
			MainWindow.localizationManager.Deregister(this.StrDeviation);
		}

		// Token: 0x060016E0 RID: 5856 RVA: 0x00089F93 File Offset: 0x00088193
		private void CleanUp(object obj)
		{
			this.DeregisterLocalizedText();
		}

		// Token: 0x04000F00 RID: 3840
		private ParameterHandling parHandling;

		// Token: 0x04000F02 RID: 3842
		private Progress progress;

		// Token: 0x04000F03 RID: 3843
		private BackgroundWorker bgDrawGraphs;

		// Token: 0x04000F04 RID: 3844
		private List<PreviewItem> previewitemforimg;

		// Token: 0x04000F05 RID: 3845
		private List<Point> plotPoints;

		// Token: 0x04000F06 RID: 3846
		private List<Point> backgroundLevel;

		// Token: 0x04000F07 RID: 3847
		private int progressCounter;

		// Token: 0x04000F13 RID: 3859
		private int maxY;

		// Token: 0x04000F14 RID: 3860
		private string pixelCount;

		// Token: 0x04000F15 RID: 3861
		private string objectPixelCount;

		// Token: 0x04000F16 RID: 3862
		private string backgroundPixelCount;

		// Token: 0x04000F17 RID: 3863
		private string minGreyvalue;

		// Token: 0x04000F18 RID: 3864
		private string maxGreyvalue;

		// Token: 0x04000F19 RID: 3865
		private string meanGreyvalue;

		// Token: 0x04000F1A RID: 3866
		private string standardDeviation;
	}
}
