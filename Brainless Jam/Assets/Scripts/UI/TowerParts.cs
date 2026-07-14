using UnityEngine;

public class TowerParts : MonoBehaviour
{
    public static TowerParts Instance;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        Instance = this;
    }

    public int GetTowerAmount(string towerID)
    {
        return PlayerPrefs.GetInt(towerID, 0);
    }

    public void AddTower(string towerID, int amount)
    {
        int current = GetTowerAmount(towerID);
        PlayerPrefs.SetInt(towerID, current + amount);
        PlayerPrefs.Save();
    }

    public bool UnlockedTower(string towerID)
    {
        if (GetTowerAmount(towerID) > 0)
        {
            return true;
        }
        else return false;
    }
}
