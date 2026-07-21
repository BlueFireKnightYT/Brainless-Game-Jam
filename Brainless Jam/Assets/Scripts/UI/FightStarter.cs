using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FightStarter : MonoBehaviour
{
    public GameObject[] fightMarkers;

    public GameObject shopUI;

    private void Start()
    {

        for (int i = 0; i < fightMarkers.Length; i++)
        {
                if (WaveData.finished)
                {
                    if (i +1 <= WaveData.amountFinished)
                    {
                        print(WaveData.amountFinished);
                        fightMarkers[i].GetComponent<FightMark>().finished = true;
                        
                        fightMarkers[i].GetComponent<Image>().color = Color.lightGreen;
                        fightMarkers[i + 1].GetComponent<FightMark>().nextLevel = true;
                        fightMarkers[i + 1].GetComponent<Image>().color = Color.softRed;
                    }
                } 
        }
        WaveData.finished = false;
    }
    public void OnPressButton()
    {
        GameObject button = EventSystem.current.currentSelectedGameObject;

        FightMark fM = button.GetComponent<FightMark>();
        if (fM.nextLevel)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            WaveData.waveData = fM.waveData;
        }
    }

    public void EnableShopUI()
    {
        if (shopUI.activeSelf)
        {
            shopUI.SetActive(false);
        }
        else shopUI.SetActive(true);
    }
}
