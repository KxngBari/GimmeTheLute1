using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GTL
{
    public class Parallax : MonoBehaviour
    {

        private float length, startPos;
        public GameObject mainCamera;
        Vector3 cameraPosition;
        public float parallaxEffect;
        // Start is called before the first frame update
        void Start()
        {
            startPos = transform.position.x;
            cameraPosition = mainCamera.transform.position;
            length = GetComponent<SpriteRenderer>().bounds.size.x;
        }

        // Update is called once per frame
        void Update()
        {
            float temp = (mainCamera.transform.position.x * (1 - parallaxEffect));
            float distance = (mainCamera.transform.position.x * parallaxEffect);

            transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);

            if (temp > startPos + length)
            {
                startPos += length;
            }
            else if (temp < startPos - length)
            {
                startPos -= length;
            }

            cameraPosition.x -= 0.35f * Time.deltaTime;
            mainCamera.transform.position = new Vector3(cameraPosition.x, mainCamera.transform.position.y, mainCamera.transform.position.z);
        }
    }
}