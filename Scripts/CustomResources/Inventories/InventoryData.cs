using Godot;



[GlobalClass]
public abstract partial class InventoryData : Resource
{
    [ExportGroup("Properties")]
    [Export] public virtual ItemData[] Items { get; set; }



    public virtual void DisplayItemsInSlots(Slot[] slots, ItemData[] items)
    {
        PackedScene invItem = (PackedScene)GD.Load("res://Objects/Inventory/InventoryItem.tscn");

        for (int i = 0; i < items.Length; i++)
        {   
            if (items[i] == null)
            {
                if (slots[i].GetChildCount() > 1)
                    slots[i].GetNode<InventoryItem>("InventoryItem").QueueFree();

                continue;
            }

            if (slots[i].GetChildCount() > 1)
            {
                InventoryItem item = slots[i].GetNode<InventoryItem>("InventoryItem");

                if (item.Data != items[i])
                {
                    item.QueueFree();
                    InventoryItem invItemInstance = invItem.Instantiate() as InventoryItem;
                    invItemInstance.Data = items[i];
                    slots[i].AddChild(invItemInstance);
                }
            }
            else
            {
                InventoryItem invItemInstance = invItem.Instantiate() as InventoryItem;
                invItemInstance.Data = items[i];
                slots[i].AddChild(invItemInstance);
            }
        }
    }



    public virtual void MoveItems()
    {
        // TODO
    }



    public void AddItems(params ItemData[] items)
    {
        // TODO
    }
}