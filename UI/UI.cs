using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchTo(GameObject _menu)
    {

        for (int i = 0; i < transform.childCount; i++)
        {
            Debug.Log("run 1");
            transform.GetChild(i).gameObject.SetActive(false);
        }

        if (_menu != null)
        {
            Debug.Log("run 2");
            _menu.SetActive(true);
            _menu.transform.parent?.gameObject.SetActive(true);
            Debug.Log(_menu);
        }
    }
}
