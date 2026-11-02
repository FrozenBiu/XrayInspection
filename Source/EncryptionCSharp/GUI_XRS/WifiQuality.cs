using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace GUI_XRS
{
	// Token: 0x02000169 RID: 361
	public class WifiQuality : UserControl, IComponentConnector
	{
		// Token: 0x170009A9 RID: 2473
		// (get) Token: 0x06002176 RID: 8566 RVA: 0x000D90DC File Offset: 0x000D72DC
		// (set) Token: 0x06002177 RID: 8567 RVA: 0x000D90FE File Offset: 0x000D72FE
		public int SignalQuality
		{
			get
			{
				return (int)base.GetValue(WifiQuality.SignalQualityProperty);
			}
			set
			{
				base.SetValue(WifiQuality.SignalQualityProperty, value);
			}
		}

		// Token: 0x170009AA RID: 2474
		// (get) Token: 0x06002178 RID: 8568 RVA: 0x000D9113 File Offset: 0x000D7313
		// (set) Token: 0x06002179 RID: 8569 RVA: 0x000D911B File Offset: 0x000D731B
		public bool DarkerColors { get; set; }

		// Token: 0x0600217B RID: 8571 RVA: 0x000D915C File Offset: 0x000D735C
		public WifiQuality()
		{
			this.InitializeComponent();
			this.brushQualityEnoughLight = (base.FindResource("WlanQualityEnoughBrushLight") as Brush);
			this.brushQualityEnoughDark = (base.FindResource("WlanQualityEnoughBrushDark") as Brush);
			this.brushQualityNotEnough = (base.FindResource("WlanQualityNotEnoughBrush") as Brush);
		}

		// Token: 0x0600217C RID: 8572 RVA: 0x000D91BC File Offset: 0x000D73BC
		private static void SetQualityBrushes(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			bool flag = e.NewValue is int && d is WifiQuality;
			if (flag)
			{
				WifiQuality wifiQuality = (WifiQuality)d;
				int num = (int)e.NewValue;
				Brush brush = wifiQuality.DarkerColors ? wifiQuality.brushQualityEnoughDark : wifiQuality.brushQualityEnoughLight;
				wifiQuality.bdLow.Background = ((num > 0) ? brush : wifiQuality.brushQualityNotEnough);
				wifiQuality.bdMediumLow.Background = ((num >= 20) ? brush : wifiQuality.brushQualityNotEnough);
				wifiQuality.bdMedium.Background = ((num >= 40) ? brush : wifiQuality.brushQualityNotEnough);
				wifiQuality.bdMediumHigh.Background = ((num >= 60) ? brush : wifiQuality.brushQualityNotEnough);
				wifiQuality.bdHigh.Background = ((num >= 80) ? brush : wifiQuality.brushQualityNotEnough);
			}
		}

		// Token: 0x0600217D RID: 8573 RVA: 0x000D929C File Offset: 0x000D749C
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/wifi/wifiquality.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x0600217E RID: 8574 RVA: 0x000D92D4 File Offset: 0x000D74D4
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				this.bdLow = (Border)target;
				break;
			case 2:
				this.bdMediumLow = (Border)target;
				break;
			case 3:
				this.bdMedium = (Border)target;
				break;
			case 4:
				this.bdMediumHigh = (Border)target;
				break;
			case 5:
				this.bdHigh = (Border)target;
				break;
			default:
				this._contentLoaded = true;
				break;
			}
		}

		// Token: 0x040015B4 RID: 5556
		private Brush brushQualityEnoughLight;

		// Token: 0x040015B5 RID: 5557
		private Brush brushQualityEnoughDark;

		// Token: 0x040015B6 RID: 5558
		private Brush brushQualityNotEnough;

		// Token: 0x040015B7 RID: 5559
		public static readonly DependencyProperty SignalQualityProperty = DependencyProperty.Register("SignalQuality", typeof(int), typeof(WifiQuality), new PropertyMetadata(new PropertyChangedCallback(WifiQuality.SetQualityBrushes)));

		// Token: 0x040015B9 RID: 5561
		internal Border bdLow;

		// Token: 0x040015BA RID: 5562
		internal Border bdMediumLow;

		// Token: 0x040015BB RID: 5563
		internal Border bdMedium;

		// Token: 0x040015BC RID: 5564
		internal Border bdMediumHigh;

		// Token: 0x040015BD RID: 5565
		internal Border bdHigh;

		// Token: 0x040015BE RID: 5566
		private bool _contentLoaded;
	}
}
