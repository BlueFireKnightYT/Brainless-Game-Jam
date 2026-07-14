using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialHandler : MonoBehaviour
{
    public TextMeshProUGUI[] tutorialText;
    public string[] textMessages;
    int count = 0;
    bool canContinue = true;

    public GameObject normalPanel;
    public GameObject buildPanel;

    public GameObject buildButton;
    public EnemySpawner enemySpawner;

    public GameObject returnButton;

    public void AdvanceText()
    {
        if (canContinue)
        count++;
        foreach(TextMeshProUGUI text in tutorialText)
        {
            text.text = textMessages[count];
        }
        

        if(count == 1)
        {
            buildButton.SetActive(true);
            canContinue = false;
        }
        if(count == 4)
        {
            normalPanel.SetActive(false);
            enemySpawner.enabled = true;
            canContinue = false;
        }
    }

    public void PressedBuild()
    {
        if(count == 1)
        {
            normalPanel.SetActive(false);
            buildPanel.SetActive(true);
            count++;
            foreach (TextMeshProUGUI text in tutorialText)
            {
                text.text = textMessages[count];
            }
        }
    }

    public void PressedFinish()
    {
        if(count == 2)
        {
            normalPanel.SetActive(true);
            buildPanel.SetActive(false);
            count++;
            canContinue = true;
            foreach (TextMeshProUGUI text in tutorialText)
            {
                text.text = textMessages[count];
            }
        }
    }

    private void Update()
    {
        bool finished = false;
        if(enemySpawner.currentWaveCount == 1 && !finished)
        {
            normalPanel.SetActive(true);
            foreach (TextMeshProUGUI text in tutorialText)
            {
                text.text = textMessages[count];
            }
            finished = true;
            returnButton.SetActive(true);
        }
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
