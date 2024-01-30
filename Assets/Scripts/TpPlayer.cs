using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TpPlayer : MonoBehaviour
{
    private void Start()
    {
        Physics.IgnoreLayerCollision(6, 6);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("gate"))
        {
            SpawnerController.controller.SaveObject(gameObject);
        }
    }


}
