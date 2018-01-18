using UnityEngine;
using System.Collections;

namespace CoreEngine.InputRef
{
    public class InputComponet : MonoBehaviour
    {
        private bool _isInputEnabled = false;

        public InputActionMgr g_inputActionMgr = InputMgr.GetInputActionMgr();

        private InputActionListener m_inputActionListener = null;

        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }

        void OnDestroy()
        {
            DisableInput();
        }

        public bool IsInputEnabled
        {
            get
            {
                return _isInputEnabled;
            }
            set { }
        }
        //输入事件注册
        public void RegisterInputAction(InputActionPrior pri, InputActionCallBack call)
        {
            if (m_inputActionListener != null && m_inputActionListener.callBack == call)
                return;
            else
            {
                if(m_inputActionListener == null)
                    m_inputActionListener = new InputActionListener();
                else
                    g_inputActionMgr.UnRegisterListener(m_inputActionListener);

                m_inputActionListener.callBack = call;
                m_inputActionListener.prior = pri;
                m_inputActionListener.active = true;

                EnableInput(m_inputActionListener);
            }

        }

        public void EnableInput(InputActionListener listener)
        {
            if (listener == null && m_inputActionListener == null)
            {
               // Logger.Log("Error about Input");
                return;
            }

            if (_isInputEnabled)
            {
                if (listener != m_inputActionListener)
                {
                    g_inputActionMgr.UnRegisterListener(m_inputActionListener);
                    m_inputActionListener = listener;

                    g_inputActionMgr.RegisterListener(listener);
                }
            }
            else
            {
                if (listener != null)
                {
                    m_inputActionListener = listener;
                }

                g_inputActionMgr.RegisterListener(listener);
            }

            _isInputEnabled = true;
        }

        public void DisableInput()
        {
            if (m_inputActionListener != null)
            {
                g_inputActionMgr.UnRegisterListener(m_inputActionListener);
            }
                

            _isInputEnabled = false;
        }

        public void SetActive(bool act)
        {
            if(m_inputActionListener != null)
            {
                m_inputActionListener.active = act;
            }
        }
    }

}
