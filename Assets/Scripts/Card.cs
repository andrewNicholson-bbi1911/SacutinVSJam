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
    

    public void LoadData(CardSO cardSO, GameActor owner)
    {
        _cardSO = cardSO;
        _cardOwner = owner;
        _onDataLoaded.Invoke(_cardSO);
    }


    public async void QuickPlaceToGameBoard()
    {
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
            Destroy(gameObject);
        }
    }


    public static Card SpawnNewCard(GameObject cardRefernce, CardSO cardData, GameActor holder)
    {
        var newCard = Instantiate(cardRefernce);
        Card card;
        if(newCard.TryGetComponent(out card))
        {
            card.LoadData(cardData, holder);
            return card;
        }
        else
        {
            Destroy(newCard);
        }
        return null;
    }

    private void _UpdateUI()
    {
        Debug.LogError($"{this}>>> Here is no update logic");
    }
}
