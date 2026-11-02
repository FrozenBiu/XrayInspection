using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using GUI_XRS.Properties;

namespace GUI_XRS
{
	// Token: 0x0200007E RID: 126
	public partial class Signature : Form
	{
		// Token: 0x170003C7 RID: 967
		// (get) Token: 0x06000C6D RID: 3181 RVA: 0x0004A43B File Offset: 0x0004863B
		// (set) Token: 0x06000C6E RID: 3182 RVA: 0x0004A443 File Offset: 0x00048643
		public string SignInArea { get; set; }

		// Token: 0x170003C8 RID: 968
		// (get) Token: 0x06000C6F RID: 3183 RVA: 0x0004A44C File Offset: 0x0004864C
		// (set) Token: 0x06000C70 RID: 3184 RVA: 0x0004A454 File Offset: 0x00048654
		public MemoryStream SignatureEnd { get; set; }

		// Token: 0x06000C71 RID: 3185 RVA: 0x0004A460 File Offset: 0x00048660
		public Signature()
		{
			base.SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
			this.InitializeComponent();
			this.pen = new Pen(Brushes.Black, (float)(base.Width = 5));
			this.StrSignInArea = new BoundLocalizedText("StrSignInArea");
			this.textBox1.Text = this.StrSignInArea.LocalizedText;
		}

		// Token: 0x06000C72 RID: 3186 RVA: 0x0004A4DA File Offset: 0x000486DA
		protected override void OnPaint(PaintEventArgs pe)
		{
			base.OnPaint(pe);
		}

		// Token: 0x06000C73 RID: 3187 RVA: 0x0004A4E8 File Offset: 0x000486E8
		private void Signature_Load(object sender, EventArgs e)
		{
			base.Paint += this.Signature_Paint;
			base.MouseDown += this.Signature_MouseDown;
			base.MouseUp += this.Signature_MouseUp;
			base.MouseMove += this.Signature_MouseMove;
			base.MouseLeave += this.Signature_MouseLeave;
			base.FormClosing += this.Signature_FormClosing;
			this.button1.Enabled = false;
		}

		// Token: 0x06000C74 RID: 3188 RVA: 0x0004A578 File Offset: 0x00048778
		private void Signature_FormClosing(object sender, FormClosingEventArgs e)
		{
			MainWindow.localizationManager.Deregister(this.StrSignInArea);
			bool flag = this.signature != null;
			if (flag)
			{
				this.signature.Dispose();
			}
			base.Paint -= this.Signature_Paint;
			base.MouseDown -= this.Signature_MouseDown;
			base.MouseUp -= this.Signature_MouseUp;
			base.MouseMove -= this.Signature_MouseMove;
			base.MouseLeave -= this.Signature_MouseLeave;
			base.FormClosing -= this.Signature_FormClosing;
		}

		// Token: 0x06000C75 RID: 3189 RVA: 0x0004A624 File Offset: 0x00048824
		private void Signature_MouseLeave(object sender, EventArgs e)
		{
			this.clicked = false;
		}

		// Token: 0x06000C76 RID: 3190 RVA: 0x0004A630 File Offset: 0x00048830
		private void Signature_MouseMove(object sender, MouseEventArgs e)
		{
			bool flag = this.clicked;
			if (flag)
			{
				this.button1.Enabled = true;
				bool flag2 = this.signature == null;
				if (flag2)
				{
					this.signature = new Bitmap(base.Width, base.Height);
				}
				using (Graphics graphics = Graphics.FromImage(this.signature))
				{
					graphics.DrawLine(this.pen, this.point, e.Location);
					this.point = e.Location;
					base.Invalidate();
				}
			}
		}

		// Token: 0x06000C77 RID: 3191 RVA: 0x0004A624 File Offset: 0x00048824
		private void Signature_MouseUp(object sender, MouseEventArgs e)
		{
			this.clicked = false;
		}

		// Token: 0x06000C78 RID: 3192 RVA: 0x0004A6D4 File Offset: 0x000488D4
		private void Signature_MouseDown(object sender, MouseEventArgs e)
		{
			this.clicked = true;
			this.point = e.Location;
		}

		// Token: 0x06000C79 RID: 3193 RVA: 0x0004A6EC File Offset: 0x000488EC
		private void Signature_Paint(object sender, PaintEventArgs e)
		{
			bool flag = this.signature != null;
			if (flag)
			{
				e.Graphics.DrawImage(this.signature, 0, 0);
			}
		}

		// Token: 0x06000C7A RID: 3194 RVA: 0x0004A71D File Offset: 0x0004891D
		private void button2_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		// Token: 0x06000C7B RID: 3195 RVA: 0x0004A72F File Offset: 0x0004892F
		private void button3_Click(object sender, EventArgs e)
		{
			this.signature = null;
			this.button1.Enabled = false;
			base.Invalidate();
		}

		// Token: 0x06000C7C RID: 3196 RVA: 0x0004A750 File Offset: 0x00048950
		private void button1_Click(object sender, EventArgs e)
		{
			MemoryStream memoryStream = new MemoryStream();
			Encoder quality = Encoder.Quality;
			EncoderParameter encoderParameter = new EncoderParameter(quality, 0L);
			EncoderParameters encoderParameters = new EncoderParameters(1);
			encoderParameters.Param[0] = encoderParameter;
			ImageCodecInfo encoder = this.GetEncoder();
			this.signature.Save(memoryStream, encoder, encoderParameters);
			this.SignatureEnd = memoryStream;
			base.DialogResult = DialogResult.OK;
			base.Close();
		}

		// Token: 0x06000C7D RID: 3197 RVA: 0x0001092C File Offset: 0x0000EB2C
		private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
		{
		}

		// Token: 0x06000C7E RID: 3198 RVA: 0x0001092C File Offset: 0x0000EB2C
		private void textBox1_TextChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x06000C7F RID: 3199 RVA: 0x0001092C File Offset: 0x0000EB2C
		private void tableLayoutPanel1_Paint_1(object sender, PaintEventArgs e)
		{
		}

		// Token: 0x06000C80 RID: 3200 RVA: 0x0004A7B4 File Offset: 0x000489B4
		private ImageCodecInfo GetEncoder()
		{
			ImageCodecInfo[] imageEncoders = ImageCodecInfo.GetImageEncoders();
			foreach (ImageCodecInfo imageCodecInfo in imageEncoders)
			{
				bool flag = imageCodecInfo.FormatID == ImageFormat.Png.Guid;
				if (flag)
				{
					return imageCodecInfo;
				}
			}
			return null;
		}

		// Token: 0x04000830 RID: 2096
		private Image signature;

		// Token: 0x04000831 RID: 2097
		private bool clicked = false;

		// Token: 0x04000832 RID: 2098
		private Point point;

		// Token: 0x04000833 RID: 2099
		private Pen pen;

		// Token: 0x04000834 RID: 2100
		private BoundLocalizedText StrSignInArea;
	}
}
