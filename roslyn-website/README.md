# Roslyn 專案展示網站

這是一個全面介紹 **Roslyn (.NET 編譯器平台)** 的精美展示網站。

## 🎯 專案簡介

Roslyn 是 .NET 的開源 C# 和 Visual Basic 編譯器，提供豐富的程式碼分析 API。此網站詳細展示了 Roslyn 的架構、組件、功能和開發流程。

## 📁 檔案結構

```
roslyn-website/
├── index.html      # 主要 HTML 檔案
├── styles.css      # 完整樣式表
├── script.js       # 互動功能 JavaScript
└── README.md       # 專案說明文件（本檔案）
```

## 🚀 如何使用

### 方法 1: 直接開啟
1. 找到 `index.html` 檔案
2. 使用瀏覽器開啟（建議使用 Chrome、Firefox 或 Edge）

### 方法 2: 使用本地伺服器（推薦）
使用任何本地 HTTP 伺服器都可以，例如：

#### 使用 Python
```bash
# Python 3
python -m http.server 8000

# Python 2
python -m SimpleHTTPServer 8000
```

#### 使用 Node.js (http-server)
```bash
npx http-server -p 8000
```

#### 使用 PHP
```bash
php -S localhost:8000
```

然後在瀏覽器中訪問 `http://localhost:8000`

### 方法 3: 使用 VS Code Live Server
1. 在 VS Code 中安裝 "Live Server" 擴展
2. 右鍵點擊 `index.html`
3. 選擇 "Open with Live Server"

## 📋 網站功能

### 主要區域

1. **概覽 (Overview)**
   - 專案核心使命
   - 專案規模統計
   - 主要特色
   - 語言功能狀態

2. **架構 (Architecture)**
   - 分層編譯管線
   - API 分層架構
   - 架構特色說明

3. **組件 (Components)**
   - 編譯器組件
   - 工作區組件
   - 功能層組件
   - 編輯器功能
   - Visual Studio 整合
   - 語言伺服器協定

4. **功能 (Features)**
   - 語法樹 API
   - 語意分析
   - Source Generators
   - 診斷分析器
   - 程式碼重構
   - 效能最佳化
   - 語法著色
   - IntelliSense

5. **開發流程 (Workflow)**
   - 建置專案
   - 執行測試
   - 程式碼格式化
   - 除錯編譯器
   - 最佳實踐

6. **社群 (Community)**
   - GitHub Repository
   - 討論區
   - Discord
   - 文件資源
   - 貢獻指南

### 互動功能

- ✨ 平滑滾動導航
- 🎯 標籤頁切換
- 📋 程式碼複製按鈕
- 🔄 滾動動畫
- 📊 統計數字動畫
- ⬆️ 回到頂部按鈕
- 🎨 響應式設計
- ⌨️ 鍵盤導航支援

## 🎨 設計特色

- **現代化 UI**: 採用深色主題和漸層色彩
- **響應式設計**: 適配各種螢幕尺寸
- **流暢動畫**: 優雅的過渡和互動效果
- **專業排版**: 清晰的資訊層次
- **程式碼展示**: 精美的程式碼區塊樣式

## 🛠️ 技術細節

### 使用的技術
- **HTML5**: 語意化標記
- **CSS3**: 
  - CSS Grid & Flexbox 佈局
  - CSS 變數（自訂屬性）
  - 漸層和動畫
  - 媒體查詢（響應式）
- **Vanilla JavaScript**: 
  - DOM 操作
  - Intersection Observer API
  - 平滑滾動
  - 事件處理

### 字體
- **Inter**: 主要內容字體
- **JetBrains Mono**: 程式碼字體

### 顏色方案
- 主色調: `#512bd4` (紫色)
- 次要色: `#00d4ff` (青色)
- 深色背景: `#0d1117`
- 卡片背景: `#161b22`

## 📱 瀏覽器相容性

- ✅ Chrome/Edge (最新版本)
- ✅ Firefox (最新版本)
- ✅ Safari (最新版本)
- ✅ Opera (最新版本)

## 🔧 自訂修改

### 修改顏色
編輯 `styles.css` 中的 CSS 變數：
```css
:root {
    --primary-color: #512bd4;
    --secondary-color: #00d4ff;
    /* 其他顏色... */
}
```

### 修改內容
編輯 `index.html` 中的相應區塊。

### 修改互動
編輯 `script.js` 中的 JavaScript 函數。

## 📊 專案資訊來源

所有資料基於 Roslyn GitHub Repository：
- 主要 Repository: https://github.com/dotnet/roslyn
- C# 語言設計: https://github.com/dotnet/csharplang
- VB 語言設計: https://github.com/dotnet/vblang

## 🤝 貢獻

這是一個展示性專案。如果您想貢獻到實際的 Roslyn 專案，請訪問：
https://github.com/dotnet/roslyn/blob/main/CONTRIBUTING.md

## 📄 授權

此展示網站僅供教育和展示目的。

Roslyn 專案本身採用 MIT 授權。
© Microsoft Corporation

## 📞 聯絡資訊

對於 Roslyn 專案的問題或貢獻：
- GitHub Issues: https://github.com/dotnet/roslyn/issues
- Discussions: https://github.com/dotnet/roslyn/discussions
- Discord: https://discord.com/invite/tGJvv88

## 🎉 致謝

感謝 Microsoft 和所有 Roslyn 貢獻者讓這個強大的編譯器平台成為可能。

---

**建立日期**: 2025 年 10 月
**版本**: 1.0
**狀態**: ✅ 完成

