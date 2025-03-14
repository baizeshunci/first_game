using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SkillTreeSlot : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    private UI ui;

    [SerializeField] private int skillPrice;
    [SerializeField] private string skillName;
    [TextArea]
    [SerializeField] private string skillDescription;
    [SerializeField] private Color lockedSkillColor;


    public bool unlocked;

    [SerializeField] private UI_SkillTreeSlot[] shockBeUnlocked;
    [SerializeField] private UI_SkillTreeSlot[] shockBeLocked;

    [SerializeField] private Image skillImage;

    private void OnValidate()
    {
        gameObject.name = "SkillTreeSlot_UI - " + skillName;
    }

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => UnlockSkillSlot());
    }

    private void Start()
    {
        skillImage = GetComponent<Image>();

        ui = GetComponentInParent<UI>();

        skillImage.color = lockedSkillColor;

    }

    public void UnlockSkillSlot()
    {
        if(PlayerManager.instance.HaveEnoughMoney(skillPrice)==false)
        {
            return;
        }
        
        Debug.Log("Slot unlocked");

        for(int i = 0; i < shockBeUnlocked.Length; i++)
        {
            if(shockBeUnlocked[i].unlocked == false)
            {
                Debug.Log("Can't unlock skill slot");
                return;
            }
        }
    
        for(int i = 0; i < shockBeLocked.Length; i++)
        {
            if(shockBeLocked[i].unlocked == true)
            {
                Debug.Log("Can unlock skill slot");
                return;
            }
        }

        unlocked = true;
        skillImage.color = Color.white;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.skillToolTip.ShowToolTip(skillName + "\n" + skillDescription);

        Vector2 mousePosition = Input.mousePosition;

        float xOffset = 0;
        float yOffset = 0;

        if(mousePosition.x > Screen.width / 2)
        {
            xOffset = -200;
        }
        else
        {
            xOffset = 200;
        }
        if(mousePosition.y > Screen.height / 2)
        {
            yOffset = -200;
        }
        else
        {
            yOffset = 200;
        }

        ui.skillToolTip.transform.position = new Vector2(mousePosition.x + xOffset, mousePosition.y + yOffset);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillToolTip.HideToolTip();
    }
}
