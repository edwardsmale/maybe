using System;

namespace MaybeClass
{
    public class Program
    {
        public static void Main()
        {
            {
                var maybe = new Maybe<int>(3);

                // A Maybe<T> can be compared to a T for equality:

                Assert(maybe == 3);
                Assert(3 == maybe);
            }

            {
                // A Maybe<T> can be assigned a T value:

                Maybe<int> maybe = 99;

                Assert(maybe == 99);
            }

            {
                // A Maybe<T> can be passed to a method that expects a T:

                var maybe = new Maybe<string>("A piece of text");

                var result = MethodThatTakesAString(maybe);

                Assert(result == "A piece of text was passed.");
            }

            {
                // A non-valued maybe throws an exception when implicitly converted to T:

                var maybe = new Maybe<string>(new Exception("It all went wrong!!! :("));

                try
                {
                    MethodThatTakesAString(maybe);
                }
                catch (Exception ex)
                {
                    Assert(ex.GetType() == typeof(InvalidOperationException));
                }
            }

            {
                // A method with return type Maybe<T> can return a T:

                var maybe = MethodThatReturnsAMaybe(42);

                Assert(maybe == "42 is a great number.");
            }

            {
                // A method with return type Maybe<T> can return an exception:

                var maybe = MethodThatReturnsAMaybe(-1);

                Assert(maybe.Exception.GetType() == typeof(ArgumentException));
            }

            {
                // A method with return type Maybe<T> can return maybe that contains null.

                var maybe = MethodThatReturnsAMaybe(23);

                Assert(maybe.IsNull);
            }

            void Assert(bool condition)
            {
                if (!condition)
                {
                    throw new Exception("Assert failed.");
                }
            }
        }

        public static string MethodThatTakesAString(string str)
        {
            return str + " was passed.";
        }

        public static Maybe<string> MethodThatReturnsAMaybe(int number)
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
                return Maybe<string>.Null;
            }
        }
    }
}