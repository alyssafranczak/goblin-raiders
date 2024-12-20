using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinHit : MonoBehaviour
{
    public GameObject goblinParticles;
    public AudioClip goblinDeathSFX;

    LevelManager levelManager;

    // Start is called before the first frame update
    void Start()
    {
        levelManager = GameObject.FindObjectOfType<LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelManager.isGameOver)
        {
            DestroyGoblin();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile"))
        {
            DestroyGoblin();
        }
    }

    void DestroyGoblin()
    {
        Instantiate(goblinParticles, transform.position, transform.rotation);

        AudioSource.PlayClipAtPoint(goblinDeathSFX, transform.position);

        gameObject.SetActive(false);

        levelManager.incrementPoints(1);

        Destroy(gameObject, 0.5f);

    }
}
