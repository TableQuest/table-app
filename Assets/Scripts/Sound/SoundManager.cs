using UnityEngine;

namespace DefaultNamespace
{
    public class SoundManager: MonoBehaviour
    {
        public static SoundManager Instance;

        [SerializeField]
        private AudioSource _musicSource, _effectsSource;
        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void PlaySound(AudioClip clip)
        {
            _effectsSource.PlayOneShot(clip);
        }

        public void PlayDelayed(AudioClip clip)
        {
            _effectsSource.PlayDelayed(1);
        }
    }
}