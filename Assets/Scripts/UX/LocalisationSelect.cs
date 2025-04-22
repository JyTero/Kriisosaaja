using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class LocalisationSelect : MonoBehaviour
{

    public TMP_Dropdown dropdown;

    public void Start()
    {
        StartCoroutine(changeLocale());
    }

    IEnumerator changeLocale()
    {
        // Wait for the localization system to initialize
        yield return LocalizationSettings.InitializationOperation;

        // Generate list of available Locales
        var options = new List<TMP_Dropdown.OptionData>();
        int selected = 0;
        for (int i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; ++i)
        {
            var locale = LocalizationSettings.AvailableLocales.Locales[i];
            if (LocalizationSettings.SelectedLocale == locale)
                selected = i;
            options.Add(new TMP_Dropdown.OptionData(locale.name));
        }
        dropdown.options = options;

        //index of selected locale
        dropdown.value = selected;

        //triggers the locale change
        dropdown.onValueChanged.AddListener(LocaleSelected);
    }

    //does the changing
    static void LocaleSelected(int index)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
    }

    //triggers when value changes in enumerator
    public void OnValueChange(int value)
    {
        LocaleSelected(value);
    }
}