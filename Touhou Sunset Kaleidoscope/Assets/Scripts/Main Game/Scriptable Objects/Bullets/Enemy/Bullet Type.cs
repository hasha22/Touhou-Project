using KH;
using UnityEngine;
[CreateAssetMenu(fileName = "BulletType", menuName = "Bullet/Bullet Type")]
public class BulletType : ScriptableObject
{
    [Header("Bullet Data")]
    public ColliderType colliderType;
    public Sprite sprite;
    public Vector2 colliderSize = Vector2.one;
    public Vector2 colliderOffset = Vector2.zero;

    [Header("Box Collider")]
    public Vector2 boxSize = Vector2.one;
    public Vector2 boxOffset = Vector2.zero;

    [Header("Circle Collider")]
    public float circleRadius = 0.5f;
    public Vector2 circleOffset = Vector2.zero;

    [Header("Capsule Collider")]
    public Vector2 capsuleSize = new Vector2(0.5f, 1f);
    public Vector2 capsuleOffset = Vector2.zero;
    public CapsuleDirection2D capsuleDirection = CapsuleDirection2D.Vertical;
}
