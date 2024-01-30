using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class İncome : MonoBehaviour
{
    public List<GameObject> Incomes = new List<GameObject>();
    [SerializeField] private GameObject Incomesbutton;

    public int Incomecount=20;
    public GameObject IncomePrefab;
    public int IncomeFix;
    private void Update()
    {
        
    }
    private void Start()
    {
        Incomecount = PlayerPrefs.GetInt("Incomes", 1);
        for (IncomeFix = 0; IncomeFix < Incomecount; IncomeFix++)
        {
            GameObject gameObject1 = Incomes[IncomeFix];
            gameObject1.SetActive(true);
           
        }

    }
    private void CheckIncome()
    {

        //if (Incomecount >= Incomes[IncomeFix])
        //    Incomesbutton.SetActive(true);
        //else
        //    Incomesbutton.SetActive(false);

    }
    public void Incometrue()
    {
        GameManager.manager.money -= GameManager.manager.IncomesCost;

        GameManager.manager.IncomesCost = PlayerPrefs.GetInt("IncomesCost", 1000);
        GameManager.manager.money -= GameManager.manager.IncomesCost;
        PlayerPrefs.SetInt("IncomesCost", GameManager.manager.IncomesCost += 1000);
        PlayerPrefs.SetInt("money", GameManager.manager.money);

        Incomecount++;
        for (int i = 0; i < Incomecount; i++)
        {
            GameObject gameObject1 = Incomes[i];
            gameObject1.SetActive(true);
        }

        PlayerPrefs.SetInt("Incomes", Incomecount);
    }
}
