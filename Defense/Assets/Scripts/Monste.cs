using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Monste : MonoBehaviour {
    public float max_life ;//敌人最大生命力
    private float current_life;//当前敌人的生命力
    public GameObject DieEffect;//敌人死亡的动画
    public Slider LifeSlider;//血条
    public float Power;
    public int ValueMoney;//怪物值多少钱
    // Use this for initialization
    private static Monste inst = null;
    public static Monste GetInstance()
    {
        return inst;
    }
    void Start () {
        inst = this;
        current_life=max_life;//给予怪物初始血量
 
    }
	
	// Update is called once per frame
	void Update () {
        
	}
    
    void OnTriggerEnter(Collider collision)//（刚体碰撞检测/触发器检测）
    {
        //PS:子弹击中扣血交给子弹自身处理了
        if (collision.gameObject.tag == "Bullet")//火球攻击
        {
            var power = 500;
            //当前生命减去火球攻击力
            //调用生命处理函数
            TakeDamage(power);
        }
        if (collision.gameObject.tag == "Rock")//炸药
        {
            Die();
        }
        if (collision.gameObject.tag == "Libra")//士兵攻击
        {
            //获得士兵塔士兵的攻击力
            var power = ConLibra.GetInstance().isLibraPower;
            //调用生命处理函数
            TakeDamage(power);  
        }
        if (collision.gameObject.tag == "RemBullet")
        {
            var power =10;
            //调用生命处理函数
            TakeDamage(power);
        }
        if (collision.gameObject.tag =="Matrix")
        {
            Die();
        }
    }
    //伤害处理函数
    public void TakeDamage(float damage)
    {
        //扣血
        if (current_life>0)
        {
            current_life -= damage;
            LifeSlider.value = current_life / max_life;//计算血量
        }
        //判断怪物是否死亡
        if (current_life <= 0.0f)
        {
            Die();
        }
    }
    void Die()
    {
        BuildingMgr.GetInstance().ChangeMoney(ValueMoney);
        GameObject effect=GameObject.Instantiate(DieEffect,transform.position,transform.rotation);
        Destroy(effect,1.5f );
        Destroy(this.gameObject);
    }
}
