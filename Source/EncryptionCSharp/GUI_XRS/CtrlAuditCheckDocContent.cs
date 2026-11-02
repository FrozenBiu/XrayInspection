using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using DatabaseConnection;

namespace GUI_XRS
{
	// Token: 0x02000029 RID: 41
	public class CtrlAuditCheckDocContent : UserControl, IComponentConnector, IStyleConnector
	{
		// Token: 0x060004F7 RID: 1271 RVA: 0x000229C1 File Offset: 0x00020BC1
		public CtrlAuditCheckDocContent(DBConnection dBCon, AccessRightsManager manager)
		{
			this.InitializeComponent();
			this.viewModel = new AuditCheckDocContentViewModel(dBCon, manager);
			base.DataContext = this.viewModel;
			this.accessManager = manager;
			this.dbConnection = dBCon;
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x000229FA File Offset: 0x00020BFA
		private void IVPControl_Unloaded(object sender, RoutedEventArgs e)
		{
			this.viewModel.DeregisterLocalizedText();
			BindingOperations.ClearAllBindings(this);
			base.DataContext = null;
			GC.Collect();
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x00022A20 File Offset: 0x00020C20
		private void TextBox_MouseDown(object sender, MouseButtonEventArgs e)
		{
			bool flag = e.Source is TextBox;
			if (flag)
			{
				this.viewModel.ShowEditKeyboard();
			}
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x00022A50 File Offset: 0x00020C50
		private void TextBox_MouseDown_Number(object sender, MouseButtonEventArgs e)
		{
			bool flag = e.Source is TextBox;
			if (flag)
			{
				this.viewModel.ShowEditNumKeyboard();
			}
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x00022A80 File Offset: 0x00020C80
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			bool flag = Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("txtStartNewDocument"), DialogStyle.Info, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
			if (flag)
			{
				string str = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/AuditcheckDocumentation/CurrentDoc/";
				string text = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/AuditcheckDocumentation/OtherDocs/";
				bool flag2 = File.Exists(str + "AuditCheckCurrent.pdf");
				if (flag2)
				{
					bool flag3 = !Directory.Exists(text);
					if (flag3)
					{
						Directory.CreateDirectory(text);
					}
					string text2 = "";
					this.dbConnection.GetSerialNumber(ref text2);
					File.Copy(str + "AuditCheckCurrent.pdf", string.Concat(new string[]
					{
						text,
						"AuditCheck_",
						DateTime.Now.ToString("yyyyMMdd_HHmmss").Replace(".", "_").Replace("/", "_").Replace(":", "_"),
						"_",
						text2,
						".pdf"
					}));
					File.Delete(str + "AuditCheckCurrent.pdf");
					this.UpdateDateDocumentation();
					this.dbConnection.SetYPositionAudit(0);
					DirectoryInfo directoryInfo = new DirectoryInfo(text);
					FileInfo[] files = directoryInfo.GetFiles();
					long num = files.Sum((FileInfo f) => f.Length) / 1024L;
					Array.Sort<FileInfo>(files, (FileInfo f1, FileInfo f2) => f1.Name.CompareTo(f2.Name));
					while (num > 15000L)
					{
						try
						{
							files.First<FileInfo>().Delete();
							files = directoryInfo.GetFiles();
							num = files.Sum((FileInfo f) => f.Length) / 1024L;
						}
						catch
						{
							break;
						}
					}
				}
			}
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x00022CAC File Offset: 0x00020EAC
		private void Button_Import_Click(object sender, RoutedEventArgs e)
		{
			this.viewModel.GetDocuments();
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x00022CBB File Offset: 0x00020EBB
		private void Button_Cancel_Click(object sender, RoutedEventArgs e)
		{
			this.viewModel.DeleteLogo();
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x00022CCC File Offset: 0x00020ECC
		public void UpdateDateDocumentation()
		{
			this.dbConnection.SetLastDateDocumentation(DateTime.Now.ToString("dd.MM.yyyy"));
			bool flag = false;
			this.dbConnection.GetDocIsManually(ref flag);
			bool flag2 = flag;
			if (flag2)
			{
				int num = 0;
				this.dbConnection.GetDocIntervallDays(ref num);
				this.dbConnection.SetNextDateDocumentation(DateTime.Now.AddDays((double)num).ToString("dd.MM.yyyy"));
			}
			else
			{
				this.dbConnection.GetDocIsDaily(ref flag);
				bool flag3 = flag;
				if (flag3)
				{
					this.dbConnection.SetNextDateDocumentation(DateTime.Now.AddDays(1.0).ToString("dd.MM.yyyy"));
				}
				else
				{
					this.dbConnection.SetNextDateDocumentation(DateTime.Now.AddDays(7.0).ToString("dd.MM.yyyy"));
				}
			}
		}

		// Token: 0x060004FF RID: 1279 RVA: 0x00022DC4 File Offset: 0x00020FC4
		private void ListBoxItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			this.viewModel.AddFileToLocal(sender);
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x00022DD4 File Offset: 0x00020FD4
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/auditchecknew/ctrlauditcheckdoccontent.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x00022E0C File Offset: 0x0002100C
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		internal Delegate _CreateDelegate(Type delegateType, string handler)
		{
			return Delegate.CreateDelegate(delegateType, this, handler);
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x00022E28 File Offset: 0x00021028
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				((CtrlAuditCheckDocContent)target).Unloaded += this.IVPControl_Unloaded;
				break;
			case 2:
				this.DataTextBox = (TextBox)target;
				this.DataTextBox.MouseDown += this.TextBox_MouseDown;
				break;
			case 3:
				((TextBox)target).MouseDown += this.TextBox_MouseDown_Number;
				break;
			case 4:
				((Button)target).Click += this.Button_Click;
				break;
			case 5:
				((Button)target).Click += this.Button_Cancel_Click;
				break;
			case 6:
				((Button)target).Click += this.Button_Import_Click;
				break;
			case 7:
				this.ListBoxDocuments = (ListBox)target;
				break;
			default:
				this._contentLoaded = true;
				break;
			}
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x00022F24 File Offset: 0x00021124
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IStyleConnector.Connect(int connectionId, object target)
		{
			if (connectionId == 8)
			{
				EventSetter eventSetter = new EventSetter();
				eventSetter.Event = Control.MouseDoubleClickEvent;
				eventSetter.Handler = new MouseButtonEventHandler(this.ListBoxItem_MouseDoubleClick);
				((Style)target).Setters.Add(eventSetter);
			}
		}

		// Token: 0x040003B9 RID: 953
		private AuditCheckDocContentViewModel viewModel;

		// Token: 0x040003BA RID: 954
		private AccessRightsManager accessManager;

		// Token: 0x040003BB RID: 955
		private DBConnection dbConnection;

		// Token: 0x040003BC RID: 956
		internal TextBox DataTextBox;

		// Token: 0x040003BD RID: 957
		internal ListBox ListBoxDocuments;

		// Token: 0x040003BE RID: 958
		private bool _contentLoaded;
	}
}
