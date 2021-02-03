using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public const int numBonuses = 3;
    public const int screenWidth = 12;
    public const int screenHeight = 15;

    [SerializeField]
    private GameObject bonus1Prefab;
    [SerializeField]
    private GameObject bonus2Prefab;
    [SerializeField]
    private GameObject bonus3Prefab;

    [SerializeField]
    private Text text;
    [SerializeField]
    private Button button;

    public Bonus[] Bonuses = new Bonus[numBonuses];                  // Массив бонусов в игре
    public GameObject[] BonusesVis = new GameObject[numBonuses];     // Массив визуализаций бонусов
    public Transform[] BonusesTransforms = new Transform[numBonuses];// Массив трансформов бонусов

    public int count;                                                // текущий счет 

    private int currentBonus;                                        // текущий бонус
    private WaitForSeconds currentWait;                              // длительность текущего бонуса
    private float currentX;
    private float currentY;


    private void OnEnable()
    {
        button.onClick.AddListener(CheckForMouseClick);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(CheckForMouseClick);
    }

    private void Start()
    {
        count = 0;

        Bonuses[0] = new Bonus(1, 100);
        BonusesVis[0] = Instantiate(bonus1Prefab, Vector3.zero, transform.rotation.normalized);
        BonusesTransforms[0] = BonusesVis[0].GetComponent<Transform>();
        BonusesVis[0].SetActive(false);

        Bonuses[1] = new Bonus(2, 50);
        BonusesVis[1] = Instantiate(bonus2Prefab, Vector3.zero, transform.rotation.normalized);
        BonusesTransforms[1] = BonusesVis[1].GetComponent<Transform>();
        BonusesVis[1].SetActive(false);

        Bonuses[2] = new Bonus(3, 25);
        BonusesVis[2] = Instantiate(bonus3Prefab, Vector3.zero, transform.rotation.normalized);
        BonusesTransforms[2] = BonusesVis[2].GetComponent<Transform>();
        BonusesVis[2].SetActive(false);


        InvokeRepeating("ShowBonus", 3f, 3.5f);
    }

    private void ShowBonus()
    {
        currentBonus = (int)Random.Range(0f, numBonuses);
        currentX = (int)Random.Range(0f, screenWidth) + 0.5f;
        currentY = (int)Random.Range(1f, screenHeight) + 0.5f;
        currentWait = new WaitForSeconds(Bonuses[currentBonus].Duration);

        StartCoroutine(BonusLoop());
    }

    private IEnumerator BonusLoop()
    {
        BonusesTransforms[currentBonus].Translate(new Vector3 (currentX, currentY, 0f));
        BonusesVis[currentBonus].SetActive(true);
        yield return currentWait;
        BonusesVis[currentBonus].SetActive(false);
        BonusesTransforms[currentBonus].position = Vector3.zero;
    }

    public void CheckForMouseClick(/*PointerEventData _pData*/)
    {
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float x = Mathf.Abs(worldPosition.x/*_pData.e[0].pressPosition.x*/ - currentX);
        float y = Mathf.Abs(worldPosition.y/*_pData.pressPosition.y*/ - currentY);

        if (BonusesVis[currentBonus].activeSelf && (x <= 0.5f) && (y <= 0.5f))
        {
            BonusesVis[currentBonus].SetActive(false);
            count += Bonuses[currentBonus].Reward;
            text.text = count.ToString();
            CheckForGameOver();
        }
    }

    private void CheckForGameOver()
    {
        if (count >= 1000)
        {
            text.text = "You Win! Press R for restart!";
            Time.timeScale = 0;
        }
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }


    }
}
