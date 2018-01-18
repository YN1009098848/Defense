using System;
using System.Collections.Generic;

namespace CoreEngine.InputRef
{
    /** the input action base class 
    */
    public abstract class InputAction
	{
		/** the action id */
		public int id;
		
		/** used or not */
		public bool used;
	}

    public delegate void InputActionCallBack(InputAction inputAction);//输入行动回收委托

    /** 输入动作监听者
    */
    public class InputActionListener
	{
        /** 负责处理该行动的委托 */
        public InputActionCallBack callBack;
		
		/** 这个行为的优先级 */
		public InputActionPrior prior;
		
		/** 动作是否激活 */
		public bool active;
	}
	
	/** global input action manager
    *
    * manage global input actions, get the action from input mgr and
    * send it to the responsing handler
    */
	public class InputActionMgr
	{

        /** the input action list */
        private List<InputAction> inputActions = new List<InputAction>();
		
		/** input action listener list */
		private List<List<InputActionListener>> listeners = new List<List<InputActionListener>>();

        public InputActionMgr()
		{
			for (int i = 0; i < Enum.GetValues(typeof(InputActionPrior)).Length; i++)
			{
				listeners.Add(new List<InputActionListener>());
			}
		}
		
		/** register input action listener
        *
        * @param[in] listener input action listener to register
        */
		public void RegisterListener(InputActionListener listener)
		{
			if(listener == null)
			{
				return;
			}

            var list = listeners[(int)listener.prior];
			
			if (!list.Contains(listener))
			{
				list.Add(listener);
			}
			else
			{
                //Logger.Log("duplicate listener");
                return;
			}
		}
		
		/** unregister input action listener
        *
        * @param[in] listener input action listener to unregister
        */
		public void UnRegisterListener(InputActionListener listener)
		{
			if(listener == null)
			{
				return;
			}
			
			var list = listeners[((int)listener.prior)];
			list.Remove(listener);
		}
		
		/** add a new input action
        *
        * @param[in] action the new input action
        */
		public void AddInputAction(InputAction action)
		{
			if(action == null)
			{
				return;
			}
			
			if(InteruptAction(action))
			{
				return;
			}
			
			if(!inputActions.Contains(action))
			{
				inputActions.Add(action);
			}
		}
		
		private bool InteruptAction(InputAction action)
		{
// 			if (App.Game.NeedCg)
// 			{
// 				return true;
// 			}
// 			
// 			if (App.Game.GUIFrameMgr.IsActive(GUIFrameID.NetWaitUIFrame))
// 			{
// 				return true;
// 			}
// 			
			return false;
		}
		
		/** remove a input action
        *
        * @param[in] action the removed input action
        */
		public void RemoveInputAction(InputAction action)
		{
			if(action == null)
			{
				return;
			}
			
			if(!inputActions.Contains(action))
			{
				inputActions.Remove(action);
			}
		}
		
		/** the main loop to handle all new input actions
        */
		public void Execute()
		{
			if (inputActions.Count == 0)
			{
				return;
			}
			
			foreach(InputAction action in inputActions)
			{
				if (action.used)
				{
					break;
				}
				
				foreach (var list in listeners)
				{
					int count = list.Count;
					for (int i = 0; i < count; i++)
					{
						if (action.used)
						{
							break;
						}
						
						var listener = list[i];
						if (listener.active)
						{
							listener.callBack(action);
						}
					}
				}
			}
			
			inputActions.Clear();  
		}
	}
}

