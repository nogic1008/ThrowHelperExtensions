namespace Nogic.ThrowHelperExtensions.Tests;

/// <summary>
/// Unit test class for <see cref="ArgumentNullExceptionExtensions"/>
/// </summary>
[TestClass]
public sealed class ArgumentNullExceptionExtensionsTest
{
    [TestMethod($"{nameof(ArgumentNullException)}.{nameof(ArgumentNullExceptionExtensions.ThrowIfNull)}(null) throws {nameof(ArgumentNullException)}")]
    public void When_Argument_Is_Null_ThrowIfNull_Throws_ArgumentNullException()
    {
        // Arrange
        object? argument = null;

        // Act
        var action = () => ArgumentNullExceptionExtensions.ThrowIfNull(argument!);

        // Assert
        action.ShouldThrow<ArgumentNullException>().ParamName.ShouldBe(nameof(argument));
    }

    [TestMethod($"{nameof(ArgumentNullException)}.{nameof(ArgumentNullExceptionExtensions.ThrowIfNull)}(<not null>) does not throw any {nameof(Exception)}")]
    public void When_Argument_Is_Not_Null_ThrowIfNull_Does_Not_Throw_Exception()
    {
        // Arrange
        object? argument = new();

        // Act
        var action = () => ArgumentNullExceptionExtensions.ThrowIfNull(argument);

        // Assert
        action.ShouldNotThrow();
    }

    [TestMethod($"{nameof(ArgumentNullException)}.{nameof(ArgumentNullExceptionExtensions.ThrowIfNull)}(<null pointer>) throws {nameof(ArgumentNullException)}")]
    public unsafe void When_Argument_Is_Null_Pointer_ThrowIfNull_Throws_ArgumentNullException()
    {
        // Arrange
        void* argument = null;

        // Act
        var action = () => ArgumentNullExceptionExtensions.ThrowIfNull(argument);

        // Assert
        action.ShouldThrow<ArgumentNullException>().ParamName.ShouldBe(nameof(argument));
    }

    [TestMethod($"{nameof(ArgumentNullException)}.{nameof(ArgumentNullExceptionExtensions.ThrowIfNull)}(<pointer>) does not throw any {nameof(Exception)}")]
    public unsafe void When_Argument_Is_Pointer_ThrowIfNull_Does_Not_Throw_Exception()
    {
        // Arrange
        int* argument = stackalloc int[1];

        // Act
        var action = () => ArgumentNullExceptionExtensions.ThrowIfNull(argument);

        // Assert
        action.ShouldNotThrow();
    }
}
