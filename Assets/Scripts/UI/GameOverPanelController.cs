using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverPanelController : MonoBehaviour
{
    [SerializeField] private AudioClipSO defeatAudio;
    [SerializeField] private AudioClipSO victoryAudio;
    [SerializeField] private GameObject defeatPanel;
    [SerializeField] private GameObject victoryPanel;

    private void Start()
    {
        defeatPanel.SetActive(false);
        victoryPanel.SetActive(false);
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

    public void Replay()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void ShowDefeatPanel()
    {
        defeatPanel.SetActive(true);
        if(defeatAudio) AudioManager.Instance.PlayMusic(defeatAudio);
    }
    public void ShowVictoryPanel()
    {
        victoryPanel.SetActive(true);
        if(victoryAudio) AudioManager.Instance.PlayMusic(victoryAudio);
    }
}