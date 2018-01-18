using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    
    private float m_MoveSpeedr = 5.0f;//移动速度
    private Transform[] positions;//路径点
    private int index = 0;//通过索引来得到第index个路径
    public float isMoveSpeed
    {
        get
        {
            return m_MoveSpeedr;
        }
        set
        {
            m_MoveSpeedr = value;
        }
    }

    

    private static Enemy inst = null;
    public static Enemy GetInstance()
    {
        return inst;
    }
    // Use this for initialization
    void Start () {
        inst = this;
        positions = PathNodeMgr.positions;//将场上所有的路径传过来
	}
	
	// Update is called once per frame
	void Update () {
        
        Move();
	}

    void Move()
    {
        if (index > positions.Length - 1)
        {
            Destroy(transform);
        }
        else
        {
            transform.Translate((positions[index].position - transform.position).normalized * Time.deltaTime * isMoveSpeed);//计算与路径点之间的距离
            //Vector3 MonsterPos = positions[index].transform.position;
            Vector3 targetDir = positions[index].position - transform.position; // 目标坐标与当前坐标差的向量
            float a = Vector3.Angle(transform.forward, targetDir); // 返回当前坐标与目标坐标的角度
            RotateIntoMoveDirection();
            if (Vector3.Distance(positions[index].position, transform.position) < 0.2f)
            {
                index++;
            }
            if (index>positions.Length-1)
            {
                ReachDesation();
            }
        }
    }
    //到达终点
    void ReachDesation()
    {
        GameObject.Destroy(this.gameObject);
    }
    void OnDestroy()
    {
        EnemySpawner.CountEnemyAlive--;
    }
    private void RotateIntoMoveDirection()
    {
    
        GameObject sprite = gameObject.transform.Find("Sprite").gameObject;//让子物体旋转
        sprite.transform.LookAt(positions[index]);//朝向路径
        sprite.transform.Rotate(new Vector3(0, -90, 0));//保证是x朝向
    }   
}
