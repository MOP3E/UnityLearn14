using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinsInShopRedraw : MonoBehaviour
{
    /// <summary>
    /// Текст, в который выводится запись о монетках.
    /// </summary>
    [SerializeField] private Text _text;

    /// <summary>
    /// Предыдущее количество монеток.
    /// </summary>
    private int _prevCoins = int.MinValue;

    void Update()
    {
        if (_prevCoins == Globals.Instance.Coins) return;
        _prevCoins = Globals.Instance.Coins;
        _text.text = $"У вас {_prevCoins} руб.";
    }
}
