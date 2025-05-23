using System.Numerics;

namespace Nogic.ThrowHelperExtensions.Tests;

/// <summary>
/// Unit test class for <see cref="ArgumentExceptionExtensions"/>
/// </summary>
[TestClass]
public sealed class ArgumentExceptionExtensionsTest
{
    [TestMethod($"{nameof(ArgumentException)}.{nameof(ArgumentExceptionExtensions.ThrowIfNullOrEmpty)}(null) throws {nameof(ArgumentNullException)}")]
    public void When_Argument_Is_Null_ThrowIfNullOrEmpty_Throws_ArgumentNullException()
    {
        // Arrange
        string? argument = null;

        // Act
        var action = () => ArgumentExceptionExtensions.ThrowIfNullOrEmpty(argument!);

        // Assert
        action.ShouldThrow<ArgumentNullException>().ParamName.ShouldBe(nameof(argument));
    }

    [TestMethod($"{nameof(ArgumentException)}.{nameof(ArgumentExceptionExtensions.ThrowIfNullOrEmpty)}(\"\") throws {nameof(ArgumentException)}")]
    public void When_Argument_Is_Empty_String_ThrowIfNullOrEmpty_Throws_ArgumentException()
    {
        // Arrange
        string? argument = "";

        // Act
        var action = () => ArgumentExceptionExtensions.ThrowIfNullOrEmpty(argument);

        // Assert
        action.ShouldThrow<ArgumentException>().ParamName.ShouldBe(nameof(argument));
    }

    [TestMethod($"{nameof(ArgumentException)}.{nameof(ArgumentExceptionExtensions.ThrowIfNullOrEmpty)}(\"foo\") does not throw any {nameof(Exception)}")]
    public void When_Argument_Is_Not_Empty_String_ThrowIfNullOrEmpty_Does_Not_Throw_Exception()
    {
        // Arrange
        string? argument = "foo";

        // Act
        var action = () => ArgumentExceptionExtensions.ThrowIfNullOrEmpty(argument);

        // Assert
        action.ShouldNotThrow();
    }

    [TestMethod($"{nameof(ArgumentException)}.{nameof(ArgumentExceptionExtensions.ThrowIfNullOrWhiteSpace)}(null) throws {nameof(ArgumentNullException)}")]
    public void When_Argument_Is_Null_ThrowIfNullOrWhiteSpace_Throws_ArgumentNullException()
    {
        // Arrange
        string? argument = null;

        // Act
        var action = () => ArgumentExceptionExtensions.ThrowIfNullOrWhiteSpace(argument!);

        // Assert
        action.ShouldThrow<ArgumentNullException>().ParamName.ShouldBe(nameof(argument));
    }

    [TestMethod($"{nameof(ArgumentException)}.{nameof(ArgumentExceptionExtensions.ThrowIfNullOrWhiteSpace)}(<white space>) throws {nameof(ArgumentException)}")]
    [DataRow("")]
    [DataRow(" ")]
    [DataRow("　")]
    public void When_Argument_Is_WhiteSpace_ThrowIfNullOrWhiteSpace_Throws_ArgumentException(string argument)
    {
        // Arrange - Act
        var action = () => ArgumentExceptionExtensions.ThrowIfNullOrWhiteSpace(argument);

        // Assert
        action.ShouldThrow<ArgumentException>().ParamName.ShouldBe(nameof(argument));
    }

    [TestMethod($"{nameof(ArgumentException)}.{nameof(ArgumentExceptionExtensions.ThrowIfNullOrWhiteSpace)}(\"foo\") does not throw any {nameof(Exception)}")]
    public void When_Argument_Is_Not_WhiteSpace_ThrowIfNullOrWhiteSpace_Does_Not_Throw_Exception()
    {
        // Arrange
        string? argument = "foo";

        // Act
        var action = () => ArgumentExceptionExtensions.ThrowIfNullOrWhiteSpace(argument);

        // Assert
        action.ShouldNotThrow();
    }
}
