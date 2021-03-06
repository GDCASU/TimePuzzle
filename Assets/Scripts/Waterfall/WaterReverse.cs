/*
 * Applies an upward force to the Player in a waterfall collider if the waterfall is in the reverse state.
 * 
 * Author: Cristion Dominguez
 * Date: 29 Oct. 2021
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterReverse : ComplexReverse
{
    [SerializeField]
    [Tooltip("The upward force objects in a reversed waterfall shall experience")]
    private float reverseWaterForce;

    private bool isReversing = false;  // Is the waterfall in the reverse state?
    private float elapsedTime = 0f;  // time the waterfall has been reversing for

    /// <summary>
    /// Sets the waterfall's state to reverse for reverse time.
    /// </summary>
    /// <param name="reverseTime"> time to reverse the waterfall for (in seconds) </param>
    public override void Reverse(float reverseTime) => StartCoroutine(CountdownReverseActiveTime(reverseTime));
    private IEnumerator CountdownReverseActiveTime(float reverseTime)
    {
        isReversing = true;

        elapsedTime = 0f;
        while (elapsedTime < reverseTime && effectHub.IntroducingNewEffect == false)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isReversing = false;
        effectHub.TransitionToNextEffect();
    }

    /// <summary>
    /// Disables the Player's gravity if the Player enters the waterfall in the reverse state.
    /// </summary>
    /// <param name="other"> foreign collider in current collider </param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player") && isReversing)
        {
            PlayerController.singleton.ToggleGravity(false);
        }
    }

    /// <summary>
    /// Enacts an upward force onto the Player if the waterfall is in the reverse state.
    /// Enables the Player's gravity if the Player remains in the waterfall after it exits the reverse state.
    /// </summary>
    /// <param name="other"> foreign collider in current collider </param>
    private void OnTriggerStay(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            if (isReversing)
            {
                PlayerController.singleton.ToggleGravity(false);
                other.attachedRigidbody.AddForce(0, reverseWaterForce, 0);
            }
            else
            {
                if (PlayerController.singleton.useGravity == false)
                {
                    PlayerController.singleton.ToggleGravity(true);
                }
            }
        }
    }

    /// <summary>
    /// Enables the Player's gravity if the Player exits the waterfall and the Player's gravity is disabled.
    /// </summary>
    /// <param name="other"> foreign collider in current collider </param>
    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Player") && PlayerController.singleton.useGravity == false)
        {
            PlayerController.singleton.ToggleGravity(true);
        }
    }

    public override float[] GetData()
    {
        return null;
    }
}
