# Roslyn (.NET 編譯器平台) - 完整專案摘要

## 📋 專案概述

**Roslyn** 是 Microsoft 開發的開源 .NET 編譯器平台，為 C# 和 Visual Basic 提供完整的編譯器實現和豐富的程式碼分析 API。

- **專案名稱**: .NET Compiler Platform ("Roslyn")
- **授權**: MIT License
- **組織**: .NET Foundation
- **主要語言**: C#, Visual Basic
- **GitHub**: https://github.com/dotnet/roslyn

---

## 🎯 核心使命

Roslyn 打破傳統編譯器的「黑盒子」模式，將編譯器轉變為開放平台：

**傳統編譯器**:
```
原始碼 → [黑盒子處理] → 目的碼
          ↑
    知識被遺忘
```

**Roslyn 編譯器**:
```
原始碼 → [開放 API] → 目的碼
          ↓
    所有知識可供使用
    • 語法樹
    • 符號表
    • 語意模型
    • 診斷資訊
```

這使得 IntelliSense、重構、程式碼分析、源生成器等工具成為可能。

---

## 📊 專案規模

### 檔案統計

| 組件 | 檔案數 | C# | VB | 其他 |
|------|--------|-----|-----|------|
| **Compilers** | 5,263 | 3,310 | 1,475 | 478 |
| **Workspaces** | 2,646 | 2,039 | 256 | 351 |
| **Features** | 2,895 | 2,207 | 574 | 114 |
| **EditorFeatures** | 2,285 | 1,496 | 665 | 124 |
| **VisualStudio** | 2,123 | 1,504 | 314 | 305 |
| **LanguageServer** | 1,138 | 1,074 | 0 | 64 |
| **Analyzers** | 1,011 | 723 | 188 | 100 |
| **RoslynAnalyzers** | 888 | 565 | 51 | 272 |
| **ExpressionEvaluator** | 423 | 277 | 86 | 60 |
| **Tools** | 406 | 282 | 0 | 124 |
| **Scripting** | 166 | 91 | 16 | 59 |
| **Dependencies** | 160 | 143 | 0 | 17 |

**總計**: 約 **20,000+ 原始碼檔案**

### 程式碼庫結構

```
roslyn/
├── src/
│   ├── Compilers/         # C# & VB 編譯器
│   ├── Workspaces/        # 工作區 API
│   ├── Features/          # IDE 功能
│   ├── EditorFeatures/    # 編輯器整合
│   ├── VisualStudio/      # VS 整合
│   ├── LanguageServer/    # LSP 實現
│   ├── Analyzers/         # 分析器
│   ├── RoslynAnalyzers/   # Roslyn 自身分析器
│   ├── ExpressionEvaluator/ # 除錯器運算式求值
│   ├── Scripting/         # 腳本 API
│   ├── Tools/             # 工具
│   └── Dependencies/      # 高效能集合
├── docs/                  # 文件
├── eng/                   # 工程建置腳本
├── artifacts/             # 建置輸出
└── scripts/               # 實用腳本
```

---

## 🏗️ 架構詳解

### 編譯管線

Roslyn 採用分層的編譯管線架構：

```
1️⃣ 詞法分析 (Lexer)
   ↓ 將原始碼轉換為標記 (Token)
   
2️⃣ 語法分析 (Parser)
   ↓ 建立語法樹 (Syntax Tree)
   
3️⃣ 宣告分析 (Declaration)
   ↓ 形成符號表 (Symbol Table)
   
4️⃣ 綁定 (Binding)
   ↓ 將識別碼與符號匹配
   
5️⃣ 語意分析 (Semantic Analysis)
   ↓ 類型檢查和驗證
   
6️⃣ 降低/重寫 (Lowering)
   ↓ 轉換高階結構為低階形式
   
7️⃣ 發出 (Emit)
   → 生成 IL 位元組碼
```

### API 分層

```
┌─────────────────────────────────┐
│   Workspaces APIs               │  ← 解決方案/專案管理
├─────────────────────────────────┤
│   Compiler APIs                 │  ← 語法、符號、語意
├─────────────────────────────────┤
│   Core Infrastructure           │  ← 基礎設施
└─────────────────────────────────┘
```

**Workspaces APIs**:
- `Workspace` - 解決方案的抽象
- `Solution` - 專案的集合
- `Project` - 文件和參考的集合
- `Document` - 單一原始碼檔案

**Compiler APIs**:
- `SyntaxTree` - 語法樹
- `Compilation` - 編譯實例
- `SemanticModel` - 語意資訊
- `ISymbol` - 符號資訊

---

## 🧩 主要組件

### 1. Compilers 組件
**路徑**: `src/Compilers/`

**功能**:
- C# 編譯器實現
- VB 編譯器實現
- 語法分析和語法樹建立
- 語意分析和符號表
- IL 程式碼生成
- 診斷和錯誤報告

**關鍵目錄**:
- `Core/Portable/` - 語言無關基礎設施
- `CSharp/Portable/` - C# 編譯器
- `VisualBasic/Portable/` - VB 編譯器

