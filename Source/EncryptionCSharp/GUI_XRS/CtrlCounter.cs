using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using DatabaseConnection;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x02000233 RID: 563
	public class CtrlCounter : UserControl, IComponentConnector
	{
		// Token: 0x17000BB4 RID: 2996
		// (get) Token: 0x06002ADA RID: 10970 RVA: 0x0011CBA6 File Offset: 0x0011ADA6
		// (set) Token: 0x06002ADB RID: 10971 RVA: 0x0011CBAE File Offset: 0x0011ADAE
		public List<Button> Buttons { get; set; }

		// Token: 0x06002ADC RID: 10972 RVA: 0x0011CBB7 File Offset: 0x0011ADB7
		public CtrlCounter(ParameterHandling parHandling, ImageDataTransfer imageTransfer, DBConnection dbConnection, int group, ProductManager productManager, StoreImageData imageArchiver, MachineState machineState)
		{
			this.InitializeComponent();
			this.objectCounterViewModel = new ObjectCounterViewModel(parHandling, imageTransfer, dbConnection, group, productManager, imageArchiver, machineState);
			base.DataContext = this.objectCounterViewModel;
			this.InitializeButtons();
		}

		// Token: 0x06002ADD RID: 10973 RVA: 0x0011CBF4 File Offset: 0x0011ADF4
		private void InitializeButtons()
		{
			this.Buttons = new List<Button>();
			this.btnPlayFiles = new Button();
			Image image = ImageConverter.ToImage("../img/play_bright_small.png");
			this.btnPlayFiles.Click += this.objectCounterViewModel.TestSettingsRoutedEvent;
			this.btnPlayFiles.Style = (base.FindResource("defaultButtonStyle") as Style);
			Binding binding = new Binding();
			binding.Path = new PropertyPath("IsEnabled", new object[0]);
			binding.Converter = new BoolToVisibilityHiddenConverter();
			image.SetBinding(UIElement.VisibilityProperty, binding);
			Binding binding2 = new Binding();
			binding2.Path = new PropertyPath("IsEnabled", new object[0]);
			binding2.Converter = new BoolToVisibilityInvertedHiddenConverter();
			Image image2 = ImageConverter.ToImage("../img/play_bright_small_disabled.png");
			image2.SetBinding(UIElement.VisibilityProperty, binding2);
			Grid grid = new Grid();
			grid.Children.Add(image);
			grid.Children.Add(image2);
			this.btnPlayFiles.Content = grid;
			Binding item = new Binding("MachineState.State");
			Binding item2 = new Binding("DemoMode");
			MultiBinding multiBinding = new MultiBinding();
			multiBinding.Converter = new ProductChangeMachineStatesBoolConverter();
			multiBinding.Bindings.Add(item);
			multiBinding.Bindings.Add(item2);
			multiBinding.NotifyOnSourceUpdated = true;
			this.btnPlayFiles.SetBinding(UIElement.IsEnabledProperty, multiBinding);
			this.btnPlayFiles.DataContext = base.DataContext;
			this.Buttons.Add(this.btnPlayFiles);
			image.DataContext = this.btnPlayFiles;
			image2.DataContext = this.btnPlayFiles;
		}

		// Token: 0x06002ADE RID: 10974 RVA: 0x0011CDA9 File Offset: 0x0011AFA9
		private void CtrlCounter_Unloaded(object sender, RoutedEventArgs e)
		{
			this.objectCounterViewModel.DeregisterLocalizedText();
			this.btnPlayFiles.Click -= this.objectCounterViewModel.TestSettingsRoutedEvent;
			this.Buttons.Clear();
		}

		// Token: 0x06002ADF RID: 10975 RVA: 0x0011CDE4 File Offset: 0x0011AFE4
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/objectcounter/ctrlcounter.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x06002AE0 RID: 10976 RVA: 0x0011CE1C File Offset: 0x0011B01C
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
				((CtrlCounter)target).Unloaded += this.CtrlCounter_Unloaded;
			}
		}

		// Token: 0x04001AC2 RID: 6850
		private const string imagePlayFilesPath = "../img/play_bright_small.png";

		// Token: 0x04001AC3 RID: 6851
		private ObjectCounterViewModel objectCounterViewModel;

		// Token: 0x04001AC4 RID: 6852
		private Button btnPlayFiles;

		// Token: 0x04001AC6 RID: 6854
		private bool _contentLoaded;
	}
}
