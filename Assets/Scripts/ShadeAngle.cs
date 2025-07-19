using UnityEngine;

public class ShadeAngle : MonoBehaviour
{
    public Quaternion maintainAngle = Quaternion.Euler(90, 0, 0);
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = maintainAngle;
    }
}
