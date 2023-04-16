using DefaultNamespace.Core.Audio;
using UnityEngine;

namespace Core3.SciptableObjects
{
    [CreateAssetMenu(fileName = "newAudioCue", menuName = "RCR/ScriptableObject/Audio Cue", order = 0)]
    public class AudioCueSO : BaseScriptableObject
    {
        public bool looping = false;
        [SerializeField] private AudioClipsGroup[] _audioClipGroups = default;

        public AudioClip[] GetClips()
        {
            int numberOfClips = _audioClipGroups.Length;
            AudioClip[] resultingClips = new AudioClip[numberOfClips];

            for (int i = 0; i < numberOfClips; i++)
            {
                resultingClips[i] = _audioClipGroups[i].GetNextClip();
            }

            return resultingClips;
        }
    }
}