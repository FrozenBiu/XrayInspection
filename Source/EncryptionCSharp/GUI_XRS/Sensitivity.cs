using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using DatabaseConnection;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x02000127 RID: 295
	public class Sensitivity : INotifyPropertyChanged
	{
		// Token: 0x14000090 RID: 144
		// (add) Token: 0x06001C8F RID: 7311 RVA: 0x000B690C File Offset: 0x000B4B0C
		// (remove) Token: 0x06001C90 RID: 7312 RVA: 0x000B6944 File Offset: 0x000B4B44
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x17000869 RID: 2153
		// (get) Token: 0x06001C91 RID: 7313 RVA: 0x000B697C File Offset: 0x000B4B7C
		// (set) Token: 0x06001C92 RID: 7314 RVA: 0x000B6994 File Offset: 0x000B4B94
		public int SensitivityPercent
		{
			get
			{
				return this.sensitivityPercent;
			}
			set
			{
				bool flag = this.parHandling != null && this.sensitivityPercent != value;
				if (flag)
				{
					int num = this.sensitivityPercent;
					this.sensitivityPercent = value;
					bool flag2 = this.calculateBulkSliderContrasts;
					if (flag2)
					{
						List<string> list = new List<string>();
						double num2 = 0.0;
						double num3 = 0.0;
						int num4 = 0;
						DetectorType detectorType = DetectorType.Unknown;
						this.dbConnection.GetDetectorType(ref detectorType);
						string a = "";
						bool flag3 = detectorType == DetectorType.Sesotec;
						if (flag3)
						{
							a = this.parHandling.ReadPar(Definition.detectorSesotecPixelSizePath);
						}
						int num5 = Definition.nComparerDefault;
						bool flag4 = BulkMode.NuvoPCPerformance && a == "0.4";
						if (flag4)
						{
							num5 = Definition.nComparerHXBulkNuvo;
						}
						bool flag5 = num > 0;
						if (flag5)
						{
							num2 = Convert.ToDouble(200 - this.sensitivityPercent) / Convert.ToDouble(200 - num);
							num3 = Convert.ToDouble(this.sensitivityPercent) / Convert.ToDouble(num);
						}
						for (int i = 1; i <= num5; i++)
						{
							list.Add(this.parHandling.ReadPar(Definition.filterMainPath + "/comparer/comparer" + i.ToString() + "/Contrast/Value"));
						}
						list.Add(this.parHandling.ReadPar(Definition.filterMainPath + Definition.thresholdGlobalSubPath + "/HighThreshold/Value"));
						list.Add(this.parHandling.ReadPar(Definition.filterMainPath + Definition.thresholdSubPath + "/HighThreshold/Value"));
						int num6;
						for (int j = 1; j <= num5; j++)
						{
							int.TryParse(list[j - 1], out num4);
							bool flag6 = num4 < Definition.Background10000;
							if (flag6)
							{
								num6 = (int)Math.Round(num2 * double.Parse(list[j - 1]), 0);
								bool flag7 = num6 >= Definition.Background10000;
								if (flag7)
								{
									num6 = Definition.Background10000 - 1;
								}
								this.parHandling.WriteValue(Definition.filterMainPath + "/comparer/comparer" + j.ToString() + "/Contrast/Value", Convert.ToString(num6));
							}
						}
						num6 = (int)Math.Round(num3 * double.Parse(list[num5]), 0);
						this.parHandling.WriteValue(Definition.filterMainPath + Definition.thresholdGlobalSubPath + "/HighThreshold/Value", Convert.ToString(num6));
						num6 = (int)Math.Round(num3 * double.Parse(list[num5 + 1]), 0);
						this.parHandling.WriteValue(Definition.filterMainPath + Definition.thresholdSubPath + "/HighThreshold/Value", Convert.ToString(num6));
					}
					this.parHandling.WriteValue(FlapsOverIOs.bulkflapsManualSensitvity, Convert.ToString(this.sensitivityPercent));
					this.OnPropertyChanged("SensitivityPercent");
				}
			}
		}

		// Token: 0x06001C93 RID: 7315 RVA: 0x000B6C90 File Offset: 0x000B4E90
		public Sensitivity(ParameterHandling parHandling, DBConnection dbConnection, ProductManager productManager)
		{
			this.parHandling = parHandling;
			this.dbConnection = dbConnection;
			this.productManager = productManager;
			string s = null;
			bool flag = dbConnection.Connection != null;
			if (flag)
			{
				dbConnection.GetParameterValue(FlapsOverIOs.bulkflapsManualSensitvity, ref s);
				try
				{
					bool flag2 = this.initialized;
					if (flag2)
					{
						this.initialized = false;
						this.calculateBulkSliderContrasts = false;
					}
					this.SensitivityPercent = int.Parse(s);
				}
				catch
				{
				}
			}
			bool flag3 = productManager != null;
			if (flag3)
			{
				productManager.ProductChanged += this.productManager_ProductChanged;
			}
		}

		// Token: 0x06001C94 RID: 7316 RVA: 0x000B6D44 File Offset: 0x000B4F44
		public void SetProductManager(ProductManager productManager)
		{
			bool flag = this.productManager != null && productManager != null;
			if (flag)
			{
				this.productManager.ProductChanged -= this.productManager_ProductChanged;
			}
			bool flag2 = productManager != null;
			if (flag2)
			{
				this.productManager = productManager;
				this.productManager.ProductChanged += this.productManager_ProductChanged;
				this.UpdateSensitivityLocal();
			}
		}

		// Token: 0x06001C95 RID: 7317 RVA: 0x000B6DAD File Offset: 0x000B4FAD
		private void productManager_ProductChanged(ProductData arg1, ProductData arg2)
		{
			this.UpdateSensitivityLocal();
		}

		// Token: 0x06001C96 RID: 7318 RVA: 0x000B6DAD File Offset: 0x000B4FAD
		private void dbConnection_ProductParametersChanged(string arg1, List<string> arg2, List<string> arg3)
		{
			this.UpdateSensitivityLocal();
		}

		// Token: 0x06001C97 RID: 7319 RVA: 0x000B6DB8 File Offset: 0x000B4FB8
		private void UpdateSensitivityLocal()
		{
			string s = "";
			this.dbConnection.GetParameterValue(FlapsOverIOs.bulkflapsManualSensitvity, ref s);
			bool flag = int.TryParse(s, out this.sensitivityPercent);
			if (flag)
			{
				this.OnPropertyChanged("SensitivityPercent");
			}
		}

		// Token: 0x06001C98 RID: 7320 RVA: 0x000B6E00 File Offset: 0x000B5000
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x06001C99 RID: 7321 RVA: 0x000B6E2D File Offset: 0x000B502D
		public void Close()
		{
			this.dbConnection.ProductParametersChanged -= this.dbConnection_ProductParametersChanged;
			this.productManager.ProductChanged -= this.productManager_ProductChanged;
		}

		// Token: 0x040012DB RID: 4827
		public static readonly string highThresholdPath = "/filter/threshold/HighThreshold/Value";

		// Token: 0x040012DC RID: 4828
		private ParameterHandling parHandling;

		// Token: 0x040012DD RID: 4829
		private DBConnection dbConnection;

		// Token: 0x040012DE RID: 4830
		private ProductManager productManager;

		// Token: 0x040012DF RID: 4831
		private int sensitivityPercent;

		// Token: 0x040012E0 RID: 4832
		private bool initialized = true;

		// Token: 0x040012E1 RID: 4833
		public bool calculateBulkSliderContrasts = true;
	}
}
