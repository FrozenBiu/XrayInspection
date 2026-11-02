using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using DatabaseConnection;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x02000246 RID: 582
	public class MessageSystem
	{
		// Token: 0x17000BF4 RID: 3060
		// (get) Token: 0x06002C19 RID: 11289 RVA: 0x00122BD0 File Offset: 0x00120DD0
		// (set) Token: 0x06002C1A RID: 11290 RVA: 0x00122BD8 File Offset: 0x00120DD8
		public ObservableCollection<Message> Messages { get; set; }

		// Token: 0x140000F8 RID: 248
		// (add) Token: 0x06002C1B RID: 11291 RVA: 0x00122BE4 File Offset: 0x00120DE4
		// (remove) Token: 0x06002C1C RID: 11292 RVA: 0x00122C1C File Offset: 0x00120E1C
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<Message> MessageRemoved;

		// Token: 0x140000F9 RID: 249
		// (add) Token: 0x06002C1D RID: 11293 RVA: 0x00122C54 File Offset: 0x00120E54
		// (remove) Token: 0x06002C1E RID: 11294 RVA: 0x00122C8C File Offset: 0x00120E8C
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<Message> MessageAdded;

		// Token: 0x140000FA RID: 250
		// (add) Token: 0x06002C1F RID: 11295 RVA: 0x00122CC4 File Offset: 0x00120EC4
		// (remove) Token: 0x06002C20 RID: 11296 RVA: 0x00122CFC File Offset: 0x00120EFC
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<Message> RejectFailed;

		// Token: 0x06002C21 RID: 11297 RVA: 0x00122D31 File Offset: 0x00120F31
		public MessageSystem(User currentUser, DBConnection dbConnection)
		{
			this.Messages = new ObservableCollection<Message>();
			this.currentUser = currentUser;
			this.dbConnection = dbConnection;
		}

		// Token: 0x06002C22 RID: 11298 RVA: 0x00122D58 File Offset: 0x00120F58
		private Message AddMessage(SSTLogbookEntry entry, bool checkIfExisting)
		{
			bool flag = !checkIfExisting || (from t in this.Messages
			where t.LogbookEntry.EntryCode == entry.EntryCode && t.LogbookEntry.Param1 == entry.Param1 && t.LogbookEntry.Param2 == entry.Param2 && t.LogbookEntry.Param3 == entry.Param3 && t.LogbookEntry.Param4 == entry.Param4 && t.LogbookEntry.Param5 == entry.Param5
			select t).Count<Message>() == 0;
			Message result;
			if (flag)
			{
				Message message = new Message(entry);
				this.Messages.Add(message);
				this.OnMessageAdded(message);
				message.MessageRepeated += this.msg_MessageRepeated;
				result = message;
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06002C23 RID: 11299 RVA: 0x00122DDC File Offset: 0x00120FDC
		private void msg_MessageRepeated(Message message)
		{
			bool flag = message.LogbookEntry.EntryCode != SSTEntryCode.DatabaseVersion;
			if (flag)
			{
				MachineMode machineMode = MachineMode.Standard;
				bool flag2 = true;
				this.dbConnection.GetMachineMode(ref machineMode);
				this.dbConnection.GetOp2HlExtendedRights(ref flag2);
				bool flag3 = this.currentUser.Rights > BuiltInPriviledges.Operator2 || this.currentUser.CommitErrors || (machineMode == MachineMode.MAndS && flag2 && this.currentUser.Rights > BuiltInPriviledges.Operator3 && this.IsMsLowOpModeError(message));
				if (flag3)
				{
					this.RemoveMessage(message.LogbookEntry);
				}
				else
				{
					Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtRedeemImpossible"), DialogStyle.Error, DialogButtons.OK);
				}
			}
		}

		// Token: 0x06002C24 RID: 11300 RVA: 0x00122E9C File Offset: 0x0012109C
		private bool RemoveMessage(SSTLogbookEntry entry)
		{
			bool result = false;
			Message message = null;
			IEnumerable<Message> source = from t in this.Messages
			where t.LogbookEntry.EntryCode == entry.EntryCode && t.LogbookEntry.Param1 == entry.Param1 && t.LogbookEntry.Param2 == entry.Param2 && t.LogbookEntry.Param3 == entry.Param3 && t.LogbookEntry.Param4 == entry.Param4 && t.LogbookEntry.Param5 == entry.Param5
			select t;
			bool flag = source.Count<Message>() > 0;
			if (flag)
			{
				message = source.First<Message>();
				result = this.Messages.Remove(message);
			}
			this.OnMessageRemoved(message);
			return result;
		}

		// Token: 0x06002C25 RID: 11301 RVA: 0x00122F08 File Offset: 0x00121108
		public bool PostRegisteredMessage(SSTLogbookEntry entry)
		{
			return this.PostRegisteredMessage(entry, true);
		}

		// Token: 0x06002C26 RID: 11302 RVA: 0x00122F24 File Offset: 0x00121124
		public bool PostRegisteredMessage(SSTLogbookEntry entry, bool checkIfExisting)
		{
			bool messageRegistered = false;
			bool flag = Application.Current != null && !Application.Current.Dispatcher.CheckAccess();
			if (flag)
			{
				Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
				{
					messageRegistered = (this.AddMessage(entry, checkIfExisting) != null);
				}));
			}
			else
			{
				messageRegistered = (this.AddMessage(entry, checkIfExisting) != null);
			}
			return messageRegistered;
		}

		// Token: 0x06002C27 RID: 11303 RVA: 0x00122FBC File Offset: 0x001211BC
		public void RemoveRegisteredMessage(SSTEntryCode code, byte param1)
		{
			bool flag = Application.Current != null && !Application.Current.Dispatcher.CheckAccess();
			if (flag)
			{
				Func<Message, bool> <>9__1;
				Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
				{
					IEnumerable<Message> messages = this.Messages;
					Func<Message, bool> predicate;
					if ((predicate = <>9__1) == null)
					{
						predicate = (<>9__1 = ((Message t) => t.LogbookEntry.EntryCode == code && t.LogbookEntry.Param1 == param1));
					}
					IEnumerable<Message> source2 = messages.Where(predicate);
					bool flag3 = source2.Count<Message>() > 0;
					if (flag3)
					{
						this.RemoveMessage(source2.First<Message>().LogbookEntry);
					}
				}));
			}
			else
			{
				IEnumerable<Message> source = from t in this.Messages
				where t.LogbookEntry.EntryCode == code && t.LogbookEntry.Param1 == param1
				select t;
				bool flag2 = source.Count<Message>() > 0;
				if (flag2)
				{
					this.RemoveMessage(source.First<Message>().LogbookEntry);
				}
			}
		}

		// Token: 0x06002C28 RID: 11304 RVA: 0x00123061 File Offset: 0x00121261
		public void RemoveAllMessages()
		{
			this.Messages.Clear();
		}

		// Token: 0x06002C29 RID: 11305 RVA: 0x00123070 File Offset: 0x00121270
		public bool ModifyMessage(SSTEntryCode code, byte param1, string newParam2, string newParam3, string newParam4, string newParam5)
		{
			bool result = false;
			IEnumerable<Message> source = from t in this.Messages
			where t.LogbookEntry.EntryCode == code && t.LogbookEntry.Param1 == param1
			select t;
			bool flag = source.Count<Message>() > 0;
			if (flag)
			{
				source.First<Message>().LogbookEntry.Param2 = newParam2;
				source.First<Message>().LogbookEntry.Param3 = newParam3;
				source.First<Message>().LogbookEntry.Param4 = newParam4;
				source.First<Message>().LogbookEntry.Param5 = newParam5;
				source.First<Message>().LogbookEntry.CreateLogbookText();
				source.First<Message>().UpdateTextAndTitle();
				result = true;
			}
			return result;
		}

		// Token: 0x06002C2A RID: 11306 RVA: 0x0012312C File Offset: 0x0012132C
		public bool ModifyMessageMonitoring(SSTEntryCode code, byte param1, string newParam2, string newParam3, string newParam4, string newParam5)
		{
			bool result = false;
			IEnumerable<Message> source = from t in this.Messages
			where t.LogbookEntry.EntryCode == code && t.LogbookEntry.Param1 == param1 && t.LogbookEntry.Param2 == newParam2 && t.LogbookEntry.Param3 == newParam3
			select t;
			bool flag = source.Count<Message>() > 0;
			if (flag)
			{
				source.First<Message>().LogbookEntry.Param2 = newParam2;
				source.First<Message>().LogbookEntry.Param3 = newParam3;
				source.First<Message>().LogbookEntry.Param4 = newParam4;
				source.First<Message>().LogbookEntry.Param5 = newParam5;
				source.First<Message>().LogbookEntry.CreateLogbookText();
				source.First<Message>().UpdateTextAndTitle();
				result = true;
			}
			return result;
		}

		// Token: 0x06002C2B RID: 11307 RVA: 0x00123200 File Offset: 0x00121400
		protected virtual void OnMessageRemoved(Message msg)
		{
			Action<Message> messageRemoved = this.MessageRemoved;
			bool flag = messageRemoved != null;
			if (flag)
			{
				messageRemoved(msg);
			}
		}

		// Token: 0x06002C2C RID: 11308 RVA: 0x00123228 File Offset: 0x00121428
		protected virtual void OnMessageAdded(Message msg)
		{
			Action<Message> messageAdded = this.MessageAdded;
			bool flag = messageAdded != null;
			if (flag)
			{
				messageAdded(msg);
			}
		}

		// Token: 0x06002C2D RID: 11309 RVA: 0x00123250 File Offset: 0x00121450
		public bool EntryExisiting(SSTEntryCode code, byte param1)
		{
			return (from t in this.Messages
			where t.LogbookEntry.EntryCode == code && t.LogbookEntry.Param1 == param1
			select t).Count<Message>() != 0;
		}

		// Token: 0x06002C2E RID: 11310 RVA: 0x00123298 File Offset: 0x00121498
		public bool EntryExisiting(SSTEntryCode code, byte param1, string param2, string param3, string param4, string param5)
		{
			return (from t in this.Messages
			where t.LogbookEntry.EntryCode == code && t.LogbookEntry.Param1 == param1 && t.LogbookEntry.Param2 == param2 && t.LogbookEntry.Param3 == param3 && t.LogbookEntry.Param4 == param4 && t.LogbookEntry.Param5 == param5
			select t).Count<Message>() != 0;
		}

		// Token: 0x06002C2F RID: 11311 RVA: 0x001232FC File Offset: 0x001214FC
		private bool IsMsLowOpModeError(Message message)
		{
			bool result = false;
			bool flag = message.LogbookEntry.EntryCode == SSTEntryCode.RejectVerify || message.LogbookEntry.EntryCode == SSTEntryCode.ProductsMissing || message.LogbookEntry.EntryCode == SSTEntryCode.SensorBlockedPhotogating || message.LogbookEntry.EntryCode == SSTEntryCode.SensorBlockedRejectCheck || message.LogbookEntry.EntryCode == SSTEntryCode.ErrorConsistency || message.LogbookEntry.EntryCode == SSTEntryCode.BinMissing || message.LogbookEntry.EntryCode == SSTEntryCode.ProductNotInspected || message.LogbookEntry.EntryCode == SSTEntryCode.LightBarrier || message.LogbookEntry.EntryCode == SSTEntryCode.BinFull || message.LogbookEntry.EntryCode == SSTEntryCode.LowAirPressure;
			if (flag)
			{
				result = true;
			}
			return result;
		}

		// Token: 0x06002C30 RID: 11312 RVA: 0x001233B5 File Offset: 0x001215B5
		public void NotifyReject()
		{
			this.OnRejectFailed(null);
		}

		// Token: 0x06002C31 RID: 11313 RVA: 0x001233C0 File Offset: 0x001215C0
		protected virtual void OnRejectFailed(Message msg)
		{
			Action<Message> rejectFailed = this.RejectFailed;
			bool flag = rejectFailed != null;
			if (flag)
			{
				rejectFailed(msg);
			}
		}

		// Token: 0x04001BA2 RID: 7074
		private User currentUser;

		// Token: 0x04001BA3 RID: 7075
		private DBConnection dbConnection;
	}
}
