using System.Linq;
using Godot;



[GlobalClass, Icon("res://Assets/Icons/Inventory.svg")]
public partial class InventoryPlayerData : InventoryData
{
    [ExportGroup("Properties")]
    [Export] public ItemData[] Armor { get; private set; } = new ItemData[3];
    private Slot[] armorSlots;
    private Slot[] hotbarSlots;
    public Hotbar Hotbar { get; private set; }



    public InventoryPlayerData()
    {
        Items = new ItemData[70];
    }



    public void SetupData(Tooltip _tooltip, Slot[] itemSlots, Slot[] _armorSlots, Hotbar hotbar)
    {
        SetupData(_tooltip, itemSlots);
        armorSlots = _armorSlots;
        Hotbar = hotbar;
        hotbarSlots = Hotbar.GetSlots();
    }



    protected override Godot.Collections.Array<Slot[]> GetSlots() => new Godot.Collections.Array<Slot[]> { itemSlots, armorSlots };



    #region Update items in inventory
    public override void UpdateInventoryItems()
    {
        base.UpdateInventoryItems();
        UpdateInventoryItemsInternal(armorSlots, Armor);
        UpdateInventoryItemsInternal(hotbarSlots, Items.Take(10).ToArray());
    }



    public override InventoryData GetInventoryData() => this;
    #endregion



    public override void MoveItem(InventoryItem item)
    {
        if (!MoveItemInternal(item, itemSlots, Items))
            MoveItemInternal(item, armorSlots, Armor);
    }

    
}