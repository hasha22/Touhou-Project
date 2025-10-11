#if UNITY_EDITOR
using KH;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BulletType))]
public class BulletTypeEditor : Editor
{
    private GameObject previewBullet;
    private SpriteRenderer previewSr;
    private Collider2D previewCollider;

    private void OnEnable()
    {
        CreatePreview();
    }

    private void OnDisable()
    {
        if (previewBullet != null)
            DestroyImmediate(previewBullet);
    }

    private void CreatePreview()
    {
        if (previewBullet != null)
            DestroyImmediate(previewBullet);

        previewBullet = new GameObject("BulletPreview");
        previewBullet.hideFlags = HideFlags.HideAndDontSave;

        previewSr = previewBullet.AddComponent<SpriteRenderer>();

        UpdatePreview();
    }

    private void UpdatePreview()
    {
        BulletType bulletType = (BulletType)target;

        if (previewBullet == null)
            CreatePreview();

        if (previewSr != null && bulletType.sprite != null)
            previewSr.sprite = bulletType.sprite;

        // Remove existing collider
        Collider2D[] existingColliders = previewBullet.GetComponents<Collider2D>();
        foreach (Collider2D col in existingColliders)
            DestroyImmediate(col);

        // Add appropriate collider type
        switch (bulletType.colliderType)
        {
            case ColliderType.Box:
                BoxCollider2D boxCol = previewBullet.AddComponent<BoxCollider2D>();
                boxCol.size = bulletType.boxSize;
                boxCol.offset = bulletType.boxOffset;
                break;

            case ColliderType.Circle:
                CircleCollider2D circleCol = previewBullet.AddComponent<CircleCollider2D>();
                circleCol.radius = bulletType.circleRadius;
                circleCol.offset = bulletType.circleOffset;
                break;

            case ColliderType.Capsule:
                CapsuleCollider2D capsuleCol = previewBullet.AddComponent<CapsuleCollider2D>();
                capsuleCol.size = bulletType.capsuleSize;
                capsuleCol.offset = bulletType.capsuleOffset;
                capsuleCol.direction = bulletType.capsuleDirection;
                break;
        }
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Collider Preview", EditorStyles.boldLabel);

        if (GUILayout.Button("Refresh Preview", GUILayout.Height(30)))
        {
            UpdatePreview();
        }

        // Draw the preview
        if (previewBullet != null)
        {
            Rect previewRect = EditorGUILayout.GetControlRect(GUILayout.Height(200));
            DrawPreview(previewRect);
        }
    }

    public override void DrawPreview(Rect rect)
    {
        BulletType bulletType = (BulletType)target;

        if (previewBullet == null || previewSr == null || previewSr.sprite == null)
        {
            GUI.Label(rect, "No sprite assigned", EditorStyles.centeredGreyMiniLabel);
            return;
        }

        // Draw background
        GUI.color = new Color(0.2f, 0.2f, 0.2f);
        GUI.DrawTexture(rect, Texture2D.whiteTexture);
        GUI.color = Color.white;

        // Draw sprite in center
        Sprite sprite = previewSr.sprite;
        if (sprite != null)
        {
            float aspectRatio = (float)sprite.texture.width / sprite.texture.height;
            float previewWidth = rect.height * aspectRatio;

            Rect spriteRect = new Rect(
                rect.x + (rect.width - previewWidth) / 2,
                rect.y,
                previewWidth,
                rect.height
            );

            Texture2D spriteTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(
                AssetDatabase.GetAssetPath(sprite.texture));

            GUI.DrawTexture(spriteRect, spriteTexture);

            // Draw collider outline
            DrawColliderOutline(spriteRect, bulletType);
        }
    }

    private void DrawColliderOutline(Rect spriteRect, BulletType bulletType)
    {
        Vector2 spriteCenter = new Vector2(spriteRect.x + spriteRect.width / 2, spriteRect.y + spriteRect.height / 2);

        Handles.color = new Color(0, 1, 0, 1);

        switch (bulletType.colliderType)
        {
            case ColliderType.Box:
                DrawBoxColliderPreview(spriteRect, bulletType);
                break;

            case ColliderType.Circle:
                DrawCircleColliderPreview(spriteCenter, spriteRect, bulletType);
                break;

            case ColliderType.Capsule:
                DrawCapsuleColliderPreview(spriteCenter, spriteRect, bulletType);
                break;
        }
    }

