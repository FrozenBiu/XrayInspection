using System;
using System.IO;

namespace GUI_XRS
{
	// Token: 0x02000238 RID: 568
	public class WrappingStream : Stream
	{
		// Token: 0x06002B48 RID: 11080 RVA: 0x0011EBF8 File Offset: 0x0011CDF8
		public WrappingStream(Stream streamBase)
		{
			bool flag = streamBase == null;
			if (flag)
			{
				throw new ArgumentNullException("streamBase");
			}
			this.m_streamBase = streamBase;
		}

		// Token: 0x17000BD5 RID: 3029
		// (get) Token: 0x06002B49 RID: 11081 RVA: 0x0011EC28 File Offset: 0x0011CE28
		public override bool CanRead
		{
			get
			{
				return this.m_streamBase != null && this.m_streamBase.CanRead;
			}
		}

		// Token: 0x17000BD6 RID: 3030
		// (get) Token: 0x06002B4A RID: 11082 RVA: 0x0011EC50 File Offset: 0x0011CE50
		public override bool CanSeek
		{
			get
			{
				return this.m_streamBase != null && this.m_streamBase.CanSeek;
			}
		}

		// Token: 0x17000BD7 RID: 3031
		// (get) Token: 0x06002B4B RID: 11083 RVA: 0x0011EC78 File Offset: 0x0011CE78
		public override bool CanWrite
		{
			get
			{
				return this.m_streamBase != null && this.m_streamBase.CanWrite;
			}
		}

		// Token: 0x17000BD8 RID: 3032
		// (get) Token: 0x06002B4C RID: 11084 RVA: 0x0011ECA0 File Offset: 0x0011CEA0
		public override long Length
		{
			get
			{
				this.ThrowIfDisposed();
				return this.m_streamBase.Length;
			}
		}

		// Token: 0x17000BD9 RID: 3033
		// (get) Token: 0x06002B4D RID: 11085 RVA: 0x0011ECC4 File Offset: 0x0011CEC4
		// (set) Token: 0x06002B4E RID: 11086 RVA: 0x0011ECE8 File Offset: 0x0011CEE8
		public override long Position
		{
			get
			{
				this.ThrowIfDisposed();
				return this.m_streamBase.Position;
			}
			set
			{
				this.ThrowIfDisposed();
				this.m_streamBase.Position = value;
			}
		}

		// Token: 0x06002B4F RID: 11087 RVA: 0x0011ED00 File Offset: 0x0011CF00
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			this.ThrowIfDisposed();
			return this.m_streamBase.BeginRead(buffer, offset, count, callback, state);
		}

		// Token: 0x06002B50 RID: 11088 RVA: 0x0011ED2C File Offset: 0x0011CF2C
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			this.ThrowIfDisposed();
			return this.m_streamBase.BeginWrite(buffer, offset, count, callback, state);
		}

		// Token: 0x06002B51 RID: 11089 RVA: 0x0011ED58 File Offset: 0x0011CF58
		public override int EndRead(IAsyncResult asyncResult)
		{
			this.ThrowIfDisposed();
			return this.m_streamBase.EndRead(asyncResult);
		}

		// Token: 0x06002B52 RID: 11090 RVA: 0x0011ED7D File Offset: 0x0011CF7D
		public override void EndWrite(IAsyncResult asyncResult)
		{
			this.ThrowIfDisposed();
			this.m_streamBase.EndWrite(asyncResult);
		}

		// Token: 0x06002B53 RID: 11091 RVA: 0x0011ED94 File Offset: 0x0011CF94
		public override void Flush()
		{
			this.ThrowIfDisposed();
			this.m_streamBase.Flush();
		}

		// Token: 0x06002B54 RID: 11092 RVA: 0x0011EDAC File Offset: 0x0011CFAC
		public override int Read(byte[] buffer, int offset, int count)
		{
			this.ThrowIfDisposed();
			return this.m_streamBase.Read(buffer, offset, count);
		}

		// Token: 0x06002B55 RID: 11093 RVA: 0x0011EDD4 File Offset: 0x0011CFD4
		public override int ReadByte()
		{
			this.ThrowIfDisposed();
			return this.m_streamBase.ReadByte();
		}

		// Token: 0x06002B56 RID: 11094 RVA: 0x0011EDF8 File Offset: 0x0011CFF8
		public override long Seek(long offset, SeekOrigin origin)
		{
			this.ThrowIfDisposed();
			return this.m_streamBase.Seek(offset, origin);
		}

		// Token: 0x06002B57 RID: 11095 RVA: 0x0011EE1E File Offset: 0x0011D01E
		public override void SetLength(long value)
		{
			this.ThrowIfDisposed();
			this.m_streamBase.SetLength(value);
		}

		// Token: 0x06002B58 RID: 11096 RVA: 0x0011EE35 File Offset: 0x0011D035
		public override void Write(byte[] buffer, int offset, int count)
		{
			this.ThrowIfDisposed();
			this.m_streamBase.Write(buffer, offset, count);
		}

		// Token: 0x06002B59 RID: 11097 RVA: 0x0011EE4E File Offset: 0x0011D04E
		public override void WriteByte(byte value)
		{
			this.ThrowIfDisposed();
			this.m_streamBase.WriteByte(value);
		}

		// Token: 0x17000BDA RID: 3034
		// (get) Token: 0x06002B5A RID: 11098 RVA: 0x0011EE68 File Offset: 0x0011D068
		protected Stream WrappedStream
		{
			get
			{
				return this.m_streamBase;
			}
		}

		// Token: 0x06002B5B RID: 11099 RVA: 0x0011EE80 File Offset: 0x0011D080
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.m_streamBase = null;
			}
			base.Dispose(disposing);
		}

		// Token: 0x06002B5C RID: 11100 RVA: 0x0011EEA4 File Offset: 0x0011D0A4
		private void ThrowIfDisposed()
		{
			bool flag = this.m_streamBase == null;
			if (flag)
			{
				throw new ObjectDisposedException(base.GetType().Name);
			}
		}

		// Token: 0x04001B13 RID: 6931
		private Stream m_streamBase;
	}
}
