using System;
using System.Collections;
using System.Collections.Generic;
using MainGame.Managers;
using UnityEngine;
using UnityEngine.Playables;

//Made by Einar Hallik
namespace MainGame.Cinematics
{
    public enum CinematicsEnum
    {
        NotSelected,StartingScene, FlyFirstDoor,BoatSwimsForBoarding, EndingBoat,
        MainMenuLoop, PlayerConnectionFlyTo, PlayerConnectionFlyToSpirit,MainMenuCredits
    }
    public class CinematicsManager : MonoBehaviour
    {
        [SerializeField] bool isMainLevel = true;
        [SerializeField] CinematicToPlay[] playableCinematics;
        [SerializeField] CinematicsEnum startingCinematics = CinematicsEnum.StartingScene;
        GameManager gameManager;
        PlayableDirector loopedDirector;
        
        [Serializable]
        public struct CinematicToPlay
        {
            [SerializeField] CinematicsEnum cinematic;
            [SerializeField] PlayableDirector playableDirector;

            public CinematicsEnum Cinematic => cinematic;

            public PlayableDirector PlayableDirector => playableDirector;
        }



        void Awake()
        {
            if (!isMainLevel)
            {
                return;
            }
            gameManager = GameObject.FindWithTag("GameManager")?.GetComponent<GameManager>();
            if (gameManager == null)
            {
                Destroy(this);
            }
        }

        void Start()
        {
            MakeCutscenesNeverPlayOnAwake();
            PlayCutscene(startingCinematics);
        }

        void MakeCutscenesNeverPlayOnAwake()
        {
            foreach (var playableCinematic in playableCinematics)
            {
                playableCinematic.PlayableDirector.playOnAwake = false;
            }
        }


        public PlayableDirector PlayCutscene(CinematicsEnum cinematicEnum)
        {
            if (cinematicEnum == CinematicsEnum.NotSelected)
            {
                Debug.LogError("CinematicsEnum is Not Selected");
                return null;
            }
            CinematicToPlay cinematicToPlay = GetCinematicToPlay(cinematicEnum);
            if (cinematicToPlay.PlayableDirector == null)
            {
                Debug.LogError("There is no playable Directors");
                return null;
            }
            loopedDirector?.Stop();

            cinematicToPlay.PlayableDirector.Play();
            
            loopedDirector = cinematicToPlay.PlayableDirector.extrapolationMode == DirectorWrapMode.Loop ? 
                cinematicToPlay.PlayableDirector : null;
            
            
            if (!isMainLevel)
            {
                return cinematicToPlay.PlayableDirector;
            }
            cinematicToPlay.PlayableDirector.stopped += _ =>
            {
                gameManager.ToggleCinematics(false);
            };
            gameManager.ToggleCinematics(true);
            return cinematicToPlay.PlayableDirector;
        }

        CinematicToPlay GetCinematicToPlay(CinematicsEnum cinematicEnum)
        {
            foreach (var playableCinematic in playableCinematics)
            {
                if (playableCinematic.Cinematic == cinematicEnum)
                {
                    return playableCinematic;
                }
            }
            Debug.LogError("Cinematic is not placed in playable Cinematics");
            return default;
        }
        
        
    }
    
}
