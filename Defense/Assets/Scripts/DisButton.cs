using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisButton : MonoBehaviour {
    public GameObject PagodaPrefab;
    private GameObject Pagoda;
    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void OnClick()
    {
        if (canPlacePagoda())
        {
            Pagoda = Instantiate(PagodaPrefab, GameManager.Instance.current_Building.transform.position, Quaternion.identity) as GameObject;//添加塔
        }
        GameManager.Instance.DesBuilding();
        Destroy(transform.parent.gameObject);
    }
    
    private bool canPlacePagoda()
    {
        return Pagoda == null;
    }
}
