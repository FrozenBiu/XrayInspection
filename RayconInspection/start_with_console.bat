@echo off
title Raycon X-Ray Inspection Console Manager
cd /d "%~dp0"

echo ========================================================
echo   KHOI DONG HE THONG RAYCON X-RAY INSPECTION
echo ========================================================
echo.

powershell -ExecutionPolicy Bypass -NoProfile -File "%~dp0RayconTrayManager.ps1"

