using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public Animator transition;
    public AudioSource proceedSound;
    public AudioSource menuAudio;
    public float transitionTime = 0.1f;
    bool isPressed = true;

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            if (isPressed)
            {
                proceedSound.Play();
                isPressed = false;
            }
            LoadLevelSelect();
        }
    }

    public void LoadLevelSelect()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
        
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);
    }
}
