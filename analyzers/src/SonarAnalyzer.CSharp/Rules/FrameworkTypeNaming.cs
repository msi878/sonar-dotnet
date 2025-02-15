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
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using SonarAnalyzer.Helpers;

namespace SonarAnalyzer.Rules.CSharp
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class FrameworkTypeNaming : SonarDiagnosticAnalyzer
    {
        private const string DiagnosticId = "S3376";
        private const string MessageFormat = "Make this class name end with '{0}'.";
        private const int SelfAndBaseTypesCount = 2;

        private static readonly DiagnosticDescriptor Rule = DiagnosticDescriptorBuilder.GetDescriptor(DiagnosticId, MessageFormat, RspecStrings.ResourceManager);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create(Rule);

        protected override void Initialize(SonarAnalysisContext context) =>
            context.RegisterSyntaxNodeActionInNonGenerated(
                c =>
                {
                    var classDeclaration = (ClassDeclarationSyntax)c.Node;
                    var symbol = c.SemanticModel.GetDeclaredSymbol(classDeclaration);
                    if (symbol == null)
                    {
                        return;
                    }

                    var baseTypes = symbol.BaseType.GetSelfAndBaseTypes().ToList();
                    if (baseTypes.Count < SelfAndBaseTypesCount || !baseTypes.Last().Is(KnownType.System_Object))
                    {
                        return;
                    }

                    var baseTypeKey = FrameworkTypesWithEnding.Keys
                                                              .FirstOrDefault(ft => baseTypes[baseTypes.Count - SelfAndBaseTypesCount].ToDisplayString().Equals(ft, System.StringComparison.Ordinal));

                    if (baseTypeKey == null)
                    {
                        return;
                    }

                    var baseTypeName = FrameworkTypesWithEnding[baseTypeKey];

                    if (symbol.Name.EndsWith(baseTypeName, System.StringComparison.Ordinal)
                        || !baseTypes[0].Name.EndsWith(baseTypeName, System.StringComparison.Ordinal))
                    {
                        return;
                    }

                    c.ReportIssue(Diagnostic.Create(Rule, classDeclaration.Identifier.GetLocation(), baseTypeName));
                },
                SyntaxKind.ClassDeclaration);

        private static readonly Dictionary<string, string> FrameworkTypesWithEnding = new Dictionary<string, string>
        {
            { "System.Exception", "Exception" },
            { "System.EventArgs", "EventArgs" },
            { "System.Attribute", "Attribute" }
        };
    }
}
