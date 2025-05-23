namespace Nogic.ThrowHelperExtensions.Tests;

/// <summary>
/// Unit test class for <see cref="ThrowHelperExtensions"/>
/// </summary>
public sealed class ThrowHelperExtensionsTest
{
    /// <summary>
    /// Unit test for <see cref="ArgumentException"/> extension members
    /// </summary>
    [TestClass]
    public sealed class ArgumentExceptionTest
    {
        [TestMethod($"{nameof(ArgumentException)}.{nameof(ThrowHelperExtensions.ThrowIfNullOrEmpty)}(null) throws {nameof(ArgumentNullException)}")]
        public void When_Argument_Is_Null_ThrowIfNullOrEmpty_Throws_ArgumentNullException()
        {
            // Arrange
            string? argument = null;

            // Act
#if NET7_0_OR_GREATER
#pragma warning disable CS0618
#endif
            var action = () => ThrowHelperExtensions.ThrowIfNullOrEmpty(argument!);
#if NET7_0_OR_GREATER
#pragma warning restore CS0618
#endif

            // Assert
            action.ShouldThrow<ArgumentNullException>().ParamName.ShouldBe(nameof(argument));
        }

        [TestMethod($"{nameof(ArgumentException)}.{nameof(ThrowHelperExtensions.ThrowIfNullOrEmpty)}(\"\") throws {nameof(ArgumentException)}")]
        public void When_Argument_Is_Empty_String_ThrowIfNullOrEmpty_Throws_ArgumentException()
        {
            // Arrange
            string? argument = "";

            // Act
#if NET7_0_OR_GREATER
#pragma warning disable CS0618
#endif
            var action = () => ThrowHelperExtensions.ThrowIfNullOrEmpty(argument);
#if NET7_0_OR_GREATER
#pragma warning restore CS0618
#endif

            // Assert
            action.ShouldThrow<ArgumentException>().ParamName.ShouldBe(nameof(argument));
        }

        [TestMethod($"{nameof(ArgumentNullException)}.{nameof(ThrowHelperExtensions.ThrowIfNullOrEmpty)}(\"foo\") does not throws any {nameof(Exception)}")]
        public void When_Argument_Is_Not_Empty_String_ThrowIfNullOrEmpty_Does_Not_Throw_Exception()
        {
            // Arrange
            string? argument = "foo";

            // Act
#if NET7_0_OR_GREATER
#pragma warning disable CS0618
#endif
            var action = () => ThrowHelperExtensions.ThrowIfNullOrEmpty(argument);
#if NET7_0_OR_GREATER
#pragma warning restore CS0618
#endif

            // Assert
            action.ShouldNotThrow();
        }
    }

    /// <summary>
    /// Unit test for <see cref="ArgumentNullException"/> extension members
    /// </summary>
    [TestClass]
    public sealed class ArgumentNullExceptionTest
    {
        [TestMethod($"{nameof(ArgumentNullException)}.{nameof(ThrowHelperExtensions.ThrowIfNull)}(null) throws {nameof(ArgumentNullException)}")]
        public void When_Argument_Is_Null_ThrowIfNull_Throws_ArgumentNullException()
        {
            // Arrange
            object? argument = null;

            // Act
#if NET6_0_OR_GREATER
#pragma warning disable CS0618
#endif
            var action = () => ThrowHelperExtensions.ThrowIfNull(argument!);
#if NET6_0_OR_GREATER
#pragma warning restore CS0618
#endif

            // Assert
            action.ShouldThrow<ArgumentNullException>().ParamName.ShouldBe(nameof(argument));
        }

        [TestMethod($"{nameof(ArgumentNullException)}.{nameof(ThrowHelperExtensions.ThrowIfNull)}(<not null>) does not throws any {nameof(Exception)}")]
        public void When_Argument_Is_Not_Null_ThrowIfNull_Does_Not_Throws_Exception()
        {
            // Arrange
            object? argument = new();

            // Act
#if NET6_0_OR_GREATER
#pragma warning disable CS0618
#endif
            var action = () => ThrowHelperExtensions.ThrowIfNull(argument);
#if NET6_0_OR_GREATER
#pragma warning restore CS0618
#endif

            // Assert
            action.ShouldNotThrow();
        }

        [TestMethod($"{nameof(ArgumentNullException)}.{nameof(ThrowHelperExtensions.ThrowIfNull)}(<null pointer>) throws {nameof(ArgumentNullException)}")]
        public unsafe void When_Argument_Is_Null_Pointer_ThrowIfNull_Throws_ArgumentNullException()
        {
            // Arrange
            void* argument = null;

            // Act
#if NET7_0_OR_GREATER
#pragma warning disable CS0618
#endif
            var action = () => ThrowHelperExtensions.ThrowIfNull(argument);
#if NET7_0_OR_GREATER
#pragma warning restore CS0618
#endif

            // Assert
            action.ShouldThrow<ArgumentNullException>().ParamName.ShouldBe(nameof(argument));
        }

        [TestMethod($"{nameof(ArgumentNullException)}.{nameof(ThrowHelperExtensions.ThrowIfNull)}(<pointer>) does not throws any {nameof(Exception)}")]
        public unsafe void When_Argument_Is_Pointer_ThrowIfNull_Does_Not_Throw_Exception()
        {
            // Arrange
            int* argument = stackalloc int[1];

            // Act
#if NET7_0_OR_GREATER
#pragma warning disable CS0618
#endif
            var action = () => ThrowHelperExtensions.ThrowIfNull(argument);
#if NET7_0_OR_GREATER
#pragma warning restore CS0618
#endif

            // Assert
            action.ShouldNotThrow();
        }
    }

    /// <summary>
    /// Unit test for <see cref="ObjectDisposedException"/> extension members
    /// </summary>
    [TestClass]
    public sealed class ObjectDisposedExceptionTest
    {
        [TestMethod($"{nameof(ObjectDisposedException)}.{nameof(ThrowHelperExtensions.ThrowIf)}(true, <object or Type>) throws {nameof(ObjectDisposedException)}")]
        public void When_Condition_Is_True_ThrowIf_Throws_ObjectDisposedException()
        {
            // Arrange
            object? argument = new();
            // Act
#if NET7_0_OR_GREATER
#pragma warning disable CS0618
#endif
            var throwIfObject = () => ThrowHelperExtensions.ThrowIf(true, argument);
            var throwIfType = () => ThrowHelperExtensions.ThrowIf(true, typeof(object));
#if NET7_0_OR_GREATER
#pragma warning restore CS0618
#endif
            // Assert
            throwIfObject.ShouldThrow<ObjectDisposedException>();
            throwIfType.ShouldThrow<ObjectDisposedException>();
        }

        [TestMethod($"{nameof(ObjectDisposedException)}.{nameof(ThrowHelperExtensions.ThrowIf)}(false, <object or Type>) does not throws any {nameof(Exception)}")]
        public void When_Condition_Is_False_ThrowIf_Does_Not_Throw_Exception()
        {
            // Arrange
            object? argument = new();
            // Act
#if NET7_0_OR_GREATER
#pragma warning disable CS0618
#endif
            var throwIfObject = () => ThrowHelperExtensions.ThrowIf(false, argument);
            var throwIfType = () => ThrowHelperExtensions.ThrowIf(false, typeof(object));
#if NET7_0_OR_GREATER
#pragma warning restore CS0618
#endif
            // Assert
            throwIfObject.ShouldNotThrow();
            throwIfType.ShouldNotThrow();
        }
    }
}
