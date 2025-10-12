using KH;
using UnityEngine;
[CreateAssetMenu(fileName = "BulletType", menuName = "Bullet/Bullet Type")]
public class BulletType : ScriptableObject
{
    [Header("Bullet Data")]
    public int bulletID = 0;
    public ColliderType colliderType;
    public ColliderType grazeColliderType;
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

    [Header("Box Graze Collider")]
    public Vector2 grazeBoxSize = Vector2.one * 1.5f;
    public Vector2 grazeBoxOffset = Vector2.zero;

    [Header("Circle Graze Collider")]
    public float grazeCircleRadius = 0.75f;
    public Vector2 grazeCircleOffset = Vector2.zero;

    [Header("Capsule Graze Collider")]
    public Vector2 grazeCapsuleSize = new Vector2(0.75f, 1.5f);
    public Vector2 grazeCapsuleOffset = Vector2.zero;
    public CapsuleDirection2D grazeCapsuleDirection = CapsuleDirection2D.Vertical;
}
