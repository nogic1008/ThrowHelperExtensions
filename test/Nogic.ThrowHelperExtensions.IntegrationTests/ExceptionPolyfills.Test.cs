using System.Reflection;

namespace Nogic.ThrowHelperExtensions.IntegrationTests;

/// <summary>
/// Unit test class for <see cref="ExceptionPolyfills"/>
/// </summary>
public sealed class ExceptionPolyfillsTest
{
    #region ArgumentException
    [Test]
    [DisplayName(
        $"{nameof(ArgumentException)}.ThrowIfNullOrEmpty(null) throws {nameof(ArgumentNullException)}"
    )]
    public async ValueTask When_Argument_Is_Null_ThrowIfNullOrEmpty_Throws_ArgumentNullException()
    {
        // Arrange
        string? argument = null;

        // Act - Assert
        await Assert
            .That(() => ArgumentException.ThrowIfNullOrEmpty(argument))
            .ThrowsExactly<ArgumentNullException>()
            .And.WithParameterName(nameof(argument));
    }

    [Test]
    [DisplayName(
        $"{nameof(ArgumentException)}.ThrowIfNullOrEmpty(\"\") throws {nameof(ArgumentException)}"
    )]
    public async ValueTask When_Argument_Is_Empty_String_ThrowIfNullOrEmpty_Throws_ArgumentException()
    {
        // Arrange
        string? argument = "";

        // Act - Assert
        await Assert
            .That(() => ArgumentException.ThrowIfNullOrEmpty(argument))
            .ThrowsExactly<ArgumentException>()
            .WithParameterName(nameof(argument));
    }

    [Test]
    [DisplayName(
        $"{nameof(ArgumentException)}.ThrowIfNullOrEmpty(\"$argument\") does not throw any Exception"
    )]
    [Arguments(" ")]
    [Arguments("foo")]
    public async ValueTask When_Argument_Is_Not_Empty_String_ThrowIfNullOrEmpty_Does_Not_Throw_Exception(
        string argument
    ) => await Assert.That(() => ArgumentException.ThrowIfNullOrEmpty(argument)).ThrowsNothing();

    [Test]
    [DisplayName(
        $"{nameof(ArgumentException)}.ThrowIfNullOrWhiteSpace(null) throws {nameof(ArgumentNullException)}"
    )]
    public async ValueTask When_Argument_Is_Null_ThrowIfNullOrWhiteSpace_Throws_ArgumentNullException()
    {
        // Arrange
        string? argument = null;

        // Act - Assert
        await Assert
            .That(() => ArgumentException.ThrowIfNullOrWhiteSpace(argument))
            .ThrowsExactly<ArgumentNullException>()
            .WithParameterName(nameof(argument));
    }

    [Test]
    [DisplayName(
        $"{nameof(ArgumentException)}.ThrowIfNullOrWhiteSpace(\"$argument\") throws {nameof(ArgumentException)}"
    )]
    [Arguments("")]
    [Arguments(" ")]
    [Arguments("　")]
    public async ValueTask When_Argument_Is_WhiteSpace_ThrowIfNullOrWhiteSpace_Throws_ArgumentException(
        string argument
    ) =>
        await Assert
            .That(() => ArgumentException.ThrowIfNullOrWhiteSpace(argument))
            .ThrowsExactly<ArgumentException>()
            .WithParameterName(nameof(argument));

    [Test]
    [DisplayName(
        $"{nameof(ArgumentException)}.ThrowIfNullOrWhiteSpace(\"$argument\") does not throw any Exception"
    )]
    [Arguments("foo")]
    public async ValueTask When_Argument_Is_Not_WhiteSpace_ThrowIfNullOrWhiteSpace_Does_Not_Throw_Exception(
        string argument
    ) =>
        await Assert
            .That(() => ArgumentException.ThrowIfNullOrWhiteSpace(argument))
            .ThrowsNothing();
    #endregion ArgumentException

    #region ArgumentNullException
    [Test]
    [DisplayName(
        $"{nameof(ArgumentNullException)}.ThrowIfNull(null) throws {nameof(ArgumentNullException)}"
    )]
    public async ValueTask When_Argument_Is_Null_ThrowIfNull_Throws_ArgumentNullException()
    {
        // Arrange
        object? argument = null;

        // Act - Assert
        await Assert
            .That(() => ArgumentNullException.ThrowIfNull(argument))
            .ThrowsExactly<ArgumentNullException>()
            .WithParameterName(nameof(argument));
    }

