using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.Charts;
using Microsoft.Research.DynamicDataDisplay.Charts.Axes;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using Microsoft.Research.DynamicDataDisplay.ViewportRestrictions;

namespace GUI_XRS
{
	// Token: 0x0200010F RID: 271
	public class CtrlChart : UserControl, INotifyPropertyChanged, IComponentConnector
	{
		// Token: 0x17000799 RID: 1945
		// (get) Token: 0x06001967 RID: 6503 RVA: 0x00095E9C File Offset: 0x0009409C
		// (set) Token: 0x06001968 RID: 6504 RVA: 0x00095EA4 File Offset: 0x000940A4
		public bool SemiTransparentBorder { get; set; }

		// Token: 0x1700079A RID: 1946
		// (get) Token: 0x06001969 RID: 6505 RVA: 0x00095EB0 File Offset: 0x000940B0
		// (set) Token: 0x0600196A RID: 6506 RVA: 0x00095EC8 File Offset: 0x000940C8
		public CompositeDataSource DateTimeSource
		{
			get
			{
				return this.dateTimeSource;
			}
			set
			{
				this.dateTimeSource = value;
				this.OnPropertyChanged("DateTimeSource");
			}
		}

		// Token: 0x1700079B RID: 1947
		// (get) Token: 0x0600196B RID: 6507 RVA: 0x00095EE0 File Offset: 0x000940E0
		// (set) Token: 0x0600196C RID: 6508 RVA: 0x00095EF8 File Offset: 0x000940F8
		public CompositeDataSource IntSource
		{
			get
			{
				return this.intSource;
			}
			set
			{
				this.intSource = value;
				this.OnPropertyChanged("IntSource");
			}
		}

		// Token: 0x1700079C RID: 1948
		// (get) Token: 0x0600196D RID: 6509 RVA: 0x00095F10 File Offset: 0x00094110
		// (set) Token: 0x0600196E RID: 6510 RVA: 0x00095F28 File Offset: 0x00094128
		public CompositeDataSource IgnoreLeftSource
		{
			get
			{
				return this.ignoreLeftSource;
			}
			set
			{
				this.ignoreLeftSource = value;
				this.OnPropertyChanged("IgnoreLeftSource");
			}
		}

		// Token: 0x1700079D RID: 1949
		// (get) Token: 0x0600196F RID: 6511 RVA: 0x00095F40 File Offset: 0x00094140
		// (set) Token: 0x06001970 RID: 6512 RVA: 0x00095F58 File Offset: 0x00094158
		public CompositeDataSource IgnoreRightSource
		{
			get
			{
				return this.ignoreRightSource;
			}
			set
			{
				this.ignoreRightSource = value;
				this.OnPropertyChanged("IgnoreRightSource");
			}
		}

		// Token: 0x1700079E RID: 1950
		// (get) Token: 0x06001971 RID: 6513 RVA: 0x00095F70 File Offset: 0x00094170
		// (set) Token: 0x06001972 RID: 6514 RVA: 0x00095F88 File Offset: 0x00094188
		public bool GraphVisible
		{
			get
			{
				return this.graphVisible;
			}
			set
			{
				this.graphVisible = value;
				this.OnPropertyChanged("GraphVisible");
			}
		}

		// Token: 0x14000081 RID: 129
		// (add) Token: 0x06001973 RID: 6515 RVA: 0x00095FA0 File Offset: 0x000941A0
		// (remove) Token: 0x06001974 RID: 6516 RVA: 0x00095FD8 File Offset: 0x000941D8
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x06001975 RID: 6517 RVA: 0x0009600D File Offset: 0x0009420D
		public CtrlChart()
		{
			this.InitializeComponent();
			this.graphVisible = true;
			this.PreparePlotter();
			base.DataContext = this;
		}

		// Token: 0x06001976 RID: 6518 RVA: 0x0009604C File Offset: 0x0009424C
		private void PreparePlotter()
		{
			this.plotter.Children.Remove(this.plotter.MouseNavigation);
			this.plotter.Children.Remove(this.plotter.KeyboardNavigation);
			this.plotter.Children.Remove(this.plotter.Legend);
			this.plotter.Children.Remove(this.plotter.DefaultContextMenu);
			this.plotter.Children.Remove(this.plotter.NewLegend);
		}

