namespace uFrame.ProjectC {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    //using uFrame.IOC;
   // using uFrame.Kernel;
    //using uFrame.MVVM;
    //using UniRx;

    //public delegate void ShowMsgHandler(string msg);

    //public class LoggerService : LoggerServiceBase {

        
    //    private static LoggerService __inst = null;
    //    private List<ShowMsgHandler> mListHandlers = new List<ShowMsgHandler>();


    //    public override void Setup()
    //    {
    //        base.Setup();

    //        __inst = this;
    //    }


    //    public void RegisterHandler(ShowMsgHandler handle)
    //    {
    //        if (!mListHandlers.Contains(handle))
    //            mListHandlers.Add(handle);
    //    }

    //    public void UnRegisterHandler(ShowMsgHandler handle)
    //    {
    //        if (mListHandlers.Contains(handle))
    //            mListHandlers.Remove(handle);
    //    }

    //    public static LoggerService GetInstance()
    //    {
    //        return __inst;
    //    }

    //    public void ShowMsg(string msg)
    //    {
    //        for (int i = 0; i < mListHandlers.Count; i++)
    //        {
    //            mListHandlers[i](msg);
    //        }
    //    }

    //    public void ShowPopMsg(GameObject obj, string msg)
    //    {
    //        if (msg.Length > 0)
    //        {
    //            var popText = Instantiate(Resources.Load("PopMsg")) as GameObject;
    //            popText.GetComponent<UnityEngine.UI.Text>().text = msg;
    //            popText.transform.SetParent(obj.transform);
    //            popText.transform.localPosition = Vector3.zero;
    //            popText.transform.localScale = Vector3.one;

    //            popText.GetComponent<Animator>().Play("popmessage", -1, 0.0f);
    //        }
    //    }
    //}
}
