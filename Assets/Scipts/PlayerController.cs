using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rbPlayer;
    public float gravityModifier;
    public float jumpForce;
    private bool onGround = true;
    public bool gameOver = false;

    //Particle Variables
    private Animator animPlayer;
    public ParticleSystem expSystem;
    public ParticleSystem dirtSystem;

    //Sound Variables
    private AudioSource asPlayer;
    public AudioClip jumpSound;
    public AudioClip crashSound;

    // Start is called before the first frame update
    void Start()
    {
        rbPlayer = GetComponent<Rigidbody>();
        Physics.gravity *= gravityModifier;

        animPlayer = GetComponent<Animator>();
        asPlayer = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //Jumping
        bool spaceDown = Input.GetKeyDown(KeyCode.Space);
        if (spaceDown && onGround && !gameOver)
        {
            rbPlayer.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            onGround = false;

            //Animations
            animPlayer.SetTrigger("Jump_trig");

            //Particles
            dirtSystem.Stop();

            //Jump Sound
            asPlayer.PlayOneShot(jumpSound, 1.0f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            onGround = true;
            dirtSystem.Play();
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            //Game Over
            gameOver = true;
            Debug.Log("Game Over!");

            //Animations
            animPlayer.SetBool("Death_b", true);
            animPlayer.SetInteger("DeathType_int", 1);

            //Particles
            expSystem.Play();
            dirtSystem.Stop();

            //Crash Sound
            asPlayer.PlayOneShot(crashSound, 1.0f);
        }
    }
}
