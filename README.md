# <img src='icon.png' width='40' align='top'/> QuickFuzzr

> **A type-walking cheetah with a hand full of random.**

Generate realistic test data and *fuzz* your domain models using composable LINQ expressions.

[![Docs](https://img.shields.io/badge/docs-QuickFuzzr-blue?style=flat-square&logo=readthedocs)](https://github.com/kilfour/QuickFuzzr/blob/main/Docs/ToC.md)
[![NuGet](https://img.shields.io/nuget/v/QuickFuzzr.svg?style=flat-square&logo=nuget)](https://www.nuget.org/packages/QuickFuzzr)
[![License: MIT](https://img.shields.io/badge/license-MIT-success?style=flat-square)](https://github.com/kilfour/QuickFuzzr/blob/main/LICENSE)  
## Examples
### It Just Works
```csharp
Fuzzr.One<Person>().Generate();
// Results in => Person { Name = "ddnegsn", Age = 18 }
```
### Configurable
```csharp
var fuzzr =
    // Generate complete customer with orders and payments
    from counter in Fuzzr.Counter("my-key") // <= keyed auto incrementing int
    from customer in Fuzzr.One(() => new Customer($"Customer-{counter}"))
    from orders in Fuzzr.One<Order>()
        .Apply(customer.PlaceOrder) // <= add order to customer
        .Many(1, 4) // <= add between 1 and 4 random orders
    from payment in Fuzzr.One<Payment>()
        .Apply(p => p.Amount = orders.Sum(o => o.Total)) // <= calculate total from orders
        .Apply(customer.MakePayment) // <= add payment to customer
    select customer;
fuzzr.Many(2).Generate();
```
**Output:**  
```
[
    Customer {
        Name: "Customer-1",
        Orders: [ Order { Total: 42.73 }, Order { Total: 67.25 } ],
        Payments: [ Payment { Amount: 109.98 } ]
    },
    Customer {
        Name: "Customer-2",
        Orders: [ Order { Total: 10.51 }, Order { Total: 14.66 }, Order { Total: 60.86 } ],
        Payments: [ Payment { Amount: 86.03 } ]
    }
]
```
## Highlights

* **Zero-config generation:** `Fuzzr.One<T>()` works out of the box.
* **LINQ-composable:** Build complex generators from simple parts.
* **Property-based testing ready:** Great for fuzzing and edge case discovery.  
* **Configurable defaults:** Fine-tune generation with `Configr`.
* **Recursive object graphs:** Automatic depth-controlled nesting.
* **Seed-based reproducibility:** Deterministic generation for reliable tests.
* **Handles real-world domains:** Aggregates, value objects, and complex relationships.  
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
* **[One File Doc](https://github.com/kilfour/QuickFuzzr/blob/main/doc.md)**

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
## Addendum
The How and Why of QuickFuzzr: [From Kitten to Cheetah](https://github.com/kilfour/QuickFuzzr/blob/main/from-kitten-to-cheetah.md).  
