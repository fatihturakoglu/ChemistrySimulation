using UnityEngine;
using System.Collections.Generic;

public class ColorMixerService
{
    private readonly List<Color> _addedColors = new List<Color>();

    public void AddColor(Color color) => _addedColors.Add(color);
    public void Clear() => _addedColors.Clear();
    public int AddedColorCount => _addedColors.Count;

    public Color GetMixedColor()
    {
        if (_addedColors.Count == 0) return Color.white;
        float r = 0, g = 0, b = 0;
        foreach (var c in _addedColors) { r += c.r; g += c.g; b += c.b; }
        return new Color(r / _addedColors.Count, g / _addedColors.Count, b / _addedColors.Count);
    }
}