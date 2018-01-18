using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FireBullet : MonoBehaviour {
    public float speed = 10.0f;
    public float DestroyTime = 5.0f;
    private Vector3 velocity;
    public GameObject BoomEffectPrefab;
    // Use this for initialization
    void Start () {
        Vector3 fwd = transform.TransformDirection(Vector3.forward);//面朝的方向作为力的方向  
        velocity = fwd ;
        Destroy(gameObject, DestroyTime);
    }
	
	void FixedUpdate()
    {
        transform.position += velocity * 0.5f;
    }
    void Update()
    {

    }
    void OnTriggerEnter(Collider collision)//（刚体碰撞检测/触发器检测）
    {
        if (collision.gameObject.tag == "Monst")
        {
            GameObject DieEffect = GameObject.Instantiate(BoomEffectPrefab, transform.position, transform.rotation);//实例化粒子效果
            Destroy(DieEffect, 1);
            Destroy(gameObject);//删除子弹
        }
    }
}
