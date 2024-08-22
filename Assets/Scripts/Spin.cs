using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Spin : MonoBehaviour
{
[SerializeField] public float spinRate = 1;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, spinRate*Time.deltaTime, 0);
    }
}
