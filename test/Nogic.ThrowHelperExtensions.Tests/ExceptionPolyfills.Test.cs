namespace Nogic.ThrowHelperExtensions.Tests;

/// <summary>
/// Unit test class for <see cref="ExceptionPolyfills"/>
/// </summary>
[TestClass]
public sealed class ExceptionPolyfillsTest
{
    #region ArgumentException
    [TestMethod($"{nameof(ArgumentException)}.ThrowIfNullOrEmpty(null) throws {nameof(ArgumentNullException)}")]
    public void When_Argument_Is_Null_ThrowIfNullOrEmpty_Throws_ArgumentNullException()
    {
        // Arrange
        string? argument = null;

        // Act
        var action = () => ArgumentException.ThrowIfNullOrEmpty(argument);

        // Assert
        action.ShouldThrow<ArgumentNullException>().ParamName.ShouldBe(nameof(argument));
    }

    [TestMethod($"{nameof(ArgumentException)}.ThrowIfNullOrEmpty(\"\") throws {nameof(ArgumentException)}")]
    public void When_Argument_Is_Empty_String_ThrowIfNullOrEmpty_Throws_ArgumentException()
    {
        // Arrange
        string? argument = "";

        // Act
        var action = () => ArgumentException.ThrowIfNullOrEmpty(argument);

        // Assert
        action.ShouldThrow<ArgumentException>().ParamName.ShouldBe(nameof(argument));
    }

    [TestMethod($"{nameof(ArgumentException)}.ThrowIfNullOrEmpty(\"foo\") does not throw any {nameof(Exception)}")]
    public void When_Argument_Is_Not_Empty_String_ThrowIfNullOrEmpty_Does_Not_Throw_Exception()
    {
        // Arrange
        string? argument = "foo";

        // Act
        var action = () => ArgumentException.ThrowIfNullOrEmpty(argument);

        // Assert
        action.ShouldNotThrow();
    }

    [TestMethod($"{nameof(ArgumentException)}.ThrowIfNullOrWhiteSpace(null) throws {nameof(ArgumentNullException)}")]
    public void When_Argument_Is_Null_ThrowIfNullOrWhiteSpace_Throws_ArgumentNullException()
    {
        // Arrange
        string? argument = null;

        // Act
        var action = () => ArgumentException.ThrowIfNullOrWhiteSpace(argument);

        // Assert
        action.ShouldThrow<ArgumentNullException>().ParamName.ShouldBe(nameof(argument));
    }

    [TestMethod($"{nameof(ArgumentException)}.ThrowIfNullOrWhiteSpace(<white space>) throws {nameof(ArgumentException)}")]
    [DataRow("")]
    [DataRow(" ")]
    [DataRow("　")]
    public void When_Argument_Is_WhiteSpace_ThrowIfNullOrWhiteSpace_Throws_ArgumentException(string argument)
    {
        // Arrange - Act
        var action = () => ArgumentException.ThrowIfNullOrWhiteSpace(argument);

        // Assert
        action.ShouldThrow<ArgumentException>().ParamName.ShouldBe(nameof(argument));
    }

    [TestMethod($"{nameof(ArgumentException)}.ThrowIfNullOrWhiteSpace(\"foo\") does not throw any {nameof(Exception)}")]
    public void When_Argument_Is_Not_WhiteSpace_ThrowIfNullOrWhiteSpace_Does_Not_Throw_Exception()
    {
        // Arrange
        string? argument = "foo";

        // Act
        var action = () => ArgumentException.ThrowIfNullOrWhiteSpace(argument);

        // Assert
        action.ShouldNotThrow();
    }
    #endregion ArgumentException

    #region ArgumentNullException
    [TestMethod($"{nameof(ArgumentNullException)}.ThrowIfNull(null) throws {nameof(ArgumentNullException)}")]
    public void When_Argument_Is_Null_ThrowIfNull_Throws_ArgumentNullException()
    {
        // Arrange
        object? argument = null;

        // Act
        var action = () => ArgumentNullException.ThrowIfNull(argument!);

        // Assert
        action.ShouldThrow<ArgumentNullException>().ParamName.ShouldBe(nameof(argument));
    }

