using System.Diagnostics.CodeAnalysis;
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
