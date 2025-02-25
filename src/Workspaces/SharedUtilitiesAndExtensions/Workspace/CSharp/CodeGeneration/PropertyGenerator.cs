﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis.CodeGeneration;
using Microsoft.CodeAnalysis.CodeStyle;
using Microsoft.CodeAnalysis.CSharp.Extensions;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.PooledObjects;
using Microsoft.CodeAnalysis.Shared.Extensions;
using Roslyn.Utilities;

namespace Microsoft.CodeAnalysis.CSharp.CodeGeneration;

using static CodeGenerationHelpers;
using static CSharpCodeGenerationHelpers;
using static CSharpSyntaxTokens;
using static SyntaxFactory;

internal static class PropertyGenerator
{
    public static bool CanBeGenerated(IPropertySymbol property)
        => property.IsIndexer || property.Parameters.Length == 0;

    private static MemberDeclarationSyntax? LastPropertyOrField(
        SyntaxList<MemberDeclarationSyntax> members)
    {
        var lastProperty = members.LastOrDefault(m => m is PropertyDeclarationSyntax);
        return lastProperty ?? LastField(members);
    }

    internal static CompilationUnitSyntax AddPropertyTo(
        CompilationUnitSyntax destination,
        IPropertySymbol property,
        CSharpCodeGenerationContextInfo info,
        IList<bool>? availableIndices,
        CancellationToken cancellationToken)
    {
        var declaration = GeneratePropertyOrIndexer(
            property, CodeGenerationDestination.CompilationUnit, info,
            cancellationToken);

        var members = Insert(destination.Members, declaration, info,
            availableIndices, after: LastPropertyOrField, before: FirstMember);
        return destination.WithMembers(members);
    }

    internal static TypeDeclarationSyntax AddPropertyTo(
        TypeDeclarationSyntax destination,
        IPropertySymbol property,
        CSharpCodeGenerationContextInfo info,
        IList<bool>? availableIndices,
        CancellationToken cancellationToken)
    {
        var declaration = GeneratePropertyOrIndexer(property, GetDestination(destination), info, cancellationToken);

        // Create a clone of the original type with the new method inserted. 
        var members = Insert(destination.Members, declaration, info,
            availableIndices, after: LastPropertyOrField, before: FirstMember);

        // Find the best place to put the field.  It should go after the last field if we already
        // have fields, or at the beginning of the file if we don't.
        return AddMembersTo(destination, members, cancellationToken);
    }

    public static MemberDeclarationSyntax GeneratePropertyOrIndexer(
        IPropertySymbol property,
        CodeGenerationDestination destination,
        CSharpCodeGenerationContextInfo info,
        CancellationToken cancellationToken)
    {
        var reusableSyntax = GetReuseableSyntaxNodeForSymbol<MemberDeclarationSyntax>(property, info);
        if (reusableSyntax != null)
        {
            return reusableSyntax;
        }

        var declaration = property.IsIndexer
            ? GenerateIndexerDeclaration(property, destination, info, cancellationToken)
            : GeneratePropertyDeclaration(property, destination, info, cancellationToken);

        return ConditionallyAddDocumentationCommentTo(declaration, property, info, cancellationToken);
    }

    private static MemberDeclarationSyntax GenerateIndexerDeclaration(
        IPropertySymbol property,
        CodeGenerationDestination destination,
        CSharpCodeGenerationContextInfo info,
        CancellationToken cancellationToken)
    {
        var explicitInterfaceSpecifier = GenerateExplicitInterfaceSpecifier(property.ExplicitInterfaceImplementations);

        var isExplicit = explicitInterfaceSpecifier is not null;
        var declaration = IndexerDeclaration(
                attributeLists: GenerateAttributes(property, isExplicit, info),
                modifiers: GenerateModifiers(property, destination, info),
                type: GenerateTypeSyntax(property),
                explicitInterfaceSpecifier: explicitInterfaceSpecifier,
                parameterList: ParameterGenerator.GenerateBracketedParameterList(property.Parameters, isExplicit: isExplicit, info),
                accessorList: GenerateAccessorList(property, destination, info, cancellationToken));
        declaration = UseExpressionBodyIfDesired(info, declaration, cancellationToken);

        return AddFormatterAndCodeGeneratorAnnotationsTo(
            AddAnnotationsTo(property, declaration));
    }

