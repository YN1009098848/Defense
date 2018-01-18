using UnityEngine;
using System.Collections.Generic;


namespace CoreEngine.InputRef
{
	/** global input mgr
 *
 * catch the input from device and convert it to the action
 */
	
	public class InputMgr
	{
		/** simulate mouse drag and double click */
		public const float DCLICK_TIME = 0.03f;
		public static bool ForceUIPenerate = false;
		public static GameObject LastDragedNGUIObject = null;

        private Vector2 PressPos = Vector2.zero;
		
		/** flag for mouse drag simulating */
		private int mouseType = 0;
		
		/** dragging or not */
		private bool isDragging = false;
		
		/** the drag time */
		private float lastDragTime;
		
		/** the last mouse pos */
		private Vector2 lastMousePos;
		
		/** flag to implement double click */
		private bool waitDClick;
		
		/** click time to implement double click */
		private float clickTime;
		
		//private Vector2 clickPos;
		
		/** whether touch moved or not */
		private bool isTouchMoved = false;
		
		/** whether tow touch moved */
		private bool isTwoTouchMoved = false;
		
		/** touch point count */
		public int touchNum = 0;
		
		/** mouse button type */
		public const int BUTTON_LEFT = 0;
		public const int BUTTON_RIGHT = 1;
		public const int BUTTON_MIDDLE = 2;
		
		/** the key map */
		private Dictionary<int, KeyCode> keyMap = new Dictionary<int, KeyCode>();
		
		/** the input action mgr */
		private InputActionMgr actionMgr = new InputActionMgr();
		
		/** focus on the keyborad */
		private bool kbfocus = true;
		
		public event System.Action OnAndroidBackButton;
		
		public bool KeyBoardFocus
		{
			get { return kbfocus; }
			set { kbfocus = value; }
		}
		
		/** focus on the mouse */
		private bool msfocus = true;
		
		public bool MouseFocus
		{
			get { return msfocus; }
			set { msfocus = value; }
		}
		
		/** mouse screen pos in pixel */
		
		public Vector3 MousePosition
		{
			get { return Input.mousePosition; }
		}
		
		/** is any key pressed */
		
		public bool AnyKey
		{
			get { return Input.anyKey; }
		}
		
		/** is any key down */
		
		public bool AnyKeyDown
		{
			get { return Input.anyKeyDown; }
		}
		
		/** the input action mgr */
		
		public InputActionMgr ActionMgr
		{
			get { return actionMgr; }
		}
		
		private readonly float ClickThreshhold = 20f; //Screen.dpi * 0.003f;


        private static InputMgr g_inputMgr;
        public static InputMgr GetInstant()
        {
            if (g_inputMgr == null)
            {
                g_inputMgr = new InputMgr();
            }

            return g_inputMgr;
        }

        public static InputActionMgr GetInputActionMgr()
        {
            return GetInstant().ActionMgr;
        }


        /** the start method, you must invoke it first 
    */

        public void Start()
		{
			KeyBoardFocus = true;
			MouseFocus = true;
		}
		
		/** the clean method, you must invoke it at last 
    */
		
		public void Shutdown()
		{
		}
		
		/** main loop. update every frame
    */
		
		public void Update()
		{
			ParseEscKey();
			
//			if (   !ForceUIPenerate
//			    && UICamera.isOverUI)
//			{
//				return;
//			}
			
			if (MouseFocus)
			{
				#if (UNITY_ANDROID || UNITY_IPHONE) && !UNITY_EDITOR
				
				ParseIOSInput();
				#else
				ParseMouseInput();
				ParseKeyInput();
				#endif
			}
			actionMgr.Execute();

            
		}
		
		/** get the mouse axis scrolling value
    *
    * @return the scrolling distance of the mouse wheel
    */
		
		public float GetMouseAxis()
		{
			if (!MouseFocus)
			{
				return 0;
			}
			return Input.GetAxis("Mouse ScrollWheel");
		}
		
		/** check the spesified button pressed or not 
    *
    * @param[in] type the mouse button type(left, right, middle)
    * @return pressed or not
    */
		
		public bool MouseButton(int type)
		{
			if (!MouseFocus)
			{
				return false;
			}
			return Input.GetMouseButton(type);
		}
		
		/** check the specified button down 
    *
    * @param[in] type the mouse button type(left, right, middle)
    * @return down or not
    */
		
		public bool MouseButtonDown(int type)
		{
			if (!MouseFocus)
			{
				return false;
			}
			return Input.GetMouseButtonDown(type);
		}
		
		/** check the specified button up
    *
    * @param[in] type the mouse button type(left, right, middle)
    * @return up or not
    */
		
		public bool MouseButtonUp(int type)
		{
			if (!MouseFocus)
			{
				return false;
			}
			return Input.GetMouseButtonUp(type);
		}
		
