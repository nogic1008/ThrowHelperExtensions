namespace Nogic.ThrowHelperExtensions.Tests;

/// <summary>
/// Unit test class for <see cref="ExceptionPolyfills"/>
/// </summary>
[TestClass]
public sealed class ExceptionPolyfillsTest
{
    /// <summary>
    /// Asserts that <paramref name="action"/> does not throw any exceptions.
    /// </summary>
    private static void ShouldNotThrow(Action action)
        => action.ShouldNotThrow();

    /// <summary>
    /// Asserts that <paramref name="action"/> throws an exception of type <typeparamref name="TException"/>.
    /// </summary>
    private static TException ShouldThrow<TException>(Action action) where TException : Exception
        => action.ShouldThrow<TException>();

    #region ArgumentException
    [TestMethod($"{nameof(ArgumentException)}.ThrowIfNullOrEmpty(null) throws {nameof(ArgumentNullException)}")]
    public void When_Argument_Is_Null_ThrowIfNullOrEmpty_Throws_ArgumentNullException()
    {
        // Arrange
        string? argument = null;

        // Act - Assert
        ShouldThrow<ArgumentNullException>(() => ArgumentException.ThrowIfNullOrEmpty(argument))
            .ParamName.ShouldBe(nameof(argument));
    }

    [TestMethod($"{nameof(ArgumentException)}.ThrowIfNullOrEmpty(\"\") throws {nameof(ArgumentException)}")]
    public void When_Argument_Is_Empty_String_ThrowIfNullOrEmpty_Throws_ArgumentException()
    {
        // Arrange
        string? argument = "";

        // Act - Assert
        ShouldThrow<ArgumentException>(() => ArgumentException.ThrowIfNullOrEmpty(argument))
            .ParamName.ShouldBe(nameof(argument));
    }

    [TestMethod($"{nameof(ArgumentException)}.ThrowIfNullOrEmpty(\"foo\") does not throw any {nameof(Exception)}")]
    public void When_Argument_Is_Not_Empty_String_ThrowIfNullOrEmpty_Does_Not_Throw_Exception()
        => ShouldNotThrow(static () => ArgumentException.ThrowIfNullOrEmpty("foo"));

    [TestMethod($"{nameof(ArgumentException)}.ThrowIfNullOrWhiteSpace(null) throws {nameof(ArgumentNullException)}")]
    public void When_Argument_Is_Null_ThrowIfNullOrWhiteSpace_Throws_ArgumentNullException()
    {
        // Arrange
        string? argument = null;

        // Act - Assert
        ShouldThrow<ArgumentNullException>(() => ArgumentException.ThrowIfNullOrWhiteSpace(argument))
            .ParamName.ShouldBe(nameof(argument));
    }

    [TestMethod($"{nameof(ArgumentException)}.ThrowIfNullOrWhiteSpace(<white space>) throws {nameof(ArgumentException)}")]
    [DataRow("")]
    [DataRow(" ")]
    [DataRow("　")]
    public void When_Argument_Is_WhiteSpace_ThrowIfNullOrWhiteSpace_Throws_ArgumentException(string argument)
        => ShouldThrow<ArgumentException>(() => ArgumentException.ThrowIfNullOrWhiteSpace(argument))
            .ParamName.ShouldBe(nameof(argument));

    [TestMethod($"{nameof(ArgumentException)}.ThrowIfNullOrWhiteSpace(\"foo\") does not throw any {nameof(Exception)}")]
    public void When_Argument_Is_Not_WhiteSpace_ThrowIfNullOrWhiteSpace_Does_Not_Throw_Exception()
        => ShouldNotThrow(static () => ArgumentException.ThrowIfNullOrWhiteSpace("foo"));
    #endregion ArgumentException

    #region ArgumentNullException
    [TestMethod($"{nameof(ArgumentNullException)}.ThrowIfNull(null) throws {nameof(ArgumentNullException)}")]
    public void When_Argument_Is_Null_ThrowIfNull_Throws_ArgumentNullException()
    {
        // Arrange
        object? argument = null;

        // Act - Assert
        ShouldThrow<ArgumentNullException>(() => ArgumentNullException.ThrowIfNull(argument))
            .ParamName.ShouldBe(nameof(argument));
    }

