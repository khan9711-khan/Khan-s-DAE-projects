using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject knifePrefab;
    public Transform knifeSpawnPoint;
    public TextMeshProUGUI resultText;
    public GameObject targetPrefab;
    public Transform targetSpawnPoint;
    public GameObject applePrefab;
    private GameObject currentTarget;
    private GameObject currentApple;

    private GameObject levelCompleteDialog;
    private GameObject finalWinDialog;

    private int knifeCount = 0;
    private int currentLevel = 1;
    private int knivesNeeded = 5;
    private bool canThrow = true;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        CreateUIElements(); // ✅ Now exists in the script

        if (targetSpawnPoint == null)
        {
            Debug.LogWarning("Target Spawn Point is not assigned! Trying to find it...");
            GameObject tempSpawnPoint = GameObject.Find("TargetSpawnPoint");
            if (tempSpawnPoint != null)
            {
                targetSpawnPoint = tempSpawnPoint.transform;
            }
            else
            {
                Debug.LogError("No Target Spawn Point found! Please assign one manually in the Inspector.");
            }
        }

        SpawnTarget();
        SpawnApple();

        if (resultText == null)
        {
            GameObject canvas = GameObject.Find("Canvas");
            resultText = CreateUIText(canvas.transform, "ResultText", "Click to Throw!").GetComponent<TextMeshProUGUI>(); // ✅ Fixed
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && canThrow)
        {
            ThrowKnife();
        }
    }

    void ThrowKnife()
    {
        if (knifePrefab == null || knifeSpawnPoint == null)
        {
            Debug.LogError("KnifePrefab or KnifeSpawnPoint is not assigned in GameManager!");
            return;
        }

        canThrow = false;
        GameObject knife = Instantiate(knifePrefab, knifeSpawnPoint.position, Quaternion.identity);
        Rigidbody2D knifeRigidbody = knife.GetComponent<Rigidbody2D>();

        if (knifeRigidbody != null)
        {
            knifeRigidbody.gravityScale = 0;
            knifeRigidbody.linearVelocity = new Vector2(0, 10f);
        }
    }

    public void ShowResult(bool isHit, string message = "")
    {
        if (resultText == null) return;

        resultText.text = isHit ? "Well Done!" : "Try Again!";
        if (!string.IsNullOrEmpty(message))
        {
            resultText.text = message;
        }

        if (!message.Contains("Game Over"))
        {
            canThrow = true;
        }
    }

    public void RegisterKnifeHit()
    {
        knifeCount++;
        if (knifeCount >= knivesNeeded)
        {
            if (currentLevel == 1)
            {
                StartCoroutine(LevelComplete());
            }
            else if (currentLevel == 2)
            {
                StartCoroutine(FinalWin());
            }
        }
        else
        {
            canThrow = true;
        }
    }

    IEnumerator LevelComplete()
    {
        if (levelCompleteDialog != null)
        {
            levelCompleteDialog.GetComponent<TextMeshProUGUI>().text = "This level is complete!";
            levelCompleteDialog.SetActive(true);
        }

        yield return new WaitForSeconds(2);

        levelCompleteDialog.SetActive(false);
        SpawnTarget();
        StartNextLevel();
    }

    IEnumerator FinalWin()
    {
        if (finalWinDialog != null)
        {
            finalWinDialog.GetComponent<TextMeshProUGUI>().text = "You completed everything!";
            finalWinDialog.SetActive(true);
        }

        yield return new WaitForSeconds(2);
        finalWinDialog.SetActive(false);

        if (currentTarget != null)
        {
            SpinningTarget targetScript = currentTarget.GetComponent<SpinningTarget>();
            if (targetScript != null)
            {
                StartCoroutine(targetScript.SpinLeftAndDisappear());
            }
        }

        yield return new WaitForSeconds(2);
        RestartGame();
    }

    void StartNextLevel()
    {
        currentLevel = 2;
        knifeCount = 0;
        knivesNeeded = 8;
        ClearKnives();
        resultText.text = "Level 2: Place 8 Knives!";
        canThrow = true;
    }

    void ClearKnives()
    {
        GameObject[] knives = GameObject.FindGameObjectsWithTag("Knife");
        foreach (GameObject knife in knives)
        {
            Destroy(knife);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void SpawnTarget()
    {
        if (targetPrefab == null || targetSpawnPoint == null)
        {
            Debug.LogError("TargetPrefab or TargetSpawnPoint is missing in GameManager!");
            return;
        }

        if (currentTarget != null)
        {
            Destroy(currentTarget);
        }

        currentTarget = Instantiate(targetPrefab, targetSpawnPoint.position, Quaternion.identity);
    }

    void SpawnApple()
    {
        if (applePrefab == null) return;

        if (currentApple != null)
        {
            Destroy(currentApple);
        }

        Vector3 applePosition = new Vector3(
            targetSpawnPoint.position.x + Random.Range(-1.5f, 1.5f),
            targetSpawnPoint.position.y + Random.Range(-1.5f, 1.5f),
            0
        );

        currentApple = Instantiate(applePrefab, applePosition, Quaternion.identity);
    }

    // ✅ Fix: Now creates UI elements correctly
    void CreateUIElements()
    {
        GameObject canvas = GameObject.Find("Canvas");
        if (canvas == null)
        {
            canvas = new GameObject("Canvas");
            Canvas c = canvas.AddComponent<Canvas>();
            c.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.AddComponent<CanvasScaler>();
            canvas.AddComponent<GraphicRaycaster>();
        }

        levelCompleteDialog = CreateUIText(canvas.transform, "LevelCompleteText", "This level is complete!");
        finalWinDialog = CreateUIText(canvas.transform, "FinalWinText", "You completed everything!");
    }

    // ✅ Fix: Now creates UI text correctly
    GameObject CreateUIText(Transform parent, string name, string message)
    {
        GameObject textObj = new GameObject(name);
        textObj.transform.SetParent(parent, false);
        TextMeshProUGUI text = textObj.AddComponent<TextMeshProUGUI>();

        text.text = message;
        text.fontSize = 36;
        text.alignment = TextAlignmentOptions.Center;
        text.color = Color.white;

        RectTransform rectTransform = textObj.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = Vector2.zero;
        rectTransform.sizeDelta = new Vector2(600, 200);

        textObj.SetActive(false);
        return textObj;
    }
}














