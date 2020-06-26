using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public GameObject pole;
    public List<GameObject> PoleList;
    public Color PlayerColor;
    public static PlayerScript Instance { get; set; }
    public GameObject MyPost;
    public string PlayerName;
    public int FormationNumber;
    public List<Transform> Formation0, Formation1, Formation2;
    private List<List<Transform>> _formations = new List<List<Transform>>();
    public TextMeshProUGUI MyScoreText;
    public int MyScore;
    public bool isInTurn;

    public static PlayerScript GetInstance()
    {
        return Instance == null ? FindObjectOfType<PlayerScript>() : Instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject poler = Instantiate(pole);

            PoleList.Add(poler);
            poler.GetComponent<SpriteRenderer>().color = PlayerColor;
        }

        if (Formation0 != null)
        {
            _formations.Add(Formation0);
        }

        if (Formation1 != null)
        {
            _formations.Add(Formation1);
        }

        if (Formation2 != null)
        {
            _formations.Add(Formation2);
        }


        FormationSetter(PlayerPrefs.GetInt("FormationNumber", 0));
    }

    public void FormationSetter(int formationNumber)
    {
        FormationNumber = formationNumber;
        StartCoroutine(SetFormation());

    }
    
   private IEnumerator SetFormation()
   {
       float totalTime = 1f, elapsedTime = 0f;
       while (totalTime>elapsedTime)
       {
           for (int i = 0; i < 5; i++)
           {
               PoleList[i].transform.position = Vector3.Lerp(PoleList[i].transform.position,_formations[FormationNumber][i].position,elapsedTime/totalTime);
               elapsedTime += Time.deltaTime;
               yield return null;
           }
       }
        
    }


    public void CountOfGoal(GameObject post)
    {
        
        

        // ReSharper disable once SuspiciousTypeConversion.Global
        if (MyPost.transform != null && post!=MyPost.transform.GetChild(1).gameObject)
        {
            Debug.Log("Not my Post i am player "+ PlayerName);
            MyPost.gameObject.transform.GetChild(0)
                .GetComponent<BoxCollider2D>().enabled = true; //ball lai bahira jana nadina post ko barrier on gareko

            TurnHandler.GetInstance().GoaledPlayer= gameObject.GetComponent<PlayerScript>();
           
            IncreaseScore();

            
        }
        else
        {
            Debug.Log("My Post i am player "+ PlayerName);

        }
        

    }


 

    public void IncreaseScore()
    {
        MyScore++;
        MyScoreText.text = MyScore.ToString();
        Debug.Log("My goals= " + MyScore);
        //check own's score and game over if conditionmeet
        if (MyScore > GameManager.GetInstance().GameScoreAmount)
        {
            StartCoroutine(UIHandler.GetInstance().GameOver(true));
        }
        StartCoroutine(GameManager.GetInstance().AfterGoal());
        TouchHandler.GetInstance().IsInTouch = false;


    }
}