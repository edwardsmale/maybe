using System;

namespace MaybeClass
{
    /// <summary>
    /// A class containing a value of type <typeparamref name="T"/>, an exception, or null.
    /// </summary>
    public class Maybe<T>
    {
        public readonly T Value;
        public readonly bool HasValue;
        public readonly Exception Exception;
        public readonly bool IsNull;

        /// <summary>
        /// Initialises a maybe object with a value of type <typeparamref name="T"/>.
        /// </summary>
        public Maybe(T value)
        {
            this.HasValue = true;
            this.Value = value;
        }

        /// <summary>
        /// Initialises a maybe object containing an exception or null.
        /// </summary>
        public Maybe(Exception ex = null)
        {
            if (ex != null)
            {
                this.Exception = ex;
            }
            else
            {
                this.IsNull = true;
            }
        }

        // The true and false operators allow a Maybe<T> value to be used in a conditional, e.g.
        //
        //     var response = this.GetResponse();  // Returns Maybe<ResponseObject>
        //
        //     if (response)
        //     {
        //         ...
        //     }
        //
        public static bool operator true(Maybe<T> m) => m.HasValue;
        public static bool operator false(Maybe<T> m) => !m.HasValue;

        // Allows a Maybe<T> to be passed to a method that takes a T parameter:
        public static implicit operator T(Maybe<T> m)
        {
            if (!m.HasValue)
            {
                throw new InvalidOperationException("Maybe<T> does not contain a value");
            }
            else
            {
                return m.Value;
            }
        }

        // Allows a method with return type Maybe<T> to return a T:
        public static implicit operator Maybe<T>(T value) => new Maybe<T>(value);

        // Allows a method with return type Maybe<T> to return an Exception:
        public static implicit operator Maybe<T>(Exception ex) => new Maybe<T>(ex);

        // Allows a method with return type Maybe<T> to return Maybe<T>.Null:
        public static readonly Exception Null = null;

        // Allows comparison between a Maybe<T> and a T.
        public static bool operator ==(Maybe<T> x, T y) => x.HasValue && x.Value.Equals(y);
        public static bool operator !=(Maybe<T> x, T y) => !(x == y);

        // Allows comparison between a T and a Maybe<T>.
        public static bool operator ==(T x, Maybe<T> y) => (y == x);
        public static bool operator !=(T x, Maybe<T> y) => (y != x);

        // Allows comparison between two Maybe<T> objects by value.
        public static bool operator ==(Maybe<T> x, Maybe<T> y) => x.Equals(y);
        public static bool operator !=(Maybe<T> x, Maybe<T> y) => !(x == y);

        public override bool Equals(object obj)
        {
            return
                obj is Maybe<T> other &&
                this.HasValue &&
                other.HasValue &&
                this.Value.Equals(other.Value);
        }

        public override int GetHashCode() => this.GetValueOrNonValue().GetHashCode();
        public override string ToString() => this.GetValueOrNonValue().ToString();

        private object GetValueOrNonValue()
        {
            return this.HasValue ? this.Value : (object)this.Exception ?? "null";
        }
    }
}