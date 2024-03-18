using Godot;



[GlobalClass]
public partial class InventoryPlayerData : InventoryData
{
    [ExportGroup("Properties")]
    [Export] public ItemData[] Armor { get; set; } = new ItemData[3];
    public Slot[] ArmorSlots { get; set; }



    public InventoryPlayerData()
    {
        Items = new ItemData[70];
    }



    #region Update items in inventory
    public override void UpdateInventoryItems()
    {
        base.UpdateInventoryItems();
        UpdateInventoryItemsInternal(ArmorSlots, Armor);
    }



    public override InventoryData GetInventoryData() => this;
    #endregion



    public override void MoveItem(InventoryItem item)
    {
        if (!MoveItemInternal(item, ItemSlots))
            MoveItemInternal(item, ArmorSlots);
    }
}