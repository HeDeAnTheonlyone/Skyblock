using Godot;



[GlobalClass]
public abstract partial class InventoryData : Resource
{
    [ExportGroup("Properties")]
    [Export] public virtual ItemData[] Items { get; set; }
    public Slot[] ItemSlots { get; set; }



    public virtual void MoveItem(InventoryItem item) => MoveItemInternal(item, ItemSlots);



    protected bool MoveItemInternal(InventoryItem item, Slot[] slots)
    {
        Rect2 itemCenter = new Rect2(item.GlobalPosition + item.Size / 2, Vector2.Zero);
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



    public void AddItems(params ItemData[] items)
    {
        // TODO
    }
}