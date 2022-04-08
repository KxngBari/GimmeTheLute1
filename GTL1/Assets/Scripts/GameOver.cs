using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace GTL
{
    public class GameOver : MonoBehaviour
    {
        public Text scoreText;
        public Text restartText;
        public Text levelSelectText;

        Color changedColor;
        Color originalColor;
        float changeColorTime = 0.025f;
        // Start is called before the first frame update
        void Start()
        {
            scoreText.text = "Your Score Is: " + GTLController.score.ToString();
        }

        // Update is called once per frame
        void Update()
        {
            changedColor = new Color(0.7882353f, 0.3176471f, 0.2509804f);
            originalColor = new Color(0.827451f, 0.7843137f, 0.7058824f);
        }

        public void OnMouseOver()
        {
            if (tag == "Restart")
            {
                restartText.color = Color.Lerp(restartText.color, changedColor, changeColorTime);
            }

            if (tag == "Level Select")
            {
                levelSelectText.color = Color.Lerp(levelSelectText.color, changedColor, changeColorTime);
            }
        }

        public void OnMouseExit()
        {
            restartText.color = originalColor;
            levelSelectText.color = originalColor;
        }

        public void Restart()
        {
            SceneManager.LoadScene(TrackScenes.currentIndex);
        }

        public void ToLevelSelect()
        {
            SceneManager.LoadScene("Level Select Screen");
        }
    }
}