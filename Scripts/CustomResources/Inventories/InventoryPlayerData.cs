using Godot;



[GlobalClass]
public partial class InventoryPlayerData : InventoryData
{
    [ExportGroup("Properties")]
    [Export] public ItemData[] Armor { get; set; } = new ItemData[3];
    [Export] public override ItemData[] Items { get; set; } = new ItemData[70];



    public void DisplayItemsInSlots(Slot[] itemSlots, ItemData[] items, Slot[] armorSlots, ItemData[] armor)
    {
        DisplayItemsInSlots(itemSlots, items);
        DisplayItemsInSlots(armorSlots, armor);
    }
}