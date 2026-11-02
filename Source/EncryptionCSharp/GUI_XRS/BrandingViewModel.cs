using System;
using System.Collections.ObjectModel;
using Common;
using DatabaseConnection;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x0200003D RID: 61
	public class BrandingViewModel
	{
		// Token: 0x170001EE RID: 494
		// (get) Token: 0x0600068A RID: 1674 RVA: 0x0002B936 File Offset: 0x00029B36
		// (set) Token: 0x0600068B RID: 1675 RVA: 0x0002B93E File Offset: 0x00029B3E
		public ObservableCollection<Branding> Brandings { get; set; }

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x0600068C RID: 1676 RVA: 0x0002B947 File Offset: 0x00029B47
		// (set) Token: 0x0600068D RID: 1677 RVA: 0x0002B94F File Offset: 0x00029B4F
		public RelayCommand CloseCommand { get; set; }

		// Token: 0x0600068E RID: 1678 RVA: 0x0002B958 File Offset: 0x00029B58
		public BrandingViewModel(DBConnection dbConnection, Branding selectedBranding, ParameterHandling parHandling)
		{
			this.selectedBranding = selectedBranding;
			this.dbConnection = dbConnection;
			this.parHandling = parHandling;
			this.CloseCommand = new RelayCommand(new Action<object>(this.Close));
			this.FillCompanyList(dbConnection, selectedBranding);
		}

		// Token: 0x0600068F RID: 1679 RVA: 0x0002B998 File Offset: 0x00029B98
		private void FillCompanyList(DBConnection dbConnection, Branding selectedBranding)
		{
			this.Brandings = new ObservableCollection<Branding>();
			Branding branding = new Branding(Definition.GUIDesign.SuS);
			this.Brandings.Add(branding);
			branding.ActiveBrandingChanged += this.BrandingViewModel_ActiveBrandingChanged;
			bool flag = selectedBranding.Design == branding.Design;
			if (flag)
			{
				branding.IsActiveBranding = true;
			}
			branding = new Branding(Definition.GUIDesign.Bizerba);
			this.Brandings.Add(branding);
			branding.ActiveBrandingChanged += this.BrandingViewModel_ActiveBrandingChanged;
			bool flag2 = selectedBranding.Design == branding.Design;
			if (flag2)
			{
				branding.IsActiveBranding = true;
			}
			branding = new Branding(Definition.GUIDesign.Gracida);
			this.Brandings.Add(branding);
			branding.ActiveBrandingChanged += this.BrandingViewModel_ActiveBrandingChanged;
			bool flag3 = selectedBranding.Design == branding.Design;
			if (flag3)
			{
				branding.IsActiveBranding = true;
			}
			branding = new Branding(Definition.GUIDesign.Hoefelmeyer);
			this.Brandings.Add(branding);
			branding.ActiveBrandingChanged += this.BrandingViewModel_ActiveBrandingChanged;
			bool flag4 = selectedBranding.Design == branding.Design;
			if (flag4)
			{
				branding.IsActiveBranding = true;
			}
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x0002BAB8 File Offset: 0x00029CB8
		private void BrandingViewModel_ActiveBrandingChanged(Branding newBranding, bool active)
		{
			if (active)
			{
				for (int i = 0; i < this.Brandings.Count; i++)
				{
					bool flag = this.Brandings[i] != newBranding;
					if (flag)
					{
						this.Brandings[i].IsActiveBranding = false;
					}
				}
				this.selectedBranding.CompanyLogo = newBranding.CompanyLogo;
				this.selectedBranding.CompanyName = newBranding.CompanyName;
				this.selectedBranding.Design = newBranding.Design;
				this.selectedBranding.MachineDescription = newBranding.MachineDescription;
				this.parHandling.WriteValue(Definition.guiDesignPath, newBranding.CompanyName, false);
				this.dbConnection.SetDesign(this.selectedBranding.Design);
			}
		}

		// Token: 0x06000691 RID: 1681 RVA: 0x0002BB8C File Offset: 0x00029D8C
		private void Close(object obj)
		{
			foreach (Branding branding in this.Brandings)
			{
				branding.ActiveBrandingChanged -= this.BrandingViewModel_ActiveBrandingChanged;
			}
			bool flag = this.Brandings != null;
			if (flag)
			{
				this.Brandings.Clear();
				this.Brandings = null;
			}
		}

		// Token: 0x0400049D RID: 1181
		private Branding selectedBranding;

		// Token: 0x0400049E RID: 1182
		private DBConnection dbConnection;

		// Token: 0x0400049F RID: 1183
		private ParameterHandling parHandling;
	}
}
