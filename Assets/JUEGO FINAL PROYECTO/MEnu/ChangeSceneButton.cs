using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneButton : MonoBehaviour
{
    // Este método se llama cuando se hace clic en el botón
    public void ChangeScene(string sceneName)
    {
        // Carga la escena especificada por su nombre
        SceneManager.LoadScene(sceneName);
    }
}
