using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField]
    Image progress;
    private void Start()
    {
        progress.fillAmount = 0;
        StartCoroutine(LoadSceneAsync(GameManager.Instance.SavedSceneIndex));

    }
    IEnumerator LoadSceneAsync(int sceneID)
    {
        AsyncOperation opertation = SceneManager.LoadSceneAsync(sceneID);
        while (!opertation.isDone)
        {
            float progressValue = Mathf.Clamp01(opertation.progress / 0.9f);
            progress.fillAmount = progressValue;
            yield return null;
        }
    }

}
