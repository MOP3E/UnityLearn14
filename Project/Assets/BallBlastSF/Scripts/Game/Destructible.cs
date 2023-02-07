using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Разрушаемый объект.
/// </summary>
public class Destructible : MonoBehaviour
{
    /// <summary>
    /// Максимальное число очков жизни разрушаемого объекта.
    /// </summary>
    public int MaxHitpoints;

    /// <summary>
    /// Очки жизни разрушаемого объекта.
    /// </summary>
    private int _hitpoints;

    /// <summary>
    /// Событие разрушения объекта.
    /// </summary>
    [HideInInspector] public UnityEvent Destruction;

    /// <summary>
    /// Очки жизни разрушаемого объекта.
    /// </summary>
    private int Hitpoints
    {
        get => _hitpoints;
        set
        {
            _hitpoints = value;
            if (HitpointsChange != null) HitpointsChange.Invoke();
        }
    }

    /// <summary>
    /// Событие изменения очков жизни персонажа.
    /// </summary>
    [HideInInspector] public UnityEvent HitpointsChange;

    /// <summary>
    /// Уже убит.
    /// </summary>
    private bool _isKilled = false;

    /// <summary>
    /// Вызывается перед первым кадром.
    /// </summary>
    public void Start()
    {
        Hitpoints = MaxHitpoints;
    }

    /// <summary>
    /// Нанесение урона объекту.
    /// </summary>
    /// <param name="damage">Величина наносимого урона.</param>
    public void Hit(int damage)
    {
        Hitpoints -= damage;
        if (Hitpoints <= 0) Kill();
    }

    /// <summary>
    /// Лечение объекта.
    /// </summary>
    public bool Cure(int cure)
    {
        if (Hitpoints >= MaxHitpoints) return false;
        Hitpoints += cure;
        if (Hitpoints > MaxHitpoints) Hitpoints = MaxHitpoints;
        return true;
    }

    /// <summary>
    /// Гарантированное убийство объекта.
    /// </summary>
    public void Kill()
    {
        if (_isKilled) return;

        Hitpoints = 0;
        if (Destruction != null) Destruction.Invoke();
        _isKilled = true;
    }

    /// <summary>
    /// Получить число очков жизни.
    /// </summary>
    public int GetHitpoints()
    {
        return Hitpoints;
    }

    /// <summary>
    /// Получить нормализованное число очков жизни.
    /// </summary>
    public float GetNormalizedHitpoints()
    {
        return Hitpoints < MaxHitpoints ? (float)Hitpoints / (float)MaxHitpoints : 1f;
    }
}
