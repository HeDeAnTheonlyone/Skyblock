using System;
using System.Linq;
using Godot;



[GlobalClass, Icon("res://Assets/Icons/Inventory.svg")]
public abstract partial class InventoryData : Resource
{
    [ExportGroup("Properties")]
    [Export] public ItemData[] Items { get; set; }
    public Slot[] ItemSlots { get; set; }
    //private Control itemBuffer;
    protected readonly PackedScene invItem = GD.Load("res://Objects/Inventory/InventoryItem.tscn") as PackedScene;



    //public void SetItemBufferNode(Control bufferNode) => itemBuffer = bufferNode;



    // public virtual void AttachToBuffer(InventoryItem item) => AttachToBufferInternal(item, Items);
    // protected void AttachToBufferInternal(InventoryItem item, ItemData[] itemList)
    // {
    //     itemList[item.GetIndex()] = null;
    //     Vector2 size = item.Size;



    //     item.GetParent().RemoveChild(item);
    //     itemBuffer.AddChild(item);
    //     item.Size = size;
    // }



    #region Update items in inventory
    public virtual void UpdateInventoryItems() => UpdateInventoryItemsInternal(ItemSlots, Items);
    protected void UpdateInventoryItemsInternal(Slot[] slots, ItemData[] items)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
            {
                if (slots[i].GetChildCount() > 1)
                    slots[i].GetChildren().OfType<InventoryItem>().First().QueueFree();

                continue;
            }

            if (slots[i].GetChildCount() > 1)
            {
                InventoryItem item = slots[i].GetChildren().OfType<InventoryItem>().First();

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



    public virtual void MoveItem(InventoryItem item) => MoveItemInternal(item, ItemSlots, Items);
    protected bool MoveItemInternal(InventoryItem item, Slot[] slots, ItemData[] itemList)
    {
        int oldIndex = Array.IndexOf(itemList, item.Data);
        Rect2 itemCenter = new Rect2(item.GlobalPosition + item.Size / 2, 25, 25);
        Slot slot = slots.FirstOrDefault(slot => new Rect2(slot.GlobalPosition, slot.Size).Intersects(itemCenter), null);

        if (slot == null)
            return false;

        if (!slot.MatchType(item.Data.SlotType))
            return true;

        if (slot.GetChildCount() > 1)
        {
            ItemData occupyingItem = itemList[slot.GetIndex()];

            if (ReferenceEquals(item.Data, occupyingItem))
                return true;

            if (occupyingItem.CanMergeWith(item.Data))
            {
                AddItem(item.Data, occupyingItem, item.Data.StackSize);
                UpdateInventoryItems();
                return true;
            }

            itemList.Swap(oldIndex, Array.IndexOf(itemList, occupyingItem));
        }
        else
        {
            itemList[oldIndex] = null;
            itemList[slot.GetIndex()] = item.Data;
        }

        UpdateInventoryItems();
        return true;
    }



    public void SplitItem(InventoryItem splitItem, int amount) => SplitItemInternal(splitItem, ItemSlots, amount, Items);
    protected virtual bool SplitItemInternal(InventoryItem splitItem, Slot[] slots, int amount, ItemData[] itemList)
    {
        Rect2 itemCenter = new Rect2(splitItem.GlobalPosition + splitItem.Size / 2, 25, 25);
        Slot slot = slots.FirstOrDefault(slot => new Rect2(slot.GlobalPosition, slot.Size).Intersects(itemCenter), null);

        if (slot == null)
            return false;

        if (!slot.MatchType(splitItem.Data.SlotType))
            return true;

        if (slot.GetChildCount() > 1)
        {
            ItemData occupyingItem = slot.GetChildren().OfType<InventoryItem>().First().Data;

            if (!occupyingItem.CanMergeWith(splitItem.Data))
                return true;

            AddItem(splitItem.Data, occupyingItem, amount);
        }
        else
            AddItem(itemList, slot.GetIndex(), splitItem.Data, amount);

        UpdateInventoryItems();
        return true;
    }



    /// <summary>
    /// Adds a variable amount of differnt items only to the main item list of the inventory. It fills the inventory up from the top left to the bottom right.
    /// </summary>
    /// <param name="newItems"></param>
    public void AddItem(params ItemData[] newItems)
    {
        foreach (ItemData newItem in newItems)
        {
            ItemData[] itemStacks = Items.Where(item => item != null && item.CanMergeWith(newItem)).ToArray();

            int index = 0;
            while (newItem.StackSize > 0 && index < itemStacks.Length)
            {
                int stackSize = Mathf.Clamp(itemStacks[index].MaxStackSize - itemStacks[index].StackSize, 0, newItem.StackSize);

                itemStacks[index].StackSize += stackSize;
                newItem.StackSize -= stackSize;

                index++;
            }

            if (newItem.StackSize < 1)
                continue;

            int emptySlotIndex = Array.IndexOf(Items, null);

            if (emptySlotIndex == -1)
                continue;
            else
            {
                Items[emptySlotIndex] = newItem.Duplicate(true) as ItemData;
                newItem.StackSize = 0;
            }
        }

        UpdateInventoryItems();
    }



    /// <summary>
    /// Adds a single item to a specific slot in the inventory.
    /// </summary>
    /// <param name="itemList"></param>
    /// <param name="index"></param>
    /// <param name="item"></param>
    /// <param name="amount"></param>
    protected void AddItem(ItemData[] itemList, int index, ItemData item, int amount)
    {
        int splitSize = Mathf.Clamp(amount, 0, item.StackSize);

        ItemData splitItem = item.Duplicate(true) as ItemData;
        splitItem.StackSize = splitSize;
        itemList[index] = splitItem;
        item.StackSize -= splitSize;
    }



    /// <summary>
    /// Adds a specified amount from a single item's stacksize to another item.
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="amount"></param>
    protected void AddItem(ItemData from, ItemData to, int amount)
    {
        int splitSize = Mathf.Clamp(to.MaxStackSize - to.StackSize, 0, Mathf.Min(from.StackSize, amount));
        
        from.StackSize -= splitSize;
        to.StackSize += splitSize;
    }
}