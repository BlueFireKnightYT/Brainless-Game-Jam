using TMPro;
using UnityEngine;

public class WaveTextHandler : MonoBehaviour
{
    EnemySpawner eS;
    public TextMeshProUGUI text;

    private void Start()
    {
        eS = GameObject.FindGameObjectWithTag("Spawner").GetComponent<EnemySpawner>();
    }

    private void Update()
    {
        text.text = (("Wave: ") + (eS.currentWaveCount + 1).ToString());
    }
}
