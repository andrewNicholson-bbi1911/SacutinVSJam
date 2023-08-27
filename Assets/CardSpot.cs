using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CardSpot : MonoBehaviour
{

    public bool IsEmpty { get => _inObjectDnD == null; }
    
    [SerializeField] private UnityEvent _onStartBeingPotential;
    [SerializeField] private UnityEvent _onStopBeingPotential;

    [SerializeField] private DragAndDrop _inObjectDnD = null;

    private static CardSpot _potentialSpot = null;
    private static CardSpot _lastPotentialSpot = null;


    public void PlaceCard(Card card)
    {
        //card
        var dnd = card.GetComponent<DragAndDrop>();
        dnd.Take();
        dnd.transform.position = transform.position;
        _inObjectDnD = dnd;
        _potentialSpot = this;
        dnd.Drop();
        _PlaceObjectAtPotentialCell(dnd);

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_inObjectDnD != null)
        {
            return;
        }

        DragAndDrop dnd;

        if (collision.TryGetComponent(out dnd))
        {
            _UpdatePotentialSpot();
            dnd.onDrop += _PlaceObjectAtPotentialCell;
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_inObjectDnD != null)
        {
            return;
        }

        DragAndDrop dnd;

        if (collision.TryGetComponent(out dnd))
        {
            if(_potentialSpot == this)
            {
                _ClearPotentialSpot();
            }
        }
    }


    private void _PlaceObjectAtPotentialCell(DragAndDrop dndObject)
    {
        dndObject.onDrop -= _PlaceObjectAtPotentialCell;
        dndObject.onTake += _EmptyCell;
        dndObject.onTake += _MarkAsLastPotential;

        if (_potentialSpot == null)
        {
            _potentialSpot = _lastPotentialSpot;
        }

        if (_potentialSpot != null)
        {
            _potentialSpot._inObjectDnD = dndObject;
            dndObject.transform.parent = _potentialSpot.transform;
            dndObject.transform.localPosition = Vector2.zero;
            dndObject.transform.localRotation = _potentialSpot.transform.localRotation;
            dndObject.transform.localScale = Vector3.one;
            _lastPotentialSpot = null;
            _ClearPotentialSpot();
        }
    }


    private void _EmptyCell()
    {
        if (_inObjectDnD != null)
        {
            _inObjectDnD.onTake -= _EmptyCell;
            _inObjectDnD = null;
        }
    }


    private void _MarkAsLastPotential()
    {
        _lastPotentialSpot = this;
        if(_inObjectDnD!= null)
        {
            _inObjectDnD.onTake -= _MarkAsLastPotential;
            _inObjectDnD.onDrop += _PlaceObjectAtPotentialCell;
        }
    }


    private void _UpdatePotentialSpot()
    {
        _ClearPotentialSpot();
        _MarkAsPotential();
    }


    private void _ClearPotentialSpot()
    {
        if (_potentialSpot != null)
        {
            _potentialSpot._onStopBeingPotential.Invoke();
            _potentialSpot = null;
        }
    }


    private void _MarkAsPotential()
    {
        _potentialSpot = this;
        _onStartBeingPotential.Invoke();
    }

}
