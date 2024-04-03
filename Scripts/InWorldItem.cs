using Godot;



public partial class InWorldItem : Node2D
{
    [Export] public ItemData Data { get; set; }
    private AnimatedSprite2D sprite;



    public override void _Ready()
    {
        sprite = GetNode<AnimatedSprite2D>("Sprite");
        GenerateAnimationFrames();
        Data.UpdateStackSize += DeleteOnItemStackEmpty;
        
        sprite.Offset = sprite.Offset with { Y = -2 };
        Hover();
    }



    public override void _ExitTree() => Data.UpdateStackSize -= DeleteOnItemStackEmpty;



    private void GenerateAnimationFrames()
    {
        SpriteFrames anim = new SpriteFrames();
        anim.AddAnimation("Idle");

        if (Data.Frames > 1)
        {
            Rect2I rect = (Rect2I)Data.Texture.Region;
            Image altasImage = Data.Texture.Atlas.GetImage();

            for (int i = 0; i < Data.Frames; i++)
            {
                anim.AddFrame("Idle", ImageTexture.CreateFromImage(altasImage.GetRegion(rect)));
                rect.Position = rect.Position with { X = rect.Position.X + rect.Size.X + GameManager.Instance.TileOffSet };
            }

            sprite.SpriteFrames = anim;
            sprite.Play("Idle");
        }
        else
        {
            anim.AddFrame("Idle", Data.Texture);
            sprite.SpriteFrames = anim;
            sprite.Animation = "Idle";
        }
    }



    private void Hover()
    {
        Tween tween = CreateTween().SetLoops().SetTrans(Tween.TransitionType.Sine);
        tween.TweenProperty(sprite, "offset:y", 2, 0.5);
        tween.TweenProperty(sprite, "offset:y", -2, 0.5);
    }



    private void CollectItem(Node2D body)
    {
        Player player = body as Player;
        
        if (player != null)
            player.CollectItem(Data);
    }



    #region Signals
    private void DeleteOnItemStackEmpty(int stackSize)
    {
        if (stackSize < 1)
            QueueFree();
    }
    #endregion
}