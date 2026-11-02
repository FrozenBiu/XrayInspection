using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;
using Common;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x020000B6 RID: 182
	public class LearnCanFilterViewModel : INotifyPropertyChanged
	{
		// Token: 0x17000586 RID: 1414
		// (get) Token: 0x06001229 RID: 4649 RVA: 0x0006D768 File Offset: 0x0006B968
		// (set) Token: 0x0600122A RID: 4650 RVA: 0x0006D780 File Offset: 0x0006B980
		public string AutotrainText
		{
			get
			{
				return this.autotrainText;
			}
			set
			{
				this.autotrainText = value;
				this.OnPropertyChanged("AutotrainText");
			}
		}

		// Token: 0x17000587 RID: 1415
		// (get) Token: 0x0600122B RID: 4651 RVA: 0x0006D796 File Offset: 0x0006B996
		// (set) Token: 0x0600122C RID: 4652 RVA: 0x0006D79E File Offset: 0x0006B99E
		public XRSImage CurrentImage { get; set; }

		// Token: 0x17000588 RID: 1416
		// (get) Token: 0x0600122D RID: 4653 RVA: 0x0006D7A7 File Offset: 0x0006B9A7
		// (set) Token: 0x0600122E RID: 4654 RVA: 0x0006D7AF File Offset: 0x0006B9AF
		public RelayCommand CleanUpCommand { get; set; }

		// Token: 0x17000589 RID: 1417
		// (get) Token: 0x0600122F RID: 4655 RVA: 0x0006D7B8 File Offset: 0x0006B9B8
		// (set) Token: 0x06001230 RID: 4656 RVA: 0x0006D7C0 File Offset: 0x0006B9C0
		public RelayCommand SetupCommand { get; set; }

		// Token: 0x1400005A RID: 90
		// (add) Token: 0x06001231 RID: 4657 RVA: 0x0006D7CC File Offset: 0x0006B9CC
		// (remove) Token: 0x06001232 RID: 4658 RVA: 0x0006D804 File Offset: 0x0006BA04
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action LearnFinished;

		// Token: 0x1400005B RID: 91
		// (add) Token: 0x06001233 RID: 4659 RVA: 0x0006D83C File Offset: 0x0006BA3C
		// (remove) Token: 0x06001234 RID: 4660 RVA: 0x0006D874 File Offset: 0x0006BA74
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x1400005C RID: 92
		// (add) Token: 0x06001235 RID: 4661 RVA: 0x0006D8AC File Offset: 0x0006BAAC
		// (remove) Token: 0x06001236 RID: 4662 RVA: 0x0006D8E4 File Offset: 0x0006BAE4
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<int> CrossRemovedOrAdded;

		// Token: 0x06001237 RID: 4663 RVA: 0x0006D91C File Offset: 0x0006BB1C
		public LearnCanFilterViewModel(XRSImage currentImage, XRSImageBox imagebox, ParameterHandling parHandling, ImageDataTransfer imageTransfer, int imageGroup, CanFilter canFilter)
		{
			this.imagebox = imagebox;
			this.parHandling = parHandling;
			this.imageTransfer = imageTransfer;
			this.imageGroup = imageGroup;
			this.canFilter = canFilter;
			this.SetupCommand = new RelayCommand(new Action<object>(this.Setup));
			this.CleanUpCommand = new RelayCommand(new Action<object>(this.CleanUp));
			imagebox.IncreaseInitialBrightness(5);
			imagebox.MaxCrosses = 4;
			imagebox.CrossDrawn += this.imagebox_CrossDrawn;
			this.CurrentImage = currentImage;
			this.AutotrainText = LocalizationManager.Localize("txtSelectLeftCanEdge");
		}

		// Token: 0x06001238 RID: 4664 RVA: 0x0006D9C4 File Offset: 0x0006BBC4
		private void imagebox_CrossDrawn(Point coordinates)
		{
			bool flag = this.imagebox.Crosses.Count == 1;
			if (flag)
			{
				this.AutotrainText = LocalizationManager.Localize("txtSelectUpperCanEdge");
			}
			else
			{
				bool flag2 = this.imagebox.Crosses.Count == 2;
				if (flag2)
				{
					this.AutotrainText = LocalizationManager.Localize("txtSelectRightCanEdge");
				}
				else
				{
					bool flag3 = this.imagebox.Crosses.Count == 3;
					if (flag3)
					{
						this.AutotrainText = LocalizationManager.Localize("txtSelectLowerCanEdge");
					}
					else
					{
						bool flag4 = this.imagebox.Crosses.Count == 4;
						if (flag4)
						{
							bool flag5 = this.timerLastCrossDrawn == null;
							if (flag5)
							{
								this.timerLastCrossDrawn = new DispatcherTimer();
								this.timerLastCrossDrawn.Tick += this.DoLastCrossDrawnAction;
								this.timerLastCrossDrawn.Interval = new TimeSpan(0, 0, 0, 0, 300);
							}
							this.timerLastCrossDrawn.Start();
							return;
						}
					}
				}
			}
			this.OnCrossRemovedOrAdded(this.imagebox.Crosses.Count);
		}

		// Token: 0x06001239 RID: 4665 RVA: 0x0006DAE8 File Offset: 0x0006BCE8
		private void DoLastCrossDrawnAction(object sender, EventArgs e)
		{
			this.timerLastCrossDrawn.Stop();
			bool flag = Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtOverwriteExisting"), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
			if (flag)
			{
				this.WriteCanParameters();
				this.imageTransfer.SendImage(this.CurrentImage);
				this.AutotrainText = LocalizationManager.Localize("txtAfterAutotrain");
				this.OnLearnFinished();
			}
			else
			{
				this.imagebox.RemoveLastDrawnCross();
				this.AutotrainText = LocalizationManager.Localize("txtSelectLowerCanEdge");
			}
			this.OnCrossRemovedOrAdded(this.imagebox.Crosses.Count);
		}

		// Token: 0x0600123A RID: 4666 RVA: 0x0006DB8F File Offset: 0x0006BD8F
		private void Setup(object obj)
		{
			this.imagebox.HideOverlayImage();
		}

		// Token: 0x0600123B RID: 4667 RVA: 0x0006DB9E File Offset: 0x0006BD9E
		private void CleanUp(object obj)
		{
			this.imagebox.CrossDrawn -= this.imagebox_CrossDrawn;
		}

		// Token: 0x0600123C RID: 4668 RVA: 0x0006DBBC File Offset: 0x0006BDBC
		protected virtual void OnLearnFinished()
		{
			Action learnFinished = this.LearnFinished;
			bool flag = learnFinished != null;
			if (flag)
			{
				learnFinished();
			}
		}

		// Token: 0x0600123D RID: 4669 RVA: 0x0006DBE0 File Offset: 0x0006BDE0
		private void WriteCanParameters()
		{
			string text = (this.imageGroup != 0) ? this.imageGroup.ToString() : "";
			double maxXOfCrosses = this.imagebox.GetMaxXOfCrosses();
			double minXOfCrosses = this.imagebox.GetMinXOfCrosses();
			double maxYOfCrosses = this.imagebox.GetMaxYOfCrosses();
			double minYOfCrosses = this.imagebox.GetMinYOfCrosses();
			this.canFilter.RadiusX = (int)(Math.Abs(maxXOfCrosses - minXOfCrosses) / 2.0);
			this.canFilter.CenterX = (int)((maxXOfCrosses + minXOfCrosses) / 2.0);
			this.canFilter.RadiusY = (int)(Math.Abs(maxYOfCrosses - minYOfCrosses) / 2.0);
			this.canFilter.CenterY = (int)((maxYOfCrosses + minYOfCrosses) / 2.0);
		}

		// Token: 0x0600123E RID: 4670 RVA: 0x0006DCB0 File Offset: 0x0006BEB0
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x0600123F RID: 4671 RVA: 0x0006DCDD File Offset: 0x0006BEDD
		public void RemoveLastCross()
		{
			this.imagebox.RemoveLastDrawnCross();
			this.OnCrossRemovedOrAdded(this.imagebox.Crosses.Count);
		}

		// Token: 0x06001240 RID: 4672 RVA: 0x0006DD04 File Offset: 0x0006BF04
		protected virtual void OnCrossRemovedOrAdded(int numCrosses)
		{
			Action<int> crossRemovedOrAdded = this.CrossRemovedOrAdded;
			bool flag = crossRemovedOrAdded != null;
			if (flag)
			{
				crossRemovedOrAdded(numCrosses);
			}
		}

		// Token: 0x04000C0A RID: 3082
		private XRSImageBox imagebox;

		// Token: 0x04000C0B RID: 3083
		private ParameterHandling parHandling;

		// Token: 0x04000C0C RID: 3084
		private ImageDataTransfer imageTransfer;

		// Token: 0x04000C0D RID: 3085
		private int imageGroup;

		// Token: 0x04000C0E RID: 3086
		private CanFilter canFilter;

		// Token: 0x04000C0F RID: 3087
		private string autotrainText;

		// Token: 0x04000C10 RID: 3088
		private DispatcherTimer timerLastCrossDrawn;
	}
}