    [TestMethod($"{nameof(ArgumentNullException)}.ThrowIfNull(<not null>) does not throw any {nameof(Exception)}")]
    public void When_Argument_Is_Not_Null_ThrowIfNull_Does_Not_Throw_Exception()
#pragma warning disable CS2264 // Do not pass a non-nullable value to 'ArgumentNullException.ThrowIfNull'
        => ShouldNotThrow(static () => ArgumentNullException.ThrowIfNull(new object()));
#pragma warning restore CS2264

    [TestMethod($"{nameof(ArgumentNullException)}.ThrowIfNull(<null pointer>) throws {nameof(ArgumentNullException)}")]
    public unsafe void When_Argument_Is_Null_Pointer_ThrowIfNull_Throws_ArgumentNullException()
    {
        // Arrange
        void* argument = null;

        // Act - Assert
        ShouldThrow<ArgumentNullException>(() => ArgumentNullException.ThrowIfNull(argument))
            .ParamName.ShouldBe(nameof(argument));
    }

    [TestMethod($"{nameof(ArgumentNullException)}.ThrowIfNull(<pointer>) does not throw any {nameof(Exception)}")]
    public unsafe void When_Argument_Is_Pointer_ThrowIfNull_Does_Not_Throw_Exception()
    {
        // Arrange
        int* argument = stackalloc int[1];

        // Act - Assert
        ShouldNotThrow(() => ArgumentNullException.ThrowIfNull(argument));
    }
    #endregion ArgumentNullException

    #region ArgumentOutOfRangeException
    [TestMethod($"{nameof(ArgumentOutOfRangeException)}.ThrowIfEqual() throws {nameof(ArgumentOutOfRangeException)}")]
    [DataRow(1, 1)]
    [DataRow("foo", "foo")]
    public void When_Value_Equals_Other_ThrowIfEqual_Throws_ArgumentOutOfRangeException<T>(T value, T other) where T : IEquatable<T>
        => ShouldThrow<ArgumentOutOfRangeException>(() => ArgumentOutOfRangeException.ThrowIfEqual(value, other))
            .ParamName.ShouldBe(nameof(value));

    [TestMethod($"{nameof(ArgumentOutOfRangeException)}.ThrowIfEqual() does not throw any {nameof(Exception)}")]
    [DataRow(1, 0)]
    [DataRow("foo", "bar")]
    public void When_Value_Not_Equals_Other_ThrowIfEqual_Does_Not_Throw_Exception<T>(T value, T other) where T : IEquatable<T>
        => ShouldNotThrow(() => ArgumentOutOfRangeException.ThrowIfEqual(value, other));

    [TestMethod($"{nameof(ArgumentOutOfRangeException)}.ThrowIfNotEqual() throws {nameof(ArgumentOutOfRangeException)}")]
    [DataRow(1, 0)]
    [DataRow("foo", "bar")]
    public void When_Value_Not_Equals_Other_ThrowIfNotEqual_Throws_ArgumentOutOfRangeException<T>(T value, T other) where T : IEquatable<T>
        => ShouldThrow<ArgumentOutOfRangeException>(() => ArgumentOutOfRangeException.ThrowIfNotEqual(value, other))
            .ParamName.ShouldBe(nameof(value));

    [TestMethod($"{nameof(ArgumentOutOfRangeException)}.ThrowIfNotEqual() does not throw any {nameof(Exception)}")]
    [DataRow(1, 1)]
    [DataRow("foo", "foo")]
    public void When_Value_Equals_Other_ThrowIfNotEqual_Does_Not_Throw_Exception<T>(T value, T other) where T : IEquatable<T>
        => ShouldNotThrow(() => ArgumentOutOfRangeException.ThrowIfNotEqual(value, other));

