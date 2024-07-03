using System;



#nullable enable



/// <summary>
/// This exception is thrown when a field is null when it should have a value at this point.
/// </summary>
public partial class FieldIsNullException : Exception
{
    public FieldIsNullException() : base() {}



    public FieldIsNullException(string? message) : base(message) {}
}