		// Token: 0x06001977 RID: 6519 RVA: 0x000960E6 File Offset: 0x000942E6
		public void ToggleGraphVisibility()
		{
			this.GraphVisible = !this.graphVisible;
		}

		// Token: 0x06001978 RID: 6520 RVA: 0x000960F9 File Offset: 0x000942F9
		public void SetHorizontalTicks(int ticks)
		{
			this.plotter.MainHorizontalAxis = new IntegerAxis();
			((IntegerTicksProvider)((IntegerAxis)this.plotter.MainHorizontalAxis).TicksProvider).MinStep = ticks;
		}

		// Token: 0x06001979 RID: 6521 RVA: 0x00096130 File Offset: 0x00094330
		public int GetHorizontalTicks()
		{
			return (this.plotter.MainHorizontalAxis != null) ? ((IntegerTicksProvider)((IntegerAxis)this.plotter.MainHorizontalAxis).TicksProvider).MinStep : 0;
		}

		// Token: 0x0600197A RID: 6522 RVA: 0x00096174 File Offset: 0x00094374
		public void SetHorizontalTicks()
		{
			HorizontalDateTimeAxis horizontalDateTimeAxis = new HorizontalDateTimeAxis();
			horizontalDateTimeAxis.MajorLabelProvider = null;
			horizontalDateTimeAxis.LabelProvider.SetCustomFormatter((LabelTickInfo<DateTime> dateTime) => dateTime.Tick.ToString("ddd, dd. MMM - HH:mm", LocalizationManager.CurrentCulture));
			horizontalDateTimeAxis.LabelProvider.SetCustomView(delegate(LabelTickInfo<DateTime> info, UIElement ui)
			{
				FrameworkElement frameworkElement = (FrameworkElement)ui;
				frameworkElement.Margin = new Thickness(2.0);
				frameworkElement.LayoutTransform = new RotateTransform(-90.0, 0.0, 0.0);
			});
			horizontalDateTimeAxis.AxisControl.DrawMinorTicks = false;
			horizontalDateTimeAxis.TicksProvider = new ChartCustomTicksProvider();
			this.plotter.MainHorizontalAxis = horizontalDateTimeAxis;
			this.plotter.MainVerticalAxis = new VerticalIntegerAxis();
		}

		// Token: 0x0600197B RID: 6523 RVA: 0x00096220 File Offset: 0x00094420
		public void DrawGraph(int[] xValues, int[] yValues)
		{
			EnumerableDataSource<int> enumerableDataSource = new EnumerableDataSource<int>(xValues);
			enumerableDataSource.SetXMapping((int x) => (double)x);
			EnumerableDataSource<int> enumerableDataSource2 = new EnumerableDataSource<int>(yValues);
			enumerableDataSource2.SetYMapping((int y) => (double)y);
			this.IntSource = new CompositeDataSource(new IPointDataSource[]
			{
				enumerableDataSource,
				enumerableDataSource2
			});
			ViewportAxesRangeRestriction viewportAxesRangeRestriction = new ViewportAxesRangeRestriction();
			int num = yValues.Max();
			bool flag = this.lastMaxGrayvalue + (int)(0.3 * (double)this.lastMaxGrayvalue) < num;
			if (flag)
			{
				this.lastMaxGrayvalue = num;
				int num2 = (num > this.maxReferencePlotValue) ? num : this.maxReferencePlotValue;
				viewportAxesRangeRestriction.YRange = new DisplayRange(-20.0, (double)(num2 + (int)(0.3 * (double)num2)));
				bool flag2 = this.plotter.Viewport.Restrictions.Count > 0;
				if (flag2)
				{
					this.plotter.Viewport.Restrictions.RemoveAt(0);
				}
				this.plotter.Viewport.Restrictions.Add(viewportAxesRangeRestriction);
			}
		}

