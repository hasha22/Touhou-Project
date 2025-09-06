using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attack/Attack Sequence")]
public class AttackSequence : ScriptableObject
{
    public List<PatternStep> steps;
    public bool loop = true;
}

[System.Serializable]
public class PatternStep
{
    public EnemyShotPattern pattern;
    public float delayAfter = 1f;
}
