using System.Reflection;

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
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable CA2264 // Do not pass a non-nullable value to 'ArgumentNullException.ThrowIfNull'
        => ShouldNotThrow(static () => ArgumentNullException.ThrowIfNull(new object()));
#pragma warning restore CA2264 // Do not pass a non-nullable value to 'ArgumentNullException.ThrowIfNull'
#pragma warning restore IDE0079 // Remove unnecessary suppression

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

    #region Extension Methods Verification
    [TestMethod($"{nameof(ExceptionPolyfills)} should contain expected extension methods")]
    public void ExceptionPolyfills_Should_Contain_Expected_Extension_Methods()
    {
        var polyfillMethods = typeof(ExceptionPolyfills)
            .GetMethods(BindingFlags.Public | BindingFlags.Static);

        VerifyPolyfillMethod(typeof(ArgumentNullException), "ThrowIfNull", polyfillMethods, typeof(object));
        VerifyPolyfillMethod(typeof(ArgumentNullException), "ThrowIfNull", polyfillMethods, typeof(void*));
        VerifyPolyfillMethod(typeof(ArgumentException), "ThrowIfNullOrEmpty", polyfillMethods, typeof(string));
        VerifyPolyfillMethod(typeof(ArgumentException), "ThrowIfNullOrWhiteSpace", polyfillMethods, typeof(string));
        VerifyPolyfillMethod(typeof(ArgumentOutOfRangeException), "ThrowIfEqual", polyfillMethods, null, null);
        VerifyPolyfillMethod(typeof(ArgumentOutOfRangeException), "ThrowIfNotEqual", polyfillMethods, null, null);
        VerifyPolyfillMethod(typeof(ArgumentOutOfRangeException), "ThrowIfGreaterThan", polyfillMethods, typeof(IComparable<>), typeof(IComparable<>));
        VerifyPolyfillMethod(typeof(ArgumentOutOfRangeException), "ThrowIfGreaterThanOrEqual", polyfillMethods, typeof(IComparable<>), typeof(IComparable<>));
        VerifyPolyfillMethod(typeof(ArgumentOutOfRangeException), "ThrowIfLessThan", polyfillMethods, typeof(IComparable<>), typeof(IComparable<>));
        VerifyPolyfillMethod(typeof(ArgumentOutOfRangeException), "ThrowIfLessThanOrEqual", polyfillMethods, typeof(IComparable<>), typeof(IComparable<>));

        // Numeric validation methods - verify they exist as either generic or specific overloads
        VerifyNumericValidationMethods(polyfillMethods);

        VerifyPolyfillMethod(typeof(ObjectDisposedException), "ThrowIf", polyfillMethods, typeof(bool), typeof(object));
        VerifyPolyfillMethod(typeof(ObjectDisposedException), "ThrowIf", polyfillMethods, typeof(bool), typeof(Type));
    }

    /// <summary>
    /// Verifies numeric validation methods exist as either generic or specific overloads
    /// </summary>
    /// <param name="polyfillMethods">All available polyfill methods</param>
    private static void VerifyNumericValidationMethods(MethodInfo[] polyfillMethods)
    {
        if (Type.GetType("System.Numerics.INumberBase`1") is Type numberBaseType)
        {
            VerifyPolyfillMethod(typeof(ArgumentOutOfRangeException), "ThrowIfZero", polyfillMethods, numberBaseType);
            VerifyPolyfillMethod(typeof(ArgumentOutOfRangeException), "ThrowIfNegative", polyfillMethods, numberBaseType);
            VerifyPolyfillMethod(typeof(ArgumentOutOfRangeException), "ThrowIfNegativeOrZero", polyfillMethods, numberBaseType);
        }
        else
        {
            VerifySpecificNumericTypeMethods(polyfillMethods);
        }
    }

    /// <summary>
    /// Verifies specific numeric type methods for frameworks without INumberBase&lt;T&gt;
    /// </summary>
    /// <param name="polyfillMethods">All available polyfill methods</param>
    private static void VerifySpecificNumericTypeMethods(MethodInfo[] polyfillMethods)
    {
        Type[] unsignedNumericTypes = [
            typeof(byte), typeof(ushort),
            typeof(uint), typeof(ulong),
            typeof(nuint), typeof(char)
        ];
        Type[] signedNumericTypes = [
            typeof(sbyte), typeof(short),
            typeof(int), typeof(long),
            typeof(float), typeof(double),
            typeof(decimal), typeof(nint)
        ];

        foreach (var numericType in unsignedNumericTypes)
            VerifyPolyfillMethod(typeof(ArgumentOutOfRangeException), "ThrowIfZero", polyfillMethods, numericType);
        foreach (var type in signedNumericTypes)
        {
            VerifyPolyfillMethod(typeof(ArgumentOutOfRangeException), "ThrowIfZero", polyfillMethods, type);
            VerifyPolyfillMethod(typeof(ArgumentOutOfRangeException), "ThrowIfNegative", polyfillMethods, type);
            VerifyPolyfillMethod(typeof(ArgumentOutOfRangeException), "ThrowIfNegativeOrZero", polyfillMethods, type);
        }
    }

    /// <summary>
    /// Verifies that polyfill methods exist when framework lacks built-in implementation
    /// </summary>
    /// <param name="frameworkType">The framework exception type to check</param>
    /// <param name="methodName">The method name to verify</param>
    /// <param name="polyfillMethods">All available polyfill methods</param>
    /// <param name="expectedOverloadTypes">Expected parameter types for specific overloads to verify. Use null to represent generic T parameter.</param>
    private static void VerifyPolyfillMethod(Type frameworkType, string methodName, MethodInfo[] polyfillMethods, params Type?[] expectedOverloadTypes)
    {
        var frameworkMethods = frameworkType.GetMethods(BindingFlags.Public | BindingFlags.Static);
        if (!frameworkMethods.Any(m => m.Name == methodName && DoesMethodMatchExpectedSignature(m, expectedOverloadTypes)))
            AssertPolyfillMethodExists(frameworkType, methodName, polyfillMethods, expectedOverloadTypes);
    }

    /// <summary>
    /// Asserts that a polyfill method exists with the expected signature
    /// </summary>
    /// <param name="frameworkType">The framework exception type</param>
    /// <param name="methodName">The method name</param>
    /// <param name="polyfillMethods">Available polyfill methods</param>
    /// <param name="expectedOverloadTypes">Expected parameter types. Use null to represent generic T parameter.</param>
    private static void AssertPolyfillMethodExists(Type frameworkType, string methodName, MethodInfo[] polyfillMethods, Type?[] expectedOverloadTypes)
    {
        string typeNames = string.Join(", ", expectedOverloadTypes.Select(t => t?.Name ?? "T"));
        polyfillMethods.ShouldContain(
            m => m.Name == methodName && DoesMethodMatchExpectedSignature(m, expectedOverloadTypes),
            $"{frameworkType.Name}.{methodName}({typeNames}) should have polyfill when framework lacks built-in version"
        );
    }

    /// <summary>
    /// Checks if a method matches the expected signature, with optional string? parameter at the end
    /// </summary>
    /// <param name="method">The method to check</param>
    /// <param name="expectedOverloadTypes">Expected parameter types (excluding optional string? parameter). Use null to represent generic T parameter.</param>
    /// <returns>True if the method matches the expected signature</returns>
    private static bool DoesMethodMatchExpectedSignature(MethodInfo method, Type?[] expectedOverloadTypes)
    {
        var parameters = method.GetParameters();
        int expectedCount = expectedOverloadTypes.Length;

        // Method can have expected parameters with or without optional string parameter
        if (parameters.Length != expectedCount && parameters.Length != expectedCount + 1)
            return false;

        // Check that the first N parameters match exactly
        for (int i = 0; i < expectedCount; i++)
        {
            if (!DoesParameterTypeMatch(parameters[i].ParameterType, expectedOverloadTypes[i], method))
                return false;
        }

        // If there's an additional parameter, verify it's an optional string
        return parameters.Length == expectedCount ||
            (parameters[expectedCount].ParameterType == typeof(string) && parameters[expectedCount].HasDefaultValue);
    }

    /// <summary>
    /// Checks if a parameter type matches the expected type, handling generic constraints
    /// </summary>
    /// <param name="actualType">The actual parameter type</param>
    /// <param name="expectedType">The expected parameter type. Use null to represent generic T parameter.</param>
    /// <param name="method">The method containing the parameter</param>
    /// <returns>True if the types match considering generic constraints</returns>
    private static bool DoesParameterTypeMatch(Type actualType, Type? expectedType, MethodInfo method)
    {
        // If expectedType is null, it represents a generic T parameter
        if (expectedType is null)
            return method.IsGenericMethodDefinition && actualType.IsGenericParameter;

        // Direct type match
        if (actualType == expectedType)
            return true;

        // Handle generic method parameters with constraints
        if (!method.IsGenericMethodDefinition || !actualType.IsGenericParameter)
            return false;

        // For unconstrained generic type parameters, check if expectedType is a generic type definition that could match
        if (expectedType.IsGenericTypeDefinition)
        {
            var constraints = actualType.GetGenericParameterConstraints();

            // For generic constraints like IComparable<T>, or INumberBase<T>:
            // The constraint should be the expected interface with T as the same generic parameter
            return constraints.Any(constraint =>
            {
                if (!constraint.IsGenericType)
                    return false;

                var constraintDefinition = constraint.GetGenericTypeDefinition();
                if (constraintDefinition != expectedType)
                    return false;

                // Verify that T in the constraint (e.g., IComparable<T>) is the same as our generic parameter
                var constraintArgs = constraint.GetGenericArguments();
                return constraintArgs.Length == 1 && constraintArgs[0] == actualType;
            });
        }

        return false;
    }
    #endregion Extension Methods Verification
}
