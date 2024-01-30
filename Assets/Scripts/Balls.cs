using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balls : MonoBehaviour
{
    public static Balls Instance;

    public int Income;

    void Awake()
    {
        Instance = this;
    }
}
