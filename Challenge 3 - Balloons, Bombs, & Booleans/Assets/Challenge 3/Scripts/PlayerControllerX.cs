using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    public bool gameOver;

    public float floatForce;
    private float gravityModifier = 1.5f;
    private float upBoundary = 15.0f;
    private float score = 0;
    private Rigidbody playerRb;

    public ParticleSystem explosionParticle;
    public ParticleSystem fireworksParticle;

    private AudioSource playerAudio;
    private AudioSource gameMusic;
    public AudioClip moneySound;
    public AudioClip explodeSound;
    public AudioClip bounceSound;


    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity *= gravityModifier;
        playerAudio = GetComponent<AudioSource>();
        playerRb = GetComponent<Rigidbody>(); // Created a GetComponent variable to reference the rigidbody of the player

        // Apply a small upward force at the start of the game
        playerRb.AddForce(Vector3.up * 5, ForceMode.Impulse);

        gameMusic = GameObject.Find("Main Camera").GetComponent<AudioSource>(); // Gets audio source component from the main camera
        Debug.Log("Earn points by collecting money while avoiding the upcoming bombs!"); // Displays message at the log terminal at the start of the game
    }

    // Update is called once per frame
    void Update()
    {
        // While space is pressed and player is low enough, float up
        if (Input.GetKey(KeyCode.Space) && !gameOver && transform.position.y < upBoundary)
        {
            playerRb.AddForce(Vector3.up * floatForce);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        // if player collides with bomb, explode and set gameOver to true
        if (other.gameObject.CompareTag("Bomb"))
        {
            explosionParticle.Play();
            gameMusic.Stop();
            playerAudio.PlayOneShot(explodeSound, 1.0f);
            gameOver = true;
            Debug.Log("Game Over! Your final score is: " + score.ToString()); // Displays the game over message and the player's score
            Destroy(other.gameObject);
        } 

        // if player collides with money, fireworks
        else if (other.gameObject.CompareTag("Money"))
        {
            fireworksParticle.Play();
            playerAudio.PlayOneShot(moneySound, 1.0f);
            Destroy(other.gameObject);
            Debug.Log("Score: " + (++score).ToString()); // Displays the current score of the player at the log terminal
        }

        // if player collides with ground, bounce the player up with bounce sound
        else if (other.gameObject.CompareTag("Ground") && !gameOver)
        {
            playerRb.AddForce(Vector3.up * 7, ForceMode.Impulse);
            playerAudio.PlayOneShot(bounceSound, 1.0f); // Plays a bouncing sound when the player touches the ground
        }
    }
}