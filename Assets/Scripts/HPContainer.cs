using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class HPContainer : MonoBehaviour
{
    public UnityAction onDead;

    [SerializeField] private UnityEvent<int> _onHPChanged = new UnityEvent<int>();
    [Space]
    [SerializeField] private TextMeshProUGUI _hpText;
    [SerializeField] private int _currentHP = 10;

    private void OnEnable()
    {
        UpdateHealthStatus();
    }


    public void AddDamage(DamageEffect effect)
    {
        _currentHP -= effect.DamageAmount;
        _onHPChanged.Invoke(_currentHP);
        UpdateHealthStatus();
    }


    public void Heal(HealEffect effect)
    {
        _currentHP += effect.HealAmount;
        _onHPChanged.Invoke(_currentHP);
        UpdateHealthStatus();
    }


    public void UpdateHealthStatus()
    {
        if(_currentHP <= 0)
        {
            _currentHP = 0;
            _Die();
        }
        _hpText.text = $"HP: {_currentHP}";
    }

    private void _Die()
    {
        onDead.Invoke();
    }
}
