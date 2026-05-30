using KH;
using UnityEngine;

public class PillarofLight : EnemyShotPattern
{
    [Header("Pillar of Light Settings")]
    public float rotation = 0f;
    public float timeBeforeActivation = 1f;
    public Transform spawnPoint;
}
