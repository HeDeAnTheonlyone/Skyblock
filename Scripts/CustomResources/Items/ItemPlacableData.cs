using Godot;


[Tool]
[GlobalClass, Icon("res://Assets/Icons/PlaceDown.svg")]
public partial class ItemPlacableData : ItemData
{
    [ExportGroup("Item")]
    [ExportSubgroup("Texture")]
    public new AtlasTexture Texture
    {
        get => base.Texture;
        set
        {
            base.Texture = value;
            if (Texture != null)
                Texture.Changed += UpdateTexCoords;
        }
    }
    [Export] private Vector2I TextureCoordinates { get; set; }



    public ItemPlacableData() : base() {}



    public ItemPlacableData(AtlasTexture texture, string name, string description, int maxStackSize = 1000) : base(texture, name, description, maxStackSize) {}



    private void UpdateTexCoords()
    {
        TextureCoordinates = TextureCoordinates with
        {
            X = (int)Texture.Region.Position.X / 16,
            Y = (int)Texture.Region.Position.Y / 16
        };
    }
}