    [TestMethod($"{nameof(ArgumentOutOfRangeException)}.ThrowIfGreaterThan() throws {nameof(ArgumentOutOfRangeException)}")]
    [DataRow(1, 0)]
    [DataRow("foo", "bar")]
    public void When_Value_Is_Greater_Than_Other_ThrowIfGreaterThan_Throws_ArgumentOutOfRangeException<T>(T value, T other) where T : IComparable<T>
        => ShouldThrow<ArgumentOutOfRangeException>(() => ArgumentOutOfRangeException.ThrowIfGreaterThan(value, other))
            .ParamName.ShouldBe(nameof(value));

    [TestMethod($"{nameof(ArgumentOutOfRangeException)}.ThrowIfGreaterThan() does not throw any {nameof(Exception)}")]
    [DataRow(0, 1)]
    [DataRow(1, 1)]
    [DataRow("bar", "foo")]
    [DataRow("foo", "foo")]
    public void When_Value_Is_Not_Greater_Than_Other_ThrowIfGreaterThan_Does_Not_Throw_Exception<T>(T value, T other) where T : IComparable<T>
        => ShouldNotThrow(() => ArgumentOutOfRangeException.ThrowIfGreaterThan(value, other));

    [TestMethod($"{nameof(ArgumentOutOfRangeException)}.ThrowIfGreaterThanOrEqual() throws {nameof(ArgumentOutOfRangeException)}")]
    [DataRow(1, 0)]
    [DataRow(1, 1)]
    [DataRow("foo", "bar")]
    [DataRow("foo", "foo")]
    public void When_Value_Is_Greater_Than_Or_Equal_Other_ThrowIfGreaterThanOrEqual_Throws_ArgumentOutOfRangeException<T>(T value, T other) where T : IComparable<T>
        => ShouldThrow<ArgumentOutOfRangeException>(() => ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(value, other))
            .ParamName.ShouldBe(nameof(value));

    [TestMethod($"{nameof(ArgumentOutOfRangeException)}.ThrowIfGreaterThanOrEqual() does not throw any {nameof(Exception)}")]
    [DataRow(0, 1)]
    [DataRow("bar", "foo")]
    public void When_Value_Is_Not_Greater_Than_Or_Equal_Other_ThrowIfGreaterThanOrEqual_Does_Not_Throw_Exception<T>(T value, T other) where T : IComparable<T>
        => ShouldNotThrow(() => ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(value, other));

    [TestMethod($"{nameof(ArgumentOutOfRangeException)}.ThrowIfLessThan() throws {nameof(ArgumentOutOfRangeException)}")]
    [DataRow(0, 1)]
    [DataRow("bar", "foo")]
    public void When_Value_Is_Less_Than_Other_ThrowIfLessThan_Throws_ArgumentOutOfRangeException<T>(T value, T other) where T : IComparable<T>
        => ShouldThrow<ArgumentOutOfRangeException>(() => ArgumentOutOfRangeException.ThrowIfLessThan(value, other))
            .ParamName.ShouldBe(nameof(value));

    [TestMethod($"{nameof(ArgumentOutOfRangeException)}.ThrowIfLessThan() does not throw any {nameof(Exception)}")]
    [DataRow(1, 0)]
    [DataRow(1, 1)]
    [DataRow("foo", "bar")]
    [DataRow("bar", "bar")]
    public void When_Value_Is_Not_Less_Than_Other_ThrowIfLessThan_Does_Not_Throw_Exception<T>(T value, T other) where T : IComparable<T>
        => ShouldNotThrow(() => ArgumentOutOfRangeException.ThrowIfLessThan(value, other));

    [TestMethod($"{nameof(ArgumentOutOfRangeException)}.ThrowIfLessThanOrEqual() throws {nameof(ArgumentOutOfRangeException)}")]
    [DataRow(0, 1)]
    [DataRow(1, 1)]
    [DataRow("bar", "foo")]
    [DataRow("bar", "bar")]
    public void When_Value_Is_Less_Than_Or_Equal_Other_ThrowIfLessThanOrEqual_Throws_ArgumentOutOfRangeException<T>(T value, T other) where T : IComparable<T>
        => ShouldThrow<ArgumentOutOfRangeException>(() => ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(value, other))
            .ParamName.ShouldBe(nameof(value));

