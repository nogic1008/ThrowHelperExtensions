namespace Nogic.ThrowHelperExtensions.Tests;

/// <summary>
/// Unit test class for <see cref="ArgumentOutOfRangeExceptionExtensions"/>
/// </summary>
public sealed class ArgumentOutOfRangeExceptionExtensionsTest
{
    [TestMethod($"{nameof(ArgumentOutOfRangeException)}.{nameof(ArgumentOutOfRangeExceptionExtensions.ThrowIfEqual)}() throws {nameof(ArgumentOutOfRangeException)}")]
    [DataRow(1, 1)]
    [DataRow("foo", "foo")]
    public void When_Value_Equals_Other_ThrowIfEqual_Throws_ArgumentOutOfRangeException<T>(T value, T other) where T : IEquatable<T>?
    {
        // Arrange - Act
        var action = () => ArgumentOutOfRangeExceptionExtensions.ThrowIfEqual(value, other);

        // Assert
        action.ShouldThrow<ArgumentOutOfRangeException>().ParamName.ShouldBe(nameof(value));
    }

    [TestMethod($"{nameof(ArgumentOutOfRangeException)}.{nameof(ArgumentOutOfRangeExceptionExtensions.ThrowIfEqual)}() does not throw any {nameof(Exception)}")]
    [DataRow(1, 0)]
    [DataRow("foo", "bar")]
    public void When_Value_Not_Equals_Other_ThrowIfEqual_Does_Not_Throw_Exception<T>(T value, T other) where T : IEquatable<T>?
    {
        // Arrange - Act
        var action = () => ArgumentOutOfRangeExceptionExtensions.ThrowIfEqual(value, other);

        // Assert
        action.ShouldNotThrow();
    }

    [TestMethod($"{nameof(ArgumentOutOfRangeException)}.{nameof(ArgumentOutOfRangeExceptionExtensions.ThrowIfNotEqual)}() throws {nameof(ArgumentOutOfRangeException)}")]
    [DataRow(1, 0)]
    [DataRow("foo", "bar")]
    public void When_Value_Not_Equals_Other_ThrowIfNotEqual_Throws_ArgumentOutOfRangeException<T>(T value, T other) where T : IEquatable<T>?
    {
        // Arrange - Act
        var action = () => ArgumentOutOfRangeExceptionExtensions.ThrowIfNotEqual(value, other);

        // Assert
        action.ShouldThrow<ArgumentOutOfRangeException>().ParamName.ShouldBe(nameof(value));
    }

    [TestMethod($"{nameof(ArgumentOutOfRangeException)}.{nameof(ArgumentOutOfRangeExceptionExtensions.ThrowIfNotEqual)}() does not throw any {nameof(Exception)}")]
    [DataRow(1, 1)]
    [DataRow("foo", "foo")]
    public void When_Value_Equals_Other_ThrowIfNotEqual_Does_Not_Throw_Exception<T>(T value, T other) where T : IEquatable<T>?
    {
        // Arrange - Act
        var action = () => ArgumentOutOfRangeExceptionExtensions.ThrowIfNotEqual(value, other);

        // Assert
        action.ShouldNotThrow();
    }

    [TestMethod($"{nameof(ArgumentOutOfRangeException)}.{nameof(ArgumentOutOfRangeExceptionExtensions.ThrowIfGreaterThan)}() throws {nameof(ArgumentOutOfRangeException)}")]
    [DataRow(1, 0)]
    [DataRow("foo", "bar")]
    public void When_Value_Is_Greater_Than_Other_ThrowIfGreaterThan_Throws_ArgumentOutOfRangeException<T>(T value, T other) where T : IComparable<T>
    {
        // Arrange - Act
        var action = () => ArgumentOutOfRangeExceptionExtensions.ThrowIfGreaterThan(value, other);

        // Assert
        action.ShouldThrow<ArgumentOutOfRangeException>().ParamName.ShouldBe(nameof(value));
    }

