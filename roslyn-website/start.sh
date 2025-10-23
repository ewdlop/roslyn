#!/bin/bash
# Roslyn 網站啟動腳本 (Bash)
# 自動啟動本地 HTTP 伺服器並開啟瀏覽器

echo "🚀 正在啟動 Roslyn 專案展示網站..."

# 檢查是否安裝 Python 3
if command -v python3 &> /dev/null; then
    echo "✓ 使用 Python 3 啟動本地伺服器"
    echo "📍 伺服器地址: http://localhost:8000"
    echo "⏹️  按 Ctrl+C 停止伺服器"
    echo ""
    
    # 在背景等待 2 秒後開啟瀏覽器
    (sleep 2 && xdg-open http://localhost:8000 2>/dev/null || open http://localhost:8000 2>/dev/null) &
    
    # 啟動 Python HTTP 伺服器
    python3 -m http.server 8000
    
# 檢查是否安裝 Python 2
elif command -v python &> /dev/null; then
    echo "✓ 使用 Python 2 啟動本地伺服器"
    echo "📍 伺服器地址: http://localhost:8000"
    echo "⏹️  按 Ctrl+C 停止伺服器"
    echo ""
    
    # 在背景等待 2 秒後開啟瀏覽器
    (sleep 2 && xdg-open http://localhost:8000 2>/dev/null || open http://localhost:8000 2>/dev/null) &
    
    # 啟動 Python HTTP 伺服器
    python -m SimpleHTTPServer 8000
else
    echo "❌ 未找到 Python"
    echo "請安裝 Python 或使用其他方式開啟網站："
    echo "1. 直接在瀏覽器中開啟 index.html"
    echo "2. 使用 VS Code Live Server 擴展"
    echo "3. 使用其他 HTTP 伺服器"
    echo ""
    read -p "按 Enter 鍵退出"
fi

