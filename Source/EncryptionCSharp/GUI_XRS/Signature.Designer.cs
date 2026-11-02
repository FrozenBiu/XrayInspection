namespace GUI_XRS
{
	// Token: 0x0200007E RID: 126
	public partial class Signature : global::System.Windows.Forms.Form
	{
		// Token: 0x06000C81 RID: 3201 RVA: 0x0004A808 File Offset: 0x00048A08
		protected override void Dispose(bool disposing)
		{
			bool flag = disposing && this.components != null;
			if (flag)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000C82 RID: 3202 RVA: 0x0004A840 File Offset: 0x00048A40
		private void InitializeComponent()
		{
			this.button3 = new global::System.Windows.Forms.Button();
			this.button1 = new global::System.Windows.Forms.Button();
			this.textBox1 = new global::System.Windows.Forms.TextBox();
			this.tableLayoutPanel1 = new global::System.Windows.Forms.TableLayoutPanel();
			this.label1 = new global::System.Windows.Forms.Label();
			this.tableLayoutPanel1.SuspendLayout();
			base.SuspendLayout();
			this.button3.Anchor = global::System.Windows.Forms.AnchorStyles.Top;
			this.button3.BackColor = global::System.Drawing.Color.Transparent;
			this.button3.BackgroundImage = global::GUI_XRS.Properties.Resources.buttonUndo;
			this.button3.BackgroundImageLayout = global::System.Windows.Forms.ImageLayout.Zoom;
			this.button3.FlatAppearance.BorderSize = 0;
			this.button3.FlatAppearance.MouseDownBackColor = global::System.Drawing.Color.Transparent;
			this.button3.FlatAppearance.MouseOverBackColor = global::System.Drawing.Color.Transparent;
			this.button3.FlatStyle = global::System.Windows.Forms.FlatStyle.Flat;
			this.button3.Font = new global::System.Drawing.Font("Microsoft Sans Serif", 12f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			this.button3.Location = new global::System.Drawing.Point(379, 5);
			this.button3.Margin = new global::System.Windows.Forms.Padding(0, 5, 0, 5);
			this.button3.MaximumSize = new global::System.Drawing.Size(60, 60);
			this.button3.MinimumSize = new global::System.Drawing.Size(60, 60);
			this.button3.Name = "button3";
			this.button3.Size = new global::System.Drawing.Size(60, 60);
			this.button3.TabIndex = 2;
			this.button3.UseVisualStyleBackColor = false;
			this.button3.Click += new global::System.EventHandler(this.button3_Click);
			this.button1.Anchor = global::System.Windows.Forms.AnchorStyles.Top;
			this.button1.BackColor = global::System.Drawing.Color.Transparent;
			this.button1.BackgroundImage = global::GUI_XRS.Properties.Resources.buttonOK;
			this.button1.BackgroundImageLayout = global::System.Windows.Forms.ImageLayout.Zoom;
			this.button1.DialogResult = global::System.Windows.Forms.DialogResult.OK;
			this.button1.FlatAppearance.BorderSize = 0;
			this.button1.FlatAppearance.MouseDownBackColor = global::System.Drawing.Color.Transparent;
			this.button1.FlatAppearance.MouseOverBackColor = global::System.Drawing.Color.Transparent;
			this.button1.FlatStyle = global::System.Windows.Forms.FlatStyle.Flat;
			this.button1.Font = new global::System.Drawing.Font("Microsoft Sans Serif", 12f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			this.button1.Location = new global::System.Drawing.Point(56, 5);
			this.button1.Margin = new global::System.Windows.Forms.Padding(0, 5, 0, 5);
			this.button1.MaximumSize = new global::System.Drawing.Size(60, 60);
			this.button1.MinimumSize = new global::System.Drawing.Size(60, 60);
			this.button1.Name = "button1";
			this.button1.Size = new global::System.Drawing.Size(60, 60);
			this.button1.TabIndex = 0;
			this.button1.UseVisualStyleBackColor = false;
			this.button1.Click += new global::System.EventHandler(this.button1_Click);
			this.textBox1.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right);
			this.textBox1.BackColor = global::System.Drawing.Color.White;
			this.textBox1.BorderStyle = global::System.Windows.Forms.BorderStyle.None;
			this.textBox1.Enabled = false;
			this.textBox1.Font = new global::System.Drawing.Font("Calibri", 18f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			this.textBox1.Location = new global::System.Drawing.Point(0, 0);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new global::System.Drawing.Size(1014, 37);
			this.textBox1.TabIndex = 4;
			this.textBox1.TextChanged += new global::System.EventHandler(this.textBox1_TextChanged);
			this.tableLayoutPanel1.BackColor = global::System.Drawing.Color.White;
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new global::System.Windows.Forms.ColumnStyle(global::System.Windows.Forms.SizeType.Percent, 52.33918f));
			this.tableLayoutPanel1.ColumnStyles.Add(new global::System.Windows.Forms.ColumnStyle(global::System.Windows.Forms.SizeType.Percent, 47.66082f));
			this.tableLayoutPanel1.ColumnStyles.Add(new global::System.Windows.Forms.ColumnStyle(global::System.Windows.Forms.SizeType.Absolute, 160f));
			this.tableLayoutPanel1.Controls.Add(this.button1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.button3, 2, 0);
			this.tableLayoutPanel1.Location = new global::System.Drawing.Point(0, 217);
			this.tableLayoutPanel1.MaximumSize = new global::System.Drawing.Size(490, 70);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new global::System.Windows.Forms.RowStyle(global::System.Windows.Forms.SizeType.Percent, 50f));
			this.tableLayoutPanel1.RowStyles.Add(new global::System.Windows.Forms.RowStyle(global::System.Windows.Forms.SizeType.Percent, 50f));
			this.tableLayoutPanel1.Size = new global::System.Drawing.Size(490, 70);
			this.tableLayoutPanel1.TabIndex = 5;
			this.tableLayoutPanel1.Paint += new global::System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel1_Paint_1);
			this.label1.AutoSize = true;
			this.label1.BackColor = global::System.Drawing.Color.Gray;
			this.label1.Location = new global::System.Drawing.Point(10, 212);
			this.label1.Margin = new global::System.Windows.Forms.Padding(10, 0, 10, 0);
			this.label1.MaximumSize = new global::System.Drawing.Size(480, 2);
			this.label1.MinimumSize = new global::System.Drawing.Size(480, 2);
			this.label1.Name = "label1";
			this.label1.Size = new global::System.Drawing.Size(480, 2);
			this.label1.TabIndex = 6;
			base.AcceptButton = this.button1;
			this.AutoScroll = true;
			this.BackColor = global::System.Drawing.Color.White;
			base.ClientSize = new global::System.Drawing.Size(494, 294);
			base.ControlBox = false;
			base.Controls.Add(this.label1);
			base.Controls.Add(this.tableLayoutPanel1);
			base.Controls.Add(this.textBox1);
			this.Cursor = global::System.Windows.Forms.Cursors.Hand;
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.FixedSingle;
			base.MaximizeBox = false;
			this.MaximumSize = new global::System.Drawing.Size(500, 300);
			base.MinimizeBox = false;
			this.MinimumSize = new global::System.Drawing.Size(500, 300);
			base.Name = "Signature";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.SizeGripStyle = global::System.Windows.Forms.SizeGripStyle.Show;
			base.TopMost = true;
			base.Load += new global::System.EventHandler(this.Signature_Load);
			this.tableLayoutPanel1.ResumeLayout(false);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04000837 RID: 2103
		private global::System.ComponentModel.IContainer components = null;

		// Token: 0x04000838 RID: 2104
		private global::System.Windows.Forms.Button button1;

		// Token: 0x04000839 RID: 2105
		private global::System.Windows.Forms.TextBox textBox1;

		// Token: 0x0400083A RID: 2106
		private global::System.Windows.Forms.Button button3;

		// Token: 0x0400083B RID: 2107
		private global::System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;

		// Token: 0x0400083C RID: 2108
		private global::System.Windows.Forms.Label label1;
	}
}
