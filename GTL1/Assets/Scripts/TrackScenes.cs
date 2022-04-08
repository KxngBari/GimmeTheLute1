using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrackScenes : MonoBehaviour
{
    public static int currentIndex = 0;
    private void Start()
    {
        currentIndex = SceneManager.GetActiveScene().buildIndex;
    }
}
