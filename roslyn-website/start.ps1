# Roslyn 網站啟動腳本 (PowerShell)
# 自動啟動本地 HTTP 伺服器並開啟瀏覽器

Write-Host "🚀 正在啟動 Roslyn 專案展示網站..." -ForegroundColor Cyan

# 檢查是否安裝 Python
$pythonInstalled = Get-Command python -ErrorAction SilentlyContinue

if ($pythonInstalled) {
    Write-Host "✓ 使用 Python 啟動本地伺服器" -ForegroundColor Green
    Write-Host "📍 伺服器地址: http://localhost:8000" -ForegroundColor Yellow
    Write-Host "⏹️  按 Ctrl+C 停止伺服器" -ForegroundColor Yellow
    Write-Host ""
    
    # 等待 2 秒後開啟瀏覽器
    Start-Job -ScriptBlock {
        Start-Sleep -Seconds 2
        Start-Process "http://localhost:8000"
    }
    
    # 啟動 Python HTTP 伺服器
    python -m http.server 8000
} else {
    Write-Host "❌ 未找到 Python" -ForegroundColor Red
    Write-Host "請安裝 Python 或使用其他方式開啟網站：" -ForegroundColor Yellow
    Write-Host "1. 直接在瀏覽器中開啟 index.html" -ForegroundColor White
    Write-Host "2. 使用 VS Code Live Server 擴展" -ForegroundColor White
    Write-Host "3. 使用其他 HTTP 伺服器" -ForegroundColor White
    Write-Host ""
    Read-Host "按 Enter 鍵退出"
}

