using System;
using System.ComponentModel;
using System.Diagnostics;
using DatabaseConnection;

namespace GUI_XRS
{
	// Token: 0x02000146 RID: 326
	public class Buzzer : INotifyPropertyChanged
	{
		// Token: 0x140000A5 RID: 165
		// (add) Token: 0x06001EF8 RID: 7928 RVA: 0x000CE888 File Offset: 0x000CCA88
		// (remove) Token: 0x06001EF9 RID: 7929 RVA: 0x000CE8C0 File Offset: 0x000CCAC0
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x170008E0 RID: 2272
		// (get) Token: 0x06001EFA RID: 7930 RVA: 0x000CE8F8 File Offset: 0x000CCAF8
		// (set) Token: 0x06001EFB RID: 7931 RVA: 0x000CE910 File Offset: 0x000CCB10
		public int Delay
		{
			get
			{
				return this.delay;
			}
			set
			{
				bool flag = this.delay != value;
				if (flag)
				{
					this.delay = value;
					this.parHandling.WriteValue(this.buzzerMainPath + "/Delay/Value", this.delay.ToString());
					this.OnPropertyChanged("Delay");
				}
			}
		}

		// Token: 0x170008E1 RID: 2273
		// (get) Token: 0x06001EFC RID: 7932 RVA: 0x000CE96C File Offset: 0x000CCB6C
		// (set) Token: 0x06001EFD RID: 7933 RVA: 0x000CE984 File Offset: 0x000CCB84
		public int Length
		{
			get
			{
				return this.length;
			}
			set
			{
				bool flag = this.length != value;
				if (flag)
				{
					this.length = value;
					this.parHandling.WriteValue(this.buzzerMainPath + "/Length/Value", this.length.ToString());
					this.OnPropertyChanged("Length");
				}
			}
		}

		// Token: 0x170008E2 RID: 2274
		// (get) Token: 0x06001EFE RID: 7934 RVA: 0x000CE9E0 File Offset: 0x000CCBE0
		// (set) Token: 0x06001EFF RID: 7935 RVA: 0x000CE9F8 File Offset: 0x000CCBF8
		public bool Enable
		{
			get
			{
				return this.enable;
			}
			set
			{
				bool flag = this.enable != value;
				if (flag)
				{
					this.enable = value;
					this.parHandling.WriteValue(this.buzzerMainPath + "/Enabled/Value", this.enable ? "1" : "0");
					this.OnPropertyChanged("Enable");
				}
			}
		}

		// Token: 0x170008E3 RID: 2275
		// (get) Token: 0x06001F00 RID: 7936 RVA: 0x000CEA5C File Offset: 0x000CCC5C
		// (set) Token: 0x06001F01 RID: 7937 RVA: 0x000CEA74 File Offset: 0x000CCC74
		public bool TurnOnIfError
		{
			get
			{
				return this.turnOnIfError;
			}
			set
			{
				bool flag = this.turnOnIfError != value;
				if (flag)
				{
					this.turnOnIfError = value;
					this.parHandling.WriteValue(this.buzzerMainPath + "/TurnOnIfError/Value", this.turnOnIfError ? "1" : "0");
					this.OnPropertyChanged("TurnOnIfError");
				}
			}
		}

		// Token: 0x170008E4 RID: 2276
		// (get) Token: 0x06001F02 RID: 7938 RVA: 0x000CEAD7 File Offset: 0x000CCCD7
		// (set) Token: 0x06001F03 RID: 7939 RVA: 0x000CEADF File Offset: 0x000CCCDF
		public BoundLocalizedText StrLength { get; set; }

		// Token: 0x170008E5 RID: 2277
		// (get) Token: 0x06001F04 RID: 7940 RVA: 0x000CEAE8 File Offset: 0x000CCCE8
		// (set) Token: 0x06001F05 RID: 7941 RVA: 0x000CEAF0 File Offset: 0x000CCCF0
		public BoundLocalizedText StrDelay { get; set; }

		// Token: 0x170008E6 RID: 2278
		// (get) Token: 0x06001F06 RID: 7942 RVA: 0x000CEAF9 File Offset: 0x000CCCF9
		// (set) Token: 0x06001F07 RID: 7943 RVA: 0x000CEB01 File Offset: 0x000CCD01
		public BoundLocalizedText StrBuzzer { get; set; }

		// Token: 0x170008E7 RID: 2279
		// (get) Token: 0x06001F08 RID: 7944 RVA: 0x000CEB0A File Offset: 0x000CCD0A
		// (set) Token: 0x06001F09 RID: 7945 RVA: 0x000CEB12 File Offset: 0x000CCD12
		public BoundLocalizedText StrBuzzerEnabled { get; set; }

