using System;

namespace MaybeClass
{
    public struct Maybe
    {
        // NullClass is a marker type which we can use in one of the implicit conversion operators in Maybe<T>.
        //
        // This allows a method that has return type Maybe<T> to do:
        //
        //     return Maybe.Null;
        //
        public static NullClass Null => new NullClass();
        public struct NullClass { }
    }

    public struct Maybe<T>
    {
        public Maybe(T value)
        {
            this.Value = value;
            this.NonValue = new MaybeNonValue(MaybeNonValue.NonValueTypeEnum.HasValue);
        }

        public Maybe(Exception ex = null)
        {
            this.Value = default(T);

            if (ex != null)
            {
                this.NonValue = new MaybeNonValue(ex);
            }
            else
            {
                this.NonValue = new MaybeNonValue();
            }
        }

        public T Value { get; }
        private MaybeNonValue NonValue { get; set; }

        public bool HasValue => this.NonValue?.NonValueType == MaybeNonValue.NonValueTypeEnum.HasValue;
        public bool HasError => !this.HasValue;

        public Exception Exception => this.NonValue.Exception;
        public string Message => this.NonValue.Message;
        public bool IsNull => this.NonValue.NonValueType == MaybeNonValue.NonValueTypeEnum.Null;

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
        public static implicit operator T(Maybe<T> m) => m.HasValue ? m.Value : throw new InvalidCastException();

        // Allows a method with return type Maybe<T> to return a T directly:
        public static implicit operator Maybe<T>(T value) => new Maybe<T>(value);

        // Allows a method with return type Maybe<T> to return an Exception:
        public static implicit operator Maybe<T>(Exception ex) => new Maybe<T>(ex);

        // Allows a method with return type Maybe<T> to return Maybe.Null:
        public static implicit operator Maybe<T>(Maybe.NullClass _) => new Maybe<T>() { NonValue = new MaybeNonValue() };

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
                (obj is Maybe<T> other) &&
                this.HasValue &&
                other.HasValue &&
                this.Value.Equals(other.Value);
        }

        public override int GetHashCode() => this.GetValueOrNonValue().GetHashCode();
        public override string ToString() => this.GetValueOrNonValue().ToString();

        private object GetValueOrNonValue()
        {
            if (this.HasValue)
            {
                return this.Value;
            }
            else
            {
                return (object)this.NonValue ?? "null";
            }
        }

        private class MaybeNonValue
        {
            public enum NonValueTypeEnum
            {
                /// <summary>
                /// The Maybe actually has a value.
                /// </summary>
                /// <remarks>
                /// It is more convenient to have this placeholder than to have 
                /// <see cref="Maybe{T}.NonValue"/> set to null.
                /// </remarks>
                HasValue,
                /// <summary>
                /// The maybe contains null, rather than a value.
                /// </summary>
                Null,
                /// <summary>
                /// The maybe contains some kind of message, rather than a value.
                /// </summary>
                Message,
                /// <summary>
                /// The maybe contains an exception, rather than a value.
                /// </summary>
                Exception
            }

            /// <summary>
            /// Initialises the object by setting <see cref="NonValueType"/> as specified.
            /// </summary>
            /// <remarks>
            /// Allows the object to be initialised to <see cref="NonValueTypeEnum.HasValue"/>.
            /// </remarks>
            public MaybeNonValue(NonValueTypeEnum nonValueType)
            {
                this.NonValueType = nonValueType;
            }

            /// <summary>
            /// Initialises the object to contain null.
            /// </summary>
            public MaybeNonValue()
            {
                this.NonValueType = NonValueTypeEnum.Null;
            }

            /// <summary>
            /// If <paramref name="message"/> is not null, initialises the object to contain a 
            /// string message; otherwise initialises the object to contain null.
            /// </summary>
            public MaybeNonValue(string message)
            {
                if (message == null)
                {
                    this.NonValueType = NonValueTypeEnum.Null;
                }
                else
                {
                    this.Message = message;
                    this.NonValueType = NonValueTypeEnum.Message;
                }
            }

            /// <summary>
            /// Initialises the object to contain an exception.
            /// </summary>
            public MaybeNonValue(Exception exception)
            {
                this.Exception = exception;
                this.NonValueType = NonValueTypeEnum.Exception;
            }

            /// <summary>
            /// Gets a value indicating the nature of the data stored in the object.
            /// </summary>
            public NonValueTypeEnum NonValueType { get; }

            /// <summary>
            /// Gets the message that the object contains, if any.
            /// </summary>
            public string Message { get; }

            /// <summary>
            /// Gets the exception that the object contains, if any.
            /// </summary>
            public Exception Exception { get; }
        }
    }
}