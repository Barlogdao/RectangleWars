using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections;

public class EndScreen : MonoBehaviour
{

    public Transform Text;
    public Transform Hill;
    public Transform Mountain;
    public Transform MountainFar;
    public Image sky;
    [SerializeField] private Image _endButton;

    
    public Gradient gradient;
    Tween loop;
    private IEnumerator Start()
    {
        loop = sky.DOGradientColor(gradient, 10f).SetEase(Ease.Linear).SetLoops(-1);
        MountainFar.DOMoveY(-10f, 1f).From();
        Mountain.DOMoveY(-10f, 1.2f).From();
        Hill.DOMoveY(-10f, 2f).SetEase(Ease.OutQuad).From();
        Text.DOMoveY(-20f, 20f).From().OnComplete(Final);
        yield return new WaitForSeconds(2.5f);
        _endButton.DOFade(1f, 0.5f);

    }

    public void Final()
    {
        loop.Kill();
        EventBus.BackToMainMenuEvent?.Invoke();
    }

}
