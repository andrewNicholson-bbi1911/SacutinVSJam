using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActorController : ActorController
{
    [SerializeField] private GameObject _lostGameCanvas;

    protected override IEnumerator StartPlayGame()
    {
        yield return null;
    }

    protected override void _OnDead(GameActor gameActor)
    {
        if (_lostGameCanvas != null)
        {
            _lostGameCanvas.SetActive(true);
        }
    }
}
