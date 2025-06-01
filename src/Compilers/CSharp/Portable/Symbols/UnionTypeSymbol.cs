// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#nullable disable

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using Microsoft.CodeAnalysis.PooledObjects;
using Roslyn.Utilities;

namespace Microsoft.CodeAnalysis.CSharp.Symbols
{
    /// <summary>
    /// Represents a union type symbol (A | B | C).
    /// </summary>
    internal sealed class UnionTypeSymbol : NamedTypeSymbol
    {
        private readonly ImmutableArray<TypeWithAnnotations> _types;
        private readonly bool _isNullable;

        internal UnionTypeSymbol(ImmutableArray<TypeWithAnnotations> types, bool isNullable = false)
        {
            Debug.Assert(!types.IsDefaultOrEmpty);
            Debug.Assert(types.Length >= 2);
            
            _types = types;
            _isNullable = isNullable;
        }

        /// <summary>
        /// The types that comprise this union.
        /// </summary>
        public ImmutableArray<TypeWithAnnotations> Types => _types;

        public override TypeKind TypeKind => TypeKind.Union;

        public override SymbolKind Kind => SymbolKind.NamedType;

        public override bool IsReferenceType => CheckAllTypes(static t => t.Type.IsReferenceType);

        public override bool IsValueType => CheckAllTypes(static t => t.Type.IsValueType);

        internal override bool IsNullableType => _isNullable;

        public override string Name => string.Empty;

        internal override bool MangleName => false;

        public override int Arity => 0;

        public override ImmutableArray<TypeParameterSymbol> TypeParameters => ImmutableArray<TypeParameterSymbol>.Empty;

        public override NamedTypeSymbol ConstructedFrom => this;

        public override bool MightContainExtensionMethods => false;

        internal override bool IsFileLocal => false;

        internal override FileIdentifier AssociatedFileIdentifier => null;

        public override ImmutableArray<Symbol> GetMembers() => ImmutableArray<Symbol>.Empty;

        public override ImmutableArray<Symbol> GetMembers(string name) => ImmutableArray<Symbol>.Empty;

        public override ImmutableArray<NamedTypeSymbol> GetTypeMembers() => ImmutableArray<NamedTypeSymbol>.Empty;

        public override ImmutableArray<NamedTypeSymbol> GetTypeMembers(string name) => ImmutableArray<NamedTypeSymbol>.Empty;

        public override ImmutableArray<NamedTypeSymbol> GetTypeMembers(string name, int arity) => ImmutableArray<NamedTypeSymbol>.Empty;

        public override Accessibility DeclaredAccessibility => Accessibility.NotApplicable;

        public override bool IsStatic => false;

        public override bool IsAbstract => false;

        public override bool IsSealed => false;

        internal override NamedTypeSymbol BaseTypeNoUseSiteDiagnostics => null;

        internal override ImmutableArray<NamedTypeSymbol> InterfacesNoUseSiteDiagnostics(ConsList<TypeSymbol> basesBeingResolved = null)
            => ImmutableArray<NamedTypeSymbol>.Empty;

        internal override NamedTypeSymbol GetDeclaredBaseType(ConsList<TypeSymbol> basesBeingResolved) => null;

        internal override ImmutableArray<NamedTypeSymbol> GetDeclaredInterfaces(ConsList<TypeSymbol> basesBeingResolved)
            => ImmutableArray<NamedTypeSymbol>.Empty;

        public override Symbol ContainingSymbol => null;

        public override ImmutableArray<Location> Locations => ImmutableArray<Location>.Empty;

        public override ImmutableArray<SyntaxReference> DeclaringSyntaxReferences => ImmutableArray<SyntaxReference>.Empty;

        internal override bool HasSpecialName => false;

        internal override bool IsWindowsRuntimeImport => false;

        internal override bool ShouldAddWinRTMembers => false;

        internal override TypeLayout Layout => default;

