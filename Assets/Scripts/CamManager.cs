using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamManager : MonoBehaviour
{
    private float hor;
    public float rotationSpeed;

    void Start()
    {
        
    }

    
    void Update()
    {
        CamKontrol();
    }


    void CamKontrol()
    {
        if (Input.GetMouseButton(0))
        {
            hor = Input.GetAxisRaw("Mouse X");
            transform.Rotate(Vector3.up, hor * rotationSpeed * Time.deltaTime);
        }
    }
}
