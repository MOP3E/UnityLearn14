using UnityEngine;
using UnityEngine.UI;

public class StoneHitpointsText : MonoBehaviour
{
    /// <summary>
    /// Текст с очками жизни камня.
    /// </summary>
    [SerializeField] private Text _hitpointsText;

    /// <summary>
    /// Разрушаемый объект, из которого нужно брать очки жизни.
    /// </summary>
    private Destructible _destructible;

    private void Awake()
    {
        _destructible = GetComponent<Destructible>();
        _destructible.HitpointsChange.AddListener(OnHitpointsChange);
    }

    private void OnDestroy()
    {
        _destructible.HitpointsChange.RemoveListener(OnHitpointsChange);
    }

    private void OnHitpointsChange()
    {
        DrawHitpoints();
    }

    private void DrawHitpoints()
    {
        int hitpoints = _destructible.GetHitpoints();
        _hitpointsText.text = hitpoints > 1000 ? $"{hitpoints / 1000}K" : hitpoints.ToString();
    }
}
