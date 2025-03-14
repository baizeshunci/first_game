using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;

public enum RCShockWave
{
    Regular,
    Bounce,
    Pierce,
    Spin
}

public class RecircleShockWave : Skill
{
    public RCShockWave RCSW = RCShockWave.Regular;

    [Header("Bounce info ")]
    [SerializeField] private UI_SkillTreeSlot bounceUnlockButton;
    [SerializeField] private int bounceAmount;
    [SerializeField] private float bouceGravity;
    [SerializeField] private float bouceSpeed;

    [Header("Perice info")]
    [SerializeField] private UI_SkillTreeSlot pierceUnlockButton;
    [SerializeField] private int pierceAmount;
    [SerializeField] private float pierceGravity;

    [Header("Spin info")]
    [SerializeField] private UI_SkillTreeSlot spinUnlockButton;
    [SerializeField] private float hitCooldown = .35f;
    [SerializeField] private float maxTravelDistance = 7;
    [SerializeField] private float spinDuration = 2;
    [SerializeField] private float spinGravity = 1;

    [Header("Skill info")]
    [SerializeField] private UI_SkillTreeSlot recircleShockWaveUnlockButton;
    public bool recircleUnlocked { get ; private set; }
    [SerializeField] private GameObject recircleShockWavePrefab;
    [SerializeField] private Vector2 launchDir;
    [SerializeField] private float recircleShockWaveGravity;
    [SerializeField] public float coolDown = 2f;
    [SerializeField] public float coolDownRecord = -2f;
    [SerializeField] private float freezeTimeDuration;
    [SerializeField] private float returnSpeed;

    [Header("Passsive skills")]
    [SerializeField] private UI_SkillTreeSlot timeStopUnlockButton;
    public bool timeStopUnlocked { get ; private set; }
    [SerializeField] private UI_SkillTreeSlot vulnurableUnlockButton;
    public bool vulnurableUnlocked { get ; private set; }



    public Vector2 playerPosition;

    private Vector2 finalDir;

    [Header("Aim dots")]
    [SerializeField] private int numberOfDots;
    [SerializeField] private float spaceBetweenDots;
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private Transform dotParent;

    private GameObject[] dots;

    protected override void Start()
    {
        base.Start();

        GenerateDots();

        recircleShockWaveUnlockButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(UnlockRecircleShockWave);
        bounceUnlockButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(UnlockBounce);
        pierceUnlockButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(UnlockPierce);
        spinUnlockButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(UnlockSpin);
        timeStopUnlockButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(UnlockTimeStop);
        vulnurableUnlockButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(UnlockVolnurable);
    }

    private void SetupGrativty()
    {
        switch(RCSW)
        {
            //case RCShockWave.Regular:
            //    recircleShockWaveGravity = bouceGravity; break;
            case RCShockWave.Bounce:
                recircleShockWaveGravity = bouceGravity; break;
            case RCShockWave.Pierce:
                recircleShockWaveGravity = pierceGravity;break;
            case RCShockWave.Spin:
                recircleShockWaveGravity = spinGravity; break;
        }
            
    }

    protected override void Update()
    {
        SetupGrativty();
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            finalDir = new Vector2(AimDirection().normalized.x * launchDir.x ,AimDirection().normalized.y * launchDir.y);
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            
            for (int i = 0; i < dots.Length; i++)
            {
                dots[i].transform.position = DotsPosition(i * spaceBetweenDots);
            }
        }
    }

    public void CreateRecircleShockWave()
    {
        GameObject newRecircleShockWave = Instantiate(recircleShockWavePrefab, PlayerManager.instance.player.recircleShockWavePosition.position, transform.rotation);
        RecircleShockWaveSkill_controller newRecircleShockWaveScript = newRecircleShockWave.GetComponent<RecircleShockWaveSkill_controller>();


        if(RCSW == RCShockWave.Bounce)
        {
            newRecircleShockWaveScript.SetupBounce(true, bounceAmount,bouceSpeed);
        }
        else if(RCSW == RCShockWave.Pierce)
        {
            newRecircleShockWaveScript.SetupPierce(pierceAmount);
        }
        else if(RCSW ==RCShockWave.Spin)
        {
            newRecircleShockWaveScript.SetupSpin(true,maxTravelDistance,spinDuration,hitCooldown);
        }

        newRecircleShockWaveScript.SetuupRecircleShockWave(finalDir,recircleShockWaveGravity, player,freezeTimeDuration,returnSpeed);

        player.AssignNewRCShockWave(newRecircleShockWave);

        DotActive(false);
    }

    #region Unlock region

    private void UnlockTimeStop()
    {
        if(timeStopUnlockButton.unlocked)
        {
            //RCSW = RCShockWave.Regular;
            timeStopUnlocked = true;
        }
    }

    private void UnlockVolnurable()
    {
        if(vulnurableUnlockButton.unlocked)
        {
            //RCSW = RCShockWave.Regular;
            vulnurableUnlocked = true;
        }
    }

    private void UnlockRecircleShockWave()
    {
        if(recircleShockWaveUnlockButton.unlocked)
        {
            RCSW = RCShockWave.Regular;
            recircleUnlocked = true;
        }
    }

    private void UnlockBounce()
    {
        if(bounceUnlockButton.unlocked)
        {
            RCSW = RCShockWave.Bounce;
        }
    }

    private void UnlockPierce()
    {
        if(pierceUnlockButton.unlocked)
        {
            RCSW = RCShockWave.Pierce;
        }
    }

    private void UnlockSpin()
    {
        if(spinUnlockButton.unlocked)
        {
            RCSW = RCShockWave.Spin;
        }
    }



    #endregion
    #region Aim region
    public Vector2 AimDirection()
    {
        playerPosition.x = player.recircleShockWavePosition.position.x;
        playerPosition.y = player.recircleShockWavePosition.position.y;
        Vector2 mousrPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousrPosition - playerPosition;

        return direction;
    }

    public void DotActive(bool _isActive)
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].SetActive(_isActive);
        }
    }

    private void GenerateDots()
    {
        dots = new GameObject[numberOfDots];
        for (int i = 0; i < numberOfDots; i++)
        {
            //some action
            dots[i] = Instantiate(dotPrefab, player.recircleShockWavePosition.position, Quaternion.identity, dotParent);
            dots[i].SetActive(false);
        }
    }

    private Vector2 DotsPosition(float t)
    {
        Vector2 position = (Vector2)player.recircleShockWavePosition.position + new Vector2(
            AimDirection().normalized.x * launchDir.x,
            AimDirection().normalized.y * launchDir.y) * t + .5f * (Physics2D.gravity * recircleShockWaveGravity)*(t*t);

        return position;
    }
    #endregion
}
