using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConLibra : MonoBehaviour {
    public float max_life = 100.0f;//士兵最大生命力
    public float current_life = 100.0f;//当前士兵的生命
    private static ConLibra inst = null;
    public int damage = 15;//子弹的攻击力
    public float speed = 5.0f;//子弹速度
    public GameObject BoomEffectPrefab;//击中的粒子效果
    private Transform targe;//目标
    public static ConLibra GetInstance()
    {
        return inst;
    }
    // Use this for initialization
    void Start () {
        inst = this;
	}

    // Update is called once per frame
    void Update()
    {
        //如果怪物被其它炮塔消灭了或者怪物到了终点，子弹还没击中怪物，就销毁子弹
        if (targe == null)
        {
            Die2();
            return;
        }
        transform.LookAt(targe.position);
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
    //子弹击中怪物扣血
    void OnTriggerEnter(Collider monster)
    {
        if (monster.tag == "Monst")
        {
            monster.GetComponent<Monste>().TakeDamage(damage);//计算伤害
            Die1();
        }
    }
    private int m_LibraPower = 50;//士兵战斗力
    public int isLibraPower
    {
        get
        {
            return m_LibraPower;
        }
        set
        {
            m_LibraPower = value;
        }
    }
    void Die1()
    {
        GameObject DieEffect = GameObject.Instantiate(BoomEffectPrefab, transform.position, transform.rotation);//实例化粒子效果
        Destroy(DieEffect, 1);
        Destroy(this.gameObject);//销毁子弹
    }
    void Die2()
    {
        Destroy(this.gameObject);//销毁子弹
    }
}
