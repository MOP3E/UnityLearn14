using UnityEngine;
using UnityEngine.UI;

public class FireRateRedraw : MonoBehaviour
{
    /// <summary>
    /// Текст, в который выводится запись об очках.
    /// </summary>
    [SerializeField] private Text _text;

    /// <summary>
    /// Предыдущее количество монеток.
    /// </summary>
    private float _prevValue = float.MinValue;

    void Update()
    {
        if (_prevValue == Globals.Instance.FirePeriod) return;
        _prevValue = Globals.Instance.FirePeriod;
        float nextValue = Globals.Instance.NextFirePeriod;
        _text.text = $"Чаще стрельба ({(1f/_prevValue):###0.0}->{(1f / nextValue):###0.0}/{(int)(1f/nextValue)} руб.)";
    }
}
