using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Blankhole_HotKey_Controller : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private KeyCode myHotKey;
    [SerializeField] private TextMeshProUGUI myText;

    [SerializeField] private Transform myEnemy;
    [SerializeField] private Blackhole_Skill_Controller blackHole;

    public void SetupHotKey(KeyCode _myNewHotKey,Transform _myEnemy,Blackhole_Skill_Controller _myBlackHole)
    {
        sr = GetComponent<SpriteRenderer>();
        myText = GetComponentInChildren<TextMeshProUGUI>();

        myEnemy = _myEnemy;
        blackHole = _myBlackHole;

        myHotKey = _myNewHotKey;
        myText.text = _myNewHotKey.ToString();
    }

    private void Update()
    {
        if (Input.GetKeyDown(myHotKey))
        {
            blackHole.AddEnemyToList(myEnemy);

            myText.color = Color.clear;
            sr.color = Color.clear;
        }
    }
}