### 2. Workspaces 組件
**路徑**: `src/Workspaces/`

**功能**:
- 解決方案和專案模型
- 文件管理和追蹤
- 主機服務整合
- MSBuild 整合
- 持久性儲存

### 3. Features 組件
**路徑**: `src/Features/`

**功能**:
- 程式碼完成 (IntelliSense)
- 程式碼重構
- 程式碼修復
- 尋找參考
- 重新命名
- 導航功能
- 診斷分析器

### 4. EditorFeatures 組件
**路徑**: `src/EditorFeatures/`

**功能**:
- 文字緩衝區管理
- 語法著色
- 錯誤波浪線
- 即時程式碼分析
- 程式碼摺疊
- 括號匹配

### 5. VisualStudio 組件
**路徑**: `src/VisualStudio/`

**功能**:
- Visual Studio 語言服務
- 專案系統整合
- 工具視窗
- 選項頁面
- 命令處理
- UI 組件

### 6. LanguageServer 組件
**路徑**: `src/LanguageServer/`

**功能**:
- 語言伺服器協定 (LSP) 實現
- 跨編輯器相容性
- VS Code 整合
- 文件同步
- 自動完成
- 診斷

---

## ✨ 核心功能

### 1. 語法樹 API
```csharp
var tree = CSharpSyntaxTree.ParseText(code);
var root = await tree.GetRootAsync();
var nodes = root.DescendantNodes();
```

**特色**:
- 完整保真度（保留所有原始碼資訊）
- 不可變結構
- 高效增量更新
- 包含 Token、Node、Trivia

### 2. 語意模型
```csharp
var compilation = CSharpCompilation.Create(...)
    .AddReferences(references)
    .AddSyntaxTrees(tree);
    
var model = compilation.GetSemanticModel(tree);
var symbolInfo = model.GetSymbolInfo(node);
var typeInfo = model.GetTypeInfo(expression);
```

**提供**:
- 符號解析
- 類型資訊
- 資料流分析
- 控制流分析

### 3. Source Generators
```csharp
[Generator]
public class MyGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // 在編譯時生成程式碼
    }
}
```

**特色**:
- 編譯時程式碼生成
- 增量計算支援
- 與 IDE 整合

### 4. 診斷分析器
```csharp
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class MyAnalyzer : DiagnosticAnalyzer
{
    public override void Initialize(AnalysisContext context)
    {
        context.RegisterSyntaxNodeAction(AnalyzeNode, 
            SyntaxKind.MethodDeclaration);
    }
}
```

**提供**:
- 自訂程式碼分析
- 程式碼修復
- 重構建議
- 與建置整合

### 5. 程式碼重構
```csharp
[ExportCodeRefactoringProvider]
public class MyRefactoring : CodeRefactoringProvider
{
    public override async Task ComputeRefactoringsAsync(
        CodeRefactoringContext context)
    {
        // 提供重構選項
    }
}
```

### 6. 腳本 API
```csharp
var script = CSharpScript.Create("Console.WriteLine(\"Hello\")");
var result = await script.RunAsync();
```

---

## 🔧 開發工作流程

### 建置專案

**Windows**:
```powershell
.\Build.cmd
```

**Linux/macOS**:
```bash
./build.sh
```

**特定解決方案**:
```bash
dotnet build Compilers.slnf  # 只建置編譯器
dotnet build Ide.slnf        # 只建置 IDE 功能
```

### 執行測試

```bash
# 所有測試
.\Test.cmd
./test.sh

# 特定測試專案
dotnet test src/Compilers/CSharp/Test/Syntax/

# 特定測試
dotnet test --filter "FullyQualifiedName~MyTestClass"
```

### 程式碼格式化

```bash
# 格式化單一檔案
dotnet format whitespace --folder . --include path/to/file.cs

# 驗證格式
.\eng\validate-code-formatting.ps1

# 更新本地化檔案
dotnet msbuild path/to/project.csproj /t:UpdateXlf
```

---

## 🎓 語言功能狀態

### C# 14.0 (進行中)

| 功能 | 狀態 | 說明 |
|------|------|------|
| `field` 關鍵字 | 已合併 | 屬性中的欄位關鍵字 |
| 一級 Span 類型 | 已合併 | 改進的 Span 支援 |
| 字典表達式 | 進行中 | 簡化的字典建立語法 |
| 集合表達式參數 | 進行中 | 擴展集合表達式 |
| 部分事件和建構函式 | 已合併 | 更多部分成員支援 |
| Extensions | 已合併 | 擴展類型系統 |
| Null 條件賦值 | 已合併 | `x?.y = z` 語法 |

### C# 13.0 (已發布)

- Escape 字元
- Lock 物件改進
- 隱式索引器存取
- Params 集合
- Ref/unsafe 在迭代器/async 中
- 覆寫優先順序
- 部分屬性

### C# 12.0 (已發布)

- ref readonly 參數
- 集合表達式
- 攔截器（實驗性）
- 內聯陣列
- 主要建構函式
- Lambda 可選參數

---

## 🛠️ 最佳實踐

### 記憶體管理

