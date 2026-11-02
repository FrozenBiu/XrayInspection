using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Common;
using DatabaseConnection;
using Definitions;
using ICSharpCode.SharpZipLib.Zip;

namespace GUI_XRS
{
	// Token: 0x0200000E RID: 14
	internal class AIRestoreListProductsViewModel : INotifyPropertyChanged
	{
		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000A9 RID: 169 RVA: 0x0000527D File Offset: 0x0000347D
		// (set) Token: 0x060000AA RID: 170 RVA: 0x00005285 File Offset: 0x00003485
		public RelayCommand DoRestoreCommand { get; set; }

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000AB RID: 171 RVA: 0x0000528E File Offset: 0x0000348E
		// (set) Token: 0x060000AC RID: 172 RVA: 0x00005296 File Offset: 0x00003496
		public RelayCommand CleanUpCommand { get; set; }

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x060000AD RID: 173 RVA: 0x000052A0 File Offset: 0x000034A0
		// (remove) Token: 0x060000AE RID: 174 RVA: 0x000052D8 File Offset: 0x000034D8
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000AF RID: 175 RVA: 0x00005310 File Offset: 0x00003510
		// (set) Token: 0x060000B0 RID: 176 RVA: 0x00005328 File Offset: 0x00003528
		public string SelectedProduct
		{
			get
			{
				return this.selectedProduct;
			}
			set
			{
				bool flag = this.selectedProduct != value;
				if (flag)
				{
					this.selectedProduct = value;
					this.OnPropertyChanged("SelectedProduct");
				}
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000B1 RID: 177 RVA: 0x0000535C File Offset: 0x0000355C
		// (set) Token: 0x060000B2 RID: 178 RVA: 0x00005374 File Offset: 0x00003574
		public ObservableCollection<string> ProductNamesList
		{
			get
			{
				return this.productNamesList;
			}
			set
			{
				bool flag = this.productNamesList != value;
				if (flag)
				{
					this.productNamesList = value;
					this.OnPropertyChanged("ProductNamesList");
				}
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000B3 RID: 179 RVA: 0x000053A8 File Offset: 0x000035A8
		// (set) Token: 0x060000B4 RID: 180 RVA: 0x000053C0 File Offset: 0x000035C0
		public bool ShowOnlyValidProducts
		{
			get
			{
				return this.showOnlyValidProducts;
			}
			set
			{
				this.showOnlyValidProducts = value;
				this.OnPropertyChanged("ShowOnlyValidProducts");
			}
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x000053D8 File Offset: 0x000035D8
		public AIRestoreListProductsViewModel(string backupPath, List<string> tableNamesAI, ParameterHandling parHandling, DBConnection dbConnection, bool restoreThinkXA, bool restoreThinkXO, ProductManager productManager)
		{
			this.ProductNamesList = new ObservableCollection<string>();
			this.parHandling = parHandling;
			this.dbConnection = dbConnection;
			this.restoreThinkXA = restoreThinkXA;
			this.restoreThinkXO = restoreThinkXO;
			this.backupPath = backupPath;
			this.productManager = productManager;
			this.DoRestoreCommand = new RelayCommand(new Action<object>(this.DoRestore));
			this.CleanUpCommand = new RelayCommand(new Action<object>(this.CleanUp));
			this.CreateProductList(tableNamesAI, new Action<List<string>>(this.restore_ProductNamesExtracted));
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x0000546C File Offset: 0x0000366C
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00005499 File Offset: 0x00003699
		private void CleanUp(object obj)
		{
			this.ProductNamesList.Clear();
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x000054A8 File Offset: 0x000036A8
		private void DoRestore(object obj)
		{
			bool flag = this.parHandling.ReadPar(XrayState.xrayPowerOnPath) == "1" || this.parHandling.ReadPar(XrayState.xrayStarterPath) == "1";
			if (flag)
			{
				Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtTurnXrayOff"), DialogStyle.Error, DialogButtons.OK);
			}
			else
			{
				bool flag2 = Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtOverwriteExisting"), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
				if (flag2)
				{
					bool flag3 = this.SelectedProduct != null && this.SelectedProduct != "";
					if (flag3)
					{
						bool flag4 = true;
						bool flag5 = this.parHandling.ReadPar(Definition.anomalyDetectionTrainingInProgress) == "1" && this.restoreThinkXA && this.parHandling.ReadPar(Definition.anomalyDetectionTrainModelID) == this.parHandling.ReadPar(Definition.anomalyDetectionInferenceModelID);
						if (flag5)
						{
							string message = LocalizationManager.Localize("txtAITrainingAbort3");
							flag4 = (Dialog.Show(LocalizationManager.Localize("txtWarning"), message, DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok);
							bool flag6 = flag4;
							if (flag6)
							{
								this.parHandling.WriteValue(Definition.anomalyDetectionTrainEnabled, "0");
							}
						}
						bool flag7 = flag4;
						if (flag7)
						{
							bool flag8 = this.restoreThinkXA && this.restoreThinkXO;
							if (flag8)
							{
								LogbookManager.AddEntry(SSTEntryCode.AIDetectionDeactivated, 5, "Think XA/XO", this.productManager.CurrentLoadedProduct.ProductNumber, this.productManager.CurrentLoadedProduct.ProductName, this.backupPath, this.selectedProduct);
							}
							else
							{
								bool flag9 = this.restoreThinkXA;
								if (flag9)
								{
									LogbookManager.AddEntry(SSTEntryCode.AIDetectionDeactivated, 5, "Think XA", this.productManager.CurrentLoadedProduct.ProductNumber, this.productManager.CurrentLoadedProduct.ProductName, this.backupPath, this.selectedProduct);
								}
								else
								{
									bool flag10 = this.restoreThinkXO;
									if (flag10)
									{
										LogbookManager.AddEntry(SSTEntryCode.AIDetectionDeactivated, 5, "Think XO", this.productManager.CurrentLoadedProduct.ProductNumber, this.productManager.CurrentLoadedProduct.ProductName, this.backupPath, this.selectedProduct);
									}
								}
							}
							this.RestoreThink(new Action(this.restore_ThinkRestored));
						}
					}
					else
					{
						Dialog.Show(LocalizationManager.Localize("txtError"), "No backup chosen!", DialogStyle.Error);
					}
				}
			}
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00005720 File Offset: 0x00003920
		private void RestoreThink(Action onFinished)
		{
			Action <>9__1;
			Thread thread = new Thread(delegate()
			{
				try
				{
					HashSet<string> hashSet;
					AIRestoreListBackupsViewModel.GetRequiredModelFiles(this.parHandling, this.restoreThinkXA, this.restoreThinkXO, out hashSet);
					bool flag = ZipHelper.ZipContainsFile(this.backupPath, DBConnection.database);
					if (flag)
					{
						bool flag2 = this.restoreThinkXA;
						if (flag2)
						{
							this.parHandling.WriteValue(Definition.anomalyDetectionInferenceEnabled, "0");
						}
						bool flag3 = this.restoreThinkXO;
						if (flag3)
						{
							this.parHandling.WriteValue(Definition.objectDetectionInferenceEnabled, "0");
						}
						string text = ";-\\.svn";
						FastZip fastZip = new FastZip();
						fastZip.Password = Backup.zipPassword;
						text = text + ";+" + DBConnection.database.Replace("\\", "\\\\");
						bool flag4 = Directory.Exists(AIRestoreListProductsViewModel.tempBackupPath);
						if (flag4)
						{
							Directory.Delete(AIRestoreListProductsViewModel.tempBackupPath, true);
						}
						Directory.CreateDirectory(AIRestoreListProductsViewModel.tempBackupPath);
						fastZip.ExtractZip(this.backupPath, AIRestoreListProductsViewModel.tempBackupPath, text);
						DBConnection dbconnection = new DBConnection();
						dbconnection.Connect(AIRestoreListProductsViewModel.tempBackupPath + "\\" + DBConnection.database);
						string text2 = "";
						dbconnection.GetAppTableNameWithProductName(this.selectedProduct, ref text2);
						text2 = text2.Insert("AppDefault".Length, "AI");
						List<string> list = new List<string>();
						List<string> list2 = new List<string>();
						dbconnection.GetProductEntries(text2, list, list2);
						for (int i = 0; i < list.Count; i++)
						{
							bool flag5 = list[i].StartsWith("/filter/anomalyDetection/inference/");
							bool flag6 = list[i].StartsWith("/filter/objectDetection/");
							bool flag7 = (this.restoreThinkXA && flag5) || (this.restoreThinkXO && flag6);
							if (flag7)
							{
								this.parHandling.WriteValue(list[i], list2[i], true);
							}
						}
						dbconnection.CloseConnection();
						List<string> list3 = new List<string>();
						List<string> list4 = new List<string>();
						this.dbConnection.GetProductEntries(DBConnection.defaultApplicationAI, list3, list4);
						HashSet<string> hashSet2 = new HashSet<string>(list);
						for (int j = 0; j < list3.Count; j++)
						{
							bool flag8 = hashSet2.Contains(list3[j]);
							if (!flag8)
							{
								bool flag9 = list3[j].StartsWith("/filter/anomalyDetection/inference/");
								bool flag10 = list3[j].StartsWith("/filter/objectDetection/");
								bool flag11 = (this.restoreThinkXA && flag9) || (this.restoreThinkXO && flag10);
								if (flag11)
								{
									this.parHandling.WriteValue(list3[j], list4[j], true);
								}
							}
						}
						text = ";-\\.svn";
						text = text + ";+Archive/Model/" + text2 + "/*";
						text = text + ";+Archive/Image/TrainAnomalyDetection/" + text2 + "/*";
						fastZip.ExtractZip(this.backupPath, AIRestoreListProductsViewModel.tempBackupPath, text);
						string tableName = "";
						this.dbConnection.GetCurrentProductTableNameAI(ref tableName);
						string archiveDirectory = ModelTransfer.GetArchiveDirectory(tableName);
						bool flag12 = !Directory.Exists(archiveDirectory);
						if (flag12)
						{
							Directory.CreateDirectory(archiveDirectory);
						}
						string path = Path.Combine(AIRestoreListProductsViewModel.tempBackupPath, "Archive", "Model", text2);
						string text3 = Path.Combine(new string[]
						{
							AIRestoreListProductsViewModel.tempBackupPath,
							"Archive",
							"Image",
							"TrainAnomalyDetection",
							text2
						});
						foreach (string path2 in hashSet)
						{
							string text4 = Path.Combine(path, path2);
							string destFileName = Path.Combine(archiveDirectory, path2);
							bool flag13 = File.Exists(text4);
							if (flag13)
							{
								File.Copy(text4, destFileName, true);
							}
						}
						bool flag14 = this.restoreThinkXO;
						if (flag14)
						{
							string text5 = Path.Combine(path, AIObjectViewModel.fileClassNames);
							string destFileName2 = Path.Combine(archiveDirectory, AIObjectViewModel.fileClassNames);
							bool flag15 = File.Exists(text5);
							if (flag15)
							{
								File.Copy(text5, destFileName2, true);
							}
						}
						bool flag16 = this.restoreThinkXA && Directory.Exists(text3);
						if (flag16)
						{
							string text6 = archiveDirectory.Replace("Model", "Image\\TrainAnomalyDetection");
							bool flag17 = Directory.Exists(text6);
							if (flag17)
							{
								Directory.Delete(text6, true);
							}
							Directory.CreateDirectory(text6);
							foreach (string text7 in Directory.GetFiles(text3, "*.*", SearchOption.AllDirectories))
							{
								string path3 = text7.Substring(text3.Length + 1);
								string destFileName3 = Path.Combine(text6, path3);
								File.Copy(text7, destFileName3, true);
							}
						}
						bool flag18 = Directory.Exists(AIRestoreListProductsViewModel.tempBackupPath);
						if (flag18)
						{
							Directory.Delete(AIRestoreListProductsViewModel.tempBackupPath, true);
						}
						Dispatcher dispatcher = Application.Current.Dispatcher;
						Action method;
						if ((method = <>9__1) == null)
						{
							method = (<>9__1 = delegate()
							{
								Action onFinished2 = onFinished;
								if (onFinished2 != null)
								{
									onFinished2();
								}
							});
						}
						dispatcher.BeginInvoke(method, new object[0]);
					}
				}
				catch (Exception ex)
				{
					Exception ex3;
					Exception ex2 = ex3;
					Exception ex = ex2;
					bool flag19 = Directory.Exists(AIRestoreListProductsViewModel.tempBackupPath);
					if (flag19)
					{
						Directory.Delete(AIRestoreListProductsViewModel.tempBackupPath, true);
					}
					Progress.CloseStaticProgress(false);
					Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(delegate()
					{
						Dialog.Show("Error", ex.Message, DialogStyle.Error, DialogButtons.OK);
					}));
				}
			});
			Progress.ShowStaticProgress("Updating product...", false);
			thread.Name = "RestoreThink";
			thread.Start();
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00005774 File Offset: 0x00003974
		private void CreateProductList(List<string> tableNamesAI, Action<List<string>> onFinished)
		{
			bool flag = this.backupPath != null;
			if (flag)
			{
				Progress.ShowStaticProgress(LocalizationManager.Localize("txtProcessing"), false);
				this.GetProductNamesFromBackup(tableNamesAI, onFinished);
			}
		}

		// Token: 0x060000BB RID: 187 RVA: 0x000057AC File Offset: 0x000039AC
		private void GetProductNamesFromBackup(List<string> tableNamesAI, Action<List<string>> onFinished)
		{
			Thread thread = new Thread(delegate()
			{
				try
				{
					List<string> productNames = null;
					bool flag = ZipHelper.ZipContainsFile(this.backupPath, DBConnection.database);
					if (flag)
					{
						string text = ";-\\.svn";
						FastZip fastZip = new FastZip();
						fastZip.Password = Backup.zipPassword;
						text = text + ";+" + DBConnection.database.Replace("\\", "\\\\");
						bool flag2 = !Directory.Exists(AIRestoreListProductsViewModel.tempBackupPath);
						if (flag2)
						{
							Directory.CreateDirectory(AIRestoreListProductsViewModel.tempBackupPath);
						}
						fastZip.ExtractZip(this.backupPath, AIRestoreListProductsViewModel.tempBackupPath, text);
						DBConnection dbconnection = new DBConnection();
						dbconnection.Connect(AIRestoreListProductsViewModel.tempBackupPath + "\\" + DBConnection.database);
						productNames = new List<string>();
						foreach (string text2 in tableNamesAI)
						{
							string tableName = text2.Replace("AI", "");
							string item = null;
							bool productName = dbconnection.GetProductName(tableName, ref item);
							bool flag3 = productName;
							if (flag3)
							{
								productNames.Add(item);
							}
						}
						List<string> list = new List<string>();
						dbconnection.GetAllAIProductTableNames(list);
						bool flag4 = list.Count<string>() > productNames.Count<string>();
						if (flag4)
						{
							this.ShowOnlyValidProducts = true;
						}
						dbconnection.CloseConnection();
						bool flag5 = Directory.Exists(AIRestoreListProductsViewModel.tempBackupPath);
						if (flag5)
						{
							Directory.Delete(AIRestoreListProductsViewModel.tempBackupPath, true);
						}
					}
					else
					{
						Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(delegate()
						{
							Dialog.Show("Error", "No database found!", DialogStyle.Error, DialogButtons.OK);
						}));
					}
					Application.Current.Dispatcher.BeginInvoke(new Action(delegate()
					{
						Action<List<string>> onFinished2 = onFinished;
						if (onFinished2 != null)
						{
							onFinished2(productNames);
						}
					}), new object[0]);
				}
				catch (Exception ex)
				{
					Exception ex3;
					Exception ex2 = ex3;
					Exception ex = ex2;
					bool flag6 = Directory.Exists(AIRestoreListProductsViewModel.tempBackupPath);
					if (flag6)
					{
						Directory.Delete(AIRestoreListProductsViewModel.tempBackupPath, true);
					}
					Progress.CloseStaticProgress(false);
					Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(delegate()
					{
						Dialog.Show("Error", ex.Message, DialogStyle.Error, DialogButtons.OK);
					}));
				}
			});
			thread.Start();
		}

		// Token: 0x060000BC RID: 188 RVA: 0x000057F0 File Offset: 0x000039F0
		private void restore_ProductNamesExtracted(List<string> productNames)
		{
			Progress.CloseStaticProgress(false);
			bool flag = productNames != null;
			if (flag)
			{
				Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(delegate()
				{
					foreach (string item in productNames)
					{
						this.ProductNamesList.Add(item);
					}
				}));
			}
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00005848 File Offset: 0x00003A48
		private void restore_ThinkRestored()
		{
			bool flag = this.restoreThinkXO;
			if (flag)
			{
				this.parHandling.WriteValue(Definition.objectDetectionInferenceEnabled, "0");
			}
			Progress.CloseStaticProgress(false);
		}

		// Token: 0x040000FE RID: 254
		public static string tempBackupPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\tempBackupAI";

		// Token: 0x040000FF RID: 255
		private bool restoreThinkXA;

		// Token: 0x04000100 RID: 256
		private bool restoreThinkXO;

		// Token: 0x04000101 RID: 257
		private string backupPath;

		// Token: 0x04000102 RID: 258
		private ParameterHandling parHandling;

		// Token: 0x04000103 RID: 259
		private DBConnection dbConnection;

		// Token: 0x04000104 RID: 260
		private ProductManager productManager;

		// Token: 0x04000108 RID: 264
		private string selectedProduct;

		// Token: 0x04000109 RID: 265
		private ObservableCollection<string> productNamesList;

		// Token: 0x0400010A RID: 266
		private bool showOnlyValidProducts;
	}
}
