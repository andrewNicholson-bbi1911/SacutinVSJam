using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EffectSO : ScriptableObject
{
    public string EffectName { get => _effectName; }
    public string EffectDescription { get => _effectDescription; }

    [SerializeField] private string _effectName;
    [SerializeField][TextArea(minLines: 2, maxLines:6)] private string _effectDescription;

    public abstract void Activate(GameActor holder, List<GameActor> targets);
}
