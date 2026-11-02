using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x02000034 RID: 52
	public class CtrlBackupList : UserControl, IComponentConnector
	{
		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x0600061B RID: 1563 RVA: 0x00029A8C File Offset: 0x00027C8C
		// (set) Token: 0x0600061C RID: 1564 RVA: 0x00029A94 File Offset: 0x00027C94
		public Backup Backup { get; set; }

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x0600061D RID: 1565 RVA: 0x00029A9D File Offset: 0x00027C9D
		// (set) Token: 0x0600061E RID: 1566 RVA: 0x00029AA5 File Offset: 0x00027CA5
		public List<Button> Buttons { get; set; }

		// Token: 0x0600061F RID: 1567 RVA: 0x00029AB0 File Offset: 0x00027CB0
		public CtrlBackupList(ProductManager productManager, Backup backup, ParameterHandling parHandling)
		{
			this.productManager = productManager;
			this.Backup = backup;
			this.InitializeComponent();
			this.InitializeButtons();
			this.lstProducts.ItemsSource = productManager.ProductList;
			this.backupListViewModel = new BackupListViewModel(this.Backup, productManager, parHandling);
			base.DataContext = this.backupListViewModel;
			this.lstProducts.Loaded += delegate(object s, RoutedEventArgs e)
			{
				base.Dispatcher.Invoke(delegate()
				{
					this.lstProducts.ScrollIntoView(this.lstProducts.SelectedItem);
					this.lstProducts.Dispatcher.InvokeAsync(delegate()
					{
						bool flag = this.lstProducts.SelectedItem != null;
						if (flag)
						{
							FrameworkElement frameworkElement = this.lstProducts.ItemContainerGenerator.ContainerFromItem(this.lstProducts.SelectedItem) as FrameworkElement;
							bool flag2 = frameworkElement != null;
							if (flag2)
							{
								frameworkElement.BringIntoView(new Rect(0.0, 0.0, frameworkElement.ActualWidth, frameworkElement.ActualHeight));
							}
						}
					}, DispatcherPriority.Loaded);
					this.DrawMarkings();
				});
			};
		}

		// Token: 0x06000620 RID: 1568 RVA: 0x00029B2C File Offset: 0x00027D2C
		private void InitializeButtons()
		{
			this.Buttons = new List<Button>();
			this.btnMarkAll = new Button();
			Image image = ImageConverter.ToImage("/img/mark_all.png");
			image.Height = 40.0;
			image.Width = 40.0;
			image.Stretch = Stretch.Uniform;
			this.btnMarkAll.Content = image;
			this.btnMarkAll.Click += this.MarkAllProducts;
			this.btnMarkAll.Style = (base.FindResource("defaultButtonStyle") as Style);
			this.btnMarkAll.Margin = new Thickness(0.0, 0.0, 10.0, 0.0);
			this.Buttons.Add(this.btnMarkAll);
			this.btnStartBackup = new Button();
			Image image2 = ImageConverter.ToImage("/img/backup_disabled.png");
			image2.Height = 45.0;
			image2.Width = 45.0;
			image2.Stretch = Stretch.Uniform;
			this.btnStartBackup.Content = image2;
			this.btnStartBackup.Click += this.btnStartBackup_Click;
			this.btnStartBackup.Style = (base.FindResource("defaultButtonStyle") as Style);
			this.Buttons.Add(this.btnStartBackup);
		}

		// Token: 0x06000621 RID: 1569 RVA: 0x00029CA0 File Offset: 0x00027EA0
		private void MarkAllProducts(object sender, RoutedEventArgs e)
		{
			bool flag = this.productManager.SelectedProducts.Count == this.productManager.ProductList.Count;
			if (flag)
			{
				this.lstProducts.SelectedItems.Clear();
			}
			else
			{
				this.lstProducts.SelectAll();
				this.DrawMarkings();
			}
		}

		// Token: 0x06000622 RID: 1570 RVA: 0x00029D00 File Offset: 0x00027F00
		public void SelectedItems(object sender, SelectionChangedEventArgs eventArgs)
		{
			IList selectedItems = ((ListBox)sender).SelectedItems;
			this.productManager.SelectedProducts = selectedItems.Cast<ProductData>().ToList<ProductData>();
			bool flag = this.productManager.SelectedProducts != null;
			if (flag)
			{
				bool flag2 = this.productManager.SelectedProducts.Count > 0;
				if (flag2)
				{
					this.productManager.SelectedProduct = this.productManager.SelectedProducts.Last<ProductData>();
				}
			}
			else
			{
				this.productManager.SelectedProduct = null;
			}
			bool flag3 = this.productManager.SelectedProducts.Count >= 1;
			if (flag3)
			{
				this.btnStartBackup.IsEnabled = true;
				Image image = ImageConverter.ToImage("/img/backup.png");
				image.Height = 45.0;
				image.Width = 45.0;
				image.Stretch = Stretch.Uniform;
				this.btnStartBackup.Content = image;
			}
			else
			{
				this.btnStartBackup.IsEnabled = false;
				Image image2 = ImageConverter.ToImage("/img/backup_disabled.png");
				image2.Height = 45.0;
				image2.Width = 45.0;
				image2.Stretch = Stretch.Uniform;
				this.btnStartBackup.Content = image2;
			}
		}

		// Token: 0x06000623 RID: 1571 RVA: 0x00029E50 File Offset: 0x00028050
		private void DrawMarkings()
		{
			bool flag = this.lstProducts.Items.Count > 0;
			if (flag)
			{
				ListBoxItem listBoxItem = this.lstProducts.ItemContainerGenerator.ContainerFromItem(this.lstProducts.Items[0]) as ListBoxItem;
				bool flag2 = listBoxItem != null;
				if (flag2)
				{
					listBoxItem.Focus();
				}
			}
		}

		// Token: 0x06000624 RID: 1572 RVA: 0x00029EB0 File Offset: 0x000280B0
		private void btnStartBackup_Click(object sender, RoutedEventArgs e)
		{
			List<string> list = new List<string>();
			foreach (object obj in this.lstProducts.SelectedItems)
			{
				ProductData productData = obj as ProductData;
				bool flag = productData != null;
				if (flag)
				{
					string tableName = productData.TableName;
					list.Add(tableName.Insert("AppDefault".Length, "AI"));
				}
			}
			this.backupListViewModel.DoBackup(list.ToArray());
			this.DrawMarkings();
		}

		// Token: 0x06000625 RID: 1573 RVA: 0x00029F64 File Offset: 0x00028164
		public void AddBackupPath(string path)
		{
			this.backupListViewModel.AdditionalBackupPath = path;
		}

		// Token: 0x06000626 RID: 1574 RVA: 0x00029F74 File Offset: 0x00028174
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/backup/ctrlbackuplist.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x06000627 RID: 1575 RVA: 0x00029FAC File Offset: 0x000281AC
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			if (connectionId != 1)
			{
				this._contentLoaded = true;
			}
			else
			{
				this.lstProducts = (ListBox)target;
				this.lstProducts.SelectionChanged += this.SelectedItems;
			}
		}

		// Token: 0x0400046F RID: 1135
		private Button btnStartBackup;

		// Token: 0x04000470 RID: 1136
		private Button btnMarkAll;

		// Token: 0x04000471 RID: 1137
		private BackupListViewModel backupListViewModel;

		// Token: 0x04000472 RID: 1138
		private ProductManager productManager;

		// Token: 0x04000475 RID: 1141
		internal ListBox lstProducts;

		// Token: 0x04000476 RID: 1142
		private bool _contentLoaded;
	}
}
