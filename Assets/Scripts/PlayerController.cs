using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private TMP_Text walletText;
    public List<GameObject> Players = new List<GameObject>();
    public GameObject[] Levels;

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("YearGate"))
        {
            
          
            
        }
    }
}
