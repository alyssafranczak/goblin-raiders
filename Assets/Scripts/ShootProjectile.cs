using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootProjectile : MonoBehaviour
{
    public GameObject projectilePrefab;
    public GameObject rockPrefab;
    public float projectileSpeed = 50f;
    public AudioClip arrowSFX;

    public Image reticleImage;
    public Color goblinColor;
    public Color objectColor;

    Color originalReticleColor;

    GameObject currentProjectilePrefab;

    void Start()
    {
        originalReticleColor = reticleImage.color;
        currentProjectilePrefab = projectilePrefab;
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            GameObject projectile = Instantiate(currentProjectilePrefab, 
                transform.position + transform.forward, transform.rotation) as GameObject;

            Rigidbody rb = projectile.GetComponent<Rigidbody>();

            rb.AddForce(transform.forward * projectileSpeed, ForceMode.VelocityChange);

            projectile.transform.SetParent(GameObject.FindGameObjectWithTag("ProjectileParent").transform);

            AudioSource.PlayClipAtPoint(arrowSFX, transform.position);
        }
    }

    private void FixedUpdate()
    {
        ReticleEffect();
    }

    void ReticleEffect()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                currentProjectilePrefab = projectilePrefab;

                reticleImage.color = Color.Lerp
                    (reticleImage.color, goblinColor, Time.deltaTime * 2);

                reticleImage.transform.localScale = Vector3.Lerp
                    (reticleImage.transform.localScale, new Vector3(0.7f, 0.7f, 1), Time.deltaTime * 2);
            }
            else if (hit.collider.CompareTag("Crate"))
            {
                currentProjectilePrefab = rockPrefab;

                reticleImage.color = Color.Lerp
                    (reticleImage.color, objectColor, Time.deltaTime * 2);

                reticleImage.transform.localScale = Vector3.Lerp
                    (reticleImage.transform.localScale, new Vector3(0.7f, 0.7f, 1), Time.deltaTime * 2);
            }
            else
            {
                currentProjectilePrefab = projectilePrefab;

                reticleImage.color = Color.Lerp
                   (reticleImage.color, originalReticleColor, Time.deltaTime * 2);

                reticleImage.transform.localScale = Vector3.Lerp
                    (reticleImage.transform.localScale, Vector3.one, Time.deltaTime * 2);
            }
        }
        else
        {
            reticleImage.color = Color.Lerp
                (reticleImage.color, originalReticleColor, Time.deltaTime * 2);

            reticleImage.transform.localScale = Vector3.Lerp
                (reticleImage.transform.localScale, Vector3.one, Time.deltaTime * 2);
        }
    }
}
