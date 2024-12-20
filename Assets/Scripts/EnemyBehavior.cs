using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 10f;
    public float minDistance = 2f;
    public float swingDistance = 2f;
    public int damageAmt = 20;
    public AudioClip goblinSFX;

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        anim = GetComponent<Animator>();

        InvokeRepeating("PlayGoblinSFX", 0f, Random.Range(2, 5));
    }

    // Update is called once per frame
    void Update()
    {
        float step = moveSpeed * Time.deltaTime;

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance > minDistance)
        {
            transform.LookAt(player);
            transform.position = Vector3.MoveTowards(transform.position, player.position, step);
            anim.SetFloat("Speed_f", 5f);
        } else
        {
            anim.SetInteger("WeaponType_int", 12);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var playerHealth = other.GetComponent<PlayerHealth>();
            playerHealth.TakeDamage(damageAmt);
        }
    }

    private void PlayGoblinSFX()
    {
        AudioSource.PlayClipAtPoint(goblinSFX, transform.position);
    }
}
