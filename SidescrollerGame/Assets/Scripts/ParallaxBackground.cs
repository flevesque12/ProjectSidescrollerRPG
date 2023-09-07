using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{

    private GameObject camera;

    [SerializeField] private float parallaxEffect;

    private float xPosition;
    private float spriteLength;

    void Start()
    {
        camera = Camera.main.gameObject;
        xPosition = transform.position.x;
        spriteLength = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        float distanceMoved =  camera.transform.position.x * (1 - parallaxEffect);
        float distanceToMove = camera.transform.position.x * parallaxEffect;

        transform.position = new Vector3(xPosition + distanceToMove, transform.position.y);

        if(distanceMoved > xPosition + spriteLength){
            xPosition = xPosition + spriteLength;
        }
        else if(distanceMoved < xPosition - spriteLength){
            xPosition = xPosition - spriteLength;
        }
    }
}
