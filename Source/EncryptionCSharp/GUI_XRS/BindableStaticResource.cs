using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace GUI_XRS
{
	// Token: 0x02000198 RID: 408
	public class BindableStaticResource : StaticResourceExtension
	{
		// Token: 0x17000AFE RID: 2814
		// (get) Token: 0x060026BD RID: 9917 RVA: 0x00107476 File Offset: 0x00105676
		// (set) Token: 0x060026BE RID: 9918 RVA: 0x0010747E File Offset: 0x0010567E
		public Binding MyBinding { get; set; }

		// Token: 0x060026BF RID: 9919 RVA: 0x00107487 File Offset: 0x00105687
		public BindableStaticResource()
		{
		}

		// Token: 0x060026C0 RID: 9920 RVA: 0x00107491 File Offset: 0x00105691
		public BindableStaticResource(Binding binding)
		{
			this.MyBinding = binding;
		}

		// Token: 0x060026C1 RID: 9921 RVA: 0x001074A4 File Offset: 0x001056A4
		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			IProvideValueTarget provideValueTarget = (IProvideValueTarget)serviceProvider.GetService(typeof(IProvideValueTarget));
			FrameworkElement frameworkElement = (FrameworkElement)provideValueTarget.TargetObject;
			this.MyBinding.Source = frameworkElement.DataContext;
			DependencyObject dependencyObject = new DependencyObject();
			BindingOperations.SetBinding(dependencyObject, BindableStaticResource.DummyProperty, this.MyBinding);
			base.ResourceKey = dependencyObject.GetValue(BindableStaticResource.DummyProperty);
			return base.ProvideValue(serviceProvider);
		}

		// Token: 0x040018D7 RID: 6359
		private static readonly DependencyProperty DummyProperty = DependencyProperty.RegisterAttached("Dummy", typeof(object), typeof(DependencyObject), new UIPropertyMetadata(null));
	}
}