    private static PropertyDeclarationSyntax GeneratePropertyDeclaration(
       IPropertySymbol property, CodeGenerationDestination destination,
       CSharpCodeGenerationContextInfo info, CancellationToken cancellationToken)
    {
        var initializer = CodeGenerationPropertyInfo.GetInitializer(property) is ExpressionSyntax initializerNode
            ? EqualsValueClause(initializerNode)
            : null;

        var explicitInterfaceSpecifier = GenerateExplicitInterfaceSpecifier(property.ExplicitInterfaceImplementations);

        var isExplicit = explicitInterfaceSpecifier is not null;
        var accessorList = GenerateAccessorList(property, destination, info, cancellationToken);

        var propertyDeclaration = PropertyDeclaration(
            attributeLists: GenerateAttributes(property, isExplicit, info),
            modifiers: GenerateModifiers(property, destination, info),
            type: GenerateTypeSyntax(property),
            explicitInterfaceSpecifier: explicitInterfaceSpecifier,
            identifier: property.Name.ToIdentifierToken(),
            accessorList: accessorList,
            expressionBody: null,
            initializer: initializer,
            semicolonToken: initializer is null ? default : SemicolonToken);

        propertyDeclaration = UseExpressionBodyIfDesired(info, propertyDeclaration, cancellationToken);

        return AddFormatterAndCodeGeneratorAnnotationsTo(
            AddAnnotationsTo(property, propertyDeclaration));
    }

    private static SyntaxList<AttributeListSyntax> GenerateAttributes(
        IPropertySymbol property, bool isExplicit, CSharpCodeGenerationContextInfo info)
    {
        if (isExplicit)
        {
            return default;
        }

        var attributes = property.GetAttributes();
        if (attributes.Length == 0)
        {
            return default;
        }

        return AttributeGenerator.GenerateAttributeLists(attributes, info);
    }

    private static TypeSyntax GenerateTypeSyntax(IPropertySymbol property)
    {
        var returnType = property.Type;

        if (property.ReturnsByRef)
        {
            return returnType.GenerateRefTypeSyntax();
        }
        else if (property.ReturnsByRefReadonly)
        {
            return returnType.GenerateRefReadOnlyTypeSyntax();
        }
        else
        {
            return returnType.GenerateTypeSyntax();
        }
    }

    private static bool TryGetExpressionBody(
        BasePropertyDeclarationSyntax baseProperty, LanguageVersion languageVersion, ExpressionBodyPreference preference, CancellationToken cancellationToken,
        [NotNullWhen(true)] out ArrowExpressionClauseSyntax? arrowExpression, out SyntaxToken semicolonToken)
    {
        var accessorList = baseProperty.AccessorList;
        if (preference != ExpressionBodyPreference.Never &&
            accessorList?.Accessors.Count == 1)
        {
            var accessor = accessorList.Accessors[0];
            if (accessor.IsKind(SyntaxKind.GetAccessorDeclaration))
            {
                return TryGetArrowExpressionBody(
                    baseProperty.Kind(), accessor, languageVersion, preference, cancellationToken,
                    out arrowExpression, out semicolonToken);
            }
        }

        arrowExpression = null;
        semicolonToken = default;
        return false;
    }

    private static PropertyDeclarationSyntax UseExpressionBodyIfDesired(
        CSharpCodeGenerationContextInfo info, PropertyDeclarationSyntax declaration, CancellationToken cancellationToken)
    {
        if (declaration.ExpressionBody == null &&
            declaration.Initializer == null &&
            TryGetExpressionBody(
                declaration, info.LanguageVersion, info.Options.PreferExpressionBodiedProperties.Value, cancellationToken,
                out var expressionBody, out var semicolonToken))
        {
            declaration = declaration.WithAccessorList(null)
                                     .WithExpressionBody(expressionBody)
                                     .WithSemicolonToken(semicolonToken);
        }

        return declaration;
    }

