using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private ItemData itemData;

    private void OnValidate()
    {
        if (itemData == null)
            return;

        //change object in hierarchy with icon and name
        GetComponent<SpriteRenderer>().sprite = itemData.icon;
        gameObject.name = "Item object - "+ itemData.itemName;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            Inventory.Instance.AddItem(itemData);
            Destroy(gameObject);
        }
    }
}
