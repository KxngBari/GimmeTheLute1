using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo;
using UnityEngine.UI;

namespace GTL
{
    public class ButtonController : MonoBehaviour
    {
        public Color colorOfNotes;
        public SpriteRenderer targetColour;
        public KeyCode keyboardButton;
        public List<string> samePayload = new List<string>();

        List<KoreographyEvent> noteEvents = new List<KoreographyEvent>();
        Queue<NoteObject> trackedNotes = new Queue<NoteObject>();
        GTLController gtlController;

        float spawnY = 0;

        int nextEventID = 0;
        Vector3 defaultScale;
        float scaleNormal = 1f;
        float scalePress = 1.25f;
        float scaleHold = 1.15f;

        public AudioSource leftSound;
        public AudioSource downSound;
        public AudioSource upSound;
        public AudioSource rightSound;
        public AudioSource aSound;
        public AudioSource dSound;

        public Text noteValue;

        public Vector3 TargetPosition
        {
            get
            {
                return new Vector3(transform.position.x, transform.position.y);
            }
        }

        public void Initialize(GTLController controller)
        {
            gtlController = controller;
        }
        // Start is called before the first frame update
        void Start()
        {
            targetColour.color = colorOfNotes;
            spawnY = 10;
            defaultScale = targetColour.transform.localScale;
        }

        // Update is called once per frame
        void Update()
        {
            CheckSpawnNext();
            while (trackedNotes.Count > 0 && trackedNotes.Peek().IsNoteMissed())
            {
                trackedNotes.Dequeue();
            }
            if (Input.GetKeyDown(keyboardButton))
            {
                if (tag == "Left")
                {
                    leftSound.Play();
                    noteValue.text = "B";
                }
                if (tag == "Down")
                {
                    downSound.Play();
                    noteValue.text = "A#";
                }
                if (tag == "Up")
                {
                    upSound.Play();
                    noteValue.text = "F#";
                }
                if (tag == "Right")
                {
                    rightSound.Play();
                    noteValue.text = "F";
                }
                if (tag == "A")
                {
                    aSound.Play();
                    noteValue.text = "G#";
                }
                if (tag == "D")
                {
                    dSound.Play();
                    noteValue.text = "D#";
                }
                CheckNoteHit();
                AdjustScale(scalePress);
            }
            else if (Input.GetKey(keyboardButton))
            {
                AdjustScale(scaleHold);
            }
            else if (Input.GetKeyUp(keyboardButton))
            {
                AdjustScale(scaleNormal);
            }
        }
        void AdjustScale(float multiplier)
        {
            targetColour.transform.localScale = defaultScale * multiplier;
        }

        int GetSpawnSampleOffset()
        {
            float spawnDistToTarget = spawnY - transform.position.y;
            double spawnSecsToTarget = (double)spawnDistToTarget / (double)gtlController.noteSpeed;
            return (int)(spawnSecsToTarget * gtlController.SongSampleRate);
        }

        public void CheckNoteHit()
        {
            if (trackedNotes.Count > 0 && trackedNotes.Peek().IsNoteHittable())
            {
                NoteObject hitNote = trackedNotes.Dequeue();
                hitNote.OnHit();
            }
            else
            {
                gtlController.MissedNote();
            }
        }

        void CheckSpawnNext()
        {
            int samplesToTarget = GetSpawnSampleOffset();

            int currentTime = gtlController.CurrentSampleTime;

            while (nextEventID < noteEvents.Count &&
                   noteEvents[nextEventID].StartSample < currentTime + samplesToTarget)
            {
                KoreographyEvent evt = noteEvents[nextEventID];

                NoteObject newObj = gtlController.GetFreshNoteObject();
                newObj.Initialize(evt, colorOfNotes, this, gtlController);

                trackedNotes.Enqueue(newObj);

                nextEventID++;
            }
        }

        public void AddEventToLane(KoreographyEvent evt)
        {
            noteEvents.Add(evt);
        }

        public bool DoesMatchPayload(string payload)
        {
            bool bMatched = false;

            for (int i = 0; i < samePayload.Count; ++i)
            {
                if (payload == samePayload[i])
                {
                    bMatched = true;
                    break;
                }
            }
            return bMatched;
        }
    }
}