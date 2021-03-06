using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    [SerializeField] GameObject cubePrefab;
    [SerializeField] private float bulletSpeed = 45f;
    private float nextTimeToShoot = 0f;
    private float fireRate = 0.5f;
    public float reloadTime = 8f;

    private void Update()
    {
        if (Time.time >= nextTimeToShoot)
        {
            //cooldown management
            nextTimeToShoot = Time.time + 1f / fireRate;
            Shoot();
        }
    }
    private void Shoot()
    {
        GameObject b = Instantiate(cubePrefab, transform.position, transform.rotation) as GameObject;
        b.transform.position = transform.position;
        Vector3 difference = (transform.forward - b.transform.position).normalized;
        b.transform.rotation = transform.rotation;
        b.GetComponent<Rigidbody>().AddForce(difference * bulletSpeed, ForceMode.Impulse);
        StartCoroutine(Reload());
    }
    IEnumerator Reload()
    {
        yield return new WaitForSeconds(reloadTime);
    }
}
