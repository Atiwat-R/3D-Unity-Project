using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void BackToMenu() {
        SceneManager.LoadScene("MainMenu");
    }

    public void PlayGame() {
        SceneManager.LoadScene("MainScene");
    }

    public void StoryScene() {
        SceneManager.LoadScene("StoryScene");
    }

    public void QuitGame() {
        Application.Quit();
    } 
}
