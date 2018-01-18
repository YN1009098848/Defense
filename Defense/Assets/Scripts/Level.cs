using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;

public class Level 
{

    /// <summary>
    /// 关卡ID
    /// </summary>
    public string ID;

    /// <summary>
    /// 关卡名称
    /// </summary>
    public string Name;

    public string Difficulty;
    /// <summary>
    /// 关卡是否解锁  
    /// </summary>
    public string UnLock = "0";

}
