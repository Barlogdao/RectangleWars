using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using Assets.SimpleLocalization;

public class UICanvas : MonoBehaviour
{
    const string LOOSE = "Message.Loose";
    const string WIN = "Message.Win";
    const string FINALWIN = "Message.FinalWin";
    const string AGAIN = "Button.TryAgain";
    const string NEXT = "Button.Next";
    [SerializeField]
    private Transform _settingsWindow;
    [SerializeField]
    private HeroHealthBar allyBar, enemyBar;
    private ArmyPanel _armyPanel;
    private AbilityPanel _abilityPanel;
    [SerializeField]
    HeroArmyInformation _allyArmy, _enemyArmy;

    [Header("Игрок")]
    private Player _allyPlayer;
    private Player _enemyPlayer;


    [Header("Меню паузы")]
    public Transform PauseWindow;
    private CanvasGroup _generalgroup;
    [SerializeField]
    private Button _resumeButton, _endGameStarButton;


    [Header("Меню конца уровня")]
    [SerializeField] private Button _exitButton;
    public TMPro.TextMeshProUGUI EndLevelMessage;
    public TMPro.TextMeshProUGUI NextButtontext;
    public EndLevelButton EndLevelButton;
    public Transform Viewport;
    public Image EndLevelWindow, RewardWindow;
    public float GameSpeed = 1f;

    private void Awake()
    {
        _generalgroup= GetComponent<CanvasGroup>(); 
        _armyPanel = GetComponentInChildren<ArmyPanel>();
        _abilityPanel = GetComponentInChildren<AbilityPanel>();
    }
    private void OnEnable()
    {
        EventBus.GameOverEvent += Gameover;
    }

    void Start()
    {
        EndLevelWindow.gameObject.SetActive(false);
        PauseWindow.gameObject.SetActive(false);
        Time.timeScale = GameSpeed;
        _generalgroup.alpha = 0f;
    }

    public void Init(Player player, AIPlayer enemyPlayer)
    {
        PlayerInputController.Instance.PausePressed += Pause;
        _allyPlayer = player;
        _enemyPlayer = enemyPlayer;
        allyBar.Init(_allyPlayer.PlayerColor, _allyPlayer);
        enemyBar.Init(_enemyPlayer.PlayerColor, _enemyPlayer);
        if (_allyPlayer is HumanPlayer)
        {
            GetComponentInChildren<ResoursePanel>().Init(_allyPlayer as HumanPlayer);
            GetComponentInChildren<InfoBar>().Init(_allyPlayer.Hero);
            _armyPanel.InitBar(_allyPlayer.Hero, _allyPlayer as HumanPlayer);
            _abilityPanel.InitBar(_allyPlayer.Hero, _allyPlayer as HumanPlayer);
            
        }
        _generalgroup.DOFade(1f, 0.5f).SetUpdate(true).SetEase(Ease.InQuart);
    }


    private void Gameover(BattlefieldHero hero)
    {
        // Постепенное замедление времени до 0
        DOTween.To(x => Time.timeScale = x, 1f, 0f, 2f).SetUpdate(true).OnComplete(() =>
        {
            EndLevelWindow.gameObject.SetActive(true);
            _endGameStarButton.Select();
            Viewport.gameObject.transform.DOScale(0, 0.3f).SetEase(Ease.InSine).From().SetUpdate(true);
        });
        // Если проиграл
        if (hero.Hero == _allyPlayer.Hero)
        {
            EndLevelMessage.text = LocalizationManager.Localize(LOOSE);
            NextButtontext.text = LocalizationManager.Localize(AGAIN);
        }
        //Если выиграл
        else
        {
            _exitButton.gameObject.SetActive(false);
            EndLevelButton.IsWin = true;
            NextButtontext.text = LocalizationManager.Localize(NEXT);
            //Если уровень не последний
            if (SceneManager.GetActiveScene().buildIndex != SceneManager.sceneCountInBuildSettings - 2)
            {
                EndLevelMessage.text = LocalizationManager.Localize(WIN);

            }
            //Если уровень последний
            else
            {
                EndLevelMessage.text = LocalizationManager.Localize(FINALWIN);
            }
        }
    }
    private void OnDisable()
    {
        EventBus.GameOverEvent -= Gameover;
    }
    public void Pause()
    {
        if (RewardWindow.gameObject.activeInHierarchy) return;
        if (EndLevelWindow.gameObject.activeInHierarchy) return;
        if (PauseWindow.gameObject.activeInHierarchy)
        {
            _settingsWindow.localScale = Vector3.one;
            PauseWindow.gameObject.SetActive(false);
            Time.timeScale = GameSpeed;
            allyBar.gameObject.SetActive(true);
            enemyBar.gameObject.SetActive(true);
        }
        else
        {
            PauseWindow.gameObject.SetActive(true);
            _resumeButton.Select();
            _settingsWindow.DOScale(0f, 0.15f).SetUpdate(true).From();
            Time.timeScale = 0;
            _allyArmy.Init(_allyPlayer.Hero);
            _enemyArmy.Init(_enemyPlayer.Hero);
            allyBar.gameObject.SetActive(false);
            enemyBar.gameObject.SetActive(false);
        }
    }
}
