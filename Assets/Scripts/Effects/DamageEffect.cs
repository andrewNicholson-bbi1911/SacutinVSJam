using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO/Effects/DamageEffects", menuName = "new Effect/Add new Damage Effect")]
public class DamageEffect : EffectSO
{
    public int DamageAmount { get => _damageAmount; }

    [SerializeField] private int _damageAmount = 1;

    public override void Activate(GameActor holder, List<GameActor> targets)
    {
        foreach(var target in targets)
        {
            HPContainer hpContainer;
            if(target.TryGetComponent(out hpContainer))
            {
                hpContainer.AddDamage(this);
            }
        }
    }
}