		// Token: 0x0600197C RID: 6524 RVA: 0x00096364 File Offset: 0x00094564
		public void DrawReferencePlot(int[] xValues, int[] yValues)
		{
			EnumerableDataSource<int> enumerableDataSource = new EnumerableDataSource<int>(xValues);
			enumerableDataSource.SetXMapping((int x) => (double)x);
			EnumerableDataSource<int> enumerableDataSource2 = new EnumerableDataSource<int>(yValues);
			enumerableDataSource2.SetYMapping((int y) => (double)y);
			CompositeDataSource pointSource = new CompositeDataSource(new IPointDataSource[]
			{
				enumerableDataSource,
				enumerableDataSource2
			});
			Pen pen = new Pen(Brushes.DarkRed, 2.0);
			pen.DashStyle = DashStyles.Dot;
			this.referencePlot = this.plotter.AddLineGraph(pointSource, pen, null);
			ViewportAxesRangeRestriction viewportAxesRangeRestriction = new ViewportAxesRangeRestriction();
			this.maxReferencePlotValue = yValues.Max();
			bool flag = this.lastMaxGrayvalue + (int)(0.3 * (double)this.lastMaxGrayvalue) < this.maxReferencePlotValue;
			if (flag)
			{
				this.lastMaxGrayvalue = this.maxReferencePlotValue;
				viewportAxesRangeRestriction.YRange = new DisplayRange(-20.0, (double)(this.maxReferencePlotValue + (int)(0.3 * (double)this.maxReferencePlotValue)));
				bool flag2 = this.plotter.Viewport.Restrictions.Count > 0;
				if (flag2)
				{
					this.plotter.Viewport.Restrictions.RemoveAt(0);
				}
				this.plotter.Viewport.Restrictions.Add(viewportAxesRangeRestriction);
			}
		}

		// Token: 0x0600197D RID: 6525 RVA: 0x000964DC File Offset: 0x000946DC
		public void DrawIgnoreLines(int nIgnoreLeft, int nIgnoreRight, int nPixel)
		{
			EnumerableDataSource<int> enumerableDataSource = new EnumerableDataSource<int>(new int[]
			{
				nIgnoreLeft,
				nIgnoreLeft
			});
			enumerableDataSource.SetXMapping((int x) => (double)x);
			EnumerableDataSource<int> enumerableDataSource2 = new EnumerableDataSource<int>(new int[]
			{
				0,
				(this.lastMaxGrayvalue > 0) ? this.lastMaxGrayvalue : 10000
			});
			enumerableDataSource2.SetYMapping((int y) => (double)y);
			EnumerableDataSource<int> enumerableDataSource3 = new EnumerableDataSource<int>(new int[]
			{
				nPixel - nIgnoreRight,
				nPixel - nIgnoreRight
			});
			enumerableDataSource3.SetXMapping((int x) => (double)x);
			EnumerableDataSource<int> enumerableDataSource4 = new EnumerableDataSource<int>(new int[]
			{
				0,
				(this.lastMaxGrayvalue > 0) ? this.lastMaxGrayvalue : 10000
			});
			enumerableDataSource4.SetYMapping((int y) => (double)y);
			this.IgnoreLeftSource = new CompositeDataSource(new IPointDataSource[]
			{
				enumerableDataSource,
				enumerableDataSource2
			});
			this.IgnoreRightSource = new CompositeDataSource(new IPointDataSource[]
			{
				enumerableDataSource3,
				enumerableDataSource4
			});
		}

		// Token: 0x0600197E RID: 6526 RVA: 0x0009662C File Offset: 0x0009482C
		public void DrawGraph(DateTime[] xValues, int[] yValues, bool navigateToNewest)
		{
			DateTime now = DateTime.Now;
			EnumerableDataSource<DateTime> enumerableDataSource = new EnumerableDataSource<DateTime>(xValues);
			bool flag = this.plotter.MainHorizontalAxis is HorizontalDateTimeAxis;
			if (flag)
			{
				enumerableDataSource.SetXMapping((DateTime x) => ((HorizontalDateTimeAxis)this.plotter.MainHorizontalAxis).ConvertToDouble(x));
			}
			EnumerableDataSource<int> enumerableDataSource2 = new EnumerableDataSource<int>(yValues);
			enumerableDataSource2.SetYMapping((int y) => (double)y);
			this.DateTimeSource = new CompositeDataSource(new IPointDataSource[]
			{
				enumerableDataSource,
				enumerableDataSource2
			});
			int num = yValues.Max();
			num = ((num > 1000) ? 1000 : num);
			bool flag2 = num <= 10;
			if (flag2)
			{
				num = 10;
			}
			double num2;
			double width;
			if (navigateToNewest)
			{
				num2 = ((HorizontalDateTimeAxis)this.plotter.MainHorizontalAxis).ConvertToDouble(now.AddMinutes((double)(-(double)this.navigationStepMinutes)));
				width = (((HorizontalDateTimeAxis)this.plotter.MainHorizontalAxis).ConvertToDouble(now) - num2) * 2.0;
			}
			else
			{
				num2 = this.plotter.Visible.XMin;
				width = this.plotter.Visible.Width;
			}
			Rect rect = new Rect(num2, -0.01 * (double)num, width, (double)(num + (int)(0.3 * (double)num)));
			this.plotter.Visible = rect;
		}

