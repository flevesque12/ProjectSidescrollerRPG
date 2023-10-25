using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New item data",menuName ="Data/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
}
