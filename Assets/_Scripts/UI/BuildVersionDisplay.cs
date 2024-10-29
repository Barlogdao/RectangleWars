using TMPro;
using UnityEngine;

namespace RB.UI
{
    public class BuildVersionDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _versionText;

        private void Start()
        {
            _versionText.text = "v " + Application.version;
        }
    }
}


