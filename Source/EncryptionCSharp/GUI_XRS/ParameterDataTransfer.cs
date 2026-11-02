using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x02000104 RID: 260
	public class ParameterDataTransfer : Network
	{
		// Token: 0x1400007E RID: 126
		// (add) Token: 0x060018A1 RID: 6305 RVA: 0x00093A4C File Offset: 0x00091C4C
		// (remove) Token: 0x060018A2 RID: 6306 RVA: 0x00093A84 File Offset: 0x00091C84
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<string, int, int> ParameterRead;

		// Token: 0x060018A3 RID: 6307 RVA: 0x00093AB9 File Offset: 0x00091CB9
		public ParameterDataTransfer() : this(Definition.defaultElinosIP)
		{
		}

		// Token: 0x060018A4 RID: 6308 RVA: 0x00093AC8 File Offset: 0x00091CC8
		public ParameterDataTransfer(string ipAddress) : base(ipAddress, ParameterDataTransfer.parameterPort, "ParameterDataTransfer")
		{
			base.SetReceiveTimeout(5000);
		}

		// Token: 0x060018A5 RID: 6309 RVA: 0x00093AF0 File Offset: 0x00091CF0
		public XRSParameter GetParameterData()
		{
			XRSParameter xrsparameter = null;
			this.bufferHeader = base.ReceiveBytes(0, (int)XRSParameter.parameterHeaderSize);
			bool flag = this.bufferHeader != null;
			if (flag)
			{
				xrsparameter = new XRSParameter(this.bufferHeader);
				xrsparameter.Data0 = base.ReceiveBytes(0, (int)xrsparameter.Length0);
				xrsparameter.Data1 = base.ReceiveBytes(0, (int)xrsparameter.Length1);
			}
			return xrsparameter;
		}

		// Token: 0x060018A6 RID: 6310 RVA: 0x00093B5C File Offset: 0x00091D5C
		public void SendRequest(byte version, byte command, byte[] data0, byte[] data1)
		{
			bool connectionEstablished = base.ConnectionEstablished;
			if (connectionEstablished)
			{
				XRSParameter xrsparameter = new XRSParameter();
				xrsparameter.Version = version;
				xrsparameter.Command = command;
				xrsparameter.Type = 0;
				bool flag = data0 != null;
				if (flag)
				{
					xrsparameter.Length0 = (short)data0.Length;
				}
				bool flag2 = data1 != null;
				if (flag2)
				{
					xrsparameter.Length1 = (short)data1.Length;
				}
				xrsparameter.Data0 = data0;
				xrsparameter.Data1 = data1;
				byte[] array = xrsparameter.CreateHeader();
				bool flag3 = array.Length != 0;
				if (flag3)
				{
					base.SendBytes(array, SocketFlags.None);
					bool flag4 = xrsparameter.Length0 != 0;
					if (flag4)
					{
						base.SendBytes(data0, SocketFlags.None);
					}
					bool flag5 = xrsparameter.Length1 != 0;
					if (flag5)
					{
						base.SendBytes(xrsparameter.Data1, SocketFlags.None);
					}
				}
			}
		}

		// Token: 0x060018A7 RID: 6311 RVA: 0x00093C24 File Offset: 0x00091E24
		public string ReadEntry(string path)
		{
			XRSParameter xrsparameter = new XRSParameter();
			byte[] data = new byte[0];
			string result = "";
			this.SendRequest(this.version, 10, Encoding.ASCII.GetBytes(path.ToString()), data);
			xrsparameter = this.GetParameterData();
			bool flag = xrsparameter != null;
			if (flag)
			{
				bool flag2 = xrsparameter.Data0 != null;
				if (flag2)
				{
					result = Encoding.Default.GetString(xrsparameter.Data0);
				}
			}
			return result;
		}

		// Token: 0x060018A8 RID: 6312 RVA: 0x00093CA0 File Offset: 0x00091EA0
		public void WriteEntry(string path, string value)
		{
			XRSParameter xrsparameter = new XRSParameter();
			xrsparameter.Version = this.version;
			xrsparameter.Command = 11;
			xrsparameter.Type = 1;
			byte[] bytes = Encoding.ASCII.GetBytes(path);
			byte[] bytes2 = Encoding.Default.GetBytes(value);
			xrsparameter.Length0 = (short)bytes.Length;
			xrsparameter.Length1 = (short)bytes2.Length;
			xrsparameter.Data0 = bytes;
			xrsparameter.Data1 = bytes2;
			this.SendRequest(this.version, 11, bytes, bytes2);
			XRSParameter parameterData = this.GetParameterData();
		}

		// Token: 0x060018A9 RID: 6313 RVA: 0x00093D28 File Offset: 0x00091F28
		public string[] GetSubEntries(byte[] path, int numSubentries)
		{
			string[] array = new string[numSubentries + 1];
			for (int i = 0; i < numSubentries; i++)
			{
				this.SendRequest(this.version, 3, path, BitConverter.GetBytes(i));
				XRSParameter parameterData = this.GetParameterData();
				array[i] = Encoding.ASCII.GetString(parameterData.Data0);
			}
			return array;
		}

		// Token: 0x060018AA RID: 6314 RVA: 0x00093D88 File Offset: 0x00091F88
		public int GetTotalEntryNumber()
		{
			byte[] data = null;
			byte[] data2 = null;
			int result = 0;
			this.SendRequest(this.version, 0, data, data2);
			XRSParameter parameterData = this.GetParameterData();
			bool flag = parameterData != null && parameterData.Data0 != null;
			if (flag)
			{
				string @string = Encoding.ASCII.GetString(parameterData.Data0);
				bool flag2 = !int.TryParse(@string, out result);
				if (flag2)
				{
				}
			}
			return result;
		}

		// Token: 0x060018AB RID: 6315 RVA: 0x00093DFC File Offset: 0x00091FFC
		public List<string> GetEntries(int entryNumberTotal)
		{
			List<string> list = new List<string>();
			for (int i = 0; i < entryNumberTotal; i++)
			{
				byte[] data = null;
				this.SendRequest(this.version, 1, Encoding.ASCII.GetBytes(i.ToString()), data);
				XRSParameter parameterData = this.GetParameterData();
				bool flag = parameterData != null && parameterData.Data0 != null;
				if (flag)
				{
					list.Add(Encoding.ASCII.GetString(parameterData.Data0));
					this.OnParameterRead(list[list.Count - 1], i, entryNumberTotal);
				}
			}
			return list;
		}

		// Token: 0x060018AC RID: 6316 RVA: 0x00093E9C File Offset: 0x0009209C
		public void DisableWriteProtection()
		{
			byte[] data = null;
			byte[] data2 = null;
			this.SendRequest(this.version, 20, data, data2);
			XRSParameter parameterData = this.GetParameterData();
		}

		// Token: 0x060018AD RID: 6317 RVA: 0x00093EC8 File Offset: 0x000920C8
		public void EnableWriteProtection()
		{
			byte[] data = null;
			byte[] data2 = null;
			this.SendRequest(this.version, 21, data, data2);
			XRSParameter parameterData = this.GetParameterData();
		}

		// Token: 0x060018AE RID: 6318 RVA: 0x00093EF4 File Offset: 0x000920F4
		public void Reboot()
		{
			byte[] data = null;
			byte[] data2 = null;
			this.SendRequest(this.version, 22, data, data2);
			XRSParameter parameterData = this.GetParameterData();
		}

		// Token: 0x060018AF RID: 6319 RVA: 0x00093F20 File Offset: 0x00092120
		protected virtual void OnParameterRead(string parameter, int index, int nEntries)
		{
			Action<string, int, int> parameterRead = this.ParameterRead;
			bool flag = parameterRead != null;
			if (flag)
			{
				parameterRead(parameter, index, nEntries);
			}
		}

		// Token: 0x04001023 RID: 4131
		private readonly byte version = 1;

		// Token: 0x04001024 RID: 4132
		public static readonly int parameterPort = 1377;

		// Token: 0x04001025 RID: 4133
		private byte[] bufferHeader;
	}
}
