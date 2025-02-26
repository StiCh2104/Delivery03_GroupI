using UnityEngine;
using UnityEngine.UI;

public class ButtonScriptOnClick : MonoBehaviour 
{
    public int sceneChanger; //0=start,1=exit;
    void Start()
    {
        Button btn = GetComponent<Button>();
        switch (sceneChanger)
        {
            case 0: btn.onClick.AddListener(() => Manager.Instance.StartGame()); break;
            case 1: btn.onClick.AddListener(() => Manager.Instance.EndGame()); break;
        }
    }
}

