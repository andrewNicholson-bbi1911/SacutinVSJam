using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CardHolder : MonoBehaviour
{
    public UnityAction onActivePoolUpdated;

    [SerializeField] private List<CardSO> _allCards = new List<CardSO>();
    [SerializeField] private GameObject _cardReference;
    [SerializeField] private int _maxActiveCards = 6;

    [Space]
    [Header("Для проверки")]

    [SerializeField] private GameActor _owner = null;
    [SerializeField] private List<Card> _activeCardPool = new List<Card>();


    public void LoadOwner(GameActor owner)
    {
        if(_owner == null)
        {
            _owner = owner;
            _owner.onActorTurnEnd += _UpdateActiveCardsPool;
            _UpdateActiveCardsPool();
        }
    }


    public Card TakeCard(int cardIndex)
    {
        if(cardIndex >= _activeCardPool.Count || cardIndex < 0)
        {
            return null;
        }
        var card = _TakeCard(cardIndex);
        card.OnReturnCard += _ReturnCard;
        onActivePoolUpdated?.Invoke();
        return card;
    }


    //　можно менять
    private Card _TakeCard(int index)
    {
        var takenCard = _activeCardPool[index];
        _activeCardPool.RemoveAt(index);
        return takenCard;
    }


    public int GetCardIndex(Card card)
    {
        if (_activeCardPool.Contains(card))
        {
            return _activeCardPool.IndexOf(card);
        }
        Debug.LogWarning($"{this}>>>doesn't contain {card}, but contains other {_activeCardPool.Count} cards");
        return -1 ;
    }


    private void _ReturnCard(Card card)
    {
        if(_allCards.Count >= _maxActiveCards)
        {
            return;
        }
        else
        {
            _activeCardPool.Add(card);
            card.OnReturnCard -= _ReturnCard;
            onActivePoolUpdated?.Invoke();
        }
    }


    private void _UpdateActiveCardsPool()
    {
        int needCards = _maxActiveCards - _activeCardPool.Count;

        while (needCards > 0)
        {
            var newCard = Card.SpawnNewCard(_cardReference, GetRandomCardData(), _owner);
            PlaceCardAtEmptySpace(newCard);
            _activeCardPool.Add(newCard);
            needCards--;
        }
        onActivePoolUpdated?.Invoke();
    }

    //можно менять
    private CardSO GetRandomCardData()
    {
        var totalCards = _allCards.Count;

        return _allCards[Random.Range(0, totalCards)];
    }

    private void PlaceCardAtEmptySpace(Card card)
    {

    }
}
