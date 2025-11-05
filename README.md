# <img src='icon.png' width='40' align='top'/> QuickFuzzr

> **A type-walking cheetah with a hand full of random.**

Generate realistic test data and *fuzz* your domain models using composable LINQ expressions.

[![Docs](https://img.shields.io/badge/docs-QuickFuzzr-blue?style=flat-square&logo=readthedocs)](https://github.com/kilfour/QuickFuzzr/blob/main/Docs/ToC.md)
[![NuGet](https://img.shields.io/nuget/v/QuickFuzzr.svg?style=flat-square&logo=nuget)](https://www.nuget.org/packages/QuickFuzzr)
[![License: MIT](https://img.shields.io/badge/license-MIT-success?style=flat-square)](https://github.com/kilfour/QuickFuzzr/blob/main/LICENSE)  
## Example
```csharp
var fuzzr =
    from decimalPrecision in Fuzzr.Decimal().Apply(d => Math.Round(d, 2)).Replace()
    from name in Fuzzr.OneOf("John", "Paul", "George", "Ringo")
    let email = $"{name.ToLower()}@mail.com"
    from customer in Fuzzr.One(() => new Customer(name, email))
    from orders in Fuzzr.One<Order>()
        .Apply(customer.PlaceOrder)
        .Many(1, 4)
    from payment in Fuzzr.One<Payment>()
        .Apply(p => p.Amount = orders.Sum(o => o.Total))
        .Apply(customer.MakePayment)
    select (customer, orders, payment);
fuzzr.Generate();
// Results in =>
// (
//     Customer {
//         Name: "Paul",
//         Email: "paul@mail.com",
//         Orders: [ Order { Total: 67.25 }, Order { Total: 23.41 } ],
//         Payments: [ Payment { Amount: 90.66 } ]
//     },
//     [ Order { Total: 67.25 }, Order { Total: 23.41 } ],
//     Payment {
//         Amount: 90.66
//     }
// )
```
## Highlights

* **Zero-config generation:** `Fuzzr.One<T>()` works out of the box.
* **LINQ-composable:** Build complex generators from simple parts.
* **Property-based testing ready:** Great for fuzzing and edge case discovery.  
* **Configurable defaults:** Fine-tune generation with `Configr`.
* **Recursive object graphs:** Automatic depth-controlled nesting.
* **Seed-based reproducibility:** Deterministic generation for reliable tests.
* **Real-world domain ready:** Handles aggregates, value objects, and complex relationships.  
## Installation

QuickFuzzr is available on NuGet:
```bash
Install-Package QuickFuzzr
```
Or via the .NET CLI:
```bash
dotnet add package QuickFuzzr
```  
## Documentation

QuickFuzzr is fully documented, with real, executable examples for each combinator and concept.

You can explore it here:

* **[Table of Contents](https://github.com/kilfour/QuickFuzzr/blob/main/Docs/ToC.md)**
* **[One File Doc](https://github.com/kilfour/QuickFuzzr/blob/main/Docs/all-in-one.md)**

*Note:* A full API reference is currently being worked on.  
For now all public methods have xml summary comments.

  
## Roadmap

- Complete API reference documentation.
- More Elm-style error messages.
- More Primitives: DateTimeOffset, NInt, NUint, ...
- QuickFuzzr.Reactor: Cookbook, reusable patterns.
- QuickFuzzr.Evil: For all your property based testing needs.
  
## License

This project is licensed under the [MIT License](https://github.com/kilfour/QuickFuzzr/blob/main/LICENSE).  
