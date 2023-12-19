using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private ItemData itemData;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Vector2 velocity;

    private void OnValidate()
    {
        if (itemData == null)
            return;

        //change object in hierarchy with icon and name
        GetComponent<SpriteRenderer>().sprite = itemData.icon;
        gameObject.name = "Item object - "+ itemData.itemName;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            rb.velocity = velocity;
        }
    }

    public void SetupItem(ItemData _itemData, Vector2 _velocity)
    {
        itemData = _itemData;
        velocity = _velocity;
    }

    public void PickupItem()
    {
        Inventory.Instance.AddItem(itemData);
        Destroy(gameObject);
    }
}
