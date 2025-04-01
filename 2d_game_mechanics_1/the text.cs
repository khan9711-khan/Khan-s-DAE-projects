using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public GameObject levelCompleteDialog;
    public GameObject finalWinDialog;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void ShowLevelComplete()
    {
        levelCompleteDialog.SetActive(true);
    }

    public void HideLevelComplete()
    {
        levelCompleteDialog.SetActive(false);
    }

    public void ShowFinalWin()
    {
        finalWinDialog.SetActive(true);
    }

    public void HideFinalWin()
    {
        finalWinDialog.SetActive(false);
    }
}