    [TestMethod($"{nameof(ArgumentOutOfRangeException)}.{nameof(ArgumentOutOfRangeExceptionExtensions.ThrowIfGreaterThan)}() does not throw any {nameof(Exception)}")]
    [DataRow(0, 1)]
    [DataRow(1, 1)]
    [DataRow("bar", "foo")]
    [DataRow("foo", "foo")]
    public void When_Value_Is_Not_Greater_Than_Other_ThrowIfGreaterThan_Does_Not_Throw_Exception<T>(T value, T other) where T : IComparable<T>
    {
        // Arrange - Act
        var action = () => ArgumentOutOfRangeExceptionExtensions.ThrowIfGreaterThan(value, other);

        // Assert
        action.ShouldNotThrow();
    }

    [TestMethod($"{nameof(ArgumentOutOfRangeException)}.{nameof(ArgumentOutOfRangeExceptionExtensions.ThrowIfGreaterThanOrEqual)}() throws {nameof(ArgumentOutOfRangeException)}")]
    [DataRow(1, 0)]
    [DataRow(1, 1)]
    [DataRow("foo", "bar")]
    [DataRow("foo", "foo")]
    public void When_Value_Is_Greater_Than_Or_Equal_Other_ThrowIfGreaterThanOrEqual_Throws_ArgumentOutOfRangeException<T>(T value, T other) where T : IComparable<T>
    {
        // Arrange - Act
        var action = () => ArgumentOutOfRangeExceptionExtensions.ThrowIfGreaterThanOrEqual(value, other);

        // Assert
        action.ShouldThrow<ArgumentOutOfRangeException>().ParamName.ShouldBe(nameof(value));
    }

    [TestMethod($"{nameof(ArgumentOutOfRangeException)}.{nameof(ArgumentOutOfRangeExceptionExtensions.ThrowIfGreaterThanOrEqual)}() does not throw any {nameof(Exception)}")]
    [DataRow(0, 1)]
    [DataRow("bar", "foo")]
    public void When_Value_Is_Not_Greater_Than_Or_Equal_Other_ThrowIfGreaterThanOrEqual_Does_Not_Throw_Exception<T>(T value, T other) where T : IComparable<T>
    {
        // Arrange - Act
        var action = () => ArgumentOutOfRangeExceptionExtensions.ThrowIfGreaterThanOrEqual(value, other);

        // Assert
        action.ShouldNotThrow();
    }

    [TestMethod($"{nameof(ArgumentOutOfRangeException)}.{nameof(ArgumentOutOfRangeExceptionExtensions.ThrowIfLessThan)}() throws {nameof(ArgumentOutOfRangeException)}")]
    [DataRow(0, 1)]
    [DataRow("bar", "foo")]
    public void When_Value_Is_Less_Than_Other_ThrowIfLessThan_Throws_ArgumentOutOfRangeException<T>(T value, T other) where T : IComparable<T>
    {
        // Arrange - Act
        var action = () => ArgumentOutOfRangeExceptionExtensions.ThrowIfLessThan(value, other);

        // Assert
        action.ShouldThrow<ArgumentOutOfRangeException>().ParamName.ShouldBe(nameof(value));
    }

    [TestMethod($"{nameof(ArgumentOutOfRangeException)}.{nameof(ArgumentOutOfRangeExceptionExtensions.ThrowIfLessThan)}() does not throw any {nameof(Exception)}")]
    [DataRow(1, 0)]
    [DataRow(1, 1)]
    [DataRow("foo", "bar")]
    [DataRow("bar", "bar")]
    public void When_Value_Is_Not_Less_Than_Other_ThrowIfLessThan_Does_Not_Throw_Exception<T>(T value, T other) where T : IComparable<T>
    {
        // Arrange - Act
        var action = () => ArgumentOutOfRangeExceptionExtensions.ThrowIfLessThan(value, other);

        // Assert
        action.ShouldNotThrow();
    }

