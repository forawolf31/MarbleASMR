using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    public static SpawnerController controller;
    public List<GameObject> gameObjects = new List<GameObject>();

    private void OnEnable()
    {
        if (controller == null)
        {
            controller = this;
        }
    }

    private void Start()
    {
        StartCoroutine(Loop());
    }
    public void SaveObject(GameObject obj)
    {
        gameObjects.Add(obj);
        obj.transform.gameObject.SetActive(false);
    }


    IEnumerator Loop()
    {
        while (true)
        {
            while (gameObjects.Count != 0)
            {
                gameObjects[0].SetActive(true);
                gameObjects[0].transform.position= new Vector3(-0.7f, 2.5f, 0.12f);
                gameObjects.RemoveAt(0);
                yield return new WaitForSeconds(1.5f);
            }
            yield return new WaitForSeconds(1.5f);
        }
    }
}