    [Test]
    [DisplayName(
        $"{nameof(ArgumentNullException)}.ThrowIfNull($argument) does not throw any Exception"
    )]
    [Arguments("")]
    [Arguments(0)]
    public async ValueTask When_Argument_Is_Not_Null_ThrowIfNull_Does_Not_Throw_Exception(
        object? argument
    ) => await Assert.That(() => ArgumentNullException.ThrowIfNull(argument)).ThrowsNothing();

    [Test]
    [DisplayName(
        $"{nameof(ArgumentNullException)}.ThrowIfNull(<null pointer>) throws {nameof(ArgumentNullException)}"
    )]
    public unsafe void When_Argument_Is_Null_Pointer_ThrowIfNull_Throws_ArgumentNullException()
    {
        // Arrange
        void* argument = null;

        // Act - Assert
#pragma warning disable TUnitAssertions0002
        Assert
            .That(() => ArgumentNullException.ThrowIfNull(argument))
            .ThrowsExactly<ArgumentNullException>()
            .WithParameterName(nameof(argument))
            .GetAwaiter()
            .GetResult();
#pragma warning restore TUnitAssertions0002
    }

    [Test]
    [DisplayName(
        $"{nameof(ArgumentNullException)}.ThrowIfNull(<pointer>) does not throw any Exception"
    )]
    public unsafe void When_Argument_Is_Pointer_ThrowIfNull_Does_Not_Throw_Exception()
    {
        // Arrange
        int* argument = stackalloc int[1];

        // Act - Assert
#pragma warning disable TUnitAssertions0002
        Assert.That(() => ArgumentNullException.ThrowIfNull(argument)).ThrowsNothing();
#pragma warning restore TUnitAssertions0002
    }
    #endregion ArgumentNullException

    #region ArgumentOutOfRangeException
    [Test]
    [DisplayName(
        $"{nameof(ArgumentOutOfRangeException)}.ThrowIfEqual($value, $other) throws {nameof(ArgumentOutOfRangeException)}"
    )]
    [Arguments(1, 1)]
    [Arguments("foo", "foo")]
    public async ValueTask When_Value_Equals_Other_ThrowIfEqual_Throws_ArgumentOutOfRangeException<T>(
        T value,
        T other
    )
        where T : IEquatable<T> =>
        await Assert
            .That(() => ArgumentOutOfRangeException.ThrowIfEqual(value, other))
            .ThrowsExactly<ArgumentOutOfRangeException>()
            .WithParameterName(nameof(value));

    [Test]
    [DisplayName(
        $"{nameof(ArgumentOutOfRangeException)}.ThrowIfEqual($value, $other) does not throw Exception"
    )]
    [Arguments(1, 0)]
    [Arguments("foo", "bar")]
    public async ValueTask When_Value_Not_Equals_Other_ThrowIfEqual_Does_Not_Throw_Exception<T>(
        T value,
        T other
    )
        where T : IEquatable<T> =>
        await Assert
            .That(() => ArgumentOutOfRangeException.ThrowIfEqual(value, other))
            .ThrowsNothing();

    [Test]
    [DisplayName(
        $"{nameof(ArgumentOutOfRangeException)}.ThrowIfNotEqual($value, $other) throws {nameof(ArgumentOutOfRangeException)}"
    )]
    [Arguments(1, 0)]
    [Arguments("foo", "bar")]
    public async ValueTask When_Value_Not_Equals_Other_ThrowIfNotEqual_Throws_ArgumentOutOfRangeException<T>(
        T value,
        T other
    )
        where T : IEquatable<T> =>
        await Assert
            .That(() => ArgumentOutOfRangeException.ThrowIfNotEqual(value, other))
            .ThrowsExactly<ArgumentOutOfRangeException>()
            .WithParameterName(nameof(value));

    [Test]
    [DisplayName(
        $"{nameof(ArgumentOutOfRangeException)}.ThrowIfNotEqual($value, $other) does not throw any Exception"
    )]
    [Arguments(1, 1)]
    [Arguments("foo", "foo")]
    public async ValueTask When_Value_Equals_Other_ThrowIfNotEqual_Does_Not_Throw_Exception<T>(
        T value,
        T other
    )
        where T : IEquatable<T> =>
        await Assert
            .That(() => ArgumentOutOfRangeException.ThrowIfNotEqual(value, other))
            .ThrowsNothing();

