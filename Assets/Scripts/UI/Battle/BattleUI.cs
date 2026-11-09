using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour, IBattlePresenter
{
    [SerializeField] private BattleContext context;
    [SerializeField] private BattleLauncher launcher;
    [SerializeField] private TMP_Text turnCount;
    [SerializeField] private TMP_Text actorName;
    [SerializeField] private RectTransform logBox;
    [SerializeField] private ScrollRect logScrollbar;
    [SerializeField] private TMP_Text logPrefab;

    public bool IsPlaying { get; private set; }

    private readonly List<TMP_Text> logs = new();

    private void Start()
    {
        context.TurnChanged += OnTurnChanged;
        context.SetPresenter(this);
    }

    private void OnTurnChanged(Turn newTurn)
    {
        turnCount.text = $"Turn: {newTurn.Index + 1}";
        actorName.text = newTurn.Actor.DisplayName;
    }

    private void Log(string message)
    {
        TMP_Text newLog = Instantiate(logPrefab, logBox);
        newLog.text = message;

        logs.Add(newLog);
        StartCoroutine(ScrollToBottom());
    }

    private IEnumerator ScrollToBottom()
    {
        yield return null;

        Canvas.ForceUpdateCanvases();
        logScrollbar.StopMovement();
        logScrollbar.verticalNormalizedPosition = 0.0f;
    }

    public void Play(Battle battle, List<BattleEvent> events)
    {
        IsPlaying = true;
        StartCoroutine(PlayEvents(battle, events));
    }

    private IEnumerator PlayEvents(Battle battle, List<BattleEvent> events)
    {
        foreach (BattleEvent battleEvent in events)
        {
            switch (battleEvent)
            {
                case AbilityUsedEvent abilityUsedEvent:
                    Log(abilityUsedEvent.Source != abilityUsedEvent.Target
                        ? $"{abilityUsedEvent.Source.DisplayName} used {abilityUsedEvent.Action.Name} on  {abilityUsedEvent.Target.DisplayName}"
                        : $"{abilityUsedEvent.Source.DisplayName} used {abilityUsedEvent.Action.Name} on self");
                    break;
                case DamageEvent damageEvent:
                    Log(
                        $"{damageEvent.Source.DisplayName} has dealt {damageEvent.Damage} {damageEvent.Type} damage to {damageEvent.Target.DisplayName}: HP {damageEvent.HealthBeforeDamage} -> {damageEvent.HealthAfterDamage}");
                    break;
                case BattleEndedEvent battleEndedEvent:
                    switch (battleEndedEvent.Result.Outcome)
                    {
                        case BattleOutcome.NotFinished: break;
                        case BattleOutcome.Victory: Log($"Player has won!"); break;
                        case BattleOutcome.Defeat: Log($"Player has been defeated..."); break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    break;
                case ManaEvent manaEvent:
                    if (manaEvent.ManaDelta == 0) break;
                    Log(
                        manaEvent.ManaDelta < 0
                            ? $"{manaEvent.Target.DisplayName} spent {Mathf.Abs(manaEvent.ManaDelta)} mana: {manaEvent.ManaBefore} -> {manaEvent.ManaAfter}"
                            : $"{manaEvent.Target.DisplayName} recovered {manaEvent.ManaDelta} mana: {manaEvent.ManaBefore} -> {manaEvent.ManaAfter}");
                    break;
                case StatusEffectAddedEvent inflictStatusEffectEvent:
                    Log(inflictStatusEffectEvent.Source != inflictStatusEffectEvent.Target
                        ? $"{inflictStatusEffectEvent.Source.DisplayName} inflicted {inflictStatusEffectEvent.StatusEffect.DisplayName} to {inflictStatusEffectEvent.Target.DisplayName}"
                        : $"{inflictStatusEffectEvent.Source.DisplayName} applied {inflictStatusEffectEvent.StatusEffect.DisplayName} on self");
                    break;
                case StatusEffectRemovedEvent statusEffectRemovedEvent:
                    Log(
                        $"{statusEffectRemovedEvent.Source.DisplayName}'s {statusEffectRemovedEvent.StatusEffect.DisplayName} has been removed");
                    break;
                case ActionEndEvent: yield return new WaitForSeconds(2.0f); break;
            }
        }

        IsPlaying = false;
    }

    private void OnDestroy() => context.TurnChanged -= OnTurnChanged;
}