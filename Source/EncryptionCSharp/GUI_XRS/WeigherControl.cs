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
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x0200025A RID: 602
	public class WeigherControl : UserControl, IComponentConnector
	{
		// Token: 0x17000C68 RID: 3176
		// (get) Token: 0x06002E53 RID: 11859 RVA: 0x0012EBEB File Offset: 0x0012CDEB
		// (set) Token: 0x06002E54 RID: 11860 RVA: 0x0012EBF3 File Offset: 0x0012CDF3
		public List<Button> Buttons { get; set; }

		// Token: 0x06002E55 RID: 11861 RVA: 0x0012EBFC File Offset: 0x0012CDFC
		public WeigherControl(ParameterHandling parHandling, ImageDataTransfer imageTransfer, DBConnection dbConnection, MachineState machineState, ProductManager productManager, int imageGroup, AccessRightsManager accessRightsManager)
		{
			this.InitializeComponent();
			this.InitializeButtons();
			this.weigher = new Weigher(dbConnection, parHandling, imageTransfer, machineState, productManager, imageGroup, accessRightsManager);
			base.DataContext = this.weigher;
			this.Buttons[0].DataContext = this.weigher;
		}

		// Token: 0x06002E56 RID: 11862 RVA: 0x0012EC5C File Offset: 0x0012CE5C
		private void InitializeButtons()
		{
			this.Buttons = new List<Button>();
			Button button = new Button();
			button.Name = "btnLearn";
			Binding binding = new Binding();
			binding.Path = new PropertyPath("IsEnabled", new object[0]);
			binding.Converter = new BoolToVisibilityHiddenConverter();
			Image image = ImageConverter.ToImage("../img/learnweigher.png");
			image.Stretch = Stretch.Uniform;
			image.Height = 48.0;
			image.Width = 41.0;
			image.SetBinding(UIElement.VisibilityProperty, binding);
			Binding binding2 = new Binding();
			binding2.Path = new PropertyPath("IsEnabled", new object[0]);
			binding2.Converter = new BoolToVisibilityInvertedHiddenConverter();
			Image image2 = ImageConverter.ToImage("../img/learnweigher_disabled.png");
			image2.Stretch = Stretch.Uniform;
			image2.Height = 48.0;
			image2.Width = 41.0;
			image2.SetBinding(UIElement.VisibilityProperty, binding2);
			button.Content = new Grid
			{
				Children = 
				{
					image,
					image2
				}
			};
			Binding item = new Binding("MachineState.State");
			Binding item2 = new Binding("DemoMode");
			MultiBinding multiBinding = new MultiBinding();
			multiBinding.Converter = new AutolearnMachineStatesBoolConverter();
			multiBinding.Bindings.Add(item);
			multiBinding.Bindings.Add(item2);
			multiBinding.NotifyOnSourceUpdated = true;
			button.SetBinding(UIElement.IsEnabledProperty, multiBinding);
			button.Click += this.btnLearn_Click;
			button.Style = (base.FindResource("defaultButtonStyle") as Style);
			this.Buttons.Add(button);
			image.DataContext = button;
			image2.DataContext = button;
		}

		// Token: 0x06002E57 RID: 11863 RVA: 0x0012EE35 File Offset: 0x0012D035
		private void btnLearn_Click(object sender, RoutedEventArgs e)
		{
			this.weigher.LearnWeight();
		}

		// Token: 0x06002E58 RID: 11864 RVA: 0x0012EE44 File Offset: 0x0012D044
		private void WeigherControl_Unloaded(object sender, RoutedEventArgs e)
		{
			this.weigher.CleanUp();
			this.Buttons.Clear();
		}

		// Token: 0x06002E59 RID: 11865 RVA: 0x0012EE60 File Offset: 0x0012D060
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/weigher/weighercontrol.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x06002E5A RID: 11866 RVA: 0x0012EE98 File Offset: 0x0012D098
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		internal Delegate _CreateDelegate(Type delegateType, string handler)
		{
			return Delegate.CreateDelegate(delegateType, this, handler);
		}

		// Token: 0x06002E5B RID: 11867 RVA: 0x0012EEB4 File Offset: 0x0012D0B4
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
				((WeigherControl)target).Unloaded += this.WeigherControl_Unloaded;
			}
		}

		// Token: 0x04001D06 RID: 7430
		private const string imageLearnPath = "../img/learnweigher.png";

		// Token: 0x04001D07 RID: 7431
		private Weigher weigher;

		// Token: 0x04001D09 RID: 7433
		private bool _contentLoaded;
	}
}
