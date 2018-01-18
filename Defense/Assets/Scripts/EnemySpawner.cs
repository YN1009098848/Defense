using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour {

    public static int CountEnemyAlive;//记录销毁
    public Transform mStart;
    public List<GameObject> mMonstList;
    public float waveRateTime = 5.0f;
    public static EnemySpawner Instance;//EnemySpawner实例
    private Coroutine coroutine;//??
    public GameObject StartWaveButton;//开始出怪按钮
    public bool isStartWave;//是否开始出怪
    private int mCurrentWaveIndex = 0;//波次索引
    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        Button btn = (Button)StartWaveButton.GetComponent<Button>();
        btn.onClick.AddListener(OnStartWave);
        //coroutine = StartCoroutine(SpawnEnemy());
        //coroutine = StartCoroutine(SpawnEnemy()); 
    }
     void Update()
    {
        
    }

    public IEnumerator startWave(int index)//遍历每一波
    {
        WaveDate oneWave = MonsterCreateManager.instance.mWaves[index];
        int count = oneWave.MonsterCount;
        float creatTime = oneWave.MonsterCreateTime;
        string monsterName = oneWave.MonsterMod;
        for (int i = 0; i < count; i++)
        {
            GameObject Monster = Resources.Load("Prefabs/Monster/" + monsterName) as GameObject;
            GameObject obj = GameObject.Instantiate(Monster, mStart.position, Quaternion.identity);
            mMonstList.Add(obj);
            CountEnemyAlive++;
            yield return new WaitForSeconds(creatTime);
        }
        StartWaveButton.SetActive(true);
    }
    //遍历一共几波
    public void OnStartWave()
    {
        isStartWave = true;   
        if (mCurrentWaveIndex < MonsterCreateManager.instance.mWaves.Count)
        {
            StartCoroutine(startWave(mCurrentWaveIndex));
            mCurrentWaveIndex++;
        }
        if (mCurrentWaveIndex== MonsterCreateManager.instance.mWaves.Count)//遍历完后没失败就胜利
        {
            StartWaveButton.SetActive(false);
            GameManager.Instance.Win();
        }
        StartWaveButton.SetActive(false);
    }
}