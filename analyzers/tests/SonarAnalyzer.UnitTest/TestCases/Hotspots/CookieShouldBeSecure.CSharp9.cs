﻿using System;
using Microsoft.AspNetCore.Http;
using Nancy.Cookies;

CookieOptions topLevelStatement1 = new();                // Noncompliant
CookieOptions topLevelStatement2 = new CookieOptions();  // Noncompliant
NancyCookie topLevelStatement3 = new ("name", "secure"); // Noncompliant

class Program
{
    CookieOptions field1 = new(); // Noncompliant
    CookieOptions field2;

    CookieOptions Property0 { get; init; } = new CookieOptions(); // Noncompliant
    CookieOptions Property1 { get; init; } = new (); // Noncompliant
    CookieOptions Property2 { get; init; }

    Program()
    {
        Property2.Secure = false; // Noncompliant
    }

    void InitializerSetsNotAllowedValue(DateTime? expires, string domain, string path)
    {
        CookieOptions c0 = new () { Secure = false };  // Noncompliant
        CookieOptions c1 = new () { HttpOnly = true }; // Noncompliant
        NancyCookie cookie2 = new ("name", "secure") { Expires = expires, Domain = domain, Path = path };  // Noncompliant
    }
}
