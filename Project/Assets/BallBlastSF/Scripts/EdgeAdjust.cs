using UnityEngine;

public class EdgeAdjust : MonoBehaviour
{
    /// <summary>
    /// Левая граница.
    /// </summary>
    [SerializeField] private Transform _leftEdge;
    
    /// <summary>
    /// Правая граница.
    /// </summary>
    [SerializeField] private Transform _rightEdge;

    /// <summary>
    /// Позиционирование краёв уровня согласно размеру экрана.
    /// </summary>
    void Start()
    {
        //ширина границы 0.2, следовательно, сдвигать нужно на 0.1
        _leftEdge.position = new Vector3(Globals.Instance.LeftBound - .1f, transform.position.y, transform.position.z);
        _rightEdge.position = new Vector3(Globals.Instance.RightBound + .1f, transform.position.y, transform.position.z);
    }
}
