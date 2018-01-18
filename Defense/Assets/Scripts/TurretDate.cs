using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class TurretDate{
    public GameObject turretPrefab;//塔的预制体
    public int cost;//塔价格
    public GameObject turretUpGradedPrefab;//升级塔的预制体
    public int costUpGraded;//升级价格
    public TurretType type;
}
//塔的数据类型
public enum TurretType
{
    LaserTurret,//激光炮台
    MissileTurret,//炮弹炮台
    StandardTurret,//基础炮台
    LibraTurret//士兵塔
}