    private static IndexerDeclarationSyntax UseExpressionBodyIfDesired(
        CSharpCodeGenerationContextInfo info, IndexerDeclarationSyntax declaration, CancellationToken cancellationToken)
    {
        if (declaration.ExpressionBody == null)
        {
            if (TryGetExpressionBody(
                    declaration, info.LanguageVersion, info.Options.PreferExpressionBodiedIndexers.Value, cancellationToken,
                    out var expressionBody, out var semicolonToken))
            {
                declaration = declaration.WithAccessorList(null)
                                         .WithExpressionBody(expressionBody)
                                         .WithSemicolonToken(semicolonToken);
            }
        }

        return declaration;
    }

    private static AccessorDeclarationSyntax UseExpressionBodyIfDesired(
        CSharpCodeGenerationContextInfo info, AccessorDeclarationSyntax declaration, CancellationToken cancellationToken)
    {
        if (declaration.ExpressionBody == null)
        {
            if (declaration.Body?.TryConvertToArrowExpressionBody(
                declaration.Kind(), info.LanguageVersion, info.Options.PreferExpressionBodiedAccessors.Value, cancellationToken,
                out var expressionBody, out var semicolonToken) == true)
            {
                declaration = declaration.WithBody(null)
                                         .WithExpressionBody(expressionBody)
                                         .WithSemicolonToken(semicolonToken);
            }
        }

        return declaration;
    }

    private static bool TryGetArrowExpressionBody(
        SyntaxKind declarationKind, AccessorDeclarationSyntax accessor, LanguageVersion languageVersion, ExpressionBodyPreference preference, CancellationToken cancellationToken,
        [NotNullWhen(true)] out ArrowExpressionClauseSyntax? arrowExpression, out SyntaxToken semicolonToken)
    {
        // If the accessor has an expression body already, then use that as the expression body
        // for the property.
        if (accessor.ExpressionBody != null)
        {
            arrowExpression = accessor.ExpressionBody;
            semicolonToken = accessor.SemicolonToken;
            return true;
        }

        if (accessor.Body == null)
        {
            arrowExpression = null;
            semicolonToken = default;
            return false;
        }

        return accessor.Body.TryConvertToArrowExpressionBody(
            declarationKind, languageVersion, preference, cancellationToken, out arrowExpression, out semicolonToken);
    }

    private static AccessorListSyntax? GenerateAccessorList(
        IPropertySymbol property, CodeGenerationDestination destination,
        CSharpCodeGenerationContextInfo info, CancellationToken cancellationToken)
    {
        var setAccessorKind = property.SetMethod?.IsInitOnly == true ? SyntaxKind.InitAccessorDeclaration : SyntaxKind.SetAccessorDeclaration;
        var accessors = new[]
        {
            GenerateAccessorDeclaration(property, property.GetMethod, SyntaxKind.GetAccessorDeclaration, destination, info, cancellationToken),
            GenerateAccessorDeclaration(property, property.SetMethod, setAccessorKind, destination, info, cancellationToken),
        };

        return accessors[0] == null && accessors[1] == null
            ? null
            : AccessorList([.. accessors.WhereNotNull()]);
    }

    private static AccessorDeclarationSyntax? GenerateAccessorDeclaration(
        IPropertySymbol property,
        IMethodSymbol? accessor,
        SyntaxKind kind,
        CodeGenerationDestination destination,
        CSharpCodeGenerationContextInfo info,
        CancellationToken cancellationToken)
    {
        var hasBody = info.Context.GenerateMethodBodies && HasAccessorBodies(property, destination, accessor);
        return accessor == null
            ? null
            : GenerateAccessorDeclaration(property, accessor, kind, hasBody, info, cancellationToken);
    }

