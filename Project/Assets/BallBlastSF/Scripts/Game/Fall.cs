using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fall : MonoBehaviour
{
    /// <summary>
    /// Сила гравитации.
    /// </summary>
    [SerializeField] private float _gravity;

    /// <summary>
    /// Объект падает.
    /// </summary>
    private bool _isFall = true;

    /// <summary>
    /// Скорость падения.
    /// </summary>
    private float _speed = 0f;

    /// <summary>
    /// Падение предмета.
    /// </summary>
    private void Update()
    {
        if (!_isFall) return;
        //падение
        _speed -= _gravity * Time.deltaTime;
        transform.position += new Vector3(0, _speed * Time.deltaTime, 0);
    }

    /// <summary>
    /// Остановка падения при столкновении с полом.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Попал!");
        if (!_isFall) return;
        if (collision == null) return;
        if (!collision.TryGetComponent(out LevelEdge edge)) return;
        _isFall = edge.EdgeType != EdgeType.Bottom;
        //Debug.Log(_isFall.ToString());
    }
}