		// Token: 0x0600197F RID: 6527 RVA: 0x000967C8 File Offset: 0x000949C8
		public void Rescale()
		{
			this.lastMaxGrayvalue = 0;
		}

		// Token: 0x06001980 RID: 6528 RVA: 0x000967D4 File Offset: 0x000949D4
		public void SetHorizontalAxisDescription(string description)
		{
			this.plotter.Children.Remove(this.horizontalTitle);
			this.horizontalTitle = new HorizontalAxisTitle();
			this.horizontalTitle.Content = description;
			this.plotter.Children.Add(this.horizontalTitle);
		}

		// Token: 0x06001981 RID: 6529 RVA: 0x00096828 File Offset: 0x00094A28
		public void SetVerticalAxisDescription(string description)
		{
			this.plotter.Children.Remove(this.verticalTitle);
			this.verticalTitle = new VerticalAxisTitle();
			this.verticalTitle.Content = description;
			this.plotter.Children.Add(this.verticalTitle);
		}

		// Token: 0x06001982 RID: 6530 RVA: 0x0009687C File Offset: 0x00094A7C
		public void NavigateLeft()
		{
			HorizontalDateTimeAxis horizontalDateTimeAxis = (HorizontalDateTimeAxis)this.plotter.MainHorizontalAxis;
			bool flag = horizontalDateTimeAxis.ConvertFromDouble(this.plotter.Visible.XMin - this.plotter.Visible.Width) > DateTime.Now.AddDays(-15.0);
			if (flag)
			{
				Rect rect = new Rect(this.plotter.Visible.XMin - this.plotter.Visible.Width, this.plotter.Visible.YMin, this.plotter.Visible.Width, this.plotter.Visible.Height);
				this.plotter.Visible = rect;
			}
		}

		// Token: 0x06001983 RID: 6531 RVA: 0x00096968 File Offset: 0x00094B68
		public void NavigateRight()
		{
			HorizontalDateTimeAxis horizontalDateTimeAxis = (HorizontalDateTimeAxis)this.plotter.MainHorizontalAxis;
			bool flag = horizontalDateTimeAxis.ConvertFromDouble(this.plotter.Visible.XMin + this.plotter.Visible.Width) < DateTime.Now;
			if (flag)
			{
				Rect rect = new Rect(this.plotter.Visible.XMin + this.plotter.Visible.Width, this.plotter.Visible.YMin, this.plotter.Visible.Width, this.plotter.Visible.Height);
				this.plotter.Visible = rect;
			}
		}

		// Token: 0x06001984 RID: 6532 RVA: 0x00096A42 File Offset: 0x00094C42
		public void ZoomIn()
		{
			this.ZoomIn(true);
		}

		// Token: 0x06001985 RID: 6533 RVA: 0x00096A50 File Offset: 0x00094C50
		public void ZoomIn(bool xAxis)
		{
			if (xAxis)
			{
				bool flag = this.plotter.Visible.Width > 1.0;
				if (flag)
				{
					this.plotter.Visible = this.plotter.Visible.ZoomX(this.plotter.Visible.GetCenter(), 0.5);
				}
			}
			else
			{
				bool flag2 = this.plotter.Visible.Height > 2.0;
				if (flag2)
				{
					DataRect visible = this.plotter.Visible.ZoomY(this.plotter.Visible.GetCenter(), 0.5);
					visible.YMin = 0.0;
					this.plotter.Visible = visible;
				}
			}
		}

