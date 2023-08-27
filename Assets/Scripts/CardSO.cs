using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO/Cards SO", menuName ="Create new Basic Card")]
public class CardSO : ScriptableObject
{
    public string CardName { get => _cardName; }
    public string CardDescription { get => _cardDescription; }
    public Sprite CardSprite { get => _cardSprite; }
    public Sprite CardBackSprite { get => _cardSprite; }
    public TargetType TargetType { get => _targetType; }

    [SerializeField] private string _cardName;
    [SerializeField] [TextArea(minLines: 2, maxLines: 6)] private string _cardDescription;
    [SerializeField] private Sprite _cardSprite;
    [SerializeField] private Sprite _cardBackSprite;
    [SerializeField] private List<EffectSO> _effects = new List<EffectSO>();

    [SerializeField] TargetType _targetType;

    public virtual void ActivateCard(GameActor holder, List<GameActor> _targets)
    {
        foreach(var effect in _effects)
        {
            effect.Activate(holder, _targets);
        }
    }
}

public enum TargetType
{
    oneEnemy,
    allEnemies,
    self,
    random
}
