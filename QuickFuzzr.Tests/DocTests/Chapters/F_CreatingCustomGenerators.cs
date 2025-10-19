using QuickFuzzr.UnderTheHood;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.DocTests.Chapters;

[DocFile]
[DocContent(@"Any function that returns a value of type `Generator<T>` can be used as a generator.

`Generator<T>` is defined as a delegate like so :
```csharp
public delegate IResult<TValue> Generator<out TValue>(State input)
```")]
public class F_CreatingCustomGenerators
{
    [Fact]
    [DocContent(@"So f.i. to define a generator that always returns the number forty-two we need a function that returns the following :
```csharp
return s => new Result<State, int>(42, s);
```")]
    public void CustomGeneratorExample()
    {
        Assert.Equal(42, Generate42().Generate());
    }

    private Generator<int> Generate42()
    {
        return s => new Result<int>(42, s);
    }

    [Fact]
    [DocContent(@"As you can see from the signature a state object is passed to the generator.
This is where the random seed lives.
If you want any kind of random, it is advised to use that one, like so :
```csharp
return s => new Result<State, int>(s.Random.Next(42, 666), s);
```

")]//See also : [Creating a counter generator](./QuickFuzzr.Tests/CreatingCustomGenerators/CreatingACounterGeneratorExample.cs).
    public void CustomGeneratorExampleWithRandom()
    {
        Assert.Equal(42, Generate42OtherWay().Generate());
    }

    public Generator<int> Generate42OtherWay()
    {
        return s => new Result<int>(s.Random.Next(42, 42), s);
    }

    [Fact]
    public void CreatingACounterGeneratorExample()
    {
        var generator =
            from s in Fuzzr.Constant("SomeString")
            from c in Counter()
            from defaultString in Fuzzr.Constant(s + c).Replace()
            from thing in Fuzzr.One<SomethingToGenerate>()
            select thing;

        var values = generator.Many(6).Generate().ToArray();

        Assert.Equal("SomeString1", values[0].MyProperty);
        Assert.Equal("SomeString2", values[1].MyProperty);
        Assert.Equal("SomeString3", values[2].MyProperty);
        Assert.Equal("SomeString4", values[3].MyProperty);
        Assert.Equal("SomeString5", values[4].MyProperty);
        Assert.Equal("SomeString6", values[5].MyProperty);
    }

    public class SomethingToGenerate
    {
        public string? MyProperty { get; set; }
    }

    public Generator<int> Counter()
    {
        return
            state =>
                {
                    var counter = state.Get("MyCounter", 0);
                    var newVal = counter + 1;
                    state.Set("MyCounter", newVal);
                    return new Result<int>(newVal, state);
                };
    }
}
