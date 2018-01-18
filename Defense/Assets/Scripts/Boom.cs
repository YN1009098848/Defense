using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boom : MonoBehaviour {
    public float DestroyTime = 15.0f;

    void Start()
    {
        Destroy(gameObject, DestroyTime);
    }
    void Update()
    {

    }
    void OnTriggerEnter(Collider collision)//（刚体碰撞检测/触发器检测）
    {
        if (collision.gameObject.tag == "Monst")
        {
            print("击中");
            Destroy(gameObject);//删除障碍
        }
    }
}
