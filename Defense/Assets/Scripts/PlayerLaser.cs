using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLaser : MonoBehaviour
{
    private Light light;
    private LineRenderer line;
    public GameObject DieEffect;
    private GameObject playerLaser;
    public GameObject FireBud;
    private float LaserPower=1;
    int litghtcount;
    void Awake()
    {
        line = this.GetComponent<LineRenderer>();
        line.enabled = false;
    }
    void Start()
    {
    }
    void Update()
    {        
    }
    public void shoot()//开火  
    {
        //发光效果  
        //粒子系统播放 
        if (litghtcount<=0)
        {
            playerLaser = Instantiate(DieEffect, FireBud.transform.position, FireBud.transform.rotation);//实例化粒子效果
            playerLaser.transform.SetParent(FireBud.transform);
            playerLaser.transform.localScale = new Vector3(2,2,2);//设定预制体的比例
            litghtcount++;
        }
        //射击音乐  
        //激光开始  
        line.enabled = true;//激光显示  
        line.SetPosition(0, transform.position);//设定激光起点  
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hitinfo;
        if (Physics.Raycast(ray, out hitinfo))
        {
            line.SetPosition(1, hitinfo.point); //设置终点位置                
        }
        else
        {
            line.SetPosition(1, transform.position + transform.forward * 100);//射击到100m（终点位置）
        }
        bool isMonst= Physics.Raycast(ray, out hitinfo, 100, LayerMask.GetMask("monster"));
        if (isMonst)
        {
            hitinfo.collider.GetComponent<Monste>().TakeDamage(LaserPower);
            Debug.Log("射击中");
        }
    }
    public void shutshoot()//关闭发光和激光等效果  
    {
        line.enabled = false;
        Destroy(playerLaser);
        litghtcount = 0;   
    }
}
