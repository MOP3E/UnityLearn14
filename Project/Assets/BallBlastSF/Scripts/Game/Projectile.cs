using UnityEngine;

public class Projectile : MonoBehaviour
{
    /// <summary>
    /// Скорость снаряда.
    /// </summary>
    [SerializeField] private float _speed;

    /// <summary>
    /// Время жизни снаряда.
    /// </summary>
    [SerializeField] private float _lifetime;

    /// <summary>
    /// Урон снаряда.
    /// </summary>
    private int _damage;

    /// <summary>
    /// Время жизни снаряда.
    /// </summary>
    private float _life = 0;

    /// <summary>
    /// Перемещение снаряда вверх.
    /// </summary>
    private void Update()
    {
        transform.Translate(0, _speed * Time.deltaTime, 0);
        //если время жизни вышло - самоуничтожиться
        _life += Time.deltaTime;
        if (_life >= _lifetime) SelfDestruct();
    }

    /// <summary>
    /// Нанесение урона при столкновении.
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //снаряды не должны сталкиваться с другими снарядами
        if (collision.transform.root.GetComponent<Projectile>() != null) return;

        //попытаться получить Destructible
        if (!collision.transform.root.TryGetComponent(out Destructible destructible)) return;

        //нанести урон
        destructible.Hit(_damage);

        //самоуничтожиться
        SelfDestruct();
    }

    /// <summary>
    /// Настроить урон у снаряда.
    /// </summary>
    public void SetDamage(int damage)
    {
        _damage = damage;
    }

    /// <summary>
    /// Самоуничтожиться.
    /// </summary>
    private void SelfDestruct()
    {
        //удалить игровой объект из списка разрушаемых объектов
        Globals.Instance.RemoveGameObject(gameObject);

        //самоуничтожиться
        Destroy(gameObject);
    }
}
