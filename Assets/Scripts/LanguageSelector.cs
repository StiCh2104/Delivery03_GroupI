using UnityEngine;
using UnityEngine.UI;

public class LanguageSelector : MonoBehaviour
{
    public GameObject languagePanel; 
    public Button languageButton;    
    public Button ButtonToSpanish;
    public Button ButtonToCatalan;
    public Button ButtonToEnglish;
    public Button ButtonToGerman;

    private void Start()
    {
        languagePanel.SetActive(false);

        languageButton.onClick.AddListener(ToggleLanguagePanel);
        ButtonToSpanish.onClick.AddListener(ChangeLanguageToSpanish);
        ButtonToCatalan.onClick.AddListener(ChangeLanguageToCatalan);
        ButtonToEnglish.onClick.AddListener(ChangeLanguageToEnglish);
        ButtonToGerman.onClick.AddListener(ChangeLanguageToGerman);
    }

    private void ToggleLanguagePanel()
    {
        languagePanel.SetActive(!languagePanel.activeSelf);
    }


    private void ChangeLanguageToSpanish()
    {
        Localizer.Instance.SetLanguage(Localizer.Language.Spanish); 
        languagePanel.SetActive(false);
    }
    private void ChangeLanguageToCatalan()
    {
        Localizer.Instance.SetLanguage(Localizer.Language.Catalan);
        languagePanel.SetActive(false);
    }
    private void ChangeLanguageToEnglish()
    {
        Localizer.Instance.SetLanguage(Localizer.Language.English);
        languagePanel.SetActive(false);
    }
    private void ChangeLanguageToGerman()
    {
        Localizer.Instance.SetLanguage(Localizer.Language.German);
        languagePanel.SetActive(false);
    }
}
