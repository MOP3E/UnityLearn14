using UnityEngine;
using UnityEngine.Events;

public class TankController : MonoBehaviour
{
    /// <summary>
    /// Ось ввода мыши.
    /// </summary>
    [SerializeField] private string _mouseInputAxis;

    /// <summary>
    /// Ось ввода клавиатуры.
    /// </summary>
    [SerializeField] private string _keyboardInputAxis;

    /// <summary>
    /// Чувствительность при повороте.
    /// </summary>
    [SerializeField] private float _sensitivity;

    /// <summary>
    /// Пушка танка.
    /// </summary>
    [SerializeField] private Cannon _cannon;

    /// <summary>
    /// Энергетический щит.
    /// </summary>
    [SerializeField] private EnergyShield _energyShield;

    /// <summary>
    /// Чувствительность при повороте клавиатурой.
    /// </summary>
    private const float _keyboardSensitivityFactor = 25f;

    /// <summary>
    /// Событие столкновения с камнем.
    /// </summary>
    [HideInInspector] public UnityEvent StoneCollision;
    
    /// <summary>
    /// Время нахождения под щитом.
    /// </summary>
    private float _shieldTime = 0f;

    /// <summary>
    /// Время заморозки камней на уровне.
    /// </summary>
    private float _froizenTime = 0f;

    /// <summary>
    /// Управление танком с мыши и клавиатуры.
    /// </summary>
    void Update()
    {
        float leftBound = Globals.Instance.LeftBound;
        float rightBound = Globals.Instance.RightBound;

        //перемещение танка вправо-влево
        if (!string.IsNullOrEmpty(_mouseInputAxis))
        {
            float mouseInputAxis = Input.GetAxis(_mouseInputAxis);
            if ((transform.position.x > leftBound && mouseInputAxis < 0) || (transform.position.x < rightBound && mouseInputAxis > 0))
                transform.Translate(Input.GetAxis(_mouseInputAxis) * _sensitivity, 0, 0);
        }
        if (!string.IsNullOrEmpty(_keyboardInputAxis))
        {
            float keyboardInputAxis = Input.GetAxis(_keyboardInputAxis);
            if ((transform.position.x > leftBound && keyboardInputAxis < 0) || (transform.position.x < rightBound && keyboardInputAxis > 0))
                transform.Translate(keyboardInputAxis * _sensitivity * _keyboardSensitivityFactor * Time.deltaTime, 0, 0);
        }

        if (transform.position.x < leftBound) transform.position = new Vector3(leftBound, transform.position.y, transform.position.z);
        if (transform.position.x > rightBound) transform.position = new Vector3(rightBound, transform.position.y, transform.position.z);

        //стрельба из пушки
        if(Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space))
        {
            //горшочек, вари!
            _cannon.Fire();
        }
        else
        {
            //горшошчек, не вари!
            _cannon.NotFire();
        }

        //уменьшение времени нахождения под щитом
        if (_shieldTime > 0)
        {
            _shieldTime -= Time.deltaTime;
            if(_shieldTime <= 0)
            {
                //отключить энергощит
                _shieldTime = 0;
                _energyShield.gameObject.SetActive(false);
            }
        }

        //уменьшение времени заморозки камней
        if (_froizenTime > 0)
        {
            _froizenTime -= Time.deltaTime;
            if (_froizenTime <= 0)
            {
                //отключить заморозку камней
                _froizenTime = 0;
                Globals.Instance.WarmStones();
            }
        }
    }

    /// <summary>
    /// Проверка на столкновение с игровыми объектами.
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //проверить, не подобрали ли мы щит?
        if (collision.transform.root.GetComponent<Shield>() != null)
        {
            //включить энергощит на 5 секунд
            _shieldTime = 5;
            _energyShield.gameObject.SetActive(true);
        }

        //проверить, не подобрали ли мы снежинку?
        if (collision.transform.root.GetComponent<Snowflake>() != null)
        {
            //включить заморозку на 5 секунд
            if(_froizenTime <= 0) Globals.Instance.FreezeStones();
            _froizenTime = 5;
        }

        //если энергощит включен - столкновение с камнем не проверять
        if (_shieldTime > 0) return;

        //проверить на столкновение с камнем
        if(StoneCollision == null || collision.transform.root.GetComponent<Stone>() == null) return;
        StoneCollision.Invoke();
    }

    /// <summary>
    /// Обнуление танка.
    /// </summary>
    public void TankReset()
    {
        _shieldTime = 0f;
        _energyShield.gameObject.SetActive(false);
    }
}