    [TestMethod($"{nameof(ArgumentOutOfRangeException)}.ThrowIfLessThanOrEqual() does not throw any {nameof(Exception)}")]
    [DataRow(1, 0)]
    [DataRow("foo", "bar")]
    public void When_Value_Is_Not_Less_Than_Or_Equal_Other_ThrowIfLessThanOrEqual_Does_Not_Throw_Exception<T>(T value, T other) where T : IComparable<T>
        => ShouldNotThrow(() => ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(value, other));

    [TestMethod($"{nameof(ArgumentOutOfRangeException)}.ThrowIfZero(0) throws {nameof(ArgumentOutOfRangeException)}")]
    public void When_Value_Is_Zero_ThrowIfZero_Throws_ArgumentOutOfRangeException()
    {
        ShouldThrow<ArgumentOutOfRangeException>(() => ArgumentOutOfRangeException.ThrowIfZero((byte)0));
        ShouldThrow<ArgumentOutOfRangeException>(() => ArgumentOutOfRangeException.ThrowIfZero((sbyte)0));
        ShouldThrow<ArgumentOutOfRangeException>(() => ArgumentOutOfRangeException.ThrowIfZero((short)0));
        ShouldThrow<ArgumentOutOfRangeException>(() => ArgumentOutOfRangeException.ThrowIfZero((ushort)0));
        ShouldThrow<ArgumentOutOfRangeException>(() => ArgumentOutOfRangeException.ThrowIfZero(0));
        ShouldThrow<ArgumentOutOfRangeException>(() => ArgumentOutOfRangeException.ThrowIfZero((uint)0));
        ShouldThrow<ArgumentOutOfRangeException>(() => ArgumentOutOfRangeException.ThrowIfZero((long)0));
        ShouldThrow<ArgumentOutOfRangeException>(() => ArgumentOutOfRangeException.ThrowIfZero((ulong)0));
        ShouldThrow<ArgumentOutOfRangeException>(() => ArgumentOutOfRangeException.ThrowIfZero((float)0));
        ShouldThrow<ArgumentOutOfRangeException>(() => ArgumentOutOfRangeException.ThrowIfZero((double)0));
        ShouldThrow<ArgumentOutOfRangeException>(() => ArgumentOutOfRangeException.ThrowIfZero((decimal)0));
        ShouldThrow<ArgumentOutOfRangeException>(() => ArgumentOutOfRangeException.ThrowIfZero((nint)0));
        ShouldThrow<ArgumentOutOfRangeException>(() => ArgumentOutOfRangeException.ThrowIfZero((nuint)0));
        ShouldThrow<ArgumentOutOfRangeException>(() => ArgumentOutOfRangeException.ThrowIfZero((char)0));
    }

    [TestMethod($"{nameof(ArgumentOutOfRangeException)}.ThrowIfZero(1) does not throw any {nameof(Exception)}")]
    public void When_Value_Is_Not_Zero_ThrowIfZero_Does_Not_Throw()
    {
        ShouldNotThrow(() => ArgumentOutOfRangeException.ThrowIfZero((byte)1));
        ShouldNotThrow(() => ArgumentOutOfRangeException.ThrowIfZero((sbyte)1));
        ShouldNotThrow(() => ArgumentOutOfRangeException.ThrowIfZero((short)1));
        ShouldNotThrow(() => ArgumentOutOfRangeException.ThrowIfZero((ushort)1));
        ShouldNotThrow(() => ArgumentOutOfRangeException.ThrowIfZero((ushort)1));
        ShouldNotThrow(() => ArgumentOutOfRangeException.ThrowIfZero(1));
        ShouldNotThrow(() => ArgumentOutOfRangeException.ThrowIfZero((uint)1));
        ShouldNotThrow(() => ArgumentOutOfRangeException.ThrowIfZero((long)1));
        ShouldNotThrow(() => ArgumentOutOfRangeException.ThrowIfZero((ulong)1));
        ShouldNotThrow(() => ArgumentOutOfRangeException.ThrowIfZero((float)1));
        ShouldNotThrow(() => ArgumentOutOfRangeException.ThrowIfZero((double)1));
        ShouldNotThrow(() => ArgumentOutOfRangeException.ThrowIfZero((decimal)1));
        ShouldNotThrow(() => ArgumentOutOfRangeException.ThrowIfZero((nint)1));
        ShouldNotThrow(() => ArgumentOutOfRangeException.ThrowIfZero((nuint)1));
        ShouldNotThrow(() => ArgumentOutOfRangeException.ThrowIfZero((char)1));
    }

