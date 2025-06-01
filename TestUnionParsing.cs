using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

class TestUnionParsing
{
    static void Main()
    {
        TestTaggedUnions();
        TestUnionTypes();
    }
    
    static void TestTaggedUnions()
    {
        // Test parsing a simple tagged union
        string unionCode = @"
public union Result
{
    Success(string value),
    Error(int code)
}";

        Console.WriteLine("=== Testing Tagged Union Parsing ===");
        
        try
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(unionCode);
            var root = syntaxTree.GetRoot();
            
            Console.WriteLine("Parse succeeded!");
            Console.WriteLine($"Root kind: {root.Kind()}");
            
            // Look for the union declaration
            var unionDecl = root.DescendantNodes().OfType<TaggedUnionDeclarationSyntax>().FirstOrDefault();
            if (unionDecl != null)
            {
                Console.WriteLine($"Found union: {unionDecl.Identifier.ValueText}");
                Console.WriteLine($"Union keyword: {unionDecl.UnionKeyword}");
                Console.WriteLine($"Members count: {unionDecl.Members.Count}");
                
                foreach (var member in unionDecl.Members)
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
                Console.WriteLine("No union declaration found");
            }
            
            // Print any diagnostics
            var diagnostics = syntaxTree.GetDiagnostics();
            if (diagnostics.Any())
            {
                Console.WriteLine("\nDiagnostics:");
                foreach (var diagnostic in diagnostics)
                {
                    Console.WriteLine($"  {diagnostic}");
                }
            }
            else
            {
                Console.WriteLine("\nNo parsing errors!");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Parse failed: {ex.Message}");
        }
    }
    
    static void TestUnionTypes()
    {
        // Test parsing union types in variable declarations
        string unionTypeCode = @"
public class Test 
{
    public (string | int | bool) value;
    public (HttpResponse | TimeoutError) result;
}";

        Console.WriteLine("\n=== Testing Union Type Parsing ===");
        
        try
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(unionTypeCode);
            var root = syntaxTree.GetRoot();
            
            Console.WriteLine("Parse succeeded!");
            
            // Look for union types
            var unionTypes = root.DescendantNodes().OfType<UnionTypeSyntax>().ToList();
            Console.WriteLine($"Found {unionTypes.Count} union types");
            
            foreach (var unionType in unionTypes)
            {
                Console.WriteLine($"Union type: {unionType}");
                Console.WriteLine($"  Types count: {unionType.Types.Count}");
                foreach (var type in unionType.Types)
                {
                    Console.WriteLine($"    - {type}");
                }
            }
            
            // Print any diagnostics
            var diagnostics = syntaxTree.GetDiagnostics();
            if (diagnostics.Any())
            {
                Console.WriteLine("\nDiagnostics:");
                foreach (var diagnostic in diagnostics)
                {
                    Console.WriteLine($"  {diagnostic}");
                }
            }
            else
            {
                Console.WriteLine("\nNo parsing errors!");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Parse failed: {ex.Message}");
        }
    }
} 