# Discriminated Unions Implementation for C#

This document describes the implementation of discriminated unions as a C# language feature, based on the [Type Unions proposal](https://github.com/dotnet/csharplang/blob/main/proposals/TypeUnions.md).

## Overview

We have successfully added the foundational syntax support for discriminated unions to the Roslyn C# compiler. This implementation includes:

1. **Type Unions**: `(A | B | C)` syntax for union types
2. **Tagged Unions**: `union` keyword for declaring discriminated union types
3. **Syntax Tree Support**: Full syntax node generation and parsing infrastructure

## Implementation Details

### 1. Syntax Kinds Added

We added the following new syntax kinds to `SyntaxKind.cs`:

```csharp
// Keywords
UnionKeyword = 8452,

// Type syntax
UnionType = 8926,

// Declaration syntax  
TaggedUnionDeclaration = 8859,
UnionCaseDeclaration = 8860,
```

### 2. Syntax Nodes Added

#### UnionTypeSyntax
Represents type union syntax like `(string | int | bool)`:

```csharp
public sealed partial class UnionTypeSyntax : TypeSyntax
{
    public SyntaxToken OpenParenToken { get; }
    public SeparatedSyntaxList<TypeSyntax> Types { get; }
    public SyntaxToken CloseParenToken { get; }
}
```

#### TaggedUnionDeclarationSyntax
Represents tagged union declarations like:

```csharp
public union Result<T, E>
{
    Success(T value),
    Error(E error)
}
```

```csharp
public sealed partial class TaggedUnionDeclarationSyntax : BaseTypeDeclarationSyntax
{
    public SyntaxToken UnionKeyword { get; }
    public SyntaxToken Identifier { get; }
    public TypeParameterListSyntax? TypeParameterList { get; }
    public BaseListSyntax? BaseList { get; }
    public SeparatedSyntaxList<UnionCaseDeclarationSyntax> Members { get; }
}
```

#### UnionCaseDeclarationSyntax
Represents individual cases within a tagged union:

```csharp
public sealed partial class UnionCaseDeclarationSyntax : MemberDeclarationSyntax
{
    public SyntaxToken Identifier { get; }
    public ParameterListSyntax? ParameterList { get; }
}
```

### 3. Files Modified

#### Core Syntax Files
- `src/Compilers/CSharp/Portable/Syntax/SyntaxKind.cs` - Added new syntax kinds
- `src/Compilers/CSharp/Portable/Syntax/SyntaxKindFacts.cs` - Added keyword recognition and type checking
- `src/Compilers/CSharp/Portable/Syntax/Syntax.xml` - Added syntax node definitions

#### Generated Files
The following files were automatically generated from our XML definitions:
- `Syntax.xml.Syntax.Generated.cs` - Contains the syntax node classes
- `Syntax.xml.Main.Generated.cs` - Contains factory methods and visitors
- `Syntax.xml.Internal.Generated.cs` - Contains internal syntax implementations

### 4. Language Features Supported

#### Type Unions
```csharp
// Variable declarations with union types
var value: (string | int | null) = GetValue();
var result: (Success | Error) = ProcessData();

// Method parameters and return types
public (HttpResponse | TimeoutError | NetworkError) MakeRequest(string url);
public void ProcessValue((string | int | bool) value);
```

#### Tagged Unions
```csharp
// Option type
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

// JSON value representation
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

#### Pattern Matching (Future)
```csharp
// Switch expressions
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
```

## What's Implemented vs. What's Missing

### ✅ Implemented
- [x] Syntax tree definitions for union types and tagged unions
- [x] Keyword recognition for `union`
- [x] Syntax node generation and factory methods
- [x] Visitor pattern support
- [x] Basic syntax validation
- [x] Code generation infrastructure

### ❌ Still Needed for Full Implementation
- [ ] **Parser Integration**: Update the C# parser to recognize and parse union syntax
- [ ] **Semantic Analysis**: Type checking, symbol resolution, and semantic validation
- [ ] **Code Generation**: IL emission for union types and operations
- [ ] **Pattern Matching**: Enhanced pattern matching for union types
- [ ] **Type System Integration**: Integration with the existing type system
- [ ] **Runtime Support**: Runtime representation and boxing/unboxing
- [ ] **Interop**: Integration with existing .NET types and generics
- [ ] **Tooling**: IntelliSense, debugging, and IDE support

## Next Steps

To complete the discriminated union implementation, the following major components need to be implemented:

### 1. Parser Updates
Update the C# parser in `src/Compilers/CSharp/Portable/Parser/` to:
- Recognize union type syntax in type contexts
- Parse tagged union declarations
- Handle union case declarations with parameters

### 2. Semantic Analysis
Implement semantic analysis in `src/Compilers/CSharp/Portable/Binder/` to:
- Create symbols for union types and cases
- Validate union type compatibility
- Handle type inference with unions
- Implement exhaustiveness checking

### 3. Code Generation
Update the code generator in `src/Compilers/CSharp/Portable/CodeGen/` to:
- Emit IL for union types
- Generate efficient runtime representations
- Handle union construction and deconstruction

### 4. Pattern Matching
Enhance pattern matching in `src/Compilers/CSharp/Portable/FlowAnalysis/` to:
- Support union case patterns
- Implement exhaustiveness analysis
- Generate efficient switch code

## Example Usage

The `DiscriminatedUnionExample.cs` file demonstrates how to work with the new syntax nodes programmatically:

```csharp
// Create union type syntax
var unionType = UnionSyntaxExtensions.CreateUnionType(
    SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.StringKeyword)),
    SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.IntKeyword)),
    SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.BoolKeyword))
);

// Create tagged union declaration
var resultUnion = UnionSyntaxExtensions.CreateTaggedUnion(
    "Result",
    ("Success", SyntaxFactory.ParameterList(/* parameters */)),
    ("Error", SyntaxFactory.ParameterList(/* parameters */))
);
```

## Building and Testing

To build the modified compiler:

```bash
# Build the C# compiler with union support
.\.dotnet\dotnet.exe build src/Compilers/CSharp/Portable/Microsoft.CodeAnalysis.CSharp.csproj

# Run the example
.\.dotnet\dotnet.exe run --project DiscriminatedUnionExample.csproj
```

## Conclusion

This implementation provides the foundational syntax infrastructure for discriminated unions in C#. While the syntax tree support is complete, significant work remains to integrate this feature into the parser, semantic analyzer, and code generator to create a fully functional language feature.

The implementation follows the official [Type Unions proposal](https://github.com/dotnet/csharplang/blob/main/proposals/TypeUnions.md) and provides a solid foundation for the complete feature implementation. 