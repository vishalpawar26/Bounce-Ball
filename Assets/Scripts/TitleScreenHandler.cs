using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenHandler : MonoBehaviour
{
    private int sceneIndex = 1;
    public void ExitGame() => Application.Quit();

    public void StartGame() => SceneManager.LoadScene(sceneIndex);
}
