# Maybe<T>

The idea of this class is to help with:

- Methods that can either return a value or error
- Methods that can either return a value or null


#### A Maybe&lt;T&gt; can be assigned a T value:

```
Maybe<int> maybe = 99;

Assert(maybe == 99);
```


#### A Maybe&lt;T&gt; can be compared to a T for equality:

```
var maybe = new Maybe<int>(3);

Assert(maybe == 3);
```


#### A method with return type Maybe&lt;T&gt; can return a T, or an exception, or empty:

```
Maybe<string> MethodThatReturnsAMaybe(int number)
{
    if (number == 42)
    {
        return "42 is a great number.";
    }
    else if (number < 0)
    {
        return new ArgumentException();
    }
    else
    {
        return Maybe<string>.Empty;
    }
}
```

```
var maybe = MethodThatReturnsAMaybe(42);

Assert(maybe == "42 is a great number.");
```

```
var maybe = MethodThatReturnsAMaybe(-1);

Assert(maybe.Exception.GetType() == typeof(ArgumentException));
```

```
var maybe = MethodThatReturnsAMaybe(23);

Assert(maybe.IsEmpty);
```


#### Maybe&lt;T&gt; can be passed to a method that expects a T:

```
var maybe = new Maybe<string>("A piece of text");

MethodThatTakesAString(maybe);
```


unless it is empty or contains an exception:
    
```
var maybe = new Maybe<string>.Empty;

MethodThatTakesAString(maybe); // throws an InvalidOperationException
```

```
var maybe = new Maybe<string>(new Exception());

MethodThatTakesAString(maybe); // throws an InvalidOperationException
```
