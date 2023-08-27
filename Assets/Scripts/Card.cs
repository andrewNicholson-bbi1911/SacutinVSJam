using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Card : MonoBehaviour
{
    public TargetType TargetType { get => _cardSO.TargetType; }

    public UnityAction<Card> OnReturnCard;

    [SerializeField] private UnityEvent<CardSO> _onDataLoaded;
    [SerializeField] private bool _destroyOnActivate = true;
    [Space]
    [Header("Просто прочекать, что всё ок")]
    [SerializeField] private GameActor _cardOwner;
    [SerializeField] private CardSO _cardSO;
    private List<GameActor> _targets = new List<GameActor>();


    public static Card SpawnNewCard(GameObject cardRefernce, CardSO cardData, GameActor holder)
    {
        var newCard = Instantiate(cardRefernce);
        Card card;
        if (newCard.TryGetComponent(out card))
        {
            card.LoadData(cardData, holder);
            card.enabled = false;
            return card;
        }
        else
        {
            Destroy(newCard);
        }
        return null;
    }


    public void LoadData(CardSO cardSO, GameActor owner)
    {
        
        _cardSO = cardSO;
        _cardOwner = owner;
        _cardOwner.onActorTurnStart += _OpenCard;
        _cardOwner.onActorTurnEnd += _CloseCard;

        _onDataLoaded.Invoke(_cardSO);
    }


    public async void QuickPlaceToGameBoard()
    {
        if (!enabled)
            return;

         Debug.LogError($"{this}>>> Тут расположен примерный алгоритм того, как надо выкладывать карту на стол." +
            $"\nНЕ ИСПОЛЬЗОВАТЬ В РЕАЛЬНОЙ ИГРЕ");
        _cardOwner.ReturnCardToHolder();
        if (_cardOwner.TryTakeCard(this))
        {
            await _cardOwner.TryPlaceCardAtGameBoard();
        }
        else
        {
            Debug.LogWarning($"{this}>>> Что-то пошло не так");
        }
        
    }


    public void SetTargets(List<GameActor> targets)
    {
        _targets = targets;
    }


    public void ReturnCard()
    {
        OnReturnCard?.Invoke(this);
    }


    public void ActivateCard()
    {
        _cardSO.ActivateCard(_cardOwner, _targets);
        if (_destroyOnActivate)
        {
            Destroy(gameObject, 1f);
        }
    }


    private void OnDestroy()
    {
        _cardOwner.onActorTurnEnd -= _CloseCard;
        _cardOwner.onActorTurnStart -= _OpenCard;
    }


    private void _CloseCard()
    {
        enabled = false;
    }


    private void _OpenCard()
    {
        enabled = true;
    }
}
