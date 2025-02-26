using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Localizer : MonoBehaviour
{
    public enum Language
    {
        English,
        Catalan,
        Spanish,
        German
    }

    public static Localizer Instance;//singleton
    public TextAsset DataSheet; // CSV file

    private Dictionary<string, Dictionary<Language, string>> data = new();
    public Language DefaultLanguage = Language.English;//starting language
    private Language currentLanguage;//change language

    public static event Action OnLanguageChange;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        LoadLanguageSheet();
        SetLanguage(DefaultLanguage);
    }

    private void LoadLanguageSheet()
    {
        data = new Dictionary<string, Dictionary<Language, string>>(); // Reiniciamos el diccionario
        string[] lines = DataSheet.text.Split('\n'); // Dividimos por líneas

        if (lines.Length < 2) return; // Si el archivo no tiene contenido suficiente, salimos

        string[] headers = lines[0].Trim().Split(','); // Primera línea (nombres de los idiomas)

        for (int i = 1; i < lines.Length; i++) // Recorremos desde la segunda línea
        {
            string[] values = lines[i].Trim().Split(','); // Dividimos en columnas

            if (values.Length != headers.Length) continue; // Si la línea está incompleta, la ignoramos

            string key = values[0].Trim(); // Primera columna = clave del texto
            Dictionary<Language, string> translations = new();

            for (int j = 1; j < headers.Length; j++) // Empezamos en 1 porque la 0 es la clave
            {
                if (Enum.TryParse(headers[j].Trim(), out Language lang)) // Convertimos el idioma a enum
                {
                    translations[lang] = values[j].Trim(); // Guardamos la traducción sin espacios extra
                }
            }

            data[key] = translations; // Guardamos la clave con sus traducciones
        }
    }
    public void SetLanguage(Language lang)
    {
        currentLanguage = lang;
        OnLanguageChange?.Invoke();
        FindObjectOfType<ShopSystem>().UpdateUI();
    }

    public string GetText(string key)
    {
        if (data.TryGetValue(key, out var translations)) // Buscamos la clave en el diccionario
        {
            if (translations.TryGetValue(currentLanguage, out string text)) // Buscamos la traducción en el idioma actual
            {
                return text; // Si existe, la devolvemos
            }
        }
        return key; // Si no encontramos la clave o el idioma, devolvemos la clave original
    }

}
