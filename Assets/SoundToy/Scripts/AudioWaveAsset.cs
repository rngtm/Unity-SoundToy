namespace SoundToy
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public interface IAudioAsset
    {
        int GetSize();
        float GetValue(int index);
        void SetValue(int index, float value);
        void Resize(int size);
    }

    [CreateAssetMenu(menuName = "SoundTool/AudioWaveAsset", fileName = "AudioWaveAsset.asset")]
    public class AudioWaveAsset : ScriptableObject, IAudioAsset
    {
        [SerializeField] float[] _data = new float[0];

        public int GetSize()
        {
            return _data.Length;
        }

        public float GetValue(int index)
        {
            return _data[index];
        }

        public void Resize(int size)
        {
            _data = new float[size];
            ResetValues();
        }

        public void SetValue(int index, float value)
        {
            _data[index] = value;
        }

        public void ResetValues()
        {
            for (int i = 0; i < _data.Length; i++)
            {
                _data[i] = 0.5f;
            }
        }

        public void SetValues(float[] values)
        {
            for (int i = 0; i < _data.Length; i++)
            {
                _data[i] = values[i];
            }
        }
    }

}