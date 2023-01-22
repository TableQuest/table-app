using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class PlaySoundOnStart: MonoBehaviour
    {
        [SerializeField] private AudioClip _clip;
        
        
        private void Start()
        {
            SoundManager.Instance.PlaySound(_clip);
        }
    }
}