using System;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x020000ED RID: 237
	public class USBSave
	{
		// Token: 0x06001770 RID: 6000 RVA: 0x0008E638 File Offset: 0x0008C838
		public void SaveToUSB(XRSImage image, string usbdriveLetter, bool saveEdgePixelInPNG, string timestamp)
		{
			this.usbdriveLetter = usbdriveLetter;
			this.image = image;
			this.saveEdgePixelInPNG = saveEdgePixelInPNG;
			this.timestamp = timestamp;
			bool flag = Directory.Exists(usbdriveLetter + "\\");
			if (flag)
			{
				bool flag2 = image == null;
				if (!flag2)
				{
					this.cancel = false;
					string path;
					string text;
					this.CreateDefaultUSBFileNames(out path, out text);
					this.keyboard = new Keyboard("", Path.GetFileNameWithoutExtension(path), KeyboardType.Application);
					this.keyboard.TextInputKeyboard += this.keyboard_TextInputKeyboard;
					this.keyboard.ShowDialog();
				}
			}
			else
			{
				Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtNoUSB"), DialogStyle.Error);
			}
		}

		// Token: 0x06001771 RID: 6001 RVA: 0x0008E6F0 File Offset: 0x0008C8F0
		public void SaveToUSBWithName(XRSImage image, string usbdriveLetter, bool saveEdgePixelInPNG, string timestamp, string name)
		{
			this.usbdriveLetter = usbdriveLetter;
			this.image = image;
			this.saveEdgePixelInPNG = saveEdgePixelInPNG;
			this.timestamp = timestamp;
			bool flag = Directory.Exists(usbdriveLetter + "\\");
			if (flag)
			{
				bool flag2 = image == null;
				if (!flag2)
				{
					string path;
					string text;
					this.CreateDefaultUSBFileNames(out path, out text);
					this.cancel = true;
					this.keyboard = new Keyboard(name, Path.GetFileNameWithoutExtension(path), KeyboardType.Cancelable);
					this.keyboard.TextInputKeyboard += this.keyboard_TextInputKeyboard;
					this.keyboard.ShowDialog();
				}
			}
			else
			{
				Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtNoUSB"), DialogStyle.Error);
			}
		}

		// Token: 0x06001772 RID: 6002 RVA: 0x0008E7A4 File Offset: 0x0008C9A4
		private void CreateDefaultUSBFileNames(out string xriFilename, out string pngFilename)
		{
			bool flag = this.image.ProductName != null;
			if (flag)
			{
				bool flag2 = this.image.ProductNumber == 0U && this.image.ProductName.Equals("Reference Product");
				if (flag2)
				{
					xriFilename = string.Concat(new string[]
					{
						"\\RayconImages\\P-1_",
						this.image.ProductName,
						"\\",
						this.timestamp,
						".xri"
					});
					pngFilename = string.Concat(new string[]
					{
						"\\RayconImages\\P-1_",
						this.image.ProductName,
						"\\",
						this.timestamp,
						".png"
					});
				}
				else
				{
					xriFilename = string.Concat(new string[]
					{
						"\\RayconImages\\P",
						this.image.ProductNumber.ToString(),
						"_",
						this.image.ProductName,
						"\\",
						this.timestamp,
						".xri"
					});
					pngFilename = string.Concat(new string[]
					{
						"\\RayconImages\\P",
						this.image.ProductNumber.ToString(),
						"_",
						this.image.ProductName,
						"\\",
						this.timestamp,
						".png"
					});
				}
			}
			else
			{
				xriFilename = "\\RayconImages\\" + this.timestamp + ".xri";
				pngFilename = "\\RayconImages\\" + this.timestamp + ".png";
			}
			xriFilename = xriFilename.Replace(':', '-');
			pngFilename = pngFilename.Replace(':', '-');
			xriFilename = this.usbdriveLetter + xriFilename;
			pngFilename = this.usbdriveLetter + pngFilename;
		}

		// Token: 0x06001773 RID: 6003 RVA: 0x0008E994 File Offset: 0x0008CB94
		private void keyboard_TextInputKeyboard(object sender, KeyboardInputEventArgs e)
		{
			this.keyboard.TextInputKeyboard -= this.keyboard_TextInputKeyboard;
			bool flag = e.Text != "";
			if (flag)
			{
				bool flag2 = this.image.ProductNumber == 0U && this.image.ProductName.Equals("Reference Product");
				string text;
				string text2;
				if (flag2)
				{
					text = "\\RayconImages\\P-1_" + this.image.ProductName + "\\";
					text2 = "\\RayconImages\\P-1_" + this.image.ProductName + "\\";
				}
				else
				{
					text = string.Concat(new string[]
					{
						"\\RayconImages\\P",
						this.image.ProductNumber.ToString(),
						"_",
						this.image.ProductName,
						"\\"
					});
					text2 = string.Concat(new string[]
					{
						"\\RayconImages\\P",
						this.image.ProductNumber.ToString(),
						"_",
						this.image.ProductName,
						"\\"
					});
				}
				text = string.Concat(new string[]
				{
					this.usbdriveLetter,
					"\\",
					text,
					e.Text,
					".xri"
				});
				text2 = string.Concat(new string[]
				{
					this.usbdriveLetter,
					"\\",
					text2,
					e.Text,
					".png"
				});
				this.SaveToFile(text, text2);
			}
		}

		// Token: 0x06001774 RID: 6004 RVA: 0x0008EB3C File Offset: 0x0008CD3C
		private void SaveToFile(string xriFilename, string pngFilename)
		{
			FileInfo fileInfo = new FileInfo(xriFilename);
			bool exists = fileInfo.Exists;
			if (exists)
			{
				bool flag = Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("msgFileAlreadyExistent"), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Cancel;
				if (flag)
				{
					return;
				}
			}
			bool flag2 = Directory.Exists(this.usbdriveLetter);
			if (flag2)
			{
				Action <>9__1;
				Thread thread = new Thread(delegate()
				{
					try
					{
						this.saveProgressRunning = true;
						new Thread(new ThreadStart(this.UpdateProgress))
						{
							Name = "SaveProgress"
						}.Start();
						this.image.SaveToFile(xriFilename);
						FileStream fileStream = new FileStream(pngFilename, FileMode.Create);
						byte[] pngBytes = PngHelper.GetPngBytes(this.image, this.saveEdgePixelInPNG, this.timestamp);
						fileStream.Write(pngBytes, 0, pngBytes.Length);
						fileStream.Close();
						this.saveProgressRunning = false;
						Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
						{
							Dialog.Show(LocalizationManager.Localize("txtSuccess"), LocalizationManager.Localize("txtSaveSuccessful"), DialogStyle.Ok, DialogButtons.None, true);
						}));
					}
					catch (UnauthorizedAccessException)
					{
						Dispatcher dispatcher = Application.Current.Dispatcher;
						DispatcherPriority priority = DispatcherPriority.Normal;
						Action method;
						if ((method = <>9__1) == null)
						{
							method = (<>9__1 = delegate()
							{
								Dialog.Show(LocalizationManager.Localize("txtError"), string.Concat(new string[]
								{
									LocalizationManager.Localize("msgPathAccess"),
									" \"",
									Path.GetDirectoryName(xriFilename),
									"\" ",
									LocalizationManager.Localize("msgForbidden")
								}), DialogStyle.Error);
							});
						}
						dispatcher.Invoke(priority, method);
						this.saveProgressRunning = false;
					}
					catch (Exception ex)
					{
						Exception ex3;
						Exception ex2 = ex3;
						Exception ex = ex2;
						Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
						{
							Dialog.Show(LocalizationManager.Localize("txtError"), ex.Message, DialogStyle.Error);
						}));
						this.saveProgressRunning = false;
					}
				});
				thread.Start();
			}
			else
			{
				Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtNoUSB"), DialogStyle.Error);
			}
		}

		// Token: 0x06001775 RID: 6005 RVA: 0x0008EBEC File Offset: 0x0008CDEC
		private void UpdateProgress()
		{
			this.UpdateProgress(LocalizationManager.Localize("txtSavingImage") + "...");
		}

		// Token: 0x06001776 RID: 6006 RVA: 0x0008EC0C File Offset: 0x0008CE0C
		private void UpdateProgress(object obj)
		{
			string progText = null;
			Progress prog = null;
			int progValue = 0;
			bool flag = obj is string;
			if (flag)
			{
				progText = (string)obj;
			}
			Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
			{
				prog = new Progress((progText != null) ? progText : "", false);
				prog.Show();
			}));
			while (this.saveProgressRunning)
			{
				bool flag2 = prog != null;
				if (flag2)
				{
					Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
					{
						prog.progBar.Value = (double)progValue;
					}));
					progValue += 2;
					bool flag3 = progValue > 100;
					if (flag3)
					{
						progValue = 0;
					}
				}
				Thread.Sleep(50);
			}
			Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
			{
				prog.Close();
			}));
		}

		// Token: 0x04000F6C RID: 3948
		private Keyboard keyboard;

		// Token: 0x04000F6D RID: 3949
		private string usbdriveLetter = "";

		// Token: 0x04000F6E RID: 3950
		private XRSImage image;

		// Token: 0x04000F6F RID: 3951
		private bool saveProgressRunning;

		// Token: 0x04000F70 RID: 3952
		private bool saveEdgePixelInPNG;

		// Token: 0x04000F71 RID: 3953
		private string timestamp;

		// Token: 0x04000F72 RID: 3954
		private bool cancel = false;
	}
}
