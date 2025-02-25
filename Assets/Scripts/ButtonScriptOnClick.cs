using UnityEngine;
using UnityEngine.UI;

public class ButtonScriptOnClick : MonoBehaviour 
{
    public int sceneChanger; //0=start,1=exit;
    public bool isLanguageChangeButton;
    private int languageChanger;
    void Start()
    {
        Button btn = GetComponent<Button>();
        if (isLanguageChangeButton)
        {
            languageChanger = sceneChanger;
            switch (languageChanger)
            {
                case 0: //eng
                    btn.onClick.AddListener(() => LanguageManager.Instance.ChangeLanguage(LanguageManager.Language.English));
                    Debug.Log("Cambio a ingles");
                    break;
                case 1: //esp
                    btn.onClick.AddListener(() => LanguageManager.Instance.ChangeLanguage(LanguageManager.Language.Spanish));
                    Debug.Log("Cambio a ingles");
                    break;
                case 2: // cat
                    btn.onClick.AddListener(() => LanguageManager.Instance.ChangeLanguage(LanguageManager.Language.Catalan));
                    Debug.Log("Cambio a ingles");
                    break;
            }
                
        }
        else
        {
            switch (sceneChanger)
            {
                case 0: btn.onClick.AddListener(() => Manager.Instance.StartGame()); break;
                case 1: btn.onClick.AddListener(() => Manager.Instance.EndGame()); break;
            }
        }
    }
}

