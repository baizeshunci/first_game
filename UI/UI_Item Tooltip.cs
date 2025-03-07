using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_ItemTooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemTypeText;
    [SerializeField] private TextMeshProUGUI itemDescription;

    [SerializeField] private int defaultFontSize = 10;
    public void ShowTooltip(ItemData_Equipment item)
    {
        if(item == null)
        {
            return;
        }
        
        itemNameText.text = item.itemName;
        itemTypeText.text = item.equipmentType.ToString();
        itemDescription.text = item.GetDescription();

        if(itemNameText.text.Length > 13)
        {
            itemNameText.fontSize = itemNameText.fontSize * 0.8f;
        }
        else
        {
            itemNameText.fontSize = defaultFontSize;
        }

        gameObject.SetActive(true);
    }

    public void HideTooltip() 
    {
        itemNameText.fontSize = defaultFontSize;
        gameObject.SetActive(false);
    }
}