    [TestMethod($"{nameof(ArgumentNullException)}.ThrowIfNull(<not null>) does not throw any {nameof(Exception)}")]
    public void When_Argument_Is_Not_Null_ThrowIfNull_Does_Not_Throw_Exception()
    {
        // Arrange
        object? argument = new();

        // Act
        var action = () => ArgumentNullException.ThrowIfNull(argument);

        // Assert
        action.ShouldNotThrow();
    }

    [TestMethod($"{nameof(ArgumentNullException)}.ThrowIfNull(<null pointer>) throws {nameof(ArgumentNullException)}")]
    public unsafe void When_Argument_Is_Null_Pointer_ThrowIfNull_Throws_ArgumentNullException()
    {
        // Arrange
        void* argument = null;

        // Act
        var action = () => ArgumentNullException.ThrowIfNull(argument);

        // Assert
        action.ShouldThrow<ArgumentNullException>().ParamName.ShouldBe(nameof(argument));
    }

    [TestMethod($"{nameof(ArgumentNullException)}.ThrowIfNull(<pointer>) does not throw any {nameof(Exception)}")]
    public unsafe void When_Argument_Is_Pointer_ThrowIfNull_Does_Not_Throw_Exception()
    {
        // Arrange
        int* argument = stackalloc int[1];

        // Act
        var action = () => ArgumentNullException.ThrowIfNull(argument);

        // Assert
        action.ShouldNotThrow();
    }
    #endregion ArgumentNullException

    #region ArgumentOutOfRangeException
    [TestMethod($"{nameof(ArgumentOutOfRangeException)}.ThrowIfEqual() throws {nameof(ArgumentOutOfRangeException)}")]
    [DataRow(1, 1)]
    [DataRow("foo", "foo")]
    public void When_Value_Equals_Other_ThrowIfEqual_Throws_ArgumentOutOfRangeException<T>(T value, T other) where T : IEquatable<T>
    {
        // Arrange - Act
        var action = () => ArgumentOutOfRangeException.ThrowIfEqual(value, other);

        // Assert
        action.ShouldThrow<ArgumentOutOfRangeException>().ParamName.ShouldBe(nameof(value));
    }

    [TestMethod($"{nameof(ArgumentOutOfRangeException)}.ThrowIfEqual() does not throw any {nameof(Exception)}")]
    [DataRow(1, 0)]
    [DataRow("foo", "bar")]
    public void When_Value_Not_Equals_Other_ThrowIfEqual_Does_Not_Throw_Exception<T>(T value, T other) where T : IEquatable<T>
    {
        // Arrange - Act
        var action = () => ArgumentOutOfRangeException.ThrowIfEqual(value, other);

        // Assert
        action.ShouldNotThrow();
    }

    [TestMethod($"{nameof(ArgumentOutOfRangeException)}.ThrowIfNotEqual() throws {nameof(ArgumentOutOfRangeException)}")]
    [DataRow(1, 0)]
    [DataRow("foo", "bar")]
    public void When_Value_Not_Equals_Other_ThrowIfNotEqual_Throws_ArgumentOutOfRangeException<T>(T value, T other) where T : IEquatable<T>
    {
        // Arrange - Act
        var action = () => ArgumentOutOfRangeException.ThrowIfNotEqual(value, other);

        // Assert
        action.ShouldThrow<ArgumentOutOfRangeException>().ParamName.ShouldBe(nameof(value));
    }

