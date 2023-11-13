using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Made by Einar Hallik
namespace MainGame.Audio
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }
        
        [SerializeField] bool debug;
        [SerializeField] AudioObject[] tracks;
        
        Hashtable m_JobTable;
        HashSet<AudioType> allAddedAudioClips = new HashSet<AudioType>();
        class AudioJob
        {
            public readonly AudioAction Action;
            public readonly AudioType Type;
            public readonly AudioSource Source;
            public AudioJob(AudioAction action, AudioType type, AudioSource source)
            {
                Action = action;
                Type = type;
                Source = source;
            }
        }
        
        [Serializable]
        public class AudioObject
        {
            [SerializeField] AudioType type;
            [SerializeField] AudioClip clip;
            
            [SerializeField] [Range(0, 10)] float fade;
            [SerializeField] [Range(0, 10)] float delay;
            
            [SerializeField] [Range(0, 1)] float volume =1;
            [SerializeField] [Range(0, 1)] float pitch = 0.5f;
            [SerializeField] bool playOnAwake = false;
            [SerializeField] bool loop = false;
            
            public bool ShouldFade() => Fade > 0.0f;
            public float Fade => fade;
            public float Delay => delay;
            public AudioType Type => type;
            public AudioClip Clip => clip;

            public float Volume => volume;
            public float Pitch => pitch;
            public bool PlayOnAwake => playOnAwake;
            public bool Loop => loop;

        }

        
        [Serializable]
        public struct AudioTrack
        {
            [SerializeField] AudioObject[] audio;
            
            public AudioObject[] Audio => audio;
        }
        
        enum AudioAction
        {
            START,
            STOP,
            RESTART
        }

        #region UnityFuntions
        
        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }
            Configure();
        }

        void OnDisable()
        {
            Dispose();
        }

        #endregion

        #region Public functions

        

        public void PlayAudio(AudioType type, AudioSource source)
        {
            if (!source) return;
            AddJob(new AudioJob(AudioAction.START, type, source));

        }
        
        public void StopAudio(AudioType type, AudioSource source)
        {
            if (!source) return;
            AddJob(new AudioJob(AudioAction.STOP, type,source));
        }

        public void RestartAudio(AudioType type, AudioSource source)
        {
            if (!source) return;
            AddJob(new AudioJob(AudioAction.RESTART, type,source));
        }

        #endregion

        void Configure()
        {
            Instance = this;
            m_JobTable = new Hashtable();
            foreach (var track in tracks)
            {
                allAddedAudioClips.Add(track.Type);
            }
        }

        void Dispose()
        {
            foreach (DictionaryEntry entry in m_JobTable)
            {
                IEnumerator job = (IEnumerator)entry.Value;
                StopCoroutine(job);
            }
        }
        

        IEnumerator RunAudioJob(AudioJob audioJob)
        {
            if (audioJob.Source == null)
            {
                LogWarning($"No audio source has been found");
                yield break;
            }

            if (!allAddedAudioClips.Contains(audioJob.Type))
            {
                LogWarning($"You are trying to play {audioJob.Type} that has not been added in Audio manager");
                yield break;
            }
            audioJob.Source.clip = GetAudioClipFromAudioTrack(audioJob.Type, out AudioObject audioObject);
            yield return new WaitForSeconds(audioObject.Delay);
            
            
            audioJob.Source.volume = audioObject.Volume;
            audioJob.Source.playOnAwake = audioObject.PlayOnAwake;
            audioJob.Source.pitch = audioObject.Pitch;
            audioJob.Source.loop = audioObject.Loop;
            
            switch (audioJob.Action)
            {
                case AudioAction.START:
                    audioJob.Source.Play();
                    break;
                case AudioAction.STOP:
                    if (!audioObject.ShouldFade())
                    {
                        audioJob.Source.Stop();
                    }

                    break;
                case AudioAction.RESTART:
                    audioJob.Source.Stop();
                    audioJob.Source.Play();
                    break;
            }

            if (audioObject.ShouldFade())
            {
                float initialValue = audioJob.Action == AudioAction.START || audioJob.Action == AudioAction.RESTART ? 0.0f : 1.0f;
                float target = initialValue == 0 ? 1 : 0;
                float duration = audioObject.Fade;
                float timer = 0.0f;
                while (timer <= duration)
                {
                    audioJob.Source.volume = Mathf.Lerp(initialValue, target, timer / duration);
                    timer += Time.deltaTime;
                    yield return null;
                }

                if (audioJob.Action == AudioAction.STOP)
                {
                    audioJob.Source.Stop();
                }
            }

            m_JobTable.Remove(audioJob.Type);
        }

        AudioClip GetAudioClipFromAudioTrack(AudioType audioJobType, out AudioObject chosenAudioObject)
        {
            foreach (AudioObject audioObject in tracks)
            {
                if (audioObject.Type == audioJobType)
                {
                    chosenAudioObject = audioObject;
                    return audioObject.Clip;
                }
            }
            LogWarning("Chosen audioObject is defualt");
            chosenAudioObject = default;
            return null;
        }

        void AddJob(AudioJob audioJob)
        {
            //RemoveConflictingJob(audioJob);
            //Start job
            IEnumerator jobRunner = RunAudioJob(audioJob);
            if (m_JobTable.ContainsKey(audioJob))
            {
            
                IEnumerator runningJob = (IEnumerator)m_JobTable[audioJob];
                StopCoroutine(runningJob);
            }
            m_JobTable.Add(audioJob, jobRunner);
            StartCoroutine(jobRunner);
        }


        // void RemoveConflictingJob(AudioJob audioJob)
        // {
        //     if (m_JobTable.ContainsKey(audioJob))
        //     {
        //         RemoveJob(audioJob);
        //     }
        //
        //     AudioType conflictAudio = AudioType.None;
        //     foreach (DictionaryEntry entry in m_JobTable)
        //     {
        //         AudioType audioType = (AudioType)entry.Key;
        //         AudioTrack audioTrackInUse = (AudioTrack)m_AudioTable[audioType];
        //         AudioTrack audioTrackNeeded = (AudioTrack)m_AudioTable[audioJob];
        //         // if (audioTrackNeeded.Source == audioTrackInUse.Source)
        //         // {
        //         //     conflictAudio = audioType;
        //         // }
        //     }
        //
        //     // if (conflictAudio != AudioType.None)
        //     // {
        //     //     RemoveJob(conflictAudio);
        //     // }
        // }

        void RemoveJob(AudioJob type)
        {
            if (!m_JobTable.ContainsKey(type))
            {
                LogWarning($"$Trying to stop a job {type} that is not running");
                return;
            }

            IEnumerator runningJob = (IEnumerator)m_JobTable[type];
            StopCoroutine(runningJob);
            m_JobTable.Remove(type);
        }
        
        void LogWarning(string message)
        {
            if (!debug) return;
            Debug.LogWarning("[Audio Controller] " + message);
        }
    }
}