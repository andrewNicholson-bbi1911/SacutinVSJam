using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CardHolder), typeof(HPContainer))]
public class GameActor : MonoBehaviour
{
    public int MaxCards { get => _cardHolder.MaxActiveCards; }


    public UnityAction onActorTurnStart;
    public UnityAction onActorTurnEnd;
    public UnityAction<GameActor> onActorDead;

    [SerializeField] private List<GameActor> _enemies = new List<GameActor>();
    [SerializeField] private UnityEvent _onActorsTurnStart;
    [SerializeField] private UnityEvent _onActorsTurnEnd;

    private CardHolder _cardHolder = null;
    private HPContainer _hpContainer = null;
    private GameBoard _gameBoard = null;

    [SerializeField] private Card _holdingCard = null;


    public void Init(GameBoard gameBoard, CardHolder cardHolder = null)
    {
        _gameBoard = gameBoard;
        if(cardHolder != null)
        {
            _cardHolder = cardHolder;
        }
        else
        {
            _cardHolder = GetComponent<CardHolder>();
        }
        _cardHolder.LoadOwner(this);
        _hpContainer = GetComponent<HPContainer>();
        _hpContainer.onDead += _Die;
        EndTurn();
    }


    public bool TryTakeCard(Card card)
    {
        var indexOfCard = _cardHolder.GetCardIndex(card);
        Debug.Log($"{this}>>>taking card of index {indexOfCard}");
        if(indexOfCard >= 0)
        {
            return TryTakeCard(indexOfCard);
        }
        return false;
    }


    public bool TryTakeCard(int index)
    {
        if (_holdingCard != null)
            return false;

        var card =  _cardHolder.TakeCard(index);
        _holdingCard = card;
        return true;
    }


    public async Task<bool> TryPlaceCardAtGameBoard()
    {
        if (_holdingCard == null)
        {
            return false;
        }

        var res = await _gameBoard.TryPlaceCard(_holdingCard);
        if (res)
        {
            _holdingCard = null;
            return true;
        }
        return false;
    }


    /// 
    ///
    /// ВОТ ТУТ ТОЧНО НАДО ОБНОВИТЬ
    /// 
    /// 
    public async Task<List<GameActor>> GetTargets()
    {
        if(_holdingCard == null)
        {
            return null;
        }

        List<GameActor> _targets = new List<GameActor>();

        switch (_holdingCard.TargetType)
        {
            case TargetType.oneEnemy:
                if(_enemies.Count == 1)
                {
                    _targets.Add(_enemies[0]);
                }
                else
                {
                    Debug.LogError("Can't choose one enemy between more then one enemies");
                }
                break;
            case TargetType.allEnemies:
                _targets.AddRange(_enemies);
                break;
            case TargetType.self:
                _targets.Add(this);
                break;
            case TargetType.random:
                Debug.LogError("Random Target type is not declared");
                break;
        }
        Debug.Log($"{this}>>>найдено {_targets.Count}");
        return _targets;
    }


    public bool TryTakeCardFromGameBoard(int index)
    {
        if(_holdingCard != null)
        {
            return false;
        }

        var card = _gameBoard.TakeCard(index);

        if (card == null)
            return false;

        _holdingCard = card;
        return true;
    }

    public void ReturnCardToHolder()
    {
        if (_holdingCard == null)
            return;

        else
        {
            _holdingCard.ReturnCard();
            _holdingCard = null;
        }
    }

    public void EndTurn()
    {
        onActorTurnEnd?.Invoke();
        _onActorsTurnEnd.Invoke();
        Debug.Log($"{this}>>>finished Turn");
    }

    public void StartTurn()
    {
        onActorTurnStart?.Invoke();
        _onActorsTurnStart.Invoke();
        Debug.Log($"{this}>>>start Turn");
    }


    private void _Die()
    {
        onActorDead?.Invoke(this);
        EndTurn();
    }
}
