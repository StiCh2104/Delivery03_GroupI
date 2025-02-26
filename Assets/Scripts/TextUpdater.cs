using TMPro; // Asegúrate de importar esto
using UnityEngine;

public class TextUpdater : MonoBehaviour
{
    public string key;
    private TMP_Text textComponent; // Cambio a TMP_Text

    void Start()
    {
        textComponent = GetComponent<TMP_Text>(); // Cambio aquí
        Localizer.OnLanguageChange += UpdateText;
        UpdateText();
    }

    void OnDestroy()
    {
        Localizer.OnLanguageChange -= UpdateText;
    }

    void UpdateText()
    {
        if (textComponent != null)
            textComponent.text = Localizer.Instance.GetText(key);
        else
            Debug.LogError("TextUpdater: No se encontró un componente de texto en " + gameObject.name);
    }
}
