using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Rigidbody rb;
    private AudioSource audioSource;
    [SerializeField] float accelerationThrust = 1000f;
    [SerializeField] float rotationThrust = 100f;
    [SerializeField] ParticleSystem mainParticles;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            BeginThrust();
        }
        else
        {
            StopThrust();
        }
    }

    void BeginThrust()
    {
        rb.AddRelativeForce(Vector3.up * Time.deltaTime * accelerationThrust);
        if (audioSource.isPlaying.Equals(false))
        {
            mainParticles.Play();
            audioSource.Play();
        }
    }

    void StopThrust()
    {
        audioSource.loop = false;
        audioSource.Stop();
        mainParticles.Stop();
    }

    void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            ApplyRotation(rotationThrust);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            ApplyRotation(-rotationThrust);
        }
    }

    void ApplyRotation(float rotationThisFrame)
    {
        rb.freezeRotation = true;
        transform.Rotate(Vector3.forward * Time.deltaTime * rotationThisFrame);
        rb.freezeRotation = false;
    }
}