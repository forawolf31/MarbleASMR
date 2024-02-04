using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public int money;

    [SerializeField] Button BallsButton;
    [SerializeField] Button AddRoadButton;
    [SerializeField] public Button IncomesButton;
    [SerializeField] Button MergeButton;
    [SerializeField] Button AutoClickButton;

    [HideInInspector]
    public int IncomesCost;

    [HideInInspector]
    public int BallsCost = 1000;
    
    [HideInInspector]
    public int MergeCost; 

    [HideInInspector]
    public int AddRoadCost;

    [HideInInspector]
    public int AutoClickCost;


    [SerializeField] private TMP_Text MergeCostText;
    [SerializeField] private TMP_Text BallsCostText;
    [SerializeField] private TMP_Text AddRoadCostText;
    [SerializeField] private TMP_Text IncomesCostText;
    [SerializeField] private TMP_Text AutoClickCostText;


    [SerializeField] private TMP_Text moneyText;
    public static GameManager manager;
    public GameObject[] ballPrefabs;
    public int ballcount;
    [SerializeField] private int maxBallCount;
    private List<UpgradeChecker> UpgradeBalls = new List<UpgradeChecker>();

    public static int CurrentLevelIndex => SceneManager.GetActiveScene().buildIndex;
    //zaman hýzlanmasý 
    public float fastForwardSpeed = 2f; 
    private float originalForwardSpeed; 
    private float lastClickTime; 
    private bool hasClicked; 
    //bar image
    //public Image barImage;  
    //public float decreaseRate = 0.1f / 10f; // barýn azalma hýzý
    
    
    public int moneyPerClick = 5;
    public int autoClickIncome = 10;  // Otomatik týklama baþýna kazanýlan para miktarý (saniyede)
    private float nextClickTime = 0.0f; // sonraki oto týklama için zaman
    public float clickInterval = 1.0f; // týklamalar arasýndaki süre 
    [SerializeField] private TMP_Text autoClickIncomeText;

    // click efekt
    public TMP_Text textPrefab; // TextMeshPro prefab'ý
    public float displayDuration = 2.0f; // Text'in ekranda kalacaðý süre
    public Canvas canvas; // Canvas referansý
 
    private void OnEnable()
    {
        if (manager == null)
        {
            manager = this;
        }
    }

    private void OnApplicationQuit()
    {
        SahneKaydet();
        SaveBalls();
    }
    private void Awake()
    {
        money = PlayerPrefs.GetInt("money", 0);

        CheckSceneIndex();
        SahneKaydet();
        UpdateTexts();
        Load();
    }

    private void SaveBalls()
    {
        PlayerPrefs.SetInt("BallLevel", UpgradeBalls.Count);
        for (int i = 0; i < UpgradeBalls.Count; i++)
        {
            PlayerPrefs.SetInt("Level" + i.ToString(), UpgradeBalls[i].ballCount);
        }
    }

    private void Load()
    {
        for (int i = 0; i < PlayerPrefs.GetInt("BallLevel"); i++)
        {
            UpgradeBalls.Add(new UpgradeChecker());
        }
        for (int i = 0; i < UpgradeBalls.Count; i++)
        {
            StartCoroutine(SpawnBalls(i));
        }
    }
    private void Update()
    {

        if (Input.GetMouseButtonDown(0) && !hasClicked)
        {            
            lastClickTime = Time.time;
            
            Time.timeScale = fastForwardSpeed;

            hasClicked = true;
        }
        // eðer 3 saniye içinde týklanmazsa ve dönme süresi hala düþükse, eski süreye geri yükleyin
        if (hasClicked && Time.time - lastClickTime >= 1f)
        {
            Time.timeScale = 1;
            hasClicked = false; 
        }

        // ekrana her týklandýðýnda barý artýr
        if (Input.GetMouseButtonDown(0))
        {
            //barImage.fillAmount += 0.042f; // bar doluluðunu yüzde olarak artýr
            AddMoney(moneyPerClick);
            CreateTextAtPosition(Input.mousePosition);
        }

        // Otomatik týklama için zaman kontrolü
        if (Time.time >= nextClickTime)
        {
            AddMoney(autoClickIncome);
            UpdateAutoClickIncomeText();  // Yeni metni burada çaðýr
            nextClickTime = Time.time + clickInterval;
        }

        //barImage.fillAmount -= decreaseRate * Time.deltaTime/3;

        money = PlayerPrefs.GetInt("money", 0);
        moneyText.SetText($"{money} $");

        UpdateTexts();
        CheckIdles();
    }

    void Start()
    {
        BallsCost = PlayerPrefs.GetInt("BallCost", 0);
        MergeCost = PlayerPrefs.GetInt("MergeCost", 2000);
        AddRoadCost = PlayerPrefs.GetInt("AddRoadCost", 100000);
        AutoClickCost = PlayerPrefs.GetInt("AutoClickCost", 1000);
        IncomesCost = PlayerPrefs.GetInt("IncomesCost", 1000);
        moneyPerClick = PlayerPrefs.GetInt("moneyPerClick", 5);

        UpdateTexts();
        //StartCoroutine(SpawnBalls());
        originalForwardSpeed = fastForwardSpeed;
    }
    private void CheckSceneIndex()
    {
        int tempLevelIndex = PlayerPrefs.GetInt("Levels");
        if (CurrentLevelIndex == tempLevelIndex | PlayerPrefs.GetInt("Levels") == 0 | PlayerPrefs.GetInt("Levels") == SceneManager.GetActiveScene().buildIndex - 1) return;
        LoadScene(PlayerPrefs.GetInt("Levels"));
    }
    private void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
    private void SahneKaydet() => PlayerPrefs.SetInt("Levels", SceneManager.GetActiveScene().buildIndex);

    IEnumerator SpawnBalls(int i)
    {
        for (int j = 0; j < PlayerPrefs.GetInt("Level" + i.ToString()); j++)
        {
            Upgrade(ballPrefabs[i], i);
            yield return new WaitForSeconds(1.5f);
        }
    }

    public void AddMoney(int i)
    {
        money += i;
        PlayerPrefs.SetInt("money", money);
    }

    public void AddBall()
    {
        
        BallsCost = PlayerPrefs.GetInt("BallCost",0);
        money -= BallsCost;
        PlayerPrefs.SetInt("BallCost", BallsCost += 1000);

        PlayerPrefs.SetInt("money", money);
        if (ballcount >= maxBallCount) return;
        ballcount++;
        PlayerPrefs.SetInt("balls", ballcount);
        GameObject ball = Instantiate(ballPrefabs[0], new Vector3(-0.7f, 2.5f, 0.12f), Quaternion.identity);
        if (UpgradeBalls.Count < 1) UpgradeBalls.Add(new UpgradeChecker());
        UpgradeBalls[0].balls.Add(ball);
        UpgradeBalls[0].ballCount++;
        //if (UpgradeBalls[0].balls.Count >= 3)
        //    Mergebutton.SetActive(true);
        //else
        //    Mergebutton.SetActive(false);
        UpdateTexts();
    }


    private void Upgrade(GameObject ballPrefab, int level)
    {
        ballcount++;
        PlayerPrefs.SetInt("balls", ballcount);
        GameObject ball = Instantiate(ballPrefab, new Vector3(-0.7f, 2.5f, 0.12f), Quaternion.identity);
        if (UpgradeBalls.Count <= level) UpgradeBalls.Add(new UpgradeChecker());
        UpgradeBalls[level].balls.Add(ball);
        UpgradeBalls[level].ballCount++;
        UpdateTexts();
    }

    public int ForUpgrade;
    // Merge Cost burdan deðiþecek
    public void LevelUpBalls()
    {
        money -= MergeCost;


        MergeCost = PlayerPrefs.GetInt("MergeCost", 2000);
        //money -= MergeCost;
        PlayerPrefs.SetInt("MergeCost", MergeCost += 2000);

        PlayerPrefs.SetInt("money", money);
        foreach (var upgradeChecker in UpgradeBalls)
        {
            if (upgradeChecker.ballCount >= 3)
            {
                for (ForUpgrade = 0; ForUpgrade < UpgradeBalls.Count; ForUpgrade++)
                {
                    if (UpgradeBalls[ForUpgrade].balls.Count >= 3)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            GameObject ball = UpgradeBalls[ForUpgrade].balls[0];
                            ball.transform.DOMove(Camera.main.transform.GetChild(0).transform.position, 1).OnComplete(() => SetActiveFalse(ball));
                            UpgradeBalls[ForUpgrade].balls.RemoveAt(0);
                            ballcount--;
                            UpgradeBalls[ForUpgrade].ballCount--;
                        }

                        Upgrade(ballPrefabs[ForUpgrade + 1], ForUpgrade + 1);
                        if (UpgradeBalls[ForUpgrade].balls.Count >= 3)
                            MergeButton.interactable = true;
                        else
                            MergeButton.interactable = false;
                        break;
                    }
                }
                // Toplarý birleþtirme iþlemi burada yapýlacak
                // Örnek olarak, ilk 3 topu birleþtirip yeni bir top yaratma iþlemi
                // Daha sonra, `upgradeChecker` listesinden bu toplarý çýkarýn ve yeni topu ekleyin

                break; // Ýþlem tamamlandýktan sonra döngüden çýk
            }
        }
        
    }
    public void UpdateTexts()
    {
        MergeCostText.text = "$" + MergeCost;
        BallsCostText.text = "$" + BallsCost.ToString();
        AddRoadCostText.text = "$" + AddRoadCost;
        AutoClickCostText.text = "$" + AutoClickCost;
        IncomesCostText.text = "$" + IncomesCost;
    }

    private void SetActiveFalse(GameObject ball)
    {
        ball.SetActive(false);
    }

    [Obsolete]
    public void bolumac(string bolumismi)
    {
        SaveBalls();

        AddRoadCost = PlayerPrefs.GetInt("AddRoadCost", 100000);
        money -= AddRoadCost;
        PlayerPrefs.SetInt("AddRoadCost", AddRoadCost += 900000);
        PlayerPrefs.SetInt("money", money);
        Application.LoadLevel(bolumismi);
    }
    
    public void CheckIdles()
    {
        BallsButton.interactable = (money >= BallsCost) && (ballcount  < maxBallCount);

        //if (money < IncomesCost)
        //    IncomesButton.interactable = false;
        //else
        //    IncomesButton.interactable = true;
        //if (Ýncome.income.Incomecount + 1 <= Ýncome.income.Incomes.Count)
        //    IncomesButton.interactable = true;
        //else
        //IncomesButton.interactable = false;
        //IncomesButton.interactable = (money >= IncomesCost) && (Ýncome.income.Incomecount + 1 <= Ýncome.income.Incomes.Count);

        if (money < MergeCost)
            MergeButton.interactable = false;
        else
            MergeButton.interactable = true;

        if (money < AddRoadCost)
            AddRoadButton.interactable = false;
        else
            AddRoadButton.interactable = true;
        if (money < AutoClickCost)
            AutoClickButton.interactable = false;
        else
            AutoClickButton.interactable = true;
        //if (UpgradeBalls[ForUpgrade].balls.Count >= 3)
        //    MergeButton.interactable = true;
        //else
        //    MergeButton.interactable = false;
        bool canMerge = false; // Merge için gerekli koþulun baþlangýçta false olduðunu varsayalým
        foreach (var upgradeChecker in UpgradeBalls)
        {
            if (upgradeChecker.ballCount >= 3) // Herhangi bir top türünden 3 veya daha fazla top var mý?
            {
                canMerge = true;
                break; // Eðer varsa, döngüden çýk
            }
        }

        MergeButton.interactable = canMerge;
    }

    void AddMoneyClick(int amount)
    {
        money += amount;
        Debug.Log("Toplam Para: " + money);
    }

    private int autoClickUpgradeIndex = 0;
    private int[] autoClickUpgradeAmounts = new int[19]
    {
        15, 25, 50, 100, 300, 500, 1000, 3000, 5000, 10000,
        30000, 50000, 100000, 300000, 500000, 1000000, 3000000, 5000000, 10000000
    };
    public void UpgradeClickValue(int increaseAmount)
    {

        AutoClickCost = PlayerPrefs.GetInt("AutoClickCost", 1000);
        money -= AutoClickCost;

        PlayerPrefs.SetInt("AutoClickCost", AutoClickCost += 2000);


        PlayerPrefs.SetInt("money", money);
        if (autoClickUpgradeIndex < autoClickUpgradeAmounts.Length)
        {
            autoClickIncome += autoClickUpgradeAmounts[autoClickUpgradeIndex];
            autoClickUpgradeIndex++;
            UpdateAutoClickIncomeText();
        }
    }

    private void UpdateAutoClickIncomeText()
    {
        autoClickIncomeText.text = "Income: $" + (autoClickIncome / clickInterval).ToString("F2") + " per second";
    }

    void CreateTextAtPosition(Vector3 position)
    {
        Vector2 viewportPosition = Camera.main.ScreenToViewportPoint(position);

        // Rastgele bir konum deðeri oluþturun
        float randomX = Random.Range(0.2f, 0.8f);
        float randomY = Random.Range(0.2f, 0.8f);

        Vector2 worldPosition = new Vector2(
            (viewportPosition.x * canvas.GetComponent<RectTransform>().sizeDelta.x) - (canvas.GetComponent<RectTransform>().sizeDelta.x * randomX),
            (viewportPosition.y * canvas.GetComponent<RectTransform>().sizeDelta.y) - (canvas.GetComponent<RectTransform>().sizeDelta.y * randomY));

        // TextMeshPro objesini oluþtur
        TMP_Text createdText = Instantiate(textPrefab, canvas.transform);
        createdText.rectTransform.anchoredPosition = worldPosition;
        createdText.text = "+$" + moneyPerClick.ToString(); // Text'i güncel int deðeriyle ayarla

        //// Belirlenen süre sonra yok et
        //Destroy(createdText.gameObject, displayDuration);
        // Coroutine'i baþlatarak animasyonu uygula
        StartCoroutine(MoveAndDestroyText(createdText));
    }

    IEnumerator MoveAndDestroyText(TMP_Text text)
    {
        float elapsedTime = 0;

        while (elapsedTime < displayDuration)
        {
            // Saða ve sola hareket et
            text.transform.localPosition += new Vector3(Mathf.Sin(elapsedTime * 10) * 1.6f, 0, 0);
            // Aþaðý doðru düþme efekti
            text.transform.localPosition -= new Vector3(0, elapsedTime * 1.6f, 0);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Destroy(text.gameObject);
    }


}

[Serializable]
public class UpgradeChecker
{
    public int ballCount;
    public List<GameObject> balls = new List<GameObject>();
}