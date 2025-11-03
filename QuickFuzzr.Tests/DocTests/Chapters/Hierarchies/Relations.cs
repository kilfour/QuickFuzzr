using QuickPulse.Explains;

namespace QuickFuzzr.Tests.DocTests.Chapters.Hierarchies;

[DocContent("In the same way one can `Customize` primitives, this can also be done for references.")]
public class Relations
{

	[Fact]
	[DocContent(
@"E.g. :

```
var generator =
	from product in Fuzzr.One<ProductItem>()
	from setProduct in Fuzzr.For<OrderLine>().Customize(orderline => orderline.Product, product)
	from orderline in Fuzzr.One<OrderLine>()
	select orderline;
```
")]
	public void SetOneToOne()
	{
		var generator =
			from product in Fuzzr.One<ProductItem>()
			from setProduct in Configr<OrderLine>.Property(order => order.Product, product)
			from orderline in Fuzzr.One<OrderLine>()
			select orderline;

		var value = generator.Generate();
		Assert.NotNull(value.Product);
	}

	[Fact]
	[DocContent(
@"In case of a one-to-many relation where the collection is inaccessible, but a method is provided for adding the many to the one,
we can use the `Apply` method, which is explained in detail in the chapter 'Other Useful Generators'.
E.g. :

```
var generator =
	from order in Fuzzr.One<Order>()
	from lines in Fuzzr.One<OrderLine>()
		.Apply(a => order.AddOrderLine(a)).Many(20)
	select order;
```
")]
	public void OneToMany()
	{
		var generator =
			from order in Fuzzr.One<Order>()
			from lines in Fuzzr.One<OrderLine>().Apply(order.AddOrderLine).Many(2)
			select order;

		var value = generator.Generate();
		Assert.Equal(2, value.OrderLines.Count());
	}


	[Fact]
	[DocContent(
@"Or select the lines instead of the order.")]
	public void OneToManyVerifying()
	{
		var generator =
			from lines in Fuzzr.One<OrderLine>().Many(2)
			from order in Fuzzr.One<Order>().Apply(a => lines.ToList().ForEach(b => a.AddOrderLine(b)))
			select lines;

		var value = generator.Generate().ToArray();
		Assert.NotNull(value[0].MyOrder);
		Assert.NotNull(value[1].MyOrder);
		Assert.Equal(value[0].MyOrder, value[1].MyOrder);
	}


	[Fact]
	[DocContent(
@"Relations defined by constructor injection can be generated using the `One<T>(Func<T> constructor)` overload.
E.g. :

```csharp
var generator =
	from category in Fuzzr.One<Category>()
	from subCategory in Fuzzr.One(() => new SubCategory(category)).Many(20)
	select category;
```
")]
	public void ThroughConstructor()
	{
		var generator =
			from category in Fuzzr.One<Category>()
			from subCategory in Fuzzr.One(() => new SubCategory(category)).Many(2)
			select category;

		var value = generator.Generate();
		Assert.Equal(2, value.SubCategories.Count());
	}

	public class Order
	{
		public Order()
		{
			OrderLines = new List<OrderLine>();
		}
		public List<OrderLine> OrderLines { get; set; }
		public void AddOrderLine(OrderLine line)
		{
			line.MyOrder = this;
			OrderLines.Add(line);
		}
	}

	public class OrderLine
	{
		public Order? MyOrder { get; set; }
		public ProductItem? Product { get; set; }
	}

	public class ProductItem { }

	public class Category
	{
		public Category()
		{
			SubCategories = new List<SubCategory>();
		}
		public List<SubCategory> SubCategories { get; set; }
	}

	public class SubCategory
	{
		public SubCategory(Category category)
		{
			MyCategory = category;
			category.SubCategories.Add(this);
		}
		public Category MyCategory { get; set; }
	}
}