		// Token: 0x170008E8 RID: 2280
		// (get) Token: 0x06001F0A RID: 7946 RVA: 0x000CEB1B File Offset: 0x000CCD1B
		// (set) Token: 0x06001F0B RID: 7947 RVA: 0x000CEB23 File Offset: 0x000CCD23
		public BoundLocalizedText StrTurnOnBuzzerIfError { get; set; }

		// Token: 0x06001F0C RID: 7948 RVA: 0x000CEB2C File Offset: 0x000CCD2C
		public Buzzer(ParameterHandling parHandling, DBConnection dbConnection, string buzzerMainPath)
		{
			this.parHandling = parHandling;
			this.dbConnection = dbConnection;
			this.buzzerMainPath = buzzerMainPath;
			this.GetValuesFromDatabase();
			this.RegisterLocalizedText();
		}

		// Token: 0x06001F0D RID: 7949 RVA: 0x000CEB5C File Offset: 0x000CCD5C
		private void RegisterLocalizedText()
		{
			this.StrLength = new BoundLocalizedText("txtLength");
			this.StrDelay = new BoundLocalizedText("txtDelay");
			this.StrBuzzer = new BoundLocalizedText("txtBuzzer");
			this.StrBuzzerEnabled = new BoundLocalizedText("txtBuzzerEnable");
			this.StrTurnOnBuzzerIfError = new BoundLocalizedText("txtBuzzerEnableIfError");
		}

		// Token: 0x06001F0E RID: 7950 RVA: 0x000CEBC0 File Offset: 0x000CCDC0
		public void DeregisterLocalizedText()
		{
			MainWindow.localizationManager.Deregister(this.StrLength);
			MainWindow.localizationManager.Deregister(this.StrDelay);
			MainWindow.localizationManager.Deregister(this.StrBuzzerEnabled);
			MainWindow.localizationManager.Deregister(this.StrTurnOnBuzzerIfError);
			MainWindow.localizationManager.Deregister(this.StrBuzzer);
		}

		// Token: 0x06001F0F RID: 7951 RVA: 0x000CEC24 File Offset: 0x000CCE24
		private void GetValuesFromDatabase()
		{
			string text = null;
			this.dbConnection.GetParameterValue(this.buzzerMainPath + "/Delay/Value", ref text);
			int.TryParse(text, out this.delay);
			this.dbConnection.GetParameterValue(this.buzzerMainPath + "/Length/Value", ref text);
			int.TryParse(text, out this.length);
			this.dbConnection.GetParameterValue(this.buzzerMainPath + "/Enabled/Value", ref text);
			this.enable = (text == "1");
			this.dbConnection.GetParameterValue(this.buzzerMainPath + "/TurnOnIfError/Value", ref text);
			this.turnOnIfError = (text == "1");
		}

		// Token: 0x06001F10 RID: 7952 RVA: 0x000CECE8 File Offset: 0x000CCEE8
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x06001F11 RID: 7953 RVA: 0x000CED18 File Offset: 0x000CCF18
		public void SetDelay()
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtDelay"), this.Delay.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_SetDelay;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x06001F12 RID: 7954 RVA: 0x000CED70 File Offset: 0x000CCF70
		private void numKeyboard_SetDelay(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				this.Delay = int.Parse(e.Text);
			}
			catch
			{
			}
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_SetDelay;
		}

		// Token: 0x06001F13 RID: 7955 RVA: 0x000CEDC4 File Offset: 0x000CCFC4
		public void SetLength()
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtEjectTime"), this.Length.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_SetLength;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x06001F14 RID: 7956 RVA: 0x000CEE1C File Offset: 0x000CD01C
		private void numKeyboard_SetLength(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				this.Length = int.Parse(e.Text);
			}
			catch
			{
			}
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_SetLength;
		}

		// Token: 0x04001439 RID: 5177
		private NumKeyboard numKeyboard;

		// Token: 0x0400143A RID: 5178
		private ParameterHandling parHandling;

		// Token: 0x0400143B RID: 5179
		private DBConnection dbConnection;

		// Token: 0x0400143C RID: 5180
		private string buzzerMainPath;

		// Token: 0x0400143D RID: 5181
		private int delay;

		// Token: 0x0400143E RID: 5182
		private int length;

		// Token: 0x0400143F RID: 5183
		private bool enable;

		// Token: 0x04001440 RID: 5184
		private bool turnOnIfError;
	}
}
