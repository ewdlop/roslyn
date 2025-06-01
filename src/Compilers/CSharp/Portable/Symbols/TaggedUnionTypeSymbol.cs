// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#nullable disable

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis.PooledObjects;
using Roslyn.Utilities;

namespace Microsoft.CodeAnalysis.CSharp.Symbols
{
    /// <summary>
    /// Represents a tagged union type symbol declared with the 'union' keyword.
    /// </summary>
    internal sealed class TaggedUnionTypeSymbol : SourceMemberContainerTypeSymbol
    {
        private readonly ImmutableArray<UnionCaseSymbol> _cases;

        internal TaggedUnionTypeSymbol(
            NamespaceOrTypeSymbol containingSymbol,
            SyntaxReference syntaxReference,
            Location location,
            DiagnosticBag diagnostics)
            : base(containingSymbol, syntaxReference, location, diagnostics)
        {
            // Initialize cases from syntax - for now empty
            _cases = ImmutableArray<UnionCaseSymbol>.Empty;
        }

        /// <summary>
        /// The union cases defined in this tagged union.
        /// </summary>
        public ImmutableArray<UnionCaseSymbol> Cases => _cases;

        public override TypeKind TypeKind => TypeKind.TaggedUnion;

        public override SymbolKind Kind => SymbolKind.NamedType;

        protected override Location GetCorrespondingBaseListLocation(NamedTypeSymbol @base)
        {
            // Tagged unions don't have base lists in the traditional sense
            return null;
        }

        internal override NamedTypeSymbol BaseTypeNoUseSiteDiagnostics
        {
            get
            {
                // Tagged unions derive from object by default
                return DeclaringCompilation?.GetSpecialType(SpecialType.System_Object);
            }
        }

        internal override ImmutableArray<NamedTypeSymbol> InterfacesNoUseSiteDiagnostics(ConsList<TypeSymbol> basesBeingResolved = null)
        {
            // For now, tagged unions don't implement interfaces by default
            return ImmutableArray<NamedTypeSymbol>.Empty;
        }

        public override IEnumerable<string> MemberNames
        {
            get
            {
                return _cases.Select(c => c.Name);
            }
        }

        public override ImmutableArray<Symbol> GetMembers()
        {
            return _cases.CastArray<Symbol>();
        }

        public override ImmutableArray<Symbol> GetMembers(string name)
        {
            return _cases.Where(c => c.Name == name).CastArray<Symbol>();
        }

        internal override ImmutableArray<Symbol> GetEarlyAttributeDecodingMembers()
        {
            return GetMembers();
        }

        internal override ImmutableArray<Symbol> GetEarlyAttributeDecodingMembers(string name)
        {
            return GetMembers(name);
        }

        internal override IEnumerable<FieldSymbol> GetFieldsToEmit()
        {
            // Tagged unions may need special fields for runtime representation
            return SpecializedCollections.EmptyEnumerable<FieldSymbol>();
        }

        internal override IEnumerable<MethodSymbol> GetMethodsToEmit()
        {
            // Tagged unions may need special methods (constructors, etc.)
            return SpecializedCollections.EmptyEnumerable<MethodSymbol>();
        }

        internal override void AddSynthesizedMembers(ArrayBuilder<Symbol> members)
        {
            // Add synthesized members like constructors, equality operators, etc.
            // For now, keep it simple
        }

        internal override ImmutableArray<NamedTypeSymbol> GetInterfacesToEmit()
        {
            return InterfacesNoUseSiteDiagnostics();
        }

        internal override NamedTypeSymbol GetDeclaredBaseType(ConsList<TypeSymbol> basesBeingResolved)
        {
            return BaseTypeNoUseSiteDiagnostics;
        }

        internal override ImmutableArray<NamedTypeSymbol> GetDeclaredInterfaces(ConsList<TypeSymbol> basesBeingResolved)
        {
            return InterfacesNoUseSiteDiagnostics(basesBeingResolved);
        }

        public override string ToDisplayString(SymbolDisplayFormat format = null)
        {
            return $"union {Name}";
        }
    }

    /// <summary>
    /// Represents a case within a tagged union.
    /// </summary>
    internal sealed class UnionCaseSymbol : Symbol
    {
        private readonly TaggedUnionTypeSymbol _containingUnion;
        private readonly string _name;
        private readonly ImmutableArray<ParameterSymbol> _parameters;
        private readonly Location _location;

        internal UnionCaseSymbol(
            TaggedUnionTypeSymbol containingUnion,
            string name,
            ImmutableArray<ParameterSymbol> parameters,
            Location location)
        {
            Debug.Assert(containingUnion != null);
            Debug.Assert(!string.IsNullOrEmpty(name));

            _containingUnion = containingUnion;
            _name = name;
            _parameters = parameters;
            _location = location;
        }

        public override string Name => _name;

        public override SymbolKind Kind => SymbolKind.Field; // Union cases are similar to enum members

        public override Symbol ContainingSymbol => _containingUnion;

        public override Accessibility DeclaredAccessibility => Accessibility.Public;

        public override bool IsStatic => false;

        public override bool IsVirtual => false;

        public override bool IsOverride => false;

        public override bool IsAbstract => false;

        public override bool IsSealed => false;

        public override bool IsExtern => false;

        public override ImmutableArray<Location> Locations => ImmutableArray.Create(_location);

        public override ImmutableArray<SyntaxReference> DeclaringSyntaxReferences => ImmutableArray<SyntaxReference>.Empty;

        /// <summary>
        /// The parameters for this union case (if any).
        /// </summary>
        public ImmutableArray<ParameterSymbol> Parameters => _parameters;

        /// <summary>
        /// Whether this union case has parameters.
        /// </summary>
        public bool HasParameters => !_parameters.IsEmpty;

        public override void Accept(SymbolVisitor visitor)
        {
            visitor.VisitField(this);
        }

        public override TResult Accept<TResult>(SymbolVisitor<TResult> visitor)
        {
            return visitor.VisitField(this);
        }

        internal override void Accept(CSharpSymbolVisitor visitor)
        {
            // Union case doesn't map directly to C# symbol visitor
            // May need custom handling
        }

        internal override TResult Accept<TResult>(CSharpSymbolVisitor<TResult> visitor)
        {
            // Union case doesn't map directly to C# symbol visitor
            // May need custom handling
            return default(TResult);
        }

        public override string ToDisplayString(SymbolDisplayFormat format = null)
        {
            if (!HasParameters)
                return _name;

            var builder = PooledStringBuilder.GetInstance();
            var sb = builder.Builder;
            
            sb.Append(_name);
            sb.Append('(');
            for (int i = 0; i < _parameters.Length; i++)
            {
                if (i > 0)
                    sb.Append(", ");
                sb.Append(_parameters[i].ToDisplayString(format));
            }
            sb.Append(')');
            
            return builder.ToStringAndFree();
        }
    }
} 