namespace SoundToy
{
    using UnityEngine;
    using UnityEditor;

    public static class CustomUI
    {
        public static Color WhiteColorField(string label, Color color)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label, EditorStyles.whiteLabel);
            color = EditorGUILayout.ColorField(color);
            EditorGUILayout.EndHorizontal();
            return color;
        }
    }
}