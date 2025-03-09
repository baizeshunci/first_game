using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SkillToolTip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI skillText;


    public void ShowToolTip(string _text)
    {
        skillText.text = _text;
        gameObject.SetActive(true);
    }

    public void HideToolTip() => gameObject.SetActive(false);
}
