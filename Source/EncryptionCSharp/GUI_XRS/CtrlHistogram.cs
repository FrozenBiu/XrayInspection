using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Threading;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x02000119 RID: 281
	public class CtrlHistogram : UserControl, IComponentConnector
	{
		// Token: 0x06001B2E RID: 6958 RVA: 0x000A3AEC File Offset: 0x000A1CEC
		public CtrlHistogram(ParameterHandling parHandling, ImageDataTransfer imageTransfer)
		{
			this.parHandling = parHandling;
			this.imageTransfer = imageTransfer;
			this.InitializeComponent();
			this.InitializeButtons();
			this.chart.SetHorizontalTicks(1000);
			parHandling.WriteValue("/filter/histogram/Mode/Value", "0", false);
			parHandling.WriteValue("/filter/histogram/Enable/Value", "1", false);
			imageTransfer.FrameArrived += this.imageTransfer_FrameArrived;
		}

		// Token: 0x06001B2F RID: 6959 RVA: 0x000A3B68 File Offset: 0x000A1D68
		private void InitializeButtons()
		{
			this.Buttons = new List<Button>();
			Button button = new Button();
			button.Content = ImageConverter.ToImage("/img/zoomIn.png");
			button.Click += this.btnZoomIn_Click;
			button.Style = (base.FindResource("defaultButtonStyle") as Style);
			this.Buttons.Add(button);
			Button button2 = new Button();
			button2.Content = ImageConverter.ToImage("/img/zoomOut.png");
			button2.Click += this.btnZoomOut_Click;
			button2.Style = (base.FindResource("defaultButtonStyle") as Style);
			this.Buttons.Add(button2);
			this.btnToogleHistogramm = new Button();
			this.btnToogleHistogramm.Content = "Hist";
			this.btnToogleHistogramm.Click += this.btnToogleHistogramm_Click;
			this.Buttons.Add(this.btnToogleHistogramm);
		}

		// Token: 0x06001B30 RID: 6960 RVA: 0x000A3C64 File Offset: 0x000A1E64
		private void imageTransfer_FrameArrived(object sender, ReceiveEventArgs e)
		{
			int num = 0;
			int num2 = 1;
			bool flag = e.Image.Type == ImageType.Histogram && e.Image.Width > 0 && e.Image.Height > 0 && Application.Current != null;
			if (flag)
			{
				int[] pxIndices = new int[(int)(e.Image.Width * e.Image.Height)];
				int[] nPixel = new int[(int)(e.Image.Width * e.Image.Height)];
				for (int i = 0; i < (int)(e.Image.Width * e.Image.Height); i++)
				{
					bool flag2 = i % num2 == 0;
					if (flag2)
					{
						num = 0;
						for (int j = 0; j < num2; j++)
						{
							bool flag3 = e.Image.Data.Length > i + j;
							if (flag3)
							{
								num += (int)e.Image.Data[i + j];
							}
						}
					}
					pxIndices[i] = i;
					nPixel[i] = num;
				}
				Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
				{
					this.chart.DrawGraph(pxIndices, nPixel);
					this.chart.Rescale();
				}));
			}
		}

		// Token: 0x06001B31 RID: 6961 RVA: 0x000A3DBF File Offset: 0x000A1FBF
		private void CtrlHistogram_Unloaded(object sender, RoutedEventArgs e)
		{
			this.imageTransfer.FrameArrived -= this.imageTransfer_FrameArrived;
			this.parHandling.WriteValue("/filter/histogram/Enable/Value", "0", false);
		}

		// Token: 0x06001B32 RID: 6962 RVA: 0x000A3DF4 File Offset: 0x000A1FF4
		private void btnZoomIn_Click(object sender, RoutedEventArgs e)
		{
			bool flag = this.chart.GetHorizontalTicks() > 1;
			if (flag)
			{
				this.chart.SetHorizontalTicks(this.chart.GetHorizontalTicks() / 2);
				this.chart.ZoomIn();
			}
		}

		// Token: 0x06001B33 RID: 6963 RVA: 0x000A3E3C File Offset: 0x000A203C
		private void btnZoomOut_Click(object sender, RoutedEventArgs e)
		{
			bool flag = this.chart.GetHorizontalTicks() < 20000;
			if (flag)
			{
				this.chart.SetHorizontalTicks(this.chart.GetHorizontalTicks() * 2);
				this.chart.ZoomOut();
			}
		}

		// Token: 0x06001B34 RID: 6964 RVA: 0x000A3E88 File Offset: 0x000A2088
		private void btnToogleHistogramm_Click(object sender, RoutedEventArgs e)
		{
			bool flag = (string)this.btnToogleHistogramm.Content == "Hist";
			if (flag)
			{
				this.parHandling.WriteValue("/filter/histogram/Mode/Value", "1", false);
				this.btnToogleHistogramm.Content = "Grad";
			}
			else
			{
				this.parHandling.WriteValue("/filter/histogram/Mode/Value", "0", false);
				this.btnToogleHistogramm.Content = "Hist";
			}
		}

		// Token: 0x06001B35 RID: 6965 RVA: 0x000A3F0C File Offset: 0x000A210C
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/histogram/ctrlhistogram.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x06001B36 RID: 6966 RVA: 0x000A3F44 File Offset: 0x000A2144
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			if (connectionId != 1)
			{
				if (connectionId != 2)
				{
					this._contentLoaded = true;
				}
				else
				{
					this.chart = (CtrlChart)target;
				}
			}
			else
			{
				((CtrlHistogram)target).Unloaded += this.CtrlHistogram_Unloaded;
			}
		}

		// Token: 0x0400120C RID: 4620
		private const string histogramEnablePath = "/filter/histogram/Enable/Value";

		// Token: 0x0400120D RID: 4621
		private const string histogramModePath = "/filter/histogram/Mode/Value";

		// Token: 0x0400120E RID: 4622
		private const string histogramMinGrayvaluePath = "/filter/histogram/MinGrayvalue/Value";

		// Token: 0x0400120F RID: 4623
		private const string histogramMaxGrayvaluePath = "/filter/histogram/MaxGrayvalue/Value";

		// Token: 0x04001210 RID: 4624
		private const string histogramNPixelPath = "/filter/histogram/NPixel/Value";

		// Token: 0x04001211 RID: 4625
		private const string histogramCurrentReadGrayvaluePath = "/filter/histogram/CurrentReadGrayvalue/Value";

		// Token: 0x04001212 RID: 4626
		private const string histIdentifier = "Hist";

		// Token: 0x04001213 RID: 4627
		private const string gradIdentifier = "Grad";

		// Token: 0x04001214 RID: 4628
		private ParameterHandling parHandling;

		// Token: 0x04001215 RID: 4629
		private ImageDataTransfer imageTransfer;

		// Token: 0x04001216 RID: 4630
		private Button btnToogleHistogramm;

		// Token: 0x04001217 RID: 4631
		public List<Button> Buttons;

		// Token: 0x04001218 RID: 4632
		internal CtrlChart chart;

		// Token: 0x04001219 RID: 4633
		private bool _contentLoaded;
	}
}