		/** check the spesified key pressed
    *
    * @param[in] key the virtual key to check
    * @return pressed or not
    */
		
		public bool GetKey(int key)
		{
			if (!KeyBoardFocus)
			{
				return false;
			}
			if (!keyMap.ContainsKey(key))
			{
				return false;
			}
			return Input.GetKey(keyMap[key]);
		}
		
		/** check the spesified key down
    *
    * @param[in] key the virtual key to check
    * @return down or not
    */
		
		public bool GetKeyDown(int key)
		{
			if (!KeyBoardFocus)
			{
				return false;
			}
			if (!keyMap.ContainsKey(key))
			{
				return false;
			}
			return Input.GetKeyDown(keyMap[key]);
		}
		
		/** check the spesified key up
    *
    * @param[in] key the virtual key to check
    * @return up or not
    */
		
		public bool GetKeyUp(int key)
		{
			if (!KeyBoardFocus)
			{
				return false;
			}
			if (!keyMap.ContainsKey(key))
			{
				return false;
			}
			return Input.GetKeyUp(keyMap[key]);
		}
		
		/** set the virtual key -> device key map
    *
    * @param[in] keyMap the virtual to device key map
    */
		
		public void SetKeyMap(Dictionary<int, KeyCode> keyMap)
		{
			if (keyMap == null)
			{
				return;
			}
			
			this.keyMap = keyMap;
		}
		
		/** set the virtual key for the spesified device key
    *
    * @param[in] key the virtual key
    * @param[in] keyCode the device key
    */
		
		public void SetKeyCode(int key, KeyCode keyCode)
		{
			keyMap.Remove(key);
			keyMap.Add(key, keyCode);
		}
		
		/** remove a virtual key
    *
    * @param[in] key the removed virtual key
    */
		
		public void RemoveKeyCode(int key)
		{
			if (ContainsKey(key))
			{
				keyMap.Remove(key);
			}
		}
		
		/** get the device key from the virtual key
    *
    * @param[in] key the virtual key
    * @return the device key
    */
		
		public KeyCode GetKeyCode(int key)
		{
			if (!ContainsKey(key))
			{
				return KeyCode.None;
			}
			return keyMap[key];
		}
		
		/** check whether has a spesified virtual key
    *
    * @param[in] key the virtual key to check
    * @return the key exists or not
    */
		