    [Test]
    [DisplayName(
        $"{nameof(ArgumentOutOfRangeException)}.ThrowIfGreaterThan($value, $other) throws {nameof(ArgumentOutOfRangeException)}"
    )]
    [Arguments(1, 0)]
    [Arguments("foo", "bar")]
    public async ValueTask When_Value_Is_Greater_Than_Other_ThrowIfGreaterThan_Throws_ArgumentOutOfRangeException<T>(
        T value,
        T other
    )
        where T : IComparable<T> =>
        await Assert
            .That(() => ArgumentOutOfRangeException.ThrowIfGreaterThan(value, other))
            .ThrowsExactly<ArgumentOutOfRangeException>()
            .WithParameterName(nameof(value));

    [Test]
    [DisplayName(
        $"{nameof(ArgumentOutOfRangeException)}.ThrowIfGreaterThan($value, $other) does not throw any Exception"
    )]
    [Arguments(0, 1)]
    [Arguments(1, 1)]
    [Arguments("bar", "foo")]
    [Arguments("foo", "foo")]
    public async ValueTask When_Value_Is_Not_Greater_Than_Other_ThrowIfGreaterThan_Does_Not_Throw_Exception<T>(
        T value,
        T other
    )
        where T : IComparable<T> =>
        await Assert
            .That(() => ArgumentOutOfRangeException.ThrowIfGreaterThan(value, other))
            .ThrowsNothing();

    [Test]
    [DisplayName(
        $"{nameof(ArgumentOutOfRangeException)}.ThrowIfGreaterThanOrEqual($value, $other) throws {nameof(ArgumentOutOfRangeException)}"
    )]
    [Arguments(1, 0)]
    [Arguments(1, 1)]
    [Arguments("foo", "bar")]
    [Arguments("foo", "foo")]
    public async ValueTask When_Value_Is_Greater_Than_Or_Equal_Other_ThrowIfGreaterThanOrEqual_Throws_ArgumentOutOfRangeException<T>(
        T value,
        T other
    )
        where T : IComparable<T> =>
        await Assert
            .That(() => ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(value, other))
            .ThrowsExactly<ArgumentOutOfRangeException>()
            .WithParameterName(nameof(value));

    [Test]
    [DisplayName(
        $"{nameof(ArgumentOutOfRangeException)}.ThrowIfGreaterThanOrEqual($value, $other) does not throw any Exception"
    )]
    [Arguments(0, 1)]
    [Arguments("bar", "foo")]
    public async ValueTask When_Value_Is_Not_Greater_Than_Or_Equal_Other_ThrowIfGreaterThanOrEqual_Does_Not_Throw_Exception<T>(
        T value,
        T other
    )
        where T : IComparable<T> =>
        await Assert
            .That(() => ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(value, other))
            .ThrowsNothing();

    [Test]
    [DisplayName(
        $"{nameof(ArgumentOutOfRangeException)}.ThrowIfLessThan($value, $other) throws {nameof(ArgumentOutOfRangeException)}"
    )]
    [Arguments(0, 1)]
    [Arguments("bar", "foo")]
    public async ValueTask When_Value_Is_Less_Than_Other_ThrowIfLessThan_Throws_ArgumentOutOfRangeException<T>(
        T value,
        T other
    )
        where T : IComparable<T> =>
        await Assert
            .That(() => ArgumentOutOfRangeException.ThrowIfLessThan(value, other))
            .ThrowsExactly<ArgumentOutOfRangeException>()
            .WithParameterName(nameof(value));

    [Test]
    [DisplayName(
        $"{nameof(ArgumentOutOfRangeException)}.ThrowIfLessThan($value, $other) does not throw any Exception"
    )]
    [Arguments(1, 0)]
    [Arguments(1, 1)]
    [Arguments("foo", "bar")]
    [Arguments("bar", "bar")]
    public async ValueTask When_Value_Is_Not_Less_Than_Other_ThrowIfLessThan_Does_Not_Throw_Exception<T>(
        T value,
        T other
    )
        where T : IComparable<T> =>
        await Assert
            .That(() => ArgumentOutOfRangeException.ThrowIfLessThan(value, other))
            .ThrowsNothing();

