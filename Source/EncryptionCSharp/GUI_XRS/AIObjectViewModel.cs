using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Controls;
using Common;
using DatabaseConnection;
using Definitions;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x0200000A RID: 10
	internal class AIObjectViewModel
	{
		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600005C RID: 92 RVA: 0x000036BD File Offset: 0x000018BD
		// (set) Token: 0x0600005D RID: 93 RVA: 0x000036C5 File Offset: 0x000018C5
		public List<AIObjectClass> ObjectDetectionClasses { get; set; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600005E RID: 94 RVA: 0x000036CE File Offset: 0x000018CE
		// (set) Token: 0x0600005F RID: 95 RVA: 0x000036D6 File Offset: 0x000018D6
		public RelayCommand TestSettingsCommand { get; set; }

		// Token: 0x06000060 RID: 96 RVA: 0x000036E0 File Offset: 0x000018E0
		public AIObjectViewModel(ParameterHandling parHandling, StoreImageData imageArchiver, ImageDataTransfer imageTransfer, ProductManager productManager, DBConnection dbConnection)
		{
			this.parHandling = parHandling;
			this.imageArchiver = imageArchiver;
			this.imageTransfer = imageTransfer;
			this.dbConnection = dbConnection;
			this.productManager = productManager;
			List<string> list;
			this.GetClassNames(out list);
			this.ObjectDetectionClasses = new List<AIObjectClass>();
			for (int i = 0; i < Definition.nAIObjectClasses; i++)
			{
				this.ObjectDetectionClasses.Add(new AIObjectClass(parHandling, productManager, i + 1, list[i]));
			}
			this.TestSettingsCommand = new RelayCommand(new Action<object>(this.TestSettings));
			this.btnTestSettings = new Button();
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00003784 File Offset: 0x00001984
		public void TestSettings(object obj)
		{
			bool flag = true;
			bool flag2 = this.parHandling.ReadPar(Definition.objectDetectionInferenceEnabled) == "0";
			if (flag2)
			{
				flag = (Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("msgThinkXOEnableInference"), DialogStyle.Info, DialogButtons.OKCancel) == Dialog.DialogResults.Ok);
				bool flag3 = flag;
				if (flag3)
				{
					this.parHandling.WriteValue(Definition.objectDetectionInferenceEnabled, "1");
					LogbookManager.AddEntry(SSTEntryCode.AIDetectionActivated, 3, this.productManager.CurrentLoadedProduct.ProductNumber, this.productManager.CurrentLoadedProduct.ProductName);
					Thread.Sleep(2500);
				}
			}
			bool flag4 = flag;
			if (flag4)
			{
				CtrlAIObjectSimulation ctrlAIObjectSimulation = new CtrlAIObjectSimulation(this.imageArchiver, this.imageTransfer, this.parHandling, this.dbConnection, this.productManager);
				EventHandler handler = null;
				handler = delegate(object sender, EventArgs args)
				{
					foreach (AIObjectClass aiobjectClass in this.ObjectDetectionClasses)
					{
						aiobjectClass.UpdateConfThreshold();
					}
					ctrlAIObjectSimulation.UpdateConfThreshold -= handler;
				};
				ctrlAIObjectSimulation.UpdateConfThreshold += handler;
				Subwindow.Show(ctrlAIObjectSimulation, SubwindowButtons.Back, ctrlAIObjectSimulation.PanelButtons, ctrlAIObjectSimulation.UiElementRightAligned);
			}
		}

		// Token: 0x06000062 RID: 98 RVA: 0x000038C4 File Offset: 0x00001AC4
		public void CleanUp()
		{
			foreach (AIObjectClass aiobjectClass in this.ObjectDetectionClasses)
			{
				aiobjectClass.CleanUp();
			}
			this.ObjectDetectionClasses.Clear();
			this.btnTestSettings = null;
			this.TestSettingsCommand = null;
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00003938 File Offset: 0x00001B38
		private void GetClassNames(out List<string> classNames)
		{
			string tableName = "";
			this.dbConnection.GetCurrentProductTableNameAI(ref tableName);
			string text = ModelTransfer.GetArchiveDirectory(tableName);
			text = Path.Combine(text, AIObjectViewModel.fileClassNames);
			classNames = new List<string>();
			bool flag = !File.Exists(text);
			if (flag)
			{
				for (int i = 0; i < Definition.nAIObjectClasses; i++)
				{
					classNames.Add("-");
				}
			}
			else
			{
				string[] array = File.ReadAllLines(text);
				foreach (string text2 in array)
				{
					classNames.Add(text2.Trim());
				}
				while (classNames.Count < Definition.nAIObjectClasses)
				{
					classNames.Add("-");
				}
			}
		}

		// Token: 0x040000C3 RID: 195
		public static readonly string fileClassNames = "classes.names";

		// Token: 0x040000C4 RID: 196
		private ParameterHandling parHandling;

		// Token: 0x040000C5 RID: 197
		private StoreImageData imageArchiver;

		// Token: 0x040000C6 RID: 198
		private ImageDataTransfer imageTransfer;

		// Token: 0x040000C7 RID: 199
		private DBConnection dbConnection;

		// Token: 0x040000C8 RID: 200
		private ProductManager productManager;

		// Token: 0x040000C9 RID: 201
		public Button btnTestSettings;
	}
}
