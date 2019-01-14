namespace SoundToy
{
    using UnityEngine;
    using UnityEditor;

    public static class MyStyles
    {
        [System.NonSerialized] private static bool s_isInitialize = false;
        public static bool IsInitialize { get => s_isInitialize; private set => s_isInitialize = value; }

        private static GUIStyle _audioBarBoxStyle;
        public static GUIStyle AudioBarBoxStyle => _audioBarBoxStyle;

        private static GUIStyle _inputFieldBoxStyle;
        public static GUIStyle InputFieldBoxStyle => _inputFieldBoxStyle;

        private static GUIStyle _whiteObjectFieldStyle;
        public static GUIStyle WhiteObjectFieldStyle => _whiteObjectFieldStyle;

        private static GUIStyle _soundResetMiniButton;
        public static GUIStyle SoundResetMiniButton => _soundResetMiniButton;

        private static GUIStyle _soundFooterButton;
        public static GUIStyle SoundFooterButton => _soundFooterButton;
        public static void Initialize()
        {
            _audioBarBoxStyle = new GUIStyle(GUI.skin.box);
            _audioBarBoxStyle.margin.left = 4;
            _audioBarBoxStyle.margin.right = 4;
            _audioBarBoxStyle.padding.left = 3;
            _audioBarBoxStyle.padding.right = 5;

            _inputFieldBoxStyle = new GUIStyle(GUI.skin.box);
            _inputFieldBoxStyle.margin.left = 4;
            _inputFieldBoxStyle.margin.right = 4;
            _inputFieldBoxStyle.padding.left = 3;
            _inputFieldBoxStyle.padding.right = 5;

            _whiteObjectFieldStyle = new GUIStyle(EditorStyles.objectField);
            _whiteObjectFieldStyle.normal.textColor = Color.white;

            // mini button
            _soundResetMiniButton = new GUIStyle(EditorStyles.miniButton);
            _soundResetMiniButton.normal.textColor = Color.white;
            _soundResetMiniButton.margin.top = 3;
            _soundResetMiniButton.normal.textColor = Color.black;

            // mini button
            _soundFooterButton = new GUIStyle(GUI.skin.button);

            IsInitialize = true;
        }
    }
}