    private void DrawBoxColliderPreview(Rect spriteRect, BulletType bulletType)
    {
        Sprite sprite = bulletType.sprite;
        float spritePixelWidth = sprite.bounds.size.x;
        float spritePixelHeight = sprite.bounds.size.y;

        float colliderWidth = spriteRect.width * bulletType.boxSize.x;
        float colliderHeight = spriteRect.height * bulletType.boxSize.y;

        float offsetX = spriteRect.x + (spriteRect.width / 2) + (spriteRect.width * bulletType.boxOffset.x);
        float offsetY = spriteRect.y + (spriteRect.height / 2) - (spriteRect.height * bulletType.boxOffset.y);

        Rect colliderRect = new Rect(
            offsetX - colliderWidth / 2,
            offsetY - colliderHeight / 2,
            colliderWidth,
            colliderHeight
        );

        Handles.DrawSolidRectangleWithOutline(colliderRect, new Color(0, 1, 0, 0.1f), new Color(0, 1, 0, 1));
    }

    private void DrawCircleColliderPreview(Vector2 center, Rect spriteRect, BulletType bulletType)
    {
        Sprite sprite = bulletType.sprite;
        float spritePixelWidth = sprite.bounds.size.x;
        float spritePixelHeight = sprite.bounds.size.y;

        Vector2 offsetPos = new Vector2(
            center.x + (spriteRect.width * bulletType.circleOffset.x),
            center.y - (spriteRect.height * bulletType.circleOffset.y)
        );

        float radiusPixels = Mathf.Max(spriteRect.width, spriteRect.height) * bulletType.circleRadius;

        Handles.DrawWireDisc(offsetPos, Vector3.forward, radiusPixels);
        Handles.color = new Color(0, 1, 0, 0.1f);
        Handles.DrawSolidDisc(offsetPos, Vector3.forward, radiusPixels);
    }

    private void DrawCapsuleColliderPreview(Vector2 center, Rect spriteRect, BulletType bulletType)
    {
        Sprite sprite = bulletType.sprite;
        float spritePixelWidth = sprite.bounds.size.x;
        float spritePixelHeight = sprite.bounds.size.y;

        float width = spriteRect.width * bulletType.capsuleSize.x;
        float height = spriteRect.height * bulletType.capsuleSize.y;

        Vector2 offsetPos = new Vector2(
            center.x + (spriteRect.width * bulletType.capsuleOffset.x),
            center.y - (spriteRect.height * bulletType.capsuleOffset.y)
        );

        float radius = Mathf.Min(width, height) / 2;

        if (bulletType.capsuleDirection == CapsuleDirection2D.Vertical)
        {
            float halfHeight = height / 2 - radius;

            Handles.DrawWireDisc(offsetPos + Vector2.up * halfHeight, Vector3.forward, radius);
            Handles.DrawWireDisc(offsetPos - Vector2.up * halfHeight, Vector3.forward, radius);

            Handles.DrawLine(offsetPos + new Vector2(-radius, halfHeight), offsetPos + new Vector2(-radius, -halfHeight));
            Handles.DrawLine(offsetPos + new Vector2(radius, halfHeight), offsetPos + new Vector2(radius, -halfHeight));
        }
        else
        {
            float halfWidth = width / 2 - radius;

            Handles.DrawWireDisc(offsetPos + Vector2.right * halfWidth, Vector3.forward, radius);
            Handles.DrawWireDisc(offsetPos - Vector2.right * halfWidth, Vector3.forward, radius);

            Handles.DrawLine(offsetPos + new Vector2(halfWidth, -radius), offsetPos + new Vector2(halfWidth, radius));
            Handles.DrawLine(offsetPos + new Vector2(-halfWidth, -radius), offsetPos + new Vector2(-halfWidth, radius));
        }
    }
}
#endif