using UnityEngine;

public class SpellInfoTableUI : MonoBehaviour
{
    [SerializeField] private SpellDataUI _spellPrefab;
    [SerializeField] private Transform _contentParent;
    [SerializeField] private SpellFullInfoTip _fullSpellInfo;
    void Start()
    {
        foreach (var spell in GameLibrary.Instance.Fractions.GetAllSpells())
        {
            var im = Instantiate(_spellPrefab, _contentParent);
            im.Init(spell, ShowFullSpellInfo);
        }
        Instantiate(_fullSpellInfo, transform.root);
    }

    private void ShowFullSpellInfo(SpellSO spell)
    {
        SpellFullInfoTip.ShowInfo?.Invoke(spell);
    }
}
