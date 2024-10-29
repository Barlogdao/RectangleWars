using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Assets.SimpleLocalization;
using System;

public class GameManager : Singleton<GameManager>
{

    public PlayerSaveSO saveSO;
    public int SavedSceneIndex;
    public GameSettings Settings;
    [ColorUsage(true, true)]
    public Color CurrentPlayerColor;
    public Complexity GameComplexity;
    public GamePlayersCondition Condition;
    private Hero _hero;
    [SerializeField] private HeroInventorySO _inventorySO;
    public Hero Hero { get => _hero; private set => _hero = value; }
    public Hero EnemyHero { get; set; }
    [SerializeField]
    BattlefieldManager battlefieldManager;
    LevelTransitionService levelTransitionService;

    protected override void OnAwake()
    {
        DontDestroyOnLoad(this.gameObject);
        levelTransitionService = GetComponent<LevelTransitionService>();
        saveSO = Resources.Load<PlayerSaveSO>("PlayerSave");
        LanguageInit();
    }
    private void OnEnable()
    {
        saveSO.LoadDataFromFile();
        SavedSceneIndex = saveSO.SceneIndex;
        CurrentPlayerColor = saveSO.PlayerColor;
        GameComplexity = saveSO.GameCompexity;
        Hero = saveSO.hero;
        EnemyHero = saveSO.EnemyHero;
        Condition = saveSO.Condition;

        levelTransitionService.OnSavedSceneChanged += ChangeSavedScene;
        levelTransitionService.GameSceneLoadedEvent += OnGameSceneLoaded;

        SceneManager.sceneUnloaded += SceneManagerOnSceneUnloaded;
    }

    private void OnGameSceneLoaded()
    {
        var bm = Instantiate(battlefieldManager);
        bm.Init(Condition);

    }

    private void ChangeSavedScene(int sceneindex)
    {
        SavedSceneIndex = sceneindex;
    }

    private static void SceneManagerOnSceneUnloaded(Scene scene)
    {
        DOTween.KillAll();
    }



    private void OnApplicationQuit()
    {
        saveSO.PlayerColor = CurrentPlayerColor;
        saveSO.GameCompexity = GameComplexity;
        saveSO.SceneIndex = SavedSceneIndex;
        saveSO.hero = Hero;
        saveSO.EnemyHero = EnemyHero;
        saveSO.Condition = Condition;
        saveSO.SaveToFile();
        DOTween.KillAll();
    }

    private void LanguageInit()
    {
        LocalizationManager.Read();

        LocalizationManager.Language = Application.systemLanguage switch
        {
            SystemLanguage.Russian => "Russian",
            _ => "English",
        };
    }

    public Hero CreateEnemyHero()
    {
        var heroAI = new Hero(GameLibrary.Instance.HeroList[UnityEngine.Random.Range(0, GameLibrary.Instance.HeroList.Count)].hero);
        heroAI.level = Hero.level < 2 ? 1 : Hero.level < 3 ? 2 : Hero.level + (int)GameComplexity;
        heroAI.CheckAILevel();
        return heroAI;
    }
    public void SetNewHero(Hero hero, bool includeCheatSpells = false)
    {
        Hero = new Hero(hero);
        if (includeCheatSpells == true)
        {
            Hero.Inventory.Spells = new(_inventorySO.GetInventorySpells());
        }

    }
    protected override void OnDisableHandler()
    {
        SceneManager.sceneUnloaded -= SceneManagerOnSceneUnloaded;
        levelTransitionService.OnSavedSceneChanged -= ChangeSavedScene;
        levelTransitionService.GameSceneLoadedEvent -= OnGameSceneLoaded;
    }
}





