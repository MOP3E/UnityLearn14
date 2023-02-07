using UnityEngine;
using UnityEngine.Events;

public class StoneSpawner : MonoBehaviour
{
    /// <summary>
    /// Префаб камня.
    /// </summary>
    [Header("Размещение объектов")]
    [SerializeField] private Stone _stonePrefab;

    /// <summary>
    /// Точки размещения камней.
    /// </summary>
    [SerializeField] private Transform[] _spawnPoints;

    /// <summary>
    /// Период размещения камней.
    /// </summary>
    [SerializeField] private float _spawnPeriod;

    /// <summary>
    /// Пушка.
    /// </summary>
    [Header("Балансировка")]
    [SerializeField] private Cannon _cannon;

    /// <summary>
    /// Количество камней, которое нужно размещать.
    /// </summary>
    private int _count;

    /// <summary>
    /// Максимальное число очков жизни у камня.
    /// </summary>
    private int _maxStoneHitpoints;

    /// <summary>
    /// Минимальный процент жизни у нового камня от максимального числа ОЖ.
    /// </summary>
    [SerializeField] [Range(0f, 1f)] private float _minStoneHitpointsPercentage;

    /// <summary>
    /// Коэффициент увеличения прочности камня в зависимости от урона.
    /// </summary>
    [SerializeField] private float _maxHitpointRate;

    private float _spawnTimer;

    /// <summary>
    /// Размещение камней на уровне завершено.
    /// </summary>
    [HideInInspector] public UnityEvent SpawnComplete;

    /// <summary>
    /// Максимальное число очков жизни у камня.
    /// </summary>
    private int _minStoneHitpoints;

    /// <summary>
    /// Количество созданных камней.
    /// </summary>
    private int _spawned;

    private void Start()
    {
        SpawnReset();
    }

    public void SpawnReset()
    {
        //рассчитать ДПС и пределы прочности камней
        int damagePerSecond = (int)(Globals.Instance.BulletDamage * Globals.Instance.BulletsCount * 1f / Globals.Instance.FirePeriod);
        _maxStoneHitpoints = (int)(damagePerSecond * _maxHitpointRate);
        if (_maxStoneHitpoints == 0) _maxStoneHitpoints = 1;
        _minStoneHitpoints = (int)(_maxStoneHitpoints * _minStoneHitpointsPercentage);
        if (_minStoneHitpoints == 0) _minStoneHitpoints = 1;
        _count = Globals.Instance.CurrentLevel;
        _spawned = 0;

        //чтобы камень вылетал сразу при старте уровня
        _spawnTimer = _spawnPeriod;

        //включить себя
        enabled = true;
    }

    private void Update()
    {
        _spawnTimer += Time.deltaTime;

        if (_spawnTimer >= _spawnPeriod)
        {
            Spawn();
            _spawnTimer = 0;
        }

        //отключить генерацию новых камней когда всё, что нужно, сгенерировано
        if (_spawned == _count) 
        { 
            enabled = false;
            if (SpawnComplete != null)
            {
                SpawnComplete.Invoke(); 
            }
        }
    }

    /// <summary>
    /// Размещение новых камней на уровне.
    /// </summary>
    private void Spawn()
    {
        Vector3 spawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length)].position;
        spawnPoint.z = Globals.Instance.GetNewStoneZ();
        Stone stone = Instantiate(_stonePrefab, spawnPoint, Quaternion.identity);
        stone.SetSize((StoneSize)Random.Range(1, 4));
        stone.MaxHitpoints = Random.Range(_minStoneHitpoints, _maxStoneHitpoints + 1);

        //добавить камень в список удаляемых объектов
        Globals.Instance.AddGameObject(stone.gameObject);

        _spawned++;
    }
}
