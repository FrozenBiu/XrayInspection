using System;
using System.ComponentModel;
using System.Diagnostics;
using Common;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x02000241 RID: 577
	public class Message : INotifyPropertyChanged
	{
		// Token: 0x17000BE1 RID: 3041
		// (get) Token: 0x06002BD4 RID: 11220 RVA: 0x00121B04 File Offset: 0x0011FD04
		// (set) Token: 0x06002BD5 RID: 11221 RVA: 0x00121B1C File Offset: 0x0011FD1C
		public SSTLogbookEntry LogbookEntry
		{
			get
			{
				return this.logbookEntry;
			}
			set
			{
				this.logbookEntry = value;
				this.OnPropertyChanged("LogbookEntry");
				this.UpdateTextAndTitle();
			}
		}

		// Token: 0x17000BE2 RID: 3042
		// (get) Token: 0x06002BD6 RID: 11222 RVA: 0x00121B3C File Offset: 0x0011FD3C
		// (set) Token: 0x06002BD7 RID: 11223 RVA: 0x00121B54 File Offset: 0x0011FD54
		public string MessageString
		{
			get
			{
				return this.messageString;
			}
			set
			{
				this.messageString = value;
				this.OnPropertyChanged("MessageString");
			}
		}

		// Token: 0x17000BE3 RID: 3043
		// (get) Token: 0x06002BD8 RID: 11224 RVA: 0x00121B6C File Offset: 0x0011FD6C
		// (set) Token: 0x06002BD9 RID: 11225 RVA: 0x00121B84 File Offset: 0x0011FD84
		public string MessageTitle
		{
			get
			{
				return this.messageTitle;
			}
			set
			{
				this.messageTitle = value;
				this.OnPropertyChanged("MessageTitle");
			}
		}

		// Token: 0x17000BE4 RID: 3044
		// (get) Token: 0x06002BDA RID: 11226 RVA: 0x00121B9A File Offset: 0x0011FD9A
		// (set) Token: 0x06002BDB RID: 11227 RVA: 0x00121BA2 File Offset: 0x0011FDA2
		public RelayCommand MessageCommand { get; set; }

		// Token: 0x140000F5 RID: 245
		// (add) Token: 0x06002BDC RID: 11228 RVA: 0x00121BAC File Offset: 0x0011FDAC
		// (remove) Token: 0x06002BDD RID: 11229 RVA: 0x00121BE4 File Offset: 0x0011FDE4
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<Message> MessageRepeated;

		// Token: 0x140000F6 RID: 246
		// (add) Token: 0x06002BDE RID: 11230 RVA: 0x00121C1C File Offset: 0x0011FE1C
		// (remove) Token: 0x06002BDF RID: 11231 RVA: 0x00121C54 File Offset: 0x0011FE54
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x06002BE0 RID: 11232 RVA: 0x00121C89 File Offset: 0x0011FE89
		public Message(SSTLogbookEntry logbookEntry)
		{
			this.LogbookEntry = logbookEntry;
			this.MessageCommand = new RelayCommand(new Action<object>(this.RepeatMessage));
		}

		// Token: 0x06002BE1 RID: 11233 RVA: 0x00121CB4 File Offset: 0x0011FEB4
		public void RepeatMessage(object o)
		{
			bool flag = this.LogbookEntry.Type == MessageType.Error;
			DialogStyle dialogStyle;
			if (flag)
			{
				dialogStyle = DialogStyle.Error;
			}
			else
			{
				bool flag2 = this.LogbookEntry.Type == MessageType.Warning;
				if (flag2)
				{
					dialogStyle = DialogStyle.Warning;
				}
				else
				{
					dialogStyle = DialogStyle.Info;
				}
			}
			Dialog.Show("Repeated " + dialogStyle.ToString(), this.MessageString, dialogStyle);
			bool flag3 = dialogStyle != DialogStyle.Info;
			if (flag3)
			{
				this.OnMessageRepeated(this);
			}
		}

		// Token: 0x06002BE2 RID: 11234 RVA: 0x00121D28 File Offset: 0x0011FF28
		protected virtual void OnMessageRepeated(Message message)
		{
			Action<Message> messageRepeated = this.MessageRepeated;
			bool flag = messageRepeated != null;
			if (flag)
			{
				messageRepeated(message);
			}
		}

		// Token: 0x06002BE3 RID: 11235 RVA: 0x00121D50 File Offset: 0x0011FF50
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x06002BE4 RID: 11236 RVA: 0x00121D80 File Offset: 0x0011FF80
		public void UpdateTextAndTitle()
		{
			this.MessageTitle = this.logbookEntry.MessageText;
			this.MessageString = this.logbookEntry.MessageText + Environment.NewLine + this.logbookEntry.EntryTime.ToString(LocalizationManager.CurrentCulture);
		}

		// Token: 0x04001B7C RID: 7036
		private string messageTitle;

		// Token: 0x04001B7D RID: 7037
		private string messageString;

		// Token: 0x04001B7E RID: 7038
		private SSTLogbookEntry logbookEntry;
	}
}
