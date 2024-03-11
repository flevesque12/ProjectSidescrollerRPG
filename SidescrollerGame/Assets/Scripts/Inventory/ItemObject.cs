using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private ItemData itemData;
    [SerializeField] private Rigidbody2D rb;
    

    private void SetupVisuals(){
        if (itemData == null)
            return;

        //change object in hierarchy with icon and name
        GetComponent<SpriteRenderer>().sprite = itemData.icon;
        gameObject.name = "Item object - "+ itemData.itemName;
    }
   
    public void SetupItem(ItemData _itemData, Vector2 _velocity)
    {
        itemData = _itemData;
        rb.velocity = _velocity;

        SetupVisuals();
    }

    public void PickupItem()
    {
        Inventory.Instance.AddItem(itemData);
        Destroy(gameObject);
    }
}
