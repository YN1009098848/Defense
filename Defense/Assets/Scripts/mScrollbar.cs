using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class mScrollbar : MonoBehaviour
{

    public static mScrollbar instance;
    public List<Button> items = new List<Button>();

    public GameObject content;//内容
    public Vector2 contentSize;//内容的原始高度

    public GameObject itemPrefab;//列表项
    public float itemHeight;
    public Vector3 itemLocalPos;
    public Button LevelButton;
    
    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        content = GameObject.Find("Content");//找到列表内容
        contentSize = content.GetComponent<RectTransform>().sizeDelta;
        itemPrefab = (GameObject)Resources.Load("Prefabs/Button/LevelButton");//找到预制按钮
        itemHeight = itemPrefab.GetComponent<RectTransform>().rect.height;//得到预制体的高
        itemLocalPos = itemPrefab.transform.localPosition;//得到预制体的坐标
    }

    //添加列表项
    public void AddItem()
    {
        GameObject a = Instantiate(itemPrefab) as GameObject;
        LevelButton = a.GetComponent<Button>();
        LevelButton.transform.parent = content.transform;
        LevelButton.transform.localPosition = new Vector3(itemLocalPos.x, itemLocalPos.y - items.Count * itemHeight, 0);
        LevelButton.transform.localScale= new Vector3(1, 1, 1);
        items.Add(LevelButton);
        if (contentSize.y <= items.Count * itemHeight)//增加内容的高度
        {
            content.GetComponent<RectTransform>().sizeDelta = new Vector2(contentSize.x, items.Count * itemHeight);
        }
    }
    
}
