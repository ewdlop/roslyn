<p align="center">
<img width="450" src="https://user-images.githubusercontent.com/46729679/109719841-17b7dd00-7b5e-11eb-8f5e-87eb2d4d1be9.png" alt="Roslyn logo">
</p>

<h1 align="center">The .NET Compiler Platform</h1>

<p align="center"><a href="http://aka.ms/discord-csharp-roslyn" rel="nofollow"><img title="Chat on Discord" src="docs/img/discord-mark-white.png" /></a></p>

Roslyn is the open-source implementation of both the C# and Visual Basic compilers with an API surface for building code analysis tools.

### C# and Visual Basic Language Feature Suggestions

If you want to suggest a new feature for the C# or Visual Basic languages go here:
- [dotnet/csharplang](https://github.com/dotnet/csharplang) for C# specific issues
- [dotnet/vblang](https://github.com/dotnet/vblang) for VB-specific features
- [dotnet/csharplang](https://github.com/dotnet/csharplang) for features that affect both languages

### Contributing

All work on the C# and Visual Basic compiler happens directly on [GitHub](https://github.com/dotnet/roslyn). Both core team members and external contributors send pull requests which go through the same review process.

If you are interested in fixing issues and contributing directly to the code base, a great way to get started is to ask some questions on [GitHub Discussions](https://github.com/dotnet/roslyn/discussions)! Then check out our [contributing guide](https://github.com/dotnet/roslyn/blob/main/CONTRIBUTING.md) which covers the following:

- [Coding guidelines](https://github.com/dotnet/roslyn/blob/main/docs/wiki/Contributing-Code.md)
- [The development workflow, including debugging and running tests](https://github.com/dotnet/roslyn/blob/main/docs/contributing/Building%2C%20Debugging%2C%20and%20Testing%20on%20Windows.md)
- [Submitting pull requests](<https://github.com/dotnet/roslyn/blob/main/CONTRIBUTING.md#How-to-submit-a-PR>)
- Finding a bug to fix in the [IDE](https://aka.ms/roslyn-ide-bugs-help-wanted) or [Compiler](https://aka.ms/roslyn-compiler-bugs-help-wanted)
- Finding a feature to implement in the [IDE](https://aka.ms/roslyn-ide-feature-help-wanted) or [Compiler](https://aka.ms/roslyn-compiler-feature-help-wanted)
- Roslyn API suggestions should go through the [API review process](<docs/contributing/API Review Process.md>)

### Community

The Roslyn community can be found on [GitHub Discussions](https://github.com/dotnet/roslyn/discussions), where you can ask questions, voice ideas, and share your projects.

To chat with other community members, you can join the Roslyn channel on the [CSharp Community Discord](https://discord.com/invite/tGJvv88).

Our [Code of Conduct](CODE-OF-CONDUCT.md) applies to all Roslyn community channels and has adopted the [.NET Foundation Code of Conduct](https://dotnetfoundation.org/code-of-conduct).

### Documentation

Visit [Roslyn Architecture Overview](https://docs.microsoft.com/en-us/dotnet/csharp/roslyn-sdk/compiler-api-model) to get started with Roslyn's API's.

### NuGet Feeds

**The latest pre-release builds** are available from the following public NuGet feeds: 
- [Compiler](https://dev.azure.com/dnceng/public/_packaging?_a=feed&feed=dotnet-tools): `https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet-tools/nuget/v3/index.json`
- [IDE Services](https://dev.azure.com/azure-public/vside/_packaging?_a=feed&feed=vssdk): `https://pkgs.dev.azure.com/azure-public/vside/_packaging/vssdk/nuget/v3/index.json`
- [.NET SDK](https://dev.azure.com/dnceng/public/_packaging?_a=feed&feed=dotnet5): `https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet5/nuget/v3/index.json`

[//]: # (Begin current test results)

### Continuous Integration status
#### Builds

|Branch|Windows Debug|Windows Release|Unix Debug|
|:--:|:--:|:--:|:--:|
**main**|[![Build Status](https://dev.azure.com/dnceng-public/public/_apis/build/status/dotnet/roslyn/roslyn-CI?branchname=main&jobname=Build_Windows_Debug&configuration=Build_Windows_Debug&label=build)](https://dev.azure.com/dnceng-public/public/_build/latest?definitionId=95&branchname=main&view=logs)|[![Build Status](https://dev.azure.com/dnceng-public/public/_apis/build/status/dotnet/roslyn/roslyn-CI?branchname=main&jobname=Build_Windows_Release&configuration=Build_Windows_Release&label=build)](https://dev.azure.com/dnceng-public/public/_build/latest?definitionId=95&branchname=main&view=logs)|[![Build Status](https://dev.azure.com/dnceng-public/public/_apis/build/status/dotnet/roslyn/roslyn-CI?branchname=main&jobname=Build_Unix_Debug&configuration=Build_Unix_Debug&label=build)](https://dev.azure.com/dnceng-public/public/_build/latest?definitionId=95&branchname=main&view=logs)|

#### Desktop Unit Tests

|Branch|Debug x86|Debug x64|Release x86|Release x64|
|:--:|:--:|:--:|:--:|:--:|
**main**|[![Build Status](https://dev.azure.com/dnceng-public/public/_apis/build/status/dotnet/roslyn/roslyn-CI?branchname=main&jobname=Test_Windows_Desktop_Debug_32&configuration=Test_Windows_Desktop_Debug_32&label=build)](https://dev.azure.com/dnceng-public/public/_build/latest?definitionId=95&branchname=main&view=logs)|[![Build Status](https://dev.azure.com/dnceng-public/public/_apis/build/status/dotnet/roslyn/roslyn-CI?branchname=main&jobname=Test_Windows_Desktop_Debug_64&configuration=Test_Windows_Desktop_Debug_64&label=build)](https://dev.azure.com/dnceng-public/public/_build/latest?definitionId=95&branchname=main&view=logs)|[![Build Status](https://dev.azure.com/dnceng-public/public/_apis/build/status/dotnet/roslyn/roslyn-CI?branchname=main&jobname=Test_Windows_Desktop_Release_32&configuration=Test_Windows_Desktop_Release_32&label=build)](https://dev.azure.com/dnceng-public/public/_build/latest?definitionId=95&branchname=main&view=logs)|[![Build Status](https://dev.azure.com/dnceng-public/public/_apis/build/status/dotnet/roslyn/roslyn-CI?branchname=main&jobname=Test_Windows_Desktop_Release_64&configuration=Test_Windows_Desktop_Release_64&label=build)](https://dev.azure.com/dnceng-public/public/_build/latest?definitionId=95&branchname=main&view=logs)|

#### CoreClr Unit Tests

|Branch|Windows Debug|Windows Release|Linux|
|:--:|:--:|:--:|:--:|
**main**|[![Build Status](https://dev.azure.com/dnceng-public/public/_apis/build/status/dotnet/roslyn/roslyn-CI?branchname=main&jobname=Test_Windows_CoreClr_Debug&configuration=Test_Windows_CoreClr_Debug&label=build)](https://dev.azure.com/dnceng-public/public/_build/latest?definitionId=95&branchname=main&view=logs)|[![Build Status](https://dev.azure.com/dnceng-public/public/_apis/build/status/dotnet/roslyn/roslyn-CI?branchname=main&jobname=Test_Windows_CoreClr_Release&configuration=Test_Windows_CoreClr_Release&label=build)](https://dev.azure.com/dnceng-public/public/_build/latest?definitionId=95&branchname=main&view=logs)|[![Build Status](https://dev.azure.com/dnceng-public/public/_apis/build/status/dotnet/roslyn/roslyn-CI?branchname=main&jobname=Test_Linux_Debug&configuration=Test_Linux_Debug&label=build)](https://dev.azure.com/dnceng-public/public/_build/latest?definitionId=95&branchname=main&view=logs)|

#### Integration Tests

|Branch|Debug x86|Debug x64|Release x86|Release x64
|:--:|:--:|:--:|:--:|:--:|
**main**|[![Build Status](https://dev.azure.com/dnceng-public/public/_apis/build/status/dotnet/roslyn/roslyn-integration-CI?branchname=main&jobname=VS_Integration_Debug_32&configuration=VS_Integration_Debug_32&label=build)](https://dev.azure.com/dnceng-public/public/_build/latest?definitionId=96&branchname=main&view=logs)|[![Build Status](https://dev.azure.com/dnceng-public/public/_apis/build/status/dotnet/roslyn/roslyn-integration-CI?branchname=main&jobname=VS_Integration_Debug_64&configuration=VS_Integration_Debug_64&label=build)](https://dev.azure.com/dnceng-public/public/_build/latest?definitionId=96&branchname=main&view=logs)|[![Build Status](https://dev.azure.com/dnceng-public/public/_apis/build/status/dotnet/roslyn/roslyn-integration-CI?branchname=main&jobname=VS_Integration_Release_32&configuration=VS_Integration_Release_32&label=build)](https://dev.azure.com/dnceng-public/public/_build/latest?definitionId=96&branchname=main&view=logs)|[![Build Status](https://dev.azure.com/dnceng-public/public/_apis/build/status/dotnet/roslyn/roslyn-integration-CI?branchname=main&jobname=VS_Integration_Release_64&configuration=VS_Integration_Release_64&label=build)](https://dev.azure.com/dnceng-public/public/_build/latest?definitionId=96&branchname=main&view=logs)|

#### Misc Tests

|Branch|Determinism|Analyzers|Build Correctness|Source build|TODO/Prototype|Spanish|MacOS|
|:--:|:--:|:--:|:--:|:--:|:--:|:--:|:--:|
**main**|[![Build Status](https://dev.azure.com/dnceng-public/public/_apis/build/status/dotnet/roslyn/roslyn-CI?branchname=main&jobname=Correctness_Determinism&configuration=Correctness_Determinism&label=build)](https://dev.azure.com/dnceng-public/public/_build/latest?definitionId=95&branchname=main&view=logs)|[![Build Status](https://dev.azure.com/dnceng-public/public/_apis/build/status/dotnet/roslyn/roslyn-CI?branchname=main&jobname=Correctness_Analyzers&configuration=Correctness_Analyzers&label=build)](https://dev.azure.com/dnceng-public/public/_build/latest?definitionId=95&branchname=main&view=logs)|[![Build Status](https://dev.azure.com/dnceng-public/public/_apis/build/status/dotnet/roslyn/roslyn-CI?branchname=main&jobname=Correctness_Build_Artifacts&configuration=Correctness_Build_Artifacts&label=build)](https://dev.azure.com/dnceng-public/public/_build/latest?definitionId=95&branchname=main&view=logs)|[![Build Status](https://dev.azure.com/dnceng-public/public/_apis/build/status/dotnet/roslyn/roslyn-CI?branchname=main&jobname=Source-Build+(Managed)&configuration=Source-Build+(Managed)&label=build)](https://dev.azure.com/dnceng-public/public/_build/latest?definitionId=95&branchname=main&view=logs)|[![Build Status](https://dev.azure.com/dnceng-public/public/_apis/build/status/dotnet/roslyn/roslyn-CI?branchname=main&jobname=Correctness_TodoCheck&configuration=Correctness_TodoCheck&label=build)](https://dev.azure.com/dnceng-public/public/_build/latest?definitionId=95&branchname=main&view=logs)|[![Build Status](https://dev.azure.com/dnceng-public/public/_apis/build/status/dotnet/roslyn/roslyn-CI?branchname=main&jobname=Test_Windows_Desktop_Spanish_Release_64&configuration=Test_Windows_Desktop_Spanish_Release_64&label=build)](https://dev.azure.com/dnceng-public/public/_build/latest?definitionId=95&branchname=main&view=logs)|[![Build Status](https://dev.azure.com/dnceng-public/public/_apis/build/status/dotnet/roslyn/roslyn-CI?branchname=main&jobname=Test_macOS_Debug&configuration=Test_macOS_Debug&label=build)](https://dev.azure.com/dnceng-public/public/_build/latest?definitionId=95&branchname=main&view=logs)|


[//]: # (End current test results)

### .NET Foundation

This project is part of the [.NET Foundation](http://www.dotnetfoundation.org/projects) along with other projects like [the .NET Runtime](https://github.com/dotnet/runtime/).

# C# Discriminated Unions Implementation Progress

This repository tracks the implementation of discriminated unions as a C# language feature, based on the [official Type Unions proposal](https://github.com/dotnet/csharplang/blob/main/proposals/TypeUnions.md).

## 🎯 Project Goal

Add support for discriminated unions to the C# language, enabling:
- **Type Unions**: `(string | int | bool)` syntax for union types
- **Tagged Unions**: `union` keyword for declaring discriminated union types with cases
- **Pattern Matching**: Enhanced pattern matching for union types

## 📋 Implementation Progress

### Phase 1: Syntax Infrastructure ✅ COMPLETED

#### Step 1.1: Add New SyntaxKind Enums ✅
- [x] **UnionKeyword** (8452) - Added contextual keyword for `union`
- [x] **UnionType** (8926) - Added syntax kind for type unions `(A | B | C)`
- [x] **TaggedUnionDeclaration** (8859) - Added syntax kind for tagged union declarations
- [x] **UnionCaseDeclaration** (8860) - Added syntax kind for union cases
- **Files Modified**: `src/Compilers/CSharp/Portable/Syntax/SyntaxKind.cs`

#### Step 1.2: Update SyntaxKindFacts ✅
- [x] Added `union` to contextual keyword recognition in `GetContextualKeywordKind()`
- [x] Added `UnionKeyword` to `IsContextualKeyword()` method
- [x] Added `UnionType` to `IsTypeSyntax()` method
- [x] Added `TaggedUnionDeclaration` to `IsTypeDeclaration()` method
- [x] Updated `GetContextualKeywordKinds()` to include `UnionKeyword`
- **Files Modified**: `src/Compilers/CSharp/Portable/Syntax/SyntaxKindFacts.cs`

#### Step 1.3: Define Syntax Node Structure ✅
- [x] **UnionTypeSyntax** - Represents `(A | B | C)` syntax
  - OpenParenToken, Types (SeparatedSyntaxList), CloseParenToken
- [x] **TaggedUnionDeclarationSyntax** - Represents `union` declarations
  - AttributeLists, Modifiers, UnionKeyword, Identifier, TypeParameterList, BaseList, OpenBraceToken, Members, CloseBraceToken, SemicolonToken
- [x] **UnionCaseDeclarationSyntax** - Represents individual union cases
  - AttributeLists, Modifiers, Identifier, ParameterList
- **Files Modified**: `src/Compilers/CSharp/Portable/Syntax/Syntax.xml`

#### Step 1.4: Generate Syntax Classes ✅
- [x] Fixed XML syntax error (extra quote character)
- [x] Generated syntax node classes using `eng\generate-compiler-code.cmd`
- [x] Verified generated classes:
  - `UnionTypeSyntax` class with proper members
  - `TaggedUnionDeclarationSyntax` class with proper inheritance
  - `UnionCaseDeclarationSyntax` class with parameter support
- **Files Generated**: 
  - `Syntax.xml.Syntax.Generated.cs`
  - `Syntax.xml.Main.Generated.cs` 
  - `Syntax.xml.Internal.Generated.cs`

#### Step 1.5: Factory Methods and Visitors ✅
- [x] `SyntaxFactory.UnionType()` - Create union type syntax programmatically
- [x] `SyntaxFactory.TaggedUnionDeclaration()` - Create tagged union declarations
- [x] `SyntaxFactory.UnionCaseDeclaration()` - Create union cases
- [x] Visitor pattern support:
  - `VisitUnionType()` in `CSharpSyntaxVisitor`
  - `VisitTaggedUnionDeclaration()` in `CSharpSyntaxVisitor`
  - `VisitUnionCaseDeclaration()` in `CSharpSyntaxVisitor`

#### Step 1.6: Build and Test ✅
- [x] Successfully built modified compiler with `.\.dotnet\dotnet.exe build`
- [x] Verified syntax nodes are accessible in generated assemblies
- [x] Created demonstration examples showing syntax tree API usage
- [x] Documented implementation status and next steps

### Phase 2: Parser Integration 🚧 TODO

#### Step 2.1: Update Language Parser 📋
- [ ] **Type Union Parsing** - Recognize `(A | B | C)` syntax in type contexts
  - [ ] Update `ParseType()` to handle parenthesized union types
  - [ ] Add precedence handling for `|` operator in type context
  - [ ] Handle nested union types and complex scenarios
- [ ] **Tagged Union Parsing** - Recognize `union` declarations
  - [ ] Update `ParseTypeDeclaration()` to handle `union` keyword
  - [ ] Parse union case declarations with optional parameters
  - [ ] Handle generic union types and constraints
- **Files to Modify**: `src/Compilers/CSharp/Portable/Parser/`

#### Step 2.2: Language Version Support 📋
- [ ] Add feature flag for union types in `LanguageVersion.cs`
- [ ] Update feature availability checks
- [ ] Add appropriate error messages for unsupported language versions

### Phase 3: Semantic Analysis 🚧 TODO

#### Step 3.1: Symbol Creation 📋
- [ ] **Union Type Symbols** - Create symbols for union types
  - [ ] `IUnionTypeSymbol` interface
  - [ ] Union type symbol implementation
  - [ ] Member type resolution
- [ ] **Tagged Union Symbols** - Create symbols for tagged unions
  - [ ] Union declaration symbols
  - [ ] Union case symbols with parameter information
  - [ ] Constructor generation for union cases

#### Step 3.2: Type System Integration 📋
- [ ] **Type Checking** - Validate union type usage
  - [ ] Assignment compatibility checks
  - [ ] Method resolution with union parameters
  - [ ] Generic type inference with unions
- [ ] **Conversion Handling** - Implicit/explicit conversions
  - [ ] Member type to union type conversions
  - [ ] Union type compatibility rules

#### Step 3.3: Binding and Resolution 📋
- [ ] **Symbol Resolution** - Resolve union members and cases
- [ ] **Overload Resolution** - Handle method overloads with union types
- [ ] **Type Inference** - Infer union types from expressions

### Phase 4: Code Generation 🚧 TODO

#### Step 4.1: Runtime Representation 📋
- [ ] **Union Type Layout** - Efficient memory representation
  - [ ] Tagged union runtime structure
  - [ ] Discriminator field management
  - [ ] Value storage optimization
- [ ] **IL Emission** - Generate appropriate IL code
  - [ ] Union construction
  - [ ] Member access
  - [ ] Pattern matching support

#### Step 4.2: Runtime Support 📋
- [ ] **Boxing/Unboxing** - Handle union types with value types
- [ ] **Reflection Support** - Runtime type information
- [ ] **Serialization** - Support for union type serialization

### Phase 5: Pattern Matching Enhancement 🚧 TODO

#### Step 5.1: Pattern Syntax 📋
- [ ] **Union Case Patterns** - `case Success(var value)` syntax
- [ ] **Exhaustiveness Checking** - Ensure all cases are handled
- [ ] **Guard Clauses** - Support for pattern guards

#### Step 5.2: Switch Expressions 📋
- [ ] **Union Switch** - Efficient switch compilation
- [ ] **Deconstruction** - Extract union case values
- [ ] **Performance Optimization** - Jump table generation

### Phase 6: Tooling and IDE Support 🚧 TODO

#### Step 6.1: IntelliSense 📋
- [ ] **Syntax Highlighting** - Color union keywords and syntax
- [ ] **Code Completion** - Suggest union cases and members
- [ ] **Error Squiggles** - Real-time syntax validation

#### Step 6.2: Debugging Support 📋
- [ ] **Union Value Display** - Show union values in debugger
- [ ] **Case Inspection** - Navigate union case information

## 📊 Overall Progress

| Phase | Component | Status | Progress |
|-------|-----------|--------|----------|
| 1 | Syntax Infrastructure | ✅ Complete | 100% |
| 2 | Parser Integration | 🚧 Not Started | 0% |
| 3 | Semantic Analysis | 🚧 Not Started | 0% |
| 4 | Code Generation | 🚧 Not Started | 0% |
| 5 | Pattern Matching | 🚧 Not Started | 0% |
| 6 | Tooling Support | 🚧 Not Started | 0% |

**Overall Project Progress: 16.7% (1/6 phases complete)**

## 🎨 Syntax Examples

### Type Unions (Planned)
```csharp
// Variable declarations with union types
var value: (string | int | bool) = GetValue();
var result: (Success | Error) = ProcessData();

// Method signatures
public (HttpResponse | TimeoutError | NetworkError) MakeRequest(string url);
public void ProcessValue((string | int | bool) value);
```

### Tagged Unions (Planned)
```csharp
// Option type - functional programming pattern
public union Option<T>
{
    Some(T value),
    None
}

// Result type for error handling
public union Result<T, E>
{
    Ok(T value),
    Err(E error)
}

// Complex union with multiple cases
public union JsonValue
{
    String(string value),
    Number(double value),
    Boolean(bool value),
    Array(JsonValue[] items),
    Object(Dictionary<string, JsonValue> properties),
    Null
}
```

### Pattern Matching (Planned)
```csharp
// Switch expressions with union types
var message = result switch
{
    Ok(var value) => $"Success: {value}",
    Err(var error) => $"Error: {error}"
};

// Pattern matching in if statements
if (option is Some(var value))
{
    Console.WriteLine($"Value: {value}");
}

// Exhaustive checking ensures all cases handled
var status = httpResult switch
{
    Success(var code, var body) => $"HTTP {code}: {body}",
    Redirect(var location) => $"Redirected to {location}",
    ClientError(var code, var msg) => $"Client Error {code}: {msg}",
    ServerError(var code, var ex) => $"Server Error {code}: {ex.Message}"
    // Compiler ensures all cases are covered
};
```

## 🏗️ Current Implementation

### ✅ Working Features
- Full syntax tree support for union types and tagged unions
- Contextual keyword recognition for `union`
- Factory methods for creating union syntax nodes programmatically
- Visitor pattern integration for syntax tree traversal
- Generated test infrastructure

### 🔧 Current Limitations
- Parser does not yet recognize union syntax in source code
- No semantic analysis or type checking
- No IL code generation
- No runtime support
- Pattern matching enhancements not yet implemented

## 🚀 Running the Demo

```bash
# Build the modified compiler
.\.dotnet\dotnet.exe build src/Compilers/CSharp/Portable/Microsoft.CodeAnalysis.CSharp.csproj

# View the implementation demo
type SimpleUnionDemo.cs
```

## 📁 Repository Structure

```
roslyn/
├── src/Compilers/CSharp/Portable/
│   ├── Syntax/
│   │   ├── SyntaxKind.cs           # ✅ Added union syntax kinds
│   │   ├── SyntaxKindFacts.cs      # ✅ Added union keyword handling
│   │   └── Syntax.xml              # ✅ Added union syntax definitions
│   ├── Generated/                  # ✅ Generated syntax classes
│   ├── Parser/                     # 🚧 TODO: Add union parsing
│   ├── Binder/                     # 🚧 TODO: Add semantic analysis
│   └── CodeGen/                    # 🚧 TODO: Add IL generation
├── SimpleUnionDemo.cs              # ✅ Implementation demonstration
├── DiscriminatedUnionExample.cs    # ✅ Roslyn API examples
└── DISCRIMINATED_UNIONS_IMPLEMENTATION.md  # ✅ Technical documentation
```

## 📚 References

- [Official C# Type Unions Proposal](https://github.com/dotnet/csharplang/blob/main/proposals/TypeUnions.md)
- [Roslyn Compiler Architecture](https://github.com/dotnet/roslyn/blob/main/docs/wiki/Roslyn-Overview.md)
- [Contributing to Roslyn](https://github.com/dotnet/roslyn/blob/main/CONTRIBUTING.md)

## 🤝 Contributing

This implementation follows the official Type Unions proposal and Roslyn contribution guidelines. The foundational syntax infrastructure is complete and ready for the next phases of development.

---

**Status**: Phase 1 Complete ✅ | Next: Parser Integration 🚧
