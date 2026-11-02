using System;
using System.ComponentModel;
using System.Diagnostics;
using Common;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x0200010E RID: 270
	public class EdgeSubButtonsViewModel : INotifyPropertyChanged
	{
		// Token: 0x14000080 RID: 128
		// (add) Token: 0x06001952 RID: 6482 RVA: 0x00095A0C File Offset: 0x00093C0C
		// (remove) Token: 0x06001953 RID: 6483 RVA: 0x00095A44 File Offset: 0x00093C44
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x17000792 RID: 1938
		// (get) Token: 0x06001954 RID: 6484 RVA: 0x00095A7C File Offset: 0x00093C7C
		// (set) Token: 0x06001955 RID: 6485 RVA: 0x00095A94 File Offset: 0x00093C94
		public EdgeFilterMode SelectedEdgeMode
		{
			get
			{
				return this.selectedEdgeMode;
			}
			set
			{
				bool flag = this.parHandling != null;
				if (flag)
				{
					bool flag2 = value == EdgeFilterMode.RemoveElliptic;
					if (flag2)
					{
						this.MultipleVisible = false;
						this.MultipleEnabled = false;
					}
					else
					{
						this.MultipleVisible = true;
					}
					this.selectedEdgeMode = value;
					bool flag3 = this.initialized;
					if (flag3)
					{
						ParameterHandling parameterHandling = this.parHandling;
						string path = Definition.filterMainPath + this.strImageGroup + Filter.edgeOperationSubPath;
						int num = (int)value;
						parameterHandling.WriteValue(path, num.ToString());
						this.OnPropertyChanged("SelectedEdgeMode");
					}
				}
			}
		}

		// Token: 0x17000793 RID: 1939
		// (get) Token: 0x06001956 RID: 6486 RVA: 0x00095B20 File Offset: 0x00093D20
		// (set) Token: 0x06001957 RID: 6487 RVA: 0x00095B38 File Offset: 0x00093D38
		public int EdgeThreshold
		{
			get
			{
				return this.edgeThreshold;
			}
			set
			{
				bool flag = this.parHandling != null;
				if (flag)
				{
					this.edgeThreshold = value;
					bool flag2 = this.initialized;
					if (flag2)
					{
						this.parHandling.WriteValue(Definition.filterMainPath + this.strImageGroup + Filter.edgeThresholdSubPath, value.ToString());
						this.OnPropertyChanged("EdgeThreshold");
					}
				}
			}
		}

		// Token: 0x17000794 RID: 1940
		// (get) Token: 0x06001958 RID: 6488 RVA: 0x00095B9C File Offset: 0x00093D9C
		// (set) Token: 0x06001959 RID: 6489 RVA: 0x00095BB4 File Offset: 0x00093DB4
		public bool MultipleEnabled
		{
			get
			{
				return this.multipleEnabled;
			}
			set
			{
				bool flag = this.parHandling != null;
				if (flag)
				{
					this.multipleEnabled = value;
					bool flag2 = this.initialized;
					if (flag2)
					{
						this.parHandling.WriteValue(Definition.filterMainPath + this.strImageGroup + Filter.edgeMultipleObjectsSubPath, value ? "1" : "0");
						this.OnPropertyChanged("MultipleEnabled");
					}
				}
			}
		}

		// Token: 0x17000795 RID: 1941
		// (get) Token: 0x0600195A RID: 6490 RVA: 0x00095C20 File Offset: 0x00093E20
		// (set) Token: 0x0600195B RID: 6491 RVA: 0x00095C28 File Offset: 0x00093E28
		public bool ServiceMode { get; set; }

		// Token: 0x17000796 RID: 1942
		// (get) Token: 0x0600195C RID: 6492 RVA: 0x00095C34 File Offset: 0x00093E34
		// (set) Token: 0x0600195D RID: 6493 RVA: 0x00095C4C File Offset: 0x00093E4C
		public bool MultipleVisible
		{
			get
			{
				return this.multipleVisible;
			}
			set
			{
				this.multipleVisible = value;
				this.OnPropertyChanged("MultipleVisible");
			}
		}

		// Token: 0x17000797 RID: 1943
		// (get) Token: 0x0600195E RID: 6494 RVA: 0x00095C62 File Offset: 0x00093E62
		// (set) Token: 0x0600195F RID: 6495 RVA: 0x00095C6A File Offset: 0x00093E6A
		public RelayCommand ChangeEdgeThresholdCommand { get; set; }

		// Token: 0x17000798 RID: 1944
		// (get) Token: 0x06001960 RID: 6496 RVA: 0x00095C73 File Offset: 0x00093E73
		// (set) Token: 0x06001961 RID: 6497 RVA: 0x00095C7B File Offset: 0x00093E7B
		public BoundLocalizedText StrThreshold { get; set; }

		// Token: 0x06001962 RID: 6498 RVA: 0x00095C84 File Offset: 0x00093E84
		public EdgeSubButtonsViewModel(ParameterHandling parHandling, AccessRightsManager accessManager, int imageGroup)
		{
			this.parHandling = parHandling;
			this.accessManager = accessManager;
			this.strImageGroup = ((imageGroup > 0) ? imageGroup.ToString() : "");
			try
			{
				this.ChangeEdgeThresholdCommand = new RelayCommand(new Action<object>(this.ChangeEdgeThreshold));
				this.StrThreshold = new BoundLocalizedText("txtThreshold");
				this.SelectedEdgeMode = (EdgeFilterMode)int.Parse(parHandling.ReadPar(Definition.filterMainPath + this.strImageGroup + Filter.edgeOperationSubPath));
				this.EdgeThreshold = int.Parse(parHandling.ReadPar(Definition.filterMainPath + this.strImageGroup + Filter.edgeThresholdSubPath));
				this.MultipleEnabled = (parHandling.ReadPar(Definition.filterMainPath + this.strImageGroup + Filter.edgeMultipleObjectsSubPath) == "1");
				bool flag = accessManager != null;
				if (flag)
				{
					this.ServiceMode = (accessManager.CurrentUser.Rights == BuiltInPriviledges.Service);
				}
			}
			catch
			{
			}
			this.initialized = true;
		}

		// Token: 0x06001963 RID: 6499 RVA: 0x00095DAC File Offset: 0x00093FAC
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x06001964 RID: 6500 RVA: 0x00095DDC File Offset: 0x00093FDC
		public void ChangeEdgeThreshold(object obj)
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtThreshold"), this.EdgeThreshold.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_InputEdgeThreshold;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x06001965 RID: 6501 RVA: 0x00095E34 File Offset: 0x00094034
		private void numKeyboard_InputEdgeThreshold(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				this.EdgeThreshold = int.Parse(e.Text);
			}
			catch
			{
			}
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_InputEdgeThreshold;
		}

		// Token: 0x06001966 RID: 6502 RVA: 0x00095E88 File Offset: 0x00094088
		public void CleanUp()
		{
			MainWindow.localizationManager.Deregister(this.StrThreshold);
		}

		// Token: 0x04001094 RID: 4244
		private bool initialized = false;

		// Token: 0x04001095 RID: 4245
		private NumKeyboard numKeyboard;

		// Token: 0x04001096 RID: 4246
		private ParameterHandling parHandling;

		// Token: 0x04001097 RID: 4247
		private AccessRightsManager accessManager;

		// Token: 0x04001098 RID: 4248
		private EdgeFilterMode selectedEdgeMode;

		// Token: 0x04001099 RID: 4249
		private int edgeThreshold;

		// Token: 0x0400109A RID: 4250
		private bool multipleEnabled;

		// Token: 0x0400109B RID: 4251
		private bool multipleVisible;

		// Token: 0x0400109C RID: 4252
		private string strImageGroup;
	}
}
