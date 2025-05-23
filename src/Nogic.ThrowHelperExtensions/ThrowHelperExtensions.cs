using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Nogic.ThrowHelperExtensions;

/// <summary>
/// Extension class for polyfill throw helper methods.
/// </summary>
public static class ThrowHelperExtensions
{
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
        [Obsolete($"Use {nameof(ArgumentNullException)}.{nameof(ThrowIfNull)}(object?, string) directly instead.")]
#endif
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfNull([NotNull] object? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        {
            if (argument is null)
                Throw(paramName);
        }

        /// <summary>
        /// <inheritdoc cref="ThrowIfNull(object?, string?)" path="/summary"/>
        /// </summary>
        /// <param name="argument">The pointer argument to validate as non-null.</param>
        /// <param name="paramName"><inheritdoc cref="ThrowIfNull(object?, string?)" path="/param[@name='paramName']"/></param>
        /// <exception cref="ArgumentNullException"><inheritdoc cref="ThrowIfNull(object?, string?)" path="/exception"/></exception>
#if NET7_0_OR_GREATER
        [Obsolete($"Use {nameof(ArgumentNullException)}.{nameof(ThrowIfNull)}(void*, string) directly instead.")]
#endif
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void ThrowIfNull([NotNull] void* argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        {
            if (argument is null)
                Throw(paramName);
        }

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void Throw(string? paramName) => throw new ArgumentNullException(paramName);
    }
}
