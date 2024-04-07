using Godot;



[GlobalClass, Icon("res://Assets/Icons/PlaceDown.svg")]
public partial class ItemPlacableData : ItemData
{
    private int? placeLayer = null;
    public int PlaceLayer
    {
        get
        {
            if (placeLayer != null)
                return (int)placeLayer; 

            switch (Texture.Atlas.ResourcePath)
            {
                case "res://Assets/Items/Floor.png":
                    placeLayer = 0;
                    break;

                case "res://Assets/Items/Decoration.png":
                    placeLayer = 1;
                    break;

                default:
                    placeLayer = 0;
                    break;
            }

            return (int)placeLayer;
        }
    }
    private Vector2I? tileCoordinates = null;
    public Vector2I TileCoordinates
    {
        get
        {
            if (tileCoordinates != null)
                return (Vector2I)tileCoordinates;

            tileCoordinates = new Vector2I
            (
                (int)Texture.Region.Position.X / (GameManager.Instance.SingleTileSize + GameManager.Instance.TileOffSet),
                (int)Texture.Region.Position.Y / (GameManager.Instance.SingleTileSize + GameManager.Instance.TileOffSet)
            );
            
            return (Vector2I)tileCoordinates;
        }
    } 



    public ItemPlacableData() : base()
    {
        MaxStackSize = 1000;
    }



    public ItemPlacableData(AtlasTexture texture, string name, StringName id, string description, int maxStackSize = 1000) : base(texture, name, id, description, maxStackSize) {}
}