    [TestMethod($"{nameof(ArgumentOutOfRangeException)}.ThrowIfNotEqual() does not throw any {nameof(Exception)}")]
    [DataRow(1, 1)]
    [DataRow("foo", "foo")]
    public void When_Value_Equals_Other_ThrowIfNotEqual_Does_Not_Throw_Exception<T>(T value, T other) where T : IEquatable<T>
    {
        // Arrange - Act
        var action = () => ArgumentOutOfRangeException.ThrowIfNotEqual(value, other);

        // Assert
        action.ShouldNotThrow();
    }

    [TestMethod($"{nameof(ArgumentOutOfRangeException)}.ThrowIfGreaterThan() throws {nameof(ArgumentOutOfRangeException)}")]
    [DataRow(1, 0)]
    [DataRow("foo", "bar")]
    public void When_Value_Is_Greater_Than_Other_ThrowIfGreaterThan_Throws_ArgumentOutOfRangeException<T>(T value, T other) where T : IComparable<T>
    {
        // Arrange - Act
        var action = () => ArgumentOutOfRangeException.ThrowIfGreaterThan(value, other);

        // Assert
        action.ShouldThrow<ArgumentOutOfRangeException>().ParamName.ShouldBe(nameof(value));
    }

    [TestMethod($"{nameof(ArgumentOutOfRangeException)}.ThrowIfGreaterThan() does not throw any {nameof(Exception)}")]
    [DataRow(0, 1)]
    [DataRow(1, 1)]
    [DataRow("bar", "foo")]
    [DataRow("foo", "foo")]
    public void When_Value_Is_Not_Greater_Than_Other_ThrowIfGreaterThan_Does_Not_Throw_Exception<T>(T value, T other) where T : IComparable<T>
    {
        // Arrange - Act
        var action = () => ArgumentOutOfRangeException.ThrowIfGreaterThan(value, other);

        // Assert
        action.ShouldNotThrow();
    }

    [TestMethod($"{nameof(ArgumentOutOfRangeException)}.ThrowIfGreaterThanOrEqual() throws {nameof(ArgumentOutOfRangeException)}")]
    [DataRow(1, 0)]
    [DataRow(1, 1)]
    [DataRow("foo", "bar")]
    [DataRow("foo", "foo")]
    public void When_Value_Is_Greater_Than_Or_Equal_Other_ThrowIfGreaterThanOrEqual_Throws_ArgumentOutOfRangeException<T>(T value, T other) where T : IComparable<T>
    {
        // Arrange - Act
        var action = () => ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(value, other);

        // Assert
        action.ShouldThrow<ArgumentOutOfRangeException>().ParamName.ShouldBe(nameof(value));
    }

    [TestMethod($"{nameof(ArgumentOutOfRangeException)}.ThrowIfGreaterThanOrEqual() does not throw any {nameof(Exception)}")]
    [DataRow(0, 1)]
    [DataRow("bar", "foo")]
    public void When_Value_Is_Not_Greater_Than_Or_Equal_Other_ThrowIfGreaterThanOrEqual_Does_Not_Throw_Exception<T>(T value, T other) where T : IComparable<T>
    {
        // Arrange - Act
        var action = () => ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(value, other);

        // Assert
        action.ShouldNotThrow();
    }

    [TestMethod($"{nameof(ArgumentOutOfRangeException)}.ThrowIfLessThan() throws {nameof(ArgumentOutOfRangeException)}")]
    [DataRow(0, 1)]
    [DataRow("bar", "foo")]
    public void When_Value_Is_Less_Than_Other_ThrowIfLessThan_Throws_ArgumentOutOfRangeException<T>(T value, T other) where T : IComparable<T>
    {
        // Arrange - Act
        var action = () => ArgumentOutOfRangeException.ThrowIfLessThan(value, other);

        // Assert
        action.ShouldThrow<ArgumentOutOfRangeException>().ParamName.ShouldBe(nameof(value));
    }

