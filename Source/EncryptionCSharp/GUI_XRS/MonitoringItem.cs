using System;
using System.ComponentModel;
using System.Diagnostics;

// Token: 0x02000002 RID: 2
public class MonitoringItem : INotifyPropertyChanged
{
	// Token: 0x14000001 RID: 1
	// (add) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
	// (remove) Token: 0x06000002 RID: 2 RVA: 0x00002088 File Offset: 0x00000288
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event PropertyChangedEventHandler PropertyChanged;

	// Token: 0x17000001 RID: 1
	// (get) Token: 0x06000003 RID: 3 RVA: 0x000020BD File Offset: 0x000002BD
	// (set) Token: 0x06000004 RID: 4 RVA: 0x000020C5 File Offset: 0x000002C5
	public string Name { get; set; }

	// Token: 0x17000002 RID: 2
	// (get) Token: 0x06000005 RID: 5 RVA: 0x000020D0 File Offset: 0x000002D0
	// (set) Token: 0x06000006 RID: 6 RVA: 0x000020E8 File Offset: 0x000002E8
	public string CurrentValue
	{
		get
		{
			return this.currentValue;
		}
		set
		{
			this.currentValue = value;
			this.OnPropertyChanged("CurrentValue");
		}
	}

	// Token: 0x17000003 RID: 3
	// (get) Token: 0x06000007 RID: 7 RVA: 0x00002100 File Offset: 0x00000300
	// (set) Token: 0x06000008 RID: 8 RVA: 0x00002118 File Offset: 0x00000318
	public string Limit
	{
		get
		{
			return this.limit;
		}
		set
		{
			this.limit = value;
			this.OnPropertyChanged("Limit");
		}
	}

	// Token: 0x17000004 RID: 4
	// (get) Token: 0x06000009 RID: 9 RVA: 0x00002130 File Offset: 0x00000330
	// (set) Token: 0x0600000A RID: 10 RVA: 0x00002148 File Offset: 0x00000348
	public string Limit2
	{
		get
		{
			return this.limit2;
		}
		set
		{
			this.limit2 = value;
			this.OnPropertyChanged("Limit2");
		}
	}

	// Token: 0x17000005 RID: 5
	// (get) Token: 0x0600000B RID: 11 RVA: 0x0000215E File Offset: 0x0000035E
	// (set) Token: 0x0600000C RID: 12 RVA: 0x00002166 File Offset: 0x00000366
	public bool Changeable { get; set; }

	// Token: 0x17000006 RID: 6
	// (get) Token: 0x0600000D RID: 13 RVA: 0x0000216F File Offset: 0x0000036F
	// (set) Token: 0x0600000E RID: 14 RVA: 0x00002177 File Offset: 0x00000377
	public string ressourceText { get; set; }

	// Token: 0x17000007 RID: 7
	// (get) Token: 0x0600000F RID: 15 RVA: 0x00002180 File Offset: 0x00000380
	// (set) Token: 0x06000010 RID: 16 RVA: 0x00002188 File Offset: 0x00000388
	public string ressourceText2 { get; set; }

	// Token: 0x06000011 RID: 17 RVA: 0x00002194 File Offset: 0x00000394
	public MonitoringItem(string name, string currentValue, string limit, string limit2, bool change)
	{
		this.Name = name;
		this.CurrentValue = currentValue;
		this.Limit = limit;
		this.Limit2 = limit2;
		this.Changeable = change;
		this.ressourceText = "";
		this.ressourceText2 = "";
	}

	// Token: 0x06000012 RID: 18 RVA: 0x000021EC File Offset: 0x000003EC
	public MonitoringItem(string name, string currentValue, string limit, string limit2, bool change, string ressourceT, string ressourceT2)
	{
		this.Name = name;
		this.CurrentValue = currentValue;
		this.Limit = limit;
		this.Limit2 = limit2;
		this.Changeable = change;
		this.ressourceText = ressourceT;
		this.ressourceText2 = ressourceT2;
	}

	// Token: 0x06000013 RID: 19 RVA: 0x00002240 File Offset: 0x00000440
	protected virtual void OnPropertyChanged(string propertyName)
	{
		PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
		bool flag = propertyChanged != null;
		if (flag)
		{
			propertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
	}

	// Token: 0x04000002 RID: 2
	private string limit;

	// Token: 0x04000003 RID: 3
	private string limit2;

	// Token: 0x04000005 RID: 5
	private string currentValue;
}
