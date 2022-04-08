using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo;
using UnityEngine.UI;

namespace GTL
{
    public class NoteObject : MonoBehaviour
    {

        public SpriteRenderer appearance;
        KoreographyEvent trackedEvent;
        ButtonController buttonController;
        GTLController gtlController;

        public void Initialize(KoreographyEvent evt, Color color, ButtonController laneCont, GTLController gameCont)
        {
            trackedEvent = evt;
            appearance.color = color;
            buttonController = laneCont;
            gtlController = gameCont;

            UpdatePosition();
        }

        void Reset()
        {
            trackedEvent = null;
            buttonController = null;
            gtlController = null;
        }

        // Update is called once per frame
        void Update()
        {
            UpdateHeight();

            UpdatePosition();

            if (transform.position.y <= -5.45f)
            {
                gtlController.MissedNote();
                gtlController.ReturnNoteObjectToPool(this);
                Reset();
            }
        }

        void UpdateHeight()
        {
            float originalHeight = appearance.sprite.rect.height / appearance.sprite.pixelsPerUnit;
            float targetHeight = gtlController.WindowSizeInUnits * 5f;
        }

        void UpdatePosition()
        {
            float samplesPerUnit = gtlController.SongSampleRate / gtlController.noteSpeed;

            Vector3 position = buttonController.TargetPosition;
            transform.rotation = buttonController.transform.rotation;
            position.y -= (gtlController.CurrentSampleTime - trackedEvent.StartSample) / samplesPerUnit;
            transform.position = position;
        }

        public bool IsNoteHittable()
        {
            int noteTime = trackedEvent.StartSample;
            int curTime = gtlController.CurrentSampleTime;
            int hitWindow = gtlController.HitWindowWidth;

            return (Mathf.Abs(noteTime - curTime) <= hitWindow);
        }

        public bool IsNoteMissed()
        {
            bool bMissed = true;

            if (enabled)
            {
                int noteTime = trackedEvent.StartSample;
                int curTime = gtlController.CurrentSampleTime;
                int hitWindow = gtlController.HitWindowWidth;

                bMissed = (curTime - noteTime > hitWindow);
            }

            return bMissed;
        }

        void ReturnToPool()
        {
            gtlController.ReturnNoteObjectToPool(this);
            Reset();
        }

        public void OnHit()
        {
            gtlController.AddToScore();
            if (transform.position.y >= -3.8f && transform.position.y <= -3.6f)
            {
                gtlController.PerfectScore();
            }
            else
            {
                gtlController.GoodScore();
            }
            ReturnToPool();
        }
    }
}