    [TestMethod($"{nameof(ArgumentOutOfRangeException)}.ThrowIfLessThan() does not throw any {nameof(Exception)}")]
    [DataRow(1, 0)]
    [DataRow(1, 1)]
    [DataRow("foo", "bar")]
    [DataRow("bar", "bar")]
    public void When_Value_Is_Not_Less_Than_Other_ThrowIfLessThan_Does_Not_Throw_Exception<T>(T value, T other) where T : IComparable<T>
    {
        // Arrange - Act
        var action = () => ArgumentOutOfRangeException.ThrowIfLessThan(value, other);

        // Assert
        action.ShouldNotThrow();
    }

    [TestMethod($"{nameof(ArgumentOutOfRangeException)}.ThrowIfLessThanOrEqual() throws {nameof(ArgumentOutOfRangeException)}")]
    [DataRow(0, 1)]
    [DataRow(1, 1)]
    [DataRow("bar", "foo")]
    [DataRow("bar", "bar")]
    public void When_Value_Is_Less_Than_Or_Equal_Other_ThrowIfLessThanOrEqual_Throws_ArgumentOutOfRangeException<T>(T value, T other) where T : IComparable<T>
    {
        // Arrange - Act
        var action = () => ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(value, other);

        // Assert
        action.ShouldThrow<ArgumentOutOfRangeException>().ParamName.ShouldBe(nameof(value));
    }

    [TestMethod($"{nameof(ArgumentOutOfRangeException)}.ThrowIfLessThanOrEqual() does not throw any {nameof(Exception)}")]
    [DataRow(1, 0)]
    [DataRow("foo", "bar")]
    public void When_Value_Is_Not_Less_Than_Or_Equal_Other_ThrowIfLessThanOrEqual_Does_Not_Throw_Exception<T>(T value, T other) where T : IComparable<T>
    {
        // Arrange - Act
        var action = () => ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(value, other);

        // Assert
        action.ShouldNotThrow();
    }

    [TestMethod($"{nameof(ArgumentOutOfRangeException)}.ThrowIfZero(0) throws {nameof(ArgumentOutOfRangeException)}")]
    public void When_Value_Is_Zero_ThrowIfZero_Throws_ArgumentOutOfRangeException()
    {
        byte byteValue = 0;
        ShouldThrow_ArgumentOutOfRangeException_With_ParamName(() => ArgumentOutOfRangeException.ThrowIfZero(byteValue), nameof(byteValue));
        sbyte sbyteValue = 0;
        ShouldThrow_ArgumentOutOfRangeException_With_ParamName(() => ArgumentOutOfRangeException.ThrowIfZero(sbyteValue), nameof(sbyteValue));
        short shortValue = 0;
        ShouldThrow_ArgumentOutOfRangeException_With_ParamName(() => ArgumentOutOfRangeException.ThrowIfZero(shortValue), nameof(shortValue));
        ushort ushortValue = 0;
        ShouldThrow_ArgumentOutOfRangeException_With_ParamName(() => ArgumentOutOfRangeException.ThrowIfZero(ushortValue), nameof(ushortValue));
        int intValue = 0;
        ShouldThrow_ArgumentOutOfRangeException_With_ParamName(() => ArgumentOutOfRangeException.ThrowIfZero(intValue), nameof(intValue));
        uint uintValue = 0;
        ShouldThrow_ArgumentOutOfRangeException_With_ParamName(() => ArgumentOutOfRangeException.ThrowIfZero(uintValue), nameof(uintValue));
        long longValue = 0;
        ShouldThrow_ArgumentOutOfRangeException_With_ParamName(() => ArgumentOutOfRangeException.ThrowIfZero(longValue), nameof(longValue));
        ulong ulongValue = 0;
        ShouldThrow_ArgumentOutOfRangeException_With_ParamName(() => ArgumentOutOfRangeException.ThrowIfZero(ulongValue), nameof(ulongValue));
        float floatValue = 0;
        ShouldThrow_ArgumentOutOfRangeException_With_ParamName(() => ArgumentOutOfRangeException.ThrowIfZero(floatValue), nameof(floatValue));
        double doubleValue = 0;
        ShouldThrow_ArgumentOutOfRangeException_With_ParamName(() => ArgumentOutOfRangeException.ThrowIfZero(doubleValue), nameof(doubleValue));
        decimal decimalValue = 0;
        ShouldThrow_ArgumentOutOfRangeException_With_ParamName(() => ArgumentOutOfRangeException.ThrowIfZero(decimalValue), nameof(decimalValue));
        nint nintValue = 0;
        ShouldThrow_ArgumentOutOfRangeException_With_ParamName(() => ArgumentOutOfRangeException.ThrowIfZero(nintValue), nameof(nintValue));
        nuint nuintValue = 0;
        ShouldThrow_ArgumentOutOfRangeException_With_ParamName(() => ArgumentOutOfRangeException.ThrowIfZero(nuintValue), nameof(nuintValue));
        char charValue = (char)0;
        ShouldThrow_ArgumentOutOfRangeException_With_ParamName(() => ArgumentOutOfRangeException.ThrowIfZero(charValue), nameof(charValue));

        static void ShouldThrow_ArgumentOutOfRangeException_With_ParamName(Action action, string paramName) =>
            action.ShouldThrow<ArgumentOutOfRangeException>().ParamName.ShouldBe(paramName);
    }

