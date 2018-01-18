using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BagManager : MonoBehaviour {
    //玩家金钱
    public float money;
    private Text moneyText; //钱的text
    // Use this for initialization
    void Start () {
        GameObject moneyObj = GameObject.FindWithTag("BagMoney");
        moneyText = (Text)moneyObj.GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
        moneyText.text = BuildingMgr.GetInstance().money.ToString();
	}
}
