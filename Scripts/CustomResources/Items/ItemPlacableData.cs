using Godot;



[GlobalClass, Icon("res://Assets/Icons/PlaceDown.svg")]
public partial class ItemPlacableData : ItemData
{
    [Export] public PlaceLayer Layer { get; private set; } = PlaceLayer.Floor;
    private Vector2I? tileCoordinates = null;
    public Vector2I TileCoordinates
    {
        get
        {
            if (tileCoordinates != null)
                return (Vector2I)tileCoordinates;

            tileCoordinates = new Vector2I
            (
                (int)Texture.Region.Position.X / GameManager.Instance.SingleTileSize,
                (int)Texture.Region.Position.Y / GameManager.Instance.SingleTileSize
            );
            
            return (Vector2I)tileCoordinates;
        }
    } 



    public ItemPlacableData() : base()
    {
        MaxStackSize = 1000;
    }



    public ItemPlacableData(AtlasTexture texture, string name, StringName id, string description, int maxStackSize = 1000) : base(texture, name, id, description, maxStackSize) {}



    public enum PlaceLayer {
        Floor,
        Decoration
    }
}