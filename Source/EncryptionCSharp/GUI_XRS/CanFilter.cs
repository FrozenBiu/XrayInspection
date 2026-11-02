using System;
using System.ComponentModel;
using System.Diagnostics;
using DatabaseConnection;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x020000AA RID: 170
	public class CanFilter : INotifyPropertyChanged
	{
		// Token: 0x14000055 RID: 85
		// (add) Token: 0x060010CD RID: 4301 RVA: 0x00067710 File Offset: 0x00065910
		// (remove) Token: 0x060010CE RID: 4302 RVA: 0x00067748 File Offset: 0x00065948
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x17000514 RID: 1300
		// (get) Token: 0x060010CF RID: 4303 RVA: 0x00067780 File Offset: 0x00065980
		// (set) Token: 0x060010D0 RID: 4304 RVA: 0x00067798 File Offset: 0x00065998
		public int RadiusX
		{
			get
			{
				return this.radiusX;
			}
			set
			{
				this.radiusX = value;
				this.parHandling.WriteValue(Definition.filterMainPath + this.strImageGroup + CanFilter.radiusXSubPath, value.ToString());
				this.OnPropertyChanged("RadiusX");
			}
		}

		// Token: 0x17000515 RID: 1301
		// (get) Token: 0x060010D1 RID: 4305 RVA: 0x000677D8 File Offset: 0x000659D8
		// (set) Token: 0x060010D2 RID: 4306 RVA: 0x000677F0 File Offset: 0x000659F0
		public int RadiusY
		{
			get
			{
				return this.radiusY;
			}
			set
			{
				this.radiusY = value;
				this.parHandling.WriteValue(Definition.filterMainPath + this.strImageGroup + CanFilter.radiusYSubPath, value.ToString());
				this.OnPropertyChanged("RadiusY");
			}
		}

		// Token: 0x17000516 RID: 1302
		// (get) Token: 0x060010D3 RID: 4307 RVA: 0x00067830 File Offset: 0x00065A30
		// (set) Token: 0x060010D4 RID: 4308 RVA: 0x00067848 File Offset: 0x00065A48
		public int CenterX
		{
			get
			{
				return this.centerX;
			}
			set
			{
				this.centerX = value;
				this.parHandling.WriteValue(Definition.filterMainPath + this.strImageGroup + CanFilter.centerXSubPath, value.ToString());
				this.OnPropertyChanged("CenterX");
			}
		}

		// Token: 0x17000517 RID: 1303
		// (get) Token: 0x060010D5 RID: 4309 RVA: 0x00067888 File Offset: 0x00065A88
		// (set) Token: 0x060010D6 RID: 4310 RVA: 0x000678A0 File Offset: 0x00065AA0
		public int CenterY
		{
			get
			{
				return this.centerY;
			}
			set
			{
				this.centerY = value;
				this.parHandling.WriteValue(Definition.filterMainPath + this.strImageGroup + CanFilter.centerYSubPath, value.ToString());
				this.OnPropertyChanged("CenterY");
			}
		}

		// Token: 0x17000518 RID: 1304
		// (get) Token: 0x060010D7 RID: 4311 RVA: 0x000678E0 File Offset: 0x00065AE0
		// (set) Token: 0x060010D8 RID: 4312 RVA: 0x000678F8 File Offset: 0x00065AF8
		public int NPixel
		{
			get
			{
				return this.nPixel;
			}
			set
			{
				this.nPixel = value;
				this.parHandling.WriteValue(Definition.filterMainPath + this.strImageGroup + CanFilter.nPixelSubPath, value.ToString());
				this.OnPropertyChanged("NPixel");
			}
		}

		// Token: 0x060010D9 RID: 4313 RVA: 0x00067938 File Offset: 0x00065B38
		public CanFilter(ParameterHandling parHandling, ImageDataTransfer imageTransfer, DBConnection dbConnection, ProductManager productManager, int imageGroup)
		{
			this.parHandling = parHandling;
			this.dbConnection = dbConnection;
			this.imageTransfer = imageTransfer;
			this.productManager = productManager;
			this.imageGroup = imageGroup;
			this.strImageGroup = ((imageGroup > 0) ? imageGroup.ToString() : "");
			this.GetValuesFromDatabase();
		}

		// Token: 0x060010DA RID: 4314 RVA: 0x00067994 File Offset: 0x00065B94
		private void GetValuesFromDatabase()
		{
			string s = "";
			string tableName = (this.imageGroup <= 0) ? this.productManager.CurrentLoadedProduct.TableName : (this.productManager.CurrentLoadedProduct.TableName + Definition.subTableIdentifier.ToString() + this.strImageGroup);
			bool parameterValue = this.dbConnection.GetParameterValue(Definition.filterMainPath + this.strImageGroup + CanFilter.radiusXSubPath, tableName, ref s);
			if (parameterValue)
			{
				int.TryParse(s, out this.radiusX);
			}
			bool parameterValue2 = this.dbConnection.GetParameterValue(Definition.filterMainPath + this.strImageGroup + CanFilter.radiusYSubPath, tableName, ref s);
			if (parameterValue2)
			{
				int.TryParse(s, out this.radiusY);
			}
			bool parameterValue3 = this.dbConnection.GetParameterValue(Definition.filterMainPath + this.strImageGroup + CanFilter.centerXSubPath, tableName, ref s);
			if (parameterValue3)
			{
				int.TryParse(s, out this.centerX);
			}
			bool parameterValue4 = this.dbConnection.GetParameterValue(Definition.filterMainPath + this.strImageGroup + CanFilter.centerYSubPath, tableName, ref s);
			if (parameterValue4)
			{
				int.TryParse(s, out this.centerY);
			}
			bool parameterValue5 = this.dbConnection.GetParameterValue(Definition.filterMainPath + this.strImageGroup + CanFilter.nPixelSubPath, tableName, ref s);
			if (parameterValue5)
			{
				int.TryParse(s, out this.nPixel);
			}
		}

		// Token: 0x060010DB RID: 4315 RVA: 0x00067B00 File Offset: 0x00065D00
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x04000B2C RID: 2860
		public static readonly string centerXSubPath = "/can/CenterX/Value";

		// Token: 0x04000B2D RID: 2861
		public static readonly string centerYSubPath = "/can/CenterY/Value";

		// Token: 0x04000B2E RID: 2862
		public static readonly string radiusXSubPath = "/can/RadiusX/Value";

		// Token: 0x04000B2F RID: 2863
		public static readonly string radiusYSubPath = "/can/RadiusY/Value";

		// Token: 0x04000B30 RID: 2864
		public static readonly string nPixelSubPath = "/can/NPixel/Value";

		// Token: 0x04000B31 RID: 2865
		public static readonly string debugSubPath = "/can/Debug/Value";

		// Token: 0x04000B32 RID: 2866
		private ParameterHandling parHandling;

		// Token: 0x04000B33 RID: 2867
		private ImageDataTransfer imageTransfer;

		// Token: 0x04000B34 RID: 2868
		private DBConnection dbConnection;

		// Token: 0x04000B35 RID: 2869
		private ProductManager productManager;

		// Token: 0x04000B36 RID: 2870
		private int imageGroup;

		// Token: 0x04000B37 RID: 2871
		private string strImageGroup;

		// Token: 0x04000B38 RID: 2872
		private int nPixel;

		// Token: 0x04000B39 RID: 2873
		private int centerX;

		// Token: 0x04000B3A RID: 2874
		private int centerY;

		// Token: 0x04000B3B RID: 2875
		private int radiusX;

		// Token: 0x04000B3C RID: 2876
		private int radiusY;
	}
}
