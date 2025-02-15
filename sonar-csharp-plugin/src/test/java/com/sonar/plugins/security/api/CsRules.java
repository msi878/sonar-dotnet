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
package com.sonar.plugins.security.api;

import java.util.HashSet;
import java.util.Set;

public class CsRules {
  public static Set<String> ruleKeys = new HashSet<>();
  public static Exception exceptionToThrow;
  public static boolean returnRepository;

  public static Set<String> getRuleKeys() throws Exception {
    if (exceptionToThrow != null) {
      Exception exception = exceptionToThrow;
      // cleanup for the next execution
      exceptionToThrow = null;
      throw exception;
    }
    return ruleKeys;
  }

  public static String getRepositoryKey() throws Exception {
    if (returnRepository) {
      return "roslyn.TEST";
    } else {
      throw exceptionToThrow;
    }
  }
}
