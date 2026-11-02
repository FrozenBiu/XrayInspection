using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using DatabaseConnection;
using Definitions;
using Microsoft.Windows.Controls;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x0200013F RID: 319
	public class CtrlQAParams : UserControl, IComponentConnector
	{
		// Token: 0x170008D1 RID: 2257
		// (get) Token: 0x06001E99 RID: 7833 RVA: 0x000C8B1D File Offset: 0x000C6D1D
		// (set) Token: 0x06001E9A RID: 7834 RVA: 0x000C8B25 File Offset: 0x000C6D25
		public List<Button> Buttons { get; set; }

		// Token: 0x06001E9B RID: 7835 RVA: 0x000C8B30 File Offset: 0x000C6D30
		public CtrlQAParams(DBConnection dbConnection, ParameterHandling parHandling, AccessRightsManager accessManager, bool showProgress, Conveyor conveyor, ProductManager productManager, MachineState machineState)
		{
			this.showProgress = showProgress;
			MachineType machineType = MachineType.MachineTypeUndefined;
			dbConnection.GetMachineType(ref machineType);
			this.showAll = true;
			this.InitializeComponent();
			this.InitializeButtons(dbConnection, showProgress);
			this.viewModel = new QAParamsViewModel(dbConnection, parHandling, accessManager, showProgress, conveyor, productManager, machineState);
			base.DataContext = this.viewModel;
		}

		// Token: 0x06001E9C RID: 7836 RVA: 0x000C8BB0 File Offset: 0x000C6DB0
		private void InitializeButtons(DBConnection dbConnection, bool showProgress)
		{
			string usbLetter = "";
			this.Buttons = new List<Button>();
			dbConnection.GetUsbDriveLetter(ref usbLetter);
			this.btnSaveChkpoint = new Button();
			this.btnSaveChkpoint.Content = ImageConverter.ToImage("/img/saveParams.png");
			this.btnSaveChkpoint.Style = (base.FindResource("defaultButtonStyle") as Style);
			this.btnSaveChkpoint.Click += new RoutedEventHandler(this.BtnCreateChkpoint_Click);
			this.Buttons.Add(this.btnSaveChkpoint);
			this.btnDelete = new Button();
			this.btnDelete.Content = ImageConverter.ToImage("/img/settings_delete.png");
			this.btnDelete.Style = (base.FindResource("defaultButtonStyle") as Style);
			this.btnDelete.Click += this.BtnDelete_Click;
			this.Buttons.Add(this.btnDelete);
			this.btnRestore = new Button();
			this.btnRestore.Content = ImageConverter.ToImage("/img/restoreCheckpoint.png");
			((Image)this.btnRestore.Content).Stretch = Stretch.Uniform;
			((Image)this.btnRestore.Content).Width = 45.0;
			((Image)this.btnRestore.Content).Height = 45.0;
			this.btnRestore.Style = (base.FindResource("defaultButtonStyle") as Style);
			this.btnRestore.Click += new RoutedEventHandler(this.BtnRestore_Click);
			this.Buttons.Add(this.btnRestore);
			if (showProgress)
			{
				this.btnSaveToUSB = new ButtonUSB(usbLetter, "/img/saveallonusb.png", "/img/saveallonusb_nousb.png", new RoutedEventHandler(this.BtnSaveToUSB_Click));
				this.btnSaveSingleToUSB = new ButtonUSB(usbLetter, "/img/saveonusb_small.png", "/img/saveonusbsmall_nousb.png", new RoutedEventHandler(this.BtnSaveSingleToUSB_Click));
				this.Buttons.Add(this.btnSaveSingleToUSB);
				this.Buttons.Add(this.btnSaveToUSB);
			}
			bool flag = !this.showOnlyChanged;
			if (flag)
			{
				this.btnFilter = new Button();
				this.btnFilter.Content = ImageConverter.ToImage("/img/errorLog_small.png");
				((Image)this.btnFilter.Content).Stretch = Stretch.Uniform;
				((Image)this.btnFilter.Content).Width = 45.0;
				((Image)this.btnFilter.Content).Height = 45.0;
				this.btnFilter.Style = (base.FindResource("defaultButtonStyle") as Style);
				this.btnFilter.Click += this.BtnFilter_Click;
				this.Buttons.Add(this.btnFilter);
			}
		}

		// Token: 0x06001E9D RID: 7837 RVA: 0x000C8EA0 File Offset: 0x000C70A0
		private void BtnFilter_Click(object sender, RoutedEventArgs e)
		{
			bool flag = !this.showOnlyChanged;
			if (flag)
			{
				this.showAll = !this.showAll;
				bool flag2 = this.showAll;
				if (flag2)
				{
					this.btnFilter.Content = ImageConverter.ToImage("/img/errorLog_small.png");
				}
				else
				{
					this.btnFilter.Content = ImageConverter.ToImage("/img/plus2.png");
				}
				CollectionViewSource collectionViewSource = (CollectionViewSource)base.FindResource("Parameter");
				collectionViewSource.View.Refresh();
			}
		}

		// Token: 0x06001E9E RID: 7838 RVA: 0x0001092C File Offset: 0x0000EB2C
		private void DataGrid_Loaded(object sender, RoutedEventArgs e)
		{
		}

		// Token: 0x06001E9F RID: 7839 RVA: 0x000C8F28 File Offset: 0x000C7128
		private void BtnCreateChkpoint_Click(object sender, EventArgs e)
		{
			this.viewModel.CreateCheckpoint();
			bool flag = !this.showOnlyChanged;
			if (flag)
			{
				this.showAll = true;
				bool flag2 = this.btnFilter != null;
				if (flag2)
				{
					this.btnFilter.Content = ImageConverter.ToImage("/img/errorLog_small.png");
				}
			}
			else
			{
				this.showAll = false;
			}
			CollectionViewSource collectionViewSource = (CollectionViewSource)base.FindResource("Parameter");
			collectionViewSource.View.Refresh();
		}

		// Token: 0x06001EA0 RID: 7840 RVA: 0x000C8FA1 File Offset: 0x000C71A1
		private void BtnSaveToUSB_Click(object sender, EventArgs e)
		{
			this.viewModel.SaveToUSB(true);
		}

		// Token: 0x06001EA1 RID: 7841 RVA: 0x000C8FB1 File Offset: 0x000C71B1
		private void BtnSaveSingleToUSB_Click(object sender, EventArgs e)
		{
			this.viewModel.SaveToUSB(false);
		}

		// Token: 0x06001EA2 RID: 7842 RVA: 0x000C8FC4 File Offset: 0x000C71C4
		private void BtnRestore_Click(object sender, EventArgs e)
		{
			this.viewModel.RestoreProductChkpoint();
			bool flag = !this.showOnlyChanged;
			if (flag)
			{
				this.showAll = true;
				bool flag2 = this.btnFilter != null;
				if (flag2)
				{
					this.btnFilter.Content = ImageConverter.ToImage("/img/errorLog_small.png");
				}
			}
			else
			{
				this.showAll = false;
			}
			CollectionViewSource collectionViewSource = (CollectionViewSource)base.FindResource("Parameter");
			collectionViewSource.View.Refresh();
		}

		// Token: 0x06001EA3 RID: 7843 RVA: 0x000C9040 File Offset: 0x000C7240
		private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			bool flag = sender is ListBox && ((ListBox)sender).SelectedItem != null && ((ListBox)sender).SelectedItem is ProductData;
			if (flag)
			{
				ProductData productData = (ProductData)((ListBox)sender).SelectedItem;
				bool flag2 = productData.ProductNumber == "0" || !productData.IsSingleProduct;
				if (flag2)
				{
					bool flag3 = e.RemovedItems.Count > 0 && e.RemovedItems[0] is ProductData;
					if (flag3)
					{
						((ListBox)sender).SelectedItem = e.RemovedItems[0];
					}
					else
					{
						((ListBox)sender).SelectedIndex = -1;
					}
				}
				bool flag4 = this.showProgress && this.initialized && !this.first;
				if (flag4)
				{
					CollectionViewSource collectionViewSource = (CollectionViewSource)base.FindResource("Parameter");
					bool flag5 = collectionViewSource.View != null;
					if (flag5)
					{
						collectionViewSource.View.Refresh();
					}
				}
				bool flag6 = this.first;
				if (flag6)
				{
					this.first = false;
				}
			}
		}

		// Token: 0x06001EA4 RID: 7844 RVA: 0x000C9170 File Offset: 0x000C7370
		private void BtnRestoreSingle_Click(object sender, RoutedEventArgs e)
		{
			this.viewModel.RestoreProductChkpoint();
			bool flag = this.showOnlyChanged;
			if (flag)
			{
				this.showAll = true;
				bool flag2 = this.btnFilter != null;
				if (flag2)
				{
					this.btnFilter.Content = ImageConverter.ToImage("/img/errorLog_small.png");
				}
			}
			else
			{
				this.showAll = false;
			}
			CollectionViewSource collectionViewSource = (CollectionViewSource)base.FindResource("Parameter");
			collectionViewSource.View.Refresh();
		}

		// Token: 0x06001EA5 RID: 7845 RVA: 0x000C91E8 File Offset: 0x000C73E8
		private void BtnDelete_Click(object sender, RoutedEventArgs e)
		{
			this.viewModel.RemoveCheckpoint();
			bool flag = !this.showOnlyChanged;
			if (flag)
			{
				this.showAll = true;
				bool flag2 = this.btnFilter != null;
				if (flag2)
				{
					this.btnFilter.Content = ImageConverter.ToImage("/img/errorLog_small.png");
				}
			}
			else
			{
				this.showAll = false;
			}
			this.dgParams.Items.Refresh();
			CollectionViewSource collectionViewSource = (CollectionViewSource)base.FindResource("Parameter");
			collectionViewSource.View.Refresh();
		}

		// Token: 0x06001EA6 RID: 7846 RVA: 0x000C9274 File Offset: 0x000C7474
		private void UserControl_Unloaded(object sender, RoutedEventArgs e)
		{
			this.viewModel.CleanUp();
		}

		// Token: 0x06001EA7 RID: 7847 RVA: 0x000C9284 File Offset: 0x000C7484
		private void CollectionViewSource_Filter(object sender, FilterEventArgs e)
		{
			QualityParameter qualityParameter = e.Item as QualityParameter;
			bool flag = qualityParameter != null;
			if (flag)
			{
				bool flag2 = !this.showAll || this.showOnlyChanged;
				if (flag2)
				{
					bool flag3 = qualityParameter.ParamStates[0] == ParamState.Changed;
					if (flag3)
					{
						e.Accepted = true;
					}
					else
					{
						e.Accepted = false;
					}
				}
				else
				{
					e.Accepted = true;
				}
			}
		}

		// Token: 0x06001EA8 RID: 7848 RVA: 0x000C92EC File Offset: 0x000C74EC
		private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			bool flag = this.showProgress && this.initialized;
			if (flag)
			{
				CollectionViewSource collectionViewSource = (CollectionViewSource)base.FindResource("Parameter");
				collectionViewSource.View.Refresh();
			}
		}

		// Token: 0x06001EA9 RID: 7849 RVA: 0x000C932E File Offset: 0x000C752E
		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			this.initialized = true;
		}

		// Token: 0x06001EAA RID: 7850 RVA: 0x000C9338 File Offset: 0x000C7538
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/qualitycntrldoc/ctrlqaparams.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x06001EAB RID: 7851 RVA: 0x000C9370 File Offset: 0x000C7570
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		internal Delegate _CreateDelegate(Type delegateType, string handler)
		{
			return Delegate.CreateDelegate(delegateType, this, handler);
		}

		// Token: 0x06001EAC RID: 7852 RVA: 0x000C938C File Offset: 0x000C758C
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				((CtrlQAParams)target).Unloaded += this.UserControl_Unloaded;
				((CtrlQAParams)target).Loaded += this.UserControl_Loaded;
				break;
			case 2:
				((CollectionViewSource)target).Filter += this.CollectionViewSource_Filter;
				break;
			case 3:
				((ListBox)target).SelectionChanged += this.ListBox_SelectionChanged;
				break;
			case 4:
				((ComboBox)target).SelectionChanged += this.ComboBox_SelectionChanged;
				break;
			case 5:
				this.dgParams = (Microsoft.Windows.Controls.DataGrid)target;
				this.dgParams.Loaded += this.DataGrid_Loaded;
				break;
			default:
				this._contentLoaded = true;
				break;
			}
		}

		// Token: 0x040013EB RID: 5099
		private QAParamsViewModel viewModel;

		// Token: 0x040013EC RID: 5100
		private Button btnSaveChkpoint;

		// Token: 0x040013ED RID: 5101
		private Button btnDelete;

		// Token: 0x040013EE RID: 5102
		private Button btnRestore;

		// Token: 0x040013EF RID: 5103
		private ButtonUSB btnSaveToUSB;

		// Token: 0x040013F0 RID: 5104
		private ButtonUSB btnSaveSingleToUSB;

		// Token: 0x040013F1 RID: 5105
		private Button btnFilter;

		// Token: 0x040013F3 RID: 5107
		private bool showAll = false;

		// Token: 0x040013F4 RID: 5108
		private bool showProgress;

		// Token: 0x040013F5 RID: 5109
		private bool initialized = false;

		// Token: 0x040013F6 RID: 5110
		private bool showOnlyChanged = false;

		// Token: 0x040013F7 RID: 5111
		private bool first = true;

		// Token: 0x040013F8 RID: 5112
		internal Microsoft.Windows.Controls.DataGrid dgParams;

		// Token: 0x040013F9 RID: 5113
		private bool _contentLoaded;
	}
}
