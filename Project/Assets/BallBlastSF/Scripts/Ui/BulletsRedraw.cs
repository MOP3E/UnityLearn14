using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletsRedraw : MonoBehaviour
{
    /// <summary>
    /// Текст, в который выводится запись об очках.
    /// </summary>
    [SerializeField] private Text _text;

    /// <summary>
    /// Предыдущее количество монеток.
    /// </summary>
    private int _prevValue = int.MinValue;

    void Update()
    {
        if (_prevValue == Globals.Instance.BulletsCount) return;
        _prevValue = Globals.Instance.BulletsCount;
        _text.text = $"Больше пуль ({_prevValue}->{_prevValue + 1}/{_prevValue + 1} руб.)";
    }
}
