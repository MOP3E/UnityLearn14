using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    /// <summary>
    /// Префаб снаряда.
    /// </summary>
    [SerializeField] private Projectile _projectile;

    /// <summary>
    /// Позиция для стрельбы.
    /// </summary>
    [SerializeField] private Transform _firePosition;

    /// <summary>
    /// Время между выстрелами.
    /// </summary>
    private float _firePeriod;

    /// <summary>
    /// Урон от одного выстрела.
    /// </summary>
    private int _damage;

    /// <summary>
    /// Угол между траекториями при стрельбе несколькими снарядами.
    /// </summary>
    [SerializeField] private float _miltipleAngle;

    /// <summary>
    /// Максимальный угол разлёта снарядов.
    /// </summary>
    [SerializeField] private float _maxAngle;

    /// <summary>
    /// Время, прошедшее между выстрелами.
    /// </summary>
    private float _timer;

    /// <summary>
    /// Число одновременно выпускаемых снарядов.
    /// </summary>
    private int _count;

    /// <summary>
    /// Разрешение стрелять.
    /// </summary>
    private bool _fire;

    /// <summary>
    /// Сброс всех параметров пушки в текущие глобальные значения.
    /// </summary>
    public void CannonReset()
    {
        //чтобы первый выстрел был сразу после нажатия на кнопку стрельбы
        _count = Globals.Instance.BulletsCount;
        _firePeriod = Globals.Instance.FirePeriod;
        _damage = Globals.Instance.BulletDamage;
        _timer = _firePeriod;
    }

    private void Update()
    {
        if (_fire) _timer += Time.deltaTime;
    }

    /// <summary>
    /// Стрельба.
    /// </summary>
    public void Fire()
    {
        if(!_fire) _fire = true;
        if(_timer >= _firePeriod)
        {
            SpawnProjectile();
            _timer -= _firePeriod;
        }
    }

    /// <summary>
    /// Не стрельба.
    /// </summary>
    public void NotFire()
    {
        if (_fire)
        {
            _fire = false;
            //чтобы первый выстрел был сразу после нажатия на кнопку стрельбы
            _timer = _firePeriod;
        }
    }

    /// <summary>
    /// Размещение снаряда на игровом поле.
    /// </summary>
    private void SpawnProjectile()
    {
        float multipleAngle = _miltipleAngle * (_count - 1) > _maxAngle ? _maxAngle / (_count - 1) : _miltipleAngle;
        float startZ = transform.rotation.eulerAngles.z - (_count - 1) / 2f * multipleAngle;

        for (int i = 0; i < _count; i++)
        {
            Projectile projectile = 
                Instantiate(_projectile, _firePosition.position, Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, startZ + i * multipleAngle));
            projectile.SetDamage(_damage);

            //добавить снаряд в список удаляемых объектов
            Globals.Instance.AddGameObject(projectile.gameObject);
        }
    }
}
