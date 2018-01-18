using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cooling : MonoBehaviour {
    private float coolDownTime = 15;//技能冷却时间
    float time;
    public Image image;
    bool Skill = false;
    private List<GameObject> MonstList;//存储怪物列表
    float mtime = 0.0f;
    //float MonstSpeed;//记录怪物的移动速度
    private GameObject mRock;//预制体-石锥
    private GameObject mFire;//预制体-火球
    public bool isButton = false;//是否按下技能按钮
    public GameObject Rem;//主角
    public GameObject InstantiateFire;//实例化火球的位置
    public bool endTime = false;//是否结束debuff   
    // Use this for initialization
    void Start () {
        MonstList = EnemySpawner.Instance.mMonstList;
        //初始化技能冷却时间
        time = coolDownTime;
        //初始化粒子效果
        Rem = GameManager.Instance.Rem;
        InstantiateFire = GameManager.Instance.InsFire;
        mRock = GameManager.Instance.rock;
        mFire = GameManager.Instance.fire;
    }
    // Update is called once per frame
    void Update () {
        time += Time.deltaTime;
        image.fillAmount= (coolDownTime - time) / coolDownTime;
        //将物体dbuff去除
        if (endTime)
        {
            mtime += Time.deltaTime;
            if (mtime > 3.0f)
            {
                //Enemy.GetInstance().isMoveSpeed = MonstSpeed;
                endTime = false;
            }
        }
    }
    public void ReleaseSkill()
    {
        if (time>coolDownTime)
        {
            if (this.gameObject.tag== "Fire")//火球技能
            {
                Skill = true;
                Fire();
                Skill = false;
            }
            if (this.gameObject.tag== "Rock")//石锥技能
            {
                Skill = true;
                Rock();
                Skill = false;
            }
            time = 0;
        }
    }
    //火球技能说明：在主角视角正前方实例化一个火球，向前飞行一段距离，击中的怪物会减少HP
    public void Fire()
    {
        GameObject obj = Instantiate(mFire, InstantiateFire.transform.position, InstantiateFire.transform.rotation) as GameObject;//实例化一个子弹
        obj.transform.SetParent(Rem.transform.parent);//将子弹的父级设置为当前transform的父级
    }
    //炸弹路障
    public void Rock()
    {
        GameObject obj = Instantiate(mRock, new Vector3(InstantiateFire.transform.position.x,
                    InstantiateFire.transform.position.y-2, InstantiateFire.transform.position.z), Quaternion.identity) as GameObject;
        
    }
}