        internal override CharSet MarshallingCharSet => DefaultMarshallingCharSet;

        internal override bool HasDeclarativeSecurity => false;

        internal override IEnumerable<Microsoft.Cci.SecurityAttribute> GetSecurityInformation() 
            => SpecializedCollections.EmptyEnumerable<Microsoft.Cci.SecurityAttribute>();

        internal override AttributeUsageInfo GetAttributeUsageInfo() => AttributeUsageInfo.Default;

        internal override NamedTypeSymbol ComImportCoClass => null;

        internal override bool IsComImport => false;

        internal override ImmutableArray<Symbol> GetEarlyAttributeDecodingMembers() => ImmutableArray<Symbol>.Empty;

        internal override ImmutableArray<Symbol> GetEarlyAttributeDecodingMembers(string name) => ImmutableArray<Symbol>.Empty;

        internal override IEnumerable<string> MemberNames => SpecializedCollections.EmptyEnumerable<string>();

        internal override ImmutableArray<TypeWithAnnotations> TypeArgumentsWithAnnotationsNoUseSiteDiagnostics 
            => ImmutableArray<TypeWithAnnotations>.Empty;

        protected override NamedTypeSymbol WithTupleDataCore(TupleExtraData newData) => this;

        public override int GetHashCode()
        {
            var hash = 0;
            foreach (var type in _types)
            {
                hash = Hash.Combine(type.GetHashCode(), hash);
            }
            return Hash.Combine(hash, (int)TypeKind.Union);
        }

        internal override bool Equals(TypeSymbol other, TypeCompareKind comparison)
        {
            if (ReferenceEquals(this, other))
                return true;

            if (other is not UnionTypeSymbol otherUnion)
                return false;

            if (_types.Length != otherUnion._types.Length)
                return false;

            for (int i = 0; i < _types.Length; i++)
            {
                if (!_types[i].Equals(otherUnion._types[i], comparison))
                    return false;
            }

            return true;
        }

        public override void Accept(SymbolVisitor visitor)
        {
            visitor.VisitNamedType(this);
        }

        public override TResult Accept<TResult>(SymbolVisitor<TResult> visitor)
        {
            return visitor.VisitNamedType(this);
        }

        internal override void Accept(CSharpSymbolVisitor visitor)
        {
            visitor.VisitNamedType(this);
        }

        internal override TResult Accept<TResult>(CSharpSymbolVisitor<TResult> visitor)
        {
            return visitor.VisitNamedType(this);
        }

        /// <summary>
        /// Check if all types in the union satisfy a predicate.
        /// </summary>
        private bool CheckAllTypes(Func<TypeWithAnnotations, bool> predicate)
        {
            foreach (var type in _types)
            {
                if (!predicate(type))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Create a union type from an array of types.
        /// </summary>
        internal static UnionTypeSymbol Create(ImmutableArray<TypeWithAnnotations> types, bool isNullable = false)
        {
            return new UnionTypeSymbol(types, isNullable);
        }

        /// <summary>
        /// Create a union type from a list of types, normalizing duplicates.
        /// </summary>
        internal static UnionTypeSymbol CreateNormalized(ImmutableArray<TypeWithAnnotations> types, bool isNullable = false)
        {
            if (types.Length < 2)
            {
                throw new ArgumentException("Union type must have at least 2 types", nameof(types));
            }

            // TODO: Remove duplicates and normalize
            // For now, just create the union as-is
            return new UnionTypeSymbol(types, isNullable);
        }

        public override string ToDisplayString(SymbolDisplayFormat format = null)
        {
            var builder = PooledStringBuilder.GetInstance();
            var sb = builder.Builder;
            
            sb.Append('(');
            for (int i = 0; i < _types.Length; i++)
            {
                if (i > 0)
                    sb.Append(" | ");
                sb.Append(_types[i].ToDisplayString(format));
            }
            sb.Append(')');
            
            return builder.ToStringAndFree();
        }
    }
} 