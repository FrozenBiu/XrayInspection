using System;
using SNRPlotLibrary;

namespace GUI_XRS.Graph
{
	// Token: 0x02000275 RID: 629
	public interface IPlotDetailsArrived
	{
		// Token: 0x14000115 RID: 277
		// (add) Token: 0x060034A6 RID: 13478
		// (remove) Token: 0x060034A7 RID: 13479
		event EventHandler<Details> PlotDetailsArrived;
	}
}
