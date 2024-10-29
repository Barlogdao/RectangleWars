using TMPro;
using UnityEngine;
using DG.Tweening;
using System.Collections;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;

public class Tutorial : MonoBehaviour
{

    private CanvasGroup _canvasGroup;
    [SerializeField] TextMeshProUGUI _unitSelecttext;
    [SerializeField] TextMeshProUGUI _spellSelectText;
    [SerializeField] RectTransform _unitSchemeText;
    [SerializeField] float _unitSchemeTitorialDuration;
    [SerializeField] CanvasGroup _workerSymbols;
    private Player _targetPlayer;
    private bool _unitSpawned = false;
    private bool _spellSpawned = false;
    CancellationToken cts;


    private void Awake()
    {
        cts = this.GetCancellationTokenOnDestroy();
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0f;
        _unitSelecttext.enabled = false;
        _spellSelectText.enabled = false;
        _unitSchemeText.gameObject.SetActive(false);
        _workerSymbols.gameObject.SetActive(false);
        _workerSymbols.alpha = 0f;
    }

    private void OnGameStarted()
    {
        _unitSelecttext.enabled = true;
        _canvasGroup.DOFade(1f, 1f);
        _targetPlayer = FindObjectOfType<BattlefieldManager>().HumanPlayer;
        UnitBase.UnitIsSpawned += UnitSpawned;
        ShowWorkerSymbols(cts).Forget();
        SequenseTutorialOrder(cts).Forget();
    }

    private async UniTask SequenseTutorialOrder(CancellationToken token)
    {

        // Подсказка спавна юнитов
        await UniTask.WhenAny(
            UniTask.WaitUntil(() => _unitSpawned == true, cancellationToken: token),
            UniTask.Delay(TimeSpan.FromSeconds(15f), cancellationToken: token)
            );
        UnitBase.UnitIsSpawned -= UnitSpawned;
        await _canvasGroup.DOFade(0f, 1f).WithCancellation(token);
        _unitSelecttext.enabled = false;

        // Подсказка спеллкаста
        _spellSelectText.enabled = true;
        await _canvasGroup.DOFade(1f, 1f).WithCancellation(token);
        _targetPlayer.SpellCasted += SpellSpawned;
        await UniTask.WhenAny(
            UniTask.WaitUntil(() => _spellSpawned == true, cancellationToken: token),
            UniTask.Delay(TimeSpan.FromSeconds(15f), cancellationToken: token)
            );
        _targetPlayer.SpellCasted -= SpellSpawned;

        // Подсказка контры

        await _canvasGroup.DOFade(0f, 1f).OnComplete(() => _spellSelectText.enabled = false).WithCancellation(token);
        await ShowUnitScheme(token);

    }

    private async UniTask ShowWorkerSymbols(CancellationToken token)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(2f), cancellationToken: token);
        _workerSymbols.gameObject.SetActive(true);
        await _workerSymbols.DOFade(1f, 1f).WithCancellation(token);
        await UniTask.Delay(TimeSpan.FromSeconds(10f), cancellationToken: token);
        await _workerSymbols.DOFade(0f, 1f).WithCancellation(token);
        _workerSymbols.gameObject.SetActive(false);
    }


    private async UniTask ShowUnitScheme(CancellationToken token)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(2f),cancellationToken: token);
        _unitSchemeText.gameObject.SetActive(true);
        await _canvasGroup.DOFade(1f, 1f).WithCancellation(token);
        await UniTask.Delay(TimeSpan.FromSeconds(_unitSchemeTitorialDuration), cancellationToken: token);
        await _canvasGroup.DOFade(0f, 1f).WithCancellation(token);
        _unitSchemeText.gameObject.SetActive(false);
    }
    private void SpellSpawned()
    {
        _spellSpawned = true;
    }

    private void UnitSpawned(UnitBase unit)
    {
        if (unit.Owner == _targetPlayer && unit.Class != ClassType.Worker)
        {
            _unitSpawned = true;
        }
    }

    private void OnEnable()
    {
        BattlefieldManager.GameStarted += OnGameStarted;
    }
    private void OnDisable()
    {
        BattlefieldManager.GameStarted -= OnGameStarted;
    }
    private void OnDestroy()
    {
        UnitBase.UnitIsSpawned -= UnitSpawned;
    }
}
