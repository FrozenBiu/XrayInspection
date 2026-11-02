using System;
using System.Collections.Generic;
using System.Globalization;
using DatabaseConnection;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x02000046 RID: 70
	public static class BulkMode
	{
		// Token: 0x060006F1 RID: 1777 RVA: 0x0002D6CC File Offset: 0x0002B8CC
		public static void Activate(DBConnection dbConnection, ParameterHandling parHandling)
		{
			parHandling.WriteValue("/filter/acquisition/ContinuousModeFiltering/Value", "1");
			parHandling.WriteValue("/filter/acquisition/ContinuousMode/Value", "1");
			parHandling.WriteValue("/filter/acquisition/AddedFrontLines/Value", "0");
			parHandling.WriteValue("/filter/acquisition/AddedBackLines/Value", "0");
			parHandling.WriteValue("/filter/cropping/Enable/Value", "0");
			parHandling.WriteValue("/filter/acquisition/ConnectImages/Value", "0");
			parHandling.WriteValue("/filter/acquisition/MinLines/Value", "0");
			parHandling.WriteValue("/filter/acquisition/SaveImage/Value", "1");
			parHandling.WriteValue("/filter/acquisition/SaveOnlyIfContaminant/Value", "0");
			parHandling.WriteValue("/filter/acquisition/SendImage/Value", "0");
			bool flag = parHandling.ReadPar(Definition.bulkShowContImages) == "1";
			if (flag)
			{
				parHandling.WriteValue("/filter/acquisition/SendOnlyIfContaminant/Value", "1");
			}
			else
			{
				parHandling.WriteValue("/filter/acquisition/SendOnlyIfContaminant/Value", "0");
			}
			parHandling.WriteValue("/filter/acquisition/ShowImage/Value", "1");
			parHandling.WriteValue("/filter/acquisition/ShowOnlyIfContaminant/Value", "0");
			parHandling.WriteValue("/filter/marker/PaintRectangles/Value", "0");
			parHandling.WriteValue("/application/NSaveImagesGood/Value", "200");
			parHandling.WriteValue("/application/NSaveImagesBad/Value", "200");
			bool flag2 = parHandling.ReadPar(FlapsOverIOs.bulkflapsEnabledPath) == "1" && parHandling.ReadPar(Definition.rejecterRejecterModePath) != "1";
			if (flag2)
			{
				parHandling.WriteValue(Definition.rejecterRejecterModePath, "1");
				parHandling.WriteValue(Definition.rejecterNRejectsLimitPath, "20");
				parHandling.WriteValue(Definition.rejecterRejectPeriodLimitPath, "30");
			}
			for (int i = 1; i <= 8; i++)
			{
				parHandling.WriteValue("/filter/comparer/comparer" + i.ToString() + "/Enabled/Value", "1");
			}
			for (int j = 1; j <= Definition.nObjectFinder; j++)
			{
				parHandling.WriteValue(Definition.filterMainPath + Definition.objectFindersSubPath + j.ToString() + "/Enabled/Value", "0");
			}
			int num = Convert.ToInt32(parHandling.ReadPar(Definition.filterMainPath + "/comparer/comparer1/Contrast/Value"));
			bool flag3 = num > 10000;
			if (flag3)
			{
				int num2 = Convert.ToInt32(parHandling.ReadPar(Definition.backroundNoisePath));
				int value = 10000 - num2 * 100;
				for (int k = 1; k <= 8; k++)
				{
					parHandling.WriteValue(Definition.filterMainPath + "/comparer/comparer" + k.ToString() + "/HighThreshold/Value", Convert.ToString(value));
				}
				parHandling.WriteValue(Definition.filterMainPath + Definition.thresholdSubPath + "/HighThreshold/Value", Convert.ToString(value));
				parHandling.WriteValue(Definition.filterMainPath + Definition.thresholdGlobalSubPath + "/HighThreshold/Value", Convert.ToString(value));
			}
			parHandling.WriteValue(Definition.isLiveCalibrationActivePath, "2");
			parHandling.WriteValue("/filter/xrayoffimage/Enable/Value", "1");
		}

		// Token: 0x060006F2 RID: 1778 RVA: 0x0002D9F4 File Offset: 0x0002BBF4
		public static void DeactivateForXRayOff(DBConnection dbConnection, ParameterHandling parHandling)
		{
			parHandling.WriteValue("/filter/acquisition/ContinuousModeFiltering/Value", "0");
			parHandling.WriteValue("/filter/acquisition/ContinuousMode/Value", "0");
			parHandling.WriteValue("/filter/acquisition/SaveImage/Value", "0");
			parHandling.WriteValue("/filter/acquisition/SaveOnlyIfContaminant/Value", "0");
			parHandling.WriteValue("/filter/acquisition/SendImage/Value", "0");
			parHandling.WriteValue("/filter/acquisition/SendOnlyIfContaminant/Value", "0");
			parHandling.WriteValue("/filter/acquisition/ShowImage/Value", "0");
			parHandling.WriteValue("/filter/acquisition/ShowOnlyIfContaminant/Value", "0");
		}

		// Token: 0x060006F3 RID: 1779 RVA: 0x0002DA8C File Offset: 0x0002BC8C
		public static int GetMinMaxLines(DBConnection dbConnection)
		{
			int result = 5;
			string s = "";
			dbConnection.GetParameterValue("ConveyorSpeed", ref s);
			double num;
			bool flag = double.TryParse(s, NumberStyles.Float, new CultureInfo("en-US"), out num);
			if (flag)
			{
				result = (int)Math.Round(16.666 * num - 3.333, 0);
			}
			return result;
		}

		// Token: 0x060006F4 RID: 1780 RVA: 0x0002DAF4 File Offset: 0x0002BCF4
		public static void ActivateObjectModeAutotrain(DBConnection dbConnection, ParameterHandling parHandling)
		{
			int num = 10;
			int num2 = 0;
			bool flag = parHandling.ReadPar(Definition.detectorSesotecPixelSizePath) == "0.4";
			if (flag)
			{
				num *= 2;
			}
			string value = Convert.ToString(num);
			parHandling.WriteValue("/machine/BulkOverlapImgActivate/Value", "0");
			int.TryParse(parHandling.ReadPar("/filter/acquisition/MaxLines/Value"), out num2);
			BulkMode.maxLinesBulk = num2;
			parHandling.WriteValue("/filter/acquisition/MaxLines/Value", BulkMode.linesForAutotrain.ToString());
			parHandling.WriteValue("/filter/acquisition/ContinuousMode/Value", "0");
			parHandling.WriteValue("/filter/acquisition/AddedBackLines/Value", BulkMode.linesForAutotrain.ToString());
			parHandling.WriteValue("/filter/acquisition/AddedFrontLines/Value", value);
			parHandling.WriteValue("/filter/acquisition/MinLines/Value", value);
			parHandling.WriteValue("/filter/acquisition/ConnectImages/Value", "1");
			parHandling.WriteValue("/filter/acquisition/NConnectedLines/Value", "2000");
			ErrorSurvey.DisableImageDelayError();
		}

		// Token: 0x060006F5 RID: 1781 RVA: 0x0002DBDC File Offset: 0x0002BDDC
		public static void DeactivateObjectModeAutotrain(DBConnection dbConnection, ParameterHandling parHandling)
		{
			parHandling.WriteValue("/filter/acquisition/ContinuousMode/Value", "1");
			parHandling.WriteValue("/filter/acquisition/AddedBackLines/Value", "0");
			parHandling.WriteValue("/filter/acquisition/AddedFrontLines/Value", "0");
			parHandling.WriteValue("/filter/acquisition/MinLines/Value", "10");
			parHandling.WriteValue("/filter/acquisition/ConnectImages/Value", "0");
			parHandling.WriteValue("/filter/acquisition/NConnectedLines/Value", "0");
			parHandling.WriteValue("/filter/cropping/Enable/Value", "0");
			parHandling.WriteValue("/filter/cropping/AddedLeftPixel/Value", "0");
			parHandling.WriteValue("/filter/cropping/AddedRightPixel/Value", "0");
			parHandling.WriteValue("/filter/acquisition/SendImage/Value", "0");
			bool flag = parHandling.ReadPar(Definition.bulkShowContImages) == "1";
			if (flag)
			{
				parHandling.WriteValue("/filter/acquisition/SendOnlyIfContaminant/Value", "1");
			}
			else
			{
				parHandling.WriteValue("/filter/acquisition/SendOnlyIfContaminant/Value", "0");
			}
			parHandling.WriteValue("/filter/acquisition/MaxLines/Value", BulkMode.maxLinesBulk.ToString());
			ErrorSurvey.EnableImageDelayError();
			BulkMode.ActivateOverlappingImages(parHandling);
		}

		// Token: 0x060006F6 RID: 1782 RVA: 0x0002DCF4 File Offset: 0x0002BEF4
		public static void ActivateAtComissioning(DBConnection dbConnection, ParameterHandling parHandling)
		{
			string text = "1";
			string tableName = null;
			int num = 1;
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			List<string> list3 = new List<string>();
			List<string> list4 = new List<string>();
			string text2 = FlapsOverIOs.bulkflapsEnabledPath;
			dbConnection.GetCurrentProductTableName(ref tableName);
			dbConnection.GetFlapsCount(tableName, ref num);
			int num2;
			int.TryParse(parHandling.ReadPar(Definition.imageVersionElinosPath), out num2);
			bool flag = num2 == 30;
			if (flag)
			{
				BulkMode.NuvoPCPerformance = true;
			}
			else
			{
				BulkMode.NuvoPCPerformance = false;
			}
			DetectorType detectorType = DetectorType.Unknown;
			dbConnection.GetDetectorType(ref detectorType);
			string a = "";
			bool flag2 = detectorType == DetectorType.Sesotec;
			if (flag2)
			{
				a = parHandling.ReadPar(Definition.detectorSesotecPixelSizePath);
			}
			int num3 = Definition.nComparerDefault;
			bool flag3 = BulkMode.NuvoPCPerformance && a == "0.4";
			if (flag3)
			{
				num3 = Definition.nComparerHXBulkNuvo;
			}
			dbConnection.CreateValveActivityEntries();
			dbConnection.SetIntegratedValidationProcess(false);
			dbConnection.UpdateTable(DBConnection.defaultApplication, text2, text);
			text2 = "/hardware/rejecter/bulkflaps/EnableValves/Value";
			parHandling.WriteValue(text2, "0", false);
			dbConnection.UpdateTable(tableName, text2, "0");
			list3.Add(text2);
			list4.Add("0");
			text2 = "/hardware/svt/Enable/Value";
			parHandling.WriteValue(text2, "0", false);
			dbConnection.UpdateTable(tableName, text2, "0");
			list3.Add(text2);
			list4.Add("0");
			for (int i = 1; i <= num; i++)
			{
				text2 = "/hardware/rejecter/rejecter" + i.ToString() + "/Enabled/Value";
				parHandling.WriteValue(text2, text, false);
				dbConnection.UpdateTable(tableName, text2, text);
				list3.Add(text2);
				list4.Add(text);
				text2 = "/hardware/rejecter/rejecter" + i.ToString() + "/Length/Value";
				parHandling.WriteValue(text2, "0", false);
				dbConnection.UpdateTable(tableName, text2, "0");
				list3.Add(text2);
				list4.Add(text);
				text2 = "/hardware/rejecter/rejecter" + i.ToString() + "/Delay/Value";
				parHandling.WriteValue(text2, "700", false);
				dbConnection.UpdateTable(tableName, text2, "700");
				list3.Add(text2);
				list4.Add("700");
			}
			list.Add(FlapsOverIOs.bulkflapsBorderPix1Path);
			list.Add(FlapsOverIOs.bulkflapsBorderPix2Path);
			list.Add(FlapsOverIOs.bulkflapsBorderPix3Path);
			list2.Add(parHandling.ReadPar(FlapsOverIOs.bulkflapsBorderPix1Path));
			list2.Add(parHandling.ReadPar(FlapsOverIOs.bulkflapsBorderPix2Path));
			list2.Add(parHandling.ReadPar(FlapsOverIOs.bulkflapsBorderPix3Path));
			list.Add(Definition.isLiveCalibrationActivePath);
			list2.Add("2");
			dbConnection.UpdateCurrentProductTable(list, list2);
			parHandling.WriteValue(Definition.isLiveCalibrationActivePath, "2");
			BulkMode.writeDefaultAndActualProd(parHandling, "/filter/acquisition/ContinuousModeFiltering/Value", "0", list3, list4);
			BulkMode.writeDefaultAndActualProd(parHandling, "/filter/acquisition/ContinuousMode/Value", "0", list3, list4);
			BulkMode.writeDefaultAndActualProd(parHandling, "/filter/acquisition/AddedFrontLines/Value", "0", list3, list4);
			BulkMode.writeDefaultAndActualProd(parHandling, "/filter/acquisition/AddedBackLines/Value", "0", list3, list4);
			BulkMode.writeDefaultAndActualProd(parHandling, "/filter/cropping/Enable/Value", "0", list3, list4);
			BulkMode.writeDefaultAndActualProd(parHandling, "/filter/acquisition/ConnectImages/Value", "0", list3, list4);
			BulkMode.writeDefaultAndActualProd(parHandling, "/filter/acquisition/MinLines/Value", "100", list3, list4);
			BulkMode.writeDefaultAndActualProd(parHandling, "/filter/acquisition/MaxLines/Value", "100", list3, list4);
			BulkMode.writeDefaultAndActualProd(parHandling, "/filter/acquisition/SaveImage/Value", "0", list3, list4);
			BulkMode.writeDefaultAndActualProd(parHandling, "/filter/acquisition/SaveOnlyIfContaminant/Value", "0", list3, list4);
			BulkMode.writeDefaultAndActualProd(parHandling, "/filter/acquisition/SendImage/Value", "0", list3, list4);
			BulkMode.writeDefaultAndActualProd(parHandling, "/filter/acquisition/SendOnlyIfContaminant/Value", "0", list3, list4);
			BulkMode.writeDefaultAndActualProd(parHandling, "/filter/acquisition/ShowImage/Value", "0", list3, list4);
			BulkMode.writeDefaultAndActualProd(parHandling, "/filter/acquisition/ShowOnlyIfContaminant/Value", "0", list3, list4);
			BulkMode.writeDefaultAndActualProd(parHandling, "/filter/marker/PaintRectangles/Value", "0", list3, list4);
			BulkMode.writeDefaultAndActualProd(parHandling, "/filter/thresholdGlobal/Enabled/Value", "1", list3, list4);
			BulkMode.writeDefaultAndActualProd(parHandling, "/filter/threshold/Enabled/Value", "0", list3, list4);
			BulkMode.writeDefaultAndActualProd(parHandling, Definition.rejecterRejecterModePath, "1", list3, list4);
			BulkMode.writeDefaultAndActualProd(parHandling, Definition.rejecterNRejectsLimitPath, "20", list3, list4);
			BulkMode.writeDefaultAndActualProd(parHandling, Definition.rejecterRejectPeriodLimitPath, "60", list3, list4);
			for (int j = 1; j <= Definition.nComparerHXBulkNuvo; j++)
			{
				bool flag4 = j <= num3;
				if (flag4)
				{
					BulkMode.writeDefaultAndActualProd(parHandling, "/filter/comparer/comparer" + j.ToString() + "/Enabled/Value", "1", list3, list4);
				}
				else
				{
					BulkMode.writeDefaultAndActualProd(parHandling, "/filter/comparer/comparer" + j.ToString() + "/Enabled/Value", "0", list3, list4);
				}
			}
			for (int k = 1; k <= Definition.nObjectFinder; k++)
			{
				BulkMode.writeDefaultAndActualProd(parHandling, Definition.filterMainPath + Definition.objectFindersSubPath + k.ToString() + "/Enabled/Value", "0", list3, list4);
			}
			for (int l = 1; l <= Definition.nComparerHXBulkNuvo; l++)
			{
				list3.Add(Definition.filterMainPath + "/comparer/comparer" + l.ToString() + "/HighThreshold/Value");
				list4.Add("8500");
				BulkMode.writeDefaultAndActualProd(parHandling, Definition.filterMainPath + "/comparer/comparer" + l.ToString() + "/HighThreshold/Value", "8500", list3, list4);
			}
			BulkMode.writeDefaultAndActualProd(parHandling, Definition.filterMainPath + Definition.thresholdSubPath + "/HighThreshold/Value", "8500", list3, list4);
			BulkMode.writeDefaultAndActualProd(parHandling, Definition.filterMainPath + Definition.thresholdGlobalSubPath + "/HighThreshold/Value", "8500", list3, list4);
			dbConnection.UpdateTable(DBConnection.defaultApplication, list3, list4);
		}

		// Token: 0x060006F7 RID: 1783 RVA: 0x0002E328 File Offset: 0x0002C528
		public static void DeactivateAtComissioning(DBConnection dbConnection, ParameterHandling parHandling)
		{
			string text = "0";
			string tableName = null;
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			List<string> list3 = new List<string>();
			List<string> list4 = new List<string>();
			string text2 = FlapsOverIOs.bulkflapsEnabledPath;
			dbConnection.GetCurrentProductTableName(ref tableName);
			dbConnection.UpdateTable(DBConnection.defaultApplication, text2, text);
			for (int i = 1; i <= 4; i++)
			{
				text2 = "/hardware/rejecter/rejecter" + i.ToString() + "/Enabled/Value";
				parHandling.WriteValue(text2, text, false);
				dbConnection.UpdateTable(tableName, text2, text);
				list3.Add(text2);
				list4.Add(text);
				text2 = "/hardware/rejecter/rejecter" + i.ToString() + "/Length/Value";
				parHandling.WriteValue(text2, "100", false);
				dbConnection.UpdateTable(tableName, text2, "100");
				list3.Add(text2);
				list4.Add("100");
				text2 = "/hardware/rejecter/rejecter" + i.ToString() + "/Delay/Value";
				parHandling.WriteValue(text2, "100", false);
				dbConnection.UpdateTable(tableName, text2, "100");
				list3.Add(text2);
				list4.Add("100");
				text2 = "/hardware/svt/Enable/Value";
				parHandling.WriteValue(text2, "0", false);
				dbConnection.UpdateTable(tableName, text2, "0");
				list3.Add(text2);
				list4.Add("0");
			}
			list.Add(FlapsOverIOs.bulkflapsBorderPix1Path);
			list.Add(FlapsOverIOs.bulkflapsBorderPix2Path);
			list.Add(FlapsOverIOs.bulkflapsBorderPix3Path);
			list2.Add("0");
			list2.Add("0");
			list2.Add("0");
			list.Add(Definition.isLiveCalibrationActivePath);
			list2.Add("1");
			dbConnection.UpdateCurrentProductTable(list, list2);
			parHandling.WriteValue(Definition.isLiveCalibrationActivePath, "1");
			BulkMode.writeDefaultAndActualProd(parHandling, "/filter/acquisition/ContinuousModeFiltering/Value", "0", list3, list4);
			BulkMode.writeDefaultAndActualProd(parHandling, "/filter/acquisition/ContinuousMode/Value", "0", list3, list4);
			BulkMode.writeDefaultAndActualProd(parHandling, "/filter/acquisition/AddedFrontLines/Value", "10", list3, list4);
			BulkMode.writeDefaultAndActualProd(parHandling, "/filter/acquisition/AddedBackLines/Value", "10", list3, list4);
			BulkMode.writeDefaultAndActualProd(parHandling, "/filter/cropping/Enable/Value", "1", list3, list4);
			BulkMode.writeDefaultAndActualProd(parHandling, "/filter/acquisition/ConnectImages/Value", "1", list3, list4);
			BulkMode.writeDefaultAndActualProd(parHandling, "/filter/acquisition/MinLines/Value", "100", list3, list4);
			BulkMode.writeDefaultAndActualProd(parHandling, "/filter/acquisition/MaxLines/Value", "3000", list3, list4);
			BulkMode.writeDefaultAndActualProd(parHandling, "/filter/acquisition/SaveImage/Value", "1", list3, list4);
			BulkMode.writeDefaultAndActualProd(parHandling, "/filter/acquisition/SaveOnlyIfContaminant/Value", "0", list3, list4);
			BulkMode.writeDefaultAndActualProd(parHandling, "/filter/acquisition/SendImage/Value", "1", list3, list4);
			BulkMode.writeDefaultAndActualProd(parHandling, "/filter/acquisition/SendOnlyIfContaminant/Value", "0", list3, list4);
			BulkMode.writeDefaultAndActualProd(parHandling, "/filter/acquisition/ShowImage/Value", "1", list3, list4);
			BulkMode.writeDefaultAndActualProd(parHandling, "/filter/acquisition/ShowOnlyIfContaminant/Value", "0", list3, list4);
			BulkMode.writeDefaultAndActualProd(parHandling, "/filter/marker/PaintRectangles/Value", "1", list3, list4);
			BulkMode.writeDefaultAndActualProd(parHandling, "/filter/thresholdGlobal/Enabled/Value", "1", list3, list4);
			BulkMode.writeDefaultAndActualProd(parHandling, "/filter/threshold/Enabled/Value", "1", list3, list4);
			BulkMode.writeDefaultAndActualProd(parHandling, Definition.rejecterRejecterModePath, "0", list3, list4);
			BulkMode.writeDefaultAndActualProd(parHandling, Definition.rejecterNRejectsLimitPath, "10", list3, list4);
			BulkMode.writeDefaultAndActualProd(parHandling, Definition.rejecterRejectPeriodLimitPath, "300", list3, list4);
			for (int j = 1; j <= 8; j++)
			{
				BulkMode.writeDefaultAndActualProd(parHandling, "/filter/comparer/comparer" + j.ToString() + "/Enabled/Value", "0", list3, list4);
			}
			for (int k = 1; k <= Definition.nObjectFinder; k++)
			{
				BulkMode.writeDefaultAndActualProd(parHandling, Definition.filterMainPath + Definition.objectFindersSubPath + k.ToString() + "/Enabled/Value", "1", list3, list4);
			}
			for (int l = 1; l <= 8; l++)
			{
				list3.Add(Definition.filterMainPath + "/comparer/comparer" + l.ToString() + "/HighThreshold/Value");
				list4.Add("8500");
				BulkMode.writeDefaultAndActualProd(parHandling, Definition.filterMainPath + "/comparer/comparer" + l.ToString() + "/HighThreshold/Value", "65535", list3, list4);
			}
			BulkMode.writeDefaultAndActualProd(parHandling, Definition.filterMainPath + Definition.thresholdSubPath + "/HighThreshold/Value", "9000", list3, list4);
			BulkMode.writeDefaultAndActualProd(parHandling, Definition.filterMainPath + Definition.thresholdGlobalSubPath + "/HighThreshold/Value", "-1", list3, list4);
			dbConnection.UpdateTable(DBConnection.defaultApplication, list3, list4);
		}

		// Token: 0x060006F8 RID: 1784 RVA: 0x0002E826 File Offset: 0x0002CA26
		private static void writeDefaultAndActualProd(ParameterHandling parHandling, string path, string val, List<string> paths, List<string> values)
		{
			paths.Add(path);
			values.Add(val);
			parHandling.WriteValue(path, val);
		}

		// Token: 0x060006F9 RID: 1785 RVA: 0x0002E844 File Offset: 0x0002CA44
		public static void InitializeBulkObjectProduct(DBConnection dbConnection, ParameterHandling parHandling, string tableName)
		{
			int num = 1;
			dbConnection.UpdateTable(tableName, "/filter/cropping/Enable/Value", "1");
			dbConnection.UpdateTable(tableName, "/filter/acquisition/AcquisitionMode/Value", "1");
			dbConnection.UpdateTable(tableName, "/filter/acquisition/AddedFrontLines/Value", "10");
			dbConnection.UpdateTable(tableName, "/filter/acquisition/AddedBackLines/Value", "10");
			dbConnection.UpdateTable(tableName, "/filter/acquisition/AddedLeftPixObjectImg/Value", "10");
			dbConnection.UpdateTable(tableName, "/filter/acquisition/AddedRightPixObjectImg/Value", "10");
			dbConnection.UpdateTable(tableName, "/filter/acquisition/MinLines/Value", "100");
			dbConnection.UpdateTable(tableName, "/filter/acquisition/MaxLines/Value", "3000");
			dbConnection.UpdateTable(tableName, "/filter/acquisition/SaveImage/Value", "1");
			dbConnection.UpdateTable(tableName, "/filter/acquisition/SendImage/Value", "1");
			dbConnection.UpdateTable(tableName, "/filter/acquisition/ShowImage/Value", "1");
			dbConnection.UpdateTable(tableName, "/filter/marker/PaintRectangles/Value", "1");
			dbConnection.UpdateTable(tableName, "/filter/threshold/Enabled/Value", "1");
			dbConnection.GetFlapsCount(tableName, ref num);
			for (int i = 1; i <= num; i++)
			{
				string path = "/hardware/rejecter/rejecter" + i.ToString() + "/Mode/Value";
				dbConnection.UpdateTable(tableName, path, "1");
				path = "/hardware/rejecter/rejecter" + i.ToString() + "/Delay/Value";
				dbConnection.UpdateTable(tableName, path, "100");
				path = "/hardware/rejecter/rejecter" + i.ToString() + "/Length/Value";
				dbConnection.UpdateTable(tableName, path, "100");
			}
			dbConnection.UpdateTable(tableName, Definition.rejecterNRejectsLimitPath, "10");
			dbConnection.UpdateTable(tableName, Definition.rejecterRejectPeriodLimitPath, "300");
		}

		// Token: 0x060006FA RID: 1786 RVA: 0x0002E9F0 File Offset: 0x0002CBF0
		public static void ActivateAtComissioningSVT(DBConnection dbConnection, ParameterHandling parHandling)
		{
			string item = "1";
			string tableName = null;
			int num = 1;
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			List<string> list3 = new List<string>();
			List<string> list4 = new List<string>();
			string text = FlapsOverIOs.bulkflapsEnabledPath;
			dbConnection.GetCurrentProductTableName(ref tableName);
			int num2;
			int.TryParse(parHandling.ReadPar(Definition.imageVersionElinosPath), out num2);
			bool flag = num2 == 30;
			if (flag)
			{
				BulkMode.NuvoPCPerformance = true;
			}
			else
			{
				BulkMode.NuvoPCPerformance = false;
			}
			DetectorType detectorType = DetectorType.Unknown;
			dbConnection.GetDetectorType(ref detectorType);
			string a = "";
			bool flag2 = detectorType == DetectorType.Sesotec;
			if (flag2)
			{
				a = parHandling.ReadPar(Definition.detectorSesotecPixelSizePath);
			}
			int num3 = Definition.nComparerDefault;
			bool flag3 = BulkMode.NuvoPCPerformance && a == "0.4";
			if (flag3)
			{
				num3 = Definition.nComparerHXBulkNuvo;
			}
			dbConnection.CreateValveActivityEntries();
			dbConnection.SetIntegratedValidationProcess(false);
			dbConnection.UpdateTable(DBConnection.defaultApplication, text, "0");
			for (int i = 1; i <= num; i++)
			{
				text = "/hardware/rejecter/rejecter" + i.ToString() + "/Enabled/Value";
				parHandling.WriteValue(text, "0", false);
				dbConnection.UpdateTable(tableName, text, "0");
				list3.Add(text);
				list4.Add(item);
				text = "/hardware/rejecter/rejecter" + i.ToString() + "/Length/Value";
				parHandling.WriteValue(text, "0", false);
				dbConnection.UpdateTable(tableName, text, "0");
				list3.Add(text);
				list4.Add(item);
				text = "/hardware/svt/SoftwareDelay/Value";
				parHandling.WriteValue(text, "700", false);
				dbConnection.UpdateTable(tableName, text, "700");
				list3.Add(text);
				list4.Add("700");
				text = "/hardware/svt/HardwareLength/Value";
				parHandling.WriteValue(text, "127", false);
				dbConnection.UpdateTable(tableName, text, "127");
				list3.Add(text);
				list4.Add("127");
				text = "/hardware/svt/Enable/Value";
				parHandling.WriteValue(text, "1", false);
				dbConnection.UpdateTable(tableName, text, "1");
				list3.Add(text);
				list4.Add("1");
				text = "/hardware/svt/ValvesEnable/Value";
				parHandling.WriteValue(text, "0", false);
				dbConnection.UpdateTable(tableName, text, "0");
				list3.Add(text);
				list4.Add("0");
			}
			list.Add(FlapsOverIOs.bulkflapsBorderPix1Path);
			list.Add(FlapsOverIOs.bulkflapsBorderPix2Path);
			list.Add(FlapsOverIOs.bulkflapsBorderPix3Path);
			list2.Add("0");
			list2.Add("0");
			list2.Add("0");
			list.Add(Definition.isLiveCalibrationActivePath);
			list2.Add("2");
			dbConnection.UpdateCurrentProductTable(list, list2);
			parHandling.WriteValue(Definition.isLiveCalibrationActivePath, "2");
			BulkMode.writeDefaultAndActualProd(parHandling, "/filter/acquisition/ContinuousModeFiltering/Value", "0", list3, list4);
			BulkMode.writeDefaultAndActualProd(parHandling, "/filter/acquisition/ContinuousMode/Value", "0", list3, list4);
			BulkMode.writeDefaultAndActualProd(parHandling, "/filter/acquisition/AddedFrontLines/Value", "0", list3, list4);
			BulkMode.writeDefaultAndActualProd(parHandling, "/filter/acquisition/AddedBackLines/Value", "0", list3, list4);
			BulkMode.writeDefaultAndActualProd(parHandling, "/filter/cropping/Enable/Value", "0", list3, list4);
			BulkMode.writeDefaultAndActualProd(parHandling, "/filter/acquisition/ConnectImages/Value", "0", list3, list4);
			BulkMode.writeDefaultAndActualProd(parHandling, "/filter/acquisition/MinLines/Value", "100", list3, list4);
			BulkMode.writeDefaultAndActualProd(parHandling, "/filter/acquisition/MaxLines/Value", "100", list3, list4);
			BulkMode.writeDefaultAndActualProd(parHandling, "/filter/acquisition/SaveImage/Value", "0", list3, list4);
			BulkMode.writeDefaultAndActualProd(parHandling, "/filter/acquisition/SaveOnlyIfContaminant/Value", "0", list3, list4);
			BulkMode.writeDefaultAndActualProd(parHandling, "/filter/acquisition/SendImage/Value", "0", list3, list4);
			BulkMode.writeDefaultAndActualProd(parHandling, "/filter/acquisition/SendOnlyIfContaminant/Value", "0", list3, list4);
			BulkMode.writeDefaultAndActualProd(parHandling, "/filter/acquisition/ShowImage/Value", "0", list3, list4);
			BulkMode.writeDefaultAndActualProd(parHandling, "/filter/acquisition/ShowOnlyIfContaminant/Value", "0", list3, list4);
			BulkMode.writeDefaultAndActualProd(parHandling, "/filter/marker/PaintRectangles/Value", "0", list3, list4);
			BulkMode.writeDefaultAndActualProd(parHandling, "/filter/thresholdGlobal/Enabled/Value", "1", list3, list4);
			BulkMode.writeDefaultAndActualProd(parHandling, "/filter/threshold/Enabled/Value", "0", list3, list4);
			BulkMode.writeDefaultAndActualProd(parHandling, Definition.rejecterRejecterModePath, "1", list3, list4);
			BulkMode.writeDefaultAndActualProd(parHandling, Definition.rejecterNRejectsLimitPath, "20", list3, list4);
			BulkMode.writeDefaultAndActualProd(parHandling, Definition.rejecterRejectPeriodLimitPath, "60", list3, list4);
			for (int j = 1; j <= Definition.nComparerHXBulkNuvo; j++)
			{
				bool flag4 = j <= num3;
				if (flag4)
				{
					BulkMode.writeDefaultAndActualProd(parHandling, "/filter/comparer/comparer" + j.ToString() + "/Enabled/Value", "1", list3, list4);
				}
				else
				{
					BulkMode.writeDefaultAndActualProd(parHandling, "/filter/comparer/comparer" + j.ToString() + "/Enabled/Value", "0", list3, list4);
				}
			}
			for (int k = 1; k <= Definition.nObjectFinder; k++)
			{
				BulkMode.writeDefaultAndActualProd(parHandling, Definition.filterMainPath + Definition.objectFindersSubPath + k.ToString() + "/Enabled/Value", "0", list3, list4);
			}
			for (int l = 1; l <= Definition.nComparerHXBulkNuvo; l++)
			{
				list3.Add(Definition.filterMainPath + "/comparer/comparer" + l.ToString() + "/HighThreshold/Value");
				list4.Add("8500");
				BulkMode.writeDefaultAndActualProd(parHandling, Definition.filterMainPath + "/comparer/comparer" + l.ToString() + "/HighThreshold/Value", "8500", list3, list4);
			}
			BulkMode.writeDefaultAndActualProd(parHandling, Definition.filterMainPath + Definition.thresholdSubPath + "/HighThreshold/Value", "8500", list3, list4);
			BulkMode.writeDefaultAndActualProd(parHandling, Definition.filterMainPath + Definition.thresholdGlobalSubPath + "/HighThreshold/Value", "8500", list3, list4);
			dbConnection.UpdateTable(DBConnection.defaultApplication, list3, list4);
			BulkMode.SVTUpdateValues(dbConnection, parHandling);
		}

		// Token: 0x060006FB RID: 1787 RVA: 0x0002F048 File Offset: 0x0002D248
		public static int GetMaximumComparerRadius(ParameterHandling parHandling)
		{
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < Definition.nComparer; i++)
			{
				bool flag = parHandling.ReadPar(Definition.filterMainPath + "/comparer/comparer" + (i + 1).ToString() + "/Enabled/Value") == "1";
				if (flag)
				{
					int.TryParse(parHandling.ReadPar(Definition.filterMainPath + "/comparer/comparer" + (i + 1).ToString() + "/Radius/Value"), out num);
					bool flag2 = num > num2;
					if (flag2)
					{
						num2 = num;
					}
				}
			}
			return num2;
		}

		// Token: 0x060006FC RID: 1788 RVA: 0x0002F0EC File Offset: 0x0002D2EC
		public static void ActivateOverlappingImages(ParameterHandling parHandling)
		{
			for (int i = 0; i < Definition.nComparer; i++)
			{
				parHandling.WriteValue(Definition.filterMainPath + "/comparer/comparer" + (i + 1).ToString() + "/BulkOverlapImgLineOffset/Value", BulkMode.GetMaximumComparerRadius(parHandling).ToString());
				parHandling.WriteValue(Definition.filterMainPath + "/comparer/comparer" + (i + 1).ToString() + "/BulkOverlapImgEvalLines/Value", BulkMode.maxLinesBulk.ToString());
				parHandling.WriteValue(Definition.filterMainPath + "/comparer/comparer" + (i + 1).ToString() + "/BulkOverlapImgEval/Value", "1");
			}
			parHandling.WriteValue("/machine/BulkOverlapImgActivate/Value", "1");
		}

		// Token: 0x060006FD RID: 1789 RVA: 0x0002F1B8 File Offset: 0x0002D3B8
		public static void UpdateBulkOverlappingInspection(ParameterHandling parHandling)
		{
			int num = 0;
			parHandling.WriteValue("/machine/BulkOverlapImgEdgeLines/Value", BulkMode.GetMaximumComparerRadius(parHandling).ToString());
			int.TryParse(parHandling.ReadPar("/filter/acquisition/MaxLines/Value"), out num);
			BulkMode.maxLinesBulk = num;
			BulkMode.ActivateOverlappingImages(parHandling);
		}

		// Token: 0x060006FE RID: 1790 RVA: 0x0002F204 File Offset: 0x0002D404
		private static void SVTUpdateValues(DBConnection dbConnection, ParameterHandling parHandling)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			int num7 = 0;
			int num8 = 0;
			bool flag = false;
			List<int> list = new List<int>();
			List<int> list2 = new List<int>();
			double num9 = 0.0;
			int num10 = 0;
			string text = parHandling.ReadPar(Definition.detectorTypePath);
			dbConnection.GetIgnoredRightPixel(ref num2);
			dbConnection.GetIgnoredLeftPixel(ref num);
			dbConnection.GetFirstValve(ref num6);
			dbConnection.GetNumValves(ref num5);
			dbConnection.GetSVTEndPixelNextToLastValve(ref num3);
			dbConnection.GetSVTStartPixelSecondValve(ref num4);
			dbConnection.GetValveOverlappingLeft(ref num7);
			dbConnection.GetValveOverlappingRight(ref num8);
			dbConnection.GetValvesInverted(ref flag);
			int num11 = 0;
			bool flag2 = text.Equals("1");
			if (flag2)
			{
				int.TryParse(parHandling.ReadPar(Definition.detectorSesotecNPixelPath), out num11);
			}
			else
			{
				int.TryParse(parHandling.ReadPar(Definition.detectorDTNPixelPath), out num11);
			}
			double num12 = Math.Round((double)(num3 - num4) / ((double)num5 - 2.0), 2);
			int num13 = (int)num12 - (num4 - num);
			int num14 = num11 - num2;
			int num15 = (int)num12;
			for (int i = 1; i <= num5; i++)
			{
				bool flag3 = i < num6;
				int num16;
				if (flag3)
				{
					num16 = -1;
					num10 = -1;
				}
				else
				{
					bool flag4 = i == num6;
					if (flag4)
					{
						num16 = 0;
						num10 = num15 - num13 - 1 + num8;
					}
					else
					{
						num16 = num10 - num7 - num8;
						num10 += num15;
					}
					bool flag5 = num16 < 0;
					if (flag5)
					{
						num16 = -1;
					}
					bool flag6 = num10 < 0;
					if (flag6)
					{
						num10 = -1;
					}
					num9 += num12 - (double)num15;
					bool flag7 = num9 >= 0.5 || i == num6;
					if (flag7)
					{
						num10++;
						num9 -= 1.0;
					}
				}
				list.Add(num16);
				list2.Add(num10);
				bool flag8 = flag;
				string str;
				if (flag8)
				{
					str = Definition.SVTValvesAssignmentMainPath + (num5 - i + 1).ToString("D2");
				}
				else
				{
					str = Definition.SVTValvesAssignmentMainPath + i.ToString("D2");
				}
				parHandling.WriteValue(str + "/MinPixel/Value", num16.ToString(), false);
				parHandling.WriteValue(str + "/MaxPixel/Value", num10.ToString(), false);
			}
			for (int j = num5 + 1; j <= 80; j++)
			{
				bool flag9 = flag;
				string str;
				if (flag9)
				{
					str = Definition.SVTValvesAssignmentMainPath + (num5 - j + 1).ToString("D2");
				}
				else
				{
					str = Definition.SVTValvesAssignmentMainPath + j.ToString("D2");
				}
				parHandling.WriteValue(str + "/MinPixel/Value", "-1", false);
				parHandling.WriteValue(str + "/MaxPixel/Value", "-1", false);
				list.Add(-1);
				list2.Add(-1);
			}
			dbConnection.SetSVTMinPixels(0, list.ToArray(), flag);
			dbConnection.SetSVTMaxPixels(0, list2.ToArray(), flag);
			dbConnection.SetValveWidth(num12);
		}

		// Token: 0x040004CF RID: 1231
		public static readonly string svtTestEndValvePath = "/hardware/svt/testing/MaxValve/Value";

		// Token: 0x040004D0 RID: 1232
		public static int linesForAutotrain = 1000;

		// Token: 0x040004D1 RID: 1233
		public static bool NuvoPCPerformance;

		// Token: 0x040004D2 RID: 1234
		public static int maxLinesBulk;
	}
}
