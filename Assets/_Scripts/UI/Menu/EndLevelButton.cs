using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class EndLevelButton : MonoBehaviour
{
    public bool IsWin = false;


    public void OnPointerEnterEvent()
    {
        transform.DOScale(1.1f, 0.15f).SetEase(Ease.InSine).From().SetUpdate(true).OnComplete(() => transform.DOScale(1f, 0f).SetUpdate(true));
        EventBus.ButtonSelectSound?.Invoke();
    }

    public void BackToMainMenu() { EventBus.BackToMainMenuEvent?.Invoke(); }
    public void RestartOrNExtLevel() 
    {
        if (IsWin) 
        {
            GameManager.Instance.SavedSceneIndex += 1;
            GameManager.Instance.Hero.level++;
            GameManager.Instance.EnemyHero = GameManager.Instance.CreateEnemyHero();
            if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 2)
            {
                EventBus.NextLevelEvent?.Invoke();
            }
               
            else
            {
                GetComponentInParent<UICanvas>().RewardWindow.gameObject.SetActive(true);
            }
        }
        else
            EventBus.RestartLevelEvent?.Invoke();
    }
}
