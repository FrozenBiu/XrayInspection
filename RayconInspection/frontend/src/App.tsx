import { useState, useEffect } from 'react'
import * as signalR from '@microsoft/signalr'
import { Toaster, toast } from 'sonner'
import type { RejectItem, MachineStatus } from './types/inspection'
import { Sidebar, type TabType } from './components/Sidebar'
import { OverviewPage } from './pages/OverviewPage'
import { LiveInspectionPage } from './pages/LiveInspectionPage'
import { HistoryArchivePage } from './pages/HistoryArchivePage'
import { SettingsPage } from './pages/SettingsPage'

export function App() {
  const [activeTab, setActiveTab] = useState<TabType>('overview')
  const [rejects, setRejects] = useState<RejectItem[]>([])
  const [selectedReject, setSelectedReject] = useState<RejectItem | null>(null)
  const [status, setStatus] = useState<MachineStatus | null>(null)
  const [isConnected, setIsConnected] = useState(false)

  const refreshData = async () => {
    try {
      const resStatus = await fetch('/api/status')
      if (resStatus.ok) {
        const data = await resStatus.json()
        setStatus(data)
      }
      
      const resRejects = await fetch('/api/rejects')
      if (resRejects.ok) {
        const data: RejectItem[] = await resRejects.json()
        setRejects(data)
        if (data.length > 0) {
          setSelectedReject(prev => prev ?? data[0])
        }
      }
    } catch {
      // Backend may still be initializing, retry seamlessly
      setTimeout(refreshData, 1500)
    }
  }

  // 1. Fetch initial status and rejects list
  useEffect(() => {
    refreshData()
  }, [])

  // 2. Setup SignalR Realtime connection
  useEffect(() => {
    const connection = new signalR.HubConnectionBuilder()
      .withUrl('/hubs/inspection')
      .withAutomaticReconnect([0, 1000, 2000, 3000, 5000])
      .build()

    connection.on('ConfigUpdated', () => {
      refreshData()
    })

    connection.on('ScanCompleted', () => {
      refreshData()
    })

    connection.on('NewReject', (item: RejectItem) => {
      console.log('Realtime NewReject received:', item)
      setRejects(prev => [item, ...prev])
      
      // If currently on Live tab, focus immediately
      setSelectedReject(item)

      // Alert via Sonner Toast (NO Javascript alert!)
      toast.error(`CẢNH BÁO REJECT: ${item.productName}`, {
        description: `Phát hiện lúc ${new Date(item.createdAt).toLocaleTimeString('vi-VN')} | ${item.sourceVoltage}kV`,
        duration: 7000,
        action: {
          label: 'Soi ảnh ngay',
          onClick: () => {
            setSelectedReject(item)
            setActiveTab('live')
          },
        },
      })
    })

    connection.start()
      .then(() => {
        setIsConnected(true)
        console.log('SignalR connected successfully')
      })
      .catch(err => {
        setIsConnected(false)
        console.error('SignalR connection error:', err)
      })

    connection.onclose(() => setIsConnected(false))
    connection.onreconnecting(() => setIsConnected(false))
    connection.onreconnected(() => setIsConnected(true))

    return () => {
      connection.stop()
    }
  }, [])

  const handleOpenLiveInspect = (item: RejectItem) => {
    setSelectedReject(item)
    setActiveTab('live')
  }

  return (
    <div className="flex h-screen w-screen overflow-hidden bg-slate-950 text-slate-100 font-sans">
      <Toaster position="top-right" richColors />

      {/* Left Sidebar Navigation */}
      <Sidebar
        activeTab={activeTab}
        onTabChange={setActiveTab}
        isConnected={isConnected}
        totalRejects={rejects.length}
      />

      {/* Main Content Pages */}
      <main className="flex-1 flex flex-col h-full overflow-hidden">
        {activeTab === 'overview' && (
          <OverviewPage
            rejects={rejects}
            status={status}
            onOpenLiveInspect={handleOpenLiveInspect}
          />
        )}

        {activeTab === 'live' && (
          <LiveInspectionPage
            rejects={rejects}
            selectedReject={selectedReject}
            onSelectReject={setSelectedReject}
          />
        )}

        {activeTab === 'history' && (
          <HistoryArchivePage
            rejects={rejects}
            onOpenLiveInspect={handleOpenLiveInspect}
          />
        )}

        {activeTab === 'settings' && (
          <SettingsPage
            status={status}
            isConnected={isConnected}
            onConfigUpdated={refreshData}
          />
        )}
      </main>
    </div>
  )
}

export default App
