using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using CoreEngine.InputRef;
using System;
using Defense;
using UnityEngine.UI;

//摄像机跟随
public class CameaContorl : MonoBehaviour
{
    private Transform player;//角色
    private Vector3 offsetPosition;//位置
    public float distance;//向量长度
    public float scrollSpeed = 0.0001f;//拉近拉远的速度
    public GameObject startButton;//镜头归位
    private float Speed = 0.3f;//滑动移动速度 
    public GameObject touchArea;//touchArea是点击区域
    private Vector2 OneClick;//初次点击位置
    private Vector2 OneClick1;
    private Vector2 OneClick2;
    private Vector3 ExitPos;//手指离开的位置
    public float dis;//拖动距离
    public static bool mIsDraging = false;//判断是否是拖动状态
    public bool misZoomCamera = false;
    private float DoubleTouchCurrDis;//当前的双指间距
    private float DoubleTouchLastDis;//上一帧的双指间距
    private Vector3 screenPos;
    public bool IsMoveCamera = false;//是否移动镜头
    //记录上一次手机触摸位置
    private Vector2 oldPosition1;
    private Vector2 oldPosition2;
    public float maxDistance = 30f;
    public float minDistance = 5f;

    //缩放系数  
    public float scaleFactor = 0.00001f;
    private Vector2 lastSingleTouchPosition;
    private Vector3 velocity = Vector3.zero;

