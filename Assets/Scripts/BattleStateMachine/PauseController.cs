using System.Threading;
using UnityEngine;
using UnityEngine.Serialization;

public class PauseController : MonoBehaviour
{
    public GameObject pauseMenuPanel;
    [FormerlySerializedAs("muteButton")] public GameObject muteMusicButton;
    [FormerlySerializedAs("unmuteButton")] public GameObject unmuteMusicButton;
    public GameObject muteSfxButton;
    public GameObject unmuteSfxButton;
    private bool isPaused = false;
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

    public void MuteMusic()
    {
        AudioManager.Instance.MuteMusic();
        unmuteMusicButton.SetActive(true);
        muteMusicButton.SetActive(false);
    }
    
    public void UnmuteMusic()
    {
        AudioManager.Instance.UnmuteMusic();
        muteMusicButton.SetActive(true);
        unmuteMusicButton.SetActive(false);
    }
    
    public void MuteSfx()
    {
        AudioManager.Instance.MuteSfx();
        unmuteSfxButton.SetActive(true);
        muteSfxButton.SetActive(false);
    }
    
    public void UnmuteSfx()
    {
        AudioManager.Instance.UnmuteSfx();
        muteSfxButton.SetActive(true);
        unmuteSfxButton.SetActive(false);
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