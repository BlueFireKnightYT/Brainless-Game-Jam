using UnityEngine;

[CreateAssetMenu(menuName = "Waves/Wave Data")]
public class EnemyWaveData : ScriptableObject
{
    public EnemySpawner.EnemyWave[] waves;
}