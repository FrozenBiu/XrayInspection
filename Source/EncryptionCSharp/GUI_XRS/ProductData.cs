using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Data;
using DatabaseConnection;
using Definitions;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x02000132 RID: 306
	public class ProductData : INotifyPropertyChanged
	{
		// Token: 0x17000893 RID: 2195
		// (get) Token: 0x06001D6A RID: 7530 RVA: 0x000BFB40 File Offset: 0x000BDD40
		// (set) Token: 0x06001D6B RID: 7531 RVA: 0x000BFB58 File Offset: 0x000BDD58
		public string ProductName
		{
			get
			{
				return this.productName;
			}
			set
			{
				this.productName = value;
				this.OnPropertyChanged("ProductName");
				this.OnPropertyChanged("ProductAndBatchName");
				this.OnPropertyChanged("ProductNoNameAndBatchName");
			}
		}

		// Token: 0x17000894 RID: 2196
		// (get) Token: 0x06001D6C RID: 7532 RVA: 0x000BFB88 File Offset: 0x000BDD88
		// (set) Token: 0x06001D6D RID: 7533 RVA: 0x000BFBA0 File Offset: 0x000BDDA0
		public string CurrentBatchName
		{
			get
			{
				return this.currentBatchName;
			}
			set
			{
				this.currentBatchName = value;
				this.OnPropertyChanged("CurrentBatchName");
				this.OnPropertyChanged("ProductAndBatchName");
				this.OnPropertyChanged("ProductNoNameAndBatchName");
			}
		}

		// Token: 0x17000895 RID: 2197
		// (get) Token: 0x06001D6E RID: 7534 RVA: 0x000BFBD0 File Offset: 0x000BDDD0
		public string ProductAndBatchName
		{
			get
			{
				return (this.currentBatchName != null && this.currentBatchName.Length > 0) ? (this.productName + " (" + this.currentBatchName + ")") : this.productName;
			}
		}

		// Token: 0x17000896 RID: 2198
		// (get) Token: 0x06001D6F RID: 7535 RVA: 0x000BFC1C File Offset: 0x000BDE1C
		public string ProductNoNameAndBatchName
		{
			get
			{
				return this.productNumber + "     " + this.ProductAndBatchName;
			}
		}

		// Token: 0x17000897 RID: 2199
		// (get) Token: 0x06001D70 RID: 7536 RVA: 0x000BFC44 File Offset: 0x000BDE44
		// (set) Token: 0x06001D71 RID: 7537 RVA: 0x000BFC5C File Offset: 0x000BDE5C
		public string ProductNumber
		{
			get
			{
				return this.productNumber;
			}
			set
			{
				this.productNumber = value;
				this.OnPropertyChanged("ProductNumber");
				this.OnPropertyChanged("ProductNoNameAndBatchName");
			}
		}

		// Token: 0x17000898 RID: 2200
		// (get) Token: 0x06001D72 RID: 7538 RVA: 0x000BFC80 File Offset: 0x000BDE80
		// (set) Token: 0x06001D73 RID: 7539 RVA: 0x000BFC98 File Offset: 0x000BDE98
		public ProductType ProductType
		{
			get
			{
				return this.productType;
			}
			set
			{
				this.productType = value;
				this.OnPropertyChanged("ProductType");
				this.OnPropertyChanged("IsMultiProduct");
				this.OnPropertyChanged("IsSingleProduct");
			}
		}

		// Token: 0x17000899 RID: 2201
		// (get) Token: 0x06001D74 RID: 7540 RVA: 0x000BFCC8 File Offset: 0x000BDEC8
		public bool IsMultiProduct
		{
			get
			{
				return this.productType == ProductType.MultiProduct;
			}
		}

		// Token: 0x1700089A RID: 2202
		// (get) Token: 0x06001D75 RID: 7541 RVA: 0x000BFCE4 File Offset: 0x000BDEE4
		public bool IsSingleProduct
		{
			get
			{
				return this.productType == ProductType.SingleProduct;
			}
		}

		// Token: 0x1700089B RID: 2203
		// (get) Token: 0x06001D76 RID: 7542 RVA: 0x000BFD00 File Offset: 0x000BDF00
		public bool IsLaneProduct
		{
			get
			{
				return this.productType == ProductType.LaneProduct;
			}
		}

		// Token: 0x1700089C RID: 2204
		// (get) Token: 0x06001D77 RID: 7543 RVA: 0x000BFD1C File Offset: 0x000BDF1C
		public bool IsRegionProduct
		{
			get
			{
				return this.productType == ProductType.RegionProduct;
			}
		}

		// Token: 0x1700089D RID: 2205
		// (get) Token: 0x06001D78 RID: 7544 RVA: 0x000BFD38 File Offset: 0x000BDF38
		public bool IsBulkObjectProduct
		{
			get
			{
				return this.productType == ProductType.BulkObjectProduct;
			}
		}

		// Token: 0x1700089E RID: 2206
		// (get) Token: 0x06001D79 RID: 7545 RVA: 0x000BFD54 File Offset: 0x000BDF54
		// (set) Token: 0x06001D7A RID: 7546 RVA: 0x000BFD6C File Offset: 0x000BDF6C
		public string TableName
		{
			get
			{
				return this.tableName;
			}
			set
			{
				this.tableName = value;
			}
		}

		// Token: 0x1700089F RID: 2207
		// (get) Token: 0x06001D7B RID: 7547 RVA: 0x000BFD78 File Offset: 0x000BDF78
		public ObservableCollection<ProductData> SubProducts
		{
			get
			{
				return this.subProducts;
			}
		}

		// Token: 0x14000097 RID: 151
		// (add) Token: 0x06001D7C RID: 7548 RVA: 0x000BFD90 File Offset: 0x000BDF90
		// (remove) Token: 0x06001D7D RID: 7549 RVA: 0x000BFDC8 File Offset: 0x000BDFC8
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x170008A0 RID: 2208
		// (get) Token: 0x06001D7E RID: 7550 RVA: 0x000BFDFD File Offset: 0x000BDFFD
		// (set) Token: 0x06001D7F RID: 7551 RVA: 0x000BFE05 File Offset: 0x000BE005
		public List<QualityParameter> QAParameters { get; set; }

		// Token: 0x170008A1 RID: 2209
		// (get) Token: 0x06001D80 RID: 7552 RVA: 0x000BFE10 File Offset: 0x000BE010
		// (set) Token: 0x06001D81 RID: 7553 RVA: 0x000BFE28 File Offset: 0x000BE028
		public Lane Lane
		{
			get
			{
				return this.lane;
			}
			set
			{
				this.lane = value;
				this.OnPropertyChanged("Lane");
			}
		}

		// Token: 0x170008A2 RID: 2210
		// (get) Token: 0x06001D82 RID: 7554 RVA: 0x000BFE40 File Offset: 0x000BE040
		// (set) Token: 0x06001D83 RID: 7555 RVA: 0x000BFE58 File Offset: 0x000BE058
		public bool LaneActive
		{
			get
			{
				return this.laneActive;
			}
			set
			{
				this.laneActive = value;
				this.OnPropertyChanged("LaneActive");
			}
		}

		// Token: 0x170008A3 RID: 2211
		// (get) Token: 0x06001D84 RID: 7556 RVA: 0x000BFE70 File Offset: 0x000BE070
		// (set) Token: 0x06001D85 RID: 7557 RVA: 0x000BFE88 File Offset: 0x000BE088
		public byte Group
		{
			get
			{
				return this.group;
			}
			set
			{
				this.group = value;
				this.OnPropertyChanged("Group");
			}
		}

		// Token: 0x170008A4 RID: 2212
		// (get) Token: 0x06001D86 RID: 7558 RVA: 0x000BFEA0 File Offset: 0x000BE0A0
		// (set) Token: 0x06001D87 RID: 7559 RVA: 0x000BFEB8 File Offset: 0x000BE0B8
		public XRSRegion Region
		{
			get
			{
				return this.region;
			}
			set
			{
				this.region = value;
				this.OnPropertyChanged("Region");
				this.OnPropertyChanged("IsRectangleRegion");
				this.OnPropertyChanged("IsPolygonalRegion");
				this.OnPropertyChanged("IsEllipticRegion");
			}
		}

		// Token: 0x170008A5 RID: 2213
		// (get) Token: 0x06001D88 RID: 7560 RVA: 0x000BFEF4 File Offset: 0x000BE0F4
		public bool ProductAssigned
		{
			get
			{
				return this.productNumber != null && this.productNumber.Length > 0 && this.productName != null && this.productName.Length > 0;
			}
		}

		// Token: 0x170008A6 RID: 2214
		// (get) Token: 0x06001D89 RID: 7561 RVA: 0x000BFF38 File Offset: 0x000BE138
		public int SubProductIndex
		{
			get
			{
				return (int)(this.group - 1);
			}
		}

		// Token: 0x170008A7 RID: 2215
		// (get) Token: 0x06001D8A RID: 7562 RVA: 0x000BFF54 File Offset: 0x000BE154
		public bool IsRectangleRegion
		{
			get
			{
				return this.region.Type == XRSRegionType.Rectangle;
			}
		}

		// Token: 0x170008A8 RID: 2216
		// (get) Token: 0x06001D8B RID: 7563 RVA: 0x000BFF74 File Offset: 0x000BE174
		public bool IsPolygonalRegion
		{
			get
			{
				return this.region.Type == XRSRegionType.Polygon;
			}
		}

		// Token: 0x170008A9 RID: 2217
		// (get) Token: 0x06001D8C RID: 7564 RVA: 0x000BFF94 File Offset: 0x000BE194
		public bool IsEllipticRegion
		{
			get
			{
				return this.region.Type == XRSRegionType.Ellipse;
			}
		}

		// Token: 0x170008AA RID: 2218
		// (get) Token: 0x06001D8D RID: 7565 RVA: 0x000BFFB4 File Offset: 0x000BE1B4
		// (set) Token: 0x06001D8E RID: 7566 RVA: 0x000BFFCC File Offset: 0x000BE1CC
		public bool IsDefaultSubProduct
		{
			get
			{
				return this.isDefaultSubProduct;
			}
			set
			{
				this.isDefaultSubProduct = value;
				this.OnPropertyChanged("IsDefaultSubProduct");
			}
		}

		// Token: 0x06001D8F RID: 7567 RVA: 0x000BFFE2 File Offset: 0x000BE1E2
		public ProductData() : this("", "", "")
		{
		}

		// Token: 0x06001D90 RID: 7568 RVA: 0x000BFFFB File Offset: 0x000BE1FB
		public ProductData(string tableName, string productName, string productNumber) : this(tableName, productName, productNumber, ProductType.SingleProduct, null)
		{
		}

		// Token: 0x06001D91 RID: 7569 RVA: 0x000C000C File Offset: 0x000BE20C
		public ProductData(string tableName, string productName, string productNumber, ProductType productType, DBConnection dbConnection)
		{
			this.tableName = tableName;
			this.productName = productName;
			this.productNumber = productNumber;
			this.productType = productType;
			this.subProducts = new ObservableCollection<ProductData>();
			this.QAParameters = new List<QualityParameter>();
			this.GetGroupFromTableName();
			bool flag = dbConnection != null;
			if (flag)
			{
				this.SetCurrentBatchName(dbConnection);
				bool flag2 = this.group > 0;
				if (flag2)
				{
					this.GetRegion(dbConnection);
				}
				bool flag3 = productType > ProductType.SingleProduct;
				if (flag3)
				{
					this.CreateSubProductList(dbConnection);
				}
			}
			this.lane = Lane.All;
			this.laneActive = true;
		}

		// Token: 0x06001D92 RID: 7570 RVA: 0x000C00B4 File Offset: 0x000BE2B4
		public void SetCurrentBatchName(DBConnection dbConnection)
		{
			bool flag = false;
			dbConnection.GetBatchesEnabled(ref flag);
			bool flag2 = flag;
			if (flag2)
			{
				string text = null;
				dbConnection.GetCurrentBatchID(ref text, this.tableName);
				this.CurrentBatchName = text;
			}
			else
			{
				this.CurrentBatchName = null;
			}
		}

		// Token: 0x06001D93 RID: 7571 RVA: 0x000C00F8 File Offset: 0x000BE2F8
		private void CreateSubProductList(DBConnection dbConnection)
		{
			List<string> list = new List<string>();
			string text = null;
			string text2 = "";
			string s = "";
			int num = -1;
			dbConnection.GetSubProductTablenames(this.tableName, list);
			this.subProducts.Clear();
			dbConnection.GetParameterValue(Definition.recognitionDefaultGroupPath, this.tableName, ref s);
			int.TryParse(s, out num);
			foreach (string text3 in list)
			{
				dbConnection.GetProductName(text3, ref text);
				dbConnection.GetProductNumber(text3, ref text2);
				ProductData productData = new ProductData(text3, text, text2, ProductType.SingleProduct, dbConnection);
				bool flag = this.productType == ProductType.LaneProduct;
				if (flag)
				{
					this.SetLaneProductProperties(productData, dbConnection);
				}
				bool flag2 = this.productType == ProductType.MultiProduct;
				if (flag2)
				{
					bool flag3 = num == (int)productData.group;
					if (flag3)
					{
						productData.IsDefaultSubProduct = true;
					}
				}
				this.subProducts.Add(productData);
			}
			bool flag4 = this.productType != ProductType.LaneProduct;
			if (flag4)
			{
				ListCollectionView listCollectionView = (ListCollectionView)CollectionViewSource.GetDefaultView(this.subProducts);
				bool flag5 = listCollectionView != null;
				if (flag5)
				{
					listCollectionView.CustomSort = new ProductDataComparer();
					listCollectionView.Refresh();
				}
			}
		}

		// Token: 0x06001D94 RID: 7572 RVA: 0x000C0258 File Offset: 0x000BE458
		private void SetLaneProductProperties(ProductData currentSubProduct, DBConnection dbConnection)
		{
			string s = "";
			int num = 0;
			int num2 = 0;
			string s2 = currentSubProduct.tableName.Remove(0, currentSubProduct.TableName.IndexOf(Definition.subTableIdentifier) + 1);
			int.TryParse(s2, out num);
			dbConnection.GetParameterValue(Definition.filterMainPath + num.ToString() + Definition.propertiesSubPath + "/Lane/Value", currentSubProduct.tableName, ref s);
			bool flag = int.TryParse(s, out num2);
			if (flag)
			{
				currentSubProduct.lane = (Lane)num2;
			}
			currentSubProduct.SetLaneActiveFromDB(this.tableName, dbConnection);
		}

		// Token: 0x06001D95 RID: 7573 RVA: 0x000C02EC File Offset: 0x000BE4EC
		public void SetLaneActiveFromDB(string mainTableName, DBConnection dbConnection)
		{
			string a = "";
			string path;
			switch (this.lane)
			{
			default:
				path = Acquisition.lane1EnablePath;
				break;
			case Lane.Lane2:
				path = Acquisition.lane2EnablePath;
				break;
			case Lane.Lane3:
				path = Acquisition.lane3EnablePath;
				break;
			case Lane.Lane4:
				path = Acquisition.lane4EnablePath;
				break;
			}
			dbConnection.GetParameterValue(path, mainTableName, ref a);
			bool flag = a != "1";
			if (flag)
			{
				this.laneActive = false;
			}
		}

		// Token: 0x06001D96 RID: 7574 RVA: 0x000C036C File Offset: 0x000BE56C
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x06001D97 RID: 7575 RVA: 0x000C039C File Offset: 0x000BE59C
		public void ResetQAValues(bool resetCurrentValues)
		{
			bool flag = this.QAParameters != null;
			if (flag)
			{
				foreach (QualityParameter qualityParameter in this.QAParameters)
				{
					int i = 0;
					bool flag2 = !resetCurrentValues;
					if (flag2)
					{
						i++;
					}
					while (i < qualityParameter.Values.Length)
					{
						qualityParameter.Values[i] = null;
						i++;
					}
				}
			}
		}

		// Token: 0x06001D98 RID: 7576 RVA: 0x000C0430 File Offset: 0x000BE630
		private void GetGroupFromTableName()
		{
			string s = this.tableName.Remove(0, this.tableName.IndexOf(Definition.subTableIdentifier) + 1);
			byte.TryParse(s, out this.group);
		}

		// Token: 0x06001D99 RID: 7577 RVA: 0x000C046C File Offset: 0x000BE66C
		public void GetRegion(DBConnection dbConnection)
		{
			string value = null;
			string s = null;
			string s2 = null;
			string s3 = null;
			string s4 = null;
			string s5 = null;
			string s6 = null;
			string s7 = null;
			string s8 = null;
			string s9 = null;
			string s10 = null;
			string s11 = null;
			string s12 = null;
			string s13 = null;
			string s14 = null;
			string s15 = null;
			string s16 = null;
			string s17 = null;
			string s18 = null;
			string s19 = null;
			string s20 = null;
			dbConnection.GetParameterValue(Definition.filterMainPath + this.group.ToString() + Definition.regionTypeSubPath, this.TableName, ref value);
			dbConnection.GetParameterValue(Definition.filterMainPath + this.group.ToString() + Definition.vertex1XSubPath, this.TableName, ref s);
			dbConnection.GetParameterValue(Definition.filterMainPath + this.group.ToString() + Definition.vertex1YSubPath, this.TableName, ref s2);
			dbConnection.GetParameterValue(Definition.filterMainPath + this.group.ToString() + Definition.vertex2XSubPath, this.TableName, ref s3);
			dbConnection.GetParameterValue(Definition.filterMainPath + this.group.ToString() + Definition.vertex2YSubPath, this.TableName, ref s4);
			dbConnection.GetParameterValue(Definition.filterMainPath + this.group.ToString() + Definition.vertex3XSubPath, this.TableName, ref s5);
			dbConnection.GetParameterValue(Definition.filterMainPath + this.group.ToString() + Definition.vertex3YSubPath, this.TableName, ref s6);
			dbConnection.GetParameterValue(Definition.filterMainPath + this.group.ToString() + Definition.vertex4XSubPath, this.TableName, ref s7);
			dbConnection.GetParameterValue(Definition.filterMainPath + this.group.ToString() + Definition.vertex4YSubPath, this.TableName, ref s8);
			dbConnection.GetParameterValue(Definition.filterMainPath + this.group.ToString() + Definition.vertex5XSubPath, this.TableName, ref s9);
			dbConnection.GetParameterValue(Definition.filterMainPath + this.group.ToString() + Definition.vertex5YSubPath, this.TableName, ref s10);
			dbConnection.GetParameterValue(Definition.filterMainPath + this.group.ToString() + Definition.vertex6XSubPath, this.TableName, ref s11);
			dbConnection.GetParameterValue(Definition.filterMainPath + this.group.ToString() + Definition.vertex6YSubPath, this.TableName, ref s12);
			dbConnection.GetParameterValue(Definition.filterMainPath + this.group.ToString() + Definition.vertex7XSubPath, this.TableName, ref s13);
			dbConnection.GetParameterValue(Definition.filterMainPath + this.group.ToString() + Definition.vertex7YSubPath, this.TableName, ref s14);
			dbConnection.GetParameterValue(Definition.filterMainPath + this.group.ToString() + Definition.vertex8XSubPath, this.TableName, ref s15);
			dbConnection.GetParameterValue(Definition.filterMainPath + this.group.ToString() + Definition.vertex8YSubPath, this.TableName, ref s16);
			dbConnection.GetParameterValue(Definition.filterMainPath + this.group.ToString() + Definition.vertex9XSubPath, this.TableName, ref s17);
			dbConnection.GetParameterValue(Definition.filterMainPath + this.group.ToString() + Definition.vertex9YSubPath, this.TableName, ref s18);
			dbConnection.GetParameterValue(Definition.filterMainPath + this.group.ToString() + Definition.vertex10XSubPath, this.TableName, ref s19);
			dbConnection.GetParameterValue(Definition.filterMainPath + this.group.ToString() + Definition.vertex10YSubPath, this.TableName, ref s20);
			try
			{
				this.region.Type = (XRSRegionType)Enum.Parse(typeof(XRSRegionType), value);
				this.region.Vertex1 = new Point(double.Parse(s), double.Parse(s2));
				this.region.Vertex2 = new Point(double.Parse(s3), double.Parse(s4));
				this.region.Vertex3 = new Point(double.Parse(s5), double.Parse(s6));
				this.region.Vertex4 = new Point(double.Parse(s7), double.Parse(s8));
				this.region.Vertex5 = new Point(double.Parse(s9), double.Parse(s10));
				this.region.Vertex6 = new Point(double.Parse(s11), double.Parse(s12));
				this.region.Vertex7 = new Point(double.Parse(s13), double.Parse(s14));
				this.region.Vertex8 = new Point(double.Parse(s15), double.Parse(s16));
				this.region.Vertex9 = new Point(double.Parse(s17), double.Parse(s18));
				this.region.Vertex10 = new Point(double.Parse(s19), double.Parse(s20));
			}
			catch
			{
			}
		}

		// Token: 0x0400136D RID: 4973
		private string tableName;

		// Token: 0x0400136E RID: 4974
		private string productName;

		// Token: 0x0400136F RID: 4975
		private string productNumber;

		// Token: 0x04001370 RID: 4976
		private ProductType productType;

		// Token: 0x04001371 RID: 4977
		private ObservableCollection<ProductData> subProducts;

		// Token: 0x04001372 RID: 4978
		private Lane lane;

		// Token: 0x04001373 RID: 4979
		private bool laneActive;

		// Token: 0x04001374 RID: 4980
		private byte group;

		// Token: 0x04001375 RID: 4981
		private XRSRegion region = new XRSRegion();

		// Token: 0x04001376 RID: 4982
		private string currentBatchName;

		// Token: 0x04001377 RID: 4983
		private bool isDefaultSubProduct;
	}
}
