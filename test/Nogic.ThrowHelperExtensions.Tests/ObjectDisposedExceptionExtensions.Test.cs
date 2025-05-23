namespace Nogic.ThrowHelperExtensions.Tests;

/// <summary>
/// Unit test class for <see cref="ObjectDisposedExceptionExtensions"/>
/// </summary>
[TestClass]
public sealed class ObjectDisposedExceptionExtensionsTest
{
    [TestMethod($"{nameof(ObjectDisposedException)}.{nameof(ObjectDisposedExceptionExtensions.ThrowIf)}(true, <object or Type>) throws {nameof(ObjectDisposedException)}")]
    public void When_Condition_Is_True_ThrowIf_Throws_ObjectDisposedException()
    {
        // Arrange
        object? argument = new();

        // Act
        var throwIfObject = () => ObjectDisposedExceptionExtensions.ThrowIf(true, argument);
        var throwIfType = () => ObjectDisposedExceptionExtensions.ThrowIf(true, typeof(object));

        // Assert
        throwIfObject.ShouldThrow<ObjectDisposedException>();
        throwIfType.ShouldThrow<ObjectDisposedException>();
    }

    [TestMethod($"{nameof(ObjectDisposedException)}.{nameof(ObjectDisposedExceptionExtensions.ThrowIf)}(false, <object or Type>) does not throw any {nameof(Exception)}")]
    public void When_Condition_Is_False_ThrowIf_Does_Not_Throw_Exception()
    {
        // Arrange
        object? argument = new();

        // Act
        var throwIfObject = () => ObjectDisposedExceptionExtensions.ThrowIf(false, argument);
        var throwIfType = () => ObjectDisposedExceptionExtensions.ThrowIf(false, typeof(object));

        // Assert
        throwIfObject.ShouldNotThrow();
        throwIfType.ShouldNotThrow();
    }
}
