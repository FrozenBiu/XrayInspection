using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Common;
using DatabaseConnection;
using Definitions;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x020000EA RID: 234
	public class TestcardSimStatisticsViewModel : INotifyPropertyChanged
	{
		// Token: 0x14000071 RID: 113
		// (add) Token: 0x060016ED RID: 5869 RVA: 0x0008A080 File Offset: 0x00088280
		// (remove) Token: 0x060016EE RID: 5870 RVA: 0x0008A0B8 File Offset: 0x000882B8
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x170006EE RID: 1774
		// (get) Token: 0x060016EF RID: 5871 RVA: 0x0008A0ED File Offset: 0x000882ED
		// (set) Token: 0x060016F0 RID: 5872 RVA: 0x0008A0F5 File Offset: 0x000882F5
		public RelayCommand CleanUpCommand { get; set; }

		// Token: 0x170006EF RID: 1775
		// (get) Token: 0x060016F1 RID: 5873 RVA: 0x0008A0FE File Offset: 0x000882FE
		// (set) Token: 0x060016F2 RID: 5874 RVA: 0x0008A106 File Offset: 0x00088306
		public BoundLocalizedText StrStatistics { get; set; }

		// Token: 0x170006F0 RID: 1776
		// (get) Token: 0x060016F3 RID: 5875 RVA: 0x0008A10F File Offset: 0x0008830F
		// (set) Token: 0x060016F4 RID: 5876 RVA: 0x0008A117 File Offset: 0x00088317
		public BoundLocalizedText StrInformation { get; set; }

		// Token: 0x170006F1 RID: 1777
		// (get) Token: 0x060016F5 RID: 5877 RVA: 0x0008A120 File Offset: 0x00088320
		// (set) Token: 0x060016F6 RID: 5878 RVA: 0x0008A128 File Offset: 0x00088328
		public BoundLocalizedText StrSens { get; set; }

		// Token: 0x170006F2 RID: 1778
		// (get) Token: 0x060016F7 RID: 5879 RVA: 0x0008A131 File Offset: 0x00088331
		// (set) Token: 0x060016F8 RID: 5880 RVA: 0x0008A139 File Offset: 0x00088339
		public BoundLocalizedText StrMode { get; set; }

		// Token: 0x170006F3 RID: 1779
		// (get) Token: 0x060016F9 RID: 5881 RVA: 0x0008A142 File Offset: 0x00088342
		// (set) Token: 0x060016FA RID: 5882 RVA: 0x0008A14A File Offset: 0x0008834A
		public BoundLocalizedText StrSimulation { get; set; }

		// Token: 0x170006F4 RID: 1780
		// (get) Token: 0x060016FB RID: 5883 RVA: 0x0008A153 File Offset: 0x00088353
		// (set) Token: 0x060016FC RID: 5884 RVA: 0x0008A15B File Offset: 0x0008835B
		public BoundLocalizedText StrNumberChoosenImg { get; set; }

		// Token: 0x170006F5 RID: 1781
		// (get) Token: 0x060016FD RID: 5885 RVA: 0x0008A164 File Offset: 0x00088364
		// (set) Token: 0x060016FE RID: 5886 RVA: 0x0008A16C File Offset: 0x0008836C
		public BoundLocalizedText StrNumberSimulatedImg { get; set; }

		// Token: 0x170006F6 RID: 1782
		// (get) Token: 0x060016FF RID: 5887 RVA: 0x0008A175 File Offset: 0x00088375
		// (set) Token: 0x06001700 RID: 5888 RVA: 0x0008A17D File Offset: 0x0008837D
		public BoundLocalizedText StrNumberBallCount { get; set; }

		// Token: 0x170006F7 RID: 1783
		// (get) Token: 0x06001701 RID: 5889 RVA: 0x0008A186 File Offset: 0x00088386
		// (set) Token: 0x06001702 RID: 5890 RVA: 0x0008A18E File Offset: 0x0008838E
		public BoundLocalizedText StrGlass { get; set; }

		// Token: 0x170006F8 RID: 1784
		// (get) Token: 0x06001703 RID: 5891 RVA: 0x0008A197 File Offset: 0x00088397
		// (set) Token: 0x06001704 RID: 5892 RVA: 0x0008A19F File Offset: 0x0008839F
		public BoundLocalizedText StrVA { get; set; }

		// Token: 0x170006F9 RID: 1785
		// (get) Token: 0x06001705 RID: 5893 RVA: 0x0008A1A8 File Offset: 0x000883A8
		// (set) Token: 0x06001706 RID: 5894 RVA: 0x0008A1B0 File Offset: 0x000883B0
		public string StrInfoVASens { get; set; }

		// Token: 0x170006FA RID: 1786
		// (get) Token: 0x06001707 RID: 5895 RVA: 0x0008A1B9 File Offset: 0x000883B9
		// (set) Token: 0x06001708 RID: 5896 RVA: 0x0008A1C1 File Offset: 0x000883C1
		public string StrInfoGlassSens { get; set; }

		// Token: 0x170006FB RID: 1787
		// (get) Token: 0x06001709 RID: 5897 RVA: 0x0008A1CA File Offset: 0x000883CA
		// (set) Token: 0x0600170A RID: 5898 RVA: 0x0008A1D2 File Offset: 0x000883D2
		public string StrInfoSimMode { get; set; }

		// Token: 0x170006FC RID: 1788
		// (get) Token: 0x0600170B RID: 5899 RVA: 0x0008A1DC File Offset: 0x000883DC
		// (set) Token: 0x0600170C RID: 5900 RVA: 0x0008A1F4 File Offset: 0x000883F4
		public List<string> YAxisGlassSensitivities
		{
			get
			{
				return this.yAxisGlassSensitivities;
			}
			set
			{
				bool flag = this.yAxisGlassSensitivities != value;
				if (flag)
				{
					this.yAxisGlassSensitivities = value;
					this.OnPropertyChanged("YAxisGlassSensitivities");
				}
			}
		}

		// Token: 0x170006FD RID: 1789
		// (get) Token: 0x0600170D RID: 5901 RVA: 0x0008A228 File Offset: 0x00088428
		// (set) Token: 0x0600170E RID: 5902 RVA: 0x0008A240 File Offset: 0x00088440
		public List<string> YAxisVASensitivities
		{
			get
			{
				return this.yAxisVASensitivities;
			}
			set
			{
				bool flag = this.yAxisVASensitivities != value;
				if (flag)
				{
					this.yAxisVASensitivities = value;
					this.OnPropertyChanged("YAxisVASensitivities");
				}
			}
		}

		// Token: 0x170006FE RID: 1790
		// (get) Token: 0x0600170F RID: 5903 RVA: 0x0008A274 File Offset: 0x00088474
		// (set) Token: 0x06001710 RID: 5904 RVA: 0x0008A28C File Offset: 0x0008848C
		public List<string> StatisticInfoList
		{
			get
			{
				return this.statisticInfoList;
			}
			set
			{
				bool flag = this.statisticInfoList != value;
				if (flag)
				{
					this.statisticInfoList = value;
					this.OnPropertyChanged("StatisticInfoList");
				}
			}
		}

		// Token: 0x06001711 RID: 5905 RVA: 0x0008A2C0 File Offset: 0x000884C0
		public TestcardSimStatisticsViewModel(ParameterHandling parHandling, string selectedimages, int simulatedimages, DBConnection dbConnection, ProductManager productManager, string simmode)
		{
			this.parHandling = parHandling;
			this.dbConnection = dbConnection;
			this.btnCsvExport = new Button();
			this.btnPngExport = new Button();
			this.btnStatisticTable = new Button();
			this.InitSensitivities();
			this.SetStatisticGraphs(productManager.CurrentLoadedProduct.TableName, simmode);
			this.SetStatisticInfos(selectedimages, simulatedimages, simmode);
			this.RegisterLocalizedText();
			this.CleanUpCommand = new RelayCommand(new Action<object>(this.CleanUp));
		}

		// Token: 0x06001712 RID: 5906 RVA: 0x0008A34C File Offset: 0x0008854C
		private void SetStatisticGraphs(string tablename, string mode)
		{
			List<string> list = new List<string>();
			List<int> list2 = new List<int>();
			List<int> list3 = new List<int>();
			this.YAxisGlassSensitivities = new List<string>();
			this.YAxisVASensitivities = new List<string>();
			this.dbConnection.GetTestcardSimSensitivities(tablename, list);
			bool flag = list.Count != 0;
			if (flag)
			{
				for (int i = 0; i < Definition.sumOfGlassTestcards; i++)
				{
					bool flag2 = i < Definition.sumOfVATestcards;
					if (flag2)
					{
						list2.Add(Convert.ToInt32(list[i]));
					}
					list3.Add(Convert.ToInt32(list[i + 9]));
				}
			}
			bool flag3 = list2 != null && list2.Count != 0;
			if (flag3)
			{
				for (int j = 0; j < list2.Count; j++)
				{
					this.YAxisVASensitivities.Add(Convert.ToString(list2[j]));
				}
			}
			bool flag4 = list3 != null && list3.Count != 0;
			if (flag4)
			{
				for (int k = 0; k < list3.Count; k++)
				{
					this.YAxisGlassSensitivities.Add(Convert.ToString(list3[k]));
				}
			}
		}

		// Token: 0x06001713 RID: 5907 RVA: 0x0008A490 File Offset: 0x00088690
		private void SetStatisticInfos(string selectedimages, int simulatedimages, string simmode)
		{
			this.StatisticInfoList = new List<string>();
			bool flag = this.parHandling != null;
			if (flag)
			{
				this.StatisticInfoList.Add(this.ConvertVASensitivity(this.parHandling.ReadPar(Definition.simulationActSensVA)));
				this.StatisticInfoList.Add(this.ConvertGlassSensitivity(this.parHandling.ReadPar(Definition.simulationActSensGlass)));
				this.StatisticInfoList.Add(this.ConvertSimulationMode(simmode));
				this.StatisticInfoList.Add(this.setNumSelectedImg(simmode, selectedimages));
				this.StatisticInfoList.Add(this.setNumSimulatedImg(simmode, simulatedimages));
				this.StatisticInfoList.Add(Convert.ToString(this.setNumPattern(simmode)));
			}
		}

		// Token: 0x06001714 RID: 5908 RVA: 0x0008A554 File Offset: 0x00088754
		private string setNumSelectedImg(string mode, string selimg)
		{
			bool flag = mode == "3";
			if (flag)
			{
				selimg = "3";
			}
			else
			{
				bool flag2 = mode == "5" || mode == "6";
				if (flag2)
				{
					selimg = "1";
				}
			}
			return selimg;
		}

		// Token: 0x06001715 RID: 5909 RVA: 0x0008A5A8 File Offset: 0x000887A8
		private string setNumSimulatedImg(string mode, int simimg)
		{
			bool flag = mode == "3";
			if (flag)
			{
				simimg = 3 * Definition.sumOfAllTestcards;
			}
			else
			{
				bool flag2 = mode == "5";
				if (flag2)
				{
					simimg = Definition.sumOfAllTestcards;
				}
				else
				{
					bool flag3 = mode == "6";
					if (flag3)
					{
						simimg = 2;
					}
				}
			}
			return Convert.ToString(simimg);
		}

		// Token: 0x06001716 RID: 5910 RVA: 0x0008A604 File Offset: 0x00088804
		private string setNumPattern(string mode)
		{
			int num = Convert.ToInt32(this.parHandling.ReadPar(Definition.testpatternxdirectionPath));
			int num2 = Convert.ToInt32(this.parHandling.ReadPar(Definition.testpatternydirectionPath));
			int value = num * num2;
			bool flag = mode == "3" || mode == "5" || mode == "6";
			if (flag)
			{
				value = 100;
			}
			return Convert.ToString(value);
		}

		// Token: 0x06001717 RID: 5911 RVA: 0x0008A684 File Offset: 0x00088884
		private string ConvertSimulationMode(string parameter)
		{
			string result = "n.a.";
			bool flag = parameter == "0";
			if (flag)
			{
				result = LocalizationManager.Localize("txtAll");
			}
			bool flag2 = parameter == "1";
			if (flag2)
			{
				result = LocalizationManager.Localize("txtUptToHundered");
			}
			bool flag3 = parameter == "2";
			if (flag3)
			{
				result = LocalizationManager.Localize("txtSingle");
			}
			bool flag4 = parameter == "3";
			if (flag4)
			{
				result = LocalizationManager.Localize("txtAutotrain");
			}
			bool flag5 = parameter == "4";
			if (flag5)
			{
				result = LocalizationManager.Localize("txtDrawing");
			}
			bool flag6 = parameter == "5";
			if (flag6)
			{
				result = LocalizationManager.Localize("txtRetrain");
			}
			bool flag7 = parameter == "6";
			if (flag7)
			{
				result = "ICM";
			}
			return result;
		}

		// Token: 0x06001718 RID: 5912 RVA: 0x0008A758 File Offset: 0x00088958
		private string ConvertVASensitivity(string parameter)
		{
			string text = "n.a.";
			bool flag = parameter == "0";
			if (flag)
			{
				text = "0.3";
			}
			bool flag2 = parameter == "1";
			if (flag2)
			{
				text = "0.4";
			}
			bool flag3 = parameter == "2";
			if (flag3)
			{
				text = "0.5";
			}
			bool flag4 = parameter == "3";
			if (flag4)
			{
				text = "0.6";
			}
			bool flag5 = parameter == "4";
			if (flag5)
			{
				text = "0.8";
			}
			bool flag6 = parameter == "5";
			if (flag6)
			{
				text = "1.0";
			}
			bool flag7 = parameter == "6";
			if (flag7)
			{
				text = "1.2";
			}
			bool flag8 = parameter == "7";
			if (flag8)
			{
				text = "1.6";
			}
			bool flag9 = parameter == "8";
			if (flag9)
			{
				text = "1.8";
			}
			bool flag10 = text != "n.a.";
			if (flag10)
			{
				text += " mm";
			}
			return text;
		}

		// Token: 0x06001719 RID: 5913 RVA: 0x0008A85C File Offset: 0x00088A5C
		private string ConvertGlassSensitivity(string parameter)
		{
			string text = "n.a.";
			bool flag = parameter == "0";
			if (flag)
			{
				text = "0.6";
			}
			bool flag2 = parameter == "1";
			if (flag2)
			{
				text = "0.8";
			}
			bool flag3 = parameter == "2";
			if (flag3)
			{
				text = "1.0";
			}
			bool flag4 = parameter == "3";
			if (flag4)
			{
				text = "1.2";
			}
			bool flag5 = parameter == "4";
			if (flag5)
			{
				text = "1.5";
			}
			bool flag6 = parameter == "5";
			if (flag6)
			{
				text = "2.0";
			}
			bool flag7 = parameter == "6";
			if (flag7)
			{
				text = "2.5";
			}
			bool flag8 = parameter == "7";
			if (flag8)
			{
				text = "3.0";
			}
			bool flag9 = parameter == "8";
			if (flag9)
			{
				text = "3.5";
			}
			bool flag10 = parameter == "9";
			if (flag10)
			{
				text = "4.0";
			}
			bool flag11 = parameter == "10";
			if (flag11)
			{
				text = "6.0";
			}
			bool flag12 = parameter == "11";
			if (flag12)
			{
				text = "7.0";
			}
			bool flag13 = parameter == "12";
			if (flag13)
			{
				text = "8.0";
			}
			bool flag14 = parameter == "13";
			if (flag14)
			{
				text = "9.0";
			}
			bool flag15 = parameter == "14";
			if (flag15)
			{
				text = "10.0";
			}
			bool flag16 = text != "n.a.";
			if (flag16)
			{
				text += " mm";
			}
			return text;
		}

		// Token: 0x0600171A RID: 5914 RVA: 0x0008A9E8 File Offset: 0x00088BE8
		private void InitSensitivities()
		{
			TestcardSimStatisticsViewModel.SensitivityVAPerImgList = new List<TestcardSensitivitiesVA>();
			TestcardSimStatisticsViewModel.SensitivityGlassPerImgList = new List<TestcardSensitivitiesGlass>();
			TestcardSensitivitiesVA testcardSensitivitiesVA = new TestcardSensitivitiesVA();
			TestcardSensitivitiesGlass testcardSensitivitiesGlass = new TestcardSensitivitiesGlass();
			bool flag = TestcardSimViewModel.ImgNamesForSensitivities != null;
			if (flag)
			{
				for (int i = 0; i < TestcardSimViewModel.ImgNamesForSensitivities.Count; i++)
				{
					testcardSensitivitiesVA = new TestcardSensitivitiesVA();
					testcardSensitivitiesGlass = new TestcardSensitivitiesGlass();
					int num = TestcardSimViewModel.ImgNamesForSensitivities[i].LastIndexOf("\\");
					testcardSensitivitiesVA.ImageName = TestcardSimViewModel.ImgNamesForSensitivities[i].Substring(num + 1);
					testcardSensitivitiesVA.Nr = Convert.ToString(i + 1);
					testcardSensitivitiesGlass.ImageName = TestcardSimViewModel.ImgNamesForSensitivities[i].Substring(num + 1);
					testcardSensitivitiesGlass.Nr = Convert.ToString(i + 1);
					TestcardSimStatisticsViewModel.SensitivityVAPerImgList.Add(testcardSensitivitiesVA);
					TestcardSimStatisticsViewModel.SensitivityGlassPerImgList.Add(testcardSensitivitiesGlass);
				}
				this.SetVASensitivities(TestcardSimStatisticsViewModel.SensitivityVAPerImgList);
				this.SetGlassSensitivities(TestcardSimStatisticsViewModel.SensitivityGlassPerImgList);
			}
		}

		// Token: 0x0600171B RID: 5915 RVA: 0x0008AAFC File Offset: 0x00088CFC
		public void CreateCsvForStatistics(ProductManager productManager)
		{
			string text = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_ff");
			string text2 = "";
			string text3 = "TestcardSim";
			int num = 0;
			List<string> list = new List<string>();
			string text4 = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\TestcardSim\\";
			string text5 = string.Concat(new string[]
			{
				text4,
				"\\TestcardSim_",
				productManager.CurrentLoadedProduct.ProductName,
				"_",
				text,
				".csv"
			});
			bool flag = !Directory.Exists(text4);
			if (flag)
			{
				Directory.CreateDirectory(text4);
			}
			this.csvwriter = File.CreateText(text5);
			try
			{
				bool flag2 = TestcardSimViewModel.ImgNamesForSensitivities != null;
				if (flag2)
				{
					num = TestcardSimViewModel.ImgNamesForSensitivities.Count;
				}
				this.dbConnection.GetTestcardSimSensitivities(productManager.CurrentLoadedProduct.TableName, list);
				bool flag3 = list.Count == Definition.sumOfAllTestcards;
				if (flag3)
				{
					string text6 = "STAINLESS STEEL; Average; ";
					string text7 = "0,3; " + list[0] + "; ";
					string text8 = "0,4; " + list[1] + "; ";
					string text9 = "0,5; " + list[2] + "; ";
					string text10 = "0,6; " + list[3] + "; ";
					string text11 = "0,8; " + list[4] + "; ";
					string text12 = "1,0; " + list[5] + "; ";
					string text13 = "1,2; " + list[6] + "; ";
					string text14 = "1,6; " + list[7] + "; ";
					string text15 = "1,8; " + list[8] + "; ";
					bool flag4 = TestcardSimStatisticsViewModel.SensitivityVAPerImgList != null;
					if (flag4)
					{
						for (int i = 0; i < num; i++)
						{
							text6 = text6 + TestcardSimStatisticsViewModel.SensitivityVAPerImgList[i].ImageName + "; ";
							text7 = text7 + TestcardSimStatisticsViewModel.SensitivityVAPerImgList[i].VA0_3 + "; ";
							text8 = text8 + TestcardSimStatisticsViewModel.SensitivityVAPerImgList[i].VA0_4 + "; ";
							text9 = text9 + TestcardSimStatisticsViewModel.SensitivityVAPerImgList[i].VA0_5 + "; ";
							text10 = text10 + TestcardSimStatisticsViewModel.SensitivityVAPerImgList[i].VA0_6 + "; ";
							text11 = text11 + TestcardSimStatisticsViewModel.SensitivityVAPerImgList[i].VA0_8 + "; ";
							text12 = text12 + TestcardSimStatisticsViewModel.SensitivityVAPerImgList[i].VA1_0 + "; ";
							text13 = text13 + TestcardSimStatisticsViewModel.SensitivityVAPerImgList[i].VA1_2 + "; ";
							text14 = text14 + TestcardSimStatisticsViewModel.SensitivityVAPerImgList[i].VA1_6 + "; ";
							text15 = text15 + TestcardSimStatisticsViewModel.SensitivityVAPerImgList[i].VA1_8 + "; ";
						}
					}
					this.csvwriter.WriteLine(text6);
					this.csvwriter.WriteLine(text7);
					this.csvwriter.WriteLine(text8);
					this.csvwriter.WriteLine(text9);
					this.csvwriter.WriteLine(text10);
					this.csvwriter.WriteLine(text11);
					this.csvwriter.WriteLine(text12);
					this.csvwriter.WriteLine(text13);
					this.csvwriter.WriteLine(text14);
					this.csvwriter.WriteLine(text15);
					this.csvwriter.WriteLine(";");
					string text16 = "GLASS; Average; ";
					string text17 = "0,6; " + list[9] + "; ";
					string text18 = "0,8; " + list[10] + "; ";
					string text19 = "1,0; " + list[11] + "; ";
					string text20 = "1,2; " + list[12] + "; ";
					string text21 = "1,5; " + list[13] + "; ";
					string text22 = "2,0; " + list[14] + "; ";
					string text23 = "2,5; " + list[15] + "; ";
					string text24 = "3,0; " + list[16] + "; ";
					string text25 = "3,5; " + list[17] + "; ";
					string text26 = "4,0; " + list[18] + "; ";
					string text27 = "6,0; " + list[19] + "; ";
					string text28 = "7,0; " + list[20] + "; ";
					string text29 = "8,0; " + list[21] + "; ";
					string text30 = "9,0; " + list[22] + "; ";
					string text31 = "10,0; " + list[23] + "; ";
					bool flag5 = TestcardSimStatisticsViewModel.SensitivityGlassPerImgList != null;
					if (flag5)
					{
						for (int j = 0; j < num; j++)
						{
							text16 = text16 + TestcardSimStatisticsViewModel.SensitivityGlassPerImgList[j].ImageName + "; ";
							text17 = text17 + TestcardSimStatisticsViewModel.SensitivityGlassPerImgList[j].GL0_6 + "; ";
							text18 = text18 + TestcardSimStatisticsViewModel.SensitivityGlassPerImgList[j].GL0_8 + "; ";
							text19 = text19 + TestcardSimStatisticsViewModel.SensitivityGlassPerImgList[j].GL1_0 + "; ";
							text20 = text20 + TestcardSimStatisticsViewModel.SensitivityGlassPerImgList[j].GL1_2 + "; ";
							text21 = text21 + TestcardSimStatisticsViewModel.SensitivityGlassPerImgList[j].GL1_5 + "; ";
							text22 = text22 + TestcardSimStatisticsViewModel.SensitivityGlassPerImgList[j].GL2_0 + "; ";
							text23 = text23 + TestcardSimStatisticsViewModel.SensitivityGlassPerImgList[j].GL2_5 + "; ";
							text24 = text24 + TestcardSimStatisticsViewModel.SensitivityGlassPerImgList[j].GL3_0 + "; ";
							text25 = text25 + TestcardSimStatisticsViewModel.SensitivityGlassPerImgList[j].GL3_5 + "; ";
							text26 = text26 + TestcardSimStatisticsViewModel.SensitivityGlassPerImgList[j].GL4_0 + "; ";
							text27 = text27 + TestcardSimStatisticsViewModel.SensitivityGlassPerImgList[j].GL6_0 + "; ";
							text28 = text28 + TestcardSimStatisticsViewModel.SensitivityGlassPerImgList[j].GL7_0 + "; ";
							text29 = text29 + TestcardSimStatisticsViewModel.SensitivityGlassPerImgList[j].GL8_0 + "; ";
							text30 = text30 + TestcardSimStatisticsViewModel.SensitivityGlassPerImgList[j].GL9_0 + "; ";
							text31 = text31 + TestcardSimStatisticsViewModel.SensitivityGlassPerImgList[j].GL10_0 + "; ";
						}
					}
					this.csvwriter.WriteLine(text16);
					this.csvwriter.WriteLine(text17);
					this.csvwriter.WriteLine(text18);
					this.csvwriter.WriteLine(text19);
					this.csvwriter.WriteLine(text20);
					this.csvwriter.WriteLine(text21);
					this.csvwriter.WriteLine(text22);
					this.csvwriter.WriteLine(text23);
					this.csvwriter.WriteLine(text24);
					this.csvwriter.WriteLine(text25);
					this.csvwriter.WriteLine(text26);
					this.csvwriter.WriteLine(text27);
					this.csvwriter.WriteLine(text28);
					this.csvwriter.WriteLine(text29);
					this.csvwriter.WriteLine(text30);
					this.csvwriter.WriteLine(text31);
					this.csvwriter.Close();
					this.csvwriter.Dispose();
					this.dbConnection.GetUsbDriveLetter(ref text2);
					bool flag6 = Directory.Exists(text2);
					if (flag6)
					{
						bool flag7 = !Directory.Exists(text2 + text3);
						if (flag7)
						{
							Directory.CreateDirectory(text2 + "\\" + text3);
						}
						File.Copy(text5, string.Concat(new string[]
						{
							text2,
							text3,
							"\\",
							text3,
							"_",
							productManager.CurrentLoadedProduct.ProductName,
							"_",
							text,
							".csv"
						}));
					}
					Dialog.Show(LocalizationManager.Localize("txtSuccess"), LocalizationManager.Localize("txtSaveSuccessful"), DialogStyle.Ok, DialogButtons.None, true);
				}
			}
			catch (UnauthorizedAccessException)
			{
				Dialog.Show(LocalizationManager.Localize("txtError"), string.Concat(new string[]
				{
					LocalizationManager.Localize("msgPathAccess"),
					" \"",
					Path.GetDirectoryName(text2 + "\\TestcardSim"),
					"\" ",
					LocalizationManager.Localize("msgForbidden")
				}), DialogStyle.Error);
			}
			catch
			{
				Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtError"), DialogStyle.Error);
			}
		}

		// Token: 0x0600171C RID: 5916 RVA: 0x0008B504 File Offset: 0x00089704
		public void CreatePNGForStatistics(Grid chartGrid, Rect bounds, ProductManager productManager)
		{
			string text = "";
			string text2 = "";
			string text3 = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\TestcardSim\\";
			string text4 = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_ff");
			bool flag = !Directory.Exists(text3);
			if (flag)
			{
				Directory.CreateDirectory(text3);
			}
			this.dbConnection.GetUsbDriveLetter(ref text);
			try
			{
				List<string> list = Directory.GetFiles(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\TestcardSim").ToList<string>();
				list.Sort(new NamesComparer());
				int count = list.Count;
				string str = string.Concat(new string[]
				{
					"TestcardSim_",
					productManager.CurrentLoadedProduct.ProductName,
					"_",
					text4,
					".png"
				});
				text2 = text + "\\TestcardSim";
				bool flag2 = Directory.Exists(text);
				if (flag2)
				{
					bool flag3 = !Directory.Exists(text2);
					if (flag3)
					{
						Directory.CreateDirectory(text2);
					}
				}
				text3 = text3 + "\\" + str;
				text2 = text + "\\TestcardSim\\" + str;
				FileInfo fileInfo = new FileInfo(text2);
				RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int)bounds.Width, (int)bounds.Height, 96.0, 96.0, PixelFormats.Pbgra32);
				DrawingVisual drawingVisual = new DrawingVisual();
				using (DrawingContext drawingContext = drawingVisual.RenderOpen())
				{
					VisualBrush brush = new VisualBrush(chartGrid);
					Pen pen = new Pen();
					drawingContext.DrawRectangle(brush, pen, new Rect(default(Point), bounds.Size));
				}
				renderTargetBitmap.Render(drawingVisual);
				FileStream fileStream = new FileStream(text3, FileMode.Create);
				new PngBitmapEncoder
				{
					Frames = 
					{
						BitmapFrame.Create(renderTargetBitmap)
					}
				}.Save(fileStream);
				fileStream.Close();
				bool flag4 = Directory.Exists(text + "\\");
				if (flag4)
				{
					fileStream = new FileStream(text2, FileMode.Create);
					new PngBitmapEncoder
					{
						Frames = 
						{
							BitmapFrame.Create(renderTargetBitmap)
						}
					}.Save(fileStream);
					fileStream.Close();
				}
				Dialog.Show(LocalizationManager.Localize("txtSuccess"), LocalizationManager.Localize("txtSaveSuccessful"), DialogStyle.Ok, DialogButtons.None, true);
			}
			catch (UnauthorizedAccessException)
			{
				Dialog.Show(LocalizationManager.Localize("txtError"), string.Concat(new string[]
				{
					LocalizationManager.Localize("msgPathAccess"),
					" \"",
					Path.GetDirectoryName(text2),
					"\" ",
					LocalizationManager.Localize("msgForbidden")
				}), DialogStyle.Error);
			}
			catch
			{
				Dialog.Show(LocalizationManager.Localize("txtError"), string.Concat(new string[]
				{
					LocalizationManager.Localize("msgPathAccess"),
					" \"",
					Path.GetDirectoryName(text2),
					"\" ",
					LocalizationManager.Localize("msgForbidden")
				}), DialogStyle.Error);
			}
		}

		// Token: 0x0600171D RID: 5917 RVA: 0x0008B868 File Offset: 0x00089A68
		private void SetVASensitivities(List<TestcardSensitivitiesVA> list)
		{
			int num = 0;
			bool flag = list != null && list.Count > 0;
			if (flag)
			{
				for (int i = 0; i < TestcardSimViewModel.VASensitivities.Count; i++)
				{
					string text = Convert.ToString(TestcardSimViewModel.VASensitivities[i + 1]);
					bool flag2 = i >= 2 && TestcardSimViewModel.VASensitivities[i] <= TestcardSimViewModel.VASensitivities[i - 2];
					if (flag2)
					{
						num++;
					}
					bool flag3 = num < list.Count;
					if (flag3)
					{
						bool flag4 = TestcardSimViewModel.VASensitivities[i] == 0;
						if (flag4)
						{
							list[num].VA0_3 = text;
						}
						else
						{
							bool flag5 = TestcardSimViewModel.VASensitivities[i] == 1;
							if (flag5)
							{
								list[num].VA0_4 = text;
							}
							else
							{
								bool flag6 = TestcardSimViewModel.VASensitivities[i] == 2;
								if (flag6)
								{
									list[num].VA0_5 = text;
								}
								else
								{
									bool flag7 = TestcardSimViewModel.VASensitivities[i] == 3;
									if (flag7)
									{
										list[num].VA0_6 = text;
									}
									else
									{
										bool flag8 = TestcardSimViewModel.VASensitivities[i] == 4;
										if (flag8)
										{
											list[num].VA0_8 = text;
										}
										else
										{
											bool flag9 = TestcardSimViewModel.VASensitivities[i] == 5;
											if (flag9)
											{
												list[num].VA1_0 = text;
											}
											else
											{
												bool flag10 = TestcardSimViewModel.VASensitivities[i] == 6;
												if (flag10)
												{
													list[num].VA1_2 = text;
												}
												else
												{
													bool flag11 = TestcardSimViewModel.VASensitivities[i] == 7;
													if (flag11)
													{
														list[num].VA1_6 = text;
													}
													else
													{
														bool flag12 = TestcardSimViewModel.VASensitivities[i] == 8;
														if (flag12)
														{
															list[num].VA1_8 = text;
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
					i++;
				}
			}
		}

		// Token: 0x0600171E RID: 5918 RVA: 0x0008BA70 File Offset: 0x00089C70
		private void SetGlassSensitivities(List<TestcardSensitivitiesGlass> list)
		{
			int num = 0;
			bool flag = list != null && list.Count > 0;
			if (flag)
			{
				for (int i = 0; i < TestcardSimViewModel.GlassSensitivities.Count; i++)
				{
					string text = Convert.ToString(TestcardSimViewModel.GlassSensitivities[i + 1]);
					bool flag2 = i >= 2 && TestcardSimViewModel.GlassSensitivities[i] <= TestcardSimViewModel.GlassSensitivities[i - 2];
					if (flag2)
					{
						bool flag3 = num < list.Count - 1;
						if (flag3)
						{
							num++;
						}
					}
					bool flag4 = TestcardSimViewModel.GlassSensitivities[i] == 0;
					if (flag4)
					{
						list[num].GL0_6 = text;
					}
					else
					{
						bool flag5 = TestcardSimViewModel.GlassSensitivities[i] == 1;
						if (flag5)
						{
							list[num].GL0_8 = text;
						}
						else
						{
							bool flag6 = TestcardSimViewModel.GlassSensitivities[i] == 2;
							if (flag6)
							{
								list[num].GL1_0 = text;
							}
							else
							{
								bool flag7 = TestcardSimViewModel.GlassSensitivities[i] == 3;
								if (flag7)
								{
									list[num].GL1_2 = text;
								}
								else
								{
									bool flag8 = TestcardSimViewModel.GlassSensitivities[i] == 4;
									if (flag8)
									{
										list[num].GL1_5 = text;
									}
									else
									{
										bool flag9 = TestcardSimViewModel.GlassSensitivities[i] == 5;
										if (flag9)
										{
											list[num].GL2_0 = text;
										}
										else
										{
											bool flag10 = TestcardSimViewModel.GlassSensitivities[i] == 6;
											if (flag10)
											{
												list[num].GL2_5 = text;
											}
											else
											{
												bool flag11 = TestcardSimViewModel.GlassSensitivities[i] == 7;
												if (flag11)
												{
													list[num].GL3_0 = text;
												}
												else
												{
													bool flag12 = TestcardSimViewModel.GlassSensitivities[i] == 8;
													if (flag12)
													{
														list[num].GL3_5 = text;
													}
													else
													{
														bool flag13 = TestcardSimViewModel.GlassSensitivities[i] == 9;
														if (flag13)
														{
															list[num].GL4_0 = text;
														}
														else
														{
															bool flag14 = TestcardSimViewModel.GlassSensitivities[i] == 10;
															if (flag14)
															{
																list[num].GL6_0 = text;
															}
															else
															{
																bool flag15 = TestcardSimViewModel.GlassSensitivities[i] == 11;
																if (flag15)
																{
																	list[num].GL7_0 = text;
																}
																else
																{
																	bool flag16 = TestcardSimViewModel.GlassSensitivities[i] == 12;
																	if (flag16)
																	{
																		list[num].GL8_0 = text;
																	}
																	else
																	{
																		bool flag17 = TestcardSimViewModel.GlassSensitivities[i] == 13;
																		if (flag17)
																		{
																			list[num].GL9_0 = text;
																		}
																		else
																		{
																			bool flag18 = TestcardSimViewModel.GlassSensitivities[i] == 14;
																			if (flag18)
																			{
																				list[num].GL10_0 = text;
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
									}
								}
							}
						}
					}
					i++;
				}
			}
		}

		// Token: 0x0600171F RID: 5919 RVA: 0x0008BD74 File Offset: 0x00089F74
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x06001720 RID: 5920 RVA: 0x0008BDA4 File Offset: 0x00089FA4
		private void RegisterLocalizedText()
		{
			this.StrStatistics = new BoundLocalizedText("menuItemTestcardSim");
			this.StrInformation = new BoundLocalizedText("txtInfo");
			this.StrSens = new BoundLocalizedText("txtActualSensitivities");
			this.StrMode = new BoundLocalizedText("txtMode");
			this.StrSimulation = new BoundLocalizedText("txtSimulation");
			this.StrNumberChoosenImg = new BoundLocalizedText("txtNumOfChoosenImg");
			this.StrNumberSimulatedImg = new BoundLocalizedText("txtNumOfSimImg");
			this.StrNumberBallCount = new BoundLocalizedText("txtTestcardPattern");
			this.StrGlass = new BoundLocalizedText("txtGlass");
			this.StrVA = new BoundLocalizedText("txtVA");
			this.StrInfoVASens = this.StrVA.LocalizedText;
			this.StrInfoGlassSens = this.StrGlass.LocalizedText;
			this.StrInfoSimMode = this.StrSimulation.LocalizedText + " " + this.StrMode.LocalizedText;
		}

		// Token: 0x06001721 RID: 5921 RVA: 0x0008BEA8 File Offset: 0x0008A0A8
		public void DeregisterLocalizedText()
		{
			MainWindow.localizationManager.Deregister(this.StrStatistics);
			MainWindow.localizationManager.Deregister(this.StrInformation);
			MainWindow.localizationManager.Deregister(this.StrSens);
			MainWindow.localizationManager.Deregister(this.StrMode);
			MainWindow.localizationManager.Deregister(this.StrSimulation);
			MainWindow.localizationManager.Deregister(this.StrNumberChoosenImg);
			MainWindow.localizationManager.Deregister(this.StrNumberSimulatedImg);
			MainWindow.localizationManager.Deregister(this.StrNumberBallCount);
			MainWindow.localizationManager.Deregister(this.StrGlass);
			MainWindow.localizationManager.Deregister(this.StrVA);
		}

		// Token: 0x06001722 RID: 5922 RVA: 0x0008BF60 File Offset: 0x0008A160
		private void CleanUp(object obj)
		{
			this.DeregisterLocalizedText();
			bool flag = TestcardSimStatisticsViewModel.SensitivityVAPerImgList != null;
			if (flag)
			{
				TestcardSimStatisticsViewModel.SensitivityVAPerImgList.Clear();
				TestcardSimStatisticsViewModel.SensitivityVAPerImgList = null;
			}
			bool flag2 = TestcardSimStatisticsViewModel.SensitivityGlassPerImgList != null;
			if (flag2)
			{
				TestcardSimStatisticsViewModel.SensitivityGlassPerImgList.Clear();
				TestcardSimStatisticsViewModel.SensitivityGlassPerImgList = null;
			}
			bool flag3 = this.StatisticInfoList != null;
			if (flag3)
			{
				this.StatisticInfoList.Clear();
				this.StatisticInfoList = null;
			}
			bool flag4 = this.YAxisGlassSensitivities != null;
			if (flag4)
			{
				this.YAxisGlassSensitivities.Clear();
				this.YAxisGlassSensitivities = null;
			}
			bool flag5 = this.YAxisVASensitivities != null;
			if (flag5)
			{
				this.YAxisVASensitivities.Clear();
				this.YAxisVASensitivities = null;
			}
		}

		// Token: 0x04000F1F RID: 3871
		private ParameterHandling parHandling;

		// Token: 0x04000F20 RID: 3872
		private DBConnection dbConnection;

		// Token: 0x04000F21 RID: 3873
		private StreamWriter csvwriter;

		// Token: 0x04000F31 RID: 3889
		public Button btnCsvExport;

		// Token: 0x04000F32 RID: 3890
		public Button btnPngExport;

		// Token: 0x04000F33 RID: 3891
		public Button btnStatisticTable;

		// Token: 0x04000F34 RID: 3892
		public static List<TestcardSensitivitiesVA> SensitivityVAPerImgList;

		// Token: 0x04000F35 RID: 3893
		public static List<TestcardSensitivitiesGlass> SensitivityGlassPerImgList;

		// Token: 0x04000F36 RID: 3894
		private List<string> yAxisGlassSensitivities;

		// Token: 0x04000F37 RID: 3895
		private List<string> yAxisVASensitivities;

		// Token: 0x04000F38 RID: 3896
		private List<string> statisticInfoList;
	}
}
