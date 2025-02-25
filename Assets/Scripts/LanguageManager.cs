using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager Instance;

    public enum Language { English, Spanish, Catalan }
    private Language currentLanguage = Language.English;

    // Diccionarios de traducción
    private Dictionary<string, string> englishDict = new Dictionary<string, string>()
{
    {"starttext", "Start"},
    {"exittext", "Exit"},
    {"lenguagetext", "Language"},
    {"coinstext", "Coins"},
    {"inventorytext", "Inventory"},
    {"shoptext", "Shop"},
    {"buytext", "<--Buy"},
    {"selltext", "Sell-->"},
    {"usetext", "Use"}
};

    private Dictionary<string, string> spanishDict = new Dictionary<string, string>()
{
    {"starttext", "Empezar"},
    {"exittext", "Salir"},
    {"lenguagetext", "Lenguaje"},
    {"coinstext", "Monedas"},
    {"inventorytext", "Inventario"},
    {"shoptext", "Tienda"},
    {"buytext", "<--Comprar"},
    {"selltext", "Vender-->"},
    {"usetext", "Usar"}
};

    private Dictionary<string, string> catalanDict = new Dictionary<string, string>()
{
    {"starttext", "Començar"},
    {"exittext", "Sortir"},
    {"lenguagetext", "Lenguatge"},
    {"coinstext", "Calers"},
    {"inventorytext", "Inventari"},
    {"shoptext", "Botiga"},
    {"buytext", "<--Comprar"},
    {"selltext", "Vendre-->"},
    {"usetext", "Usar"}
};


    private void Awake()
    {
        // Asegurarse de que solo haya una instancia del LanguageManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void ChangeLanguage(Language newLanguage)
    {
        currentLanguage = newLanguage;
        UpdateText();
    }

    private void UpdateText()
    {
        Text[] texts = FindObjectsOfType<Text>(); //find all <text> objects in scene
        foreach (Text text in texts)
        {
            string key = text.name.ToLower();
            if (currentLanguage == Language.English && englishDict.ContainsKey(key))
                text.text = englishDict[key];
            else if (currentLanguage == Language.Spanish && spanishDict.ContainsKey(key))
                text.text = spanishDict[key];
            else if (currentLanguage == Language.Catalan && catalanDict.ContainsKey(key))
                text.text = catalanDict[key];
        }
    }
}
