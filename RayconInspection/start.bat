@echo off
title Raycon X-Ray Inspection Tray Launcher
cd /d "%~dp0"

echo ========================================================
echo   KHOI DONG HE THONG RAYCON X-RAY INSPECTION
echo ========================================================
echo.
echo [1] Dang khoi dong Backend API va doi Port 5000 san sang...
echo [2] Tu dong khoi dong Frontend va mo trinh duyet...
echo [3] Thu gon toan bo vao Khay He Thong (System Tray)...
echo.

powershell -ExecutionPolicy Bypass -NoProfile -WindowStyle Hidden -File "%~dp0RayconTrayManager.ps1"
