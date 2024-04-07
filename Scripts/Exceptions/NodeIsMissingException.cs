using System;



#nullable enable



/// <summary>
/// The exception that is thrown when a node with a fixed position in a tree is not there.
/// </summary>
public partial class NodeIsMissingException : Exception
{
    public NodeIsMissingException() : base() {}



    public NodeIsMissingException(string? message) : base(message) {}
}