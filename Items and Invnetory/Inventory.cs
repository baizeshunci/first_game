using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    public List<ItemData> startingItem;

    public List<InventoryItem> equipment;
    public Dictionary<ItemData_Equipment, InventoryItem> equipmentDictionary;

    public List<InventoryItem> inventory;
    public Dictionary<ItemData, InventoryItem> inventoryDictianory;

    public List<InventoryItem> stash;
    public Dictionary<ItemData,InventoryItem> stashDictianory;

    [Header("Inventory UI")]
    [SerializeField] private Transform inventorySlotParent;
    [SerializeField] private Transform stashSlotParent;
    [SerializeField] private Transform equipmentSlotParent;
    [SerializeField] private Transform statSlotParent;

    private UI_StatSlot[] stashItemSlot;
    private UI_StatSlot[] inventoryItemSlot;
    private UI_EquipmentSlot[] equipmentSlot;
    private UI_Stat_Slot[] statSlot;

    [Header("Items cooldown")]
    //private float flaskCooldown;
    private float lastTimeUsedFlask;
    private float lastTimeUsedArmor;

    private float flaskCooldown;
    private float armorCooldown;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        inventory = new List<InventoryItem>();
        inventoryDictianory = new Dictionary<ItemData, InventoryItem>();

        stash = new List<InventoryItem>();
        stashDictianory = new Dictionary<ItemData, InventoryItem>();

        equipment = new List<InventoryItem>();
        equipmentDictionary = new Dictionary<ItemData_Equipment, InventoryItem>();

        inventoryItemSlot = inventorySlotParent.GetComponentsInChildren<UI_StatSlot>();
        stashItemSlot = stashSlotParent.GetComponentsInChildren<UI_StatSlot>();
        equipmentSlot = equipmentSlotParent.GetComponentsInChildren<UI_EquipmentSlot>();
        statSlot = statSlotParent.GetComponentsInChildren<UI_Stat_Slot>();
        AddStartingItems();
    }

    private void AddStartingItems()
    {
        for (int i = 0; i < startingItem.Count; i++)
        {
            if(startingItem[i] != null)
            {
                AddItem(startingItem[i]);
            }
        }
    }

    public void EquipItem(ItemData _item)
    {
        ItemData_Equipment newEquipment = _item as ItemData_Equipment;
        InventoryItem newItem = new InventoryItem(newEquipment);

        ItemData_Equipment oldEquipment = null;

        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
        {
            if (item.Key.equipmentType == newEquipment.equipmentType)
            {
                oldEquipment = item.Key;
            }
        }

        if (oldEquipment != null)
        {
            UnequipItem(oldEquipment);
            AddItem(oldEquipment);
        }

        equipment.Add(newItem);
        equipmentDictionary.Add(newEquipment, newItem);
        newEquipment.AddModifiers();

        RemoveItem(_item);

        PlayerManager.instance.player.GetComponentInChildren<UI_HealthBar>().UpdateHealthUI();

        UpdateSlotUI();
        //GetComponent<PlayerStats>.
    }

    public void UnequipItem(ItemData_Equipment itemToRemove)
    {
        if (equipmentDictionary.TryGetValue(itemToRemove, out InventoryItem value))
        {
            //AddItem(itemToRemove);
            equipment.Remove(value);
            equipmentDictionary.Remove(itemToRemove);
            itemToRemove.RemoveModifiers();
        }
    }

    private void UpdateSlotUI()
    {
        for (int i = 0; i < equipmentSlot.Length; i++)
        {
            foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
            {
                if (item.Key.equipmentType == equipmentSlot[i].slotType)
                {
                    equipmentSlot[i].UpdateSlot(item.Value);
                }
            }
        }

        for (int i = 0; i < inventoryItemSlot.Length; i++)
        {
            inventoryItemSlot[i].CleanUpSlot();
        }

        for (int i = 0; i < stashItemSlot.Length; i++)
        {
            stashItemSlot[i].CleanUpSlot();
        }

        for (int i = 0; i < inventory.Count; i++)
        {
            inventoryItemSlot[i].UpdateSlot(inventory[i]);
        }

        for (int i = 0; i < stash.Count; i++)
        {
            stashItemSlot[i].UpdateSlot(stash[i]);
        }

        UpdateStatsUI();
    }

    public void UpdateStatsUI()
    {
        for (int i = 0; i < statSlot.Length; i++) // update info of stats in character UI
        {
            statSlot[i].UpdateStatValueUI();
        }
    }

    public void AddItem(ItemData _item)
    {
        if (_item.itemType == ItemType.Equilpment && CanAddItem())
        {
            AddToInventory(_item);
        }
        else if (_item.itemType == ItemType.Material)
        {
            AddToStash(_item);
        }

        UpdateSlotUI();
    }

    private void AddToStash(ItemData _item)
    {
        if (stashDictianory.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            stash.Add(newItem);
            stashDictianory.Add(_item, newItem);
        }
    }

    private void AddToInventory(ItemData _item)
    {
        if (inventoryDictianory.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            inventory.Add(newItem);
            inventoryDictianory.Add(_item, newItem);
        }
    }

    public void RemoveItem(ItemData _item)
    {
        if (inventoryDictianory.TryGetValue(_item, out InventoryItem value)) 
        { 
            if(value.stackSize <=1)
            {
                inventory.Remove(value);
                inventoryDictianory.Remove(_item);
            }
            else
            {
                value.RemoveStack();
            }
        }

        if(stashDictianory.TryGetValue(_item, out InventoryItem stashValue))
        {
            if(stashValue.stackSize <=1)
            {
                stash.Remove(stashValue);
                stashDictianory.Remove(_item);
            }
            else
            {
                stashValue.RemoveStack();
            }    
        }

        UpdateSlotUI();
    }

    public bool CanAddItem()
    {
        if(inventory.Count >= inventoryItemSlot.Length)
        {
            Debug.Log("Inventory full");
            return false;
        }
        return true;
    }

    public bool CanCraft(ItemData_Equipment _itemCraft, List<InventoryItem> _requireMatherials)
    {
        List<InventoryItem> materialToMove = new List<InventoryItem>();

        for (int i = 0; i < _requireMatherials.Count; i++)
        {
            if (stashDictianory.TryGetValue(_requireMatherials[i].data,out InventoryItem stashValue))
            {
                //add this to used material
                if(stashValue.stackSize < _requireMatherials[i].stackSize)
                {
                    Debug.Log("not enough materials");
                    return false;
                }
                else
                {
                    for (int j = 0; j < _requireMatherials[i].stackSize; j++)
                    {
                        materialToMove.Add(stashValue);
                    }
                }
            }
            else
            {
                Debug.Log("not enough materials");
                return false;
            }
        }

        for(int i = 0; i < materialToMove.Count; i++)
        {
            RemoveItem(materialToMove[i].data);
        }

        AddItem(_itemCraft);
        Debug.Log("Here is your item" + _itemCraft.name);

        return true;
    }

    public List<InventoryItem> GetEquipmentList() => equipment;

    public List<InventoryItem> GetStashList() => stash;

    public ItemData_Equipment GetEquipment(EquipmentType _type)
    {
        //Debug.Log(_type);
        ItemData_Equipment equipedItem = null;
        //Debug.Log("run 1");
        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
        {
            Debug.Log("run 2");
            if (item.Key.equipmentType == _type)
            {
                //Debug.Log("run 3");
                equipedItem = item.Key;
                //Debug.Log("run 4");
            }
        }

        return equipedItem;
    }

    public void UseFlask()
    {
        ItemData_Equipment currentFlask = GetEquipment(EquipmentType.Flask);

        if (currentFlask == null)
        {
            return;
        }

        bool canUseFlask = Time.time > lastTimeUsedFlask + flaskCooldown;

        if (canUseFlask)
        {
            flaskCooldown = currentFlask.itemCooldown;
            currentFlask.Effect(null);
            lastTimeUsedFlask = Time.time;
        }
        else
        {
            Debug.Log("Flask on cooldown.");
        }
        // if can use // cooldown

        // use flask
        // set cooldown
    }

    public bool CanUseArmor()
    {
        ItemData_Equipment currenArmor = GetEquipment(EquipmentType.Armor);

        if (Time.time > lastTimeUsedArmor + armorCooldown)
        {
            armorCooldown = currenArmor.itemCooldown;
            lastTimeUsedArmor = Time.time;
            return true;
        }
        return false;
    }
}
