# Maybe<T>

The idea of this class is to help with:

- Methods that can either return a value or error
- Methods that can either return a value or null


## Using a Maybe<T> as its contained value

#### A Maybe<T> can be assigned a T value:

```
Maybe<int> maybe = 99;

Assert(maybe == 99);
```


#### A Maybe<T> can be compared to a T for equality:

```
var maybe = new Maybe<int>(3);

Assert(maybe == 3);

Assert(3 == maybe);
```


## Maybe<T> as a method parameter

#### Maybe<T> can be passed to a method that expects a T:

```
var maybe = new Maybe<string>("A piece of text");

var result = MethodThatTakesAString(maybe);

Assert(result == "A piece of text was passed.");
```


#### A non-valued Maybe<T> throws an exception when implicitly converted to T:

```
var maybe = new Maybe<string>(new Exception("It all went wrong!!! :("));

try
{
    MethodThatTakesAString(maybe);
}
catch (Exception ex)
{
    Assert(ex.GetType() == typeof(InvalidOperationException));
}
```

```
string MethodThatTakesAString(string str)
{
    return str + " was passed.";
}
```


## Returning Maybe<T> from a method

#### A method with return type Maybe<T> can return a T:

```
var maybe = MethodThatReturnsAMaybe(42);

Assert(maybe == "42 is a great number.");
```

#### A method with return type Maybe<T> can return an exception:

```
var maybe = MethodThatReturnsAMaybe(-1);

Assert(maybe.Exception.GetType() == typeof(ArgumentException));
```

#### A method with return type Maybe<T> can return maybe that contains null.

```
var maybe = MethodThatReturnsAMaybe(23);

Assert(maybe.IsNull);
```

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
        return Maybe.Null;
    }
}
```