using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour
{
    [SerializeField] private Camera _activeCamera;

    [SerializeField] private CardSpot _activeSpot = null;


   
    
    private void Awake()
    {
        _PlaceOnActiveSpot();
        _activeCamera = FindObjectOfType<Camera>();
    }

    public void Take()
    {
        Debug.Log("taken");
    }

    public void Drag()
    {
        var position = Input.mousePosition;
        var worldPosition = _activeCamera.ScreenToWorldPoint(position);
        transform.position = (Vector2)worldPosition;
    }

    public void Drop()
    {

        if (_activeSpot != null && !_activeSpot.IsOccupied())
        {
            _PlaceOnActiveSpot();
            _activeSpot.SetOccupied(true); // Устанавливаем флаг занятости спота
            Debug.Log("Карта помещена на спот!");
        }
        else
        {
            Debug.Log("Карта не может быть размещена на споте.");
        }
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        CardSpot newSpot;
        if (collision.TryGetComponent(out newSpot))
        {
            _activeSpot = newSpot;
        }
    }


    private void _PlaceOnActiveSpot()
    {
        if(_activeSpot != null)
        {
            transform.position = _activeSpot.transform.position;

        }
    }
 }
