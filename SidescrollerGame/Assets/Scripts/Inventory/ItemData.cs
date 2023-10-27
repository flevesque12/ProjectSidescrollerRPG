using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Material,
    Equipment
}

[CreateAssetMenu(fileName = "New item data",menuName ="Data/Item")]
public class ItemData : ScriptableObject
{
    public ItemType itemType;
    public string itemName;
    public Sprite icon;
}
