﻿/*
 * SonarAnalyzer for .NET
 * Copyright (C) 2015-2020 SonarSource SA
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

extern alias csharp;
using csharp::SonarAnalyzer.Rules.CSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SonarAnalyzer.UnitTest.TestFramework;

namespace SonarAnalyzer.UnitTest.Rules
{
    [TestClass]
    public class DisposableTypesNeedFinalizersTest
    {
        [TestMethod]
        [TestCategory("Rule")]
        public void DisposableTypesNeedFinalizers() =>
            Verifier.VerifyAnalyzer(@"TestCases\DisposableTypesNeedFinalizers.cs", new DisposableTypesNeedFinalizers());

#if NET
        [TestMethod]
        [TestCategory("Rule")]
        public void DisposableTypesNeedFinalizers_CSharp9() =>
            Verifier.VerifyAnalyzerFromCSharp9Library(@"TestCases\DisposableTypesNeedFinalizers.CSharp9.cs", new DisposableTypesNeedFinalizers());
#endif

        [TestMethod]
        [TestCategory("Rule")]
        public void DisposableTypesNeedFinalizers_InvalidCode() =>
            Verifier.VerifyCSharpAnalyzer(@"
public class Foo_05 : IDisposable
{
    private HandleRef;
}", new DisposableTypesNeedFinalizers(), checkMode: CompilationErrorBehavior.Ignore);
    }
}
