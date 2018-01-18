using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode : MonoBehaviour {
    //方便编辑用
    public PathNode parentNode;
    public PathNode nextNode;
    // Use this for initialization
    public void SetNextNode(PathNode node)
    {
        if (nextNode != null)
        {
            nextNode.parentNode = null;
        }
        nextNode = node;
        node.parentNode = this;
    }
    //辅助类，用于标记出可能在视图里看不见的物体，例如路径
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;     
        //    在指定位置处绘制一个黄色的球体
        Gizmos.DrawSphere(this.transform.position, 1);
        //在指定位置处绘制一个图标，图标的路径在Assets/Gizmos
        //Gizmos.DrawIcon(this.transform.position, "Nodlight.tif");        
    }
}
