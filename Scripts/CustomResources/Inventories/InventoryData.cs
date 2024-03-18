using System;
using System.Linq;
using Godot;



[GlobalClass]
public abstract partial class InventoryData : Resource
{
    [ExportGroup("Properties")]
    [Export] public ItemData[] Items { get; set; }
    public Slot[] ItemSlots { get; set; }
    protected readonly PackedScene invItem = GD.Load("res://Objects/Inventory/InventoryItem.tscn") as PackedScene;



    #region Update items in inventory
    public virtual void UpdateInventoryItems() => UpdateInventoryItemsInternal(ItemSlots, Items);



    protected void UpdateInventoryItemsInternal(Slot[] slots, ItemData[] items)
    {
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
                    UpdateInventoryItemsInstantiastor(slots[i], items[i]);
                }
            }
            else
                UpdateInventoryItemsInstantiastor(slots[i], items[i]);
        }
    }



    private void UpdateInventoryItemsInstantiastor(Slot slot, ItemData item)
    {
        InventoryItem invItemInstance = invItem.Instantiate() as InventoryItem;
        invItemInstance.Data = item;
        invItemInstance.inventory = GetInventoryData();
        slot.AddChild(invItemInstance);
    }



    public virtual InventoryData GetInventoryData() => this;
    #endregion



    public virtual void MoveItem(InventoryItem item) => MoveItemInternal(item, ItemSlots);



    protected bool MoveItemInternal(InventoryItem item, Slot[] slots)
    {
        Rect2 itemCenter = new Rect2(item.GlobalPosition + item.Size / 2, 25, 25);
        Slot oldSlot = item.GetParent<Slot>();

        foreach (Slot slot in slots)
        {
            Rect2 slotArea = new Rect2(slot.GlobalPosition, slot.Size);

            if (slotArea.Intersects(itemCenter))
            {
                if (!slot.MatchType(item.Data.SlotType))
                    return false;

                oldSlot.RemoveChild(item);

                if (slot.GetChildCount() > 1)
                {
                    InventoryItem occupyingItem = slot.GetNode<InventoryItem>("InventoryItem");

                    slot.RemoveChild(occupyingItem);
                    slot.AddChild(item);
                    oldSlot.AddChild(occupyingItem);
                }
                else
                    slot.AddChild(item);

                return true;
            }
        }

        return false;
    }



    public void AddItems(params ItemData[] newItems)
    {
        foreach (ItemData newItem in newItems)
        {
            ItemData[] itemStacks = Items.Where(item => item != null && item.Name == newItem.Name && item.StackSize < item.MaxStackSize).ToArray();

            int index = 0;
            while(newItem.StackSize > 0 && index < itemStacks.Length)
            {
                int stackSpace = Mathf.Clamp(itemStacks[index].MaxStackSize - itemStacks[index].StackSize, 0, newItem.StackSize);

                itemStacks[index].StackSize += stackSpace;
                newItem.StackSize -= stackSpace;

                index++;
            }

            if (newItem.StackSize == 0)
                continue;

            int emptySlotIndex = Array.IndexOf(Items, null);
            
            if (emptySlotIndex == -1)
                continue; //TODO If no empty slots in the Inventory. Not yet implemented to drop the items.
            else
                        Items[emptySlotIndex] = newItem;
        }

        UpdateInventoryItems();
    }
}