    [Test]
    [DisplayName(
        $"{nameof(ArgumentOutOfRangeException)}.ThrowIfLessThanOrEqual($value, $other) throws {nameof(ArgumentOutOfRangeException)}"
    )]
    [Arguments(0, 1)]
    [Arguments(1, 1)]
    [Arguments("bar", "foo")]
    [Arguments("bar", "bar")]
    public async ValueTask When_Value_Is_Less_Than_Or_Equal_Other_ThrowIfLessThanOrEqual_Throws_ArgumentOutOfRangeException<T>(
        T value,
        T other
    )
        where T : IComparable<T> =>
        await Assert
            .That(() => ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(value, other))
            .ThrowsExactly<ArgumentOutOfRangeException>()
            .WithParameterName(nameof(value));

    [Test]
    [DisplayName(
        $"{nameof(ArgumentOutOfRangeException)}.ThrowIfLessThanOrEqual($value, $other) does not throw any Exception"
    )]
    [Arguments(1, 0)]
    [Arguments("foo", "bar")]
    public async ValueTask When_Value_Is_Not_Less_Than_Or_Equal_Other_ThrowIfLessThanOrEqual_Does_Not_Throw_Exception<T>(
        T value,
        T other
    )
        where T : IComparable<T> =>
        await Assert
            .That(() => ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(value, other))
            .ThrowsNothing();

    [Test]
    [DisplayName(
        $"{nameof(ArgumentOutOfRangeException)}.ThrowIfZero(0) throws {nameof(ArgumentOutOfRangeException)}"
    )]
    public async ValueTask When_Value_Is_Zero_ThrowIfZero_Throws_ArgumentOutOfRangeException()
    {
        await Assert
            .That(() => ArgumentOutOfRangeException.ThrowIfZero((byte)0))
            .ThrowsExactly<ArgumentOutOfRangeException>();
        await Assert
            .That(() => ArgumentOutOfRangeException.ThrowIfZero((sbyte)0))
            .ThrowsExactly<ArgumentOutOfRangeException>();
        await Assert
            .That(() => ArgumentOutOfRangeException.ThrowIfZero((short)0))
            .ThrowsExactly<ArgumentOutOfRangeException>();
        await Assert
            .That(() => ArgumentOutOfRangeException.ThrowIfZero((ushort)0))
            .ThrowsExactly<ArgumentOutOfRangeException>();
        await Assert
            .That(() => ArgumentOutOfRangeException.ThrowIfZero(0))
            .ThrowsExactly<ArgumentOutOfRangeException>();
        await Assert
            .That(() => ArgumentOutOfRangeException.ThrowIfZero((uint)0))
            .ThrowsExactly<ArgumentOutOfRangeException>();
        await Assert
            .That(() => ArgumentOutOfRangeException.ThrowIfZero((long)0))
            .ThrowsExactly<ArgumentOutOfRangeException>();
        await Assert
            .That(() => ArgumentOutOfRangeException.ThrowIfZero((ulong)0))
            .ThrowsExactly<ArgumentOutOfRangeException>();
        await Assert
            .That(() => ArgumentOutOfRangeException.ThrowIfZero((float)0))
            .ThrowsExactly<ArgumentOutOfRangeException>();
        await Assert
            .That(() => ArgumentOutOfRangeException.ThrowIfZero((double)0))
            .ThrowsExactly<ArgumentOutOfRangeException>();
        await Assert
            .That(() => ArgumentOutOfRangeException.ThrowIfZero((decimal)0))
            .ThrowsExactly<ArgumentOutOfRangeException>();
        await Assert
            .That(() => ArgumentOutOfRangeException.ThrowIfZero((nint)0))
            .ThrowsExactly<ArgumentOutOfRangeException>();
        await Assert
            .That(() => ArgumentOutOfRangeException.ThrowIfZero((nuint)0))
            .ThrowsExactly<ArgumentOutOfRangeException>();
        await Assert
            .That(() => ArgumentOutOfRangeException.ThrowIfZero((char)0))
            .ThrowsExactly<ArgumentOutOfRangeException>();
    }

