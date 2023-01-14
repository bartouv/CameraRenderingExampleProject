using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 rotationSpeed = new Vector3(0,1,0);

    void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime);
        
        
        
    }

}