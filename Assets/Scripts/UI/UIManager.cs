using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("Game Over")]
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private AudioClip gameOverSound;

    [Header("Pause")]
    [SerializeField] private GameObject pauseScreen;

    private void Awake()
    {
        gameOverScreen.SetActive(false);
        pauseScreen.SetActive(false);
    }
    #region GAMEOVER
    public void GameOver()
    {
        gameOverScreen.SetActive(true);
        SoundManager.instance.PlaySound(gameOverSound);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Mainmenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; //Stop play mode in editor
        #endif
    }
    #endregion
    
    #region Pause

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            if (pauseScreen.activeInHierarchy)
                PauseGame(false);
            else
                PauseGame(true);
    }
    public void PauseGame (bool status)
    {
        pauseScreen.SetActive(status);

        Time.timeScale = status ? 0 : 1; //If status is true, set time scale to 0, otherwise set it to 1
    }

    public void SoundVolume()
    {
        SoundManager.instance.ChangeSoundVolume(0.2f); //Increase sound volume by 0.1        
    }

    public void MusicVolume()
    {
        SoundManager.instance.ChangeMusicVolume(0.2f); //Increase music volume by 0.1        
    }
        
    #endregion
}
