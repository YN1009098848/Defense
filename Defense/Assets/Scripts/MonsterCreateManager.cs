using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MonsterCreateManager : MonoBehaviour {
    //波次列表
    private List<WaveDate> wavesList = new List<WaveDate>();
    public static MonsterCreateManager instance;
    public string LoadWaveJsonName;
    private List<WaveDate> Monsterwaves= new List<WaveDate>();
    void Awake()
    {
        instance = this;
        isSceneName();
        LoadJSON();
        if (mWaves.Count > 0)
        {
            CreateWave();
        }
    }
    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }
    public List<WaveDate> mWaves
    {
        set { }
        get { return wavesList; }

    }
    private void isSceneName()
    {
        string SceneName = SceneManager.GetActiveScene().name;
        switch (SceneName)
        {
            case "Level1Scene":
                LoadWaveJsonName = "WaveDate1";
                break;
            case "Level2Scene":
                LoadWaveJsonName = "WaveDate2";
                break;
            default:
                break;
        }
    }
    private void LoadJSON()
    {
        string JsonName = LoadWaveJsonName;
        TextAsset text = Resources.Load(JsonName) as TextAsset;
        JsonData jd = JsonMapper.ToObject(text.text);//JsonMapper.ToObject解析文件
        JsonData gameObjectArray = jd["WaveList"];//导出GameObject
        int i;
        for (i = 0; i < gameObjectArray.Count; i++)//遍历数组，得到所有的关卡
        {
            WaveDate mWave = new WaveDate();
            mWave.ID = (string)gameObjectArray[i]["ID"];
            mWave.MonsterMod = (string)gameObjectArray[i]["MonsterMod"];
            mWave.MonsterCount = (int)gameObjectArray[i]["MonsterCount"];
            mWave.MonsterCreateTime = Convert.ToSingle((string)gameObjectArray[i]["MonsterCreateTime"]);
            mWaves.Add(mWave);//向list里添加一个关卡
        }
    }
    public void CreateWave()
    {
        
        int j;
        for (j = 0; j < mWaves.Count; j++)
        {
            WaveDate LevelWave=new WaveDate();
            LevelWave.ID = mWaves[j].ID;
            LevelWave.MonsterMod = mWaves[j].MonsterMod;
            LevelWave.MonsterCount = mWaves[j].MonsterCount;
            LevelWave.MonsterCreateTime = mWaves[j].MonsterCreateTime;
            Monsterwaves.Add(LevelWave);
        }
    }
}
