using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Nogic.ThrowHelperExtensions;

/// <summary>
/// Extension class for polyfill throw helper methods.
/// </summary>
public static class ArgumentOutOfRangeExceptionExtensions
{
    /// <summary>
    /// Extension for <see cref="ArgumentOutOfRangeException"/>.
    /// </summary>
    extension(ArgumentOutOfRangeException)
    {
        /// <summary>Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is equal to <paramref name="other"/>.</summary>
        /// <param name="value">The argument to validate as not equal to <paramref name="other"/>.</param>
        /// <param name="other">The value to compare with <paramref name="value"/>.</param>
        /// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
#if NET8_0_OR_GREATER
        [Obsolete($"Use {nameof(ArgumentOutOfRangeException)}.{nameof(ThrowIfEqual)}<T>(T, T, string?) directly instead.")]
#endif
        public static void ThrowIfEqual<T>(T value, T other, [CallerArgumentExpression(nameof(value))] string? paramName = null) where T : IEquatable<T>?
        {
            if (EqualityComparer<T>.Default.Equals(value, other))
                ThrowEqual(value, other, paramName);
        }

        /// <summary>Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is not equal to <paramref name="other"/>.</summary>
        /// <param name="value">The argument to validate as equal to <paramref name="other"/>.</param>
        /// <param name="other">The value to compare with <paramref name="value"/>.</param>
        /// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
#if NET8_0_OR_GREATER
        [Obsolete($"Use {nameof(ArgumentOutOfRangeException)}.{nameof(ThrowIfNotEqual)}<T>(T, T, string?) directly instead.")]
#endif
        public static void ThrowIfNotEqual<T>(T value, T other, [CallerArgumentExpression(nameof(value))] string? paramName = null) where T : IEquatable<T>?
        {
            if (!EqualityComparer<T>.Default.Equals(value, other))
                ThrowNotEqual(value, other, paramName);
        }

        /// <summary>Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is greater than <paramref name="other"/>.</summary>
        /// <param name="value">The argument to validate as less or equal than <paramref name="other"/>.</param>
        /// <param name="other">The value to compare with <paramref name="value"/>.</param>
        /// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
#if NET8_0_OR_GREATER
        [Obsolete($"Use {nameof(ArgumentOutOfRangeException)}.{nameof(ThrowIfGreaterThan)}<T>(T, T, string?) directly instead.")]
#endif
        public static void ThrowIfGreaterThan<T>(T value, T other, [CallerArgumentExpression(nameof(value))] string? paramName = null) where T : IComparable<T>
        {
            if (value.CompareTo(other) > 0)
                ThrowGreaterThan(value, other, paramName);
        }

        /// <summary>Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is greater than or equal <paramref name="other"/>.</summary>
        /// <param name="value">The argument to validate as less than <paramref name="other"/>.</param>
        /// <param name="other">The value to compare with <paramref name="value"/>.</param>
        /// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
#if NET8_0_OR_GREATER
        [Obsolete($"Use {nameof(ArgumentOutOfRangeException)}.{nameof(ThrowIfGreaterThanOrEqual)}<T>(T, T, string?) directly instead.")]
#endif
        public static void ThrowIfGreaterThanOrEqual<T>(T value, T other, [CallerArgumentExpression(nameof(value))] string? paramName = null) where T : IComparable<T>
        {
            if (value.CompareTo(other) >= 0)
                ThrowGreaterThanOrEqual(value, other, paramName);
        }

        /// <summary>Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is less than <paramref name="other"/>.</summary>
        /// <param name="value">The argument to validate as greater than or equal than <paramref name="other"/>.</param>
        /// <param name="other">The value to compare with <paramref name="value"/>.</param>
        /// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
#if NET8_0_OR_GREATER
        [Obsolete($"Use {nameof(ArgumentOutOfRangeException)}.{nameof(ThrowIfLessThanOrEqual)}<T>(T, T, string?) directly instead.")]
#endif
        public static void ThrowIfLessThan<T>(T value, T other, [CallerArgumentExpression(nameof(value))] string? paramName = null) where T : IComparable<T>
        {
            if (value.CompareTo(other) < 0)
                ThrowLessThan(value, other, paramName);
        }

