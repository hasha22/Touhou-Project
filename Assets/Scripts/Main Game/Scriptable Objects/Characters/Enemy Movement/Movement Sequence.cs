using KH;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Movement/Movement Sequence")]
public class MovementSequence : ScriptableObject
{
    public List<MovementStep> movementSteps;
    public bool loopSequence = false;
}

[System.Serializable]
public class MovementStep
{
    public EnemyMovementPattern pattern;
    public float duration = 2f;
    public float delayBeforeNext = 0.5f;
}
