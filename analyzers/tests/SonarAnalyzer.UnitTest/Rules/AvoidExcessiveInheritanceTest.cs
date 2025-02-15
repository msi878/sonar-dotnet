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

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SonarAnalyzer.Rules.CSharp;
using SonarAnalyzer.UnitTest.TestFramework;

namespace SonarAnalyzer.UnitTest.Rules
{
    [TestClass]
    public class AvoidExcessiveInheritanceTest
    {
        [TestMethod]
        public void AvoidExcessiveInheritance_DefaultValues() =>
            OldVerifier.VerifyNonConcurrentAnalyzer(@"TestCases\AvoidExcessiveInheritance_DefaultValues.cs",
                new AvoidExcessiveInheritance());

#if NET
        [TestMethod]
        public void AvoidExcessiveInheritance_DefaultValues_Records() =>
            OldVerifier.VerifyAnalyzerFromCSharp9Library(@"TestCases\AvoidExcessiveInheritance_DefaultValues.Records.cs",
                new AvoidExcessiveInheritance());
#endif

        [TestMethod]
        public void AvoidExcessiveInheritance_CustomValuesFullyNamedFilteredClass() =>
            OldVerifier.VerifyNonConcurrentAnalyzer(@"TestCases\AvoidExcessiveInheritance_CustomValues.cs",
                new AvoidExcessiveInheritance { MaximumDepth = 2, FilteredClasses = "Tests.Diagnostics.SecondSubClass" });

        [TestMethod]
        public void AvoidExcessiveInheritance_CustomValuesWilcardFilteredClass() =>
            OldVerifier.VerifyNonConcurrentAnalyzer(@"TestCases\AvoidExcessiveInheritance_CustomValues.cs",
                new AvoidExcessiveInheritance { MaximumDepth = 2, FilteredClasses = "Tests.Diagnostics.*SubClass" });

#if NET
        [TestMethod]
        public void AvoidExcessiveInheritance_CustomValuesWilcardFilteredRecord() =>
            OldVerifier.VerifyAnalyzerFromCSharp9Library(@"TestCases\AvoidExcessiveInheritance_CustomValues.Records.cs",
                new AvoidExcessiveInheritance { MaximumDepth = 2, FilteredClasses = "Tests.Diagnostics.*SubRecord" });
#endif

        [TestMethod]
        public void FilteredClasses_ByDefault_ShouldBeEmpty() =>
            new AvoidExcessiveInheritance().FilteredClasses.Should().BeEmpty();
    }
}