        /// <summary>Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is less than or equal <paramref name="other"/>.</summary>
        /// <param name="value">The argument to validate as greater than than <paramref name="other"/>.</param>
        /// <param name="other">The value to compare with <paramref name="value"/>.</param>
        /// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
#if NET8_0_OR_GREATER
        [Obsolete($"Use {nameof(ArgumentOutOfRangeException)}.{nameof(ThrowIfLessThanOrEqual)}<T>(T, T, string?) directly instead.")]
#endif
        public static void ThrowIfLessThanOrEqual<T>(T value, T other, [CallerArgumentExpression(nameof(value))] string? paramName = null) where T : IComparable<T>
        {
            if (value.CompareTo(other) <= 0)
                ThrowLessThanOrEqual(value, other, paramName);
        }

#if NET7_0_OR_GREATER
        /// <summary>Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is zero.</summary>
        /// <param name="value">The argument to validate as non-zero.</param>
        /// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
#if NET8_0_OR_GREATER
        [Obsolete($"Use {nameof(ArgumentOutOfRangeException)}.{nameof(ThrowIfZero)}<T>(T, string?) directly instead.")]
#endif
        public static void ThrowIfZero<T>(T value, [CallerArgumentExpression(nameof(value))] string? paramName = null) where T : INumberBase<T>
        {
            if (T.IsZero(value))
                ThrowZero(value, paramName);
        }

        /// <summary>Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is negative.</summary>
        /// <param name="value">The argument to validate as non-negative.</param>
        /// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
#if NET8_0_OR_GREATER
        [Obsolete($"Use {nameof(ArgumentOutOfRangeException)}.{nameof(ThrowIfNegative)}<T>(T, string?) directly instead.")]
#endif
        public static void ThrowIfNegative<T>(T value, [CallerArgumentExpression(nameof(value))] string? paramName = null) where T : INumberBase<T>
        {
            if (T.IsNegative(value))
                ThrowNegative(value, paramName);
        }

        /// <summary>Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is negative or zero.</summary>
        /// <param name="value">The argument to validate as non-zero or non-negative.</param>
        /// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
#if NET8_0_OR_GREATER
        [Obsolete($"Use {nameof(ArgumentOutOfRangeException)}.{nameof(ThrowIfNegativeOrZero)}<T>(T, string?) directly instead.")]
#endif
        public static void ThrowIfNegativeOrZero<T>(T value, [CallerArgumentExpression(nameof(value))] string? paramName = null) where T : INumberBase<T>
        {
            if (T.IsNegative(value) || T.IsZero(value))
                ThrowNegativeOrZero(value, paramName);
        }
#else
        /// <summary>Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is zero.</summary>
        /// <param name="value">The argument to validate as non-zero.</param>
        /// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
        public static void ThrowIfZero(byte value, [CallerArgumentExpression(nameof(value))] string? paramName = null)
        {
            if (value == 0)
                ThrowZero(value, paramName);
        }

        /// <inheritdoc cref="ThrowIfZero(byte, string?)"/>
        [CLSCompliant(false)]
        public static void ThrowIfZero(sbyte value, [CallerArgumentExpression(nameof(value))] string? paramName = null)
        {
            if (value == 0)
                ThrowZero(value, paramName);
        }

        /// <summary>Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is negative.</summary>
        /// <param name="value">The argument to validate as non-negative.</param>
        /// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
        [CLSCompliant(false)]
        public static void ThrowIfNegative(sbyte value, [CallerArgumentExpression(nameof(value))] string? paramName = null)
        {
            if (value < 0)
                ThrowNegative(value, paramName);
        }

        /// <summary>Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is negative or zero.</summary>
        /// <param name="value">The argument to validate as non-zero or non-negative.</param>
        /// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
        [CLSCompliant(false)]
        public static void ThrowIfNegativeOrZero(sbyte value, [CallerArgumentExpression(nameof(value))] string? paramName = null)
        {
            if (value <= 0)
                ThrowNegativeOrZero(value, paramName);
        }

        /// <inheritdoc cref="ThrowIfZero(byte, string?)"/>
        public static void ThrowIfZero(short value, [CallerArgumentExpression(nameof(value))] string? paramName = null)
        {
            if (value == 0)
                ThrowZero(value, paramName);
        }

        /// <inheritdoc cref="ThrowIfNegative(sbyte, string?)"/>
        public static void ThrowIfNegative(short value, [CallerArgumentExpression(nameof(value))] string? paramName = null)
        {
            if (value < 0)
                ThrowNegative(value, paramName);
        }

        /// <inheritdoc cref="ThrowIfNegativeOrZero(sbyte, string?)"/>
        public static void ThrowIfNegativeOrZero(short value, [CallerArgumentExpression(nameof(value))] string? paramName = null)
        {
            if (value <= 0)
                ThrowNegativeOrZero(value, paramName);
        }