		// Token: 0x06001986 RID: 6534 RVA: 0x00096B30 File Offset: 0x00094D30
		public void ZoomOut()
		{
			this.ZoomOut(true);
		}

		// Token: 0x06001987 RID: 6535 RVA: 0x00096B3C File Offset: 0x00094D3C
		public void ZoomOut(bool xAxis)
		{
			if (xAxis)
			{
				this.plotter.Visible = this.plotter.Visible.ZoomX(this.plotter.Visible.GetCenter(), 2.0);
			}
			else
			{
				this.plotter.Visible = this.plotter.Visible.ZoomY(this.plotter.Visible.GetCenter(), 2.0);
			}
		}

		// Token: 0x06001988 RID: 6536 RVA: 0x00096BBC File Offset: 0x00094DBC
		public void ZoomOut(int maxWidthOrHeight, bool xAxis)
		{
			if (xAxis)
			{
				bool flag = this.plotter.Visible.Width < (double)maxWidthOrHeight;
				if (flag)
				{
					this.plotter.Visible = this.plotter.Visible.ZoomX(this.plotter.Visible.GetCenter(), 2.0);
				}
			}
			else
			{
				bool flag2 = this.plotter.Visible.Height < (double)maxWidthOrHeight;
				if (flag2)
				{
					DataRect visible = this.plotter.Visible.ZoomY(this.plotter.Visible.GetCenter(), 2.0);
					visible.YMin = 0.0;
					this.plotter.Visible = visible;
				}
			}
		}

		// Token: 0x06001989 RID: 6537 RVA: 0x00096C8C File Offset: 0x00094E8C
		private void CtrlChart_Loaded(object sender, RoutedEventArgs e)
		{
			bool semiTransparentBorder = this.SemiTransparentBorder;
			if (semiTransparentBorder)
			{
				this.plotter.BorderBrush = new SolidColorBrush(Color.FromArgb(63, 0, 0, 0));
			}
		}

		// Token: 0x0600198A RID: 6538 RVA: 0x00096CC0 File Offset: 0x00094EC0
		public void RemoveReferencePlot()
		{
			bool flag = this.referencePlot != null;
			if (flag)
			{
				this.referencePlot.RemoveFromPlotter();
			}
		}

		// Token: 0x0600198B RID: 6539 RVA: 0x00096CE8 File Offset: 0x00094EE8
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x0600198C RID: 6540 RVA: 0x00096D18 File Offset: 0x00094F18
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/graph/ctrlchart.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x0600198D RID: 6541 RVA: 0x00096D50 File Offset: 0x00094F50
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
					this.plotter = (ChartPlotter)target;
				}
			}
			else
			{
				((CtrlChart)target).Loaded += this.CtrlChart_Loaded;
			}
		}

		// Token: 0x040010A1 RID: 4257
		private LineGraph referencePlot;

		// Token: 0x040010A2 RID: 4258
		private VerticalAxisTitle verticalTitle;

		// Token: 0x040010A3 RID: 4259
		private HorizontalAxisTitle horizontalTitle;

		// Token: 0x040010A4 RID: 4260
		private int lastMaxGrayvalue = 0;

		// Token: 0x040010A5 RID: 4261
		private int maxReferencePlotValue = 0;

		// Token: 0x040010A6 RID: 4262
		private int navigationStepMinutes = 60;

		// Token: 0x040010A7 RID: 4263
		private CompositeDataSource dateTimeSource;

		// Token: 0x040010A8 RID: 4264
		private CompositeDataSource intSource;

		// Token: 0x040010A9 RID: 4265
		private CompositeDataSource ignoreLeftSource;

		// Token: 0x040010AA RID: 4266
		private CompositeDataSource ignoreRightSource;

		// Token: 0x040010AB RID: 4267
		private bool graphVisible;

		// Token: 0x040010AE RID: 4270
		internal ChartPlotter plotter;

		// Token: 0x040010AF RID: 4271
		private bool _contentLoaded;
	}
}
