using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

    public class RemFire:MonoBehaviour
{
    //子弹预制体
    public GameObject bullet;

    //推动力大小
    public float thrust = 50.0f;

    //开火速率。
    public float fireRate = 10f;

    //开火时间，初始化为零。
    public float fireTime = 0.0f;
    public GameObject Player;
    public GameObject FireBuilding;
    void Start()
    {

    }
    void Update()
    {
    }
    public void CheckShoot()
    {
        if (Time.time > fireTime)
        {
            //根据开火速率调整开火时间
            fireTime = Time.time + fireRate;

            //实例化一个bullet的克隆体。
            GameObject clone = Instantiate(bullet, FireBuilding.transform.position, FireBuilding.transform.rotation)as GameObject;
            clone.transform.SetParent(Player.transform.parent);//将子弹的父级设置为当前transform的父级
            //获取克隆体的刚体组件。
            Rigidbody rb = clone.GetComponent<Rigidbody>();

            //给克隆体的刚体组件一个推动力。
            rb.AddForce(clone.transform.forward * thrust);
        }
    }
}

