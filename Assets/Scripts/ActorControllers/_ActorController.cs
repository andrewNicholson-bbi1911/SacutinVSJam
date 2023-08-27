using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class ActorController : MonoBehaviour, IActorController
{
    [SerializeField] protected GameActor _gameActor = null;
    [SerializeField] protected GameBoard _gameBoard;
    [SerializeField] private UnityEvent _onTurnStart;
    [SerializeField] private UnityEvent _onTurnEnd;


    private void Start()
    {
        _gameActor.onActorTurnStart += _OnTurnStart;
        _gameActor.onActorTurnEnd += _OnTurnEnd;
        _gameActor.onActorDead += _OnDead;
    }

    private void _OnTurnStart()
    {
        _onTurnStart.Invoke();
        StartCoroutine(StartPlayGame());
    }

    private void _OnTurnEnd()
    {
        _onTurnEnd.Invoke();
        StopAllCoroutines();
    }

    protected abstract void _OnDead(GameActor gameActor);

    protected abstract IEnumerator StartPlayGame();

    public virtual void AskForTarget()
    {

    }
}
