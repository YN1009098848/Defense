using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Matrix : MonoBehaviour {
    public float MaxHp;//基地最大血量
    private float mHp;//当前血量
    public Slider matrixLifeSlider;//血条
    public GameObject damageMatrix;//被攻击的粒子效果
    private Vector3 dieMatrixPos;//记录基地的位置
    private Quaternion dieMatrixRot; 
    public AnimationCurve Ac;//曲线动画
    Vector3 LastPos;//运动前的物体坐标
    // Use this for initialization
    void Start () {
        LastPos = transform.localScale;
        mHp=MaxHp;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Monst")//攻击
        {  
            TakeDamage(10.0f);
            lastPos();
        }
    }
    void lastPos()
    {
        float r = Ac.Evaluate(Time.time);//一个随时间变化的值
        transform.localScale = new Vector3(LastPos.x * r, LastPos.y * r, LastPos.z * r);//缩放，缩放y轴实现上下移动
    }
    //伤害处理函数
    public void TakeDamage(float damage)
    {
        //扣血
        if (MaxHp > 0)
        {
            MaxHp -= damage;
            matrixLifeSlider.value = MaxHp / mHp;//计算血量
        }
        //判断
        if (MaxHp <= 0.0f)
        {
            Die();
            GameManager.Instance.Failed();
        }
    }
    void Die()
    {
        GameObject effect = GameObject.Instantiate(damageMatrix, transform.position, transform.rotation);
        Destroy(effect, 1.5f);
        Instantiate(GameManager.Instance.DieMatrix, transform.position, transform.rotation);
        Destroy(this.gameObject);
    }
}
