using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayButtonWDelay : MonoBehaviour
{
    private IEnumerator Start()
    {
        Button button = GetComponent<Button>();
        button.interactable = false;
        yield return Utilis.GetWait(1f);
        button.interactable = true ;
    }
}
