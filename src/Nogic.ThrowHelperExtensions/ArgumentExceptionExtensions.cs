#if NETSTANDARD2_0
#pragma warning disable CS8777
#endif

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Nogic.ThrowHelperExtensions;

/// <summary>
/// Extension class for <see cref="ArgumentException"/>.
/// </summary>
public static class ArgumentExceptionExtensions
{
    /// <summary>
    /// Extension for <see cref="ArgumentException"/>.
    /// </summary>
    extension(ArgumentException)
    {
        /// <summary>Throws an exception if <paramref name="argument"/> is <see langword="null"/> or empty.</summary>
        /// <param name="argument">The string argument to validate as non-null and non-empty.</param>
        /// <param name="paramName">The name of the parameter with which <paramref name="argument"/> corresponds.</param>
        /// <exception cref="ArgumentNullException"><paramref name="argument"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="argument"/> is empty.</exception>
#if NET7_0_OR_GREATER
        [Obsolete($"Use {nameof(ArgumentException)}.{nameof(ThrowIfNullOrEmpty)}(string?, string?) directly instead.")]
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

        /// <summary>Throws an exception if <paramref name="argument"/> is null, empty, or consists only of white-space characters.</summary>
        /// <param name="argument">The string argument to validate.</param>
        /// <param name="paramName">The name of the parameter with which <paramref name="argument"/> corresponds.</param>
        /// <exception cref="ArgumentNullException"><paramref name="argument"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="argument"/> is empty or consists only of white-space characters.</exception>
#if NET8_0_OR_GREATER
        [Obsolete($"Use {nameof(ArgumentException)}.{nameof(ThrowIfNullOrWhiteSpace)}(string?, string?) directly instead.")]
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
    }
}
