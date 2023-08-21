
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

        static Dictionary<Locale, Sprite> _localSpriteDictionnary = new Dictionary<Locale, Sprite>();
        static List<Locale> _locale;

        void Awake()
        {
            _dropdown = GetComponent<TMP_Dropdown>();
            // Wait for the localization system to initialize
            if (_locale != null && _localSpriteDictionnary != null) return;

            StartCoroutine(LoadLocalization());
        }

        private void OnEnable()
        {
            _dropdown.onValueChanged.AddListener(LocaleSelected);

            if (_locale == null) return;

            PopulateDropdown();
        }

        private void OnDisable()
        {
            _dropdown.onValueChanged.RemoveListener(LocaleSelected);
        }


        IEnumerator LoadLocalization()
        {
            yield return LocalizationSettings.InitializationOperation;

            _locale = LocalizationSettings.AvailableLocales.Locales;

            foreach (Locale locale in _locale)
            {
                Sprite sprite = LocalizationSettings.AssetDatabase.GetLocalizedAsset<Sprite>("Images", "Flag", locale);
                _localSpriteDictionnary.Add(locale, sprite);
            }

            PopulateDropdown();
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