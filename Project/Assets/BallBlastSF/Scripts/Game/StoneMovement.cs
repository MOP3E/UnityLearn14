using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneMovement : MonoBehaviour
{
    /// <summary>
    /// Сила гравитации.
    /// </summary>
    [SerializeField] private float _gravity;

    /// <summary>
    /// Вертикальная скорость, которая появляется у камня при соприкосновении с нижним краем экрана.
    /// </summary>
    [SerializeField] private float _reboundSpeed;

    /// <summary>
    /// Горизонтальная скорость камня.
    /// </summary>
    [SerializeField] private float _horizontalSpeed;

    /// <summary>
    /// Скорости по осям.
    /// </summary>
    private Vector3 _velocity;

    /// <summary>
    /// Использовать гравитацию.
    /// </summary>
    private bool _useGravity;

    /// <summary>
    /// Начальная инициализация перменных.
    /// </summary>
    private void Awake()
    {
        _useGravity = false;
        _velocity.x = -Mathf.Sign(transform.position.x) * _horizontalSpeed;
    }

    private void Update()
    {
        TryEnableGravity();
        Move();
    }

    /// <summary>
    /// Перемещение камня.
    /// </summary>
    private void Move()
    {
        _velocity.x = Mathf.Sign(_velocity.x) * _horizontalSpeed;
        if(_useGravity) _velocity.y -= _gravity * Time.deltaTime;
        transform.position += _velocity * Time.deltaTime;
    }

    /// <summary>
    /// Попытаться включить гравитацию.
    /// </summary>
    private void TryEnableGravity()
    {
        if (_useGravity) return;
        if (Mathf.Abs(transform.position.x) <= Mathf.Abs(Globals.Instance.LeftBound)) _useGravity = true;
    }

    /// <summary>
    /// Столкновение с границами уровня.
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out LevelEdge levelEdge)) return;

        switch (levelEdge.EdgeType)
        {
            case EdgeType.Bottom:
                _velocity.y = _reboundSpeed;
                break;
            case EdgeType.Left:
                if(_velocity.x < 0) _velocity.x *= -1;
                break;
            case EdgeType.Right:
                if (_velocity.x > 0) _velocity.x *= -1;
                break;
        }
    }

    /// <summary>
    /// Откорректировать вертикальную скорость.
    /// </summary>
    /// <param name="velocity">Величина коррекции скорости.</param>
    public void AddVerticalVelocity(float velocity)
    {
        _velocity.y += velocity;
    }
    
    /// <summary>
    /// Изменить направление движения.
    /// </summary>
    /// <param name="direction">Направление движения (истина - движение вправо).</param>
    public void SetHorisontalDirection(bool direction)
    {
        _velocity.x = direction ? _horizontalSpeed : -_horizontalSpeed;
    }
}
