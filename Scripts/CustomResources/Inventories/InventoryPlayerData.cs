using System.Linq;
using Godot;



[GlobalClass, Icon("res://Assets/Icons/Inventory.svg")]
public partial class InventoryPlayerData : InventoryData
{
    [ExportGroup("Properties")]
    [Export] public ItemData[] Armor { get; private set; } = new ItemData[3];
    public Slot[] ArmorSlots { get; private set; }
    public Slot[] HotBarSlots { get; private set; }


    public InventoryPlayerData()
    {
        Items = new ItemData[70];
    }



    public void SetSlots(Slot[] itemSlots, Slot[] armorSlots, Slot[] hotBarSlots)
    {
        SetSlots(itemSlots);
        ArmorSlots = armorSlots;
        HotBarSlots = hotBarSlots;
    }



    #region Update items in inventory
    public override void UpdateInventoryItems()
    {
        base.UpdateInventoryItems();
        UpdateInventoryItemsInternal(ArmorSlots, Armor);
        UpdateInventoryItemsInternal(HotBarSlots, Items.Take(10).ToArray());
    }



    public override InventoryData GetInventoryData() => this;
    #endregion



    public override void MoveItem(InventoryItem item)
    {
        if (!MoveItemInternal(item, ItemSlots, Items))
            MoveItemInternal(item, ArmorSlots, Armor);
    }
}