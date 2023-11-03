using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_CraftSlot : UI_ItemSlot
{
    private void OnEnable()
    {
        UpdateSlot(item);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        //inventory craft item data

        ItemData_Equipment craftData = item.data as ItemData_Equipment;

        Inventory.Instance.CanCraftItem(craftData, craftData.craftingMaterials);
    }
}
