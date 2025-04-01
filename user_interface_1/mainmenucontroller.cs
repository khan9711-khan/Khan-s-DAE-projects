using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    // Call this function from your Start Button's OnClick event
    public void LoadGameScene()
    {
        SceneManager.LoadScene("GameScene"); // 🔁 Replace "GameScene" with the name of your actual scene
    }
}