    [TestMethod($"{nameof(ArgumentOutOfRangeException)}.ThrowIfNegative(-1) throws {nameof(ArgumentOutOfRangeException)}")]
    public void When_Value_Is_Negative_ThrowIfNegative_Throws_ArgumentOutOfRangeException()
    {
        ShouldThrow<ArgumentOutOfRangeException>(() => ArgumentOutOfRangeException.ThrowIfNegative((sbyte)-1));
        ShouldThrow<ArgumentOutOfRangeException>(() => ArgumentOutOfRangeException.ThrowIfNegative((short)-1));
        ShouldThrow<ArgumentOutOfRangeException>(() => ArgumentOutOfRangeException.ThrowIfNegative(-1));
        ShouldThrow<ArgumentOutOfRangeException>(() => ArgumentOutOfRangeException.ThrowIfNegative((long)-1));
        ShouldThrow<ArgumentOutOfRangeException>(() => ArgumentOutOfRangeException.ThrowIfNegative((float)-1));
        ShouldThrow<ArgumentOutOfRangeException>(() => ArgumentOutOfRangeException.ThrowIfNegative((double)-1));
        ShouldThrow<ArgumentOutOfRangeException>(() => ArgumentOutOfRangeException.ThrowIfNegative((decimal)-1));
        ShouldThrow<ArgumentOutOfRangeException>(() => ArgumentOutOfRangeException.ThrowIfNegative((nint)(-1)));
    }

    [TestMethod($"{nameof(ArgumentOutOfRangeException)}.ThrowIfNegative(0 or 1) does not throw any {nameof(Exception)}")]
    [DataRow(0, DisplayName = $"{nameof(ArgumentOutOfRangeException)}.ThrowIfNegative(0) does not throw any {nameof(Exception)}")]
    [DataRow(1, DisplayName = $"{nameof(ArgumentOutOfRangeException)}.ThrowIfNegative(1) does not throw any {nameof(Exception)}")]
    public void When_Value_Is_Not_Negative_ThrowIfNegative_Does_Not_Throw(int value)
    {
        ShouldNotThrow(() => ArgumentOutOfRangeException.ThrowIfNegative((sbyte)value));
        ShouldNotThrow(() => ArgumentOutOfRangeException.ThrowIfNegative((short)value));
        ShouldNotThrow(() => ArgumentOutOfRangeException.ThrowIfNegative(value));
        ShouldNotThrow(() => ArgumentOutOfRangeException.ThrowIfNegative((long)value));
        ShouldNotThrow(() => ArgumentOutOfRangeException.ThrowIfNegative((float)value));
        ShouldNotThrow(() => ArgumentOutOfRangeException.ThrowIfNegative((double)value));
        ShouldNotThrow(() => ArgumentOutOfRangeException.ThrowIfNegative((decimal)value));
        ShouldNotThrow(() => ArgumentOutOfRangeException.ThrowIfNegative((nint)value));
    }

