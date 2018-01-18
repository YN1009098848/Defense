using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateLibra : MonoBehaviour {
    public int maxLibra;//最大士兵个数
    public int Libra_count=0;//产生的士兵的个数
    public GameObject createTrans;//实例化的士兵
    public Transform create_transform;//士兵初始化位置

    private float createTime = 6.0f;
    private float m_time = 0.0f;

    public CreateLibra inst;
	// Use this for initialization
	void Start () {
        m_time = Time.time;
        create_transform=this.transform;
        inst = this;
    }
	
	// Update is called once per frame
	void Update () {
		if (Libra_count<3)//限制产生士兵的个数
		{
            if (Time.time> m_time+createTime)
            {
                Libra_count++;
                GameObject obj = Instantiate(createTrans,new Vector3(create_transform.position.x, 
                    create_transform.position.y, create_transform.position.z+(25-5* Libra_count)),Quaternion.identity) as GameObject;//实例化士兵
                obj.transform.SetParent(transform);//将父级设置为当前transform的父级
                m_time = Time.time;
            }
		}
	}
}
