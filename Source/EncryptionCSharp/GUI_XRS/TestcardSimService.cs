using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using DatabaseConnection;
using Definitions;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x020000DA RID: 218
	public class TestcardSimService
	{
		// Token: 0x1400006E RID: 110
		// (add) Token: 0x0600159A RID: 5530 RVA: 0x00080428 File Offset: 0x0007E628
		// (remove) Token: 0x0600159B RID: 5531 RVA: 0x00080460 File Offset: 0x0007E660
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action PropertiesSet;

		// Token: 0x1700068C RID: 1676
		// (get) Token: 0x0600159C RID: 5532 RVA: 0x00080495 File Offset: 0x0007E695
		// (set) Token: 0x0600159D RID: 5533 RVA: 0x0008049D File Offset: 0x0007E69D
		public int RealImgCountForSim { get; set; }

		// Token: 0x1700068D RID: 1677
		// (get) Token: 0x0600159E RID: 5534 RVA: 0x000804A6 File Offset: 0x0007E6A6
		// (set) Token: 0x0600159F RID: 5535 RVA: 0x000804AE File Offset: 0x0007E6AE
		public int NewVABallSizeAfterSim { get; set; }

		// Token: 0x1700068E RID: 1678
		// (get) Token: 0x060015A0 RID: 5536 RVA: 0x000804B7 File Offset: 0x0007E6B7
		// (set) Token: 0x060015A1 RID: 5537 RVA: 0x000804BF File Offset: 0x0007E6BF
		public int NewGlassBallSizeAfterSim { get; set; }

		// Token: 0x1700068F RID: 1679
		// (get) Token: 0x060015A2 RID: 5538 RVA: 0x000804C8 File Offset: 0x0007E6C8
		// (set) Token: 0x060015A3 RID: 5539 RVA: 0x000804D0 File Offset: 0x0007E6D0
		public bool CheckSensitivityDecrease { get; set; }

		// Token: 0x17000690 RID: 1680
		// (get) Token: 0x060015A4 RID: 5540 RVA: 0x000804D9 File Offset: 0x0007E6D9
		// (set) Token: 0x060015A5 RID: 5541 RVA: 0x000804E1 File Offset: 0x0007E6E1
		public bool CheckBallCounts { get; set; }

		// Token: 0x17000691 RID: 1681
		// (get) Token: 0x060015A6 RID: 5542 RVA: 0x000804EA File Offset: 0x0007E6EA
		// (set) Token: 0x060015A7 RID: 5543 RVA: 0x000804F2 File Offset: 0x0007E6F2
		public bool MonitoringSensIsActive { get; set; }

		// Token: 0x17000692 RID: 1682
		// (get) Token: 0x060015A8 RID: 5544 RVA: 0x000804FB File Offset: 0x0007E6FB
		// (set) Token: 0x060015A9 RID: 5545 RVA: 0x00080503 File Offset: 0x0007E703
		public bool MonitoringSensIsInProgress { get; set; }

		// Token: 0x17000693 RID: 1683
		// (get) Token: 0x060015AA RID: 5546 RVA: 0x0008050C File Offset: 0x0007E70C
		// (set) Token: 0x060015AB RID: 5547 RVA: 0x00080514 File Offset: 0x0007E714
		public bool ProductChangedForStatisticTable { get; set; }

		// Token: 0x17000694 RID: 1684
		// (get) Token: 0x060015AC RID: 5548 RVA: 0x0008051D File Offset: 0x0007E71D
		// (set) Token: 0x060015AD RID: 5549 RVA: 0x00080525 File Offset: 0x0007E725
		public bool ImagesAvailable { get; set; }

		// Token: 0x060015AE RID: 5550 RVA: 0x00080530 File Offset: 0x0007E730
		public TestcardSimService(ParameterHandling parHandling, ImageDataTransfer imageTransfer, StoreImageData imageArchiver, DBConnection dbConnection, ProductManager productManager, ErrorSurvey errorSurvey)
		{
			this.parHandling = parHandling;
			this.imageTransfer = imageTransfer;
			this.imageArchiver = imageArchiver;
			this.dbConnection = dbConnection;
			this.productManager = productManager;
			this.errorSurvey = errorSurvey;
			this.simModelforeignBody = new SimModelForeignBody();
			this.simModelcreateImage = new SimModelCreateImage();
			this.selectedimgForTimeIntervall = new XRSImage();
			this.MonitoringSensIsActive = true;
			this.MonitoringSensIsInProgress = false;
			this.ProductChangedForStatisticTable = false;
			this.setOrCheckImgToSimulateIsNew(true);
			this.machinetype = parHandling.ReadPar(Definition.machineTypePath);
			this.ImagesAvailable = true;
		}

		// Token: 0x060015AF RID: 5551 RVA: 0x000805DC File Offset: 0x0007E7DC
		public void InitSimulation()
		{
			this.checkContCountIsEnable = (this.parHandling.ReadPar(Definition.contaminationsCounterEnablePath) == "1");
			this.parHandling.WriteValue(Definition.contaminationsCounterEnablePath, "1");
			this.parHandling.WriteValue(Definition.simulationWrongDetPix, "0");
			TestcardSimService.VASensitivities = new List<int>();
			TestcardSimService.GlassSensitivities = new List<int>();
			this.simulationIsDoneVATimeIntvervall = false;
			this.simulationIsDoneGlassTimeIntervall = false;
			this.selectedimgForTimeIntervall = this.GetLastNSavedImages(0);
			this.CheckVersionAndXRaySource = false;
			this.CheckSensitivityDecrease = false;
			this.CheckBallCounts = false;
			this.originalImageState = 0;
			this.MonitoringSensIsActive = false;
		}

		// Token: 0x060015B0 RID: 5552 RVA: 0x00080694 File Offset: 0x0007E894
		public void TestcardSimAfterTrainingSendImg(bool isAutotrain, XRSImage selectedimg, bool isAI)
		{
			int backgroundnoise = (int)(10000 - 100 * Convert.ToUInt16(this.parHandling.ReadPar(Definition.backroundNoisePath)));
			this.dbConnection.GetNAutolearn(ref this.nAutolearn);
			this.InitSimulation();
			this.totalSimImageCounter = 0;
			this.ImagesAvailable = true;
			XRSImage xrsimage;
			int xpattern;
			int ypattern;
			if (isAI)
			{
				xrsimage = this.GetLastNSavedImages(0);
				bool flag = xrsimage == null;
				if (flag)
				{
					xrsimage = this.GetLastNSavedImageFromAIModell();
				}
				bool flag2 = xrsimage == null;
				if (flag2)
				{
					this.ImagesAvailable = false;
					return;
				}
				bool flag3 = xrsimage.Height < 2000;
				if (flag3)
				{
					this.RealImgCountForSim = 3;
					xpattern = this.testpatternxy;
					ypattern = this.testpatternxy;
				}
				else
				{
					this.RealImgCountForSim = 1;
					xpattern = this.testpatternxy;
					ypattern = 30;
				}
				this.parHandling.WriteValue(Definition.simulationmodelPath, "5");
			}
			else if (isAutotrain)
			{
				xrsimage = this.GetLastNSavedImages(0);
				bool flag4 = xrsimage.Height < 2000;
				if (flag4)
				{
					this.RealImgCountForSim = 3;
					xpattern = this.testpatternxy;
					ypattern = this.testpatternxy;
				}
				else
				{
					this.RealImgCountForSim = 1;
					xpattern = this.testpatternxy;
					ypattern = 30;
				}
				bool flag5 = this.RealImgCountForSim > this.nAutolearn;
				if (flag5)
				{
					this.RealImgCountForSim = this.nAutolearn;
				}
				this.parHandling.WriteValue(Definition.simulationmodelPath, "3");
			}
			else
			{
				xrsimage = selectedimg;
				this.originalImageState = selectedimg.State;
				this.RealImgCountForSim = 1;
				xpattern = this.testpatternxy;
				ypattern = this.testpatternxy;
				this.parHandling.WriteValue(Definition.simulationmodelPath, "5");
			}
			for (int i = 0; i < this.RealImgCountForSim; i++)
			{
				if (isAutotrain)
				{
					xrsimage = this.GetLastNSavedImages(i);
				}
				bool calcedgepointspattern = true;
				for (int j = 0; j < Definition.sumOfAllTestcards; j++)
				{
					bool flag6 = j < Definition.sumOfVATestcards;
					int simmaterial;
					int simsize;
					if (flag6)
					{
						simmaterial = 1;
						simsize = j;
					}
					else
					{
						simmaterial = 2;
						simsize = j - Definition.sumOfVATestcards;
					}
					this.CheckVersionAndXRaySource = this.SimulateOneTestcardOneImage(xrsimage, simmaterial, simsize, xpattern, ypattern, backgroundnoise, calcedgepointspattern);
					calcedgepointspattern = false;
				}
			}
		}

		// Token: 0x060015B1 RID: 5553 RVA: 0x000808F8 File Offset: 0x0007EAF8
		public bool TestcardSimAfterTrainingGetImg(XRSImage simulatedimg, bool isAutotrain)
		{
			this.totalSimImageCounter++;
			bool flag = this.totalSimImageCounter == Definition.sumOfAllTestcards * this.RealImgCountForSim;
			this.CheckSensitivityDecrease = this.calcSensitivitiesAfterTraining((int)simulatedimg.TestcardSimBallCount, (int)simulatedimg.ContaminationCount, simulatedimg.TestcardSimMaterial, (int)simulatedimg.TestcardSimBallSize, flag, isAutotrain);
			return flag;
		}

		// Token: 0x060015B2 RID: 5554 RVA: 0x0008095C File Offset: 0x0007EB5C
		public void TestcardSimAfterTimeIntervall(int ballsizeVA, int ballsizeGlass, bool sendVA, bool sendGlass)
		{
			bool flag = this.selectedimgForTimeIntervall != null && this.selectedimgForTimeIntervall.State < 128;
			if (flag)
			{
				int backgroundnoise = (int)(10000 - 100 * Convert.ToUInt16(this.parHandling.ReadPar(Definition.backroundNoisePath)));
				this.parHandling.WriteValue(Definition.simulationmodelPath, "6");
				if (sendVA)
				{
					bool flag2 = ballsizeVA >= 0 && ballsizeVA < Definition.sumOfVATestcards;
					if (flag2)
					{
						this.CheckVersionAndXRaySource = this.SimulateOneTestcardOneImage(this.selectedimgForTimeIntervall, 1, this.CheckBallCountsBecauseOfNewVersion() ? ballsizeVA : (ballsizeVA + 1), this.testpatternxy, this.testpatternxy, backgroundnoise, true);
					}
					else
					{
						this.NewVABallSizeAfterSim = 999;
					}
				}
				if (sendGlass)
				{
					bool flag3 = ballsizeGlass >= 0 && ballsizeGlass < Definition.sumOfGlassTestcards;
					if (flag3)
					{
						this.CheckVersionAndXRaySource = this.SimulateOneTestcardOneImage(this.selectedimgForTimeIntervall, 2, this.CheckBallCountsBecauseOfNewVersion() ? ballsizeGlass : (ballsizeGlass + 1), this.testpatternxy, this.testpatternxy, backgroundnoise, true);
					}
					else
					{
						this.NewGlassBallSizeAfterSim = 999;
					}
				}
				bool flag4 = this.NewVABallSizeAfterSim == 999 && this.NewGlassBallSizeAfterSim == 999;
				if (flag4)
				{
					this.setDBValuesforMonitoring();
				}
			}
		}

		// Token: 0x060015B3 RID: 5555 RVA: 0x00080AA4 File Offset: 0x0007ECA4
		public bool TestcardSimAfterTimeIntervallGetImg(XRSImage simulatedXrsImg)
		{
			int num = (int)simulatedXrsImg.TestcardSimBallSize;
			bool flag = this.calcSensitivitiesForTimeIntervallMode((int)simulatedXrsImg.TestcardSimBallCount, (int)simulatedXrsImg.ContaminationCount);
			if (flag)
			{
				bool flag2 = simulatedXrsImg.TestcardSimMaterial.Trim() == "VA";
				if (flag2)
				{
					this.NewVABallSizeAfterSim = (int)simulatedXrsImg.TestcardSimBallSize;
					this.simulationIsDoneVATimeIntvervall = true;
				}
				else
				{
					bool flag3 = simulatedXrsImg.TestcardSimMaterial.Trim() == "Glass";
					if (flag3)
					{
						this.NewGlassBallSizeAfterSim = (int)simulatedXrsImg.TestcardSimBallSize;
						this.simulationIsDoneGlassTimeIntervall = true;
					}
				}
			}
			else
			{
				bool flag4 = num >= 0;
				if (flag4)
				{
					bool flag5 = simulatedXrsImg.TestcardSimMaterial.Trim() == "VA";
					if (flag5)
					{
						bool flag6 = num < Definition.sumOfVATestcards - 1;
						if (flag6)
						{
							num++;
							this.TestcardSimAfterTimeIntervall(num, 999, true, false);
						}
						else
						{
							this.simulationIsDoneVATimeIntvervall = true;
							this.NewVABallSizeAfterSim = 999;
						}
					}
					else
					{
						bool flag7 = simulatedXrsImg.TestcardSimMaterial.Trim() == "Glass";
						if (flag7)
						{
							bool flag8 = num < Definition.sumOfGlassTestcards - 1;
							if (flag8)
							{
								num++;
								this.TestcardSimAfterTimeIntervall(999, num, false, true);
							}
							else
							{
								this.simulationIsDoneGlassTimeIntervall = true;
								this.NewGlassBallSizeAfterSim = 999;
							}
						}
					}
				}
			}
			bool flag9 = this.simulationIsDoneVATimeIntvervall && this.simulationIsDoneGlassTimeIntervall;
			bool flag10 = flag9;
			if (flag10)
			{
				this.setDBValuesforMonitoring();
			}
			return flag9;
		}

		// Token: 0x060015B4 RID: 5556 RVA: 0x00080C2C File Offset: 0x0007EE2C
		private bool calcSensitivitiesForTimeIntervallMode(int totalcounts, int detectedcounts)
		{
			double num = 0.0;
			bool flag = totalcounts != 0;
			if (flag)
			{
				num = 100.0 * ((double)detectedcounts / (double)totalcounts);
			}
			return num >= 95.0;
		}

		// Token: 0x060015B5 RID: 5557 RVA: 0x00080C74 File Offset: 0x0007EE74
		public bool TestcardSimForComplianceSensitivities(XRSImage complianceImg, int materialForProcess, bool isFirstImage)
		{
			bool flag = this.parHandling != null;
			bool result;
			if (flag)
			{
				int backgroundnoise = (int)(10000 - 100 * Convert.ToUInt16(this.parHandling.ReadPar(Definition.backroundNoisePath)));
				bool flag2 = true;
				bool flag3 = true;
				int num = 0;
				int num2 = 0;
				int num3 = 0;
				string value = "0";
				List<int> list = new List<int>();
				int num4 = 0;
				bool flag4 = complianceImg != null;
				if (flag4)
				{
					value = this.parHandling.ReadPar(Definition.contaminationsCounterEnablePath);
					this.parHandling.WriteValue(Definition.contaminationsCounterEnablePath, "1");
					this.parHandling.WriteValue(Definition.simulationmodelPath, "7");
					bool flag5 = materialForProcess == 1;
					if (flag5)
					{
						num = Convert.ToInt32(this.parHandling.ReadPar(Definition.simulationCompSensVA));
						num2 = Convert.ToInt32(this.parHandling.ReadPar(Definition.simulationActSensVA));
						num3 = 8;
					}
					else
					{
						bool flag6 = materialForProcess == 2;
						if (flag6)
						{
							num = Convert.ToInt32(this.parHandling.ReadPar(Definition.simulationCompSensGlass));
							num2 = Convert.ToInt32(this.parHandling.ReadPar(Definition.simulationActSensGlass));
							num3 = 14;
						}
					}
					bool flag7 = num > num3;
					if (flag7)
					{
						num = num3 + 1;
					}
					bool flag8 = num > num2 && num > 0 && num > 1;
					if (flag8)
					{
						for (int i = 0; i <= num - 2; i++)
						{
							list.Add(i);
						}
						this.CheckVersionAndXRaySource = this.SimulateDifferentBallSizesOneImage(complianceImg, materialForProcess, list, 20, 20, backgroundnoise, true);
					}
					else
					{
						flag2 = false;
					}
					while (flag2 && !this.CheckVersionAndXRaySource)
					{
						Thread.Sleep(200);
						flag2 = !(this.parHandling.ReadPar(Definition.simulationmodelPath) == "0");
						num4++;
						bool flag9 = num4 > 100;
						if (flag9)
						{
							flag2 = false;
						}
					}
					this.parHandling.WriteValue(Definition.simulationmodelPath, "7");
					flag2 = true;
					bool flag10 = num > num2 && num > 0;
					if (flag10)
					{
						this.CheckVersionAndXRaySource = this.SimulateOneTestcardOneImage(complianceImg, materialForProcess, num - 1, this.testpatternxy, this.testpatternxy, backgroundnoise, true);
					}
					else
					{
						flag2 = false;
						flag3 = false;
					}
					num4 = 0;
					while (flag2 && !this.CheckVersionAndXRaySource)
					{
						Thread.Sleep(200);
						flag2 = !(this.parHandling.ReadPar(Definition.simulationmodelPath) == "0");
						num4++;
						bool flag11 = num4 > 100;
						if (flag11)
						{
							flag2 = false;
						}
					}
					bool flag12 = flag3;
					if (flag12)
					{
						this.updateFilterContrasts();
					}
				}
				this.parHandling.WriteValue(Definition.contaminationsCounterEnablePath, value);
				this.parHandling.WriteValue(Definition.simulationmodelPath, "0");
				Thread.Sleep(200);
				result = (flag3 && !this.CheckVersionAndXRaySource);
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x060015B6 RID: 5558 RVA: 0x00080F74 File Offset: 0x0007F174
		private void updateFilterContrasts()
		{
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			for (int i = 0; i < Definition.nObjectFinder; i++)
			{
				list.Add(string.Concat(new string[]
				{
					Definition.filterMainPath,
					Definition.objectFindersSubPath,
					(i + 1).ToString(),
					"/Contrast/",
					Definition.strValue
				}));
				list2.Add(this.parHandling.ReadPar(string.Concat(new string[]
				{
					Definition.filterMainPath,
					Definition.objectFindersSubPath,
					(i + 1).ToString(),
					"/Contrast/",
					Definition.strValue
				})));
			}
			for (int j = 0; j < Definition.nComparer; j++)
			{
				list.Add(string.Concat(new string[]
				{
					Definition.filterMainPath,
					Definition.comparerSubPath,
					(j + 1).ToString(),
					"/Contrast/",
					Definition.strValue
				}));
				list2.Add(this.parHandling.ReadPar(string.Concat(new string[]
				{
					Definition.filterMainPath,
					Definition.comparerSubPath,
					(j + 1).ToString(),
					"/Contrast/",
					Definition.strValue
				})));
			}
			for (int k = 1; k <= 5; k++)
			{
				list.Add(string.Concat(new string[]
				{
					Definition.filterMainPath,
					Definition.lowDensitySubPath,
					"/Contrast",
					k.ToString(),
					"/",
					Definition.strValue
				}));
				list2.Add(this.parHandling.ReadPar(string.Concat(new string[]
				{
					Definition.filterMainPath,
					Definition.lowDensitySubPath,
					"/Contrast",
					k.ToString(),
					"/",
					Definition.strValue
				})));
			}
			for (int l = 1; l <= 4; l++)
			{
				list.Add(string.Concat(new string[]
				{
					Definition.filterMainPath,
					Definition.lowDensitySubPath,
					"/CountThreshold",
					l.ToString(),
					"/",
					Definition.strValue
				}));
				list2.Add(this.parHandling.ReadPar(string.Concat(new string[]
				{
					Definition.filterMainPath,
					Definition.lowDensitySubPath,
					"/CountThreshold",
					l.ToString(),
					"/",
					Definition.strValue
				})));
			}
			list.Add(Definition.filterMainPath + Definition.thresholdSubPath + "/HighThreshold/" + Definition.strValue);
			list2.Add(this.parHandling.ReadPar(Definition.filterMainPath + Definition.thresholdSubPath + "/HighThreshold/" + Definition.strValue));
			list.Add(Definition.filterMainPath + Definition.thresholdGlobalSubPath + "/HighThreshold/" + Definition.strValue);
			list2.Add(this.parHandling.ReadPar(Definition.filterMainPath + Definition.thresholdGlobalSubPath + "/HighThreshold/" + Definition.strValue));
			this.dbConnection.UpdateCurrentProductTable(list, list2);
		}

		// Token: 0x060015B7 RID: 5559 RVA: 0x000812F4 File Offset: 0x0007F4F4
		public bool CheckWrongDetectedPixel(int imagecount)
		{
			int num = Convert.ToInt32(this.parHandling.ReadPar(Definition.simulationWrongDetPix));
			bool flag = imagecount == 0;
			int num2;
			if (flag)
			{
				num2 = Definition.sumOfAllTestcards * this.RealImgCountForSim;
			}
			else
			{
				num2 = imagecount;
			}
			num2 *= 100;
			byte b = this.originalImageState;
			return (b & 1) == 0 && num > num2;
		}

		// Token: 0x060015B8 RID: 5560 RVA: 0x00081364 File Offset: 0x0007F564
		public bool SimulateOneTestcardOneImage(XRSImage imageforsimulation, int simmaterial, int simsize, int xpattern, int ypattern, int backgroundnoise, bool calcedgepointspattern)
		{
			bool flag = true;
			bool flag2 = imageforsimulation != null;
			if (flag2)
			{
				XRSImage xrsimage = imageforsimulation.DeepCopy();
				int num = Convert.ToInt32(xrsimage.SourceVoltage);
				double beltspeed = (double)xrsimage.ConveyingSpeed;
				bool simulateAllBalls = this.parHandling.ReadPar(Definition.simulationDrawAllBalls) == "1";
				string machinesubtype = this.parHandling.ReadPar(Definition.machineSubTypePath);
				string detectorsubtype = this.parHandling.ReadPar(Definition.detectorSubtypePath);
				flag = this.checkVersionAndSourceAdj(num, xrsimage.SourceCurrent, (int)xrsimage.Version, xrsimage.State, xrsimage.DetectorResolution, xrsimage.Binning);
				bool flag3 = num % 10 != 0;
				if (flag3)
				{
					num -= num % 10;
				}
				bool flag4 = !flag;
				if (flag4)
				{
					this.simModelcreateImage.Height = (int)xrsimage.Width;
					this.simModelcreateImage.Width = (int)xrsimage.Height;
					this.simModelforeignBody.LoadParameters(simmaterial, simsize, num, beltspeed, machinesubtype, detectorsubtype);
					if (calcedgepointspattern)
					{
						this.simModelcreateImage.SetStartAndEndPointsforPattern(xrsimage.Data, backgroundnoise);
					}
					xrsimage.Data = this.simModelcreateImage.SimulateTestcard(xrsimage, this.simModelforeignBody, num, xpattern, ypattern, backgroundnoise, simulateAllBalls);
					bool flag5 = this.simModelcreateImage.BallCounter == 0;
					if (flag5)
					{
						xrsimage.Data = this.simModelcreateImage.SimulateOneDefaultBall(xrsimage, this.simModelforeignBody, num, xpattern, ypattern, backgroundnoise, simulateAllBalls);
					}
					this.setPropertiesForXRSImage(xrsimage, simmaterial, simsize, 0, this.simModelcreateImage.BallCounter);
					this.checkBallCounts(this.simModelcreateImage.BallCounter);
					this.imageTransfer.SendImage(xrsimage);
				}
			}
			return flag;
		}

		// Token: 0x060015B9 RID: 5561 RVA: 0x00081518 File Offset: 0x0007F718
		public bool SimulateDifferentBallSizesOneImage(XRSImage imageforsimulation, int simmaterial, List<int> simsize, int xpattern, int ypattern, int backgroundnoise, bool calcedgepointspattern)
		{
			bool flag = true;
			bool flag2 = imageforsimulation != null && simsize != null && simsize.Count > 0;
			if (flag2)
			{
				XRSImage xrsimage = imageforsimulation.DeepCopy();
				int num = Convert.ToInt32(Math.Round((double)xrsimage.SourceVoltage));
				double beltspeed = (double)xrsimage.ConveyingSpeed;
				bool simulateAllBalls = this.parHandling.ReadPar(Definition.simulationDrawAllBalls) == "1";
				string machinesubtype = this.parHandling.ReadPar(Definition.machineSubTypePath);
				string detectorsubtype = this.parHandling.ReadPar(Definition.detectorSubtypePath);
				bool flag3 = num % 10 != 0;
				if (flag3)
				{
					num -= num % 10;
				}
				flag = this.checkVersionAndSourceAdj(num, xrsimage.SourceCurrent, (int)xrsimage.Version, xrsimage.State, xrsimage.DetectorResolution, xrsimage.Binning);
				bool flag4 = !flag;
				if (flag4)
				{
					this.simModelcreateImage.Height = (int)xrsimage.Width;
					this.simModelcreateImage.Width = (int)xrsimage.Height;
					if (calcedgepointspattern)
					{
						this.simModelcreateImage.SetStartAndEndPointsforPattern(xrsimage.Data, backgroundnoise);
					}
					xrsimage.Data = this.simModelcreateImage.SimulateTestcardWithDifferentSizes(xrsimage, this.simModelforeignBody, num, xpattern, ypattern, backgroundnoise, simulateAllBalls, simmaterial, simsize, beltspeed, machinesubtype, detectorsubtype, this.machinetype);
					bool flag5 = this.simModelcreateImage.BallCounter == 0;
					if (flag5)
					{
						xrsimage.Data = this.simModelcreateImage.SimulateOneDefaultBall(xrsimage, this.simModelforeignBody, num, xpattern, ypattern, backgroundnoise, simulateAllBalls);
					}
					this.setPropertiesForXRSImage(xrsimage, simmaterial, 128, 0, this.simModelcreateImage.BallCounter);
					this.checkBallCounts(this.simModelcreateImage.BallCounter);
					this.imageTransfer.SendImage(xrsimage);
				}
			}
			return flag;
		}

		// Token: 0x060015BA RID: 5562 RVA: 0x000816DC File Offset: 0x0007F8DC
		private bool checkVersionAndSourceAdj(int voltage, float current, int version, byte state, short resolution, byte binning)
		{
			bool result = false;
			float num = (float)voltage * current;
			bool flag = voltage < 40 || voltage > 100 || version < 7;
			if (flag)
			{
				result = true;
			}
			bool flag2 = num < 198f || num > 202f;
			if (flag2)
			{
				result = true;
			}
			state &= 128;
			bool flag3 = state > 0;
			if (flag3)
			{
				result = true;
			}
			bool flag4 = resolution != 400 || binning != 1;
			if (flag4)
			{
				result = true;
			}
			return result;
		}

		// Token: 0x060015BB RID: 5563 RVA: 0x0008175C File Offset: 0x0007F95C
		private void checkBallCounts(int ballcounts)
		{
			bool flag = ballcounts == 0;
			if (flag)
			{
				this.CheckBallCounts = true;
			}
		}

		// Token: 0x060015BC RID: 5564 RVA: 0x0008177C File Offset: 0x0007F97C
		private bool calcSensitivitiesAfterTraining(int totalcounts, int detectedcounts, string material, int ballsize, bool simIsDone, bool isAutotrain)
		{
			material = material.Trim();
			double num = 0.0;
			List<string> list = new List<string>();
			bool flag = true;
			bool result = false;
			bool flag2 = TestcardSimService.VASensitivities != null && TestcardSimService.GlassSensitivities != null;
			if (flag2)
			{
				bool flag3 = totalcounts != 0;
				if (flag3)
				{
					num = 100.0 * ((double)detectedcounts / (double)totalcounts);
				}
				bool flag4 = material == "VA";
				if (flag4)
				{
					TestcardSimService.VASensitivities.Add(ballsize);
					TestcardSimService.VASensitivities.Add((int)num);
				}
				else
				{
					bool flag5 = material == "Glass";
					if (flag5)
					{
						TestcardSimService.GlassSensitivities.Add(ballsize);
						TestcardSimService.GlassSensitivities.Add((int)num);
					}
				}
				if (simIsDone)
				{
					for (int i = 0; i < Definition.sumOfVATestcards; i++)
					{
						int num2 = 0;
						int num3 = 0;
						for (int j = 0; j < TestcardSimService.VASensitivities.Count; j++)
						{
							bool flag6 = TestcardSimService.VASensitivities[j] == i;
							if (flag6)
							{
								num2 += TestcardSimService.VASensitivities[j + 1];
								num3++;
							}
							j++;
						}
						bool flag7 = num3 != 0;
						if (flag7)
						{
							num2 /= num3;
						}
						bool flag8 = num2 >= 95 && flag;
						if (flag8)
						{
							flag = false;
							this.NewVABallSizeAfterSim = i;
						}
						bool flag9 = !flag && num2 < 95;
						if (flag9)
						{
							result = true;
						}
						list.Add(Convert.ToString(num2));
					}
					bool flag10 = flag;
					if (flag10)
					{
						this.NewVABallSizeAfterSim = 999;
					}
					flag = true;
					for (int k = 0; k < Definition.sumOfGlassTestcards; k++)
					{
						int num2 = 0;
						int num3 = 0;
						for (int l = 0; l < TestcardSimService.GlassSensitivities.Count; l++)
						{
							bool flag11 = TestcardSimService.GlassSensitivities[l] == k;
							if (flag11)
							{
								num2 += TestcardSimService.GlassSensitivities[l + 1];
								num3++;
							}
							l++;
						}
						bool flag12 = num3 != 0;
						if (flag12)
						{
							num2 /= num3;
						}
						bool flag13 = num2 >= 95 && flag;
						if (flag13)
						{
							flag = false;
							this.NewGlassBallSizeAfterSim = k;
						}
						bool flag14 = !flag && num2 < 95;
						if (flag14)
						{
							result = true;
						}
						list.Add(Convert.ToString(num2));
					}
					bool flag15 = flag;
					if (flag15)
					{
						this.NewGlassBallSizeAfterSim = 999;
					}
					bool flag16 = !this.CheckBallCountsBecauseOfNewVersion();
					if (flag16)
					{
						int num4 = Convert.ToInt32(this.parHandling.ReadPar(Definition.simulationCompSensVA)) + 1;
						int num5 = Convert.ToInt32(this.parHandling.ReadPar(Definition.simulationCompSensGlass)) + 1;
						int num6 = Convert.ToInt32(this.parHandling.ReadPar(Definition.simulationInitSensVA)) + 1;
						int num7 = Convert.ToInt32(this.parHandling.ReadPar(Definition.simulationInitSensGlass)) + 1;
						int num8 = Convert.ToInt32(this.parHandling.ReadPar(Definition.simulationActSensVA)) + 1;
						int num9 = Convert.ToInt32(this.parHandling.ReadPar(Definition.simulationActSensGlass)) + 1;
						this.parHandling.WriteValue(Definition.simulationCompSensVA, Convert.ToString((num4 < 999) ? num4 : 999));
						this.parHandling.WriteValue(Definition.simulationCompSensGlass, Convert.ToString((num5 < 999) ? num5 : 999));
						this.parHandling.WriteValue(Definition.simulationInitSensVA, Convert.ToString((num6 < 999) ? num6 : 999));
						this.parHandling.WriteValue(Definition.simulationInitSensGlass, Convert.ToString((num7 < 999) ? num7 : 999));
						this.parHandling.WriteValue(Definition.simulationActSensVA, Convert.ToString((num8 < 999) ? num8 : 999));
						this.parHandling.WriteValue(Definition.simulationActSensGlass, Convert.ToString((num9 < 999) ? num9 : 999));
					}
					this.dbConnection.SetTestcardSimSensitivities(this.productManager.CurrentLoadedProduct.TableName, list);
					if (isAutotrain)
					{
						this.saveContrastsForInitSensitivitiesAfterAutotrain();
					}
				}
			}
			return result;
		}

		// Token: 0x060015BD RID: 5565 RVA: 0x00081BD8 File Offset: 0x0007FDD8
		private void saveContrastsForInitSensitivitiesAfterAutotrain()
		{
			string tableName = this.productManager.CurrentLoadedProduct.TableName;
			int num = 0;
			string path = "";
			string contrast = "";
			this.dbConnection.GetLengthForInitSensTestcardSim(ref num, tableName);
			for (int i = 0; i < num; i++)
			{
				this.dbConnection.GetEntriesForInitSensTestcardSim(tableName, i, ref path, ref contrast);
				contrast = this.parHandling.ReadPar(path);
				this.dbConnection.SetTestcardSimContrastsForInitSens(this.productManager.CurrentLoadedProduct.TableName, i, contrast);
			}
		}

		// Token: 0x060015BE RID: 5566 RVA: 0x00081C6C File Offset: 0x0007FE6C
		public void SetContrastsBackToInitSensitivities()
		{
			string tableName = this.productManager.CurrentLoadedProduct.TableName;
			int num = 0;
			string path = "";
			string value = "";
			this.dbConnection.GetLengthForInitSensTestcardSim(ref num, tableName);
			for (int i = 0; i < num; i++)
			{
				this.dbConnection.GetEntriesForInitSensTestcardSim(tableName, i, ref path, ref value);
				this.parHandling.WriteValue(path, value);
			}
		}

		// Token: 0x060015BF RID: 5567 RVA: 0x00081CE4 File Offset: 0x0007FEE4
		private void setPropertiesForXRSImage(XRSImage image, int material, int size, int density, int ballcount)
		{
			image.Type = ImageType.TestcardSimulation;
			image.TestcardSimBallSize = Convert.ToByte(size);
			image.TestcardSimBallCount = Convert.ToInt16(ballcount);
			string testcardSimMaterial = "n.a.";
			bool flag = material == 1;
			if (flag)
			{
				testcardSimMaterial = "VA";
				density = 7900;
			}
			else
			{
				bool flag2 = material == 2;
				if (flag2)
				{
					testcardSimMaterial = "Glass";
					density = 2540;
				}
			}
			image.TestcardSimMaterial = testcardSimMaterial;
			image.TestcardSimMaterialDensity = Convert.ToInt16(density);
		}

		// Token: 0x060015C0 RID: 5568 RVA: 0x00081D64 File Offset: 0x0007FF64
		public string ConvertBallSizeToString(string strsimmaterial, int simsize, bool withPoint, bool withUnit)
		{
			string text = "n.a.";
			bool flag = strsimmaterial.Trim() == "VA";
			if (flag)
			{
				bool flag2 = simsize == 0;
				if (flag2)
				{
					text = "0,3";
				}
				else
				{
					bool flag3 = simsize == 1;
					if (flag3)
					{
						text = "0,4";
					}
					else
					{
						bool flag4 = simsize == 2;
						if (flag4)
						{
							text = "0,5";
						}
						else
						{
							bool flag5 = simsize == 3;
							if (flag5)
							{
								text = "0,6";
							}
							else
							{
								bool flag6 = simsize == 4;
								if (flag6)
								{
									text = "0,8";
								}
								else
								{
									bool flag7 = simsize == 5;
									if (flag7)
									{
										text = "1,0";
									}
									else
									{
										bool flag8 = simsize == 6;
										if (flag8)
										{
											text = "1,2";
										}
										else
										{
											bool flag9 = simsize == 7;
											if (flag9)
											{
												text = "1,6";
											}
											else
											{
												bool flag10 = simsize == 8;
												if (flag10)
												{
													text = "1,8";
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
			else
			{
				bool flag11 = strsimmaterial.Trim() == "Glass";
				if (flag11)
				{
					bool flag12 = simsize == 0;
					if (flag12)
					{
						text = "0,6";
					}
					else
					{
						bool flag13 = simsize == 1;
						if (flag13)
						{
							text = "0,8";
						}
						else
						{
							bool flag14 = simsize == 2;
							if (flag14)
							{
								text = "1,0";
							}
							else
							{
								bool flag15 = simsize == 3;
								if (flag15)
								{
									text = "1,2";
								}
								else
								{
									bool flag16 = simsize == 4;
									if (flag16)
									{
										text = "1,5";
									}
									else
									{
										bool flag17 = simsize == 5;
										if (flag17)
										{
											text = "2,0";
										}
										else
										{
											bool flag18 = simsize == 6;
											if (flag18)
											{
												text = "2,5";
											}
											else
											{
												bool flag19 = simsize == 7;
												if (flag19)
												{
													text = "3,0";
												}
												else
												{
													bool flag20 = simsize == 8;
													if (flag20)
													{
														text = "3,5";
													}
													else
													{
														bool flag21 = simsize == 9;
														if (flag21)
														{
															text = "4,0";
														}
														else
														{
															bool flag22 = simsize == 10;
															if (flag22)
															{
																text = "6,0";
															}
															else
															{
																bool flag23 = simsize == 11;
																if (flag23)
																{
																	text = "7,0";
																}
																else
																{
																	bool flag24 = simsize == 12;
																	if (flag24)
																	{
																		text = "8,0";
																	}
																	else
																	{
																		bool flag25 = simsize == 13;
																		if (flag25)
																		{
																			text = "9,0";
																		}
																		else
																		{
																			bool flag26 = simsize == 14;
																			if (flag26)
																			{
																				text = "10,0";
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
				}
			}
			bool flag27 = withPoint && text != "n.a.";
			if (flag27)
			{
				text = text.Replace(",", ".");
			}
			bool flag28 = text != "n.a." && withUnit;
			if (flag28)
			{
				text += " mm";
			}
			return text;
		}

		// Token: 0x060015C1 RID: 5569 RVA: 0x00081FCC File Offset: 0x000801CC
		public int ConvertBallSizeToInt(string strsimmaterial, string simsize, bool withUnit)
		{
			int result = 999;
			string str = "";
			if (withUnit)
			{
				str = " mm";
			}
			bool flag = strsimmaterial.Trim() == "VA";
			if (flag)
			{
				bool flag2 = simsize == "0.4" + str;
				if (flag2)
				{
					result = 0;
				}
				bool flag3 = simsize == "0.5" + str;
				if (flag3)
				{
					result = 1;
				}
				bool flag4 = simsize == "0.6" + str;
				if (flag4)
				{
					result = 2;
				}
				bool flag5 = simsize == "0.8" + str;
				if (flag5)
				{
					result = 3;
				}
				bool flag6 = simsize == "1.0" + str;
				if (flag6)
				{
					result = 4;
				}
				bool flag7 = simsize == "1.2" + str;
				if (flag7)
				{
					result = 5;
				}
				bool flag8 = simsize == "1.6" + str;
				if (flag8)
				{
					result = 6;
				}
				bool flag9 = simsize == "1.8" + str;
				if (flag9)
				{
					result = 7;
				}
				bool flag10 = simsize == "n.a.";
				if (flag10)
				{
					result = 999;
				}
			}
			else
			{
				bool flag11 = strsimmaterial.Trim() == "Glass";
				if (flag11)
				{
					bool flag12 = simsize == "0.8" + str;
					if (flag12)
					{
						result = 0;
					}
					bool flag13 = simsize == "1.0" + str;
					if (flag13)
					{
						result = 1;
					}
					bool flag14 = simsize == "1.2" + str;
					if (flag14)
					{
						result = 2;
					}
					bool flag15 = simsize == "1.5" + str;
					if (flag15)
					{
						result = 3;
					}
					bool flag16 = simsize == "2.0" + str;
					if (flag16)
					{
						result = 4;
					}
					bool flag17 = simsize == "2.5" + str;
					if (flag17)
					{
						result = 5;
					}
					bool flag18 = simsize == "3.0" + str;
					if (flag18)
					{
						result = 6;
					}
					bool flag19 = simsize == "3.5" + str;
					if (flag19)
					{
						result = 7;
					}
					bool flag20 = simsize == "4.0" + str;
					if (flag20)
					{
						result = 8;
					}
					bool flag21 = simsize == "6.0" + str;
					if (flag21)
					{
						result = 9;
					}
					bool flag22 = simsize == "7.0" + str;
					if (flag22)
					{
						result = 10;
					}
					bool flag23 = simsize == "8.0" + str;
					if (flag23)
					{
						result = 11;
					}
					bool flag24 = simsize == "9.0" + str;
					if (flag24)
					{
						result = 12;
					}
					bool flag25 = simsize == "10.0" + str;
					if (flag25)
					{
						result = 13;
					}
					bool flag26 = simsize == "n.a.";
					if (flag26)
					{
						result = 999;
					}
				}
			}
			return result;
		}

		// Token: 0x060015C2 RID: 5570 RVA: 0x0008228C File Offset: 0x0008048C
		public XRSImage GetLastNSavedImages(int imgcount)
		{
			XRSImage result = null;
			List<string> list = new List<string>();
			string archiveDirectoryThroughput = StoreImageData.archiveDirectoryThroughput;
			bool flag = Directory.Exists(archiveDirectoryThroughput);
			if (flag)
			{
				list.AddRange(Directory.GetFiles(archiveDirectoryThroughput).ToList<string>());
				bool flag2 = list.Count > 0;
				if (flag2)
				{
					result = this.imageArchiver.GetImage(list[list.Count - 1 - imgcount]);
				}
			}
			return result;
		}

		// Token: 0x060015C3 RID: 5571 RVA: 0x000822F8 File Offset: 0x000804F8
		public XRSImage GetLastNSavedImageFromAIModell()
		{
			XRSImage result = null;
			List<string> list = new List<string>();
			string tableName = "";
			this.dbConnection.GetCurrentProductTableNameAI(ref tableName);
			string archiveDirectoryAnomalyDetection = this.imageArchiver.GetArchiveDirectoryAnomalyDetection(tableName);
			bool flag = Directory.Exists(archiveDirectoryAnomalyDetection);
			if (flag)
			{
				list.AddRange(Directory.GetFiles(archiveDirectoryAnomalyDetection).ToList<string>());
				bool flag2 = list.Count > 0;
				if (flag2)
				{
					result = this.imageArchiver.GetImage(list[list.Count - 1]);
				}
			}
			return result;
		}

		// Token: 0x060015C4 RID: 5572 RVA: 0x00082380 File Offset: 0x00080580
		public string setAutotrainTestcardSimMsg()
		{
			string text = this.ConvertBallSizeToString("VA", Convert.ToInt32(this.parHandling.ReadPar(Definition.simulationActSensVA)), true, true);
			string text2 = this.ConvertBallSizeToString("Glass", Convert.ToInt32(this.parHandling.ReadPar(Definition.simulationActSensGlass)), true, true);
			string result = string.Concat(new string[]
			{
				LocalizationManager.Localize("txtInitTestcardSim"),
				":",
				Environment.NewLine,
				Environment.NewLine,
				LocalizationManager.Localize("txtVA"),
				": ",
				text,
				"      ",
				LocalizationManager.Localize("txtGlass"),
				": ",
				text2
			});
			LogbookManager.AddEntry(SSTEntryCode.InfoInitSensTestcardSim, 0, text, text2, "", "");
			return result;
		}

		// Token: 0x060015C5 RID: 5573 RVA: 0x00082474 File Offset: 0x00080674
		public string setRetrainTestcardSimMsg(bool ismsgname)
		{
			string text = LocalizationManager.Localize("txtWarning");
			string text2 = "";
			byte b = 0;
			bool flag = false;
			string a = this.parHandling.ReadPar(Definition.simulationAfterTimeIntervall);
			int num = Convert.ToInt32(this.parHandling.ReadPar(Definition.simulationActSensVA));
			int num2 = Convert.ToInt32(this.parHandling.ReadPar(Definition.simulationActSensGlass));
			int num3 = Convert.ToInt32(this.parHandling.ReadPar(Definition.simulationCompSensVA));
			int num4 = Convert.ToInt32(this.parHandling.ReadPar(Definition.simulationCompSensGlass));
			bool flag2 = !this.CheckBallCountsBecauseOfNewVersion();
			if (flag2)
			{
				num++;
				num2++;
				num3++;
				num4++;
			}
			string text3 = this.ConvertBallSizeToString("VA", num, true, true);
			string text4 = this.ConvertBallSizeToString("Glass", num2, true, true);
			string text5 = this.ConvertBallSizeToString("Glass", num4, true, true);
			string text6 = this.ConvertBallSizeToString("VA", num3, true, true);
			string text7 = this.ConvertBallSizeToString("VA", this.NewVABallSizeAfterSim, true, true);
			string text8 = this.ConvertBallSizeToString("Glass", this.NewGlassBallSizeAfterSim, true, true);
			int num5 = 1;
			this.dbConnection.GetTestcardSimMonitoringMessLev(ref num5);
			bool flag3 = this.NewVABallSizeAfterSim == num && this.NewGlassBallSizeAfterSim == num2;
			string text9;
			SSTEntryCode messagelevel;
			if (flag3)
			{
				text9 = LocalizationManager.Localize("txtNotChangedTestcardSim");
				text = LocalizationManager.Localize("txtInfo");
				messagelevel = SSTEntryCode.InfoActSensTestcardSim;
			}
			else
			{
				text9 = LocalizationManager.Localize("txtChangedTestcardSim");
				text = LocalizationManager.Localize("txtWarning");
				messagelevel = SSTEntryCode.WarningActSensTestcardSim;
				b = 1;
				flag = true;
			}
			bool flag4 = a == "1";
			if (flag4)
			{
				b += 2;
				bool flag5 = this.NewVABallSizeAfterSim <= num3 && this.NewGlassBallSizeAfterSim <= num4;
				string text10;
				if (flag5)
				{
					text = LocalizationManager.Localize("txtInfo");
					messagelevel = SSTEntryCode.InfoActSensTestcardSim;
					text10 = LocalizationManager.Localize("txtComplianceMsg");
				}
				else
				{
					bool flag6 = num5 == 0;
					if (flag6)
					{
						text = LocalizationManager.Localize("txtInfo");
						messagelevel = SSTEntryCode.InfoActSensTestcardSim;
					}
					else
					{
						bool flag7 = num5 == 1;
						if (flag7)
						{
							text = LocalizationManager.Localize("txtWarning");
							messagelevel = SSTEntryCode.WarningActSensTestcardSim;
						}
						else
						{
							bool flag8 = num5 == 2;
							if (flag8)
							{
								text = LocalizationManager.Localize("txtError");
								messagelevel = SSTEntryCode.ErrorActSensTestcardSim;
							}
						}
					}
					text10 = LocalizationManager.Localize("txtNoComplianceMsg");
					b += 2;
					flag = true;
				}
				text2 = string.Concat(new string[]
				{
					text10,
					".",
					Environment.NewLine,
					LocalizationManager.Localize("txtThreshold"),
					" ",
					LocalizationManager.Localize("txtVA"),
					": ",
					text6,
					" ",
					Environment.NewLine,
					LocalizationManager.Localize("txtThreshold"),
					" ",
					LocalizationManager.Localize("txtGlass"),
					": ",
					text5
				});
			}
			bool flag9 = flag;
			if (flag9)
			{
				this.errorSurvey.SetTestcardSimLogbookMsg(text3, text7, text4, text8, text6, text5, true, messagelevel, b);
			}
			string result;
			if (ismsgname)
			{
				result = text;
			}
			else
			{
				result = string.Concat(new string[]
				{
					text9,
					":",
					Environment.NewLine,
					Environment.NewLine,
					LocalizationManager.Localize("txtVA"),
					": ",
					text3,
					" --> ",
					text7,
					Environment.NewLine,
					LocalizationManager.Localize("txtGlass"),
					": ",
					text4,
					" --> ",
					text8,
					Environment.NewLine,
					Environment.NewLine,
					text2
				});
			}
			return result;
		}

		// Token: 0x060015C6 RID: 5574 RVA: 0x00082890 File Offset: 0x00080A90
		public string setComplianceTestcardSimMsg()
		{
			string text = LocalizationManager.Localize("txtWarning");
			byte compliance = 3;
			string text2 = this.parHandling.ReadPar(Definition.simulationAfterTimeIntervall);
			int num = Convert.ToInt32(this.parHandling.ReadPar(Definition.simulationActSensVA));
			int num2 = Convert.ToInt32(this.parHandling.ReadPar(Definition.simulationActSensGlass));
			bool flag = !this.CheckBallCountsBecauseOfNewVersion();
			if (flag)
			{
				num++;
				num2++;
			}
			string text3 = this.ConvertBallSizeToString("VA", num, true, true);
			string text4 = this.ConvertBallSizeToString("Glass", num2, true, true);
			string text5 = this.ConvertBallSizeToString("VA", this.NewVABallSizeAfterSim, true, true);
			string text6 = this.ConvertBallSizeToString("Glass", this.NewGlassBallSizeAfterSim, true, true);
			int num3 = 1;
			this.dbConnection.GetTestcardSimMonitoringMessLev(ref num3);
			bool flag2 = this.NewVABallSizeAfterSim == num && this.NewGlassBallSizeAfterSim == num2;
			string text7;
			SSTEntryCode messagelevel;
			if (flag2)
			{
				text7 = LocalizationManager.Localize("txtNotChangedTestcardSim");
				messagelevel = SSTEntryCode.InfoActSensTestcardSim;
			}
			else
			{
				text7 = LocalizationManager.Localize("txtChangedTestcardSim");
				messagelevel = SSTEntryCode.WarningActSensTestcardSim;
			}
			string text8 = LocalizationManager.Localize("txtSetComplianceMsg");
			string text9 = string.Concat(new string[]
			{
				text8,
				".",
				Environment.NewLine,
				LocalizationManager.Localize("txtThreshold"),
				" ",
				LocalizationManager.Localize("txtVA"),
				": ",
				text5,
				" ",
				Environment.NewLine,
				LocalizationManager.Localize("txtThreshold"),
				" ",
				LocalizationManager.Localize("txtGlass"),
				": ",
				text6
			});
			this.errorSurvey.SetTestcardSimLogbookMsg(text3, text5, text4, text6, text3, text4, true, messagelevel, compliance);
			return string.Concat(new string[]
			{
				text7,
				":",
				Environment.NewLine,
				Environment.NewLine,
				LocalizationManager.Localize("txtVA"),
				": ",
				text3,
				" --> ",
				text5,
				Environment.NewLine,
				LocalizationManager.Localize("txtGlass"),
				": ",
				text4,
				" --> ",
				text6,
				Environment.NewLine,
				Environment.NewLine,
				text9
			});
		}

		// Token: 0x060015C7 RID: 5575 RVA: 0x00082B44 File Offset: 0x00080D44
		public string setExpectedDurationMsg(int pixelcount, bool isRetrain)
		{
			double num = 0.0;
			double num2 = 8.793;
			double num3 = 0.905;
			double num4 = 1.551;
			double num5 = 1.594;
			double num6 = 4.19;
			string a = this.parHandling.ReadPar(Definition.simulationmodelPath);
			string a2 = this.parHandling.ReadPar(Definition.simulationmaterialPath);
			bool flag = this.parHandling.ReadPar(Definition.filterMainPath + Definition.comparerSubPath + "1/Enabled/" + Definition.strValue) == "1";
			bool flag2 = this.parHandling.ReadPar(Definition.filterMainPath + Definition.comparerSubPath + "9/Enabled/" + Definition.strValue) == "1";
			bool flag3 = this.parHandling.ReadPar(Definition.filterMainPath + Definition.comparerSubPath + "11/Enabled/" + Definition.strValue) == "1";
			bool flag4 = this.parHandling.ReadPar(Definition.filterMainPath + Definition.comparerSubPath + "15/Enabled/" + Definition.strValue) == "1";
			bool flag5 = this.parHandling.ReadPar(Definition.filterMainPath + "/lowDensity/Enabled/Value") == "1";
			bool flag6 = flag;
			if (flag6)
			{
				num = num2;
			}
			bool flag7 = flag2;
			if (flag7)
			{
				num += num3;
			}
			bool flag8 = flag3;
			if (flag8)
			{
				num += num4;
			}
			bool flag9 = flag4;
			if (flag9)
			{
				num += num5;
			}
			bool flag10 = flag5;
			if (flag10)
			{
				num += num6;
			}
			bool flag11 = !isRetrain;
			if (flag11)
			{
				bool flag12 = a == "0";
				if (flag12)
				{
					bool flag13 = a2 == "1";
					if (flag13)
					{
						num *= 0.363;
					}
					else
					{
						bool flag14 = a2 == "2";
						if (flag14)
						{
							num *= 0.636;
						}
					}
				}
				else
				{
					bool flag15 = a == "2";
					if (flag15)
					{
						num /= (double)Definition.sumOfAllTestcards;
					}
				}
			}
			int num7 = Convert.ToInt32(Math.Round(num * ((double)pixelcount / 1000000.0), 0));
			bool flag16 = num7 < 2;
			if (flag16)
			{
				num7 = 2;
			}
			return string.Concat(new string[]
			{
				Environment.NewLine,
				LocalizationManager.Localize("txtExpectedDuration"),
				": ",
				Convert.ToString(num7),
				" sec"
			});
		}

		// Token: 0x060015C8 RID: 5576 RVA: 0x00082DD8 File Offset: 0x00080FD8
		public bool CheckChangeOfSensitivities()
		{
			int num = Convert.ToInt32(this.parHandling.ReadPar(Definition.simulationActSensVA));
			int num2 = Convert.ToInt32(this.parHandling.ReadPar(Definition.simulationActSensGlass));
			bool result = true;
			bool flag = !this.CheckBallCountsBecauseOfNewVersion();
			if (flag)
			{
				num++;
				num2++;
			}
			bool flag2 = this.NewVABallSizeAfterSim == num && this.NewGlassBallSizeAfterSim == num2;
			if (flag2)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x060015C9 RID: 5577 RVA: 0x00082E50 File Offset: 0x00081050
		public DialogStyle setRetrainTestcardSimMsgLevel()
		{
			int num = Convert.ToInt32(this.parHandling.ReadPar(Definition.simulationActSensVA));
			int num2 = Convert.ToInt32(this.parHandling.ReadPar(Definition.simulationActSensGlass));
			int num3 = Convert.ToInt32(this.parHandling.ReadPar(Definition.simulationCompSensVA));
			int num4 = Convert.ToInt32(this.parHandling.ReadPar(Definition.simulationCompSensGlass));
			bool flag = !this.CheckBallCountsBecauseOfNewVersion();
			if (flag)
			{
				num++;
				num2++;
				num3++;
				num4++;
			}
			int num5 = 1;
			this.dbConnection.GetTestcardSimMonitoringMessLev(ref num5);
			string a = this.parHandling.ReadPar(Definition.simulationAfterTimeIntervall);
			bool flag2 = this.NewVABallSizeAfterSim == num && this.NewGlassBallSizeAfterSim == num2;
			DialogStyle result;
			if (flag2)
			{
				result = DialogStyle.Info;
			}
			else
			{
				result = DialogStyle.Warning;
			}
			bool flag3 = a == "1";
			if (flag3)
			{
				bool flag4 = this.NewVABallSizeAfterSim <= num3 && this.NewGlassBallSizeAfterSim <= num4;
				if (flag4)
				{
					result = DialogStyle.Info;
				}
				else
				{
					bool flag5 = num5 == 0;
					if (flag5)
					{
						result = DialogStyle.Info;
					}
					else
					{
						bool flag6 = num5 == 1;
						if (flag6)
						{
							result = DialogStyle.Warning;
						}
						else
						{
							bool flag7 = num5 == 2;
							if (flag7)
							{
								result = DialogStyle.Error;
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x060015CA RID: 5578 RVA: 0x00082F8C File Offset: 0x0008118C
		public void setParameterAfterTestcardSim(bool isAutotrain)
		{
			string value = Convert.ToString(this.NewVABallSizeAfterSim);
			string value2 = Convert.ToString(this.NewGlassBallSizeAfterSim);
			if (isAutotrain)
			{
				this.parHandling.WriteValue(Definition.simulationInitSensVA, value);
				this.parHandling.WriteValue(Definition.simulationInitSensGlass, value2);
				this.parHandling.WriteValue(Definition.simulationCompSensVA, value);
				this.parHandling.WriteValue(Definition.simulationCompSensGlass, value2);
				this.setDBValuesforMonitoring();
			}
			this.parHandling.WriteValue(Definition.simulationActSensVA, value);
			this.parHandling.WriteValue(Definition.simulationActSensGlass, value2);
			bool flag = !this.checkContCountIsEnable;
			if (flag)
			{
				this.parHandling.WriteValue(Definition.contaminationsCounterEnablePath, "0");
			}
			this.SetProperties();
		}

		// Token: 0x060015CB RID: 5579 RVA: 0x00083058 File Offset: 0x00081258
		public void setParameterAfterMonitoringSensitivities(bool senschanged)
		{
			if (senschanged)
			{
				int num = this.NewVABallSizeAfterSim;
				int num2 = this.NewGlassBallSizeAfterSim;
				bool flag = !this.CheckBallCountsBecauseOfNewVersion() && num > 0 && num < 999;
				if (flag)
				{
					num--;
				}
				bool flag2 = !this.CheckBallCountsBecauseOfNewVersion() && num2 > 0 && num2 < 999;
				if (flag2)
				{
					num2--;
				}
				this.parHandling.WriteValue(Definition.simulationActSensVA, Convert.ToString(num));
				this.parHandling.WriteValue(Definition.simulationActSensGlass, Convert.ToString(num2));
				this.SetProperties();
			}
			else
			{
				bool flag3 = !this.checkContCountIsEnable;
				if (flag3)
				{
					this.parHandling.WriteValue(Definition.contaminationsCounterEnablePath, "0");
				}
			}
		}

		// Token: 0x060015CC RID: 5580 RVA: 0x0008311C File Offset: 0x0008131C
		public bool MonitoringSensCheckMachineStates(MachineState machinestate)
		{
			return machinestate.State == MachineStates.ConveyorOff || machinestate.State == MachineStates.ConveyorOnKeyOpened || machinestate.State == MachineStates.ConveyorOnKeyClosedXrayOff || machinestate.State == MachineStates.Inspecting || machinestate.State == MachineStates.NoConnectToSourceOrDetector || machinestate.State == MachineStates.StateForNoBeltStartOnError;
		}

		// Token: 0x060015CD RID: 5581 RVA: 0x0008316C File Offset: 0x0008136C
		public bool setOrCheckImgToSimulateIsNew(bool setPath)
		{
			List<string> list = new List<string>();
			bool result = false;
			XRSImage lastNSavedImages = this.GetLastNSavedImages(0);
			bool flag = lastNSavedImages != null;
			if (flag)
			{
				if (setPath)
				{
					this.xrsimageFilePathOld = lastNSavedImages.FilePath;
				}
				else
				{
					bool flag2 = lastNSavedImages.FilePath != this.xrsimageFilePathOld;
					if (flag2)
					{
						this.xrsimageFilePathOld = lastNSavedImages.FilePath;
						result = true;
					}
				}
			}
			return result;
		}

		// Token: 0x060015CE RID: 5582 RVA: 0x000831DC File Offset: 0x000813DC
		private void setDBValuesforMonitoring()
		{
			int num = 0;
			string a = "";
			string tableName = this.productManager.CurrentLoadedProduct.TableName;
			int num2 = Convert.ToInt32(this.parHandling.ReadPar(Definition.simulationCompSensVA));
			int num3 = Convert.ToInt32(this.parHandling.ReadPar(Definition.simulationCompSensGlass));
			this.dbConnection.GetLengthForMonitoringTestcardSim(ref num, tableName);
			bool flag = !this.CheckBallCountsBecauseOfNewVersion();
			if (flag)
			{
				num2++;
				num3++;
			}
			string vacompl = this.ConvertBallSizeToString("VA", num2, true, true);
			string glassCompl = this.ConvertBallSizeToString("Glass", num3, true, true);
			bool flag2 = num < 100;
			if (flag2)
			{
				this.dbConnection.SetTestcardSimSensitivitiesForMonitoring(tableName, num, DateTime.Now.ToString(new CultureInfo("de-DE")), this.ConvertBallSizeToString("VA", this.NewVABallSizeAfterSim, true, true), this.ConvertBallSizeToString("Glass", this.NewGlassBallSizeAfterSim, true, true), vacompl, glassCompl);
				bool flag3 = num == 99;
				if (flag3)
				{
					this.dbConnection.SetTestcardSimSensitivitiesForMonitoring(tableName, num, "0", "0", "0", "0", "0");
				}
			}
			bool flag4 = num == 100;
			if (flag4)
			{
				for (int i = 0; i < 100; i++)
				{
					this.dbConnection.GetTimestampForMonitoringTestcardSim(tableName, i, ref a);
					bool flag5 = a == "0";
					if (flag5)
					{
						this.dbConnection.SetTestcardSimSensitivitiesForMonitoring(tableName, i, DateTime.Now.ToString(new CultureInfo("de-DE")), this.ConvertBallSizeToString("VA", this.NewVABallSizeAfterSim, true, true), this.ConvertBallSizeToString("Glass", this.NewGlassBallSizeAfterSim, true, true), vacompl, glassCompl);
						int id = i + 1;
						bool flag6 = i == 99;
						if (flag6)
						{
							id = 0;
						}
						this.dbConnection.SetTestcardSimSensitivitiesForMonitoring(tableName, id, "0", "0", "0", "0", "0");
						break;
					}
				}
			}
		}

		// Token: 0x060015CF RID: 5583 RVA: 0x00083404 File Offset: 0x00081604
		public bool CompareActSensWithDB()
		{
			bool result = true;
			int num = Convert.ToInt32(this.parHandling.ReadPar(Definition.simulationActSensVA));
			int num2 = Convert.ToInt32(this.parHandling.ReadPar(Definition.simulationActSensGlass));
			int num3 = 999;
			int num4 = 999;
			List<string> list = new List<string>();
			this.dbConnection.GetTestcardSimSensitivities(this.productManager.CurrentLoadedProduct.TableName, list);
			bool flag = list.Count != 0;
			if (flag)
			{
				for (int i = 0; i < Definition.sumOfVATestcards; i++)
				{
					int num5 = Convert.ToInt32(list[i]);
					bool flag2 = num5 >= 95;
					if (flag2)
					{
						num3 = i;
						break;
					}
				}
				for (int j = Definition.sumOfVATestcards; j < Definition.sumOfAllTestcards; j++)
				{
					int num5 = Convert.ToInt32(list[j]);
					bool flag3 = num5 >= 95;
					if (flag3)
					{
						num4 = j - Definition.sumOfVATestcards;
						break;
					}
				}
			}
			bool flag4 = num == num3 && num2 == num4;
			if (flag4)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x060015D0 RID: 5584 RVA: 0x00083534 File Offset: 0x00081734
		public bool CheckAndSetActivationTestcardSim(ProductData product)
		{
			string a = this.parHandling.ReadPar(Definition.detectorSesotecPixelSizePath);
			DetectorType detectorType = DetectorType.Sesotec;
			this.dbConnection.GetDetectorType(ref detectorType);
			bool flag = false;
			this.dbConnection.GetDemoMode(ref flag);
			bool flag2 = (!product.IsRegionProduct && !product.IsMultiProduct && !product.IsLaneProduct) || detectorType != DetectorType.Sesotec || !(a == "0.4");
			bool flag3 = !flag2;
			if (flag3)
			{
				this.parHandling.WriteValue(Definition.simulationAfterAutolearn, "0");
				this.parHandling.WriteValue(Definition.simulationAfterTimeIntervall, "0");
				this.parHandling.WriteValue(Autotrain.nAutolearnPath, "5");
			}
			bool flag4 = flag;
			if (flag4)
			{
				flag2 = true;
			}
			return flag2;
		}

		// Token: 0x060015D1 RID: 5585 RVA: 0x00083604 File Offset: 0x00081804
		public bool CheckBallCountsBecauseOfNewVersion()
		{
			bool result = true;
			List<string> list = new List<string>();
			this.dbConnection.GetTestcardSimSensitivities(this.productManager.CurrentLoadedProduct.TableName, list);
			bool flag = list.Count == Definition.sumOfAllTestcards && list[Definition.sumOfVATestcards - 1] == "" && list[Definition.sumOfAllTestcards - 1] == "";
			if (flag)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x060015D2 RID: 5586 RVA: 0x00083682 File Offset: 0x00081882
		public void SetProperties()
		{
			this.OnPropertiesSet();
		}

		// Token: 0x060015D3 RID: 5587 RVA: 0x0008368C File Offset: 0x0008188C
		protected virtual void OnPropertiesSet()
		{
			Action propertiesSet = this.PropertiesSet;
			bool flag = propertiesSet != null;
			if (flag)
			{
				propertiesSet();
			}
		}

		// Token: 0x04000E45 RID: 3653
		private ParameterHandling parHandling;

		// Token: 0x04000E46 RID: 3654
		private DBConnection dbConnection;

		// Token: 0x04000E47 RID: 3655
		private ImageDataTransfer imageTransfer;

		// Token: 0x04000E48 RID: 3656
		private StoreImageData imageArchiver;

		// Token: 0x04000E49 RID: 3657
		private SimModelForeignBody simModelforeignBody;

		// Token: 0x04000E4A RID: 3658
		private SimModelCreateImage simModelcreateImage;

		// Token: 0x04000E4B RID: 3659
		private ProductManager productManager;

		// Token: 0x04000E4D RID: 3661
		public bool CheckVersionAndXRaySource;

		// Token: 0x04000E4E RID: 3662
		private int nAutolearn;

		// Token: 0x04000E4F RID: 3663
		private int totalSimImageCounter = 0;

		// Token: 0x04000E50 RID: 3664
		private int testpatternxy = 10;

		// Token: 0x04000E51 RID: 3665
		private static List<int> VASensitivities;

		// Token: 0x04000E52 RID: 3666
		private static List<int> GlassSensitivities;

		// Token: 0x04000E54 RID: 3668
		private bool checkContCountIsEnable;

		// Token: 0x04000E59 RID: 3673
		private byte originalImageState;

		// Token: 0x04000E5A RID: 3674
		private bool simulationIsDoneVATimeIntvervall;

		// Token: 0x04000E5B RID: 3675
		private bool simulationIsDoneGlassTimeIntervall;

		// Token: 0x04000E5C RID: 3676
		private XRSImage selectedimgForTimeIntervall;

		// Token: 0x04000E60 RID: 3680
		private string xrsimageFilePathOld;

		// Token: 0x04000E61 RID: 3681
		private ErrorSurvey errorSurvey;

		// Token: 0x04000E62 RID: 3682
		private string machinetype;
	}
}
