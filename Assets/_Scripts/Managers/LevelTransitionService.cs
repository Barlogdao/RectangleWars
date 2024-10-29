using UnityEngine;
using NaughtyAttributes;
using UnityEngine.SceneManagement;
using System;

public class LevelTransitionService : MonoBehaviour
{
    [Scene]
    [SerializeField]
    private int mainMenu, armyManager, firstLevel, bootScene, ending, _randomBattleScene;
    public event Action<int> OnSavedSceneChanged;
    public event Action GameSceneLoadedEvent;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        EventBus.NewGameEvent += NewGame;
        EventBus.ContinueGameEvent += ContinueGame;
        EventBus.BackToMainMenuEvent += BackToMainMenu;
        EventBus.GoToArmyManagerEvent += GoToArmyManager;
        EventBus.NextLevelEvent += NextLevel;
        EventBus.RestartLevelEvent += RestartLevel;
        EventBus.QuitGameEvent += QuitGame;
        EventBus.RandomGameStart += RandomGame;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        EventBus.NewGameEvent -= NewGame;
        EventBus.ContinueGameEvent -= ContinueGame;
        EventBus.BackToMainMenuEvent -= BackToMainMenu;
        EventBus.GoToArmyManagerEvent -= GoToArmyManager;
        EventBus.NextLevelEvent -= NextLevel;
        EventBus.RestartLevelEvent -= RestartLevel;
        EventBus.QuitGameEvent -= QuitGame;
        EventBus.RandomGameStart -= RandomGame;
    }

    private void RandomGame()
    {

        GameManager.Instance.EnemyHero = GameManager.Instance.CreateEnemyHero();
        SceneManager.LoadScene(_randomBattleScene);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == ending)
            OnSavedSceneChanged?.Invoke(mainMenu);
        else if (IsGameLevelScene(scene.buildIndex) || scene.buildIndex == _randomBattleScene)
        {
            OnSavedSceneChanged?.Invoke(scene.buildIndex);
            GameSceneLoadedEvent?.Invoke();
        }
        Time.timeScale = 1;
    }
    private bool IsGameLevelScene(int index)
    {
        return index != mainMenu && index != armyManager && index != bootScene && index != ending && index != _randomBattleScene;
    }

    public void NewGame()
    {
        GameManager.Instance.EnemyHero = GameManager.Instance.CreateEnemyHero();
        OnSavedSceneChanged?.Invoke(firstLevel);
        SceneManager.LoadScene(bootScene);
    }
    public void GoToArmyManager()
    {
        SceneManager.LoadScene(armyManager);
    }

    public void ContinueGame() => GoToArmyManager();
    public void NextLevel()
    {
        SceneManager.LoadScene(bootScene);
    }
    public void RestartLevel() => GoToArmyManager();
    public void BackToMainMenu() => SceneManager.LoadScene(mainMenu);
    public void QuitGame() =>Application.Quit();
}
