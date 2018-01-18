using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxAmin : MonoBehaviour {
    public AnimationCurve Ac;//曲线动画
    Vector3 LastPos;//运动前的物体坐标
	// Use this for initialization
	void Start () {
        LastPos = transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
        float r = Ac.Evaluate(Time.time);//一个随时间变化的值
        transform.localScale = new Vector3(LastPos.x, LastPos.y*r, LastPos.z);//缩放，缩放y轴实现上下移动
	}
}
