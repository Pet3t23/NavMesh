using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneButton : MonoBehaviour
{
    // Este m�todo se llama cuando se hace clic en el bot�n
    public void ChangeScene(string sceneName)
    {
        // Carga la escena especificada por su nombre
        SceneManager.LoadScene(sceneName);
    }
}
