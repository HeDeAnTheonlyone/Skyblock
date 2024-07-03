using System;



#nullable enable



/// <summary>
/// This exception is thrown when a placeable item is looking in which layer it's texture is but can't find it.
/// </summary>
public partial class NotAnItemException : Exception
{
    public NotAnItemException() : base() {}



    public NotAnItemException(string? message) : base(message) {}
}