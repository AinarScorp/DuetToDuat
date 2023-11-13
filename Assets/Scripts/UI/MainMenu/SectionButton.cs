using System.Collections;
using System.Collections.Generic;
using MainGame.Cinematics;
using UnityEngine;

//Made by Einar Hallik

namespace MainMenu.UI
{
    public class SectionButton : MonoBehaviour
    {
        [SerializeField] Section sectionToActivate = Section.None;
        [SerializeField] CinematicsEnum cinematic = CinematicsEnum.NotSelected;
        public Section SectionToActivate => sectionToActivate;

        public CinematicsEnum Cinematic => cinematic;
    }
}
