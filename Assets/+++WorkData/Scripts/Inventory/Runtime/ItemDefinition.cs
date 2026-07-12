//Volodymyr Pavlik
using UnityEngine;
public enum ItemType
{
    Normal,
    Special
}

[CreateAssetMenu(fileName = "New Item", menuName = "MotionBrain/Inventory/Item")]

public class ItemDefinition : ScriptableObject
{
    public ItemType itemType;
    
    public string id;

    [Min(1)]
    public int stackingCap = 1;
    
    public Sprite sprite;

    public string displayName;
    
    [TextArea(3,10)]
    public string description;
}
