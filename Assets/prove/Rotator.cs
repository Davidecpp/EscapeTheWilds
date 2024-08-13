using UnityEngine;

public class Rotator : MonoBehaviour
{
    private Vector3 rotationSpeed = new Vector3(0f, 0f, 50f);

    void Update()
    {
        foreach (Transform child in transform)
        {
            child.Rotate(rotationSpeed * Time.deltaTime);
        }
    }
}