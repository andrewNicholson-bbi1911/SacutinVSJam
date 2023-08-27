using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour
{
    [SerializeField] private Camera _activeCamera;

    [SerializeField] private CardSpot _activeSpot;

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
        _PlaceOnActiveSpot();
        Debug.Log("dropped");
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("spoted");
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
