namespace SoundToy
{
    using UnityEngine;
    using UnityEditor;
    using System;

    using Random = UnityEngine.Random;

    public class SoundWaveWindow : EditorWindow
    {
        [System.NonSerialized] static bool _needReload = true;

        // bar margin
        const int _barMarginLeft = 7;
        const int _barMarginRight = 32;

        // sound bar box margin
        const float _boxMarginBottom = 12;

        // footer button margin
        const float _footerButtonMarginLeft = 8;
        const float _footerButtonMarginBottom = 4;

        // bar size
        const int _barWidth = 10;
        const int _barHeight = 64;
        Vector2 scrollPos = Vector2.zero;
        [SerializeField] Texture2D _bgTexture;
        [SerializeField] Texture2D _barTexture;
        [SerializeField] Texture2D _frameTexture;
        [SerializeField] Texture2D _windowBgTexture;
        [SerializeField] Color _windowColor;
        [SerializeField] Color _barColor = Color.white;
        [SerializeField] Color _bgColor = Color.white;
        [SerializeField] Color _frameColor = new Color(0f, 0f, 0f, 1f);
        [SerializeField] bool _needCreateColor = true;
        [SerializeField] private AudioWaveAsset _audioAsset;
        bool _isMousePress = false;
        bool _isOpenSettings = false;
        bool _isUpdateTextureEveryFrame = false;

        [MenuItem("Tools/Sound Wave Window")]
        static void Open()
        {
            GetWindow<SoundWaveWindow>("SoundWave Window");
        }

        [InitializeOnLoadMethod]
        static void OnLoad()
        {
            _needReload = true;
        }

        int cnt = 0;
        void Update()
        {
            if (cnt++ % 3 == 0)
                Repaint();
        }

        void OnFocus()
        {
            _needReload = true;
        }

        void CreateColor()
        {
            _barColor = "#D1D4E7".ToColor();
            _bgColor = "#585973".ToColor();
            _frameColor = "#020203".ToColor();
            _windowColor = "#393B50".ToColor();
        }

        void DrawHeader()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Refresh", EditorStyles.toolbarButton))
            {
                UpdateTexture();
            }
            _isOpenSettings = GUILayout.Toggle(_isOpenSettings, "Settings", EditorStyles.toolbarButton);

            EditorGUILayout.EndHorizontal();
        }

        private void UpdateTexture()
        {
            CreateTextureIfNull();

            // update texture
            _bgTexture.SetPixel(0, 0, _bgColor);
            _bgTexture.Apply();
            _barTexture.SetPixel(0, 0, _barColor);
            _barTexture.Apply();
            _frameTexture.SetPixel(0, 0, _frameColor);
            _frameTexture.Apply();
            _windowBgTexture.SetPixel(0, 0, _windowColor);
            _windowBgTexture.Apply();
        }

        private void CreateTextureIfNull()
        {
            // create texture
            if (_bgTexture == null) { _bgTexture = new Texture2D(1, 1); }
            if (_barTexture == null) { _barTexture = new Texture2D(1, 1); }
            if (_frameTexture == null) { _frameTexture = new Texture2D(1, 1); }
            if (_windowBgTexture == null) { _windowBgTexture = new Texture2D(1, 1); }
        }