    [TestMethod($"{nameof(ArgumentOutOfRangeException)}.ThrowIfNegativeOrZero(0 or -1) throws {nameof(ArgumentOutOfRangeException)}")]
    [DataRow(0, DisplayName = $"{nameof(ArgumentOutOfRangeException)}.ThrowIfNegativeOrZero(0) throws {nameof(ArgumentOutOfRangeException)}")]
    [DataRow(-1, DisplayName = $"{nameof(ArgumentOutOfRangeException)}.ThrowIfNegativeOrZero(-1) throws {nameof(ArgumentOutOfRangeException)}")]
    public void When_Value_Is_Negative_Or_Zero_ThrowIfNegativeOrZero_Throws_ArgumentOutOfRangeException(int value)
    {
        ShouldThrow<ArgumentOutOfRangeException>(() => ArgumentOutOfRangeException.ThrowIfNegativeOrZero((sbyte)value));
        ShouldThrow<ArgumentOutOfRangeException>(() => ArgumentOutOfRangeException.ThrowIfNegativeOrZero((short)value));
        ShouldThrow<ArgumentOutOfRangeException>(() => ArgumentOutOfRangeException.ThrowIfNegativeOrZero(value));
        ShouldThrow<ArgumentOutOfRangeException>(() => ArgumentOutOfRangeException.ThrowIfNegativeOrZero((long)value));
        ShouldThrow<ArgumentOutOfRangeException>(() => ArgumentOutOfRangeException.ThrowIfNegativeOrZero((float)value));
        ShouldThrow<ArgumentOutOfRangeException>(() => ArgumentOutOfRangeException.ThrowIfNegativeOrZero((double)value));
        ShouldThrow<ArgumentOutOfRangeException>(() => ArgumentOutOfRangeException.ThrowIfNegativeOrZero((decimal)value));
        ShouldThrow<ArgumentOutOfRangeException>(() => ArgumentOutOfRangeException.ThrowIfNegativeOrZero((nint)value));
    }

    [TestMethod($"{nameof(ArgumentOutOfRangeException)}.ThrowIfNegativeOrZero(1) does not throw any {nameof(Exception)}")]
    public void When_Value_Is_Not_Negative_Or_Zero_ThrowIfNegativeOrZero_Does_Not_Throw()
    {
        ShouldNotThrow(static () => ArgumentOutOfRangeException.ThrowIfNegativeOrZero((sbyte)1));
        ShouldNotThrow(static () => ArgumentOutOfRangeException.ThrowIfNegativeOrZero((short)1));
        ShouldNotThrow(static () => ArgumentOutOfRangeException.ThrowIfNegativeOrZero(1));
        ShouldNotThrow(static () => ArgumentOutOfRangeException.ThrowIfNegativeOrZero((long)1));
        ShouldNotThrow(static () => ArgumentOutOfRangeException.ThrowIfNegativeOrZero((float)1));
        ShouldNotThrow(static () => ArgumentOutOfRangeException.ThrowIfNegativeOrZero((double)1));
        ShouldNotThrow(static () => ArgumentOutOfRangeException.ThrowIfNegativeOrZero((decimal)1));
        ShouldNotThrow(static () => ArgumentOutOfRangeException.ThrowIfNegativeOrZero((nint)1));
    }
    #endregion ArgumentOutOfRangeException

    #region ObjectDisposedException
    [TestMethod($"{nameof(ObjectDisposedException)}.ThrowIf(true, <object or Type>) throws {nameof(ObjectDisposedException)}")]
    public void When_Condition_Is_True_ThrowIf_Throws_ObjectDisposedException()
    {
        ShouldThrow<ObjectDisposedException>(() => ObjectDisposedException.ThrowIf(true, new object()));
        ShouldThrow<ObjectDisposedException>(() => ObjectDisposedException.ThrowIf(true, typeof(object)));
    }

    [TestMethod($"{nameof(ObjectDisposedException)}.ThrowIf(false, <object or Type>) does not throw any {nameof(Exception)}")]
    public void When_Condition_Is_False_ThrowIf_Does_Not_Throw_Exception()
    {
        ShouldNotThrow(() => ObjectDisposedException.ThrowIf(false, new object()));
        ShouldNotThrow(() => ObjectDisposedException.ThrowIf(false, typeof(object)));
    }
    #endregion ObjectDisposedException
}
