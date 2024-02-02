using UnityEngine;
using UnityEngine.SceneManagement;

public class Restarter : MonoBehaviour
{
    [SerializeField] private KeyCode _restartKey = KeyCode.R;
    [SerializeField] private Starter _starter;
    private bool restart;
    
    //restarts the game to the initial state
    
    void Awake() {
        // DontDestroyOnLoad(this.gameObject);
        restart = false;
        Debug.Log("Restarter Start");
    }

    void Update() {
        if (Input.GetKeyDown(_restartKey) && !restart) {
            restart = true;  
            Debug.Log("Restarted");
            AudioManager.PlayRestartButtonPressed();
            EventManagerScript.Instance.restart(); //doing it because it's a singleton
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
