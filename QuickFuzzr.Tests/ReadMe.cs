using QuickFuzzr;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests;

[DocFile]
[DocFileHeader("<img src='icon.png' width='40' align='top'/> QuickFuzzr")]
[DocContent(
@"
> **A type-walking cheetah with a hand full of random.**

A tiny library for building stateful, inspectable, composable flows.

[![Docs](https://img.shields.io/badge/docs-QuickFuzzr-blue?style=flat-square&logo=readthedocs)](https://github.com/kilfour/QuickFuzzr/blob/main/aal-in-one.md)
[![NuGet](https://img.shields.io/nuget/v/QuickFuzzr.svg?style=flat-square&logo=nuget)](https://www.nuget.org/packages/QuickFuzzr)
[![License: MIT](https://img.shields.io/badge/license-MIT-success?style=flat-square)](https://github.com/kilfour/QuickFuzzr/blob/main/LICENSE)")]
public class ReadMe
{

    [DocHeader("Highlights")]
    [DocContent(@"
* ")]
    private static void Highlights() { }

    [DocHeader("Installation")]
    [DocContent(@"
QuickFuzzr is available on NuGet:
```bash
Install-Package QuickFuzzr
```
Or via the .NET CLI:
```bash
dotnet add package QuickFuzzr
```")]
    private static void Installation() { }

    [DocHeader("Documentation")]
    [DocContent(@"
QuickFuzzr is fully documented, with real, executable examples for each combinator and concept.

You can explore it here:

* **[All The Doc](https://github.com/kilfour/QuickFuzzr/blob/main/all-in-one.md)**
")]
    private static void Documentation() { }

    [DocHeader("License")]
    [DocContent(@"
This project is licensed under the [MIT License](https://github.com/kilfour/QuickFuzzr/blob/main/LICENSE).")]
    private static void License() { }
}