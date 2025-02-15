/*
 * SonarC#
 * Copyright (C) 2014-2022 SonarSource SA
 * mailto:info AT sonarsource DOT com
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
package org.sonar.plugins.csharp;

import org.junit.Test;
import org.sonar.api.rules.RuleType;
import org.sonar.api.server.debt.DebtRemediationFunction;
import org.sonar.api.server.rule.RulesDefinition;
import org.sonar.api.server.rule.RulesDefinition.Context;
import org.sonar.api.server.rule.RulesDefinition.Rule;

import static org.assertj.core.api.Assertions.assertThat;

public class CSharpSonarRulesDefinitionTest {
  private static final String SECURITY_HOTSPOT_RULE_KEY = "S5766";
  private static final String VULNERABILITY_RULE_KEY = "S4426";

  @Test
  public void test() {
    Context context = new Context();
    assertThat(context.repositories()).isEmpty();

    CSharpSonarRulesDefinition csharpRulesDefinition = new CSharpSonarRulesDefinition();
    csharpRulesDefinition.define(context);

    assertThat(context.repositories()).hasSize(1);
    assertThat(context.repository("csharpsquid").rules()).isNotEmpty();

    Rule s100 = context.repository("csharpsquid").rule("S100");
    assertThat(s100.debtRemediationFunction().type()).isEqualTo(DebtRemediationFunction.Type.CONSTANT_ISSUE);
    assertThat(s100.debtRemediationFunction().baseEffort()).isEqualTo("5min");
    assertThat(s100.type()).isEqualTo(RuleType.CODE_SMELL);
  }

  @Test
  public void test_security_hotspot() {
    CSharpSonarRulesDefinition definition = new CSharpSonarRulesDefinition();
    RulesDefinition.Context context = new RulesDefinition.Context();
    definition.define(context);
    RulesDefinition.Repository repository = context.repository("csharpsquid");

    RulesDefinition.Rule hardcodedCredentialsRule = repository.rule(SECURITY_HOTSPOT_RULE_KEY);
    assertThat(hardcodedCredentialsRule.type()).isEqualTo(RuleType.SECURITY_HOTSPOT);
    assertThat(hardcodedCredentialsRule.activatedByDefault()).isFalse();
  }

  @Test
  public void test_security_hotspot_has_correct_type_and_security_standards() {
    CSharpSonarRulesDefinition definition = new CSharpSonarRulesDefinition();
    RulesDefinition.Context context = new RulesDefinition.Context();
    definition.define(context);
    RulesDefinition.Repository repository = context.repository("csharpsquid");

    RulesDefinition.Rule rule = repository.rule(SECURITY_HOTSPOT_RULE_KEY);
    assertThat(rule.type()).isEqualTo(RuleType.SECURITY_HOTSPOT);
    assertThat(rule.securityStandards()).containsExactlyInAnyOrder("cwe:502", "owaspTop10:a8");
  }

  @Test
  public void test_security_standards_with_vulnerability() {
    CSharpSonarRulesDefinition definition = new CSharpSonarRulesDefinition();
    RulesDefinition.Context context = new RulesDefinition.Context();
    definition.define(context);
    RulesDefinition.Repository repository = context.repository("csharpsquid");

    RulesDefinition.Rule rule = repository.rule(VULNERABILITY_RULE_KEY);
    assertThat(rule.type()).isEqualTo(RuleType.VULNERABILITY);
    assertThat(rule.securityStandards()).containsExactlyInAnyOrder("cwe:326", "owaspTop10:a3", "owaspTop10:a6");
  }

  @Test
  public void test_all_rules_have_status_set(){
    CSharpSonarRulesDefinition definition = new CSharpSonarRulesDefinition();
    RulesDefinition.Context context = new RulesDefinition.Context();
    definition.define(context);
    RulesDefinition.Repository repository = context.repository("csharpsquid");

    for (RulesDefinition.Rule rule:repository.rules()) {
      assertThat(rule.status()).isNotNull();
    }
  }
}
