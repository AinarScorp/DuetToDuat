using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

//Made by Einar Hallik
namespace MainMenu.UI
{
    public enum Section
    {
        None, Start, Settings, PlayerConnection,IntroSection, Credits
    }
    public class MainMenuSection : MonoBehaviour
    {
        [SerializeField] Section section = Section.None;
        
        public Section Section => section;
        public virtual void SetEnabled(bool turnOn)
        {
            this.gameObject.SetActive(turnOn);

        }
        
    }
    
}
