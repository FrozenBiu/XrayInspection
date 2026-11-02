export interface RejectItem {
  id: string
  fileName: string
  createdAt: string
  productName: string
  width: number
  height: number
  conveyingSpeed: number
  sourceVoltage: number
  sourceCurrent: number
  isContaminant: boolean
  hasErrors: boolean
  imageUrl: string
  thumbnailUrl: string
  fileSizeBytes: string
}

export interface MachineStatus {
  machineId: string
  machineName: string
  watchDirectory: string
  totalLoaded: number
  serverTime: string
}

export interface ConfigModel {
  machineId: string
  machineName: string
  watchDirectory: string
}

