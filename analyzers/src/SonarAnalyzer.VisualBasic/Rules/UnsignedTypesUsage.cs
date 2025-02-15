﻿/*
 * SonarAnalyzer for .NET
 * Copyright (C) 2015-2022 SonarSource SA
 * mailto: contact AT sonarsource DOT com
 *
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 3 of the License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public License
 * along with this program; if not, write to the Free Software Foundation,
 * Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
 */

using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.VisualBasic;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using SonarAnalyzer.Helpers;

namespace SonarAnalyzer.Rules.VisualBasic
{
    [DiagnosticAnalyzer(LanguageNames.VisualBasic)]
    public sealed class UnsignedTypesUsage : SonarDiagnosticAnalyzer
    {
        internal const string DiagnosticId = "S2374";
        private const string MessageFormat = "Change this unsigned type to '{0}'.";

        private static readonly DiagnosticDescriptor rule =
            DiagnosticDescriptorBuilder.GetDescriptor(DiagnosticId, MessageFormat, RspecStrings.ResourceManager);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create(rule);

        protected override void Initialize(SonarAnalysisContext context)
        {
            context.RegisterSyntaxNodeActionInNonGenerated(
                c =>
                {
                    var typeSyntax = (TypeSyntax)c.Node;
                    if (typeSyntax.Parent is QualifiedNameSyntax)
                    {
                        return;
                    }

                    var typeSymbol = c.SemanticModel.GetSymbolInfo(typeSyntax).Symbol as ITypeSymbol;
                    if (typeSymbol.IsAny(KnownType.UnsignedIntegers))
                    {
                        c.ReportIssue(Diagnostic.Create(rule, typeSyntax.GetLocation(),
                            SignedPairs[typeSymbol.SpecialType]));
                    }
                },
                SyntaxKind.PredefinedType,
                SyntaxKind.IdentifierName,
                SyntaxKind.QualifiedName);
        }

        private static readonly IDictionary<SpecialType, string> SignedPairs =
            new Dictionary<SpecialType, string>
            {
                {SpecialType.System_UInt16, "Short"},
                {SpecialType.System_UInt32, "Integer"},
                {SpecialType.System_UInt64, "Long"}
            };
    }
}
