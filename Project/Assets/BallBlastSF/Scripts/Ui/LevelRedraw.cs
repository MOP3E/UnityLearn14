using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelRedraw : MonoBehaviour
{
    /// <summary>
    /// Текст, в который выводится запись об уровне.
    /// </summary>
    [SerializeField] private Text _text;

    /// <summary>
    /// Предыдущее количество монеток.
    /// </summary>
    private int _prevLevel = int.MinValue;

    void Update()
    {
        if (_prevLevel == Globals.Instance.CurrentLevel) return;
        _prevLevel = Globals.Instance.CurrentLevel;
        _text.text = $"Уровень {_prevLevel}";
    }
}
