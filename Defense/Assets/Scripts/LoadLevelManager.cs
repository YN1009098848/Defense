using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadLevelManager : MonoBehaviour {
    //关卡列表
    List<Level> mlevels = new List<Level>();
    public static LoadLevelManager instance;
    void Awake()
    {
        instance = this;  
        LoadSenceJSON();
    }
    // Use this for initialization
    void Start () {
          if (mlevels.Count>0)
        {
            CreateButton();
        }
    }
	
	// Update is called once per frame
	void Update () { 
	}
    public void LoadSenceJSON()
    {
        TextAsset text = Resources.Load("LevelData") as TextAsset;
        //StartManager.Instance.text.text = filepath;
        JsonData jd = JsonMapper.ToObject(text.text);//JsonMapper.ToObject解析文件
        JsonData gameObjectArray = jd["LevelList"];//导出GameObject
        int i;
        for (i = 0; i < gameObjectArray.Count; i++)//遍历数组，得到所有的关卡
        {
            Level mlevel = new Level();
            mlevel.ID = (string)gameObjectArray[i]["ID"];
            mlevel.Name = (string)gameObjectArray[i]["Name"];
            mlevel.Difficulty=(string)gameObjectArray[i]["Difficulty"];
            mlevel.UnLock = (string)gameObjectArray[i]["UnLock"];
            mlevels.Add(mlevel);//向list里添加一个关卡
        }
    }
    public void CreateButton()
    {
        int j;
        for (j=0;j<mlevels.Count;j++)
        {
            Debug.Log(mlevels.Count);
            mScrollbar.instance.AddItem();
            mScrollbar.instance.LevelButton.name =mlevels[j].Name;
            Text nametext = mScrollbar.instance.LevelButton.transform.Find("Text").GetComponent<Text>();
            Text Difficultytext = mScrollbar.instance.LevelButton.transform.Find("Difficulty").GetComponent<Text>();
            nametext.text = mlevels[j].Name;
            Difficultytext.text = mlevels[j].Difficulty;
        }
    }
}  

