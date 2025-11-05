using QuickFuzzr.Tests;
using QuickFuzzr.Tests.Docs;
using QuickFuzzr.Tests.Reference;
using QuickPulse.Explains;

Explain.These<Documentation>("Docs/");
Explain.This<Documentation>("doc.md");
Explain.These<Reference>("APIReference/");
Explain.OnlyThis<ReadMe>("README.md");