```csharp
// ❌ 避免在熱路徑使用 LINQ
foreach (var item in collection.Where(x => x.IsValid)) { }

// ✅ 使用手動列舉
foreach (var item in collection)
{
    if (item.IsValid) { }
}

// ✅ 使用物件池
using var pooled = ArrayBuilder<T>.GetInstance();
```

### 不可變性

```csharp
// ✅ Compilation 是不可變的
var newCompilation = compilation
    .AddSyntaxTrees(tree)
    .RemoveSyntaxTrees(oldTree)
    .WithOptions(newOptions);
```

### 測試模式

```csharp
public class MyTests : CSharpTestBase
{
    [Fact]
    public void TestMethod()
    {
        var comp = CreateCompilation(@"
            class C { void M() { } }
        ");
        comp.VerifyDiagnostics();
    }
}
```

---

## 🤝 社群與貢獻

### GitHub Repository
- 主專案: https://github.com/dotnet/roslyn
- C# 語言: https://github.com/dotnet/csharplang
- VB 語言: https://github.com/dotnet/vblang

### 社群頻道
- **Discussions**: GitHub Discussions
- **Discord**: CSharp Community Discord - Roslyn 頻道
- **Issues**: 錯誤報告和功能請求

### 貢獻流程

1. **尋找議題**: 標有 "help wanted" 或 "good first issue"
2. **討論設計**: 在議題中討論實作方案
3. **Fork 專案**: Fork repository 並複製
4. **開發**: 實作功能並添加測試
5. **提交 PR**: 提交 Pull Request
6. **審核**: 等待團隊審核和反饋
7. **合併**: PR 被接受後合併到主分支

### 貢獻要求
- ✅ 添加測試覆蓋
- ✅ 遵循程式碼風格
- ✅ 更新文件
- ✅ 所有 CI 檢查通過
- ✅ CLA 簽署

---

## 📚 重要資源

### 文件
- [Roslyn Overview](https://docs.microsoft.com/en-us/dotnet/csharp/roslyn-sdk/)
- [API Documentation](https://docs.microsoft.com/en-us/dotnet/api/)
- [Contributing Guide](https://github.com/dotnet/roslyn/blob/main/CONTRIBUTING.md)

### 學習資源
- [Roslyn Wiki](https://github.com/dotnet/roslyn/wiki)
- [C# Language Spec](https://github.com/dotnet/csharplang/tree/main/spec)
- [Source Generators Cookbook](https://github.com/dotnet/roslyn/blob/main/docs/features/source-generators.cookbook.md)

### 工具
- [Roslyn Quoter](http://roslynquoter.azurewebsites.net/) - 語法樹視覺化
- [SharpLab](https://sharplab.io/) - 線上 C# 編譯器
- [Source Browser](http://sourceroslyn.io/) - 增強的原始碼檢視

---

## 📊 CI/CD 管線

### Azure Pipelines

Roslyn 使用多個 Azure Pipeline 進行持續整合：

- **PR Validation**: Pull Request 驗證
- **Official Build**: 正式建置
- **Integration Tests**: 整合測試
- **Compliance**: 合規性檢查
- **LSP Tests**: 語言伺服器測試

### 測試覆蓋

- Desktop Unit Tests (x86, x64)
- CoreCLR Unit Tests
- Linux Tests
- macOS Tests
- Integration Tests
- Determinism Tests
- Build Correctness
- Source Build

---

## 🏆 專案成就

- ✅ **100% 開源** - 完整的編譯器實現
- ✅ **生產就緒** - 用於 Visual Studio 和 .NET SDK
- ✅ **活躍開發** - 持續新增語言功能
- ✅ **社群驅動** - 接受社群貢獻
- ✅ **跨平台** - 支援 Windows、Linux、macOS
- ✅ **高效能** - 針對大型程式碼庫優化
- ✅ **可擴展** - 豐富的 API 生態系統

---

## 📅 版本歷程

- **2011**: 專案啟動（代號 Roslyn）
- **2014**: 首次公開預覽
- **2015**: C# 6 / VB 14（.NET Framework 4.6）
- **2017**: C# 7.x（.NET Core 2.0+）
- **2019**: C# 8（.NET Core 3.0）
- **2020**: C# 9（.NET 5）
- **2021**: C# 10（.NET 6）
- **2022**: C# 11（.NET 7）
- **2023**: C# 12（.NET 8）
- **2024**: C# 13（.NET 9）
- **2025**: C# 14（.NET 10）進行中

---

## 🔮 未來方向

- 更多語言功能（字典表達式、集合表達式參數）
- 改進的效能和記憶體使用
- 增強的 Source Generator 體驗
- 更好的工具支援
- 跨平台改進

---

## 📞 聯絡與支援

- **Issues**: https://github.com/dotnet/roslyn/issues
- **Discussions**: https://github.com/dotnet/roslyn/discussions
- **Security**: secure@microsoft.com
- **Twitter**: @dotnet

---

**最後更新**: 2025 年 10 月
**文件版本**: 1.0
**基於**: Roslyn main 分支

© Microsoft Corporation | MIT License | .NET Foundation Project

