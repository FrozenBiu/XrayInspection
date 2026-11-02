using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Markup;
using System.Windows.Threading;
using DatabaseConnection;
using Definitions;
using Microsoft.Win32;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x0200022C RID: 556
	public class CtrlSendImage : System.Windows.Controls.UserControl, INotifyPropertyChanged, IComponentConnector
	{
		// Token: 0x17000B5E RID: 2910
		// (get) Token: 0x060029C5 RID: 10693 RVA: 0x0011477D File Offset: 0x0011297D
		// (set) Token: 0x060029C6 RID: 10694 RVA: 0x00114785 File Offset: 0x00112985
		public List<System.Windows.Controls.Button> Buttons { get; set; }

		// Token: 0x140000DD RID: 221
		// (add) Token: 0x060029C7 RID: 10695 RVA: 0x00114790 File Offset: 0x00112990
		// (remove) Token: 0x060029C8 RID: 10696 RVA: 0x001147C8 File Offset: 0x001129C8
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x060029C9 RID: 10697 RVA: 0x00114800 File Offset: 0x00112A00
		public CtrlSendImage(ImageDataTransfer imageTransfer, ParameterHandling parHandling, DBConnection dbConnection, StoreImageData imageArchiver, ProductManager productManager, TestcardSimService testcardsimservice)
		{
			this.imageTransfer = imageTransfer;
			this.parHandling = parHandling;
			this.dbConnection = dbConnection;
			this.imageArchiver = imageArchiver;
			this.productManager = productManager;
			this.testcardsimservice = testcardsimservice;
			bool flag = true;
			dbConnection.GetDemoMode(ref flag);
			this.InitializeComponent();
			this.InitializeButtons();
			base.DataContext = this;
			imageTransfer.FrameArrived += this.imageTransfer_FrameArrived;
			MachineType machineType = MachineType.RayconXRS;
			dbConnection.GetMachineType(ref machineType);
			this.showFilterParams = false;
			this.toggleBtn.Visibility = Visibility.Hidden;
			bool flag2 = ((machineType == MachineType.RayconEX2A || machineType == MachineType.RayconDPlus) && productManager.CurrentLoadedProduct.IsSingleProduct) || flag;
			if (flag2)
			{
				this.showFilterParams = true;
				this.toggleBtn.Visibility = Visibility.Visible;
			}
			dbConnection.GetMachineMode(ref this.machineMode);
			bool flag3 = this.machineMode == MachineMode.Bulk;
			if (flag3)
			{
				this.oldstateSendImage = parHandling.ReadPar("/filter/acquisition/SendImage/Value");
				this.oldstateSaveImage = parHandling.ReadPar("/filter/acquisition/SaveImage/Value");
				this.oldstateShowImage = parHandling.ReadPar("/filter/acquisition/ShowImage/Value");
				parHandling.WriteValue("/filter/acquisition/SendImage/Value", "1");
				parHandling.WriteValue("/filter/acquisition/SaveImage/Value", "1");
				parHandling.WriteValue("/filter/acquisition/ShowImage/Value", "1");
			}
			bool flag4 = testcardsimservice != null;
			if (flag4)
			{
				testcardsimservice.MonitoringSensIsActive = false;
			}
			this.ShowBtnToggleAIFilters = (parHandling.ReadPar(Definition.anomalyDetectionEnabled) == "1" || parHandling.ReadPar(Definition.objectDetectionEnabled) == "1");
		}

		// Token: 0x060029CA RID: 10698 RVA: 0x001149F0 File Offset: 0x00112BF0
		private void imageTransfer_FrameArrived(object sender, ReceiveEventArgs e)
		{
			bool flag = e.Image.Type == ImageType.Demo || e.Image.Type == ImageType.Debug || e.Image.Type == ImageType.PerformanceTest || e.Image.Type == ImageType.TestcardSimulation || e.Image.Type == ImageType.TrainAI;
			if (flag)
			{
				bool flag2 = this.parHandling.ReadPar("/filter/acquisition/ShowImage/Value") != "0" && (this.parHandling.ReadPar("/filter/acquisition/ShowOnlyIfContaminant/Value") != "1" || e.Image.HasErrors());
				if (flag2)
				{
					this.imagebox.ViewXRSImage(e.Image);
				}
				string strProcessingTime = this.parHandling.ReadPar(Definition.processingTimePath);
				string strSendTime = this.parHandling.ReadPar(Definition.sendTimePath);
				string strPaintRectanglesTime = this.parHandling.ReadPar(Definition.paintRectanglesTimePath);
				string strTotalTime = this.parHandling.ReadPar(Definition.totalTimeImgProcPath);
				string strProductsPerMinute = this.parHandling.ReadPar(Definition.productsPerMinutePath);
				bool flag3 = System.Windows.Application.Current != null;
				if (flag3)
				{
					System.Windows.Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(delegate()
					{
						bool flag4 = this.parHandling.ReadPar(Definition.testActivePath) != "1";
						if (flag4)
						{
							this.txtProdPerMinute.Visibility = Visibility.Collapsed;
							this.txtProdPerMinuteValue.Visibility = Visibility.Collapsed;
						}
						else
						{
							this.txtProdPerMinute.Visibility = Visibility.Visible;
							this.txtProdPerMinuteValue.Visibility = Visibility.Visible;
						}
						this.txtProcessingTime.Text = strProcessingTime;
						this.txtSendTime.Text = strSendTime;
						this.txtPaintRectanglesTime.Text = strPaintRectanglesTime;
						this.txtTotalTime.Text = strTotalTime;
						this.txtProdPerMinuteValue.Text = strProductsPerMinute;
						this.gridTimes.Visibility = Visibility.Visible;
						bool flag6;
						if (this.showBtnToggleAIFilters && this.showFilterParams)
						{
							bool? isChecked = this.toggleBtnAIFilters.IsChecked;
							bool flag5 = true;
							flag6 = (isChecked.GetValueOrDefault() == flag5 & isChecked != null);
						}
						else
						{
							flag6 = false;
						}
						bool flag7 = flag6;
						if (flag7)
						{
							this.ReadAIFilterParameter();
						}
						else
						{
							bool flag8 = this.showFilterParams;
							if (flag8)
							{
								this.ReadFilterParameter();
							}
						}
					}));
				}
			}
		}

		// Token: 0x060029CB RID: 10699 RVA: 0x00114B58 File Offset: 0x00112D58
		private void InitializeButtons()
		{
			this.Buttons = new List<System.Windows.Controls.Button>();
			System.Windows.Controls.Button button = new System.Windows.Controls.Button();
			button.Content = ImageConverter.ToImage("/img/simulateimage_small.png");
			button.Style = (base.FindResource("defaultButtonStyle") as Style);
			button.Click += new RoutedEventHandler(this.btnSend_Click);
			this.Buttons.Add(button);
			System.Windows.Controls.Button button2 = new System.Windows.Controls.Button();
			button2.Content = ImageConverter.ToImage("/img/simulateimages_small.png");
			button2.Style = (base.FindResource("defaultButtonStyle") as Style);
			button2.Click += new RoutedEventHandler(this.btnSendAll_Click);
			this.Buttons.Add(button2);
		}

		// Token: 0x060029CC RID: 10700 RVA: 0x00114C10 File Offset: 0x00112E10
		private void btnSend_Click(object sender, EventArgs e)
		{
			Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
			bool value = openFileDialog.ShowDialog().Value;
			if (value)
			{
				bool flag = openFileDialog.FileName.EndsWith(XRSImage.fileEnding);
				if (flag)
				{
					bool flag2 = !this.imageTransfer.SendImage(openFileDialog.FileName);
					if (flag2)
					{
						Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtTransmissionFailed"), DialogStyle.Error);
					}
				}
			}
		}

		// Token: 0x060029CD RID: 10701 RVA: 0x00114C84 File Offset: 0x00112E84
		private void btnSendAll_Click(object sender, EventArgs e)
		{
			FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
			bool flag = CtrlSendImage.lastRootFolder != null;
			if (flag)
			{
				folderBrowserDialog.SelectedPath = CtrlSendImage.lastRootFolder;
			}
			else
			{
				string text = "";
				this.dbConnection.GetUsbDriveLetter(ref text);
				bool flag2 = Directory.Exists(text);
				if (flag2)
				{
					folderBrowserDialog.SelectedPath = text + "\\";
				}
			}
			bool flag3 = folderBrowserDialog.ShowDialog() == DialogResult.OK;
			if (flag3)
			{
				CtrlSendImage.lastRootFolder = folderBrowserDialog.SelectedPath;
				string[] files = Directory.GetFiles(folderBrowserDialog.SelectedPath);
				this.currentXriFiles = (from file in files
				where file.EndsWith(XRSImage.fileEnding)
				select file).ToArray<string>();
				bool flag4 = this.currentXriFiles.Length != 0;
				if (flag4)
				{
					this.parHandling.WriteValue(Definition.testIntervalPath, "200", false);
					this.parHandling.WriteValue(Definition.testActivePath, "0", false);
					this.parHandling.WriteValue(Definition.testImageIndexPath, "0", false);
					this.parHandling.WriteValue(Definition.clearTestImagesPath, "1", false);
					this.parHandling.WriteValue(Definition.testUseLocalImagesPath, "0", false);
					new Thread(new ThreadStart(this.WaitForRemoteDeletionAndSend))
					{
						Name = "WaitForDeleteAndSend"
					}.Start();
				}
			}
		}

		// Token: 0x060029CE RID: 10702 RVA: 0x00114DF4 File Offset: 0x00112FF4
		private void WaitForRemoteDeletionAndSend()
		{
			CtrlSendImage.<>c__DisplayClass33_0 CS$<>8__locals1 = new CtrlSendImage.<>c__DisplayClass33_0();
			CS$<>8__locals1.deleteProg = null;
			bool flag = System.Windows.Application.Current != null;
			if (flag)
			{
				System.Windows.Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
				{
					CS$<>8__locals1.deleteProg = new Progress("Initializing...", false);
					CS$<>8__locals1.deleteProg.Show();
					CS$<>8__locals1.deleteProg.progBar.Value = 10.0;
				}));
			}
			int i;
			int j;
			for (i = 0; i < 5; i = j + 1)
			{
				bool flag2 = this.parHandling.ReadPar(Definition.testNRemoteImagesPath) != "0";
				if (!flag2)
				{
					bool flag3 = System.Windows.Application.Current != null;
					if (flag3)
					{
						Dispatcher dispatcher = System.Windows.Application.Current.Dispatcher;
						DispatcherPriority priority = DispatcherPriority.Background;
						Action method;
						if ((method = CS$<>8__locals1.<>9__3) == null)
						{
							method = (CS$<>8__locals1.<>9__3 = delegate()
							{
								CS$<>8__locals1.deleteProg.progBar.Value = 100.0;
							});
						}
						dispatcher.Invoke(priority, method);
					}
					break;
				}
				Thread.Sleep(1000);
				bool flag4 = System.Windows.Application.Current != null;
				if (flag4)
				{
					System.Windows.Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
					{
						CS$<>8__locals1.deleteProg.progBar.Value = (double)((i + 1) * 20);
					}));
				}
				j = i;
			}
			this.SendImages();
			bool flag5 = System.Windows.Application.Current != null;
			if (flag5)
			{
				System.Windows.Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
				{
					CS$<>8__locals1.deleteProg.Close();
				}));
			}
			this.parHandling.WriteValue(Definition.testIntervalPath, "0", false);
			this.parHandling.WriteValue(Definition.testActivePath, "1", false);
		}

		// Token: 0x060029CF RID: 10703 RVA: 0x00114F8C File Offset: 0x0011318C
		private void SendImages()
		{
			for (int i = 0; i < this.currentXriFiles.Length; i++)
			{
				XRSImage image = this.imageArchiver.GetImage(this.currentXriFiles[i]);
				image.Type = ImageType.PerformanceTest;
				this.imageTransfer.SendImage(image);
			}
		}

		// Token: 0x060029D0 RID: 10704 RVA: 0x00114FDC File Offset: 0x001131DC
		private void CtrlSendImage_Unloaded(object sender, RoutedEventArgs e)
		{
			this.parHandling.WriteValue(Definition.testIntervalPath, "200", false);
			this.parHandling.WriteValue(Definition.testActivePath, "0", false);
			this.parHandling.WriteValue(Definition.clearTestImagesPath, "1", false);
			this.imageTransfer.FrameArrived -= this.imageTransfer_FrameArrived;
			BindingOperations.ClearAllBindings(this);
			base.DataContext = null;
			bool flag = this.testcardsimservice != null;
			if (flag)
			{
				this.testcardsimservice.MonitoringSensIsActive = true;
			}
			bool flag2 = this.machineMode == MachineMode.Bulk;
			if (flag2)
			{
				this.parHandling.WriteValue("/filter/acquisition/SendImage/Value", this.oldstateSendImage);
				this.parHandling.WriteValue("/filter/acquisition/SaveImage/Value", this.oldstateSaveImage);
				this.parHandling.WriteValue("/filter/acquisition/ShowImage/Value", this.oldstateShowImage);
			}
		}

		// Token: 0x060029D1 RID: 10705 RVA: 0x001150C4 File Offset: 0x001132C4
		private void toggleBtn_Click(object sender, RoutedEventArgs e)
		{
			bool? isChecked = this.toggleBtn.IsChecked;
			bool flag = true;
			bool flag2 = isChecked.GetValueOrDefault() == flag & isChecked != null;
			if (flag2)
			{
				this.GMain.ColumnDefinitions[0].Width = new GridLength(0.0, GridUnitType.Auto);
			}
			else
			{
				this.GMain.ColumnDefinitions[0].Width = new GridLength(60.0, GridUnitType.Pixel);
			}
		}

		// Token: 0x060029D2 RID: 10706 RVA: 0x00115148 File Offset: 0x00113348
		private void ReadFilterParameter()
		{
			this.txtFilter.Text = "";
			this.txtEnable.Text = "";
			this.txtMatches.Text = "";
			this.txtRadius.Text = "";
			this.txtMinSize.Text = "";
			this.txtContrast.Text = "";
			this.txtTriggered.Text = "";
			for (int i = 1; i <= 30; i++)
			{
				bool flag = i <= 30;
				if (flag)
				{
					this.txtFilter.Text = string.Concat(new string[]
					{
						this.txtFilter.Text,
						"Comp",
						i.ToString(),
						"   ",
						Environment.NewLine
					});
					this.txtEnable.Text = this.txtEnable.Text + this.parHandling.ReadPar(string.Concat(new string[]
					{
						Definition.filterMainPath,
						Definition.comparerSubPath,
						i.ToString(),
						"/Enabled/",
						Definition.strValue
					})) + "   " + Environment.NewLine;
					this.txtMatches.Text = this.txtMatches.Text + this.parHandling.ReadPar(string.Concat(new string[]
					{
						Definition.filterMainPath,
						Definition.comparerSubPath,
						i.ToString(),
						"/Matches/",
						Definition.strValue
					})) + "   " + Environment.NewLine;
					this.txtRadius.Text = this.txtRadius.Text + this.parHandling.ReadPar(string.Concat(new string[]
					{
						Definition.filterMainPath,
						Definition.comparerSubPath,
						i.ToString(),
						"/Radius/",
						Definition.strValue
					})) + "   " + Environment.NewLine;
					this.txtMinSize.Text = this.txtMinSize.Text + this.parHandling.ReadPar(string.Concat(new string[]
					{
						Definition.filterMainPath,
						Definition.comparerSubPath,
						i.ToString(),
						"/MinSize/",
						Definition.strValue
					})) + "   " + Environment.NewLine;
					this.txtContrast.Text = this.txtContrast.Text + this.parHandling.ReadPar(string.Concat(new string[]
					{
						Definition.filterMainPath,
						Definition.comparerSubPath,
						i.ToString(),
						"/Contrast/",
						Definition.strValue
					})) + "   " + Environment.NewLine;
					this.txtTriggered.Text = this.txtTriggered.Text + this.parHandling.ReadPar(string.Concat(new string[]
					{
						Definition.filterMainPath,
						Definition.comparerSubPath,
						i.ToString(),
						"/Triggered/",
						Definition.strValue
					})) + "   " + Environment.NewLine;
				}
			}
			this.txtFilter.Text = string.Concat(new string[]
			{
				this.txtFilter.Text,
				"Thr   ",
				Environment.NewLine,
				"ThrG   ",
				Environment.NewLine
			});
			this.txtEnable.Text = this.txtEnable.Text + this.parHandling.ReadPar(Definition.filterMainPath + Definition.thresholdSubPath + "/Enabled/" + Definition.strValue) + "   " + Environment.NewLine;
			this.txtEnable.Text = this.txtEnable.Text + this.parHandling.ReadPar(Definition.filterMainPath + Definition.thresholdGlobalSubPath + "/Enabled/" + Definition.strValue) + "   " + Environment.NewLine;
			this.txtContrast.Text = this.txtContrast.Text + this.parHandling.ReadPar(Definition.filterMainPath + Definition.thresholdSubPath + "/HighThreshold/" + Definition.strValue) + "   " + Environment.NewLine;
			this.txtContrast.Text = this.txtContrast.Text + this.parHandling.ReadPar(Definition.filterMainPath + Definition.thresholdGlobalSubPath + "/HighThreshold/" + Definition.strValue) + "   " + Environment.NewLine;
			this.txtTriggered.Text = this.txtTriggered.Text + this.parHandling.ReadPar(Definition.filterMainPath + Definition.thresholdSubPath + "/Triggered/" + Definition.strValue) + "   " + Environment.NewLine;
			this.txtTriggered.Text = this.txtTriggered.Text + this.parHandling.ReadPar(Definition.filterMainPath + Definition.thresholdGlobalSubPath + "/Triggered/" + Definition.strValue) + "   " + Environment.NewLine;
			for (int j = 1; j <= 2; j++)
			{
				this.txtMatches.Text = this.txtMatches.Text + "-   " + Environment.NewLine;
				this.txtRadius.Text = this.txtRadius.Text + "-   " + Environment.NewLine;
				this.txtMinSize.Text = this.txtMinSize.Text + "-   " + Environment.NewLine;
			}
		}

		// Token: 0x060029D3 RID: 10707 RVA: 0x00115734 File Offset: 0x00113934
		private void toggleBtnAIFilters_Click(object sender, RoutedEventArgs e)
		{
			bool? isChecked = this.toggleBtnAIFilters.IsChecked;
			bool flag = true;
			bool flag2 = isChecked.GetValueOrDefault() == flag & isChecked != null;
			if (flag2)
			{
				bool flag3 = this.parHandling.ReadPar(Definition.anomalyDetectionEnabled) == "1";
				if (flag3)
				{
					this.HeaderCol1 = "Filter";
					this.HeaderCol2 = "En";
					this.HeaderCol3 = "GrC";
					this.HeaderCol4 = "RecC";
					this.HeaderCol5 = "GrI";
					this.HeaderCol6 = "RecI";
					this.HeaderCol7 = "Trig";
				}
				else
				{
					bool flag4 = this.parHandling.ReadPar(Definition.objectDetectionEnabled) == "1";
					if (flag4)
					{
						this.HeaderCol1 = "Filter";
						this.HeaderCol2 = "En";
						this.HeaderCol3 = "Typ";
						this.HeaderCol4 = "Min";
						this.HeaderCol5 = "Max";
						this.HeaderCol6 = "Thr";
						this.HeaderCol7 = "Trig";
					}
				}
				this.ReadAIFilterParameter();
			}
			else
			{
				this.HeaderCol1 = "Filter";
				this.HeaderCol2 = "En";
				this.HeaderCol3 = "Mat";
				this.HeaderCol4 = "Rad";
				this.HeaderCol5 = "MinS";
				this.HeaderCol6 = "Con";
				this.HeaderCol7 = "Trig";
				this.ReadFilterParameter();
			}
		}

		// Token: 0x060029D4 RID: 10708 RVA: 0x001158C0 File Offset: 0x00113AC0
		private void ReadAIFilterParameter()
		{
			this.txtFilter.Inlines.Clear();
			this.txtEnable.Inlines.Clear();
			this.txtMatches.Inlines.Clear();
			this.txtRadius.Inlines.Clear();
			this.txtMinSize.Inlines.Clear();
			this.txtContrast.Inlines.Clear();
			this.txtTriggered.Inlines.Clear();
			bool flag = this.parHandling.ReadPar(Definition.anomalyDetectionEnabled) == "1";
			if (flag)
			{
				this.AddFormattedLine(this.txtFilter, "XA", new double?(11.5));
				this.AddFormattedLine(this.txtEnable, this.parHandling.ReadPar(Definition.anomalyDetectionInferenceEnabled), new double?(11.5));
				this.AddFormattedLine(this.txtMatches, "-", new double?(11.5));
				this.AddFormattedLine(this.txtRadius, "-", new double?(11.5));
				this.AddFormattedLine(this.txtMinSize, "-", new double?(11.5));
				this.AddFormattedLine(this.txtContrast, "-", new double?(11.5));
				this.AddFormattedLine(this.txtTriggered, "-", new double?(11.5));
				for (int i = 1; i <= Definition.nAIPostproFilters; i++)
				{
					this.AddFormattedLine(this.txtFilter, "Post" + i.ToString(), new double?(11.5));
					this.AddFormattedLine(this.txtEnable, this.parHandling.ReadPar(string.Format("{0}{1}{2}{3}{4}/Enabled/{5}", new object[]
					{
						Definition.filterMainPath,
						Definition.anomalyDetectionMain,
						Definition.anomalyDetectionInference,
						Definition.aipostproFilterSubPath,
						i,
						Definition.strValue
					})), null);
					this.AddFormattedLine(this.txtMatches, this.parHandling.ReadPar(string.Format("{0}{1}{2}{3}{4}{5}/{6}", new object[]
					{
						Definition.filterMainPath,
						Definition.anomalyDetectionMain,
						Definition.anomalyDetectionInference,
						Definition.aipostproFilterSubPath,
						i,
						Definition.anomalyDetectionInferenceThresholdGroupContour,
						Definition.strValue
					})), null);
					this.AddFormattedLine(this.txtRadius, this.parHandling.ReadPar(string.Format("{0}{1}{2}{3}{4}{5}/{6}", new object[]
					{
						Definition.filterMainPath,
						Definition.anomalyDetectionMain,
						Definition.anomalyDetectionInference,
						Definition.aipostproFilterSubPath,
						i,
						Definition.anomalyDetectionInferenceThresholdReconErrorContour,
						Definition.strValue
					})) + " ", null);
					this.AddFormattedLine(this.txtMinSize, this.parHandling.ReadPar(string.Format("{0}{1}{2}{3}{4}{5}/{6}", new object[]
					{
						Definition.filterMainPath,
						Definition.anomalyDetectionMain,
						Definition.anomalyDetectionInference,
						Definition.aipostproFilterSubPath,
						i,
						Definition.anomalyDetectionInferenceThresholdGroupInside,
						Definition.strValue
					})), null);
					this.AddFormattedLine(this.txtContrast, this.parHandling.ReadPar(string.Format("{0}{1}{2}{3}{4}{5}/{6}", new object[]
					{
						Definition.filterMainPath,
						Definition.anomalyDetectionMain,
						Definition.anomalyDetectionInference,
						Definition.aipostproFilterSubPath,
						i,
						Definition.anomalyDetectionInferenceThresholdReconErrorInside,
						Definition.strValue
					})) + " ", null);
					this.AddFormattedLine(this.txtTriggered, this.parHandling.ReadPar(string.Format("{0}{1}{2}{3}{4}/Triggered/{5}", new object[]
					{
						Definition.filterMainPath,
						Definition.anomalyDetectionMain,
						Definition.anomalyDetectionInference,
						Definition.aipostproFilterSubPath,
						i,
						Definition.strValue
					})), null);
				}
				this.AddFormattedLine(this.txtFilter, "", null);
				this.AddFormattedLine(this.txtEnable, "", null);
				this.AddFormattedLine(this.txtMatches, "", null);
				this.AddFormattedLine(this.txtRadius, "", null);
				this.AddFormattedLine(this.txtMinSize, "", null);
				this.AddFormattedLine(this.txtContrast, "", null);
				this.AddFormattedLine(this.txtTriggered, "", null);
			}
			bool flag2 = this.parHandling.ReadPar(Definition.objectDetectionEnabled) == "1";
			if (flag2)
			{
				bool flag3 = this.parHandling.ReadPar(Definition.anomalyDetectionEnabled) == "1";
				if (flag3)
				{
					this.AddFormattedLine(this.txtFilter, "Filter", new double?((double)13));
					this.AddFormattedLine(this.txtEnable, "En", new double?((double)13));
					this.AddFormattedLine(this.txtMatches, "Typ", new double?((double)13));
					this.AddFormattedLine(this.txtRadius, "Min", new double?((double)13));
					this.AddFormattedLine(this.txtMinSize, "Max", new double?((double)13));
					this.AddFormattedLine(this.txtContrast, "Thr", new double?((double)13));
					this.AddFormattedLine(this.txtTriggered, "Trig", new double?((double)13));
				}
				this.AddFormattedLine(this.txtFilter, "XO", new double?(11.5));
				this.AddFormattedLine(this.txtEnable, this.parHandling.ReadPar(Definition.objectDetectionInferenceEnabled), null);
				this.AddFormattedLine(this.txtMatches, "-", null);
				this.AddFormattedLine(this.txtRadius, "-", null);
				this.AddFormattedLine(this.txtMinSize, "-", null);
				this.AddFormattedLine(this.txtContrast, "-", null);
				this.AddFormattedLine(this.txtTriggered, "-", null);
				for (int j = 1; j <= Definition.nAIObjectClasses; j++)
				{
					string a = this.parHandling.ReadPar(string.Format("{0}{1}{2}", Definition.objectDetectionClassesSubPath, j, Definition.objectDetectionClassAvailable));
					string a2 = this.parHandling.ReadPar(string.Format("{0}{1}{2}", Definition.objectDetectionClassesSubPath, j, Definition.objectDetectionClassEnabled));
					string text = (a == "1" && a2 == "1") ? "1" : "0";
					string text2 = this.parHandling.ReadPar(string.Format("{0}{1}{2}", Definition.objectDetectionClassesSubPath, j, Definition.objectDetectionType));
					string text3 = (text2 == "0") ? this.parHandling.ReadPar(string.Format("{0}{1}{2}", Definition.objectDetectionClassesSubPath, j, Definition.objectDetectionMinObjects)) : "-";
					string text4 = (text2 == "0") ? this.parHandling.ReadPar(string.Format("{0}{1}{2}", Definition.objectDetectionClassesSubPath, j, Definition.objectDetectionMaxObjects)) : "-";
					this.AddFormattedLine(this.txtFilter, "Class" + j.ToString() + " ", null);
					this.AddFormattedLine(this.txtEnable, text, null);
					this.AddFormattedLine(this.txtMatches, text2, null);
					this.AddFormattedLine(this.txtRadius, text3, null);
					this.AddFormattedLine(this.txtMinSize, text4, null);
					this.AddFormattedLine(this.txtContrast, this.parHandling.ReadPar(string.Format("{0}{1}{2}", Definition.objectDetectionClassesSubPath, j, Definition.objectDetectionConfThreshold)) + " ", null);
					this.AddFormattedLine(this.txtTriggered, this.parHandling.ReadPar(string.Format("{0}{1}/Triggered/{2}", Definition.objectDetectionClassesSubPath, j, Definition.strValue)), null);
				}
			}
		}

		// Token: 0x060029D5 RID: 10709 RVA: 0x001161F8 File Offset: 0x001143F8
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x17000B5F RID: 2911
		// (get) Token: 0x060029D6 RID: 10710 RVA: 0x00116228 File Offset: 0x00114428
		// (set) Token: 0x060029D7 RID: 10711 RVA: 0x00116240 File Offset: 0x00114440
		public bool ShowBtnToggleAIFilters
		{
			get
			{
				return this.showBtnToggleAIFilters;
			}
			set
			{
				bool flag = this.showBtnToggleAIFilters != value;
				if (flag)
				{
					this.showBtnToggleAIFilters = value;
					this.OnPropertyChanged("ShowBtnToggleAIFilters");
				}
			}
		}

		// Token: 0x17000B60 RID: 2912
		// (get) Token: 0x060029D8 RID: 10712 RVA: 0x00116274 File Offset: 0x00114474
		// (set) Token: 0x060029D9 RID: 10713 RVA: 0x0011628C File Offset: 0x0011448C
		public string HeaderCol1
		{
			get
			{
				return this.headerCol1;
			}
			set
			{
				bool flag = this.headerCol1 != value;
				if (flag)
				{
					this.headerCol1 = value;
					this.OnPropertyChanged("HeaderCol1");
				}
			}
		}

		// Token: 0x17000B61 RID: 2913
		// (get) Token: 0x060029DA RID: 10714 RVA: 0x001162C0 File Offset: 0x001144C0
		// (set) Token: 0x060029DB RID: 10715 RVA: 0x001162D8 File Offset: 0x001144D8
		public string HeaderCol2
		{
			get
			{
				return this.headerCol2;
			}
			set
			{
				bool flag = this.headerCol2 != value;
				if (flag)
				{
					this.headerCol2 = value;
					this.OnPropertyChanged("HeaderCol2");
				}
			}
		}

		// Token: 0x17000B62 RID: 2914
		// (get) Token: 0x060029DC RID: 10716 RVA: 0x0011630C File Offset: 0x0011450C
		// (set) Token: 0x060029DD RID: 10717 RVA: 0x00116324 File Offset: 0x00114524
		public string HeaderCol3
		{
			get
			{
				return this.headerCol3;
			}
			set
			{
				bool flag = this.headerCol3 != value;
				if (flag)
				{
					this.headerCol3 = value;
					this.OnPropertyChanged("HeaderCol3");
				}
			}
		}

		// Token: 0x17000B63 RID: 2915
		// (get) Token: 0x060029DE RID: 10718 RVA: 0x00116358 File Offset: 0x00114558
		// (set) Token: 0x060029DF RID: 10719 RVA: 0x00116370 File Offset: 0x00114570
		public string HeaderCol4
		{
			get
			{
				return this.headerCol4;
			}
			set
			{
				bool flag = this.headerCol4 != value;
				if (flag)
				{
					this.headerCol4 = value;
					this.OnPropertyChanged("HeaderCol4");
				}
			}
		}

		// Token: 0x17000B64 RID: 2916
		// (get) Token: 0x060029E0 RID: 10720 RVA: 0x001163A4 File Offset: 0x001145A4
		// (set) Token: 0x060029E1 RID: 10721 RVA: 0x001163BC File Offset: 0x001145BC
		public string HeaderCol5
		{
			get
			{
				return this.headerCol5;
			}
			set
			{
				bool flag = this.headerCol5 != value;
				if (flag)
				{
					this.headerCol5 = value;
					this.OnPropertyChanged("HeaderCol5");
				}
			}
		}

		// Token: 0x17000B65 RID: 2917
		// (get) Token: 0x060029E2 RID: 10722 RVA: 0x001163F0 File Offset: 0x001145F0
		// (set) Token: 0x060029E3 RID: 10723 RVA: 0x00116408 File Offset: 0x00114608
		public string HeaderCol6
		{
			get
			{
				return this.headerCol6;
			}
			set
			{
				bool flag = this.headerCol6 != value;
				if (flag)
				{
					this.headerCol6 = value;
					this.OnPropertyChanged("HeaderCol6");
				}
			}
		}

		// Token: 0x17000B66 RID: 2918
		// (get) Token: 0x060029E4 RID: 10724 RVA: 0x0011643C File Offset: 0x0011463C
		// (set) Token: 0x060029E5 RID: 10725 RVA: 0x00116454 File Offset: 0x00114654
		public string HeaderCol7
		{
			get
			{
				return this.headerCol7;
			}
			set
			{
				bool flag = this.headerCol7 != value;
				if (flag)
				{
					this.headerCol7 = value;
					this.OnPropertyChanged("HeaderCol7");
				}
			}
		}

		// Token: 0x060029E6 RID: 10726 RVA: 0x00116488 File Offset: 0x00114688
		private void AddFormattedLine(TextBlock textBlock, string text, double? fontSize = null)
		{
			Run item = new Run(text + " " + Environment.NewLine)
			{
				FontSize = (fontSize ?? 11.5),
				FontWeight = FontWeights.Bold
			};
			textBlock.Inlines.Add(item);
		}

		// Token: 0x060029E7 RID: 10727 RVA: 0x001164EC File Offset: 0x001146EC
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/sendimage/ctrlsendimage.xaml", UriKind.Relative);
				System.Windows.Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x060029E8 RID: 10728 RVA: 0x00116524 File Offset: 0x00114724
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				((CtrlSendImage)target).Unloaded += this.CtrlSendImage_Unloaded;
				break;
			case 2:
				this.GMain = (Grid)target;
				break;
			case 3:
				this.gridTimes = (Grid)target;
				break;
			case 4:
				this.txtProcessingTime = (TextBlock)target;
				break;
			case 5:
				this.txtPaintRectanglesTime = (TextBlock)target;
				break;
			case 6:
				this.txtSendTime = (TextBlock)target;
				break;
			case 7:
				this.txtProdPerMinute = (TextBlock)target;
				break;
			case 8:
				this.txtProdPerMinuteValue = (TextBlock)target;
				break;
			case 9:
				this.txtTotalTime = (TextBlock)target;
				break;
			case 10:
				this.toggleBtn = (ToggleButton)target;
				this.toggleBtn.Click += this.toggleBtn_Click;
				break;
			case 11:
				this.txtFilter = (TextBlock)target;
				break;
			case 12:
				this.txtEnable = (TextBlock)target;
				break;
			case 13:
				this.txtMatches = (TextBlock)target;
				break;
			case 14:
				this.txtRadius = (TextBlock)target;
				break;
			case 15:
				this.txtMinSize = (TextBlock)target;
				break;
			case 16:
				this.txtContrast = (TextBlock)target;
				break;
			case 17:
				this.txtTriggered = (TextBlock)target;
				break;
			case 18:
				this.toggleBtnAIFilters = (ToggleButton)target;
				this.toggleBtnAIFilters.Click += this.toggleBtnAIFilters_Click;
				break;
			case 19:
				this.imagebox = (XRSImageBox)target;
				break;
			default:
				this._contentLoaded = true;
				break;
			}
		}

		// Token: 0x040019EF RID: 6639
		private ImageDataTransfer imageTransfer;

		// Token: 0x040019F0 RID: 6640
		private ParameterHandling parHandling;

		// Token: 0x040019F1 RID: 6641
		private DBConnection dbConnection;

		// Token: 0x040019F2 RID: 6642
		private string[] currentXriFiles;

		// Token: 0x040019F3 RID: 6643
		private static string lastRootFolder;

		// Token: 0x040019F4 RID: 6644
		private StoreImageData imageArchiver;

		// Token: 0x040019F6 RID: 6646
		private bool showFilterParams;

		// Token: 0x040019F7 RID: 6647
		private ProductManager productManager;

		// Token: 0x040019F8 RID: 6648
		private TestcardSimService testcardsimservice;

		// Token: 0x040019F9 RID: 6649
		private MachineMode machineMode = MachineMode.Standard;

		// Token: 0x040019FA RID: 6650
		private string oldstateSendImage;

		// Token: 0x040019FB RID: 6651
		private string oldstateSaveImage;

		// Token: 0x040019FC RID: 6652
		private string oldstateShowImage;

		// Token: 0x040019FE RID: 6654
		private bool showBtnToggleAIFilters = false;

		// Token: 0x040019FF RID: 6655
		private string headerCol1 = "Filter";

		// Token: 0x04001A00 RID: 6656
		private string headerCol2 = "En";

		// Token: 0x04001A01 RID: 6657
		private string headerCol3 = "Mat";

		// Token: 0x04001A02 RID: 6658
		private string headerCol4 = "Rad";

		// Token: 0x04001A03 RID: 6659
		private string headerCol5 = "MinS";

		// Token: 0x04001A04 RID: 6660
		private string headerCol6 = "Con";

		// Token: 0x04001A05 RID: 6661
		private string headerCol7 = "Trig";

		// Token: 0x04001A06 RID: 6662
		internal Grid GMain;

		// Token: 0x04001A07 RID: 6663
		internal Grid gridTimes;

		// Token: 0x04001A08 RID: 6664
		internal TextBlock txtProcessingTime;

		// Token: 0x04001A09 RID: 6665
		internal TextBlock txtPaintRectanglesTime;

		// Token: 0x04001A0A RID: 6666
		internal TextBlock txtSendTime;

		// Token: 0x04001A0B RID: 6667
		internal TextBlock txtProdPerMinute;

		// Token: 0x04001A0C RID: 6668
		internal TextBlock txtProdPerMinuteValue;

		// Token: 0x04001A0D RID: 6669
		internal TextBlock txtTotalTime;

		// Token: 0x04001A0E RID: 6670
		internal ToggleButton toggleBtn;

		// Token: 0x04001A0F RID: 6671
		internal TextBlock txtFilter;

		// Token: 0x04001A10 RID: 6672
		internal TextBlock txtEnable;

		// Token: 0x04001A11 RID: 6673
		internal TextBlock txtMatches;

		// Token: 0x04001A12 RID: 6674
		internal TextBlock txtRadius;

		// Token: 0x04001A13 RID: 6675
		internal TextBlock txtMinSize;

		// Token: 0x04001A14 RID: 6676
		internal TextBlock txtContrast;

		// Token: 0x04001A15 RID: 6677
		internal TextBlock txtTriggered;

		// Token: 0x04001A16 RID: 6678
		internal ToggleButton toggleBtnAIFilters;

		// Token: 0x04001A17 RID: 6679
		internal XRSImageBox imagebox;

		// Token: 0x04001A18 RID: 6680
		private bool _contentLoaded;
	}
}
