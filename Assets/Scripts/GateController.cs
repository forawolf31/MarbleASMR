using DG.Tweening;
using System.Collections;
using UnityEngine;




public class GateController : MonoBehaviour
{
    [SerializeField] private GameObject ClickEffectText;

    public float duration = 1f; // Objeyi sahnede tutaca��n�z s�re
    public float moveDistance = 1.0f; // Objeyi ne kadar yukar� ta��yaca��n�z
    
    private GameObject lastCreatedObject = null; // Son yarat�lan objenin referans�
   
    [SerializeField] GameObject kapi;
        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("player"))
            {
                kapi.transform.DORotate(new Vector3(0, -45, 0), 0.5f, RotateMode.WorldAxisAdd);
                GameManager.manager.AddMoney(other.GetComponent<Balls>().Income);
                StopAllCoroutines();
                StartCoroutine(ReturnGate());
            TriggerObjectCreation();
            }
        }

        IEnumerator ReturnGate()
        {
            yield return new WaitForSeconds(0.5f);
            kapi.transform.DORotate(new Vector3(0, 0, 0), 0.5f, RotateMode.Fast);
        }

    public void TriggerObjectCreation()
    {
        // E�er bir obje varsa, onu yok et
        if (lastCreatedObject != null)
        {
            Destroy(lastCreatedObject);
        }

        StartCoroutine(CreateAndDestroyObject());
    }

    private IEnumerator CreateAndDestroyObject()
    {
        // Yeni objeyi yarat ve referans� sakla
        GameObject tempObject = Instantiate(ClickEffectText, transform.position, Quaternion.identity);
        lastCreatedObject = tempObject;

        // Hareket ba�lang�� ve biti� pozisyonlar�n� hesapla
        Vector3 startPosition = tempObject.transform.position;
        Vector3 endPosition = startPosition + Vector3.up * moveDistance;

        // Belirlenen s�re boyunca objeyi yukar� do�ru hareket ettir
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            tempObject.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // S�re sonunda objeyi yok et ve referans� s�f�rla
        Destroy(tempObject);
        if (lastCreatedObject == tempObject)
        {
            lastCreatedObject = null;
        }
    }
   
}

    
