using UnityEngine;
using UnityEngine.UI;

public class LanguageDropdown : MonoBehaviour
{
    public Button languageButton;  // Bot�n principal
    public GameObject languagePanel;  // Panel que contiene los botones de idioma
    public Button englishButton;  // Bot�n para cambiar a ingl�s
    public Button spanishButton;  // Bot�n para cambiar a espa�ol
    public Button catalanButton;  // Bot�n para cambiar a catal�n

    void Start()
    {
        // Asegurarse de que el panel est� inicialmente oculto
        languagePanel.SetActive(false);

        // Agregar listeners para los botones de idioma
        languageButton.onClick.AddListener(ToggleLanguagePanel);
        englishButton.onClick.AddListener(() => ChangeLanguage(LanguageManager.Language.English));
        spanishButton.onClick.AddListener(() => ChangeLanguage(LanguageManager.Language.Spanish));
        catalanButton.onClick.AddListener(() => ChangeLanguage(LanguageManager.Language.Catalan));
    }

    // Cambiar entre mostrar/ocultar el panel de idiomas
    void ToggleLanguagePanel()
    {
        languagePanel.SetActive(!languagePanel.activeSelf);  // Alterna la visibilidad
    }

    // M�todo para cambiar el idioma
    void ChangeLanguage(LanguageManager.Language language)
    {
        LanguageManager.Instance.ChangeLanguage(language);
        languagePanel.SetActive(false);  // Cerrar el panel despu�s de elegir el idioma
    }
}
