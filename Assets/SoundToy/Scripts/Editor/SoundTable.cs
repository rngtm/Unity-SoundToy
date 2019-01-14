namespace SoundToy
{
    using UnityEngine;

    public static class SoundTable
    {
        public const int fs = 48000; // サンプリング周波数
        const float _freq = 440;
        public static float SinWave(float time)
        {
            return Mathf.Sin(2f * Mathf.PI * _freq * time) * 0.5f + 0.5f;
        }

        public static float RandomWave(float time)
        {
            return Random.value;
        }

        public static float SawWave(float time)
        {
            return (time * _freq) % 1f;
        }

        public static float PulseWave(float time)
        {
            if ((time * _freq) % 1f < 0.5f)
            {
                return 0f;
            }
            else
            {
                return 1f;
            }
        }
    }
}