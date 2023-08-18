
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.U2D;
using UnityEngine.UI;

namespace Assets.Scripts.Controller.Ui.LanguageSelection
{
    public class LanguageSelectorDropDownController : MonoBehaviour
    {
        TMP_Dropdown _dropdown;
        List<Locale> _currLocalList;
        Dictionary<Locale, Sprite> _localSpriteDictionnary = new Dictionary<Locale, Sprite>();

        IEnumerator Start()
        {
            _dropdown = GetComponent<TMP_Dropdown>();
            // Wait for the localization system to initialize
            yield return LocalizationSettings.InitializationOperation;

            foreach (Locale locale in LocalizationSettings.AvailableLocales.Locales)
            {
                Sprite sprite = LocalizationSettings.AssetDatabase.GetLocalizedAsset<Sprite>("Images", "Flag", locale);
                _localSpriteDictionnary.Add(locale, sprite);
            }
            PopulateDropdown();

            // Generate list of available Locales

            _dropdown.onValueChanged.AddListener(LocaleSelected);
        }

        void PopulateDropdown()
        {
            var options = new List<TMP_Dropdown.OptionData>();
            _currLocalList = new List<Locale>();

            foreach (Locale locale in _localSpriteDictionnary.Keys)
            {
                //We add all locale except the selected one
                if (LocalizationSettings.SelectedLocale == locale) continue;

                _currLocalList.Add(locale);          
                options.Add(new TMP_Dropdown.OptionData(_localSpriteDictionnary.GetValueOrDefault(locale)));
            }

            //We add the selected at the end, to hide it and avoid to distrub navigation
            _currLocalList.Add(LocalizationSettings.SelectedLocale);
            options.Add(new TMP_Dropdown.OptionData((_localSpriteDictionnary.GetValueOrDefault(LocalizationSettings.SelectedLocale))));

            _dropdown.options = options;

            _dropdown.value = options.Count-1;
        }

        void LocaleSelected(int index)
        {
            LocalizationSettings.SelectedLocale = _currLocalList[index];
            PopulateDropdown();
        }
    }
}