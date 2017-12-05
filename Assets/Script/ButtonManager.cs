using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour {
    public void StartGameBtn(string StartGame)
    {
        SceneManager.LoadScene(StartGame);
    }
    public void ExitGameBtn()
    {
        Application.Quit();
    }
}