using UnityEngine;
using UnityEngine.UI;

public class ScoreRedraw : MonoBehaviour
{
    /// <summary>
    /// Текст, в который выводится запись об очках.
    /// </summary>
    [SerializeField] private Text _text;

    /// <summary>
    /// Предыдущее количество монеток.
    /// </summary>
    private int _prevScore = int.MinValue;

    void Update()
    {
        if (_prevScore == Globals.Instance.Score) return;
        _prevScore = Globals.Instance.Score;
        _text.text = _prevScore.ToString();
    }
}
