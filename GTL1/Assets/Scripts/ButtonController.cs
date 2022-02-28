using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo;

namespace GTL
{
    public class ButtonController : MonoBehaviour
    {
        public Color colorOfNotes = Color.blue;
        public SpriteRenderer targetColour;
        public KeyCode keyboardButton;
        public List<string> payload = new List<string>();

        List<KoreographyEvent> arrowEvents = new List<KoreographyEvent>();
        // Start is called before the first frame update
        void Start()
        {
            targetColour.color = colorOfNotes;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}