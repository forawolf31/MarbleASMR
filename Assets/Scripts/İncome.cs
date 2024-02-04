using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class İncome : MonoBehaviour
{
    public List<GameObject> Incomes = new List<GameObject>();
    [SerializeField] private GameObject Incomesbutton;
    public static İncome income;

    public int Incomecount=20;
    public GameObject IncomePrefab;
    public int IncomeFix;
    private void Update()
    {
        CheckIncome();
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
        GameManager.manager.IncomesButton.interactable = (GameManager.manager.money >= GameManager.manager.IncomesCost) && (Incomecount + 1 <= Incomes.Count);
    }
    public void Incometrue()
    {
        GameManager.manager.moneyPerClick = PlayerPrefs.GetInt("moneyPerClick", 10);
        PlayerPrefs.SetInt("moneyPerClick", GameManager.manager.moneyPerClick += 10);
        
        GameManager.manager.money -= GameManager.manager.IncomesCost;
        
        GameManager.manager.IncomesCost = PlayerPrefs.GetInt("IncomesCost", 1000);
        
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
