using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSpot : MonoBehaviour
{
    [SerializeField]
    private bool _occupied = false; // Флаг, указывающий, занят ли спот картой
    private Card _currentCard; // Ссылка на текущую карту в споте

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Проверяем, что спот не занят картой и объект, который вошел в зону спота, это карта
        if (!_occupied)
        {
            Card card = other.GetComponent<Card>();
            if (card != null)
            {
                _currentCard = card; // Сохраняем ссылку на текущую карту
                Debug.Log("Карта взаимодействует со спотом!");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Если карта покинула спот, снимаем занятость и убираем ссылку на карту
        if (_currentCard != null && other.gameObject == _currentCard.gameObject)
        {
            _occupied = false;
            _currentCard = null;
        }
    }

    // Установка флага занятости спота
    public void SetOccupied(bool occupied)
    {
        _occupied = occupied;
    }

    // Проверка, свободен ли спот
    public bool IsOccupied()
    {
        return _occupied;
    }

}
