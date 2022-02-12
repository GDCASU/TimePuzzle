/*
 * Revision Author: Cristion Dominguez
 * Modification: 
 *  The SlowObject coroutine transitions to the next time effect.
 *  The slowDownFactor is set in the SlowInvocation script.
 *  The timeScale of MasterTime is multiplied to elapsedTime for environment effect stacking.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowDownSpeedUpObject : ComplexSlow
{
    private bool speedingUp = false;
    private WaitForSeconds waitTime = new WaitForSeconds(5f);
    private bool slowing = false;
    private Vector3 preVelocity;
    private float slowDownFactor = 0.5f;
    private float speedUpFactor = 2f;
    private bool casting = false;

    Rigidbody rb;
    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // this adds artificial gravity since we are disabling the in game gravity as part of acheiving our effect
        if(slowing)
            rb.AddForce(Physics.gravity * (slowDownFactor * slowDownFactor), ForceMode.Acceleration);
        if(speedingUp)
            rb.AddForce(Physics.gravity * (speedUpFactor * speedUpFactor), ForceMode.Acceleration);
    }
    public bool GetSpeedingStatus()
    {
        return speedingUp;
    }
    public bool GetSlowingStatus()
    {
        return slowing;
    }

    /// <summary>
    /// Slows the object for a specified amount of time by reducing velocity.
    /// Stacking Environment: environment freeze ceases the object from moving whilst active and environment slow decreases the object's speed further.
    /// </summary>
    /// <param name="slowTime"> time to slow the object </param>
    IEnumerator SlowObject(float slowTime)
    {
        slowing = true;
        rb.useGravity = false;
        preVelocity = rb.velocity;
        rb.velocity *= slowDownFactor;
        rb.angularVelocity *= slowDownFactor;

        float elapsedTime = 0f;
        while (elapsedTime < slowTime && effectHub.IntroducingNewEffect == false)
        {
            elapsedTime += Time.deltaTime * MasterTime.singleton.timeScale;
            yield return null;
        }

        rb.velocity = preVelocity;
        slowing = false;
        rb.useGravity = true;
        casting = false;

        effectHub.TransitionToNextEffect();
    }

     IEnumerator SpeedObject()
    {
        //all the same stuff as the Slow method but the opposite to have a speedup effect
        speedingUp = true;
        rb.useGravity = false;
        preVelocity = rb.velocity;
        rb.velocity *= speedUpFactor;
        rb.angularVelocity *= speedUpFactor;

        yield return waitTime;
        rb.velocity = preVelocity;
        speedingUp = false;
        rb.useGravity = true;
        casting = false;
    }

    public override void Slow(float slowTime, float slowFactor)
    {
        slowDownFactor = slowFactor;

        if(!casting)
        {
            casting = true;
            StartCoroutine(SlowObject(slowTime));
        }
    }

    public void SpeedUp()
    {
        if (!casting)
        {
            casting = true;
            StartCoroutine(SpeedObject());
        }
    }
}
