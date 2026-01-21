using KH;
using UnityEngine;

[CreateAssetMenu(menuName = "Playable Character Data")]
public class PlayableCharacterData : ScriptableObject
{
    [Header("Playable Character Data")]
    public string characterName;
    public string characterDescription;
    public Sprite characterSprite;
    public int maxLives;
    public float startingPower;
    public float maxPower;

    [Header("Playable Character Movement")]
    public float flyingSpeed;
    public float precisionSpeed;
    public float speedLerp;

    [Header("Playable Character Shooting")]
    public ShotType shotType;
}
