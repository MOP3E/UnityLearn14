using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Глобальные переменные.
/// </summary>
public class Globals : MonoBehaviour
{
    /// <summary>
    /// Границы уровня.
    /// </summary>
    public static Globals Instance;

    /// <summary>
    /// Игровая камера.
    /// </summary>
    [SerializeField] private Camera _camera;

    /// <summary>
    /// Разрешение экрана.
    /// </summary>
    [SerializeField] private Vector2 _screenResolution;

    /// <summary>
    /// Левая граница экрана в мировых координатах.
    /// </summary>
    public float LeftBound => _camera.ScreenToWorldPoint(Vector3.zero).x;

    /// <summary>
    /// Левая граница экрана в мировых координатах.
    /// </summary>
    public float RightBound => _camera.ScreenToWorldPoint(new Vector3(_screenResolution.x, 0, 0)).x;

    /// <summary>
    /// Позиция по z нового камня.
    /// </summary>
    private float _newStoneZ = 0;

    /// <summary>
    /// Список игровых объектов, подлежащих удалению.
    /// </summary>
    private List<GameObject> _gameObjects = new List<GameObject>();

    /// <summary>
    /// Номер уровня.
    /// </summary>
    private int _currentLevel;

    /// <summary>
    /// Номер уровня.
    /// </summary>
    public int CurrentLevel => _currentLevel;

    /// <summary>
    /// Сколько монеток у игрока.
    /// </summary>
    private int _coins;

    /// <summary>
    /// Сколько монеток у игрока.
    /// </summary>
    public int Coins => _coins;

    /// <summary>
    /// Очки игрока.
    /// </summary>
    private int _score;

    /// <summary>
    /// Очки игрока.
    /// </summary>
    public int Score => _score;

    /// <summary>
    /// Периоды стрельбы.
    /// </summary>
    private float[] _firePeriods = { .5f, .4f, .3f, .2f, .1f, .05f, .025f };

    /// <summary>
    /// Период стрельбы.
    /// </summary>
    private float _firePeriod;

    /// <summary>
    /// Период стрельбы.
    /// </summary>
    public float FirePeriod => _firePeriod;

    /// <summary>
    /// Период стрельбы.
    /// </summary>
    public float NextFirePeriod
    {
        get
        {
            foreach (float period in _firePeriods)
            {
                if(period < _firePeriod) return period;
            }

            return -1;
        }
    }

    /// <summary>
    /// Достигнут предел улучшения скорострельности.
    /// </summary>
    public bool FireRateIsMax => _firePeriod == _firePeriods[_firePeriods.Length - 1];

    /// <summary>
    /// Число одновременно выпускаемых пуль.
    /// </summary>
    private int _bulletsCount;

    /// <summary>
    /// Число одновременно выпускаемых пуль.
    /// </summary>
    public int BulletsCount => _bulletsCount;

    /// <summary>
    /// Урон от одной пули.
    /// </summary>
    private int _bulletDamage;

    /// <summary>
    /// Урон от одной пули.
    /// </summary>
    public int BulletDamage => _bulletDamage;


    /// <summary>
    /// Создать синглтон границ уровня.
    /// </summary>
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (!Application.isEditor && Application.isPlaying)
        {
            _screenResolution.x = Screen.width;
            _screenResolution.y = Screen.height;
        }
    }

    /// <summary>
    /// Получить позицию по z нового камня.
    /// </summary>
    /// <returns></returns>
    public float GetNewStoneZ()
    {
        //каждый следующий камень появляется над предыдущим
        //почему-то камера стоит так, что для приближения к ней объекта нужно уменьшать значение по оси Z
        _newStoneZ -= 0.01f;
        return _newStoneZ;
    }

    /// <summary>
    /// Добавить объект в список удаляемых объектов.
    /// </summary>
    /// <param name="gameObject"></param>
    public void AddGameObject(GameObject gameObject)
    {
        _gameObjects.Add(gameObject);
    }

    /// <summary>
    /// Удалить объект из списка удаляемых объектов.
    /// </summary>
    /// <param name="gameObject"></param>
    public void RemoveGameObject(GameObject gameObject)
    {
        _gameObjects.Remove(gameObject);
    }

    /// <summary>
    /// Обнулить данные игры и сохранить рекорд.
    /// </summary>
    public void DataReset()
    {
        _currentLevel = 1;
        _coins = 0;
        _score= 0;
        _firePeriod = _firePeriods[0];
        _bulletsCount = 1;
        _bulletDamage = 1;
        GraphicsReset();
    }

    /// <summary>
    /// Обнулить состояние графики.
    /// </summary>
    /// <returns></returns>
    public void GraphicsReset()
    {
        _newStoneZ = 0;
        foreach (GameObject gameObject in _gameObjects) Destroy(gameObject);
        _gameObjects.Clear();
    }

    /// <summary>
    /// Переход на следующий уровень.
    /// </summary>
    public void NextLevel()
    {
        _currentLevel++;
    }

    /// <summary>
    /// Добавить монетку.
    /// </summary>
    public void AddCoin()
    {
        _coins++;
    }

    /// <summary>
    /// Удалить монетки.
    /// </summary>
    /// <param name="coins">Количество удаляемых монеток.</param>
    public bool RemoveCoins(int coins)
    {
        if(coins > _coins) return false;
        _coins -= coins;
        return true;
    }

    /// <summary>
    /// Добавить очки.
    /// </summary>
    /// <param name="score"></param>
    public void AddScore(int score)
    {
        _score += score;
    }

    /// <summary>
    /// Добавить пулю, выпускаемую при стрельбе.
    /// </summary>
    public void AddBullet()
    {
        if(_coins < _bulletsCount + 1) return;
        _bulletsCount++;
        RemoveCoins(_bulletsCount);
    }

    /// <summary>
    /// Повысить урон оружия.
    /// </summary>
    public void AddDamage()
    {
        if (_coins < _bulletDamage + 1) return;
        _bulletDamage++;
        RemoveCoins(_bulletDamage);
    }

    /// <summary>
    /// Переход на следующий уровень скорострельности.
    /// </summary>
    public void NextFireRaito()
    {
        foreach (float period in _firePeriods)
        {
            if (period < _firePeriod)
            {
                int coins = (int)(1f / _firePeriod);
                if(_coins < coins) return;
                _firePeriod = period;
                RemoveCoins(coins);
                break;
            }
        }
    }

    /// <summary>
    /// Заморозка камней.
    /// </summary>
    public void FreezeStones()
    {
        foreach (GameObject o in _gameObjects)
        {
            if (!o.TryGetComponent(out StoneMovement movement)) continue;
            movement.enabled = false;
        }
    }

    /// <summary>
    /// Разморозка камней.
    /// </summary>
    public void WarmStones()
    {
        foreach (GameObject o in _gameObjects)
        {
            if (o == null || !o.TryGetComponent(out StoneMovement movement)) continue;
            movement.enabled = true;
        }
    }
}
