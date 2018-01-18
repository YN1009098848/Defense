using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelButtonOnCilk : MonoBehaviour {
    private string nmnumber = "";//保存关卡名字
    private int index;
    // Use this for initialization
    void Start () {
        string name=this.transform.name;
        nmnumber = Regex.Replace(name, @"[^\d.\d]", "");
        index = int.Parse(nmnumber);
        Button btn = (Button)transform.GetComponent<Button>();
        btn.onClick.AddListener(OnLevelLoaded);
    }
	// Update is called once per frame
	void Update () {
		
	}
    public void OnLevelLoaded()
    {
        Debug.Log(index);
        SceneManager.LoadScene(index);
    }
}
