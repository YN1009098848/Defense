using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatorBag : MonoBehaviour {
    public GameObject creatorBag;
	// Use this for initialization
	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void OpenBag()
    {
        GameObject rootCanvas = GameObject.Find("UICanvas");
        GameManager.Instance.current_Building = this.gameObject;
        GameObject menu = Instantiate(creatorBag, rootCanvas.transform.position, rootCanvas.transform.rotation) as GameObject;
        menu.transform.SetParent(rootCanvas.transform);//将父级设置为当前transform的父级
        menu.transform.localScale = new Vector3(1, 1, 1);
        GameManager.Instance.IsOpenBag = true;
    }
}
