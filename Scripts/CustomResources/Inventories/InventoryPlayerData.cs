using Godot;



[GlobalClass]
public partial class InventoryPlayerData : InventoryData
{
    [ExportGroup("Properties")]
    [Export] public ItemData[] Armor { get; set; } = new ItemData[3];
    public override ItemData[] Items { get; set; } = new ItemData[70];



    public void AddItemsToSlots(Slot[] itemSlots, ItemData[] items, Slot[] armorSlots, ItemData[] armor)
    {
        AddItemsToSlots(itemSlots, items);
        AddItemsToSlots(armorSlots, armor);
    }
}