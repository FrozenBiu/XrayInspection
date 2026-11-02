using System;
using System.Threading;
using DatabaseConnection;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x0200015A RID: 346
	public class Sideshooter
	{
		// Token: 0x060020E3 RID: 8419 RVA: 0x000D65F8 File Offset: 0x000D47F8
		public static void SetXRayPositioningParameters(DBConnection dbConnection, ParameterHandling parHandling, bool enableXray)
		{
			DetectorType detectorType = DetectorType.DT;
			dbConnection.GetDetectorType(ref detectorType);
			string path = (detectorType == DetectorType.DT || detectorType == DetectorType.DTNetSE || detectorType == DetectorType.DTNetDE || detectorType == DetectorType.DTNetTDI) ? Definition.detectorDTScanratePath : Definition.detectorSesotecScanratePath;
			Sideshooter.oldScanrate = parHandling.ReadPar(path);
			parHandling.WriteValue(Acquisition.lane1EnablePath, "1", false);
			parHandling.WriteValue(Acquisition.lane2EnablePath, "0", false);
			parHandling.WriteValue(Acquisition.lane3EnablePath, "0", false);
			parHandling.WriteValue(Acquisition.lane4EnablePath, "0", false);
			parHandling.WriteValue(Acquisition.lane1MinPixelPath, "0", false);
			parHandling.WriteValue(Acquisition.lane1MaxPixelPath, 10239.ToString(), false);
			parHandling.WriteValue("/filter/acquisition/AddedFrontLines/Value", "0", false);
			parHandling.WriteValue("/filter/acquisition/AddedBackLines/Value", "0", false);
			parHandling.WriteValue("/filter/acquisition/SendImage/Value", "0", false);
			parHandling.WriteValue("/filter/acquisition/ConnectImages/Value", "0", false);
			parHandling.WriteValue("/filter/acquisition/ContinuousModeFiltering/Value", "0", false);
			parHandling.WriteValue("/filter/acquisition/ContinuousMode/Value", "1", false);
			parHandling.WriteValue("/filter/acquisition/MaxLines/Value", "10", false);
			parHandling.WriteValue("/filter/acquisition/MinLines/Value", "10", false);
			parHandling.WriteValue("/filter/acquisition/Binning/Value", "1", false);
			parHandling.WriteValue("/filter/acquisition/BinPixel/Value", "1", false);
			parHandling.WriteValue(XRayOnProcessing.xrayVoltagePath, "40", false);
			parHandling.WriteValue(XRayOnProcessing.xrayCurrentPath, "1.0", false);
			parHandling.WriteValue(path, "375", false);
			if (enableXray)
			{
				parHandling.WriteValue(XRayOnProcessing.xrayEnablePath, "1", false);
				Thread.Sleep(2000);
			}
			parHandling.WriteValue(XRayOnProcessing.manualGainCalibrationPath, "0", false);
			parHandling.WriteValue(XRayOnProcessing.manualOffsetCalibrationPath, "0", false);
		}

		// Token: 0x060020E4 RID: 8420 RVA: 0x000D67E4 File Offset: 0x000D49E4
		public static void ResetXRayPositioningParameters(DBConnection dbConnection, ParameterHandling parHandling, bool readonlyMode)
		{
			string value = "";
			DetectorType detectorType = DetectorType.DT;
			dbConnection.GetDetectorType(ref detectorType);
			string path = (detectorType == DetectorType.DT || detectorType == DetectorType.DTNetSE || detectorType == DetectorType.DTNetDE || detectorType == DetectorType.DTNetTDI) ? Definition.detectorDTScanratePath : Definition.detectorSesotecScanratePath;
			dbConnection.GetParameterValue(Acquisition.lane1EnablePath, ref value);
			parHandling.WriteValue(Acquisition.lane1EnablePath, value, false);
			dbConnection.GetParameterValue(Acquisition.lane2EnablePath, ref value);
			parHandling.WriteValue(Acquisition.lane2EnablePath, value, false);
			dbConnection.GetParameterValue(Acquisition.lane3EnablePath, ref value);
			parHandling.WriteValue(Acquisition.lane3EnablePath, value, false);
			dbConnection.GetParameterValue(Acquisition.lane4EnablePath, ref value);
			parHandling.WriteValue(Acquisition.lane4EnablePath, value, false);
			dbConnection.GetParameterValue(Acquisition.lane1MinPixelPath, ref value);
			parHandling.WriteValue(Acquisition.lane1MinPixelPath, value, false);
			dbConnection.GetParameterValue(Acquisition.lane1MaxPixelPath, ref value);
			parHandling.WriteValue(Acquisition.lane1MaxPixelPath, value, false);
			dbConnection.GetParameterValue("/hardware/xraysource/VoltageSetpoint/Value", ref value);
			parHandling.WriteValue(XRayOnProcessing.xrayVoltagePath, value, false);
			dbConnection.GetParameterValue("/hardware/xraysource/CurrentSetpoint/Value", ref value);
			parHandling.WriteValue(XRayOnProcessing.xrayCurrentPath, value, false);
			parHandling.WriteValue(path, Sideshooter.oldScanrate, false);
			dbConnection.GetParameterValue("/filter/acquisition/BinPixel/Value", ref value);
			parHandling.WriteValue("/filter/acquisition/BinPixel/Value", value, false);
			dbConnection.GetParameterValue("/filter/acquisition/Binning/Value", ref value);
			parHandling.WriteValue("/filter/acquisition/Binning/Value", value, false);
			dbConnection.GetParameterValue("/filter/acquisition/MinLines/Value", ref value);
			parHandling.WriteValue("/filter/acquisition/MinLines/Value", value, false);
			dbConnection.GetParameterValue("/filter/acquisition/MaxLines/Value", ref value);
			parHandling.WriteValue("/filter/acquisition/MaxLines/Value", value, false);
			dbConnection.GetParameterValue("/filter/acquisition/ContinuousMode/Value", ref value);
			parHandling.WriteValue("/filter/acquisition/ContinuousMode/Value", value, false);
			Thread.Sleep(300);
			dbConnection.GetParameterValue("/filter/acquisition/ContinuousModeFiltering/Value", ref value);
			parHandling.WriteValue("/filter/acquisition/ContinuousModeFiltering/Value", value, false);
			dbConnection.GetParameterValue("/filter/acquisition/ConnectImages/Value", ref value);
			parHandling.WriteValue("/filter/acquisition/ConnectImages/Value", value, false);
			dbConnection.GetParameterValue("/filter/acquisition/SendImage/Value", ref value);
			parHandling.WriteValue("/filter/acquisition/SendImage/Value", value, false);
			dbConnection.GetParameterValue("/filter/acquisition/AddedBackLines/Value", ref value);
			parHandling.WriteValue("/filter/acquisition/AddedBackLines/Value", value, false);
			dbConnection.GetParameterValue("/filter/acquisition/AddedFrontLines/Value", ref value);
			parHandling.WriteValue("/filter/acquisition/AddedFrontLines/Value", value, false);
			bool flag = !readonlyMode;
			if (flag)
			{
				parHandling.WriteValue(XRayOnProcessing.manualOffsetCalibrationPath, "1", false);
				Thread.Sleep(1000);
				parHandling.WriteValue(XRayOnProcessing.manualGainCalibrationPath, "1", false);
			}
		}

		// Token: 0x04001550 RID: 5456
		private static string oldScanrate = "400";
	}
}
