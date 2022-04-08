using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace GTL
{
    
    public class GTLController : MonoBehaviour
    {
        [EventID]
        public string eventID;

        public float hitWindowRangeInMS = 80;
        public float noteSpeed = 1;

        public NoteObject noteObjectBlueprint;
        public List<ButtonController> noteLanes = new List<ButtonController>();

        public AudioSource audioPlayback;
        public AudioSource gameOverSound;

        Koreography playingKoreoTrack;

        int hitWindowRangeInSamples;

        public Text scoreText;
        public static int score = 0;

        public Image progressBar;
        public static int multiplier = 1;
        int notesHit = 0;
        bool isPassing = true;
        bool gameOver = true;

        Stack<NoteObject> noteObjectPool = new Stack<NoteObject>();

        public int HitWindowWidth
        {
            get
            {
                return hitWindowRangeInSamples;
            }
        }

        public float WindowSizeInUnits
        {
            get
            {
                return noteSpeed * (hitWindowRangeInMS * 0.005f);
            }
        }

        public int SongSampleRate
        {
            get
            {
                return playingKoreoTrack.SampleRate;
            }
        }

        public int CurrentSampleTime
        {
            get
            {
                return playingKoreoTrack.GetLatestSampleTime();
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            audioPlayback.Play();
            score = 0;

            for (int i = 0; i < noteLanes.Count; ++i)
            {
                noteLanes[i].Initialize(this);
            }

            playingKoreoTrack = Koreographer.Instance.GetKoreographyAtIndex(0);

            KoreographyTrackBase rhythmTrack = playingKoreoTrack.GetTrackByID(eventID);
            List<KoreographyEvent> koreoEvents = rhythmTrack.GetAllEvents();

            for (int i = 0; i < koreoEvents.Count; ++i)
            {
                KoreographyEvent evt = koreoEvents[i];
                string payload = evt.GetTextValue();

                for (int j = 0; j < noteLanes.Count; ++j)
                {
                    ButtonController lane = noteLanes[j];
                    if (lane.DoesMatchPayload(payload))
                    {
                        lane.AddEventToLane(evt);
                        break;
                    }
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            UpdateInternalValues();

            if (audioPlayback.time > 109f)
            {
                SetHighScore();
                SceneManager.LoadScene("Game Over");
            }

            PlayGameOverSound();
        }

        void UpdateInternalValues()
        {
            hitWindowRangeInSamples = (int)(0.001f * hitWindowRangeInMS * SongSampleRate);
        }

        public NoteObject GetFreshNoteObject()
        {
            NoteObject retObj;

            if (noteObjectPool.Count > 0)
            {
                retObj = noteObjectPool.Pop();
            }
            else
            {
                retObj = GameObject.Instantiate<NoteObject>(noteObjectBlueprint);
            }

            retObj.gameObject.SetActive(true);
            retObj.enabled = true;

            return retObj;
        }

        public void ReturnNoteObjectToPool(NoteObject obj)
        {
            if (obj != null)
            {
                obj.enabled = false;
                obj.gameObject.SetActive(false);

                noteObjectPool.Push(obj);
            }
        }

        public void AddToScore()
        {
            progressBar.rectTransform.localScale = new Vector3(progressBar.rectTransform.localScale.x + 0.10f, progressBar.rectTransform.localScale.y, progressBar.rectTransform.localScale.z);

            if (progressBar.rectTransform.localScale.x >= 1.0f)
            {
                progressBar.rectTransform.localScale = new Vector3(1, progressBar.rectTransform.localScale.y, progressBar.rectTransform.localScale.z);
            }
        }

        public void PerfectScore()
        {
            notesHit += 1;

            if (notesHit % 10 == 0)
            {
                multiplier += 1;  
            }
            score += 100 * multiplier;
            scoreText.text = score.ToString();
        }

        public void GoodScore()
        {
            notesHit += 1;

            if (notesHit % 10 == 0)
            {
                multiplier += 1;
            }
            score += 75 * multiplier;
            scoreText.text = score.ToString();
        }

        public void MissedNote()
        {
            multiplier = 1;
            notesHit = 0;
            if (isPassing)
            {
                progressBar.rectTransform.localScale = new Vector3(progressBar.rectTransform.localScale.x - 0.05f, progressBar.rectTransform.localScale.y, progressBar.rectTransform.localScale.z);

                if (progressBar.rectTransform.localScale.x < 0)
                {
                    audioPlayback.Stop();
                    isPassing = false;
                }
            }
        }

        public void PlayGameOverSound()
        {
            if (!audioPlayback.isPlaying && gameOver)
            {
                gameOverSound.Play();
                gameOver = false;
            }
            if (!gameOverSound.isPlaying && gameOver == false)
            {
                SetHighScore();
                SceneManager.LoadScene("Game Over");
            }
        }

        public void SetHighScore()
        {
            if (GTLController.score > PlayerPrefs.GetInt("HighScore", 0))
            {
                PlayerPrefs.SetInt("HighScore", score);
            }
        }
    }
}