using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace GUI_XRS.Properties
{
	// Token: 0x0200026E RID: 622
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	public class Resources
	{
		// Token: 0x06002FC8 RID: 12232 RVA: 0x00029776 File Offset: 0x00027976
		internal Resources()
		{
		}

		// Token: 0x17000CCC RID: 3276
		// (get) Token: 0x06002FC9 RID: 12233 RVA: 0x0013BCA0 File Offset: 0x00139EA0
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static ResourceManager ResourceManager
		{
			get
			{
				bool flag = Resources.resourceMan == null;
				if (flag)
				{
					ResourceManager resourceManager = new ResourceManager("GUI_XRS.Properties.Resources", typeof(Resources).Assembly);
					Resources.resourceMan = resourceManager;
				}
				return Resources.resourceMan;
			}
		}

		// Token: 0x17000CCD RID: 3277
		// (get) Token: 0x06002FCA RID: 12234 RVA: 0x0013BCE8 File Offset: 0x00139EE8
		// (set) Token: 0x06002FCB RID: 12235 RVA: 0x0013BCFF File Offset: 0x00139EFF
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static CultureInfo Culture
		{
			get
			{
				return Resources.resourceCulture;
			}
			set
			{
				Resources.resourceCulture = value;
			}
		}

		// Token: 0x17000CCE RID: 3278
		// (get) Token: 0x06002FCC RID: 12236 RVA: 0x0013BD08 File Offset: 0x00139F08
		public static Bitmap bizerba_powered_by_sesotec
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("bizerba_powered_by_sesotec", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}

		// Token: 0x17000CCF RID: 3279
		// (get) Token: 0x06002FCD RID: 12237 RVA: 0x0013BD38 File Offset: 0x00139F38
		public static Bitmap BizerbaLogo_big
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("BizerbaLogo_big", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}

		// Token: 0x17000CD0 RID: 3280
		// (get) Token: 0x06002FCE RID: 12238 RVA: 0x0013BD68 File Offset: 0x00139F68
		public static Bitmap buttonCancel
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("buttonCancel", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}

		// Token: 0x17000CD1 RID: 3281
		// (get) Token: 0x06002FCF RID: 12239 RVA: 0x0013BD98 File Offset: 0x00139F98
		public static Bitmap buttonOK
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("buttonOK", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}

		// Token: 0x17000CD2 RID: 3282
		// (get) Token: 0x06002FD0 RID: 12240 RVA: 0x0013BDC8 File Offset: 0x00139FC8
		public static Bitmap buttonUndo
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("buttonUndo", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}

		// Token: 0x17000CD3 RID: 3283
		// (get) Token: 0x06002FD1 RID: 12241 RVA: 0x0013BDF8 File Offset: 0x00139FF8
		public static string Check_RFID_Login
		{
			get
			{
				return Resources.ResourceManager.GetString("Check_RFID_Login", Resources.resourceCulture);
			}
		}

		// Token: 0x17000CD4 RID: 3284
		// (get) Token: 0x06002FD2 RID: 12242 RVA: 0x0013BE20 File Offset: 0x0013A020
		public static string create_rfid_login
		{
			get
			{
				return Resources.ResourceManager.GetString("create_rfid_login", Resources.resourceCulture);
			}
		}

		// Token: 0x17000CD5 RID: 3285
		// (get) Token: 0x06002FD3 RID: 12243 RVA: 0x0013BE48 File Offset: 0x0013A048
		public static string create_rfid_login_for_selected_user
		{
			get
			{
				return Resources.ResourceManager.GetString("create_rfid_login_for_selected_user", Resources.resourceCulture);
			}
		}

		// Token: 0x17000CD6 RID: 3286
		// (get) Token: 0x06002FD4 RID: 12244 RVA: 0x0013BE70 File Offset: 0x0013A070
		public static string delete_rfid_login
		{
			get
			{
				return Resources.ResourceManager.GetString("delete_rfid_login", Resources.resourceCulture);
			}
		}

		// Token: 0x17000CD7 RID: 3287
		// (get) Token: 0x06002FD5 RID: 12245 RVA: 0x0013BE98 File Offset: 0x0013A098
		public static string delete_rfid_login_for_the_selected_user
		{
			get
			{
				return Resources.ResourceManager.GetString("delete_rfid_login_for_the_selected_user", Resources.resourceCulture);
			}
		}

		// Token: 0x17000CD8 RID: 3288
		// (get) Token: 0x06002FD6 RID: 12246 RVA: 0x0013BEC0 File Offset: 0x0013A0C0
		public static string Hold_RFID_token_to_reader
		{
			get
			{
				return Resources.ResourceManager.GetString("Hold_RFID_token_to_reader", Resources.resourceCulture);
			}
		}

		// Token: 0x17000CD9 RID: 3289
		// (get) Token: 0x06002FD7 RID: 12247 RVA: 0x0013BEE8 File Offset: 0x0013A0E8
		public static string lbAnomalyDetectionCorruptedFileInference
		{
			get
			{
				return Resources.ResourceManager.GetString("lbAnomalyDetectionCorruptedFileInference", Resources.resourceCulture);
			}
		}

		// Token: 0x17000CDA RID: 3290
		// (get) Token: 0x06002FD8 RID: 12248 RVA: 0x0013BF10 File Offset: 0x0013A110
		public static string lbAnomalyDetectionCorruptedFileTrain
		{
			get
			{
				return Resources.ResourceManager.GetString("lbAnomalyDetectionCorruptedFileTrain", Resources.resourceCulture);
			}
		}

		// Token: 0x17000CDB RID: 3291
		// (get) Token: 0x06002FD9 RID: 12249 RVA: 0x0013BF38 File Offset: 0x0013A138
		public static string lbAnomalyDetectionDeleteModel
		{
			get
			{
				return Resources.ResourceManager.GetString("lbAnomalyDetectionDeleteModel", Resources.resourceCulture);
			}
		}

		// Token: 0x17000CDC RID: 3292
		// (get) Token: 0x06002FDA RID: 12250 RVA: 0x0013BF60 File Offset: 0x0013A160
		public static string lbAnomalyDetectionDeploymentError
		{
			get
			{
				return Resources.ResourceManager.GetString("lbAnomalyDetectionDeploymentError", Resources.resourceCulture);
			}
		}

		// Token: 0x17000CDD RID: 3293
		// (get) Token: 0x06002FDB RID: 12251 RVA: 0x0013BF88 File Offset: 0x0013A188
		public static string lbAnomalyDetectionInferenceActivated
		{
			get
			{
				return Resources.ResourceManager.GetString("lbAnomalyDetectionInferenceActivated", Resources.resourceCulture);
			}
		}

		// Token: 0x17000CDE RID: 3294
		// (get) Token: 0x06002FDC RID: 12252 RVA: 0x0013BFB0 File Offset: 0x0013A1B0
		public static string lbAnomalyDetectionInferenceDeactivated
		{
			get
			{
				return Resources.ResourceManager.GetString("lbAnomalyDetectionInferenceDeactivated", Resources.resourceCulture);
			}
		}

		// Token: 0x17000CDF RID: 3295
		// (get) Token: 0x06002FDD RID: 12253 RVA: 0x0013BFD8 File Offset: 0x0013A1D8
		public static string lbAnomalyDetectionInferenceError
		{
			get
			{
				return Resources.ResourceManager.GetString("lbAnomalyDetectionInferenceError", Resources.resourceCulture);
			}
		}

		// Token: 0x17000CE0 RID: 3296
		// (get) Token: 0x06002FDE RID: 12254 RVA: 0x0013C000 File Offset: 0x0013A200
		public static string lbAnomalyDetectionMissingFileInference
		{
			get
			{
				return Resources.ResourceManager.GetString("lbAnomalyDetectionMissingFileInference", Resources.resourceCulture);
			}
		}

		// Token: 0x17000CE1 RID: 3297
		// (get) Token: 0x06002FDF RID: 12255 RVA: 0x0013C028 File Offset: 0x0013A228
		public static string lbAnomalyDetectionMissingMachineSubType
		{
			get
			{
				return Resources.ResourceManager.GetString("lbAnomalyDetectionMissingMachineSubType", Resources.resourceCulture);
			}
		}

		// Token: 0x17000CE2 RID: 3298
		// (get) Token: 0x06002FE0 RID: 12256 RVA: 0x0013C050 File Offset: 0x0013A250
		public static string lbAnomalyDetectionRestoreModel
		{
			get
			{
				return Resources.ResourceManager.GetString("lbAnomalyDetectionRestoreModel", Resources.resourceCulture);
			}
		}

		// Token: 0x17000CE3 RID: 3299
		// (get) Token: 0x06002FE1 RID: 12257 RVA: 0x0013C078 File Offset: 0x0013A278
		public static string lbAnomalyDetectionStopXray
		{
			get
			{
				return Resources.ResourceManager.GetString("lbAnomalyDetectionStopXray", Resources.resourceCulture);
			}
		}

		// Token: 0x17000CE4 RID: 3300
		// (get) Token: 0x06002FE2 RID: 12258 RVA: 0x0013C0A0 File Offset: 0x0013A2A0
		public static string lbAnomalyDetectionTrainActivated
		{
			get
			{
				return Resources.ResourceManager.GetString("lbAnomalyDetectionTrainActivated", Resources.resourceCulture);
			}
		}

		// Token: 0x17000CE5 RID: 3301
		// (get) Token: 0x06002FE3 RID: 12259 RVA: 0x0013C0C8 File Offset: 0x0013A2C8
		public static string lbAnomalyDetectionTrainDeactivated
		{
			get
			{
				return Resources.ResourceManager.GetString("lbAnomalyDetectionTrainDeactivated", Resources.resourceCulture);
			}
		}

		// Token: 0x17000CE6 RID: 3302
		// (get) Token: 0x06002FE4 RID: 12260 RVA: 0x0013C0F0 File Offset: 0x0013A2F0
		public static string lbAnomalyDetectionTrainFinished
		{
			get
			{
				return Resources.ResourceManager.GetString("lbAnomalyDetectionTrainFinished", Resources.resourceCulture);
			}
		}

		// Token: 0x17000CE7 RID: 3303
		// (get) Token: 0x06002FE5 RID: 12261 RVA: 0x0013C118 File Offset: 0x0013A318
		public static string lbAnomalyDetectionTrainTransferData
		{
			get
			{
				return Resources.ResourceManager.GetString("lbAnomalyDetectionTrainTransferData", Resources.resourceCulture);
			}
		}

		// Token: 0x17000CE8 RID: 3304
		// (get) Token: 0x06002FE6 RID: 12262 RVA: 0x0013C140 File Offset: 0x0013A340
		public static string lbAuditCheck
		{
			get
			{
				return Resources.ResourceManager.GetString("lbAuditCheck", Resources.resourceCulture);
			}
		}

		// Token: 0x17000CE9 RID: 3305
		// (get) Token: 0x06002FE7 RID: 12263 RVA: 0x0013C168 File Offset: 0x0013A368
		public static string lbAuditContaminationActivated
		{
			get
			{
				return Resources.ResourceManager.GetString("lbAuditContaminationActivated", Resources.resourceCulture);
			}
		}

		// Token: 0x17000CEA RID: 3306
		// (get) Token: 0x06002FE8 RID: 12264 RVA: 0x0013C190 File Offset: 0x0013A390
		public static string lbAuditContaminationDeactivated
		{
			get
			{
				return Resources.ResourceManager.GetString("lbAuditContaminationDeactivated", Resources.resourceCulture);
			}
		}

		// Token: 0x17000CEB RID: 3307
		// (get) Token: 0x06002FE9 RID: 12265 RVA: 0x0013C1B8 File Offset: 0x0013A3B8
		public static string lbAuditObjectcountActivated
		{
			get
			{
				return Resources.ResourceManager.GetString("lbAuditObjectcountActivated", Resources.resourceCulture);
			}
		}

		// Token: 0x17000CEC RID: 3308
		// (get) Token: 0x06002FEA RID: 12266 RVA: 0x0013C1E0 File Offset: 0x0013A3E0
		public static string lbAuditObjectcountDeactivated
		{
			get
			{
				return Resources.ResourceManager.GetString("lbAuditObjectcountDeactivated", Resources.resourceCulture);
			}
		}

		// Token: 0x17000CED RID: 3309
		// (get) Token: 0x06002FEB RID: 12267 RVA: 0x0013C208 File Offset: 0x0013A408
		public static string lbAuditStarted
		{
			get
			{
				return Resources.ResourceManager.GetString("lbAuditStarted", Resources.resourceCulture);
			}
		}

		// Token: 0x17000CEE RID: 3310
		// (get) Token: 0x06002FEC RID: 12268 RVA: 0x0013C230 File Offset: 0x0013A430
		public static string lbAuditWeightActivated
		{
			get
			{
				return Resources.ResourceManager.GetString("lbAuditWeightActivated", Resources.resourceCulture);
			}
		}

		// Token: 0x17000CEF RID: 3311
		// (get) Token: 0x06002FED RID: 12269 RVA: 0x0013C258 File Offset: 0x0013A458
		public static string lbAuditWeightDeactivated
		{
			get
			{
				return Resources.ResourceManager.GetString("lbAuditWeightDeactivated", Resources.resourceCulture);
			}
		}

		// Token: 0x17000CF0 RID: 3312
		// (get) Token: 0x06002FEE RID: 12270 RVA: 0x0013C280 File Offset: 0x0013A480
		public static string lbAutolearnStarted
		{
			get
			{
				return Resources.ResourceManager.GetString("lbAutolearnStarted", Resources.resourceCulture);
			}
		}

		// Token: 0x17000CF1 RID: 3313
		// (get) Token: 0x06002FEF RID: 12271 RVA: 0x0013C2A8 File Offset: 0x0013A4A8
		public static string lbBinMissing
		{
			get
			{
				return Resources.ResourceManager.GetString("lbBinMissing", Resources.resourceCulture);
			}
		}

		// Token: 0x17000CF2 RID: 3314
		// (get) Token: 0x06002FF0 RID: 12272 RVA: 0x0013C2D0 File Offset: 0x0013A4D0
		public static string lbCheckpointRestored
		{
			get
			{
				return Resources.ResourceManager.GetString("lbCheckpointRestored", Resources.resourceCulture);
			}
		}

		// Token: 0x17000CF3 RID: 3315
		// (get) Token: 0x06002FF1 RID: 12273 RVA: 0x0013C2F8 File Offset: 0x0013A4F8
		public static string lbDatabaseOutOfDate
		{
			get
			{
				return Resources.ResourceManager.GetString("lbDatabaseOutOfDate", Resources.resourceCulture);
			}
		}

		// Token: 0x17000CF4 RID: 3316
		// (get) Token: 0x06002FF2 RID: 12274 RVA: 0x0013C320 File Offset: 0x0013A520
		public static string lbDate
		{
			get
			{
				return Resources.ResourceManager.GetString("lbDate", Resources.resourceCulture);
			}
		}

		// Token: 0x17000CF5 RID: 3317
		// (get) Token: 0x06002FF3 RID: 12275 RVA: 0x0013C348 File Offset: 0x0013A548
		public static string lbDetectorCommError
		{
			get
			{
				return Resources.ResourceManager.GetString("lbDetectorCommError", Resources.resourceCulture);
			}
		}

		// Token: 0x17000CF6 RID: 3318
		// (get) Token: 0x06002FF4 RID: 12276 RVA: 0x0013C370 File Offset: 0x0013A570
		public static string lbDiskSpaceLow
		{
			get
			{
				return Resources.ResourceManager.GetString("lbDiskSpaceLow", Resources.resourceCulture);
			}
		}

		// Token: 0x17000CF7 RID: 3319
		// (get) Token: 0x06002FF5 RID: 12277 RVA: 0x0013C398 File Offset: 0x0013A598
		public static string lbEdgeFilter
		{
			get
			{
				return Resources.ResourceManager.GetString("lbEdgeFilter", Resources.resourceCulture);
			}
		}

		// Token: 0x17000CF8 RID: 3320
		// (get) Token: 0x06002FF6 RID: 12278 RVA: 0x0013C3C0 File Offset: 0x0013A5C0
		public static string lbEdgeFilterActivated
		{
			get
			{
				return Resources.ResourceManager.GetString("lbEdgeFilterActivated", Resources.resourceCulture);
			}
		}

		// Token: 0x17000CF9 RID: 3321
		// (get) Token: 0x06002FF7 RID: 12279 RVA: 0x0013C3E8 File Offset: 0x0013A5E8
		public static string lbEdgeFilterDeactivated
		{
			get
			{
				return Resources.ResourceManager.GetString("lbEdgeFilterDeactivated", Resources.resourceCulture);
			}
		}

		// Token: 0x17000CFA RID: 3322
		// (get) Token: 0x06002FF8 RID: 12280 RVA: 0x0013C410 File Offset: 0x0013A610
		public static string lbElinosOutOfDate
		{
			get
			{
				return Resources.ResourceManager.GetString("lbElinosOutOfDate", Resources.resourceCulture);
			}
		}

		// Token: 0x17000CFB RID: 3323
		// (get) Token: 0x06002FF9 RID: 12281 RVA: 0x0013C438 File Offset: 0x0013A638
		public static string lbErrorConsistency
		{
			get
			{
				return Resources.ResourceManager.GetString("lbErrorConsistency", Resources.resourceCulture);
			}
		}

		// Token: 0x17000CFC RID: 3324
		// (get) Token: 0x06002FFA RID: 12282 RVA: 0x0013C460 File Offset: 0x0013A660
		public static string lbFalseObjectCountDetected
		{
			get
			{
				return Resources.ResourceManager.GetString("lbFalseObjectCountDetected", Resources.resourceCulture);
			}
		}

		// Token: 0x17000CFD RID: 3325
		// (get) Token: 0x06002FFB RID: 12283 RVA: 0x0013C488 File Offset: 0x0013A688
		public static string lbFalseweightDetected
		{
			get
			{
				return Resources.ResourceManager.GetString("lbFalseweightDetected", Resources.resourceCulture);
			}
		}

		// Token: 0x17000CFE RID: 3326
		// (get) Token: 0x06002FFC RID: 12284 RVA: 0x0013C4B0 File Offset: 0x0013A6B0
		public static string lbIgnoreClipActivated
		{
			get
			{
				return Resources.ResourceManager.GetString("lbIgnoreClipActivated", Resources.resourceCulture);
			}
		}

		// Token: 0x17000CFF RID: 3327
		// (get) Token: 0x06002FFD RID: 12285 RVA: 0x0013C4D8 File Offset: 0x0013A6D8
		public static string lbIgnoreClipDeactivated
		{
			get
			{
				return Resources.ResourceManager.GetString("lbIgnoreClipDeactivated", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D00 RID: 3328
		// (get) Token: 0x06002FFE RID: 12286 RVA: 0x0013C500 File Offset: 0x0013A700
		public static string lbImageProcessingPcOffline
		{
			get
			{
				return Resources.ResourceManager.GetString("lbImageProcessingPcOffline", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D01 RID: 3329
		// (get) Token: 0x06002FFF RID: 12287 RVA: 0x0013C528 File Offset: 0x0013A728
		public static string lbImageSizeAcitvated
		{
			get
			{
				return Resources.ResourceManager.GetString("lbImageSizeAcitvated", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D02 RID: 3330
		// (get) Token: 0x06003000 RID: 12288 RVA: 0x0013C550 File Offset: 0x0013A750
		public static string lbImageSizeDeacitvated
		{
			get
			{
				return Resources.ResourceManager.GetString("lbImageSizeDeacitvated", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D03 RID: 3331
		// (get) Token: 0x06003001 RID: 12289 RVA: 0x0013C578 File Offset: 0x0013A778
		public static string lbInsightNetConnectionBroken
		{
			get
			{
				return Resources.ResourceManager.GetString("lbInsightNetConnectionBroken", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D04 RID: 3332
		// (get) Token: 0x06003002 RID: 12290 RVA: 0x0013C5A0 File Offset: 0x0013A7A0
		public static string lbLogbookSaved
		{
			get
			{
				return Resources.ResourceManager.GetString("lbLogbookSaved", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D05 RID: 3333
		// (get) Token: 0x06003003 RID: 12291 RVA: 0x0013C5C8 File Offset: 0x0013A7C8
		public static string lbMachineTurnedOff
		{
			get
			{
				return Resources.ResourceManager.GetString("lbMachineTurnedOff", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D06 RID: 3334
		// (get) Token: 0x06003004 RID: 12292 RVA: 0x0013C5F0 File Offset: 0x0013A7F0
		public static string lbMachineTurnedOn
		{
			get
			{
				return Resources.ResourceManager.GetString("lbMachineTurnedOn", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D07 RID: 3335
		// (get) Token: 0x06003005 RID: 12293 RVA: 0x0013C618 File Offset: 0x0013A818
		public static string lbMemoryError
		{
			get
			{
				return Resources.ResourceManager.GetString("lbMemoryError", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D08 RID: 3336
		// (get) Token: 0x06003006 RID: 12294 RVA: 0x0013C640 File Offset: 0x0013A840
		public static string lbMemoryFileSystemError
		{
			get
			{
				return Resources.ResourceManager.GetString("lbMemoryFileSystemError", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D09 RID: 3337
		// (get) Token: 0x06003007 RID: 12295 RVA: 0x0013C668 File Offset: 0x0013A868
		public static string lbMemoryUsage
		{
			get
			{
				return Resources.ResourceManager.GetString("lbMemoryUsage", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D0A RID: 3338
		// (get) Token: 0x06003008 RID: 12296 RVA: 0x0013C690 File Offset: 0x0013A890
		public static string lbNewProductCreated
		{
			get
			{
				return Resources.ResourceManager.GetString("lbNewProductCreated", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D0B RID: 3339
		// (get) Token: 0x06003009 RID: 12297 RVA: 0x0013C6B8 File Offset: 0x0013A8B8
		public static string lbNewUserCreated
		{
			get
			{
				return Resources.ResourceManager.GetString("lbNewUserCreated", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D0C RID: 3340
		// (get) Token: 0x0600300A RID: 12298 RVA: 0x0013C6E0 File Offset: 0x0013A8E0
		public static string lbObjectCountActivated
		{
			get
			{
				return Resources.ResourceManager.GetString("lbObjectCountActivated", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D0D RID: 3341
		// (get) Token: 0x0600300B RID: 12299 RVA: 0x0013C708 File Offset: 0x0013A908
		public static string lbObjectCountDeactivated
		{
			get
			{
				return Resources.ResourceManager.GetString("lbObjectCountDeactivated", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D0E RID: 3342
		// (get) Token: 0x0600300C RID: 12300 RVA: 0x0013C730 File Offset: 0x0013A930
		public static string lbObjectCounter
		{
			get
			{
				return Resources.ResourceManager.GetString("lbObjectCounter", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D0F RID: 3343
		// (get) Token: 0x0600300D RID: 12301 RVA: 0x0013C758 File Offset: 0x0013A958
		public static string lbObjectDetectionInferenceActivated
		{
			get
			{
				return Resources.ResourceManager.GetString("lbObjectDetectionInferenceActivated", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D10 RID: 3344
		// (get) Token: 0x0600300E RID: 12302 RVA: 0x0013C780 File Offset: 0x0013A980
		public static string lbObjectDetectionInferenceDeactivated
		{
			get
			{
				return Resources.ResourceManager.GetString("lbObjectDetectionInferenceDeactivated", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D11 RID: 3345
		// (get) Token: 0x0600300F RID: 12303 RVA: 0x0013C7A8 File Offset: 0x0013A9A8
		public static string lbObjectDetectionInferenceError
		{
			get
			{
				return Resources.ResourceManager.GetString("lbObjectDetectionInferenceError", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D12 RID: 3346
		// (get) Token: 0x06003010 RID: 12304 RVA: 0x0013C7D0 File Offset: 0x0013A9D0
		public static string lbProcessingDelay
		{
			get
			{
				return Resources.ResourceManager.GetString("lbProcessingDelay", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D13 RID: 3347
		// (get) Token: 0x06003011 RID: 12305 RVA: 0x0013C7F8 File Offset: 0x0013A9F8
		public static string lbProductCopied
		{
			get
			{
				return Resources.ResourceManager.GetString("lbProductCopied", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D14 RID: 3348
		// (get) Token: 0x06003012 RID: 12306 RVA: 0x0013C820 File Offset: 0x0013AA20
		public static string lbProductDeleted
		{
			get
			{
				return Resources.ResourceManager.GetString("lbProductDeleted", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D15 RID: 3349
		// (get) Token: 0x06003013 RID: 12307 RVA: 0x0013C848 File Offset: 0x0013AA48
		public static string lbProductNotInspected
		{
			get
			{
				return Resources.ResourceManager.GetString("lbProductNotInspected", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D16 RID: 3350
		// (get) Token: 0x06003014 RID: 12308 RVA: 0x0013C870 File Offset: 0x0013AA70
		public static string lbProductNotRecognized
		{
			get
			{
				return Resources.ResourceManager.GetString("lbProductNotRecognized", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D17 RID: 3351
		// (get) Token: 0x06003015 RID: 12309 RVA: 0x0013C898 File Offset: 0x0013AA98
		public static string lbProductNumberChanged
		{
			get
			{
				return Resources.ResourceManager.GetString("lbProductNumberChanged", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D18 RID: 3352
		// (get) Token: 0x06003016 RID: 12310 RVA: 0x0013C8C0 File Offset: 0x0013AAC0
		public static string lbProductRenamed
		{
			get
			{
				return Resources.ResourceManager.GetString("lbProductRenamed", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D19 RID: 3353
		// (get) Token: 0x06003017 RID: 12311 RVA: 0x0013C8E8 File Offset: 0x0013AAE8
		public static string lbProductsMissing
		{
			get
			{
				return Resources.ResourceManager.GetString("lbProductsMissing", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D1A RID: 3354
		// (get) Token: 0x06003018 RID: 12312 RVA: 0x0013C910 File Offset: 0x0013AB10
		public static string lbSafetySwitchOpen
		{
			get
			{
				return Resources.ResourceManager.GetString("lbSafetySwitchOpen", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D1B RID: 3355
		// (get) Token: 0x06003019 RID: 12313 RVA: 0x0013C938 File Offset: 0x0013AB38
		public static string lbSensitivityChanged
		{
			get
			{
				return Resources.ResourceManager.GetString("lbSensitivityChanged", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D1C RID: 3356
		// (get) Token: 0x0600301A RID: 12314 RVA: 0x0013C960 File Offset: 0x0013AB60
		public static string lbSensorBlockedPhotogating
		{
			get
			{
				return Resources.ResourceManager.GetString("lbSensorBlockedPhotogating", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D1D RID: 3357
		// (get) Token: 0x0600301B RID: 12315 RVA: 0x0013C988 File Offset: 0x0013AB88
		public static string lbSensorBlockedRejectCheck
		{
			get
			{
				return Resources.ResourceManager.GetString("lbSensorBlockedRejectCheck", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D1E RID: 3358
		// (get) Token: 0x0600301C RID: 12316 RVA: 0x0013C9B0 File Offset: 0x0013ABB0
		public static string lbSShError
		{
			get
			{
				return Resources.ResourceManager.GetString("lbSShError", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D1F RID: 3359
		// (get) Token: 0x0600301D RID: 12317 RVA: 0x0013C9D8 File Offset: 0x0013ABD8
		public static string lbSshOvercurrent
		{
			get
			{
				return Resources.ResourceManager.GetString("lbSshOvercurrent", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D20 RID: 3360
		// (get) Token: 0x0600301E RID: 12318 RVA: 0x0013CA00 File Offset: 0x0013AC00
		public static string lbSVTCommError
		{
			get
			{
				return Resources.ResourceManager.GetString("lbSVTCommError", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D21 RID: 3361
		// (get) Token: 0x0600301F RID: 12319 RVA: 0x0013CA28 File Offset: 0x0013AC28
		public static string lbSVTFault
		{
			get
			{
				return Resources.ResourceManager.GetString("lbSVTFault", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D22 RID: 3362
		// (get) Token: 0x06003020 RID: 12320 RVA: 0x0013CA50 File Offset: 0x0013AC50
		public static string lbSystemTimeChanged
		{
			get
			{
				return Resources.ResourceManager.GetString("lbSystemTimeChanged", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D23 RID: 3363
		// (get) Token: 0x06003021 RID: 12321 RVA: 0x0013CA78 File Offset: 0x0013AC78
		public static string lbTime
		{
			get
			{
				return Resources.ResourceManager.GetString("lbTime", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D24 RID: 3364
		// (get) Token: 0x06003022 RID: 12322 RVA: 0x0013CAA0 File Offset: 0x0013ACA0
		public static string lbUserChange
		{
			get
			{
				return Resources.ResourceManager.GetString("lbUserChange", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D25 RID: 3365
		// (get) Token: 0x06003023 RID: 12323 RVA: 0x0013CAC8 File Offset: 0x0013ACC8
		public static string lbUserDeleted
		{
			get
			{
				return Resources.ResourceManager.GetString("lbUserDeleted", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D26 RID: 3366
		// (get) Token: 0x06003024 RID: 12324 RVA: 0x0013CAF0 File Offset: 0x0013ACF0
		public static string lbUserLoggedIn
		{
			get
			{
				return Resources.ResourceManager.GetString("lbUserLoggedIn", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D27 RID: 3367
		// (get) Token: 0x06003025 RID: 12325 RVA: 0x0013CB18 File Offset: 0x0013AD18
		public static string lbUserLoggedOut
		{
			get
			{
				return Resources.ResourceManager.GetString("lbUserLoggedOut", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D28 RID: 3368
		// (get) Token: 0x06003026 RID: 12326 RVA: 0x0013CB40 File Offset: 0x0013AD40
		public static string lbUserParameterChanged
		{
			get
			{
				return Resources.ResourceManager.GetString("lbUserParameterChanged", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D29 RID: 3369
		// (get) Token: 0x06003027 RID: 12327 RVA: 0x0013CB68 File Offset: 0x0013AD68
		public static string lbWAGOCommError
		{
			get
			{
				return Resources.ResourceManager.GetString("lbWAGOCommError", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D2A RID: 3370
		// (get) Token: 0x06003028 RID: 12328 RVA: 0x0013CB90 File Offset: 0x0013AD90
		public static string lbWeigher
		{
			get
			{
				return Resources.ResourceManager.GetString("lbWeigher", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D2B RID: 3371
		// (get) Token: 0x06003029 RID: 12329 RVA: 0x0013CBB8 File Offset: 0x0013ADB8
		public static string lbWeigherActivated
		{
			get
			{
				return Resources.ResourceManager.GetString("lbWeigherActivated", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D2C RID: 3372
		// (get) Token: 0x0600302A RID: 12330 RVA: 0x0013CBE0 File Offset: 0x0013ADE0
		public static string lbWeigherDeactivated
		{
			get
			{
				return Resources.ResourceManager.GetString("lbWeigherDeactivated", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D2D RID: 3373
		// (get) Token: 0x0600302B RID: 12331 RVA: 0x0013CC08 File Offset: 0x0013AE08
		public static string lbXRayCommError
		{
			get
			{
				return Resources.ResourceManager.GetString("lbXRayCommError", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D2E RID: 3374
		// (get) Token: 0x0600302C RID: 12332 RVA: 0x0013CC30 File Offset: 0x0013AE30
		public static string lbXRayError
		{
			get
			{
				return Resources.ResourceManager.GetString("lbXRayError", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D2F RID: 3375
		// (get) Token: 0x0600302D RID: 12333 RVA: 0x0013CC58 File Offset: 0x0013AE58
		public static string lbXRayTurnedOff
		{
			get
			{
				return Resources.ResourceManager.GetString("lbXRayTurnedOff", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D30 RID: 3376
		// (get) Token: 0x0600302E RID: 12334 RVA: 0x0013CC80 File Offset: 0x0013AE80
		public static string lbXRayTurnedOn
		{
			get
			{
				return Resources.ResourceManager.GetString("lbXRayTurnedOn", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D31 RID: 3377
		// (get) Token: 0x0600302F RID: 12335 RVA: 0x0013CCA8 File Offset: 0x0013AEA8
		public static string menuItemAcquisition
		{
			get
			{
				return Resources.ResourceManager.GetString("menuItemAcquisition", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D32 RID: 3378
		// (get) Token: 0x06003030 RID: 12336 RVA: 0x0013CCD0 File Offset: 0x0013AED0
		public static string menuItemAuditCheck
		{
			get
			{
				return Resources.ResourceManager.GetString("menuItemAuditCheck", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D33 RID: 3379
		// (get) Token: 0x06003031 RID: 12337 RVA: 0x0013CCF8 File Offset: 0x0013AEF8
		public static string menuItemAuditCheckStart
		{
			get
			{
				return Resources.ResourceManager.GetString("menuItemAuditCheckStart", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D34 RID: 3380
		// (get) Token: 0x06003032 RID: 12338 RVA: 0x0013CD20 File Offset: 0x0013AF20
		public static string menuItemBackup
		{
			get
			{
				return Resources.ResourceManager.GetString("menuItemBackup", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D35 RID: 3381
		// (get) Token: 0x06003033 RID: 12339 RVA: 0x0013CD48 File Offset: 0x0013AF48
		public static string menuItemBranding
		{
			get
			{
				return Resources.ResourceManager.GetString("menuItemBranding", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D36 RID: 3382
		// (get) Token: 0x06003034 RID: 12340 RVA: 0x0013CD70 File Offset: 0x0013AF70
		public static string menuItemBrowseImages
		{
			get
			{
				return Resources.ResourceManager.GetString("menuItemBrowseImages", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D37 RID: 3383
		// (get) Token: 0x06003035 RID: 12341 RVA: 0x0013CD98 File Offset: 0x0013AF98
		public static string menuItemClose
		{
			get
			{
				return Resources.ResourceManager.GetString("menuItemClose", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D38 RID: 3384
		// (get) Token: 0x06003036 RID: 12342 RVA: 0x0013CDC0 File Offset: 0x0013AFC0
		public static string menuItemComissioning
		{
			get
			{
				return Resources.ResourceManager.GetString("menuItemComissioning", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D39 RID: 3385
		// (get) Token: 0x06003037 RID: 12343 RVA: 0x0013CDE8 File Offset: 0x0013AFE8
		public static string menuItemDatalogger
		{
			get
			{
				return Resources.ResourceManager.GetString("menuItemDatalogger", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D3A RID: 3386
		// (get) Token: 0x06003038 RID: 12344 RVA: 0x0013CE10 File Offset: 0x0013B010
		public static string menuItemDocuments
		{
			get
			{
				return Resources.ResourceManager.GetString("menuItemDocuments", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D3B RID: 3387
		// (get) Token: 0x06003039 RID: 12345 RVA: 0x0013CE38 File Offset: 0x0013B038
		public static string menuItemDocumentsContent
		{
			get
			{
				return Resources.ResourceManager.GetString("menuItemDocumentsContent", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D3C RID: 3388
		// (get) Token: 0x0600303A RID: 12346 RVA: 0x0013CE60 File Offset: 0x0013B060
		public static string menuItemFactorySettings
		{
			get
			{
				return Resources.ResourceManager.GetString("menuItemFactorySettings", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D3D RID: 3389
		// (get) Token: 0x0600303B RID: 12347 RVA: 0x0013CE88 File Offset: 0x0013B088
		public static string menuItemFilter
		{
			get
			{
				return Resources.ResourceManager.GetString("menuItemFilter", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D3E RID: 3390
		// (get) Token: 0x0600303C RID: 12348 RVA: 0x0013CEB0 File Offset: 0x0013B0B0
		public static string menuItemHistogram
		{
			get
			{
				return Resources.ResourceManager.GetString("menuItemHistogram", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D3F RID: 3391
		// (get) Token: 0x0600303D RID: 12349 RVA: 0x0013CED8 File Offset: 0x0013B0D8
		public static string menuItemImageTransfer
		{
			get
			{
				return Resources.ResourceManager.GetString("menuItemImageTransfer", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D40 RID: 3392
		// (get) Token: 0x0600303E RID: 12350 RVA: 0x0013CF00 File Offset: 0x0013B100
		public static string menuItemIVP
		{
			get
			{
				return Resources.ResourceManager.GetString("menuItemIVP", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D41 RID: 3393
		// (get) Token: 0x0600303F RID: 12351 RVA: 0x0013CF28 File Offset: 0x0013B128
		public static string menuItemIVPCertificats
		{
			get
			{
				return Resources.ResourceManager.GetString("menuItemIVPCertificats", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D42 RID: 3394
		// (get) Token: 0x06003040 RID: 12352 RVA: 0x0013CF50 File Offset: 0x0013B150
		public static string menuItemIVPCertificatsContent
		{
			get
			{
				return Resources.ResourceManager.GetString("menuItemIVPCertificatsContent", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D43 RID: 3395
		// (get) Token: 0x06003041 RID: 12353 RVA: 0x0013CF78 File Offset: 0x0013B178
		public static string menuItemIVPStart
		{
			get
			{
				return Resources.ResourceManager.GetString("menuItemIVPStart", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D44 RID: 3396
		// (get) Token: 0x06003042 RID: 12354 RVA: 0x0013CFA0 File Offset: 0x0013B1A0
		public static string menuItemIVPTestsamples
		{
			get
			{
				return Resources.ResourceManager.GetString("menuItemIVPTestsamples", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D45 RID: 3397
		// (get) Token: 0x06003043 RID: 12355 RVA: 0x0013CFC8 File Offset: 0x0013B1C8
		public static string menuItemLanguage
		{
			get
			{
				return Resources.ResourceManager.GetString("menuItemLanguage", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D46 RID: 3398
		// (get) Token: 0x06003044 RID: 12356 RVA: 0x0013CFF0 File Offset: 0x0013B1F0
		public static string menuItemLiveImage
		{
			get
			{
				return Resources.ResourceManager.GetString("menuItemLiveImage", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D47 RID: 3399
		// (get) Token: 0x06003045 RID: 12357 RVA: 0x0013D018 File Offset: 0x0013B218
		public static string menuItemManageWireless
		{
			get
			{
				return Resources.ResourceManager.GetString("menuItemManageWireless", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D48 RID: 3400
		// (get) Token: 0x06003046 RID: 12358 RVA: 0x0013D040 File Offset: 0x0013B240
		public static string menuItemManual
		{
			get
			{
				return Resources.ResourceManager.GetString("menuItemManual", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D49 RID: 3401
		// (get) Token: 0x06003047 RID: 12359 RVA: 0x0013D068 File Offset: 0x0013B268
		public static string menuItemNetworkSettings
		{
			get
			{
				return Resources.ResourceManager.GetString("menuItemNetworkSettings", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D4A RID: 3402
		// (get) Token: 0x06003048 RID: 12360 RVA: 0x0013D090 File Offset: 0x0013B290
		public static string menuItemObjectCounter
		{
			get
			{
				return Resources.ResourceManager.GetString("menuItemObjectCounter", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D4B RID: 3403
		// (get) Token: 0x06003049 RID: 12361 RVA: 0x0013D0B8 File Offset: 0x0013B2B8
		public static string menuItemOverview
		{
			get
			{
				return Resources.ResourceManager.GetString("menuItemOverview", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D4C RID: 3404
		// (get) Token: 0x0600304A RID: 12362 RVA: 0x0013D0E0 File Offset: 0x0013B2E0
		public static string menuItemParameter
		{
			get
			{
				return Resources.ResourceManager.GetString("menuItemParameter", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D4D RID: 3405
		// (get) Token: 0x0600304B RID: 12363 RVA: 0x0013D108 File Offset: 0x0013B308
		public static string menuItemPlot
		{
			get
			{
				return Resources.ResourceManager.GetString("menuItemPlot", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D4E RID: 3406
		// (get) Token: 0x0600304C RID: 12364 RVA: 0x0013D130 File Offset: 0x0013B330
		public static string menuItemRemote
		{
			get
			{
				return Resources.ResourceManager.GetString("menuItemRemote", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D4F RID: 3407
		// (get) Token: 0x0600304D RID: 12365 RVA: 0x0013D158 File Offset: 0x0013B358
		public static string menuItemRestart
		{
			get
			{
				return Resources.ResourceManager.GetString("menuItemRestart", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D50 RID: 3408
		// (get) Token: 0x0600304E RID: 12366 RVA: 0x0013D180 File Offset: 0x0013B380
		public static string menuItemSettingMode
		{
			get
			{
				return Resources.ResourceManager.GetString("menuItemSettingMode", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D51 RID: 3409
		// (get) Token: 0x0600304F RID: 12367 RVA: 0x0013D1A8 File Offset: 0x0013B3A8
		public static string menuItemShowImage
		{
			get
			{
				return Resources.ResourceManager.GetString("menuItemShowImage", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D52 RID: 3410
		// (get) Token: 0x06003050 RID: 12368 RVA: 0x0013D1D0 File Offset: 0x0013B3D0
		public static string menuItemSimulate
		{
			get
			{
				return Resources.ResourceManager.GetString("menuItemSimulate", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D53 RID: 3411
		// (get) Token: 0x06003051 RID: 12369 RVA: 0x0013D1F8 File Offset: 0x0013B3F8
		public static string menuItemTestcardSim
		{
			get
			{
				return Resources.ResourceManager.GetString("menuItemTestcardSim", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D54 RID: 3412
		// (get) Token: 0x06003052 RID: 12370 RVA: 0x0013D220 File Offset: 0x0013B420
		public static string menuItemVersion
		{
			get
			{
				return Resources.ResourceManager.GetString("menuItemVersion", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D55 RID: 3413
		// (get) Token: 0x06003053 RID: 12371 RVA: 0x0013D248 File Offset: 0x0013B448
		public static string menuItemWeigher
		{
			get
			{
				return Resources.ResourceManager.GetString("menuItemWeigher", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D56 RID: 3414
		// (get) Token: 0x06003054 RID: 12372 RVA: 0x0013D270 File Offset: 0x0013B470
		public static string menuItemWired
		{
			get
			{
				return Resources.ResourceManager.GetString("menuItemWired", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D57 RID: 3415
		// (get) Token: 0x06003055 RID: 12373 RVA: 0x0013D298 File Offset: 0x0013B498
		public static string menuItemWireless
		{
			get
			{
				return Resources.ResourceManager.GetString("menuItemWireless", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D58 RID: 3416
		// (get) Token: 0x06003056 RID: 12374 RVA: 0x0013D2C0 File Offset: 0x0013B4C0
		public static string msgAbort
		{
			get
			{
				return Resources.ResourceManager.GetString("msgAbort", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D59 RID: 3417
		// (get) Token: 0x06003057 RID: 12375 RVA: 0x0013D2E8 File Offset: 0x0013B4E8
		public static string msgAlreadyLoaded
		{
			get
			{
				return Resources.ResourceManager.GetString("msgAlreadyLoaded", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D5A RID: 3418
		// (get) Token: 0x06003058 RID: 12376 RVA: 0x0013D310 File Offset: 0x0013B510
		public static string msgAlreadyLoggedIn
		{
			get
			{
				return Resources.ResourceManager.GetString("msgAlreadyLoggedIn", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D5B RID: 3419
		// (get) Token: 0x06003059 RID: 12377 RVA: 0x0013D338 File Offset: 0x0013B538
		public static string msgAppNameExisting
		{
			get
			{
				return Resources.ResourceManager.GetString("msgAppNameExisting", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D5C RID: 3420
		// (get) Token: 0x0600305A RID: 12378 RVA: 0x0013D360 File Offset: 0x0013B560
		public static string msgCommentNotPossible
		{
			get
			{
				return Resources.ResourceManager.GetString("msgCommentNotPossible", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D5D RID: 3421
		// (get) Token: 0x0600305B RID: 12379 RVA: 0x0013D388 File Offset: 0x0013B588
		public static string msgCopyAppl
		{
			get
			{
				return Resources.ResourceManager.GetString("msgCopyAppl", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D5E RID: 3422
		// (get) Token: 0x0600305C RID: 12380 RVA: 0x0013D3B0 File Offset: 0x0013B5B0
		public static string msgCopyApplError
		{
			get
			{
				return Resources.ResourceManager.GetString("msgCopyApplError", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D5F RID: 3423
		// (get) Token: 0x0600305D RID: 12381 RVA: 0x0013D3D8 File Offset: 0x0013B5D8
		public static string msgCreateAppl
		{
			get
			{
				return Resources.ResourceManager.GetString("msgCreateAppl", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D60 RID: 3424
		// (get) Token: 0x0600305E RID: 12382 RVA: 0x0013D400 File Offset: 0x0013B600
		public static string msgCreateRegion
		{
			get
			{
				return Resources.ResourceManager.GetString("msgCreateRegion", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D61 RID: 3425
		// (get) Token: 0x0600305F RID: 12383 RVA: 0x0013D428 File Offset: 0x0013B628
		public static string msgCurrentApplicationDeleteForbidden
		{
			get
			{
				return Resources.ResourceManager.GetString("msgCurrentApplicationDeleteForbidden", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D62 RID: 3426
		// (get) Token: 0x06003060 RID: 12384 RVA: 0x0013D450 File Offset: 0x0013B650
		public static string msgCurrentUserDeleteForbidden
		{
			get
			{
				return Resources.ResourceManager.GetString("msgCurrentUserDeleteForbidden", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D63 RID: 3427
		// (get) Token: 0x06003061 RID: 12385 RVA: 0x0013D478 File Offset: 0x0013B678
		public static string msgCurrentWeight
		{
			get
			{
				return Resources.ResourceManager.GetString("msgCurrentWeight", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D64 RID: 3428
		// (get) Token: 0x06003062 RID: 12386 RVA: 0x0013D4A0 File Offset: 0x0013B6A0
		public static string msgDeleteApplError
		{
			get
			{
				return Resources.ResourceManager.GetString("msgDeleteApplError", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D65 RID: 3429
		// (get) Token: 0x06003063 RID: 12387 RVA: 0x0013D4C8 File Offset: 0x0013B6C8
		public static string msgDeleteUserError
		{
			get
			{
				return Resources.ResourceManager.GetString("msgDeleteUserError", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D66 RID: 3430
		// (get) Token: 0x06003064 RID: 12388 RVA: 0x0013D4F0 File Offset: 0x0013B6F0
		public static string msgEdgeFilterDisabled
		{
			get
			{
				return Resources.ResourceManager.GetString("msgEdgeFilterDisabled", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D67 RID: 3431
		// (get) Token: 0x06003065 RID: 12389 RVA: 0x0013D518 File Offset: 0x0013B718
		public static string msgEnableCounterFailed
		{
			get
			{
				return Resources.ResourceManager.GetString("msgEnableCounterFailed", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D68 RID: 3432
		// (get) Token: 0x06003066 RID: 12390 RVA: 0x0013D540 File Offset: 0x0013B740
		public static string msgEnableEdgeFilterFailed
		{
			get
			{
				return Resources.ResourceManager.GetString("msgEnableEdgeFilterFailed", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D69 RID: 3433
		// (get) Token: 0x06003067 RID: 12391 RVA: 0x0013D568 File Offset: 0x0013B768
		public static string msgEnableFilterFailed
		{
			get
			{
				return Resources.ResourceManager.GetString("msgEnableFilterFailed", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D6A RID: 3434
		// (get) Token: 0x06003068 RID: 12392 RVA: 0x0013D590 File Offset: 0x0013B790
		public static string msgEnableSizeFilterFailed
		{
			get
			{
				return Resources.ResourceManager.GetString("msgEnableSizeFilterFailed", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D6B RID: 3435
		// (get) Token: 0x06003069 RID: 12393 RVA: 0x0013D5B8 File Offset: 0x0013B7B8
		public static string msgEnableWeightFailed
		{
			get
			{
				return Resources.ResourceManager.GetString("msgEnableWeightFailed", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D6C RID: 3436
		// (get) Token: 0x0600306A RID: 12394 RVA: 0x0013D5E0 File Offset: 0x0013B7E0
		public static string msgEnterPassword
		{
			get
			{
				return Resources.ResourceManager.GetString("msgEnterPassword", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D6D RID: 3437
		// (get) Token: 0x0600306B RID: 12395 RVA: 0x0013D608 File Offset: 0x0013B808
		public static string msgErrorComImgServer
		{
			get
			{
				return Resources.ResourceManager.GetString("msgErrorComImgServer", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D6E RID: 3438
		// (get) Token: 0x0600306C RID: 12396 RVA: 0x0013D630 File Offset: 0x0013B830
		public static string msgErrorComParamServer
		{
			get
			{
				return Resources.ResourceManager.GetString("msgErrorComParamServer", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D6F RID: 3439
		// (get) Token: 0x0600306D RID: 12397 RVA: 0x0013D658 File Offset: 0x0013B858
		public static string msgFileAlreadyExistent
		{
			get
			{
				return Resources.ResourceManager.GetString("msgFileAlreadyExistent", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D70 RID: 3440
		// (get) Token: 0x0600306E RID: 12398 RVA: 0x0013D680 File Offset: 0x0013B880
		public static string msgFilterDisabled
		{
			get
			{
				return Resources.ResourceManager.GetString("msgFilterDisabled", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D71 RID: 3441
		// (get) Token: 0x0600306F RID: 12399 RVA: 0x0013D6A8 File Offset: 0x0013B8A8
		public static string msgForbidden
		{
			get
			{
				return Resources.ResourceManager.GetString("msgForbidden", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D72 RID: 3442
		// (get) Token: 0x06003070 RID: 12400 RVA: 0x0013D6D0 File Offset: 0x0013B8D0
		public static string msgGettingErased
		{
			get
			{
				return Resources.ResourceManager.GetString("msgGettingErased", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D73 RID: 3443
		// (get) Token: 0x06003071 RID: 12401 RVA: 0x0013D6F8 File Offset: 0x0013B8F8
		public static string msgGettingLoaded
		{
			get
			{
				return Resources.ResourceManager.GetString("msgGettingLoaded", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D74 RID: 3444
		// (get) Token: 0x06003072 RID: 12402 RVA: 0x0013D720 File Offset: 0x0013B920
		public static string msgICMDisabled
		{
			get
			{
				return Resources.ResourceManager.GetString("msgICMDisabled", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D75 RID: 3445
		// (get) Token: 0x06003073 RID: 12403 RVA: 0x0013D748 File Offset: 0x0013B948
		public static string msgIDAlreadyExistent
		{
			get
			{
				return Resources.ResourceManager.GetString("msgIDAlreadyExistent", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D76 RID: 3446
		// (get) Token: 0x06003074 RID: 12404 RVA: 0x0013D770 File Offset: 0x0013B970
		public static string msgInsertAppName
		{
			get
			{
				return Resources.ResourceManager.GetString("msgInsertAppName", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D77 RID: 3447
		// (get) Token: 0x06003075 RID: 12405 RVA: 0x0013D798 File Offset: 0x0013B998
		public static string msgInsertPasswordFirst
		{
			get
			{
				return Resources.ResourceManager.GetString("msgInsertPasswordFirst", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D78 RID: 3448
		// (get) Token: 0x06003076 RID: 12406 RVA: 0x0013D7C0 File Offset: 0x0013B9C0
		public static string msgLampCurrentHigh
		{
			get
			{
				return Resources.ResourceManager.GetString("msgLampCurrentHigh", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D79 RID: 3449
		// (get) Token: 0x06003077 RID: 12407 RVA: 0x0013D7E8 File Offset: 0x0013B9E8
		public static string msgLampCurrentLow
		{
			get
			{
				return Resources.ResourceManager.GetString("msgLampCurrentLow", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D7A RID: 3450
		// (get) Token: 0x06003078 RID: 12408 RVA: 0x0013D810 File Offset: 0x0013BA10
		public static string msgLoadingApplication
		{
			get
			{
				return Resources.ResourceManager.GetString("msgLoadingApplication", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D7B RID: 3451
		// (get) Token: 0x06003079 RID: 12409 RVA: 0x0013D838 File Offset: 0x0013BA38
		public static string msgLogDataAvailable
		{
			get
			{
				return Resources.ResourceManager.GetString("msgLogDataAvailable", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D7C RID: 3452
		// (get) Token: 0x0600307A RID: 12410 RVA: 0x0013D860 File Offset: 0x0013BA60
		public static string msgLogin
		{
			get
			{
				return Resources.ResourceManager.GetString("msgLogin", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D7D RID: 3453
		// (get) Token: 0x0600307B RID: 12411 RVA: 0x0013D888 File Offset: 0x0013BA88
		public static string msgMaxProductLength
		{
			get
			{
				return Resources.ResourceManager.GetString("msgMaxProductLength", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D7E RID: 3454
		// (get) Token: 0x0600307C RID: 12412 RVA: 0x0013D8B0 File Offset: 0x0013BAB0
		public static string msgNewApplError
		{
			get
			{
				return Resources.ResourceManager.GetString("msgNewApplError", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D7F RID: 3455
		// (get) Token: 0x0600307D RID: 12413 RVA: 0x0013D8D8 File Offset: 0x0013BAD8
		public static string msgNoMaterialCurveAvailable
		{
			get
			{
				return Resources.ResourceManager.GetString("msgNoMaterialCurveAvailable", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D80 RID: 3456
		// (get) Token: 0x0600307E RID: 12414 RVA: 0x0013D900 File Offset: 0x0013BB00
		public static string msgNumberExists
		{
			get
			{
				return Resources.ResourceManager.GetString("msgNumberExists", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D81 RID: 3457
		// (get) Token: 0x0600307F RID: 12415 RVA: 0x0013D928 File Offset: 0x0013BB28
		public static string msgObjectCount
		{
			get
			{
				return Resources.ResourceManager.GetString("msgObjectCount", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D82 RID: 3458
		// (get) Token: 0x06003080 RID: 12416 RVA: 0x0013D950 File Offset: 0x0013BB50
		public static string msgObjectCounterDisabled
		{
			get
			{
				return Resources.ResourceManager.GetString("msgObjectCounterDisabled", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D83 RID: 3459
		// (get) Token: 0x06003081 RID: 12417 RVA: 0x0013D978 File Offset: 0x0013BB78
		public static string msgObjectCounterEnabled
		{
			get
			{
				return Resources.ResourceManager.GetString("msgObjectCounterEnabled", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D84 RID: 3460
		// (get) Token: 0x06003082 RID: 12418 RVA: 0x0013D9A0 File Offset: 0x0013BBA0
		public static string msgPasswordsDontMatch
		{
			get
			{
				return Resources.ResourceManager.GetString("msgPasswordsDontMatch", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D85 RID: 3461
		// (get) Token: 0x06003083 RID: 12419 RVA: 0x0013D9C8 File Offset: 0x0013BBC8
		public static string msgPathAccess
		{
			get
			{
				return Resources.ResourceManager.GetString("msgPathAccess", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D86 RID: 3462
		// (get) Token: 0x06003084 RID: 12420 RVA: 0x0013D9F0 File Offset: 0x0013BBF0
		public static string msgProductsErased
		{
			get
			{
				return Resources.ResourceManager.GetString("msgProductsErased", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D87 RID: 3463
		// (get) Token: 0x06003085 RID: 12421 RVA: 0x0013DA18 File Offset: 0x0013BC18
		public static string msgReject
		{
			get
			{
				return Resources.ResourceManager.GetString("msgReject", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D88 RID: 3464
		// (get) Token: 0x06003086 RID: 12422 RVA: 0x0013DA40 File Offset: 0x0013BC40
		public static string msgReloadNotPossible
		{
			get
			{
				return Resources.ResourceManager.GetString("msgReloadNotPossible", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D89 RID: 3465
		// (get) Token: 0x06003087 RID: 12423 RVA: 0x0013DA68 File Offset: 0x0013BC68
		public static string msgRenameAndSave
		{
			get
			{
				return Resources.ResourceManager.GetString("msgRenameAndSave", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D8A RID: 3466
		// (get) Token: 0x06003088 RID: 12424 RVA: 0x0013DA90 File Offset: 0x0013BC90
		public static string msgRenameAppl
		{
			get
			{
				return Resources.ResourceManager.GetString("msgRenameAppl", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D8B RID: 3467
		// (get) Token: 0x06003089 RID: 12425 RVA: 0x0013DAB8 File Offset: 0x0013BCB8
		public static string msgRenameApplError
		{
			get
			{
				return Resources.ResourceManager.GetString("msgRenameApplError", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D8C RID: 3468
		// (get) Token: 0x0600308A RID: 12426 RVA: 0x0013DAE0 File Offset: 0x0013BCE0
		public static string msgRenameApplNumber
		{
			get
			{
				return Resources.ResourceManager.GetString("msgRenameApplNumber", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D8D RID: 3469
		// (get) Token: 0x0600308B RID: 12427 RVA: 0x0013DB08 File Offset: 0x0013BD08
		public static string msgRenameRegion
		{
			get
			{
				return Resources.ResourceManager.GetString("msgRenameRegion", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D8E RID: 3470
		// (get) Token: 0x0600308C RID: 12428 RVA: 0x0013DB30 File Offset: 0x0013BD30
		public static string msgRepeatPassword
		{
			get
			{
				return Resources.ResourceManager.GetString("msgRepeatPassword", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D8F RID: 3471
		// (get) Token: 0x0600308D RID: 12429 RVA: 0x0013DB58 File Offset: 0x0013BD58
		public static string msgResetCounters
		{
			get
			{
				return Resources.ResourceManager.GetString("msgResetCounters", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D90 RID: 3472
		// (get) Token: 0x0600308E RID: 12430 RVA: 0x0013DB80 File Offset: 0x0013BD80
		public static string msgResetRejectCounter
		{
			get
			{
				return Resources.ResourceManager.GetString("msgResetRejectCounter", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D91 RID: 3473
		// (get) Token: 0x0600308F RID: 12431 RVA: 0x0013DBA8 File Offset: 0x0013BDA8
		public static string msgResetThroughputCounter
		{
			get
			{
				return Resources.ResourceManager.GetString("msgResetThroughputCounter", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D92 RID: 3474
		// (get) Token: 0x06003090 RID: 12432 RVA: 0x0013DBD0 File Offset: 0x0013BDD0
		public static string msgRestartingImagePc
		{
			get
			{
				return Resources.ResourceManager.GetString("msgRestartingImagePc", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D93 RID: 3475
		// (get) Token: 0x06003091 RID: 12433 RVA: 0x0013DBF8 File Offset: 0x0013BDF8
		public static string msgSaveDualMaterialCurveDB
		{
			get
			{
				return Resources.ResourceManager.GetString("msgSaveDualMaterialCurveDB", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D94 RID: 3476
		// (get) Token: 0x06003092 RID: 12434 RVA: 0x0013DC20 File Offset: 0x0013BE20
		public static string msgSaveDualMaterialCurvePNG
		{
			get
			{
				return Resources.ResourceManager.GetString("msgSaveDualMaterialCurvePNG", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D95 RID: 3477
		// (get) Token: 0x06003093 RID: 12435 RVA: 0x0013DC48 File Offset: 0x0013BE48
		public static string msgShowHistory
		{
			get
			{
				return Resources.ResourceManager.GetString("msgShowHistory", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D96 RID: 3478
		// (get) Token: 0x06003094 RID: 12436 RVA: 0x0013DC70 File Offset: 0x0013BE70
		public static string msgSignOut
		{
			get
			{
				return Resources.ResourceManager.GetString("msgSignOut", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D97 RID: 3479
		// (get) Token: 0x06003095 RID: 12437 RVA: 0x0013DC98 File Offset: 0x0013BE98
		public static string msgSizeFilterDisabled
		{
			get
			{
				return Resources.ResourceManager.GetString("msgSizeFilterDisabled", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D98 RID: 3480
		// (get) Token: 0x06003096 RID: 12438 RVA: 0x0013DCC0 File Offset: 0x0013BEC0
		public static string msgSoftwareFeatureDisabled
		{
			get
			{
				return Resources.ResourceManager.GetString("msgSoftwareFeatureDisabled", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D99 RID: 3481
		// (get) Token: 0x06003097 RID: 12439 RVA: 0x0013DCE8 File Offset: 0x0013BEE8
		public static string msgStartVibro
		{
			get
			{
				return Resources.ResourceManager.GetString("msgStartVibro", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D9A RID: 3482
		// (get) Token: 0x06003098 RID: 12440 RVA: 0x0013DD10 File Offset: 0x0013BF10
		public static string msgStopVibro
		{
			get
			{
				return Resources.ResourceManager.GetString("msgStopVibro", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D9B RID: 3483
		// (get) Token: 0x06003099 RID: 12441 RVA: 0x0013DD38 File Offset: 0x0013BF38
		public static string msgTempTooHigh
		{
			get
			{
				return Resources.ResourceManager.GetString("msgTempTooHigh", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D9C RID: 3484
		// (get) Token: 0x0600309A RID: 12442 RVA: 0x0013DD60 File Offset: 0x0013BF60
		public static string msgThinkXODisableInference
		{
			get
			{
				return Resources.ResourceManager.GetString("msgThinkXODisableInference", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D9D RID: 3485
		// (get) Token: 0x0600309B RID: 12443 RVA: 0x0013DD88 File Offset: 0x0013BF88
		public static string msgThinkXoEnabled
		{
			get
			{
				return Resources.ResourceManager.GetString("msgThinkXoEnabled", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D9E RID: 3486
		// (get) Token: 0x0600309C RID: 12444 RVA: 0x0013DDB0 File Offset: 0x0013BFB0
		public static string msgThinkXOEnableInference
		{
			get
			{
				return Resources.ResourceManager.GetString("msgThinkXOEnableInference", Resources.resourceCulture);
			}
		}

		// Token: 0x17000D9F RID: 3487
		// (get) Token: 0x0600309D RID: 12445 RVA: 0x0013DDD8 File Offset: 0x0013BFD8
		public static string msgThinkXOEnableTTA
		{
			get
			{
				return Resources.ResourceManager.GetString("msgThinkXOEnableTTA", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DA0 RID: 3488
		// (get) Token: 0x0600309E RID: 12446 RVA: 0x0013DE00 File Offset: 0x0013C000
		public static string msgThinkXoNoModel
		{
			get
			{
				return Resources.ResourceManager.GetString("msgThinkXoNoModel", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DA1 RID: 3489
		// (get) Token: 0x0600309F RID: 12447 RVA: 0x0013DE28 File Offset: 0x0013C028
		public static string msgThroughput
		{
			get
			{
				return Resources.ResourceManager.GetString("msgThroughput", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DA2 RID: 3490
		// (get) Token: 0x060030A0 RID: 12448 RVA: 0x0013DE50 File Offset: 0x0013C050
		public static string msgUser
		{
			get
			{
				return Resources.ResourceManager.GetString("msgUser", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DA3 RID: 3491
		// (get) Token: 0x060030A1 RID: 12449 RVA: 0x0013DE78 File Offset: 0x0013C078
		public static string msgUserChangeForbidden
		{
			get
			{
				return Resources.ResourceManager.GetString("msgUserChangeForbidden", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DA4 RID: 3492
		// (get) Token: 0x060030A2 RID: 12450 RVA: 0x0013DEA0 File Offset: 0x0013C0A0
		public static string msgUserDeleteForbidden
		{
			get
			{
				return Resources.ResourceManager.GetString("msgUserDeleteForbidden", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DA5 RID: 3493
		// (get) Token: 0x060030A3 RID: 12451 RVA: 0x0013DEC8 File Offset: 0x0013C0C8
		public static string msgUserFieldEntriesMissing
		{
			get
			{
				return Resources.ResourceManager.GetString("msgUserFieldEntriesMissing", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DA6 RID: 3494
		// (get) Token: 0x060030A4 RID: 12452 RVA: 0x0013DEF0 File Offset: 0x0013C0F0
		public static string msgValveActivity
		{
			get
			{
				return Resources.ResourceManager.GetString("msgValveActivity", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DA7 RID: 3495
		// (get) Token: 0x060030A5 RID: 12453 RVA: 0x0013DF18 File Offset: 0x0013C118
		public static string msgVibroInManualMode
		{
			get
			{
				return Resources.ResourceManager.GetString("msgVibroInManualMode", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DA8 RID: 3496
		// (get) Token: 0x060030A6 RID: 12454 RVA: 0x0013DF40 File Offset: 0x0013C140
		public static string msgVoltageTooHigh
		{
			get
			{
				return Resources.ResourceManager.GetString("msgVoltageTooHigh", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DA9 RID: 3497
		// (get) Token: 0x060030A7 RID: 12455 RVA: 0x0013DF68 File Offset: 0x0013C168
		public static string msgVoltageTooLow
		{
			get
			{
				return Resources.ResourceManager.GetString("msgVoltageTooLow", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DAA RID: 3498
		// (get) Token: 0x060030A8 RID: 12456 RVA: 0x0013DF90 File Offset: 0x0013C190
		public static string msgWagoOutOfDate
		{
			get
			{
				return Resources.ResourceManager.GetString("msgWagoOutOfDate", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DAB RID: 3499
		// (get) Token: 0x060030A9 RID: 12457 RVA: 0x0013DFB8 File Offset: 0x0013C1B8
		public static string msgWagoWrongVersion
		{
			get
			{
				return Resources.ResourceManager.GetString("msgWagoWrongVersion", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DAC RID: 3500
		// (get) Token: 0x060030AA RID: 12458 RVA: 0x0013DFE0 File Offset: 0x0013C1E0
		public static string msgWeightDisabled
		{
			get
			{
				return Resources.ResourceManager.GetString("msgWeightDisabled", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DAD RID: 3501
		// (get) Token: 0x060030AB RID: 12459 RVA: 0x0013E008 File Offset: 0x0013C208
		public static string msgWrongPassword
		{
			get
			{
				return Resources.ResourceManager.GetString("msgWrongPassword", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DAE RID: 3502
		// (get) Token: 0x060030AC RID: 12460 RVA: 0x0013E030 File Offset: 0x0013C230
		public static string msgWrongWeight
		{
			get
			{
				return Resources.ResourceManager.GetString("msgWrongWeight", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DAF RID: 3503
		// (get) Token: 0x060030AD RID: 12461 RVA: 0x0013E058 File Offset: 0x0013C258
		public static string overwrite_rfid_login_for_selected_user
		{
			get
			{
				return Resources.ResourceManager.GetString("overwrite_rfid_login_for_selected_user", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DB0 RID: 3504
		// (get) Token: 0x060030AE RID: 12462 RVA: 0x0013E080 File Offset: 0x0013C280
		public static string parConveyorDoor
		{
			get
			{
				return Resources.ResourceManager.GetString("parConveyorDoor", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DB1 RID: 3505
		// (get) Token: 0x060030AF RID: 12463 RVA: 0x0013E0A8 File Offset: 0x0013C2A8
		public static string parItemBinFull
		{
			get
			{
				return Resources.ResourceManager.GetString("parItemBinFull", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DB2 RID: 3506
		// (get) Token: 0x060030B0 RID: 12464 RVA: 0x0013E0D0 File Offset: 0x0013C2D0
		public static string parItemBinFull0
		{
			get
			{
				return Resources.ResourceManager.GetString("parItemBinFull0", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DB3 RID: 3507
		// (get) Token: 0x060030B1 RID: 12465 RVA: 0x0013E0F8 File Offset: 0x0013C2F8
		public static string parItemBinFull1
		{
			get
			{
				return Resources.ResourceManager.GetString("parItemBinFull1", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DB4 RID: 3508
		// (get) Token: 0x060030B2 RID: 12466 RVA: 0x0013E120 File Offset: 0x0013C320
		public static string parItemBinFull2
		{
			get
			{
				return Resources.ResourceManager.GetString("parItemBinFull2", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DB5 RID: 3509
		// (get) Token: 0x060030B3 RID: 12467 RVA: 0x0013E148 File Offset: 0x0013C348
		public static string parItemBinFull3
		{
			get
			{
				return Resources.ResourceManager.GetString("parItemBinFull3", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DB6 RID: 3510
		// (get) Token: 0x060030B4 RID: 12468 RVA: 0x0013E170 File Offset: 0x0013C370
		public static string parItemCleaningDoorBackLH
		{
			get
			{
				return Resources.ResourceManager.GetString("parItemCleaningDoorBackLH", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DB7 RID: 3511
		// (get) Token: 0x060030B5 RID: 12469 RVA: 0x0013E198 File Offset: 0x0013C398
		public static string parItemCleaningDoorBackRH
		{
			get
			{
				return Resources.ResourceManager.GetString("parItemCleaningDoorBackRH", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DB8 RID: 3512
		// (get) Token: 0x060030B6 RID: 12470 RVA: 0x0013E1C0 File Offset: 0x0013C3C0
		public static string parItemCleaningDoorFront
		{
			get
			{
				return Resources.ResourceManager.GetString("parItemCleaningDoorFront", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DB9 RID: 3513
		// (get) Token: 0x060030B7 RID: 12471 RVA: 0x0013E1E8 File Offset: 0x0013C3E8
		public static string parItemCleaningDoorFrontLH
		{
			get
			{
				return Resources.ResourceManager.GetString("parItemCleaningDoorFrontLH", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DBA RID: 3514
		// (get) Token: 0x060030B8 RID: 12472 RVA: 0x0013E210 File Offset: 0x0013C410
		public static string parItemCleaningDoorFrontRH
		{
			get
			{
				return Resources.ResourceManager.GetString("parItemCleaningDoorFrontRH", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DBB RID: 3515
		// (get) Token: 0x060030B9 RID: 12473 RVA: 0x0013E238 File Offset: 0x0013C438
		public static string parItemCleaningDoorLeft
		{
			get
			{
				return Resources.ResourceManager.GetString("parItemCleaningDoorLeft", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DBC RID: 3516
		// (get) Token: 0x060030BA RID: 12474 RVA: 0x0013E260 File Offset: 0x0013C460
		public static string parItemCleaningDoorLH
		{
			get
			{
				return Resources.ResourceManager.GetString("parItemCleaningDoorLH", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DBD RID: 3517
		// (get) Token: 0x060030BB RID: 12475 RVA: 0x0013E288 File Offset: 0x0013C488
		public static string parItemCleaningDoorRH
		{
			get
			{
				return Resources.ResourceManager.GetString("parItemCleaningDoorRH", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DBE RID: 3518
		// (get) Token: 0x060030BC RID: 12476 RVA: 0x0013E2B0 File Offset: 0x0013C4B0
		public static string parItemCleaningDoorRight
		{
			get
			{
				return Resources.ResourceManager.GetString("parItemCleaningDoorRight", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DBF RID: 3519
		// (get) Token: 0x060030BD RID: 12477 RVA: 0x0013E2D8 File Offset: 0x0013C4D8
		public static string parItemConveyorBackNotDocked
		{
			get
			{
				return Resources.ResourceManager.GetString("parItemConveyorBackNotDocked", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DC0 RID: 3520
		// (get) Token: 0x060030BE RID: 12478 RVA: 0x0013E300 File Offset: 0x0013C500
		public static string parItemDetector
		{
			get
			{
				return Resources.ResourceManager.GetString("parItemDetector", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DC1 RID: 3521
		// (get) Token: 0x060030BF RID: 12479 RVA: 0x0013E328 File Offset: 0x0013C528
		public static string parItemEmergencyStopPressed
		{
			get
			{
				return Resources.ResourceManager.GetString("parItemEmergencyStopPressed", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DC2 RID: 3522
		// (get) Token: 0x060030C0 RID: 12480 RVA: 0x0013E350 File Offset: 0x0013C550
		public static string parItemErrorCooling
		{
			get
			{
				return Resources.ResourceManager.GetString("parItemErrorCooling", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DC3 RID: 3523
		// (get) Token: 0x060030C1 RID: 12481 RVA: 0x0013E378 File Offset: 0x0013C578
		public static string parItemErrorEngine
		{
			get
			{
				return Resources.ResourceManager.GetString("parItemErrorEngine", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DC4 RID: 3524
		// (get) Token: 0x060030C2 RID: 12482 RVA: 0x0013E3A0 File Offset: 0x0013C5A0
		public static string parItemExternalEmergencyStopPressed
		{
			get
			{
				return Resources.ResourceManager.GetString("parItemExternalEmergencyStopPressed", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DC5 RID: 3525
		// (get) Token: 0x060030C3 RID: 12483 RVA: 0x0013E3C8 File Offset: 0x0013C5C8
		public static string parItemHopperGoodProduct
		{
			get
			{
				return Resources.ResourceManager.GetString("parItemHopperGoodProduct", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DC6 RID: 3526
		// (get) Token: 0x060030C4 RID: 12484 RVA: 0x0013E3F0 File Offset: 0x0013C5F0
		public static string parItemHopperInfeed
		{
			get
			{
				return Resources.ResourceManager.GetString("parItemHopperInfeed", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DC7 RID: 3527
		// (get) Token: 0x060030C5 RID: 12485 RVA: 0x0013E418 File Offset: 0x0013C618
		public static string parItemHopperWasteProduct
		{
			get
			{
				return Resources.ResourceManager.GetString("parItemHopperWasteProduct", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DC8 RID: 3528
		// (get) Token: 0x060030C6 RID: 12486 RVA: 0x0013E440 File Offset: 0x0013C640
		public static string parItemLampMonitor
		{
			get
			{
				return Resources.ResourceManager.GetString("parItemLampMonitor", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DC9 RID: 3529
		// (get) Token: 0x060030C7 RID: 12487 RVA: 0x0013E468 File Offset: 0x0013C668
		public static string parItemLightBarrier0
		{
			get
			{
				return Resources.ResourceManager.GetString("parItemLightBarrier0", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DCA RID: 3530
		// (get) Token: 0x060030C8 RID: 12488 RVA: 0x0013E490 File Offset: 0x0013C690
		public static string parItemLightBarrier1
		{
			get
			{
				return Resources.ResourceManager.GetString("parItemLightBarrier1", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DCB RID: 3531
		// (get) Token: 0x060030C9 RID: 12489 RVA: 0x0013E4B8 File Offset: 0x0013C6B8
		public static string parItemLightBarrier2
		{
			get
			{
				return Resources.ResourceManager.GetString("parItemLightBarrier2", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DCC RID: 3532
		// (get) Token: 0x060030CA RID: 12490 RVA: 0x0013E4E0 File Offset: 0x0013C6E0
		public static string parItemLightBarrier3
		{
			get
			{
				return Resources.ResourceManager.GetString("parItemLightBarrier3", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DCD RID: 3533
		// (get) Token: 0x060030CB RID: 12491 RVA: 0x0013E508 File Offset: 0x0013C708
		public static string parItemLowAirPressure
		{
			get
			{
				return Resources.ResourceManager.GetString("parItemLowAirPressure", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DCE RID: 3534
		// (get) Token: 0x060030CC RID: 12492 RVA: 0x0013E530 File Offset: 0x0013C730
		public static string parItemMachineEnableExtern
		{
			get
			{
				return Resources.ResourceManager.GetString("parItemMachineEnableExtern", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DCF RID: 3535
		// (get) Token: 0x060030CD RID: 12493 RVA: 0x0013E558 File Offset: 0x0013C758
		public static string parItemPanelLeft
		{
			get
			{
				return Resources.ResourceManager.GetString("parItemPanelLeft", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DD0 RID: 3536
		// (get) Token: 0x060030CE RID: 12494 RVA: 0x0013E580 File Offset: 0x0013C780
		public static string parItemPanelRight
		{
			get
			{
				return Resources.ResourceManager.GetString("parItemPanelRight", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DD1 RID: 3537
		// (get) Token: 0x060030CF RID: 12495 RVA: 0x0013E5A8 File Offset: 0x0013C7A8
		public static string parItemPanelTop
		{
			get
			{
				return Resources.ResourceManager.GetString("parItemPanelTop", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DD2 RID: 3538
		// (get) Token: 0x060030D0 RID: 12496 RVA: 0x0013E5D0 File Offset: 0x0013C7D0
		public static string parItemPanelTopLH
		{
			get
			{
				return Resources.ResourceManager.GetString("parItemPanelTopLH", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DD3 RID: 3539
		// (get) Token: 0x060030D1 RID: 12497 RVA: 0x0013E5F8 File Offset: 0x0013C7F8
		public static string parItemPanelTopRH
		{
			get
			{
				return Resources.ResourceManager.GetString("parItemPanelTopRH", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DD4 RID: 3540
		// (get) Token: 0x060030D2 RID: 12498 RVA: 0x0013E620 File Offset: 0x0013C820
		public static string parItemRejecterUnitCoverFront
		{
			get
			{
				return Resources.ResourceManager.GetString("parItemRejecterUnitCoverFront", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DD5 RID: 3541
		// (get) Token: 0x060030D3 RID: 12499 RVA: 0x0013E648 File Offset: 0x0013C848
		public static string parItemRejecterUnitCoverTop
		{
			get
			{
				return Resources.ResourceManager.GetString("parItemRejecterUnitCoverTop", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DD6 RID: 3542
		// (get) Token: 0x060030D4 RID: 12500 RVA: 0x0013E670 File Offset: 0x0013C870
		public static string parItemRejecterUnitNotAvailable
		{
			get
			{
				return Resources.ResourceManager.GetString("parItemRejecterUnitNotAvailable", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DD7 RID: 3543
		// (get) Token: 0x060030D5 RID: 12501 RVA: 0x0013E698 File Offset: 0x0013C898
		public static string parItemServiceDoor
		{
			get
			{
				return Resources.ResourceManager.GetString("parItemServiceDoor", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DD8 RID: 3544
		// (get) Token: 0x060030D6 RID: 12502 RVA: 0x0013E6C0 File Offset: 0x0013C8C0
		public static string parItemServiceDoorRejectionBack
		{
			get
			{
				return Resources.ResourceManager.GetString("parItemServiceDoorRejectionBack", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DD9 RID: 3545
		// (get) Token: 0x060030D7 RID: 12503 RVA: 0x0013E6E8 File Offset: 0x0013C8E8
		public static string parItemServiceDoorRejectionFront
		{
			get
			{
				return Resources.ResourceManager.GetString("parItemServiceDoorRejectionFront", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DDA RID: 3546
		// (get) Token: 0x060030D8 RID: 12504 RVA: 0x0013E710 File Offset: 0x0013C910
		public static string parItemServiceDoorRejectionMiddle
		{
			get
			{
				return Resources.ResourceManager.GetString("parItemServiceDoorRejectionMiddle", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DDB RID: 3547
		// (get) Token: 0x060030D9 RID: 12505 RVA: 0x0013E738 File Offset: 0x0013C938
		public static string parItemShieldBottom
		{
			get
			{
				return Resources.ResourceManager.GetString("parItemShieldBottom", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DDC RID: 3548
		// (get) Token: 0x060030DA RID: 12506 RVA: 0x0013E760 File Offset: 0x0013C960
		public static string parItemShieldLHBotBack
		{
			get
			{
				return Resources.ResourceManager.GetString("parItemShieldLHBotBack", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DDD RID: 3549
		// (get) Token: 0x060030DB RID: 12507 RVA: 0x0013E788 File Offset: 0x0013C988
		public static string parItemShieldLHBotFront
		{
			get
			{
				return Resources.ResourceManager.GetString("parItemShieldLHBotFront", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DDE RID: 3550
		// (get) Token: 0x060030DC RID: 12508 RVA: 0x0013E7B0 File Offset: 0x0013C9B0
		public static string parItemShieldLHTopBack
		{
			get
			{
				return Resources.ResourceManager.GetString("parItemShieldLHTopBack", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DDF RID: 3551
		// (get) Token: 0x060030DD RID: 12509 RVA: 0x0013E7D8 File Offset: 0x0013C9D8
		public static string parItemShieldLHTopFront
		{
			get
			{
				return Resources.ResourceManager.GetString("parItemShieldLHTopFront", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DE0 RID: 3552
		// (get) Token: 0x060030DE RID: 12510 RVA: 0x0013E800 File Offset: 0x0013CA00
		public static string parItemShieldRHBotBack
		{
			get
			{
				return Resources.ResourceManager.GetString("parItemShieldRHBotBack", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DE1 RID: 3553
		// (get) Token: 0x060030DF RID: 12511 RVA: 0x0013E828 File Offset: 0x0013CA28
		public static string parItemShieldRHBotFront
		{
			get
			{
				return Resources.ResourceManager.GetString("parItemShieldRHBotFront", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DE2 RID: 3554
		// (get) Token: 0x060030E0 RID: 12512 RVA: 0x0013E850 File Offset: 0x0013CA50
		public static string parItemShieldRHTopBack
		{
			get
			{
				return Resources.ResourceManager.GetString("parItemShieldRHTopBack", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DE3 RID: 3555
		// (get) Token: 0x060030E1 RID: 12513 RVA: 0x0013E878 File Offset: 0x0013CA78
		public static string parItemShieldRHTopFront
		{
			get
			{
				return Resources.ResourceManager.GetString("parItemShieldRHTopFront", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DE4 RID: 3556
		// (get) Token: 0x060030E2 RID: 12514 RVA: 0x0013E8A0 File Offset: 0x0013CAA0
		public static string parItemSlidingPlate
		{
			get
			{
				return Resources.ResourceManager.GetString("parItemSlidingPlate", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DE5 RID: 3557
		// (get) Token: 0x060030E3 RID: 12515 RVA: 0x0013E8C8 File Offset: 0x0013CAC8
		public static string parItemTempSensor0
		{
			get
			{
				return Resources.ResourceManager.GetString("parItemTempSensor0", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DE6 RID: 3558
		// (get) Token: 0x060030E4 RID: 12516 RVA: 0x0013E8F0 File Offset: 0x0013CAF0
		public static string parItemTempSensor1
		{
			get
			{
				return Resources.ResourceManager.GetString("parItemTempSensor1", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DE7 RID: 3559
		// (get) Token: 0x060030E5 RID: 12517 RVA: 0x0013E918 File Offset: 0x0013CB18
		public static string parItemVoltage12V
		{
			get
			{
				return Resources.ResourceManager.GetString("parItemVoltage12V", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DE8 RID: 3560
		// (get) Token: 0x060030E6 RID: 12518 RVA: 0x0013E940 File Offset: 0x0013CB40
		public static string parItemVoltage24V
		{
			get
			{
				return Resources.ResourceManager.GetString("parItemVoltage24V", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DE9 RID: 3561
		// (get) Token: 0x060030E7 RID: 12519 RVA: 0x0013E968 File Offset: 0x0013CB68
		public static string parItemXRaySource
		{
			get
			{
				return Resources.ResourceManager.GetString("parItemXRaySource", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DEA RID: 3562
		// (get) Token: 0x060030E8 RID: 12520 RVA: 0x0013E990 File Offset: 0x0013CB90
		public static string parShieldingLeftBack
		{
			get
			{
				return Resources.ResourceManager.GetString("parShieldingLeftBack", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DEB RID: 3563
		// (get) Token: 0x060030E9 RID: 12521 RVA: 0x0013E9B8 File Offset: 0x0013CBB8
		public static string parShieldingLeftFront
		{
			get
			{
				return Resources.ResourceManager.GetString("parShieldingLeftFront", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DEC RID: 3564
		// (get) Token: 0x060030EA RID: 12522 RVA: 0x0013E9E0 File Offset: 0x0013CBE0
		public static string parShieldingRightBack
		{
			get
			{
				return Resources.ResourceManager.GetString("parShieldingRightBack", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DED RID: 3565
		// (get) Token: 0x060030EB RID: 12523 RVA: 0x0013EA08 File Offset: 0x0013CC08
		public static string parShieldingRightFront
		{
			get
			{
				return Resources.ResourceManager.GetString("parShieldingRightFront", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DEE RID: 3566
		// (get) Token: 0x060030EC RID: 12524 RVA: 0x0013EA30 File Offset: 0x0013CC30
		public static string Please_select_a_user_for_rfid_authentication
		{
			get
			{
				return Resources.ResourceManager.GetString("Please_select_a_user_for_rfid_authentication", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DEF RID: 3567
		// (get) Token: 0x060030ED RID: 12525 RVA: 0x0013EA58 File Offset: 0x0013CC58
		public static string rfid_already_mapped_to_a_user_map_to_a_second_user
		{
			get
			{
				return Resources.ResourceManager.GetString("rfid_already_mapped_to_a_user_map_to_a_second_user", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DF0 RID: 3568
		// (get) Token: 0x060030EE RID: 12526 RVA: 0x0013EA80 File Offset: 0x0013CC80
		public static string RFID_not_mapped_with_a_user_check_one_more
		{
			get
			{
				return Resources.ResourceManager.GetString("RFID_not_mapped_with_a_user_check_one_more", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DF1 RID: 3569
		// (get) Token: 0x060030EF RID: 12527 RVA: 0x0013EAA8 File Offset: 0x0013CCA8
		public static Bitmap sesotec_certificate_raycon_de_2023
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("sesotec_certificate_raycon_de_2023", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}

		// Token: 0x17000DF2 RID: 3570
		// (get) Token: 0x060030F0 RID: 12528 RVA: 0x0013EAD8 File Offset: 0x0013CCD8
		public static Bitmap sesotec_certificate_raycon_en_2023
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("sesotec_certificate_raycon_en_2023", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}

		// Token: 0x17000DF3 RID: 3571
		// (get) Token: 0x060030F1 RID: 12529 RVA: 0x0013EB08 File Offset: 0x0013CD08
		public static string StrSignInArea
		{
			get
			{
				return Resources.ResourceManager.GetString("StrSignInArea", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DF4 RID: 3572
		// (get) Token: 0x060030F2 RID: 12530 RVA: 0x0013EB30 File Offset: 0x0013CD30
		public static Bitmap SuS_logo
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("SuS_logo", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}

		// Token: 0x17000DF5 RID: 3573
		// (get) Token: 0x060030F3 RID: 12531 RVA: 0x0013EB60 File Offset: 0x0013CD60
		public static string txtAbortAutotrain
		{
			get
			{
				return Resources.ResourceManager.GetString("txtAbortAutotrain", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DF6 RID: 3574
		// (get) Token: 0x060030F4 RID: 12532 RVA: 0x0013EB88 File Offset: 0x0013CD88
		public static string txtAbortTestcardSim
		{
			get
			{
				return Resources.ResourceManager.GetString("txtAbortTestcardSim", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DF7 RID: 3575
		// (get) Token: 0x060030F5 RID: 12533 RVA: 0x0013EBB0 File Offset: 0x0013CDB0
		public static string txtAboveMaxImageSize
		{
			get
			{
				return Resources.ResourceManager.GetString("txtAboveMaxImageSize", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DF8 RID: 3576
		// (get) Token: 0x060030F6 RID: 12534 RVA: 0x0013EBD8 File Offset: 0x0013CDD8
		public static string txtAboveMaxObjectCount
		{
			get
			{
				return Resources.ResourceManager.GetString("txtAboveMaxObjectCount", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DF9 RID: 3577
		// (get) Token: 0x060030F7 RID: 12535 RVA: 0x0013EC00 File Offset: 0x0013CE00
		public static string txtAcquisitionMode
		{
			get
			{
				return Resources.ResourceManager.GetString("txtAcquisitionMode", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DFA RID: 3578
		// (get) Token: 0x060030F8 RID: 12536 RVA: 0x0013EC28 File Offset: 0x0013CE28
		public static string txtActivateImageSize
		{
			get
			{
				return Resources.ResourceManager.GetString("txtActivateImageSize", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DFB RID: 3579
		// (get) Token: 0x060030F9 RID: 12537 RVA: 0x0013EC50 File Offset: 0x0013CE50
		public static string txtActivateTestcardSim
		{
			get
			{
				return Resources.ResourceManager.GetString("txtActivateTestcardSim", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DFC RID: 3580
		// (get) Token: 0x060030FA RID: 12538 RVA: 0x0013EC78 File Offset: 0x0013CE78
		public static string txtActSensBetterInitSens
		{
			get
			{
				return Resources.ResourceManager.GetString("txtActSensBetterInitSens", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DFD RID: 3581
		// (get) Token: 0x060030FB RID: 12539 RVA: 0x0013ECA0 File Offset: 0x0013CEA0
		public static string txtActSensTestcardSim
		{
			get
			{
				return Resources.ResourceManager.GetString("txtActSensTestcardSim", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DFE RID: 3582
		// (get) Token: 0x060030FC RID: 12540 RVA: 0x0013ECC8 File Offset: 0x0013CEC8
		public static string txtActSensWorseInitSens
		{
			get
			{
				return Resources.ResourceManager.GetString("txtActSensWorseInitSens", Resources.resourceCulture);
			}
		}

		// Token: 0x17000DFF RID: 3583
		// (get) Token: 0x060030FD RID: 12541 RVA: 0x0013ECF0 File Offset: 0x0013CEF0
		public static string txtActual
		{
			get
			{
				return Resources.ResourceManager.GetString("txtActual", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E00 RID: 3584
		// (get) Token: 0x060030FE RID: 12542 RVA: 0x0013ED18 File Offset: 0x0013CF18
		public static string txtActualImage
		{
			get
			{
				return Resources.ResourceManager.GetString("txtActualImage", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E01 RID: 3585
		// (get) Token: 0x060030FF RID: 12543 RVA: 0x0013ED40 File Offset: 0x0013CF40
		public static string txtActualSensitivities
		{
			get
			{
				return Resources.ResourceManager.GetString("txtActualSensitivities", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E02 RID: 3586
		// (get) Token: 0x06003100 RID: 12544 RVA: 0x0013ED68 File Offset: 0x0013CF68
		public static string txtAddedSidePixel
		{
			get
			{
				return Resources.ResourceManager.GetString("txtAddedSidePixel", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E03 RID: 3587
		// (get) Token: 0x06003101 RID: 12545 RVA: 0x0013ED90 File Offset: 0x0013CF90
		public static string txtAdditionalAddress
		{
			get
			{
				return Resources.ResourceManager.GetString("txtAdditionalAddress", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E04 RID: 3588
		// (get) Token: 0x06003102 RID: 12546 RVA: 0x0013EDB8 File Offset: 0x0013CFB8
		public static string txtAdjustingXRayVoltage
		{
			get
			{
				return Resources.ResourceManager.GetString("txtAdjustingXRayVoltage", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E05 RID: 3589
		// (get) Token: 0x06003103 RID: 12547 RVA: 0x0013EDE0 File Offset: 0x0013CFE0
		public static string txtAdjustmentTestcardPat
		{
			get
			{
				return Resources.ResourceManager.GetString("txtAdjustmentTestcardPat", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E06 RID: 3590
		// (get) Token: 0x06003104 RID: 12548 RVA: 0x0013EE08 File Offset: 0x0013D008
		public static string txtAfterAutotrain
		{
			get
			{
				return Resources.ResourceManager.GetString("txtAfterAutotrain", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E07 RID: 3591
		// (get) Token: 0x06003105 RID: 12549 RVA: 0x0013EE30 File Offset: 0x0013D030
		public static string txtAI
		{
			get
			{
				return Resources.ResourceManager.GetString("txtAI", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E08 RID: 3592
		// (get) Token: 0x06003106 RID: 12550 RVA: 0x0013EE58 File Offset: 0x0013D058
		public static string txtAICreateDefaultTable
		{
			get
			{
				return Resources.ResourceManager.GetString("txtAICreateDefaultTable", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E09 RID: 3593
		// (get) Token: 0x06003107 RID: 12551 RVA: 0x0013EE80 File Offset: 0x0013D080
		public static string txtAIDeleteSelected
		{
			get
			{
				return Resources.ResourceManager.GetString("txtAIDeleteSelected", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E0A RID: 3594
		// (get) Token: 0x06003108 RID: 12552 RVA: 0x0013EEA8 File Offset: 0x0013D0A8
		public static string txtAINoInferenceStartPossible
		{
			get
			{
				return Resources.ResourceManager.GetString("txtAINoInferenceStartPossible", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E0B RID: 3595
		// (get) Token: 0x06003109 RID: 12553 RVA: 0x0013EED0 File Offset: 0x0013D0D0
		public static string txtAISendImages
		{
			get
			{
				return Resources.ResourceManager.GetString("txtAISendImages", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E0C RID: 3596
		// (get) Token: 0x0600310A RID: 12554 RVA: 0x0013EEF8 File Offset: 0x0013D0F8
		public static string txtAIStartFinetuning
		{
			get
			{
				return Resources.ResourceManager.GetString("txtAIStartFinetuning", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E0D RID: 3597
		// (get) Token: 0x0600310B RID: 12555 RVA: 0x0013EF20 File Offset: 0x0013D120
		public static string txtAITraining
		{
			get
			{
				return Resources.ResourceManager.GetString("txtAITraining", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E0E RID: 3598
		// (get) Token: 0x0600310C RID: 12556 RVA: 0x0013EF48 File Offset: 0x0013D148
		public static string txtAITrainingAbort1
		{
			get
			{
				return Resources.ResourceManager.GetString("txtAITrainingAbort1", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E0F RID: 3599
		// (get) Token: 0x0600310D RID: 12557 RVA: 0x0013EF70 File Offset: 0x0013D170
		public static string txtAITrainingAbort2
		{
			get
			{
				return Resources.ResourceManager.GetString("txtAITrainingAbort2", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E10 RID: 3600
		// (get) Token: 0x0600310E RID: 12558 RVA: 0x0013EF98 File Offset: 0x0013D198
		public static string txtAITrainingAbort3
		{
			get
			{
				return Resources.ResourceManager.GetString("txtAITrainingAbort3", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E11 RID: 3601
		// (get) Token: 0x0600310F RID: 12559 RVA: 0x0013EFC0 File Offset: 0x0013D1C0
		public static string txtAITrainingAbort4
		{
			get
			{
				return Resources.ResourceManager.GetString("txtAITrainingAbort4", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E12 RID: 3602
		// (get) Token: 0x06003110 RID: 12560 RVA: 0x0013EFE8 File Offset: 0x0013D1E8
		public static string txtAITrainingDelete
		{
			get
			{
				return Resources.ResourceManager.GetString("txtAITrainingDelete", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E13 RID: 3603
		// (get) Token: 0x06003111 RID: 12561 RVA: 0x0013F010 File Offset: 0x0013D210
		public static string txtAITrainingDeleteActiv
		{
			get
			{
				return Resources.ResourceManager.GetString("txtAITrainingDeleteActiv", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E14 RID: 3604
		// (get) Token: 0x06003112 RID: 12562 RVA: 0x0013F038 File Offset: 0x0013D238
		public static string txtAITrainingDeleteDone
		{
			get
			{
				return Resources.ResourceManager.GetString("txtAITrainingDeleteDone", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E15 RID: 3605
		// (get) Token: 0x06003113 RID: 12563 RVA: 0x0013F060 File Offset: 0x0013D260
		public static string txtAITrainingRestore
		{
			get
			{
				return Resources.ResourceManager.GetString("txtAITrainingRestore", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E16 RID: 3606
		// (get) Token: 0x06003114 RID: 12564 RVA: 0x0013F088 File Offset: 0x0013D288
		public static string txtAITrainingRestoreDone
		{
			get
			{
				return Resources.ResourceManager.GetString("txtAITrainingRestoreDone", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E17 RID: 3607
		// (get) Token: 0x06003115 RID: 12565 RVA: 0x0013F0B0 File Offset: 0x0013D2B0
		public static string txtAITrainingRestoreLoad
		{
			get
			{
				return Resources.ResourceManager.GetString("txtAITrainingRestoreLoad", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E18 RID: 3608
		// (get) Token: 0x06003116 RID: 12566 RVA: 0x0013F0D8 File Offset: 0x0013D2D8
		public static string txtAll
		{
			get
			{
				return Resources.ResourceManager.GetString("txtAll", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E19 RID: 3609
		// (get) Token: 0x06003117 RID: 12567 RVA: 0x0013F100 File Offset: 0x0013D300
		public static string txtAllTestcards
		{
			get
			{
				return Resources.ResourceManager.GetString("txtAllTestcards", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E1A RID: 3610
		// (get) Token: 0x06003118 RID: 12568 RVA: 0x0013F128 File Offset: 0x0013D328
		public static string txtAlternateDNSServer
		{
			get
			{
				return Resources.ResourceManager.GetString("txtAlternateDNSServer", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E1B RID: 3611
		// (get) Token: 0x06003119 RID: 12569 RVA: 0x0013F150 File Offset: 0x0013D350
		public static string txtAluminium
		{
			get
			{
				return Resources.ResourceManager.GetString("txtAluminium", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E1C RID: 3612
		// (get) Token: 0x0600311A RID: 12570 RVA: 0x0013F178 File Offset: 0x0013D378
		public static string txtAnomalyDetectionDisableInference
		{
			get
			{
				return Resources.ResourceManager.GetString("txtAnomalyDetectionDisableInference", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E1D RID: 3613
		// (get) Token: 0x0600311B RID: 12571 RVA: 0x0013F1A0 File Offset: 0x0013D3A0
		public static string txtAnomalyDetectionDisableTraining
		{
			get
			{
				return Resources.ResourceManager.GetString("txtAnomalyDetectionDisableTraining", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E1E RID: 3614
		// (get) Token: 0x0600311C RID: 12572 RVA: 0x0013F1C8 File Offset: 0x0013D3C8
		public static string txtAnomalyDetectionNoModel
		{
			get
			{
				return Resources.ResourceManager.GetString("txtAnomalyDetectionNoModel", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E1F RID: 3615
		// (get) Token: 0x0600311D RID: 12573 RVA: 0x0013F1F0 File Offset: 0x0013D3F0
		public static string txtAnomalyDetectionOnlyFineTuning
		{
			get
			{
				return Resources.ResourceManager.GetString("txtAnomalyDetectionOnlyFineTuning", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E20 RID: 3616
		// (get) Token: 0x0600311E RID: 12574 RVA: 0x0013F218 File Offset: 0x0013D418
		public static string txtAnomalyDetectionTrainingProgress1
		{
			get
			{
				return Resources.ResourceManager.GetString("txtAnomalyDetectionTrainingProgress1", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E21 RID: 3617
		// (get) Token: 0x0600311F RID: 12575 RVA: 0x0013F240 File Offset: 0x0013D440
		public static string txtAnomalyDetectionTrainingProgress2
		{
			get
			{
				return Resources.ResourceManager.GetString("txtAnomalyDetectionTrainingProgress2", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E22 RID: 3618
		// (get) Token: 0x06003120 RID: 12576 RVA: 0x0013F268 File Offset: 0x0013D468
		public static string txtApplication
		{
			get
			{
				return Resources.ResourceManager.GetString("txtApplication", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E23 RID: 3619
		// (get) Token: 0x06003121 RID: 12577 RVA: 0x0013F290 File Offset: 0x0013D490
		public static string txtArcs
		{
			get
			{
				return Resources.ResourceManager.GetString("txtArcs", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E24 RID: 3620
		// (get) Token: 0x06003122 RID: 12578 RVA: 0x0013F2B8 File Offset: 0x0013D4B8
		public static string txtAssignNewDefaultProduct
		{
			get
			{
				return Resources.ResourceManager.GetString("txtAssignNewDefaultProduct", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E25 RID: 3621
		// (get) Token: 0x06003123 RID: 12579 RVA: 0x0013F2E0 File Offset: 0x0013D4E0
		public static string txtAuditActive
		{
			get
			{
				return Resources.ResourceManager.GetString("txtAuditActive", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E26 RID: 3622
		// (get) Token: 0x06003124 RID: 12580 RVA: 0x0013F308 File Offset: 0x0013D508
		public static string txtAuditCancelled
		{
			get
			{
				return Resources.ResourceManager.GetString("txtAuditCancelled", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E27 RID: 3623
		// (get) Token: 0x06003125 RID: 12581 RVA: 0x0013F330 File Offset: 0x0013D530
		public static string txtAuditCheckContRight
		{
			get
			{
				return Resources.ResourceManager.GetString("txtAuditCheckContRight", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E28 RID: 3624
		// (get) Token: 0x06003126 RID: 12582 RVA: 0x0013F358 File Offset: 0x0013D558
		public static string txtAuditCheckImages
		{
			get
			{
				return Resources.ResourceManager.GetString("txtAuditCheckImages", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E29 RID: 3625
		// (get) Token: 0x06003127 RID: 12583 RVA: 0x0013F380 File Offset: 0x0013D580
		public static string txtAuditCheckProductChange
		{
			get
			{
				return Resources.ResourceManager.GetString("txtAuditCheckProductChange", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E2A RID: 3626
		// (get) Token: 0x06003128 RID: 12584 RVA: 0x0013F3A8 File Offset: 0x0013D5A8
		public static string txtAuditContaminationActive
		{
			get
			{
				return Resources.ResourceManager.GetString("txtAuditContaminationActive", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E2B RID: 3627
		// (get) Token: 0x06003129 RID: 12585 RVA: 0x0013F3D0 File Offset: 0x0013D5D0
		public static string txtAuditCountActive
		{
			get
			{
				return Resources.ResourceManager.GetString("txtAuditCountActive", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E2C RID: 3628
		// (get) Token: 0x0600312A RID: 12586 RVA: 0x0013F3F8 File Offset: 0x0013D5F8
		public static string txtAuditDay
		{
			get
			{
				return Resources.ResourceManager.GetString("txtAuditDay", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E2D RID: 3629
		// (get) Token: 0x0600312B RID: 12587 RVA: 0x0013F420 File Offset: 0x0013D620
		public static string txtAuditFailedContinue
		{
			get
			{
				return Resources.ResourceManager.GetString("txtAuditFailedContinue", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E2E RID: 3630
		// (get) Token: 0x0600312C RID: 12588 RVA: 0x0013F448 File Offset: 0x0013D648
		public static string txtAuditFailedError
		{
			get
			{
				return Resources.ResourceManager.GetString("txtAuditFailedError", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E2F RID: 3631
		// (get) Token: 0x0600312D RID: 12589 RVA: 0x0013F470 File Offset: 0x0013D670
		public static string txtAuditInterval
		{
			get
			{
				return Resources.ResourceManager.GetString("txtAuditInterval", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E30 RID: 3632
		// (get) Token: 0x0600312E RID: 12590 RVA: 0x0013F498 File Offset: 0x0013D698
		public static string txtAuditIntervalHour
		{
			get
			{
				return Resources.ResourceManager.GetString("txtAuditIntervalHour", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E31 RID: 3633
		// (get) Token: 0x0600312F RID: 12591 RVA: 0x0013F4C0 File Offset: 0x0013D6C0
		public static string txtAuditIntervalMinute
		{
			get
			{
				return Resources.ResourceManager.GetString("txtAuditIntervalMinute", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E32 RID: 3634
		// (get) Token: 0x06003130 RID: 12592 RVA: 0x0013F4E8 File Offset: 0x0013D6E8
		public static string txtAuditMode
		{
			get
			{
				return Resources.ResourceManager.GetString("txtAuditMode", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E33 RID: 3635
		// (get) Token: 0x06003131 RID: 12593 RVA: 0x0013F510 File Offset: 0x0013D710
		public static string txtAuditNeeded
		{
			get
			{
				return Resources.ResourceManager.GetString("txtAuditNeeded", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E34 RID: 3636
		// (get) Token: 0x06003132 RID: 12594 RVA: 0x0013F538 File Offset: 0x0013D738
		public static string txtAuditNumberContaminationsLess
		{
			get
			{
				return Resources.ResourceManager.GetString("txtAuditNumberContaminationsLess", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E35 RID: 3637
		// (get) Token: 0x06003133 RID: 12595 RVA: 0x0013F560 File Offset: 0x0013D760
		public static string txtAuditNumberContaminationsRight
		{
			get
			{
				return Resources.ResourceManager.GetString("txtAuditNumberContaminationsRight", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E36 RID: 3638
		// (get) Token: 0x06003134 RID: 12596 RVA: 0x0013F588 File Offset: 0x0013D788
		public static string txtAuditTestballFinished
		{
			get
			{
				return Resources.ResourceManager.GetString("txtAuditTestballFinished", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E37 RID: 3639
		// (get) Token: 0x06003135 RID: 12597 RVA: 0x0013F5B0 File Offset: 0x0013D7B0
		public static string txtAuditTime
		{
			get
			{
				return Resources.ResourceManager.GetString("txtAuditTime", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E38 RID: 3640
		// (get) Token: 0x06003136 RID: 12598 RVA: 0x0013F5D8 File Offset: 0x0013D7D8
		public static string txtAuditTimeHour
		{
			get
			{
				return Resources.ResourceManager.GetString("txtAuditTimeHour", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E39 RID: 3641
		// (get) Token: 0x06003137 RID: 12599 RVA: 0x0013F600 File Offset: 0x0013D800
		public static string txtAuditTimeMinute
		{
			get
			{
				return Resources.ResourceManager.GetString("txtAuditTimeMinute", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E3A RID: 3642
		// (get) Token: 0x06003138 RID: 12600 RVA: 0x0013F628 File Offset: 0x0013D828
		public static string txtAuditWeightActive
		{
			get
			{
				return Resources.ResourceManager.GetString("txtAuditWeightActive", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E3B RID: 3643
		// (get) Token: 0x06003139 RID: 12601 RVA: 0x0013F650 File Offset: 0x0013D850
		public static string txtAutoEdgeFilterActive
		{
			get
			{
				return Resources.ResourceManager.GetString("txtAutoEdgeFilterActive", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E3C RID: 3644
		// (get) Token: 0x0600313A RID: 12602 RVA: 0x0013F678 File Offset: 0x0013D878
		public static string txtAutoKVActive
		{
			get
			{
				return Resources.ResourceManager.GetString("txtAutoKVActive", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E3D RID: 3645
		// (get) Token: 0x0600313B RID: 12603 RVA: 0x0013F6A0 File Offset: 0x0013D8A0
		public static string txtAutotrain
		{
			get
			{
				return Resources.ResourceManager.GetString("txtAutotrain", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E3E RID: 3646
		// (get) Token: 0x0600313C RID: 12604 RVA: 0x0013F6C8 File Offset: 0x0013D8C8
		public static string txtAvailableWlanConnections
		{
			get
			{
				return Resources.ResourceManager.GetString("txtAvailableWlanConnections", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E3F RID: 3647
		// (get) Token: 0x0600313D RID: 12605 RVA: 0x0013F6F0 File Offset: 0x0013D8F0
		public static string txtBackground
		{
			get
			{
				return Resources.ResourceManager.GetString("txtBackground", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E40 RID: 3648
		// (get) Token: 0x0600313E RID: 12606 RVA: 0x0013F718 File Offset: 0x0013D918
		public static string txtBackgroundIndex
		{
			get
			{
				return Resources.ResourceManager.GetString("txtBackgroundIndex", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E41 RID: 3649
		// (get) Token: 0x0600313F RID: 12607 RVA: 0x0013F740 File Offset: 0x0013D940
		public static string txtBackgroundMonitoring
		{
			get
			{
				return Resources.ResourceManager.GetString("txtBackgroundMonitoring", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E42 RID: 3650
		// (get) Token: 0x06003140 RID: 12608 RVA: 0x0013F768 File Offset: 0x0013D968
		public static string txtBackgroundNoise
		{
			get
			{
				return Resources.ResourceManager.GetString("txtBackgroundNoise", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E43 RID: 3651
		// (get) Token: 0x06003141 RID: 12609 RVA: 0x0013F790 File Offset: 0x0013D990
		public static string txtBackupCopied
		{
			get
			{
				return Resources.ResourceManager.GetString("txtBackupCopied", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E44 RID: 3652
		// (get) Token: 0x06003142 RID: 12610 RVA: 0x0013F7B8 File Offset: 0x0013D9B8
		public static string txtBackupFailed
		{
			get
			{
				return Resources.ResourceManager.GetString("txtBackupFailed", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E45 RID: 3653
		// (get) Token: 0x06003143 RID: 12611 RVA: 0x0013F7E0 File Offset: 0x0013D9E0
		public static string txtBackupRestoredThink
		{
			get
			{
				return Resources.ResourceManager.GetString("txtBackupRestoredThink", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E46 RID: 3654
		// (get) Token: 0x06003144 RID: 12612 RVA: 0x0013F808 File Offset: 0x0013DA08
		public static string txtBackupSuccess
		{
			get
			{
				return Resources.ResourceManager.GetString("txtBackupSuccess", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E47 RID: 3655
		// (get) Token: 0x06003145 RID: 12613 RVA: 0x0013F830 File Offset: 0x0013DA30
		public static string txtBackupTime
		{
			get
			{
				return Resources.ResourceManager.GetString("txtBackupTime", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E48 RID: 3656
		// (get) Token: 0x06003146 RID: 12614 RVA: 0x0013F858 File Offset: 0x0013DA58
		public static string txtBackupTo
		{
			get
			{
				return Resources.ResourceManager.GetString("txtBackupTo", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E49 RID: 3657
		// (get) Token: 0x06003147 RID: 12615 RVA: 0x0013F880 File Offset: 0x0013DA80
		public static string txtBadImages
		{
			get
			{
				return Resources.ResourceManager.GetString("txtBadImages", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E4A RID: 3658
		// (get) Token: 0x06003148 RID: 12616 RVA: 0x0013F8A8 File Offset: 0x0013DAA8
		public static string txtBadPixel
		{
			get
			{
				return Resources.ResourceManager.GetString("txtBadPixel", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E4B RID: 3659
		// (get) Token: 0x06003149 RID: 12617 RVA: 0x0013F8D0 File Offset: 0x0013DAD0
		public static string txtBallCountPerImg
		{
			get
			{
				return Resources.ResourceManager.GetString("txtBallCountPerImg", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E4C RID: 3660
		// (get) Token: 0x0600314A RID: 12618 RVA: 0x0013F8F8 File Offset: 0x0013DAF8
		public static string txtBallDiameter
		{
			get
			{
				return Resources.ResourceManager.GetString("txtBallDiameter", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E4D RID: 3661
		// (get) Token: 0x0600314B RID: 12619 RVA: 0x0013F920 File Offset: 0x0013DB20
		public static string txtBatchAlreadyEnded
		{
			get
			{
				return Resources.ResourceManager.GetString("txtBatchAlreadyEnded", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E4E RID: 3662
		// (get) Token: 0x0600314C RID: 12620 RVA: 0x0013F948 File Offset: 0x0013DB48
		public static string txtBatchAlreadyStarted
		{
			get
			{
				return Resources.ResourceManager.GetString("txtBatchAlreadyStarted", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E4F RID: 3663
		// (get) Token: 0x0600314D RID: 12621 RVA: 0x0013F970 File Offset: 0x0013DB70
		public static string txtBatchChange
		{
			get
			{
				return Resources.ResourceManager.GetString("txtBatchChange", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E50 RID: 3664
		// (get) Token: 0x0600314E RID: 12622 RVA: 0x0013F998 File Offset: 0x0013DB98
		public static string txtBatchIDExists
		{
			get
			{
				return Resources.ResourceManager.GetString("txtBatchIDExists", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E51 RID: 3665
		// (get) Token: 0x0600314F RID: 12623 RVA: 0x0013F9C0 File Offset: 0x0013DBC0
		public static string txtBelowMinImageSize
		{
			get
			{
				return Resources.ResourceManager.GetString("txtBelowMinImageSize", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E52 RID: 3666
		// (get) Token: 0x06003150 RID: 12624 RVA: 0x0013F9E8 File Offset: 0x0013DBE8
		public static string txtBelowMinObjectCount
		{
			get
			{
				return Resources.ResourceManager.GetString("txtBelowMinObjectCount", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E53 RID: 3667
		// (get) Token: 0x06003151 RID: 12625 RVA: 0x0013FA10 File Offset: 0x0013DC10
		public static string txtBeltContamination
		{
			get
			{
				return Resources.ResourceManager.GetString("txtBeltContamination", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E54 RID: 3668
		// (get) Token: 0x06003152 RID: 12626 RVA: 0x0013FA38 File Offset: 0x0013DC38
		public static string txtBeltStopOnContaminant
		{
			get
			{
				return Resources.ResourceManager.GetString("txtBeltStopOnContaminant", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E55 RID: 3669
		// (get) Token: 0x06003153 RID: 12627 RVA: 0x0013FA60 File Offset: 0x0013DC60
		public static string txtBinFullTimeout
		{
			get
			{
				return Resources.ResourceManager.GetString("txtBinFullTimeout", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E56 RID: 3670
		// (get) Token: 0x06003154 RID: 12628 RVA: 0x0013FA88 File Offset: 0x0013DC88
		public static string txtBinIsOpen
		{
			get
			{
				return Resources.ResourceManager.GetString("txtBinIsOpen", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E57 RID: 3671
		// (get) Token: 0x06003155 RID: 12629 RVA: 0x0013FAB0 File Offset: 0x0013DCB0
		public static string txtBinning2
		{
			get
			{
				return Resources.ResourceManager.GetString("txtBinning2", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E58 RID: 3672
		// (get) Token: 0x06003156 RID: 12630 RVA: 0x0013FAD8 File Offset: 0x0013DCD8
		public static string txtBinUnlocked
		{
			get
			{
				return Resources.ResourceManager.GetString("txtBinUnlocked", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E59 RID: 3673
		// (get) Token: 0x06003157 RID: 12631 RVA: 0x0013FB00 File Offset: 0x0013DD00
		public static string txtBinUnlocked1
		{
			get
			{
				return Resources.ResourceManager.GetString("txtBinUnlocked1", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E5A RID: 3674
		// (get) Token: 0x06003158 RID: 12632 RVA: 0x0013FB28 File Offset: 0x0013DD28
		public static string txtBinUnlocked2
		{
			get
			{
				return Resources.ResourceManager.GetString("txtBinUnlocked2", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E5B RID: 3675
		// (get) Token: 0x06003159 RID: 12633 RVA: 0x0013FB50 File Offset: 0x0013DD50
		public static string txtBone
		{
			get
			{
				return Resources.ResourceManager.GetString("txtBone", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E5C RID: 3676
		// (get) Token: 0x0600315A RID: 12634 RVA: 0x0013FB78 File Offset: 0x0013DD78
		public static string txtBulkRetrain
		{
			get
			{
				return Resources.ResourceManager.GetString("txtBulkRetrain", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E5D RID: 3677
		// (get) Token: 0x0600315B RID: 12635 RVA: 0x0013FBA0 File Offset: 0x0013DDA0
		public static string txtBulkShowContimage
		{
			get
			{
				return Resources.ResourceManager.GetString("txtBulkShowContimage", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E5E RID: 3678
		// (get) Token: 0x0600315C RID: 12636 RVA: 0x0013FBC8 File Offset: 0x0013DDC8
		public static string txtBuzzer
		{
			get
			{
				return Resources.ResourceManager.GetString("txtBuzzer", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E5F RID: 3679
		// (get) Token: 0x0600315D RID: 12637 RVA: 0x0013FBF0 File Offset: 0x0013DDF0
		public static string txtBuzzerEnable
		{
			get
			{
				return Resources.ResourceManager.GetString("txtBuzzerEnable", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E60 RID: 3680
		// (get) Token: 0x0600315E RID: 12638 RVA: 0x0013FC18 File Offset: 0x0013DE18
		public static string txtBuzzerEnableIfError
		{
			get
			{
				return Resources.ResourceManager.GetString("txtBuzzerEnableIfError", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E61 RID: 3681
		// (get) Token: 0x0600315F RID: 12639 RVA: 0x0013FC40 File Offset: 0x0013DE40
		public static string txtCalcSensQuestionTestcardSim
		{
			get
			{
				return Resources.ResourceManager.GetString("txtCalcSensQuestionTestcardSim", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E62 RID: 3682
		// (get) Token: 0x06003160 RID: 12640 RVA: 0x0013FC68 File Offset: 0x0013DE68
		public static string txtCalcSensTestcardSim
		{
			get
			{
				return Resources.ResourceManager.GetString("txtCalcSensTestcardSim", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E63 RID: 3683
		// (get) Token: 0x06003161 RID: 12641 RVA: 0x0013FC90 File Offset: 0x0013DE90
		public static string txtCalibrating
		{
			get
			{
				return Resources.ResourceManager.GetString("txtCalibrating", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E64 RID: 3684
		// (get) Token: 0x06003162 RID: 12642 RVA: 0x0013FCB8 File Offset: 0x0013DEB8
		public static string txtCalibration
		{
			get
			{
				return Resources.ResourceManager.GetString("txtCalibration", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E65 RID: 3685
		// (get) Token: 0x06003163 RID: 12643 RVA: 0x0013FCE0 File Offset: 0x0013DEE0
		public static string txtCanFilterActive
		{
			get
			{
				return Resources.ResourceManager.GetString("txtCanFilterActive", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E66 RID: 3686
		// (get) Token: 0x06003164 RID: 12644 RVA: 0x0013FD08 File Offset: 0x0013DF08
		public static string txtCannotContinueAuditCheck
		{
			get
			{
				return Resources.ResourceManager.GetString("txtCannotContinueAuditCheck", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E67 RID: 3687
		// (get) Token: 0x06003165 RID: 12645 RVA: 0x0013FD30 File Offset: 0x0013DF30
		public static string txtCertificate
		{
			get
			{
				return Resources.ResourceManager.GetString("txtCertificate", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E68 RID: 3688
		// (get) Token: 0x06003166 RID: 12646 RVA: 0x0013FD58 File Offset: 0x0013DF58
		public static string txtChangedTestcardSim
		{
			get
			{
				return Resources.ResourceManager.GetString("txtChangedTestcardSim", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E69 RID: 3689
		// (get) Token: 0x06003167 RID: 12647 RVA: 0x0013FD80 File Offset: 0x0013DF80
		public static string txtCheckpoint
		{
			get
			{
				return Resources.ResourceManager.GetString("txtCheckpoint", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E6A RID: 3690
		// (get) Token: 0x06003168 RID: 12648 RVA: 0x0013FDA8 File Offset: 0x0013DFA8
		public static string txtCheckpointCreated
		{
			get
			{
				return Resources.ResourceManager.GetString("txtCheckpointCreated", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E6B RID: 3691
		// (get) Token: 0x06003169 RID: 12649 RVA: 0x0013FDD0 File Offset: 0x0013DFD0
		public static string txtCheckpointDelete
		{
			get
			{
				return Resources.ResourceManager.GetString("txtCheckpointDelete", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E6C RID: 3692
		// (get) Token: 0x0600316A RID: 12650 RVA: 0x0013FDF8 File Offset: 0x0013DFF8
		public static string txtCheckpointDeleted
		{
			get
			{
				return Resources.ResourceManager.GetString("txtCheckpointDeleted", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E6D RID: 3693
		// (get) Token: 0x0600316B RID: 12651 RVA: 0x0013FE20 File Offset: 0x0013E020
		public static string txtCheckpointDeleteOldest
		{
			get
			{
				return Resources.ResourceManager.GetString("txtCheckpointDeleteOldest", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E6E RID: 3694
		// (get) Token: 0x0600316C RID: 12652 RVA: 0x0013FE48 File Offset: 0x0013E048
		public static string txtCheckpointDeleteOldestFormOtherProduct
		{
			get
			{
				return Resources.ResourceManager.GetString("txtCheckpointDeleteOldestFormOtherProduct", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E6F RID: 3695
		// (get) Token: 0x0600316D RID: 12653 RVA: 0x0013FE70 File Offset: 0x0013E070
		public static string txtCheckpoints
		{
			get
			{
				return Resources.ResourceManager.GetString("txtCheckpoints", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E70 RID: 3696
		// (get) Token: 0x0600316E RID: 12654 RVA: 0x0013FE98 File Offset: 0x0013E098
		public static string txtChooseAnotherSubnet
		{
			get
			{
				return Resources.ResourceManager.GetString("txtChooseAnotherSubnet", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E71 RID: 3697
		// (get) Token: 0x0600316F RID: 12655 RVA: 0x0013FEC0 File Offset: 0x0013E0C0
		public static string txtChooseSubproduct
		{
			get
			{
				return Resources.ResourceManager.GetString("txtChooseSubproduct", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E72 RID: 3698
		// (get) Token: 0x06003170 RID: 12656 RVA: 0x0013FEE8 File Offset: 0x0013E0E8
		public static string txtChoseBackup
		{
			get
			{
				return Resources.ResourceManager.GetString("txtChoseBackup", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E73 RID: 3699
		// (get) Token: 0x06003171 RID: 12657 RVA: 0x0013FF10 File Offset: 0x0013E110
		public static string txtComment
		{
			get
			{
				return Resources.ResourceManager.GetString("txtComment", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E74 RID: 3700
		// (get) Token: 0x06003172 RID: 12658 RVA: 0x0013FF38 File Offset: 0x0013E138
		public static string txtCommitErrors
		{
			get
			{
				return Resources.ResourceManager.GetString("txtCommitErrors", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E75 RID: 3701
		// (get) Token: 0x06003173 RID: 12659 RVA: 0x0013FF60 File Offset: 0x0013E160
		public static string txtCommonSettings
		{
			get
			{
				return Resources.ResourceManager.GetString("txtCommonSettings", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E76 RID: 3702
		// (get) Token: 0x06003174 RID: 12660 RVA: 0x0013FF88 File Offset: 0x0013E188
		public static string txtCompareActSensWithDBValues
		{
			get
			{
				return Resources.ResourceManager.GetString("txtCompareActSensWithDBValues", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E77 RID: 3703
		// (get) Token: 0x06003175 RID: 12661 RVA: 0x0013FFB0 File Offset: 0x0013E1B0
		public static string txtComparer
		{
			get
			{
				return Resources.ResourceManager.GetString("txtComparer", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E78 RID: 3704
		// (get) Token: 0x06003176 RID: 12662 RVA: 0x0013FFD8 File Offset: 0x0013E1D8
		public static string txtCompliance
		{
			get
			{
				return Resources.ResourceManager.GetString("txtCompliance", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E79 RID: 3705
		// (get) Token: 0x06003177 RID: 12663 RVA: 0x00140000 File Offset: 0x0013E200
		public static string txtComplianceAdjustFilters
		{
			get
			{
				return Resources.ResourceManager.GetString("txtComplianceAdjustFilters", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E7A RID: 3706
		// (get) Token: 0x06003178 RID: 12664 RVA: 0x00140028 File Offset: 0x0013E228
		public static string txtComplianceMode
		{
			get
			{
				return Resources.ResourceManager.GetString("txtComplianceMode", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E7B RID: 3707
		// (get) Token: 0x06003179 RID: 12665 RVA: 0x00140050 File Offset: 0x0013E250
		public static string txtComplianceMsg
		{
			get
			{
				return Resources.ResourceManager.GetString("txtComplianceMsg", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E7C RID: 3708
		// (get) Token: 0x0600317A RID: 12666 RVA: 0x00140078 File Offset: 0x0013E278
		public static string txtComplianceNotPossible
		{
			get
			{
				return Resources.ResourceManager.GetString("txtComplianceNotPossible", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E7D RID: 3709
		// (get) Token: 0x0600317B RID: 12667 RVA: 0x001400A0 File Offset: 0x0013E2A0
		public static string txtConfirmFactoryReset
		{
			get
			{
				return Resources.ResourceManager.GetString("txtConfirmFactoryReset", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E7E RID: 3710
		// (get) Token: 0x0600317C RID: 12668 RVA: 0x001400C8 File Offset: 0x0013E2C8
		public static string txtConfirmLogbookDelete
		{
			get
			{
				return Resources.ResourceManager.GetString("txtConfirmLogbookDelete", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E7F RID: 3711
		// (get) Token: 0x0600317D RID: 12669 RVA: 0x001400F0 File Offset: 0x0013E2F0
		public static string txtConfirmRejection
		{
			get
			{
				return Resources.ResourceManager.GetString("txtConfirmRejection", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E80 RID: 3712
		// (get) Token: 0x0600317E RID: 12670 RVA: 0x00140118 File Offset: 0x0013E318
		public static string txtConnectImages
		{
			get
			{
				return Resources.ResourceManager.GetString("txtConnectImages", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E81 RID: 3713
		// (get) Token: 0x0600317F RID: 12671 RVA: 0x00140140 File Offset: 0x0013E340
		public static string txtConnecting
		{
			get
			{
				return Resources.ResourceManager.GetString("txtConnecting", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E82 RID: 3714
		// (get) Token: 0x06003180 RID: 12672 RVA: 0x00140168 File Offset: 0x0013E368
		public static string txtConnectionAttemptFailed
		{
			get
			{
				return Resources.ResourceManager.GetString("txtConnectionAttemptFailed", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E83 RID: 3715
		// (get) Token: 0x06003181 RID: 12673 RVA: 0x00140190 File Offset: 0x0013E390
		public static string txtConnectionStatus
		{
			get
			{
				return Resources.ResourceManager.GetString("txtConnectionStatus", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E84 RID: 3716
		// (get) Token: 0x06003182 RID: 12674 RVA: 0x001401B8 File Offset: 0x0013E3B8
		public static string txtContactPerson
		{
			get
			{
				return Resources.ResourceManager.GetString("txtContactPerson", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E85 RID: 3717
		// (get) Token: 0x06003183 RID: 12675 RVA: 0x001401E0 File Offset: 0x0013E3E0
		public static string txtContaminatedProduct
		{
			get
			{
				return Resources.ResourceManager.GetString("txtContaminatedProduct", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E86 RID: 3718
		// (get) Token: 0x06003184 RID: 12676 RVA: 0x00140208 File Offset: 0x0013E408
		public static string txtContamination
		{
			get
			{
				return Resources.ResourceManager.GetString("txtContamination", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E87 RID: 3719
		// (get) Token: 0x06003185 RID: 12677 RVA: 0x00140230 File Offset: 0x0013E430
		public static string txtContaminationDetected
		{
			get
			{
				return Resources.ResourceManager.GetString("txtContaminationDetected", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E88 RID: 3720
		// (get) Token: 0x06003186 RID: 12678 RVA: 0x00140258 File Offset: 0x0013E458
		public static string txtContinue
		{
			get
			{
				return Resources.ResourceManager.GetString("txtContinue", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E89 RID: 3721
		// (get) Token: 0x06003187 RID: 12679 RVA: 0x00140280 File Offset: 0x0013E480
		public static string txtContinueValidation
		{
			get
			{
				return Resources.ResourceManager.GetString("txtContinueValidation", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E8A RID: 3722
		// (get) Token: 0x06003188 RID: 12680 RVA: 0x001402A8 File Offset: 0x0013E4A8
		public static string txtConveyContaminantProduct
		{
			get
			{
				return Resources.ResourceManager.GetString("txtConveyContaminantProduct", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E8B RID: 3723
		// (get) Token: 0x06003189 RID: 12681 RVA: 0x001402D0 File Offset: 0x0013E4D0
		public static string txtConveyCountProduct
		{
			get
			{
				return Resources.ResourceManager.GetString("txtConveyCountProduct", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E8C RID: 3724
		// (get) Token: 0x0600318A RID: 12682 RVA: 0x001402F8 File Offset: 0x0013E4F8
		public static string txtConveyingDirection
		{
			get
			{
				return Resources.ResourceManager.GetString("txtConveyingDirection", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E8D RID: 3725
		// (get) Token: 0x0600318B RID: 12683 RVA: 0x00140320 File Offset: 0x0013E520
		public static string txtConveyor
		{
			get
			{
				return Resources.ResourceManager.GetString("txtConveyor", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E8E RID: 3726
		// (get) Token: 0x0600318C RID: 12684 RVA: 0x00140348 File Offset: 0x0013E548
		public static string txtConveyorDistance
		{
			get
			{
				return Resources.ResourceManager.GetString("txtConveyorDistance", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E8F RID: 3727
		// (get) Token: 0x0600318D RID: 12685 RVA: 0x00140370 File Offset: 0x0013E570
		public static string txtConveyorSpeed
		{
			get
			{
				return Resources.ResourceManager.GetString("txtConveyorSpeed", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E90 RID: 3728
		// (get) Token: 0x0600318E RID: 12686 RVA: 0x00140398 File Offset: 0x0013E598
		public static string txtConveyProduct
		{
			get
			{
				return Resources.ResourceManager.GetString("txtConveyProduct", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E91 RID: 3729
		// (get) Token: 0x0600318F RID: 12687 RVA: 0x001403C0 File Offset: 0x0013E5C0
		public static string txtConveyWeightProduct
		{
			get
			{
				return Resources.ResourceManager.GetString("txtConveyWeightProduct", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E92 RID: 3730
		// (get) Token: 0x06003190 RID: 12688 RVA: 0x001403E8 File Offset: 0x0013E5E8
		public static string txtCount
		{
			get
			{
				return Resources.ResourceManager.GetString("txtCount", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E93 RID: 3731
		// (get) Token: 0x06003191 RID: 12689 RVA: 0x00140410 File Offset: 0x0013E610
		public static string txtCountContaminations
		{
			get
			{
				return Resources.ResourceManager.GetString("txtCountContaminations", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E94 RID: 3732
		// (get) Token: 0x06003192 RID: 12690 RVA: 0x00140438 File Offset: 0x0013E638
		public static string txtCountDeactivated
		{
			get
			{
				return Resources.ResourceManager.GetString("txtCountDeactivated", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E95 RID: 3733
		// (get) Token: 0x06003193 RID: 12691 RVA: 0x00140460 File Offset: 0x0013E660
		public static string txtCountRight
		{
			get
			{
				return Resources.ResourceManager.GetString("txtCountRight", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E96 RID: 3734
		// (get) Token: 0x06003194 RID: 12692 RVA: 0x00140488 File Offset: 0x0013E688
		public static string txtCountsPhotogatingSensor
		{
			get
			{
				return Resources.ResourceManager.GetString("txtCountsPhotogatingSensor", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E97 RID: 3735
		// (get) Token: 0x06003195 RID: 12693 RVA: 0x001404B0 File Offset: 0x0013E6B0
		public static string txtCountsRejectCheckSensor
		{
			get
			{
				return Resources.ResourceManager.GetString("txtCountsRejectCheckSensor", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E98 RID: 3736
		// (get) Token: 0x06003196 RID: 12694 RVA: 0x001404D8 File Offset: 0x0013E6D8
		public static string txtCountsRejectVerifySensor
		{
			get
			{
				return Resources.ResourceManager.GetString("txtCountsRejectVerifySensor", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E99 RID: 3737
		// (get) Token: 0x06003197 RID: 12695 RVA: 0x00140500 File Offset: 0x0013E700
		public static string txtCrushingHazard
		{
			get
			{
				return Resources.ResourceManager.GetString("txtCrushingHazard", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E9A RID: 3738
		// (get) Token: 0x06003198 RID: 12696 RVA: 0x00140528 File Offset: 0x0013E728
		public static string txtCurrentBatchDeleteForbidden
		{
			get
			{
				return Resources.ResourceManager.GetString("txtCurrentBatchDeleteForbidden", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E9B RID: 3739
		// (get) Token: 0x06003199 RID: 12697 RVA: 0x00140550 File Offset: 0x0013E750
		public static string txtCurrentCount
		{
			get
			{
				return Resources.ResourceManager.GetString("txtCurrentCount", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E9C RID: 3740
		// (get) Token: 0x0600319A RID: 12698 RVA: 0x00140578 File Offset: 0x0013E778
		public static string txtCurrentHeight
		{
			get
			{
				return Resources.ResourceManager.GetString("txtCurrentHeight", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E9D RID: 3741
		// (get) Token: 0x0600319B RID: 12699 RVA: 0x001405A0 File Offset: 0x0013E7A0
		public static string txtCurrentLanguage
		{
			get
			{
				return Resources.ResourceManager.GetString("txtCurrentLanguage", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E9E RID: 3742
		// (get) Token: 0x0600319C RID: 12700 RVA: 0x001405C8 File Offset: 0x0013E7C8
		public static string txtCurrentValue
		{
			get
			{
				return Resources.ResourceManager.GetString("txtCurrentValue", Resources.resourceCulture);
			}
		}

		// Token: 0x17000E9F RID: 3743
		// (get) Token: 0x0600319D RID: 12701 RVA: 0x001405F0 File Offset: 0x0013E7F0
		public static string txtCurrentValues
		{
			get
			{
				return Resources.ResourceManager.GetString("txtCurrentValues", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EA0 RID: 3744
		// (get) Token: 0x0600319E RID: 12702 RVA: 0x00140618 File Offset: 0x0013E818
		public static string txtCurrentWeight
		{
			get
			{
				return Resources.ResourceManager.GetString("txtCurrentWeight", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EA1 RID: 3745
		// (get) Token: 0x0600319F RID: 12703 RVA: 0x00140640 File Offset: 0x0013E840
		public static string txtCurrentWidth
		{
			get
			{
				return Resources.ResourceManager.GetString("txtCurrentWidth", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EA2 RID: 3746
		// (get) Token: 0x060031A0 RID: 12704 RVA: 0x00140668 File Offset: 0x0013E868
		public static string txtDaily
		{
			get
			{
				return Resources.ResourceManager.GetString("txtDaily", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EA3 RID: 3747
		// (get) Token: 0x060031A1 RID: 12705 RVA: 0x00140690 File Offset: 0x0013E890
		public static string txtData
		{
			get
			{
				return Resources.ResourceManager.GetString("txtData", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EA4 RID: 3748
		// (get) Token: 0x060031A2 RID: 12706 RVA: 0x001406B8 File Offset: 0x0013E8B8
		public static string txtDatabaseError
		{
			get
			{
				return Resources.ResourceManager.GetString("txtDatabaseError", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EA5 RID: 3749
		// (get) Token: 0x060031A3 RID: 12707 RVA: 0x001406E0 File Offset: 0x0013E8E0
		public static string txtDataLoggerMenu
		{
			get
			{
				return Resources.ResourceManager.GetString("txtDataLoggerMenu", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EA6 RID: 3750
		// (get) Token: 0x060031A4 RID: 12708 RVA: 0x00140708 File Offset: 0x0013E908
		public static string txtDateNextDoc
		{
			get
			{
				return Resources.ResourceManager.GetString("txtDateNextDoc", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EA7 RID: 3751
		// (get) Token: 0x060031A5 RID: 12709 RVA: 0x00140730 File Offset: 0x0013E930
		public static string txtDateTooEarly
		{
			get
			{
				return Resources.ResourceManager.GetString("txtDateTooEarly", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EA8 RID: 3752
		// (get) Token: 0x060031A6 RID: 12710 RVA: 0x00140758 File Offset: 0x0013E958
		public static string txtDay
		{
			get
			{
				return Resources.ResourceManager.GetString("txtDay", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EA9 RID: 3753
		// (get) Token: 0x060031A7 RID: 12711 RVA: 0x00140780 File Offset: 0x0013E980
		public static string txtDays
		{
			get
			{
				return Resources.ResourceManager.GetString("txtDays", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EAA RID: 3754
		// (get) Token: 0x060031A8 RID: 12712 RVA: 0x001407A8 File Offset: 0x0013E9A8
		public static string txtDeactivateTestcardSim
		{
			get
			{
				return Resources.ResourceManager.GetString("txtDeactivateTestcardSim", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EAB RID: 3755
		// (get) Token: 0x060031A9 RID: 12713 RVA: 0x001407D0 File Offset: 0x0013E9D0
		public static string txtDebounceTime
		{
			get
			{
				return Resources.ResourceManager.GetString("txtDebounceTime", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EAC RID: 3756
		// (get) Token: 0x060031AA RID: 12714 RVA: 0x001407F8 File Offset: 0x0013E9F8
		public static string txtDecreasesTestcardSim
		{
			get
			{
				return Resources.ResourceManager.GetString("txtDecreasesTestcardSim", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EAD RID: 3757
		// (get) Token: 0x060031AB RID: 12715 RVA: 0x00140820 File Offset: 0x0013EA20
		public static string txtDefaultGateway
		{
			get
			{
				return Resources.ResourceManager.GetString("txtDefaultGateway", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EAE RID: 3758
		// (get) Token: 0x060031AC RID: 12716 RVA: 0x00140848 File Offset: 0x0013EA48
		public static string txtDefectiveModule
		{
			get
			{
				return Resources.ResourceManager.GetString("txtDefectiveModule", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EAF RID: 3759
		// (get) Token: 0x060031AD RID: 12717 RVA: 0x00140870 File Offset: 0x0013EA70
		public static string txtDeformationFactor
		{
			get
			{
				return Resources.ResourceManager.GetString("txtDeformationFactor", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EB0 RID: 3760
		// (get) Token: 0x060031AE RID: 12718 RVA: 0x00140898 File Offset: 0x0013EA98
		public static string txtDelay
		{
			get
			{
				return Resources.ResourceManager.GetString("txtDelay", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EB1 RID: 3761
		// (get) Token: 0x060031AF RID: 12719 RVA: 0x001408C0 File Offset: 0x0013EAC0
		public static string txtDeleteDocuments
		{
			get
			{
				return Resources.ResourceManager.GetString("txtDeleteDocuments", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EB2 RID: 3762
		// (get) Token: 0x060031B0 RID: 12720 RVA: 0x001408E8 File Offset: 0x0013EAE8
		public static string txtDeleteDone
		{
			get
			{
				return Resources.ResourceManager.GetString("txtDeleteDone", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EB3 RID: 3763
		// (get) Token: 0x060031B1 RID: 12721 RVA: 0x00140910 File Offset: 0x0013EB10
		public static string txtDeleteImage
		{
			get
			{
				return Resources.ResourceManager.GetString("txtDeleteImage", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EB4 RID: 3764
		// (get) Token: 0x060031B2 RID: 12722 RVA: 0x00140938 File Offset: 0x0013EB38
		public static string txtDeleteImages
		{
			get
			{
				return Resources.ResourceManager.GetString("txtDeleteImages", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EB5 RID: 3765
		// (get) Token: 0x060031B3 RID: 12723 RVA: 0x00140960 File Offset: 0x0013EB60
		public static string txtDeleteSelected
		{
			get
			{
				return Resources.ResourceManager.GetString("txtDeleteSelected", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EB6 RID: 3766
		// (get) Token: 0x060031B4 RID: 12724 RVA: 0x00140988 File Offset: 0x0013EB88
		public static string txtDeletingImages
		{
			get
			{
				return Resources.ResourceManager.GetString("txtDeletingImages", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EB7 RID: 3767
		// (get) Token: 0x060031B5 RID: 12725 RVA: 0x001409B0 File Offset: 0x0013EBB0
		public static string txtDeletingNotPossible
		{
			get
			{
				return Resources.ResourceManager.GetString("txtDeletingNotPossible", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EB8 RID: 3768
		// (get) Token: 0x060031B6 RID: 12726 RVA: 0x001409D8 File Offset: 0x0013EBD8
		public static string txtDensity
		{
			get
			{
				return Resources.ResourceManager.GetString("txtDensity", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EB9 RID: 3769
		// (get) Token: 0x060031B7 RID: 12727 RVA: 0x00140A00 File Offset: 0x0013EC00
		public static string txtDetectedContaminations
		{
			get
			{
				return Resources.ResourceManager.GetString("txtDetectedContaminations", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EBA RID: 3770
		// (get) Token: 0x060031B8 RID: 12728 RVA: 0x00140A28 File Offset: 0x0013EC28
		public static string txtDetectorReady
		{
			get
			{
				return Resources.ResourceManager.GetString("txtDetectorReady", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EBB RID: 3771
		// (get) Token: 0x060031B9 RID: 12729 RVA: 0x00140A50 File Offset: 0x0013EC50
		public static string txtDeviceData
		{
			get
			{
				return Resources.ResourceManager.GetString("txtDeviceData", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EBC RID: 3772
		// (get) Token: 0x060031BA RID: 12730 RVA: 0x00140A78 File Offset: 0x0013EC78
		public static string txtDifferent
		{
			get
			{
				return Resources.ResourceManager.GetString("txtDifferent", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EBD RID: 3773
		// (get) Token: 0x060031BB RID: 12731 RVA: 0x00140AA0 File Offset: 0x0013ECA0
		public static string txtDisablingAirPressure
		{
			get
			{
				return Resources.ResourceManager.GetString("txtDisablingAirPressure", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EBE RID: 3774
		// (get) Token: 0x060031BC RID: 12732 RVA: 0x00140AC8 File Offset: 0x0013ECC8
		public static string txtDiscardChanges
		{
			get
			{
				return Resources.ResourceManager.GetString("txtDiscardChanges", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EBF RID: 3775
		// (get) Token: 0x060031BD RID: 12733 RVA: 0x00140AF0 File Offset: 0x0013ECF0
		public static string txtDistanceConnectImages
		{
			get
			{
				return Resources.ResourceManager.GetString("txtDistanceConnectImages", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EC0 RID: 3776
		// (get) Token: 0x060031BE RID: 12734 RVA: 0x00140B18 File Offset: 0x0013ED18
		public static string txtDoBackup
		{
			get
			{
				return Resources.ResourceManager.GetString("txtDoBackup", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EC1 RID: 3777
		// (get) Token: 0x060031BF RID: 12735 RVA: 0x00140B40 File Offset: 0x0013ED40
		public static string txtDocumentation
		{
			get
			{
				return Resources.ResourceManager.GetString("txtDocumentation", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EC2 RID: 3778
		// (get) Token: 0x060031C0 RID: 12736 RVA: 0x00140B68 File Offset: 0x0013ED68
		public static string txtDoRestore
		{
			get
			{
				return Resources.ResourceManager.GetString("txtDoRestore", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EC3 RID: 3779
		// (get) Token: 0x060031C1 RID: 12737 RVA: 0x00140B90 File Offset: 0x0013ED90
		public static string txtDrawing
		{
			get
			{
				return Resources.ResourceManager.GetString("txtDrawing", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EC4 RID: 3780
		// (get) Token: 0x060031C2 RID: 12738 RVA: 0x00140BB8 File Offset: 0x0013EDB8
		public static string txtDropSignal
		{
			get
			{
				return Resources.ResourceManager.GetString("txtDropSignal", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EC5 RID: 3781
		// (get) Token: 0x060031C3 RID: 12739 RVA: 0x00140BE0 File Offset: 0x0013EDE0
		public static string txtDuringAudit
		{
			get
			{
				return Resources.ResourceManager.GetString("txtDuringAudit", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EC6 RID: 3782
		// (get) Token: 0x060031C4 RID: 12740 RVA: 0x00140C08 File Offset: 0x0013EE08
		public static string txtDuringIVP
		{
			get
			{
				return Resources.ResourceManager.GetString("txtDuringIVP", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EC7 RID: 3783
		// (get) Token: 0x060031C5 RID: 12741 RVA: 0x00140C30 File Offset: 0x0013EE30
		public static string txtEdgeFilterActive
		{
			get
			{
				return Resources.ResourceManager.GetString("txtEdgeFilterActive", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EC8 RID: 3784
		// (get) Token: 0x060031C6 RID: 12742 RVA: 0x00140C58 File Offset: 0x0013EE58
		public static string txtEjectTime
		{
			get
			{
				return Resources.ResourceManager.GetString("txtEjectTime", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EC9 RID: 3785
		// (get) Token: 0x060031C7 RID: 12743 RVA: 0x00140C80 File Offset: 0x0013EE80
		public static string txtEjectTimeAdditional
		{
			get
			{
				return Resources.ResourceManager.GetString("txtEjectTimeAdditional", Resources.resourceCulture);
			}
		}

		// Token: 0x17000ECA RID: 3786
		// (get) Token: 0x060031C8 RID: 12744 RVA: 0x00140CA8 File Offset: 0x0013EEA8
		public static string txtEjectTimeInit
		{
			get
			{
				return Resources.ResourceManager.GetString("txtEjectTimeInit", Resources.resourceCulture);
			}
		}

		// Token: 0x17000ECB RID: 3787
		// (get) Token: 0x060031C9 RID: 12745 RVA: 0x00140CD0 File Offset: 0x0013EED0
		public static string txtEllipse
		{
			get
			{
				return Resources.ResourceManager.GetString("txtEllipse", Resources.resourceCulture);
			}
		}

		// Token: 0x17000ECC RID: 3788
		// (get) Token: 0x060031CA RID: 12746 RVA: 0x00140CF8 File Offset: 0x0013EEF8
		public static string txtEmail
		{
			get
			{
				return Resources.ResourceManager.GetString("txtEmail", Resources.resourceCulture);
			}
		}

		// Token: 0x17000ECD RID: 3789
		// (get) Token: 0x060031CB RID: 12747 RVA: 0x00140D20 File Offset: 0x0013EF20
		public static string txtEncoderCorrectionTime
		{
			get
			{
				return Resources.ResourceManager.GetString("txtEncoderCorrectionTime", Resources.resourceCulture);
			}
		}

		// Token: 0x17000ECE RID: 3790
		// (get) Token: 0x060031CC RID: 12748 RVA: 0x00140D48 File Offset: 0x0013EF48
		public static string txtEncryptionType
		{
			get
			{
				return Resources.ResourceManager.GetString("txtEncryptionType", Resources.resourceCulture);
			}
		}

		// Token: 0x17000ECF RID: 3791
		// (get) Token: 0x060031CD RID: 12749 RVA: 0x00140D70 File Offset: 0x0013EF70
		public static string txtEndResult
		{
			get
			{
				return Resources.ResourceManager.GetString("txtEndResult", Resources.resourceCulture);
			}
		}

		// Token: 0x17000ED0 RID: 3792
		// (get) Token: 0x060031CE RID: 12750 RVA: 0x00140D98 File Offset: 0x0013EF98
		public static string txtEndTime
		{
			get
			{
				return Resources.ResourceManager.GetString("txtEndTime", Resources.resourceCulture);
			}
		}

		// Token: 0x17000ED1 RID: 3793
		// (get) Token: 0x060031CF RID: 12751 RVA: 0x00140DC0 File Offset: 0x0013EFC0
		public static string txtEnhancedLowDensity
		{
			get
			{
				return Resources.ResourceManager.GetString("txtEnhancedLowDensity", Resources.resourceCulture);
			}
		}

		// Token: 0x17000ED2 RID: 3794
		// (get) Token: 0x060031D0 RID: 12752 RVA: 0x00140DE8 File Offset: 0x0013EFE8
		public static string txtEPDM
		{
			get
			{
				return Resources.ResourceManager.GetString("txtEPDM", Resources.resourceCulture);
			}
		}

		// Token: 0x17000ED3 RID: 3795
		// (get) Token: 0x060031D1 RID: 12753 RVA: 0x00140E10 File Offset: 0x0013F010
		public static string txtErasingFiles
		{
			get
			{
				return Resources.ResourceManager.GetString("txtErasingFiles", Resources.resourceCulture);
			}
		}

		// Token: 0x17000ED4 RID: 3796
		// (get) Token: 0x060031D2 RID: 12754 RVA: 0x00140E38 File Offset: 0x0013F038
		public static string txtError
		{
			get
			{
				return Resources.ResourceManager.GetString("txtError", Resources.resourceCulture);
			}
		}

		// Token: 0x17000ED5 RID: 3797
		// (get) Token: 0x060031D3 RID: 12755 RVA: 0x00140E60 File Offset: 0x0013F060
		public static string txtErrorAnalogInput
		{
			get
			{
				return Resources.ResourceManager.GetString("txtErrorAnalogInput", Resources.resourceCulture);
			}
		}

		// Token: 0x17000ED6 RID: 3798
		// (get) Token: 0x060031D4 RID: 12756 RVA: 0x00140E88 File Offset: 0x0013F088
		public static string txtErrorDigitalOutput
		{
			get
			{
				return Resources.ResourceManager.GetString("txtErrorDigitalOutput", Resources.resourceCulture);
			}
		}

		// Token: 0x17000ED7 RID: 3799
		// (get) Token: 0x060031D5 RID: 12757 RVA: 0x00140EB0 File Offset: 0x0013F0B0
		public static string txtErrorGeneralTestcardSim
		{
			get
			{
				return Resources.ResourceManager.GetString("txtErrorGeneralTestcardSim", Resources.resourceCulture);
			}
		}

		// Token: 0x17000ED8 RID: 3800
		// (get) Token: 0x060031D6 RID: 12758 RVA: 0x00140ED8 File Offset: 0x0013F0D8
		public static string txtErrorLimitXRayOperatingHours
		{
			get
			{
				return Resources.ResourceManager.GetString("txtErrorLimitXRayOperatingHours", Resources.resourceCulture);
			}
		}

		// Token: 0x17000ED9 RID: 3801
		// (get) Token: 0x060031D7 RID: 12759 RVA: 0x00140F00 File Offset: 0x0013F100
		public static string txtErrorSecondaryComm
		{
			get
			{
				return Resources.ResourceManager.GetString("txtErrorSecondaryComm", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EDA RID: 3802
		// (get) Token: 0x060031D8 RID: 12760 RVA: 0x00140F28 File Offset: 0x0013F128
		public static string txtExpectedDuration
		{
			get
			{
				return Resources.ResourceManager.GetString("txtExpectedDuration", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EDB RID: 3803
		// (get) Token: 0x060031D9 RID: 12761 RVA: 0x00140F50 File Offset: 0x0013F150
		public static string txtExternalDrive
		{
			get
			{
				return Resources.ResourceManager.GetString("txtExternalDrive", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EDC RID: 3804
		// (get) Token: 0x060031DA RID: 12762 RVA: 0x00140F78 File Offset: 0x0013F178
		public static string txtExternalDriveAndFTP
		{
			get
			{
				return Resources.ResourceManager.GetString("txtExternalDriveAndFTP", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EDD RID: 3805
		// (get) Token: 0x060031DB RID: 12763 RVA: 0x00140FA0 File Offset: 0x0013F1A0
		public static string txtFailed
		{
			get
			{
				return Resources.ResourceManager.GetString("txtFailed", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EDE RID: 3806
		// (get) Token: 0x060031DC RID: 12764 RVA: 0x00140FC8 File Offset: 0x0013F1C8
		public static string txtFalseCountDetected
		{
			get
			{
				return Resources.ResourceManager.GetString("txtFalseCountDetected", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EDF RID: 3807
		// (get) Token: 0x060031DD RID: 12765 RVA: 0x00140FF0 File Offset: 0x0013F1F0
		public static string txtFalseWeightDetected
		{
			get
			{
				return Resources.ResourceManager.GetString("txtFalseWeightDetected", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EE0 RID: 3808
		// (get) Token: 0x060031DE RID: 12766 RVA: 0x00141018 File Offset: 0x0013F218
		public static string txtFastModeContaminations
		{
			get
			{
				return Resources.ResourceManager.GetString("txtFastModeContaminations", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EE1 RID: 3809
		// (get) Token: 0x060031DF RID: 12767 RVA: 0x00141040 File Offset: 0x0013F240
		public static string txtFE
		{
			get
			{
				return Resources.ResourceManager.GetString("txtFE", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EE2 RID: 3810
		// (get) Token: 0x060031E0 RID: 12768 RVA: 0x00141068 File Offset: 0x0013F268
		public static string txtFilamentCurrent
		{
			get
			{
				return Resources.ResourceManager.GetString("txtFilamentCurrent", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EE3 RID: 3811
		// (get) Token: 0x060031E1 RID: 12769 RVA: 0x00141090 File Offset: 0x0013F290
		public static string txtFileNameLogo
		{
			get
			{
				return Resources.ResourceManager.GetString("txtFileNameLogo", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EE4 RID: 3812
		// (get) Token: 0x060031E2 RID: 12770 RVA: 0x001410B8 File Offset: 0x0013F2B8
		public static string txtFilterSize
		{
			get
			{
				return Resources.ResourceManager.GetString("txtFilterSize", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EE5 RID: 3813
		// (get) Token: 0x060031E3 RID: 12771 RVA: 0x001410E0 File Offset: 0x0013F2E0
		public static string txtFineTuningBaseDataset
		{
			get
			{
				return Resources.ResourceManager.GetString("txtFineTuningBaseDataset", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EE6 RID: 3814
		// (get) Token: 0x060031E4 RID: 12772 RVA: 0x00141108 File Offset: 0x0013F308
		public static string txtFineTuningEnabled
		{
			get
			{
				return Resources.ResourceManager.GetString("txtFineTuningEnabled", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EE7 RID: 3815
		// (get) Token: 0x060031E5 RID: 12773 RVA: 0x00141130 File Offset: 0x0013F330
		public static string txtFineTuningMin
		{
			get
			{
				return Resources.ResourceManager.GetString("txtFineTuningMin", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EE8 RID: 3816
		// (get) Token: 0x060031E6 RID: 12774 RVA: 0x00141158 File Offset: 0x0013F358
		public static string txtFineTuningMissingFile
		{
			get
			{
				return Resources.ResourceManager.GetString("txtFineTuningMissingFile", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EE9 RID: 3817
		// (get) Token: 0x060031E7 RID: 12775 RVA: 0x00141180 File Offset: 0x0013F380
		public static string txtFirstImageHistoryOrNotTeachedIn
		{
			get
			{
				return Resources.ResourceManager.GetString("txtFirstImageHistoryOrNotTeachedIn", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EEA RID: 3818
		// (get) Token: 0x060031E8 RID: 12776 RVA: 0x001411A8 File Offset: 0x0013F3A8
		public static string txtFirstPixelSecondValve
		{
			get
			{
				return Resources.ResourceManager.GetString("txtFirstPixelSecondValve", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EEB RID: 3819
		// (get) Token: 0x060031E9 RID: 12777 RVA: 0x001411D0 File Offset: 0x0013F3D0
		public static string txtFirstValve
		{
			get
			{
				return Resources.ResourceManager.GetString("txtFirstValve", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EEC RID: 3820
		// (get) Token: 0x060031EA RID: 12778 RVA: 0x001411F8 File Offset: 0x0013F3F8
		public static string txtFlapsCount
		{
			get
			{
				return Resources.ResourceManager.GetString("txtFlapsCount", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EED RID: 3821
		// (get) Token: 0x060031EB RID: 12779 RVA: 0x00141220 File Offset: 0x0013F420
		public static string txtFound
		{
			get
			{
				return Resources.ResourceManager.GetString("txtFound", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EEE RID: 3822
		// (get) Token: 0x060031EC RID: 12780 RVA: 0x00141248 File Offset: 0x0013F448
		public static string txtFriday
		{
			get
			{
				return Resources.ResourceManager.GetString("txtFriday", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EEF RID: 3823
		// (get) Token: 0x060031ED RID: 12781 RVA: 0x00141270 File Offset: 0x0013F470
		public static string txtFTP
		{
			get
			{
				return Resources.ResourceManager.GetString("txtFTP", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EF0 RID: 3824
		// (get) Token: 0x060031EE RID: 12782 RVA: 0x00141298 File Offset: 0x0013F498
		public static string txtGlass
		{
			get
			{
				return Resources.ResourceManager.GetString("txtGlass", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EF1 RID: 3825
		// (get) Token: 0x060031EF RID: 12783 RVA: 0x001412C0 File Offset: 0x0013F4C0
		public static string txtGoodAndBadImages
		{
			get
			{
				return Resources.ResourceManager.GetString("txtGoodAndBadImages", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EF2 RID: 3826
		// (get) Token: 0x060031F0 RID: 12784 RVA: 0x001412E8 File Offset: 0x0013F4E8
		public static string txtGoodImages
		{
			get
			{
				return Resources.ResourceManager.GetString("txtGoodImages", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EF3 RID: 3827
		// (get) Token: 0x060031F1 RID: 12785 RVA: 0x00141310 File Offset: 0x0013F510
		public static string txtHardwareBrigeDPlus1Module
		{
			get
			{
				return Resources.ResourceManager.GetString("txtHardwareBrigeDPlus1Module", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EF4 RID: 3828
		// (get) Token: 0x060031F2 RID: 12786 RVA: 0x00141338 File Offset: 0x0013F538
		public static string txtHeight
		{
			get
			{
				return Resources.ResourceManager.GetString("txtHeight", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EF5 RID: 3829
		// (get) Token: 0x060031F3 RID: 12787 RVA: 0x00141360 File Offset: 0x0013F560
		public static string txtHigh
		{
			get
			{
				return Resources.ResourceManager.GetString("txtHigh", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EF6 RID: 3830
		// (get) Token: 0x060031F4 RID: 12788 RVA: 0x00141388 File Offset: 0x0013F588
		public static string txtHighest
		{
			get
			{
				return Resources.ResourceManager.GetString("txtHighest", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EF7 RID: 3831
		// (get) Token: 0x060031F5 RID: 12789 RVA: 0x001413B0 File Offset: 0x0013F5B0
		public static string txtHighThreshold
		{
			get
			{
				return Resources.ResourceManager.GetString("txtHighThreshold", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EF8 RID: 3832
		// (get) Token: 0x060031F6 RID: 12790 RVA: 0x001413D8 File Offset: 0x0013F5D8
		public static string txtHighThroughputMode
		{
			get
			{
				return Resources.ResourceManager.GetString("txtHighThroughputMode", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EF9 RID: 3833
		// (get) Token: 0x060031F7 RID: 12791 RVA: 0x00141400 File Offset: 0x0013F600
		public static string txtHour
		{
			get
			{
				return Resources.ResourceManager.GetString("txtHour", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EFA RID: 3834
		// (get) Token: 0x060031F8 RID: 12792 RVA: 0x00141428 File Offset: 0x0013F628
		public static string txtHourly
		{
			get
			{
				return Resources.ResourceManager.GetString("txtHourly", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EFB RID: 3835
		// (get) Token: 0x060031F9 RID: 12793 RVA: 0x00141450 File Offset: 0x0013F650
		public static string txtICMsuccessful
		{
			get
			{
				return Resources.ResourceManager.GetString("txtICMsuccessful", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EFC RID: 3836
		// (get) Token: 0x060031FA RID: 12794 RVA: 0x00141478 File Offset: 0x0013F678
		public static string txtIfQuestions
		{
			get
			{
				return Resources.ResourceManager.GetString("txtIfQuestions", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EFD RID: 3837
		// (get) Token: 0x060031FB RID: 12795 RVA: 0x001414A0 File Offset: 0x0013F6A0
		public static string txtIfQuestions2
		{
			get
			{
				return Resources.ResourceManager.GetString("txtIfQuestions2", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EFE RID: 3838
		// (get) Token: 0x060031FC RID: 12796 RVA: 0x001414C8 File Offset: 0x0013F6C8
		public static string txtIgnoreClip
		{
			get
			{
				return Resources.ResourceManager.GetString("txtIgnoreClip", Resources.resourceCulture);
			}
		}

		// Token: 0x17000EFF RID: 3839
		// (get) Token: 0x060031FD RID: 12797 RVA: 0x001414F0 File Offset: 0x0013F6F0
		public static string txtImage
		{
			get
			{
				return Resources.ResourceManager.GetString("txtImage", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F00 RID: 3840
		// (get) Token: 0x060031FE RID: 12798 RVA: 0x00141518 File Offset: 0x0013F718
		public static string txtImageForSingleEval
		{
			get
			{
				return Resources.ResourceManager.GetString("txtImageForSingleEval", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F01 RID: 3841
		// (get) Token: 0x060031FF RID: 12799 RVA: 0x00141540 File Offset: 0x0013F740
		public static string txtImageMode
		{
			get
			{
				return Resources.ResourceManager.GetString("txtImageMode", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F02 RID: 3842
		// (get) Token: 0x06003200 RID: 12800 RVA: 0x00141568 File Offset: 0x0013F768
		public static string txtImageNotSaved
		{
			get
			{
				return Resources.ResourceManager.GetString("txtImageNotSaved", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F03 RID: 3843
		// (get) Token: 0x06003201 RID: 12801 RVA: 0x00141590 File Offset: 0x0013F790
		public static string txtImages
		{
			get
			{
				return Resources.ResourceManager.GetString("txtImages", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F04 RID: 3844
		// (get) Token: 0x06003202 RID: 12802 RVA: 0x001415B8 File Offset: 0x0013F7B8
		public static string txtImagesDontExist
		{
			get
			{
				return Resources.ResourceManager.GetString("txtImagesDontExist", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F05 RID: 3845
		// (get) Token: 0x06003203 RID: 12803 RVA: 0x001415E0 File Offset: 0x0013F7E0
		public static string txtImageSize
		{
			get
			{
				return Resources.ResourceManager.GetString("txtImageSize", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F06 RID: 3846
		// (get) Token: 0x06003204 RID: 12804 RVA: 0x00141608 File Offset: 0x0013F808
		public static string txtImagesNotAllSaved
		{
			get
			{
				return Resources.ResourceManager.GetString("txtImagesNotAllSaved", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F07 RID: 3847
		// (get) Token: 0x06003205 RID: 12805 RVA: 0x00141630 File Offset: 0x0013F830
		public static string txtImagesSaved
		{
			get
			{
				return Resources.ResourceManager.GetString("txtImagesSaved", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F08 RID: 3848
		// (get) Token: 0x06003206 RID: 12806 RVA: 0x00141658 File Offset: 0x0013F858
		public static string txtImportFromAuditCheck
		{
			get
			{
				return Resources.ResourceManager.GetString("txtImportFromAuditCheck", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F09 RID: 3849
		// (get) Token: 0x06003207 RID: 12807 RVA: 0x00141680 File Offset: 0x0013F880
		public static string txtImportFromIVP
		{
			get
			{
				return Resources.ResourceManager.GetString("txtImportFromIVP", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F0A RID: 3850
		// (get) Token: 0x06003208 RID: 12808 RVA: 0x001416A8 File Offset: 0x0013F8A8
		public static string txtImpurityFinderSettings
		{
			get
			{
				return Resources.ResourceManager.GetString("txtImpurityFinderSettings", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F0B RID: 3851
		// (get) Token: 0x06003209 RID: 12809 RVA: 0x001416D0 File Offset: 0x0013F8D0
		public static string txtInferenceEnabled
		{
			get
			{
				return Resources.ResourceManager.GetString("txtInferenceEnabled", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F0C RID: 3852
		// (get) Token: 0x0600320A RID: 12810 RVA: 0x001416F8 File Offset: 0x0013F8F8
		public static string txtInfo
		{
			get
			{
				return Resources.ResourceManager.GetString("txtInfo", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F0D RID: 3853
		// (get) Token: 0x0600320B RID: 12811 RVA: 0x00141720 File Offset: 0x0013F920
		public static string txtInformationCustomer
		{
			get
			{
				return Resources.ResourceManager.GetString("txtInformationCustomer", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F0E RID: 3854
		// (get) Token: 0x0600320C RID: 12812 RVA: 0x00141748 File Offset: 0x0013F948
		public static string txtInformationManufacturer
		{
			get
			{
				return Resources.ResourceManager.GetString("txtInformationManufacturer", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F0F RID: 3855
		// (get) Token: 0x0600320D RID: 12813 RVA: 0x00141770 File Offset: 0x0013F970
		public static string txtInformationProduct
		{
			get
			{
				return Resources.ResourceManager.GetString("txtInformationProduct", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F10 RID: 3856
		// (get) Token: 0x0600320E RID: 12814 RVA: 0x00141798 File Offset: 0x0013F998
		public static string txtInitSensitivities
		{
			get
			{
				return Resources.ResourceManager.GetString("txtInitSensitivities", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F11 RID: 3857
		// (get) Token: 0x0600320F RID: 12815 RVA: 0x001417C0 File Offset: 0x0013F9C0
		public static string txtInitTestcardSim
		{
			get
			{
				return Resources.ResourceManager.GetString("txtInitTestcardSim", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F12 RID: 3858
		// (get) Token: 0x06003210 RID: 12816 RVA: 0x001417E8 File Offset: 0x0013F9E8
		public static string txtInletSensorBlocked
		{
			get
			{
				return Resources.ResourceManager.GetString("txtInletSensorBlocked", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F13 RID: 3859
		// (get) Token: 0x06003211 RID: 12817 RVA: 0x00141810 File Offset: 0x0013FA10
		public static string txtINNumber
		{
			get
			{
				return Resources.ResourceManager.GetString("txtINNumber", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F14 RID: 3860
		// (get) Token: 0x06003212 RID: 12818 RVA: 0x00141838 File Offset: 0x0013FA38
		public static string txtInOutCheck
		{
			get
			{
				return Resources.ResourceManager.GetString("txtInOutCheck", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F15 RID: 3861
		// (get) Token: 0x06003213 RID: 12819 RVA: 0x00141860 File Offset: 0x0013FA60
		public static string txtInsertID
		{
			get
			{
				return Resources.ResourceManager.GetString("txtInsertID", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F16 RID: 3862
		// (get) Token: 0x06003214 RID: 12820 RVA: 0x00141888 File Offset: 0x0013FA88
		public static string txtInsightNetConnection
		{
			get
			{
				return Resources.ResourceManager.GetString("txtInsightNetConnection", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F17 RID: 3863
		// (get) Token: 0x06003215 RID: 12821 RVA: 0x001418B0 File Offset: 0x0013FAB0
		public static string txtInspectionIsRunning
		{
			get
			{
				return Resources.ResourceManager.GetString("txtInspectionIsRunning", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F18 RID: 3864
		// (get) Token: 0x06003216 RID: 12822 RVA: 0x001418D8 File Offset: 0x0013FAD8
		public static string txtInspectionPauses
		{
			get
			{
				return Resources.ResourceManager.GetString("txtInspectionPauses", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F19 RID: 3865
		// (get) Token: 0x06003217 RID: 12823 RVA: 0x00141900 File Offset: 0x0013FB00
		public static string txtInterval
		{
			get
			{
				return Resources.ResourceManager.GetString("txtInterval", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F1A RID: 3866
		// (get) Token: 0x06003218 RID: 12824 RVA: 0x00141928 File Offset: 0x0013FB28
		public static string txtIntervallInMonths
		{
			get
			{
				return Resources.ResourceManager.GetString("txtIntervallInMonths", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F1B RID: 3867
		// (get) Token: 0x06003219 RID: 12825 RVA: 0x00141950 File Offset: 0x0013FB50
		public static string txtIOsCount
		{
			get
			{
				return Resources.ResourceManager.GetString("txtIOsCount", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F1C RID: 3868
		// (get) Token: 0x0600321A RID: 12826 RVA: 0x00141978 File Offset: 0x0013FB78
		public static string txtIPAddress
		{
			get
			{
				return Resources.ResourceManager.GetString("txtIPAddress", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F1D RID: 3869
		// (get) Token: 0x0600321B RID: 12827 RVA: 0x001419A0 File Offset: 0x0013FBA0
		public static string txtIVPActivated
		{
			get
			{
				return Resources.ResourceManager.GetString("txtIVPActivated", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F1E RID: 3870
		// (get) Token: 0x0600321C RID: 12828 RVA: 0x001419C8 File Offset: 0x0013FBC8
		public static string txtIVPCancelled
		{
			get
			{
				return Resources.ResourceManager.GetString("txtIVPCancelled", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F1F RID: 3871
		// (get) Token: 0x0600321D RID: 12829 RVA: 0x001419F0 File Offset: 0x0013FBF0
		public static string txtIVPDeactivated
		{
			get
			{
				return Resources.ResourceManager.GetString("txtIVPDeactivated", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F20 RID: 3872
		// (get) Token: 0x0600321E RID: 12830 RVA: 0x00141A18 File Offset: 0x0013FC18
		public static string txtIVPFailed
		{
			get
			{
				return Resources.ResourceManager.GetString("txtIVPFailed", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F21 RID: 3873
		// (get) Token: 0x0600321F RID: 12831 RVA: 0x00141A40 File Offset: 0x0013FC40
		public static string txtIVPNeeded
		{
			get
			{
				return Resources.ResourceManager.GetString("txtIVPNeeded", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F22 RID: 3874
		// (get) Token: 0x06003220 RID: 12832 RVA: 0x00141A68 File Offset: 0x0013FC68
		public static string txtIVPStarted
		{
			get
			{
				return Resources.ResourceManager.GetString("txtIVPStarted", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F23 RID: 3875
		// (get) Token: 0x06003221 RID: 12833 RVA: 0x00141A90 File Offset: 0x0013FC90
		public static string txtIVPTestballFinished
		{
			get
			{
				return Resources.ResourceManager.GetString("txtIVPTestballFinished", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F24 RID: 3876
		// (get) Token: 0x06003222 RID: 12834 RVA: 0x00141AB8 File Offset: 0x0013FCB8
		public static string txtKeramik
		{
			get
			{
				return Resources.ResourceManager.GetString("txtKeramik", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F25 RID: 3877
		// (get) Token: 0x06003223 RID: 12835 RVA: 0x00141AE0 File Offset: 0x0013FCE0
		public static string txtLane
		{
			get
			{
				return Resources.ResourceManager.GetString("txtLane", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F26 RID: 3878
		// (get) Token: 0x06003224 RID: 12836 RVA: 0x00141B08 File Offset: 0x0013FD08
		public static string txtLastPixelNextToLastValve
		{
			get
			{
				return Resources.ResourceManager.GetString("txtLastPixelNextToLastValve", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F27 RID: 3879
		// (get) Token: 0x06003225 RID: 12837 RVA: 0x00141B30 File Offset: 0x0013FD30
		public static string txtLastValidation
		{
			get
			{
				return Resources.ResourceManager.GetString("txtLastValidation", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F28 RID: 3880
		// (get) Token: 0x06003226 RID: 12838 RVA: 0x00141B58 File Offset: 0x0013FD58
		public static string txtLastValve
		{
			get
			{
				return Resources.ResourceManager.GetString("txtLastValve", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F29 RID: 3881
		// (get) Token: 0x06003227 RID: 12839 RVA: 0x00141B80 File Offset: 0x0013FD80
		public static string txtLength
		{
			get
			{
				return Resources.ResourceManager.GetString("txtLength", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F2A RID: 3882
		// (get) Token: 0x06003228 RID: 12840 RVA: 0x00141BA8 File Offset: 0x0013FDA8
		public static string txtLengthProduct
		{
			get
			{
				return Resources.ResourceManager.GetString("txtLengthProduct", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F2B RID: 3883
		// (get) Token: 0x06003229 RID: 12841 RVA: 0x00141BD0 File Offset: 0x0013FDD0
		public static string txtLimit100ExternalStorage
		{
			get
			{
				return Resources.ResourceManager.GetString("txtLimit100ExternalStorage", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F2C RID: 3884
		// (get) Token: 0x0600322A RID: 12842 RVA: 0x00141BF8 File Offset: 0x0013FDF8
		public static string txtLimit90ExternalStorage
		{
			get
			{
				return Resources.ResourceManager.GetString("txtLimit90ExternalStorage", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F2D RID: 3885
		// (get) Token: 0x0600322B RID: 12843 RVA: 0x00141C20 File Offset: 0x0013FE20
		public static string txtLimit90PercReached
		{
			get
			{
				return Resources.ResourceManager.GetString("txtLimit90PercReached", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F2E RID: 3886
		// (get) Token: 0x0600322C RID: 12844 RVA: 0x00141C48 File Offset: 0x0013FE48
		public static string txtLimitMonitoring
		{
			get
			{
				return Resources.ResourceManager.GetString("txtLimitMonitoring", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F2F RID: 3887
		// (get) Token: 0x0600322D RID: 12845 RVA: 0x00141C70 File Offset: 0x0013FE70
		public static string txtLimitReached
		{
			get
			{
				return Resources.ResourceManager.GetString("txtLimitReached", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F30 RID: 3888
		// (get) Token: 0x0600322E RID: 12846 RVA: 0x00141C98 File Offset: 0x0013FE98
		public static string txtLimits
		{
			get
			{
				return Resources.ResourceManager.GetString("txtLimits", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F31 RID: 3889
		// (get) Token: 0x0600322F RID: 12847 RVA: 0x00141CC0 File Offset: 0x0013FEC0
		public static string txtLimitXRayOperatingHours
		{
			get
			{
				return Resources.ResourceManager.GetString("txtLimitXRayOperatingHours", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F32 RID: 3890
		// (get) Token: 0x06003230 RID: 12848 RVA: 0x00141CE8 File Offset: 0x0013FEE8
		public static string txtLineName
		{
			get
			{
				return Resources.ResourceManager.GetString("txtLineName", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F33 RID: 3891
		// (get) Token: 0x06003231 RID: 12849 RVA: 0x00141D10 File Offset: 0x0013FF10
		public static string txtLocation
		{
			get
			{
				return Resources.ResourceManager.GetString("txtLocation", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F34 RID: 3892
		// (get) Token: 0x06003232 RID: 12850 RVA: 0x00141D38 File Offset: 0x0013FF38
		public static string txtLogData
		{
			get
			{
				return Resources.ResourceManager.GetString("txtLogData", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F35 RID: 3893
		// (get) Token: 0x06003233 RID: 12851 RVA: 0x00141D60 File Offset: 0x0013FF60
		public static string txtLow
		{
			get
			{
				return Resources.ResourceManager.GetString("txtLow", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F36 RID: 3894
		// (get) Token: 0x06003234 RID: 12852 RVA: 0x00141D88 File Offset: 0x0013FF88
		public static string txtLowerMaterialCurve
		{
			get
			{
				return Resources.ResourceManager.GetString("txtLowerMaterialCurve", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F37 RID: 3895
		// (get) Token: 0x06003235 RID: 12853 RVA: 0x00141DB0 File Offset: 0x0013FFB0
		public static string txtLowest
		{
			get
			{
				return Resources.ResourceManager.GetString("txtLowest", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F38 RID: 3896
		// (get) Token: 0x06003236 RID: 12854 RVA: 0x00141DD8 File Offset: 0x0013FFD8
		public static string txtLowThreshold
		{
			get
			{
				return Resources.ResourceManager.GetString("txtLowThreshold", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F39 RID: 3897
		// (get) Token: 0x06003237 RID: 12855 RVA: 0x00141E00 File Offset: 0x00140000
		public static string txtMachine
		{
			get
			{
				return Resources.ResourceManager.GetString("txtMachine", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F3A RID: 3898
		// (get) Token: 0x06003238 RID: 12856 RVA: 0x00141E28 File Offset: 0x00140028
		public static string txtMachineName
		{
			get
			{
				return Resources.ResourceManager.GetString("txtMachineName", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F3B RID: 3899
		// (get) Token: 0x06003239 RID: 12857 RVA: 0x00141E50 File Offset: 0x00140050
		public static string txtMachineSubType
		{
			get
			{
				return Resources.ResourceManager.GetString("txtMachineSubType", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F3C RID: 3900
		// (get) Token: 0x0600323A RID: 12858 RVA: 0x00141E78 File Offset: 0x00140078
		public static string txtMachineType
		{
			get
			{
				return Resources.ResourceManager.GetString("txtMachineType", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F3D RID: 3901
		// (get) Token: 0x0600323B RID: 12859 RVA: 0x00141EA0 File Offset: 0x001400A0
		public static string txtMakeEndAssembly
		{
			get
			{
				return Resources.ResourceManager.GetString("txtMakeEndAssembly", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F3E RID: 3902
		// (get) Token: 0x0600323C RID: 12860 RVA: 0x00141EC8 File Offset: 0x001400C8
		public static string txtMakeSesotecService
		{
			get
			{
				return Resources.ResourceManager.GetString("txtMakeSesotecService", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F3F RID: 3903
		// (get) Token: 0x0600323D RID: 12861 RVA: 0x00141EF0 File Offset: 0x001400F0
		public static string txtMakeValidator
		{
			get
			{
				return Resources.ResourceManager.GetString("txtMakeValidator", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F40 RID: 3904
		// (get) Token: 0x0600323E RID: 12862 RVA: 0x00141F18 File Offset: 0x00140118
		public static string txtManually
		{
			get
			{
				return Resources.ResourceManager.GetString("txtManually", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F41 RID: 3905
		// (get) Token: 0x0600323F RID: 12863 RVA: 0x00141F40 File Offset: 0x00140140
		public static string txtManualMode
		{
			get
			{
				return Resources.ResourceManager.GetString("txtManualMode", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F42 RID: 3906
		// (get) Token: 0x06003240 RID: 12864 RVA: 0x00141F68 File Offset: 0x00140168
		public static string txtMarkedPixel
		{
			get
			{
				return Resources.ResourceManager.GetString("txtMarkedPixel", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F43 RID: 3907
		// (get) Token: 0x06003241 RID: 12865 RVA: 0x00141F90 File Offset: 0x00140190
		public static string txtMaterial
		{
			get
			{
				return Resources.ResourceManager.GetString("txtMaterial", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F44 RID: 3908
		// (get) Token: 0x06003242 RID: 12866 RVA: 0x00141FB8 File Offset: 0x001401B8
		public static string txtMaterialCurve
		{
			get
			{
				return Resources.ResourceManager.GetString("txtMaterialCurve", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F45 RID: 3909
		// (get) Token: 0x06003243 RID: 12867 RVA: 0x00141FE0 File Offset: 0x001401E0
		public static string txtMaterialDensity
		{
			get
			{
				return Resources.ResourceManager.GetString("txtMaterialDensity", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F46 RID: 3910
		// (get) Token: 0x06003244 RID: 12868 RVA: 0x00142008 File Offset: 0x00140208
		public static string txtMaxCount
		{
			get
			{
				return Resources.ResourceManager.GetString("txtMaxCount", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F47 RID: 3911
		// (get) Token: 0x06003245 RID: 12869 RVA: 0x00142030 File Offset: 0x00140230
		public static string txtMaxHeight
		{
			get
			{
				return Resources.ResourceManager.GetString("txtMaxHeight", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F48 RID: 3912
		// (get) Token: 0x06003246 RID: 12870 RVA: 0x00142058 File Offset: 0x00140258
		public static string txtMaxImagesAcquired
		{
			get
			{
				return Resources.ResourceManager.GetString("txtMaxImagesAcquired", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F49 RID: 3913
		// (get) Token: 0x06003247 RID: 12871 RVA: 0x00142080 File Offset: 0x00140280
		public static string txtMaximum
		{
			get
			{
				return Resources.ResourceManager.GetString("txtMaximum", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F4A RID: 3914
		// (get) Token: 0x06003248 RID: 12872 RVA: 0x001420A8 File Offset: 0x001402A8
		public static string txtMaximumNumberPerFolder
		{
			get
			{
				return Resources.ResourceManager.GetString("txtMaximumNumberPerFolder", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F4B RID: 3915
		// (get) Token: 0x06003249 RID: 12873 RVA: 0x001420D0 File Offset: 0x001402D0
		public static string txtMaxObjectLength
		{
			get
			{
				return Resources.ResourceManager.GetString("txtMaxObjectLength", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F4C RID: 3916
		// (get) Token: 0x0600324A RID: 12874 RVA: 0x001420F8 File Offset: 0x001402F8
		public static string txtMaxProductHeight
		{
			get
			{
				return Resources.ResourceManager.GetString("txtMaxProductHeight", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F4D RID: 3917
		// (get) Token: 0x0600324B RID: 12875 RVA: 0x00142120 File Offset: 0x00140320
		public static string txtMaxProductLength
		{
			get
			{
				return Resources.ResourceManager.GetString("txtMaxProductLength", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F4E RID: 3918
		// (get) Token: 0x0600324C RID: 12876 RVA: 0x00142148 File Offset: 0x00140348
		public static string txtMaxProducts
		{
			get
			{
				return Resources.ResourceManager.GetString("txtMaxProducts", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F4F RID: 3919
		// (get) Token: 0x0600324D RID: 12877 RVA: 0x00142170 File Offset: 0x00140370
		public static string txtMaxProductsReached
		{
			get
			{
				return Resources.ResourceManager.GetString("txtMaxProductsReached", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F50 RID: 3920
		// (get) Token: 0x0600324E RID: 12878 RVA: 0x00142198 File Offset: 0x00140398
		public static string txtMaxSize
		{
			get
			{
				return Resources.ResourceManager.GetString("txtMaxSize", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F51 RID: 3921
		// (get) Token: 0x0600324F RID: 12879 RVA: 0x001421C0 File Offset: 0x001403C0
		public static string txtMaxStorageCapacity
		{
			get
			{
				return Resources.ResourceManager.GetString("txtMaxStorageCapacity", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F52 RID: 3922
		// (get) Token: 0x06003250 RID: 12880 RVA: 0x001421E8 File Offset: 0x001403E8
		public static string txtMaxWidth
		{
			get
			{
				return Resources.ResourceManager.GetString("txtMaxWidth", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F53 RID: 3923
		// (get) Token: 0x06003251 RID: 12881 RVA: 0x00142210 File Offset: 0x00140410
		public static string txtMeanValue
		{
			get
			{
				return Resources.ResourceManager.GetString("txtMeanValue", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F54 RID: 3924
		// (get) Token: 0x06003252 RID: 12882 RVA: 0x00142238 File Offset: 0x00140438
		public static string txtMeanValueHELE
		{
			get
			{
				return Resources.ResourceManager.GetString("txtMeanValueHELE", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F55 RID: 3925
		// (get) Token: 0x06003253 RID: 12883 RVA: 0x00142260 File Offset: 0x00140460
		public static string txtMessageLevelMonitoring
		{
			get
			{
				return Resources.ResourceManager.GetString("txtMessageLevelMonitoring", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F56 RID: 3926
		// (get) Token: 0x06003254 RID: 12884 RVA: 0x00142288 File Offset: 0x00140488
		public static string txtMessageLevelMonitoring90Per
		{
			get
			{
				return Resources.ResourceManager.GetString("txtMessageLevelMonitoring90Per", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F57 RID: 3927
		// (get) Token: 0x06003255 RID: 12885 RVA: 0x001422B0 File Offset: 0x001404B0
		public static string txtMessageSafetySwitches
		{
			get
			{
				return Resources.ResourceManager.GetString("txtMessageSafetySwitches", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F58 RID: 3928
		// (get) Token: 0x06003256 RID: 12886 RVA: 0x001422D8 File Offset: 0x001404D8
		public static string txtMetal
		{
			get
			{
				return Resources.ResourceManager.GetString("txtMetal", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F59 RID: 3929
		// (get) Token: 0x06003257 RID: 12887 RVA: 0x00142300 File Offset: 0x00140500
		public static string txtMinCount
		{
			get
			{
				return Resources.ResourceManager.GetString("txtMinCount", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F5A RID: 3930
		// (get) Token: 0x06003258 RID: 12888 RVA: 0x00142328 File Offset: 0x00140528
		public static string txtMinHeight
		{
			get
			{
				return Resources.ResourceManager.GetString("txtMinHeight", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F5B RID: 3931
		// (get) Token: 0x06003259 RID: 12889 RVA: 0x00142350 File Offset: 0x00140550
		public static string txtMinimum
		{
			get
			{
				return Resources.ResourceManager.GetString("txtMinimum", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F5C RID: 3932
		// (get) Token: 0x0600325A RID: 12890 RVA: 0x00142378 File Offset: 0x00140578
		public static string txtMinObjectLength
		{
			get
			{
				return Resources.ResourceManager.GetString("txtMinObjectLength", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F5D RID: 3933
		// (get) Token: 0x0600325B RID: 12891 RVA: 0x001423A0 File Offset: 0x001405A0
		public static string txtMinProductHeight
		{
			get
			{
				return Resources.ResourceManager.GetString("txtMinProductHeight", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F5E RID: 3934
		// (get) Token: 0x0600325C RID: 12892 RVA: 0x001423C8 File Offset: 0x001405C8
		public static string txtMinSize
		{
			get
			{
				return Resources.ResourceManager.GetString("txtMinSize", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F5F RID: 3935
		// (get) Token: 0x0600325D RID: 12893 RVA: 0x001423F0 File Offset: 0x001405F0
		public static string txtMinute
		{
			get
			{
				return Resources.ResourceManager.GetString("txtMinute", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F60 RID: 3936
		// (get) Token: 0x0600325E RID: 12894 RVA: 0x00142418 File Offset: 0x00140618
		public static string txtMinWidth
		{
			get
			{
				return Resources.ResourceManager.GetString("txtMinWidth", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F61 RID: 3937
		// (get) Token: 0x0600325F RID: 12895 RVA: 0x00142440 File Offset: 0x00140640
		public static string txtMissingProducts
		{
			get
			{
				return Resources.ResourceManager.GetString("txtMissingProducts", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F62 RID: 3938
		// (get) Token: 0x06003260 RID: 12896 RVA: 0x00142468 File Offset: 0x00140668
		public static string txtMode
		{
			get
			{
				return Resources.ResourceManager.GetString("txtMode", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F63 RID: 3939
		// (get) Token: 0x06003261 RID: 12897 RVA: 0x00142490 File Offset: 0x00140690
		public static string txtMonday
		{
			get
			{
				return Resources.ResourceManager.GetString("txtMonday", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F64 RID: 3940
		// (get) Token: 0x06003262 RID: 12898 RVA: 0x001424B8 File Offset: 0x001406B8
		public static string txtMonitoring
		{
			get
			{
				return Resources.ResourceManager.GetString("txtMonitoring", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F65 RID: 3941
		// (get) Token: 0x06003263 RID: 12899 RVA: 0x001424E0 File Offset: 0x001406E0
		public static string txtMonitoringActivated
		{
			get
			{
				return Resources.ResourceManager.GetString("txtMonitoringActivated", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F66 RID: 3942
		// (get) Token: 0x06003264 RID: 12900 RVA: 0x00142508 File Offset: 0x00140708
		public static string txtMonitoringDeactivated
		{
			get
			{
				return Resources.ResourceManager.GetString("txtMonitoringDeactivated", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F67 RID: 3943
		// (get) Token: 0x06003265 RID: 12901 RVA: 0x00142530 File Offset: 0x00140730
		public static string txtMonitoringMessagesOnOff
		{
			get
			{
				return Resources.ResourceManager.GetString("txtMonitoringMessagesOnOff", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F68 RID: 3944
		// (get) Token: 0x06003266 RID: 12902 RVA: 0x00142558 File Offset: 0x00140758
		public static string txtMonitoringPred
		{
			get
			{
				return Resources.ResourceManager.GetString("txtMonitoringPred", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F69 RID: 3945
		// (get) Token: 0x06003267 RID: 12903 RVA: 0x00142580 File Offset: 0x00140780
		public static string txtMonth
		{
			get
			{
				return Resources.ResourceManager.GetString("txtMonth", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F6A RID: 3946
		// (get) Token: 0x06003268 RID: 12904 RVA: 0x001425A8 File Offset: 0x001407A8
		public static string txtNA
		{
			get
			{
				return Resources.ResourceManager.GetString("txtNA", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F6B RID: 3947
		// (get) Token: 0x06003269 RID: 12905 RVA: 0x001425D0 File Offset: 0x001407D0
		public static string txtName
		{
			get
			{
				return Resources.ResourceManager.GetString("txtName", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F6C RID: 3948
		// (get) Token: 0x0600326A RID: 12906 RVA: 0x001425F8 File Offset: 0x001407F8
		public static string txtNetworkPath
		{
			get
			{
				return Resources.ResourceManager.GetString("txtNetworkPath", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F6D RID: 3949
		// (get) Token: 0x0600326B RID: 12907 RVA: 0x00142620 File Offset: 0x00140820
		public static string txtNewAutotrainRequired
		{
			get
			{
				return Resources.ResourceManager.GetString("txtNewAutotrainRequired", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F6E RID: 3950
		// (get) Token: 0x0600326C RID: 12908 RVA: 0x00142648 File Offset: 0x00140848
		public static string txtNewBatch
		{
			get
			{
				return Resources.ResourceManager.GetString("txtNewBatch", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F6F RID: 3951
		// (get) Token: 0x0600326D RID: 12909 RVA: 0x00142670 File Offset: 0x00140870
		public static string txtNewStick
		{
			get
			{
				return Resources.ResourceManager.GetString("txtNewStick", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F70 RID: 3952
		// (get) Token: 0x0600326E RID: 12910 RVA: 0x00142698 File Offset: 0x00140898
		public static string txtNewValidationUser
		{
			get
			{
				return Resources.ResourceManager.GetString("txtNewValidationUser", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F71 RID: 3953
		// (get) Token: 0x0600326F RID: 12911 RVA: 0x001426C0 File Offset: 0x001408C0
		public static string txtNextDate
		{
			get
			{
				return Resources.ResourceManager.GetString("txtNextDate", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F72 RID: 3954
		// (get) Token: 0x06003270 RID: 12912 RVA: 0x001426E8 File Offset: 0x001408E8
		public static string txtNextDoc
		{
			get
			{
				return Resources.ResourceManager.GetString("txtNextDoc", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F73 RID: 3955
		// (get) Token: 0x06003271 RID: 12913 RVA: 0x00142710 File Offset: 0x00140910
		public static string txtNextTestsample
		{
			get
			{
				return Resources.ResourceManager.GetString("txtNextTestsample", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F74 RID: 3956
		// (get) Token: 0x06003272 RID: 12914 RVA: 0x00142738 File Offset: 0x00140938
		public static string txtNFE
		{
			get
			{
				return Resources.ResourceManager.GetString("txtNFE", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F75 RID: 3957
		// (get) Token: 0x06003273 RID: 12915 RVA: 0x00142760 File Offset: 0x00140960
		public static string txtNoAuditActive
		{
			get
			{
				return Resources.ResourceManager.GetString("txtNoAuditActive", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F76 RID: 3958
		// (get) Token: 0x06003274 RID: 12916 RVA: 0x00142788 File Offset: 0x00140988
		public static string txtNoBatchSelected
		{
			get
			{
				return Resources.ResourceManager.GetString("txtNoBatchSelected", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F77 RID: 3959
		// (get) Token: 0x06003275 RID: 12917 RVA: 0x001427B0 File Offset: 0x001409B0
		public static string txtNoBeltStartOnError
		{
			get
			{
				return Resources.ResourceManager.GetString("txtNoBeltStartOnError", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F78 RID: 3960
		// (get) Token: 0x06003276 RID: 12918 RVA: 0x001427D8 File Offset: 0x001409D8
		public static string txtNoCheckpointAvailable
		{
			get
			{
				return Resources.ResourceManager.GetString("txtNoCheckpointAvailable", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F79 RID: 3961
		// (get) Token: 0x06003277 RID: 12919 RVA: 0x00142800 File Offset: 0x00140A00
		public static string txtNoCheckpointSelected
		{
			get
			{
				return Resources.ResourceManager.GetString("txtNoCheckpointSelected", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F7A RID: 3962
		// (get) Token: 0x06003278 RID: 12920 RVA: 0x00142828 File Offset: 0x00140A28
		public static string txtNoComplianceMsg
		{
			get
			{
				return Resources.ResourceManager.GetString("txtNoComplianceMsg", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F7B RID: 3963
		// (get) Token: 0x06003279 RID: 12921 RVA: 0x00142850 File Offset: 0x00140A50
		public static string txtNoCounterSelection
		{
			get
			{
				return Resources.ResourceManager.GetString("txtNoCounterSelection", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F7C RID: 3964
		// (get) Token: 0x0600327A RID: 12922 RVA: 0x00142878 File Offset: 0x00140A78
		public static string txtNoDatabase
		{
			get
			{
				return Resources.ResourceManager.GetString("txtNoDatabase", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F7D RID: 3965
		// (get) Token: 0x0600327B RID: 12923 RVA: 0x001428A0 File Offset: 0x00140AA0
		public static string txtNoExternalConnection
		{
			get
			{
				return Resources.ResourceManager.GetString("txtNoExternalConnection", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F7E RID: 3966
		// (get) Token: 0x0600327C RID: 12924 RVA: 0x001428C8 File Offset: 0x00140AC8
		public static string txtNoImageSelected
		{
			get
			{
				return Resources.ResourceManager.GetString("txtNoImageSelected", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F7F RID: 3967
		// (get) Token: 0x0600327D RID: 12925 RVA: 0x001428F0 File Offset: 0x00140AF0
		public static string txtNoImagesFound
		{
			get
			{
				return Resources.ResourceManager.GetString("txtNoImagesFound", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F80 RID: 3968
		// (get) Token: 0x0600327E RID: 12926 RVA: 0x00142918 File Offset: 0x00140B18
		public static string txtNoImageTransferToExternalConnection
		{
			get
			{
				return Resources.ResourceManager.GetString("txtNoImageTransferToExternalConnection", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F81 RID: 3969
		// (get) Token: 0x0600327F RID: 12927 RVA: 0x00142940 File Offset: 0x00140B40
		public static string txtNoLearnImagesChosen
		{
			get
			{
				return Resources.ResourceManager.GetString("txtNoLearnImagesChosen", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F82 RID: 3970
		// (get) Token: 0x06003280 RID: 12928 RVA: 0x00142968 File Offset: 0x00140B68
		public static string txtNone
		{
			get
			{
				return Resources.ResourceManager.GetString("txtNone", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F83 RID: 3971
		// (get) Token: 0x06003281 RID: 12929 RVA: 0x00142990 File Offset: 0x00140B90
		public static string txtNonFE
		{
			get
			{
				return Resources.ResourceManager.GetString("txtNonFE", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F84 RID: 3972
		// (get) Token: 0x06003282 RID: 12930 RVA: 0x001429B8 File Offset: 0x00140BB8
		public static string txtNoOptionChosen
		{
			get
			{
				return Resources.ResourceManager.GetString("txtNoOptionChosen", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F85 RID: 3973
		// (get) Token: 0x06003283 RID: 12931 RVA: 0x001429E0 File Offset: 0x00140BE0
		public static string txtNoProductAssigned
		{
			get
			{
				return Resources.ResourceManager.GetString("txtNoProductAssigned", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F86 RID: 3974
		// (get) Token: 0x06003284 RID: 12932 RVA: 0x00142A08 File Offset: 0x00140C08
		public static string txtNoSerialNumber
		{
			get
			{
				return Resources.ResourceManager.GetString("txtNoSerialNumber", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F87 RID: 3975
		// (get) Token: 0x06003285 RID: 12933 RVA: 0x00142A30 File Offset: 0x00140C30
		public static string txtNotActive
		{
			get
			{
				return Resources.ResourceManager.GetString("txtNotActive", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F88 RID: 3976
		// (get) Token: 0x06003286 RID: 12934 RVA: 0x00142A58 File Offset: 0x00140C58
		public static string txtNotAllEntriesLoaded
		{
			get
			{
				return Resources.ResourceManager.GetString("txtNotAllEntriesLoaded", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F89 RID: 3977
		// (get) Token: 0x06003287 RID: 12935 RVA: 0x00142A80 File Offset: 0x00140C80
		public static string txtNotAllRetrained
		{
			get
			{
				return Resources.ResourceManager.GetString("txtNotAllRetrained", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F8A RID: 3978
		// (get) Token: 0x06003288 RID: 12936 RVA: 0x00142AA8 File Offset: 0x00140CA8
		public static string txtNotChangedTestcardSim
		{
			get
			{
				return Resources.ResourceManager.GetString("txtNotChangedTestcardSim", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F8B RID: 3979
		// (get) Token: 0x06003289 RID: 12937 RVA: 0x00142AD0 File Offset: 0x00140CD0
		public static string txtNoTestcardsForIVP
		{
			get
			{
				return Resources.ResourceManager.GetString("txtNoTestcardsForIVP", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F8C RID: 3980
		// (get) Token: 0x0600328A RID: 12938 RVA: 0x00142AF8 File Offset: 0x00140CF8
		public static string txtNotPossible
		{
			get
			{
				return Resources.ResourceManager.GetString("txtNotPossible", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F8D RID: 3981
		// (get) Token: 0x0600328B RID: 12939 RVA: 0x00142B20 File Offset: 0x00140D20
		public static string txtNoUSB
		{
			get
			{
				return Resources.ResourceManager.GetString("txtNoUSB", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F8E RID: 3982
		// (get) Token: 0x0600328C RID: 12940 RVA: 0x00142B48 File Offset: 0x00140D48
		public static string txtNoValidImage
		{
			get
			{
				return Resources.ResourceManager.GetString("txtNoValidImage", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F8F RID: 3983
		// (get) Token: 0x0600328D RID: 12941 RVA: 0x00142B70 File Offset: 0x00140D70
		public static string txtNumberContaminations
		{
			get
			{
				return Resources.ResourceManager.GetString("txtNumberContaminations", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F90 RID: 3984
		// (get) Token: 0x0600328E RID: 12942 RVA: 0x00142B98 File Offset: 0x00140D98
		public static string txtNumberContaminationsLess
		{
			get
			{
				return Resources.ResourceManager.GetString("txtNumberContaminationsLess", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F91 RID: 3985
		// (get) Token: 0x0600328F RID: 12943 RVA: 0x00142BC0 File Offset: 0x00140DC0
		public static string txtNumberContaminationsMore
		{
			get
			{
				return Resources.ResourceManager.GetString("txtNumberContaminationsMore", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F92 RID: 3986
		// (get) Token: 0x06003290 RID: 12944 RVA: 0x00142BE8 File Offset: 0x00140DE8
		public static string txtNumberContaminationsRight
		{
			get
			{
				return Resources.ResourceManager.GetString("txtNumberContaminationsRight", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F93 RID: 3987
		// (get) Token: 0x06003291 RID: 12945 RVA: 0x00142C10 File Offset: 0x00140E10
		public static string txtNumberOfBalls
		{
			get
			{
				return Resources.ResourceManager.GetString("txtNumberOfBalls", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F94 RID: 3988
		// (get) Token: 0x06003292 RID: 12946 RVA: 0x00142C38 File Offset: 0x00140E38
		public static string txtNumberOfProducts
		{
			get
			{
				return Resources.ResourceManager.GetString("txtNumberOfProducts", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F95 RID: 3989
		// (get) Token: 0x06003293 RID: 12947 RVA: 0x00142C60 File Offset: 0x00140E60
		public static string txtNumBottomPixel
		{
			get
			{
				return Resources.ResourceManager.GetString("txtNumBottomPixel", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F96 RID: 3990
		// (get) Token: 0x06003294 RID: 12948 RVA: 0x00142C88 File Offset: 0x00140E88
		public static string txtNumContaminations
		{
			get
			{
				return Resources.ResourceManager.GetString("txtNumContaminations", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F97 RID: 3991
		// (get) Token: 0x06003295 RID: 12949 RVA: 0x00142CB0 File Offset: 0x00140EB0
		public static string txtNumEdgePixel
		{
			get
			{
				return Resources.ResourceManager.GetString("txtNumEdgePixel", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F98 RID: 3992
		// (get) Token: 0x06003296 RID: 12950 RVA: 0x00142CD8 File Offset: 0x00140ED8
		public static string txtNumImagesValidation
		{
			get
			{
				return Resources.ResourceManager.GetString("txtNumImagesValidation", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F99 RID: 3993
		// (get) Token: 0x06003297 RID: 12951 RVA: 0x00142D00 File Offset: 0x00140F00
		public static string txtNumLeftPixel
		{
			get
			{
				return Resources.ResourceManager.GetString("txtNumLeftPixel", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F9A RID: 3994
		// (get) Token: 0x06003298 RID: 12952 RVA: 0x00142D28 File Offset: 0x00140F28
		public static string txtNumOfChoosenImg
		{
			get
			{
				return Resources.ResourceManager.GetString("txtNumOfChoosenImg", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F9B RID: 3995
		// (get) Token: 0x06003299 RID: 12953 RVA: 0x00142D50 File Offset: 0x00140F50
		public static string txtNumOfSimImg
		{
			get
			{
				return Resources.ResourceManager.GetString("txtNumOfSimImg", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F9C RID: 3996
		// (get) Token: 0x0600329A RID: 12954 RVA: 0x00142D78 File Offset: 0x00140F78
		public static string txtNumProductsMissing
		{
			get
			{
				return Resources.ResourceManager.GetString("txtNumProductsMissing", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F9D RID: 3997
		// (get) Token: 0x0600329B RID: 12955 RVA: 0x00142DA0 File Offset: 0x00140FA0
		public static string txtNumRightPixel
		{
			get
			{
				return Resources.ResourceManager.GetString("txtNumRightPixel", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F9E RID: 3998
		// (get) Token: 0x0600329C RID: 12956 RVA: 0x00142DC8 File Offset: 0x00140FC8
		public static string txtNumTopPixel
		{
			get
			{
				return Resources.ResourceManager.GetString("txtNumTopPixel", Resources.resourceCulture);
			}
		}

		// Token: 0x17000F9F RID: 3999
		// (get) Token: 0x0600329D RID: 12957 RVA: 0x00142DF0 File Offset: 0x00140FF0
		public static string txtNumValves
		{
			get
			{
				return Resources.ResourceManager.GetString("txtNumValves", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FA0 RID: 4000
		// (get) Token: 0x0600329E RID: 12958 RVA: 0x00142E18 File Offset: 0x00141018
		public static string txtObject
		{
			get
			{
				return Resources.ResourceManager.GetString("txtObject", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FA1 RID: 4001
		// (get) Token: 0x0600329F RID: 12959 RVA: 0x00142E40 File Offset: 0x00141040
		public static string txtObjectClassEnabled
		{
			get
			{
				return Resources.ResourceManager.GetString("txtObjectClassEnabled", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FA2 RID: 4002
		// (get) Token: 0x060032A0 RID: 12960 RVA: 0x00142E68 File Offset: 0x00141068
		public static string txtObjectClassName
		{
			get
			{
				return Resources.ResourceManager.GetString("txtObjectClassName", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FA3 RID: 4003
		// (get) Token: 0x060032A1 RID: 12961 RVA: 0x00142E90 File Offset: 0x00141090
		public static string txtObjectConfThreshold
		{
			get
			{
				return Resources.ResourceManager.GetString("txtObjectConfThreshold", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FA4 RID: 4004
		// (get) Token: 0x060032A2 RID: 12962 RVA: 0x00142EB8 File Offset: 0x001410B8
		public static string txtObjectCounterActive
		{
			get
			{
				return Resources.ResourceManager.GetString("txtObjectCounterActive", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FA5 RID: 4005
		// (get) Token: 0x060032A3 RID: 12963 RVA: 0x00142EE0 File Offset: 0x001410E0
		public static string txtObjectDetectionType
		{
			get
			{
				return Resources.ResourceManager.GetString("txtObjectDetectionType", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FA6 RID: 4006
		// (get) Token: 0x060032A4 RID: 12964 RVA: 0x00142F08 File Offset: 0x00141108
		public static string txtObjectGroupId
		{
			get
			{
				return Resources.ResourceManager.GetString("txtObjectGroupId", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FA7 RID: 4007
		// (get) Token: 0x060032A5 RID: 12965 RVA: 0x00142F30 File Offset: 0x00141130
		public static string txtObjectMinGroupSize
		{
			get
			{
				return Resources.ResourceManager.GetString("txtObjectMinGroupSize", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FA8 RID: 4008
		// (get) Token: 0x060032A6 RID: 12966 RVA: 0x00142F58 File Offset: 0x00141158
		public static string txtObjectTTAEnabled
		{
			get
			{
				return Resources.ResourceManager.GetString("txtObjectTTAEnabled", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FA9 RID: 4009
		// (get) Token: 0x060032A7 RID: 12967 RVA: 0x00142F80 File Offset: 0x00141180
		public static string txtObjectUnmaskForConEnabled
		{
			get
			{
				return Resources.ResourceManager.GetString("txtObjectUnmaskForConEnabled", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FAA RID: 4010
		// (get) Token: 0x060032A8 RID: 12968 RVA: 0x00142FA8 File Offset: 0x001411A8
		public static string txtObjectUnmaskForConMethod
		{
			get
			{
				return Resources.ResourceManager.GetString("txtObjectUnmaskForConMethod", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FAB RID: 4011
		// (get) Token: 0x060032A9 RID: 12969 RVA: 0x00142FD0 File Offset: 0x001411D0
		public static string txtObtainDNSAutomatically
		{
			get
			{
				return Resources.ResourceManager.GetString("txtObtainDNSAutomatically", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FAC RID: 4012
		// (get) Token: 0x060032AA RID: 12970 RVA: 0x00142FF8 File Offset: 0x001411F8
		public static string txtObtainIPAutomatically
		{
			get
			{
				return Resources.ResourceManager.GetString("txtObtainIPAutomatically", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FAD RID: 4013
		// (get) Token: 0x060032AB RID: 12971 RVA: 0x00143020 File Offset: 0x00141220
		public static string txtOffice
		{
			get
			{
				return Resources.ResourceManager.GetString("txtOffice", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FAE RID: 4014
		// (get) Token: 0x060032AC RID: 12972 RVA: 0x00143048 File Offset: 0x00141248
		public static string txtOnOff
		{
			get
			{
				return Resources.ResourceManager.GetString("txtOnOff", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FAF RID: 4015
		// (get) Token: 0x060032AD RID: 12973 RVA: 0x00143070 File Offset: 0x00141270
		public static string txtOnTime
		{
			get
			{
				return Resources.ResourceManager.GetString("txtOnTime", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FB0 RID: 4016
		// (get) Token: 0x060032AE RID: 12974 RVA: 0x00143098 File Offset: 0x00141298
		public static string txtOpen
		{
			get
			{
				return Resources.ResourceManager.GetString("txtOpen", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FB1 RID: 4017
		// (get) Token: 0x060032AF RID: 12975 RVA: 0x001430C0 File Offset: 0x001412C0
		public static string txtOperatingDays
		{
			get
			{
				return Resources.ResourceManager.GetString("txtOperatingDays", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FB2 RID: 4018
		// (get) Token: 0x060032B0 RID: 12976 RVA: 0x001430E8 File Offset: 0x001412E8
		public static string txtOperatingHours
		{
			get
			{
				return Resources.ResourceManager.GetString("txtOperatingHours", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FB3 RID: 4019
		// (get) Token: 0x060032B1 RID: 12977 RVA: 0x00143110 File Offset: 0x00141310
		public static string txtOperatingHoursPerOperatingPoint
		{
			get
			{
				return Resources.ResourceManager.GetString("txtOperatingHoursPerOperatingPoint", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FB4 RID: 4020
		// (get) Token: 0x060032B2 RID: 12978 RVA: 0x00143138 File Offset: 0x00141338
		public static string txtOperatingParameters
		{
			get
			{
				return Resources.ResourceManager.GetString("txtOperatingParameters", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FB5 RID: 4021
		// (get) Token: 0x060032B3 RID: 12979 RVA: 0x00143160 File Offset: 0x00141360
		public static string txtOrderNumber
		{
			get
			{
				return Resources.ResourceManager.GetString("txtOrderNumber", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FB6 RID: 4022
		// (get) Token: 0x060032B4 RID: 12980 RVA: 0x00143188 File Offset: 0x00141388
		public static string txtOther
		{
			get
			{
				return Resources.ResourceManager.GetString("txtOther", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FB7 RID: 4023
		// (get) Token: 0x060032B5 RID: 12981 RVA: 0x001431B0 File Offset: 0x001413B0
		public static string txtOtherMaterial
		{
			get
			{
				return Resources.ResourceManager.GetString("txtOtherMaterial", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FB8 RID: 4024
		// (get) Token: 0x060032B6 RID: 12982 RVA: 0x001431D8 File Offset: 0x001413D8
		public static string txtOutletSensorBlocked
		{
			get
			{
				return Resources.ResourceManager.GetString("txtOutletSensorBlocked", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FB9 RID: 4025
		// (get) Token: 0x060032B7 RID: 12983 RVA: 0x00143200 File Offset: 0x00141400
		public static string txtOverlappingLeft
		{
			get
			{
				return Resources.ResourceManager.GetString("txtOverlappingLeft", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FBA RID: 4026
		// (get) Token: 0x060032B8 RID: 12984 RVA: 0x00143228 File Offset: 0x00141428
		public static string txtOverlappingRight
		{
			get
			{
				return Resources.ResourceManager.GetString("txtOverlappingRight", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FBB RID: 4027
		// (get) Token: 0x060032B9 RID: 12985 RVA: 0x00143250 File Offset: 0x00141450
		public static string txtOverweight
		{
			get
			{
				return Resources.ResourceManager.GetString("txtOverweight", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FBC RID: 4028
		// (get) Token: 0x060032BA RID: 12986 RVA: 0x00143278 File Offset: 0x00141478
		public static string txtOverwriteExisting
		{
			get
			{
				return Resources.ResourceManager.GetString("txtOverwriteExisting", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FBD RID: 4029
		// (get) Token: 0x060032BB RID: 12987 RVA: 0x001432A0 File Offset: 0x001414A0
		public static string txtOverwriteLoginToGoUser
		{
			get
			{
				return Resources.ResourceManager.GetString("txtOverwriteLoginToGoUser", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FBE RID: 4030
		// (get) Token: 0x060032BC RID: 12988 RVA: 0x001432C8 File Offset: 0x001414C8
		public static string txtOverwriteProductAssignment
		{
			get
			{
				return Resources.ResourceManager.GetString("txtOverwriteProductAssignment", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FBF RID: 4031
		// (get) Token: 0x060032BD RID: 12989 RVA: 0x001432F0 File Offset: 0x001414F0
		public static string txtParameterLoadedSuccess
		{
			get
			{
				return Resources.ResourceManager.GetString("txtParameterLoadedSuccess", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FC0 RID: 4032
		// (get) Token: 0x060032BE RID: 12990 RVA: 0x00143318 File Offset: 0x00141518
		public static string txtPassed
		{
			get
			{
				return Resources.ResourceManager.GetString("txtPassed", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FC1 RID: 4033
		// (get) Token: 0x060032BF RID: 12991 RVA: 0x00143340 File Offset: 0x00141540
		public static string txtPassword
		{
			get
			{
				return Resources.ResourceManager.GetString("txtPassword", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FC2 RID: 4034
		// (get) Token: 0x060032C0 RID: 12992 RVA: 0x00143368 File Offset: 0x00141568
		public static string txtPathExternalConnectionTooLong
		{
			get
			{
				return Resources.ResourceManager.GetString("txtPathExternalConnectionTooLong", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FC3 RID: 4035
		// (get) Token: 0x060032C1 RID: 12993 RVA: 0x00143390 File Offset: 0x00141590
		public static string txtPercentileThreshold
		{
			get
			{
				return Resources.ResourceManager.GetString("txtPercentileThreshold", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FC4 RID: 4036
		// (get) Token: 0x060032C2 RID: 12994 RVA: 0x001433B8 File Offset: 0x001415B8
		public static string txtPerDay
		{
			get
			{
				return Resources.ResourceManager.GetString("txtPerDay", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FC5 RID: 4037
		// (get) Token: 0x060032C3 RID: 12995 RVA: 0x001433E0 File Offset: 0x001415E0
		public static string txtPerInterval
		{
			get
			{
				return Resources.ResourceManager.GetString("txtPerInterval", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FC6 RID: 4038
		// (get) Token: 0x060032C4 RID: 12996 RVA: 0x00143408 File Offset: 0x00141608
		public static string txtPerProductChange
		{
			get
			{
				return Resources.ResourceManager.GetString("txtPerProductChange", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FC7 RID: 4039
		// (get) Token: 0x060032C5 RID: 12997 RVA: 0x00143430 File Offset: 0x00141630
		public static string txtPixel
		{
			get
			{
				return Resources.ResourceManager.GetString("txtPixel", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FC8 RID: 4040
		// (get) Token: 0x060032C6 RID: 12998 RVA: 0x00143458 File Offset: 0x00141658
		public static string txtPixelDeltaMax
		{
			get
			{
				return Resources.ResourceManager.GetString("txtPixelDeltaMax", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FC9 RID: 4041
		// (get) Token: 0x060032C7 RID: 12999 RVA: 0x00143480 File Offset: 0x00141680
		public static string txtPixelDeltaMaxPosition
		{
			get
			{
				return Resources.ResourceManager.GetString("txtPixelDeltaMaxPosition", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FCA RID: 4042
		// (get) Token: 0x060032C8 RID: 13000 RVA: 0x001434A8 File Offset: 0x001416A8
		public static string txtPLZ
		{
			get
			{
				return Resources.ResourceManager.GetString("txtPLZ", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FCB RID: 4043
		// (get) Token: 0x060032C9 RID: 13001 RVA: 0x001434D0 File Offset: 0x001416D0
		public static string txtPolygon
		{
			get
			{
				return Resources.ResourceManager.GetString("txtPolygon", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FCC RID: 4044
		// (get) Token: 0x060032CA RID: 13002 RVA: 0x001434F8 File Offset: 0x001416F8
		public static string txtPositionX
		{
			get
			{
				return Resources.ResourceManager.GetString("txtPositionX", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FCD RID: 4045
		// (get) Token: 0x060032CB RID: 13003 RVA: 0x00143520 File Offset: 0x00141720
		public static string txtPositionY
		{
			get
			{
				return Resources.ResourceManager.GetString("txtPositionY", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FCE RID: 4046
		// (get) Token: 0x060032CC RID: 13004 RVA: 0x00143548 File Offset: 0x00141748
		public static string txtPostAIAutotrain
		{
			get
			{
				return Resources.ResourceManager.GetString("txtPostAIAutotrain", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FCF RID: 4047
		// (get) Token: 0x060032CD RID: 13005 RVA: 0x00143570 File Offset: 0x00141770
		public static string txtPostAutotrain
		{
			get
			{
				return Resources.ResourceManager.GetString("txtPostAutotrain", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FD0 RID: 4048
		// (get) Token: 0x060032CE RID: 13006 RVA: 0x00143598 File Offset: 0x00141798
		public static string txtPreAIAutotrain
		{
			get
			{
				return Resources.ResourceManager.GetString("txtPreAIAutotrain", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FD1 RID: 4049
		// (get) Token: 0x060032CF RID: 13007 RVA: 0x001435C0 File Offset: 0x001417C0
		public static string txtPreAutotrain
		{
			get
			{
				return Resources.ResourceManager.GetString("txtPreAutotrain", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FD2 RID: 4050
		// (get) Token: 0x060032D0 RID: 13008 RVA: 0x001435E8 File Offset: 0x001417E8
		public static string txtPreferredDNSServer
		{
			get
			{
				return Resources.ResourceManager.GetString("txtPreferredDNSServer", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FD3 RID: 4051
		// (get) Token: 0x060032D1 RID: 13009 RVA: 0x00143610 File Offset: 0x00141810
		public static string txtPriority
		{
			get
			{
				return Resources.ResourceManager.GetString("txtPriority", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FD4 RID: 4052
		// (get) Token: 0x060032D2 RID: 13010 RVA: 0x00143638 File Offset: 0x00141838
		public static string txtProcessing
		{
			get
			{
				return Resources.ResourceManager.GetString("txtProcessing", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FD5 RID: 4053
		// (get) Token: 0x060032D3 RID: 13011 RVA: 0x00143660 File Offset: 0x00141860
		public static string txtProductAssignment
		{
			get
			{
				return Resources.ResourceManager.GetString("txtProductAssignment", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FD6 RID: 4054
		// (get) Token: 0x060032D4 RID: 13012 RVA: 0x00143688 File Offset: 0x00141888
		public static string txtProductCounter
		{
			get
			{
				return Resources.ResourceManager.GetString("txtProductCounter", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FD7 RID: 4055
		// (get) Token: 0x060032D5 RID: 13013 RVA: 0x001436B0 File Offset: 0x001418B0
		public static string txtProductCounterBad
		{
			get
			{
				return Resources.ResourceManager.GetString("txtProductCounterBad", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FD8 RID: 4056
		// (get) Token: 0x060032D6 RID: 13014 RVA: 0x001436D8 File Offset: 0x001418D8
		public static string txtProductCounterGood
		{
			get
			{
				return Resources.ResourceManager.GetString("txtProductCounterGood", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FD9 RID: 4057
		// (get) Token: 0x060032D7 RID: 13015 RVA: 0x00143700 File Offset: 0x00141900
		public static string txtProductDuringTest
		{
			get
			{
				return Resources.ResourceManager.GetString("txtProductDuringTest", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FDA RID: 4058
		// (get) Token: 0x060032D8 RID: 13016 RVA: 0x00143728 File Offset: 0x00141928
		public static string txtProductEnded
		{
			get
			{
				return Resources.ResourceManager.GetString("txtProductEnded", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FDB RID: 4059
		// (get) Token: 0x060032D9 RID: 13017 RVA: 0x00143750 File Offset: 0x00141950
		public static string txtProductHeight
		{
			get
			{
				return Resources.ResourceManager.GetString("txtProductHeight", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FDC RID: 4060
		// (get) Token: 0x060032DA RID: 13018 RVA: 0x00143778 File Offset: 0x00141978
		public static string txtProductNotLearned
		{
			get
			{
				return Resources.ResourceManager.GetString("txtProductNotLearned", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FDD RID: 4061
		// (get) Token: 0x060032DB RID: 13019 RVA: 0x001437A0 File Offset: 0x001419A0
		public static string txtProductNotTeachedIn
		{
			get
			{
				return Resources.ResourceManager.GetString("txtProductNotTeachedIn", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FDE RID: 4062
		// (get) Token: 0x060032DC RID: 13020 RVA: 0x001437C8 File Offset: 0x001419C8
		public static string txtProductNumber
		{
			get
			{
				return Resources.ResourceManager.GetString("txtProductNumber", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FDF RID: 4063
		// (get) Token: 0x060032DD RID: 13021 RVA: 0x001437F0 File Offset: 0x001419F0
		public static string txtProductSpecificSettings
		{
			get
			{
				return Resources.ResourceManager.GetString("txtProductSpecificSettings", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FE0 RID: 4064
		// (get) Token: 0x060032DE RID: 13022 RVA: 0x00143818 File Offset: 0x00141A18
		public static string txtProductSubGroup
		{
			get
			{
				return Resources.ResourceManager.GetString("txtProductSubGroup", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FE1 RID: 4065
		// (get) Token: 0x060032DF RID: 13023 RVA: 0x00143840 File Offset: 0x00141A40
		public static string txtPTFE
		{
			get
			{
				return Resources.ResourceManager.GetString("txtPTFE", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FE2 RID: 4066
		// (get) Token: 0x060032E0 RID: 13024 RVA: 0x00143868 File Offset: 0x00141A68
		public static string txtRate
		{
			get
			{
				return Resources.ResourceManager.GetString("txtRate", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FE3 RID: 4067
		// (get) Token: 0x060032E1 RID: 13025 RVA: 0x00143890 File Offset: 0x00141A90
		public static string txtRatedConveyorSpeed
		{
			get
			{
				return Resources.ResourceManager.GetString("txtRatedConveyorSpeed", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FE4 RID: 4068
		// (get) Token: 0x060032E2 RID: 13026 RVA: 0x001438B8 File Offset: 0x00141AB8
		public static string txtReallyResetLimits
		{
			get
			{
				return Resources.ResourceManager.GetString("txtReallyResetLimits", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FE5 RID: 4069
		// (get) Token: 0x060032E3 RID: 13027 RVA: 0x001438E0 File Offset: 0x00141AE0
		public static string txtRebootRequired
		{
			get
			{
				return Resources.ResourceManager.GetString("txtRebootRequired", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FE6 RID: 4070
		// (get) Token: 0x060032E4 RID: 13028 RVA: 0x00143908 File Offset: 0x00141B08
		public static string txtRecordImagesStarted
		{
			get
			{
				return Resources.ResourceManager.GetString("txtRecordImagesStarted", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FE7 RID: 4071
		// (get) Token: 0x060032E5 RID: 13029 RVA: 0x00143930 File Offset: 0x00141B30
		public static string txtRecordImagesStopped
		{
			get
			{
				return Resources.ResourceManager.GetString("txtRecordImagesStopped", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FE8 RID: 4072
		// (get) Token: 0x060032E6 RID: 13030 RVA: 0x00143958 File Offset: 0x00141B58
		public static string txtRectangle
		{
			get
			{
				return Resources.ResourceManager.GetString("txtRectangle", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FE9 RID: 4073
		// (get) Token: 0x060032E7 RID: 13031 RVA: 0x00143980 File Offset: 0x00141B80
		public static string txtRedeemImpossible
		{
			get
			{
				return Resources.ResourceManager.GetString("txtRedeemImpossible", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FEA RID: 4074
		// (get) Token: 0x060032E8 RID: 13032 RVA: 0x001439A8 File Offset: 0x00141BA8
		public static string txtReferenceproduct
		{
			get
			{
				return Resources.ResourceManager.GetString("txtReferenceproduct", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FEB RID: 4075
		// (get) Token: 0x060032E9 RID: 13033 RVA: 0x001439D0 File Offset: 0x00141BD0
		public static string txtReject
		{
			get
			{
				return Resources.ResourceManager.GetString("txtReject", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FEC RID: 4076
		// (get) Token: 0x060032EA RID: 13034 RVA: 0x001439F8 File Offset: 0x00141BF8
		public static string txtRejectAll
		{
			get
			{
				return Resources.ResourceManager.GetString("txtRejectAll", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FED RID: 4077
		// (get) Token: 0x060032EB RID: 13035 RVA: 0x00143A20 File Offset: 0x00141C20
		public static string txtRejectBulk
		{
			get
			{
				return Resources.ResourceManager.GetString("txtRejectBulk", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FEE RID: 4078
		// (get) Token: 0x060032EC RID: 13036 RVA: 0x00143A48 File Offset: 0x00141C48
		public static string txtRejectBulkPerMinute
		{
			get
			{
				return Resources.ResourceManager.GetString("txtRejectBulkPerMinute", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FEF RID: 4079
		// (get) Token: 0x060032ED RID: 13037 RVA: 0x00143A70 File Offset: 0x00141C70
		public static string txtRejected
		{
			get
			{
				return Resources.ResourceManager.GetString("txtRejected", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FF0 RID: 4080
		// (get) Token: 0x060032EE RID: 13038 RVA: 0x00143A98 File Offset: 0x00141C98
		public static string txtRejecter
		{
			get
			{
				return Resources.ResourceManager.GetString("txtRejecter", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FF1 RID: 4081
		// (get) Token: 0x060032EF RID: 13039 RVA: 0x00143AC0 File Offset: 0x00141CC0
		public static string txtRejecterDeactivated
		{
			get
			{
				return Resources.ResourceManager.GetString("txtRejecterDeactivated", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FF2 RID: 4082
		// (get) Token: 0x060032F0 RID: 13040 RVA: 0x00143AE8 File Offset: 0x00141CE8
		public static string txtRejecterEnabled
		{
			get
			{
				return Resources.ResourceManager.GetString("txtRejecterEnabled", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FF3 RID: 4083
		// (get) Token: 0x060032F1 RID: 13041 RVA: 0x00143B10 File Offset: 0x00141D10
		public static string txtRejecterIsDisabled
		{
			get
			{
				return Resources.ResourceManager.GetString("txtRejecterIsDisabled", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FF4 RID: 4084
		// (get) Token: 0x060032F2 RID: 13042 RVA: 0x00143B38 File Offset: 0x00141D38
		public static string txtRejectModeCenter
		{
			get
			{
				return Resources.ResourceManager.GetString("txtRejectModeCenter", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FF5 RID: 4085
		// (get) Token: 0x060032F3 RID: 13043 RVA: 0x00143B60 File Offset: 0x00141D60
		public static string txtRejectModeLeadingEdge
		{
			get
			{
				return Resources.ResourceManager.GetString("txtRejectModeLeadingEdge", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FF6 RID: 4086
		// (get) Token: 0x060032F4 RID: 13044 RVA: 0x00143B88 File Offset: 0x00141D88
		public static string txtRejectNotVerified
		{
			get
			{
				return Resources.ResourceManager.GetString("txtRejectNotVerified", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FF7 RID: 4087
		// (get) Token: 0x060032F5 RID: 13045 RVA: 0x00143BB0 File Offset: 0x00141DB0
		public static string txtRejectVerify
		{
			get
			{
				return Resources.ResourceManager.GetString("txtRejectVerify", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FF8 RID: 4088
		// (get) Token: 0x060032F6 RID: 13046 RVA: 0x00143BD8 File Offset: 0x00141DD8
		public static string txtRejectVerifyEnabled
		{
			get
			{
				return Resources.ResourceManager.GetString("txtRejectVerifyEnabled", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FF9 RID: 4089
		// (get) Token: 0x060032F7 RID: 13047 RVA: 0x00143C00 File Offset: 0x00141E00
		public static string txtRejectVerifyLearn
		{
			get
			{
				return Resources.ResourceManager.GetString("txtRejectVerifyLearn", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FFA RID: 4090
		// (get) Token: 0x060032F8 RID: 13048 RVA: 0x00143C28 File Offset: 0x00141E28
		public static string txtRejectVerifySensorBlockedTimeout
		{
			get
			{
				return Resources.ResourceManager.GetString("txtRejectVerifySensorBlockedTimeout", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FFB RID: 4091
		// (get) Token: 0x060032F9 RID: 13049 RVA: 0x00143C50 File Offset: 0x00141E50
		public static string txtRejectVerifyTest
		{
			get
			{
				return Resources.ResourceManager.GetString("txtRejectVerifyTest", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FFC RID: 4092
		// (get) Token: 0x060032FA RID: 13050 RVA: 0x00143C78 File Offset: 0x00141E78
		public static string txtRejectWithAllFlaps
		{
			get
			{
				return Resources.ResourceManager.GetString("txtRejectWithAllFlaps", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FFD RID: 4093
		// (get) Token: 0x060032FB RID: 13051 RVA: 0x00143CA0 File Offset: 0x00141EA0
		public static string txtRemainingProducts
		{
			get
			{
				return Resources.ResourceManager.GetString("txtRemainingProducts", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FFE RID: 4094
		// (get) Token: 0x060032FC RID: 13052 RVA: 0x00143CC8 File Offset: 0x00141EC8
		public static string txtRemainingTime
		{
			get
			{
				return Resources.ResourceManager.GetString("txtRemainingTime", Resources.resourceCulture);
			}
		}

		// Token: 0x17000FFF RID: 4095
		// (get) Token: 0x060032FD RID: 13053 RVA: 0x00143CF0 File Offset: 0x00141EF0
		public static string txtRemoveDefaultProductAssignment
		{
			get
			{
				return Resources.ResourceManager.GetString("txtRemoveDefaultProductAssignment", Resources.resourceCulture);
			}
		}

		// Token: 0x17001000 RID: 4096
		// (get) Token: 0x060032FE RID: 13054 RVA: 0x00143D18 File Offset: 0x00141F18
		public static string txtRenameBatch
		{
			get
			{
				return Resources.ResourceManager.GetString("txtRenameBatch", Resources.resourceCulture);
			}
		}

		// Token: 0x17001001 RID: 4097
		// (get) Token: 0x060032FF RID: 13055 RVA: 0x00143D40 File Offset: 0x00141F40
		public static string txtRepeatPassword
		{
			get
			{
				return Resources.ResourceManager.GetString("txtRepeatPassword", Resources.resourceCulture);
			}
		}

		// Token: 0x17001002 RID: 4098
		// (get) Token: 0x06003300 RID: 13056 RVA: 0x00143D68 File Offset: 0x00141F68
		public static string txtReport
		{
			get
			{
				return Resources.ResourceManager.GetString("txtReport", Resources.resourceCulture);
			}
		}

		// Token: 0x17001003 RID: 4099
		// (get) Token: 0x06003301 RID: 13057 RVA: 0x00143D90 File Offset: 0x00141F90
		public static string txtRequest
		{
			get
			{
				return Resources.ResourceManager.GetString("txtRequest", Resources.resourceCulture);
			}
		}

		// Token: 0x17001004 RID: 4100
		// (get) Token: 0x06003302 RID: 13058 RVA: 0x00143DB8 File Offset: 0x00141FB8
		public static string txtReseasoning
		{
			get
			{
				return Resources.ResourceManager.GetString("txtReseasoning", Resources.resourceCulture);
			}
		}

		// Token: 0x17001005 RID: 4101
		// (get) Token: 0x06003303 RID: 13059 RVA: 0x00143DE0 File Offset: 0x00141FE0
		public static string txtResetAuditCheck
		{
			get
			{
				return Resources.ResourceManager.GetString("txtResetAuditCheck", Resources.resourceCulture);
			}
		}

		// Token: 0x17001006 RID: 4102
		// (get) Token: 0x06003304 RID: 13060 RVA: 0x00143E08 File Offset: 0x00142008
		public static string txtResetConveyorDistance
		{
			get
			{
				return Resources.ResourceManager.GetString("txtResetConveyorDistance", Resources.resourceCulture);
			}
		}

		// Token: 0x17001007 RID: 4103
		// (get) Token: 0x06003305 RID: 13061 RVA: 0x00143E30 File Offset: 0x00142030
		public static string txtResetFiltersToInitSens
		{
			get
			{
				return Resources.ResourceManager.GetString("txtResetFiltersToInitSens", Resources.resourceCulture);
			}
		}

		// Token: 0x17001008 RID: 4104
		// (get) Token: 0x06003306 RID: 13062 RVA: 0x00143E58 File Offset: 0x00142058
		public static string txtResetLimits
		{
			get
			{
				return Resources.ResourceManager.GetString("txtResetLimits", Resources.resourceCulture);
			}
		}

		// Token: 0x17001009 RID: 4105
		// (get) Token: 0x06003307 RID: 13063 RVA: 0x00143E80 File Offset: 0x00142080
		public static string txtResetSelectedCounter
		{
			get
			{
				return Resources.ResourceManager.GetString("txtResetSelectedCounter", Resources.resourceCulture);
			}
		}

		// Token: 0x1700100A RID: 4106
		// (get) Token: 0x06003308 RID: 13064 RVA: 0x00143EA8 File Offset: 0x001420A8
		public static string txtRestartAuditCheck
		{
			get
			{
				return Resources.ResourceManager.GetString("txtRestartAuditCheck", Resources.resourceCulture);
			}
		}

		// Token: 0x1700100B RID: 4107
		// (get) Token: 0x06003309 RID: 13065 RVA: 0x00143ED0 File Offset: 0x001420D0
		public static string txtRestartNeeded
		{
			get
			{
				return Resources.ResourceManager.GetString("txtRestartNeeded", Resources.resourceCulture);
			}
		}

		// Token: 0x1700100C RID: 4108
		// (get) Token: 0x0600330A RID: 13066 RVA: 0x00143EF8 File Offset: 0x001420F8
		public static string txtRestoreAIImages
		{
			get
			{
				return Resources.ResourceManager.GetString("txtRestoreAIImages", Resources.resourceCulture);
			}
		}

		// Token: 0x1700100D RID: 4109
		// (get) Token: 0x0600330B RID: 13067 RVA: 0x00143F20 File Offset: 0x00142120
		public static string txtRestoreBackupFailed
		{
			get
			{
				return Resources.ResourceManager.GetString("txtRestoreBackupFailed", Resources.resourceCulture);
			}
		}

		// Token: 0x1700100E RID: 4110
		// (get) Token: 0x0600330C RID: 13068 RVA: 0x00143F48 File Offset: 0x00142148
		public static string txtRestoreDatabase
		{
			get
			{
				return Resources.ResourceManager.GetString("txtRestoreDatabase", Resources.resourceCulture);
			}
		}

		// Token: 0x1700100F RID: 4111
		// (get) Token: 0x0600330D RID: 13069 RVA: 0x00143F70 File Offset: 0x00142170
		public static string txtRestoreFailed
		{
			get
			{
				return Resources.ResourceManager.GetString("txtRestoreFailed", Resources.resourceCulture);
			}
		}

		// Token: 0x17001010 RID: 4112
		// (get) Token: 0x0600330E RID: 13070 RVA: 0x00143F98 File Offset: 0x00142198
		public static string txtRestoreGui
		{
			get
			{
				return Resources.ResourceManager.GetString("txtRestoreGui", Resources.resourceCulture);
			}
		}

		// Token: 0x17001011 RID: 4113
		// (get) Token: 0x0600330F RID: 13071 RVA: 0x00143FC0 File Offset: 0x001421C0
		public static string txtRestoreImageProcessing
		{
			get
			{
				return Resources.ResourceManager.GetString("txtRestoreImageProcessing", Resources.resourceCulture);
			}
		}

		// Token: 0x17001012 RID: 4114
		// (get) Token: 0x06003310 RID: 13072 RVA: 0x00143FE8 File Offset: 0x001421E8
		public static string txtRestoreProductParameters
		{
			get
			{
				return Resources.ResourceManager.GetString("txtRestoreProductParameters", Resources.resourceCulture);
			}
		}

		// Token: 0x17001013 RID: 4115
		// (get) Token: 0x06003311 RID: 13073 RVA: 0x00144010 File Offset: 0x00142210
		public static string txtRestoreSuccess
		{
			get
			{
				return Resources.ResourceManager.GetString("txtRestoreSuccess", Resources.resourceCulture);
			}
		}

		// Token: 0x17001014 RID: 4116
		// (get) Token: 0x06003312 RID: 13074 RVA: 0x00144038 File Offset: 0x00142238
		public static string txtRestoreSystem
		{
			get
			{
				return Resources.ResourceManager.GetString("txtRestoreSystem", Resources.resourceCulture);
			}
		}

		// Token: 0x17001015 RID: 4117
		// (get) Token: 0x06003313 RID: 13075 RVA: 0x00144060 File Offset: 0x00142260
		public static string txtRestoreThinkXA
		{
			get
			{
				return Resources.ResourceManager.GetString("txtRestoreThinkXA", Resources.resourceCulture);
			}
		}

		// Token: 0x17001016 RID: 4118
		// (get) Token: 0x06003314 RID: 13076 RVA: 0x00144088 File Offset: 0x00142288
		public static string txtRestoreThinkXO
		{
			get
			{
				return Resources.ResourceManager.GetString("txtRestoreThinkXO", Resources.resourceCulture);
			}
		}

		// Token: 0x17001017 RID: 4119
		// (get) Token: 0x06003315 RID: 13077 RVA: 0x001440B0 File Offset: 0x001422B0
		public static string txtResult
		{
			get
			{
				return Resources.ResourceManager.GetString("txtResult", Resources.resourceCulture);
			}
		}

		// Token: 0x17001018 RID: 4120
		// (get) Token: 0x06003316 RID: 13078 RVA: 0x001440D8 File Offset: 0x001422D8
		public static string txtResume
		{
			get
			{
				return Resources.ResourceManager.GetString("txtResume", Resources.resourceCulture);
			}
		}

		// Token: 0x17001019 RID: 4121
		// (get) Token: 0x06003317 RID: 13079 RVA: 0x00144100 File Offset: 0x00142300
		public static string txtRetrain
		{
			get
			{
				return Resources.ResourceManager.GetString("txtRetrain", Resources.resourceCulture);
			}
		}

		// Token: 0x1700101A RID: 4122
		// (get) Token: 0x06003318 RID: 13080 RVA: 0x00144128 File Offset: 0x00142328
		public static string txtRetrainMax
		{
			get
			{
				return Resources.ResourceManager.GetString("txtRetrainMax", Resources.resourceCulture);
			}
		}

		// Token: 0x1700101B RID: 4123
		// (get) Token: 0x06003319 RID: 13081 RVA: 0x00144150 File Offset: 0x00142350
		public static string txtRetrainMin
		{
			get
			{
				return Resources.ResourceManager.GetString("txtRetrainMin", Resources.resourceCulture);
			}
		}

		// Token: 0x1700101C RID: 4124
		// (get) Token: 0x0600331A RID: 13082 RVA: 0x00144178 File Offset: 0x00142378
		public static string txtSafetyCircutEx2A
		{
			get
			{
				return Resources.ResourceManager.GetString("txtSafetyCircutEx2A", Resources.resourceCulture);
			}
		}

		// Token: 0x1700101D RID: 4125
		// (get) Token: 0x0600331B RID: 13083 RVA: 0x001441A0 File Offset: 0x001423A0
		public static string txtSafetySwitch
		{
			get
			{
				return Resources.ResourceManager.GetString("txtSafetySwitch", Resources.resourceCulture);
			}
		}

		// Token: 0x1700101E RID: 4126
		// (get) Token: 0x0600331C RID: 13084 RVA: 0x001441C8 File Offset: 0x001423C8
		public static string txtSaturday
		{
			get
			{
				return Resources.ResourceManager.GetString("txtSaturday", Resources.resourceCulture);
			}
		}

		// Token: 0x1700101F RID: 4127
		// (get) Token: 0x0600331D RID: 13085 RVA: 0x001441F0 File Offset: 0x001423F0
		public static string txtSaveAIImages
		{
			get
			{
				return Resources.ResourceManager.GetString("txtSaveAIImages", Resources.resourceCulture);
			}
		}

		// Token: 0x17001020 RID: 4128
		// (get) Token: 0x0600331E RID: 13086 RVA: 0x00144218 File Offset: 0x00142418
		public static string txtSaveAndRename
		{
			get
			{
				return Resources.ResourceManager.GetString("txtSaveAndRename", Resources.resourceCulture);
			}
		}

		// Token: 0x17001021 RID: 4129
		// (get) Token: 0x0600331F RID: 13087 RVA: 0x00144240 File Offset: 0x00142440
		public static string txtSaveDatabase
		{
			get
			{
				return Resources.ResourceManager.GetString("txtSaveDatabase", Resources.resourceCulture);
			}
		}

		// Token: 0x17001022 RID: 4130
		// (get) Token: 0x06003320 RID: 13088 RVA: 0x00144268 File Offset: 0x00142468
		public static string txtSaveDocuments
		{
			get
			{
				return Resources.ResourceManager.GetString("txtSaveDocuments", Resources.resourceCulture);
			}
		}

		// Token: 0x17001023 RID: 4131
		// (get) Token: 0x06003321 RID: 13089 RVA: 0x00144290 File Offset: 0x00142490
		public static string txtSavedOnUSB
		{
			get
			{
				return Resources.ResourceManager.GetString("txtSavedOnUSB", Resources.resourceCulture);
			}
		}

		// Token: 0x17001024 RID: 4132
		// (get) Token: 0x06003322 RID: 13090 RVA: 0x001442B8 File Offset: 0x001424B8
		public static string txtSaveGui
		{
			get
			{
				return Resources.ResourceManager.GetString("txtSaveGui", Resources.resourceCulture);
			}
		}

		// Token: 0x17001025 RID: 4133
		// (get) Token: 0x06003323 RID: 13091 RVA: 0x001442E0 File Offset: 0x001424E0
		public static string txtSaveImageProcessing
		{
			get
			{
				return Resources.ResourceManager.GetString("txtSaveImageProcessing", Resources.resourceCulture);
			}
		}

		// Token: 0x17001026 RID: 4134
		// (get) Token: 0x06003324 RID: 13092 RVA: 0x00144308 File Offset: 0x00142508
		public static string txtSaveImages
		{
			get
			{
				return Resources.ResourceManager.GetString("txtSaveImages", Resources.resourceCulture);
			}
		}

		// Token: 0x17001027 RID: 4135
		// (get) Token: 0x06003325 RID: 13093 RVA: 0x00144330 File Offset: 0x00142530
		public static string txtSaveSimulatedImg
		{
			get
			{
				return Resources.ResourceManager.GetString("txtSaveSimulatedImg", Resources.resourceCulture);
			}
		}

		// Token: 0x17001028 RID: 4136
		// (get) Token: 0x06003326 RID: 13094 RVA: 0x00144358 File Offset: 0x00142558
		public static string txtSaveSuccessful
		{
			get
			{
				return Resources.ResourceManager.GetString("txtSaveSuccessful", Resources.resourceCulture);
			}
		}

		// Token: 0x17001029 RID: 4137
		// (get) Token: 0x06003327 RID: 13095 RVA: 0x00144380 File Offset: 0x00142580
		public static string txtSaveSystem
		{
			get
			{
				return Resources.ResourceManager.GetString("txtSaveSystem", Resources.resourceCulture);
			}
		}

		// Token: 0x1700102A RID: 4138
		// (get) Token: 0x06003328 RID: 13096 RVA: 0x001443A8 File Offset: 0x001425A8
		public static string txtSavingImage
		{
			get
			{
				return Resources.ResourceManager.GetString("txtSavingImage", Resources.resourceCulture);
			}
		}

		// Token: 0x1700102B RID: 4139
		// (get) Token: 0x06003329 RID: 13097 RVA: 0x001443D0 File Offset: 0x001425D0
		public static string txtSavingMode
		{
			get
			{
				return Resources.ResourceManager.GetString("txtSavingMode", Resources.resourceCulture);
			}
		}

		// Token: 0x1700102C RID: 4140
		// (get) Token: 0x0600332A RID: 13098 RVA: 0x001443F8 File Offset: 0x001425F8
		public static string txtSearch
		{
			get
			{
				return Resources.ResourceManager.GetString("txtSearch", Resources.resourceCulture);
			}
		}

		// Token: 0x1700102D RID: 4141
		// (get) Token: 0x0600332B RID: 13099 RVA: 0x00144420 File Offset: 0x00142620
		public static string txtSecond
		{
			get
			{
				return Resources.ResourceManager.GetString("txtSecond", Resources.resourceCulture);
			}
		}

		// Token: 0x1700102E RID: 4142
		// (get) Token: 0x0600332C RID: 13100 RVA: 0x00144448 File Offset: 0x00142648
		public static string txtSeconds
		{
			get
			{
				return Resources.ResourceManager.GetString("txtSeconds", Resources.resourceCulture);
			}
		}

		// Token: 0x1700102F RID: 4143
		// (get) Token: 0x0600332D RID: 13101 RVA: 0x00144470 File Offset: 0x00142670
		public static string txtSecurityKey
		{
			get
			{
				return Resources.ResourceManager.GetString("txtSecurityKey", Resources.resourceCulture);
			}
		}

		// Token: 0x17001030 RID: 4144
		// (get) Token: 0x0600332E RID: 13102 RVA: 0x00144498 File Offset: 0x00142698
		public static string txtSecurityType
		{
			get
			{
				return Resources.ResourceManager.GetString("txtSecurityType", Resources.resourceCulture);
			}
		}

		// Token: 0x17001031 RID: 4145
		// (get) Token: 0x0600332F RID: 13103 RVA: 0x001444C0 File Offset: 0x001426C0
		public static string txtSelectLeftCanEdge
		{
			get
			{
				return Resources.ResourceManager.GetString("txtSelectLeftCanEdge", Resources.resourceCulture);
			}
		}

		// Token: 0x17001032 RID: 4146
		// (get) Token: 0x06003330 RID: 13104 RVA: 0x001444E8 File Offset: 0x001426E8
		public static string txtSelectLowerCanEdge
		{
			get
			{
				return Resources.ResourceManager.GetString("txtSelectLowerCanEdge", Resources.resourceCulture);
			}
		}

		// Token: 0x17001033 RID: 4147
		// (get) Token: 0x06003331 RID: 13105 RVA: 0x00144510 File Offset: 0x00142710
		public static string txtSelectRightCanEdge
		{
			get
			{
				return Resources.ResourceManager.GetString("txtSelectRightCanEdge", Resources.resourceCulture);
			}
		}

		// Token: 0x17001034 RID: 4148
		// (get) Token: 0x06003332 RID: 13106 RVA: 0x00144538 File Offset: 0x00142738
		public static string txtSelectUpperCanEdge
		{
			get
			{
				return Resources.ResourceManager.GetString("txtSelectUpperCanEdge", Resources.resourceCulture);
			}
		}

		// Token: 0x17001035 RID: 4149
		// (get) Token: 0x06003333 RID: 13107 RVA: 0x00144560 File Offset: 0x00142760
		public static string txtSensitivity
		{
			get
			{
				return Resources.ResourceManager.GetString("txtSensitivity", Resources.resourceCulture);
			}
		}

		// Token: 0x17001036 RID: 4150
		// (get) Token: 0x06003334 RID: 13108 RVA: 0x00144588 File Offset: 0x00142788
		public static string txtSensitivityNotPossible
		{
			get
			{
				return Resources.ResourceManager.GetString("txtSensitivityNotPossible", Resources.resourceCulture);
			}
		}

		// Token: 0x17001037 RID: 4151
		// (get) Token: 0x06003335 RID: 13109 RVA: 0x001445B0 File Offset: 0x001427B0
		public static string txtSensitivityPercent
		{
			get
			{
				return Resources.ResourceManager.GetString("txtSensitivityPercent", Resources.resourceCulture);
			}
		}

		// Token: 0x17001038 RID: 4152
		// (get) Token: 0x06003336 RID: 13110 RVA: 0x001445D8 File Offset: 0x001427D8
		public static string txtSensors
		{
			get
			{
				return Resources.ResourceManager.GetString("txtSensors", Resources.resourceCulture);
			}
		}

		// Token: 0x17001039 RID: 4153
		// (get) Token: 0x06003337 RID: 13111 RVA: 0x00144600 File Offset: 0x00142800
		public static string txtSerialNumber
		{
			get
			{
				return Resources.ResourceManager.GetString("txtSerialNumber", Resources.resourceCulture);
			}
		}

		// Token: 0x1700103A RID: 4154
		// (get) Token: 0x06003338 RID: 13112 RVA: 0x00144628 File Offset: 0x00142828
		public static string txtServiceTechnician
		{
			get
			{
				return Resources.ResourceManager.GetString("txtServiceTechnician", Resources.resourceCulture);
			}
		}

		// Token: 0x1700103B RID: 4155
		// (get) Token: 0x06003339 RID: 13113 RVA: 0x00144650 File Offset: 0x00142850
		public static string txtSetComplianceMsg
		{
			get
			{
				return Resources.ResourceManager.GetString("txtSetComplianceMsg", Resources.resourceCulture);
			}
		}

		// Token: 0x1700103C RID: 4156
		// (get) Token: 0x0600333A RID: 13114 RVA: 0x00144678 File Offset: 0x00142878
		public static string txtSetComplSensQuestion
		{
			get
			{
				return Resources.ResourceManager.GetString("txtSetComplSensQuestion", Resources.resourceCulture);
			}
		}

		// Token: 0x1700103D RID: 4157
		// (get) Token: 0x0600333B RID: 13115 RVA: 0x001446A0 File Offset: 0x001428A0
		public static string txtSetErrorOutput
		{
			get
			{
				return Resources.ResourceManager.GetString("txtSetErrorOutput", Resources.resourceCulture);
			}
		}

		// Token: 0x1700103E RID: 4158
		// (get) Token: 0x0600333C RID: 13116 RVA: 0x001446C8 File Offset: 0x001428C8
		public static string txtSetInitSens
		{
			get
			{
				return Resources.ResourceManager.GetString("txtSetInitSens", Resources.resourceCulture);
			}
		}

		// Token: 0x1700103F RID: 4159
		// (get) Token: 0x0600333D RID: 13117 RVA: 0x001446F0 File Offset: 0x001428F0
		public static string txtSettings
		{
			get
			{
				return Resources.ResourceManager.GetString("txtSettings", Resources.resourceCulture);
			}
		}

		// Token: 0x17001040 RID: 4160
		// (get) Token: 0x0600333E RID: 13118 RVA: 0x00144718 File Offset: 0x00142918
		public static string txtSettingsInvalid
		{
			get
			{
				return Resources.ResourceManager.GetString("txtSettingsInvalid", Resources.resourceCulture);
			}
		}

		// Token: 0x17001041 RID: 4161
		// (get) Token: 0x0600333F RID: 13119 RVA: 0x00144740 File Offset: 0x00142940
		public static string txtSeveralArcs
		{
			get
			{
				return Resources.ResourceManager.GetString("txtSeveralArcs", Resources.resourceCulture);
			}
		}

		// Token: 0x17001042 RID: 4162
		// (get) Token: 0x06003340 RID: 13120 RVA: 0x00144768 File Offset: 0x00142968
		public static string txtShowOnlyValidBackups
		{
			get
			{
				return Resources.ResourceManager.GetString("txtShowOnlyValidBackups", Resources.resourceCulture);
			}
		}

		// Token: 0x17001043 RID: 4163
		// (get) Token: 0x06003341 RID: 13121 RVA: 0x00144790 File Offset: 0x00142990
		public static string txtSign
		{
			get
			{
				return Resources.ResourceManager.GetString("txtSign", Resources.resourceCulture);
			}
		}

		// Token: 0x17001044 RID: 4164
		// (get) Token: 0x06003342 RID: 13122 RVA: 0x001447B8 File Offset: 0x001429B8
		public static string txtSignalFluctuation
		{
			get
			{
				return Resources.ResourceManager.GetString("txtSignalFluctuation", Resources.resourceCulture);
			}
		}

		// Token: 0x17001045 RID: 4165
		// (get) Token: 0x06003343 RID: 13123 RVA: 0x001447E0 File Offset: 0x001429E0
		public static string txtSignalFluctuationModule
		{
			get
			{
				return Resources.ResourceManager.GetString("txtSignalFluctuationModule", Resources.resourceCulture);
			}
		}

		// Token: 0x17001046 RID: 4166
		// (get) Token: 0x06003344 RID: 13124 RVA: 0x00144808 File Offset: 0x00142A08
		public static string txtSignalMonitoring
		{
			get
			{
				return Resources.ResourceManager.GetString("txtSignalMonitoring", Resources.resourceCulture);
			}
		}

		// Token: 0x17001047 RID: 4167
		// (get) Token: 0x06003345 RID: 13125 RVA: 0x00144830 File Offset: 0x00142A30
		public static string txtSignalValue
		{
			get
			{
				return Resources.ResourceManager.GetString("txtSignalValue", Resources.resourceCulture);
			}
		}

		// Token: 0x17001048 RID: 4168
		// (get) Token: 0x06003346 RID: 13126 RVA: 0x00144858 File Offset: 0x00142A58
		public static string txtSignalValueModul
		{
			get
			{
				return Resources.ResourceManager.GetString("txtSignalValueModul", Resources.resourceCulture);
			}
		}

		// Token: 0x17001049 RID: 4169
		// (get) Token: 0x06003347 RID: 13127 RVA: 0x00144880 File Offset: 0x00142A80
		public static string txtSigned
		{
			get
			{
				return Resources.ResourceManager.GetString("txtSigned", Resources.resourceCulture);
			}
		}

		// Token: 0x1700104A RID: 4170
		// (get) Token: 0x06003348 RID: 13128 RVA: 0x001448A8 File Offset: 0x00142AA8
		public static string txtSimAfterLearn
		{
			get
			{
				return Resources.ResourceManager.GetString("txtSimAfterLearn", Resources.resourceCulture);
			}
		}

		// Token: 0x1700104B RID: 4171
		// (get) Token: 0x06003349 RID: 13129 RVA: 0x001448D0 File Offset: 0x00142AD0
		public static string txtSimAfterTimeIntervall
		{
			get
			{
				return Resources.ResourceManager.GetString("txtSimAfterTimeIntervall", Resources.resourceCulture);
			}
		}

		// Token: 0x1700104C RID: 4172
		// (get) Token: 0x0600334A RID: 13130 RVA: 0x001448F8 File Offset: 0x00142AF8
		public static string txtSimulation
		{
			get
			{
				return Resources.ResourceManager.GetString("txtSimulation", Resources.resourceCulture);
			}
		}

		// Token: 0x1700104D RID: 4173
		// (get) Token: 0x0600334B RID: 13131 RVA: 0x00144920 File Offset: 0x00142B20
		public static string txtSingle
		{
			get
			{
				return Resources.ResourceManager.GetString("txtSingle", Resources.resourceCulture);
			}
		}

		// Token: 0x1700104E RID: 4174
		// (get) Token: 0x0600334C RID: 13132 RVA: 0x00144948 File Offset: 0x00142B48
		public static string txtSingleBall
		{
			get
			{
				return Resources.ResourceManager.GetString("txtSingleBall", Resources.resourceCulture);
			}
		}

		// Token: 0x1700104F RID: 4175
		// (get) Token: 0x0600334D RID: 13133 RVA: 0x00144970 File Offset: 0x00142B70
		public static string txtSingleTestcard
		{
			get
			{
				return Resources.ResourceManager.GetString("txtSingleTestcard", Resources.resourceCulture);
			}
		}

		// Token: 0x17001050 RID: 4176
		// (get) Token: 0x0600334E RID: 13134 RVA: 0x00144998 File Offset: 0x00142B98
		public static string txtSize
		{
			get
			{
				return Resources.ResourceManager.GetString("txtSize", Resources.resourceCulture);
			}
		}

		// Token: 0x17001051 RID: 4177
		// (get) Token: 0x0600334F RID: 13135 RVA: 0x001449C0 File Offset: 0x00142BC0
		public static string txtSortingOff
		{
			get
			{
				return Resources.ResourceManager.GetString("txtSortingOff", Resources.resourceCulture);
			}
		}

		// Token: 0x17001052 RID: 4178
		// (get) Token: 0x06003350 RID: 13136 RVA: 0x001449E8 File Offset: 0x00142BE8
		public static string txtSortingSwitchedOff
		{
			get
			{
				return Resources.ResourceManager.GetString("txtSortingSwitchedOff", Resources.resourceCulture);
			}
		}

		// Token: 0x17001053 RID: 4179
		// (get) Token: 0x06003351 RID: 13137 RVA: 0x00144A10 File Offset: 0x00142C10
		public static string txtSortingSwitchedOn
		{
			get
			{
				return Resources.ResourceManager.GetString("txtSortingSwitchedOn", Resources.resourceCulture);
			}
		}

		// Token: 0x17001054 RID: 4180
		// (get) Token: 0x06003352 RID: 13138 RVA: 0x00144A38 File Offset: 0x00142C38
		public static string txtSpecializedToDetect
		{
			get
			{
				return Resources.ResourceManager.GetString("txtSpecializedToDetect", Resources.resourceCulture);
			}
		}

		// Token: 0x17001055 RID: 4181
		// (get) Token: 0x06003353 RID: 13139 RVA: 0x00144A60 File Offset: 0x00142C60
		public static string txtSpeedUnit
		{
			get
			{
				return Resources.ResourceManager.GetString("txtSpeedUnit", Resources.resourceCulture);
			}
		}

		// Token: 0x17001056 RID: 4182
		// (get) Token: 0x06003354 RID: 13140 RVA: 0x00144A88 File Offset: 0x00142C88
		public static string txtSSID
		{
			get
			{
				return Resources.ResourceManager.GetString("txtSSID", Resources.resourceCulture);
			}
		}

		// Token: 0x17001057 RID: 4183
		// (get) Token: 0x06003355 RID: 13141 RVA: 0x00144AB0 File Offset: 0x00142CB0
		public static string txtStartAudit
		{
			get
			{
				return Resources.ResourceManager.GetString("txtStartAudit", Resources.resourceCulture);
			}
		}

		// Token: 0x17001058 RID: 4184
		// (get) Token: 0x06003356 RID: 13142 RVA: 0x00144AD8 File Offset: 0x00142CD8
		public static string txtStartConveyor
		{
			get
			{
				return Resources.ResourceManager.GetString("txtStartConveyor", Resources.resourceCulture);
			}
		}

		// Token: 0x17001059 RID: 4185
		// (get) Token: 0x06003357 RID: 13143 RVA: 0x00144B00 File Offset: 0x00142D00
		public static string txtStartNewDoc
		{
			get
			{
				return Resources.ResourceManager.GetString("txtStartNewDoc", Resources.resourceCulture);
			}
		}

		// Token: 0x1700105A RID: 4186
		// (get) Token: 0x06003358 RID: 13144 RVA: 0x00144B28 File Offset: 0x00142D28
		public static string txtStartNewDocument
		{
			get
			{
				return Resources.ResourceManager.GetString("txtStartNewDocument", Resources.resourceCulture);
			}
		}

		// Token: 0x1700105B RID: 4187
		// (get) Token: 0x06003359 RID: 13145 RVA: 0x00144B50 File Offset: 0x00142D50
		public static string txtStartTime
		{
			get
			{
				return Resources.ResourceManager.GetString("txtStartTime", Resources.resourceCulture);
			}
		}

		// Token: 0x1700105C RID: 4188
		// (get) Token: 0x0600335A RID: 13146 RVA: 0x00144B78 File Offset: 0x00142D78
		public static string txtStartValidation
		{
			get
			{
				return Resources.ResourceManager.GetString("txtStartValidation", Resources.resourceCulture);
			}
		}

		// Token: 0x1700105D RID: 4189
		// (get) Token: 0x0600335B RID: 13147 RVA: 0x00144BA0 File Offset: 0x00142DA0
		public static string txtStartValveTest
		{
			get
			{
				return Resources.ResourceManager.GetString("txtStartValveTest", Resources.resourceCulture);
			}
		}

		// Token: 0x1700105E RID: 4190
		// (get) Token: 0x0600335C RID: 13148 RVA: 0x00144BC8 File Offset: 0x00142DC8
		public static string txtStartVerifyTest
		{
			get
			{
				return Resources.ResourceManager.GetString("txtStartVerifyTest", Resources.resourceCulture);
			}
		}

		// Token: 0x1700105F RID: 4191
		// (get) Token: 0x0600335D RID: 13149 RVA: 0x00144BF0 File Offset: 0x00142DF0
		public static string txtStartXray
		{
			get
			{
				return Resources.ResourceManager.GetString("txtStartXray", Resources.resourceCulture);
			}
		}

		// Token: 0x17001060 RID: 4192
		// (get) Token: 0x0600335E RID: 13150 RVA: 0x00144C18 File Offset: 0x00142E18
		public static string txtState
		{
			get
			{
				return Resources.ResourceManager.GetString("txtState", Resources.resourceCulture);
			}
		}

		// Token: 0x17001061 RID: 4193
		// (get) Token: 0x0600335F RID: 13151 RVA: 0x00144C40 File Offset: 0x00142E40
		public static string txtStatistics
		{
			get
			{
				return Resources.ResourceManager.GetString("txtStatistics", Resources.resourceCulture);
			}
		}

		// Token: 0x17001062 RID: 4194
		// (get) Token: 0x06003360 RID: 13152 RVA: 0x00144C68 File Offset: 0x00142E68
		public static string txtStatisticTable
		{
			get
			{
				return Resources.ResourceManager.GetString("txtStatisticTable", Resources.resourceCulture);
			}
		}

		// Token: 0x17001063 RID: 4195
		// (get) Token: 0x06003361 RID: 13153 RVA: 0x00144C90 File Offset: 0x00142E90
		public static string txtStDeviation
		{
			get
			{
				return Resources.ResourceManager.GetString("txtStDeviation", Resources.resourceCulture);
			}
		}

		// Token: 0x17001064 RID: 4196
		// (get) Token: 0x06003362 RID: 13154 RVA: 0x00144CB8 File Offset: 0x00142EB8
		public static string txtStopConveyorIfError
		{
			get
			{
				return Resources.ResourceManager.GetString("txtStopConveyorIfError", Resources.resourceCulture);
			}
		}

		// Token: 0x17001065 RID: 4197
		// (get) Token: 0x06003363 RID: 13155 RVA: 0x00144CE0 File Offset: 0x00142EE0
		public static string txtStoppConveyorAndXRay
		{
			get
			{
				return Resources.ResourceManager.GetString("txtStoppConveyorAndXRay", Resources.resourceCulture);
			}
		}

		// Token: 0x17001066 RID: 4198
		// (get) Token: 0x06003364 RID: 13156 RVA: 0x00144D08 File Offset: 0x00142F08
		public static string txtStopValveTest
		{
			get
			{
				return Resources.ResourceManager.GetString("txtStopValveTest", Resources.resourceCulture);
			}
		}

		// Token: 0x17001067 RID: 4199
		// (get) Token: 0x06003365 RID: 13157 RVA: 0x00144D30 File Offset: 0x00142F30
		public static string txtStreet
		{
			get
			{
				return Resources.ResourceManager.GetString("txtStreet", Resources.resourceCulture);
			}
		}

		// Token: 0x17001068 RID: 4200
		// (get) Token: 0x06003366 RID: 13158 RVA: 0x00144D58 File Offset: 0x00142F58
		public static string txtSubnetMask
		{
			get
			{
				return Resources.ResourceManager.GetString("txtSubnetMask", Resources.resourceCulture);
			}
		}

		// Token: 0x17001069 RID: 4201
		// (get) Token: 0x06003367 RID: 13159 RVA: 0x00144D80 File Offset: 0x00142F80
		public static string txtSubnetReserved
		{
			get
			{
				return Resources.ResourceManager.GetString("txtSubnetReserved", Resources.resourceCulture);
			}
		}

		// Token: 0x1700106A RID: 4202
		// (get) Token: 0x06003368 RID: 13160 RVA: 0x00144DA8 File Offset: 0x00142FA8
		public static string txtSuccess
		{
			get
			{
				return Resources.ResourceManager.GetString("txtSuccess", Resources.resourceCulture);
			}
		}

		// Token: 0x1700106B RID: 4203
		// (get) Token: 0x06003369 RID: 13161 RVA: 0x00144DD0 File Offset: 0x00142FD0
		public static string txtSumValves
		{
			get
			{
				return Resources.ResourceManager.GetString("txtSumValves", Resources.resourceCulture);
			}
		}

		// Token: 0x1700106C RID: 4204
		// (get) Token: 0x0600336A RID: 13162 RVA: 0x00144DF8 File Offset: 0x00142FF8
		public static string txtSunday
		{
			get
			{
				return Resources.ResourceManager.GetString("txtSunday", Resources.resourceCulture);
			}
		}

		// Token: 0x1700106D RID: 4205
		// (get) Token: 0x0600336B RID: 13163 RVA: 0x00144E20 File Offset: 0x00143020
		public static string txtSwitchOnCycles
		{
			get
			{
				return Resources.ResourceManager.GetString("txtSwitchOnCycles", Resources.resourceCulture);
			}
		}

		// Token: 0x1700106E RID: 4206
		// (get) Token: 0x0600336C RID: 13164 RVA: 0x00144E48 File Offset: 0x00143048
		public static string txtTarget
		{
			get
			{
				return Resources.ResourceManager.GetString("txtTarget", Resources.resourceCulture);
			}
		}

		// Token: 0x1700106F RID: 4207
		// (get) Token: 0x0600336D RID: 13165 RVA: 0x00144E70 File Offset: 0x00143070
		public static string txtTelephone
		{
			get
			{
				return Resources.ResourceManager.GetString("txtTelephone", Resources.resourceCulture);
			}
		}

		// Token: 0x17001070 RID: 4208
		// (get) Token: 0x0600336E RID: 13166 RVA: 0x00144E98 File Offset: 0x00143098
		public static string txtTemperature
		{
			get
			{
				return Resources.ResourceManager.GetString("txtTemperature", Resources.resourceCulture);
			}
		}

		// Token: 0x17001071 RID: 4209
		// (get) Token: 0x0600336F RID: 13167 RVA: 0x00144EC0 File Offset: 0x001430C0
		public static string txtTestAirPressureMonitoing
		{
			get
			{
				return Resources.ResourceManager.GetString("txtTestAirPressureMonitoing", Resources.resourceCulture);
			}
		}

		// Token: 0x17001072 RID: 4210
		// (get) Token: 0x06003370 RID: 13168 RVA: 0x00144EE8 File Offset: 0x001430E8
		public static string txtTestballs
		{
			get
			{
				return Resources.ResourceManager.GetString("txtTestballs", Resources.resourceCulture);
			}
		}

		// Token: 0x17001073 RID: 4211
		// (get) Token: 0x06003371 RID: 13169 RVA: 0x00144F10 File Offset: 0x00143110
		public static string txtTestcard
		{
			get
			{
				return Resources.ResourceManager.GetString("txtTestcard", Resources.resourceCulture);
			}
		}

		// Token: 0x17001074 RID: 4212
		// (get) Token: 0x06003372 RID: 13170 RVA: 0x00144F38 File Offset: 0x00143138
		public static string txtTestCard100
		{
			get
			{
				return Resources.ResourceManager.GetString("txtTestCard100", Resources.resourceCulture);
			}
		}

		// Token: 0x17001075 RID: 4213
		// (get) Token: 0x06003373 RID: 13171 RVA: 0x00144F60 File Offset: 0x00143160
		public static string txtTestCard5
		{
			get
			{
				return Resources.ResourceManager.GetString("txtTestCard5", Resources.resourceCulture);
			}
		}

		// Token: 0x17001076 RID: 4214
		// (get) Token: 0x06003374 RID: 13172 RVA: 0x00144F88 File Offset: 0x00143188
		public static string txtTestCard5Same
		{
			get
			{
				return Resources.ResourceManager.GetString("txtTestCard5Same", Resources.resourceCulture);
			}
		}

		// Token: 0x17001077 RID: 4215
		// (get) Token: 0x06003375 RID: 13173 RVA: 0x00144FB0 File Offset: 0x001431B0
		public static string txtTestcardPattern
		{
			get
			{
				return Resources.ResourceManager.GetString("txtTestcardPattern", Resources.resourceCulture);
			}
		}

		// Token: 0x17001078 RID: 4216
		// (get) Token: 0x06003376 RID: 13174 RVA: 0x00144FD8 File Offset: 0x001431D8
		public static string txtTestCardSimAutokV
		{
			get
			{
				return Resources.ResourceManager.GetString("txtTestCardSimAutokV", Resources.resourceCulture);
			}
		}

		// Token: 0x17001079 RID: 4217
		// (get) Token: 0x06003377 RID: 13175 RVA: 0x00145000 File Offset: 0x00143200
		public static string txtTestcardSimNoImages
		{
			get
			{
				return Resources.ResourceManager.GetString("txtTestcardSimNoImages", Resources.resourceCulture);
			}
		}

		// Token: 0x1700107A RID: 4218
		// (get) Token: 0x06003378 RID: 13176 RVA: 0x00145028 File Offset: 0x00143228
		public static string txtTestcardSimNotPossible
		{
			get
			{
				return Resources.ResourceManager.GetString("txtTestcardSimNotPossible", Resources.resourceCulture);
			}
		}

		// Token: 0x1700107B RID: 4219
		// (get) Token: 0x06003379 RID: 13177 RVA: 0x00145050 File Offset: 0x00143250
		public static string txtTestcardSize
		{
			get
			{
				return Resources.ResourceManager.GetString("txtTestcardSize", Resources.resourceCulture);
			}
		}

		// Token: 0x1700107C RID: 4220
		// (get) Token: 0x0600337A RID: 13178 RVA: 0x00145078 File Offset: 0x00143278
		public static string txtTestFailed
		{
			get
			{
				return Resources.ResourceManager.GetString("txtTestFailed", Resources.resourceCulture);
			}
		}

		// Token: 0x1700107D RID: 4221
		// (get) Token: 0x0600337B RID: 13179 RVA: 0x001450A0 File Offset: 0x001432A0
		public static string txtTestOk
		{
			get
			{
				return Resources.ResourceManager.GetString("txtTestOk", Resources.resourceCulture);
			}
		}

		// Token: 0x1700107E RID: 4222
		// (get) Token: 0x0600337C RID: 13180 RVA: 0x001450C8 File Offset: 0x001432C8
		public static string txtTestsamplesNotSetAudit
		{
			get
			{
				return Resources.ResourceManager.GetString("txtTestsamplesNotSetAudit", Resources.resourceCulture);
			}
		}

		// Token: 0x1700107F RID: 4223
		// (get) Token: 0x0600337D RID: 13181 RVA: 0x001450F0 File Offset: 0x001432F0
		public static string txtThickness
		{
			get
			{
				return Resources.ResourceManager.GetString("txtThickness", Resources.resourceCulture);
			}
		}

		// Token: 0x17001080 RID: 4224
		// (get) Token: 0x0600337E RID: 13182 RVA: 0x00145118 File Offset: 0x00143318
		public static string txtThreshold
		{
			get
			{
				return Resources.ResourceManager.GetString("txtThreshold", Resources.resourceCulture);
			}
		}

		// Token: 0x17001081 RID: 4225
		// (get) Token: 0x0600337F RID: 13183 RVA: 0x00145140 File Offset: 0x00143340
		public static string txtThroughput
		{
			get
			{
				return Resources.ResourceManager.GetString("txtThroughput", Resources.resourceCulture);
			}
		}

		// Token: 0x17001082 RID: 4226
		// (get) Token: 0x06003380 RID: 13184 RVA: 0x00145168 File Offset: 0x00143368
		public static string txtThroughputPerMin
		{
			get
			{
				return Resources.ResourceManager.GetString("txtThroughputPerMin", Resources.resourceCulture);
			}
		}

		// Token: 0x17001083 RID: 4227
		// (get) Token: 0x06003381 RID: 13185 RVA: 0x00145190 File Offset: 0x00143390
		public static string txtThursday
		{
			get
			{
				return Resources.ResourceManager.GetString("txtThursday", Resources.resourceCulture);
			}
		}

		// Token: 0x17001084 RID: 4228
		// (get) Token: 0x06003382 RID: 13186 RVA: 0x001451B8 File Offset: 0x001433B8
		public static string txtTimeElapsed
		{
			get
			{
				return Resources.ResourceManager.GetString("txtTimeElapsed", Resources.resourceCulture);
			}
		}

		// Token: 0x17001085 RID: 4229
		// (get) Token: 0x06003383 RID: 13187 RVA: 0x001451E0 File Offset: 0x001433E0
		public static string txtTimeIntervall
		{
			get
			{
				return Resources.ResourceManager.GetString("txtTimeIntervall", Resources.resourceCulture);
			}
		}

		// Token: 0x17001086 RID: 4230
		// (get) Token: 0x06003384 RID: 13188 RVA: 0x00145208 File Offset: 0x00143408
		public static string txtTimeIntervallinMin
		{
			get
			{
				return Resources.ResourceManager.GetString("txtTimeIntervallinMin", Resources.resourceCulture);
			}
		}

		// Token: 0x17001087 RID: 4231
		// (get) Token: 0x06003385 RID: 13189 RVA: 0x00145230 File Offset: 0x00143430
		public static string txtTimeOfDay
		{
			get
			{
				return Resources.ResourceManager.GetString("txtTimeOfDay", Resources.resourceCulture);
			}
		}

		// Token: 0x17001088 RID: 4232
		// (get) Token: 0x06003386 RID: 13190 RVA: 0x00145258 File Offset: 0x00143458
		public static string txtTimeOver
		{
			get
			{
				return Resources.ResourceManager.GetString("txtTimeOver", Resources.resourceCulture);
			}
		}

		// Token: 0x17001089 RID: 4233
		// (get) Token: 0x06003387 RID: 13191 RVA: 0x00145280 File Offset: 0x00143480
		public static string txtTimeSpanRepetitionMessage
		{
			get
			{
				return Resources.ResourceManager.GetString("txtTimeSpanRepetitionMessage", Resources.resourceCulture);
			}
		}

		// Token: 0x1700108A RID: 4234
		// (get) Token: 0x06003388 RID: 13192 RVA: 0x001452A8 File Offset: 0x001434A8
		public static string txtTimeToNextAudit
		{
			get
			{
				return Resources.ResourceManager.GetString("txtTimeToNextAudit", Resources.resourceCulture);
			}
		}

		// Token: 0x1700108B RID: 4235
		// (get) Token: 0x06003389 RID: 13193 RVA: 0x001452D0 File Offset: 0x001434D0
		public static string txtTimeToStartAudit
		{
			get
			{
				return Resources.ResourceManager.GetString("txtTimeToStartAudit", Resources.resourceCulture);
			}
		}

		// Token: 0x1700108C RID: 4236
		// (get) Token: 0x0600338A RID: 13194 RVA: 0x001452F8 File Offset: 0x001434F8
		public static string txtTimeTraining
		{
			get
			{
				return Resources.ResourceManager.GetString("txtTimeTraining", Resources.resourceCulture);
			}
		}

		// Token: 0x1700108D RID: 4237
		// (get) Token: 0x0600338B RID: 13195 RVA: 0x00145320 File Offset: 0x00143520
		public static string txtToday
		{
			get
			{
				return Resources.ResourceManager.GetString("txtToday", Resources.resourceCulture);
			}
		}

		// Token: 0x1700108E RID: 4238
		// (get) Token: 0x0600338C RID: 13196 RVA: 0x00145348 File Offset: 0x00143548
		public static string txtTooManyImages
		{
			get
			{
				return Resources.ResourceManager.GetString("txtTooManyImages", Resources.resourceCulture);
			}
		}

		// Token: 0x1700108F RID: 4239
		// (get) Token: 0x0600338D RID: 13197 RVA: 0x00145370 File Offset: 0x00143570
		public static string txtTooManyRejects
		{
			get
			{
				return Resources.ResourceManager.GetString("txtTooManyRejects", Resources.resourceCulture);
			}
		}

		// Token: 0x17001090 RID: 4240
		// (get) Token: 0x0600338E RID: 13198 RVA: 0x00145398 File Offset: 0x00143598
		public static string txtTrainingEnabled
		{
			get
			{
				return Resources.ResourceManager.GetString("txtTrainingEnabled", Resources.resourceCulture);
			}
		}

		// Token: 0x17001091 RID: 4241
		// (get) Token: 0x0600338F RID: 13199 RVA: 0x001453C0 File Offset: 0x001435C0
		public static string txtTrainingEnabled2
		{
			get
			{
				return Resources.ResourceManager.GetString("txtTrainingEnabled2", Resources.resourceCulture);
			}
		}

		// Token: 0x17001092 RID: 4242
		// (get) Token: 0x06003390 RID: 13200 RVA: 0x001453E8 File Offset: 0x001435E8
		public static string txtTrainingProgress
		{
			get
			{
				return Resources.ResourceManager.GetString("txtTrainingProgress", Resources.resourceCulture);
			}
		}

		// Token: 0x17001093 RID: 4243
		// (get) Token: 0x06003391 RID: 13201 RVA: 0x00145410 File Offset: 0x00143610
		public static string txtTrainInOutDelay
		{
			get
			{
				return Resources.ResourceManager.GetString("txtTrainInOutDelay", Resources.resourceCulture);
			}
		}

		// Token: 0x17001094 RID: 4244
		// (get) Token: 0x06003392 RID: 13202 RVA: 0x00145438 File Offset: 0x00143638
		public static string txtTransmissionFailed
		{
			get
			{
				return Resources.ResourceManager.GetString("txtTransmissionFailed", Resources.resourceCulture);
			}
		}

		// Token: 0x17001095 RID: 4245
		// (get) Token: 0x06003393 RID: 13203 RVA: 0x00145460 File Offset: 0x00143660
		public static string txtTuesday
		{
			get
			{
				return Resources.ResourceManager.GetString("txtTuesday", Resources.resourceCulture);
			}
		}

		// Token: 0x17001096 RID: 4246
		// (get) Token: 0x06003394 RID: 13204 RVA: 0x00145488 File Offset: 0x00143688
		public static string txtTunnel
		{
			get
			{
				return Resources.ResourceManager.GetString("txtTunnel", Resources.resourceCulture);
			}
		}

		// Token: 0x17001097 RID: 4247
		// (get) Token: 0x06003395 RID: 13205 RVA: 0x001454B0 File Offset: 0x001436B0
		public static string txtTurningOffProductInspection
		{
			get
			{
				return Resources.ResourceManager.GetString("txtTurningOffProductInspection", Resources.resourceCulture);
			}
		}

		// Token: 0x17001098 RID: 4248
		// (get) Token: 0x06003396 RID: 13206 RVA: 0x001454D8 File Offset: 0x001436D8
		public static string txtTurnOnXray
		{
			get
			{
				return Resources.ResourceManager.GetString("txtTurnOnXray", Resources.resourceCulture);
			}
		}

		// Token: 0x17001099 RID: 4249
		// (get) Token: 0x06003397 RID: 13207 RVA: 0x00145500 File Offset: 0x00143700
		public static string txtTurnXrayOff
		{
			get
			{
				return Resources.ResourceManager.GetString("txtTurnXrayOff", Resources.resourceCulture);
			}
		}

		// Token: 0x1700109A RID: 4250
		// (get) Token: 0x06003398 RID: 13208 RVA: 0x00145528 File Offset: 0x00143728
		public static string txtTurnXrayOn
		{
			get
			{
				return Resources.ResourceManager.GetString("txtTurnXrayOn", Resources.resourceCulture);
			}
		}

		// Token: 0x1700109B RID: 4251
		// (get) Token: 0x06003399 RID: 13209 RVA: 0x00145550 File Offset: 0x00143750
		public static string txtType
		{
			get
			{
				return Resources.ResourceManager.GetString("txtType", Resources.resourceCulture);
			}
		}

		// Token: 0x1700109C RID: 4252
		// (get) Token: 0x0600339A RID: 13210 RVA: 0x00145578 File Offset: 0x00143778
		public static string txtUnderweight
		{
			get
			{
				return Resources.ResourceManager.GetString("txtUnderweight", Resources.resourceCulture);
			}
		}

		// Token: 0x1700109D RID: 4253
		// (get) Token: 0x0600339B RID: 13211 RVA: 0x001455A0 File Offset: 0x001437A0
		public static string txtUndoRetrain
		{
			get
			{
				return Resources.ResourceManager.GetString("txtUndoRetrain", Resources.resourceCulture);
			}
		}

		// Token: 0x1700109E RID: 4254
		// (get) Token: 0x0600339C RID: 13212 RVA: 0x001455C8 File Offset: 0x001437C8
		public static string txtUndoRetrainLogbook
		{
			get
			{
				return Resources.ResourceManager.GetString("txtUndoRetrainLogbook", Resources.resourceCulture);
			}
		}

		// Token: 0x1700109F RID: 4255
		// (get) Token: 0x0600339D RID: 13213 RVA: 0x001455F0 File Offset: 0x001437F0
		public static string txtUnknownXRayModelNumber
		{
			get
			{
				return Resources.ResourceManager.GetString("txtUnknownXRayModelNumber", Resources.resourceCulture);
			}
		}

		// Token: 0x170010A0 RID: 4256
		// (get) Token: 0x0600339E RID: 13214 RVA: 0x00145618 File Offset: 0x00143818
		public static string txtUnlockBin
		{
			get
			{
				return Resources.ResourceManager.GetString("txtUnlockBin", Resources.resourceCulture);
			}
		}

		// Token: 0x170010A1 RID: 4257
		// (get) Token: 0x0600339F RID: 13215 RVA: 0x00145640 File Offset: 0x00143840
		public static string txtUnlockBin1
		{
			get
			{
				return Resources.ResourceManager.GetString("txtUnlockBin1", Resources.resourceCulture);
			}
		}

		// Token: 0x170010A2 RID: 4258
		// (get) Token: 0x060033A0 RID: 13216 RVA: 0x00145668 File Offset: 0x00143868
		public static string txtUnlockBin2
		{
			get
			{
				return Resources.ResourceManager.GetString("txtUnlockBin2", Resources.resourceCulture);
			}
		}

		// Token: 0x170010A3 RID: 4259
		// (get) Token: 0x060033A1 RID: 13217 RVA: 0x00145690 File Offset: 0x00143890
		public static string txtUnlockBinTimeout
		{
			get
			{
				return Resources.ResourceManager.GetString("txtUnlockBinTimeout", Resources.resourceCulture);
			}
		}

		// Token: 0x170010A4 RID: 4260
		// (get) Token: 0x060033A2 RID: 13218 RVA: 0x001456B8 File Offset: 0x001438B8
		public static string txtUploadLogo
		{
			get
			{
				return Resources.ResourceManager.GetString("txtUploadLogo", Resources.resourceCulture);
			}
		}

		// Token: 0x170010A5 RID: 4261
		// (get) Token: 0x060033A3 RID: 13219 RVA: 0x001456E0 File Offset: 0x001438E0
		public static string txtUpperMaterialCurve
		{
			get
			{
				return Resources.ResourceManager.GetString("txtUpperMaterialCurve", Resources.resourceCulture);
			}
		}

		// Token: 0x170010A6 RID: 4262
		// (get) Token: 0x060033A4 RID: 13220 RVA: 0x00145708 File Offset: 0x00143908
		public static string txtUptToHundered
		{
			get
			{
				return Resources.ResourceManager.GetString("txtUptToHundered", Resources.resourceCulture);
			}
		}

		// Token: 0x170010A7 RID: 4263
		// (get) Token: 0x060033A5 RID: 13221 RVA: 0x00145730 File Offset: 0x00143930
		public static string txtUseFollowingDNS
		{
			get
			{
				return Resources.ResourceManager.GetString("txtUseFollowingDNS", Resources.resourceCulture);
			}
		}

		// Token: 0x170010A8 RID: 4264
		// (get) Token: 0x060033A6 RID: 13222 RVA: 0x00145758 File Offset: 0x00143958
		public static string txtUseFollowingIP
		{
			get
			{
				return Resources.ResourceManager.GetString("txtUseFollowingIP", Resources.resourceCulture);
			}
		}

		// Token: 0x170010A9 RID: 4265
		// (get) Token: 0x060033A7 RID: 13223 RVA: 0x00145780 File Offset: 0x00143980
		public static string txtUser
		{
			get
			{
				return Resources.ResourceManager.GetString("txtUser", Resources.resourceCulture);
			}
		}

		// Token: 0x170010AA RID: 4266
		// (get) Token: 0x060033A8 RID: 13224 RVA: 0x001457A8 File Offset: 0x001439A8
		public static string txtUserGroup
		{
			get
			{
				return Resources.ResourceManager.GetString("txtUserGroup", Resources.resourceCulture);
			}
		}

		// Token: 0x170010AB RID: 4267
		// (get) Token: 0x060033A9 RID: 13225 RVA: 0x001457D0 File Offset: 0x001439D0
		public static string txtUserID
		{
			get
			{
				return Resources.ResourceManager.GetString("txtUserID", Resources.resourceCulture);
			}
		}

		// Token: 0x170010AC RID: 4268
		// (get) Token: 0x060033AA RID: 13226 RVA: 0x001457F8 File Offset: 0x001439F8
		public static string txtUserName
		{
			get
			{
				return Resources.ResourceManager.GetString("txtUserName", Resources.resourceCulture);
			}
		}

		// Token: 0x170010AD RID: 4269
		// (get) Token: 0x060033AB RID: 13227 RVA: 0x00145820 File Offset: 0x00143A20
		public static string txtVA
		{
			get
			{
				return Resources.ResourceManager.GetString("txtVA", Resources.resourceCulture);
			}
		}

		// Token: 0x170010AE RID: 4270
		// (get) Token: 0x060033AC RID: 13228 RVA: 0x00145848 File Offset: 0x00143A48
		public static string txtVAGlass
		{
			get
			{
				return Resources.ResourceManager.GetString("txtVAGlass", Resources.resourceCulture);
			}
		}

		// Token: 0x170010AF RID: 4271
		// (get) Token: 0x060033AD RID: 13229 RVA: 0x00145870 File Offset: 0x00143A70
		public static string txtValidation
		{
			get
			{
				return Resources.ResourceManager.GetString("txtValidation", Resources.resourceCulture);
			}
		}

		// Token: 0x170010B0 RID: 4272
		// (get) Token: 0x060033AE RID: 13230 RVA: 0x00145898 File Offset: 0x00143A98
		public static string txtValidationFailed
		{
			get
			{
				return Resources.ResourceManager.GetString("txtValidationFailed", Resources.resourceCulture);
			}
		}

		// Token: 0x170010B1 RID: 4273
		// (get) Token: 0x060033AF RID: 13231 RVA: 0x001458C0 File Offset: 0x00143AC0
		public static string txtValidationFailure
		{
			get
			{
				return Resources.ResourceManager.GetString("txtValidationFailure", Resources.resourceCulture);
			}
		}

		// Token: 0x170010B2 RID: 4274
		// (get) Token: 0x060033B0 RID: 13232 RVA: 0x001458E8 File Offset: 0x00143AE8
		public static string txtValidationIsDue
		{
			get
			{
				return Resources.ResourceManager.GetString("txtValidationIsDue", Resources.resourceCulture);
			}
		}

		// Token: 0x170010B3 RID: 4275
		// (get) Token: 0x060033B1 RID: 13233 RVA: 0x00145910 File Offset: 0x00143B10
		public static string txtValidationOk
		{
			get
			{
				return Resources.ResourceManager.GetString("txtValidationOk", Resources.resourceCulture);
			}
		}

		// Token: 0x170010B4 RID: 4276
		// (get) Token: 0x060033B2 RID: 13234 RVA: 0x00145938 File Offset: 0x00143B38
		public static string txtValidationSuccess
		{
			get
			{
				return Resources.ResourceManager.GetString("txtValidationSuccess", Resources.resourceCulture);
			}
		}

		// Token: 0x170010B5 RID: 4277
		// (get) Token: 0x060033B3 RID: 13235 RVA: 0x00145960 File Offset: 0x00143B60
		public static string txtValidationWasDue
		{
			get
			{
				return Resources.ResourceManager.GetString("txtValidationWasDue", Resources.resourceCulture);
			}
		}

		// Token: 0x170010B6 RID: 4278
		// (get) Token: 0x060033B4 RID: 13236 RVA: 0x00145988 File Offset: 0x00143B88
		public static string txtValve
		{
			get
			{
				return Resources.ResourceManager.GetString("txtValve", Resources.resourceCulture);
			}
		}

		// Token: 0x170010B7 RID: 4279
		// (get) Token: 0x060033B5 RID: 13237 RVA: 0x001459B0 File Offset: 0x00143BB0
		public static string txtValvesInverted
		{
			get
			{
				return Resources.ResourceManager.GetString("txtValvesInverted", Resources.resourceCulture);
			}
		}

		// Token: 0x170010B8 RID: 4280
		// (get) Token: 0x060033B6 RID: 13238 RVA: 0x001459D8 File Offset: 0x00143BD8
		public static string txtValveStartPixel
		{
			get
			{
				return Resources.ResourceManager.GetString("txtValveStartPixel", Resources.resourceCulture);
			}
		}

		// Token: 0x170010B9 RID: 4281
		// (get) Token: 0x060033B7 RID: 13239 RVA: 0x00145A00 File Offset: 0x00143C00
		public static string txtValveWidth
		{
			get
			{
				return Resources.ResourceManager.GetString("txtValveWidth", Resources.resourceCulture);
			}
		}

		// Token: 0x170010BA RID: 4282
		// (get) Token: 0x060033B8 RID: 13240 RVA: 0x00145A28 File Offset: 0x00143C28
		public static string txtVAWire
		{
			get
			{
				return Resources.ResourceManager.GetString("txtVAWire", Resources.resourceCulture);
			}
		}

		// Token: 0x170010BB RID: 4283
		// (get) Token: 0x060033B9 RID: 13241 RVA: 0x00145A50 File Offset: 0x00143C50
		public static string txtVersionCheckSensitivityPrediction
		{
			get
			{
				return Resources.ResourceManager.GetString("txtVersionCheckSensitivityPrediction", Resources.resourceCulture);
			}
		}

		// Token: 0x170010BC RID: 4284
		// (get) Token: 0x060033BA RID: 13242 RVA: 0x00145A78 File Offset: 0x00143C78
		public static string txtVibro
		{
			get
			{
				return Resources.ResourceManager.GetString("txtVibro", Resources.resourceCulture);
			}
		}

		// Token: 0x170010BD RID: 4285
		// (get) Token: 0x060033BB RID: 13243 RVA: 0x00145AA0 File Offset: 0x00143CA0
		public static string txtVibroSpeed
		{
			get
			{
				return Resources.ResourceManager.GetString("txtVibroSpeed", Resources.resourceCulture);
			}
		}

		// Token: 0x170010BE RID: 4286
		// (get) Token: 0x060033BC RID: 13244 RVA: 0x00145AC8 File Offset: 0x00143CC8
		public static string txtVoltage
		{
			get
			{
				return Resources.ResourceManager.GetString("txtVoltage", Resources.resourceCulture);
			}
		}

		// Token: 0x170010BF RID: 4287
		// (get) Token: 0x060033BD RID: 13245 RVA: 0x00145AF0 File Offset: 0x00143CF0
		public static string txtWagoVersionNotCompatible
		{
			get
			{
				return Resources.ResourceManager.GetString("txtWagoVersionNotCompatible", Resources.resourceCulture);
			}
		}

		// Token: 0x170010C0 RID: 4288
		// (get) Token: 0x060033BE RID: 13246 RVA: 0x00145B18 File Offset: 0x00143D18
		public static string txtWaitForCalibration
		{
			get
			{
				return Resources.ResourceManager.GetString("txtWaitForCalibration", Resources.resourceCulture);
			}
		}

		// Token: 0x170010C1 RID: 4289
		// (get) Token: 0x060033BF RID: 13247 RVA: 0x00145B40 File Offset: 0x00143D40
		public static string txtWaitingForCalibration
		{
			get
			{
				return Resources.ResourceManager.GetString("txtWaitingForCalibration", Resources.resourceCulture);
			}
		}

		// Token: 0x170010C2 RID: 4290
		// (get) Token: 0x060033C0 RID: 13248 RVA: 0x00145B68 File Offset: 0x00143D68
		public static string txtWarning
		{
			get
			{
				return Resources.ResourceManager.GetString("txtWarning", Resources.resourceCulture);
			}
		}

		// Token: 0x170010C3 RID: 4291
		// (get) Token: 0x060033C1 RID: 13249 RVA: 0x00145B90 File Offset: 0x00143D90
		public static string txtWarningLimitXRayOperatingHours
		{
			get
			{
				return Resources.ResourceManager.GetString("txtWarningLimitXRayOperatingHours", Resources.resourceCulture);
			}
		}

		// Token: 0x170010C4 RID: 4292
		// (get) Token: 0x060033C2 RID: 13250 RVA: 0x00145BB8 File Offset: 0x00143DB8
		public static string txtWarningResetAuditCheck
		{
			get
			{
				return Resources.ResourceManager.GetString("txtWarningResetAuditCheck", Resources.resourceCulture);
			}
		}

		// Token: 0x170010C5 RID: 4293
		// (get) Token: 0x060033C3 RID: 13251 RVA: 0x00145BE0 File Offset: 0x00143DE0
		public static string txtWednesday
		{
			get
			{
				return Resources.ResourceManager.GetString("txtWednesday", Resources.resourceCulture);
			}
		}

		// Token: 0x170010C6 RID: 4294
		// (get) Token: 0x060033C4 RID: 13252 RVA: 0x00145C08 File Offset: 0x00143E08
		public static string txtWeekly
		{
			get
			{
				return Resources.ResourceManager.GetString("txtWeekly", Resources.resourceCulture);
			}
		}

		// Token: 0x170010C7 RID: 4295
		// (get) Token: 0x060033C5 RID: 13253 RVA: 0x00145C30 File Offset: 0x00143E30
		public static string txtWeigherActive
		{
			get
			{
				return Resources.ResourceManager.GetString("txtWeigherActive", Resources.resourceCulture);
			}
		}

		// Token: 0x170010C8 RID: 4296
		// (get) Token: 0x060033C6 RID: 13254 RVA: 0x00145C58 File Offset: 0x00143E58
		public static string txtWeight
		{
			get
			{
				return Resources.ResourceManager.GetString("txtWeight", Resources.resourceCulture);
			}
		}

		// Token: 0x170010C9 RID: 4297
		// (get) Token: 0x060033C7 RID: 13255 RVA: 0x00145C80 File Offset: 0x00143E80
		public static string txtWeightAuditCheck
		{
			get
			{
				return Resources.ResourceManager.GetString("txtWeightAuditCheck", Resources.resourceCulture);
			}
		}

		// Token: 0x170010CA RID: 4298
		// (get) Token: 0x060033C8 RID: 13256 RVA: 0x00145CA8 File Offset: 0x00143EA8
		public static string txtWeightRight
		{
			get
			{
				return Resources.ResourceManager.GetString("txtWeightRight", Resources.resourceCulture);
			}
		}

		// Token: 0x170010CB RID: 4299
		// (get) Token: 0x060033C9 RID: 13257 RVA: 0x00145CD0 File Offset: 0x00143ED0
		public static string txtWeightUnit
		{
			get
			{
				return Resources.ResourceManager.GetString("txtWeightUnit", Resources.resourceCulture);
			}
		}

		// Token: 0x170010CC RID: 4300
		// (get) Token: 0x060033CA RID: 13258 RVA: 0x00145CF8 File Offset: 0x00143EF8
		public static string txtWidth
		{
			get
			{
				return Resources.ResourceManager.GetString("txtWidth", Resources.resourceCulture);
			}
		}

		// Token: 0x170010CD RID: 4301
		// (get) Token: 0x060033CB RID: 13259 RVA: 0x00145D20 File Offset: 0x00143F20
		public static string txtWillBeStarted
		{
			get
			{
				return Resources.ResourceManager.GetString("txtWillBeStarted", Resources.resourceCulture);
			}
		}

		// Token: 0x170010CE RID: 4302
		// (get) Token: 0x060033CC RID: 13260 RVA: 0x00145D48 File Offset: 0x00143F48
		public static string txtWrongImages
		{
			get
			{
				return Resources.ResourceManager.GetString("txtWrongImages", Resources.resourceCulture);
			}
		}

		// Token: 0x170010CF RID: 4303
		// (get) Token: 0x060033CD RID: 13261 RVA: 0x00145D70 File Offset: 0x00143F70
		public static string txtWrongImageSizeDetected
		{
			get
			{
				return Resources.ResourceManager.GetString("txtWrongImageSizeDetected", Resources.resourceCulture);
			}
		}

		// Token: 0x170010D0 RID: 4304
		// (get) Token: 0x060033CE RID: 13262 RVA: 0x00145D98 File Offset: 0x00143F98
		public static string txtWrongInput
		{
			get
			{
				return Resources.ResourceManager.GetString("txtWrongInput", Resources.resourceCulture);
			}
		}

		// Token: 0x170010D1 RID: 4305
		// (get) Token: 0x060033CF RID: 13263 RVA: 0x00145DC0 File Offset: 0x00143FC0
		public static string txtWrongPixelTestcardSim
		{
			get
			{
				return Resources.ResourceManager.GetString("txtWrongPixelTestcardSim", Resources.resourceCulture);
			}
		}

		// Token: 0x170010D2 RID: 4306
		// (get) Token: 0x060033D0 RID: 13264 RVA: 0x00145DE8 File Offset: 0x00143FE8
		public static string txtXRayCurrent
		{
			get
			{
				return Resources.ResourceManager.GetString("txtXRayCurrent", Resources.resourceCulture);
			}
		}

		// Token: 0x170010D3 RID: 4307
		// (get) Token: 0x060033D1 RID: 13265 RVA: 0x00145E10 File Offset: 0x00144010
		public static string txtXRayPower
		{
			get
			{
				return Resources.ResourceManager.GetString("txtXRayPower", Resources.resourceCulture);
			}
		}

		// Token: 0x170010D4 RID: 4308
		// (get) Token: 0x060033D2 RID: 13266 RVA: 0x00145E38 File Offset: 0x00144038
		public static string txtXraysStart
		{
			get
			{
				return Resources.ResourceManager.GetString("txtXraysStart", Resources.resourceCulture);
			}
		}

		// Token: 0x170010D5 RID: 4309
		// (get) Token: 0x060033D3 RID: 13267 RVA: 0x00145E60 File Offset: 0x00144060
		public static string txtXRayVoltage
		{
			get
			{
				return Resources.ResourceManager.GetString("txtXRayVoltage", Resources.resourceCulture);
			}
		}

		// Token: 0x170010D6 RID: 4310
		// (get) Token: 0x060033D4 RID: 13268 RVA: 0x00145E88 File Offset: 0x00144088
		public static string txtYear
		{
			get
			{
				return Resources.ResourceManager.GetString("txtYear", Resources.resourceCulture);
			}
		}

		// Token: 0x170010D7 RID: 4311
		// (get) Token: 0x060033D5 RID: 13269 RVA: 0x00145EB0 File Offset: 0x001440B0
		public static string user_not_allowed_for_rfid_login
		{
			get
			{
				return Resources.ResourceManager.GetString("user_not_allowed_for_rfid_login", Resources.resourceCulture);
			}
		}

		// Token: 0x04001E01 RID: 7681
		private static ResourceManager resourceMan;

		// Token: 0x04001E02 RID: 7682
		private static CultureInfo resourceCulture;
	}
}