    private static AccessorDeclarationSyntax GenerateAccessorDeclaration(
        IPropertySymbol property,
        IMethodSymbol accessor,
        SyntaxKind kind,
        bool hasBody,
        CSharpCodeGenerationContextInfo info,
        CancellationToken cancellationToken)
    {
        var declaration = AccessorDeclaration(kind)
                                       .WithModifiers(GenerateAccessorModifiers(property, accessor, info))
                                       .WithBody(hasBody ? GenerateBlock(accessor) : null)
                                       .WithSemicolonToken(hasBody ? default : SemicolonToken);

        declaration = UseExpressionBodyIfDesired(info, declaration, cancellationToken);

        return AddAnnotationsTo(accessor, declaration);
    }

    private static BlockSyntax GenerateBlock(IMethodSymbol accessor)
    {
        return Block(
            StatementGenerator.GenerateStatements(CodeGenerationMethodInfo.GetStatements(accessor)));
    }

    private static bool HasAccessorBodies(
        IPropertySymbol property,
        CodeGenerationDestination destination,
        IMethodSymbol? accessor)
    {
        return destination != CodeGenerationDestination.InterfaceType &&
            !property.IsAbstract &&
            accessor != null &&
            !accessor.IsAbstract;
    }

    private static SyntaxTokenList GenerateAccessorModifiers(
        IPropertySymbol property,
        IMethodSymbol accessor,
        CSharpCodeGenerationContextInfo info)
    {
        using var _ = ArrayBuilder<SyntaxToken>.GetInstance(out var modifiers);

        if (accessor.DeclaredAccessibility != Accessibility.NotApplicable &&
            accessor.DeclaredAccessibility != property.DeclaredAccessibility)
        {
            AddAccessibilityModifiers(accessor.DeclaredAccessibility, modifiers, info, property.DeclaredAccessibility);
        }

        var hasNonReadOnlyAccessor = property.GetMethod?.IsReadOnly == false || property.SetMethod?.IsReadOnly == false;
        if (hasNonReadOnlyAccessor && accessor.IsReadOnly)
            modifiers.Add(ReadOnlyKeyword);

        return [.. modifiers];
    }

    private static SyntaxTokenList GenerateModifiers(
        IPropertySymbol property, CodeGenerationDestination destination, CSharpCodeGenerationContextInfo info)
    {
        var tokens = ArrayBuilder<SyntaxToken>.GetInstance();

        // Only "static" allowed if we're an explicit impl.
        if (property.ExplicitInterfaceImplementations.Any())
        {
            if (property.IsStatic)
                tokens.Add(StaticKeyword);
        }
        else
        {
            // If we're generating into an interface, then allow modifiers for static abstract members
            if (destination is CodeGenerationDestination.InterfaceType)
            {
                if (property.IsStatic)
                {
                    tokens.Add(StaticKeyword);

                    if (property.IsAbstract)
                        tokens.Add(AbstractKeyword);
                }
            }
            else if (destination is not CodeGenerationDestination.CompilationUnit)
            {
                AddAccessibilityModifiers(property.DeclaredAccessibility, tokens, info, Accessibility.Private);

                if (property.IsStatic)
                    tokens.Add(StaticKeyword);

                // note: explicit interface impls are allowed to be 'readonly' but it never actually affects callers
                // because of the boxing requirement in order to call the method.
                // therefore it seems like a small oversight to leave out the keyword for an explicit impl from metadata.
                var hasAllReadOnlyAccessors = property.GetMethod?.IsReadOnly != false && property.SetMethod?.IsReadOnly != false;

                // Don't show the readonly modifier if the containing type is already readonly
                if (hasAllReadOnlyAccessors && !property.ContainingType.IsReadOnly)
                    tokens.Add(ReadOnlyKeyword);

                if (property.IsSealed)
                    tokens.Add(SealedKeyword);

                if (property.IsOverride)
                    tokens.Add(OverrideKeyword);

                if (property.IsVirtual)
                    tokens.Add(VirtualKeyword);

                if (property.IsAbstract)
                    tokens.Add(AbstractKeyword);

                if (property.IsRequired)
                    tokens.Add(RequiredKeyword);
            }
        }

        if (CodeGenerationPropertyInfo.GetIsUnsafe(property))
            tokens.Add(UnsafeKeyword);

        return [.. tokens];
    }
}
