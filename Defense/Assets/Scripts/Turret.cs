using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {
    public List<GameObject> enemys = new List<GameObject>();
    void OnTriggerEnter(Collider col)
    {
        if (col.tag=="Monst")
        {
            enemys.Add(col.gameObject);
            Debug.Log("进入射程的怪物："+ enemys.Count);
        }
    }
    void OnTriggerExit(Collider col)
    {
        if (col.tag == "Monst")
        {
            enemys.Remove(col.gameObject);
            Debug.Log("射程内的怪物："+ enemys.Count);
        }
    }
    public float attackRateTime=30.0f;//多长时间攻击一次
    private float timer = 0.0f;
    public GameObject attackWeaponPrefab;//攻击的子弹
    public Transform BuildingFire;//子弹实例化的位置
    private static Turret inst = null;
    public bool useLaser;//是否使用激光
    public float damageRate=10;//激光伤害
    public LineRenderer LaserRender;//激光组件
    public GameObject LaserEffect;//激光粒子组件
    //public GameObject FireLaser;//发射激光的粒子效果
    public static Turret GetInstance()
    {
        return inst;
    }
    void Start()
    {
        inst = this;
        timer = attackRateTime;
    }
    void Update()
    {
        //不使用激光
        if (useLaser == false)
        {
            timer += Time.deltaTime;
            if (enemys.Count > 0 && timer > attackRateTime)//有敌人在附近和时间到进行攻击
            {
                timer = 0;
                Attack();
            }

        }
        else if (enemys.Count > 0)//使用激光
        {
            LaserEffect.SetActive(true);
            //FireLaser.SetActive(true);
            if (LaserRender.enabled==false)
            {
                LaserRender.enabled = true;
            }
            if (enemys[0] == null)
            {
                UpdateEnemys();
            }
            if (enemys.Count > 0)
            {
                LaserRender.SetPositions(new Vector3[] { BuildingFire.position, enemys[0].transform.position });//获取目标
                //FireLaser.transform.position = BuildingFire.position;//发射镭射的粒子的位置
                enemys[0].GetComponent<Monste>().TakeDamage(damageRate*Time.deltaTime);//计算伤害
                LaserEffect.transform.position=enemys[0].transform.position;//实例化粒子效果
                //让粒子效果朝向塔
                Vector3 pos = transform.position;//得到要朝向的塔
                pos = enemys[0].transform.position;
               
                LaserEffect.transform.LookAt(pos);
            }
        }
        else
        {
            //FireLaser.SetActive(false);
            LaserEffect.SetActive(false);
            LaserRender.enabled = false;//不攻击时不显示
        }
    }
    void Attack()
    {
        if (enemys[0] == null)//如果炮台已经击杀了怪物
         {
            UpdateEnemys();
         }
        if (enemys.Count > 0)
        {
            GameObject bullet = GameObject.Instantiate(attackWeaponPrefab, BuildingFire.position, BuildingFire.rotation);
            bullet.GetComponent<Bullet>().SetTarget(enemys[0].transform);
        }
        else
        {
            timer = attackRateTime;//重置计时器
        }
    }
    //更新Enemys[]数组
    void UpdateEnemys()
    {
        List<GameObject> NoNullEnemys = new List<GameObject>();
        foreach (GameObject obj in enemys)
        {
            if (obj!=null)
            {
                NoNullEnemys.Add(obj);
            }
        }
        enemys.Clear();
        enemys = NoNullEnemys;
        
    }
    private bool misUpGraded =false ;//是否升级过
    public bool isUpGraded
    {
        get
        {
            return misUpGraded;
        }
        set
        {
            misUpGraded = value;
        }
    }
}
