using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    /// <summary>
    /// Трансформа, которую нужно вращать.
    /// </summary>
    [SerializeField] private Transform m_gameObject;

    /// <summary>
    /// Скорость вращения, единиц/с.
    /// </summary>
    [SerializeField] private Vector3 m_speed;

    /// <summary>
    /// Перемещение объекта в заданную точку.
    /// </summary>
    private void Update()
    {
        m_gameObject.Rotate(m_speed * Time.deltaTime);
    }
}