    public GameObject ZoomButton;
    public bool IsZoomStop;
    public bool IsZoomButton;
    public Vector3 TouchCameraPos;
    public Vector3 CameraPos;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("player").transform;//找到角色 
        transform.LookAt(player.position);//让相机朝向物体
        offsetPosition = transform.position - player.position;//得到偏移量
        this.touchArea.GetComponent<InputComponet>().RegisterInputAction(InputActionPrior.INPUT_FIRST, onTouch);
        OneButtonStart();
        screenPos = Camera.main.WorldToScreenPoint(transform.position);
    }
    void Update()
    {
        if (player == null)
        {
            Vector3 pos = GameManager.Instance.DiePos;
            transform.position = new Vector3(pos.x, pos.y * 30, pos.z);
        }
        else
        {
            if (!GameManager.Instance.IsOpenBag)
            {
                if (!mIsDraging && !IsMoveCamera && !misZoomCamera)
                {
                    Vector3 position = offsetPosition + player.position;
                    Vector3 mPos = new Vector3(position.x, position.y, position.z);
                    transform.position = Vector3.SmoothDamp(transform.position, mPos, ref velocity, 0.05f);
                }
                if (!IsZoomButton && !mIsDraging && !IsMoveCamera)//点击缩放按钮，放大
                {
                    Vector3 position = offsetPosition + player.position;
                    transform.position = Vector3.SmoothDamp(transform.position, position, ref velocity, 1f);
                    startCamear();
                }
                if (IsZoomButton && !mIsDraging && !IsMoveCamera)//点击缩放按钮，缩小
                {
                    mIsDraging = false;
                    misZoomCamera = true;
                    Vector3 position = transform.forward *-100;
                    //Debug.Log("position:" + position);
                    CameraPos = position + player.position;
                    transform.position = Vector3.SmoothDamp(transform.position, CameraPos, ref velocity, 0.5f);
                }
            }
        }

    }
    public void onTouch(InputAction action)
    {
        //移动
        if (mIsDraging && action.id == InputActionID.IA_ONEPOINT_DRAG && IsMoveCamera)
        {
            if (misZoomCamera && !IsZoomButton)
            {
                return;
            }
            else
            {
                IsMoveCamera = true;
                IA_OnePointDrag act = (IA_OnePointDrag)action;
                Vector2 clickPos = new Vector2(act.pos.x, act.pos.y);//点击动作发生的点的坐标
                Vector3 deltaVec = new Vector3(clickPos.x - OneClick.x, 0, clickPos.y - OneClick.y);
                Vector3 tarVec = deltaVec * Speed;
                transform.localPosition = tarVec + TouchCameraPos;
                //transform.Translate(tarVec, Space.World);//?
                // Debug.Log(tarVec + "---------xxxx");
                // Debug.Log(transform.localPosition + "");

            }

        }
        else if (mIsDraging && action.id == InputActionID.IA_ONEPOINT_CLICK)
        {
            mIsDraging = false;
        }
        else if (!mIsDraging && action.id == InputActionID.IA_ONEPOINT_PRESS && !GameManager.Instance.IsOpenBag)//得到拖动的初始点
        {
            IA_OnePointPress act = (IA_OnePointPress)action;
            bool isonClick = isTouchInCollider(act.pos);//判断是否点击到区域
            if (!isonClick)
            {
                mIsDraging = false;
            }
            else
            {
                TouchCameraPos = this.transform.localPosition;
                Debug.Log("点击坐标："+act.pos);
                IsMoveCamera = true;
                OneClick = act.pos;
                mIsDraging = true;
            }
        }
        else if (action.id == InputActionID.IA_ONEPOINT_DRAG_EXIT)//移动退出
        {
            mIsDraging = false;
            IA_OnePointDragExit act = (IA_OnePointDragExit)action;
            OneClick = act.pos;
            IsMoveCamera = false;
        }
        //缩放
        if (!IsZoomButton&& misZoomCamera && action.id == InputActionID.IA_TWOPOINT_DRAG)
        {
            mIsDraging = true;
            IA_TwoPointDrag act = (IA_TwoPointDrag)action;
            //计算出当前两点触摸点的位置  
            Vector2 tempPosition1 = new Vector2(act.pos0.x, act.pos0.y);
            Vector2 tempPosition2 = new Vector2(act.pos1.x, act.pos1.y);

            float currentTouchDistance = Vector3.Distance(tempPosition1, tempPosition2);
            float lastTouchDistance = Vector3.Distance(OneClick1, OneClick2);
            //计算上次和这次双指触摸之间的距离差距  
            //然后去更改摄像机的距离  
            distance -= (currentTouchDistance - lastTouchDistance) * scaleFactor;
            //把距离限制住在min和max之间  
            distance = Mathf.Clamp(distance, minDistance, maxDistance);
            Vector3 position = transform.forward * -distance;
            transform.localPosition= position + player.position;
            //Debug.Log("mPosition:" + mPosition);
            //Debug.Log("POS:" + transform.position);
            
        }
        else if (Input.touchCount > 1 && !GameManager.Instance.IsOpenBag)
        {
            oldPosition1 = Input.GetTouch(0).position;
            oldPosition2 = Input.GetTouch(1).position;
            bool isonClick0 = isTouchInCollider(oldPosition1);
            bool isonClick1 = isTouchInCollider(oldPosition2);//判断是否点击到区域
            //判断是否点击到区域
            if (isonClick0 && isonClick1)
            {
                Debug.Log("Touch OK");
                OneClick1 = oldPosition1;
                OneClick2 = oldPosition2;
                misZoomCamera = true;
                mIsDraging = true;
            }

        }
        else if (action.id == InputActionID.IA_TWOPOINT_DRAG_EXIT)
        {
            
            IA_TwoPointDragExit act = (IA_TwoPointDragExit)action;
            mIsDraging = false;
            misZoomCamera = false;
            OneClick1 = act.pos0;
            OneClick2 = act.pos1;
            startCamear();
        }
    }

    public void OneButtonStart()
    {

        Button btn = (Button)startButton.GetComponent<Button>();
        Button btn1 = (Button)ZoomButton.GetComponent<Button>();
        btn.onClick.AddListener(startCamear);
        btn1.onClick.AddListener(OnStartZoom);
    }

    public void OnStartZoom()
    {
        if (!IsZoomButton)
        {
            IsZoomButton = true;
        }
        else
        {
            IsZoomButton = false;
        }
        
    }

    public void startCamear()
    {
        if (player)
        {
            misZoomCamera = false;
            transform.position = offsetPosition + player.position;
        }
    }
    //计算点击事件的canvas坐标
    Vector3 uiPos(Vector3 pos, RectTransform rect)
    {
        Vector2 realActPos = new Vector2();
        //用点击发生的坐标和点击区域的RectTrasform再加上UI摄像机，换算出点击事件在Canvas的2D坐标。
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, pos, Camera.main, out realActPos);
        Vector3 newPos = new Vector3(realActPos.x, realActPos.y, 0);
        //Debug.Log("newPos:" + newPos);
        return newPos;
    }
    //判断是否在点击区域
    bool isTouchInCollider(Vector3 pos)
    {
        bool isTouchCrad = false;
        RectTransform rect = touchArea.transform as RectTransform;
        //isTouchCrad = uiPos(pos,rect);
        Vector3 newPos = uiPos(pos, rect);
        float rectXpos = rect.sizeDelta.x / 2;
        float rectYpos = rect.sizeDelta.y / 2;
        if (newPos.x >= -(rectXpos) && newPos.x <= rectXpos && newPos.y >= -(rectYpos) && newPos.y <= rectYpos)
        {
            isTouchCrad = true;
        }
        return isTouchCrad;
    }
}
