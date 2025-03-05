using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ThunderStrike_Controll : MonoBehaviour
{
    [SerializeField] private CharacterStats targetStats;
    [SerializeField] private float speed;
    private int damage;

    private Animator anim;
    private bool triggered;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    public void Setup(int _damage,CharacterStats _targetStats)
    {
        damage = _damage;
        targetStats = _targetStats;
    }

    // Update is called once per frame
    void Update()
    {
        if(!targetStats)
        {
            Destroy(gameObject);
            return;
        }

        if (triggered)
        {
            return;
        }

        transform.position = Vector2.MoveTowards(transform.position,targetStats.transform.position,speed * Time.deltaTime);
        transform.right = targetStats.transform.position - transform.position;

        if (Vector2.Distance(transform.position,targetStats.transform.position) < .1f)
        {
            anim.transform.localPosition = new Vector3(0, .3f);
            anim.transform.localRotation = Quaternion.Euler(0, 0, -45);
            transform.localRotation = Quaternion.Euler(0, 0, -45);
            transform.localScale = new Vector3(3, 3);
            triggered = true;
            anim.SetTrigger("Hit");
            Invoke("DamageAndSelfDestory", .2f);
        }
    }

    private void DamageAndSelfDestory()
    {
        targetStats.ApplyShock(true);
        targetStats.TakeDamage(damage);
        Destroy(gameObject, .4f);
    }
}
