using Godot;



public partial class InWorldItem : Node2D
{
    [Export] public ItemData Data { get; set; }
    private bool followPlayer = false;
    private float followSpeed;
    private CollisionShape2D followTarget;
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



    public override void _PhysicsProcess(double delta)
    {
        if (followPlayer)
        {
            Vector2 motion = Vector2.Zero.DirectionTo(followTarget.GlobalPosition - Position) * followSpeed;
            Position += motion;
        }
    }



    public void StartFollowingPlayer(Player player)
    {
        followPlayer = true;
        followSpeed = player.MoveSpeed * 1.2f;
        followTarget = player.GetNode<CollisionShape2D>("ItemCollector/Hitbox");
    }



    private void GenerateAnimationFrames()
    {
        SpriteFrames anim = new SpriteFrames();
        anim.AddAnimation("Idle");

        if (Data.Frames > 1)
        {
            Rect2I rect = (Rect2I)Data.Texture.Region;
            Image atlasImage = Data.Texture.Atlas.GetImage();

            for (int i = 0; i < Data.Frames; i++)
            {
                anim.AddFrame("Idle", ImageTexture.CreateFromImage(atlasImage.GetRegion(rect)));
                rect.Position = rect.Position with { X = rect.Position.X + rect.Size.X };
            }

            sprite.SpriteFrames = anim;
            sprite.Animation = "Idle";
            if (Data.AutoPlayAnim) sprite.Play();
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



    #region Signals
    private void DeleteOnItemStackEmpty(int stackSize)
    {
        if (stackSize < 1)
            QueueFree();
    }
    #endregion
}