using System;
using System.Runtime.Serialization;


#nullable enable


/// <summary>
/// The exception that is thrown when a fiel is null when it should have a value at this point.
/// </summary>
public partial class FieldIsNullException : Exception, ISerializable
{
    public FieldIsNullException() : base() {}



    public FieldIsNullException(string? message) : base(message) {}
}