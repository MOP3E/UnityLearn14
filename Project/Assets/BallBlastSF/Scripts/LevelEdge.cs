using UnityEngine;

/// <summary>
/// Тип края экрана.
/// </summary>
public enum EdgeType
{
    Bottom,
    Left,
    Right,
}

/// <summary>
/// Край экрана.
/// </summary>
public class LevelEdge : MonoBehaviour
{
    /// <summary>
    /// Тип границы уровня.
    /// </summary>
    [SerializeField] private EdgeType _edgeType;

    /// <summary>
    /// Тип границы уровня.
    /// </summary>
    public EdgeType EdgeType => _edgeType;
}
