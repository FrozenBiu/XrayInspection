using System;
using System.Runtime.InteropServices;
using System.Windows;

namespace GUI_XRS
{
	// Token: 0x02000171 RID: 369
	internal class APIStructDefinitions
	{
		// Token: 0x060021F8 RID: 8696
		[DllImport("user32")]
		internal static extern bool GetMonitorInfo(IntPtr hMonitor, APIStructDefinitions.MONITORINFO lpmi);

		// Token: 0x060021F9 RID: 8697
		[DllImport("User32")]
		internal static extern IntPtr MonitorFromWindow(IntPtr handle, int flags);

		// Token: 0x0200032E RID: 814
		public struct POINT
		{
			// Token: 0x060036B6 RID: 14006 RVA: 0x001550AB File Offset: 0x001532AB
			public POINT(int x, int y)
			{
				this.x = x;
				this.y = y;
			}

			// Token: 0x0400211C RID: 8476
			public int x;

			// Token: 0x0400211D RID: 8477
			public int y;
		}

		// Token: 0x0200032F RID: 815
		public struct MINMAXINFO
		{
			// Token: 0x0400211E RID: 8478
			public APIStructDefinitions.POINT ptReserved;

			// Token: 0x0400211F RID: 8479
			public APIStructDefinitions.POINT ptMaxSize;

			// Token: 0x04002120 RID: 8480
			public APIStructDefinitions.POINT ptMaxPosition;

			// Token: 0x04002121 RID: 8481
			public APIStructDefinitions.POINT ptMinTrackSize;

			// Token: 0x04002122 RID: 8482
			public APIStructDefinitions.POINT ptMaxTrackSize;
		}

		// Token: 0x02000330 RID: 816
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public class MONITORINFO
		{
			// Token: 0x04002123 RID: 8483
			public int cbSize = Marshal.SizeOf(typeof(APIStructDefinitions.MONITORINFO));

			// Token: 0x04002124 RID: 8484
			public APIStructDefinitions.RECT rcMonitor = default(APIStructDefinitions.RECT);

			// Token: 0x04002125 RID: 8485
			public APIStructDefinitions.RECT rcWork = default(APIStructDefinitions.RECT);

			// Token: 0x04002126 RID: 8486
			public int dwFlags = 0;
		}

		// Token: 0x02000331 RID: 817
		public struct RECT
		{
			// Token: 0x17001122 RID: 4386
			// (get) Token: 0x060036B8 RID: 14008 RVA: 0x001550FC File Offset: 0x001532FC
			public int Width
			{
				get
				{
					return Math.Abs(this.right - this.left);
				}
			}

			// Token: 0x17001123 RID: 4387
			// (get) Token: 0x060036B9 RID: 14009 RVA: 0x00155120 File Offset: 0x00153320
			public int Height
			{
				get
				{
					return this.bottom - this.top;
				}
			}

			// Token: 0x060036BA RID: 14010 RVA: 0x0015513F File Offset: 0x0015333F
			public RECT(int left, int top, int right, int bottom)
			{
				this.left = left;
				this.top = top;
				this.right = right;
				this.bottom = bottom;
			}

			// Token: 0x060036BB RID: 14011 RVA: 0x0015515F File Offset: 0x0015335F
			public RECT(APIStructDefinitions.RECT rcSrc)
			{
				this.left = rcSrc.left;
				this.top = rcSrc.top;
				this.right = rcSrc.right;
				this.bottom = rcSrc.bottom;
			}

			// Token: 0x17001124 RID: 4388
			// (get) Token: 0x060036BC RID: 14012 RVA: 0x00155194 File Offset: 0x00153394
			public bool IsEmpty
			{
				get
				{
					return this.left >= this.right || this.top >= this.bottom;
				}
			}

			// Token: 0x060036BD RID: 14013 RVA: 0x001551C8 File Offset: 0x001533C8
			public override string ToString()
			{
				bool flag = this == APIStructDefinitions.RECT.Empty;
				string result;
				if (flag)
				{
					result = "RECT {Empty}";
				}
				else
				{
					result = string.Concat(new string[]
					{
						"RECT { left : ",
						this.left.ToString(),
						" / top : ",
						this.top.ToString(),
						" / right : ",
						this.right.ToString(),
						" / bottom : ",
						this.bottom.ToString(),
						" }"
					});
				}
				return result;
			}

			// Token: 0x060036BE RID: 14014 RVA: 0x00155264 File Offset: 0x00153464
			public override bool Equals(object obj)
			{
				bool flag = !(obj is Rect);
				return !flag && this == (APIStructDefinitions.RECT)obj;
			}

			// Token: 0x060036BF RID: 14015 RVA: 0x0015529C File Offset: 0x0015349C
			public override int GetHashCode()
			{
				return this.left.GetHashCode() + this.top.GetHashCode() + this.right.GetHashCode() + this.bottom.GetHashCode();
			}

			// Token: 0x060036C0 RID: 14016 RVA: 0x001552E0 File Offset: 0x001534E0
			public static bool operator ==(APIStructDefinitions.RECT rect1, APIStructDefinitions.RECT rect2)
			{
				return rect1.left == rect2.left && rect1.top == rect2.top && rect1.right == rect2.right && rect1.bottom == rect2.bottom;
			}

			// Token: 0x060036C1 RID: 14017 RVA: 0x00155330 File Offset: 0x00153530
			public static bool operator !=(APIStructDefinitions.RECT rect1, APIStructDefinitions.RECT rect2)
			{
				return !(rect1 == rect2);
			}

			// Token: 0x04002127 RID: 8487
			public int left;

			// Token: 0x04002128 RID: 8488
			public int top;

			// Token: 0x04002129 RID: 8489
			public int right;

			// Token: 0x0400212A RID: 8490
			public int bottom;

			// Token: 0x0400212B RID: 8491
			public static readonly APIStructDefinitions.RECT Empty = default(APIStructDefinitions.RECT);
		}
	}
}