    [Test]
    [DisplayName(
        $"{nameof(ArgumentOutOfRangeException)}.ThrowIfZero(1) does not throw any Exception"
    )]
    public async ValueTask When_Value_Is_Not_Zero_ThrowIfZero_Does_Not_Throw()
    {
        await Assert.That(() => ArgumentOutOfRangeException.ThrowIfZero((byte)1)).ThrowsNothing();
        await Assert.That(() => ArgumentOutOfRangeException.ThrowIfZero((sbyte)1)).ThrowsNothing();
        await Assert.That(() => ArgumentOutOfRangeException.ThrowIfZero((short)1)).ThrowsNothing();
        await Assert.That(() => ArgumentOutOfRangeException.ThrowIfZero((ushort)1)).ThrowsNothing();
        await Assert.That(() => ArgumentOutOfRangeException.ThrowIfZero(1)).ThrowsNothing();
        await Assert.That(() => ArgumentOutOfRangeException.ThrowIfZero((uint)1)).ThrowsNothing();
        await Assert.That(() => ArgumentOutOfRangeException.ThrowIfZero((long)1)).ThrowsNothing();
        await Assert.That(() => ArgumentOutOfRangeException.ThrowIfZero((ulong)1)).ThrowsNothing();
        await Assert.That(() => ArgumentOutOfRangeException.ThrowIfZero((float)1)).ThrowsNothing();
        await Assert.That(() => ArgumentOutOfRangeException.ThrowIfZero((double)1)).ThrowsNothing();
        await Assert
            .That(() => ArgumentOutOfRangeException.ThrowIfZero((decimal)1))
            .ThrowsNothing();
        await Assert.That(() => ArgumentOutOfRangeException.ThrowIfZero((nint)1)).ThrowsNothing();
        await Assert.That(() => ArgumentOutOfRangeException.ThrowIfZero((nuint)1)).ThrowsNothing();
        await Assert.That(() => ArgumentOutOfRangeException.ThrowIfZero((char)1)).ThrowsNothing();
    }

    [Test]
    [DisplayName(
        $"{nameof(ArgumentOutOfRangeException)}.ThrowIfNegative(-1) throws {nameof(ArgumentOutOfRangeException)}"
    )]
    public async ValueTask When_Value_Is_Negative_ThrowIfNegative_Throws_ArgumentOutOfRangeException()
    {
        await Assert
            .That(() => ArgumentOutOfRangeException.ThrowIfNegative((sbyte)-1))
            .ThrowsExactly<ArgumentOutOfRangeException>();
        await Assert
            .That(() => ArgumentOutOfRangeException.ThrowIfNegative((short)-1))
            .ThrowsExactly<ArgumentOutOfRangeException>();
        await Assert
            .That(() => ArgumentOutOfRangeException.ThrowIfNegative(-1))
            .ThrowsExactly<ArgumentOutOfRangeException>();
        await Assert
            .That(() => ArgumentOutOfRangeException.ThrowIfNegative((long)-1))
            .ThrowsExactly<ArgumentOutOfRangeException>();
        await Assert
            .That(() => ArgumentOutOfRangeException.ThrowIfNegative((float)-1))
            .ThrowsExactly<ArgumentOutOfRangeException>();
        await Assert
            .That(() => ArgumentOutOfRangeException.ThrowIfNegative((double)-1))
            .ThrowsExactly<ArgumentOutOfRangeException>();
        await Assert
            .That(() => ArgumentOutOfRangeException.ThrowIfNegative((decimal)-1))
            .ThrowsExactly<ArgumentOutOfRangeException>();
        await Assert
            .That(() => ArgumentOutOfRangeException.ThrowIfNegative((nint)(-1)))
            .ThrowsExactly<ArgumentOutOfRangeException>();
    }

    [Test]
    [DisplayName(
        $"{nameof(ArgumentOutOfRangeException)}.ThrowIfNegative($value) does not throw any Exception"
    )]
    [Arguments(0)]
    [Arguments(1)]
    public async ValueTask When_Value_Is_Not_Negative_ThrowIfNegative_Does_Not_Throw(int value)
    {
        await Assert
            .That(() => ArgumentOutOfRangeException.ThrowIfNegative((sbyte)value))
            .ThrowsNothing();
        await Assert
            .That(() => ArgumentOutOfRangeException.ThrowIfNegative((short)value))
            .ThrowsNothing();
        await Assert.That(() => ArgumentOutOfRangeException.ThrowIfNegative(value)).ThrowsNothing();
        await Assert
            .That(() => ArgumentOutOfRangeException.ThrowIfNegative((long)value))
            .ThrowsNothing();
        await Assert
            .That(() => ArgumentOutOfRangeException.ThrowIfNegative((float)value))
            .ThrowsNothing();
        await Assert
            .That(() => ArgumentOutOfRangeException.ThrowIfNegative((double)value))
            .ThrowsNothing();
        await Assert
            .That(() => ArgumentOutOfRangeException.ThrowIfNegative((decimal)value))
            .ThrowsNothing();
        await Assert
            .That(() => ArgumentOutOfRangeException.ThrowIfNegative((nint)value))
            .ThrowsNothing();
    }

