using UnityEngine;
namespace KH
{
    public interface ILuminousZone
    {
        bool ContainsPoint(Vector2 point);
        float Intensity { get; }
        Color ZoneColor { get; }
    }
}
