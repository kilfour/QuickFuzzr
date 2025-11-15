using QuickFuzzr.Tests._Tools;
using QuickFuzzr.Tests._Tools.Models;
using QuickPulse.Arteries;
using QuickPulse.Explains;
using QuickPulse.Show;

namespace QuickFuzzr.Tests;

[DocFile]
[DocFileHeader("<img src='icon.png' width='40' align='top'/> QuickFuzzr")]
[DocContent(
@"
> **A type-walking cheetah with a hand full of random.**

Generate realistic test data and *fuzz* your domain models using composable LINQ expressions.

[![Docs](https://img.shields.io/badge/docs-QuickFuzzr-blue?style=flat-square&logo=readthedocs)](https://github.com/kilfour/QuickFuzzr/blob/main/Docs/ToC.md)
[![NuGet](https://img.shields.io/nuget/v/QuickFuzzr.svg?style=flat-square&logo=nuget)](https://www.nuget.org/packages/QuickFuzzr)
[![License: MIT](https://img.shields.io/badge/license-MIT-success?style=flat-square)](https://github.com/kilfour/QuickFuzzr/blob/main/LICENSE)")]
public class ReadMe
{
    private readonly bool ToFile = false;
    [Fact]
    [DocHeader("Examples")]
    [DocHeader("It Just Works", 1)]
    [DocExample(typeof(ReadMe), nameof(Example_Simple_Fuzzr))]
    public void Example_Simple()
    {
        var result = Example_Simple_Fuzzr();
        Assert.Equal("ddnegsn", result.Name);
        Assert.Equal(18, result.Age);
    }

    [CodeSnippet]
    [CodeRemove("return ")]
    [CodeRemove("42")]
    private static Person Example_Simple_Fuzzr()
    {
        return Fuzzr.One<Person>().Generate(42);
        // Results in => Person { Name = "ddnegsn", Age = 18 }
    }

    [Fact]
    [DocHeader("Configurable", 1)]
    [DocExample(typeof(ReadMe), nameof(Example_Configr_Fuzzr))]
    [DocOutput]
    [DocCodeFile("readme-example-output.txt")]
    public void Example_Configr()
    {
        var result = Example_Configr_Fuzzr().ToList();
        if (ToFile)
            FileLog.Write("./QuickFuzzr.Tests/readme-example-output.txt").Absorb(
                Please.AllowMe()
                    .ToAddSomeClass()
                    .ToInline<List<Order>>()
                    .ToInline<List<Payment>>()
                    .IntroduceThis(result));
        Assert.Equal(2, result.Count);

        Assert.Equal("Customer-1", result[0].Name);
        Assert.Equal(2, result[0].Orders.Count);
        Assert.Equal(42.72M, result[0].Orders[0].Total);
        Assert.Equal(67.24M, result[0].Orders[1].Total);
        Assert.Single(result[0].Payments);
        Assert.Equal(109.96M, result[0].Payments[0].Amount);

        Assert.Equal("Customer-2", result[1].Name);
        Assert.Equal(3, result[1].Orders.Count);
        Assert.Equal(10.50M, result[1].Orders[0].Total);
        Assert.Equal(14.66M, result[1].Orders[1].Total);
        Assert.Equal(60.86M, result[1].Orders[2].Total);
        Assert.Single(result[1].Payments);
        Assert.Equal(86.02M, result[1].Payments[0].Amount);
    }

    [CodeSnippet]
    [CodeRemove("return ")]
    [CodeRemove("987")]
    private static IEnumerable<Customer> Example_Configr_Fuzzr()
    {
        var fuzzr =
            // Generate complete customer with orders and payments
            from counter in Fuzzr.Counter("my-key") // <= keyed auto incrementing int
            from customer in Fuzzr.One(() => new Customer($"Customer-{counter}"))
            from orders in Fuzzr.One<Order>()
                .Apply(customer.PlaceOrder) // <= add order to customer
                .Many(1, 3) // <= add between 1 and 3 random orders
            from payment in Fuzzr.One<Payment>()
                .Apply(p => p.Amount = orders.Sum(o => o.Total)) // <= calculate total from orders
                .Apply(customer.MakePayment) // <= add payment to customer
            select customer;
        return fuzzr.Many(2).Generate(987);
    }

    public class Customer(string name)
    {
        public string Name { get; init; } = name;
        public List<Order> Orders { get; set; } = [];
        public List<Payment> Payments { get; set; } = [];
        public void MakePayment(Payment payment) => Payments.Add(payment);
        public void PlaceOrder(Order order) => Orders.Add(order);
    }

    public class Order { public decimal Total { get; set; } }

    public class Payment { public decimal Amount { get; set; } }

    [DocHeader("Highlights")]
    [DocContent(@"
* **Zero-config generation:** `Fuzzr.One<T>()` works out of the box.
* **LINQ-composable:** Build complex fuzzrs from simple parts.
* **Configurable defaults:** Fine-tune generation with `Configr`.
* **Recursive object graphs:** Automatic depth-controlled nesting.
* **Seed-based reproducibility:** Deterministic results for reliable tests.
* **Built for Devs:** Clean LINQ workflows and Elm-style errors that guide you to the fix.
")]
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
QuickFuzzr is fully documented, with real, executable examples for every feature,
and every statement in the docs is backed by a test.

You can explore it here:
* **[Table of Contents](https://github.com/kilfour/QuickFuzzr/blob/main/Docs/ToC.md)**
* **[One File Guide](https://github.com/kilfour/QuickFuzzr/blob/main/guide.md)**
* **[One File Reference](https://github.com/kilfour/QuickFuzzr/blob/main/reference.md)**
")]
    private static void Documentation() { }

    [DocHeader("Roadmap")]
    [DocContent(@"
* Add a 'When Things Go Wrong' chapter to reference.
* Benchmarks: Only believe the *measured* hype.
* Add the missing Primitives: DateTimeOffset, NInt, NUint, ...
* Expand the [Cookbook](https://github.com/kilfour/QuickFuzzr/blob/main/cookbook.md) with more real-world recipes
* QuickFuzzr.Reactor: Convention-based generation and reusable patterns.
* QuickFuzzr.Evil: For all your property-based testing needs.
")]
    private static void Roadmap() { }

    [DocHeader("License")]
    [DocContent(@"
This project is licensed under the [MIT License](https://github.com/kilfour/QuickFuzzr/blob/main/LICENSE).")]
    private static void License() { }

    [DocHeader("Addendum")]
    [DocContent("The How and Why of QuickFuzzr: [From Kitten to Cheetah](https://github.com/kilfour/QuickFuzzr/blob/main/from-kitten-to-cheetah.md).")]
    private static void Origins() { }
}