using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace GTL
{
    public class ToMainMenu : MonoBehaviour
    {
        public Text exitButton;
        Color changedColor;
        Color originalColor;
        float changeColorTime = 0.025f;

        // Start is called before the first frame update
        void Start()
        {
            originalColor = new Color(0.827451f, 0.6941177f, 0.3019608f);
            changedColor = new Color(0.827451f, 0.1372549f, 0.07058824f);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnMouseOver()
        {
                exitButton.color = Color.Lerp(exitButton.color, changedColor, changeColorTime);
        }

        public void OnMouseExit()
        {
            exitButton.color = originalColor;
        }

        public void ExitToMainMenu()
        {
            SceneManager.LoadScene("Title Screen");
        }


    }
}