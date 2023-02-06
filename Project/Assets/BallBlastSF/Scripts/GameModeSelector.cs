using System;
using UnityEngine;
using UnityEngine.Events;

public enum GameModes
{
    /// <summary>
    /// Главное меню.
    /// </summary>
    MainMenu,
    /// <summary>
    /// Игра.
    /// </summary>
    Game,
    /// <summary>
    /// Пауза.
    /// </summary>
    Pause,
    /// <summary>
    /// Уровень пройден.
    /// </summary>
    Passed,
    /// <summary>
    /// Конец игры.
    /// </summary>
    Defeat
}

public class GameModeSelector : MonoBehaviour
{
    /// <summary>
    /// Танк.
    /// </summary>
    [SerializeField] private TankController _tank;

    /// <summary>
    /// Пушка танка.
    /// </summary>
    [SerializeField] private Cannon _cannon;

    /// <summary>
    /// Генератор камней на уровне.
    /// </summary>
    [SerializeField] private StoneSpawner _spawner;

    /// <summary>
    /// Текущий режим игры.
    /// </summary>
    private GameModes _mode;

    /// <summary>
    /// Главное меню.
    /// </summary>
    [Space(5)]
    public UnityEvent MainMenu;

    /// <summary>
    /// Запуск игры.
    /// </summary>
    public UnityEvent GameStart;

    /// <summary>
    /// Постановка игры на паузу.
    /// </summary>
    public UnityEvent Pause;

    /// <summary>
    /// Продолжение игры.
    /// </summary>
    public UnityEvent Play;

    /// <summary>
    /// Уровень пройден.
    /// </summary>
    public UnityEvent Passed;

    /// <summary>
    /// Проигрыш.
    /// </summary>
    public UnityEvent Defeat;

    /// <summary>
    /// Таймер проверки существования камней на уровне.
    /// </summary>
    private float _stonesTestTimer;

    /// <summary>
    /// Размещение камней на уровне завершено.
    /// </summary>
    private bool _spawnComlete;

    private void Awake()
    {
        _tank.StoneCollision.AddListener(OnStoneCollision);
        _stonesTestTimer = 0f;
        _spawnComlete = false;
        _spawner.SpawnComplete.AddListener(OnSpawnComplete);
        _mode = GameModes.MainMenu;
    }

    private void OnDestroy()
    {
        _tank.StoneCollision.RemoveListener(OnStoneCollision);
        _spawner.SpawnComplete.RemoveListener(OnSpawnComplete);
    }

    /// <summary>
    /// Обработка события столкновения камня с танком.
    /// </summary>
    private void OnStoneCollision()
    {
        //перейти в меню поражения
        SetDefeatdMenu();
    }

    /// <summary>
    /// Обработка события завершения генерации камней на уровне.
    /// </summary>
    private void OnSpawnComplete()
    {
        _spawnComlete = true;
    }

    private void Start()
    {
        SetMainMenu();
    }

    private void Update()
    {
        switch (_mode)
        {
            case GameModes.MainMenu:
                //запуск игры
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)) StartGame();
                break;
            case GameModes.Game:
                //проверка на победу (камней на уровне не осталось)
                if (Passed != null && _spawnComlete)
                {
                    _stonesTestTimer += Time.deltaTime;
                    if (_stonesTestTimer >= .5f)
                    {
                        _stonesTestTimer = 0;
                        if (FindObjectOfType<Stone>() == null && FindObjectOfType<Coin>() == null)
                        {
                            //перейти в меню перехода на следующий уровень
                            SetPassedMenu();
                        }
                    }
                }
                //постановка игры на паузу
                if (Input.GetKeyDown(KeyCode.Escape)) SetPause();
                break;
            case GameModes.Pause:
                //снятие игры с паузы
                if (Input.GetKeyDown(KeyCode.Escape)) SetMainMenu();
                else if (Input.GetKeyDown(KeyCode.Return)) SetGame();
                break;
            case GameModes.Passed:
                //переход на следующий уровень
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)) ContinueGame();
                break;
            case GameModes.Defeat:
                //переход в главное меню
                if (Input.GetKeyDown(KeyCode.Escape)) SetMainMenu();
                break;
        }
    }



    /// <summary>
    /// Действия по переключению в главное меню.
    /// </summary>
    public void SetMainMenu()
    {
        _mode = GameModes.MainMenu;
        //остановить время
        Time.timeScale = 0;
        //поставить танк по центру
        _tank.transform.position = Vector3.zero;
        //обнулить память о генерации камней
        _spawnComlete = false;
        //обнулить состояние игры
        Globals.Instance.DataReset();
        if (MainMenu != null) MainMenu.Invoke();
    }

    /// <summary>
    /// Действия по запуску новой игры.
    /// </summary>
    public void StartGame()
    {
        //TODO: обнулить очки, монеты, урон пушки, настройку генерации уровня, номер уровня и т.д.
        _mode = GameModes.Game;
        //поставить танк по центру
        _tank.transform.position = Vector3.zero;
        //запустить время
        Time.timeScale = 1;
        if (GameStart != null) GameStart.Invoke();
    }

    /// <summary>
    /// Действия по продолжению игры после победы.
    /// </summary>
    public void ContinueGame()
    {
        _mode = GameModes.Game;
        //поставить танк по центру
        _tank.transform.position = Vector3.zero;
        //настроить пушку
        _cannon.CannonReset();
        //настроить генерацию камней под новую пушку
        _spawner.SpawnReset();
        //запустить время
        Time.timeScale = 1;
        if (GameStart != null) GameStart.Invoke();
    }

    /// <summary>
    /// Постановка игры на паузу.
    /// </summary>
    public void SetPause()
    {
        _mode = GameModes.Pause;
        //остановить время
        Time.timeScale = 0;
        if (Pause != null) Pause.Invoke();
    }

    /// <summary>
    /// Возврат обратно в игру из паузы.
    /// </summary>
    public void SetGame()
    {
        _mode = GameModes.Game;
        //запустить время
        Time.timeScale = 1;
        if (Play != null) Play.Invoke();
    }

    /// <summary>
    /// Действия по переключению в меню перехода к следующему уровню.
    /// </summary>
    public void SetPassedMenu()
    {
        _mode = GameModes.Passed;
        //остановить время
        Time.timeScale = 0;
        //обнулить память о генерации камней
        _spawnComlete = false;
        //обнулить игровую графику
        Globals.Instance.GraphicsReset();
        //перейти на следующий уровень
        Globals.Instance.NextLevel();
        //поставить танк по центру
        _tank.transform.position = Vector3.zero;
        if (Passed != null) Passed.Invoke();
    }

    /// <summary>
    /// Действия по переключению в меню перехода к следующему уровню.
    /// </summary>
    public void SetDefeatdMenu()
    {
        _mode = GameModes.Defeat;
        //остановить время
        Time.timeScale = 0;
        if (Defeat != null) Defeat.Invoke();
    }
}
