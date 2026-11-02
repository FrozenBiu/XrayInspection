using System;

// Token: 0x02000004 RID: 4
public class PathsWithAbbreviation
{
	// Token: 0x17000008 RID: 8
	// (get) Token: 0x06000014 RID: 20 RVA: 0x0000226D File Offset: 0x0000046D
	// (set) Token: 0x06000015 RID: 21 RVA: 0x00002275 File Offset: 0x00000475
	public string abbreviation { get; set; }

	// Token: 0x17000009 RID: 9
	// (get) Token: 0x06000016 RID: 22 RVA: 0x0000227E File Offset: 0x0000047E
	// (set) Token: 0x06000017 RID: 23 RVA: 0x00002286 File Offset: 0x00000486
	public string name { get; set; }

	// Token: 0x1700000A RID: 10
	// (get) Token: 0x06000018 RID: 24 RVA: 0x0000228F File Offset: 0x0000048F
	// (set) Token: 0x06000019 RID: 25 RVA: 0x00002297 File Offset: 0x00000497
	public string type { get; set; }

	// Token: 0x1700000B RID: 11
	// (get) Token: 0x0600001A RID: 26 RVA: 0x000022A0 File Offset: 0x000004A0
	// (set) Token: 0x0600001B RID: 27 RVA: 0x000022A8 File Offset: 0x000004A8
	public string[] paths { get; set; }

	// Token: 0x0600001C RID: 28 RVA: 0x000022B1 File Offset: 0x000004B1
	public PathsWithAbbreviation(string abbr, string[] paths, string name, string type)
	{
		this.abbreviation = abbr;
		this.paths = paths;
		this.name = name;
		this.type = type;
	}
}
