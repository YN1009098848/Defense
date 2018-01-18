using CoreEngine.InputRef;
using HighlightingSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildingMgr:MonoBehaviour{
    //保存塔的数据
    public TurretDate LaserTurretDate;
    public TurretDate MissileTurretDate;
    public TurretDate StandardTurretDate;
    public TurretDate LibraTurretDate;
    //选中塔高亮
    public Highlighter TurretLight;
    //当前将要建造的炮台
    private TurretDate SelectedTurretDate;
    private Building building;//储存当前点击到的建造地
    private Text moneyText; //钱的text
    public List<Building> Menulist;//实例化了菜单的建造地
    //是否实例化了建造菜单
    public bool isInsBuildingMenu=false;
    //缺钱动画
    public Animator moneyAnimator;
    //建造的粒子动画
    public GameObject buidEffect;
    //升级菜单
    public GameObject TurretMenu;
    //升级按钮
    public Button ButtonUpGraded;
    //菜单动画
    public Animator UpGraderAnimator;
    //需要/可以升级的塔
    private GameObject UpTurret;
    public GameObject touchArea;//touchArea是点击区域
    //是否已经胜利或失败
    public bool isWinRoF=false;
    //玩家金钱
    public float money;
    private static BuildingMgr inst=null;
    public bool isUpgrades;
    public GameObject TurretGo;//当前的塔

    public void init()
    {
    }
    public static BuildingMgr GetInstance()
    {
        return inst;
    }
    
    void Start()
    {
        inst = this;
        GameObject moneyObj = GameObject.FindWithTag("Money");
        moneyText = (Text)moneyObj.GetComponent<Text>();
        ChangeMoney(0);
        this.touchArea.GetComponent<InputComponet>().RegisterInputAction(InputActionPrior.INPUT_UI, OnBTouch);
        
    }
    public void ChangeMoney(int ChangeNumber)//钱的更改
    {
        money = money+ ChangeNumber;
        moneyText.text =money.ToString();
    }
    void Update()
    {
        InputMgr.GetInstant().Update();
    }
    public void OnBTouch(InputAction action)
    {
        //射线检测点击事件
        if (action.id==InputActionID.IA_ONEPOINT_CLICK&& !isWinRoF)
        {
            IA_OnePointClick act = (IA_OnePointClick)action;
            //RaycastHit得到射线碰撞的第一个物体的信息，所以提取创建它用来存储信息
            //Physics.Raycast得到物体的信息，
            //Raycast(Ray ray, out RaycastHit hitInfo, float maxDistance); 
            //ray: 射线结构体的信息，包括起点，方向；也就是一条射线
            //hitinfo:这条射线所碰撞物体的相关信息；
            //maxDistance: 这条射线的最大距离；
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                //开放建造
                Ray ray = Camera.main.ScreenPointToRay(act.pos);//从鼠标点击处发射射线
                ClickUpMenu(ray);
            }
        }
        if (isInsBuildingMenu)
        {
            ButtonOnClick();//绑定按钮点击事件
            Debug.Log(Menulist.Count + "个菜单绑定了按钮事件");
            isInsBuildingMenu = false;
        }
    }
    void ClickUpMenu(Ray ray)
    {
        RaycastHit hit;
        bool isCollider = Physics.Raycast(ray, out hit,100,LayerMask.GetMask("Building"));//得到射线ray碰到的物体信息hit，并判断物体是否是建造地，返回bool值
        if (isCollider && Menulist.Count < 1)
        {
            building = hit.collider.GetComponent<Building>();//得到点击的建造地
            building.OpenBuildingMun();
            Menulist.Add(building);
            Debug.Log("实例化了菜单：" + Menulist.Count);
            isInsBuildingMenu = true;
        }
        bool isTurret = Physics.Raycast(ray, out hit, 50, LayerMask.GetMask("turret"));//如果点击是是塔
        //GameObject ThisTurret = Physics.Raycast(ray, out hit, 40, LayerMask.GetMask("turret"));//如果点击是是塔
        if (isTurret)//如果是塔，则可以进行升级
        {
            ShowUpGradeUI(hit.transform.position, false);//显示升级ui
            UpTurret = hit.transform.gameObject;//得到当前的塔
            TurretLight=UpTurret.GetComponent<Highlighter>();
            TurretLight.ConstantOn(Color.red);
            TurretUpButtonOnClick();
        }
    }

    void TurretUpButtonOnClick()
    {
        //查找Delete按钮
        GameObject DeleteButton = GameObject.FindWithTag("TurretDelete");
        Button btn0 = (Button)DeleteButton.GetComponent<Button>();
        btn0.onClick.AddListener(BuildingMgr.GetInstance().OnDestroyButtonDown);
        //查找Up按钮
        GameObject TurretUpButton = GameObject.FindWithTag("TurretUp");
        Button btn1 = (Button)TurretUpButton.GetComponent<Button>();
        btn1.onClick.AddListener(BuildingMgr.GetInstance().OnUpGradeButtonDown);
        //查找Back按钮
        GameObject BackButton = GameObject.FindWithTag("TurretBack");
        Button btn2 = (Button)BackButton.GetComponent<Button>();
        btn2.onClick.AddListener(BuildingMgr.GetInstance().OnBackButton);
    }

    void ButtonOnClick()
    {
        //查找Standardbutton
        GameObject StandardButton = GameObject.FindWithTag("Standard");
        Button btn0 = (Button)StandardButton.GetComponent<Button>();
        btn0.onClick.AddListener(OnStandardSelected);
        //查找LibraButton
        GameObject LibraButton = GameObject.FindWithTag("LibraSelected");
        Button btn1 = (Button)LibraButton.GetComponent<Button>();
        btn1.onClick.AddListener(OnLibraSelected);
        //查找Missilebutton
        GameObject MissileButton = GameObject.FindWithTag("Missile");
        Button btn2 = (Button)MissileButton.GetComponent<Button>();
        btn2.onClick.AddListener(OnMissileSelected);
        //查找LaserButton
        GameObject LaserButton = GameObject.FindWithTag("Laser");
        Button btn3 = (Button)LaserButton.GetComponent<Button>();
        btn3.onClick.AddListener(OnLaserSelected);
        //查找BackButton
        GameObject BackButton = GameObject.FindWithTag("BuildBack");
        Button btn4 = (Button)BackButton.GetComponent<Button>();
        btn4.onClick.AddListener(OnClickBack);
    }
    //实例化塔，判断价钱
    public void OnLaserSelected()
    {
        Debug.Log("Laser");
        SelectedTurretDate =LaserTurretDate;
        if (money > SelectedTurretDate.cost&& money - SelectedTurretDate.cost >= 0)
        {
            ChangeMoney(-SelectedTurretDate.cost);//扣钱 
            TurretGo=Instantiate(SelectedTurretDate.turretPrefab, GameManager.Instance.current_Building.transform.position, Quaternion.identity);
            GameObject effect=GameObject.Instantiate(buidEffect, GameManager.Instance.current_Building.transform.position, Quaternion.identity);
            //在建造塔后销毁建造地
            GameObject boj = GameManager.Instance.current_Building.transform.gameObject;
            Destroy(boj, 1);
            Destroy(effect, 1);//延迟销毁建造粒子效果  
            GameObject obj = GameObject.FindWithTag("BuildingMenu");
            Destroy(obj.transform.gameObject);//关闭建造菜单
            Menulist.Clear();//清空list保证下一次能打开新的菜单
            Debug.Log("现在有" + Menulist.Count + "菜单");
        }
        else
        {
            //提示钱不够
            moneyAnimator.SetTrigger("Flicker");
        }
    }
    public void OnMissileSelected()
    {
        Debug.Log("Missile");
        SelectedTurretDate = MissileTurretDate;
        if (money > SelectedTurretDate.cost&& money - SelectedTurretDate.cost >= 0)
        {
            ChangeMoney(-SelectedTurretDate.cost);
            Instantiate(SelectedTurretDate.turretPrefab, GameManager.Instance.current_Building.transform.position, Quaternion.identity);
            GameObject effect = GameObject.Instantiate(buidEffect, GameManager.Instance.current_Building.transform.position, Quaternion.identity);
            //在建造塔后销毁建造地
            GameObject boj = GameManager.Instance.current_Building.transform.gameObject;
            Destroy(boj, 1);
            Destroy(effect, 1);//延迟销毁建造粒子效果
            GameObject obj = GameObject.FindWithTag("BuildingMenu");
            Destroy(obj.transform.gameObject);//关闭建造菜单
            Menulist.Clear();
            Debug.Log("现在有" + Menulist.Count + "菜单");
        }
    }
    public void OnStandardSelected()
    {

        Debug.Log("Standard");
        SelectedTurretDate = StandardTurretDate;
        if (money > SelectedTurretDate.cost&& money - SelectedTurretDate.cost >= 0)
        {
            ChangeMoney(-SelectedTurretDate.cost);
            Instantiate(SelectedTurretDate.turretPrefab, GameManager.Instance.current_Building.transform.position, Quaternion.identity);
            GameObject effect = GameObject.Instantiate(buidEffect, GameManager.Instance.current_Building.transform.position, Quaternion.identity);
            //在建造塔后销毁建造地
            GameObject boj=GameManager.Instance.current_Building.transform.gameObject;
            Destroy(boj,1);
            Destroy(effect, 1);//延迟销毁建造粒子效果
            GameObject obj = GameObject.FindWithTag("BuildingMenu");
            Destroy(obj.transform.gameObject);//关闭建造菜单
            Menulist.Clear();
            Debug.Log("现在有" + Menulist.Count + "菜单");
        }
    }
    public void OnLibraSelected()
    {

        Debug.Log("Libra");
        SelectedTurretDate = LibraTurretDate;
        if (money > SelectedTurretDate.cost&& money - SelectedTurretDate.cost>=0)
        {
            ChangeMoney(-SelectedTurretDate.cost);
            Instantiate(SelectedTurretDate.turretPrefab, GameManager.Instance.current_Building.transform.position, Quaternion.identity);
            GameObject effect = GameObject.Instantiate(buidEffect, GameManager.Instance.current_Building.transform.position, Quaternion.identity);
            //在建造塔后销毁建造地
            GameObject boj = GameManager.Instance.current_Building.transform.gameObject;
            Destroy(boj, 1);
            Destroy(effect, 1);//延迟销毁建造粒子效果
            GameObject obj = GameObject.FindWithTag("BuildingMenu");
            Destroy(obj.transform.gameObject);//关闭建造菜单
            Menulist.Clear();
            Debug.Log("现在有" + Menulist.Count + "菜单");
        }
    }
    public void OnClickBack()//建造菜单返回
    {
        Debug.Log("Back");
        GameObject obj= GameObject.FindWithTag("BuildingMenu");
        Destroy(obj.transform.gameObject);
        Menulist.Clear();
        Debug.Log("现在有"+Menulist.Count+"菜单");
    }
    void ShowUpGradeUI(Vector3 pos,bool isDisableUoGrade=false)//显示ui
    {
        StopCoroutine("HieUpGradeUI");//停止协程
        TurretMenu.SetActive(false);//初始化动画状态机（从一个塔到另一个塔）
        TurretMenu.SetActive(true);
        //TurretMenu.transform.position = pos;
        ButtonUpGraded.interactable =! isDisableUoGrade;
    }
    IEnumerator HieUpGradeUI()
    {
        yield return new WaitForSeconds(0.8f);
        TurretMenu.SetActive(false);
    }

    public void OnUpGradeButtonDown()
    {
        if (isUpgrades == false)
        {
            TurretLight.ConstantOff();
            SelectedTurretDate = isUpTurret(UpTurret);//判断应该实例化哪种塔的升级
            if (money > SelectedTurretDate.costUpGraded && money - SelectedTurretDate.costUpGraded >= 0)//钱足够升级
            {
                ChangeMoney(-SelectedTurretDate.costUpGraded);
                GameObject effect = GameObject.Instantiate(buidEffect, UpTurret.transform.position, Quaternion.identity);//粒子效果 
                Instantiate(SelectedTurretDate.turretUpGradedPrefab, UpTurret.transform.position, Quaternion.identity);
                Destroy(UpTurret);//删除之前的塔
                Destroy(effect, 1);//延迟销毁建造粒子效果
                StartCoroutine(HieUpGradeUI());//隐藏升级菜单
                isUpgrades = true;
            }
            else
            {
                Debug.Log("钱不够");
            }
        }
        else
        {
            return;
        }
    }
    public void OnDestroyButtonDown()
    {
        TurretLight.ConstantOff();
        GameObject effect = GameObject.Instantiate(buidEffect, UpTurret.transform.position, Quaternion.identity);//粒子效果
        Instantiate(GameManager.Instance.Building, UpTurret.transform.position, Quaternion.identity);//实例化一个新的建造地
        Destroy(UpTurret);//删除当前的塔
        Destroy(effect, 1);//延迟销毁建造粒子效果
        StartCoroutine(HieUpGradeUI());//隐藏菜单
        isUpgrades = false;
        TurretGo = null;
    }
    void OnBackButton()
    {
        TurretLight.ConstantOff();
        StartCoroutine(HieUpGradeUI());//协程调用隐藏
    }
    private TurretDate isUpTurret(GameObject turret)
    {
        if (turret.tag == "StandardTurret")
        {
            return StandardTurretDate;
        }
        else if (turret.tag == "LibraSelectedTurret")
        {
            return LibraTurretDate;
        }
        else if (turret.tag == "MissileTurret")
        {
            return MissileTurretDate;
        }
        else if (turret.tag == "LaserTurret")
        {
            return LaserTurretDate;
        }
        else
            return null;
    } 
}
