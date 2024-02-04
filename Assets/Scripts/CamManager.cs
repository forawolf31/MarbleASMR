using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamManager : MonoBehaviour
{
    private float hor;
    public float rotationSpeed;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private float moveTime = 0.2f;
    private float timer = 0.0f;
    private bool isMoving = false;
    void Start()
    {
        startPosition = transform.position;
        targetPosition = new Vector3(startPosition.x, startPosition.y, startPosition.z - 0.2f);
    }

    
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Sol mouse tuþuna basýldýðýnda
        {
            isMoving = true;
            timer = 0.0f;
        }

        if (isMoving)
        {
            timer += Time.deltaTime;
            transform.position = Vector3.Lerp(startPosition, targetPosition, timer / moveTime);

            if (timer >= moveTime)
            {
                isMoving = false;
                transform.position = targetPosition;
            }
        }

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
