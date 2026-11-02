using System;

// Token: 0x02000003 RID: 3
public enum SSTEntryCode
{
	// Token: 0x0400000A RID: 10
	Metal = 1,
	// Token: 0x0400000B RID: 11
	DetectorComm,
	// Token: 0x0400000C RID: 12
	CalibrationFault,
	// Token: 0x0400000D RID: 13
	XRayComm,
	// Token: 0x0400000E RID: 14
	XRayFault,
	// Token: 0x0400000F RID: 15
	ProcessingDelay,
	// Token: 0x04000010 RID: 16
	MemoryUsage,
	// Token: 0x04000011 RID: 17
	MotorError,
	// Token: 0x04000012 RID: 18
	LowAirPressure,
	// Token: 0x04000013 RID: 19
	CoolingError,
	// Token: 0x04000014 RID: 20
	BinFull,
	// Token: 0x04000015 RID: 21
	ParamComm,
	// Token: 0x04000016 RID: 22
	ImageComm,
	// Token: 0x04000017 RID: 23
	IOComm,
	// Token: 0x04000018 RID: 24
	IOController,
	// Token: 0x04000019 RID: 25
	RejectVerify,
	// Token: 0x0400001A RID: 26
	XRayLamp,
	// Token: 0x0400001B RID: 27
	Temperature,
	// Token: 0x0400001C RID: 28
	Voltage,
	// Token: 0x0400001D RID: 29
	ImageProcPC,
	// Token: 0x0400001E RID: 30
	LightBarrier,
	// Token: 0x0400001F RID: 31
	SVTComm,
	// Token: 0x04000020 RID: 32
	SVTError,
	// Token: 0x04000021 RID: 33
	AuditFailedError = 25,
	// Token: 0x04000022 RID: 34
	XRayMotor = 24,
	// Token: 0x04000023 RID: 35
	XrayArcs = 26,
	// Token: 0x04000024 RID: 36
	ProductsMissing,
	// Token: 0x04000025 RID: 37
	SensorBlockedPhotogating,
	// Token: 0x04000026 RID: 38
	SensorBlockedRejectCheck,
	// Token: 0x04000027 RID: 39
	ErrorConsistency,
	// Token: 0x04000028 RID: 40
	BinMissing,
	// Token: 0x04000029 RID: 41
	ProductNotInspected,
	// Token: 0x0400002A RID: 42
	TooManyRejects,
	// Token: 0x0400002B RID: 43
	InsightNetConnection,
	// Token: 0x0400002C RID: 44
	DiskSpaceLow,
	// Token: 0x0400002D RID: 45
	InOutletSensorBlocked,
	// Token: 0x0400002E RID: 46
	MeanValueHELEforDual,
	// Token: 0x0400002F RID: 47
	ErrorXRayOperatingHours,
	// Token: 0x04000030 RID: 48
	Printer,
	// Token: 0x04000031 RID: 49
	SafetySwitchOpenError,
	// Token: 0x04000032 RID: 50
	EmergencyStopPressedError,
	// Token: 0x04000033 RID: 51
	ErrorActSensTestcardSim,
	// Token: 0x04000034 RID: 52
	LimitsError,
	// Token: 0x04000035 RID: 53
	BackgroundMonitoringError,
	// Token: 0x04000036 RID: 54
	AIDeploymentError,
	// Token: 0x04000037 RID: 55
	AIInferenceError,
	// Token: 0x04000038 RID: 56
	NoConnectionToExternalStorageLoc,
	// Token: 0x04000039 RID: 57
	ImageNotSaveToExternalStorageLoc,
	// Token: 0x0400003A RID: 58
	PathExternalStorageLocTooLong,
	// Token: 0x0400003B RID: 59
	Limit100PercentExternalStorageLoc,
	// Token: 0x0400003C RID: 60
	DatabaseVersion = 64,
	// Token: 0x0400003D RID: 61
	WagoOutOfDate,
	// Token: 0x0400003E RID: 62
	WagoVersionWrong,
	// Token: 0x0400003F RID: 63
	VibroInManualMode,
	// Token: 0x04000040 RID: 64
	WarningXRayOperatingHours,
	// Token: 0x04000041 RID: 65
	WarningActSensTestcardSim,
	// Token: 0x04000042 RID: 66
	IVPCancelled,
	// Token: 0x04000043 RID: 67
	IVPNeeded,
	// Token: 0x04000044 RID: 68
	LimitsWarning,
	// Token: 0x04000045 RID: 69
	Limits90PerWarning,
	// Token: 0x04000046 RID: 70
	BackgroundMonitoringWarning,
	// Token: 0x04000047 RID: 71
	ElinosVersion,
	// Token: 0x04000048 RID: 72
	Limit90PercentExternalStorageLoc,
	// Token: 0x04000049 RID: 73
	MachineTurnedOn = 128,
	// Token: 0x0400004A RID: 74
	MachineTurnedOff,
	// Token: 0x0400004B RID: 75
	ProductChanged,
	// Token: 0x0400004C RID: 76
	ProductEnded,
	// Token: 0x0400004D RID: 77
	ProductCreated,
	// Token: 0x0400004E RID: 78
	ProductCopied,
	// Token: 0x0400004F RID: 79
	ProductDeleted,
	// Token: 0x04000050 RID: 80
	ProductRenamed,
	// Token: 0x04000051 RID: 81
	ProductNumberChanged,
	// Token: 0x04000052 RID: 82
	SafetySwitchOpenInfo,
	// Token: 0x04000053 RID: 83
	EmergencyStopPressedInfo,
	// Token: 0x04000054 RID: 84
	AutotrainStarted,
	// Token: 0x04000055 RID: 85
	AutotrainEnded,
	// Token: 0x04000056 RID: 86
	AutotrainAborted,
	// Token: 0x04000057 RID: 87
	Retrain,
	// Token: 0x04000058 RID: 88
	ConveyorSpeed,
	// Token: 0x04000059 RID: 89
	RatedConveyorSpeed,
	// Token: 0x0400005A RID: 90
	FilterActivated,
	// Token: 0x0400005B RID: 91
	FilterDeactivated,
	// Token: 0x0400005C RID: 92
	EdgeParameterChanged,
	// Token: 0x0400005D RID: 93
	SystemTimeChanged,
	// Token: 0x0400005E RID: 94
	CounterParameterChanged,
	// Token: 0x0400005F RID: 95
	MetalDuringAudit,
	// Token: 0x04000060 RID: 96
	XrayTurnedOn,
	// Token: 0x04000061 RID: 97
	XrayTurnedOff,
	// Token: 0x04000062 RID: 98
	WeigherParameterChanged,
	// Token: 0x04000063 RID: 99
	ParameterChanged,
	// Token: 0x04000064 RID: 100
	UserLoggedIn,
	// Token: 0x04000065 RID: 101
	UserLoggedOut,
	// Token: 0x04000066 RID: 102
	UserChanged,
	// Token: 0x04000067 RID: 103
	UserCreated,
	// Token: 0x04000068 RID: 104
	UserParameterChanged,
	// Token: 0x04000069 RID: 105
	UserDeleted,
	// Token: 0x0400006A RID: 106
	RetrainGroup,
	// Token: 0x0400006B RID: 107
	RetrainGroupFailed,
	// Token: 0x0400006C RID: 108
	SensitivityChanged,
	// Token: 0x0400006D RID: 109
	AuditParameterChanged,
	// Token: 0x0400006E RID: 110
	AuditActivated,
	// Token: 0x0400006F RID: 111
	AuditDeactivated,
	// Token: 0x04000070 RID: 112
	AuditNeeded,
	// Token: 0x04000071 RID: 113
	AuditOK,
	// Token: 0x04000072 RID: 114
	AuditFailedInfo,
	// Token: 0x04000073 RID: 115
	AuditStart,
	// Token: 0x04000074 RID: 116
	CheckpointRestored,
	// Token: 0x04000075 RID: 117
	CheckpointDeleted,
	// Token: 0x04000076 RID: 118
	CheckpointCreated,
	// Token: 0x04000077 RID: 119
	ProductDuringTest,
	// Token: 0x04000078 RID: 120
	BatchEnded,
	// Token: 0x04000079 RID: 121
	WeightStat,
	// Token: 0x0400007A RID: 122
	BatchChanged,
	// Token: 0x0400007B RID: 123
	MachineEnableExtern,
	// Token: 0x0400007C RID: 124
	InfoInitSensTestcardSim,
	// Token: 0x0400007D RID: 125
	InfoActSensTestcardSim,
	// Token: 0x0400007E RID: 126
	IVPActivated = 182,
	// Token: 0x0400007F RID: 127
	IVPDeactivated,
	// Token: 0x04000080 RID: 128
	IVPTestballFinished,
	// Token: 0x04000081 RID: 129
	IVPOK,
	// Token: 0x04000082 RID: 130
	IVPFailed,
	// Token: 0x04000083 RID: 131
	IVPStarted,
	// Token: 0x04000084 RID: 132
	AuditTestballFinished,
	// Token: 0x04000085 RID: 133
	UndoRetrain,
	// Token: 0x04000086 RID: 134
	LimitsInfo,
	// Token: 0x04000087 RID: 135
	Limits90PerInfo,
	// Token: 0x04000088 RID: 136
	MonitoringActivated,
	// Token: 0x04000089 RID: 137
	MonitoringDeactivated,
	// Token: 0x0400008A RID: 138
	BackgroundMonitoringInfo,
	// Token: 0x0400008B RID: 139
	ICMsuccesful,
	// Token: 0x0400008C RID: 140
	SortingSwitchedOn,
	// Token: 0x0400008D RID: 141
	SortingSwitchedOff,
	// Token: 0x0400008E RID: 142
	FastAuditCheck,
	// Token: 0x0400008F RID: 143
	SafetyCircutEx2A,
	// Token: 0x04000090 RID: 144
	AIDetectionActivated,
	// Token: 0x04000091 RID: 145
	AIDetectionDeactivated,
	// Token: 0x04000092 RID: 146
	ObjectDetectionParameterChanged
}
