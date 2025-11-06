# From Kitten to Cheetah
> A.k.a. Why QuickFuzzr exists and how it grew into what it is now.

This one goes way back. So my memory might have some gaps, ..., but I reckon it started somewhere around 2007 or 2008.

I was doing a lot of NHibernate stuff and wanted a minimal way to write persistancy tests.  
People were starting to play with random data generation, but there wasn't much around yet in terms of libraries.  

I think NBuilder was pretty much the go-to.  
For some reason or another, it did not suit my needs, so I rolled out my own : **QuickGenerate**.  

Fluent interfaces had just become the hype (I guess they still are), so it ended up looking something like this:
```csharp
new DomainGenerator()
    .With<Category>(g => g.For(category => category.Id, 0, val => val + 1))
    .With<Category>(g => g.For(category => category.Title, new StringGenerator(5, 10, 'H', 'a', 'l')))
    .With<Category>(g => g.For(category => category.Description,
        new StringBuilderGenerator()
            .Append("Not many", "Lots of", "Some")
            .Space()
            .Append("big", "small", "dangerous", "volatile", "futile")
            .Space()
            .Append("paintbrushes", "camels", "radio antennas")
            .Period(),
        new NumericStringGenerator(4, 12)))
    .OneToMany<Category, Product>(3, 7, (c, p) => c.Products.Add(p))
    .With<Product>(g => g.For(product => product.Id, 0, val => val + 1))
    .With<Product>(g => g.For(product => product.Title, "title1", "title2", "Another title"))
    .With<Product>(g => g.For(product => product.Price, val => Math.Round(val, 2)))
    .With<Product>(g => g.For(product => product.Created, 31.December(2009), val => val.AddDays(1)))
    .Many<Category>(4, 8);
```
It held its own in real-world projects and was even used by a few other developers.

But it had issues.  
The fluent interface just wasn't composable enough, and every new feature meant adding yet another method to the API surface.  
That made it brittle, and occasionally even wrong in behavior.

Then the community shifted away from random data toward more deterministic unit tests. 
A good idea I now think.

Meanwhile I got into property-based testing (PBT) and then the old library really started to crack.  

According to GitHub on **March 5th, 2014**, I made the following commit to a new project called **QuickMGenerate**:
> *initial spike*
```csharp
var x =
    from a in MGen.Int()
    from b in MGen.String()
    from c in MGen.Int()
    select a + b + c;
Console.WriteLine(x.Generate());
```
By then I was doing a lot of functional programming, that's what led me to PBT in the first place,
so the switch from *fluent chaining* to *LINQ composition* was inevitable.  

By **March 9th**, I basically had the foundation of what is now QuickFuzzr.
Between that date and early 2025, there were only five commits.
Not because it was abandoned, I used it a lot, but because it was good enough.

So what changed at the start of this year?

After five years writing in other languages, I got a gig teaching C#, and to brush up I decided to modernize my old testing libraries.
By then, my property-based engine had matured: shrinking worked, reporting was decent, ...

That's when I realized the new old lib (QuickMGenerate) wasn't enough anymore.
Commits started flying in, the design was cleaned up, the API refined, ... and yes, the name had to go.

Hence:  
**QuickFuzzr**: a type-walking cheetah with a hand full of random.

