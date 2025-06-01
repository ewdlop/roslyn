using System;
using System.Diagnostics.CodeAnalysis;

namespace DiscriminatedUnions
{
    /// <summary>
    /// Represents a discriminated union that can hold one of two types.
    /// </summary>
    /// <typeparam name="T1">The first possible type</typeparam>
    /// <typeparam name="T2">The second possible type</typeparam>
    public readonly struct OneOf<T1, T2> : IEquatable<OneOf<T1, T2>>
    {
        private readonly object? _value;
        private readonly int _index;

        private OneOf(object? value, int index)
        {
            _value = value;
            _index = index;
        }

        /// <summary>
        /// Creates a OneOf containing a T1 value
        /// </summary>
        public static OneOf<T1, T2> FromT1(T1 value) => new(value, 0);

        /// <summary>
        /// Creates a OneOf containing a T2 value
        /// </summary>
        public static OneOf<T1, T2> FromT2(T2 value) => new(value, 1);

        /// <summary>
        /// Implicit conversion from T1
        /// </summary>
        public static implicit operator OneOf<T1, T2>(T1 value) => FromT1(value);

        /// <summary>
        /// Implicit conversion from T2
        /// </summary>
        public static implicit operator OneOf<T1, T2>(T2 value) => FromT2(value);

        /// <summary>
        /// Pattern matching with exhaustive case handling
        /// </summary>
        public TResult Match<TResult>(Func<T1, TResult> onT1, Func<T2, TResult> onT2)
        {
            return _index switch
            {
                0 => onT1((T1)_value!),
                1 => onT2((T2)_value!),
                _ => throw new InvalidOperationException("Invalid state")
            };
        }

        /// <summary>
        /// Pattern matching with side effects
        /// </summary>
        public void Switch(Action<T1> onT1, Action<T2> onT2)
        {
            switch (_index)
            {
                case 0:
                    onT1((T1)_value!);
                    break;
                case 1:
                    onT2((T2)_value!);
                    break;
                default:
                    throw new InvalidOperationException("Invalid state");
            }
        }

        /// <summary>
        /// Try to get the value as T1
        /// </summary>
        public bool TryGetT1([MaybeNullWhen(false)] out T1 value)
        {
            if (_index == 0)
            {
                value = (T1)_value!;
                return true;
            }
            value = default;
            return false;
        }

        /// <summary>
        /// Try to get the value as T2
        /// </summary>
        public bool TryGetT2([MaybeNullWhen(false)] out T2 value)
        {
            if (_index == 1)
            {
                value = (T2)_value!;
                return true;
            }
            value = default;
            return false;
        }

        /// <summary>
        /// Gets the current value as T1, throws if not T1
        /// </summary>
        public T1 AsT1 => _index == 0 ? (T1)_value! : throw new InvalidOperationException($"Cannot return as T1 as result is T2");

        /// <summary>
        /// Gets the current value as T2, throws if not T2
        /// </summary>
        public T2 AsT2 => _index == 1 ? (T2)_value! : throw new InvalidOperationException($"Cannot return as T2 as result is T1");

        /// <summary>
        /// Checks if the current value is T1
        /// </summary>
        public bool IsT1 => _index == 0;

        /// <summary>
        /// Checks if the current value is T2
        /// </summary>
        public bool IsT2 => _index == 1;

        public bool Equals(OneOf<T1, T2> other)
        {
            return _index == other._index && Equals(_value, other._value);
        }

        public override bool Equals(object? obj)
        {
            return obj is OneOf<T1, T2> other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_value, _index);
        }

        public static bool operator ==(OneOf<T1, T2> left, OneOf<T1, T2> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(OneOf<T1, T2> left, OneOf<T1, T2> right)
        {
            return !left.Equals(right);
        }

        public override string ToString()
        {
            return _value?.ToString() ?? "null";
        }
    }

    /// <summary>
    /// Three-way discriminated union
    /// </summary>
    public readonly struct OneOf<T1, T2, T3> : IEquatable<OneOf<T1, T2, T3>>
    {
        private readonly object? _value;
        private readonly int _index;

        private OneOf(object? value, int index)
        {
            _value = value;
            _index = index;
        }

        public static OneOf<T1, T2, T3> FromT1(T1 value) => new(value, 0);
        public static OneOf<T1, T2, T3> FromT2(T2 value) => new(value, 1);
        public static OneOf<T1, T2, T3> FromT3(T3 value) => new(value, 2);

        public static implicit operator OneOf<T1, T2, T3>(T1 value) => FromT1(value);
        public static implicit operator OneOf<T1, T2, T3>(T2 value) => FromT2(value);
        public static implicit operator OneOf<T1, T2, T3>(T3 value) => FromT3(value);

        public TResult Match<TResult>(Func<T1, TResult> onT1, Func<T2, TResult> onT2, Func<T3, TResult> onT3)
        {
            return _index switch
            {
                0 => onT1((T1)_value!),
                1 => onT2((T2)_value!),
                2 => onT3((T3)_value!),
                _ => throw new InvalidOperationException("Invalid state")
            };
        }

        public void Switch(Action<T1> onT1, Action<T2> onT2, Action<T3> onT3)
        {
            switch (_index)
            {
                case 0: onT1((T1)_value!); break;
                case 1: onT2((T2)_value!); break;
                case 2: onT3((T3)_value!); break;
                default: throw new InvalidOperationException("Invalid state");
            }
        }

        public bool IsT1 => _index == 0;
        public bool IsT2 => _index == 1;
        public bool IsT3 => _index == 2;

        public T1 AsT1 => _index == 0 ? (T1)_value! : throw new InvalidOperationException($"Cannot return as T1");
        public T2 AsT2 => _index == 1 ? (T2)_value! : throw new InvalidOperationException($"Cannot return as T2");
        public T3 AsT3 => _index == 2 ? (T3)_value! : throw new InvalidOperationException($"Cannot return as T3");

        public bool Equals(OneOf<T1, T2, T3> other)
        {
            return _index == other._index && Equals(_value, other._value);
        }

        public override bool Equals(object? obj)
        {
            return obj is OneOf<T1, T2, T3> other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_value, _index);
        }

        public static bool operator ==(OneOf<T1, T2, T3> left, OneOf<T1, T2, T3> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(OneOf<T1, T2, T3> left, OneOf<T1, T2, T3> right)
        {
            return !left.Equals(right);
        }

        public override string ToString()
        {
            return _value?.ToString() ?? "null";
        }
    }
} 