    [TestMethod($"{nameof(ArgumentOutOfRangeException)}.{nameof(ArgumentOutOfRangeExceptionExtensions.ThrowIfLessThanOrEqual)}() throws {nameof(ArgumentOutOfRangeException)}")]
    [DataRow(0, 1)]
    [DataRow(1, 1)]
    [DataRow("bar", "foo")]
    [DataRow("bar", "bar")]
    public void When_Value_Is_Less_Than_Or_Equal_Other_ThrowIfLessThanOrEqual_Throws_ArgumentOutOfRangeException<T>(T value, T other) where T : IComparable<T>
    {
        // Arrange - Act
        var action = () => ArgumentOutOfRangeExceptionExtensions.ThrowIfLessThanOrEqual(value, other);

        // Assert
        action.ShouldThrow<ArgumentOutOfRangeException>().ParamName.ShouldBe(nameof(value));
    }

    [TestMethod($"{nameof(ArgumentOutOfRangeException)}.{nameof(ArgumentOutOfRangeExceptionExtensions.ThrowIfLessThanOrEqual)}() does not throw any {nameof(Exception)}")]
    [DataRow(1, 0)]
    [DataRow("foo", "bar")]
    public void When_Value_Is_Not_Less_Than_Or_Equal_Other_ThrowIfLessThanOrEqual_Does_Not_Throw_Exception<T>(T value, T other) where T : IComparable<T>
    {
        // Arrange - Act
        var action = () => ArgumentOutOfRangeExceptionExtensions.ThrowIfLessThanOrEqual(value, other);

        // Assert
        action.ShouldNotThrow();
    }

    [TestMethod($"{nameof(ArgumentOutOfRangeException)}.{nameof(ArgumentOutOfRangeExceptionExtensions.ThrowIfZero)}(0) throws {nameof(ArgumentOutOfRangeException)}")]
    public void When_Value_Is_Zero_ThrowIfZero_Throws_ArgumentOutOfRangeException()
    {
        byte byteValue = 0;
        ShouldThrow_ArgumentOutOfRangeException_With_ParamName(() => ArgumentOutOfRangeExceptionExtensions.ThrowIfZero(byteValue), nameof(byteValue));
        sbyte sbyteValue = 0;
        ShouldThrow_ArgumentOutOfRangeException_With_ParamName(() => ArgumentOutOfRangeExceptionExtensions.ThrowIfZero(sbyteValue), nameof(sbyteValue));
        short shortValue = 0;
        ShouldThrow_ArgumentOutOfRangeException_With_ParamName(() => ArgumentOutOfRangeExceptionExtensions.ThrowIfZero(shortValue), nameof(shortValue));
        ushort ushortValue = 0;
        ShouldThrow_ArgumentOutOfRangeException_With_ParamName(() => ArgumentOutOfRangeExceptionExtensions.ThrowIfZero(ushortValue), nameof(ushortValue));
        int intValue = 0;
        ShouldThrow_ArgumentOutOfRangeException_With_ParamName(() => ArgumentOutOfRangeExceptionExtensions.ThrowIfZero(intValue), nameof(intValue));
        uint uintValue = 0;
        ShouldThrow_ArgumentOutOfRangeException_With_ParamName(() => ArgumentOutOfRangeExceptionExtensions.ThrowIfZero(uintValue), nameof(uintValue));
        long longValue = 0;
        ShouldThrow_ArgumentOutOfRangeException_With_ParamName(() => ArgumentOutOfRangeExceptionExtensions.ThrowIfZero(longValue), nameof(longValue));
        ulong ulongValue = 0;
        ShouldThrow_ArgumentOutOfRangeException_With_ParamName(() => ArgumentOutOfRangeExceptionExtensions.ThrowIfZero(ulongValue), nameof(ulongValue));
        float floatValue = 0;
        ShouldThrow_ArgumentOutOfRangeException_With_ParamName(() => ArgumentOutOfRangeExceptionExtensions.ThrowIfZero(floatValue), nameof(floatValue));
        double doubleValue = 0;
        ShouldThrow_ArgumentOutOfRangeException_With_ParamName(() => ArgumentOutOfRangeExceptionExtensions.ThrowIfZero(doubleValue), nameof(doubleValue));
        decimal decimalValue = 0;
        ShouldThrow_ArgumentOutOfRangeException_With_ParamName(() => ArgumentOutOfRangeExceptionExtensions.ThrowIfZero(decimalValue), nameof(decimalValue));
        nint nintValue = 0;
        ShouldThrow_ArgumentOutOfRangeException_With_ParamName(() => ArgumentOutOfRangeExceptionExtensions.ThrowIfZero(nintValue), nameof(nintValue));
        nuint nuintValue = 0;
        ShouldThrow_ArgumentOutOfRangeException_With_ParamName(() => ArgumentOutOfRangeExceptionExtensions.ThrowIfZero(nuintValue), nameof(nuintValue));
        char charValue = (char)0;
        ShouldThrow_ArgumentOutOfRangeException_With_ParamName(() => ArgumentOutOfRangeExceptionExtensions.ThrowIfZero(charValue), nameof(charValue));

        static void ShouldThrow_ArgumentOutOfRangeException_With_ParamName(Action action, string paramName) =>
            action.ShouldThrow<ArgumentOutOfRangeException>().ParamName.ShouldBe(paramName);
    }