    [TestMethod($"{nameof(ArgumentOutOfRangeException)}.ThrowIfZero(1) does not throw any {nameof(Exception)}")]
    public void When_Value_Is_Not_Zero_ThrowIfZero_Does_Not_Throw()
    {
        byte byteValue = 1;
        ShouldNotThrow(() => ArgumentOutOfRangeException.ThrowIfZero(byteValue));
        sbyte sbyteValue = 1;
        ShouldNotThrow(() => ArgumentOutOfRangeException.ThrowIfZero(sbyteValue));
        short shortValue = 1;
        ShouldNotThrow(() => ArgumentOutOfRangeException.ThrowIfZero(shortValue));
        ushort ushortValue = 1;
        ShouldNotThrow(() => ArgumentOutOfRangeException.ThrowIfZero(ushortValue));
        int intValue = 1;
        ShouldNotThrow(() => ArgumentOutOfRangeException.ThrowIfZero(intValue));
        uint uintValue = 1;
        ShouldNotThrow(() => ArgumentOutOfRangeException.ThrowIfZero(uintValue));
        long longValue = 1;
        ShouldNotThrow(() => ArgumentOutOfRangeException.ThrowIfZero(longValue));
        ulong ulongValue = 1;
        ShouldNotThrow(() => ArgumentOutOfRangeException.ThrowIfZero(ulongValue));
        float floatValue = 1;
        ShouldNotThrow(() => ArgumentOutOfRangeException.ThrowIfZero(floatValue));
        double doubleValue = 1;
        ShouldNotThrow(() => ArgumentOutOfRangeException.ThrowIfZero(doubleValue));
        decimal decimalValue = 1;
        ShouldNotThrow(() => ArgumentOutOfRangeException.ThrowIfZero(decimalValue));
        nint nintValue = 1;
        ShouldNotThrow(() => ArgumentOutOfRangeException.ThrowIfZero(nintValue));
        nuint nuintValue = 1;
        ShouldNotThrow(() => ArgumentOutOfRangeException.ThrowIfZero(nuintValue));
        char charValue = (char)1;
        ShouldNotThrow(() => ArgumentOutOfRangeException.ThrowIfZero(charValue));

        static void ShouldNotThrow(Action action) => action.ShouldNotThrow();
    }