    [Test]
    [DisplayName(
        $"{nameof(ArgumentOutOfRangeException)}.ThrowIfNegativeOrZero($value) throws {nameof(ArgumentOutOfRangeException)}"
    )]
    [Arguments(0)]
    [Arguments(-1)]
    public async ValueTask When_Value_Is_Negative_Or_Zero_ThrowIfNegativeOrZero_Throws_ArgumentOutOfRangeException(
        int value
    )
    {
        await Assert
            .That(() => ArgumentOutOfRangeException.ThrowIfNegativeOrZero((sbyte)value))
            .ThrowsExactly<ArgumentOutOfRangeException>();
        await Assert
            .That(() => ArgumentOutOfRangeException.ThrowIfNegativeOrZero((short)value))
            .ThrowsExactly<ArgumentOutOfRangeException>();
        await Assert
            .That(() => ArgumentOutOfRangeException.ThrowIfNegativeOrZero(value))
            .ThrowsExactly<ArgumentOutOfRangeException>();
        await Assert
            .That(() => ArgumentOutOfRangeException.ThrowIfNegativeOrZero((long)value))
            .ThrowsExactly<ArgumentOutOfRangeException>();
        await Assert
            .That(() => ArgumentOutOfRangeException.ThrowIfNegativeOrZero((float)value))
            .ThrowsExactly<ArgumentOutOfRangeException>();
        await Assert
            .That(() => ArgumentOutOfRangeException.ThrowIfNegativeOrZero((double)value))
            .ThrowsExactly<ArgumentOutOfRangeException>();
        await Assert
            .That(() => ArgumentOutOfRangeException.ThrowIfNegativeOrZero((decimal)value))
            .ThrowsExactly<ArgumentOutOfRangeException>();
        await Assert
            .That(() => ArgumentOutOfRangeException.ThrowIfNegativeOrZero((nint)value))
            .ThrowsExactly<ArgumentOutOfRangeException>();
    }

    [Test]
    [DisplayName(
        $"{nameof(ArgumentOutOfRangeException)}.ThrowIfNegativeOrZero(1) does not throw any Exception"
    )]
    public async ValueTask When_Value_Is_Not_Negative_Or_Zero_ThrowIfNegativeOrZero_Does_Not_Throw()
    {
        await Assert
            .That(static () => ArgumentOutOfRangeException.ThrowIfNegativeOrZero((sbyte)1))
            .ThrowsNothing();
        await Assert
            .That(static () => ArgumentOutOfRangeException.ThrowIfNegativeOrZero((short)1))
            .ThrowsNothing();
        await Assert
            .That(static () => ArgumentOutOfRangeException.ThrowIfNegativeOrZero(1))
            .ThrowsNothing();
        await Assert
            .That(static () => ArgumentOutOfRangeException.ThrowIfNegativeOrZero((long)1))
            .ThrowsNothing();
        await Assert
            .That(static () => ArgumentOutOfRangeException.ThrowIfNegativeOrZero((float)1))
            .ThrowsNothing();
        await Assert
            .That(static () => ArgumentOutOfRangeException.ThrowIfNegativeOrZero((double)1))
            .ThrowsNothing();
        await Assert
            .That(static () => ArgumentOutOfRangeException.ThrowIfNegativeOrZero((decimal)1))
            .ThrowsNothing();
        await Assert
            .That(static () => ArgumentOutOfRangeException.ThrowIfNegativeOrZero((nint)1))
            .ThrowsNothing();
    }
    #endregion ArgumentOutOfRangeException