		public bool ContainsKey(int key)
		{
			return keyMap.ContainsKey(key);
		}
		//触屏手势
		private void ParseIOSInput()
		{
            switch (Input.touchCount)
            {
                case 1:
                    {
                        TouchPhase phase = Input.GetTouch(0).phase;
                        switch (phase)
                        {
                            case TouchPhase.Moved:
                                {
                                    if (isTouchMoved)
                                    {
                                        IA_OnePointDrag action = new IA_OnePointDrag();
                                        action.pos = Input.GetTouch(0).position;
                                        action.deltaPos = Input.GetTouch(0).deltaPosition;
                                        action.deltaTime = Input.GetTouch(0).deltaTime;
                                        actionMgr.AddInputAction(action);
                                    }
                                    else if (Vector2.Distance(Input.GetTouch(0).position, PressPos) >= ClickThreshhold)
                                    {
                                        IA_OnePointDrag action = new IA_OnePointDrag();
                                        action.pos = Input.GetTouch(0).position;
                                        action.deltaPos = Input.GetTouch(0).deltaPosition;
                                        action.deltaTime = Input.GetTouch(0).deltaTime;
                                        actionMgr.AddInputAction(action);
                                        isTouchMoved = true;
                                    }
                                    break;
                                }
                            case TouchPhase.Ended:
                                {
                                    if (isTouchMoved)
                                    {
                                        IA_OnePointDragExit action = new IA_OnePointDragExit();
                                        action.pos = Input.GetTouch(0).position;
                                        actionMgr.AddInputAction(action);
                                        isTouchMoved = false;
                                    }
                                    else if (Input.GetTouch(0).tapCount == 1)
                                    {
                                        if ((PressPos - Input.GetTouch(0).position).magnitude > ClickThreshhold)
                                        {
											IA_OnePointExit action = new IA_OnePointExit();
											action.pos = Input.GetTouch(0).position;
											actionMgr.AddInputAction(action);
                                        }
										else
										{
											IA_OnePointClick action = new IA_OnePointClick();
											action.pos = Input.GetTouch(0).position;
											actionMgr.AddInputAction(action);
										}

                                    }
                                    else if (Input.GetTouch(0).tapCount == 2)
                                    {
                                        IA_OnePointDoubleClick action = new IA_OnePointDoubleClick();
                                        action.pos = Input.GetTouch(0).position;
                                        actionMgr.AddInputAction(action);
                                    }
									else
									{
										IA_OnePointExit action = new IA_OnePointExit();
										action.pos = Input.GetTouch(0).position;
										actionMgr.AddInputAction(action);
									}
                                    break;
                                }
                            case TouchPhase.Began:
                                {
                                    IA_OnePointPress action = new IA_OnePointPress();
                                    action.pos = Input.GetTouch(0).position;
                                    actionMgr.AddInputAction(action);

                                    PressPos = action.pos;
                                    break;
                                }
                        }
                        break;
                    }
                case 2:
                    {
                        if ((Input.GetTouch(0).phase == TouchPhase.Moved
                             && Input.GetTouch(1).phase != TouchPhase.Ended
                             && Input.GetTouch(1).phase != TouchPhase.Canceled)
                            || (Input.GetTouch(1).phase == TouchPhase.Moved
                            && Input.GetTouch(0).phase != TouchPhase.Ended
                            && Input.GetTouch(0).phase != TouchPhase.Canceled))
                        {
                            if (Input.GetTouch(0).deltaPosition.magnitude > 1f ||
                                Input.GetTouch(1).deltaPosition.magnitude > 1f)
                            {
                                IA_TwoPointDrag action = new IA_TwoPointDrag();
                                action.pos0 = Input.GetTouch(0).position;
                                action.deltaPos0 = Input.GetTouch(0).deltaPosition;
                                action.deltaTime0 = Input.GetTouch(0).deltaTime;
                                action.pos1 = Input.GetTouch(1).position;
                                action.deltaPos1 = Input.GetTouch(1).deltaPosition;
                                action.deltaTime1 = Input.GetTouch(1).deltaTime;
                                actionMgr.AddInputAction(action);
                                isTwoTouchMoved = true;
                            }
                        }
                        else if (Input.GetTouch(0).phase == TouchPhase.Ended
                                 || Input.GetTouch(0).phase == TouchPhase.Canceled
                                 || Input.GetTouch(1).phase == TouchPhase.Ended
                                 || Input.GetTouch(1).phase == TouchPhase.Canceled)
                        {
                            if (isTwoTouchMoved)
                            {
                                IA_TwoPointDragExit action = new IA_TwoPointDragExit();
                                action.pos0 = Input.GetTouch(0).position;
                                action.pos1 = Input.GetTouch(1).position;
                                actionMgr.AddInputAction(action);
                                isTwoTouchMoved = false;
                            }
                        }
                        break;
                    }
                case 3:
                    {
                        IA_ShowDebugTool action = new IA_ShowDebugTool();

                        actionMgr.AddInputAction(action);
                        break;
                    }
            }
        }
		//键盘手势
		private void ParseKeyInput()
		{
            if (!Input.anyKeyDown)
            {
                return;
            }

            //if (Input.GetKeyDown("1"))
            //{
            //    Application.CaptureScreenshot("Screenshot.png", 1);
            //}
            //else if (Input.GetKeyDown("2"))
            //{
            //    Application.CaptureScreenshot("Screenshot.png", 2);
            //}
            //else if (Input.GetKeyDown("3"))
            //{
            //    Application.CaptureScreenshot("Screenshot.png", 3);
            //}
            //else if (Input.GetKeyDown("4"))
            //{
            //    Application.CaptureScreenshot("Screenshot.png", 4);
            //}
            //else if (Input.GetKeyDown("h"))
            //{
            //    if (App.Game.GameStateMgr.ActiveState is BuildingState)
            //    {
            //        App.Game.GUIFrameMgr.DeActive(GUIFrameID.MainUIFrame);
            //    }
            //    else if (App.Game.GameStateMgr.ActiveState is Battle3DState)
            //    {
            //        App.Game.GUIFrameMgr.DeActive(GUIFrameID.BattleMainFrame);
            //    }
            //}
            //else if (Input.GetKeyDown("s"))
            //{
            //    if (App.Game.GameStateMgr.ActiveState is BuildingState)
            //    {
            //        App.Game.GUIFrameMgr.Active(GUIFrameID.MainUIFrame);
            //    }
            //    else if (App.Game.GameStateMgr.ActiveState is Battle3DState)
            //    {
            //        App.Game.GUIFrameMgr.Active(GUIFrameID.BattleMainFrame);
            //    }
            //}
            //else if (Input.GetKeyDown("c"))
            //{
            //    var state = App.Game.GameStateMgr.ActiveState as BuildingState;
            //    if (state != null)
            //    {
            //        if (state.IsInClanMap)
            //        {
            //            App.Game.EventMgr.SendEvent(new StringGameEvent("BackFromClan", null));
            //        }
            //        else
            //        {
            //            ClanMapInfo mapInfo = new ClanMapInfo();
            //            mapInfo.FakeData();

            //            state.ToClanMap(mapInfo);
            //        }
            //    }
            //}
        }
		
		
		private void ParseEscKey()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				if (OnAndroidBackButton != null)
				{
					OnAndroidBackButton();
				}
			}
		}
		
		//鼠标手势
		private void ParseMouseInput()
		{
			ParseDrag();//是否拖动
			ParseClick();//是否点击
		}
		
		private void ParseDrag()
		{
            if (!isDragging && (Input.GetMouseButton(0) || Input.GetMouseButton(1))
                && lastMousePos != (Vector2)Input.mousePosition)
            {
                //Vector2 mousePos = lastMousePos;
                lastDragTime = Time.time;
                isDragging = true;
                if (Input.GetMouseButton(0))
                {
                    mouseType = 1;
                }
                else if (Input.GetMouseButton(1))
                {
                    mouseType = 2;
                }
            }
            else if (isDragging && ((Input.GetMouseButtonUp(0) && mouseType == 1)
                                    || (Input.GetMouseButtonUp(1) && mouseType == 2)))
            {
                if (mouseType == 1)
                {
                    IA_OnePointDragExit action = new IA_OnePointDragExit();
                    action.pos = Input.mousePosition;
                    actionMgr.AddInputAction(action);
                }
                else if (mouseType == 2)
                {
                    IA_TwoPointDragExit action = new IA_TwoPointDragExit();
                    action.pos0 = Input.mousePosition;
                    action.pos1 = Input.mousePosition;
                    actionMgr.AddInputAction(action);
                }

                isDragging = false;
                mouseType = 0;
                lastDragTime = 0;
            }

            if (isDragging && (Vector2)Input.mousePosition != lastMousePos)
            {
                Vector2 pos = Input.mousePosition;
                Vector2 deltaPos = pos - lastMousePos;

                if (mouseType == 1)
                {
                    IA_OnePointDrag action = new IA_OnePointDrag();
                    action.pos = pos;
                    action.deltaPos = deltaPos;
                    action.deltaTime = Time.time - lastDragTime;
                    actionMgr.AddInputAction(action);
                }
                else if (mouseType == 2)
                {
                    IA_TwoPointDrag action = new IA_TwoPointDrag();
                    action.pos0 = pos;
                    action.deltaPos0 = deltaPos;
                    action.deltaTime0 = Time.time - lastDragTime;
                    action.pos1 = pos;
                    action.deltaPos1 = deltaPos;
                    action.deltaTime1 = Time.time - lastDragTime;
                    actionMgr.AddInputAction(action);
                }

                lastDragTime = Time.time;
            }

            lastMousePos = Input.mousePosition;
        }
		
		
		private void ParseClick()
		{

            if (isDragging)
            {
                waitDClick = false;
                return;
            }
            if (Input.GetMouseButtonDown(0))//鼠标按下
            {
                IA_OnePointPress action = new IA_OnePointPress();
                action.pos = Input.mousePosition;
                actionMgr.AddInputAction(action);

                PressPos = action.pos;
            }

            if (waitDClick)
            {
                clickTime += Time.deltaTime;
                if ((Input.GetMouseButtonUp(0) && mouseType == 1)
                    || (Input.GetMouseButtonUp(1) && mouseType == 2))
                {
                    waitDClick = false;
                    if (mouseType == 1)
                    {
                        IA_OnePointDoubleClick action = new IA_OnePointDoubleClick();
                        action.pos = Input.mousePosition;
                        actionMgr.AddInputAction(action);
                    }
                    else
                    {
                        IA_TwoPointDoubleClick action = new IA_TwoPointDoubleClick();
                        action.pos0 = Input.mousePosition;
                        action.pos1 = Input.mousePosition;
                        actionMgr.AddInputAction(action);
                    }
                    mouseType = 0;
                }
                else if (clickTime > DCLICK_TIME)
                {
                    waitDClick = false;
                    if (mouseType == 1)
                    {
                        Vector2 TPos = Input.mousePosition;
                        if ((PressPos - TPos).magnitude < ClickThreshhold)
                        {
                            IA_OnePointClick action = new IA_OnePointClick();
                            action.pos = Input.mousePosition;
                            actionMgr.AddInputAction(action);
                        }
                    }
                    else if (mouseType == 2)
                    {
                        IA_TwoPointClick action = new IA_TwoPointClick();
                        action.pos0 = Input.mousePosition;
                        action.pos1 = Input.mousePosition;
                        actionMgr.AddInputAction(action);
                    }
                    mouseType = 0;
                }
            }
            //鼠标按钮点击或抬起点击重置点击时间
            else if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
            {
                waitDClick = true;
                clickTime = 0;
                //clickPos = Input.mousePosition;
                if (Input.GetMouseButtonUp(0))
                {
                    mouseType = 1;
                }
                else
                {
                    mouseType = 2;
                }
            }
        }
	}
}
