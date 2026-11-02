using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using Common;
using DatabaseConnection;

namespace GUI_XRS
{
	// Token: 0x0200005E RID: 94
	public class ComissioningOverviewViewModel : INotifyPropertyChanged
	{
		// Token: 0x14000037 RID: 55
		// (add) Token: 0x0600095B RID: 2395 RVA: 0x00039B04 File Offset: 0x00037D04
		// (remove) Token: 0x0600095C RID: 2396 RVA: 0x00039B3C File Offset: 0x00037D3C
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x0600095D RID: 2397 RVA: 0x00039B71 File Offset: 0x00037D71
		// (set) Token: 0x0600095E RID: 2398 RVA: 0x00039B79 File Offset: 0x00037D79
		public RelayCommand CleanUpCommand { get; set; }

		// Token: 0x0600095F RID: 2399 RVA: 0x00039B82 File Offset: 0x00037D82
		public ComissioningOverviewViewModel(DBConnection dbConnection, ParameterHandling parHandling)
		{
			this.parHandling = parHandling;
			this.dbConnection = dbConnection;
			this.setOverview();
			this.CleanUpCommand = new RelayCommand(new Action<object>(this.CleanUp));
		}

		// Token: 0x06000960 RID: 2400 RVA: 0x00039BBC File Offset: 0x00037DBC
		private void setOverview()
		{
			ComissioningOverviewViewModel.ComissioningOverviewList = new List<ComissioningOverviewParams>();
			ComissioningOverviewParams comissioningOverviewParams = new ComissioningOverviewParams();
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			List<string> collection = new List<string>();
			List<string> collection2 = new List<string>();
			List<string> collection3 = new List<string>();
			List<string> collection4 = new List<string>();
			List<string> list3 = new List<string>();
			List<string> list4 = new List<string>();
			List<string> list5 = new List<string>();
			string item = "";
			string item2 = "";
			string item3 = "";
			string item4 = "";
			string item5 = "";
			string item6 = "";
			string item7 = "";
			string item8 = "";
			string item9 = "";
			string item10 = "";
			string item11 = "";
			string text = "";
			string item12 = "";
			string text2 = "";
			string item13 = "";
			int num = 0;
			this.dbConnection.SetSelectionComissioningWithStatusInfo();
			this.dbConnection.GetSelectedColumnsComissioning(ref num, ref list, ref list2, ref collection, ref collection2, ref collection3, ref collection4);
			list3 = list;
			list3.AddRange(collection);
			list3.AddRange(collection3);
			list4 = list2;
			list4.AddRange(collection2);
			list4.AddRange(collection4);
			this.dbConnection.GetStatusInfoComissioning(ref item, ref item2, ref item3, ref item4, ref item5, ref item6, ref item7, ref item8, ref item9, ref item11, ref item10, ref item12, ref text2, ref item13);
			list5.Add(item13);
			list5.Add(item);
			list5.Add("GUI_Settings");
			list5.Add(item6);
			list5.Add(item7);
			list5.Add(item8);
			list5.Add(item9);
			list5.Add(item10);
			list5.Add(item11);
			list5.Add("Database_Settings");
			list5.Add(item2);
			list5.Add(item3);
			list5.Add(item4);
			list5.Add(item5);
			list5.Add(item12);
			list5.Add(text2);
			for (int i = 0; i < list3.Count; i++)
			{
				bool flag = list3[i] != "";
				if (flag)
				{
					comissioningOverviewParams = new ComissioningOverviewParams();
					comissioningOverviewParams.StrPathes = list3[i];
					comissioningOverviewParams.StrValuesComissioning = list4[i];
					bool flag2 = list3[i].Contains("/hardware/rejecter/bulkflaps/Enable") && text2 == "1";
					if (flag2)
					{
						comissioningOverviewParams.StrValuesComissioning = "0";
						list4[i] = "0";
					}
					bool flag3 = list3[i].Contains("defaultProduct");
					if (flag3)
					{
						bool flag4 = list3[i].Contains("subTable");
						if (flag4)
						{
							string str = list3[i].Substring(24);
							this.dbConnection.GetParameterValue(str + "/Value", "AppDefault000_1", ref text);
						}
						else
						{
							string str = list3[i].Substring(25);
							this.dbConnection.GetParameterValue(str + "/Value", "AppDefault000", ref text);
						}
					}
					else
					{
						text = this.parHandling.ReadPar(list3[i] + "/Value");
					}
					comissioningOverviewParams.StrValuesActual = text;
					comissioningOverviewParams.DifferentCellRed = this.setCellBackgroundToRed(list3[i], text, list4[i]);
					comissioningOverviewParams.DefaultRedCell = false;
					ComissioningOverviewViewModel.ComissioningOverviewList.Add(comissioningOverviewParams);
				}
			}
			ComissioningOverviewViewModel.ComissioningOverviewList.Sort((ComissioningOverviewParams x, ComissioningOverviewParams y) => x.StrPathes.CompareTo(y.StrPathes));
			for (int j = 0; j < ComissioningOverviewViewModel.ComissioningOverviewList.Count; j++)
			{
				ComissioningOverviewViewModel.ComissioningOverviewList[j].StrID = Convert.ToString(j + 1);
				bool flag5 = j < list5.Count;
				if (flag5)
				{
					ComissioningOverviewViewModel.ComissioningOverviewList[j].StrStatus = list5[j];
				}
				else
				{
					ComissioningOverviewViewModel.ComissioningOverviewList[j].StrStatus = "";
				}
			}
		}

		// Token: 0x06000961 RID: 2401 RVA: 0x0003A008 File Offset: 0x00038208
		private bool setCellBackgroundToRed(string path, string valueactual, string valueSavedDB)
		{
			bool result = false;
			bool flag = valueactual != valueSavedDB;
			if (flag)
			{
				result = true;
			}
			return result;
		}

		// Token: 0x06000962 RID: 2402 RVA: 0x0003A02C File Offset: 0x0003822C
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x06000963 RID: 2403 RVA: 0x0003A05C File Offset: 0x0003825C
		private void CleanUp(object obj)
		{
			bool flag = ComissioningOverviewViewModel.ComissioningOverviewList != null;
			if (flag)
			{
				ComissioningOverviewViewModel.ComissioningOverviewList.Clear();
				ComissioningOverviewViewModel.ComissioningOverviewList = null;
			}
		}

		// Token: 0x04000662 RID: 1634
		private ParameterHandling parHandling;

		// Token: 0x04000663 RID: 1635
		private DBConnection dbConnection;

		// Token: 0x04000665 RID: 1637
		public static List<ComissioningOverviewParams> ComissioningOverviewList;
	}
}
