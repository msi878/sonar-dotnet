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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SonarAnalyzer.Rules.CSharp;
using SonarAnalyzer.UnitTest.TestFramework;

namespace SonarAnalyzer.UnitTest.Rules
{
    [TestClass]
    public class UnusedReturnValueTest
    {
        [TestMethod]
        public void UnusedReturnValue() =>
            OldVerifier.VerifyAnalyzer(
                @"TestCases\UnusedReturnValue.cs",
                new UnusedReturnValue(),
                ParseOptionsHelper.FromCSharp8);

        [TestMethod]
        public void UnusedReturnValueWithPartialClasses() =>
            OldVerifier.VerifyAnalyzer(
                new[] { @"TestCases\UnusedReturnValue.part1.cs", @"TestCases\UnusedReturnValue.part2.cs", @"TestCases\UnusedReturnValue.External.cs" },
                new UnusedReturnValue(),
                ParseOptionsHelper.FromCSharp8);

#if NET
        [TestMethod]
        public void UnusedReturnValue_CSharp9() =>
            OldVerifier.VerifyAnalyzerFromCSharp9Console(@"TestCases\UnusedReturnValue.CSharp9.cs", new UnusedReturnValue());

        [TestMethod]
        public void UnusedReturnValue_CSharp10() =>
            OldVerifier.VerifyAnalyzerFromCSharp10Console(@"TestCases\UnusedReturnValue.CSharp10.cs", new UnusedReturnValue());

        [TestMethod]
        public void UnusedReturnValue_CSharpPreview() =>
            OldVerifier.VerifyAnalyzerCSharpPreviewLibrary(@"TestCases\UnusedReturnValue.CSharpPreview.cs", new UnusedReturnValue());
#endif
    }
}
