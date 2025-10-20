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
            case ColliderType.Polygon:
                var poly = previewBullet.AddComponent<PolygonCollider2D>();
                poly.points = bulletType.polygonPoints.ToArray();
                poly.offset = bulletType.polygonOffset;
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

            // Draw hitbox collider (green)
            DrawColliderOutline(spriteRect, bulletType, false);

            // Draw graze collider (yellow)
            DrawColliderOutline(spriteRect, bulletType, true);
        }
    }

    private void DrawColliderOutline(Rect spriteRect, BulletType bulletType, bool isGraze)
    {
        Vector2 spriteCenter = new Vector2(spriteRect.x + spriteRect.width / 2, spriteRect.y + spriteRect.height / 2);

        // Green for hitbox, yellow for graze
        Handles.color = isGraze ? new Color(1, 1, 0, 1) : new Color(0, 1, 0, 1);

        ColliderType type = isGraze ? bulletType.grazeColliderType : bulletType.colliderType;

        switch (type)
        {
            case ColliderType.Box:
                DrawBoxColliderPreview(spriteRect, bulletType, isGraze);
                break;

            case ColliderType.Circle:
                DrawCircleColliderPreview(spriteCenter, spriteRect, bulletType, isGraze);
                break;

            case ColliderType.Capsule:
                DrawCapsuleColliderPreview(spriteCenter, spriteRect, bulletType, isGraze);
                break;
            case ColliderType.Polygon:
                DrawPolygonColliderPreview(spriteCenter, spriteRect, bulletType, isGraze);
                break;
        }
    }

    private void DrawBoxColliderPreview(Rect spriteRect, BulletType bulletType, bool isGraze)
    {
        Vector2 size = isGraze ? bulletType.grazeBoxSize : bulletType.boxSize;
        Vector2 offset = isGraze ? bulletType.grazeBoxOffset : bulletType.boxOffset;

        float colliderWidth = spriteRect.width * size.x;
        float colliderHeight = spriteRect.height * size.y;

        float offsetX = spriteRect.x + (spriteRect.width / 2) + (spriteRect.width * offset.x);
        float offsetY = spriteRect.y + (spriteRect.height / 2) - (spriteRect.height * offset.y);

        Rect colliderRect = new Rect(
            offsetX - colliderWidth / 2,
            offsetY - colliderHeight / 2,
            colliderWidth,
            colliderHeight
        );

        Color outlineColor = isGraze ? new Color(1, 1, 0, 1) : new Color(0, 1, 0, 1);
        Handles.DrawSolidRectangleWithOutline(colliderRect, new Color(outlineColor.r, outlineColor.g, outlineColor.b, 0.1f), outlineColor);
    }

    private void DrawCircleColliderPreview(Vector2 center, Rect spriteRect, BulletType bulletType, bool isGraze)
    {
        Sprite sprite = bulletType.sprite;
        float spritePixelWidth = sprite.bounds.size.x;
        float spritePixelHeight = sprite.bounds.size.y;

        float radius = isGraze ? bulletType.grazeCircleRadius : bulletType.circleRadius;
        Vector2 offset = isGraze ? bulletType.grazeCircleOffset : bulletType.circleOffset;

        Vector2 offsetPos = new Vector2(
            center.x + (spriteRect.width * offset.x),
            center.y - (spriteRect.height * offset.y)
        );

        float radiusPixels = Mathf.Max(spriteRect.width, spriteRect.height) * radius;

        Handles.DrawWireDisc(offsetPos, Vector3.forward, radiusPixels);
        Color outlineColor = isGraze ? new Color(1, 1, 0, 1) : new Color(0, 1, 0, 1);
        Handles.color = new Color(outlineColor.r, outlineColor.g, outlineColor.b, 0.1f);
        Handles.DrawSolidDisc(offsetPos, Vector3.forward, radiusPixels);
    }


    private void DrawCapsuleColliderPreview(Vector2 center, Rect spriteRect, BulletType bulletType, bool isGraze)
    {
        Sprite sprite = bulletType.sprite;
        float spritePixelWidth = sprite.bounds.size.x;
        float spritePixelHeight = sprite.bounds.size.y;

        Vector2 size = isGraze ? bulletType.grazeCapsuleSize : bulletType.capsuleSize;
        Vector2 offset = isGraze ? bulletType.grazeCapsuleOffset : bulletType.capsuleOffset;
        CapsuleDirection2D direction = isGraze ? bulletType.grazeCapsuleDirection : bulletType.capsuleDirection;

        float width = spriteRect.width * size.x;
        float height = spriteRect.height * size.y;

        Vector2 offsetPos = new Vector2(
            center.x + (spriteRect.width * offset.x),
            center.y - (spriteRect.height * offset.y)
        );

        float radius = Mathf.Min(width, height) / 2;
        Color outlineColor = isGraze ? new Color(1, 1, 0, 1) : new Color(0, 1, 0, 1);
        Handles.color = outlineColor;

        if (direction == CapsuleDirection2D.Vertical)
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
    private void DrawPolygonColliderPreview(Vector2 center, Rect spriteRect, BulletType bulletType, bool isGraze)
    {
        var points = isGraze ? bulletType.grazePolygonPoints : bulletType.polygonPoints;
        Vector2 offset = isGraze ? bulletType.grazePolygonOffset : bulletType.polygonOffset;
        if (points == null || points.Count < 3) return;

        Vector3[] scaledPoints = new Vector3[points.Count + 1];
        for (int i = 0; i < points.Count; i++)
        {
            float x = center.x + spriteRect.width * (points[i].x + offset.x);
            float y = center.y - spriteRect.height * (points[i].y + offset.y);
            scaledPoints[i] = new Vector3(x, y, 0);
        }
        scaledPoints[points.Count] = scaledPoints[0];
        Handles.DrawPolyLine(scaledPoints);
    }
}
#endif