    [TestMethod($"{nameof(ArgumentOutOfRangeException)}.ThrowIfNegative(-1) throws {nameof(ArgumentOutOfRangeException)}")]
    public void When_Value_Is_Negative_ThrowIfNegative_Throws_ArgumentOutOfRangeException()
    {
        sbyte sbyteValue = -1;
        ShouldThrow_ArgumentOutOfRangeException_With_ParamName(() => ArgumentOutOfRangeException.ThrowIfNegative(sbyteValue), nameof(sbyteValue));
        short shortValue = -1;
        ShouldThrow_ArgumentOutOfRangeException_With_ParamName(() => ArgumentOutOfRangeException.ThrowIfNegative(shortValue), nameof(shortValue));
        int intValue = -1;
        ShouldThrow_ArgumentOutOfRangeException_With_ParamName(() => ArgumentOutOfRangeException.ThrowIfNegative(intValue), nameof(intValue));
        long longValue = -1;
        ShouldThrow_ArgumentOutOfRangeException_With_ParamName(() => ArgumentOutOfRangeException.ThrowIfNegative(longValue), nameof(longValue));
        float floatValue = -1;
        ShouldThrow_ArgumentOutOfRangeException_With_ParamName(() => ArgumentOutOfRangeException.ThrowIfNegative(floatValue), nameof(floatValue));
        double doubleValue = -1;
        ShouldThrow_ArgumentOutOfRangeException_With_ParamName(() => ArgumentOutOfRangeException.ThrowIfNegative(doubleValue), nameof(doubleValue));
        decimal decimalValue = -1;
        ShouldThrow_ArgumentOutOfRangeException_With_ParamName(() => ArgumentOutOfRangeException.ThrowIfNegative(decimalValue), nameof(decimalValue));
        nint nintValue = -1;
        ShouldThrow_ArgumentOutOfRangeException_With_ParamName(() => ArgumentOutOfRangeException.ThrowIfNegative(nintValue), nameof(nintValue));

        static void ShouldThrow_ArgumentOutOfRangeException_With_ParamName(Action action, string paramName) =>
            action.ShouldThrow<ArgumentOutOfRangeException>().ParamName.ShouldBe(paramName);
    }

    [TestMethod($"{nameof(ArgumentOutOfRangeException)}.ThrowIfNegative(0 or 1) does not throw any {nameof(Exception)}")]
    public void When_Value_Is_Not_Negative_ThrowIfNegative_Does_Not_Throw()
    {
        sbyte sbyteValue0 = 0;
        ShouldNotThrow(() => ArgumentOutOfRangeException.ThrowIfNegative(sbyteValue0));
        sbyte sbyteValue1 = 1;
        ShouldNotThrow(() => ArgumentOutOfRangeException.ThrowIfNegative(sbyteValue1));
        short shortValue0 = 0;
        ShouldNotThrow(() => ArgumentOutOfRangeException.ThrowIfNegative(shortValue0));
        short shortValue1 = 1;
        ShouldNotThrow(() => ArgumentOutOfRangeException.ThrowIfNegative(shortValue1));
        int intValue0 = 0;
        ShouldNotThrow(() => ArgumentOutOfRangeException.ThrowIfNegative(intValue0));
        int intValue1 = 1;
        ShouldNotThrow(() => ArgumentOutOfRangeException.ThrowIfNegative(intValue1));
        long longValue0 = 0;
        ShouldNotThrow(() => ArgumentOutOfRangeException.ThrowIfNegative(longValue0));
        long longValue1 = 1;
        ShouldNotThrow(() => ArgumentOutOfRangeException.ThrowIfNegative(longValue1));
        float floatValue0 = 0;
        ShouldNotThrow(() => ArgumentOutOfRangeException.ThrowIfNegative(floatValue0));
        float floatValue1 = 1;
        ShouldNotThrow(() => ArgumentOutOfRangeException.ThrowIfNegative(floatValue1));
        double doubleValue0 = 0;
        ShouldNotThrow(() => ArgumentOutOfRangeException.ThrowIfNegative(doubleValue0));
        double doubleValue1 = 1;
        ShouldNotThrow(() => ArgumentOutOfRangeException.ThrowIfNegative(doubleValue1));
        decimal decimalValue0 = 0;
        ShouldNotThrow(() => ArgumentOutOfRangeException.ThrowIfNegative(decimalValue0));
        decimal decimalValue1 = 1;
        ShouldNotThrow(() => ArgumentOutOfRangeException.ThrowIfNegative(decimalValue1));
        nint nintValue0 = 0;
        ShouldNotThrow(() => ArgumentOutOfRangeException.ThrowIfNegative(nintValue0));
        nint nintValue1 = 1;
        ShouldNotThrow(() => ArgumentOutOfRangeException.ThrowIfNegative(nintValue1));

        static void ShouldNotThrow(Action action) => action.ShouldNotThrow();
    }

