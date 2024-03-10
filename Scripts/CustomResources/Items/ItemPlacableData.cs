using Godot;



[Tool]
[GlobalClass, Icon("res://Assets/Icons/PlaceDown.svg")]
public partial class ItemPlacableData : ItemData
{
    private int placeLayer;
    private Vector2I TextureCoordinates;

    [ExportGroup("Item")]
    [ExportSubgroup("Texture")]
    public override AtlasTexture Texture
    {
        get => base.Texture;
        set
        {
            base.Texture = value;
            
            if (Texture != null)
            {
                if (!Texture.IsConnected(SignalName.Changed, Callable.From(UpdateTextureDependenValues)))
                    Texture.Changed += UpdateTextureDependenValues;
            }
        }
    }

    [ExportSubgroup("Properties")]
    public override int MaxStackSize { get; set; } = 1000;



    public ItemPlacableData() : base() {}



    public ItemPlacableData(AtlasTexture texture, string name, string description, int maxStackSize = 1000) : base(texture, name, description, maxStackSize) {}



    private void UpdateTextureDependenValues()
    {
        try
        {
            TextureCoordinates = TextureCoordinates with
            {
                X = (int)Texture.Region.Position.X / GameManager.Instance.SingleTileSize,
                Y = (int)Texture.Region.Position.Y / GameManager.Instance.SingleTileSize
            };
        }
        catch
        {
            TextureCoordinates = TextureCoordinates with
            {
                X = (int)Texture.Region.Position.X / 16,
                Y = (int)Texture.Region.Position.Y / 16
            };
        }

        if (Texture.Atlas != null)
            SetPlaceLayer();
    }



    private void SetPlaceLayer()
    {
        switch (Texture.Atlas.ResourcePath)
        {
            case "res://Assets/GroundTiles.png":
                placeLayer = 0;
                break;

            case "res://Assets/DecorationsTiles.png":
                placeLayer = 1;
                break;

            default:
                placeLayer = 1;
                break;
        }
    }
}