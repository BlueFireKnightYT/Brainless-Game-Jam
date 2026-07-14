using TMPro;
using UnityEngine;

public class TutorialHandler : MonoBehaviour
{
    public TextMeshProUGUI tutorialText;
    public string[] textMessages;
    int count = 0;

    public GameObject buildButton;

    public void AdvanceText()
    {
        count++;
        tutorialText.text = textMessages[count];

        if(count == 1)
        {
            buildButton.SetActive(true);
        }
    }
}
