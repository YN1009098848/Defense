using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour {
    public GameObject BuildingMenu;//保存建造菜单
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame 
	void Update () {
		
	}
     
    public void OpenBuildingMun()
    {
        GameObject rootCanvas = GameObject.Find("UICanvas");
        GameManager.Instance.current_Building = this.gameObject;//记录当前被点击的建造地
        GameObject menu = Instantiate(BuildingMenu, rootCanvas.transform.position, rootCanvas.transform.rotation) as GameObject;//实例化一个建造菜单
        menu.transform.SetParent(rootCanvas.transform);//将父级设置为当前transform的父级
        menu.transform.localScale = new Vector3(1, 1, 1);
    }
}
