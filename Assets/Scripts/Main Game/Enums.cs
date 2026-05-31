using UnityEngine;
namespace KH
{
    public class Enums : MonoBehaviour
    {

    }
    public enum ColliderType
    {
        Box,
        Circle,
        Capsule,
        Polygon,
    }
    public enum DialogueSpeaker
    {
        Player,
        Boss
    }
    public enum PillarPatternType
    {
        Horizontal = 0,
        Vertical = 1,
        DiagonalLeft = 2,
        DiagonalRight = 3,
        Targeted = 4,
    }

    public enum EnemyType
    {
        Fairy,
        GreatFairy,
        SunflowerFairy,
        ZombieFairy,
        Kedama,
        Raven,
    }
    public enum LightZoneSize
    {
        Small,
        Medium,
        Large,
    }
    public enum ItemToSpawn
    {
        Power,
        GreatPower,
        FullPower,
        Score,
        GreatScore,
        Faith,
        StarFaith,
        OneUp,
    }
    public enum ItemType
    {
        Power,
        Score,
        Faith,
        OneUp,
    }
}
