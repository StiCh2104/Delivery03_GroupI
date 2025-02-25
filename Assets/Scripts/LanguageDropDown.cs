using UnityEngine;
using UnityEngine.UI;

public class LanguageDropdown : MonoBehaviour
{
    public Button languageButton;  // Botón principal
    public GameObject languagePanel;  // Panel que contiene los botones de idioma
    public Button englishButton;  // Botón para cambiar a inglés
    public Button spanishButton;  // Botón para cambiar a español
    public Button catalanButton;  // Botón para cambiar a catalán

    void Start()
    {
        // Asegurarse de que el panel está inicialmente oculto
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

    // Método para cambiar el idioma
    void ChangeLanguage(LanguageManager.Language language)
    {
        LanguageManager.Instance.ChangeLanguage(language);
        languagePanel.SetActive(false);  // Cerrar el panel después de elegir el idioma
    }
}
