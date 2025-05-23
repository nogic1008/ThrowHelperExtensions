using System.Diagnostics.CodeAnalysis;

namespace Nogic.ThrowHelperExtensions;

/// <summary>
/// Extension class for <see cref="ObjectDisposedException"/>.
/// </summary>
public static class ObjectDisposedExceptionExtensions
{
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
    }

    [DoesNotReturn]
    private static void ThrowObjectDisposedException(Type type) => throw new ObjectDisposedException(type.FullName);
}
