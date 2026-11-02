using System;
using System.ComponentModel;
using System.Diagnostics;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x0200003C RID: 60
	public class Branding : INotifyPropertyChanged
	{
		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x06000678 RID: 1656 RVA: 0x0002B62C File Offset: 0x0002982C
		// (set) Token: 0x06000679 RID: 1657 RVA: 0x0002B644 File Offset: 0x00029844
		public Definition.GUIDesign Design
		{
			get
			{
				return this.design;
			}
			set
			{
				this.design = value;
				this.OnPropertyChanged("Design");
			}
		}

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x0600067A RID: 1658 RVA: 0x0002B65C File Offset: 0x0002985C
		// (set) Token: 0x0600067B RID: 1659 RVA: 0x0002B674 File Offset: 0x00029874
		public string CompanyLogo
		{
			get
			{
				return this.companyLogo;
			}
			set
			{
				this.companyLogo = value;
				this.OnPropertyChanged("CompanyLogo");
			}
		}

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x0600067C RID: 1660 RVA: 0x0002B68C File Offset: 0x0002988C
		// (set) Token: 0x0600067D RID: 1661 RVA: 0x0002B6A4 File Offset: 0x000298A4
		public string MachineDescription
		{
			get
			{
				return this.machineDescription;
			}
			set
			{
				this.machineDescription = value;
				this.OnPropertyChanged("MachineDescription");
			}
		}

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x0600067E RID: 1662 RVA: 0x0002B6BC File Offset: 0x000298BC
		// (set) Token: 0x0600067F RID: 1663 RVA: 0x0002B6D4 File Offset: 0x000298D4
		public string CompanyName
		{
			get
			{
				return this.companyName;
			}
			set
			{
				this.companyName = value;
				this.OnPropertyChanged("CompanyName");
			}
		}

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x06000680 RID: 1664 RVA: 0x0002B6EC File Offset: 0x000298EC
		// (set) Token: 0x06000681 RID: 1665 RVA: 0x0002B704 File Offset: 0x00029904
		public bool IsActiveBranding
		{
			get
			{
				return this.isActiveBranding;
			}
			set
			{
				this.isActiveBranding = value;
				this.OnActiveBrandingChanged(value);
			}
		}

		// Token: 0x14000025 RID: 37
		// (add) Token: 0x06000682 RID: 1666 RVA: 0x0002B718 File Offset: 0x00029918
		// (remove) Token: 0x06000683 RID: 1667 RVA: 0x0002B750 File Offset: 0x00029950
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x14000026 RID: 38
		// (add) Token: 0x06000684 RID: 1668 RVA: 0x0002B788 File Offset: 0x00029988
		// (remove) Token: 0x06000685 RID: 1669 RVA: 0x0002B7C0 File Offset: 0x000299C0
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<Branding, bool> ActiveBrandingChanged;

		// Token: 0x06000686 RID: 1670 RVA: 0x0002B7F5 File Offset: 0x000299F5
		public Branding() : this(Definition.GUIDesign.None)
		{
		}

		// Token: 0x06000687 RID: 1671 RVA: 0x0002B800 File Offset: 0x00029A00
		public Branding(Definition.GUIDesign design)
		{
			this.Design = design;
			switch (this.Design)
			{
			case Definition.GUIDesign.SuS:
				this.CompanyName = Definition.susCompanyName;
				this.CompanyLogo = Definition.susLogoPath;
				this.MachineDescription = Definition.machineDescriptionSuS;
				break;
			case Definition.GUIDesign.Bizerba:
				this.CompanyName = Definition.bizerbaCompanyName;
				this.CompanyLogo = Definition.bizerbaLogoPath;
				this.MachineDescription = Definition.machineDescriptionBizerba;
				break;
			case Definition.GUIDesign.Gracida:
				this.CompanyName = Definition.gracidaCompanyName;
				this.CompanyLogo = Definition.gracidaLogoPath;
				this.MachineDescription = Definition.machineDescriptionGracida;
				break;
			case Definition.GUIDesign.Hoefelmeyer:
				this.CompanyName = Definition.hoefelmeyerCompanyName;
				this.CompanyLogo = Definition.hoefelmeyerLogoPath;
				this.MachineDescription = Definition.machineDescriptionHoefelmeyer;
				break;
			}
		}

		// Token: 0x06000688 RID: 1672 RVA: 0x0002B8E0 File Offset: 0x00029AE0
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x06000689 RID: 1673 RVA: 0x0002B910 File Offset: 0x00029B10
		protected virtual void OnActiveBrandingChanged(bool active)
		{
			Action<Branding, bool> activeBrandingChanged = this.ActiveBrandingChanged;
			bool flag = activeBrandingChanged != null;
			if (flag)
			{
				activeBrandingChanged(this, active);
			}
		}

		// Token: 0x04000496 RID: 1174
		private bool isActiveBranding;

		// Token: 0x04000497 RID: 1175
		private Definition.GUIDesign design;

		// Token: 0x04000498 RID: 1176
		private string companyLogo;

		// Token: 0x04000499 RID: 1177
		private string machineDescription;

		// Token: 0x0400049A RID: 1178
		private string companyName;
	}
}
