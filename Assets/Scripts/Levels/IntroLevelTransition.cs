using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroLevelTransition : MonoBehaviour
{
    public bool transitioningInside;
    public Light directionalLight;
    public float startingIntensity;
    public float transitionRate = .05f;

    private void Start()
    {
        if (directionalLight)
        {
            startingIntensity = directionalLight.intensity;
        } 
    }

    public List<GameObject> objectsToDisable;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (transitioningInside) TransitionInside();
            else TransitionOutside();
        }
    }


    private void TransitionInside()
    {
        foreach (GameObject obj in objectsToDisable) obj.SetActive(false);
        StartCoroutine(LightBrightness(true));
        transitioningInside = !transitioningInside;
    }

    private void TransitionOutside()
    {
        foreach (GameObject obj in objectsToDisable) obj.SetActive(true);
        StartCoroutine(LightBrightness(false));
        transitioningInside = !transitioningInside;
    }

    IEnumerator LightBrightness(bool dim)
    {
        float adjustmentValue = (dim ? -1 : 1) * transitionRate * Time.deltaTime;
        float goal = dim ? 0 : startingIntensity;

        if (dim)
        {
            while (directionalLight.intensity > goal)
            {
                directionalLight.intensity += adjustmentValue;
                yield return null;
            }
        }
        else
        {
            while (directionalLight.intensity < goal)
            {
                directionalLight.intensity += adjustmentValue;
                yield return null;
            }
        }
    }
}
