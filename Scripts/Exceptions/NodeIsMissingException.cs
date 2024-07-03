using System;



#nullable enable



/// <summary>
/// This exception is thrown when a node with a fixed position in a tree is not there.
/// </summary>
public partial class NodeIsMissingException : Exception
{
    public NodeIsMissingException() : base() {}



    public NodeIsMissingException(string? message) : base(message) {}
}