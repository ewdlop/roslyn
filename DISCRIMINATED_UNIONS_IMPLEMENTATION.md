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

## Phase 2 Completion - Parser Integration ✅

Phase 2 of the discriminated unions implementation has been successfully completed! The C# parser now fully supports both union syntax forms:

### Union Type Syntax `(A | B | C)`
```csharp
// Variable declarations with union types
public (string | int | null) value;
public (HttpResponse | TimeoutError | NetworkError) result;

// Method parameters and return types  
public (Success | Error) ProcessData();
public void HandleValue((string | int | bool) input);
```

### Tagged Union Declaration Syntax
```csharp
// Simple tagged union
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

// Complex union with multiple parameters
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

### Parser Implementation Details

#### Union Type Detection (`IsUnionType`)
- Lookahead parsing to distinguish `(A | B | C)` from tuple types `(A, B, C)`
- Checks for `|` (bar token) vs `,` (comma token) after the first type

#### Union Type Parsing (`ParseUnionType`)
- Parses parenthesized list of types separated by `|` tokens
- Creates `UnionTypeSyntax` nodes with proper separator handling
- Integrated into main type parsing logic

#### Tagged Union Parsing (`ParseTaggedUnionDeclaration`)
- Recognizes `union` contextual keyword in type declaration contexts
- Parses union name, optional type parameters, and base list
- Handles both brace-delimited and semicolon-terminated declarations

#### Union Case Parsing (`ParseUnionMemberDeclaration`)
- Parses individual union cases with optional parameter lists
- Creates `UnionCaseDeclarationSyntax` nodes
- Supports attributes on union cases

## Phase 3 Progress - Semantic Analysis (In Progress) 🚧

Phase 3 of the discriminated unions implementation is now underway! We're implementing the semantic layer that makes discriminated unions functionally usable.

### Phase 3.1: Symbol Creation ✅

We have successfully implemented the core symbol infrastructure:

#### UnionTypeSymbol
- Created `UnionTypeSymbol` class for union types like `(A | B | C)`
- Inherits from `NamedTypeSymbol` with proper TypeKind support
- Implements equality, hashing, and visitor pattern
- Provides type checking methods (`IsReferenceType`, `IsValueType`)
- Supports display formatting: `(string | int | bool)`

#### TaggedUnionTypeSymbol  
- Created `TaggedUnionTypeSymbol` class for declared unions like `union Result { Success, Error }`
- Inherits from `SourceMemberContainerTypeSymbol` for full source integration
- Supports union cases with parameters
- Proper base type handling (inherits from Object)

#### UnionCaseSymbol
- Created `UnionCaseSymbol` class for individual union cases
- Supports parameterized cases: `Success(T value)`
- Proper symbol hierarchy integration

#### TypeKind Extensions
- Added `TypeKind.Union` and `TypeKind.TaggedUnion` to core type system
- Added extension methods: `IsUnion()`, `IsTaggedUnion()`, `IsUnionType()`

### Phase 3.2: Binder Integration ✅

Successfully integrated union type binding into the semantic analysis pipeline:

#### Union Type Binding
- Added `SyntaxKind.UnionType` case to `BindNamespaceOrTypeOrAliasSymbol`
- Implemented `BindUnionType` method following the tuple type pattern
- Properly handles type validation and error reporting
- Creates normalized `UnionTypeSymbol` instances

#### Type System Integration
- Union types now properly bind from syntax to symbols
- Supports nested union types and complex type combinations
- Integrates with existing nullability and annotation systems

### What's Implemented vs. What's Missing

### ✅ Implemented (Phases 1, 2 & 3.1-3.2 Complete)
- [x] **Phase 1: Syntax Infrastructure**
  - [x] Syntax tree definitions for union types and tagged unions
  - [x] Keyword recognition for `union`
  - [x] Syntax node generation and factory methods
  - [x] Visitor pattern support
  - [x] Basic syntax validation
  - [x] Code generation infrastructure
- [x] **Phase 2: Parser Integration**
  - [x] Union type parsing: `(A | B | C)` syntax in type contexts
  - [x] Tagged union declaration parsing: `union Name { Case1, Case2 }` syntax
  - [x] Union case parsing with parameters: `Case(int value, string name)`
  - [x] Proper disambiguation between union types `(A | B)` and tuple types `(A, B)`
- [x] **Phase 3.1-3.2: Symbol Creation & Binder Integration**
  - [x] `UnionTypeSymbol` for union types `(A | B | C)`
  - [x] `TaggedUnionTypeSymbol` for declared unions `union T { A, B }`
  - [x] `UnionCaseSymbol` for individual union cases
  - [x] Type system integration with `TypeKind.Union` and `TypeKind.TaggedUnion`
  - [x] Binder integration for union type binding
  - [x] Extension methods for type checking

### ❌ Still Needed for Full Implementation (Phase 3.3+)
- [ ] **Phase 3.3: Type Checking & Validation**
  - [ ] Union type compatibility rules
  - [ ] Conversion and assignment semantics
  - [ ] Tagged union declaration validation
  - [ ] Generic union support and constraints
- [ ] **Phase 3.4: Type Inference**
  - [ ] Best common type determination for unions
  - [ ] Method overload resolution with unions
  - [ ] Generic type inference with union constraints
- [ ] **Phase 3.5: Pattern Matching Foundation**
  - [ ] Union case pattern recognition
  - [ ] Exhaustiveness checking for union types
  - [ ] Switch expression analysis
- [ ] **Phase 4: Code Generation**
  - [ ] IL emission for union types and operations
  - [ ] Runtime representation and layout
  - [ ] Constructor and accessor generation
- [ ] **Phase 5: Pattern Matching Enhancement**
  - [ ] Enhanced pattern matching for union types
  - [ ] Switch expression improvements
  - [ ] Exhaustiveness analysis
- [ ] **Phase 6: Type System Integration**
  - [ ] Integration with existing .NET type system
  - [ ] Generic constraint support
  - [ ] Variance and covariance rules
- [ ] **Phase 7: Runtime & Interop**
  - [ ] Runtime support and boxing/unboxing
  - [ ] Interop with existing .NET types
  - [ ] Serialization support
- [ ] **Phase 8: Tooling**
  - [ ] IntelliSense and completion
  - [ ] Debugging support
  - [ ] IDE integration

## Next Steps

With the core symbol infrastructure and binder integration complete, we're ready to continue Phase 3 with type checking and validation:

### Phase 3.3: Type Checking & Validation

The next step requires implementing proper type checking for union types:

#### Union Type Compatibility Rules
- Define when two union types are compatible
- Handle subtype relationships between union and non-union types
- Implement conversion rules (implicit/explicit)

#### Tagged Union Declaration Validation
- Validate union case names and parameters
- Check for duplicate cases and naming conflicts
- Ensure proper type constraints on generic unions

### Testing Phase 3 Progress

You can test the current implementation:

```bash
# Test the union type symbol creation and binding
dotnet run --project TestUnionParsing.csproj
```

This will verify that union types now properly create symbols and bind through the semantic analysis pipeline.

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