    #region ObjectDisposedException
    [Test]
    [DisplayName(
        $"{nameof(ObjectDisposedException)}.ThrowIf(true, <object or Type>) throws {nameof(ObjectDisposedException)}"
    )]
    public async ValueTask When_Condition_Is_True_ThrowIf_Throws_ObjectDisposedException()
    {
        await Assert
            .That(() => ObjectDisposedException.ThrowIf(true, new object()))
            .ThrowsExactly<ObjectDisposedException>();
        await Assert
            .That(() => ObjectDisposedException.ThrowIf(true, typeof(object)))
            .ThrowsExactly<ObjectDisposedException>();
    }

    [Test]
    [DisplayName(
        $"{nameof(ObjectDisposedException)}.ThrowIf(false, <object or Type>) does not throw any {nameof(Exception)}"
    )]
    public async ValueTask When_Condition_Is_False_ThrowIf_Does_Not_Throw_Exception()
    {
        await Assert
            .That(() => ObjectDisposedException.ThrowIf(false, new object()))
            .ThrowsNothing();
        await Assert
            .That(() => ObjectDisposedException.ThrowIf(false, typeof(object)))
            .ThrowsNothing();
    }
    #endregion ObjectDisposedException

    #region Extension Methods Verification
    [Test]
    [DisplayName(
        $"{nameof(ExceptionPolyfills)} contains $exceptionType.$methodName($argumentsTypes)"
    )]
    [Arguments(typeof(ArgumentNullException), "ThrowIfNull", typeof(object))]
    [Arguments(typeof(ArgumentNullException), "ThrowIfNull", typeof(void*))]
    [Arguments(typeof(ArgumentException), "ThrowIfNullOrEmpty", typeof(string))]
    [Arguments(typeof(ArgumentException), "ThrowIfNullOrWhiteSpace", typeof(string))]
    [Arguments(typeof(ArgumentOutOfRangeException), "ThrowIfEqual", null, null)]
    [Arguments(typeof(ArgumentOutOfRangeException), "ThrowIfNotEqual", null, null)]
    [Arguments(
        typeof(ArgumentOutOfRangeException),
        "ThrowIfGreaterThan",
        typeof(IComparable<>),
        typeof(IComparable<>)
    )]
    [Arguments(
        typeof(ArgumentOutOfRangeException),
        "ThrowIfGreaterThanOrEqual",
        typeof(IComparable<>),
        typeof(IComparable<>)
    )]
    [Arguments(
        typeof(ArgumentOutOfRangeException),
        "ThrowIfLessThan",
        typeof(IComparable<>),
        typeof(IComparable<>)
    )]
    [Arguments(
        typeof(ArgumentOutOfRangeException),
        "ThrowIfLessThanOrEqual",
        typeof(IComparable<>),
        typeof(IComparable<>)
    )]
    [Arguments(typeof(ObjectDisposedException), "ThrowIf", typeof(bool), typeof(object))]
    [Arguments(typeof(ObjectDisposedException), "ThrowIf", typeof(bool), typeof(Type))]
#if NET7_0_OR_GREATER
    [Arguments(
        typeof(ArgumentOutOfRangeException),
        "ThrowIfZero",
        typeof(System.Numerics.INumberBase<>)
    )]
    [Arguments(
        typeof(ArgumentOutOfRangeException),
        "ThrowIfNegative",
        typeof(System.Numerics.INumberBase<>)
    )]
    [Arguments(
        typeof(ArgumentOutOfRangeException),
        "ThrowIfNegativeOrZero",
        typeof(System.Numerics.INumberBase<>)
    )]
#else
    [Arguments(typeof(ArgumentOutOfRangeException), "ThrowIfZero", typeof(byte))]
    [Arguments(typeof(ArgumentOutOfRangeException), "ThrowIfZero", typeof(sbyte))]
    [Arguments(typeof(ArgumentOutOfRangeException), "ThrowIfNegative", typeof(sbyte))]
    [Arguments(typeof(ArgumentOutOfRangeException), "ThrowIfNegativeOrZero", typeof(sbyte))]
    [Arguments(typeof(ArgumentOutOfRangeException), "ThrowIfZero", typeof(short))]
    [Arguments(typeof(ArgumentOutOfRangeException), "ThrowIfNegative", typeof(short))]
    [Arguments(typeof(ArgumentOutOfRangeException), "ThrowIfNegativeOrZero", typeof(short))]
    [Arguments(typeof(ArgumentOutOfRangeException), "ThrowIfZero", typeof(ushort))]
    [Arguments(typeof(ArgumentOutOfRangeException), "ThrowIfZero", typeof(int))]
    [Arguments(typeof(ArgumentOutOfRangeException), "ThrowIfNegative", typeof(int))]
    [Arguments(typeof(ArgumentOutOfRangeException), "ThrowIfNegativeOrZero", typeof(int))]
    [Arguments(typeof(ArgumentOutOfRangeException), "ThrowIfZero", typeof(uint))]
    [Arguments(typeof(ArgumentOutOfRangeException), "ThrowIfZero", typeof(long))]
    [Arguments(typeof(ArgumentOutOfRangeException), "ThrowIfNegative", typeof(long))]
    [Arguments(typeof(ArgumentOutOfRangeException), "ThrowIfNegativeOrZero", typeof(long))]
    [Arguments(typeof(ArgumentOutOfRangeException), "ThrowIfZero", typeof(ulong))]
    [Arguments(typeof(ArgumentOutOfRangeException), "ThrowIfZero", typeof(float))]
    [Arguments(typeof(ArgumentOutOfRangeException), "ThrowIfNegative", typeof(float))]
    [Arguments(typeof(ArgumentOutOfRangeException), "ThrowIfNegativeOrZero", typeof(float))]
    [Arguments(typeof(ArgumentOutOfRangeException), "ThrowIfZero", typeof(double))]
    [Arguments(typeof(ArgumentOutOfRangeException), "ThrowIfNegative", typeof(double))]
    [Arguments(typeof(ArgumentOutOfRangeException), "ThrowIfNegativeOrZero", typeof(double))]
    [Arguments(typeof(ArgumentOutOfRangeException), "ThrowIfZero", typeof(decimal))]
    [Arguments(typeof(ArgumentOutOfRangeException), "ThrowIfNegative", typeof(decimal))]
    [Arguments(typeof(ArgumentOutOfRangeException), "ThrowIfNegativeOrZero", typeof(decimal))]
    [Arguments(typeof(ArgumentOutOfRangeException), "ThrowIfZero", typeof(nint))]
    [Arguments(typeof(ArgumentOutOfRangeException), "ThrowIfNegative", typeof(nint))]
    [Arguments(typeof(ArgumentOutOfRangeException), "ThrowIfNegativeOrZero", typeof(nint))]
    [Arguments(typeof(ArgumentOutOfRangeException), "ThrowIfZero", typeof(nuint))]
    [Arguments(typeof(ArgumentOutOfRangeException), "ThrowIfZero", typeof(char))]
#endif
    public async ValueTask ExceptionPolyfills_Should_Contain_Expected_Extension_Method(
        Type exceptionType,
        string methodName,
        params Type?[] argumentsTypes
    )
    {
        var polyfillMethods = typeof(ExceptionPolyfills).GetMethods(
            BindingFlags.Public | BindingFlags.Static
        );
        var frameworkMethods = exceptionType.GetMethods(BindingFlags.Public | BindingFlags.Static);
        if (!frameworkMethods.Any(SignitureEquals))
            await Assert.That(polyfillMethods).Contains(SignitureEquals);

        bool SignitureEquals(MethodInfo methodInfo)
        {
            if (methodInfo.Name != methodName)
                return false;

            var parameters = methodInfo.GetParameters();
            int expectedCount = argumentsTypes.Length;

            // Method can have expected parameters with or without optional string parameter
            if (parameters.Length != expectedCount && parameters.Length != expectedCount + 1)
                return false;

            // Check that the first N parameters match exactly
            for (int i = 0; i < expectedCount; i++)
            {
                var paramType = parameters[i].ParameterType;
                if (!DoesParameterTypeMatch(paramType, argumentsTypes[i], methodInfo))
                    return false;
            }

            // If there's an additional parameter, verify it's an optional string
            return parameters.Length == expectedCount
                || (
                    parameters[^1].ParameterType == typeof(string) && parameters[^1].HasDefaultValue
                );
        }
    }

    /// <summary>
    /// Checks if a parameter type matches the expected type, handling generic constraints
    /// </summary>
    /// <param name="actualType">The actual parameter type</param>
    /// <param name="expectedType">The expected parameter type. Use null to represent generic T parameter.</param>
    /// <param name="method">The method containing the parameter</param>
    /// <returns>True if the types match considering generic constraints</returns>
    private static bool DoesParameterTypeMatch(
        Type actualType,
        Type? expectedType,
        MethodInfo method
    )
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
