using UnityEngine;
using TMPro;

public class ComlexitySelector : MonoBehaviour
{
    private void OnEnable()
    {
        GameManager.Instance.GameComplexity= (Complexity)GetComponent<TMP_Dropdown>().value;
    }

    public void ChangeGameComplexity(int option)
    {
        GameManager.Instance.GameComplexity = (Complexity)option;
    }

}
