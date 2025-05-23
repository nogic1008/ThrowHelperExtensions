using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Nogic.ThrowHelperExtensions;

/// <summary>
/// Extension class for polyfill throw helper methods.
/// </summary>
public static class ThrowHelperExtensions
{
    /// <summary>
    /// Extension for <see cref="ArgumentException"/>.
    /// </summary>
    extension(ArgumentException)
    {
        /// <summary>Throws an exception if <paramref name="argument"/> is <see langword="null"/> or empty.</summary>
        /// <param name="argument">The string argument to validate as non-null and non-empty.</param>
        /// <param name="paramName"><inheritdoc cref="ThrowIfNull(object?, string?)" path="/param[@name='paramName']"/></param>
        /// <exception cref="ArgumentNullException"><inheritdoc cref="ThrowIfNull(object?, string?)" path="/exception"/></exception>
        /// <exception cref="ArgumentException"><paramref name="argument"/> is empty.</exception>
#if NET7_0_OR_GREATER
        [Obsolete($"Use {nameof(ArgumentException)}.{nameof(ThrowIfNullOrEmpty)}(string?, string?) directly instead.")]
#endif
#if NETSTANDARD2_0
#pragma warning disable CS8777
#endif
        public static void ThrowIfNullOrEmpty([NotNull] string? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        {
            if (string.IsNullOrEmpty(argument))
                Throw(argument, paramName);

            [DoesNotReturn]
            static void Throw(string? argument, string? paramName)
            {
                ArgumentNullException.ThrowIfNull(argument, paramName);
                throw new ArgumentException("Value cannot be an empty string.", paramName);
            }
        }
#if NETSTANDARD2_0
#pragma warning restore CS8777
#endif

        /// <summary>Throws an exception if <paramref name="argument"/> is null, empty, or consists only of white-space characters.</summary>
        /// <param name="argument">The string argument to validate.</param>
        /// <param name="paramName"><inheritdoc cref="ThrowIfNull(object?, string?)" path="/param[@name='paramName']"/></param>
        /// <exception cref="ArgumentNullException"><inheritdoc cref="ThrowIfNull(object?, string?)" path="/exception"/></exception>
        /// <exception cref="ArgumentException"><paramref name="argument"/> is empty or consists only of white-space characters.</exception>
#if NET8_0_OR_GREATER
        [Obsolete($"Use {nameof(ArgumentException)}.{nameof(ThrowIfNullOrWhiteSpace)}(string?, string?) directly instead.")]
#endif
#if NETSTANDARD2_0
#pragma warning disable CS8777
#endif
        public static void ThrowIfNullOrWhiteSpace([NotNull] string? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        {
            if (string.IsNullOrWhiteSpace(argument))
                Throw(argument, paramName);

            [DoesNotReturn]
            static void Throw(string? argument, string? paramName)
            {
                ArgumentNullException.ThrowIfNull(argument, paramName);
                throw new ArgumentException("The value cannot be an empty string or composed entirely of whitespace.", paramName);
            }
        }
#if NETSTANDARD2_0
#pragma warning restore CS8777
#endif
    }

    /// <summary>
    /// Extension for <see cref="ArgumentNullException"/>.
    /// </summary>
    extension(ArgumentNullException)
    {
        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> if <paramref name="argument"/> is <see langword="null"/>.
        /// </summary>
        /// <param name="argument">The reference type argument to validate as non-null.</param>
        /// <param name="paramName">The name of the parameter with which <paramref name="argument"/> corresponds.</param>
        /// <exception cref="ArgumentNullException"><paramref name="argument"/> is <see langword="null"/>.</exception>
#if NET6_0_OR_GREATER
        [Obsolete($"Use {nameof(ArgumentNullException)}.{nameof(ThrowIfNull)}(object?, string?) directly instead.")]
#endif
        public static void ThrowIfNull([NotNull] object? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        {
            if (argument is null)
                ThrowArgumentNullException(paramName);
        }

        /// <summary>
        /// <inheritdoc cref="ThrowIfNull(object?, string?)" path="/summary"/>
        /// </summary>
        /// <param name="argument">The pointer argument to validate as non-null.</param>
        /// <param name="paramName"><inheritdoc cref="ThrowIfNull(object?, string?)" path="/param[@name='paramName']"/></param>
        /// <exception cref="ArgumentNullException"><inheritdoc cref="ThrowIfNull(object?, string?)" path="/exception"/></exception>
#if NET7_0_OR_GREATER
        [Obsolete($"Use {nameof(ArgumentNullException)}.{nameof(ThrowIfNull)}(void*, string?) directly instead.")]
#endif
        [CLSCompliant(false)]
        public unsafe static void ThrowIfNull([NotNull] void* argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        {
            if (argument is null)
                ThrowArgumentNullException(paramName);
        }

        [DoesNotReturn]
        private static void ThrowArgumentNullException(string? paramName) => throw new ArgumentNullException(paramName);
    }

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

        /// <inheritdoc cref="ThrowIfZero(byte, string?)"/>
        public static void ThrowIfZero(BigInteger value, [CallerArgumentExpression(nameof(value))] string? paramName = null)
        {
            if (value == BigInteger.Zero)
                ThrowZero(value, paramName);
        }

        /// <inheritdoc cref="ThrowIfNegative(sbyte, string?)"/>
        public static void ThrowIfNegative(BigInteger value, [CallerArgumentExpression(nameof(value))] string? paramName = null)
        {
            if (value.Sign < 0)
                ThrowNegative(value, paramName);
        }

        /// <inheritdoc cref="ThrowIfNegativeOrZero(sbyte, string?)"/>
        public static void ThrowIfNegativeOrZero(BigInteger value, [CallerArgumentExpression(nameof(value))] string? paramName = null)
        {
            if (value.Sign <= 0)
                ThrowNegativeOrZero(value, paramName);
        }

        /// <inheritdoc cref="ThrowIfZero(byte, string?)"/>
        public static void ThrowIfZero(Complex value, [CallerArgumentExpression(nameof(value))] string? paramName = null)
        {
            if (value == Complex.Zero)
                ThrowZero(value, paramName);
        }

        /// <inheritdoc cref="ThrowIfNegative(sbyte, string?)"/>
        public static void ThrowIfNegative(Complex value, [CallerArgumentExpression(nameof(value))] string? paramName = null)
        {
            if (value.Imaginary == 0.0 && value.Real < 0.0)
                ThrowNegative(value, paramName);
        }

        /// <inheritdoc cref="ThrowIfNegativeOrZero(sbyte, string?)"/>
        public static void ThrowIfNegativeOrZero(Complex value, [CallerArgumentExpression(nameof(value))] string? paramName = null)
        {
            if (value.Imaginary == 0.0 && value.Real <= 0.0)
                ThrowNegativeOrZero(value, paramName);
        }
#endif

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

    /// <summary>
    /// Extension for <see cref="ObjectDisposedException"/>.
    /// </summary>
    extension(ObjectDisposedException)
    {
        /// <summary>Throws an <see cref="ObjectDisposedException"/> if the specified <paramref name="condition"/> is <see langword="true"/>.</summary>
        /// <param name="condition">The condition to evaluate.</param>
        /// <param name="instance">The object whose type's full name should be included in any resulting <see cref="ObjectDisposedException"/>.</param>
        /// <exception cref="ObjectDisposedException">The <paramref name="condition"/> is <see langword="true"/>.</exception>
#if NET7_0_OR_GREATER
        [Obsolete($"Use {nameof(ObjectDisposedException)}.{nameof(ThrowIf)}(bool, object) directly instead.")]
#endif
        public static void ThrowIf([DoesNotReturnIf(true)] bool condition, object instance)
        {
            if (condition)
                ThrowObjectDisposedException(instance.GetType());
        }

        /// <summary><inheritdoc cref="ThrowIf(bool, object)" path="/summary"/></summary>
        /// <param name="condition"><inheritdoc cref="ThrowIf(bool, object)" path="/param[@name='condition']"/></param>
        /// <param name="type">The type whose full name should be included in any resulting <see cref="ObjectDisposedException"/>.</param>
        /// <exception cref="ObjectDisposedException"><inheritdoc cref="ThrowIf(bool, object)" path="/exception"/></exception>
#if NET7_0_OR_GREATER
        [Obsolete($"Use {nameof(ObjectDisposedException)}.{nameof(ThrowIf)}(bool, Type) directly instead.")]
#endif
        public static void ThrowIf([DoesNotReturnIf(true)] bool condition, Type type)
        {
            if (condition)
                ThrowObjectDisposedException(type);
        }

        [DoesNotReturn]
        private static void ThrowObjectDisposedException(Type type) => throw new ObjectDisposedException(type.FullName);
    }
}
