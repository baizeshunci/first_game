using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] private GameObject characterUI;
    [SerializeField] private GameObject skillTreeUI;
    [SerializeField] private GameObject craftUI;
    [SerializeField] private GameObject optionsUI;

    public UI_SkillToolTip skillToolTip;
    public UI_ItemTooltip itemTooltip;
    public UI_StatToolTip statToolTip;
    public UI_CraftWindow craftWindow;

    private void Awake()
    {
        SwitchTo(skillTreeUI); // we need this to assign events on skill tree slots tree slots before we assign events on skill scripts
    }

    // Start is called before the first frame update
    void Start()
    {
        SwitchTo(null);

        itemTooltip.gameObject.SetActive(false);
        statToolTip.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            SwitchWithKeyTo(characterUI);
        }

        if(Input.GetKeyDown(KeyCode.V))
        {
            SwitchWithKeyTo(craftUI);
        }

        if(Input.GetKeyDown(KeyCode.B))
        {
            SwitchWithKeyTo(skillTreeUI);
        }

        if(Input.GetKeyDown(KeyCode.N))
        {
            SwitchWithKeyTo(optionsUI);
        }
    }

    public void SwitchTo(GameObject _menu)
    {

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        if (_menu != null)
        {
            _menu.SetActive(true);
        }
    }

    public void SwitchWithKeyTo(GameObject _menu)
    {
        if(_menu != null && _menu.activeSelf)
        {
            _menu.SetActive(false);
            return;
        }

        SwitchTo(_menu);
    }
}
