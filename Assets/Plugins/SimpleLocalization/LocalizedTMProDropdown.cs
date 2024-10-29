using UnityEngine;
using TMPro;


namespace Assets.SimpleLocalization
{
    [RequireComponent(typeof(TMP_Dropdown))]
    public class LocalizedTMProDropdown : MonoBehaviour
    {
        public string[] LocalizationKeys;

        public void Start()
        {
            Localize();
            LocalizationManager.LocalizationChanged += Localize;
        }

        public void OnDestroy()
        {
            LocalizationManager.LocalizationChanged -= Localize;
        }

        private void Localize()
        {
            var dropdown = GetComponent<TMP_Dropdown>();

            for (var i = 0; i < LocalizationKeys.Length; i++)
            {
                dropdown.options[i].text = LocalizationManager.Localize(LocalizationKeys[i]);
            }

            if (dropdown.value < LocalizationKeys.Length)
            {
                dropdown.captionText.text = LocalizationManager.Localize(LocalizationKeys[dropdown.value]);
            }
        }
    }
}
