namespace Nogic.ThrowHelperExtensions.Tests;

public sealed class ThrowHelperExtensionsTest
{
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
            var action = () => ThrowHelperExtensions.ThrowIfNull(argument);
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
    }
}
