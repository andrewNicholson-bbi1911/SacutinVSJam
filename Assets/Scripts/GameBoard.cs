using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class GameBoard : MonoBehaviour
{
    public UnityAction onActiveCardsUpdated;

    [SerializeField] private List<GameActor> _actors = new List<GameActor>();
    [SerializeField] private int _firstActorTurn = 1;
    [SerializeField] private GameActor _activeActor;
    [Space]
    [SerializeField] private int _maxActiveCards = 3;
    [SerializeField] private List<Card> _activeCards;
    [Space]
    [SerializeField] private float _turnBetweenTimer = 2f;


    private int _activeActorIndex;


    private void OnEnable()
    {
        foreach(var actor in _actors)
        {
            actor.Init(this);
            actor.onActorTurnEnd += _ActivateActiveCards;
            actor.onActorDead += _OnActorDead;
        }
    }


    private void OnDisable()
    {
        foreach (var actor in _actors)
        {
            actor.Init(this);
            actor.onActorTurnEnd -= _ActivateActiveCards;
            actor.onActorDead -= _OnActorDead;
        }
    }


    private void Start()
    {
        _activeActorIndex = _firstActorTurn;
        _EnableActiveActor();
    }


    public async Task<bool> TryPlaceCard(Card card)
    {
        if (_activeCards.Count >= _maxActiveCards)
        {
            return false;
        }
        _activeCards.Add(card);

        var targets = await _activeActor.GetTargets();

        card.SetTargets(targets);

        onActiveCardsUpdated?.Invoke();
        return true;
    }


    public Card TakeCard(int cardIndex)
    {
        if (cardIndex >= _activeCards.Count || cardIndex <= 0)
            return null;

        var returningCard = _activeCards[cardIndex];
        if (_activeCards.Remove(returningCard))
        {
            return returningCard;
        }

        return null;
    }


    private void _ActivateActiveCards()
    {
        int maxTryies = 10 * _maxActiveCards;
        while(_activeCards.Count > 0)
        {
            var card = _activeCards[_activeCards.Count-1];
            if (_activeCards.Remove(card))
            {
                card.ActivateCard();
            }
            maxTryies--;
            if(maxTryies <= 0)
            {
                Debug.LogError($"{this}>>>что-то пошло не так");
                break;
            }
        }
    }


    public void NextTurn()
    {
        StopAllCoroutines();
        _FinishTurn();
        _activeActorIndex++;
        _activeActorIndex %= _actors.Count;
        StartCoroutine(NextTurnTimer());
    }


    private void _FinishTurn()
    {
        if(_activeActor != null)
        {
            _activeActor.EndTurn();
            _activeActor = null;
        }
    }


    private void _EnableActiveActor()
    {
        _activeActor = _actors[_activeActorIndex];
        _activeActor.StartTurn();
    }


    private void _OnActorDead(GameActor gameActor)
    {
        if (_actors.Remove(gameActor))
        {
            gameActor.enabled = false;

            if(_activeActor == gameActor)
            {
                _FinishTurn();
                _EnableActiveActor();
            }
        }
    }

    private IEnumerator NextTurnTimer()
    {
        yield return new WaitForSeconds(_turnBetweenTimer);
        _EnableActiveActor();
    }
}