        void OnGUI()
        {
            if (!MyStyles.IsInitialize)
            {
                MyStyles.Initialize();
            }

            if (_needCreateColor)
            {
                _needCreateColor = false;
                CreateColor();
                UpdateTexture();
            }

            CreateTextureIfNull();

            Graphics.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), _windowBgTexture);
            DrawHeader();

            if (_needReload)
            {
                _needReload = false;
                UpdateTexture();
            }

            if (_isUpdateTextureEveryFrame)
            {
                UpdateTexture();
            }


            if (_isOpenSettings)
            {
                DrawColorInput();
            }

            var defaultBgColor = GUI.backgroundColor;
            GUI.backgroundColor = _bgColor;
            EditorGUILayout.BeginVertical(MyStyles.InputFieldBoxStyle);
            {
                EditorGUILayout.LabelField("AudioAsset", EditorStyles.whiteLabel);
                _audioAsset = EditorGUILayout.ObjectField(_audioAsset, typeof(AudioWaveAsset), false) as AudioWaveAsset;
            }
            EditorGUILayout.EndVertical();
            GUI.backgroundColor = defaultBgColor; // reset color

            // draw bar
            DrawBarAll();

            GUILayout.FlexibleSpace();
        }

        private void DrawColorInput()
        {
            var defaultColor = GUI.backgroundColor;
            GUI.backgroundColor = _bgColor;
            EditorGUILayout.BeginVertical(MyStyles.InputFieldBoxStyle);
            {
                GUILayout.Space(-2f);

                // color input
                _barColor = CustomUI.WhiteColorField(nameof(_barColor), _barColor);
                _bgColor = CustomUI.WhiteColorField(nameof(_bgColor), _bgColor);
                _frameColor = CustomUI.WhiteColorField(nameof(_frameColor), _frameColor);
                _windowColor = CustomUI.WhiteColorField(nameof(_windowColor), _windowColor);
                GUILayout.Space(2);
                GUI.backgroundColor = defaultColor;
            }
            EditorGUILayout.EndVertical();
        }

        Rect _startRect;
        Rect _endRect;
        private void DrawBarAll()
        {
            if (_audioAsset == null) return;

            GUILayout.Space(1f);

            var defaultColor = GUI.backgroundColor;
            GUI.backgroundColor = _bgColor;
            GUILayout.BeginVertical(MyStyles.AudioBarBoxStyle);
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField($"WaveEditor({0}~{_audioAsset.GetSize() - 1})", EditorStyles.whiteLargeLabel);
                GUILayout.FlexibleSpace();
                // GUI.backgroundColor = Color.white;
                // if (GUILayout.Button("Reset", MyStyles.SoundResetMiniButton))
                // {
                //     _audioAsset.ResetValues();
                // }
                // GUI.backgroundColor = _bgColor;

                EditorGUILayout.EndHorizontal();
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

                int id = 0;

                GUILayout.BeginHorizontal();
                {
                    GUILayout.Space(_barMarginLeft);
                    for (int x = 0; x < _audioAsset.GetSize(); x++)
                    {
                        var rect = DrawBar(id); // draw bar

                        if (x % 5 == 0)
                        {
                            var style = new GUIStyle(EditorStyles.whiteLabel);
                            style.alignment = TextAnchor.UpperLeft;
                            rect.x -= 3;
                            rect.y += rect.height;
                            rect.width = 48;
                            rect.height = 16;
                            EditorGUI.LabelField(rect, x.ToString(), style);
                        }
                        id++;

                        if (id >= _audioAsset.GetSize()) { break; }
                    }
                    GUILayout.FlexibleSpace();
                    GUILayout.Space(_barMarginRight);
                }
                GUILayout.EndHorizontal();
                GUILayout.Space(_boxMarginBottom);


                GUILayout.Space(_footerButtonMarginBottom);
            }

            GUILayout.EndVertical();
            GUI.backgroundColor = defaultColor;

            EditorGUILayout.EndScrollView();
            DrawFooterButton();

        }

        int _audioFreq = 440;

        /// <summary>
        /// 波形エディタの下のボタンの描画
        /// </summary>
        private void DrawFooterButton()
        {
            var defaultColor = GUI.backgroundColor;
            GUI.backgroundColor = Color.white;

            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Space(_footerButtonMarginLeft);

                if (GUILayout.Button("Reset", MyStyles.SoundFooterButton))
                {
                    _audioAsset.ResetValues();
                }
                if (GUILayout.Button("Randomize", MyStyles.SoundFooterButton))
                {
                    float[] values = new float[_audioAsset.GetSize()];
                    for (int i = 0; i < values.Length; i++)
                    {
                        values[i] = SoundTable.RandomWave((float)i / SoundTable.fs);
                    }
                    _audioAsset.SetValues(values);
                }
                if (GUILayout.Button("Sin", MyStyles.SoundFooterButton))
                {
                    float[] values = new float[_audioAsset.GetSize()];
                    for (int i = 0; i < values.Length; i++)
                    {
                        values[i] = SoundTable.SinWave((float)i / SoundTable.fs);
                    }
                    _audioAsset.SetValues(values);
                }

                if (GUILayout.Button("Pulse", MyStyles.SoundFooterButton))
                {
                    float[] values = new float[_audioAsset.GetSize()];
                    for (int i = 0; i < values.Length; i++)
                    {
                        values[i] = SoundTable.PulseWave((float)i / SoundTable.fs);
                    }
                    _audioAsset.SetValues(values);
                }

                if (GUILayout.Button("Saw", MyStyles.SoundFooterButton))
                {
                    float[] values = new float[_audioAsset.GetSize()];
                    for (int i = 0; i < values.Length; i++)
                    {
                        values[i] = SoundTable.SawWave((float)i / SoundTable.fs);
                    }
                    _audioAsset.SetValues(values);
                }
                GUILayout.FlexibleSpace();
            }
            EditorGUILayout.EndHorizontal();
            GUI.backgroundColor = defaultColor; // reset color
        }

        private Rect DrawBar(int index)
        {
            var rect = GUILayoutUtility.GetRect(_barWidth, _barHeight);
            if (index == 0)
            {
                _startRect = rect;
            }
            else
            if (index == _audioAsset.GetSize() - 1)
            {
                _endRect = rect;
            }
            var touchRect = new Rect(rect);
            touchRect.height += 40;
            touchRect.y -= 10;

            bool isMouseTouchBar = touchRect.Contains(Event.current.mousePosition);

            float audioValue = _audioAsset.GetValue(index);
            if (isMouseTouchBar && Event.current.isMouse)
            {
                // edit
                float mousePosY = Event.current.mousePosition.y;
                audioValue = 1f - (mousePosY - rect.y) / rect.height;
                audioValue = Mathf.Clamp01(audioValue);
                _audioAsset.SetValue(index, audioValue);
            }

            // draw frame
            var frameRect = new Rect(rect);
            frameRect.width += 2;
            GUI.DrawTexture(frameRect, _frameTexture);

            rect.x += 2f;
            rect.y += 1.5f;
            rect.width -= 2f;
            rect.height -= 3;

            // draw bg
            GUI.DrawTexture(rect, _bgTexture);

            // draw bar
            rect.y += rect.height - rect.height * audioValue;
            rect.height *= audioValue;
            GUI.DrawTexture(rect, _barTexture);

            return rect;
        }

        void OnDestroy()
        {
            if (_bgTexture != null) DestroyImmediate(_bgTexture);
            if (_barTexture != null) DestroyImmediate(_barTexture);
            if (_frameTexture != null) DestroyImmediate(_frameTexture);
            if (_windowBgTexture != null) DestroyImmediate(_windowBgTexture);
        }
    }
}