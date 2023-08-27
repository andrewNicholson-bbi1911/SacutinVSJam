using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DragAndDrop : MonoBehaviour
{
    [SerializeField] private Camera _activeCamera;
    public UnityAction onTake;
    public UnityAction<DragAndDrop> onDrop;


    private void Awake()
    {
        _activeCamera = FindObjectOfType<Camera>();
    }


    public void Take()
    {
        onTake?.Invoke();
        _AddRB();
        Debug.Log("taken");
    }


    public virtual void Drag()
    {
        var position = Input.mousePosition;
        var worldPosition = _activeCamera.ScreenToWorldPoint(position);
        transform.position = (Vector2)worldPosition;
    }


    public void Drop()
    {
        Destroy(gameObject.GetComponent<Rigidbody2D>());
        onDrop?.Invoke(this);
        Debug.Log("dropped");
    }


    private void _AddRB()
    {
        var rb = gameObject.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.angularDrag = 0;
        rb.freezeRotation = true;
    }

    
}
