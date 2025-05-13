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
    public GameObject targetPrefab;
    public Transform targetSpawnPoint;
    public GameObject applePrefab;

    public TextMeshProUGUI resultText;
    public TextMeshProUGUI progressText;

    private GameObject currentTarget;
    private GameObject currentApple;
    private TextMeshProUGUI levelCompleteDialog;
    private TextMeshProUGUI finalWinDialog;

    private int knifeCount = 0;
    private int currentLevel = 1;
    private int knivesNeeded = 5;
    private bool canThrow = true;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        CreateUIElements();

        if (resultText == null)
            resultText = CreateUIText("ResultText", "Click to Throw!");

        if (progressText == null)
        {
            progressText = CreateUIText("ProgressText", "Progress: 0%");
            progressText.rectTransform.anchoredPosition = new Vector2(0, -200);
        }

        if (targetSpawnPoint == null)
        {
            GameObject found = GameObject.Find("TargetSpawnPoint");
            if (found != null) targetSpawnPoint = found.transform;
        }

        SpawnTarget();
        SpawnApple();
        UpdateProgressText();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && canThrow)
        {
            ThrowKnife();
        }
    }

    void FixedUpdate()
    {
        GameObject knife = GameObject.FindWithTag("Knife");
        if (knife != null)
        {
            Rigidbody2D rb = knife.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Debug.DrawRay(knife.transform.position, rb.linearVelocity, Color.red);
            }
        }
    }

    void ThrowKnife()
    {
        if (!knifePrefab || !knifeSpawnPoint) return;

        canThrow = false;
        GameObject knife = Instantiate(knifePrefab, knifeSpawnPoint.position, Quaternion.identity);
        SetSortingOrder(knife, 0); // knife in front

        Rigidbody2D rb = knife.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 0;
            rb.linearVelocity = Vector2.up * 10f;
        }
    }

    public void ShowResult(bool isHit, string message = "")
    {
        if (resultText == null) return;

        resultText.text = isHit ? "Well Done!" : "Try Again!";
        if (!string.IsNullOrEmpty(message)) resultText.text = message;

        if (!message.Contains("Game Over"))
            canThrow = true;
    }

    public void RegisterKnifeHit()
    {
        knifeCount++;
        UpdateProgressText();

        if (knifeCount >= knivesNeeded)
        {
            if (currentLevel == 1)
                StartCoroutine(LevelComplete());
            else
                StartCoroutine(FinalWin());
        }
        else
        {
            canThrow = true;
        }
    }

    void UpdateProgressText()
    {
        int totalNeeded = 13; // 5 in level 1 + 8 in level 2
        int progress = (currentLevel == 1) ? knifeCount : 5 + knifeCount;
        float percent = ((float)progress / totalNeeded) * 100f;

        if (progressText != null)
        {
            progressText.text = (percent >= 100)
                ? "Progress: 100% Completed!"
                : $"Progress: {percent:F0}%";
        }
    }

    IEnumerator LevelComplete()
    {
        if (levelCompleteDialog != null)
        {
            levelCompleteDialog.text = "This level is complete!";
            levelCompleteDialog.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(2);

        if (levelCompleteDialog != null)
            levelCompleteDialog.gameObject.SetActive(false);

        StartNextLevel();
    }

    IEnumerator FinalWin()
    {
        if (finalWinDialog != null)
        {
            finalWinDialog.text = "You completed everything!";
            finalWinDialog.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(2);

        if (finalWinDialog != null)
            finalWinDialog.gameObject.SetActive(false);

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
        UpdateProgressText();
    }

    void ClearKnives()
    {
        foreach (GameObject k in GameObject.FindGameObjectsWithTag("Knife"))
            Destroy(k);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void SpawnTarget()
    {
        if (!targetPrefab || !targetSpawnPoint) return;

        if (currentTarget != null)
            Destroy(currentTarget);

        currentTarget = Instantiate(targetPrefab, targetSpawnPoint.position, Quaternion.identity);
        SetSortingOrder(currentTarget, -5); // Target behind everything
    }

    void SpawnApple()
    {
        if (!applePrefab || !targetSpawnPoint) return;

        if (currentApple != null)
            Destroy(currentApple);

        Vector3 pos = targetSpawnPoint.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
        currentApple = Instantiate(applePrefab, pos, Quaternion.identity);
        SetSortingOrder(currentApple, 0); // Same layer as knife
    }

    void SetSortingOrder(GameObject obj, int order)
    {
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.sortingLayerName = "Default";
            sr.sortingOrder = order;
        }
    }

    void CreateUIElements()
    {
        GameObject canvas = GameObject.Find("Canvas");
        if (canvas == null)
        {
            canvas = new GameObject("Canvas", typeof(Canvas));
            canvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.AddComponent<CanvasScaler>();
            canvas.AddComponent<GraphicRaycaster>();
        }

        levelCompleteDialog = CreateUIText("LevelCompleteText", "");
        finalWinDialog = CreateUIText("FinalWinText", "");
        levelCompleteDialog.gameObject.SetActive(false);
        finalWinDialog.gameObject.SetActive(false);
    }

    TextMeshProUGUI CreateUIText(string name, string content)
    {
        GameObject obj = new GameObject(name);
        obj.transform.SetParent(GameObject.Find("Canvas").transform, false);

        TextMeshProUGUI tmp = obj.AddComponent<TextMeshProUGUI>();
        tmp.text = content;
        tmp.fontSize = 36;
        tmp.color = new Color(1f, 0.5f, 0f); // ✅ vibrant orange
        tmp.alignment = TextAlignmentOptions.Center;

        RectTransform rt = tmp.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(600, 100);
        rt.anchoredPosition = Vector2.zero;

        return tmp;
    }
}


















