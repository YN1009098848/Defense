using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtMe : MonoBehaviour {
    Quaternion direction;
    // Use this for initialization
    void Start()
    {
        direction = Quaternion.AngleAxis(180, Vector3.down);
    }
	//TODO Update is called once per frame
	void Update () {
        transform.rotation = Camera.main.transform.rotation * direction;//让血条朝向我（-1）
    }
}
