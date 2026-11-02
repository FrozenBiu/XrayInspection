using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Common;
using DatabaseConnection;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x0200002A RID: 42
	public class AuditCheckDocContentViewModel : INotifyPropertyChanged
	{
		// Token: 0x14000020 RID: 32
		// (add) Token: 0x06000504 RID: 1284 RVA: 0x00022F78 File Offset: 0x00021178
		// (remove) Token: 0x06000505 RID: 1285 RVA: 0x00022FB0 File Offset: 0x000211B0
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x06000506 RID: 1286 RVA: 0x00022FE5 File Offset: 0x000211E5
		// (set) Token: 0x06000507 RID: 1287 RVA: 0x00022FED File Offset: 0x000211ED
		public RelayCommand CleanUpCommand { get; set; }

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x06000508 RID: 1288 RVA: 0x00022FF6 File Offset: 0x000211F6
		// (set) Token: 0x06000509 RID: 1289 RVA: 0x00022FFE File Offset: 0x000211FE
		public BoundLocalizedText StrData { get; set; }

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x0600050A RID: 1290 RVA: 0x00023007 File Offset: 0x00021207
		// (set) Token: 0x0600050B RID: 1291 RVA: 0x0002300F File Offset: 0x0002120F
		public BoundLocalizedText StrDaily { get; set; }

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x0600050C RID: 1292 RVA: 0x00023018 File Offset: 0x00021218
		// (set) Token: 0x0600050D RID: 1293 RVA: 0x00023020 File Offset: 0x00021220
		public BoundLocalizedText StrWeekly { get; set; }

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x0600050E RID: 1294 RVA: 0x00023029 File Offset: 0x00021229
		// (set) Token: 0x0600050F RID: 1295 RVA: 0x00023031 File Offset: 0x00021231
		public BoundLocalizedText StrCustomIntervall { get; set; }

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x06000510 RID: 1296 RVA: 0x0002303A File Offset: 0x0002123A
		// (set) Token: 0x06000511 RID: 1297 RVA: 0x00023042 File Offset: 0x00021242
		public BoundLocalizedText StrNextDate { get; set; }

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x06000512 RID: 1298 RVA: 0x0002304B File Offset: 0x0002124B
		// (set) Token: 0x06000513 RID: 1299 RVA: 0x00023053 File Offset: 0x00021253
		public BoundLocalizedText StrInterval { get; set; }

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x06000514 RID: 1300 RVA: 0x0002305C File Offset: 0x0002125C
		// (set) Token: 0x06000515 RID: 1301 RVA: 0x00023064 File Offset: 0x00021264
		public BoundLocalizedText StrTimeNextDoc { get; set; }

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x06000516 RID: 1302 RVA: 0x0002306D File Offset: 0x0002126D
		// (set) Token: 0x06000517 RID: 1303 RVA: 0x00023075 File Offset: 0x00021275
		public BoundLocalizedText StrDays { get; set; }

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x06000518 RID: 1304 RVA: 0x0002307E File Offset: 0x0002127E
		// (set) Token: 0x06000519 RID: 1305 RVA: 0x00023086 File Offset: 0x00021286
		public BoundLocalizedText StrNewDoc { get; set; }

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x0600051A RID: 1306 RVA: 0x0002308F File Offset: 0x0002128F
		// (set) Token: 0x0600051B RID: 1307 RVA: 0x00023097 File Offset: 0x00021297
		public BoundLocalizedText StrFileNameLogo { get; set; }

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x0600051C RID: 1308 RVA: 0x000230A0 File Offset: 0x000212A0
		// (set) Token: 0x0600051D RID: 1309 RVA: 0x000230A8 File Offset: 0x000212A8
		public BoundLocalizedText StrUploadLogo { get; set; }

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x0600051E RID: 1310 RVA: 0x000230B1 File Offset: 0x000212B1
		// (set) Token: 0x0600051F RID: 1311 RVA: 0x000230B9 File Offset: 0x000212B9
		public BoundLocalizedText StrMaximum { get; set; }

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x06000520 RID: 1312 RVA: 0x000230C4 File Offset: 0x000212C4
		// (set) Token: 0x06000521 RID: 1313 RVA: 0x000230DC File Offset: 0x000212DC
		public string NextDate
		{
			get
			{
				return this.nextDate;
			}
			set
			{
				this.nextDate = value;
				this.OnPropertyChanged("NextDate");
				this.dbConnection.SetNextDateDocumentation(this.NextDate);
			}
		}

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x06000522 RID: 1314 RVA: 0x00023104 File Offset: 0x00021304
		// (set) Token: 0x06000523 RID: 1315 RVA: 0x0002311C File Offset: 0x0002131C
		public string Data
		{
			get
			{
				return this.data;
			}
			set
			{
				this.data = value;
				this.OnPropertyChanged("Data");
				this.dbConnection.SetAuditCheckData(this.data);
			}
		}

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x06000524 RID: 1316 RVA: 0x00023144 File Offset: 0x00021344
		// (set) Token: 0x06000525 RID: 1317 RVA: 0x0002315C File Offset: 0x0002135C
		public int IntervalDays
		{
			get
			{
				return this.intervalDays;
			}
			set
			{
				bool flag = value > 7;
				if (flag)
				{
					value = 7;
				}
				this.intervalDays = value;
				this.NextDate = this.LastAuditCheckDateTime.AddDays((double)value).ToString("dd.MM.yyyy");
				this.dbConnection.SetDocIntervallDays(this.intervalDays);
				this.OnPropertyChanged("IntervalDays");
			}
		}

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x06000526 RID: 1318 RVA: 0x000231BC File Offset: 0x000213BC
		// (set) Token: 0x06000527 RID: 1319 RVA: 0x000231D4 File Offset: 0x000213D4
		public bool IsManually
		{
			get
			{
				return this.isManually;
			}
			set
			{
				this.isManually = value;
				if (value)
				{
					this.IntervalDays = this.IntervalDays;
				}
				this.OnPropertyChanged("IsManually");
				this.dbConnection.SetDocIsManually(this.isManually);
			}
		}

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x06000528 RID: 1320 RVA: 0x0002321C File Offset: 0x0002141C
		// (set) Token: 0x06000529 RID: 1321 RVA: 0x00023234 File Offset: 0x00021434
		public bool IsDaily
		{
			get
			{
				return this.isDaily;
			}
			set
			{
				this.isDaily = value;
				if (value)
				{
					this.NextDate = this.LastAuditCheckDateTime.AddDays(1.0).ToString("dd.MM.yyyy");
				}
				this.OnPropertyChanged("IsDaily");
				this.dbConnection.SetDocIsDaily(this.isDaily);
			}
		}

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x0600052A RID: 1322 RVA: 0x00023298 File Offset: 0x00021498
		// (set) Token: 0x0600052B RID: 1323 RVA: 0x000232B0 File Offset: 0x000214B0
		public bool IsWeekly
		{
			get
			{
				return this.isWeekly;
			}
			set
			{
				this.isWeekly = value;
				if (value)
				{
					int num = (int)(DayOfWeek.Monday - this.LastAuditCheckDateTime.DayOfWeek);
					bool flag = num < 0;
					if (flag)
					{
						num += 7;
					}
					this.NextDate = this.LastAuditCheckDateTime.AddDays((double)num).ToString("dd.MM.yyyy");
				}
				this.OnPropertyChanged("IsWeekly");
				this.dbConnection.SetDocIsWeekly(this.isWeekly);
			}
		}

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x0600052C RID: 1324 RVA: 0x00023328 File Offset: 0x00021528
		// (set) Token: 0x0600052D RID: 1325 RVA: 0x00023340 File Offset: 0x00021540
		public bool USBListVisible
		{
			get
			{
				return this.usbListVisible;
			}
			set
			{
				this.usbListVisible = value;
				this.OnPropertyChanged("USBListVisible");
			}
		}

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x0600052E RID: 1326 RVA: 0x00023356 File Offset: 0x00021556
		// (set) Token: 0x0600052F RID: 1327 RVA: 0x0002335E File Offset: 0x0002155E
		public ObservableCollection<Document> ImagesUSB { get; set; }

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x06000530 RID: 1328 RVA: 0x00023368 File Offset: 0x00021568
		// (set) Token: 0x06000531 RID: 1329 RVA: 0x00023380 File Offset: 0x00021580
		public string ImageName
		{
			get
			{
				return this.imageName;
			}
			set
			{
				this.imageName = value;
				this.OnPropertyChanged("ImageName");
			}
		}

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x06000532 RID: 1330 RVA: 0x00023398 File Offset: 0x00021598
		// (set) Token: 0x06000533 RID: 1331 RVA: 0x000233B0 File Offset: 0x000215B0
		public Document SelectedImage
		{
			get
			{
				return this.selectedImage;
			}
			set
			{
				this.selectedImage = value;
				this.OnPropertyChanged("SelectedImage");
			}
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x000233C8 File Offset: 0x000215C8
		public AuditCheckDocContentViewModel(DBConnection dBCon, AccessRightsManager accessMan)
		{
			this.dbConnection = dBCon;
			this.accessManager = accessMan;
			this.ImagesUSB = new ObservableCollection<Document>();
			this.CleanUpCommand = new RelayCommand(new Action<object>(this.CleanUp));
			this.RegisterLocalizedText();
			this.InitData();
			this.getLogoName();
		}

		// Token: 0x06000535 RID: 1333 RVA: 0x0002342C File Offset: 0x0002162C
		public void GetDocuments()
		{
			this.ImagesUSB.Clear();
			string path = "";
			this.dbConnection.GetUsbDriveLetter(ref path);
			bool flag = Directory.Exists(path);
			if (flag)
			{
				string[] files = Directory.GetFiles(path);
				int num = 0;
				files = Directory.GetFiles(path);
				for (int i = 0; i < files.Length; i++)
				{
					bool flag2 = files[i].EndsWith(Definition.pngFileEnding);
					if (flag2)
					{
						FileInfo fileInfo = new FileInfo(files[i]);
						bool flag3 = fileInfo.Length <= 40960L;
						if (flag3)
						{
							num++;
							Document item = new Document(files[i], num);
							this.ImagesUSB.Add(item);
						}
					}
				}
				bool flag4 = this.ImagesUSB.Count > 0 && this.SelectedImage == null;
				if (flag4)
				{
					this.SelectedImage = this.ImagesUSB[0];
					this.USBListVisible = true;
				}
				else
				{
					Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtNoImagesFound"), DialogStyle.Warning, DialogButtons.OK, true);
					this.SelectedImage = null;
				}
			}
			else
			{
				Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtNoUSB"), DialogStyle.Warning, DialogButtons.OK, true);
			}
		}

		// Token: 0x06000536 RID: 1334 RVA: 0x00023578 File Offset: 0x00021778
		public void AddFileToLocal(object obj)
		{
			string path = "";
			this.dbConnection.GetUsbDriveLetter(ref path);
			bool flag = this.SelectedImage != null && Directory.Exists(path);
			if (flag)
			{
				string fileName = Path.GetFileName(this.SelectedImage.FilePath);
				string text = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/AuditcheckDocumentation/Icon/";
				string text2 = text + "\\" + fileName;
				bool flag2 = !Directory.Exists(text);
				if (flag2)
				{
					Directory.CreateDirectory(text);
				}
				else
				{
					string[] files = Directory.GetFiles(text);
					foreach (string path2 in files)
					{
						File.Delete(path2);
					}
				}
				bool flag3 = !File.Exists(text2) || Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("msgFileAlreadyExistent"), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
				if (flag3)
				{
					File.Copy(this.SelectedImage.FilePath, text2, true);
				}
				this.USBListVisible = false;
				this.getLogoName();
			}
		}

		// Token: 0x06000537 RID: 1335 RVA: 0x00023690 File Offset: 0x00021890
		internal void DeleteLogo()
		{
			string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/AuditcheckDocumentation/Icon/";
			bool flag = Directory.Exists(path);
			if (flag)
			{
				string[] files = Directory.GetFiles(path);
				foreach (string path2 in files)
				{
					File.Delete(path2);
				}
			}
			this.getLogoName();
		}

		// Token: 0x06000538 RID: 1336 RVA: 0x000236FC File Offset: 0x000218FC
		public void getLogoName()
		{
			string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/AuditcheckDocumentation/Icon/";
			bool flag = Directory.Exists(path);
			if (flag)
			{
				string[] files = Directory.GetFiles(path);
				bool flag2 = files.Length != 0;
				if (flag2)
				{
					foreach (string text in files)
					{
						bool flag3 = text.EndsWith(Definition.pngFileEnding);
						if (flag3)
						{
							this.ImageName = text.Split(new char[]
							{
								'\\',
								'/'
							}).Last<string>();
						}
					}
				}
				else
				{
					this.ImageName = "-";
				}
			}
			else
			{
				this.ImageName = "-";
			}
		}

		// Token: 0x06000539 RID: 1337 RVA: 0x000237BC File Offset: 0x000219BC
		private void InitData()
		{
			string text = "";
			int num = 0;
			bool flag = false;
			this.dbConnection.GetAuditCheckData(ref text);
			this.Data = text;
			this.dbConnection.GetNextDateDocumentation(ref text);
			this.NextDate = text;
			this.dbConnection.GetDocIntervallDays(ref num);
			this.IntervalDays = num;
			this.dbConnection.GetLastDateDocumenation(ref text);
			bool flag2 = text.Length >= 10;
			int day;
			int month;
			int num2;
			if (flag2)
			{
				day = Convert.ToInt32(text.Substring(0, 2));
				month = Convert.ToInt32(text.Substring(3, 2));
				num2 = Convert.ToInt32(text.Substring(6, 4));
			}
			else
			{
				bool flag3 = text.Length >= 6;
				if (flag3)
				{
					day = Convert.ToInt32(text.Substring(0, 1));
					month = Convert.ToInt32(text.Substring(2, 2));
					num2 = Convert.ToInt32(text.Substring(5, 4));
				}
				else
				{
					num2 = DateTime.Now.Year;
					month = DateTime.Now.Month;
					day = DateTime.Now.Day;
				}
			}
			bool flag4 = num2 == 2999;
			if (flag4)
			{
				this.LastAuditCheckDateTime = DateTime.Now.Date;
				this.LastDate = DateTime.Now.Date.ToString("dd.MM.yyyy");
				this.dbConnection.SetLastDateDocumentation(this.LastDate);
			}
			else
			{
				this.LastDate = text;
				this.LastAuditCheckDateTime = new DateTime(num2, month, day);
			}
			this.dbConnection.GetDocIsDaily(ref flag);
			this.IsDaily = flag;
			this.dbConnection.GetDocIsManually(ref flag);
			this.IsManually = flag;
			this.dbConnection.GetDocIsWeekly(ref flag);
			this.IsWeekly = flag;
		}

		// Token: 0x0600053A RID: 1338 RVA: 0x00023994 File Offset: 0x00021B94
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x000239C4 File Offset: 0x00021BC4
		private void RegisterLocalizedText()
		{
			this.StrCustomIntervall = new BoundLocalizedText("txtPerInterval");
			this.StrDaily = new BoundLocalizedText("txtDaily");
			this.StrData = new BoundLocalizedText("txtData");
			this.StrDays = new BoundLocalizedText("txtDays");
			this.StrInterval = new BoundLocalizedText("txtAuditInterval");
			this.StrWeekly = new BoundLocalizedText("txtWeekly");
			this.StrTimeNextDoc = new BoundLocalizedText("txtNextDoc");
			this.StrNextDate = new BoundLocalizedText("txtDateNextDoc");
			this.StrNewDoc = new BoundLocalizedText("txtStartNewDocument");
			this.StrUploadLogo = new BoundLocalizedText("txtUploadLogo");
			this.StrFileNameLogo = new BoundLocalizedText("txtFileNameLogo");
			this.StrMaximum = new BoundLocalizedText("txtMaximum");
		}

		// Token: 0x0600053C RID: 1340 RVA: 0x00023AA0 File Offset: 0x00021CA0
		public void DeregisterLocalizedText()
		{
			MainWindow.localizationManager.Deregister(this.StrCustomIntervall);
			MainWindow.localizationManager.Deregister(this.StrDaily);
			MainWindow.localizationManager.Deregister(this.StrWeekly);
			MainWindow.localizationManager.Deregister(this.StrDays);
			MainWindow.localizationManager.Deregister(this.StrTimeNextDoc);
			MainWindow.localizationManager.Deregister(this.StrNextDate);
			MainWindow.localizationManager.Deregister(this.StrData);
			MainWindow.localizationManager.Deregister(this.StrInterval);
			MainWindow.localizationManager.Deregister(this.StrNewDoc);
			MainWindow.localizationManager.Deregister(this.StrUploadLogo);
			MainWindow.localizationManager.Deregister(this.StrFileNameLogo);
			MainWindow.localizationManager.Deregister(this.StrMaximum);
		}

		// Token: 0x0600053D RID: 1341 RVA: 0x00023B7C File Offset: 0x00021D7C
		public void CleanUp(object obj)
		{
			bool flag = this.ImagesUSB != null;
			if (flag)
			{
				this.ImagesUSB.Clear();
				this.ImagesUSB = null;
			}
			this.DeregisterLocalizedText();
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x00023BB4 File Offset: 0x00021DB4
		public void ShowEditKeyboard()
		{
			this.keyboard = new Keyboard("", "", KeyboardType.Application);
			this.keyboard.HasEnterButton = true;
			this.keyboard.TextInputKeyboard += this.keyboard_TextInputKeyboard;
			this.keyboard.ShowDialog();
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x00023C0C File Offset: 0x00021E0C
		public void ShowEditNumKeyboard()
		{
			this.numKeyboard = new NumKeyboard("", this.IntervalDays.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.keyboard_TextInputNumKeyboard;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x00023C60 File Offset: 0x00021E60
		private void keyboard_TextInputKeyboard(object sender, KeyboardInputEventArgs e)
		{
			bool flag = this.Data != e.Text;
			if (flag)
			{
				this.Data = e.Text;
			}
			this.keyboard.TextInputKeyboard -= this.keyboard_TextInputKeyboard;
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x00023CA8 File Offset: 0x00021EA8
		private void keyboard_TextInputNumKeyboard(object sender, KeyboardInputEventArgs e)
		{
			int num = int.Parse(e.Text);
			bool flag = num < 1 || num > 7;
			if (flag)
			{
				Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtWrongInput"), DialogStyle.Warning, DialogButtons.OK);
			}
			else
			{
				bool flag2 = num != this.IntervalDays;
				if (flag2)
				{
					this.IntervalDays = num;
				}
			}
			this.numKeyboard.TextInputKeyboard -= this.keyboard_TextInputNumKeyboard;
		}

		// Token: 0x040003C1 RID: 961
		private DBConnection dbConnection;

		// Token: 0x040003C2 RID: 962
		private AccessRightsManager accessManager;

		// Token: 0x040003C3 RID: 963
		private Keyboard keyboard;

		// Token: 0x040003D0 RID: 976
		private string nextDate;

		// Token: 0x040003D1 RID: 977
		private string data;

		// Token: 0x040003D2 RID: 978
		private int intervalDays;

		// Token: 0x040003D3 RID: 979
		private bool isManually;

		// Token: 0x040003D4 RID: 980
		private bool isDaily;

		// Token: 0x040003D5 RID: 981
		private bool isWeekly;

		// Token: 0x040003D6 RID: 982
		private bool usbListVisible = false;

		// Token: 0x040003D7 RID: 983
		private NumKeyboard numKeyboard;

		// Token: 0x040003D8 RID: 984
		private DateTime LastAuditCheckDateTime;

		// Token: 0x040003D9 RID: 985
		private string LastDate;

		// Token: 0x040003DB RID: 987
		private string imageName;

		// Token: 0x040003DC RID: 988
		private Document selectedImage;
	}
}
