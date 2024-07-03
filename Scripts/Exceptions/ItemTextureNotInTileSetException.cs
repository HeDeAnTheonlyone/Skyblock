using System;



#nullable enable



/// <summary>
/// This exception is thrown when a placeable item is looking in which layer it's texture is but can't find it.
/// </summary>
public partial class ItemTextureNotInTileSetException : Exception
{
    public ItemTextureNotInTileSetException() : base() {}



    public ItemTextureNotInTileSetException(string? message) : base(message) {}
}