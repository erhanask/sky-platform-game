using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    Vector3 startingPosition;
    [SerializeField] Vector3 movementVector;
    [SerializeField] float period = .5f;
    float movementFactor;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float cycles = Time.time / period; // grows continually from 0
        const float tau = Mathf.PI * 2; // about 6.28
        float rawSinWave = Mathf.Sin(cycles * tau); // goes from -1 to +1
        movementFactor = rawSinWave / 2f + 0.5f; // recalculated to go from 0 to 1 so it's cleaner


        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPosition + offset;
    }
}