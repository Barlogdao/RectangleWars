using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattlefieldManager : MonoBehaviour
{
    [SerializeField] BattlefieldHero _heroPrefab;
    public AIPlayer AiPlayer { get; private set; }
    public Player HumanPlayer { get; private set; }
    [SerializeField]
    private GameObject _allyPlayer, _enemyPlayer;
    public MapManager MManager;
    [SerializeField]
    private UICanvas _UIPrefab;
    private UICanvas _gameCanvas;
    public int MinesCount { get; private set; }
    public LayerMask EnemyAIMask, AllyAIMask;
    [SerializeField]
    ParticleSystem _heroAppearanceParticle;
    [SerializeField]
    CinemachineVirtualCamera _generalCamera, _personCamera;
    public static event Action GameStarted;


    public BattleSettings BattleSettings { get; private set; }
    private void Awake()
    {
        MManager = FindObjectOfType<MapManager>();
        MinesCount = FindObjectsOfType<ResMines>().Length;
    }
    public void Init(GamePlayersCondition condition)
    {

        RandomMapBuilder randomMapBuilder = FindObjectOfType<RandomMapBuilder>();
        if (randomMapBuilder != null)
        {
            randomMapBuilder.Init();
        }

        BattleSettings = FindObjectOfType<BattleSettings>();

        switch (condition)
        {
            case GamePlayersCondition.humanVsAi:
                HumanPlayer = _allyPlayer.AddComponent<HumanPlayer>();
                break;
            case GamePlayersCondition.AiVsAi:
                HumanPlayer = _allyPlayer.AddComponent<AIPlayer>();
                if (HumanPlayer is AIPlayer player)
                    player.Mask = AllyAIMask;
                break;
        }

        AiPlayer = _enemyPlayer.AddComponent<AIPlayer>();
        AiPlayer.Mask = EnemyAIMask;


        var humanHero = Instantiate(_heroPrefab, BattleSettings.HumanStartPoint.position, Quaternion.identity, HumanPlayer.transform);
        var enemyHero = Instantiate(_heroPrefab, BattleSettings.EnemyStartPoint.position, Quaternion.identity, AiPlayer.transform);



        HumanPlayer.Init(GameManager.Instance.Hero, GameManager.Instance.CurrentPlayerColor, this, humanHero, BattleSettings.HumanArrow);
        AiPlayer.Init(GameManager.Instance.EnemyHero, GameManager.Instance.Settings.EnemyPlayerColor, this, enemyHero, BattleSettings.EnemyArrow);

        BattleSettings.SetSpawnerColor(HumanPlayer.PlayerColor, BattleSettings.HumanSpawnPositions);
        BattleSettings.SetSpawnerColor(AiPlayer.PlayerColor, BattleSettings.EnemySpawnPositions);

        _generalCamera = Instantiate<CinemachineVirtualCamera>(_generalCamera, new Vector3(0f, 0f, -10f), Quaternion.identity);
        _personCamera = Instantiate<CinemachineVirtualCamera>(_personCamera, new Vector3(0f, 0f, -10f), Quaternion.identity);
        _personCamera.Priority = 0;
        _generalCamera.Priority = 10;
        EventBus.GameOverEvent += OnGameover;
        //_gameCanvas = Instantiate(_gameCanvas);
        //_gameCanvas.Init(HumanPlayer, AiPlayer);
        StartCoroutine(StartGame());
    }



    public Transform GetNewHeroPosition(BattlefieldHero hero)
    {
        if (hero == HumanPlayer.BattlefieldHero)
        {
            return BattleSettings.GetNewHeroPosition(hero, BattleSettings.HumanSpawnPositions);
        }
        else if (hero == AiPlayer.BattlefieldHero)
        {
            return BattleSettings.GetNewHeroPosition(hero, BattleSettings.EnemySpawnPositions);
        }
        return null;
    }



    private void OnGameover(BattlefieldHero hero)
    {
        Camera.main.GetComponent<CinemachineBrain>().enabled = true;
        _generalCamera.Priority = 10;
        StartCoroutine(ActivateCamera());

        IEnumerator ActivateCamera()
        {
            yield return null;
            _personCamera.Follow = hero.transform;
            _personCamera.Priority = 11;
        }

    }

    private IEnumerator StartGame()
    {
        yield return Utilis.GetWait(0.7f);
        _personCamera.Priority = 11;
        _personCamera.Follow = HumanPlayer.BattlefieldHero.transform;
        yield return Utilis.GetWait(0.4f);
        Instantiate<ParticleSystem>(_heroAppearanceParticle, HumanPlayer.BattlefieldHero.transform.position, Quaternion.identity);
        yield return Utilis.GetWait(0.15f);
        HumanPlayer.BattlefieldHero.ShowHeroModel();
        yield return Utilis.GetWait(0.7f);
        _personCamera.Follow = AiPlayer.BattlefieldHero.transform;
        yield return Utilis.GetWait(0.4f);
        Instantiate<ParticleSystem>(_heroAppearanceParticle, AiPlayer.BattlefieldHero.transform.position, Quaternion.identity);
        yield return Utilis.GetWait(0.15f);
        AiPlayer.BattlefieldHero.ShowHeroModel();
        yield return Utilis.GetWait(0.7f);
        _personCamera.Priority = 0;
        yield return Utilis.GetWait(0.42f);
        Camera.main.GetComponent<CinemachineBrain>().enabled = false;
        HumanPlayer.StartGame();
        AiPlayer.StartGame();
        _gameCanvas = Instantiate(_UIPrefab);
        _gameCanvas.Init(HumanPlayer, AiPlayer);
        PlayerInputController.Instance.CheatPressed += CheatAction;
        PlayerInputController.Instance.Check();
        GameStarted?.Invoke();

    }

    #region Предоставление сведений о юнитах/замках
    public List<UnitBase> GetAllyUnits(Player player)
    {
        return player.PlayerUnits;
    }
    public List<UnitBase> GetEnemyUnits(Player player)
    {
        var units =  player == HumanPlayer ?
             AiPlayer.PlayerUnits :
             HumanPlayer.PlayerUnits;
        return units.Where(unit => !unit.ImmuneToMagic).ToList();
    }
    public List<UnitBase> GetAllUnits()
    {
        List<UnitBase> units = new();
        units.AddRange(HumanPlayer.PlayerUnits);
        units.AddRange(AiPlayer.PlayerUnits);
        return units;
    }
    //Доступ к героям
    public BattlefieldHero GetEnemyBH(Player player)
    {
        return player == HumanPlayer ? AiPlayer.BattlefieldHero : HumanPlayer.BattlefieldHero;
    }
    public BattlefieldHero GetAllyBH(Player player)
    {
        return player.BattlefieldHero;
    }
    #endregion

    private void CheatAction()
    {
        if (!GetEnemyBH(HumanPlayer).IsAlive) return;
        GetEnemyBH(HumanPlayer).Health = 0;
    }



    public BattlefieldHero FindHeroOnBattlefield(Player player)
    {
        BattlefieldHero hero;
        foreach (var bHero in FindObjectsOfType<BattlefieldHero>())
        {
            if (player.CompareTag(bHero.tag))
            {
                hero = bHero;
                return hero;
            }
        }
        return null;
    }
    private void OnDestroy()
    {
        PlayerInputController.Instance.CheatPressed -= CheatAction;
        EventBus.GameOverEvent -= OnGameover;
    }
}
