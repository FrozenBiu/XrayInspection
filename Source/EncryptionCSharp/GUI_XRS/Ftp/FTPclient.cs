using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace GUI_XRS.Ftp
{
	// Token: 0x0200025C RID: 604
	public class FTPclient
	{
		// Token: 0x06002E81 RID: 11905 RVA: 0x00131A7C File Offset: 0x0012FC7C
		public FTPclient()
		{
		}

		// Token: 0x06002E82 RID: 11906 RVA: 0x00131ACC File Offset: 0x0012FCCC
		public FTPclient(string Hostname)
		{
			this._hostname = Hostname;
		}

		// Token: 0x06002E83 RID: 11907 RVA: 0x00131B20 File Offset: 0x0012FD20
		public FTPclient(string Hostname, string Username, string Password)
		{
			this._hostname = Hostname;
			this._username = Username;
			this._password = Password;
		}

		// Token: 0x06002E84 RID: 11908 RVA: 0x00131B84 File Offset: 0x0012FD84
		public List<string> ListDirectory(string directory)
		{
			FtpWebRequest request = this.GetRequest(this.GetDirectory(directory));
			request.Method = "NLST";
			string text = this.GetStringResponse(request);
			text = text.Replace("\n", "\r").TrimEnd(new char[]
			{
				'\r'
			});
			List<string> list = new List<string>();
			list.AddRange(text.Split(new char[]
			{
				'\r'
			}));
			return list;
		}

		// Token: 0x06002E85 RID: 11909 RVA: 0x00131BF8 File Offset: 0x0012FDF8
		public FTPdirectory ListDirectoryDetail(string directory)
		{
			FtpWebRequest request = this.GetRequest(this.GetDirectory(directory));
			request.Method = "LIST";
			string text = this.GetStringResponse(request);
			text = text.Replace("\n", "\r").TrimEnd(new char[]
			{
				'\r'
			});
			return new FTPdirectory(text, this._lastDirectory);
		}

		// Token: 0x06002E86 RID: 11910 RVA: 0x00131C5C File Offset: 0x0012FE5C
		public bool Upload(string localFilename, string targetFilename)
		{
			bool flag = !File.Exists(localFilename);
			if (flag)
			{
				throw new ApplicationException("File " + localFilename + " not found");
			}
			FileInfo fi = new FileInfo(localFilename);
			return this.Upload(fi, targetFilename);
		}

		// Token: 0x06002E87 RID: 11911 RVA: 0x00131CA4 File Offset: 0x0012FEA4
		public bool Upload(FileInfo fi, string targetFilename)
		{
			bool flag = targetFilename.Trim() == "";
			string str;
			if (flag)
			{
				str = this.CurrentDirectory + fi.Name;
			}
			else
			{
				bool flag2 = targetFilename.Contains("/");
				if (flag2)
				{
					str = this.AdjustDir(targetFilename);
				}
				else
				{
					str = this.CurrentDirectory + targetFilename;
				}
			}
			string uri = this.Hostname + str;
			FtpWebRequest request = this.GetRequest(uri);
			request.Method = "STOR";
			request.UseBinary = true;
			request.ContentLength = fi.Length;
			byte[] buffer = new byte[2048];
			using (FileStream fileStream = fi.OpenRead())
			{
				try
				{
					using (Stream requestStream = request.GetRequestStream())
					{
						int num;
						do
						{
							num = fileStream.Read(buffer, 0, 2048);
							requestStream.Write(buffer, 0, num);
						}
						while (num >= 2048);
						requestStream.Close();
					}
				}
				catch (Exception)
				{
				}
				finally
				{
					fileStream.Close();
				}
			}
			return true;
		}

		// Token: 0x06002E88 RID: 11912 RVA: 0x00131E08 File Offset: 0x00130008
		public bool Download(string sourceFilename, string localFilename, bool PermitOverwrite)
		{
			FileInfo targetFI = new FileInfo(localFilename);
			return this.Download(sourceFilename, targetFI, PermitOverwrite);
		}

		// Token: 0x06002E89 RID: 11913 RVA: 0x00131E2C File Offset: 0x0013002C
		public bool Download(FTPfileInfo file, string localFilename, bool PermitOverwrite)
		{
			return this.Download(file.FullName, localFilename, PermitOverwrite);
		}

		// Token: 0x06002E8A RID: 11914 RVA: 0x00131E4C File Offset: 0x0013004C
		public bool Download(FTPfileInfo file, FileInfo localFI, bool PermitOverwrite)
		{
			return this.Download(file.FullName, localFI, PermitOverwrite);
		}

		// Token: 0x06002E8B RID: 11915 RVA: 0x00131E6C File Offset: 0x0013006C
		public bool Download(string sourceFilename, FileInfo targetFI, bool PermitOverwrite)
		{
			return this.Download(sourceFilename, targetFI, PermitOverwrite, 2048);
		}

		// Token: 0x06002E8C RID: 11916 RVA: 0x00131E8C File Offset: 0x0013008C
		public bool Download(string sourceFilename, FileInfo targetFI, bool PermitOverwrite, int packetSize)
		{
			bool flag = targetFI.Exists && !PermitOverwrite;
			if (flag)
			{
				throw new ApplicationException("Target file already exists");
			}
			bool flag2 = sourceFilename.Trim() == "";
			if (flag2)
			{
				throw new ApplicationException("File not specified");
			}
			bool flag3 = sourceFilename.Contains("/");
			string text;
			if (flag3)
			{
				text = this.AdjustDir(sourceFilename);
			}
			else
			{
				text = this.CurrentDirectory + sourceFilename;
			}
			string uri = this.Hostname + text;
			FtpWebRequest request = this.GetRequest(uri);
			request.Method = "RETR";
			request.UseBinary = true;
			using (FtpWebResponse ftpWebResponse = (FtpWebResponse)request.GetResponse())
			{
				using (Stream responseStream = ftpWebResponse.GetResponseStream())
				{
					using (FileStream fileStream = targetFI.OpenWrite())
					{
						try
						{
							long fileSize = this.GetFileSize(text);
							long num = 0L;
							byte[] array = new byte[packetSize];
							int num2;
							do
							{
								num2 = responseStream.Read(array, 0, array.Length);
								fileStream.Write(array, 0, num2);
								num += (long)num2;
								this.OnDownloadProgressChanged(fileSize, num);
							}
							while (num2 != 0);
							responseStream.Close();
							fileStream.Flush();
							fileStream.Close();
						}
						catch (Exception)
						{
							fileStream.Close();
							targetFI.Delete();
							throw;
						}
					}
					responseStream.Close();
				}
				ftpWebResponse.Close();
			}
			return true;
		}

		// Token: 0x06002E8D RID: 11917 RVA: 0x00132054 File Offset: 0x00130254
		public bool FtpDelete(string filename)
		{
			string uri = this.Hostname + this.GetFullPath(filename);
			FtpWebRequest request = this.GetRequest(uri);
			request.Method = "DELE";
			try
			{
				string stringResponse = this.GetStringResponse(request);
			}
			catch (Exception)
			{
				return false;
			}
			return true;
		}

		// Token: 0x06002E8E RID: 11918 RVA: 0x001320B0 File Offset: 0x001302B0
		public bool FtpFileExists(string filename)
		{
			bool flag;
			try
			{
				long fileSize = this.GetFileSize(filename);
				flag = true;
			}
			catch (Exception ex)
			{
				bool flag2 = ex is WebException;
				if (!flag2)
				{
					throw;
				}
				bool flag3 = ex.Message.Contains("550");
				if (!flag3)
				{
					throw;
				}
				flag = false;
			}
			return flag;
		}

		// Token: 0x06002E8F RID: 11919 RVA: 0x00132110 File Offset: 0x00130310
		public long GetFileSize(string filename)
		{
			bool flag = filename.Contains("/");
			string str;
			if (flag)
			{
				str = this.AdjustDir(filename);
			}
			else
			{
				str = this.CurrentDirectory + filename;
			}
			string uri = this.Hostname + str;
			FtpWebRequest request = this.GetRequest(uri);
			request.Method = "SIZE";
			string stringResponse = this.GetStringResponse(request);
			return this.GetSize(request);
		}

		// Token: 0x06002E90 RID: 11920 RVA: 0x00132180 File Offset: 0x00130380
		public bool FtpRename(string sourceFilename, string newName)
		{
			string fullPath = this.GetFullPath(sourceFilename);
			bool flag = !this.FtpFileExists(fullPath);
			if (flag)
			{
				throw new FileNotFoundException("File " + fullPath + " not found");
			}
			string fullPath2 = this.GetFullPath(newName);
			bool flag2 = fullPath2 == fullPath;
			if (flag2)
			{
				throw new ApplicationException("Source and target are the same");
			}
			bool flag3 = this.FtpFileExists(fullPath2);
			if (flag3)
			{
				throw new ApplicationException("Target file " + fullPath2 + " already exists");
			}
			string uri = this.Hostname + fullPath;
			FtpWebRequest request = this.GetRequest(uri);
			request.Method = "RENAME";
			request.RenameTo = fullPath2;
			try
			{
				string stringResponse = this.GetStringResponse(request);
			}
			catch (Exception)
			{
				return false;
			}
			return true;
		}

		// Token: 0x06002E91 RID: 11921 RVA: 0x00132258 File Offset: 0x00130458
		public bool FtpCreateDirectory(string dirpath)
		{
			string uri = this.Hostname + this.AdjustDir(dirpath);
			FtpWebRequest request = this.GetRequest(uri);
			request.Method = "MKD";
			try
			{
				string stringResponse = this.GetStringResponse(request);
			}
			catch (Exception)
			{
				return false;
			}
			return true;
		}

		// Token: 0x06002E92 RID: 11922 RVA: 0x001322B4 File Offset: 0x001304B4
		public bool FtpDeleteDirectory(string dirpath)
		{
			string uri = this.Hostname + this.AdjustDir(dirpath);
			FtpWebRequest request = this.GetRequest(uri);
			request.Method = "RMD";
			try
			{
				string stringResponse = this.GetStringResponse(request);
			}
			catch (Exception)
			{
				return false;
			}
			return true;
		}

		// Token: 0x06002E93 RID: 11923 RVA: 0x00132310 File Offset: 0x00130510
		private FtpWebRequest GetRequest(string URI)
		{
			FtpWebRequest ftpWebRequest = (FtpWebRequest)WebRequest.Create(URI);
			ftpWebRequest.Credentials = this.GetCredentials();
			ftpWebRequest.KeepAlive = false;
			return ftpWebRequest;
		}

		// Token: 0x06002E94 RID: 11924 RVA: 0x00132344 File Offset: 0x00130544
		private ICredentials GetCredentials()
		{
			return new NetworkCredential(this.Username, this.Password);
		}

		// Token: 0x06002E95 RID: 11925 RVA: 0x00132368 File Offset: 0x00130568
		private string GetFullPath(string file)
		{
			bool flag = file.Contains("/");
			string text;
			if (flag)
			{
				text = this.AdjustDir(file);
			}
			else
			{
				text = this.CurrentDirectory + file;
			}
			return text;
		}

		// Token: 0x06002E96 RID: 11926 RVA: 0x001323A4 File Offset: 0x001305A4
		private string AdjustDir(string path)
		{
			return (path.StartsWith("/") ? "" : "/").ToString() + path;
		}

		// Token: 0x06002E97 RID: 11927 RVA: 0x001323DC File Offset: 0x001305DC
		private string GetDirectory(string directory)
		{
			bool flag = directory == "";
			string text;
			if (flag)
			{
				text = this.Hostname + this.CurrentDirectory;
				this._lastDirectory = this.CurrentDirectory;
			}
			else
			{
				bool flag2 = !directory.StartsWith("/");
				if (flag2)
				{
					throw new ApplicationException("Directory should start with /");
				}
				text = this.Hostname + directory;
				this._lastDirectory = directory;
			}
			return text;
		}

		// Token: 0x06002E98 RID: 11928 RVA: 0x00132454 File Offset: 0x00130654
		private string GetStringResponse(FtpWebRequest ftp)
		{
			string text = "";
			using (FtpWebResponse ftpWebResponse = (FtpWebResponse)ftp.GetResponse())
			{
				long contentLength = ftpWebResponse.ContentLength;
				using (Stream responseStream = ftpWebResponse.GetResponseStream())
				{
					using (StreamReader streamReader = new StreamReader(responseStream))
					{
						text = streamReader.ReadToEnd();
						streamReader.Close();
					}
					responseStream.Close();
				}
				ftpWebResponse.Close();
			}
			return text;
		}

		// Token: 0x06002E99 RID: 11929 RVA: 0x00132508 File Offset: 0x00130708
		private long GetSize(FtpWebRequest ftp)
		{
			long contentLength;
			using (FtpWebResponse ftpWebResponse = (FtpWebResponse)ftp.GetResponse())
			{
				contentLength = ftpWebResponse.ContentLength;
				ftpWebResponse.Close();
			}
			return contentLength;
		}

		// Token: 0x17000C6C RID: 3180
		// (get) Token: 0x06002E9A RID: 11930 RVA: 0x00132554 File Offset: 0x00130754
		// (set) Token: 0x06002E9B RID: 11931 RVA: 0x00132595 File Offset: 0x00130795
		public string Hostname
		{
			get
			{
				bool flag = this._hostname.StartsWith("ftp://");
				string text;
				if (flag)
				{
					text = this._hostname;
				}
				else
				{
					text = "ftp://" + this._hostname;
				}
				return text;
			}
			set
			{
				this._hostname = value;
			}
		}

		// Token: 0x17000C6D RID: 3181
		// (get) Token: 0x06002E9C RID: 11932 RVA: 0x001325A0 File Offset: 0x001307A0
		// (set) Token: 0x06002E9D RID: 11933 RVA: 0x001325D1 File Offset: 0x001307D1
		public string Username
		{
			get
			{
				return (this._username == "") ? "anonymous" : this._username;
			}
			set
			{
				this._username = value;
			}
		}

		// Token: 0x17000C6E RID: 3182
		// (get) Token: 0x06002E9E RID: 11934 RVA: 0x001325DC File Offset: 0x001307DC
		// (set) Token: 0x06002E9F RID: 11935 RVA: 0x001325F4 File Offset: 0x001307F4
		public string Password
		{
			get
			{
				return this._password;
			}
			set
			{
				this._password = value;
			}
		}

		// Token: 0x17000C6F RID: 3183
		// (get) Token: 0x06002EA0 RID: 11936 RVA: 0x00132600 File Offset: 0x00130800
		// (set) Token: 0x06002EA1 RID: 11937 RVA: 0x00132640 File Offset: 0x00130840
		public string CurrentDirectory
		{
			get
			{
				return this._currentDirectory + (this._currentDirectory.EndsWith("/") ? "" : "/").ToString();
			}
			set
			{
				bool flag = !value.StartsWith("/");
				if (flag)
				{
					throw new ApplicationException("Directory should start with /");
				}
				this._currentDirectory = value;
			}
		}

		// Token: 0x14000109 RID: 265
		// (add) Token: 0x06002EA2 RID: 11938 RVA: 0x00132674 File Offset: 0x00130874
		// (remove) Token: 0x06002EA3 RID: 11939 RVA: 0x001326AC File Offset: 0x001308AC
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event EventHandler<FtpDownloadEventArgs> DownloadProgressChanged;

		// Token: 0x06002EA4 RID: 11940 RVA: 0x001326E4 File Offset: 0x001308E4
		protected virtual void OnDownloadProgressChanged(long totalBytes, long downloadedBytes)
		{
			EventHandler<FtpDownloadEventArgs> downloadProgressChanged = this.DownloadProgressChanged;
			bool flag = downloadProgressChanged != null;
			if (flag)
			{
				downloadProgressChanged(this, new FtpDownloadEventArgs(totalBytes, downloadedBytes));
			}
		}

		// Token: 0x06002EA5 RID: 11941 RVA: 0x00132710 File Offset: 0x00130910
		private void sendCommand(string command)
		{
			byte[] bytes = Encoding.ASCII.GetBytes((command + "\r\n").ToCharArray());
			this.clientSocket.Send(bytes, bytes.Length, SocketFlags.None);
			this.readResponse();
		}

		// Token: 0x06002EA6 RID: 11942 RVA: 0x00132754 File Offset: 0x00130954
		private void readResponse()
		{
			this.message = "";
			this.result = this.readLine();
			bool flag = this.result.Length > 3;
			if (flag)
			{
				this.resultCode = int.Parse(this.result.Substring(0, 3));
			}
			else
			{
				this.result = null;
			}
		}

		// Token: 0x06002EA7 RID: 11943 RVA: 0x001327AC File Offset: 0x001309AC
		public void SetFileAttributes(string fileName, string attributes)
		{
			this.clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			this.clientSocket.ReceiveTimeout = 10000;
			this.clientSocket.SendTimeout = 10000;
			IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse(this.Hostname.Remove(0, 6)), 21);
			this.clientSocket.Connect(remoteEP);
			this.sendCommand("USER " + this.Username);
			this.sendCommand("PASS " + this.Password);
			this.sendCommand("SITE CHMOD " + attributes + " " + fileName);
			this.clientSocket.Close();
		}

		// Token: 0x06002EA8 RID: 11944 RVA: 0x00132864 File Offset: 0x00130A64
		private string readLine()
		{
			bool flag;
			do
			{
				this.clientBytes = this.clientSocket.Receive(this.clientBuffer, this.clientBuffer.Length, SocketFlags.None);
				this.message += Encoding.ASCII.GetString(this.clientBuffer, 0, this.clientBytes);
				flag = (this.clientBytes < this.clientBuffer.Length);
			}
			while (!flag);
			string[] array = this.message.Split(new char[]
			{
				'\n'
			});
			bool flag2 = this.message.Length > 2;
			if (flag2)
			{
				this.message = array[array.Length - 2];
			}
			else
			{
				this.message = array[0];
			}
			bool flag3 = this.message.Length > 4 && !this.message.Substring(3, 1).Equals(" ");
			string text;
			if (flag3)
			{
				text = this.readLine();
			}
			else
			{
				text = this.message;
			}
			return text;
		}

		// Token: 0x04001D43 RID: 7491
		private string _lastDirectory = "";

		// Token: 0x04001D44 RID: 7492
		private string _hostname;

		// Token: 0x04001D45 RID: 7493
		private string _username;

		// Token: 0x04001D46 RID: 7494
		private string _password;

		// Token: 0x04001D47 RID: 7495
		private string _currentDirectory = "/";

		// Token: 0x04001D49 RID: 7497
		private Socket clientSocket;

		// Token: 0x04001D4A RID: 7498
		private int clientBytes;

		// Token: 0x04001D4B RID: 7499
		private byte[] clientBuffer = new byte[512];

		// Token: 0x04001D4C RID: 7500
		private string message;

		// Token: 0x04001D4D RID: 7501
		private string result = "";

		// Token: 0x04001D4E RID: 7502
		private int resultCode = 0;

		// Token: 0x04001D4F RID: 7503
		private const int ftpResultOffset = 4;
	}
}