        /// <inheritdoc cref="ThrowIfZero(byte, string?)"/>
        [CLSCompliant(false)]
        public static void ThrowIfZero(ushort value, [CallerArgumentExpression(nameof(value))] string? paramName = null)
        {
            if (value == 0)
                ThrowZero(value, paramName);
        }

        /// <inheritdoc cref="ThrowIfZero(byte, string?)"/>
        public static void ThrowIfZero(int value, [CallerArgumentExpression(nameof(value))] string? paramName = null)
        {
            if (value == 0)
                ThrowZero(value, paramName);
        }

        /// <inheritdoc cref="ThrowIfNegative(sbyte, string?)"/>
        public static void ThrowIfNegative(int value, [CallerArgumentExpression(nameof(value))] string? paramName = null)
        {
            if (value < 0)
                ThrowNegative(value, paramName);
        }

        /// <inheritdoc cref="ThrowIfNegativeOrZero(sbyte, string?)"/>
        public static void ThrowIfNegativeOrZero(int value, [CallerArgumentExpression(nameof(value))] string? paramName = null)
        {
            if (value <= 0)
                ThrowNegativeOrZero(value, paramName);
        }

        /// <inheritdoc cref="ThrowIfZero(byte, string?)"/>
        [CLSCompliant(false)]
        public static void ThrowIfZero(uint value, [CallerArgumentExpression(nameof(value))] string? paramName = null)
        {
            if (value == 0)
                ThrowZero(value, paramName);
        }

        /// <inheritdoc cref="ThrowIfZero(byte, string?)"/>
        public static void ThrowIfZero(long value, [CallerArgumentExpression(nameof(value))] string? paramName = null)
        {
            if (value == 0)
                ThrowZero(value, paramName);
        }

        /// <inheritdoc cref="ThrowIfNegative(sbyte, string?)"/>
        public static void ThrowIfNegative(long value, [CallerArgumentExpression(nameof(value))] string? paramName = null)
        {
            if (value < 0)
                ThrowNegative(value, paramName);
        }

        /// <inheritdoc cref="ThrowIfNegativeOrZero(sbyte, string?)"/>
        public static void ThrowIfNegativeOrZero(long value, [CallerArgumentExpression(nameof(value))] string? paramName = null)
        {
            if (value <= 0)
                ThrowNegativeOrZero(value, paramName);
        }

        /// <inheritdoc cref="ThrowIfZero(byte, string?)"/>
        [CLSCompliant(false)]
        public static void ThrowIfZero(ulong value, [CallerArgumentExpression(nameof(value))] string? paramName = null)
        {
            if (value == 0)
                ThrowZero(value, paramName);
        }

        /// <inheritdoc cref="ThrowIfZero(byte, string?)"/>
        public static void ThrowIfZero(float value, [CallerArgumentExpression(nameof(value))] string? paramName = null)
        {
            if (value == 0)
                ThrowZero(value, paramName);
        }

        /// <inheritdoc cref="ThrowIfNegative(sbyte, string?)"/>
        public static void ThrowIfNegative(float value, [CallerArgumentExpression(nameof(value))] string? paramName = null)
        {
            if (value < 0)
                ThrowNegative(value, paramName);
        }

        /// <inheritdoc cref="ThrowIfNegativeOrZero(sbyte, string?)"/>
        public static void ThrowIfNegativeOrZero(float value, [CallerArgumentExpression(nameof(value))] string? paramName = null)
        {
            if (value <= 0)
                ThrowNegativeOrZero(value, paramName);
        }

        /// <inheritdoc cref="ThrowIfZero(byte, string?)"/>
        public static void ThrowIfZero(double value, [CallerArgumentExpression(nameof(value))] string? paramName = null)
        {
            if (value == 0)
                ThrowZero(value, paramName);
        }

        /// <inheritdoc cref="ThrowIfNegative(sbyte, string?)"/>
        public static void ThrowIfNegative(double value, [CallerArgumentExpression(nameof(value))] string? paramName = null)
        {
            if (value < 0)
                ThrowNegative(value, paramName);
        }

        /// <inheritdoc cref="ThrowIfNegativeOrZero(sbyte, string?)"/>
        public static void ThrowIfNegativeOrZero(double value, [CallerArgumentExpression(nameof(value))] string? paramName = null)
        {
            if (value <= 0)
                ThrowNegativeOrZero(value, paramName);
        }

