using UnityEngine;

/// <summary>
/// Размер камня.
/// </summary>
public enum StoneSize
{
    Small = 0,
    Medium = 1,
    Big = 2,
    Huge = 3
}

[RequireComponent(typeof(StoneMovement))]
public class Stone : Destructible
{
    /// <summary>
    /// Размер камня.
    /// </summary>
    [SerializeField] private StoneSize _size;

    /// <summary>
    /// Дополнительная вертикальная скорость при спавне камня.
    /// </summary>
    [SerializeField] private float _spawnUpForce;

    /// <summary>
    /// Растеризатор спрайта камня.
    /// </summary>
    [SerializeField] SpriteRenderer _renderer;

    /// <summary>
    /// Цвета камней при спавне.
    /// </summary>
    [SerializeField] private Color[] _colors;

    /// <summary>
    /// Префаб монетки.
    /// </summary>
    [SerializeField] private Coin _coin;

    /// <summary>
    /// Префаб щита.
    /// </summary>
    [SerializeField] private Shield _shield;

    /// <summary>
    /// Префаб снежинки.
    /// </summary>
    [SerializeField] private Snowflake _snowflake;

    /// <summary>
    /// Скрипт перемещения камня.
    /// </summary>
    private StoneMovement _stoneMovement;

    private void Awake()
    {
        _stoneMovement = GetComponent<StoneMovement>();
        SetSize(_size);
        Destruction.AddListener(OnStoneDestruction);
    }

    /// <summary>
    /// Действия, выполняемые при уничтожении объекта.
    /// </summary>
    private void OnDestroy()
    {
        Destruction.RemoveListener(OnStoneDestruction);
    }

    private new void Start()
    {
        _renderer.color = _colors[Random.Range(0, _colors.Length)];
        base.Start();
    }

    /// <summary>
    /// Обработка события разрушения объекта.
    /// </summary>
    private void OnStoneDestruction()
    {
        switch (_size)
        {
            case StoneSize.Huge:
            case StoneSize.Big:
            case StoneSize.Medium:
                SpawnStones();
                break;
            case StoneSize.Small:
                //самый маленький камень - ничего не делать
                break;
        }

        //генерация бонусов
        //монетка генерируется с 33% вероятностью
        if(Random.Range(0, 100) > 66) SpawnCoin();
        //щит генерируется с 5% вероятностью
        if (Random.Range(0, 100) > 94) SpawnShield();
        //снежинка генерируется с 5% вероятностью
        if (Random.Range(0, 100) > 94) SpawnSnowflake();

        //подсчёт очков за разрушение камня
        Globals.Instance.AddScore(MaxHitpoints);

        //удалить игровой объект из списка разрушаемых объектов
        Globals.Instance.RemoveGameObject(gameObject);

        //разрушить игровой объект
        Destroy(gameObject);
    }

    /// <summary>
    /// Создание новых камней.
    /// </summary>
    private void SpawnStones()
    {
        for (int i = 0; i < 2; i++)
        {
            //создать камень и задать ему размер
            Stone stone = Instantiate(this, new Vector3(transform.position.x, transform.position.y, Globals.Instance.GetNewStoneZ()) , Quaternion.identity);
            switch (_size)
            {
                case StoneSize.Huge:
                    stone.SetSize(StoneSize.Big);
                    break;
                case StoneSize.Big:
                    stone.SetSize(StoneSize.Medium);
                    break;
                case StoneSize.Medium:
                    stone.SetSize(StoneSize.Small);
                    break;
                case StoneSize.Small:
                    stone.SetSize(StoneSize.Small);
                    break;
            }

            stone.MaxHitpoints = (int)Mathf.Clamp(MaxHitpoints / 2f, 1f, MaxHitpoints);
            stone._stoneMovement.AddVerticalVelocity(_spawnUpForce);
            stone._stoneMovement.SetHorisontalDirection(i % 2 == 0);

            //добавить камень в список удаляемых объектов
            Globals.Instance.AddGameObject(stone.gameObject);
        }
    }

    /// <summary>
    /// Создание монетки в момент убивания камня.
    /// </summary>
    private void SpawnCoin()
    {
        GameObject gameObject = Instantiate(_coin, new Vector3(transform.position.x, transform.position.y, Globals.Instance.GetNewStoneZ()), Quaternion.identity).gameObject;
        //добавить монетку в список удаляемых объектов
        Globals.Instance.AddGameObject(gameObject);

    }

    /// <summary>
    /// Создание щита в момент убивания камня.
    /// </summary>
    private void SpawnShield()
    {
        GameObject gameObject = Instantiate(_shield, new Vector3(transform.position.x, transform.position.y, Globals.Instance.GetNewStoneZ()), Quaternion.identity).gameObject;
        //добавить щит в список удаляемых объектов
        Globals.Instance.AddGameObject(gameObject);
    }

    /// <summary>
    /// Создание снежинки в момент убивания камня.
    /// </summary>
    private void SpawnSnowflake()
    {
        GameObject gameObject = Instantiate(_snowflake, new Vector3(transform.position.x, transform.position.y, Globals.Instance.GetNewStoneZ()), Quaternion.identity).gameObject;
        //добавить снежинку в список удаляемых объектов
        Globals.Instance.AddGameObject(gameObject);
    }

    /// <summary>
    /// Задать размер камня.
    /// </summary>
    /// <param name="size">Размер камня.</param>
    public void SetSize(StoneSize size)
    {
        transform.localScale = GetSizeVector(size);
        _size = size;
    }

    /// <summary>
    /// Получить вектор размера камня.
    /// </summary>
    /// <param name="size">Размер, для которого нужно получить вектор.</param>
    /// <returns></returns>
    private Vector3 GetSizeVector(StoneSize size)
    {
        switch (size)
        {
            case StoneSize.Huge:
                return new Vector3(1, 1, 1);
            case StoneSize.Big:
                return new Vector3(.75f, .75f, .75f);
            case StoneSize.Medium:
                return new Vector3(.6f, .6f, .6f);
            case StoneSize.Small:
                return new Vector3(.4f, .4f, .4f);
        }
        //размер не распознан - возвратить самый большой камень
        return new Vector3(1, 1, 1);
    }
}
