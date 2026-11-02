using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;
using DatabaseConnection;
using Definitions;
using SesoPlot;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x02000084 RID: 132
	public class CtrlDataPlots : UserControl, IComponentConnector
	{
		// Token: 0x06000D04 RID: 3332 RVA: 0x0004C9B4 File Offset: 0x0004ABB4
		public CtrlDataPlots(ParameterHandling parHandling, DBConnection dbConnection, DBDataLogs dbDatalogs, DataModesMonitoring mode)
		{
			this.InitializeComponent();
			this.parHandling = parHandling;
			this.dBDataLogs = dbDatalogs;
			this.dbConnection = dbConnection;
			this.viewModel = new DataPlotsViewModel(dbConnection, parHandling);
			for (int i = 0; i < this.dataplot.Graphs.Count; i++)
			{
				this.dataplot.Graphs[i].DataContext = this.viewModel;
				this.dataplot.Graphs[i].GraphType = GraphType.Linear;
			}
			this.mode = mode;
			switch (mode)
			{
			case DataModesMonitoring.xray:
				this.viewModel.AddXRay();
				break;
			case DataModesMonitoring.detector:
				this.viewModel.AddDetector();
				break;
			case DataModesMonitoring.conveyor:
				this.viewModel.AddConveyor();
				break;
			case DataModesMonitoring.rejecter:
				this.viewModel.AddRejecter();
				break;
			case DataModesMonitoring.operational:
				this.viewModel.AddOperational();
				break;
			default:
				this.viewModel.AddOperational();
				break;
			}
			base.DataContext = this.viewModel;
			this.viewModel.UpdateDataPlots += this.updateDataPlots;
			this.viewModel.BarDiagramm = false;
			this.InitializeButtons(dbConnection);
			this.viewModel.SelectionComboBox = this.viewModel.DataComboBox.First<string>();
		}

		// Token: 0x06000D05 RID: 3333 RVA: 0x0004CB1B File Offset: 0x0004AD1B
		private void updateDataPlots()
		{
			this.DrawDataPlots(true, this.viewModel.SelectionComboBox, true);
		}

		// Token: 0x06000D06 RID: 3334 RVA: 0x0004CB34 File Offset: 0x0004AD34
		private void InitializeButtons(DBConnection dbConnection)
		{
			this.PanelSubButtons = new DockPanel();
			this.PanelSubButtons.HorizontalAlignment = HorizontalAlignment.Stretch;
			this.PanelSubButtons.VerticalAlignment = VerticalAlignment.Stretch;
			Grid.SetColumn(this.PanelSubButtons, 4);
			string usbLetter = "";
			dbConnection.GetUsbDriveLetter(ref usbLetter);
			ButtonUSB element = new ButtonUSB(usbLetter, "/img/savePNG.png", "/img/savePNG_disabled.png", new RoutedEventHandler(this.SavePlotinPNG), true);
			this.PanelSubButtons.Children.Add(element);
			DockPanel.SetDock(element, Dock.Left);
			Button button = new Button();
			button.Content = "Autoscale";
			button.Click += this.btnAutoscale_Click;
			button.Margin = new Thickness(10.0, 2.0, 10.0, 2.0);
			this.PanelSubButtons.Children.Add(button);
			DockPanel.SetDock(button, Dock.Left);
			Button button2 = new Button();
			button2.Content = ImageConverter.ToImage("/img/arrow-left.png");
			button2.Click += this.btnNavigateLeft_Click;
			button2.Style = (base.FindResource("defaultButtonStyle") as Style);
			button2.Margin = new Thickness(0.0, 2.0, 10.0, 2.0);
			this.PanelSubButtons.Children.Add(button2);
			DockPanel.SetDock(button2, Dock.Left);
			Button button3 = new Button();
			button3.Content = ImageConverter.ToImage("/img/arrow-right.png");
			button3.Click += this.btnNavigateRight_Click;
			button3.Style = (base.FindResource("defaultButtonStyle") as Style);
			button3.Margin = new Thickness(0.0, 2.0, 10.0, 2.0);
			this.PanelSubButtons.Children.Add(button3);
			DockPanel.SetDock(button3, Dock.Left);
		}

		// Token: 0x06000D07 RID: 3335 RVA: 0x0004CD48 File Offset: 0x0004AF48
		private void SavePlotinPNG(object sender, RoutedEventArgs e)
		{
			Rect descendantBounds = VisualTreeHelper.GetDescendantBounds(this.graphesValveActivity);
			this.viewModel.SaveActualGraphPNG(this.graphesValveActivity, descendantBounds);
		}

		// Token: 0x06000D08 RID: 3336 RVA: 0x0004CD75 File Offset: 0x0004AF75
		private void date2_Clicked(object sender, MouseButtonEventArgs e)
		{
			this.viewModel.DateClicked(2);
		}

		// Token: 0x06000D09 RID: 3337 RVA: 0x0004CD85 File Offset: 0x0004AF85
		private void date1_Clicked(object sender, RoutedEventArgs e)
		{
			this.viewModel.DateClicked(1);
		}

		// Token: 0x06000D0A RID: 3338 RVA: 0x0004CD98 File Offset: 0x0004AF98
		private void DataPlot_Loaded(object sender, RoutedEventArgs e)
		{
			Subwindow subwindow = Window.GetWindow(this) as Subwindow;
			subwindow.buttonGrid.Children.Remove(subwindow.buttonPanel);
			subwindow.buttonGrid.Children.Add(this.PanelSubButtons);
		}

		// Token: 0x06000D0B RID: 3339 RVA: 0x0004CDE0 File Offset: 0x0004AFE0
		private void btnNavigateLeft_Click(object sender, RoutedEventArgs e)
		{
			bool flag = this.viewModel.FirstDate.Date != this.viewModel.Date1.Date;
			if (flag)
			{
				bool flag2 = this.viewModel.SelectionComboBox.Equals(this.viewModel.StrOverview.LocalizedText);
				if (flag2)
				{
					this.viewModel.Date1 = this.viewModel.Date1.AddDays(-1.0);
					this.viewModel.Date2 = this.viewModel.Date1;
					bool flag3 = this.viewModel.Date1 < this.viewModel.FirstDate.Date;
					if (flag3)
					{
						this.viewModel.Date1 = this.viewModel.FirstDate.Date;
						this.viewModel.Date2 = this.viewModel.Date1;
					}
				}
				else
				{
					this.viewModel.Date2 = this.viewModel.Date1;
					bool barDiagramm = this.viewModel.BarDiagramm;
					if (barDiagramm)
					{
						this.viewModel.Date1 = this.viewModel.Date1.AddMonths(-1);
					}
					else
					{
						this.viewModel.Date1 = this.viewModel.Date1.AddMonths(-6);
					}
					bool flag4 = this.viewModel.Date1 < this.viewModel.FirstDate.Date;
					if (flag4)
					{
						this.viewModel.Date1 = this.viewModel.FirstDate.Date;
					}
					bool flag5 = this.viewModel.Date2 <= this.viewModel.FirstDate.Date;
					if (flag5)
					{
						this.viewModel.Date2 = this.viewModel.FirstDate.Date.AddDays(14.0);
					}
				}
				this.DrawDataPlots(true, this.viewModel.SelectionComboBox, true);
			}
		}

		// Token: 0x06000D0C RID: 3340 RVA: 0x0004D014 File Offset: 0x0004B214
		private void btnNavigateRight_Click(object sender, RoutedEventArgs e)
		{
			bool flag = this.viewModel.LastDate.Date != this.viewModel.Date2.AddDays(-1.0).Date;
			if (flag)
			{
				bool flag2 = this.viewModel.SelectionComboBox.Equals(this.viewModel.StrOverview.LocalizedText);
				if (flag2)
				{
					this.viewModel.Date1 = this.viewModel.Date1.AddDays(1.0);
					this.viewModel.Date2 = this.viewModel.Date1;
					bool flag3 = this.viewModel.Date2 > this.viewModel.LastDate;
					if (flag3)
					{
						this.viewModel.Date2 = this.viewModel.LastDate;
						this.viewModel.Date1 = this.viewModel.Date2;
					}
				}
				else
				{
					this.viewModel.Date1 = this.viewModel.Date2;
					bool barDiagramm = this.viewModel.BarDiagramm;
					if (barDiagramm)
					{
						this.viewModel.Date2 = this.viewModel.Date2.AddMonths(1);
					}
					else
					{
						this.viewModel.Date2 = this.viewModel.Date2.AddMonths(6);
					}
					bool flag4 = this.viewModel.Date2 >= this.viewModel.LastDate;
					if (flag4)
					{
						this.viewModel.Date2 = this.viewModel.LastDate;
					}
					bool flag5 = this.viewModel.Date1.Date >= this.viewModel.LastDate.AddDays(-1.0).Date;
					if (flag5)
					{
						this.viewModel.Date1 = this.viewModel.LastDate.AddDays(-14.0);
					}
					bool flag6 = this.viewModel.Date1.Date < this.viewModel.FirstDate.Date;
					if (flag6)
					{
						this.viewModel.Date1 = this.viewModel.FirstDate;
					}
				}
				this.DrawDataPlots(true, this.viewModel.SelectionComboBox, true);
			}
		}

		// Token: 0x06000D0D RID: 3341 RVA: 0x0004D28D File Offset: 0x0004B48D
		private void btnZoomInX_Click(object sender, RoutedEventArgs e)
		{
			this.viewModel.ZoomIn();
		}

		// Token: 0x06000D0E RID: 3342 RVA: 0x0004D29C File Offset: 0x0004B49C
		private void btnZoomOutX_Click(object sender, RoutedEventArgs e)
		{
			this.viewModel.ZoomOut();
		}

		// Token: 0x06000D0F RID: 3343 RVA: 0x0004D2AC File Offset: 0x0004B4AC
		private void btnAutoscale_Click(object sender, RoutedEventArgs e)
		{
			this.dataplot.AutoScaleDataPlot(this.viewModel.MaxYInit, this.viewModel.TicksYInit, this.viewModel.TicksXMinor, this.viewModel.TicksX, this.viewModel.MaxX);
		}

		// Token: 0x06000D10 RID: 3344 RVA: 0x0004D300 File Offset: 0x0004B500
		public void DrawDataPlots(bool drawWithZeros, string dataName, bool withLimits)
		{
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			List<string> list3 = new List<string>();
			List<string> list4 = new List<string>();
			List<string> list5 = new List<string>();
			List<string> list6 = new List<string>();
			List<string> list7 = new List<string>();
			List<string> list8 = new List<string>();
			List<string> list9 = new List<string>();
			List<string> list10 = new List<string>();
			List<string> list11 = new List<string>();
			List<string> list12 = new List<string>();
			List<string> list13 = new List<string>();
			List<string> list14 = new List<string>();
			List<string> list15 = new List<string>();
			List<string> list16 = new List<string>();
			string text = "";
			string text2 = "";
			string text3 = "";
			this.viewModel.Data1.Clear();
			this.viewModel.Data2.Clear();
			this.viewModel.Data3.Clear();
			this.viewModel.Data4.Clear();
			this.viewModel.Data5.Clear();
			CultureInfo provider = new CultureInfo("de-DE");
			int num = 1;
			this.viewModel.Unit = " ";
			bool flag = false;
			this.viewModel.Col2 = false;
			this.viewModel.Col3 = false;
			this.viewModel.Col4 = false;
			bool flag2 = false;
			this.dataplot.XAxisType = AxisType.MonitoringPlot;
			this.viewModel.NameData1 = this.viewModel.NameData;
			int num2 = 0;
			this.dbConnection.GetNumValves(ref num2);
			MachineMode machineMode = MachineMode.Standard;
			this.dbConnection.GetMachineMode(ref machineMode);
			bool flag3 = machineMode == MachineMode.Bulk;
			switch (this.mode)
			{
			case DataModesMonitoring.xray:
			{
				this.dBDataLogs.MachineInfosGetSerialNumbers(ref text2, ref text);
				string text4 = "Arcs";
				string text5 = "FilamentCurrent";
				string text6 = "SwitchOnCycles";
				bool flag4 = text2.Length > 0;
				if (flag4)
				{
					text4 = text4 + "_" + text2.Replace("-", "");
					text5 = text5 + "_" + text2.Replace("-", "");
					text6 = text6 + "_" + text2.Replace("-", "");
				}
				bool flag5 = dataName.Equals(this.viewModel.StrTemperature.LocalizedText);
				if (flag5)
				{
					this.dBDataLogs.GetLimitsEntry("TemperatureSource", ref text3);
					this.dBDataLogs.TemperaturesGetEntries("Temperatures", ref list, ref list6, ref list2, ref list7);
					this.viewModel.Unit = "[°C]";
					this.dBDataLogs.GetLimitsEntries("TemperatureSource", ref list15, ref list16);
				}
				else
				{
					bool flag6 = dataName.Equals(this.viewModel.StrArcs.LocalizedText);
					if (flag6)
					{
						this.dBDataLogs.GetLimitsEntry("Arcs", ref text3);
						this.dBDataLogs.ArcsGetEntriesType0(text4, ref list, ref list6, ref list2);
						drawWithZeros = false;
						this.viewModel.Unit = " ";
						this.dBDataLogs.GetLimitsEntries("Arcs", ref list15, ref list16);
					}
					else
					{
						bool flag7 = dataName.Equals(this.viewModel.StrFilamentCurrent.LocalizedText);
						if (flag7)
						{
							this.dBDataLogs.GetLimitsEntry("FilamentCurrent", ref text3);
							this.dBDataLogs.FilamentCurrentGetEntries(text5, ref list, ref list6, ref list9, ref list7, ref list8, ref list2, ref list10);
							this.viewModel.Unit = "[A]";
							this.dBDataLogs.GetLimitsEntries("FilamentCurrent", ref list15, ref list16);
						}
						else
						{
							bool flag8 = dataName.Equals(this.viewModel.StrSwitchOnCycles.LocalizedText);
							if (flag8)
							{
								this.dBDataLogs.XRaySwitchOnGetEntries(text6, ref list);
								drawWithZeros = false;
								this.dBDataLogs.GetLimitsEntries("SwitchOnCycles", ref list15, ref list16);
							}
							else
							{
								bool flag9 = dataName.Equals(this.viewModel.StrTime.LocalizedText);
								if (flag9)
								{
									flag = true;
									for (int i = 0; i < this.dataplot.Graphs.Count; i++)
									{
										this.dataplot.Graphs[i].GraphType = GraphType.Bar;
									}
									this.dBDataLogs.OnOffTimesGetEntries(ref list, ref list6, ref list7, ref list2, ref list8, ref list9, ref list10, ref list11, ref list12, ref list13, ref list14);
									this.viewModel.Unit = "[h] " + this.viewModel.StrPerDay.LocalizedText;
								}
							}
						}
					}
				}
				break;
			}
			case DataModesMonitoring.detector:
			{
				bool flag10 = dataName.Equals(this.viewModel.StrTemperature.LocalizedText);
				if (flag10)
				{
					this.dBDataLogs.GetLimitsEntry("TemperatureDetector", ref text3);
					this.dBDataLogs.TemperaturesGetEntries("Temperatures", ref list, ref list6, ref list7, ref list2);
					this.viewModel.Unit = "[°C]";
					this.dBDataLogs.GetLimitsEntries("TemperatureDetector", ref list15, ref list16);
				}
				break;
			}
			case DataModesMonitoring.conveyor:
			{
				bool flag11 = dataName.Equals(this.viewModel.StrPixelDeltaMax.LocalizedText);
				if (flag11)
				{
					this.dBDataLogs.MachineInfosGetSerialNumbers(ref text2, ref text);
					string tableName = "BackgroundIndex_" + text.Replace(":", "");
					this.dBDataLogs.GetLimitsEntry("PixelDeltaMax", ref text3);
					this.dBDataLogs.BackgroundIndexGetEntries(tableName, ref list, ref list9, ref list6, ref list7, ref list8, ref list2, ref list10, ref list11, ref list12);
					this.viewModel.Unit = " ";
					this.dBDataLogs.GetLimitsEntries("PixelDeltaMax", ref list15, ref list16);
				}
				else
				{
					bool flag12 = dataName.Equals(this.viewModel.StrBackgroundIndex.LocalizedText);
					if (flag12)
					{
						this.dBDataLogs.MachineInfosGetSerialNumbers(ref text2, ref text);
						string tableName2 = "BackgroundIndex_" + text.Replace(":", "");
						this.dBDataLogs.GetLimitsEntry("BackgroundIndex", ref text3);
						this.dBDataLogs.BackgroundIndexGetEntries(tableName2, ref list, ref list10, ref list6, ref list7, ref list8, ref list9, ref list2, ref list11, ref list12);
						this.viewModel.Unit = " ";
						this.dBDataLogs.GetLimitsEntries("BackgroundIndex", ref list15, ref list16);
					}
					else
					{
						bool flag13 = dataName.Equals(this.viewModel.StrTime.LocalizedText);
						if (flag13)
						{
							flag = true;
							this.dBDataLogs.OnOffTimesGetEntries(ref list, ref list6, ref list2, ref list7, ref list8, ref list9, ref list10, ref list11, ref list12, ref list13, ref list14);
							this.viewModel.Unit = "[h] " + this.viewModel.StrPerDay.LocalizedText;
							num = 1;
						}
					}
				}
				break;
			}
			case DataModesMonitoring.rejecter:
			{
				flag = true;
				this.viewModel.Unit = this.viewModel.StrPerDay.LocalizedText;
				bool flag14 = dataName.Equals(this.viewModel.StrTitleRejecter.LocalizedText + " 1");
				if (flag14)
				{
					this.dBDataLogs.OnOffTimesGetEntries(ref list, ref list6, ref list7, ref list8, ref list2, ref list9, ref list10, ref list11, ref list12, ref list13, ref list14);
					this.dBDataLogs.GetLimitsEntries("Rejecter1PerDay", ref list15, ref list16);
				}
				else
				{
					bool flag15 = dataName.Equals(this.viewModel.StrTitleRejecter.LocalizedText + " 2");
					if (flag15)
					{
						this.dBDataLogs.OnOffTimesGetEntries(ref list, ref list6, ref list7, ref list8, ref list9, ref list2, ref list10, ref list11, ref list12, ref list13, ref list14);
						this.dBDataLogs.GetLimitsEntries("Rejecter2PerDay", ref list15, ref list16);
					}
					else
					{
						bool flag16 = dataName.Equals(this.viewModel.StrTitleRejecter.LocalizedText + " 3");
						if (flag16)
						{
							this.dBDataLogs.OnOffTimesGetEntries(ref list, ref list6, ref list7, ref list8, ref list9, ref list10, ref list2, ref list11, ref list12, ref list13, ref list14);
							this.dBDataLogs.GetLimitsEntries("Rejecter3PerDay", ref list15, ref list16);
						}
						else
						{
							bool flag17 = dataName.Equals(this.viewModel.StrTitleRejecter.LocalizedText + " 4");
							if (flag17)
							{
								this.dBDataLogs.OnOffTimesGetEntries(ref list, ref list6, ref list7, ref list8, ref list9, ref list10, ref list11, ref list2, ref list12, ref list13, ref list14);
								this.dBDataLogs.GetLimitsEntries("Rejecter4PerDay", ref list15, ref list16);
							}
							else
							{
								bool flag18 = dataName.Equals(this.viewModel.StrTitleRejecter.LocalizedText);
								if (flag18)
								{
									this.dBDataLogs.OnOffTimesGetEntries(ref list, ref list6, ref list7, ref list8, ref list2, ref list3, ref list4, ref list5, ref list12, ref list13, ref list14);
									num = this.viewModel.ColumnsForRejecter;
									this.viewModel.NameData1 = this.viewModel.StrTitleRejecter.LocalizedText + " 1";
									bool flag19 = num >= 2;
									if (flag19)
									{
										this.viewModel.NameData2 = this.viewModel.StrTitleRejecter.LocalizedText + " 2";
										this.viewModel.Col2 = true;
										bool flag20 = num >= 3;
										if (flag20)
										{
											this.viewModel.NameData3 = this.viewModel.StrTitleRejecter.LocalizedText + " 3";
											this.viewModel.NameData4 = this.viewModel.StrTitleRejecter.LocalizedText + " 4";
											this.viewModel.Col3 = true;
											this.viewModel.Col4 = true;
										}
									}
								}
								else
								{
									bool flag21 = dataName.Equals(this.viewModel.StrOverview.LocalizedText);
									if (flag21)
									{
										this.dBDataLogs.GetSVTValvesForTheDay(this.viewModel.Date1.ToString("d", new CultureInfo("de-DE")), ref list2, num2);
										this.dBDataLogs.OnOffTimesGetEntriesValve(ref list, ref list6, "SVTValve1");
										flag2 = true;
										this.dataplot.XAxisType = AxisType.NumbersAscending;
										this.dataplot.MaxNumAscending = num2;
										this.viewModel.NameData1 = this.viewModel.StrValve.LocalizedText;
										this.viewModel.Unit = "";
										this.viewModel.LimitVisible = false;
									}
									else
									{
										bool flag22 = dataName.Equals("SVT " + this.viewModel.StrValveActivity.LocalizedText);
										if (flag22)
										{
											List<ValveActivityEntry> list17 = new List<ValveActivityEntry>();
											this.dbConnection.GetValveActivities(list17);
											list17.Sort((ValveActivityEntry a, ValveActivityEntry b) => a.Date.CompareTo(b.Date));
											for (int j = 0; j < list17.Count; j++)
											{
												list.Add(list17[j].Date.ToString());
												list2.Add(list17[j].Activity1.ToString());
											}
											flag = false;
											this.viewModel.LimitVisible = false;
											this.viewModel.Unit = this.viewModel.StrProductCounterBad.LocalizedText + " /min";
										}
										else
										{
											bool flag23 = dataName.Equals(this.viewModel.StrValveActivity.LocalizedText);
											if (flag23)
											{
												List<ValveActivityEntry> list18 = new List<ValveActivityEntry>();
												this.dbConnection.GetValveActivities(list18);
												list18.Sort((ValveActivityEntry a, ValveActivityEntry b) => a.Date.CompareTo(b.Date));
												for (int k = 0; k < list18.Count; k++)
												{
													list.Add(list18[k].Date.ToString());
													int num3 = list18[k].Activity1 + list18[k].Activity2 + list18[k].Activity3 + list18[k].Activity4;
													list2.Add(num3.ToString());
												}
												flag = false;
												this.viewModel.LimitVisible = false;
												this.viewModel.Unit = this.viewModel.StrProductCounterBad.LocalizedText + " /min";
											}
											else
											{
												bool flag24 = dataName.Contains(this.viewModel.StrValveActivity.LocalizedText + " " + this.viewModel.StrTitleRejecter.LocalizedText);
												if (flag24)
												{
													List<ValveActivityEntry> list19 = new List<ValveActivityEntry>();
													this.dbConnection.GetValveActivities(list19);
													list19.Sort((ValveActivityEntry a, ValveActivityEntry b) => a.Date.CompareTo(b.Date));
													for (int l = 0; l < list19.Count; l++)
													{
														list.Add(list19[l].Date.ToString());
														bool flag25 = dataName.Contains("1");
														if (flag25)
														{
															list2.Add(list19[l].Activity1.ToString());
														}
														else
														{
															bool flag26 = dataName.Contains("2");
															if (flag26)
															{
																list2.Add(list19[l].Activity2.ToString());
															}
															else
															{
																bool flag27 = dataName.Contains("3");
																if (flag27)
																{
																	list2.Add(list19[l].Activity3.ToString());
																}
																else
																{
																	bool flag28 = dataName.Contains("4");
																	if (flag28)
																	{
																		list2.Add(list19[l].Activity4.ToString());
																	}
																}
															}
														}
													}
													flag = false;
													this.viewModel.LimitVisible = false;
													this.viewModel.Unit = this.viewModel.StrProductCounterBad.LocalizedText + " /min";
												}
												else
												{
													bool flag29 = dataName.Contains(this.viewModel.StrValve.LocalizedText);
													if (flag29)
													{
														string name = "SVTValve" + dataName.Remove(0, this.viewModel.StrValve.LocalizedText.Count<char>() + 1);
														this.dBDataLogs.OnOffTimesGetEntriesValve(ref list, ref list2, name);
														this.dBDataLogs.GetLimitsEntries("Rejecter1PerDay", ref list15, ref list16);
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
				break;
			}
			case DataModesMonitoring.operational:
			{
				bool flag30 = dataName.Equals(this.viewModel.StrTemperature.LocalizedText);
				if (flag30)
				{
					this.dBDataLogs.GetLimitsEntry("Temperature", ref text3);
					this.dBDataLogs.TemperaturesGetEntries("Temperatures", ref list, ref list2, ref list6, ref list7);
					this.viewModel.Unit = "[°C]";
					this.dBDataLogs.GetLimitsEntries("Temperature", ref list15, ref list16);
				}
				else
				{
					bool flag31 = dataName.Equals(this.viewModel.StrTime.LocalizedText);
					if (flag31)
					{
						num = 3;
						flag = true;
						bool flag32 = flag3;
						if (flag32)
						{
							this.dBDataLogs.OnOffTimesGetEntries(ref list, ref list2, ref list3, ref list4, ref list8, ref list9, ref list10, ref list11, ref list12, ref list13, ref list5);
							num = 4;
							this.viewModel.Col4 = true;
							this.viewModel.NameData4 = this.viewModel.StrTime.LocalizedText + " " + this.viewModel.StrSortingSwitchedOn.LocalizedText;
						}
						else
						{
							this.dBDataLogs.OnOffTimesGetEntries(ref list, ref list2, ref list3, ref list4, ref list8, ref list9, ref list10, ref list11, ref list12, ref list13, ref list14);
						}
						this.viewModel.Unit = "[h] " + this.viewModel.StrPerDay.LocalizedText;
						this.viewModel.Col2 = true;
						this.viewModel.Col3 = true;
						this.viewModel.NameData1 = this.viewModel.StrTime.LocalizedText + " " + this.viewModel.StrMachine.LocalizedText;
						this.viewModel.NameData2 = this.viewModel.StrTime.LocalizedText + " " + this.viewModel.StrTitleConveyor.LocalizedText;
						this.viewModel.NameData3 = this.viewModel.StrTime.LocalizedText + " " + this.viewModel.StrTitleSource.LocalizedText;
					}
					else
					{
						bool flag33 = dataName.Equals(this.viewModel.StrCounter.LocalizedText);
						if (flag33)
						{
							flag = true;
							this.dBDataLogs.OnOffTimesGetEntries(ref list, ref list6, ref list7, ref list8, ref list9, ref list10, ref list11, ref list12, ref list2, ref list3, ref list14);
							this.viewModel.Unit = this.viewModel.StrPerDay.LocalizedText;
							num = 2;
							this.viewModel.Col2 = true;
							this.viewModel.NameData1 = this.viewModel.StrThroughput.LocalizedText;
							this.viewModel.NameData2 = this.viewModel.StrReject.LocalizedText;
						}
						else
						{
							bool flag34 = dataName.Equals(this.viewModel.StrThroughput.LocalizedText);
							if (flag34)
							{
								flag = true;
								this.dBDataLogs.OnOffTimesGetEntries(ref list, ref list6, ref list7, ref list8, ref list9, ref list10, ref list11, ref list12, ref list2, ref list13, ref list14);
								this.viewModel.Unit = this.viewModel.StrPerDay.LocalizedText;
								num = 1;
								this.viewModel.NameData1 = this.viewModel.StrThroughput.LocalizedText;
								this.dBDataLogs.GetLimitsEntries("ThroughputPerDay", ref list15, ref list16);
							}
							else
							{
								bool flag35 = dataName.Equals(this.viewModel.StrReject.LocalizedText);
								if (flag35)
								{
									flag = true;
									this.dBDataLogs.OnOffTimesGetEntries(ref list, ref list6, ref list7, ref list8, ref list9, ref list10, ref list11, ref list12, ref list13, ref list2, ref list14);
									this.viewModel.Unit = this.viewModel.StrPerDay.LocalizedText;
									num = 1;
									this.viewModel.NameData1 = this.viewModel.StrReject.LocalizedText;
									this.dBDataLogs.GetLimitsEntries("RejectPerDay", ref list15, ref list16);
								}
							}
						}
					}
				}
				break;
			}
			}
			this.viewModel.BarDiagramm = flag;
			this.dataplot.ValveActivityTimeAxis = new List<DateTime>();
			bool flag36 = (list2.Count > 0 && (list.Count > 0 || flag2)) || (dataName.Equals(this.viewModel.StrSwitchOnCycles.LocalizedText) && list.Count > 0);
			if (flag36)
			{
				Point item = default(Point);
				bool flag37 = list16.Count > 0;
				bool flag38 = flag37;
				List<Point> activities1 = new List<Point>();
				List<Point> activities2 = new List<Point>();
				List<Point> activities3 = new List<Point>();
				List<Point> activities4 = new List<Point>();
				List<Point> activities5 = new List<Point>();
				this.viewModel.MaxY = 0;
				int num4 = 0;
				DateTime dateTime;
				DateTime dateTime2;
				double num5;
				double num6;
				double num7;
				if (withLimits)
				{
					bool flag39 = this.viewModel.Date1.Year > 1;
					if (flag39)
					{
						dateTime = this.viewModel.Date1;
					}
					else
					{
						dateTime = DateTime.Parse(list.First<string>(), provider);
					}
					bool flag40 = this.viewModel.Date2.Year > 1;
					if (flag40)
					{
						dateTime2 = this.viewModel.Date2;
					}
					else
					{
						dateTime2 = DateTime.Parse(list.Last<string>(), provider);
					}
					num5 = (dateTime2.Date - dateTime.Date).TotalDays;
					bool flag41 = !flag;
					if (flag41)
					{
						bool flag42 = num5 > 180.0;
						if (flag42)
						{
							dateTime = dateTime2.AddMonths(-6);
						}
					}
					else
					{
						bool flag43 = num5 > 30.0;
						if (flag43)
						{
							dateTime = dateTime2.AddMonths(-1);
						}
					}
					bool flag44 = dateTime2 > this.viewModel.LastDate;
					if (flag44)
					{
						dateTime2 = this.viewModel.LastDate;
					}
					this.viewModel.CurrentDate2 = dateTime2;
					this.viewModel.CurrentDate1 = dateTime;
					num5 = (dateTime2.Date.AddDays(1.0) - dateTime.Date).TotalDays;
					num6 = (dateTime2.Date.AddDays(1.0) - dateTime.Date).TotalSeconds;
					num7 = num6 / num5;
				}
				else
				{
					bool flag45 = !flag2;
					if (flag45)
					{
						dateTime = DateTime.Parse(list.First<string>(), provider);
						this.viewModel.FirstDate = dateTime;
						dateTime2 = DateTime.Parse(list.Last<string>(), provider);
						this.viewModel.LastDate = dateTime2;
						this.viewModel.CurrentDate2 = dateTime2;
						num5 = (dateTime2.Date.AddDays(1.0) - dateTime.Date).TotalDays;
						bool flag46 = !flag;
						if (flag46)
						{
							bool flag47 = num5 > 180.0;
							if (flag47)
							{
								dateTime = dateTime2.AddMonths(-6);
							}
						}
						else
						{
							bool flag48 = num5 > 30.0;
							if (flag48)
							{
								dateTime = dateTime2.AddMonths(-1);
							}
						}
						this.viewModel.CurrentDate2 = dateTime2;
						this.viewModel.CurrentDate1 = dateTime;
						num5 = (dateTime2.Date.AddDays(1.0) - dateTime.Date).TotalDays;
						num6 = (dateTime2.Date.AddDays(1.0) - dateTime.Date).TotalSeconds;
						num7 = num6 / num5;
					}
					else
					{
						dateTime = DateTime.Parse(list.First<string>(), provider);
						this.viewModel.FirstDate = dateTime;
						dateTime2 = DateTime.Parse(list.Last<string>(), provider);
						this.viewModel.LastDate = dateTime2;
						num7 = 1.0;
						num5 = (double)num2;
						num6 = (double)num2;
						dateTime = DateTime.Parse(list.First<string>(), provider);
						this.viewModel.FirstDate = dateTime;
						dateTime2 = DateTime.Parse(list.Last<string>(), provider);
						this.viewModel.LastDate = dateTime2;
					}
				}
				double num8 = num7;
				bool flag49 = flag;
				if (flag49)
				{
					num8 = num7 / (double)(num + 1);
				}
				int num9 = 0;
				List<DateTime> list20 = new List<DateTime>();
				int num10 = 1;
				bool flag50 = num5 > 10.0 && num5 <= 20.0;
				if (flag50)
				{
					num10 = 2;
				}
				else
				{
					bool flag51 = num5 > 20.0 && num5 <= 50.0;
					if (flag51)
					{
						num10 = 5;
					}
					else
					{
						bool flag52 = num5 > 50.0 && num5 <= 100.0;
						if (flag52)
						{
							num10 = 10;
						}
						else
						{
							bool flag53 = num5 > 100.0;
							if (flag53)
							{
								num10 = 20;
							}
						}
					}
				}
				double num11 = 0.0;
				bool flag54 = !flag2;
				if (flag54)
				{
					for (int m = 0; m < list.Count; m++)
					{
						DateTime dateTime3 = DateTime.Parse(list[m], provider);
						bool flag55 = dateTime3 >= dateTime && dateTime3 <= dateTime2.AddDays(1.0);
						if (flag55)
						{
							bool flag56 = !list20.Contains(dateTime3.Date);
							if (flag56)
							{
								bool flag57 = list20.Count == 0;
								if (flag57)
								{
									bool flag58 = dateTime.Date <= dateTime3.Date;
									if (flag58)
									{
										list20.Add(dateTime.Date);
										bool flag59 = num9 % num10 == 0;
										if (flag59)
										{
											this.dataplot.ValveActivityTimeAxis.Add(list20.Last<DateTime>());
										}
										double totalSeconds = (list20.Last<DateTime>() - dateTime.Date).TotalSeconds;
										bool flag60 = drawWithZeros;
										if (flag60)
										{
											item = default(Point);
											item.X = totalSeconds;
											item.Y = 0.0;
											activities2.Add(item);
										}
										num9++;
									}
								}
								bool flag61 = list20.Count > 0;
								if (flag61)
								{
									while (list20.Last<DateTime>().AddDays(1.0) < dateTime3.Date && list20.Last<DateTime>().AddDays(1.0) <= dateTime2.AddDays(1.0))
									{
										list20.Add(list20.Last<DateTime>().AddDays(1.0));
										bool flag62 = num9 % num10 == 0;
										if (flag62)
										{
											this.dataplot.ValveActivityTimeAxis.Add(list20.Last<DateTime>());
										}
										double totalSeconds2 = (list20.Last<DateTime>() - dateTime.Date).TotalSeconds;
										bool flag63 = drawWithZeros;
										if (flag63)
										{
											item = default(Point);
											bool flag64 = activities2.Count > 0;
											if (flag64)
											{
												item.Y = 0.0;
												item.X = activities2.Last<Point>().X;
												activities2.Add(item);
											}
											item.X = totalSeconds2;
											item.Y = 0.0;
											activities2.Add(item);
										}
										num9++;
									}
									bool flag65 = !list20.Contains(dateTime3.Date);
									if (flag65)
									{
										list20.Add(dateTime3.Date);
										bool flag66 = num9 % num10 == 0;
										if (flag66)
										{
											this.dataplot.ValveActivityTimeAxis.Add(list20.Last<DateTime>());
										}
										num9++;
									}
								}
							}
							double totalSeconds3 = (dateTime3 - dateTime.Date).TotalSeconds;
							item = default(Point);
							item.X = totalSeconds3;
							bool flag67 = !drawWithZeros;
							if (flag67)
							{
								bool flag68 = !flag;
								if (flag68)
								{
									bool flag69 = activities2.Count > 0;
									if (flag69)
									{
										item.Y = activities2.Last<Point>().Y;
										activities2.Add(item);
									}
									else
									{
										bool flag70 = num11 >= 0.0;
										if (flag70)
										{
											Point item2 = default(Point);
											item2.Y = num11;
											item2.X = 0.0;
											activities2.Add(item2);
											item.Y = num11;
											activities2.Add(item);
										}
									}
								}
							}
							bool flag71 = dataName.Equals(this.viewModel.StrSwitchOnCycles.LocalizedText) || dataName.Equals(this.viewModel.StrArcs.LocalizedText);
							if (flag71)
							{
								bool flag72 = activities2.Count > 0;
								if (flag72)
								{
									item.Y = activities2.Last<Point>().Y;
									activities2.Add(item);
								}
								item.Y = (double)(m + 1);
								activities2.Add(item);
							}
							else
							{
								bool flag73 = list2[m].Length > 0;
								if (flag73)
								{
									double num12 = 0.0;
									double.TryParse(list2[m].Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out num12);
									bool flag74 = num12 < 0.0;
									if (flag74)
									{
										num12 = 0.0;
									}
									bool flag75 = flag && dataName.Contains(this.viewModel.StrTime.LocalizedText);
									if (flag75)
									{
										num12 /= 60.0;
									}
									bool flag76 = activities2.Count > 0 && !flag;
									if (flag76)
									{
										bool flag77 = activities2.Last<Point>().Y == 0.0;
										if (flag77)
										{
											item.Y = 0.0;
											activities2.Add(item);
										}
									}
									item.Y = num12;
									bool flag78 = flag;
									if (flag78)
									{
										item.X += num8 / 2.0;
										bool flag79 = item.Y > (double)num4;
										if (flag79)
										{
											num4 = Convert.ToInt32(item.Y);
										}
									}
									activities2.Add(item);
									bool flag80 = item.Y > (double)num4;
									if (flag80)
									{
										num4 = Convert.ToInt32(item.Y);
									}
									bool flag81 = num >= 2;
									if (flag81)
									{
										double.TryParse(list3[m].Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out num12);
										bool flag82 = num12 < 0.0;
										if (flag82)
										{
											num12 = 0.0;
										}
										bool flag83 = flag && dataName.Contains(this.viewModel.StrTime.LocalizedText);
										if (flag83)
										{
											num12 /= 60.0;
										}
										bool flag84 = flag;
										if (flag84)
										{
											item.X += num8;
										}
										item.Y = num12;
										activities3.Add(item);
										bool flag85 = item.Y > (double)num4;
										if (flag85)
										{
											num4 = Convert.ToInt32(item.Y);
										}
									}
									bool flag86 = num >= 3;
									if (flag86)
									{
										double.TryParse(list4[m].Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out num12);
										bool flag87 = num12 < 0.0;
										if (flag87)
										{
											num12 = 0.0;
										}
										bool flag88 = flag && dataName.Contains(this.viewModel.StrTime.LocalizedText);
										if (flag88)
										{
											num12 /= 60.0;
										}
										bool flag89 = flag;
										if (flag89)
										{
											item.X += num8;
										}
										item.Y = num12;
										activities4.Add(item);
										bool flag90 = item.Y > (double)num4;
										if (flag90)
										{
											num4 = Convert.ToInt32(item.Y);
										}
									}
									bool flag91 = num >= 4;
									if (flag91)
									{
										double.TryParse(list5[m].Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out num12);
										bool flag92 = num12 < 0.0;
										if (flag92)
										{
											num12 = 0.0;
										}
										bool flag93 = flag && dataName.Contains(this.viewModel.StrTime.LocalizedText);
										if (flag93)
										{
											num12 /= 60.0;
										}
										bool flag94 = flag;
										if (flag94)
										{
											item.X += num8;
										}
										item.Y = num12;
										activities5.Add(item);
										bool flag95 = item.Y > (double)num4;
										if (flag95)
										{
											num4 = Convert.ToInt32(item.Y);
										}
									}
								}
								else
								{
									item.Y = 0.0;
									activities2.Add(item);
								}
							}
							bool flag96 = item.Y > (double)num4;
							if (flag96)
							{
								num4 = Convert.ToInt32(item.Y);
							}
						}
						else
						{
							bool flag97 = dateTime3 < dateTime;
							if (flag97)
							{
								bool flag98 = !dataName.Equals(this.viewModel.StrSwitchOnCycles.LocalizedText) && !dataName.Equals(this.viewModel.StrArcs.LocalizedText);
								if (flag98)
								{
									double.TryParse(list2[m].Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out num11);
								}
								else
								{
									num11 = (double)(m + 1);
								}
							}
						}
					}
				}
				else
				{
					for (int n = 0; n < list2.Count; n++)
					{
						item = default(Point);
						double num13 = 0.0;
						bool flag99 = list2[n].Equals("");
						if (flag99)
						{
							num13 = 0.0;
						}
						else
						{
							double.TryParse(list2[n].Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out num13);
						}
						bool flag100 = num13 < 0.0;
						if (flag100)
						{
							num13 = 0.0;
						}
						item.X = (double)n + 0.5;
						item.Y = num13;
						bool flag101 = item.Y > (double)num4;
						if (flag101)
						{
							num4 = Convert.ToInt32(item.Y);
						}
						activities2.Add(item);
					}
				}
				bool flag102 = activities2.Count > 0;
				if (flag102)
				{
					bool flag103 = !drawWithZeros && !flag;
					if (flag103)
					{
						item.Y = activities2.Last<Point>().Y;
						item.X = num6;
						activities2.Add(item);
					}
					else
					{
						item.X = activities2.Last<Point>().X;
						item.Y = 0.0;
						activities2.Add(item);
					}
				}
				else
				{
					bool flag104 = !drawWithZeros && !flag;
					if (flag104)
					{
						item.X = 0.0;
						item.Y = num11;
						activities2.Add(item);
						item.X = num6;
						item.Y = num11;
						activities2.Add(item);
						bool flag105 = item.Y > (double)num4;
						if (flag105)
						{
							num4 = Convert.ToInt32(item.Y);
						}
					}
					list20.Add(dateTime);
					this.dataplot.ValveActivityTimeAxis.Add(list20.First<DateTime>());
					num9 = 1;
				}
				bool flag106 = list20.Count > 0;
				if (flag106)
				{
					bool flag107 = list20.Last<DateTime>().Date < dateTime2.AddDays(1.0);
					if (flag107)
					{
						while (list20.Last<DateTime>().AddDays(1.0) <= dateTime2.AddDays(1.0))
						{
							list20.Add(list20.Last<DateTime>().AddDays(1.0));
							bool flag108 = num9 % num10 == 0;
							if (flag108)
							{
								this.dataplot.ValveActivityTimeAxis.Add(list20.Last<DateTime>());
							}
							double totalSeconds4 = (list20.Last<DateTime>() - dateTime.Date).TotalSeconds;
							bool flag109 = drawWithZeros;
							if (flag109)
							{
								item = default(Point);
								item.X = totalSeconds4;
								item.Y = 0.0;
								activities2.Add(item);
							}
							num9++;
						}
					}
					else
					{
						list20.Add(list20.Last<DateTime>().AddDays(1.0));
					}
				}
				double num14 = -1.0;
				for (int num15 = 0; num15 < list15.Count; num15++)
				{
					bool flag110 = !list16[num15].Equals("-") && !list16[num15].Equals("");
					if (flag110)
					{
						DateTime dateTime4 = DateTime.Parse(list15[num15], provider);
						double.TryParse(list16[num15].Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out num11);
						bool flag111 = dateTime4 >= dateTime && dateTime4 <= dateTime2.AddDays(1.0);
						if (flag111)
						{
							bool flag112 = num14 >= 0.0 && activities1.Count == 0;
							if (flag112)
							{
								item = default(Point);
								item.X = 0.0;
								item.Y = num14;
								activities1.Add(item);
								bool flag113 = item.Y > (double)num4;
								if (flag113)
								{
									num4 = Convert.ToInt32(item.Y);
								}
							}
							bool flag114 = list20.Contains(dateTime4.Date);
							if (flag114)
							{
								double totalSeconds5 = (dateTime4 - dateTime.Date).TotalSeconds;
								item = default(Point);
								item.X = totalSeconds5;
								bool flag115 = activities1.Count<Point>() > 0;
								if (flag115)
								{
									item.Y = activities1.Last<Point>().Y;
									activities1.Add(item);
								}
								bool flag116 = list16[num15].Length > 0;
								if (flag116)
								{
									bool flag117 = num11 < 0.0;
									if (flag117)
									{
										num11 = 0.0;
									}
									item.Y = num11;
									activities1.Add(item);
								}
								else
								{
									item.Y = 0.0;
									activities1.Add(item);
								}
								bool flag118 = item.Y > (double)num4;
								if (flag118)
								{
									num4 = Convert.ToInt32(item.Y);
								}
							}
						}
						else
						{
							bool flag119 = dateTime4 < dateTime;
							if (flag119)
							{
								num14 = num11;
							}
						}
					}
					else
					{
						DateTime dateTime5 = DateTime.Parse(list15[num15], provider);
						bool flag120 = dateTime5 >= dateTime && dateTime5 <= dateTime2.AddDays(1.0);
						if (flag120)
						{
							bool flag121 = num14 >= 0.0 && activities1.Count == 0;
							if (flag121)
							{
								item = default(Point);
								item.X = 0.0;
								item.Y = num14;
								activities1.Add(item);
								bool flag122 = item.Y > (double)num4;
								if (flag122)
								{
									num4 = Convert.ToInt32(item.Y);
								}
							}
							bool flag123 = list20.Contains(dateTime5.Date);
							if (flag123)
							{
								double totalSeconds6 = (dateTime5 - dateTime.Date).TotalSeconds;
								item = default(Point);
								item.X = totalSeconds6;
								bool flag124 = activities1.Count > 0;
								if (flag124)
								{
									item.Y = activities1.Last<Point>().Y;
									activities1.Add(item);
								}
								item.Y = 0.0;
								activities1.Add(item);
							}
						}
						else
						{
							bool flag125 = dateTime5 < dateTime;
							if (flag125)
							{
								num14 = 0.0;
							}
						}
					}
				}
				bool flag126 = flag38 && activities1.Count > 0;
				if (flag126)
				{
					item = default(Point);
					item.Y = activities1.Last<Point>().Y;
					double totalSeconds7 = (list20.Last<DateTime>() - dateTime.Date).TotalSeconds;
					item.X = totalSeconds7;
					activities1.Add(item);
				}
				bool flag127 = activities1.Count <= 0;
				if (flag127)
				{
					bool flag128 = num14 >= 0.0;
					if (flag128)
					{
						item.X = 0.0;
						item.Y = num14;
						activities1.Add(item);
						item.X = num6;
						item.Y = num14;
						activities1.Add(item);
						bool flag129 = item.Y > (double)num4;
						if (flag129)
						{
							num4 = Convert.ToInt32(item.Y);
						}
					}
				}
				bool flag130 = flag;
				if (flag130)
				{
					int num16 = 8;
					bool flag131 = list20.Count < 3;
					if (flag131)
					{
						num16 = 20;
					}
					bool flag132 = list20.Count > 20;
					if (flag132)
					{
						num16 = 4;
					}
					for (int num17 = 0; num17 < this.dataplot.Graphs.Count; num17++)
					{
						this.dataplot.Graphs[num17].GraphType = GraphType.Bar;
						this.dataplot.Graphs[num17].StrokeThickness = (double)num16;
					}
					this.dataplot.Graphs[1].GraphType = GraphType.Linear;
					this.dataplot.Graphs[1].StrokeThickness = 2.0;
					bool flag133 = !this.viewModel.Col2 && !dataName.Contains(this.viewModel.StrTime.LocalizedText) && !dataName.Contains("SVT") && !dataName.Contains(this.viewModel.StrOverview.LocalizedText);
					if (flag133)
					{
						this.viewModel.LimitVisible = true;
					}
					else
					{
						this.viewModel.LimitVisible = false;
					}
					drawWithZeros = false;
				}
				else
				{
					for (int num18 = 0; num18 < this.dataplot.Graphs.Count; num18++)
					{
						this.dataplot.Graphs[num18].GraphType = GraphType.Linear;
						this.dataplot.Graphs[num18].StrokeThickness = 2.0;
					}
					this.viewModel.Col2 = false;
					this.viewModel.Col3 = false;
					this.viewModel.Col4 = false;
					bool flag134 = !dataName.Contains(this.viewModel.StrValveActivity.LocalizedText);
					if (flag134)
					{
						this.viewModel.LimitVisible = true;
					}
				}
				bool flag135 = list20.Count > 0;
				if (flag135)
				{
					this.viewModel.MaxX = num6;
					this.viewModel.TicksXMinor = num7;
					this.viewModel.TicksX = num7 * (double)num10;
					bool flag136 = flag && dataName.Contains(this.viewModel.StrTime.LocalizedText);
					if (flag136)
					{
						this.viewModel.MaxY = 24;
					}
					else
					{
						this.viewModel.MaxY = num4;
					}
					this.viewModel.setMaxYInit = true;
					this.viewModel.setTicksY();
					this.viewModel.TextDate1 = list20.First<DateTime>().ToString("dd-MM-yyyy");
					this.viewModel.TextDate2 = list20[list20.Count<DateTime>() - 2].ToString("dd-MM-yyyy");
					this.viewModel.Date1 = list20.First<DateTime>();
					this.viewModel.Date2 = list20.Last<DateTime>();
					Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
					{
						this.viewModel.Data1 = activities1;
						this.viewModel.Data2 = activities2;
						this.viewModel.Data3 = activities3;
						this.viewModel.Data4 = activities4;
						this.viewModel.Data5 = activities5;
						this.dataplot.AutoScaleDataPlot(this.viewModel.MaxYInit, this.viewModel.TicksYInit, this.viewModel.TicksXMinor, this.viewModel.TicksX, this.viewModel.MaxX);
					}));
				}
				else
				{
					bool flag137 = flag2;
					if (flag137)
					{
						this.viewModel.MaxX = (double)num2;
						this.viewModel.TicksXMinor = 1.0;
						this.viewModel.TicksX = 1.0;
						this.viewModel.MaxY = num4;
						this.viewModel.setMaxYInit = true;
						this.viewModel.setTicksY();
						this.viewModel.TextDate1 = this.viewModel.Date1.ToString("dd-MM-yyyy");
						this.viewModel.TextDate2 = this.viewModel.Date2.ToString("dd-MM-yyyy");
						Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
						{
							this.viewModel.Data1 = activities1;
							this.viewModel.Data2 = activities2;
							this.viewModel.Data3 = activities3;
							this.viewModel.Data4 = activities4;
							this.viewModel.Data5 = activities5;
							this.dataplot.AutoScaleDataPlot(this.viewModel.MaxYInit, this.viewModel.TicksYInit, this.viewModel.TicksXMinor, this.viewModel.TicksX, this.viewModel.MaxX);
						}));
					}
				}
			}
			else
			{
				List<Point> activities1 = new List<Point>();
				List<Point> activities2 = new List<Point>();
				bool flag138 = flag2;
				if (flag138)
				{
					this.viewModel.TextDate1 = this.viewModel.Date1.ToString("dd-MM-yyyy");
					this.viewModel.TextDate2 = this.viewModel.Date2.ToString("dd-MM-yyyy");
				}
				Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
				{
					this.viewModel.Data1 = activities1;
					this.viewModel.Data2 = activities2;
					this.dataplot.AutoScaleDataPlot(this.viewModel.MaxYInit, this.viewModel.TicksYInit, this.viewModel.TicksXMinor, this.viewModel.TicksX, this.viewModel.MaxX);
				}));
			}
		}

		// Token: 0x06000D11 RID: 3345 RVA: 0x000501F0 File Offset: 0x0004E3F0
		private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			bool flag = this.viewModel.NameData.Equals(this.viewModel.StrOverview.LocalizedText);
			if (flag)
			{
				this.viewModel.Date1 = DateTime.Now;
				this.viewModel.Date2 = this.viewModel.Date1;
			}
			this.DrawDataPlots(true, this.viewModel.NameData, false);
		}

		// Token: 0x06000D12 RID: 3346 RVA: 0x00050260 File Offset: 0x0004E460
		private void DataPlot_Unloaded(object sender, RoutedEventArgs e)
		{
			this.viewModel.UpdateDataPlots -= this.updateDataPlots;
		}

		// Token: 0x06000D13 RID: 3347 RVA: 0x0005027B File Offset: 0x0004E47B
		private void UpButton_Click(object sender, RoutedEventArgs e)
		{
			this.dataplot.Up();
		}

		// Token: 0x06000D14 RID: 3348 RVA: 0x0005028A File Offset: 0x0004E48A
		private void DownButton_Click(object sender, RoutedEventArgs e)
		{
			this.dataplot.Down();
		}

		// Token: 0x06000D15 RID: 3349 RVA: 0x0005029C File Offset: 0x0004E49C
		private void Button_ResetTime(object sender, RoutedEventArgs e)
		{
			bool flag = this.viewModel.NameData.Equals(this.viewModel.StrOverview.LocalizedText);
			if (flag)
			{
				this.viewModel.Date1 = DateTime.Now;
				this.viewModel.Date2 = this.viewModel.Date1;
			}
			else
			{
				this.viewModel.Date2 = this.viewModel.LastDate.Date;
				bool barDiagramm = this.viewModel.BarDiagramm;
				if (barDiagramm)
				{
					this.viewModel.Date1 = this.viewModel.Date2.AddMonths(-1);
				}
				else
				{
					this.viewModel.Date1 = this.viewModel.Date2.AddMonths(-6);
				}
				bool flag2 = this.viewModel.Date1.Date < this.viewModel.FirstDate.Date;
				if (flag2)
				{
					this.viewModel.Date1 = this.viewModel.FirstDate;
				}
			}
			this.DrawDataPlots(true, this.viewModel.SelectionComboBox, true);
		}

		// Token: 0x06000D16 RID: 3350 RVA: 0x000503CC File Offset: 0x0004E5CC
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/monitoring/ctrldataplots.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x06000D17 RID: 3351 RVA: 0x00050404 File Offset: 0x0004E604
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				((CtrlDataPlots)target).Loaded += this.DataPlot_Loaded;
				((CtrlDataPlots)target).Unloaded += this.DataPlot_Unloaded;
				break;
			case 2:
				this.graphesValveActivity = (Grid)target;
				break;
			case 3:
				((ComboBox)target).SelectionChanged += this.ComboBox_SelectionChanged;
				break;
			case 4:
				((TextBox)target).MouseDown += new MouseButtonEventHandler(this.date1_Clicked);
				break;
			case 5:
				((TextBox)target).MouseDown += this.date2_Clicked;
				break;
			case 6:
				((Button)target).Click += this.Button_ResetTime;
				break;
			case 7:
				((Button)target).Click += this.UpButton_Click;
				break;
			case 8:
				((Button)target).Click += this.DownButton_Click;
				break;
			case 9:
				this.dataplot = (SesoPlot)target;
				break;
			case 10:
				this.legendValveActivity = (Grid)target;
				break;
			default:
				this._contentLoaded = true;
				break;
			}
		}

		// Token: 0x04000886 RID: 2182
		private DataPlotsViewModel viewModel;

		// Token: 0x04000887 RID: 2183
		private ParameterHandling parHandling;

		// Token: 0x04000888 RID: 2184
		private DBConnection dbConnection;

		// Token: 0x04000889 RID: 2185
		public DockPanel PanelSubButtons;

		// Token: 0x0400088A RID: 2186
		private DataModesMonitoring mode;

		// Token: 0x0400088B RID: 2187
		private DBDataLogs dBDataLogs;

		// Token: 0x0400088C RID: 2188
		internal Grid graphesValveActivity;

		// Token: 0x0400088D RID: 2189
		internal SesoPlot dataplot;

		// Token: 0x0400088E RID: 2190
		internal Grid legendValveActivity;

		// Token: 0x0400088F RID: 2191
		private bool _contentLoaded;
	}
}
