using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x02000011 RID: 17
	public class AIObjectClass : INotifyPropertyChanged
	{
		// Token: 0x1400000A RID: 10
		// (add) Token: 0x06000112 RID: 274 RVA: 0x00006FF4 File Offset: 0x000051F4
		// (remove) Token: 0x06000113 RID: 275 RVA: 0x0000702C File Offset: 0x0000522C
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000114 RID: 276 RVA: 0x00007061 File Offset: 0x00005261
		public bool IsCountType
		{
			get
			{
				return this.DetectionType == "Count";
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000115 RID: 277 RVA: 0x00007073 File Offset: 0x00005273
		public bool IsThresholding
		{
			get
			{
				return this.UnmaskForConMethod != "Rectangle" && this.IsCountType;
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000116 RID: 278 RVA: 0x00007090 File Offset: 0x00005290
		public bool IsMinMaxAdaptiveThresholding
		{
			get
			{
				return this.UnmaskForConMethod == "MinMax" && this.IsCountType;
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000117 RID: 279 RVA: 0x000070AD File Offset: 0x000052AD
		public bool IsPercentileThresholding
		{
			get
			{
				return this.UnmaskForConMethod == "Percentile" && this.IsCountType;
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000118 RID: 280 RVA: 0x000070CA File Offset: 0x000052CA
		// (set) Token: 0x06000119 RID: 281 RVA: 0x000070D2 File Offset: 0x000052D2
		public BoundLocalizedText StrClassEnabled { get; set; }

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x0600011A RID: 282 RVA: 0x000070DB File Offset: 0x000052DB
		// (set) Token: 0x0600011B RID: 283 RVA: 0x000070E3 File Offset: 0x000052E3
		public BoundLocalizedText StrDetectionType { get; set; }

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x0600011C RID: 284 RVA: 0x000070EC File Offset: 0x000052EC
		// (set) Token: 0x0600011D RID: 285 RVA: 0x000070F4 File Offset: 0x000052F4
		public BoundLocalizedText StrGroupId { get; set; }

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x0600011E RID: 286 RVA: 0x000070FD File Offset: 0x000052FD
		// (set) Token: 0x0600011F RID: 287 RVA: 0x00007105 File Offset: 0x00005305
		public BoundLocalizedText StrMinCount { get; set; }

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000120 RID: 288 RVA: 0x0000710E File Offset: 0x0000530E
		// (set) Token: 0x06000121 RID: 289 RVA: 0x00007116 File Offset: 0x00005316
		public BoundLocalizedText StrMaxCount { get; set; }

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000122 RID: 290 RVA: 0x0000711F File Offset: 0x0000531F
		// (set) Token: 0x06000123 RID: 291 RVA: 0x00007127 File Offset: 0x00005327
		public BoundLocalizedText StrConfThreshold { get; set; }

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000124 RID: 292 RVA: 0x00007130 File Offset: 0x00005330
		// (set) Token: 0x06000125 RID: 293 RVA: 0x00007138 File Offset: 0x00005338
		public BoundLocalizedText StrUnmaskForConEnabled { get; set; }

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000126 RID: 294 RVA: 0x00007141 File Offset: 0x00005341
		// (set) Token: 0x06000127 RID: 295 RVA: 0x00007149 File Offset: 0x00005349
		public BoundLocalizedText StrUnmaskForConMethod { get; set; }

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x06000128 RID: 296 RVA: 0x00007152 File Offset: 0x00005352
		// (set) Token: 0x06000129 RID: 297 RVA: 0x0000715A File Offset: 0x0000535A
		public BoundLocalizedText StrFilterSize { get; set; }

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x0600012A RID: 298 RVA: 0x00007163 File Offset: 0x00005363
		// (set) Token: 0x0600012B RID: 299 RVA: 0x0000716B File Offset: 0x0000536B
		public BoundLocalizedText StrMinGroupSize { get; set; }

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x0600012C RID: 300 RVA: 0x00007174 File Offset: 0x00005374
		// (set) Token: 0x0600012D RID: 301 RVA: 0x0000717C File Offset: 0x0000537C
		public BoundLocalizedText StrClassName { get; set; }

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x0600012E RID: 302 RVA: 0x00007185 File Offset: 0x00005385
		// (set) Token: 0x0600012F RID: 303 RVA: 0x0000718D File Offset: 0x0000538D
		public BoundLocalizedText StrConfThresholdTTA1 { get; set; }

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000130 RID: 304 RVA: 0x00007196 File Offset: 0x00005396
		// (set) Token: 0x06000131 RID: 305 RVA: 0x0000719E File Offset: 0x0000539E
		public BoundLocalizedText StrConfThresholdTTA2 { get; set; }

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000132 RID: 306 RVA: 0x000071A7 File Offset: 0x000053A7
		// (set) Token: 0x06000133 RID: 307 RVA: 0x000071AF File Offset: 0x000053AF
		public BoundLocalizedText StrConfThresholdTTA3 { get; set; }

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000134 RID: 308 RVA: 0x000071B8 File Offset: 0x000053B8
		// (set) Token: 0x06000135 RID: 309 RVA: 0x000071C0 File Offset: 0x000053C0
		public BoundLocalizedText StrTTAEnabled { get; set; }

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000136 RID: 310 RVA: 0x000071C9 File Offset: 0x000053C9
		// (set) Token: 0x06000137 RID: 311 RVA: 0x000071D1 File Offset: 0x000053D1
		public BoundLocalizedText StrLowThresholdMinMax { get; set; }

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000138 RID: 312 RVA: 0x000071DA File Offset: 0x000053DA
		// (set) Token: 0x06000139 RID: 313 RVA: 0x000071E2 File Offset: 0x000053E2
		public BoundLocalizedText StrHighThresholdMinMax { get; set; }

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x0600013A RID: 314 RVA: 0x000071EB File Offset: 0x000053EB
		// (set) Token: 0x0600013B RID: 315 RVA: 0x000071F3 File Offset: 0x000053F3
		public BoundLocalizedText StrPercentileThreshold { get; set; }

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x0600013C RID: 316 RVA: 0x000071FC File Offset: 0x000053FC
		// (set) Token: 0x0600013D RID: 317 RVA: 0x00007204 File Offset: 0x00005404
		public string TabName
		{
			get
			{
				return this.tabName;
			}
			set
			{
				bool flag = value != this.tabName;
				if (flag)
				{
					this.tabName = value;
					this.OnPropertyChanged("TabName");
				}
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x0600013E RID: 318 RVA: 0x00007237 File Offset: 0x00005437
		// (set) Token: 0x0600013F RID: 319 RVA: 0x00007240 File Offset: 0x00005440
		public string ClassName
		{
			get
			{
				return this.className;
			}
			set
			{
				bool flag = value != this.className;
				if (flag)
				{
					this.className = value;
					this.OnPropertyChanged("ClassName");
				}
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000140 RID: 320 RVA: 0x00007273 File Offset: 0x00005473
		// (set) Token: 0x06000141 RID: 321 RVA: 0x0000727C File Offset: 0x0000547C
		public bool ClassEnabled
		{
			get
			{
				return this.classEnabled;
			}
			set
			{
				bool flag = value != this.classEnabled;
				if (flag)
				{
					this.classEnabled = value;
					this.parHandling.WriteValue(this.pathClassEnabled, value ? "1" : "0");
					LogbookManager.AddEntry(SSTEntryCode.ObjectDetectionParameterChanged, 1, this.productManager.CurrentLoadedProduct.ProductNumber, this.productManager.CurrentLoadedProduct.ProductName, this.classIndex.ToString(), (!value) ? "1" : "0", value ? "1" : "0");
					this.OnPropertyChanged("ClassEnabled");
					bool flag2 = value && this.parHandling.ReadPar(Definition.objectDetectionInferenceEnabled) == "0";
					if (flag2)
					{
						bool flag3 = Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("msgThinkXOEnableInference"), DialogStyle.Info, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
						bool flag4 = flag3;
						if (flag4)
						{
							this.parHandling.WriteValue(Definition.objectDetectionInferenceEnabled, "1");
							LogbookManager.AddEntry(SSTEntryCode.AIDetectionActivated, 3, this.productManager.CurrentLoadedProduct.ProductNumber, this.productManager.CurrentLoadedProduct.ProductName);
						}
					}
				}
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000142 RID: 322 RVA: 0x000073B9 File Offset: 0x000055B9
		// (set) Token: 0x06000143 RID: 323 RVA: 0x000073C4 File Offset: 0x000055C4
		public string DetectionType
		{
			get
			{
				return this.detectionType;
			}
			set
			{
				bool flag = value != this.detectionType;
				if (flag)
				{
					LogbookManager.AddEntry(SSTEntryCode.ObjectDetectionParameterChanged, 2, this.productManager.CurrentLoadedProduct.ProductNumber, this.productManager.CurrentLoadedProduct.ProductName, this.classIndex.ToString(), this.detectionType, value);
					this.detectionType = value;
					int num = this.DetectionTypeOptions.IndexOf(this.detectionType);
					this.parHandling.WriteValue(this.pathDetectionType, Convert.ToString(num));
					this.OnPropertyChanged("DetectionType");
					this.OnPropertyChanged("IsCountType");
					this.OnPropertyChanged("IsThresholding");
					this.OnPropertyChanged("IsMinMaxAdaptiveThresholding");
					this.OnPropertyChanged("IsPercentileThresholding");
					bool flag2 = num != 0;
					if (flag2)
					{
						this.GroupId = Convert.ToInt32(this.parHandling.ReadPar(this.pathGroupId));
						this.UnmaskForConEnabled = (this.parHandling.ReadPar(this.pathUnmaskForConEnabled) == "1");
					}
				}
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000144 RID: 324 RVA: 0x000074E1 File Offset: 0x000056E1
		// (set) Token: 0x06000145 RID: 325 RVA: 0x000074E9 File Offset: 0x000056E9
		public ObservableCollection<string> DetectionTypeOptions { get; set; }

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000146 RID: 326 RVA: 0x000074F2 File Offset: 0x000056F2
		// (set) Token: 0x06000147 RID: 327 RVA: 0x000074FC File Offset: 0x000056FC
		public int GroupId
		{
			get
			{
				return this.groupId;
			}
			set
			{
				bool flag = value != this.groupId;
				if (flag)
				{
					LogbookManager.AddEntry(SSTEntryCode.ObjectDetectionParameterChanged, 3, this.productManager.CurrentLoadedProduct.ProductNumber, this.productManager.CurrentLoadedProduct.ProductName, this.classIndex.ToString(), this.groupId.ToString(), value.ToString());
					this.groupId = value;
					this.parHandling.WriteValue(this.pathGroupId, Convert.ToString(value));
					this.OnPropertyChanged("GroupId");
				}
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000148 RID: 328 RVA: 0x00007590 File Offset: 0x00005790
		// (set) Token: 0x06000149 RID: 329 RVA: 0x00007598 File Offset: 0x00005798
		public int MinCount
		{
			get
			{
				return this.minCount;
			}
			set
			{
				bool flag = value != this.minCount;
				if (flag)
				{
					LogbookManager.AddEntry(SSTEntryCode.ObjectDetectionParameterChanged, 5, this.productManager.CurrentLoadedProduct.ProductNumber, this.productManager.CurrentLoadedProduct.ProductName, this.classIndex.ToString(), this.minCount.ToString(), value.ToString());
					this.minCount = value;
					this.parHandling.WriteValue(this.pathMinCount, Convert.ToString(value));
					this.OnPropertyChanged("MinCount");
				}
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x0600014A RID: 330 RVA: 0x0000762C File Offset: 0x0000582C
		// (set) Token: 0x0600014B RID: 331 RVA: 0x00007634 File Offset: 0x00005834
		public int MaxCount
		{
			get
			{
				return this.maxCount;
			}
			set
			{
				bool flag = value != this.maxCount;
				if (flag)
				{
					LogbookManager.AddEntry(SSTEntryCode.ObjectDetectionParameterChanged, 6, this.productManager.CurrentLoadedProduct.ProductNumber, this.productManager.CurrentLoadedProduct.ProductName, this.classIndex.ToString(), this.maxCount.ToString(), value.ToString());
					this.maxCount = value;
					this.parHandling.WriteValue(this.pathMaxCount, Convert.ToString(value));
					this.OnPropertyChanged("MaxCount");
				}
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x0600014C RID: 332 RVA: 0x000076C8 File Offset: 0x000058C8
		// (set) Token: 0x0600014D RID: 333 RVA: 0x000076D0 File Offset: 0x000058D0
		public double ConfThreshold
		{
			get
			{
				return this.confThreshold;
			}
			set
			{
				bool flag = value != this.confThreshold;
				if (flag)
				{
					LogbookManager.AddEntry(SSTEntryCode.ObjectDetectionParameterChanged, 4, this.productManager.CurrentLoadedProduct.ProductNumber, this.productManager.CurrentLoadedProduct.ProductName, this.classIndex.ToString(), this.confThreshold.ToString(), value.ToString());
					bool flag2 = value > 1.0;
					if (flag2)
					{
						this.confThreshold = 1.0;
					}
					else
					{
						bool flag3 = value < 0.0;
						if (flag3)
						{
							this.confThreshold = 0.0;
						}
						else
						{
							this.confThreshold = value;
						}
					}
					this.parHandling.WriteValue(this.pathConfThreshold, Convert.ToString(this.confThreshold));
					this.OnPropertyChanged("ConfThreshold");
				}
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x0600014E RID: 334 RVA: 0x000077AE File Offset: 0x000059AE
		// (set) Token: 0x0600014F RID: 335 RVA: 0x000077B8 File Offset: 0x000059B8
		public bool UnmaskForConEnabled
		{
			get
			{
				return this.unmaskForConEnabled;
			}
			set
			{
				bool flag = value != this.unmaskForConEnabled;
				if (flag)
				{
					this.unmaskForConEnabled = value;
					this.parHandling.WriteValue(this.pathUnmaskForConEnabled, value ? "1" : "0");
					LogbookManager.AddEntry(SSTEntryCode.ObjectDetectionParameterChanged, 7, this.productManager.CurrentLoadedProduct.ProductNumber, this.productManager.CurrentLoadedProduct.ProductName, this.classIndex.ToString(), (!value) ? "1" : "0", value ? "1" : "0");
					this.OnPropertyChanged("UnmaskForConEnabled");
				}
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000150 RID: 336 RVA: 0x00007864 File Offset: 0x00005A64
		// (set) Token: 0x06000151 RID: 337 RVA: 0x0000786C File Offset: 0x00005A6C
		public string UnmaskForConMethod
		{
			get
			{
				return this.unmaskForConMethod;
			}
			set
			{
				bool flag = value != this.unmaskForConMethod;
				if (flag)
				{
					LogbookManager.AddEntry(SSTEntryCode.ObjectDetectionParameterChanged, 8, this.productManager.CurrentLoadedProduct.ProductNumber, this.productManager.CurrentLoadedProduct.ProductName, this.classIndex.ToString(), this.unmaskForConMethod, value);
					this.unmaskForConMethod = value;
					int value2 = this.UnmaskForConMethodOptions.IndexOf(this.unmaskForConMethod);
					this.parHandling.WriteValue(this.pathUnmaskForConMethod, Convert.ToString(value2));
					this.OnPropertyChanged("UnmaskForConMethod");
					this.OnPropertyChanged("IsThresholding");
					this.OnPropertyChanged("IsMinMaxAdaptiveThresholding");
					this.OnPropertyChanged("IsPercentileThresholding");
				}
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000152 RID: 338 RVA: 0x0000792E File Offset: 0x00005B2E
		// (set) Token: 0x06000153 RID: 339 RVA: 0x00007938 File Offset: 0x00005B38
		public int FilterSize
		{
			get
			{
				return this.filterSize;
			}
			set
			{
				bool flag = value != this.filterSize;
				if (flag)
				{
					LogbookManager.AddEntry(SSTEntryCode.ObjectDetectionParameterChanged, 10, this.productManager.CurrentLoadedProduct.ProductNumber, this.productManager.CurrentLoadedProduct.ProductName, this.classIndex.ToString(), this.filterSize.ToString(), value.ToString());
					this.filterSize = value;
					this.parHandling.WriteValue(this.pathFilterSize, Convert.ToString(value));
					this.OnPropertyChanged("FilterSize");
				}
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000154 RID: 340 RVA: 0x000079CD File Offset: 0x00005BCD
		// (set) Token: 0x06000155 RID: 341 RVA: 0x000079D8 File Offset: 0x00005BD8
		public int MinGroupSize
		{
			get
			{
				return this.minGroupSize;
			}
			set
			{
				bool flag = value != this.minGroupSize;
				if (flag)
				{
					LogbookManager.AddEntry(SSTEntryCode.ObjectDetectionParameterChanged, 11, this.productManager.CurrentLoadedProduct.ProductNumber, this.productManager.CurrentLoadedProduct.ProductName, this.classIndex.ToString(), this.minGroupSize.ToString(), value.ToString());
					this.minGroupSize = value;
					this.parHandling.WriteValue(this.pathMinGroupSize, Convert.ToString(value));
					this.OnPropertyChanged("MinGroupSize");
				}
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000156 RID: 342 RVA: 0x00007A6D File Offset: 0x00005C6D
		// (set) Token: 0x06000157 RID: 343 RVA: 0x00007A78 File Offset: 0x00005C78
		public double ConfThresholdTTA1
		{
			get
			{
				return this.confThresholdTTA1;
			}
			set
			{
				bool flag = value != this.confThresholdTTA1;
				if (flag)
				{
					LogbookManager.AddEntry(SSTEntryCode.ObjectDetectionParameterChanged, 12, this.productManager.CurrentLoadedProduct.ProductNumber, this.productManager.CurrentLoadedProduct.ProductName, this.classIndex.ToString(), this.confThresholdTTA1.ToString(), value.ToString());
					bool flag2 = value > 1.0;
					if (flag2)
					{
						this.confThresholdTTA1 = 1.0;
					}
					else
					{
						bool flag3 = value < 0.0;
						if (flag3)
						{
							this.confThresholdTTA1 = 0.0;
						}
						else
						{
							this.confThresholdTTA1 = value;
						}
					}
					this.parHandling.WriteValue(this.pathConfThresholdTTA1, Convert.ToString(this.confThresholdTTA1));
					this.OnPropertyChanged("ConfThresholdTTA1");
				}
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000158 RID: 344 RVA: 0x00007B57 File Offset: 0x00005D57
		// (set) Token: 0x06000159 RID: 345 RVA: 0x00007B60 File Offset: 0x00005D60
		public double ConfThresholdTTA2
		{
			get
			{
				return this.confThresholdTTA2;
			}
			set
			{
				bool flag = value != this.confThresholdTTA2;
				if (flag)
				{
					LogbookManager.AddEntry(SSTEntryCode.ObjectDetectionParameterChanged, 13, this.productManager.CurrentLoadedProduct.ProductNumber, this.productManager.CurrentLoadedProduct.ProductName, this.classIndex.ToString(), this.confThresholdTTA2.ToString(), value.ToString());
					bool flag2 = value > 1.0;
					if (flag2)
					{
						this.confThresholdTTA2 = 1.0;
					}
					else
					{
						bool flag3 = value < 0.0;
						if (flag3)
						{
							this.confThresholdTTA2 = 0.0;
						}
						else
						{
							this.confThresholdTTA2 = value;
						}
					}
					this.parHandling.WriteValue(this.pathConfThresholdTTA2, Convert.ToString(this.confThresholdTTA2));
					this.OnPropertyChanged("ConfThresholdTTA2");
				}
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x0600015A RID: 346 RVA: 0x00007C3F File Offset: 0x00005E3F
		// (set) Token: 0x0600015B RID: 347 RVA: 0x00007C48 File Offset: 0x00005E48
		public double ConfThresholdTTA3
		{
			get
			{
				return this.confThresholdTTA3;
			}
			set
			{
				bool flag = value != this.confThresholdTTA3;
				if (flag)
				{
					LogbookManager.AddEntry(SSTEntryCode.ObjectDetectionParameterChanged, 14, this.productManager.CurrentLoadedProduct.ProductNumber, this.productManager.CurrentLoadedProduct.ProductName, this.classIndex.ToString(), this.confThresholdTTA3.ToString(), value.ToString());
					bool flag2 = value > 1.0;
					if (flag2)
					{
						this.confThresholdTTA3 = 1.0;
					}
					else
					{
						bool flag3 = value < 0.0;
						if (flag3)
						{
							this.confThresholdTTA3 = 0.0;
						}
						else
						{
							this.confThresholdTTA3 = value;
						}
					}
					this.parHandling.WriteValue(this.pathConfThresholdTTA3, Convert.ToString(this.confThresholdTTA3));
					this.OnPropertyChanged("ConfThresholdTTA3");
				}
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x0600015C RID: 348 RVA: 0x00007D27 File Offset: 0x00005F27
		// (set) Token: 0x0600015D RID: 349 RVA: 0x00007D30 File Offset: 0x00005F30
		public bool TTAEnabled
		{
			get
			{
				return this.ttaEnabled;
			}
			set
			{
				bool flag = value != this.ttaEnabled;
				if (flag)
				{
					bool flag2 = value && this.parHandling.ReadPar(Definition.objectDetectionModelType) != "3";
					if (flag2)
					{
						Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("msgThinkXOEnableTTA"), DialogStyle.Info, DialogButtons.OK);
					}
					else
					{
						bool flag3 = value && Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtNewAutotrainRequired"), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Cancel;
						if (!flag3)
						{
							this.ttaEnabled = value;
							this.parHandling.WriteValue(this.pathTTAEnabled, value ? "1" : "0");
							LogbookManager.AddEntry(SSTEntryCode.ObjectDetectionParameterChanged, 15, this.productManager.CurrentLoadedProduct.ProductNumber, this.productManager.CurrentLoadedProduct.ProductName, this.classIndex.ToString(), (!value) ? "1" : "0", value ? "1" : "0");
							this.OnPropertyChanged("TTAEnabled");
						}
					}
				}
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x0600015E RID: 350 RVA: 0x00007E50 File Offset: 0x00006050
		// (set) Token: 0x0600015F RID: 351 RVA: 0x00007E58 File Offset: 0x00006058
		public int LowThresholdMinMax
		{
			get
			{
				return this.lowThresholdMinMax;
			}
			set
			{
				bool flag = value != this.lowThresholdMinMax;
				if (flag)
				{
					LogbookManager.AddEntry(SSTEntryCode.ObjectDetectionParameterChanged, 16, this.productManager.CurrentLoadedProduct.ProductNumber, this.productManager.CurrentLoadedProduct.ProductName, this.classIndex.ToString(), this.lowThresholdMinMax.ToString(), value.ToString());
					this.lowThresholdMinMax = value;
					this.parHandling.WriteValue(this.pathLowThresholdMinMax, Convert.ToString(value));
					this.OnPropertyChanged("LowThresholdMinMax");
				}
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000160 RID: 352 RVA: 0x00007EED File Offset: 0x000060ED
		// (set) Token: 0x06000161 RID: 353 RVA: 0x00007EF8 File Offset: 0x000060F8
		public int HighThresholdMinMax
		{
			get
			{
				return this.highThresholdMinMax;
			}
			set
			{
				bool flag = value != this.highThresholdMinMax;
				if (flag)
				{
					LogbookManager.AddEntry(SSTEntryCode.ObjectDetectionParameterChanged, 17, this.productManager.CurrentLoadedProduct.ProductNumber, this.productManager.CurrentLoadedProduct.ProductName, this.classIndex.ToString(), this.highThresholdMinMax.ToString(), value.ToString());
					this.highThresholdMinMax = value;
					this.parHandling.WriteValue(this.pathHighThresholdMinMax, Convert.ToString(value));
					this.OnPropertyChanged("HighThresholdMinMax");
				}
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000162 RID: 354 RVA: 0x00007F8D File Offset: 0x0000618D
		// (set) Token: 0x06000163 RID: 355 RVA: 0x00007F98 File Offset: 0x00006198
		public double PercentileThreshold
		{
			get
			{
				return this.percentileThreshold;
			}
			set
			{
				bool flag = value != this.percentileThreshold;
				if (flag)
				{
					LogbookManager.AddEntry(SSTEntryCode.ObjectDetectionParameterChanged, 18, this.productManager.CurrentLoadedProduct.ProductNumber, this.productManager.CurrentLoadedProduct.ProductName, this.classIndex.ToString(), this.percentileThreshold.ToString(), value.ToString());
					bool flag2 = value > 1.0;
					if (flag2)
					{
						this.percentileThreshold = 1.0;
					}
					else
					{
						bool flag3 = value < 0.0;
						if (flag3)
						{
							this.percentileThreshold = 0.0;
						}
						else
						{
							this.percentileThreshold = value;
						}
					}
					this.parHandling.WriteValue(this.pathPercentileThreshold, Convert.ToString(this.percentileThreshold));
					this.OnPropertyChanged("PercentileThreshold");
				}
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000164 RID: 356 RVA: 0x00008077 File Offset: 0x00006277
		// (set) Token: 0x06000165 RID: 357 RVA: 0x0000807F File Offset: 0x0000627F
		public ObservableCollection<string> UnmaskForConMethodOptions { get; set; }

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000166 RID: 358 RVA: 0x00008088 File Offset: 0x00006288
		// (set) Token: 0x06000167 RID: 359 RVA: 0x00008090 File Offset: 0x00006290
		public bool IsClassAvailable { get; set; }

		// Token: 0x06000168 RID: 360 RVA: 0x0000809C File Offset: 0x0000629C
		public AIObjectClass(ParameterHandling parHandling, ProductManager productManager, int classIndex, string className)
		{
			this.parHandling = parHandling;
			this.productManager = productManager;
			this.className = className;
			this.classIndex = classIndex;
			this.tabName = "Filter " + classIndex.ToString();
			this.DetectionTypeOptions = new ObservableCollection<string>
			{
				"Count",
				"Foreign Body"
			};
			this.UnmaskForConMethodOptions = new ObservableCollection<string>
			{
				"Rectangle",
				"Adaptive",
				"MinMax",
				"Percentile"
			};
			this.Initialize();
			this.RegisterLocalizedText();
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00008154 File Offset: 0x00006354
		private void Initialize()
		{
			this.pathClassEnabled = string.Format("{0}{1}{2}", Definition.objectDetectionClassesSubPath, this.classIndex, Definition.objectDetectionClassEnabled);
			this.classEnabled = (this.parHandling.ReadPar(this.pathClassEnabled) == "1");
			this.pathMinCount = string.Format("{0}{1}{2}", Definition.objectDetectionClassesSubPath, this.classIndex, Definition.objectDetectionMinObjects);
			this.minCount = Convert.ToInt32(this.parHandling.ReadPar(this.pathMinCount));
			this.pathMaxCount = string.Format("{0}{1}{2}", Definition.objectDetectionClassesSubPath, this.classIndex, Definition.objectDetectionMaxObjects);
			this.maxCount = Convert.ToInt32(this.parHandling.ReadPar(this.pathMaxCount));
			this.pathDetectionType = string.Format("{0}{1}{2}", Definition.objectDetectionClassesSubPath, this.classIndex, Definition.objectDetectionType);
			this.detectionType = this.DetectionTypeOptions[Convert.ToInt32(this.parHandling.ReadPar(this.pathDetectionType))];
			this.pathGroupId = string.Format("{0}{1}{2}", Definition.objectDetectionClassesSubPath, this.classIndex, Definition.objectDetectionGroupId);
			this.groupId = Convert.ToInt32(this.parHandling.ReadPar(this.pathGroupId));
			this.pathConfThreshold = string.Format("{0}{1}{2}", Definition.objectDetectionClassesSubPath, this.classIndex, Definition.objectDetectionConfThreshold);
			this.confThreshold = Convert.ToDouble(this.parHandling.ReadPar(this.pathConfThreshold), CultureInfo.InvariantCulture);
			this.pathClassAvailable = string.Format("{0}{1}{2}", Definition.objectDetectionClassesSubPath, this.classIndex, Definition.objectDetectionClassAvailable);
			this.IsClassAvailable = (this.parHandling.ReadPar(this.pathClassAvailable) == "1");
			this.pathUnmaskForConEnabled = string.Format("{0}{1}{2}", Definition.objectDetectionClassesSubPath, this.classIndex, Definition.objectDetectionUnmaskForConEnabled);
			this.unmaskForConEnabled = (this.parHandling.ReadPar(this.pathUnmaskForConEnabled) == "1");
			this.pathUnmaskForConMethod = string.Format("{0}{1}{2}", Definition.objectDetectionClassesSubPath, this.classIndex, Definition.objectDetectionUnmaskForConMethod);
			this.unmaskForConMethod = this.UnmaskForConMethodOptions[Convert.ToInt32(this.parHandling.ReadPar(this.pathUnmaskForConMethod))];
			this.pathFilterSize = string.Format("{0}{1}{2}", Definition.objectDetectionClassesSubPath, this.classIndex, Definition.objectDetectionFilterSize);
			this.filterSize = Convert.ToInt32(this.parHandling.ReadPar(this.pathFilterSize));
			this.pathMinGroupSize = string.Format("{0}{1}{2}", Definition.objectDetectionClassesSubPath, this.classIndex, Definition.objectDetectionMinGroupSize);
			this.minGroupSize = Convert.ToInt32(this.parHandling.ReadPar(this.pathMinGroupSize));
			this.pathConfThresholdTTA1 = string.Format("{0}{1}{2}", Definition.objectDetectionClassesSubPath, this.classIndex, Definition.objectDetectionConfThresholdTTA1);
			this.confThresholdTTA1 = Convert.ToDouble(this.parHandling.ReadPar(this.pathConfThresholdTTA1), CultureInfo.InvariantCulture);
			this.pathConfThresholdTTA2 = string.Format("{0}{1}{2}", Definition.objectDetectionClassesSubPath, this.classIndex, Definition.objectDetectionConfThresholdTTA2);
			this.confThresholdTTA2 = Convert.ToDouble(this.parHandling.ReadPar(this.pathConfThresholdTTA2), CultureInfo.InvariantCulture);
			this.pathConfThresholdTTA3 = string.Format("{0}{1}{2}", Definition.objectDetectionClassesSubPath, this.classIndex, Definition.objectDetectionConfThresholdTTA3);
			this.confThresholdTTA3 = Convert.ToDouble(this.parHandling.ReadPar(this.pathConfThresholdTTA3), CultureInfo.InvariantCulture);
			this.pathTTAEnabled = string.Format("{0}{1}{2}", Definition.objectDetectionClassesSubPath, this.classIndex, Definition.objectDetectionTTAEnabled);
			this.ttaEnabled = (this.parHandling.ReadPar(this.pathTTAEnabled) == "1");
			this.pathLowThresholdMinMax = string.Format("{0}{1}{2}", Definition.objectDetectionClassesSubPath, this.classIndex, Definition.objectDetectionLowThresholdMinMax);
			this.lowThresholdMinMax = Convert.ToInt32(this.parHandling.ReadPar(this.pathLowThresholdMinMax));
			this.pathHighThresholdMinMax = string.Format("{0}{1}{2}", Definition.objectDetectionClassesSubPath, this.classIndex, Definition.objectDetectionHighThresholdMinMax);
			this.highThresholdMinMax = Convert.ToInt32(this.parHandling.ReadPar(this.pathHighThresholdMinMax));
			this.pathPercentileThreshold = string.Format("{0}{1}{2}", Definition.objectDetectionClassesSubPath, this.classIndex, Definition.objectDetectionPercentileThreshold);
			this.percentileThreshold = Convert.ToDouble(this.parHandling.ReadPar(this.pathPercentileThreshold), CultureInfo.InvariantCulture);
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00008650 File Offset: 0x00006850
		private void RegisterLocalizedText()
		{
			this.StrClassEnabled = new BoundLocalizedText("txtObjectClassEnabled");
			this.StrDetectionType = new BoundLocalizedText("txtObjectDetectionType");
			this.StrGroupId = new BoundLocalizedText("txtObjectGroupId");
			this.StrConfThreshold = new BoundLocalizedText("txtObjectConfThreshold");
			this.StrMinCount = new BoundLocalizedText("txtMinCount");
			this.StrMaxCount = new BoundLocalizedText("txtMaxCount");
			this.StrUnmaskForConEnabled = new BoundLocalizedText("txtObjectUnmaskForConEnabled");
			this.StrUnmaskForConMethod = new BoundLocalizedText("txtObjectUnmaskForConMethod");
			this.StrFilterSize = new BoundLocalizedText("txtFilterSize");
			this.StrMinGroupSize = new BoundLocalizedText("txtObjectMinGroupSize");
			this.StrClassName = new BoundLocalizedText("txtObjectClassName");
			this.StrConfThresholdTTA1 = new BoundLocalizedText("txtObjectConfThreshold");
			this.StrConfThresholdTTA1.LocalizedText = this.StrConfThresholdTTA1.LocalizedText + " 1";
			this.StrConfThresholdTTA2 = new BoundLocalizedText("txtObjectConfThreshold");
			this.StrConfThresholdTTA2.LocalizedText = this.StrConfThresholdTTA2.LocalizedText + " 2";
			this.StrConfThresholdTTA3 = new BoundLocalizedText("txtObjectConfThreshold");
			this.StrConfThresholdTTA3.LocalizedText = this.StrConfThresholdTTA3.LocalizedText + " 3";
			this.StrTTAEnabled = new BoundLocalizedText("txtObjectTTAEnabled");
			this.StrLowThresholdMinMax = new BoundLocalizedText("txtLowThreshold");
			this.StrHighThresholdMinMax = new BoundLocalizedText("txtHighThreshold");
			this.StrPercentileThreshold = new BoundLocalizedText("txtPercentileThreshold");
		}

		// Token: 0x0600016B RID: 363 RVA: 0x000087F4 File Offset: 0x000069F4
		public void DeregisterLocalizedText()
		{
			MainWindow.localizationManager.Deregister(this.StrClassEnabled);
			MainWindow.localizationManager.Deregister(this.StrDetectionType);
			MainWindow.localizationManager.Deregister(this.StrGroupId);
			MainWindow.localizationManager.Deregister(this.StrConfThreshold);
			MainWindow.localizationManager.Deregister(this.StrMinCount);
			MainWindow.localizationManager.Deregister(this.StrMaxCount);
			MainWindow.localizationManager.Deregister(this.StrUnmaskForConEnabled);
			MainWindow.localizationManager.Deregister(this.StrUnmaskForConMethod);
			MainWindow.localizationManager.Deregister(this.StrFilterSize);
			MainWindow.localizationManager.Deregister(this.StrMinGroupSize);
			MainWindow.localizationManager.Deregister(this.StrClassName);
			MainWindow.localizationManager.Deregister(this.StrConfThresholdTTA1);
			MainWindow.localizationManager.Deregister(this.StrConfThresholdTTA2);
			MainWindow.localizationManager.Deregister(this.StrConfThresholdTTA3);
			MainWindow.localizationManager.Deregister(this.StrTTAEnabled);
			MainWindow.localizationManager.Deregister(this.StrLowThresholdMinMax);
			MainWindow.localizationManager.Deregister(this.StrHighThresholdMinMax);
			MainWindow.localizationManager.Deregister(this.StrPercentileThreshold);
		}

		// Token: 0x0600016C RID: 364 RVA: 0x00008934 File Offset: 0x00006B34
		protected void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if (propertyChanged != null)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00008950 File Offset: 0x00006B50
		public void ChangeGroupId()
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtObjectGroupId"), this.GroupId.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_InputGroupId;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x0600016E RID: 366 RVA: 0x000089A6 File Offset: 0x00006BA6
		private void numKeyboard_InputGroupId(object sender, KeyboardInputEventArgs e)
		{
			this.GroupId = int.Parse(e.Text);
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_InputGroupId;
		}

		// Token: 0x0600016F RID: 367 RVA: 0x000089D4 File Offset: 0x00006BD4
		public void ChangeMinCount()
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtMinCount"), this.MinCount.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_InputMinCount;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x06000170 RID: 368 RVA: 0x00008A2A File Offset: 0x00006C2A
		private void numKeyboard_InputMinCount(object sender, KeyboardInputEventArgs e)
		{
			this.MinCount = int.Parse(e.Text);
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_InputMinCount;
		}

		// Token: 0x06000171 RID: 369 RVA: 0x00008A58 File Offset: 0x00006C58
		public void ChangeMaxCount()
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtMaxCount"), this.MaxCount.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_InputMaxCount;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x06000172 RID: 370 RVA: 0x00008AAE File Offset: 0x00006CAE
		private void numKeyboard_InputMaxCount(object sender, KeyboardInputEventArgs e)
		{
			this.MaxCount = int.Parse(e.Text);
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_InputMaxCount;
		}

		// Token: 0x06000173 RID: 371 RVA: 0x00008ADC File Offset: 0x00006CDC
		public void ChangeConfThreshold()
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtObjectConfThreshold"), this.ConfThreshold.ToString(), NumKeyboardType.TypeDouble);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_ConfThreshold;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x06000174 RID: 372 RVA: 0x00008B32 File Offset: 0x00006D32
		private void numKeyboard_ConfThreshold(object sender, KeyboardInputEventArgs e)
		{
			this.ConfThreshold = double.Parse(e.Text.Replace(',', '.'), new CultureInfo("en-US"));
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_ConfThreshold;
		}

		// Token: 0x06000175 RID: 373 RVA: 0x00008B74 File Offset: 0x00006D74
		public void ChangeFilterSize()
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtFilterSize"), this.FilterSize.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_InputFilterSize;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x06000176 RID: 374 RVA: 0x00008BCA File Offset: 0x00006DCA
		private void numKeyboard_InputFilterSize(object sender, KeyboardInputEventArgs e)
		{
			this.FilterSize = int.Parse(e.Text);
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_InputFilterSize;
		}

		// Token: 0x06000177 RID: 375 RVA: 0x00008BF8 File Offset: 0x00006DF8
		public void ChangeMinGroupSize()
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtObjectMinGroupSize"), this.MinGroupSize.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_InputMinGroupSize;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x06000178 RID: 376 RVA: 0x00008C4E File Offset: 0x00006E4E
		private void numKeyboard_InputMinGroupSize(object sender, KeyboardInputEventArgs e)
		{
			this.MinGroupSize = int.Parse(e.Text);
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_InputMinGroupSize;
		}

		// Token: 0x06000179 RID: 377 RVA: 0x00008C7C File Offset: 0x00006E7C
		public void ChangeConfThresholdTTA1()
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtObjectConfThresholdTTA1"), this.ConfThresholdTTA1.ToString(), NumKeyboardType.TypeDouble);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_ConfThresholdTTA1;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x0600017A RID: 378 RVA: 0x00008CD2 File Offset: 0x00006ED2
		private void numKeyboard_ConfThresholdTTA1(object sender, KeyboardInputEventArgs e)
		{
			this.ConfThresholdTTA1 = double.Parse(e.Text.Replace(',', '.'), new CultureInfo("en-US"));
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_ConfThresholdTTA1;
		}

		// Token: 0x0600017B RID: 379 RVA: 0x00008D14 File Offset: 0x00006F14
		public void ChangeConfThresholdTTA2()
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtObjectConfThresholdTTA2"), this.ConfThresholdTTA2.ToString(), NumKeyboardType.TypeDouble);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_ConfThresholdTTA2;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x0600017C RID: 380 RVA: 0x00008D6A File Offset: 0x00006F6A
		private void numKeyboard_ConfThresholdTTA2(object sender, KeyboardInputEventArgs e)
		{
			this.ConfThresholdTTA2 = double.Parse(e.Text.Replace(',', '.'), new CultureInfo("en-US"));
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_ConfThresholdTTA2;
		}

		// Token: 0x0600017D RID: 381 RVA: 0x00008DAC File Offset: 0x00006FAC
		public void ChangeConfThresholdTTA3()
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtObjectConfThresholdTTA3"), this.ConfThresholdTTA3.ToString(), NumKeyboardType.TypeDouble);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_ConfThresholdTTA3;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x0600017E RID: 382 RVA: 0x00008E02 File Offset: 0x00007002
		private void numKeyboard_ConfThresholdTTA3(object sender, KeyboardInputEventArgs e)
		{
			this.ConfThresholdTTA3 = double.Parse(e.Text.Replace(',', '.'), new CultureInfo("en-US"));
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_ConfThresholdTTA3;
		}

		// Token: 0x0600017F RID: 383 RVA: 0x00008E44 File Offset: 0x00007044
		public void ChangeLowThresholdMinMax()
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtLowThreshold"), this.LowThresholdMinMax.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_LowThresholdMinMax;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x06000180 RID: 384 RVA: 0x00008E9A File Offset: 0x0000709A
		private void numKeyboard_LowThresholdMinMax(object sender, KeyboardInputEventArgs e)
		{
			this.LowThresholdMinMax = int.Parse(e.Text);
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_LowThresholdMinMax;
		}

		// Token: 0x06000181 RID: 385 RVA: 0x00008EC8 File Offset: 0x000070C8
		public void ChangeHighThresholdMinMax()
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtHighThreshold"), this.HighThresholdMinMax.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_HighThresholdMinMax;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x06000182 RID: 386 RVA: 0x00008F1E File Offset: 0x0000711E
		private void numKeyboard_HighThresholdMinMax(object sender, KeyboardInputEventArgs e)
		{
			this.HighThresholdMinMax = int.Parse(e.Text);
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_HighThresholdMinMax;
		}

		// Token: 0x06000183 RID: 387 RVA: 0x00008F4C File Offset: 0x0000714C
		public void ChangePercentileThreshold()
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtPercentileThreshold"), this.PercentileThreshold.ToString(), NumKeyboardType.TypeDouble);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_PercentileThreshold;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x06000184 RID: 388 RVA: 0x00008FA2 File Offset: 0x000071A2
		private void numKeyboard_PercentileThreshold(object sender, KeyboardInputEventArgs e)
		{
			this.PercentileThreshold = double.Parse(e.Text.Replace(',', '.'), new CultureInfo("en-US"));
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_PercentileThreshold;
		}

		// Token: 0x06000185 RID: 389 RVA: 0x00008FE4 File Offset: 0x000071E4
		public void CleanUp()
		{
			this.DeregisterLocalizedText();
			bool flag = this.numKeyboard != null;
			if (flag)
			{
				this.numKeyboard.TextInputKeyboard -= this.numKeyboard_InputGroupId;
				this.numKeyboard.TextInputKeyboard -= this.numKeyboard_InputMinCount;
				this.numKeyboard.TextInputKeyboard -= this.numKeyboard_InputMaxCount;
				this.numKeyboard.TextInputKeyboard -= this.numKeyboard_ConfThreshold;
				this.numKeyboard.TextInputKeyboard -= this.numKeyboard_InputFilterSize;
				this.numKeyboard.TextInputKeyboard -= this.numKeyboard_InputMinGroupSize;
				this.numKeyboard.TextInputKeyboard -= this.numKeyboard_ConfThresholdTTA1;
				this.numKeyboard.TextInputKeyboard -= this.numKeyboard_ConfThresholdTTA2;
				this.numKeyboard.TextInputKeyboard -= this.numKeyboard_ConfThresholdTTA3;
				this.numKeyboard.TextInputKeyboard -= this.numKeyboard_LowThresholdMinMax;
				this.numKeyboard.TextInputKeyboard -= this.numKeyboard_HighThresholdMinMax;
				this.numKeyboard.TextInputKeyboard -= this.numKeyboard_PercentileThreshold;
				this.numKeyboard = null;
			}
			this.PropertyChanged = null;
		}

		// Token: 0x06000186 RID: 390 RVA: 0x0000913C File Offset: 0x0000733C
		public void UpdateConfThreshold()
		{
			this.ConfThreshold = Convert.ToDouble(this.parHandling.ReadPar(this.pathConfThreshold), CultureInfo.InvariantCulture);
			this.ConfThresholdTTA1 = Convert.ToDouble(this.parHandling.ReadPar(this.pathConfThresholdTTA1), CultureInfo.InvariantCulture);
			this.ConfThresholdTTA2 = Convert.ToDouble(this.parHandling.ReadPar(this.pathConfThresholdTTA2), CultureInfo.InvariantCulture);
			this.ConfThresholdTTA3 = Convert.ToDouble(this.parHandling.ReadPar(this.pathConfThresholdTTA3), CultureInfo.InvariantCulture);
		}

		// Token: 0x0400013D RID: 317
		private ProductManager productManager;

		// Token: 0x0400013E RID: 318
		private ParameterHandling parHandling;

		// Token: 0x0400013F RID: 319
		private NumKeyboard numKeyboard;

		// Token: 0x04000140 RID: 320
		private int classIndex;

		// Token: 0x04000153 RID: 339
		private string tabName;

		// Token: 0x04000154 RID: 340
		private string className;

		// Token: 0x04000155 RID: 341
		private string pathClassEnabled;

		// Token: 0x04000156 RID: 342
		private bool classEnabled;

		// Token: 0x04000157 RID: 343
		private string pathDetectionType;

		// Token: 0x04000158 RID: 344
		private string detectionType;

		// Token: 0x0400015A RID: 346
		private string pathGroupId;

		// Token: 0x0400015B RID: 347
		private int groupId;

		// Token: 0x0400015C RID: 348
		private string pathMinCount;

		// Token: 0x0400015D RID: 349
		private int minCount;

		// Token: 0x0400015E RID: 350
		private string pathMaxCount;

		// Token: 0x0400015F RID: 351
		private int maxCount;

		// Token: 0x04000160 RID: 352
		private string pathConfThreshold;

		// Token: 0x04000161 RID: 353
		private double confThreshold;

		// Token: 0x04000162 RID: 354
		private string pathUnmaskForConEnabled;

		// Token: 0x04000163 RID: 355
		private bool unmaskForConEnabled;

		// Token: 0x04000164 RID: 356
		private string pathUnmaskForConMethod;

		// Token: 0x04000165 RID: 357
		private string unmaskForConMethod;

		// Token: 0x04000166 RID: 358
		private string pathFilterSize;

		// Token: 0x04000167 RID: 359
		private int filterSize;

		// Token: 0x04000168 RID: 360
		private string pathMinGroupSize;

		// Token: 0x04000169 RID: 361
		private int minGroupSize;

		// Token: 0x0400016A RID: 362
		private string pathConfThresholdTTA1;

		// Token: 0x0400016B RID: 363
		private double confThresholdTTA1;

		// Token: 0x0400016C RID: 364
		private string pathConfThresholdTTA2;

		// Token: 0x0400016D RID: 365
		private double confThresholdTTA2;

		// Token: 0x0400016E RID: 366
		private string pathConfThresholdTTA3;

		// Token: 0x0400016F RID: 367
		private double confThresholdTTA3;

		// Token: 0x04000170 RID: 368
		private string pathTTAEnabled;

		// Token: 0x04000171 RID: 369
		private bool ttaEnabled;

		// Token: 0x04000172 RID: 370
		private string pathLowThresholdMinMax;

		// Token: 0x04000173 RID: 371
		private int lowThresholdMinMax;

		// Token: 0x04000174 RID: 372
		private string pathHighThresholdMinMax;

		// Token: 0x04000175 RID: 373
		private int highThresholdMinMax;

		// Token: 0x04000176 RID: 374
		private string pathPercentileThreshold;

		// Token: 0x04000177 RID: 375
		private double percentileThreshold;

		// Token: 0x04000179 RID: 377
		private string pathClassAvailable;
	}
}
