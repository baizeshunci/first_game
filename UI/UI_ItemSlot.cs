using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_StatSlot : MonoBehaviour  ,IPointerDownHandler,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField] protected Image itemImage;
    [SerializeField] protected TextMeshProUGUI itemText;

    protected UI ui;
    public InventoryItem item;

    protected virtual void Start()
    {
        ui = GetComponentInParent<UI>();
    }

    public void UpdateSlot(InventoryItem _newItem)
    {
        item = _newItem;

        itemImage.color = Color.white;

        if (item != null)
        {
            itemImage.sprite = item.data.icon;

            if (item.stackSize > 1)
            {
                itemText.text = item.stackSize.ToString();
            }
            else
            {
                itemText.text = "";
            }
        }
    }

    public void CleanUpSlot()
    {
        item = null;

        itemImage.sprite = null;
        itemImage.color = Color.clear;
        itemText.text = "";
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if ( item == null )
        {
            return;
        }

        if (Input.GetKey(KeyCode.Z))
        {
            Inventory.instance.RemoveItem(item.data);
            return;
        }

        if (item.data.itemType == ItemType.Equilpment)
        {
            Inventory.instance.EquipItem(item.data);
        }

        ui.itemTooltip.HideTooltip();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(item == null)
        {
            return;
        }
        // if (ui == null)
        // {
        //     Debug.LogError("UI is not found!");
        // }
        // if(ui.itemTooltip == null)
        // {
        //     Debug.LogError("UI_ItemTooltip is not found!");
        // }

        Vector2 mousePosition = Input.mousePosition;

        float xOffset = 0;
        float yOffset = 0;

        if(mousePosition.x > Screen.width / 2)
        {
            xOffset = -Screen.width / 10;
        }
        else
            xOffset = Screen.width / 10;
        if(mousePosition.y > Screen.height / 2)
        {
            yOffset = - Screen.height/10;
        }
        else
            yOffset = Screen.height/10;


        ui.itemTooltip.ShowTooltip(item.data as ItemData_Equipment);
        ui.itemTooltip.transform.position = Input.mousePosition + new Vector3(xOffset, yOffset, 0);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(item == null)
        {
            return;
        }

        ui.itemTooltip.HideTooltip();
    }
}
