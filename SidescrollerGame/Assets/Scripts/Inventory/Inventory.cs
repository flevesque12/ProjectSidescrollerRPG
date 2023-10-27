using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    private static Inventory _instance;

    public List<InventoryItem> inventoryItems;
    public Dictionary<ItemData, InventoryItem> inventoryDictionary;

    public List <InventoryItem> stash;
    public Dictionary <ItemData, InventoryItem> stashDictionary;

    [Header("Inventory UI")]
    [SerializeField] private Transform inventorySlotParent;
    [SerializeField] private Transform stashSlotParent;

    private UI_ItemSlot[] inventoryItemSlots;
    private UI_ItemSlot[] stashItemSlots;
    
    public static Inventory Instance
    {
        get
        {
            //if there is no instance create one
            if (_instance == null)
            {
                //check if an instance already exists in the scene
                _instance = FindObjectOfType<Inventory>();

                //if not create a new game objects and add the manager component
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("Inventory");
                    _instance = singletonObject.AddComponent<Inventory>();
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        //ensure there's only one instance of PlayerManager
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Start()
    {
        inventoryItems = new List<InventoryItem>();
        inventoryDictionary = new Dictionary<ItemData, InventoryItem>();

        stash = new List<InventoryItem>();
        stashDictionary = new Dictionary<ItemData, InventoryItem>();

        inventoryItemSlots = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();
        stashItemSlots = stashSlotParent.GetComponentsInChildren<UI_ItemSlot>();
    }

    private void UpdateSlotUI()
    {
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            inventoryItemSlots[i].UpdateSlot(inventoryItems[i]);
        }

        for (int i = 0; i < stash.Count; i++)
        {
            stashItemSlots[i].UpdateSlot(stash[i]);
        }
    }

    public void AddItem(ItemData _item)
    {
        if (_item.itemType == ItemType.Equipment)
        {
            AddIntoInventory(_item);
        }
        else if(_item.itemType == ItemType.Material)
        {
            AddIntoStash(_item);
        }

        UpdateSlotUI();
    }

    private void AddIntoStash(ItemData _item)
    {
        if (stashDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            stash.Add(newItem);
            stashDictionary.Add(_item, newItem);
        }
    }

    private void AddIntoInventory(ItemData _item)
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            //if the item is already in the dictionary stack it 
            value.AddStack();
        }
        else
        {
            //if not create new item in the inventory
            InventoryItem newItem = new InventoryItem(_item);
            inventoryItems.Add(newItem);
            inventoryDictionary.Add(_item, newItem);
        }
    }

    public void RemoveItem(ItemData _item)
    {
        if(inventoryDictionary.TryGetValue(_item,out InventoryItem value))
        {
            if (value.stackSize <= 1)
            {
                inventoryItems.Remove(value);
                inventoryDictionary.Remove(_item);
            }
            else
            {
                value.RemoveStack();
            }
        }

        if(stashDictionary.TryGetValue(_item,out InventoryItem stashValue))
        {
            if(stashValue.stackSize <= 1)
            {
                stash.Remove(stashValue);
                stashDictionary.Remove(_item);
            }
            else
            {
                stashValue.RemoveStack();
            }
        }

        UpdateSlotUI();
    }

}
