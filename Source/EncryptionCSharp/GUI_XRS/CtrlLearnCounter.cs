using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using DatabaseConnection;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x02000133 RID: 307
	public class CtrlLearnCounter : UserControl, IComponentConnector
	{
		// Token: 0x170008AB RID: 2219
		// (get) Token: 0x06001D9A RID: 7578 RVA: 0x000C098C File Offset: 0x000BEB8C
		// (set) Token: 0x06001D9B RID: 7579 RVA: 0x000C0994 File Offset: 0x000BEB94
		public Grid PanelSubButtons { get; set; }

		// Token: 0x06001D9C RID: 7580 RVA: 0x000C09A0 File Offset: 0x000BEBA0
		public CtrlLearnCounter(DBConnection dbConnection, ImageDataTransfer imageTransfer, ParameterHandling parHandling, int group, ProductManager productManager, StoreImageData imageArchiver, MachineState machineState)
		{
			this.InitializeComponent();
			this.viewModel = new LearnCounterViewModel(parHandling, imageTransfer, dbConnection, group, this.imagebox, productManager, imageArchiver, machineState);
			base.DataContext = this.viewModel;
			this.InitializeButtonsPanel();
		}

		// Token: 0x06001D9D RID: 7581 RVA: 0x000C09EC File Offset: 0x000BEBEC
		private void InitializeButtonsPanel()
		{
			this.PanelSubButtons = new Grid();
			this.PanelSubButtons.ColumnDefinitions.Add(new ColumnDefinition());
			this.PanelSubButtons.ColumnDefinitions.Add(new ColumnDefinition());
			this.PanelSubButtons.ColumnDefinitions.Add(new ColumnDefinition());
			this.PanelSubButtons.ColumnDefinitions.Add(new ColumnDefinition());
			this.moveImage = ImageConverter.ToImage("/img/moveImage.png");
			this.moveImage.Height = 40.0;
			this.moveImage.Width = 40.0;
			this.moveImage.Stretch = Stretch.Uniform;
			this.selectImage = ImageConverter.ToImage("/img/rectangleDotted.png");
			this.selectImage.Height = 40.0;
			this.selectImage.Width = 40.0;
			this.selectImage.Stretch = Stretch.Uniform;
			this.btnLearn = new Button();
			Image image = ImageConverter.ToImage("/img/autolearnregion.png");
			image.Height = 40.0;
			image.Width = 40.0;
			image.Stretch = Stretch.Uniform;
			this.btnLearn.Click += this.Learn;
			this.btnLearn.Style = (base.FindResource("defaultButtonStyle") as Style);
			Grid.SetColumn(this.btnLearn, 0);
			this.btnLearn.Margin = new Thickness(0.0, 0.0, 10.0, 0.0);
			Binding binding = new Binding();
			binding.Path = new PropertyPath("IsEnabled", new object[0]);
			binding.Converter = new BoolToVisibilityHiddenConverter();
			image.SetBinding(UIElement.VisibilityProperty, binding);
			Binding binding2 = new Binding();
			binding2.Path = new PropertyPath("IsEnabled", new object[0]);
			binding2.Converter = new BoolToVisibilityInvertedHiddenConverter();
			Image image2 = ImageConverter.ToImage("/img/autolearnregion_disabled.png");
			image2.Stretch = Stretch.Uniform;
			image2.Height = 40.0;
			image2.Width = 40.0;
			image2.SetBinding(UIElement.VisibilityProperty, binding2);
			Grid grid = new Grid();
			grid.Children.Add(image);
			grid.Children.Add(image2);
			this.btnLearn.Content = grid;
			Binding item = new Binding("MachineState.State");
			Binding item2 = new Binding("DemoMode");
			MultiBinding multiBinding = new MultiBinding();
			multiBinding.Converter = new ProductChangeMachineStatesBoolConverter();
			multiBinding.Bindings.Add(item);
			multiBinding.Bindings.Add(item2);
			multiBinding.NotifyOnSourceUpdated = true;
			this.btnLearn.SetBinding(UIElement.IsEnabledProperty, multiBinding);
			this.btnLearn.DataContext = base.DataContext;
			this.PanelSubButtons.Children.Add(this.btnLearn);
			image.DataContext = this.btnLearn;
			image2.DataContext = this.btnLearn;
			this.btnCounterSettings = new Button();
			Image image3 = ImageConverter.ToImage("../img/counter2.png");
			image3.Height = 40.0;
			image3.Width = 40.0;
			image3.Stretch = Stretch.Uniform;
			this.btnCounterSettings.Content = image3;
			this.btnCounterSettings.Click += this.ShowCounterControl;
			this.btnCounterSettings.Style = (base.FindResource("defaultButtonStyle") as Style);
			Grid.SetColumn(this.btnCounterSettings, 1);
			this.btnCounterSettings.Margin = new Thickness(0.0, 0.0, 25.0, 0.0);
			this.PanelSubButtons.Children.Add(this.btnCounterSettings);
			this.btnMoveOrSelect = new Button();
			this.btnMoveOrSelect.Content = this.selectImage;
			this.btnMoveOrSelect.Click += this.ChangeToMoveOrSelect;
			this.btnMoveOrSelect.Style = (base.FindResource("defaultButtonStyle") as Style);
			Grid.SetColumn(this.btnMoveOrSelect, 2);
			this.PanelSubButtons.Children.Add(this.btnMoveOrSelect);
			bool highSpeedMode = this.viewModel.HighSpeedMode;
			if (highSpeedMode)
			{
				Binding binding3 = new Binding();
				binding3.Path = new PropertyPath("IsEnabled", new object[0]);
				binding3.Converter = new BoolToVisibilityHiddenConverter();
				Binding binding4 = new Binding();
				binding4.Path = new PropertyPath("IsEnabled", new object[0]);
				binding4.Converter = new BoolToVisibilityInvertedHiddenConverter();
				this.btnHighSpeedMode = new Button();
				Image image4 = ImageConverter.ToImage("../img/objectxriimage.png");
				image4.SetBinding(UIElement.VisibilityProperty, binding3);
				image4.Height = 40.0;
				image4.Width = 40.0;
				image4.Stretch = Stretch.Uniform;
				Image image5 = ImageConverter.ToImage("/img/objectxriimage_disabled.png");
				image5.Stretch = Stretch.Uniform;
				image5.Height = 40.0;
				image5.Width = 40.0;
				image5.SetBinding(UIElement.VisibilityProperty, binding4);
				Grid grid2 = new Grid();
				grid2.Children.Add(image4);
				grid2.Children.Add(image5);
				this.btnHighSpeedMode.Content = grid2;
				this.btnHighSpeedMode.Click += this.HighSpeedModeCommand;
				this.btnHighSpeedMode.Style = (base.FindResource("defaultButtonStyleActive") as Style);
				Grid.SetColumn(this.btnHighSpeedMode, 3);
				this.btnHighSpeedMode.Margin = new Thickness(25.0, 0.0, 25.0, 0.0);
				MultiBinding multiBinding2 = new MultiBinding();
				multiBinding2.Converter = new AutolearnMachineStatesBoolConverter();
				multiBinding2.Bindings.Add(item);
				multiBinding2.Bindings.Add(item2);
				multiBinding2.NotifyOnSourceUpdated = true;
				this.btnHighSpeedMode.SetBinding(UIElement.IsEnabledProperty, multiBinding2);
				this.btnHighSpeedMode.DataContext = base.DataContext;
				image4.DataContext = this.btnHighSpeedMode;
				image5.DataContext = this.btnHighSpeedMode;
				this.PanelSubButtons.Children.Add(this.btnHighSpeedMode);
			}
			this.imagebox.ToggleBoxSelectionModeAndButtonImage(this.btnMoveOrSelect);
		}

		// Token: 0x06001D9E RID: 7582 RVA: 0x000C10C0 File Offset: 0x000BF2C0
		private void HighSpeedModeCommand(object sender, RoutedEventArgs e)
		{
			bool showNext = this.viewModel.ShowNext;
			if (showNext)
			{
				this.viewModel.ShowNext = false;
			}
			else
			{
				this.viewModel.ShowNext = true;
			}
		}

		// Token: 0x06001D9F RID: 7583 RVA: 0x000C10FD File Offset: 0x000BF2FD
		private void Learn(object sender, RoutedEventArgs e)
		{
			this.viewModel.Learn();
		}

		// Token: 0x06001DA0 RID: 7584 RVA: 0x000C110C File Offset: 0x000BF30C
		private void ShowCounterControl(object sender, RoutedEventArgs e)
		{
			this.viewModel.ShowCounterControl();
		}

		// Token: 0x06001DA1 RID: 7585 RVA: 0x000C111C File Offset: 0x000BF31C
		private void UserControl_Unloaded(object sender, RoutedEventArgs e)
		{
			this.viewModel.CleanUp();
			this.selectImage = null;
			BindingOperations.ClearAllBindings(this);
			this.btnLearn.Click -= this.Learn;
			this.btnCounterSettings.Click -= this.ShowCounterControl;
			this.btnMoveOrSelect.Click -= this.ChangeToMoveOrSelect;
			bool flag = this.btnHighSpeedMode != null;
			if (flag)
			{
				this.btnHighSpeedMode.Click -= this.HighSpeedModeCommand;
			}
			this.PanelSubButtons.Children.Clear();
			base.DataContext = null;
		}

		// Token: 0x06001DA2 RID: 7586 RVA: 0x000C11CA File Offset: 0x000BF3CA
		private void ChangeToMoveOrSelect(object sender, RoutedEventArgs e)
		{
			this.imagebox.ToggleBoxSelectionModeAndButtonImage(this.btnMoveOrSelect);
		}

		// Token: 0x06001DA3 RID: 7587 RVA: 0x000C11E0 File Offset: 0x000BF3E0
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/objectcounter/ctrllearncounter.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x06001DA4 RID: 7588 RVA: 0x000C1218 File Offset: 0x000BF418
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			if (connectionId != 1)
			{
				if (connectionId != 2)
				{
					this._contentLoaded = true;
				}
				else
				{
					this.imagebox = (XRSImageBox)target;
				}
			}
			else
			{
				((CtrlLearnCounter)target).Unloaded += this.UserControl_Unloaded;
			}
		}

		// Token: 0x0400137A RID: 4986
		private LearnCounterViewModel viewModel;

		// Token: 0x0400137B RID: 4987
		private Button btnMoveOrSelect;

		// Token: 0x0400137C RID: 4988
		private Button btnLearn;

		// Token: 0x0400137D RID: 4989
		private Button btnCounterSettings;

		// Token: 0x0400137E RID: 4990
		private Image moveImage;

		// Token: 0x0400137F RID: 4991
		private Image selectImage;

		// Token: 0x04001380 RID: 4992
		private Button btnHighSpeedMode;

		// Token: 0x04001382 RID: 4994
		internal XRSImageBox imagebox;

		// Token: 0x04001383 RID: 4995
		private bool _contentLoaded;
	}
}
