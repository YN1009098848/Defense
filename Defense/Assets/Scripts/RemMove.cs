using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RemMove : MonoBehaviour {
    public float Speed = 4;//速度
    public Text RemHpText;//血量提示
    public GameObject RemDieEffect;//Rem死亡动画
    public Slider RemHp;//血条
    public float max_Hp=100;//最大生命力
    private float current_Hp;//当前的生命力
    // Use this for initialization
    void Start () {
        current_Hp = max_Hp;//给予初始血量
        RemHpText.text = max_Hp.ToString();
    }
	
	// Update is called once per frame
	void Update () {
    }
    public void MoveInRightJoyStick(Vector2 weizhi)
    {
        if (weizhi.y != 0 || weizhi.x != 0)
        {
            //设置角色的朝向（朝向当前坐标+摇杆偏移量）    
            transform.LookAt(new Vector3(transform.position.x + weizhi.x, transform.position.y, transform.position.z + weizhi.y));
            //移动玩家的位置（按朝向位置移动）  
            transform.Translate(Vector3.forward * Time.deltaTime * Speed);
        }
    }
    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Monst")
        {
            TakeDamage(Monste.GetInstance().Power);
        }

    }
    //伤害处理函数
    public void TakeDamage(float damage)
    {
        //扣血
        if (current_Hp > 0)
        {
            current_Hp -= damage;
            RemHp.value = current_Hp / max_Hp;//计算血量
            RemHpText.text = current_Hp.ToString();
        }
        //是否死亡
        if (current_Hp <= 0.0f)
        {
            GameManager.Instance.DiePos = transform.position;
            GameManager.Instance.Failed();
            Die();
        }
    }
    void Die()
    {
        GameObject effect = GameObject.Instantiate(RemDieEffect, transform.position, transform.rotation);
        Destroy(effect, 1.5f);
        Destroy(this.gameObject);
    }
}
