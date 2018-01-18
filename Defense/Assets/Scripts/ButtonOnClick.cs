using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonOnClick : MonoBehaviour
{
    private Button button;
    // Use this for initialization
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(delegate ()
        {
            this.OnClick(button);
        });
    }
    //按钮事件绑定

    public void OnClick(Button sender)
    {
        switch (sender.name)
        {
            case "StartGame":
                StartManager.Instance.OnStartGame();
                break;
            case "Explain":
                StartManager.Instance.OnExplain();
                break;
            case "Escape":
                StartManager.Instance.OnEscape();
                break;
            case "BackStart1":
                Debug.Log("Start1Back");
                StartManager.Instance.OnStart1Back();
                break;
            case "Start2Back":
                Debug.Log("Start2Back");
                StartManager.Instance.OnStart2Back();
                break;
            case "Restart":
                GameManager.Instance.OnButtonRetry();
                break;
            case "Start Screen":
                GameManager.Instance.OnButtonMenu();
                break;
            default:
                break;
        }
    }
}