    [TestMethod($"{nameof(ArgumentOutOfRangeException)}.ThrowIfNegativeOrZero(0 or -1) throws {nameof(ArgumentOutOfRangeException)}")]
    public void When_Value_Is_Negative_Or_Zero_ThrowIfNegativeOrZero_Throws_ArgumentOutOfRangeException()
    {
        sbyte sbyteValue0 = 0;
        ShouldThrow_ArgumentOutOfRangeException_With_ParamName(() => ArgumentOutOfRangeException.ThrowIfNegativeOrZero(sbyteValue0), nameof(sbyteValue0));
        sbyte sbyteValue = -1;
        ShouldThrow_ArgumentOutOfRangeException_With_ParamName(() => ArgumentOutOfRangeException.ThrowIfNegativeOrZero(sbyteValue), nameof(sbyteValue));
        short shortValue0 = 0;
        ShouldThrow_ArgumentOutOfRangeException_With_ParamName(() => ArgumentOutOfRangeException.ThrowIfNegativeOrZero(shortValue0), nameof(shortValue0));
        short shortValue = -1;
        ShouldThrow_ArgumentOutOfRangeException_With_ParamName(() => ArgumentOutOfRangeException.ThrowIfNegativeOrZero(shortValue), nameof(shortValue));
        int intValue0 = 0;
        ShouldThrow_ArgumentOutOfRangeException_With_ParamName(() => ArgumentOutOfRangeException.ThrowIfNegativeOrZero(intValue0), nameof(intValue0));
        int intValue = -1;
        ShouldThrow_ArgumentOutOfRangeException_With_ParamName(() => ArgumentOutOfRangeException.ThrowIfNegativeOrZero(intValue), nameof(intValue));
        long longValue0 = 0;
        ShouldThrow_ArgumentOutOfRangeException_With_ParamName(() => ArgumentOutOfRangeException.ThrowIfNegativeOrZero(longValue0), nameof(longValue0));
        long longValue = -1;
        ShouldThrow_ArgumentOutOfRangeException_With_ParamName(() => ArgumentOutOfRangeException.ThrowIfNegativeOrZero(longValue), nameof(longValue));
        float floatValue0 = 0;
        ShouldThrow_ArgumentOutOfRangeException_With_ParamName(() => ArgumentOutOfRangeException.ThrowIfNegativeOrZero(floatValue0), nameof(floatValue0));
        float floatValue = -1;
        ShouldThrow_ArgumentOutOfRangeException_With_ParamName(() => ArgumentOutOfRangeException.ThrowIfNegativeOrZero(floatValue), nameof(floatValue));
        double doubleValue0 = 0;
        ShouldThrow_ArgumentOutOfRangeException_With_ParamName(() => ArgumentOutOfRangeException.ThrowIfNegativeOrZero(doubleValue0), nameof(doubleValue0));
        double doubleValue = -1;
        ShouldThrow_ArgumentOutOfRangeException_With_ParamName(() => ArgumentOutOfRangeException.ThrowIfNegativeOrZero(doubleValue), nameof(doubleValue));
        decimal decimalValue0 = 0;
        ShouldThrow_ArgumentOutOfRangeException_With_ParamName(() => ArgumentOutOfRangeException.ThrowIfNegativeOrZero(decimalValue0), nameof(decimalValue0));
        decimal decimalValue = -1;
        ShouldThrow_ArgumentOutOfRangeException_With_ParamName(() => ArgumentOutOfRangeException.ThrowIfNegativeOrZero(decimalValue), nameof(decimalValue));
        nint nintValue0 = 0;
        ShouldThrow_ArgumentOutOfRangeException_With_ParamName(() => ArgumentOutOfRangeException.ThrowIfNegativeOrZero(nintValue0), nameof(nintValue0));
        nint nintValue = -1;
        ShouldThrow_ArgumentOutOfRangeException_With_ParamName(() => ArgumentOutOfRangeException.ThrowIfNegativeOrZero(nintValue), nameof(nintValue));

        static void ShouldThrow_ArgumentOutOfRangeException_With_ParamName(Action action, string paramName) =>
            action.ShouldThrow<ArgumentOutOfRangeException>().ParamName.ShouldBe(paramName);
    }

