using System;
using System.Collections.Generic;
using System.Threading;
using Common;
using DatabaseConnection;
using Definitions;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x020000A0 RID: 160
	public class DualEnergy : ViewModelBase
	{
		// Token: 0x170004C1 RID: 1217
		// (get) Token: 0x06000FD0 RID: 4048 RVA: 0x00062430 File Offset: 0x00060630
		// (set) Token: 0x06000FD1 RID: 4049 RVA: 0x00062448 File Offset: 0x00060648
		public bool IsBusy
		{
			get
			{
				return this.isBusy;
			}
			set
			{
				this.SetProperty<bool>(ref this.isBusy, value, "IsBusy");
			}
		}

		// Token: 0x06000FD2 RID: 4050 RVA: 0x0006245E File Offset: 0x0006065E
		public DualEnergy(ImageDataTransfer imageTransfer, ParameterHandling parHandling)
		{
			this.imageTransfer = imageTransfer;
			this.parHandling = parHandling;
		}

		// Token: 0x06000FD3 RID: 4051 RVA: 0x00062478 File Offset: 0x00060678
		public void SendMaterialCurveImage(List<MaterialCurveElement> materialCurveDB, int group)
		{
			XRSImage xrsimage = new XRSImage();
			int num = 100;
			int num2 = 0;
			xrsimage.Width = (short)(3 * num);
			xrsimage.Height = (short)(DBMaterialCurve.sizeMaterialCurveImagePC / num);
			xrsimage.Data = new short[(int)(xrsimage.Width * xrsimage.Height)];
			xrsimage.Mask = new byte[(int)(xrsimage.Width * xrsimage.Height)];
			xrsimage.Type = ImageType.MaterialCurve;
			xrsimage.Group = Convert.ToInt16(group);
			bool flag = materialCurveDB != null && materialCurveDB.Count == DBMaterialCurve.sizeMaterialCurveDB;
			if (flag)
			{
				int i = 0;
				while (i < DBMaterialCurve.sizeMaterialCurveImagePC)
				{
					bool flag2 = i < DualEnergy.lowLimitMaterialCurveDB;
					if (flag2)
					{
						xrsimage.Data[num2] = Convert.ToInt16(i);
						xrsimage.Data[num2 + 1] = short.MaxValue;
						xrsimage.Data[num2 + 2] = 0;
					}
					else
					{
						bool flag3 = i > DualEnergy.highLimitMaterialCurveDB - 1;
						if (flag3)
						{
							xrsimage.Data[num2] = Convert.ToInt16(i);
							xrsimage.Data[num2 + 1] = (short)materialCurveDB[DualEnergy.highLimitMaterialCurveDB - DualEnergy.lowLimitMaterialCurveDB - 1].MinHigh;
							xrsimage.Data[num2 + 2] = (short)materialCurveDB[DualEnergy.highLimitMaterialCurveDB - DualEnergy.lowLimitMaterialCurveDB - 1].MaxHigh;
						}
						else
						{
							xrsimage.Data[num2] = (short)materialCurveDB[i - DualEnergy.lowLimitMaterialCurveDB].Low;
							xrsimage.Data[num2 + 1] = (short)materialCurveDB[i - DualEnergy.lowLimitMaterialCurveDB].MinHigh;
							xrsimage.Data[num2 + 2] = (short)materialCurveDB[i - DualEnergy.lowLimitMaterialCurveDB].MaxHigh;
						}
					}
					i++;
					num2 += 3;
				}
			}
			else
			{
				int j = 0;
				while (j < DBMaterialCurve.sizeMaterialCurveImagePC)
				{
					xrsimage.Data[num2] = Convert.ToInt16(j);
					xrsimage.Data[num2 + 1] = 0;
					xrsimage.Data[num2 + 2] = 0;
					j++;
					num2 += 3;
				}
			}
			this.imageTransfer.SendImage(xrsimage);
		}

		// Token: 0x06000FD4 RID: 4052 RVA: 0x00062690 File Offset: 0x00060890
		private void CheckIfBusy()
		{
			this.checkIfBusyRunning = true;
			while (this.checkIfBusy)
			{
				string a = this.parHandling.ReadPar(Definition.filterMainPath + DualEnergy.busySubPath);
				this.IsBusy = (a == "1");
				Thread.Sleep(700);
			}
			this.checkIfBusyRunning = false;
		}

		// Token: 0x06000FD5 RID: 4053 RVA: 0x000626F8 File Offset: 0x000608F8
		public void StartBusyCheck()
		{
			bool flag = !this.checkIfBusyRunning && !this.checkIfBusy;
			if (flag)
			{
				this.checkIfBusy = true;
				new Thread(new ThreadStart(this.CheckIfBusy))
				{
					Name = "CheckIfBusy"
				}.Start();
			}
		}

		// Token: 0x06000FD6 RID: 4054 RVA: 0x0006274C File Offset: 0x0006094C
		public void StopBusyCheck()
		{
			this.checkIfBusy = false;
		}

		// Token: 0x04000A9B RID: 2715
		public static readonly string dualSubPath = "/dual";

		// Token: 0x04000A9C RID: 2716
		public static readonly string pathDebug = DualEnergy.dualSubPath + "/Debug/Value";

		// Token: 0x04000A9D RID: 2717
		public static readonly string pathToleranceMax = DualEnergy.dualSubPath + "/ToleranceMax/Value";

		// Token: 0x04000A9E RID: 2718
		public static readonly string pathToleranceMin = DualEnergy.dualSubPath + "/ToleranceMin/Value";

		// Token: 0x04000A9F RID: 2719
		public static readonly string pathLowGain = "/hardware/detector/LowGain/Value";

		// Token: 0x04000AA0 RID: 2720
		public static readonly string pathHighGain = "/hardware/detector/HighGain/Value";

		// Token: 0x04000AA1 RID: 2721
		public static readonly string pathMaxYValue = DualEnergy.dualSubPath + "/MaxY/Value";

		// Token: 0x04000AA2 RID: 2722
		public static readonly string pathSetHighforSingleEval = DualEnergy.dualSubPath + "/HighforSingleEval/Value";

		// Token: 0x04000AA3 RID: 2723
		public static readonly string enableSubPath = DualEnergy.dualSubPath + "/Enabled/Value";

		// Token: 0x04000AA4 RID: 2724
		public static readonly string materialCurveIndexSubPath = DualEnergy.dualSubPath + "/MaterialCurveIndex/Value";

		// Token: 0x04000AA5 RID: 2725
		public static readonly string materialCurveMinSubPath = DualEnergy.dualSubPath + "/MaterialCurveMin/Value";

		// Token: 0x04000AA6 RID: 2726
		public static readonly string materialCurveMaxSubPath = DualEnergy.dualSubPath + "/MaterialCurveMax/Value";

		// Token: 0x04000AA7 RID: 2727
		public static readonly string requestCurveSubPath = DualEnergy.dualSubPath + "/RequestCurve/Value";

		// Token: 0x04000AA8 RID: 2728
		public static readonly string busySubPath = DualEnergy.dualSubPath + "/Busy/Value";

		// Token: 0x04000AA9 RID: 2729
		public static readonly int lowLimitMaterialCurveDB = 4000;

		// Token: 0x04000AAA RID: 2730
		public static readonly int highLimitMaterialCurveDB = 9300;

		// Token: 0x04000AAB RID: 2731
		private ImageDataTransfer imageTransfer;

		// Token: 0x04000AAC RID: 2732
		private ParameterHandling parHandling;

		// Token: 0x04000AAD RID: 2733
		private bool checkIfBusy;

		// Token: 0x04000AAE RID: 2734
		private bool checkIfBusyRunning;

		// Token: 0x04000AAF RID: 2735
		private bool isBusy;
	}
}
