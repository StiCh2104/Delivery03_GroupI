using UnityEngine;
using UnityEngine.Android;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;

public enum GameScene { Start, Gameplay, Ending }
public class Manager  : MonoBehaviour 
{
    public static Manager Instance { get; private set; }

    public GameScene CurrentState { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
        }


    private void Start()
    {
        ChangeState(GameScene.Start);
    }

    private void ChangeState(GameScene newScene)
    {
        CurrentState = newScene;
        Debug.Log("Cambio de escena a: " + CurrentState);
        switch (newScene)
        {
            case GameScene.Start:
                SceneManager.LoadScene("Title");
                break;
            case GameScene.Gameplay:
                SceneManager.LoadScene("Gameplay");
                break;
            case GameScene.Ending:
                SceneManager.LoadScene("Ending");
                break;
            default: SceneManager.LoadScene("Title");//def
                break;
        }
    }
    //procesors to change scenes (call)
    public void StartGame() { ChangeState(GameScene.Gameplay); }
    public void GoToMainMenu() { ChangeState(GameScene.Start); }
    public void EndGame() { ChangeState(GameScene.Ending); }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
        if (Input.GetKeyDown(KeyCode.Return))
            ChangeState(GameScene.Gameplay);
    }
    public void ExitGame()
    {
        Application.Quit();
    }

}
