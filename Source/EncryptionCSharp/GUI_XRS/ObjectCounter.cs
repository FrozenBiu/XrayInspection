using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using DatabaseConnection;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x02000235 RID: 565
	public class ObjectCounter : INotifyPropertyChanged
	{
		// Token: 0x140000E2 RID: 226
		// (add) Token: 0x06002AEE RID: 10990 RVA: 0x0011D5EC File Offset: 0x0011B7EC
		// (remove) Token: 0x06002AEF RID: 10991 RVA: 0x0011D624 File Offset: 0x0011B824
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x17000BB7 RID: 2999
		// (get) Token: 0x06002AF0 RID: 10992 RVA: 0x0011D65C File Offset: 0x0011B85C
		// (set) Token: 0x06002AF1 RID: 10993 RVA: 0x0011D674 File Offset: 0x0011B874
		public int CurrentCount
		{
			get
			{
				return this.currentCount;
			}
			set
			{
				this.currentCount = value;
				this.OnPropertyChanged("CurrentCount");
			}
		}

		// Token: 0x17000BB8 RID: 3000
		// (get) Token: 0x06002AF2 RID: 10994 RVA: 0x0011D68C File Offset: 0x0011B88C
		// (set) Token: 0x06002AF3 RID: 10995 RVA: 0x0011D6A4 File Offset: 0x0011B8A4
		public int MinCount
		{
			get
			{
				return this.minCount;
			}
			set
			{
				bool flag = this.minCount != value;
				if (flag)
				{
					bool flag2 = this.initialized;
					if (flag2)
					{
						LogbookManager.AddEntry(SSTEntryCode.CounterParameterChanged, 1, this.minCount.ToString(), value.ToString());
					}
					this.minCount = ((value >= 100000) ? 99999 : value);
					this.OnPropertyChanged("MinCount");
					bool flag3 = this.initialized;
					if (flag3)
					{
						this.parHandling.WriteValue(Definition.filterMainPath + this.strImageGroup + ObjectCounter.minCountSubPath, this.minCount.ToString());
					}
				}
			}
		}

		// Token: 0x17000BB9 RID: 3001
		// (get) Token: 0x06002AF4 RID: 10996 RVA: 0x0011D748 File Offset: 0x0011B948
		// (set) Token: 0x06002AF5 RID: 10997 RVA: 0x0011D760 File Offset: 0x0011B960
		public int MaxCount
		{
			get
			{
				return this.maxCount;
			}
			set
			{
				bool flag = this.maxCount != value;
				if (flag)
				{
					bool flag2 = this.initialized;
					if (flag2)
					{
						LogbookManager.AddEntry(SSTEntryCode.CounterParameterChanged, 2, this.maxCount.ToString(), value.ToString());
					}
					this.maxCount = ((value >= 100000) ? 99999 : value);
					this.OnPropertyChanged("MaxCount");
					bool flag3 = this.initialized;
					if (flag3)
					{
						this.parHandling.WriteValue(Definition.filterMainPath + this.strImageGroup + ObjectCounter.maxCountSubPath, this.maxCount.ToString());
					}
				}
			}
		}

		// Token: 0x17000BBA RID: 3002
		// (get) Token: 0x06002AF6 RID: 10998 RVA: 0x0011D804 File Offset: 0x0011BA04
		// (set) Token: 0x06002AF7 RID: 10999 RVA: 0x0011D81C File Offset: 0x0011BA1C
		public int LowThreshold
		{
			get
			{
				return this.lowThreshold;
			}
			set
			{
				bool flag = this.lowThreshold != value;
				if (flag)
				{
					bool flag2 = this.initialized;
					if (flag2)
					{
						LogbookManager.AddEntry(SSTEntryCode.CounterParameterChanged, 3, this.lowThreshold.ToString(), value.ToString());
					}
					this.lowThreshold = value;
					this.OnPropertyChanged("LowThreshold");
					bool flag3 = this.initialized;
					if (flag3)
					{
						this.parHandling.WriteValue(Definition.filterMainPath + this.strImageGroup + ObjectCounter.lowThresholdSubPath, this.lowThreshold.ToString());
					}
				}
			}
		}

		// Token: 0x17000BBB RID: 3003
		// (get) Token: 0x06002AF8 RID: 11000 RVA: 0x0011D8B0 File Offset: 0x0011BAB0
		// (set) Token: 0x06002AF9 RID: 11001 RVA: 0x0011D8C8 File Offset: 0x0011BAC8
		public int HighThreshold
		{
			get
			{
				return this.highThreshold;
			}
			set
			{
				bool flag = this.highThreshold != value;
				if (flag)
				{
					bool flag2 = this.initialized;
					if (flag2)
					{
						LogbookManager.AddEntry(SSTEntryCode.CounterParameterChanged, 4, this.highThreshold.ToString(), value.ToString());
					}
					this.highThreshold = value;
					this.OnPropertyChanged("HighThreshold");
					bool flag3 = this.initialized;
					if (flag3)
					{
						this.parHandling.WriteValue(Definition.filterMainPath + this.strImageGroup + ObjectCounter.highThresholdSubPath, this.highThreshold.ToString());
					}
				}
			}
		}

		// Token: 0x17000BBC RID: 3004
		// (get) Token: 0x06002AFA RID: 11002 RVA: 0x0011D95C File Offset: 0x0011BB5C
		// (set) Token: 0x06002AFB RID: 11003 RVA: 0x0011D974 File Offset: 0x0011BB74
		public int MinSize
		{
			get
			{
				return this.minSize;
			}
			set
			{
				bool flag = this.minSize != value;
				if (flag)
				{
					bool flag2 = this.initialized;
					if (flag2)
					{
						LogbookManager.AddEntry(SSTEntryCode.CounterParameterChanged, 5, this.minSize.ToString(), value.ToString());
					}
					this.minSize = value;
					this.OnPropertyChanged("MinSize");
					bool flag3 = this.initialized;
					if (flag3)
					{
						this.parHandling.WriteValue(Definition.filterMainPath + this.strImageGroup + ObjectCounter.minSizeSubPath, this.minSize.ToString());
					}
				}
			}
		}

		// Token: 0x17000BBD RID: 3005
		// (get) Token: 0x06002AFC RID: 11004 RVA: 0x0011DA08 File Offset: 0x0011BC08
		// (set) Token: 0x06002AFD RID: 11005 RVA: 0x0011DA20 File Offset: 0x0011BC20
		public int MaxSize
		{
			get
			{
				return this.maxSize;
			}
			set
			{
				bool flag = this.maxSize != value;
				if (flag)
				{
					bool flag2 = this.initialized;
					if (flag2)
					{
						LogbookManager.AddEntry(SSTEntryCode.CounterParameterChanged, 6, this.maxSize.ToString(), value.ToString());
					}
					this.maxSize = value;
					this.OnPropertyChanged("MaxSize");
					bool flag3 = this.initialized;
					if (flag3)
					{
						this.parHandling.WriteValue(Definition.filterMainPath + this.strImageGroup + ObjectCounter.maxSizeSubPath, this.maxSize.ToString());
					}
				}
			}
		}

		// Token: 0x17000BBE RID: 3006
		// (get) Token: 0x06002AFE RID: 11006 RVA: 0x0011DAB4 File Offset: 0x0011BCB4
		// (set) Token: 0x06002AFF RID: 11007 RVA: 0x0011DACC File Offset: 0x0011BCCC
		public int FilterSize
		{
			get
			{
				return this.filterSize;
			}
			set
			{
				bool flag = this.filterSize != value;
				if (flag)
				{
					bool flag2 = this.initialized;
					if (flag2)
					{
						LogbookManager.AddEntry(SSTEntryCode.CounterParameterChanged, 7, this.filterSize.ToString(), value.ToString());
					}
					this.filterSize = value;
					this.OnPropertyChanged("FilterSize");
					bool flag3 = this.initialized;
					if (flag3)
					{
						this.parHandling.WriteValue(Definition.filterMainPath + this.strImageGroup + ObjectCounter.filterSizePath, this.filterSize.ToString());
					}
				}
			}
		}

		// Token: 0x17000BBF RID: 3007
		// (get) Token: 0x06002B00 RID: 11008 RVA: 0x0011DB60 File Offset: 0x0011BD60
		// (set) Token: 0x06002B01 RID: 11009 RVA: 0x0011DB78 File Offset: 0x0011BD78
		public bool UnmaskForContEnabled
		{
			get
			{
				return this.unmaskForContEnabled;
			}
			set
			{
				bool flag = value != this.unmaskForContEnabled;
				if (flag)
				{
					bool flag2 = this.initialized;
					if (flag2)
					{
						LogbookManager.AddEntry(SSTEntryCode.CounterParameterChanged, 8, this.unmaskForContEnabled.ToString(), value.ToString());
					}
					this.unmaskForContEnabled = value;
					this.OnPropertyChanged("UnmaskForContEnabled");
					bool flag3 = this.initialized;
					if (flag3)
					{
						this.parHandling.WriteValue(Definition.filterMainPath + this.strImageGroup + ObjectCounter.unsmaskForContPath, value ? "1" : "0");
					}
				}
			}
		}

		// Token: 0x06002B02 RID: 11010 RVA: 0x0011DC10 File Offset: 0x0011BE10
		public ObjectCounter(ParameterHandling parHandling, int group, ProductManager productManager, DBConnection dbConnection)
		{
			this.parHandling = parHandling;
			this.dbConnection = dbConnection;
			bool flag = group <= 0;
			if (flag)
			{
				this.additionalLogbookText = ": P" + productManager.CurrentLoadedProduct.ProductNumber + " " + productManager.CurrentLoadedProduct.ProductName;
			}
			else
			{
				ProductData[] array = (from product in productManager.CurrentLoadedProduct.SubProducts
				where product.TableName.EndsWith(@group.ToString())
				select product).ToArray<ProductData>();
				bool flag2 = array.Length != 0;
				if (flag2)
				{
					this.additionalLogbookText = string.Concat(new string[]
					{
						": P",
						productManager.CurrentLoadedProduct.ProductNumber,
						"_",
						group.ToString(),
						" ",
						array[0].ProductName
					});
				}
			}
			this.strImageGroup = ((group > 0) ? group.ToString() : "");
			this.GetValuesFromDatabase(productManager);
			this.initialized = true;
		}

		// Token: 0x06002B03 RID: 11011 RVA: 0x0011DD38 File Offset: 0x0011BF38
		private void GetValuesFromDatabase(ProductManager productManager)
		{
			string text = "";
			string tableName = (this.strImageGroup.Length == 0) ? productManager.CurrentLoadedProduct.TableName : (productManager.CurrentLoadedProduct.TableName + Definition.subTableIdentifier.ToString() + this.strImageGroup);
			int.TryParse(this.parHandling.ReadPar(Definition.filterMainPath + this.strImageGroup + ObjectCounter.currentCountSubPath), out this.currentCount);
			this.dbConnection.GetParameterValue(Definition.filterMainPath + this.strImageGroup + ObjectCounter.highThresholdSubPath, tableName, ref text);
			int.TryParse(text, out this.highThreshold);
			this.dbConnection.GetParameterValue(Definition.filterMainPath + this.strImageGroup + ObjectCounter.lowThresholdSubPath, tableName, ref text);
			int.TryParse(text, out this.lowThreshold);
			this.dbConnection.GetParameterValue(Definition.filterMainPath + this.strImageGroup + ObjectCounter.maxCountSubPath, tableName, ref text);
			int.TryParse(text, out this.maxCount);
			this.dbConnection.GetParameterValue(Definition.filterMainPath + this.strImageGroup + ObjectCounter.minCountSubPath, tableName, ref text);
			int.TryParse(text, out this.minCount);
			this.dbConnection.GetParameterValue(Definition.filterMainPath + this.strImageGroup + ObjectCounter.maxSizeSubPath, tableName, ref text);
			int.TryParse(text, out this.maxSize);
			this.dbConnection.GetParameterValue(Definition.filterMainPath + this.strImageGroup + ObjectCounter.minSizeSubPath, tableName, ref text);
			int.TryParse(text, out this.minSize);
			this.dbConnection.GetParameterValue(Definition.filterMainPath + this.strImageGroup + ObjectCounter.filterSizePath, tableName, ref text);
			int.TryParse(text, out this.filterSize);
			this.dbConnection.GetParameterValue(Definition.filterMainPath + this.strImageGroup + ObjectCounter.unsmaskForContPath, tableName, ref text);
			this.UnmaskForContEnabled = (text == "1");
		}

		// Token: 0x06002B04 RID: 11012 RVA: 0x0011DF4C File Offset: 0x0011C14C
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x04001AD6 RID: 6870
		public static readonly string objectCounterSubPath = "/objectCounter/";

		// Token: 0x04001AD7 RID: 6871
		public static readonly string currentCountSubPath = ObjectCounter.objectCounterSubPath + "CountedObjects/Value";

		// Token: 0x04001AD8 RID: 6872
		public static readonly string minCountSubPath = ObjectCounter.objectCounterSubPath + "MinObjects/Value";

		// Token: 0x04001AD9 RID: 6873
		public static readonly string maxCountSubPath = ObjectCounter.objectCounterSubPath + "MaxObjects/Value";

		// Token: 0x04001ADA RID: 6874
		public static readonly string lowThresholdSubPath = ObjectCounter.objectCounterSubPath + "LowThreshold/Value";

		// Token: 0x04001ADB RID: 6875
		public static readonly string highThresholdSubPath = ObjectCounter.objectCounterSubPath + "HighThreshold/Value";

		// Token: 0x04001ADC RID: 6876
		public static readonly string minSizeSubPath = ObjectCounter.objectCounterSubPath + "MinObjectSize/Value";

		// Token: 0x04001ADD RID: 6877
		public static readonly string maxSizeSubPath = ObjectCounter.objectCounterSubPath + "MaxObjectSize/Value";

		// Token: 0x04001ADE RID: 6878
		public static readonly string enabledSubPath = ObjectCounter.objectCounterSubPath + "Enabled/Value";

		// Token: 0x04001ADF RID: 6879
		public static readonly string autolearnSubPath = ObjectCounter.objectCounterSubPath + "Autolearn/Value";

		// Token: 0x04001AE0 RID: 6880
		public static readonly string minXSubPath = ObjectCounter.objectCounterSubPath + "MinX/Value";

		// Token: 0x04001AE1 RID: 6881
		public static readonly string maxXSubPath = ObjectCounter.objectCounterSubPath + "MaxX/Value";

		// Token: 0x04001AE2 RID: 6882
		public static readonly string minYSubPath = ObjectCounter.objectCounterSubPath + "MinY/Value";

		// Token: 0x04001AE3 RID: 6883
		public static readonly string maxYSubPath = ObjectCounter.objectCounterSubPath + "MaxY/Value";

		// Token: 0x04001AE4 RID: 6884
		public static readonly string debugSubPath = ObjectCounter.objectCounterSubPath + "Debug/Value";

		// Token: 0x04001AE5 RID: 6885
		public static readonly string triggeredSubPath = ObjectCounter.objectCounterSubPath + "Triggered/Value";

		// Token: 0x04001AE6 RID: 6886
		public static readonly string unsmaskForContPath = ObjectCounter.objectCounterSubPath + "UnmaskForCont/Value";

		// Token: 0x04001AE7 RID: 6887
		public static readonly string filterSizePath = ObjectCounter.objectCounterSubPath + "FilterSize/Value";

		// Token: 0x04001AE8 RID: 6888
		private DBConnection dbConnection;

		// Token: 0x04001AE9 RID: 6889
		private ParameterHandling parHandling;

		// Token: 0x04001AEA RID: 6890
		private int highThreshold;

		// Token: 0x04001AEB RID: 6891
		private int lowThreshold;

		// Token: 0x04001AEC RID: 6892
		private int minCount;

		// Token: 0x04001AED RID: 6893
		private int maxCount;

		// Token: 0x04001AEE RID: 6894
		private int currentCount;

		// Token: 0x04001AEF RID: 6895
		private int minSize;

		// Token: 0x04001AF0 RID: 6896
		private int maxSize;

		// Token: 0x04001AF1 RID: 6897
		private string strImageGroup;

		// Token: 0x04001AF2 RID: 6898
		private string additionalLogbookText = "";

		// Token: 0x04001AF3 RID: 6899
		private int filterSize;

		// Token: 0x04001AF4 RID: 6900
		private bool unmaskForContEnabled;

		// Token: 0x04001AF5 RID: 6901
		private bool initialized;
	}
}