    [TestMethod($"{nameof(ArgumentOutOfRangeException)}.{nameof(ArgumentOutOfRangeExceptionExtensions.ThrowIfZero)}(1) does not throw any {nameof(Exception)}")]
    public void When_Value_Is_Not_Zero_ThrowIfZero_Does_Not_Throw()
    {
        byte byteValue = 1;
        ShouldNotThrow(() => ArgumentOutOfRangeExceptionExtensions.ThrowIfZero(byteValue));
        sbyte sbyteValue = 1;
        ShouldNotThrow(() => ArgumentOutOfRangeExceptionExtensions.ThrowIfZero(sbyteValue));
        short shortValue = 1;
        ShouldNotThrow(() => ArgumentOutOfRangeExceptionExtensions.ThrowIfZero(shortValue));
        ushort ushortValue = 1;
        ShouldNotThrow(() => ArgumentOutOfRangeExceptionExtensions.ThrowIfZero(ushortValue));
        int intValue = 1;
        ShouldNotThrow(() => ArgumentOutOfRangeExceptionExtensions.ThrowIfZero(intValue));
        uint uintValue = 1;
        ShouldNotThrow(() => ArgumentOutOfRangeExceptionExtensions.ThrowIfZero(uintValue));
        long longValue = 1;
        ShouldNotThrow(() => ArgumentOutOfRangeExceptionExtensions.ThrowIfZero(longValue));
        ulong ulongValue = 1;
        ShouldNotThrow(() => ArgumentOutOfRangeExceptionExtensions.ThrowIfZero(ulongValue));
        float floatValue = 1;
        ShouldNotThrow(() => ArgumentOutOfRangeExceptionExtensions.ThrowIfZero(floatValue));
        double doubleValue = 1;
        ShouldNotThrow(() => ArgumentOutOfRangeExceptionExtensions.ThrowIfZero(doubleValue));
        decimal decimalValue = 1;
        ShouldNotThrow(() => ArgumentOutOfRangeExceptionExtensions.ThrowIfZero(decimalValue));
        nint nintValue = 1;
        ShouldNotThrow(() => ArgumentOutOfRangeExceptionExtensions.ThrowIfZero(nintValue));
        nuint nuintValue = 1;
        ShouldNotThrow(() => ArgumentOutOfRangeExceptionExtensions.ThrowIfZero(nuintValue));
        char charValue = (char)1;
        ShouldNotThrow(() => ArgumentOutOfRangeExceptionExtensions.ThrowIfZero(charValue));

        static void ShouldNotThrow(Action action) => action.ShouldNotThrow();
    }

