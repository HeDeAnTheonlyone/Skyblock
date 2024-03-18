using System.Linq;
using Godot;



[GlobalClass, Icon("")]
public partial class LootTable : Resource
{
    [Export] public ItemData[] Table { get; set; }
    [Export] int Rolls { get; set; }



    public ItemData[] PlainLoot() => Table.Select(item => item.Duplicate(true) as ItemData).ToArray();

    // TODO Add additional overlaods for more specificc loots
}