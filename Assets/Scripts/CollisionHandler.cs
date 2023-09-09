using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    enum Scenes
    {
        Current,
        Next
    }

    [SerializeField] float invokeTime = 1f;
    [SerializeField] AudioClip success;
    [SerializeField] AudioClip crash;
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem crashParticles;

    Movement movement;
    AudioSource audioSource;

    bool isTransitioning = false;
    bool collisionDisabled = false;


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        movement = GetComponent<Movement>();
    }

    private void Update()
    {
        SetDebugKeys();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (isTransitioning.Equals(true) || collisionDisabled.Equals(true))
        {
            return;
        }

        switch (other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("This thing is friendly");
                break;
            case "Finish":
                StartFinishSequence();
                break;
            case "Fuel":
                // todo: fuel system not implemented yet
                Debug.Log("You collided with the fuel!");
                break;
            default:
                StartCrashSequence();
                break;
        }
    }

    void StartFinishSequence()
    {
        isTransitioning = true;
        movement.enabled = false;
        successParticles.Play();
        CallSound(success);
        Invoke(nameof(SuccessSequence), invokeTime);
    }

    void StartCrashSequence()
    {
        isTransitioning = true;
        movement.enabled = false;
        crashParticles.Play();
        CallSound(crash);
        Invoke(nameof(CrashSequence), invokeTime);
    }

    void SuccessSequence()
    {
        LoadScene(Scenes.Next);
    }

    void CrashSequence()
    {
        LoadScene(Scenes.Current);
    }

    void CallSound(AudioClip clip)
    {
        audioSource.Stop();
        audioSource.PlayOneShot(clip);
    }

    void LoadScene(Enum scene)
    {
        int loadingScene;
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        switch (scene)
        {
            case Scenes.Current:
                loadingScene = currentScene;
                break;
            case Scenes.Next:
                // TODO: if the next scene is the last scene, will do something
                loadingScene = currentScene + 1;
                break;
            default:
                loadingScene = currentScene;
                break;
        }

        if (loadingScene == SceneManager.sceneCountInBuildSettings)
        {
            loadingScene = 0;
        }
        
        SceneManager.LoadScene(loadingScene);
    }

    // Debug Functions
    void SetDebugKeys()
    {
        if (Input.GetKey(KeyCode.L))
        {
            Debug.Log("'l is pressed'");
            LoadNextLevel();
        }
        else if (Input.GetKey(KeyCode.C))
        {
            Debug.Log("'c is pressed'");
            ToggleCollision();
        }
    }

    void LoadNextLevel()
    {
        LoadScene(Scenes.Next);
    }

    void ToggleCollision()
    {
        collisionDisabled = !collisionDisabled;
    }
}