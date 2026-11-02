using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using DatabaseConnection;
using Definitions;
using SSTEthernet;

namespace GUI_XRS
{
	// Token: 0x02000142 RID: 322
	public class QAParamsViewModel : INotifyPropertyChanged
	{
		// Token: 0x140000A3 RID: 163
		// (add) Token: 0x06001EB1 RID: 7857 RVA: 0x000C9534 File Offset: 0x000C7734
		// (remove) Token: 0x06001EB2 RID: 7858 RVA: 0x000C956C File Offset: 0x000C776C
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x170008D2 RID: 2258
		// (get) Token: 0x06001EB3 RID: 7859 RVA: 0x000C95A1 File Offset: 0x000C77A1
		// (set) Token: 0x06001EB4 RID: 7860 RVA: 0x000C95A9 File Offset: 0x000C77A9
		public ObservableCollection<ProductData> ParamProducts { get; set; }

		// Token: 0x170008D3 RID: 2259
		// (get) Token: 0x06001EB5 RID: 7861 RVA: 0x000C95B2 File Offset: 0x000C77B2
		// (set) Token: 0x06001EB6 RID: 7862 RVA: 0x000C95BA File Offset: 0x000C77BA
		public ObservableCollection<ChkPointInfo> ChkPointInfos { get; set; }

		// Token: 0x170008D4 RID: 2260
		// (get) Token: 0x06001EB7 RID: 7863 RVA: 0x000C95C4 File Offset: 0x000C77C4
		// (set) Token: 0x06001EB8 RID: 7864 RVA: 0x000C95DC File Offset: 0x000C77DC
		public ChkPointInfo SelectedCheckpoint
		{
			get
			{
				return this.selectedCheckpoint;
			}
			set
			{
				this.selectedCheckpoint = value;
				this.SetChkpointsSelectionChanged(this.ChkPointInfos.IndexOf(value));
				this.OnPropertyChanged("SelectedCheckpoint");
				bool flag = this.selectedCheckpoint != null;
				if (flag)
				{
					this.SetParamStates();
				}
				else
				{
					this.ResetParamStateCurrentValue();
				}
			}
		}

		// Token: 0x170008D5 RID: 2261
		// (get) Token: 0x06001EB9 RID: 7865 RVA: 0x000C9630 File Offset: 0x000C7830
		// (set) Token: 0x06001EBA RID: 7866 RVA: 0x000C9648 File Offset: 0x000C7848
		public ProductData SelectedProduct
		{
			get
			{
				return this.selectedProduct;
			}
			set
			{
				bool flag = value != null && value.IsSingleProduct && value.ProductNumber != "0";
				if (flag)
				{
					this.selectedProduct = value;
					this.OnPropertyChanged("SelectedProduct");
					this.SetChkpointsAvailable();
					this.SetChkpointsAreChanged();
				}
			}
		}

		// Token: 0x170008D6 RID: 2262
		// (get) Token: 0x06001EBB RID: 7867 RVA: 0x000C969C File Offset: 0x000C789C
		// (set) Token: 0x06001EBC RID: 7868 RVA: 0x000C96B4 File Offset: 0x000C78B4
		public bool IsCheckpointAvailable
		{
			get
			{
				return this.isCheckpointAvailable;
			}
			set
			{
				this.isCheckpointAvailable = value;
				this.OnPropertyChanged("IsCheckpointAvailable");
			}
		}

		// Token: 0x170008D7 RID: 2263
		// (get) Token: 0x06001EBD RID: 7869 RVA: 0x000C96CA File Offset: 0x000C78CA
		// (set) Token: 0x06001EBE RID: 7870 RVA: 0x000C96D2 File Offset: 0x000C78D2
		public BoundLocalizedText StrCurrentValues { get; set; }

		// Token: 0x170008D8 RID: 2264
		// (get) Token: 0x06001EBF RID: 7871 RVA: 0x000C96DB File Offset: 0x000C78DB
		// (set) Token: 0x06001EC0 RID: 7872 RVA: 0x000C96E3 File Offset: 0x000C78E3
		public BoundLocalizedText StrDifferent { get; set; }

		// Token: 0x06001EC1 RID: 7873 RVA: 0x000C96EC File Offset: 0x000C78EC
		public QAParamsViewModel(DBConnection dbConnection, ParameterHandling parHandl, AccessRightsManager accessManager, bool showProgress, Conveyor conveyor, ProductManager productM, MachineState machine)
		{
			QAParamsViewModel <>4__this = this;
			bool showProgress2 = showProgress;
			if (showProgress2)
			{
				Progress.ShowStaticProgress(LocalizationManager.Localize("txtProcessing"), false);
			}
			this.dbConnection = dbConnection;
			this.parHandling = parHandl;
			this.accessManager = accessManager;
			this.conveyor = conveyor;
			this.productManager = productM;
			this.machineState = machine;
			MachineType machineType = MachineType.MachineTypeUndefined;
			dbConnection.GetMachineType(ref machineType);
			bool flag = machineType == MachineType.RayconEntry;
			if (flag)
			{
				this.isEX1 = true;
			}
			this.StrCurrentValues = new BoundLocalizedText("txtCurrentValues");
			this.StrDifferent = new BoundLocalizedText("txtDifferent");
			this.ParamProducts = new ObservableCollection<ProductData>();
			bool flag2 = this.parHandling != null;
			if (flag2)
			{
				string a = this.parHandling.ReadPar(Definition.imgprocarchitecture);
				this.isAIMachine = (a == "1");
			}
			else
			{
				this.isAIMachine = false;
			}
			this.InitializePaths();
			this.InitializesChkPointHeader();
			Action <>9__2;
			new Thread(delegate()
			{
				<>4__this.GetCurrentParameters();
				<>4__this.GetChkPointsData();
				bool flag3 = <>4__this.ParamProducts != null && <>4__this.ParamProducts.Count > 0;
				if (flag3)
				{
					bool flag4 = <>4__this.productManager != null;
					if (flag4)
					{
						int num = -1;
						bool isSingleProduct = <>4__this.productManager.CurrentLoadedProduct.IsSingleProduct;
						if (isSingleProduct)
						{
							for (int i = 0; i < <>4__this.ParamProducts.Count; i++)
							{
								bool flag5 = <>4__this.productManager.CurrentLoadedProduct.ProductNumber == <>4__this.ParamProducts[i].ProductNumber && <>4__this.productManager.CurrentLoadedProduct.ProductName == <>4__this.ParamProducts[i].ProductName;
								if (flag5)
								{
									num = i;
									break;
								}
							}
						}
						bool flag6 = num >= 0;
						if (flag6)
						{
							<>4__this.SelectedProduct = <>4__this.ParamProducts[num];
						}
						else
						{
							<>4__this.SelectedProduct = <>4__this.ParamProducts[0];
						}
					}
					else
					{
						<>4__this.SelectedProduct = <>4__this.ParamProducts[0];
					}
				}
				bool showProgress3 = showProgress;
				if (showProgress3)
				{
					Progress.CloseStaticProgress(false);
					List<ChkPointInfo> list = (from info in <>4__this.ChkPointInfos
					where info.Available
					select info).ToList<ChkPointInfo>();
					bool flag7 = list.Count <= 0;
					if (flag7)
					{
						Dispatcher dispatcher = Application.Current.Dispatcher;
						DispatcherPriority priority = DispatcherPriority.Normal;
						Action method;
						if ((method = <>9__2) == null)
						{
							method = (<>9__2 = delegate()
							{
								bool flag8 = Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("txtNoCheckpointAvailable"), DialogStyle.Info, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
								if (flag8)
								{
									<>4__this.CreateCheckpoint();
								}
							});
						}
						dispatcher.Invoke(priority, method);
					}
					<>4__this.SelectedProduct = <>4__this.SelectedProduct;
				}
			})
			{
				Name = "ParamCompare"
			}.Start();
		}

		// Token: 0x06001EC2 RID: 7874 RVA: 0x000C9850 File Offset: 0x000C7A50
		private void InitializePaths()
		{
			bool flag = !this.isEX1;
			if (flag)
			{
				this.filterPaths.Add(new PathsWithAbbreviation("C", QAParamsViewModel.paramQASubPathsComparer, "Comparer", "Main"));
			}
			else
			{
				this.filterPaths.Add(new PathsWithAbbreviation("OF", QAParamsViewModel.paramQASubPathsObjectFinder, "ObjectFinder", "Main"));
			}
			this.filterPaths.Add(new PathsWithAbbreviation("Th", QAParamsViewModel.paramQASubPathsThreshold, "Thresholds", "Major"));
			this.filterPaths.Add(new PathsWithAbbreviation("Ed", QAParamsViewModel.paramQASubPathsEdge, "Edge", "Additional"));
			this.filterPaths.Add(new PathsWithAbbreviation("Cr", QAParamsViewModel.paramQASubPathsCropping, "Cropping", "Additional"));
			this.filterPaths.Add(new PathsWithAbbreviation("We", QAParamsViewModel.paramQASubPathsWeight, "Weight", "Additional"));
			this.filterPaths.Add(new PathsWithAbbreviation("Co", QAParamsViewModel.paramQASubPathsCount, "Count", "Additional"));
			this.filterPaths.Add(new PathsWithAbbreviation("CC", QAParamsViewModel.paramQASubPathsCountContaminations, "CountContaminations", "Additional"));
			this.filterPaths.Add(new PathsWithAbbreviation("S", QAParamsViewModel.paramQASubPathsSize, "Size", "Additional"));
			this.filterPaths.Add(new PathsWithAbbreviation("Ca", QAParamsViewModel.paramQASubPathsCan, "Can", "Other"));
			this.filterPaths.Add(new PathsWithAbbreviation("Ro", QAParamsViewModel.paramQASubPathsRotate, "Rotate", "Other"));
			this.filterPaths.Add(new PathsWithAbbreviation("Di", QAParamsViewModel.paramQASubPathsDimension, "Dimension", "Other"));
			this.filterPaths.Add(new PathsWithAbbreviation("Lo", QAParamsViewModel.paramQASubPathsLowDensity, "LowDensity", "Other"));
			this.filterPaths.Add(new PathsWithAbbreviation("Re", QAParamsViewModel.paramQASubPathsRecognition, "Recognition", "Other"));
			this.filterPaths.Add(new PathsWithAbbreviation("A", QAParamsViewModel.paramQASubPathsAcquisition, "Acquisition", "Acquisition"));
			this.filterPaths.Add(new PathsWithAbbreviation("La", QAParamsViewModel.paramQASubPathsLane, "Lane", "Lanes"));
			this.otherPaths.Add(new PathsWithAbbreviation("RB", QAParamsViewModel.paramQASubPathsRejecterBulk, "BulkRejecter", "Rejecter"));
			this.otherPaths.Add(new PathsWithAbbreviation("Re", QAParamsViewModel.paramQASubPathsRejecter, "Rejecter", "Rejecter"));
			this.otherPaths.Add(new PathsWithAbbreviation("Bu", QAParamsViewModel.paramQASubPathsRejecterBuzzer, "Buzzer", "Rejecter"));
			this.otherPaths.Add(new PathsWithAbbreviation("Com", QAParamsViewModel.paramQASubPathsCommon, "Common", "Common"));
			this.otherPaths.Add(new PathsWithAbbreviation("W", QAParamsViewModel.paramQASubPathsWeight2, "Weight 2", "Additional 2"));
			this.pathsForAI.Add(new PathsWithAbbreviation("AI", QAParamsViewModel.paramQASubPathsAIParameters, "AI", "AI"));
		}

		// Token: 0x06001EC3 RID: 7875 RVA: 0x000C9B9C File Offset: 0x000C7D9C
		public void CleanUp()
		{
			MainWindow.localizationManager.Deregister(this.StrCurrentValues);
			MainWindow.localizationManager.Deregister(this.StrDifferent);
			bool flag = this.ParamProducts != null;
			if (flag)
			{
				this.ParamProducts.Clear();
				this.ParamProducts = null;
			}
			bool flag2 = this.ChkPointInfos != null;
			if (flag2)
			{
				this.ChkPointInfos.Clear();
				this.ChkPointInfos = null;
			}
			bool flag3 = this.otherPaths != null;
			if (flag3)
			{
				this.otherPaths.Clear();
				this.otherPaths = null;
			}
			bool flag4 = this.pathsForAI != null;
			if (flag4)
			{
				this.pathsForAI.Clear();
				this.pathsForAI = null;
			}
			bool flag5 = this.filterPaths != null;
			if (flag5)
			{
				this.filterPaths.Clear();
				this.filterPaths = null;
			}
		}

