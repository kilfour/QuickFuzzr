using QuickFuzzr.Tests;
using QuickFuzzr.Tests.Docs;
using QuickPulse.Explains;

Explain.These<Documentation>("Docs/");
Explain.This<Documentation>("Docs/all-in-one.md");
Explain.OnlyThis<ReadMe>("README.md");
