using UnityEngine;
using UnityEngine.UI;

public class FireRateButtonEnable : MonoBehaviour
{
    /// <summary>
    /// Кнопка повышения частоты стрельбы.
    /// </summary>
    [SerializeField] private Button _button;

    private void Update()
    {
        if(_button.enabled && Globals.Instance.FireRateIsMax)
            _button.gameObject.SetActive(false);
    }

}
