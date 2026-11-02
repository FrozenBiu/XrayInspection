using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;
using DatabaseConnection;
using SesoPlot;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x02000051 RID: 81
	public class CtrlValveActivity : UserControl, IComponentConnector
	{
		// Token: 0x1700026F RID: 623
		// (get) Token: 0x06000861 RID: 2145 RVA: 0x000349AB File Offset: 0x00032BAB
		// (set) Token: 0x06000862 RID: 2146 RVA: 0x000349B3 File Offset: 0x00032BB3
		public BoundLocalizedText StrValveActivity { get; set; }

		// Token: 0x06000863 RID: 2147 RVA: 0x000349BC File Offset: 0x00032BBC
		public CtrlValveActivity(ValveActivity valveActivity, ProductManager productManager, ParameterHandling parHandling, DBConnection dbConnection, SetFooterMainWindow setFooterMainWindow)
		{
			this.InitializeComponent();
			this.valveActivity = valveActivity;
			this.productManager = productManager;
			this.parHandling = parHandling;
			this.dbConnection = dbConnection;
			bool flag = dbConnection.CreateValveActivityEntries();
			this.viewModel = new ValveActivityViewModel(valveActivity, setFooterMainWindow);
			for (int i = 0; i < this.valveactplot.Graphs.Count; i++)
			{
				this.valveactplot.Graphs[i].DataContext = this.viewModel;
			}
			base.DataContext = this.viewModel;
			this.InitializeButtons(valveActivity.DbConnection);
			bool flag2 = flag;
			if (flag2)
			{
				this.DrawValveActivities(true, FlapsGraphSelection.Sum);
			}
		}

		// Token: 0x06000864 RID: 2148 RVA: 0x00034A74 File Offset: 0x00032C74
		private void InitializeButtons(DBConnection dbConnection)
		{
			this.PanelSubButtons = new DockPanel();
			this.PanelSubButtons.HorizontalAlignment = HorizontalAlignment.Stretch;
			this.PanelSubButtons.VerticalAlignment = VerticalAlignment.Stretch;
			string text = "";
			dbConnection.GetUsbDriveLetter(ref text);
			Button button = new Button();
			button.Content = ImageConverter.ToImage("/img/saveCSV.png");
			button.Click += this.SaveDBinCSV;
			button.Style = (base.FindResource("defaultButtonStyle") as Style);
			button.Margin = new Thickness(0.0, 2.0, 10.0, 2.0);
			this.PanelSubButtons.Children.Add(button);
			DockPanel.SetDock(button, Dock.Left);
			Button button2 = new Button();
			button2.Content = ImageConverter.ToImage("/img/savePNG.png");
			button2.Click += this.SavePlotinPNG;
			button2.Style = (base.FindResource("defaultButtonStyle") as Style);
			button2.Margin = new Thickness(0.0, 2.0, 10.0, 2.0);
			this.PanelSubButtons.Children.Add(button2);
			DockPanel.SetDock(button2, Dock.Left);
			Button button3 = new Button();
			button3.Content = "";
			button3.Style = (base.FindResource("defaultButtonStyle") as Style);
			button3.Visibility = Visibility.Hidden;
			button3.Width = 20.0;
			this.PanelSubButtons.Children.Add(button3);
			DockPanel.SetDock(button3, Dock.Left);
			Button button4 = new Button();
			button4.Content = ImageConverter.ToImage("/img/zoomIn.png");
			button4.Click += this.btnZoomInX_Click;
			button4.Style = (base.FindResource("defaultButtonStyle") as Style);
			button4.Margin = new Thickness(0.0, 2.0, 10.0, 2.0);
			this.PanelSubButtons.Children.Add(button4);
			DockPanel.SetDock(button4, Dock.Left);
			Button button5 = new Button();
			button5.Content = ImageConverter.ToImage("/img/zoomOut.png");
			button5.Click += this.btnZoomOutX_Click;
			button5.Style = (base.FindResource("defaultButtonStyle") as Style);
			button5.Margin = new Thickness(0.0, 2.0, 10.0, 2.0);
			this.PanelSubButtons.Children.Add(button5);
			DockPanel.SetDock(button5, Dock.Left);
			Button button6 = new Button();
			button6.Content = "";
			button6.Style = (base.FindResource("defaultButtonStyle") as Style);
			button6.Visibility = Visibility.Hidden;
			button6.Width = 20.0;
			this.PanelSubButtons.Children.Add(button6);
			DockPanel.SetDock(button6, Dock.Left);
			Button button7 = new Button();
			button7.Content = "Autoscale";
			button7.Click += this.btnAutoscale_Click;
			button7.Margin = new Thickness(0.0, 2.0, 10.0, 2.0);
			this.PanelSubButtons.Children.Add(button7);
			DockPanel.SetDock(button7, Dock.Left);
			Button button8 = new Button();
			button8.Content = "";
			button8.Style = (base.FindResource("defaultButtonStyle") as Style);
			button8.Visibility = Visibility.Hidden;
			button8.Width = 20.0;
			this.PanelSubButtons.Children.Add(button8);
			DockPanel.SetDock(button8, Dock.Left);
			Button button9 = new Button();
			button9.Content = ImageConverter.ToImage("/img/arrow-left.png");
			button9.Click += this.btnNavigateLeft_Click;
			button9.Style = (base.FindResource("defaultButtonStyle") as Style);
			button9.Margin = new Thickness(0.0, 2.0, 10.0, 2.0);
			this.PanelSubButtons.Children.Add(button9);
			DockPanel.SetDock(button9, Dock.Left);
			Button button10 = new Button();
			button10.Content = ImageConverter.ToImage("/img/arrow-right.png");
			button10.Click += this.btnNavigateRight_Click;
			button10.Style = (base.FindResource("defaultButtonStyle") as Style);
			button10.Margin = new Thickness(0.0, 2.0, 10.0, 2.0);
			this.PanelSubButtons.Children.Add(button10);
			DockPanel.SetDock(button10, Dock.Left);
			Button button11 = new Button();
			button11.Content = "";
			button11.Style = (base.FindResource("defaultButtonStyle") as Style);
			button11.Visibility = Visibility.Hidden;
			button11.Width = 20.0;
			this.PanelSubButtons.Children.Add(button11);
			DockPanel.SetDock(button8, Dock.Left);
			bool flag = this.parHandling.ReadPar(FlapsOverIOs.bulkflapsEnabledPath) == "1";
			if (flag)
			{
				ComboBox comboBox = new ComboBox();
				comboBox.FontSize = 22.0;
				comboBox.Margin = new Thickness(5.0, 0.0, 10.0, 0.0);
				comboBox.Items.Add(FlapsGraphSelection.Sum);
				comboBox.Items.Add(FlapsGraphSelection.Flap1);
				comboBox.Items.Add(FlapsGraphSelection.Flap2);
				comboBox.Items.Add(FlapsGraphSelection.Flap3);
				comboBox.Items.Add(FlapsGraphSelection.Flap4);
				comboBox.Items.Add(FlapsGraphSelection.All);
				comboBox.SelectedIndex = 0;
				comboBox.Width = 130.0;
				comboBox.SelectionChanged += this.FlapsActivitiesChanged;
				this.PanelSubButtons.Children.Add(comboBox);
				DockPanel.SetDock(comboBox, Dock.Left);
			}
		}

		// Token: 0x06000865 RID: 2149 RVA: 0x00035134 File Offset: 0x00033334
		private void btnNavigateLeft_Click(object sender, RoutedEventArgs e)
		{
			this.valveactplot.NavigateXLeft();
		}

		// Token: 0x06000866 RID: 2150 RVA: 0x00035143 File Offset: 0x00033343
		private void btnNavigateRight_Click(object sender, RoutedEventArgs e)
		{
			this.valveactplot.NavigateXRight();
		}

		// Token: 0x06000867 RID: 2151 RVA: 0x00035152 File Offset: 0x00033352
		private void btnZoomInX_Click(object sender, RoutedEventArgs e)
		{
			this.viewModel.ZoomIn();
		}

		// Token: 0x06000868 RID: 2152 RVA: 0x00035161 File Offset: 0x00033361
		private void btnZoomOutX_Click(object sender, RoutedEventArgs e)
		{
			this.viewModel.ZoomOut();
		}

		// Token: 0x06000869 RID: 2153 RVA: 0x00035170 File Offset: 0x00033370
		private void btnAutoscale_Click(object sender, RoutedEventArgs e)
		{
			this.valveactplot.AutoscaleValveActivity(this.viewModel.MaxYInit, this.viewModel.TicksYInit);
		}

		// Token: 0x0600086A RID: 2154 RVA: 0x00035198 File Offset: 0x00033398
		private void SavePlotinPNG(object sender, RoutedEventArgs e)
		{
			Rect descendantBounds = VisualTreeHelper.GetDescendantBounds(this.graphesValveActivity);
			this.viewModel.SaveActualGraphPNG(this.graphesValveActivity, descendantBounds, this.productManager);
		}

		// Token: 0x0600086B RID: 2155 RVA: 0x000351CB File Offset: 0x000333CB
		private void SaveDBinCSV(object sender, RoutedEventArgs e)
		{
			this.viewModel.SaveAllGraphesCSV(this.productManager);
		}

		// Token: 0x0600086C RID: 2156 RVA: 0x000351E0 File Offset: 0x000333E0
		public void DrawValveActivities(bool navigateToNewest, FlapsGraphSelection selection)
		{
			this.valveactplot.ValveActivityTimeAxis = new List<DateTime>();
			Point item = default(Point);
			DateTime dateTime = DateTime.Now;
			dateTime = dateTime.AddSeconds((double)(-1 * dateTime.Second));
			dateTime = dateTime.AddMilliseconds((double)(-1 * dateTime.Millisecond));
			List<Point> activities1 = new List<Point>();
			List<Point> activities2 = new List<Point>();
			List<Point> activities3 = new List<Point>();
			List<Point> activities4 = new List<Point>();
			List<Point> sum = new List<Point>();
			List<ValveActivityEntry> list = new List<ValveActivityEntry>();
			this.valveActivity.DbConnection.GetValveActivities(list);
			list.Sort((ValveActivityEntry a, ValveActivityEntry b) => a.Date.CompareTo(b.Date));
			bool flag = selection == FlapsGraphSelection.All;
			if (flag)
			{
				for (int i = 0; i < list.Count; i++)
				{
					item = default(Point);
					item.X = (double)i;
					this.valveactplot.ValveActivityTimeAxis.Add(list[i].Date);
					item.Y = Convert.ToDouble(list[i].Activity1);
					activities1.Add(item);
					item.Y = Convert.ToDouble(list[i].Activity2);
					activities2.Add(item);
					item.Y = Convert.ToDouble(list[i].Activity3);
					activities3.Add(item);
					item.Y = Convert.ToDouble(list[i].Activity4);
					activities4.Add(item);
					item.Y = Convert.ToDouble(list[i].Activity1 + list[i].Activity2 + list[i].Activity3 + list[i].Activity4);
					sum.Add(item);
					bool flag2 = item.Y > (double)this.viewModel.MaxY;
					if (flag2)
					{
						this.viewModel.MaxY = Convert.ToInt32(item.Y);
					}
				}
			}
			else
			{
				activities1.Clear();
				activities2.Clear();
				activities3.Clear();
				activities4.Clear();
				sum.Clear();
				bool flag3 = selection == FlapsGraphSelection.Flap1;
				if (flag3)
				{
					for (int j = 0; j < list.Count; j++)
					{
						item = default(Point);
						item.X = (double)j;
						this.valveactplot.ValveActivityTimeAxis.Add(list[j].Date);
						item.Y = Convert.ToDouble(list[j].Activity1);
						activities1.Add(item);
						bool flag4 = item.Y > (double)this.viewModel.MaxY;
						if (flag4)
						{
							this.viewModel.MaxY = Convert.ToInt32(item.Y);
						}
					}
				}
				else
				{
					bool flag5 = selection == FlapsGraphSelection.Flap2;
					if (flag5)
					{
						for (int k = 0; k < list.Count; k++)
						{
							item = default(Point);
							item.X = (double)k;
							this.valveactplot.ValveActivityTimeAxis.Add(list[k].Date);
							item.Y = Convert.ToDouble(list[k].Activity2);
							activities2.Add(item);
							bool flag6 = item.Y > (double)this.viewModel.MaxY;
							if (flag6)
							{
								this.viewModel.MaxY = Convert.ToInt32(item.Y);
							}
						}
					}
					else
					{
						bool flag7 = selection == FlapsGraphSelection.Flap3;
						if (flag7)
						{
							for (int l = 0; l < list.Count; l++)
							{
								item = default(Point);
								item.X = (double)l;
								this.valveactplot.ValveActivityTimeAxis.Add(list[l].Date);
								item.Y = Convert.ToDouble(list[l].Activity3);
								activities3.Add(item);
								bool flag8 = item.Y > (double)this.viewModel.MaxY;
								if (flag8)
								{
									this.viewModel.MaxY = Convert.ToInt32(item.Y);
								}
							}
						}
						else
						{
							bool flag9 = selection == FlapsGraphSelection.Flap4;
							if (flag9)
							{
								for (int m = 0; m < list.Count; m++)
								{
									item = default(Point);
									item.X = (double)m;
									this.valveactplot.ValveActivityTimeAxis.Add(list[m].Date);
									item.Y = Convert.ToDouble(list[m].Activity4);
									activities4.Add(item);
									bool flag10 = item.Y > (double)this.viewModel.MaxY;
									if (flag10)
									{
										this.viewModel.MaxY = Convert.ToInt32(item.Y);
									}
								}
							}
							else
							{
								bool flag11 = selection == FlapsGraphSelection.Sum;
								if (flag11)
								{
									for (int n = 0; n < list.Count; n++)
									{
										item = default(Point);
										item.X = (double)n;
										this.valveactplot.ValveActivityTimeAxis.Add(list[n].Date);
										item.Y = Convert.ToDouble(list[n].Activity1 + list[n].Activity2 + list[n].Activity3 + list[n].Activity4);
										sum.Add(item);
										bool flag12 = item.Y > (double)this.viewModel.MaxY;
										if (flag12)
										{
											this.viewModel.MaxY = Convert.ToInt32(item.Y);
										}
									}
								}
							}
						}
					}
				}
			}
			this.viewModel.setTicksY();
			Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
			{
				this.viewModel.Flap1Activity = activities1;
				this.viewModel.Flap2Activity = activities2;
				this.viewModel.Flap3Activity = activities3;
				this.viewModel.Flap4Activity = activities4;
				this.viewModel.SumOfActivities = sum;
			}));
		}

		// Token: 0x0600086D RID: 2157 RVA: 0x00035894 File Offset: 0x00033A94
		public void FlapsActivitiesChanged(object sender, SelectionChangedEventArgs e)
		{
			bool flag;
			if (e.AddedItems.Count > 0)
			{
				object obj = e.AddedItems[0];
				flag = (obj is FlapsGraphSelection && (FlapsGraphSelection)obj == FlapsGraphSelection.Sum);
			}
			else
			{
				flag = false;
			}
			bool flag2 = flag;
			if (flag2)
			{
				this.DrawValveActivities(false, FlapsGraphSelection.Sum);
				this.viewModel.actualModeToSavePNG = "Sum";
			}
			else
			{
				bool flag3;
				if (e.AddedItems.Count > 0)
				{
					object obj = e.AddedItems[0];
					flag3 = (obj is FlapsGraphSelection && (FlapsGraphSelection)obj == FlapsGraphSelection.Flap1);
				}
				else
				{
					flag3 = false;
				}
				bool flag4 = flag3;
				if (flag4)
				{
					this.DrawValveActivities(false, FlapsGraphSelection.Flap1);
					this.viewModel.actualModeToSavePNG = "Flap1";
				}
				else
				{
					bool flag5;
					if (e.AddedItems.Count > 0)
					{
						object obj = e.AddedItems[0];
						flag5 = (obj is FlapsGraphSelection && (FlapsGraphSelection)obj == FlapsGraphSelection.Flap2);
					}
					else
					{
						flag5 = false;
					}
					bool flag6 = flag5;
					if (flag6)
					{
						this.DrawValveActivities(false, FlapsGraphSelection.Flap2);
						this.viewModel.actualModeToSavePNG = "Flap2";
					}
					else
					{
						bool flag7;
						if (e.AddedItems.Count > 0)
						{
							object obj = e.AddedItems[0];
							flag7 = (obj is FlapsGraphSelection && (FlapsGraphSelection)obj == FlapsGraphSelection.Flap3);
						}
						else
						{
							flag7 = false;
						}
						bool flag8 = flag7;
						if (flag8)
						{
							this.DrawValveActivities(false, FlapsGraphSelection.Flap3);
							this.viewModel.actualModeToSavePNG = "Flap3";
						}
						else
						{
							bool flag9;
							if (e.AddedItems.Count > 0)
							{
								object obj = e.AddedItems[0];
								flag9 = (obj is FlapsGraphSelection && (FlapsGraphSelection)obj == FlapsGraphSelection.Flap4);
							}
							else
							{
								flag9 = false;
							}
							bool flag10 = flag9;
							if (flag10)
							{
								this.DrawValveActivities(false, FlapsGraphSelection.Flap4);
								this.viewModel.actualModeToSavePNG = "Flap4";
							}
							else
							{
								bool flag11;
								if (e.AddedItems.Count > 0)
								{
									object obj = e.AddedItems[0];
									flag11 = (obj is FlapsGraphSelection && (FlapsGraphSelection)obj == FlapsGraphSelection.All);
								}
								else
								{
									flag11 = false;
								}
								bool flag12 = flag11;
								if (flag12)
								{
									this.DrawValveActivities(false, FlapsGraphSelection.All);
									this.viewModel.actualModeToSavePNG = "All";
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0600086E RID: 2158 RVA: 0x00035AA4 File Offset: 0x00033CA4
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/bulk/valveactivity/ctrlvalveactivity.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x0600086F RID: 2159 RVA: 0x00035ADC File Offset: 0x00033CDC
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				this.graphesValveActivity = (Grid)target;
				break;
			case 2:
				this.valveactplot = (SesoPlot)target;
				break;
			case 3:
				this.legendValveActivity = (Grid)target;
				break;
			default:
				this._contentLoaded = true;
				break;
			}
		}

		// Token: 0x040005B0 RID: 1456
		private ValveActivityViewModel viewModel;

		// Token: 0x040005B1 RID: 1457
		private ProductManager productManager;

		// Token: 0x040005B2 RID: 1458
		private ParameterHandling parHandling;

		// Token: 0x040005B3 RID: 1459
		private DBConnection dbConnection;

		// Token: 0x040005B4 RID: 1460
		public DockPanel PanelSubButtons;

		// Token: 0x040005B5 RID: 1461
		private ValveActivity valveActivity;

		// Token: 0x040005B7 RID: 1463
		internal Grid graphesValveActivity;

		// Token: 0x040005B8 RID: 1464
		internal SesoPlot valveactplot;

		// Token: 0x040005B9 RID: 1465
		internal Grid legendValveActivity;

		// Token: 0x040005BA RID: 1466
		private bool _contentLoaded;
	}
}
