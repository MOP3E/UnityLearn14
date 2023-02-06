using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Вспомогательные методы для управления игровым уровнем.
/// </summary>
public class SceneHelper : MonoBehaviour
{
    /// <summary>
    /// Перезапуск уровня.
    /// </summary>
    public void LevelRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Загрузка выбранной сцены.
    /// </summary>
    public void LevelLoad(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    /// <summary>
    /// Выход из программы.
    /// </summary>
    public void Quit()
    {
        Application.Quit();
    }
}
