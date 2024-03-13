using Godot;


[GlobalClass, Icon("res://Assets/Icons/Material.svg")]
public partial class ItemMaterialData : ItemData
{
    public override int MaxStackSize { get; set; } = 1000;
    //TODO
}