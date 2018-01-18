using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour {
    public static GameManager Instance;//GameManager实例
    public ArrayList m_PathNodes;//存储路径点
    public bool m_debug;
    public GameObject current_Building=null;//记录被点击的建造地面
    public GameObject Rem;//记录主角
    public GameObject InsFire;//火球发射的位置
    public GameObject current_Turret;//点击的防御塔
    //技能效果
    public GameObject rock;//预制体-石锥
    public GameObject fire;//预制体-火球
    //建造地的预制体
    public GameObject Building;
    //失败后的基地
    public GameObject DieMatrix;
    //结束界面
    public GameObject EndUi;
    public Text EndMessage;
    //失败停止生成
    private EnemySpawner enemyspawner;
    //角色死亡位置
    public Vector3 DiePos;

    private int mCurrentLevelIndex = -1;

    public bool IsOpenBag;
    void Awake()
    {
        Instance = this;
        enemyspawner=GetComponent<EnemySpawner>(); 
        
    }

    

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
    [ContextMenu("BuildPath")]
    void BuildPath()
    {
        m_PathNodes = new ArrayList();
        GameObject[] objects = GameObject.FindGameObjectsWithTag("PathNode");
        for (int i=0;i<objects.Length;i++)
        {
            PathNode node = objects[i].GetComponent<PathNode>();
            m_PathNodes.Add(node);
        }
    }

    void OnDrawGizmos()
    {
        if (!m_debug||m_PathNodes==null)
        {
            return;
        }
        Gizmos.color = Color.red;
        foreach (PathNode node in m_PathNodes)
        {
            if (node.nextNode!=null)
            {
                Gizmos.DrawLine(node.transform.position, node.nextNode.transform.position);
            }
        }
    }
    //绑定按钮事件
    void GetButtonOnClick()
    {
        //查找RestartButton按钮
        GameObject RestartButton = GameObject.FindWithTag("RestartButton");
        Button btn0 = (Button)RestartButton.GetComponent<Button>();
        btn0.onClick.AddListener(OnButtonRetry);
        //查找StartScreenButton按钮
        GameObject StartScreenButton = GameObject.FindWithTag("StartScreenButton");
        Button btn1 = (Button)StartScreenButton.GetComponent<Button>();
        btn0.onClick.AddListener(OnButtonMenu);
    }
    //重置建造地
    public void DesBuilding()
    {
        if (current_Building!=null)
        {
            Destroy(current_Building);
        }
    }
    //胜利
    public void Win()
    {
        EndUi.SetActive(true);
        BuildingMgr.GetInstance().isWinRoF = true;
        EndMessage.text = "胜利";
    }
    //失败
    public void Failed()
    {
        BuildingMgr.GetInstance().isWinRoF = true;
        EndUi.SetActive(true);
        EndMessage.text = "失败";
    }
    //重玩
    public void OnButtonRetry()
    {
       
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);//获取index来加载场景
    }
    //回到菜单
    public void OnButtonMenu()
    {
        SceneManager.LoadScene(0);//菜单位于第一个，加载索引为0的场景
    }
}
