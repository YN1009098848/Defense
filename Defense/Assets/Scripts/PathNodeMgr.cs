using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNodeMgr : MonoBehaviour {
    //将路径点都放在这个组件下通过.Getchild找到已经是这个组件的路径点，也可以通过tag来找标签是PathNode的路径
    public static Transform[] positions;//储存路劲点
    void Awake()
    {
        //为了避免把存路径的自身的transfrom组件也得到，先构造数组，数组大小就是路径的个数
        //通过索引来得到每一个路径
        positions = new Transform[transform.childCount];
        for (int i = 0; i < positions.Length; i++)
        {
            positions[i] = transform.GetChild(i);
        }

    }
}
