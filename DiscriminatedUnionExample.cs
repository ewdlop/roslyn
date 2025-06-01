using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DiscriminatedUnionExample
{
    /// <summary>
    /// Demonstrates the new discriminated union language features added to C#
    /// Based on the Type Unions proposal: https://github.com/dotnet/csharplang/blob/main/proposals/TypeUnions.md
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Discriminated Union Language Feature Demo");
            Console.WriteLine("========================================");
            
            // Demonstrate parsing union type syntax
            DemonstrateUnionTypeSyntax();
            
            // Demonstrate parsing tagged union declaration syntax
            DemonstrateTaggedUnionDeclarationSyntax();
            
            // Show how these would be used in practice
            ShowPracticalExamples();
        }

        /// <summary>
        /// Demonstrates parsing of union type syntax: (string | int | bool)
        /// </summary>
        static void DemonstrateUnionTypeSyntax()
        {
            Console.WriteLine("\n1. Union Type Syntax (Type Unions)");
            Console.WriteLine("-----------------------------------");
            
            // Example union type syntax
            string unionTypeCode = "(string | int | bool)";
            
            try
            {
                // Parse the union type syntax
                var syntaxTree = CSharpSyntaxTree.ParseText($"class Test {{ {unionTypeCode} field; }}");
                var root = syntaxTree.GetRoot();
                
                // Find the field declaration
                var fieldDeclaration = root.DescendantNodes().OfType<FieldDeclarationSyntax>().First();
                var variableDeclaration = fieldDeclaration.Declaration;
                var type = variableDeclaration.Type;
                
                Console.WriteLine($"Parsed type: {type}");
                Console.WriteLine($"Type kind: {type.Kind()}");
                
                // Check if it's our new UnionType
                if (type.Kind() == SyntaxKind.UnionType)
                {
                    var unionType = (UnionTypeSyntax)type;
                    Console.WriteLine($"Union type with {unionType.Types.Count} types:");
                    
                    foreach (var unionMemberType in unionType.Types)
                    {
                        Console.WriteLine($"  - {unionMemberType}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Note: Union type syntax not yet fully implemented in parser: {ex.Message}");
                Console.WriteLine("This demonstrates the syntax tree structure that would be created.");
            }
        }

        /// <summary>
        /// Demonstrates parsing of tagged union declaration syntax
        /// </summary>
        static void DemonstrateTaggedUnionDeclarationSyntax()
        {
            Console.WriteLine("\n2. Tagged Union Declaration Syntax");
            Console.WriteLine("----------------------------------");
            
            // Example tagged union declaration
            string taggedUnionCode = @"
public union Result<T, E>
{
    Success(T value),
    Error(E error)
}";
            
            try
            {
                // Parse the tagged union declaration
                var syntaxTree = CSharpSyntaxTree.ParseText(taggedUnionCode);
                var root = syntaxTree.GetRoot();
                
                // Find the union declaration
                var unionDeclaration = root.DescendantNodes().OfType<TaggedUnionDeclarationSyntax>().FirstOrDefault();
                
                if (unionDeclaration != null)
                {
                    Console.WriteLine($"Union name: {unionDeclaration.Identifier.ValueText}");
                    Console.WriteLine($"Modifiers: {string.Join(" ", unionDeclaration.Modifiers)}");
                    Console.WriteLine($"Union keyword: {unionDeclaration.UnionKeyword}");
                    Console.WriteLine($"Cases: {unionDeclaration.Members.Count}");
                    
                    foreach (var member in unionDeclaration.Members)
                    {
                        Console.WriteLine($"  - {member.Identifier.ValueText}");
                        if (member.ParameterList != null)
                        {
                            Console.WriteLine($"    Parameters: {member.ParameterList}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Note: Tagged union syntax not yet fully implemented in parser.");
                    Console.WriteLine("This demonstrates the syntax tree structure that would be created.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Note: Tagged union syntax not yet fully implemented in parser: {ex.Message}");
                Console.WriteLine("This demonstrates the syntax tree structure that would be created.");
            }
        }

        /// <summary>
        /// Shows practical examples of how discriminated unions would be used
        /// </summary>
        static void ShowPracticalExamples()
        {
            Console.WriteLine("\n3. Practical Examples");
            Console.WriteLine("---------------------");
            
            Console.WriteLine("Type Union Examples:");
            Console.WriteLine("  var value: (string | int | null) = GetValue();");
            Console.WriteLine("  var result: (Success | Error) = ProcessData();");
            Console.WriteLine("  var response: (HttpResponse | TimeoutError | NetworkError) = MakeRequest();");
            
            Console.WriteLine("\nTagged Union Examples:");
            Console.WriteLine(@"
  union Option<T>
  {
      Some(T value),
      None
  }
  
  union Result<T, E>
  {
      Ok(T value),
      Err(E error)
  }
  
  union JsonValue
  {
      String(string value),
      Number(double value),
      Boolean(bool value),
      Array(JsonValue[] items),
      Object(Dictionary<string, JsonValue> properties),
      Null
  }");
            
            Console.WriteLine("\nPattern Matching Examples:");
            Console.WriteLine(@"
  var result = GetResult();
  return result switch
  {
      Ok(var value) => $""Success: {value}"",
      Err(var error) => $""Error: {error}""
  };
  
  var option = GetOption();
  if (option is Some(var value))
  {
      Console.WriteLine($""Value: {value}"");
  }");
        }
    }
    
    /// <summary>
    /// Extension methods to demonstrate working with the new syntax nodes
    /// </summary>
    public static class UnionSyntaxExtensions
    {
        /// <summary>
        /// Creates a union type syntax node programmatically
        /// </summary>
        public static UnionTypeSyntax CreateUnionType(params TypeSyntax[] types)
        {
            var separatedList = SyntaxFactory.SeparatedList(types);
            return SyntaxFactory.UnionType(
                SyntaxFactory.Token(SyntaxKind.OpenParenToken),
                separatedList,
                SyntaxFactory.Token(SyntaxKind.CloseParenToken)
            );
        }
        
        /// <summary>
        /// Creates a tagged union declaration syntax node programmatically
        /// </summary>
        public static TaggedUnionDeclarationSyntax CreateTaggedUnion(
            string name, 
            params (string caseName, ParameterListSyntax? parameters)[] cases)
        {
            var unionCases = cases.Select(c => 
                SyntaxFactory.UnionCaseDeclaration(
                    SyntaxFactory.List<AttributeListSyntax>(),
                    SyntaxFactory.TokenList(),
                    SyntaxFactory.Identifier(c.caseName),
                    c.parameters
                )
            );
            
            return SyntaxFactory.TaggedUnionDeclaration(
                SyntaxFactory.List<AttributeListSyntax>(),
                SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword)),
                SyntaxFactory.Token(SyntaxKind.UnionKeyword),
                SyntaxFactory.Identifier(name),
                null, // type parameters
                null, // base list
                SyntaxFactory.Token(SyntaxKind.OpenBraceToken),
                SyntaxFactory.SeparatedList(unionCases),
                SyntaxFactory.Token(SyntaxKind.CloseBraceToken),
                SyntaxFactory.Token(SyntaxKind.SemicolonToken)
            );
        }
    }
} 