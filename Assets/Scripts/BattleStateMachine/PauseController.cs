using System.Threading;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    public GameObject pauseMenuPanel;
    public GameObject muteButton;
    public GameObject unmuteButton;
    private bool isPaused = false;
    private bool isMuted = false;
    public void PauseOrResume()
    {
        if (isPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    private void Pause()
    {
        Time.timeScale = 0;
        pauseMenuPanel.SetActive(true);
        isPaused = true;
    }

    private void Resume()
    {
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;
    }

    public void Mute()
    {
        AudioManager.Instance.Mute();
        isMuted = true;
        unmuteButton.SetActive(true);
        muteButton.SetActive(false);
    }
    
    public void Unmute()
    {
        AudioManager.Instance.Unmute();
        isMuted = false;
        muteButton.SetActive(true);
        unmuteButton.SetActive(false);
    }
    
    public void Quit()
    {
#if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }
}