    [TestMethod($"{nameof(ArgumentOutOfRangeException)}.{nameof(ArgumentOutOfRangeExceptionExtensions.ThrowIfNegative)}(-1) throws {nameof(ArgumentOutOfRangeException)}")]
    public void When_Value_Is_Negative_ThrowIfNegative_Throws_ArgumentOutOfRangeException()
    {
        sbyte sbyteValue = -1;
        ShouldThrow_ArgumentOutOfRangeException_With_ParamName(() => ArgumentOutOfRangeExceptionExtensions.ThrowIfNegative(sbyteValue), nameof(sbyteValue));
        short shortValue = -1;
        ShouldThrow_ArgumentOutOfRangeException_With_ParamName(() => ArgumentOutOfRangeExceptionExtensions.ThrowIfNegative(shortValue), nameof(shortValue));
        int intValue = -1;
        ShouldThrow_ArgumentOutOfRangeException_With_ParamName(() => ArgumentOutOfRangeExceptionExtensions.ThrowIfNegative(intValue), nameof(intValue));
        long longValue = -1;
        ShouldThrow_ArgumentOutOfRangeException_With_ParamName(() => ArgumentOutOfRangeExceptionExtensions.ThrowIfNegative(longValue), nameof(longValue));
        float floatValue = -1;
        ShouldThrow_ArgumentOutOfRangeException_With_ParamName(() => ArgumentOutOfRangeExceptionExtensions.ThrowIfNegative(floatValue), nameof(floatValue));
        double doubleValue = -1;
        ShouldThrow_ArgumentOutOfRangeException_With_ParamName(() => ArgumentOutOfRangeExceptionExtensions.ThrowIfNegative(doubleValue), nameof(doubleValue));
        decimal decimalValue = -1;
        ShouldThrow_ArgumentOutOfRangeException_With_ParamName(() => ArgumentOutOfRangeExceptionExtensions.ThrowIfNegative(decimalValue), nameof(decimalValue));
        nint nintValue = -1;
        ShouldThrow_ArgumentOutOfRangeException_With_ParamName(() => ArgumentOutOfRangeExceptionExtensions.ThrowIfNegative(nintValue), nameof(nintValue));

        static void ShouldThrow_ArgumentOutOfRangeException_With_ParamName(Action action, string paramName) =>
            action.ShouldThrow<ArgumentOutOfRangeException>().ParamName.ShouldBe(paramName);
    }

