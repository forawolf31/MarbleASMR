using DG.Tweening;
using System.Collections;
using UnityEngine;




public class GateController : MonoBehaviour
{
    [SerializeField] private GameObject ClickEffectText;

    public float duration = 1f; // Objeyi sahnede tutacaðýnýz süre
    public float moveDistance = 1.0f; // Objeyi ne kadar yukarý taþýyacaðýnýz
    
    private GameObject lastCreatedObject = null; // Son yaratýlan objenin referansý
   
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
        // Eðer bir obje varsa, onu yok et
        if (lastCreatedObject != null)
        {
            Destroy(lastCreatedObject);
        }

        StartCoroutine(CreateAndDestroyObject());
    }

    private IEnumerator CreateAndDestroyObject()
    {
        // Yeni objeyi yarat ve referansý sakla
        GameObject tempObject = Instantiate(ClickEffectText, transform.position, Quaternion.identity);
        lastCreatedObject = tempObject;

        // Hareket baþlangýç ve bitiþ pozisyonlarýný hesapla
        Vector3 startPosition = tempObject.transform.position;
        Vector3 endPosition = startPosition + Vector3.up * moveDistance;

        // Belirlenen süre boyunca objeyi yukarý doðru hareket ettir
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            tempObject.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Süre sonunda objeyi yok et ve referansý sýfýrla
        Destroy(tempObject);
        if (lastCreatedObject == tempObject)
        {
            lastCreatedObject = null;
        }
    }
   
}

    
