using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public Player player;

    public int cuurency;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public bool HaveEnoughMoney(int _price)
    {
        if(_price > cuurency)
        {
            Debug.Log("Not enough money");
            return false;
        }
        else
        {
            cuurency -= _price;
            return true;
        }
    }
}
