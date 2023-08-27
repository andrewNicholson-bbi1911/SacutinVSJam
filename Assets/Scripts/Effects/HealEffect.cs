using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO/Effects/HealEffects", menuName = "new Effect/Add new Heal Effect")]
public class HealEffect : EffectSO
{
    public int HealAmount { get => _healAmount; }

    [SerializeField] private int _healAmount;

    public override void Activate(GameActor holder, List<GameActor> targets)
    {
        foreach(var target in targets)
        {
            HPContainer hpContainer;
            if (target.TryGetComponent(out hpContainer))
            {
                hpContainer.Heal(this);
            }
        }
    }
}
