using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO/Effects/EmptyEffects", menuName = "new Effect/Add new Empty Effect", order = 0)]
public class EmptyEffect : EffectSO
{

    [SerializeField] private string _extraText;
    public override void Activate(GameActor holder, List<GameActor> targets)
    {
        Debug.Log($"пустой эффект {EffectName} активирован {holder} на {targets.Count} целей");
        Debug.Log($"{_extraText}");
    }
}