    [TestMethod($"{nameof(ArgumentOutOfRangeException)}.{nameof(ArgumentOutOfRangeExceptionExtensions.ThrowIfNegative)}(1) does not throw any {nameof(Exception)}")]
    public void When_Value_Is_Not_Negative_ThrowIfNegative_Does_Not_Throw()
    {
        sbyte sbyteValue0 = 0;
        ShouldNotThrow(() => ArgumentOutOfRangeExceptionExtensions.ThrowIfNegative(sbyteValue0));
        sbyte sbyteValue1 = 1;
        ShouldNotThrow(() => ArgumentOutOfRangeExceptionExtensions.ThrowIfNegative(sbyteValue1));
        short shortValue0 = 0;
        ShouldNotThrow(() => ArgumentOutOfRangeExceptionExtensions.ThrowIfNegative(shortValue0));
        short shortValue1 = 1;
        ShouldNotThrow(() => ArgumentOutOfRangeExceptionExtensions.ThrowIfNegative(shortValue1));
        int intValue0 = 0;
        ShouldNotThrow(() => ArgumentOutOfRangeExceptionExtensions.ThrowIfNegative(intValue0));
        int intValue1 = 1;
        ShouldNotThrow(() => ArgumentOutOfRangeExceptionExtensions.ThrowIfNegative(intValue1));
        long longValue0 = 0;
        ShouldNotThrow(() => ArgumentOutOfRangeExceptionExtensions.ThrowIfNegative(longValue0));
        long longValue1 = 1;
        ShouldNotThrow(() => ArgumentOutOfRangeExceptionExtensions.ThrowIfNegative(longValue1));
        float floatValue0 = 0;
        ShouldNotThrow(() => ArgumentOutOfRangeExceptionExtensions.ThrowIfNegative(floatValue0));
        float floatValue1 = 1;
        ShouldNotThrow(() => ArgumentOutOfRangeExceptionExtensions.ThrowIfNegative(floatValue1));
        double doubleValue0 = 0;
        ShouldNotThrow(() => ArgumentOutOfRangeExceptionExtensions.ThrowIfNegative(doubleValue0));
        double doubleValue1 = 1;
        ShouldNotThrow(() => ArgumentOutOfRangeExceptionExtensions.ThrowIfNegative(doubleValue1));
        decimal decimalValue0 = 0;
        ShouldNotThrow(() => ArgumentOutOfRangeExceptionExtensions.ThrowIfNegative(decimalValue0));
        decimal decimalValue1 = 1;
        ShouldNotThrow(() => ArgumentOutOfRangeExceptionExtensions.ThrowIfNegative(decimalValue1));
        nint nintValue0 = 0;
        ShouldNotThrow(() => ArgumentOutOfRangeExceptionExtensions.ThrowIfNegative(nintValue0));
        nint nintValue1 = 1;
        ShouldNotThrow(() => ArgumentOutOfRangeExceptionExtensions.ThrowIfNegative(nintValue1));

        static void ShouldNotThrow(Action action) => action.ShouldNotThrow();
    }

    [TestMethod($"{nameof(ArgumentOutOfRangeException)}.{nameof(ArgumentOutOfRangeExceptionExtensions.ThrowIfNegativeOrZero)}(-1) throws {nameof(ArgumentOutOfRangeException)}")]
    public void When_Value_Is_Negative_Or_Zero_ThrowIfNegativeOrZero_Throws_ArgumentOutOfRangeException()
    {
        sbyte sbyteValue0 = 0;
        ShouldThrow_ArgumentOutOfRangeException_With_ParamName(() => ArgumentOutOfRangeExceptionExtensions.ThrowIfNegativeOrZero(sbyteValue0), nameof(sbyteValue0));
        sbyte sbyteValue = -1;
        ShouldThrow_ArgumentOutOfRangeException_With_ParamName(() => ArgumentOutOfRangeExceptionExtensions.ThrowIfNegativeOrZero(sbyteValue), nameof(sbyteValue));
        short shortValue0 = 0;
        ShouldThrow_ArgumentOutOfRangeException_With_ParamName(() => ArgumentOutOfRangeExceptionExtensions.ThrowIfNegativeOrZero(shortValue0), nameof(shortValue0));
        short shortValue = -1;
        ShouldThrow_ArgumentOutOfRangeException_With_ParamName(() => ArgumentOutOfRangeExceptionExtensions.ThrowIfNegativeOrZero(shortValue), nameof(shortValue));
        int intValue0 = 0;
        ShouldThrow_ArgumentOutOfRangeException_With_ParamName(() => ArgumentOutOfRangeExceptionExtensions.ThrowIfNegativeOrZero(intValue0), nameof(intValue0));
        int intValue = -1;
        ShouldThrow_ArgumentOutOfRangeException_With_ParamName(() => ArgumentOutOfRangeExceptionExtensions.ThrowIfNegativeOrZero(intValue), nameof(intValue));
        long longValue0 = 0;
        ShouldThrow_ArgumentOutOfRangeException_With_ParamName(() => ArgumentOutOfRangeExceptionExtensions.ThrowIfNegativeOrZero(longValue0), nameof(longValue0));
        long longValue = -1;
        ShouldThrow_ArgumentOutOfRangeException_With_ParamName(() => ArgumentOutOfRangeExceptionExtensions.ThrowIfNegativeOrZero(longValue), nameof(longValue));
        float floatValue0 = 0;
        ShouldThrow_ArgumentOutOfRangeException_With_ParamName(() => ArgumentOutOfRangeExceptionExtensions.ThrowIfNegativeOrZero(floatValue0), nameof(floatValue0));
        float floatValue = -1;
        ShouldThrow_ArgumentOutOfRangeException_With_ParamName(() => ArgumentOutOfRangeExceptionExtensions.ThrowIfNegativeOrZero(floatValue), nameof(floatValue));
        double doubleValue0 = 0;
        ShouldThrow_ArgumentOutOfRangeException_With_ParamName(() => ArgumentOutOfRangeExceptionExtensions.ThrowIfNegativeOrZero(doubleValue0), nameof(doubleValue0));
        double doubleValue = -1;
        ShouldThrow_ArgumentOutOfRangeException_With_ParamName(() => ArgumentOutOfRangeExceptionExtensions.ThrowIfNegativeOrZero(doubleValue), nameof(doubleValue));
        decimal decimalValue0 = 0;
        ShouldThrow_ArgumentOutOfRangeException_With_ParamName(() => ArgumentOutOfRangeExceptionExtensions.ThrowIfNegativeOrZero(decimalValue0), nameof(decimalValue0));
        decimal decimalValue = -1;
        ShouldThrow_ArgumentOutOfRangeException_With_ParamName(() => ArgumentOutOfRangeExceptionExtensions.ThrowIfNegativeOrZero(decimalValue), nameof(decimalValue));
        nint nintValue0 = 0;
        ShouldThrow_ArgumentOutOfRangeException_With_ParamName(() => ArgumentOutOfRangeExceptionExtensions.ThrowIfNegativeOrZero(nintValue0), nameof(nintValue0));
        nint nintValue = -1;
        ShouldThrow_ArgumentOutOfRangeException_With_ParamName(() => ArgumentOutOfRangeExceptionExtensions.ThrowIfNegativeOrZero(nintValue), nameof(nintValue));

        static void ShouldThrow_ArgumentOutOfRangeException_With_ParamName(Action action, string paramName) =>
            action.ShouldThrow<ArgumentOutOfRangeException>().ParamName.ShouldBe(paramName);
    }