        /// <inheritdoc cref="ThrowIfZero(byte, string?)"/>
        public static void ThrowIfZero(decimal value, [CallerArgumentExpression(nameof(value))] string? paramName = null)
        {
            if (value == 0)
                ThrowZero(value, paramName);
        }

        /// <inheritdoc cref="ThrowIfNegative(sbyte, string?)"/>
        public static void ThrowIfNegative(decimal value, [CallerArgumentExpression(nameof(value))] string? paramName = null)
        {
            if (value < 0)
                ThrowNegative(value, paramName);
        }

        /// <inheritdoc cref="ThrowIfNegativeOrZero(sbyte, string?)"/>
        public static void ThrowIfNegativeOrZero(decimal value, [CallerArgumentExpression(nameof(value))] string? paramName = null)
        {
            if (value <= 0)
                ThrowNegativeOrZero(value, paramName);
        }

        /// <inheritdoc cref="ThrowIfZero(byte, string?)"/>
        public static void ThrowIfZero(nint value, [CallerArgumentExpression(nameof(value))] string? paramName = null)
        {
            if (value == 0)
                ThrowZero(value, paramName);
        }

        /// <inheritdoc cref="ThrowIfNegative(sbyte, string?)"/>
        public static void ThrowIfNegative(nint value, [CallerArgumentExpression(nameof(value))] string? paramName = null)
        {
            if (value < 0)
                ThrowNegative(value, paramName);
        }

        /// <inheritdoc cref="ThrowIfNegativeOrZero(sbyte, string?)"/>
        public static void ThrowIfNegativeOrZero(nint value, [CallerArgumentExpression(nameof(value))] string? paramName = null)
        {
            if (value <= 0)
                ThrowNegativeOrZero(value, paramName);
        }

        /// <inheritdoc cref="ThrowIfZero(byte, string?)"/>
        [CLSCompliant(false)]
        public static void ThrowIfZero(nuint value, [CallerArgumentExpression(nameof(value))] string? paramName = null)
        {
            if (value == 0)
                ThrowZero(value, paramName);
        }

        /// <inheritdoc cref="ThrowIfZero(byte, string?)"/>
        public static void ThrowIfZero(char value, [CallerArgumentExpression(nameof(value))] string? paramName = null)
        {
            if (value == 0)
                ThrowZero(value, paramName);
        }
#endif
    }

    [DoesNotReturn]
    private static void ThrowEqual<T>(T value, T other, string? paramName)
        => throw new ArgumentOutOfRangeException(paramName, value, $"{paramName} ('{value}') must not be equal to '{other}'.");

    [DoesNotReturn]
    private static void ThrowNotEqual<T>(T value, T other, string? paramName)
        => throw new ArgumentOutOfRangeException(paramName, value, $"{paramName} ('{value}') must be equal to '{other}'.");

    [DoesNotReturn]
    private static void ThrowGreaterThan<T>(T value, T other, string? paramName)
        => throw new ArgumentOutOfRangeException(paramName, value, $"{paramName} ('{value}') must be less than or equal to '{other}'.");

    [DoesNotReturn]
    private static void ThrowGreaterThanOrEqual<T>(T value, T other, string? paramName)
        => throw new ArgumentOutOfRangeException(paramName, value, $"{paramName} ('{value}') must be less than '{other}'.");

    [DoesNotReturn]
    private static void ThrowLessThan<T>(T value, T other, string? paramName)
        => throw new ArgumentOutOfRangeException(paramName, value, $"{paramName} ('{value}') must be greater than or equal to '{other}'.");

    [DoesNotReturn]
    private static void ThrowLessThanOrEqual<T>(T value, T other, string? paramName)
        => throw new ArgumentOutOfRangeException(paramName, value, $"{paramName} ('{value}') must be greater than '{other}'.");

    [DoesNotReturn]
    private static void ThrowZero<T>(T value, string? paramName)
        => throw new ArgumentOutOfRangeException(paramName, value, $"{paramName} ('{value}') must be a non-zero value.");

    [DoesNotReturn]
    private static void ThrowNegative<T>(T value, string? paramName)
        => throw new ArgumentOutOfRangeException(paramName, value, $"{paramName} ('{value}') must be a non-negative value.");

    [DoesNotReturn]
    private static void ThrowNegativeOrZero<T>(T value, string? paramName)
        => throw new ArgumentOutOfRangeException(paramName, value, $"{paramName} ('{value}') must be a non-negative and non-zero value.");
}
