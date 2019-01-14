namespace SoundToy
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [RequireComponent(typeof(AudioSource))]
    public class AudioWavePlayer : MonoBehaviour
    {
        [SerializeField] AudioWaveAsset audioAsset;
        int bufferReadPos = 0;

        void OnAudioFilterRead(float[] data, int channels)
        {
            Debug.Log("OnAudioFilterRead");
            int dst = 0;
            while (dst < data.Length)
            {
                float soundValue = audioAsset.GetValue(bufferReadPos++) * 2f - 1f;
                for (int i = 0; i < channels; i++)
                {
                    data[dst++] = soundValue;
                }

                if (bufferReadPos == audioAsset.GetSize())
                {
                    bufferReadPos = 0;
                }
            }
        }
    }
}