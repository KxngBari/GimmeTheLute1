using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace GTL
{
    public class StageButton : MonoBehaviour
    {
        public Text stage1Text;
        public Text stage2Text;
        public Text stage3Text;
        public Text stage1HighScore;
        public Text stage2HighScore;
        public Text stage3HighScore;

        Color changedColor;
        Color originalColor;
        Color originalHighScoreColor;
        Color highScoreOpaque;

        float changeColorTime = 0.025f;

        public AudioSource proceedSound;

        public GameObject destroyAudio;

        bool isPressed = true;

        void Start()
        {
            changedColor = new Color(0.7882353f, 0.3176471f, 0.2509804f);
            originalColor = new Color(0.827451f, 0.7843137f, 0.7058824f);
            originalHighScoreColor = stage1HighScore.color;

            highScoreOpaque = new Color(0.2392157f, 0.172549f, 0.07058824f, 1);
        }

        public void OnMouseOver()
        {
            if (tag == "Stage 1 Button")
            {
                stage1Text.color = Color.Lerp(stage1Text.color, changedColor, changeColorTime);
                stage1HighScore.color = Color.Lerp(stage1HighScore.color, highScoreOpaque, changeColorTime);
                stage1HighScore.text = "- > High Score: " + PlayerPrefs.GetInt("HighScore", 0).ToString();
            }

            if (tag == "Stage 2 Button")
            {
                stage2Text.color = Color.Lerp(stage2Text.color, changedColor, changeColorTime);
                stage2HighScore.color = Color.Lerp(stage2HighScore.color, highScoreOpaque, changeColorTime);
                stage2HighScore.text = "- > Coming Soon!";
            }

            if (tag == "Stage 3 Button")
            {
                stage3Text.color = Color.Lerp(stage3Text.color, changedColor, changeColorTime);
                stage3HighScore.color = Color.Lerp(stage3HighScore.color, highScoreOpaque, changeColorTime);
                stage3HighScore.text = "- > Coming Soon!";
            }
        }

        public void OnMouseExit()
        {
            stage1Text.color = originalColor;
            stage2Text.color = originalColor;
            stage3Text.color = originalColor;

            stage1HighScore.color = originalHighScoreColor;
            stage2HighScore.color = originalHighScoreColor;
            stage3HighScore.color = originalHighScoreColor;
        }

        public void ToStage1()
        {
            if (isPressed)
            {
                proceedSound.Play();
                destroyAudio.SetActive(true);
                StartCoroutine(WaitUntilSoundEnds());
                isPressed = false;
            }
        }

        IEnumerator WaitUntilSoundEnds()
        {
            yield return new WaitForSeconds(1.8f);
            SceneManager.LoadScene("Stage 1");
        }
    }
}
