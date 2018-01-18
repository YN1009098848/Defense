using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartManager : MonoBehaviour {
    public static StartManager Instance;//StartManager
    public GameObject Canva;//画布
    //存储临时界面
    private GameObject mMenu;
    //开始界面
    public GameObject Start1;//Start1预制体
    public GameObject Start2;//Start2预制体
    //说明界面
    public GameObject Explain;//说明界面预制体
    public Text text;
    void Awake()
    {
        Instance = this;
        GameObject menu1 =Instantiate(Start1,Canva.transform.position, Canva.transform.rotation)as GameObject;
        mMenu = menu1;
        menu1.transform.SetParent(Canva.transform);
        menu1.transform.localScale = new Vector3(1, 1, 1);//设定预制体的比例
    }
    void Start () {
        //AddStartButton();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    //开始按钮
    public void OnStartGame()
    {
        Destroy(mMenu);//删除当前界面
        GameObject menu2 = Instantiate(Start2, Canva.transform.position, Canva.transform.rotation) as GameObject;
        mMenu = menu2;//保存实例化的界面
        menu2.transform.SetParent(Canva.transform);
        menu2.transform.localScale = new Vector3(1, 1, 1);//设定预制体的比例
    }
    //说明按钮
    public void OnExplain()
    {
        Destroy(mMenu);
        GameObject menu2 = Instantiate(Explain, Canva.transform.position, Canva.transform.rotation) as GameObject;
        mMenu = menu2;//保存实例化的界面
        menu2.transform.SetParent(Canva.transform);
        menu2.transform.localScale = new Vector3(1, 1, 1);//设定预制体的比例
    }
    //结束游戏按钮
    public void OnEscape()
    {
        //如果在编辑器下
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();  
#endif
    }
    //返回按钮
    public void OnStart1Back()
    {
        Destroy(mMenu);
        GameObject menu1 = Instantiate(Start1, Canva.transform.position, Canva.transform.rotation) as GameObject;
        mMenu = menu1;//保存实例化的界面
        menu1.transform.SetParent(Canva.transform);
        menu1.transform.localScale = new Vector3(1, 1, 1);//设定预制体的比例
    }
    public void OnStart2Back()
    {
        Destroy(mMenu);
        GameObject menu1 = Instantiate(Start1, Canva.transform.position, Canva.transform.rotation) as GameObject;
        mMenu = menu1;//保存实例化的界面
        menu1.transform.SetParent(Canva.transform);
        menu1.transform.localScale = new Vector3(1, 1, 1);//设定预制体的比例
    }
    //TODO(LevelData  序列化， 使用json格式，读取数据文件动态创建按钮 ) 选择关卡按钮
    public void OnLevel(int idx)
    {
        SceneManager.LoadScene(idx);
    }
}
