using System.Collections;
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

        public IEnumerator PlayDelayed(AudioClip clip, int delay)
        {
            yield return new WaitForSeconds(delay);
            _effectsSource.PlayOneShot(clip);
            //_effectsSource.PlayDelayed(1);
        }
    }
}