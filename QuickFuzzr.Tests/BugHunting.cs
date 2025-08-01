namespace QuickFuzzr.Tests;

public class BugHunting
{
    [Fact]
    public void Is_It_The_Many_The_Choose_Or_The_Apply()
    {
        List<int> collector = [];
        var generator =
            from numbers in Fuzz.ChooseFromThese(1, 1, 1, 1, 1, 1, 1, 1).Many(1)
            from _ in Fuzz.For<SomeThingToGenerateWithACollection>()
                .Apply(a => numbers.ForEach(b => a.MyCollection.Add(1)))
            from result in Fuzz.One<SomeThingToGenerateWithACollection>()
            select result;

        var numberToGenerate = 2;
        var values = generator.Many(numberToGenerate).Generate().ToList();
        Assert.Equal(numberToGenerate, values.Count());

        for (int i = 0; i < numberToGenerate; i++)
        {
            var value = values[i];
            Assert.Equal(i + 1, value.MyCollection.Count);
        }

        foreach (var value in values)
            Assert.Single(value.MyCollection);
    }

    public class SomeThingToGenerateWithACollection
    {
        public List<int> MyCollection { get; set; } = [];
    }
}