using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class DefaultBot : ActorController
{
    [SerializeField] private UnityEvent _onDead; 
    [Space]
    [SerializeField] private RandomTimer _timeBetwiinMoves;
    [SerializeField] private int _maxMoveTries = 4;
    [Space]
    [SerializeField] private RandomTimer _timeBetweenActions;
    

    protected override IEnumerator StartPlayGame()
    {
        Debug.Log($"{this}>>>I started to play");
        yield return new WaitForSeconds(_timeBetwiinMoves.GetRandomTime()/2);
        var moves = _maxMoveTries;
        while(moves > 0 && _gameBoard.HasEmptySpace())
        {
            if (_TryTakeRandomCard())
            {
                Debug.Log($"{this}>>>card taken");
                yield return new WaitForSeconds(_timeBetweenActions.GetRandomTime());

                var task = _gameActor.TryPlaceCardAtGameBoard();
                yield return new WaitUntil(() => task.IsCompleted);
                if (!task.Result)
                {
                    _gameActor.ReturnCardToHolder();
                }
            }
            else
            {
                Debug.Log($"{this}>>>couldn't take card");
            }
            yield return new WaitForSeconds(_timeBetwiinMoves.GetRandomTime());
            moves--;
        }
        Debug.Log($"{this}>>>I ended my turn");
        _gameBoard.NextTurn();
    }

    protected override void _OnDead(GameActor gameActor)
    {
        _onDead.Invoke();
    }

    private bool _TryTakeRandomCard()
    {
        var newCardIndex = Random.Range(0, _gameActor.MaxCards);
        return _gameActor.TryTakeCard(newCardIndex);
    }

}

[System.Serializable]
public class RandomTimer
{
    [SerializeField] private float _minTime = 1f;
    [SerializeField] private float _maxTime = 2f;

    public float GetRandomTime()
    {
        return Random.Range(_minTime, _maxTime);
    }
}