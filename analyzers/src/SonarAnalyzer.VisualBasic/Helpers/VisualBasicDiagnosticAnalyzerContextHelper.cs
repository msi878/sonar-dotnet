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

using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace SonarAnalyzer.Helpers
{
    public static class VisualBasicDiagnosticAnalyzerContextHelper
    {
        public static void RegisterSyntaxNodeActionInNonGenerated<TSyntaxKind>(this SonarAnalysisContext context,
                                                                               Action<SyntaxNodeAnalysisContext> action,
                                                                               params TSyntaxKind[] syntaxKinds) where TSyntaxKind : struct =>
            context.RegisterSyntaxNodeActionInNonGenerated(VisualBasicGeneratedCodeRecognizer.Instance, action, syntaxKinds);

        public static void RegisterSyntaxNodeActionInNonGenerated<TSyntaxKind>(this ParameterLoadingAnalysisContext context,
                                                                               Action<SyntaxNodeAnalysisContext> action,
                                                                               params TSyntaxKind[] syntaxKinds) where TSyntaxKind : struct =>
            context.RegisterSyntaxNodeActionInNonGenerated(VisualBasicGeneratedCodeRecognizer.Instance, action, syntaxKinds);

        public static void RegisterSyntaxNodeActionInNonGenerated<TSyntaxKind>(this CompilationStartAnalysisContext context,
                                                                               Action<SyntaxNodeAnalysisContext> action,
                                                                               params TSyntaxKind[] syntaxKinds) where TSyntaxKind : struct =>
            context.RegisterSyntaxNodeActionInNonGenerated(VisualBasicGeneratedCodeRecognizer.Instance, action, syntaxKinds);

        public static void RegisterSyntaxTreeActionInNonGenerated(this SonarAnalysisContext context, Action<SyntaxTreeAnalysisContext> action) =>
            context.RegisterSyntaxTreeActionInNonGenerated(VisualBasicGeneratedCodeRecognizer.Instance, action);

        public static void RegisterSyntaxTreeActionInNonGenerated(this ParameterLoadingAnalysisContext context, Action<SyntaxTreeAnalysisContext> action) =>
            context.RegisterSyntaxTreeActionInNonGenerated(VisualBasicGeneratedCodeRecognizer.Instance, action);

        public static void RegisterCodeBlockStartActionInNonGenerated<TSyntaxKind>(this SonarAnalysisContext context, Action<CodeBlockStartAnalysisContext<TSyntaxKind>> action)
            where TSyntaxKind : struct =>
            context.RegisterCodeBlockStartActionInNonGenerated(VisualBasicGeneratedCodeRecognizer.Instance, action);

        public static void ReportDiagnosticIfNonGenerated(this SymbolAnalysisContext context, Diagnostic diagnostic) =>
            context.ReportDiagnosticIfNonGenerated(VisualBasicGeneratedCodeRecognizer.Instance, diagnostic);
    }
}
