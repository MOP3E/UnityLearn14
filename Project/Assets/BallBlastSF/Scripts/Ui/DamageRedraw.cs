using UnityEngine;
using UnityEngine.UI;

public class DamageRedraw : MonoBehaviour
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
        if (_prevValue == Globals.Instance.BulletDamage) return;
        _prevValue = Globals.Instance.BulletDamage;
        _text.text = $"Больше урон ({_prevValue}->{_prevValue + 1}/{_prevValue + 1} руб.)";
    }
}
