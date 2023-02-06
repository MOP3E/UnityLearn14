using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelRotator : MonoBehaviour
{

    /// <summary>
    /// Левое колесо.
    /// </summary>
    [SerializeField] private Transform _leftWheel;

    /// <summary>
    /// Правое колесо.
    /// </summary>
    [SerializeField] private Transform _rightWheel;

    /// <summary>
    /// Диаметр колеса.
    /// </summary>
    [SerializeField] private float _wheelDiameter;

    /// <summary>
    /// Предыщущая позиция танка по X.
    /// </summary>
    private float _lastPosX = 0;

    /// <summary>
    /// Вращение колёс при изменении позиции танка.
    /// </summary>
    private void Update()
    {
        if (_lastPosX != transform.position.x)
        {
            float angle = -180 * transform.position.x / (Mathf.PI * _wheelDiameter);
            _leftWheel.eulerAngles = new Vector3(_leftWheel.eulerAngles.x, _leftWheel.eulerAngles.y, angle);
            _rightWheel.eulerAngles = new Vector3(_rightWheel.eulerAngles.x, _rightWheel.eulerAngles.y, angle);
            _lastPosX = transform.position.x;
        }
    }
}
