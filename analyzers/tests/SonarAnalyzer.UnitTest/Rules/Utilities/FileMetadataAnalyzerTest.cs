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
using FluentAssertions;
using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SonarAnalyzer.Helpers;
using SonarAnalyzer.Protobuf;
using SonarAnalyzer.Rules.CSharp;
using SonarAnalyzer.UnitTest.TestFramework;

namespace SonarAnalyzer.UnitTest.Rules
{
    [TestClass]
    public class FileMetadataAnalyzerTest
    {
        private const string Root = @"TestCases\Utilities\FileMetadataAnalyzer\";

        [DataTestMethod]
        [DataRow(ProjectType.Product)]
        [DataRow(ProjectType.Test)]
        public void Autogenerated(ProjectType projectType)
        {
            var autogeneratedFiles = new[]
            {
                @"ExtraEmptyFile.g.cs", // This is added by Verifier, we will skip it when building the project
                @"autogenerated_comment.cs",
                @"autogenerated_comment2.cs",
                @"class.designer.cs",
                @"class.g.cs",
                @"class.g.something.cs",
                @"class.generated.cs",
                @"class_generated.cs",
                @"compiler_generated.cs",
                @"compiler_generated_attr.cs",
                @"debugger_non_user_code.cs",
                @"debugger_non_user_code_attr.cs",
                @"generated_code_attr.cs",
                @"generated_code_attr2.cs",
                @"TEMPORARYGENERATEDFILE_class.cs",
            };

            var projectFiles = autogeneratedFiles
                .Skip(1) // "ExtraEmptyFile.g.cs" is automatically added by Verifier
                .Select(x => Root + x);

            VerifyAllFilesAreGenerated(nameof(Autogenerated), projectFiles, autogeneratedFiles, projectType);
        }

        [DataTestMethod]
        [DataRow(ProjectType.Product)]
        [DataRow(ProjectType.Test)]
        public void Autogenerated_CSharp9(ProjectType projectType)
        {
            var autogeneratedFiles = new[]
            {
                @"ExtraEmptyFile.g.cs", // This is added by Verifier, we will skip it when building the project
                @"generated_code_attr_local_function.cs"
            };

            var projectFiles = autogeneratedFiles
                .Skip(1) // "ExtraEmptyFile.g.cs" is automatically added by Verifier
                .Select(x => Root + x);

            VerifyAllFilesAreGenerated(nameof(Autogenerated_CSharp9), projectFiles, autogeneratedFiles, projectType, ParseOptionsHelper.FromCSharp9);
        }

        [DataTestMethod]
        [DataRow(ProjectType.Product)]
        [DataRow(ProjectType.Test)]
        public void NotAutogenerated(ProjectType projectType)
        {
            var notAutogeneratedFiles = new[]
            {
                @"normal_file.cs",
                @"generated_region.cs",
                @"generated_region_2.cs"
            };

            var projectFiles = notAutogeneratedFiles.Select(x => Root + x);
            var testRoot = Root + nameof(NotAutogenerated);

            OldVerifier.VerifyNonConcurrentUtilityAnalyzer<FileMetadataInfo>(
                projectFiles,
                new TestFileMetadataAnalyzer(testRoot, projectType == ProjectType.Test),
                @$"{testRoot}\file-metadata.pb",
                TestHelper.CreateSonarProjectConfig(testRoot, projectType),
                (messages) =>
                {
                    messages[0].IsGenerated.Should().BeTrue();
                    messages[0].FilePath.Should().Be("ExtraEmptyFile.g.cs");

                    for (var i = 0; i < notAutogeneratedFiles.Length; i++)
                    {
                        var message = messages[i + 1]; // The first message is for ExtraEmptyFile.g.cs, then is our list
                        message.IsGenerated.Should().BeFalse();
                        message.FilePath.Should().Be(notAutogeneratedFiles[i]);
                    }
                });
        }

        [DataTestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void CreateMessage_NoEncoding_SetsEmptyString(bool isTestProject)
        {
            var tree = new Mock<SyntaxTree>();
            tree.SetupGet(x => x.FilePath).Returns("File.Generated.cs");    // Generated to simplify mocking for GeneratedCodeRecognizer
            tree.SetupGet(x => x.Encoding).Returns(() => null);
            var sut = new TestFileMetadataAnalyzer(null, isTestProject);

            sut.TestCreateMessage(tree.Object, null).Encoding.Should().BeEmpty();
        }

        private static void VerifyAllFilesAreGenerated(string testName,
                                                       IEnumerable<string> projectFiles,
                                                       string[] autogeneratedFiles,
                                                       ProjectType projectType,
                                                       ImmutableArray<ParseOptions> parseOptions = default)
        {
            var testRoot = Root + testName;
            OldVerifier.VerifyNonConcurrentUtilityAnalyzer<FileMetadataInfo>(
                projectFiles,
                new TestFileMetadataAnalyzer(testRoot, projectType == ProjectType.Test),
                @$"{testRoot}\file-metadata.pb",
                TestHelper.CreateSonarProjectConfig(testRoot, projectType),
                (messages) =>
                {
                    for (var i = 0; i < autogeneratedFiles.Length; i++)
                    {
                        var message = messages[i];
                        message.IsGenerated.Should().BeTrue();
                        message.FilePath.Should().Be(autogeneratedFiles[i]);
                    }
                },
                parseOptions);
        }

        // We need to set protected properties and this class exists just to enable the analyzer without bothering with additional files with parameters
        private class TestFileMetadataAnalyzer : FileMetadataAnalyzer
        {
            public TestFileMetadataAnalyzer(string outPath, bool isTestProject)
            {
                IsAnalyzerEnabled = true;
                OutPath = outPath;
                IsTestProject = isTestProject;
            }

            public FileMetadataInfo TestCreateMessage(SyntaxTree syntaxTree, SemanticModel semanticModel) =>
                CreateMessage(syntaxTree, semanticModel);
        }
    }
}