    [TestMethod($"{nameof(ArgumentOutOfRangeException)}.ThrowIfNegativeOrZero(1) does not throw any {nameof(Exception)}")]
    public void When_Value_Is_Not_Negative_Or_Zero_ThrowIfNegativeOrZero_Does_Not_Throw()
    {
        sbyte sbyteValue1 = 1;
        ShouldNotThrow(() => ArgumentOutOfRangeException.ThrowIfNegativeOrZero(sbyteValue1));
        short shortValue1 = 1;
        ShouldNotThrow(() => ArgumentOutOfRangeException.ThrowIfNegativeOrZero(shortValue1));
        int intValue1 = 1;
        ShouldNotThrow(() => ArgumentOutOfRangeException.ThrowIfNegativeOrZero(intValue1));
        long longValue1 = 1;
        ShouldNotThrow(() => ArgumentOutOfRangeException.ThrowIfNegativeOrZero(longValue1));
        float floatValue1 = 1;
        ShouldNotThrow(() => ArgumentOutOfRangeException.ThrowIfNegativeOrZero(floatValue1));
        double doubleValue1 = 1;
        ShouldNotThrow(() => ArgumentOutOfRangeException.ThrowIfNegativeOrZero(doubleValue1));
        decimal decimalValue1 = 1;
        ShouldNotThrow(() => ArgumentOutOfRangeException.ThrowIfNegativeOrZero(decimalValue1));
        nint nintValue1 = 1;
        ShouldNotThrow(() => ArgumentOutOfRangeException.ThrowIfNegativeOrZero(nintValue1));

        static void ShouldNotThrow(Action action) => action.ShouldNotThrow();
    }
    #endregion ArgumentOutOfRangeException

    #region ObjectDisposedException
    [TestMethod($"{nameof(ObjectDisposedException)}.ThrowIf(true, <object or Type>) throws {nameof(ObjectDisposedException)}")]
    public void When_Condition_Is_True_ThrowIf_Throws_ObjectDisposedException()
    {
        // Arrange
        object? argument = new();

        // Act
        var throwIfObject = () => ObjectDisposedException.ThrowIf(true, argument);
        var throwIfType = () => ObjectDisposedException.ThrowIf(true, typeof(object));

        // Assert
        throwIfObject.ShouldThrow<ObjectDisposedException>();
        throwIfType.ShouldThrow<ObjectDisposedException>();
    }

    [TestMethod($"{nameof(ObjectDisposedException)}.ThrowIf(false, <object or Type>) does not throw any {nameof(Exception)}")]
    public void When_Condition_Is_False_ThrowIf_Does_Not_Throw_Exception()
    {
        // Arrange
        object? argument = new();

        // Act
        var throwIfObject = () => ObjectDisposedException.ThrowIf(false, argument);
        var throwIfType = () => ObjectDisposedException.ThrowIf(false, typeof(object));

        // Assert
        throwIfObject.ShouldNotThrow();
        throwIfType.ShouldNotThrow();
    }
    #endregion ObjectDisposedException
}
