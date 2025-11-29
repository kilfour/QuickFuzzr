using QuickFuzzr.Tests;
using QuickFuzzr.Tests.Docs;
using QuickFuzzr.Tests.Docs.A_Guide;
using QuickFuzzr.Tests.Docs.B_Reference;
using QuickFuzzr.Tests.Docs.C_Cookbook;
using QuickFuzzr.Tests.Docs.D_Evil;
using QuickPulse.Explains;

Explain.These<Documentation>("Docs");

Explain.This<Documentation>("full-doc.md");
Explain.This<Guide>("guide.md");
Explain.This<Reference>("reference.md");
Explain.This<Cookbook>("cookbook.md");
//Explain.This<QuickFuzzrEvil>("quickfuzzr-evil.md");

Explain.OnlyThis<CreateReadMe>("README.md");