		// Token: 0x06001EC4 RID: 7876 RVA: 0x000C9C7C File Offset: 0x000C7E7C
		private void GetCurrentParameters()
		{
			QAParamsViewModel.<>c__DisplayClass74_0 CS$<>8__locals1 = new QAParamsViewModel.<>c__DisplayClass74_0();
			CS$<>8__locals1.<>4__this = this;
			List<QualityParameter> list = new List<QualityParameter>();
			CS$<>8__locals1.lstProducts = new List<ProductData>();
			string productName = null;
			string productNumber = null;
			string text = "";
			ProductType productType = ProductType.SingleProduct;
			string text2 = "";
			List<string> list2 = new List<string>();
			this.dbConnection.GetMainProductTableNames(list2);
			foreach (string text3 in list2)
			{
				this.dbConnection.GetProductNumber(text3, ref text2);
				this.dbConnection.GetProductType(text3, ref productType);
				bool flag = text3 != DBConnection.defaultApplication && productType == ProductType.SingleProduct && !text2.Equals("-1");
				if (flag)
				{
					foreach (PathsWithAbbreviation pathsWithAbbreviation in this.filterPaths)
					{
						string[] paths = pathsWithAbbreviation.paths;
						for (int k = 0; k < paths.Length; k++)
						{
							text = "";
							string path = Definition.filterMainPath + paths[k];
							QualityParameter qualityParameter = new QualityParameter(path, pathsWithAbbreviation.abbreviation + " " + k.ToString(), pathsWithAbbreviation.type);
							this.dbConnection.GetParameterValue(path, text3, ref text);
							qualityParameter.Values[0] = text;
							list.Add(qualityParameter);
						}
					}
					foreach (PathsWithAbbreviation pathsWithAbbreviation2 in this.otherPaths)
					{
						string[] paths2 = pathsWithAbbreviation2.paths;
						for (int j = 0; j < paths2.Length; j++)
						{
							text = "";
							QualityParameter qualityParameter = new QualityParameter(paths2[j], pathsWithAbbreviation2.abbreviation + " " + j.ToString(), pathsWithAbbreviation2.type);
							this.dbConnection.GetParameterValue(paths2[j], text3, ref text);
							qualityParameter.Values[0] = text;
							list.Add(qualityParameter);
						}
					}
					this.dbConnection.GetProductName(text3, ref productName);
					this.dbConnection.GetProductNumber(text3, ref productNumber);
					ProductData productData = new ProductData(text3, productName, productNumber, productType, this.dbConnection);
					CS$<>8__locals1.lstProducts.Add(productData);
					productData.QAParameters.AddRange(list);
					list.Clear();
				}
			}
			CS$<>8__locals1.lstProducts.Sort(new ProductDataComparerGeneric());
			int i;
			int i2;
			for (i = 0; i < CS$<>8__locals1.lstProducts.Count; i = i2 + 1)
			{
				Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
				{
					CS$<>8__locals1.<>4__this.ParamProducts.Add(CS$<>8__locals1.lstProducts[i]);
				}));
				i2 = i;
			}
			Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
			{
				CS$<>8__locals1.<>4__this.ParamProducts.Add(new ProductData("", "", "0"));
			}));
		}

		// Token: 0x06001EC5 RID: 7877 RVA: 0x000CA010 File Offset: 0x000C8210
		private void InitializesChkPointHeader()
		{
			this.ChkPointInfos = new ObservableCollection<ChkPointInfo>();
			DateTime minValue = DateTime.MinValue;
			string username = "";
			for (int i = 0; i < Definition.nQACheckpoints; i++)
			{
				this.dbConnection.GetChkPointData(i, ref minValue, ref username);
				this.ChkPointInfos.Add(new ChkPointInfo(i, minValue, username));
			}
		}

		// Token: 0x06001EC6 RID: 7878 RVA: 0x000CA074 File Offset: 0x000C8274
		private void SetChkpointsAvailable()
		{
			int num = 1;
			bool flag = this.ChkPointInfos != null && this.SelectedProduct != null;
			if (flag)
			{
				for (int i = 0; i < this.ChkPointInfos.Count; i++)
				{
					bool available = false;
					for (int j = 0; j < this.SelectedProduct.QAParameters.Count; j++)
					{
						bool flag2 = this.SelectedProduct.QAParameters[j].Values[i + 1] != null;
						if (flag2)
						{
							available = true;
							this.IsCheckpointAvailable = true;
							bool flag3 = this.SelectedCheckpoint == null;
							if (flag3)
							{
								this.SelectedCheckpoint = this.ChkPointInfos[i];
							}
							this.ChkPointInfos[i].Name = "Checkpoint " + num.ToString();
							num++;
							break;
						}
					}
					this.ChkPointInfos[i].Available = available;
				}
				bool flag4 = this.SelectedCheckpoint != null && !this.SelectedCheckpoint.Available;
				if (flag4)
				{
					for (int k = 0; k < this.ChkPointInfos.Count; k++)
					{
						bool available2 = this.ChkPointInfos[k].Available;
						if (available2)
						{
							this.SelectedCheckpoint = this.ChkPointInfos[k];
							break;
						}
					}
					bool flag5 = !this.SelectedCheckpoint.Available;
					if (flag5)
					{
						this.IsCheckpointAvailable = false;
						this.SelectedCheckpoint = null;
					}
				}
			}
		}

		// Token: 0x06001EC7 RID: 7879 RVA: 0x000CA220 File Offset: 0x000C8420
		private void SetChkpointsSelectionChanged(int i)
		{
			bool flag = i >= 0;
			if (flag)
			{
				for (int j = 0; j < this.ChkPointInfos.Count; j++)
				{
					this.ChkPointInfos[j].IsSelected = false;
				}
				this.ChkPointInfos[i].IsSelected = true;
			}
		}

		// Token: 0x06001EC8 RID: 7880 RVA: 0x000CA280 File Offset: 0x000C8480
		private void GetChkPointsData()
		{
			QAParamsViewModel.<>c__DisplayClass78_0 CS$<>8__locals1 = new QAParamsViewModel.<>c__DisplayClass78_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.paths = new List<string>();
			List<string> list = new List<string>();
			CS$<>8__locals1.productNames = new List<string>();
			CS$<>8__locals1.productNumbers = new List<string>();
			CS$<>8__locals1.infosTemp = new List<ChkPointInfo>();
			CS$<>8__locals1.infosTemp.AddRange(this.ChkPointInfos);
			CS$<>8__locals1.infosTemp.Sort((ChkPointInfo chk1, ChkPointInfo chk2) => chk2.Date.CompareTo(chk1.Date));
			int i;
			int num;
			for (i = 0; i < Definition.nQACheckpoints; i = num + 1)
			{
				Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
				{
					CS$<>8__locals1.<>4__this.ChkPointInfos[i] = CS$<>8__locals1.infosTemp[i];
				}));
				this.ChkPointInfos[i].Name = "Checkpoint " + (i + 1).ToString();
				this.ChkPointInfos[i].LstIndex = i;
				num = i;
			}
			CS$<>8__locals1.infosTemp.Clear();
			foreach (ProductData productData in this.ParamProducts)
			{
				bool flag = productData.ProductNumber != "0";
				if (flag)
				{
					productData.ResetQAValues(false);
				}
			}
			for (int k = 0; k < this.ChkPointInfos.Count; k++)
			{
				this.dbConnection.GetChkPointData(this.ChkPointInfos[k].ChkIndex, CS$<>8__locals1.paths, list, CS$<>8__locals1.productNames, CS$<>8__locals1.productNumbers);
				bool flag2 = list.Count > 0;
				if (flag2)
				{
					int j;
					for (j = 0; j < CS$<>8__locals1.paths.Count; j = num + 1)
					{
						List<ProductData> list2 = (from product in this.ParamProducts
						where product.ProductName == CS$<>8__locals1.productNames[j] && product.ProductNumber == CS$<>8__locals1.productNumbers[j]
						select product).ToList<ProductData>();
						bool flag3 = list2.Count > 0;
						if (flag3)
						{
							List<QualityParameter> list3 = (from param in list2[0].QAParameters
							where param.Path == CS$<>8__locals1.paths[j]
							select param).ToList<QualityParameter>();
							bool flag4 = list3.Count > 0;
							if (flag4)
							{
								list3[0].Values[k + 1] = list[j];
							}
						}
						num = j;
					}
				}
				CS$<>8__locals1.paths.Clear();
				list.Clear();
				CS$<>8__locals1.productNames.Clear();
				CS$<>8__locals1.productNumbers.Clear();
			}
			this.OnPropertyChanged("ChkPointInfos");
		}

		// Token: 0x06001EC9 RID: 7881 RVA: 0x000CA5A4 File Offset: 0x000C87A4
		private void SetParamStates()
		{
			for (int i = 0; i < this.ParamProducts.Count; i++)
			{
				bool flag = this.ParamProducts[i].ProductNumber != "0";
				if (flag)
				{
					for (int j = 0; j < this.ParamProducts[i].QAParameters.Count; j++)
					{
						this.ParamProducts[i].QAParameters[j].SetParamStates(this.SelectedCheckpoint.LstIndex);
					}
				}
			}
		}

		// Token: 0x06001ECA RID: 7882 RVA: 0x000CA644 File Offset: 0x000C8844
		private void ResetParamStateCurrentValue()
		{
			bool flag = this.ParamProducts != null;
			if (flag)
			{
				for (int i = 0; i < this.ParamProducts.Count; i++)
				{
					bool flag2 = this.ParamProducts[i].ProductNumber != "0";
					if (flag2)
					{
						for (int j = 0; j < this.ParamProducts[i].QAParameters.Count; j++)
						{
							this.ParamProducts[i].QAParameters[j].ParamStates[0] = ParamState.Neutral;
						}
					}
				}
			}
		}

		// Token: 0x06001ECB RID: 7883 RVA: 0x000CA6F0 File Offset: 0x000C88F0
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x06001ECC RID: 7884 RVA: 0x000CA720 File Offset: 0x000C8920
		public void CreateCheckpoint()
		{
			ChkPointInfo chkPointInfo = null;
			DateTime now = DateTime.Now;
			bool flag = true;
			bool flag2 = true;
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			List<string> list3 = new List<string>();
			List<string> list4 = new List<string>();
			chkPointInfo = this.GetOldestChkPoint();
			for (int i = 0; i < this.ChkPointInfos.Count; i++)
			{
				bool flag3 = !this.ChkPointInfos[i].Available;
				if (flag3)
				{
					flag = false;
					break;
				}
			}
			bool flag4 = this.AllChkpointsOccupiedForAnyProduct();
			bool flag5 = flag;
			if (flag5)
			{
				flag2 = (Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtCheckpointDeleteOldest"), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok);
			}
			else
			{
				bool flag6 = flag4;
				if (flag6)
				{
					flag2 = (Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtCheckpointDeleteOldestFormOtherProduct"), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok);
				}
			}
			bool flag7 = flag2;
			if (flag7)
			{
				for (int j = 0; j < this.ParamProducts.Count; j++)
				{
					bool flag8 = this.ParamProducts[j].ProductNumber != "0" && this.ParamProducts[j].ProductNumber != "-1";
					if (flag8)
					{
						for (int k = 0; k < this.ParamProducts[j].QAParameters.Count; k++)
						{
							list2.Add(this.ParamProducts[j].QAParameters[k].Values[0]);
							list3.Add(this.ParamProducts[j].ProductName);
							list4.Add(this.ParamProducts[j].ProductNumber);
						}
						foreach (PathsWithAbbreviation pathsWithAbbreviation in this.filterPaths)
						{
							string[] paths = pathsWithAbbreviation.paths;
							for (int l = 0; l < paths.Length; l++)
							{
								list.Add(Definition.filterMainPath + paths[l]);
							}
						}
						foreach (PathsWithAbbreviation pathsWithAbbreviation2 in this.otherPaths)
						{
							string[] paths2 = pathsWithAbbreviation2.paths;
							for (int m = 0; m < paths2.Length; m++)
							{
								list.Add(paths2[m]);
							}
						}
					}
				}
				bool flag9 = flag;
				if (flag9)
				{
					LogbookManager.AddEntry(SSTEntryCode.CheckpointDeleted, (byte)(chkPointInfo.LstIndex + 1), chkPointInfo.Date.ToString(new CultureInfo("en-GB")));
				}
				this.dbConnection.SetChkPointData(chkPointInfo.ChkIndex, list, list2, list3, list4, now, this.accessManager.CurrentUser.Username);
				chkPointInfo.Date = now;
				chkPointInfo.Username = this.accessManager.CurrentUser.Username;
				this.GetChkPointsData();
				foreach (ProductData productData in this.ParamProducts)
				{
					bool flag10 = productData.ProductNumber != "0";
					if (flag10)
					{
						foreach (QualityParameter qualityParameter in productData.QAParameters)
						{
							qualityParameter.SignalValueChange();
						}
					}
				}
				this.SetChkpointsAvailable();
				this.SetChkpointsAreChanged();
				this.SelectedCheckpoint = this.ChkPointInfos[0];
				LogbookManager.AddEntry(SSTEntryCode.CheckpointCreated, (byte)(this.SelectedCheckpoint.LstIndex + 1), this.SelectedCheckpoint.Date.ToString(new CultureInfo("en-GB")));
				Dialog.Show(LocalizationManager.Localize("txtSuccess"), LocalizationManager.Localize("txtCheckpointCreated"), DialogStyle.Ok, DialogButtons.None, true);
			}
		}

		// Token: 0x06001ECD RID: 7885 RVA: 0x000CAB8C File Offset: 0x000C8D8C
		private ChkPointInfo GetOldestChkPoint()
		{
			ChkPointInfo chkPointInfo = null;
			bool flag = this.ChkPointInfos != null && this.ChkPointInfos.Count > 0;
			if (flag)
			{
				chkPointInfo = this.ChkPointInfos[0];
				for (int i = 1; i < this.ChkPointInfos.Count; i++)
				{
					bool flag2 = this.ChkPointInfos[i].Date < chkPointInfo.Date;
					if (flag2)
					{
						chkPointInfo = this.ChkPointInfos[i];
					}
				}
			}
			return chkPointInfo;
		}

		// Token: 0x06001ECE RID: 7886 RVA: 0x000CAC1C File Offset: 0x000C8E1C
		public void SaveToUSB(bool allProducts)
		{
			string text = "";
			string text2 = "";
			this.dbConnection.GetUsbDriveLetter(ref text);
			bool flag = Directory.Exists(text) && text != "\\";
			if (flag)
			{
				string text3 = text + "\\" + this.reportsFolderName;
				bool flag2 = !Directory.Exists(text3);
				if (flag2)
				{
					Directory.CreateDirectory(text3);
				}
				this.dbConnection.GetSerialNumber(ref text2);
				bool flag3 = text2 != "";
				if (flag3)
				{
					text3 = text3 + "\\" + text2;
					bool flag4 = !Directory.Exists(text3);
					if (flag4)
					{
						Directory.CreateDirectory(text3);
					}
				}
				if (allProducts)
				{
					for (int i = 0; i < this.ParamProducts.Count; i++)
					{
						bool flag5 = this.ParamProducts[i].ProductNumber != null && this.ParamProducts[i].ProductNumber != "0" && this.ParamProducts[i].ProductNumber != "-1";
						if (flag5)
						{
							this.WriteToCsv(this.CreateReportFilename(text3, DateTime.Now, this.ParamProducts[i].ProductName, this.ParamProducts[i].ProductNumber), i);
						}
					}
				}
				else
				{
					this.WriteToCsv(this.CreateReportFilename(text3, DateTime.Now, this.SelectedProduct.ProductName, this.SelectedProduct.ProductNumber), this.ParamProducts.IndexOf(this.SelectedProduct));
				}
				Dialog.Show(LocalizationManager.Localize("txtSuccess"), LocalizationManager.Localize("txtSaveSuccessful"), DialogStyle.Ok, DialogButtons.None, true);
			}
			else
			{
				Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtNoUSB"), DialogStyle.Error);
			}
		}

		// Token: 0x06001ECF RID: 7887 RVA: 0x000CAE10 File Offset: 0x000C9010
		private string CreateReportFilename(string reportPath, DateTime time, string productName, string productNumber)
		{
			StringBuilder stringBuilder = new StringBuilder(reportPath);
			stringBuilder.Append(string.Concat(new string[]
			{
				"\\Checkpoints_",
				productNumber,
				"_",
				productName,
				"_",
				time.ToString("yyyy_MM_dd_HH_mm_ss_ff"),
				".csv"
			}));
			return stringBuilder.ToString();
		}

		// Token: 0x06001ED0 RID: 7888 RVA: 0x000CAE79 File Offset: 0x000C9079
		private void CreateReport(string fileName, bool allProducts)
		{
			this.WriteToCsv(fileName, allProducts);
		}

		// Token: 0x06001ED1 RID: 7889 RVA: 0x000CAE88 File Offset: 0x000C9088
		private string CreateReportHeader(DateTime reportTime)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(SSTEthernetFrame.logbookDumpEndMarker);
			stringBuilder.AppendLine("Date:          " + reportTime.ToString("d", LocalizationManager.CurrentCulture));
			stringBuilder.AppendLine("Time:          " + reportTime.ToString("HH:mm:ss", LocalizationManager.CurrentCulture));
			stringBuilder.AppendLine("------------------------------------------");
			stringBuilder.AppendLine("");
			return stringBuilder.ToString();
		}

		// Token: 0x06001ED2 RID: 7890 RVA: 0x000CAF10 File Offset: 0x000C9110
		private string CreateReportContent()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (ProductData productData in this.ParamProducts)
			{
				bool flag = productData.ProductNumber != "0";
				if (flag)
				{
					stringBuilder.AppendLine("P " + productData.ProductNumber + ": " + productData.ProductName);
					stringBuilder.AppendLine();
					foreach (QualityParameter qualityParameter in productData.QAParameters)
					{
						stringBuilder.AppendLine(qualityParameter.ParamName + ": " + qualityParameter.Values[0]);
					}
					stringBuilder.AppendLine();
					stringBuilder.AppendLine();
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06001ED3 RID: 7891 RVA: 0x000CB028 File Offset: 0x000C9228
		private void WriteToCsv(string fileName, bool allProducts)
		{
			StreamWriter streamWriter = null;
			string str = "";
			string str2 = "";
			string str3 = "";
			DateTime now = DateTime.Now;
			try
			{
				ObservableCollection<ProductData> observableCollection;
				if (allProducts)
				{
					observableCollection = new ObservableCollection<ProductData>();
					for (int i = 0; i < this.ParamProducts.Count; i++)
					{
						bool flag = this.ParamProducts[i].ProductNumber != "0";
						if (flag)
						{
							observableCollection.Add(this.ParamProducts[i]);
						}
					}
				}
				else
				{
					observableCollection = new ObservableCollection<ProductData>();
					observableCollection.Add(this.SelectedProduct);
				}
				this.dbConnection.GetSerialNumber(ref str);
				this.dbConnection.GetDeviceName(ref str2);
				this.dbConnection.GetLineName(ref str3);
				streamWriter = new StreamWriter(fileName, false);
				streamWriter.Write("Device:" + Definition.csvSeparator + str2 + Environment.NewLine);
				streamWriter.Write("Line:" + Definition.csvSeparator + str3 + Environment.NewLine);
				streamWriter.Write("Serial number:" + Definition.csvSeparator + str + Environment.NewLine);
				streamWriter.Write("Date:" + Definition.csvSeparator + now.ToString("d", LocalizationManager.CurrentCulture) + Environment.NewLine);
				streamWriter.Write("Time:" + Definition.csvSeparator + now.ToString("HH:mm:ss", LocalizationManager.CurrentCulture) + Environment.NewLine);
				streamWriter.Write("User:" + Definition.csvSeparator + this.accessManager.CurrentUser.Username + Environment.NewLine);
				streamWriter.Write(Environment.NewLine);
				streamWriter.Write(Definition.csvSeparator);
				foreach (ProductData productData in observableCollection)
				{
					bool flag2 = !productData.IsMultiProduct;
					if (flag2)
					{
						streamWriter.Write(string.Concat(new string[]
						{
							"P ",
							productData.ProductNumber,
							": ",
							productData.ProductName,
							Definition.csvSeparator
						}));
					}
				}
				streamWriter.Write(Environment.NewLine);
				for (int j = 0; j < this.ParamProducts[0].QAParameters.Count; j++)
				{
					streamWriter.Write(this.ParamProducts[0].QAParameters[j].ParamName + Definition.csvSeparator);
					for (int k = 0; k < observableCollection.Count; k++)
					{
						bool flag3 = !observableCollection[k].IsMultiProduct;
						if (flag3)
						{
							streamWriter.Write(observableCollection[k].QAParameters[j].Values[0] + Definition.csvSeparator);
						}
					}
					streamWriter.Write(Environment.NewLine);
				}
				streamWriter.Close();
			}
			catch (IOException)
			{
				bool flag4 = streamWriter != null;
				if (flag4)
				{
					streamWriter.Close();
				}
			}
			finally
			{
				bool flag5 = streamWriter != null;
				if (flag5)
				{
					streamWriter.Close();
				}
			}
		}

		// Token: 0x06001ED4 RID: 7892 RVA: 0x000CB3CC File Offset: 0x000C95CC
		public void RestoreChkpoint()
		{
			bool flag = this.SelectedCheckpoint != null;
			if (flag)
			{
				bool flag2 = Dialog.Show(LocalizationManager.Localize("txtWarning"), string.Concat(new string[]
				{
					LocalizationManager.Localize("txtOverwriteExisting"),
					"\n\n",
					LocalizationManager.Localize("txtApplication"),
					": ",
					this.SelectedProduct.ProductNumber,
					" ",
					this.SelectedProduct.ProductName
				}), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
				if (flag2)
				{
					List<string> list = new List<string>();
					List<string> list2 = new List<string>();
					string b = null;
					this.dbConnection.GetCurrentProductTableName(ref b);
					foreach (ProductData productData in this.ParamProducts)
					{
						bool flag3 = productData.ProductNumber != "0";
						if (flag3)
						{
							list.Clear();
							list2.Clear();
							foreach (QualityParameter qualityParameter in productData.QAParameters)
							{
								list.Add(qualityParameter.Path);
								list2.Add(qualityParameter.Values[this.SelectedCheckpoint.LstIndex + 1]);
								qualityParameter.Values[0] = qualityParameter.Values[this.SelectedCheckpoint.LstIndex + 1];
								qualityParameter.SignalValueChange();
							}
							this.dbConnection.UpdateTable(productData.TableName, list, list2);
							bool flag4 = productData.TableName == b;
							if (flag4)
							{
								for (int i = 0; i < list.Count; i++)
								{
									bool flag5 = list[i] != "ConveyorSpeed";
									if (flag5)
									{
										this.parHandling.WriteValue(list[i], list2[i], false);
									}
									else
									{
										double speed = Convert.ToDouble(list2[i], CultureInfo.InvariantCulture);
										bool flag6 = this.conveyor != null;
										if (flag6)
										{
											this.conveyor.Speed = speed;
										}
									}
								}
							}
						}
					}
					this.SelectedCheckpoint = this.SelectedCheckpoint;
					this.SetChkpointsAreChanged();
					LogbookManager.AddEntry(SSTEntryCode.CheckpointRestored, (byte)(this.SelectedCheckpoint.LstIndex + 1), this.SelectedCheckpoint.Date.ToString(new CultureInfo("en-GB")));
					Dialog.Show(LocalizationManager.Localize("txtSuccess"), LocalizationManager.Localize("lbCheckpointRestored"), DialogStyle.Ok, DialogButtons.None, true);
				}
			}
			else
			{
				Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtNoCheckpointSelected"), DialogStyle.Error, DialogButtons.OK);
			}
		}

		// Token: 0x06001ED5 RID: 7893 RVA: 0x000CB6DC File Offset: 0x000C98DC
		public void RestoreProductChkpoint()
		{
			bool flag = this.SelectedCheckpoint != null;
			if (flag)
			{
				bool flag2 = this.SelectedProduct != null;
				if (flag2)
				{
					bool flag3 = this.machineState.State == MachineStates.ConveyorOff;
					if (flag3)
					{
						bool flag4 = Dialog.Show(LocalizationManager.Localize("txtWarning"), string.Concat(new string[]
						{
							LocalizationManager.Localize("txtOverwriteExisting"),
							"\n\n",
							LocalizationManager.Localize("txtApplication"),
							" ",
							this.SelectedProduct.ProductNumber,
							": ",
							this.SelectedProduct.ProductName
						}), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
						if (flag4)
						{
							List<string> list = new List<string>();
							List<string> list2 = new List<string>();
							List<string> list3 = new List<string>();
							List<string> list4 = new List<string>();
							string b = null;
							this.dbConnection.GetCurrentProductTableName(ref b);
							foreach (QualityParameter qualityParameter in this.SelectedProduct.QAParameters)
							{
								bool flag5 = qualityParameter.Group != "AI";
								if (flag5)
								{
									list.Add(qualityParameter.Path);
									list2.Add(qualityParameter.Values[this.SelectedCheckpoint.LstIndex + 1]);
								}
								else
								{
									list3.Add(qualityParameter.Path);
									list4.Add(qualityParameter.Values[this.SelectedCheckpoint.LstIndex + 1]);
								}
								qualityParameter.Values[0] = qualityParameter.Values[this.SelectedCheckpoint.LstIndex + 1];
								qualityParameter.SignalValueChange();
							}
							this.dbConnection.UpdateTable(this.SelectedProduct.TableName, list, list2);
							bool flag6 = this.SelectedProduct.TableName == b;
							if (flag6)
							{
								for (int i = 0; i < list.Count; i++)
								{
									bool flag7 = list[i] != "ConveyorSpeed";
									if (flag7)
									{
										this.parHandling.WriteValue(list[i], list2[i], false);
									}
									else
									{
										double speed = Convert.ToDouble(list2[i], CultureInfo.InvariantCulture);
										bool flag8 = this.conveyor != null;
										if (flag8)
										{
											this.conveyor.Speed = speed;
										}
									}
								}
							}
							this.SelectedCheckpoint = this.SelectedCheckpoint;
							this.SetChkpointsAreChanged();
							LogbookManager.AddEntry(SSTEntryCode.CheckpointRestored);
							Dialog.Show(LocalizationManager.Localize("txtSuccess"), LocalizationManager.Localize("lbCheckpointRestored"), DialogStyle.Ok, DialogButtons.None, true);
						}
					}
					else
					{
						Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtStoppConveyorAndXRay"), DialogStyle.Error, DialogButtons.OK);
					}
				}
				else
				{
					Dialog.Show(LocalizationManager.Localize("txtError"), "No product selected.", DialogStyle.Error, DialogButtons.OK);
				}
			}
			else
			{
				Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtNoCheckpointSelected"), DialogStyle.Error, DialogButtons.OK);
			}
		}

		// Token: 0x06001ED6 RID: 7894 RVA: 0x000CBA0C File Offset: 0x000C9C0C
		public void RemoveCheckpoint()
		{
			bool flag = this.SelectedCheckpoint != null;
			if (flag)
			{
				bool flag2 = Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtCheckpointDelete"), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
				if (flag2)
				{
					this.dbConnection.ClearChkpoint(this.SelectedCheckpoint.ChkIndex);
					bool flag3 = this.dbConnection.CheckPointForProductExists(this.SelectedProduct.ProductName, this.SelectedProduct.ProductNumber);
					LogbookManager.AddEntry(SSTEntryCode.CheckpointDeleted, (byte)(this.SelectedCheckpoint.LstIndex + 1), this.SelectedCheckpoint.Date.ToString(new CultureInfo("en-GB")));
					this.SelectedCheckpoint.Date = DateTime.MinValue;
					this.GetChkPointsData();
					this.SetChkpointsAvailable();
					this.SetChkpointsAreChanged();
					Dialog.Show(LocalizationManager.Localize("txtSuccess"), LocalizationManager.Localize("txtCheckpointDeleted"), DialogStyle.Ok, DialogButtons.None, true);
				}
			}
			else
			{
				Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtNoCheckpointSelected"), DialogStyle.Error, DialogButtons.OK);
			}
		}

		// Token: 0x06001ED7 RID: 7895 RVA: 0x000CBB28 File Offset: 0x000C9D28
		private void SetChkpointsAreChanged()
		{
			bool flag = this.ChkPointInfos != null && this.SelectedProduct != null;
			if (flag)
			{
				for (int i = 0; i < this.ChkPointInfos.Count; i++)
				{
					bool isChanged = false;
					int num = 0;
					bool available = this.ChkPointInfos[i].Available;
					if (available)
					{
						for (int j = 0; j < this.SelectedProduct.QAParameters.Count; j++)
						{
							bool flag2 = this.SelectedProduct.QAParameters[j].Values[i + 1] != this.SelectedProduct.QAParameters[j].Values[0];
							if (flag2)
							{
								isChanged = true;
								num++;
							}
						}
						this.ChkPointInfos[i].IsChanged = isChanged;
						this.ChkPointInfos[i].NumChanged = num;
					}
				}
			}
		}

		// Token: 0x06001ED8 RID: 7896 RVA: 0x000CBC2C File Offset: 0x000C9E2C
		private void WriteToCsv(string fileName, int index)
		{
			StreamWriter streamWriter = null;
			ProductData productData = this.ParamProducts[index];
			string str = "";
			string str2 = "";
			string str3 = "";
			DateTime now = DateTime.Now;
			try
			{
				this.dbConnection.GetSerialNumber(ref str);
				this.dbConnection.GetDeviceName(ref str2);
				this.dbConnection.GetLineName(ref str3);
				streamWriter = new StreamWriter(fileName, false);
				streamWriter.Write("Product Name:" + Definition.csvSeparator + productData.ProductName + Environment.NewLine);
				streamWriter.Write("Product Number:" + Definition.csvSeparator + productData.ProductNumber + Environment.NewLine);
				streamWriter.Write("Device:" + Definition.csvSeparator + str2 + Environment.NewLine);
				streamWriter.Write("Line:" + Definition.csvSeparator + str3 + Environment.NewLine);
				streamWriter.Write("Serial number:" + Definition.csvSeparator + str + Environment.NewLine);
				streamWriter.Write("Date:" + Definition.csvSeparator + now.ToString("d", LocalizationManager.CurrentCulture) + Environment.NewLine);
				streamWriter.Write("Time:" + Definition.csvSeparator + now.ToString("HH:mm:ss", LocalizationManager.CurrentCulture) + Environment.NewLine);
				streamWriter.Write("User:" + Definition.csvSeparator + this.accessManager.CurrentUser.Username + Environment.NewLine);
				streamWriter.Write(Environment.NewLine);
				streamWriter.Write(Definition.csvSeparator);
				streamWriter.Write(Environment.NewLine);
				streamWriter.Write(Definition.csvSeparator + "Current Data" + Definition.csvSeparator);
				for (int i = 0; i < Definition.nQACheckpoints; i++)
				{
					bool flag = this.ChkPointInfos[i].Username != "";
					if (flag)
					{
						streamWriter.Write("Checkpoint " + (i + 1).ToString() + Definition.csvSeparator);
					}
				}
				streamWriter.Write(Environment.NewLine);
				streamWriter.Write(string.Concat(new string[]
				{
					"Date",
					Definition.csvSeparator,
					"(",
					DateTime.Now.ToString(),
					")",
					Definition.csvSeparator
				}));
				for (int j = 0; j < Definition.nQACheckpoints; j++)
				{
					bool flag2 = this.ChkPointInfos[j].Username != "";
					if (flag2)
					{
						streamWriter.Write(this.ChkPointInfos[j].Date.ToString() + Definition.csvSeparator);
					}
				}
				streamWriter.Write(Environment.NewLine);
				streamWriter.Write("User" + Definition.csvSeparator + "-" + Definition.csvSeparator);
				for (int k = 0; k < Definition.nQACheckpoints; k++)
				{
					bool flag3 = this.ChkPointInfos[k].Username != "";
					if (flag3)
					{
						streamWriter.Write(this.ChkPointInfos[k].Username + Definition.csvSeparator);
					}
				}
				streamWriter.Write(Environment.NewLine);
				for (int l = 0; l < productData.QAParameters.Count; l++)
				{
					streamWriter.Write(string.Concat(new string[]
					{
						productData.QAParameters[l].ParamName,
						Definition.csvSeparator,
						productData.QAParameters[l].Values[0],
						Definition.csvSeparator,
						productData.QAParameters[l].Values[1],
						Definition.csvSeparator,
						productData.QAParameters[l].Values[2],
						Definition.csvSeparator,
						productData.QAParameters[l].Values[3],
						Definition.csvSeparator
					}));
					streamWriter.Write(Environment.NewLine);
				}
				streamWriter.Close();
			}
			catch (IOException)
			{
				bool flag4 = streamWriter != null;
				if (flag4)
				{
					streamWriter.Close();
				}
			}
			finally
			{
				bool flag5 = streamWriter != null;
				if (flag5)
				{
					streamWriter.Close();
				}
			}
		}

		// Token: 0x06001ED9 RID: 7897 RVA: 0x000CC0F8 File Offset: 0x000CA2F8
		private bool AllChkpointsOccupiedForAnyProduct()
		{
			bool result = false;
			bool flag = this.ChkPointInfos != null && this.SelectedProduct != null;
			if (flag)
			{
				foreach (ProductData productData in this.ParamProducts)
				{
					int num = 0;
					for (int i = 0; i < this.ChkPointInfos.Count; i++)
					{
						for (int j = 0; j < productData.QAParameters.Count; j++)
						{
							bool flag2 = productData.QAParameters[j].Values[i + 1] != null;
							if (flag2)
							{
								num++;
								break;
							}
						}
					}
					bool flag3 = num == 3;
					if (flag3)
					{
						result = true;
						break;
					}
				}
			}
			return result;
		}

		// Token: 0x040013FF RID: 5119
		private ObservableCollection<PathsWithAbbreviation> filterPaths = new ObservableCollection<PathsWithAbbreviation>();

		// Token: 0x04001400 RID: 5120
		private ObservableCollection<PathsWithAbbreviation> otherPaths = new ObservableCollection<PathsWithAbbreviation>();

		// Token: 0x04001401 RID: 5121
		private ObservableCollection<PathsWithAbbreviation> pathsForAI = new ObservableCollection<PathsWithAbbreviation>();

		// Token: 0x04001402 RID: 5122
		public static readonly string[] paramQASubPathsRejecter = new string[]
		{
			"/hardware/rejecter/rejecter1/Enabled/Value",
			"/hardware/rejecter/rejecter1/Length/Value",
			"/hardware/rejecter/rejecter1/Priority/Value",
			"/hardware/rejecter/rejecter1/Type/Value",
			"/hardware/rejecter/rejecter1/Lane/Value",
			"/hardware/rejecter/rejecter1/Delay/Value",
			"/hardware/rejecter/rejecter2/Enabled/Value",
			"/hardware/rejecter/rejecter2/Length/Value",
			"/hardware/rejecter/rejecter2/Priority/Value",
			"/hardware/rejecter/rejecter2/Type/Value",
			"/hardware/rejecter/rejecter2/Lane/Value",
			"/hardware/rejecter/rejecter2/Delay/Value",
			"/hardware/rejecter/rejecter3/Enabled/Value",
			"/hardware/rejecter/rejecter3/Length/Value",
			"/hardware/rejecter/rejecter3/Priority/Value",
			"/hardware/rejecter/rejecter3/Type/Value",
			"/hardware/rejecter/rejecter3/Lane/Value",
			"/hardware/rejecter/rejecter3/Delay/Value",
			"/hardware/rejecter/rejecter4/Enabled/Value",
			"/hardware/rejecter/rejecter4/Length/Value",
			"/hardware/rejecter/rejecter4/Priority/Value",
			"/hardware/rejecter/rejecter4/Type/Value",
			"/hardware/rejecter/rejecter4/Lane/Value",
			"/hardware/rejecter/rejecter4/Delay/Value",
			"/hardware/rejecter/rejecter1/VerifyEnable/Value",
			"/hardware/rejecter/rejecter2/VerifyEnable/Value",
			"/hardware/rejecter/rejecter3/VerifyEnable/Value",
			"/hardware/rejecter/rejecter4/VerifyEnable/Value"
		};

		// Token: 0x04001403 RID: 5123
		public static readonly string[] paramQASubPathsRejecterBulk = new string[]
		{
			"/hardware/rejecter/bulkflaps/Enabled/Value",
			"/hardware/rejecter/bulkflaps/FlapsCount/Value",
			"/hardware/rejecter/bulkflaps/BorderPixel1/Value",
			"/hardware/rejecter/bulkflaps/BorderPixel2/Value",
			"/hardware/rejecter/bulkflaps/BorderPixel3/Value",
			"/hardware/rejecter/bulkflaps/DelayTime/Value",
			"/hardware/rejecter/bulkflaps/EjectTime/Value",
			"/hardware/rejecter/bulkflaps/EnableValves/Value",
			"/hardware/svt/Enable/Value",
			"/hardware/svt/SoftwareDelay/Value",
			"/hardware/svt/ValvesEnable/Value"
		};

		// Token: 0x04001404 RID: 5124
		public static readonly string[] paramQASubPathsRejecterBuzzer = new string[]
		{
			"/hardware/rejecter/buzzer/Delay/Value",
			"/hardware/rejecter/buzzer/Enabled/Value",
			"/hardware/rejecter/buzzer/Length/Value",
			"/hardware/rejecter/buzzer/Priority/Value",
			"/hardware/rejecter/buzzer/Type/Value",
			"/hardware/rejecter/buzzer/Lane/Value",
			"/hardware/rejecter/buzzer/BeltStopOnContaminant/Value",
			"/hardware/rejecter/buzzer/Mode/Value"
		};

		// Token: 0x04001405 RID: 5125
		public static readonly string[] paramQASubPathsThreshold = new string[]
		{
			"/threshold/Enabled/Value",
			"/threshold/HighThreshold/Value",
			"/threshold/LowThreshold/Value",
			"/thresholdGlobal/Enabled/Value",
			"/thresholdGlobal/HighThreshold/Value",
			"/thresholdGlobal/LowThreshold/Value"
		};

		// Token: 0x04001406 RID: 5126
		public static readonly string[] paramQASubPathsEdge = new string[]
		{
			"/edge/Enabled/Value",
			"/edge/Threshold/Value",
			"/edge/BottomEnabled/Value",
			"/edge/LeftEnabled/Value",
			"/edge/RightEnabled/Value",
			"/edge/TopEnabled/Value",
			"/edge/FilterOperation/Value",
			"/edge/NLeftPixel/Value",
			"/edge/NRightPixel/Value",
			"/edge/NTopPixel/Value",
			"/edge/NBottomPixel/Value",
			"/edge/NIterations/Value",
			"/edge/FilterSize/Value",
			"/edge/autoDetect/Threshold/Value",
			"/edge/autoDetect/Enabled/Value",
			"/edge/autoDetect/AddedPixel/Value",
			"/edge/autoDetect/MinEdge/Value",
			"/edge/autoDetect/MaxMDelimiter/Value",
			"/edge/autoDetect/MaxMHigh/Value",
			"/edge/autoDetect/MaxMLow/Value",
			"/edge/autoDetect/AutoDisableAllowed/Value"
		};

		// Token: 0x04001407 RID: 5127
		public static readonly string[] paramQASubPathsClipFinder = new string[]
		{
			"/clipFinding/MinClipSize/Value",
			"/clipFinding/MaxClipSize/Value",
			"/clipFinding/ClipThreshold/Value",
			"/clipFinding/Enable/Value",
			"/clipFinding/ClipFilterSize/Value",
			"/clipFinding/AfterGlobal/Value"
		};

		// Token: 0x04001408 RID: 5128
		public static readonly string[] paramQASubPathsCropping = new string[]
		{
			"/cropping/Enable/Value",
			"/cropping/AddedLeftPixel/Value",
			"/cropping/AddedRightPixel/Value",
			"/cropping/ProcessDemoImages/Value"
		};

		// Token: 0x04001409 RID: 5129
		public static readonly string[] paramQASubPathsHistogramm = new string[]
		{
			"/histogram/Enable/Value",
			"/histogram/NPixel/Value",
			"/histogram/MinGrayvalue/Value",
			"/histogram/MaxGrayvalue/Value"
		};

		// Token: 0x0400140A RID: 5130
		public static readonly string[] paramQASubPathsCan = new string[]
		{
			"/can/Enabled/Value",
			"/can/NPixel/Value",
			"/can/CenterX/Value",
			"/can/CenterY/Value",
			"/can/RadiusX/Value",
			"/can/RadiusY/Value"
		};

		// Token: 0x0400140B RID: 5131
		public static readonly string[] paramQASubPathsRotate = new string[]
		{
			"/rotate/Enabled/Value",
			"/rotate/Angle/Value",
			"/rotate/RotateToOrientation/Value",
			"/rotate/LowThreshold/Value",
			"/rotate/HighThreshold/Value"
		};

		// Token: 0x0400140C RID: 5132
		public static readonly string[] paramQASubPathsDimension = new string[]
		{
			"/dimension/Enabled/Value",
			"/dimension/MinLength/Value",
			"/dimension/MaxLength/Value",
			"/dimension/LowThreshold/Value",
			"/dimension/HighThreshold/Value"
		};

		// Token: 0x0400140D RID: 5133
		public static readonly string[] paramQASubPathsObjectFinder = new string[]
		{
			"/objectFinder1/Enabled/Value",
			"/objectFinder1/Contrast/Value",
			"/objectFinder1/HighThreshold/Value",
			"/objectFinder1/LowThreshold/Value",
			"/objectFinder2/Enabled/Value",
			"/objectFinder2/Contrast/Value",
			"/objectFinder2/HighThreshold/Value",
			"/objectFinder2/LowThreshold/Value",
			"/objectFinder3/Enabled/Value",
			"/objectFinder3/Contrast/Value",
			"/objectFinder3/HighThreshold/Value",
			"/objectFinder3/LowThreshold/Value",
			"/objectFinder4/Enabled/Value",
			"/objectFinder4/Contrast/Value",
			"/objectFinder4/HighThreshold/Value",
			"/objectFinder4/LowThreshold/Value",
			"/objectFinder5/Enabled/Value",
			"/objectFinder5/Contrast/Value",
			"/objectFinder5/HighThreshold/Value",
			"/objectFinder5/LowThreshold/Value",
			"/objectFinder6/Enabled/Value",
			"/objectFinder6/Contrast/Value",
			"/objectFinder6/HighThreshold/Value",
			"/objectFinder6/LowThreshold/Value",
			"/objectFinder7/Enabled/Value",
			"/objectFinder7/Contrast/Value",
			"/objectFinder7/HighThreshold/Value",
			"/objectFinder7/LowThreshold/Value",
			"/objectFinder8/Enabled/Value",
			"/objectFinder8/Contrast/Value",
			"/objectFinder8/HighThreshold/Value",
			"/objectFinder8/LowThreshold/Value",
			"/objectFinder9/Enabled/Value",
			"/objectFinder9/Contrast/Value",
			"/objectFinder9/HighThreshold/Value",
			"/objectFinder9/LowThreshold/Value",
			"/objectFinder10/Enabled/Value",
			"/objectFinder10/Contrast/Value",
			"/objectFinder10/HighThreshold/Value",
			"/objectFinder10/LowThreshold/Value",
			"/objectFinder11/Enabled/Value",
			"/objectFinder11/Contrast/Value",
			"/objectFinder11/HighThreshold/Value",
			"/objectFinder11/LowThreshold/Value",
			"/objectFinder12/Enabled/Value",
			"/objectFinder12/Contrast/Value",
			"/objectFinder12/HighThreshold/Value",
			"/objectFinder12/LowThreshold/Value",
			"/objectFinder13/Enabled/Value",
			"/objectFinder13/Contrast/Value",
			"/objectFinder13/HighThreshold/Value",
			"/objectFinder13/LowThreshold/Value",
			"/objectFinder14/Enabled/Value",
			"/objectFinder14/Contrast/Value",
			"/objectFinder14/HighThreshold/Value",
			"/objectFinder14/LowThreshold/Value",
			"/objectFinder15/Enabled/Value",
			"/objectFinder15/Contrast/Value",
			"/objectFinder15/HighThreshold/Value",
			"/objectFinder15/LowThreshold/Value",
			"/objectFinder16/Enabled/Value",
			"/objectFinder16/Contrast/Value",
			"/objectFinder16/HighThreshold/Value",
			"/objectFinder16/LowThreshold/Value",
			"/objectFinder17/Enabled/Value",
			"/objectFinder17/Contrast/Value",
			"/objectFinder17/HighThreshold/Value",
			"/objectFinder17/LowThreshold/Value",
			"/objectFinder18/Enabled/Value",
			"/objectFinder18/Contrast/Value",
			"/objectFinder18/HighThreshold/Value",
			"/objectFinder18/LowThreshold/Value",
			"/objectFinder19/Enabled/Value",
			"/objectFinder19/Contrast/Value",
			"/objectFinder19/HighThreshold/Value",
			"/objectFinder19/LowThreshold/Value",
			"/objectFinder20/Enabled/Value",
			"/objectFinder20/Contrast/Value",
			"/objectFinder20/HighThreshold/Value",
			"/objectFinder20/LowThreshold/Value",
			"/objectFinder21/Enabled/Value",
			"/objectFinder21/Contrast/Value",
			"/objectFinder21/HighThreshold/Value",
			"/objectFinder21/LowThreshold/Value",
			"/objectFinder22/Enabled/Value",
			"/objectFinder22/Contrast/Value",
			"/objectFinder22/HighThreshold/Value",
			"/objectFinder22/LowThreshold/Value",
			"/objectFinder23/Enabled/Value",
			"/objectFinder23/Contrast/Value",
			"/objectFinder23/HighThreshold/Value",
			"/objectFinder23/LowThreshold/Value",
			"/objectFinder24/Enabled/Value",
			"/objectFinder24/Contrast/Value",
			"/objectFinder24/HighThreshold/Value",
			"/objectFinder24/LowThreshold/Value",
			"/objectFinder25/Enabled/Value",
			"/objectFinder25/Contrast/Value",
			"/objectFinder25/HighThreshold/Value",
			"/objectFinder25/LowThreshold/Value",
			"/objectFinder26/Enabled/Value",
			"/objectFinder26/Contrast/Value",
			"/objectFinder26/HighThreshold/Value",
			"/objectFinder26/LowThreshold/Value",
			"/objectFinder27/Enabled/Value",
			"/objectFinder27/Contrast/Value",
			"/objectFinder27/HighThreshold/Value",
			"/objectFinder27/LowThreshold/Value",
			"/objectFinder28/Enabled/Value",
			"/objectFinder28/Contrast/Value",
			"/objectFinder28/HighThreshold/Value",
			"/objectFinder28/LowThreshold/Value",
			"/objectFinder29/Enabled/Value",
			"/objectFinder29/Contrast/Value",
			"/objectFinder29/HighThreshold/Value",
			"/objectFinder29/LowThreshold/Value",
			"/objectFinder30/Enabled/Value",
			"/objectFinder30/Contrast/Value",
			"/objectFinder30/HighThreshold/Value",
			"/objectFinder30/LowThreshold/Value",
			"/objectFinder31/Enabled/Value",
			"/objectFinder31/Contrast/Value",
			"/objectFinder31/HighThreshold/Value",
			"/objectFinder31/LowThreshold/Value",
			"/objectFinder32/Enabled/Value",
			"/objectFinder32/Contrast/Value",
			"/objectFinder32/HighThreshold/Value",
			"/objectFinder32/LowThreshold/Value",
			"/objectFinder33/Enabled/Value",
			"/objectFinder33/Contrast/Value",
			"/objectFinder33/HighThreshold/Value",
			"/objectFinder33/LowThreshold/Value",
			"/objectFinder34/Enabled/Value",
			"/objectFinder34/Contrast/Value",
			"/objectFinder34/HighThreshold/Value",
			"/objectFinder34/LowThreshold/Value",
			"/objectFinder35/Enabled/Value",
			"/objectFinder35/Contrast/Value",
			"/objectFinder35/HighThreshold/Value",
			"/objectFinder35/LowThreshold/Value",
			"/objectFinder36/Enabled/Value",
			"/objectFinder36/Contrast/Value",
			"/objectFinder36/HighThreshold/Value",
			"/objectFinder36/LowThreshold/Value",
			"/objectFinder37/Enabled/Value",
			"/objectFinder37/Contrast/Value",
			"/objectFinder37/HighThreshold/Value",
			"/objectFinder37/LowThreshold/Value",
			"/objectFinder38/Enabled/Value",
			"/objectFinder38/Contrast/Value",
			"/objectFinder38/HighThreshold/Value",
			"/objectFinder38/LowThreshold/Value",
			"/objectFinder39/Enabled/Value",
			"/objectFinder39/Contrast/Value",
			"/objectFinder39/HighThreshold/Value",
			"/objectFinder39/LowThreshold/Value",
			"/objectFinder40/Enabled/Value",
			"/objectFinder40/Contrast/Value",
			"/objectFinder40/HighThreshold/Value",
			"/objectFinder40/LowThreshold/Value"
		};

		// Token: 0x0400140E RID: 5134
		public static readonly string[] paramQASubPathsLowDensity = new string[]
		{
			"/lowDensity/Enabled/Value",
			"/lowDensity/Contrast1/Value",
			"/lowDensity/Contrast2/Value",
			"/lowDensity/Contrast3/Value",
			"/lowDensity/Contrast4/Value",
			"/lowDensity/Contrast5/Value",
			"/lowDensity/Threshold/Value"
		};

		// Token: 0x0400140F RID: 5135
		public static readonly string[] paramQASubPathsComparer = new string[]
		{
			"/comparer/comparer1/Enabled/Value",
			"/comparer/comparer1/Contrast/Value",
			"/comparer/comparer1/HighThreshold/Value",
			"/comparer/comparer1/LowThreshold/Value",
			"/comparer/comparer2/Enabled/Value",
			"/comparer/comparer2/Contrast/Value",
			"/comparer/comparer2/HighThreshold/Value",
			"/comparer/comparer2/LowThreshold/Value",
			"/comparer/comparer3/Enabled/Value",
			"/comparer/comparer3/Contrast/Value",
			"/comparer/comparer3/HighThreshold/Value",
			"/comparer/comparer3/LowThreshold/Value",
			"/comparer/comparer4/Enabled/Value",
			"/comparer/comparer4/Contrast/Value",
			"/comparer/comparer4/HighThreshold/Value",
			"/comparer/comparer4/LowThreshold/Value",
			"/comparer/comparer5/Enabled/Value",
			"/comparer/comparer5/Contrast/Value",
			"/comparer/comparer5/HighThreshold/Value",
			"/comparer/comparer5/LowThreshold/Value",
			"/comparer/comparer6/Enabled/Value",
			"/comparer/comparer6/Contrast/Value",
			"/comparer/comparer6/HighThreshold/Value",
			"/comparer/comparer6/LowThreshold/Value",
			"/comparer/comparer7/Enabled/Value",
			"/comparer/comparer7/Contrast/Value",
			"/comparer/comparer7/HighThreshold/Value",
			"/comparer/comparer7/LowThreshold/Value",
			"/comparer/comparer8/Enabled/Value",
			"/comparer/comparer8/Contrast/Value",
			"/comparer/comparer8/HighThreshold/Value",
			"/comparer/comparer8/LowThreshold/Value",
			"/comparer/comparer9/Enabled/Value",
			"/comparer/comparer9/Contrast/Value",
			"/comparer/comparer9/HighThreshold/Value",
			"/comparer/comparer9/LowThreshold/Value",
			"/comparer/comparer10/Enabled/Value",
			"/comparer/comparer10/Contrast/Value",
			"/comparer/comparer10/HighThreshold/Value",
			"/comparer/comparer10/LowThreshold/Value",
			"/comparer/comparer11/Enabled/Value",
			"/comparer/comparer11/Contrast/Value",
			"/comparer/comparer11/HighThreshold/Value",
			"/comparer/comparer11/LowThreshold/Value",
			"/comparer/comparer12/Enabled/Value",
			"/comparer/comparer12/Contrast/Value",
			"/comparer/comparer12/HighThreshold/Value",
			"/comparer/comparer12/LowThreshold/Value",
			"/comparer/comparer13/Enabled/Value",
			"/comparer/comparer13/Contrast/Value",
			"/comparer/comparer13/HighThreshold/Value",
			"/comparer/comparer13/LowThreshold/Value",
			"/comparer/comparer14/Enabled/Value",
			"/comparer/comparer14/Contrast/Value",
			"/comparer/comparer14/HighThreshold/Value",
			"/comparer/comparer14/LowThreshold/Value",
			"/comparer/comparer15/Enabled/Value",
			"/comparer/comparer15/Contrast/Value",
			"/comparer/comparer15/HighThreshold/Value",
			"/comparer/comparer15/LowThreshold/Value",
			"/comparer/comparer16/Enabled/Value",
			"/comparer/comparer16/Contrast/Value",
			"/comparer/comparer16/HighThreshold/Value",
			"/comparer/comparer16/LowThreshold/Value",
			"/comparer/comparer17/Enabled/Value",
			"/comparer/comparer17/Contrast/Value",
			"/comparer/comparer17/HighThreshold/Value",
			"/comparer/comparer17/LowThreshold/Value",
			"/comparer/comparer18/Enabled/Value",
			"/comparer/comparer18/Contrast/Value",
			"/comparer/comparer18/HighThreshold/Value",
			"/comparer/comparer18/LowThreshold/Value",
			"/comparer/comparer19/Enabled/Value",
			"/comparer/comparer19/Contrast/Value",
			"/comparer/comparer19/HighThreshold/Value",
			"/comparer/comparer19/LowThreshold/Value",
			"/comparer/comparer20/Enabled/Value",
			"/comparer/comparer20/Contrast/Value",
			"/comparer/comparer20/HighThreshold/Value",
			"/comparer/comparer20/LowThreshold/Value",
			"/comparer/comparer21/Enabled/Value",
			"/comparer/comparer21/Contrast/Value",
			"/comparer/comparer21/HighThreshold/Value",
			"/comparer/comparer21/LowThreshold/Value",
			"/comparer/comparer22/Enabled/Value",
			"/comparer/comparer22/Contrast/Value",
			"/comparer/comparer22/HighThreshold/Value",
			"/comparer/comparer22/LowThreshold/Value",
			"/comparer/comparer23/Enabled/Value",
			"/comparer/comparer23/Contrast/Value",
			"/comparer/comparer23/HighThreshold/Value",
			"/comparer/comparer23/LowThreshold/Value",
			"/comparer/comparer24/Enabled/Value",
			"/comparer/comparer24/Contrast/Value",
			"/comparer/comparer24/HighThreshold/Value",
			"/comparer/comparer24/LowThreshold/Value",
			"/comparer/comparer25/Enabled/Value",
			"/comparer/comparer25/Contrast/Value",
			"/comparer/comparer25/HighThreshold/Value",
			"/comparer/comparer25/LowThreshold/Value",
			"/comparer/comparer26/Enabled/Value",
			"/comparer/comparer26/Contrast/Value",
			"/comparer/comparer26/HighThreshold/Value",
			"/comparer/comparer26/LowThreshold/Value",
			"/comparer/comparer27/Enabled/Value",
			"/comparer/comparer27/Contrast/Value",
			"/comparer/comparer27/HighThreshold/Value",
			"/comparer/comparer27/LowThreshold/Value",
			"/comparer/comparer28/Enabled/Value",
			"/comparer/comparer28/Contrast/Value",
			"/comparer/comparer28/HighThreshold/Value",
			"/comparer/comparer28/LowThreshold/Value",
			"/comparer/comparer29/Enabled/Value",
			"/comparer/comparer29/Contrast/Value",
			"/comparer/comparer29/HighThreshold/Value",
			"/comparer/comparer29/LowThreshold/Value",
			"/comparer/comparer30/Enabled/Value",
			"/comparer/comparer30/Contrast/Value",
			"/comparer/comparer30/HighThreshold/Value",
			"/comparer/comparer30/LowThreshold/Value"
		};

		// Token: 0x04001410 RID: 5136
		public static readonly string[] paramQASubPathsWeight = new string[]
		{
			"/weigher/Enabled/Value",
			"/weigher/ExpectedWeight/Value",
			"/weigher/MinWeight/Value",
			"/weigher/MaxWeight/Value"
		};

		// Token: 0x04001411 RID: 5137
		public static readonly string[] paramQASubPathsWeight2 = new string[]
		{
			"MinWeight",
			"MaxWeight",
			"ReferenceWeight"
		};

		// Token: 0x04001412 RID: 5138
		public static readonly string[] paramQASubPathsCount = new string[]
		{
			"/objectCounter/Enabled/Value",
			"/objectCounter/MaxObjects/Value",
			"/objectCounter/MaxObjectSize/Value",
			"/objectCounter/MinObjects/Value",
			"/objectCounter/MinObjectSize/Value",
			"/objectCounter/HighThreshold/Value",
			"/objectCounter/LowThreshold/Value"
		};

		// Token: 0x04001413 RID: 5139
		public static readonly string[] paramQASubPathsCountContaminations = new string[]
		{
			"/contaminationCounter/Enabled/Value",
			"/contaminationCounter/filterSize/Value"
		};

		// Token: 0x04001414 RID: 5140
		public static readonly string[] paramQASubPathsSize = new string[]
		{
			"/sizeFilter/Enabled/Value",
			"/sizeFilter/MinWidth/Value",
			"/sizeFilter/MaxWidth/Value",
			"/sizeFilter/MinHeight/Value",
			"/sizeFilter/MaxHeight/Value"
		};

		// Token: 0x04001415 RID: 5141
		public static readonly string[] paramQASubPathsWire = new string[]
		{
			"/wireFinder/Enabled/Value",
			"/wireFinder/ContrastHorizontal/Value",
			"/wireFinder/ContrastVertical/Value",
			"/wireFinder/ContrastDiag1/Value",
			"/wireFinder/ContrastDiag2/Value",
			"/wireFinder/FilterSize/Value"
		};

		// Token: 0x04001416 RID: 5142
		public static readonly string[] paramQASubPathsLane = new string[]
		{
			"/acquisition/lane1/Enable/Value",
			"/acquisition/lane1/MinPixel/Value",
			"/acquisition/lane1/MaxPixel/Value",
			"/acquisition/lane2/Enable/Value",
			"/acquisition/lane2/MinPixel/Value",
			"/acquisition/lane2/MaxPixel/Value",
			"/acquisition/lane3/Enable/Value",
			"/acquisition/lane3/MinPixel/Value",
			"/acquisition/lane3/MaxPixel/Value",
			"/acquisition/lane4/Enable/Value",
			"/acquisition/lane4/MinPixel/Value",
			"/acquisition/lane4/MaxPixel/Value"
		};

		// Token: 0x04001417 RID: 5143
		public static readonly string[] paramQASubPathsCommon = new string[]
		{
			"/hardware/xraysource/CurrentSetpoint/Value",
			"/hardware/xraysource/VoltageSetpoint/Value",
			"/machine/DeformationFactor/Value",
			"ConveyorSpeed"
		};

		// Token: 0x04001418 RID: 5144
		public static readonly string[] paramQASubPathsAcquisition = new string[]
		{
			"/acquisition/AddedBackLines/Value",
			"/acquisition/AddedFrontLines/Value",
			"/acquisition/ConnectImages/Value",
			"/acquisition/ContinuousMode/Value",
			"/acquisition/MaxLines/Value",
			"/acquisition/NConnectedLines/Value",
			"/acquisition/Threshold/Value",
			"/acquisition/BackgroundNoise/Value",
			"/acquisition/MinLines/Value",
			"/acquisition/ShowOnlyIfContaminant/Value",
			"/acquisition/SendOnlyIfContaminant/Value",
			"/acquisition/ContinuousModeFiltering/Value",
			"/acquisition/SendImage/Value",
			"/acquisition/Binning/Value",
			"/acquisition/BinMode/Value",
			"/acquisition/ShowImage/Value",
			"/acquisition/SaveImage/Value",
			"/acquisition/SaveOnlyIfContaminant/Value",
			"/acquisition/BinPixel/Value",
			"/acquisition/MinLinePixel/Value",
			"/acquisition/MinLinePixelConnected/Value",
			"/acquisition/AcquisitionMode/Value",
			"/acquisition/NLinesObjectImage/Value",
			"/acquisition/AddedLeftPixObjectImg/Value",
			"/acquisition/AddedRightPixObjectImg/Value",
			"/acquisition/NImages/Value",
			"/acquisition/ContinuousModeObjImg/Value",
			"/acquisition/HighThroughputEnabled/Value",
			"/acquisition/BulkShowContImages/Value"
		};

		// Token: 0x04001419 RID: 5145
		public static readonly string[] paramQASubPathsDual = new string[]
		{
			"/dual/Enabled/Value",
			"/dual/MaterialCurveIndex/Value",
			"/dual/MaterialCurveMin/Value",
			"/dual/MaterialCurveMax/Value",
			"/dual/RequestCurve/Value",
			"/dual/ToleranceMax/Value",
			"/dual/ToleranceMin/Value"
		};

		// Token: 0x0400141A RID: 5146
		public static readonly string[] paramQASubPathsRecognition = new string[]
		{
			"/recognition/EnableWidth/Value",
			"/recognition/EnableHeight/Value",
			"/recognition/EnableAverageGrayvalue/Value",
			"/recognition/EnableArea/Value",
			"/recognition/EnableObjectPixel/Value",
			"/recognition/EnableLane/Value",
			"/recognition/EnableGradient/Value",
			"/recognition/EnableThresGradient/Value",
			"/recognition/EnableMinMaxGradient/Value",
			"/recognition/MinGradient/Value",
			"/recognition/MaxGradient/Value",
			"/recognition/Tolerance/Value"
		};

		// Token: 0x0400141B RID: 5147
		public static readonly string[] paramQASubPathsAIParameters = new string[]
		{
			"/anomalyDetection/inference/postFilter1/Enabled/Value",
			"/anomalyDetection/inference/postFilter1/ThresholdReconErrInside/Value",
			"/anomalyDetection/inference/postFilter1/ThresholdReconErrContour/Value",
			"/anomalyDetection/inference/postFilter1/ThresholdGroupInside/Value",
			"/anomalyDetection/inference/postFilter1/ThresholdGroupContour/Value",
			"/anomalyDetection/inference/postFilter1/MinGroupSizeInside/Value",
			"/anomalyDetection/inference/postFilter1/MaxGroupSizeInside/Value",
			"/anomalyDetection/inference/postFilter1/MinGroupSizeContour/Value",
			"/anomalyDetection/inference/postFilter1/MaxGroupSizeContour/Value",
			"/anomalyDetection/inference/postFilter2/Enabled/Value",
			"/anomalyDetection/inference/postFilter2/ThresholdReconErrInside/Value",
			"/anomalyDetection/inference/postFilter2/ThresholdReconErrContour/Value",
			"/anomalyDetection/inference/postFilter2/ThresholdGroupInside/Value",
			"/anomalyDetection/inference/postFilter2/ThresholdGroupContour/Value",
			"/anomalyDetection/inference/postFilter2/MinGroupSizeInside/Value",
			"/anomalyDetection/inference/postFilter2/MaxGroupSizeInside/Value",
			"/anomalyDetection/inference/postFilter2/MinGroupSizeContour/Value",
			"/anomalyDetection/inference/postFilter2/MaxGroupSizeContour/Value",
			"/anomalyDetection/inference/postFilter3/Enabled/Value",
			"/anomalyDetection/inference/postFilter3/ThresholdReconErrInside/Value",
			"/anomalyDetection/inference/postFilter3/ThresholdReconErrContour/Value",
			"/anomalyDetection/inference/postFilter3/ThresholdGroupInside/Value",
			"/anomalyDetection/inference/postFilter3/ThresholdGroupContour/Value",
			"/anomalyDetection/inference/postFilter3/MinGroupSizeInside/Value",
			"/anomalyDetection/inference/postFilter3/MaxGroupSizeInside/Value",
			"/anomalyDetection/inference/postFilter3/MinGroupSizeContour/Value",
			"/anomalyDetection/inference/postFilter3/MaxGroupSizeContour/Value",
			"/anomalyDetection/inference/postFilter4/Enabled/Value",
			"/anomalyDetection/inference/postFilter4/ThresholdReconErrInside/Value",
			"/anomalyDetection/inference/postFilter4/ThresholdReconErrContour/Value",
			"/anomalyDetection/inference/postFilter4/ThresholdGroupInside/Value",
			"/anomalyDetection/inference/postFilter4/ThresholdGroupContour/Value",
			"/anomalyDetection/inference/postFilter4/MinGroupSizeInside/Value",
			"/anomalyDetection/inference/postFilter4/MaxGroupSizeInside/Value",
			"/anomalyDetection/inference/postFilter4/MinGroupSizeContour/Value",
			"/anomalyDetection/inference/postFilter4/MaxGroupSizeContour/Value",
			"/anomalyDetection/inference/postFilter5/Enabled/Value",
			"/anomalyDetection/inference/postFilter5/ThresholdReconErrInside/Value",
			"/anomalyDetection/inference/postFilter5/ThresholdReconErrContour/Value",
			"/anomalyDetection/inference/postFilter5/ThresholdGroupInside/Value",
			"/anomalyDetection/inference/postFilter5/ThresholdGroupContour/Value",
			"/anomalyDetection/inference/postFilter5/MinGroupSizeInside/Value",
			"/anomalyDetection/inference/postFilter5/MaxGroupSizeInside/Value",
			"/anomalyDetection/inference/postFilter5/MinGroupSizeContour/Value",
			"/anomalyDetection/inference/postFilter5/MaxGroupSizeContour/Value",
			"/anomalyDetection/inference/postFilter6/Enabled/Value",
			"/anomalyDetection/inference/postFilter6/ThresholdReconErrInside/Value",
			"/anomalyDetection/inference/postFilter6/ThresholdReconErrContour/Value",
			"/anomalyDetection/inference/postFilter6/ThresholdGroupInside/Value",
			"/anomalyDetection/inference/postFilter6/ThresholdGroupContour/Value",
			"/anomalyDetection/inference/postFilter6/MinGroupSizeInside/Value",
			"/anomalyDetection/inference/postFilter6/MaxGroupSizeInside/Value",
			"/anomalyDetection/inference/postFilter6/MinGroupSizeContour/Value",
			"/anomalyDetection/inference/postFilter6/MaxGroupSizeContour/Value",
			"/anomalyDetection/inference/postFilter7/Enabled/Value",
			"/anomalyDetection/inference/postFilter7/ThresholdReconErrInside/Value",
			"/anomalyDetection/inference/postFilter7/ThresholdReconErrContour/Value",
			"/anomalyDetection/inference/postFilter7/ThresholdGroupInside/Value",
			"/anomalyDetection/inference/postFilter7/ThresholdGroupContour/Value",
			"/anomalyDetection/inference/postFilter7/MinGroupSizeInside/Value",
			"/anomalyDetection/inference/postFilter7/MaxGroupSizeInside/Value",
			"/anomalyDetection/inference/postFilter7/MinGroupSizeContour/Value",
			"/anomalyDetection/inference/postFilter7/MaxGroupSizeContour/Value",
			"/anomalyDetection/inference/postFilter8/Enabled/Value",
			"/anomalyDetection/inference/postFilter8/ThresholdReconErrInside/Value",
			"/anomalyDetection/inference/postFilter8/ThresholdReconErrContour/Value",
			"/anomalyDetection/inference/postFilter8/ThresholdGroupInside/Value",
			"/anomalyDetection/inference/postFilter8/ThresholdGroupContour/Value",
			"/anomalyDetection/inference/postFilter8/MinGroupSizeInside/Value",
			"/anomalyDetection/inference/postFilter8/MaxGroupSizeInside/Value",
			"/anomalyDetection/inference/postFilter8/MinGroupSizeContour/Value",
			"/anomalyDetection/inference/postFilter8/MaxGroupSizeContour/Value",
			"/anomalyDetection/inference/postFilter9/Enabled/Value",
			"/anomalyDetection/inference/postFilter9/ThresholdReconErrInside/Value",
			"/anomalyDetection/inference/postFilter9/ThresholdReconErrContour/Value",
			"/anomalyDetection/inference/postFilter9/ThresholdGroupInside/Value",
			"/anomalyDetection/inference/postFilter9/ThresholdGroupContour/Value",
			"/anomalyDetection/inference/postFilter9/MinGroupSizeInside/Value",
			"/anomalyDetection/inference/postFilter9/MaxGroupSizeInside/Value",
			"/anomalyDetection/inference/postFilter9/MinGroupSizeContour/Value",
			"/anomalyDetection/inference/postFilter9/MaxGroupSizeContour/Value",
			"/anomalyDetection/inference/postFilter10/Enabled/Value",
			"/anomalyDetection/inference/postFilter10/ThresholdReconErrInside/Value",
			"/anomalyDetection/inference/postFilter10/ThresholdReconErrContour/Value",
			"/anomalyDetection/inference/postFilter10/ThresholdGroupInside/Value",
			"/anomalyDetection/inference/postFilter10/ThresholdGroupContour/Value",
			"/anomalyDetection/inference/postFilter10/MinGroupSizeInside/Value",
			"/anomalyDetection/inference/postFilter10/MaxGroupSizeInside/Value",
			"/anomalyDetection/inference/postFilter10/MinGroupSizeContour/Value",
			"/anomalyDetection/inference/postFilter10/MaxGroupSizeContour/Value",
			"/objectDetection/inference/class1/Enabled/Value",
			"/objectDetection/inference/class1/MinObjects/Value",
			"/objectDetection/inference/class1/MaxObjects/Value",
			"/objectDetection/inference/class1/DetectionType/Value",
			"/objectDetection/inference/class1/NmsThreshold/Value",
			"/objectDetection/inference/class1/ConfThreshold/Value",
			"/objectDetection/inference/class1/GroupID/Value",
			"/objectDetection/inference/class1/ClassAvailable/Value",
			"/objectDetection/inference/class1/UnmaskForCont/Value",
			"/objectDetection/inference/class1/UnmaskMethod/Value",
			"/objectDetection/inference/class1/FilterOperation/Value",
			"/objectDetection/inference/class1/FilterSize/Value",
			"/objectDetection/inference/class1/MinGroupSize/Value",
			"/objectDetection/inference/class2/Enabled/Value",
			"/objectDetection/inference/class2/MinObjects/Value",
			"/objectDetection/inference/class2/MaxObjects/Value",
			"/objectDetection/inference/class2/DetectionType/Value",
			"/objectDetection/inference/class2/NmsThreshold/Value",
			"/objectDetection/inference/class2/ConfThreshold/Value",
			"/objectDetection/inference/class2/GroupID/Value",
			"/objectDetection/inference/class2/ClassAvailable/Value",
			"/objectDetection/inference/class2/UnmaskForCont/Value",
			"/objectDetection/inference/class2/UnmaskMethod/Value",
			"/objectDetection/inference/class2/FilterOperation/Value",
			"/objectDetection/inference/class2/FilterSize/Value",
			"/objectDetection/inference/class2/MinGroupSize/Value",
			"/objectDetection/inference/class3/Enabled/Value",
			"/objectDetection/inference/class3/MinObjects/Value",
			"/objectDetection/inference/class3/MaxObjects/Value",
			"/objectDetection/inference/class3/DetectionType/Value",
			"/objectDetection/inference/class3/NmsThreshold/Value",
			"/objectDetection/inference/class3/ConfThreshold/Value",
			"/objectDetection/inference/class3/GroupID/Value",
			"/objectDetection/inference/class3/ClassAvailable/Value",
			"/objectDetection/inference/class3/UnmaskForCont/Value",
			"/objectDetection/inference/class3/UnmaskMethod/Value",
			"/objectDetection/inference/class3/FilterOperation/Value",
			"/objectDetection/inference/class3/FilterSize/Value",
			"/objectDetection/inference/class3/MinGroupSize/Value",
			"/objectDetection/inference/class4/Enabled/Value",
			"/objectDetection/inference/class4/MinObjects/Value",
			"/objectDetection/inference/class4/MaxObjects/Value",
			"/objectDetection/inference/class4/DetectionType/Value",
			"/objectDetection/inference/class4/NmsThreshold/Value",
			"/objectDetection/inference/class4/ConfThreshold/Value",
			"/objectDetection/inference/class4/GroupID/Value",
			"/objectDetection/inference/class4/ClassAvailable/Value",
			"/objectDetection/inference/class4/UnmaskForCont/Value",
			"/objectDetection/inference/class4/UnmaskMethod/Value",
			"/objectDetection/inference/class4/FilterOperation/Value",
			"/objectDetection/inference/class4/FilterSize/Value",
			"/objectDetection/inference/class4/MinGroupSize/Value",
			"/objectDetection/inference/class5/Enabled/Value",
			"/objectDetection/inference/class5/MinObjects/Value",
			"/objectDetection/inference/class5/MaxObjects/Value",
			"/objectDetection/inference/class5/DetectionType/Value",
			"/objectDetection/inference/class5/NmsThreshold/Value",
			"/objectDetection/inference/class5/ConfThreshold/Value",
			"/objectDetection/inference/class5/GroupID/Value",
			"/objectDetection/inference/class5/ClassAvailable/Value",
			"/objectDetection/inference/class5/UnmaskForCont/Value",
			"/objectDetection/inference/class5/UnmaskMethod/Value",
			"/objectDetection/inference/class5/FilterOperation/Value",
			"/objectDetection/inference/class5/FilterSize/Value",
			"/objectDetection/inference/class5/MinGroupSize/Value",
			"/anomalyDetection/inference/SharpeningFactor/Value",
			"/anomalyDetection/inference/BackgroundThreshold/Value",
			"/anomalyDetection/inference/IgnoreBorderPixels/Value",
			"/anomalyDetection/inference/Gamma/Value",
			"/anomalyDetection/inference/Size/Value",
			"/anomalyDetection/inference/MinObjectSize/Value",
			"/anomalyDetection/inference/Enabled/Value",
			"/anomalyDetection/inference/Indicator/Value",
			"/anomalyDetection/train/Enabled/Value",
			"/anomalyDetection/train/Epochs/Value",
			"/anomalyDetection/train/BatchSize/Value",
			"/anomalyDetection/train/LearningRate/Value",
			"/anomalyDetection/train/ValidationRatio/Value",
			"/anomalyDetection/train/Size/Value",
			"/anomalyDetection/train/SharpeningFactor/Value",
			"/anomalyDetection/train/BackgroundThreshold/Value",
			"/anomalyDetection/train/IgnoreBorderPixels/Value",
			"/anomalyDetection/train/Gamma/Value",
			"/anomalyDetection/train/SizeDataset/Value",
			"/anomalyDetection/train/CurrentEpoch/Value",
			"/anomalyDetection/train/TypeLossFunction/Value",
			"/anomalyDetection/train/NumberEncoders/Value",
			"/anomalyDetection/train/MinObjectSize/Value",
			"/objectDetection/inference/Enabled/Value",
			"/objectDetection/inference/Indicator/Value"
		};

		// Token: 0x0400141C RID: 5148
		public static readonly string[] paramQASubPaths = new string[]
		{
			"/threshold/LowThreshold/Value",
			"/threshold/HighThreshold/Value",
			"/threshold/Enabled/Value",
			"/thresholdGlobal/LowThreshold/Value",
			"/thresholdGlobal/HighThreshold/Value",
			"/thresholdGlobal/Enabled/Value",
			"/objectFinder1/LowThreshold/Value",
			"/objectFinder1/HighThreshold/Value",
			"/objectFinder1/Contrast/Value",
			"/objectFinder2/LowThreshold/Value",
			"/objectFinder2/HighThreshold/Value",
			"/objectFinder2/Contrast/Value",
			"/objectFinder3/LowThreshold/Value",
			"/objectFinder3/HighThreshold/Value",
			"/objectFinder3/Contrast/Value",
			"/objectFinder4/LowThreshold/Value",
			"/objectFinder4/HighThreshold/Value",
			"/objectFinder4/Contrast/Value",
			"/objectFinder5/LowThreshold/Value",
			"/objectFinder5/HighThreshold/Value",
			"/objectFinder5/Contrast/Value",
			"/objectFinder6/LowThreshold/Value",
			"/objectFinder6/HighThreshold/Value",
			"/objectFinder6/Contrast/Value",
			"/objectFinder7/LowThreshold/Value",
			"/objectFinder7/HighThreshold/Value",
			"/objectFinder7/Contrast/Value",
			"/objectFinder8/LowThreshold/Value",
			"/objectFinder8/HighThreshold/Value",
			"/objectFinder8/Contrast/Value",
			"/objectFinder9/LowThreshold/Value",
			"/objectFinder9/HighThreshold/Value",
			"/objectFinder9/Contrast/Value",
			"/objectFinder10/LowThreshold/Value",
			"/objectFinder10/HighThreshold/Value",
			"/objectFinder10/Contrast/Value",
			"/objectFinder11/LowThreshold/Value",
			"/objectFinder11/HighThreshold/Value",
			"/objectFinder11/Contrast/Value",
			"/objectFinder12/LowThreshold/Value",
			"/objectFinder12/HighThreshold/Value",
			"/objectFinder12/Contrast/Value",
			"/objectFinder13/LowThreshold/Value",
			"/objectFinder13/HighThreshold/Value",
			"/objectFinder13/Contrast/Value",
			"/objectFinder14/LowThreshold/Value",
			"/objectFinder14/HighThreshold/Value",
			"/objectFinder14/Contrast/Value",
			"/objectFinder15/LowThreshold/Value",
			"/objectFinder15/HighThreshold/Value",
			"/objectFinder15/Contrast/Value",
			"/objectFinder17/LowThreshold/Value",
			"/objectFinder16/HighThreshold/Value",
			"/objectFinder16/Contrast/Value",
			"/objectFinder17/LowThreshold/Value",
			"/objectFinder17/HighThreshold/Value",
			"/objectFinder17/Contrast/Value",
			"/objectFinder18/LowThreshold/Value",
			"/objectFinder18/HighThreshold/Value",
			"/objectFinder18/Contrast/Value",
			"/objectFinder19/LowThreshold/Value",
			"/objectFinder19/HighThreshold/Value",
			"/objectFinder19/Contrast/Value",
			"/objectFinder20/LowThreshold/Value",
			"/objectFinder20/HighThreshold/Value",
			"/objectFinder20/Contrast/Value",
			"/objectFinder21/LowThreshold/Value",
			"/objectFinder21/HighThreshold/Value",
			"/objectFinder21/Contrast/Value",
			"/objectFinder22/LowThreshold/Value",
			"/objectFinder22/HighThreshold/Value",
			"/objectFinder22/Contrast/Value",
			"/objectFinder23/LowThreshold/Value",
			"/objectFinder23/HighThreshold/Value",
			"/objectFinder23/Contrast/Value",
			"/objectFinder24/LowThreshold/Value",
			"/objectFinder24/HighThreshold/Value",
			"/objectFinder24/Contrast/Value",
			"/objectFinder25/LowThreshold/Value",
			"/objectFinder25/HighThreshold/Value",
			"/objectFinder25/Contrast/Value",
			"/objectFinder26/LowThreshold/Value",
			"/objectFinder26/HighThreshold/Value",
			"/objectFinder26/Contrast/Value",
			"/objectFinder27/LowThreshold/Value",
			"/objectFinder27/HighThreshold/Value",
			"/objectFinder27/Contrast/Value",
			"/objectFinder28/LowThreshold/Value",
			"/objectFinder28/HighThreshold/Value",
			"/objectFinder28/Contrast/Value",
			"/objectFinder29/LowThreshold/Value",
			"/objectFinder29/HighThreshold/Value",
			"/objectFinder29/Contrast/Value",
			"/objectFinder30/LowThreshold/Value",
			"/objectFinder30/HighThreshold/Value",
			"/objectFinder30/Contrast/Value",
			"/objectFinder31/LowThreshold/Value",
			"/objectFinder31/HighThreshold/Value",
			"/objectFinder31/Contrast/Value",
			"/objectFinder32/LowThreshold/Value",
			"/objectFinder32/HighThreshold/Value",
			"/objectFinder32/Contrast/Value",
			"/edge/LeftEnabled/Value",
			"/edge/RightEnabled/Value",
			"/edge/TopEnabled/Value",
			"/edge/BottomEnabled/Value",
			"/edge/Threshold/Value",
			"/edge/NLeftPixel/Value",
			"/edge/NRightPixel/Value",
			"/edge/NTopPixel/Value",
			"/edge/NBottomPixel/Value",
			"/edge/FilterOperation/Value",
			"/edge/NIterations/Value",
			"/edge/FilterSize/Value",
			"/edge/MultipleObjects/Value",
			"/edge/MinObjectSize/Value",
			"/edge/MaxObjectSize/Value",
			"/edge/UseOrientation/Value",
			"/edge/OpeningFilterSize/Value",
			"/objectFinder1/Algorithm/Value",
			"/objectFinder2/Algorithm/Value",
			"/objectFinder3/Algorithm/Value",
			"/objectFinder4/Algorithm/Value",
			"/objectFinder5/Algorithm/Value",
			"/objectFinder6/Algorithm/Value",
			"/objectFinder7/Algorithm/Value",
			"/objectFinder8/Algorithm/Value",
			"/objectFinder9/Algorithm/Value",
			"/objectFinder10/Algorithm/Value",
			"/objectFinder11/Algorithm/Value",
			"/objectFinder12/Algorithm/Value",
			"/objectFinder13/Algorithm/Value",
			"/objectFinder14/Algorithm/Value",
			"/objectFinder15/Algorithm/Value",
			"/objectFinder16/Algorithm/Value",
			"/objectFinder17/Algorithm/Value",
			"/objectFinder18/Algorithm/Value",
			"/objectFinder19/Algorithm/Value",
			"/objectFinder20/Algorithm/Value",
			"/objectFinder21/Algorithm/Value",
			"/objectFinder22/Algorithm/Value",
			"/objectFinder23/Algorithm/Value",
			"/objectFinder24/Algorithm/Value",
			"/objectFinder25/Algorithm/Value",
			"/objectFinder26/Algorithm/Value",
			"/objectFinder27/Algorithm/Value",
			"/objectFinder28/Algorithm/Value",
			"/objectFinder29/Algorithm/Value",
			"/objectFinder30/Algorithm/Value",
			"/objectFinder31/Algorithm/Value",
			"/objectFinder32/Algorithm/Value",
			"/objectFinder1/Enabled/Value",
			"/objectFinder2/Enabled/Value",
			"/objectFinder3/Enabled/Value",
			"/objectFinder4/Enabled/Value",
			"/objectFinder5/Enabled/Value",
			"/objectFinder6/Enabled/Value",
			"/objectFinder7/Enabled/Value",
			"/objectFinder8/Enabled/Value",
			"/objectFinder9/Enabled/Value",
			"/objectFinder10/Enabled/Value",
			"/objectFinder11/Enabled/Value",
			"/objectFinder12/Enabled/Value",
			"/objectFinder13/Enabled/Value",
			"/objectFinder14/Enabled/Value",
			"/objectFinder15/Enabled/Value",
			"/objectFinder16/Enabled/Value",
			"/objectFinder17/Enabled/Value",
			"/objectFinder18/Enabled/Value",
			"/objectFinder19/Enabled/Value",
			"/objectFinder20/Enabled/Value",
			"/objectFinder21/Enabled/Value",
			"/objectFinder22/Enabled/Value",
			"/objectFinder23/Enabled/Value",
			"/objectFinder24/Enabled/Value",
			"/objectFinder25/Enabled/Value",
			"/objectFinder26/Enabled/Value",
			"/objectFinder27/Enabled/Value",
			"/objectFinder28/Enabled/Value",
			"/objectFinder29/Enabled/Value",
			"/objectFinder30/Enabled/Value",
			"/objectFinder31/Enabled/Value",
			"/objectFinder32/Enabled/Value",
			"/objectFinder25/FilterSize/Value",
			"/objectFinder26/FilterSize/Value",
			"/objectFinder27/FilterSize/Value",
			"/objectFinder28/FilterSize/Value",
			"/objectFinder29/FilterSize/Value",
			"/objectFinder30/FilterSize/Value",
			"/objectFinder31/FilterSize/Value",
			"/objectFinder32/FilterSize/Value",
			"/lowDensity/Algorithm/Value",
			"/lowDensity/Contrast1/Value",
			"/lowDensity/Contrast2/Value",
			"/lowDensity/Contrast3/Value",
			"/lowDensity/Contrast4/Value",
			"/lowDensity/Contrast5/Value",
			"/lowDensity/CountThreshold1/Value",
			"/lowDensity/CountThreshold2/Value",
			"/lowDensity/CountThreshold3/Value",
			"/lowDensity/CountThreshold4/Value",
			"/lowDensity/EnableCounts/Value",
			"/lowDensity/Enabled/Value",
			"/lowDensity/FilterSizeStep/Value",
			"/lowDensity/SizeStep/Value",
			"/lowDensity/StartFilterSize/Value",
			"/lowDensity/StartMinSize/Value",
			"/lowDensity/Threshold/Value",
			"/comparer/comparer1/Enabled/Value",
			"/comparer/comparer2/Enabled/Value",
			"/comparer/comparer3/Enabled/Value",
			"/comparer/comparer4/Enabled/Value",
			"/comparer/comparer5/Enabled/Value",
			"/comparer/comparer1/HighThreshold/Value",
			"/comparer/comparer2/HighThreshold/Value",
			"/comparer/comparer3/HighThreshold/Value",
			"/comparer/comparer4/HighThreshold/Value",
			"/comparer/comparer5/HighThreshold/Value",
			"/comparer/comparer1/Matches/Value",
			"/comparer/comparer2/Matches/Value",
			"/comparer/comparer3/Matches/Value",
			"/comparer/comparer4/Matches/Value",
			"/comparer/comparer5/Matches/Value",
			"/comparer/comparer1/MinSize/Value",
			"/comparer/comparer2/MinSize/Value",
			"/comparer/comparer3/MinSize/Value",
			"/comparer/comparer4/MinSize/Value",
			"/comparer/comparer5/MinSize/Value",
			"/comparer/comparer1/Radius/Value",
			"/comparer/comparer2/Radius/Value",
			"/comparer/comparer3/Radius/Value",
			"/comparer/comparer4/Radius/Value",
			"/comparer/comparer5/Radius/Value"
		};

		// Token: 0x0400141D RID: 5149
		private const string timeFormatString = "yyyy_MM_dd_HH_mm_ss_ff";

		// Token: 0x0400141E RID: 5150
		private string reportsFolderName = "Reports_Checkpoints";

		// Token: 0x0400141F RID: 5151
		private DBConnection dbConnection;

		// Token: 0x04001420 RID: 5152
		private ParameterHandling parHandling;

		// Token: 0x04001421 RID: 5153
		private ChkPointInfo selectedCheckpoint;

		// Token: 0x04001422 RID: 5154
		private AccessRightsManager accessManager;

		// Token: 0x04001423 RID: 5155
		private ProductData selectedProduct;

		// Token: 0x04001424 RID: 5156
		private bool isCheckpointAvailable;

		// Token: 0x0400142A RID: 5162
		private Conveyor conveyor;

		// Token: 0x0400142B RID: 5163
		private ProductManager productManager;

		// Token: 0x0400142C RID: 5164
		private bool isAIMachine;

		// Token: 0x0400142D RID: 5165
		private MachineState machineState;

		// Token: 0x0400142E RID: 5166
		private bool isEX1 = false;
	}
}
