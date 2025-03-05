using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private int possibleItemDrop;
    [SerializeField] private ItemData[] possibleDrop;
    private List<ItemData> dropList = new List<ItemData>();

    [SerializeField] private GameObject dropPrefab;
    //[SerializeField] private ItemData item;

    public virtual void GenerateDrop()
    {
        for (int i = 0; i < possibleDrop.Length; i++)
        {
            if (Random.Range(0, 100) <= possibleDrop[i].dropChance)
            {
                dropList.Add(possibleDrop[i]);
            }
        }
        //必掉两个物品
        //while (dropList.Count <= possibleItemDrop)
        //{
        //    for (int i = 0; i < possibleDrop.Length; i++)
        //    {
        //        if (Random.Range(0, 100) <= possibleDrop[i].dropChance)
        //        {
        //            dropList.Add(possibleDrop[i]);
        //        }
        //    }
        //}
        if (dropList.Count >= possibleItemDrop)
        {
            for (int i = 0; i < possibleItemDrop; i++)
            {
                ItemData randomItem = dropList[Random.Range(0, dropList.Count - 1)];

                dropList.Remove(randomItem);
                DropItem(randomItem);
            }
        }
        else
        {
            for (int i = 0; i < dropList.Count;i++)
            {
                ItemData randomItem = dropList[i];
                dropList.Remove(randomItem);
                DropItem(randomItem);
            }
        }
    }
    protected void DropItem(ItemData _itemData)
    {
        GameObject newDrop = Instantiate(dropPrefab,transform.position,Quaternion.identity);

        Vector2 randomVelocity = new Vector2(Random.Range(-5,5),Random.Range(12,15));

        newDrop.GetComponent<ItemObject>().SetUpItem(_itemData,randomVelocity);
    }
}
