using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    [SerializeField]
    private bool needPlayerSave=false;
    [SerializeField]
    private bool _isMovabale = false;
    void Start()
    {
        if(SceneManager.GetActiveScene().buildIndex == 0 && _isMovabale)
            transform.DOMoveY(-15,1f).SetEase(Ease.OutBounce).From().SetDelay(0.5f);
        if(needPlayerSave && GameManager.Instance.SavedSceneIndex == 0)
            GetComponent<Button>().interactable = false;
    }

    public void OnPointerEnterEvent()
    {
        if (needPlayerSave && GameManager.Instance.SavedSceneIndex == 0)
            return;
        transform.DOScale(1.1f, 0.15f).SetEase(Ease.InSine).From().SetUpdate(true).OnComplete(() => transform.DOScale(1f, 0f).SetUpdate(true));
        EventBus.ButtonSelectSound?.Invoke();
    }

    public void NewGame() {EventBus.NewGameEvent?.Invoke();}
    public void RandomGame() { EventBus.RandomGameStart?.Invoke(); }
    public void ContinueGame() { EventBus.ContinueGameEvent?.Invoke(); }
    
    public void RestartLevel() { EventBus.RestartLevelEvent?.Invoke(); }

    public void BackToMainMenu() { EventBus.BackToMainMenuEvent?.Invoke(); }
 
    public void QuitGame() { EventBus.QuitGameEvent?.Invoke(); }
    public void OpenSettings() { }
 


}