    [TestMethod($"{nameof(ArgumentOutOfRangeException)}.{nameof(ArgumentOutOfRangeExceptionExtensions.ThrowIfNegative)}(1) does not throw any {nameof(Exception)}")]
    public void When_Value_Is_Not_Negative_Or_Zero_ThrowIfNegativeOrZero_Does_Not_Throw()
    {
        sbyte sbyteValue1 = 1;
        ShouldNotThrow(() => ArgumentOutOfRangeExceptionExtensions.ThrowIfNegative(sbyteValue1));
        short shortValue1 = 1;
        ShouldNotThrow(() => ArgumentOutOfRangeExceptionExtensions.ThrowIfNegative(shortValue1));
        int intValue1 = 1;
        ShouldNotThrow(() => ArgumentOutOfRangeExceptionExtensions.ThrowIfNegative(intValue1));
        long longValue1 = 1;
        ShouldNotThrow(() => ArgumentOutOfRangeExceptionExtensions.ThrowIfNegative(longValue1));
        float floatValue1 = 1;
        ShouldNotThrow(() => ArgumentOutOfRangeExceptionExtensions.ThrowIfNegative(floatValue1));
        double doubleValue1 = 1;
        ShouldNotThrow(() => ArgumentOutOfRangeExceptionExtensions.ThrowIfNegative(doubleValue1));
        decimal decimalValue1 = 1;
        ShouldNotThrow(() => ArgumentOutOfRangeExceptionExtensions.ThrowIfNegative(decimalValue1));
        nint nintValue1 = 1;
        ShouldNotThrow(() => ArgumentOutOfRangeExceptionExtensions.ThrowIfNegative(nintValue1));

        static void ShouldNotThrow(Action action) => action.ShouldNotThrow();
    }
}
