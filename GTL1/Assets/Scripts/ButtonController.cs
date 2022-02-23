using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{

    private SpriteRenderer theSR;
    public Color defaultImage = Color.green;
    public Color pressedImage = Color.red;

    public KeyCode keyToPress;
    // Start is called before the first frame update
    void Start()
    {
        theSR = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keyToPress))
        {
            theSR.color = pressedImage;
        }

        if (Input.GetKeyUp(keyToPress))
        {
            theSR.color = defaultImage;
        }
    }
}
