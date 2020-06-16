using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private Rigidbody2D _body;

//    public PlayerScript PlayerInControl;
//    private int _playerIndex = 0;
    public PlayerScript Player1, Player2;
    public bool BottomTurn;
    public float RunSpeed = 1.2f;
    private float _width, _height;
    public Text ChangeAccordinglyText, EndedText;
    public GameObject Boundary, GoalPostTop, GoalPostBottom,CircluarCollider;
    public float BoundaryRight, BoundryLeft, BoundaryBottom, BoundaryTop;
    public int GameScoreAmount=2;
    public Color32 ActiveColor, InactiveColor;
    
    
    
    public static GameManager Instance { get; set; }
    public static GameManager GetInstance()
    {
        return Instance == null ? FindObjectOfType<GameManager>() : Instance;
    }

    // Start is called before the first frame update
    void Start()
    {
//        CreateBoundry();
//        PositionSticks();

        Time.timeScale = 1;
        Application.targetFrameRate = 60;
    }

    private void PositionSticks()
    {
        //positioning sticks according to the screen size
        float screenAspect = (float) Screen.width / Screen.height;
        Debug.Log("Aspect is " + screenAspect);
        Vector3 screenDimen = new Vector3();
        if (screenAspect == 0.75f)
        {
            screenDimen =
                Camera.main.ScreenToWorldPoint(new Vector3(Screen.width - Screen.width / 7.5f, Screen.height, 0f)) * 2;
            Debug.Log("it is 4:3");
        }
        else
        {
            screenDimen =
                Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0f)) * 2;
        }

        for (int i = 0; i < 5; i++)
        {
            Player1.PoleList[i].transform.position = new Vector3(0, 0, 10);

            Player2.PoleList[i].transform.position = new Vector3(0, 0, 10);

            //3 sticks in a row
            if (i < 3)
            {
                Player1.PoleList[i].transform.position =
                    new Vector3(-screenDimen.x / 4 + i * screenDimen.x / 4,  screenDimen.y / 3-screenDimen.y/20, 20);

                Player2.PoleList[i].transform.position =
                    new Vector3(-screenDimen.x / 4 + i * screenDimen.x / 4,
                        -screenDimen.y / 3+screenDimen.y/20, 20);
                Debug.Log("pole2");
            }
            //2 sticks in another row
            else
            {
                Debug.Log("inside the two");
                Player1.PoleList[i].transform.position =
                    new Vector3(-screenDimen.x / 8 + (i -3) * screenDimen.x /4,
                        screenDimen.y / 5-screenDimen.y/20, 20);
                Player2.PoleList[i].transform.position =new Vector3(-screenDimen.x / 8 + (i -3) * screenDimen.x /4,
                        -screenDimen.y / 5+screenDimen.y/20, 20);
            }
        }
    }

   
    void CreateBoundry()
    {
        float screenAspect = (float) Screen.width / Screen.height;
        Debug.Log("Aspect is " + screenAspect);
        Vector3 screenDimen = new Vector3();
        if (screenAspect == 0.75f)
        {
            screenDimen =
                Camera.main.ScreenToWorldPoint(new Vector3(Screen.width - Screen.width / 8, Screen.height, 0f)) * 2;
            Debug.Log("it is 4:3");
        }
        else
        {
            screenDimen =
                Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0f)) * 2;
        }


        float sideBoundaryLength = screenDimen.y / 2 + screenDimen.y / 4 + .5f;
        float thicknessOfBoundry = 0.5f;
        // left 
        GameObject left = Instantiate(Boundary);
        left.name = "boundryLeft";
        left.transform.position = new Vector2(-screenDimen.x / 2, 0f);
        left.transform.localScale = new Vector2(thicknessOfBoundry, sideBoundaryLength);
        BoundryLeft = left.transform.position.x + 0.5f;


        //right
        GameObject right = Instantiate(Boundary);
        right.name = "boundryRight";
        right.transform.position = new Vector2(screenDimen.x / 2, 0f);
        right.transform.localScale = new Vector2(thicknessOfBoundry, sideBoundaryLength);
        BoundaryRight = right.transform.position.x - 0.5f;


        //topLeft
        GameObject topLeft = Instantiate(Boundary);
        topLeft.name = "boundryTopLeft";
        topLeft.transform.position = new Vector2(-screenDimen.x / 3, 2 * screenDimen.y / 5);
        topLeft.transform.localScale = new Vector2(2 * screenDimen.x / 5, thicknessOfBoundry-.1f);
        //topRight
        GameObject topRight = Instantiate(Boundary);
        topRight.name = "boundryTopRight";
        topRight.transform.position = new Vector2(screenDimen.x / 3, 2 * screenDimen.y / 5);
        topRight.transform.localScale = new Vector2(2 * screenDimen.x / 5, thicknessOfBoundry-.1f);
        BoundaryTop = topLeft.transform.position.y - 0.5f;


        //bottompLeft
        GameObject bottomLeft = Instantiate(Boundary);
        bottomLeft.name = "boundryBottomLeft";
        bottomLeft.transform.position = new Vector2(-screenDimen.x / 3, -2 * screenDimen.y / 5);
        bottomLeft.transform.localScale = new Vector2(2 * screenDimen.x / 5, thicknessOfBoundry-.1f);
        //bottomRight
        GameObject bottomRight = Instantiate(Boundary);
        bottomRight.name = "boundryBottomRight";
        bottomRight.transform.position = new Vector2(screenDimen.x / 3, -2 * screenDimen.y / 5);
        bottomRight.transform.localScale = new Vector2(2 * screenDimen.x / 5, thicknessOfBoundry-.1f);
        BoundaryTop = topLeft.transform.position.y - 0.5f;
        
        
        //put circular colliders at the Corners
        //top right
        GameObject CircleTopRight = Instantiate(CircluarCollider);
        CircleTopRight.name = "TopRightCircle";
        CircleTopRight.transform.position = new Vector2(screenDimen.x / 2, 2*screenDimen.y / 5);
        
        //top left
        GameObject CircleTopLeft = Instantiate(CircluarCollider);
        CircleTopLeft.name = "TopLeftCircle";
        CircleTopLeft.transform.position = new Vector2(-screenDimen.x / 2, 2*screenDimen.y / 5);
        
        //bottom right
        GameObject CircleBottomRight = Instantiate(CircluarCollider);
        CircleBottomRight.name = "BottomRightCircle";
        CircleBottomRight.transform.position = new Vector2(screenDimen.x / 2, -2*screenDimen.y / 5);
        
        //bottom right
        GameObject CircleBottomLeft = Instantiate(CircluarCollider);
        CircleBottomLeft.name = "BottomLeftCircle";
        CircleBottomLeft.transform.position = new Vector2(-screenDimen.x / 2, -2*screenDimen.y / 5);
        
        

        //put goal post

        //top
        GoalPostTop.transform.position = new Vector2(0, 2 * screenDimen.y / 5 + thicknessOfBoundry / 2);
        GoalPostTop.transform.localScale = new Vector2(screenDimen.x - 4 * screenDimen.x / 5, 1.2f);
        //bottom
        GoalPostBottom.transform.position = new Vector2(0, -2 * screenDimen.y / 5 - thicknessOfBoundry / 2);
        GoalPostBottom.transform.localScale = new Vector2(screenDimen.x - 4 * screenDimen.x / 5, 1.2f);

        //MAKE BACKGROUND IMAGE SAME SIZE TO PLAYABLE SIZE
        TouchHandler.GetInstance().transform.localScale = new Vector2(screenDimen.x, sideBoundaryLength);
    }

    
    // Update is called once per frame
    void FixedUpdate()
    {
        ReversePoleAnimations();
    }

    public void ReversePoleAnimations()
    {
        foreach (var pole in Player1.PoleList)
        {
            float randomChangingFloat = Random.Range(0, 5);
            if (randomChangingFloat > 2)
            {
                //do randomization on each orb to change colors/rigidity
                MakeSwitchingOrbHappen(pole.GetComponent<Pole>());
            }
        }

        foreach (var pole1 in Player2.PoleList)
        {
            float randomChangingFloat = Random.Range(0, 5);
            if (randomChangingFloat > 2)
            {
                MakeSwitchingOrbHappen(pole1.GetComponent<Pole>());
            }
        }
    }

    private void MakeSwitchingOrbHappen(Pole pole)
    {
        pole.TimeCounter = pole.TimeCounter + Time.deltaTime;

        if (pole.TimeCounter > pole.MaxCounter)
        {
            pole.TimeCounter = 0;
            if (Random.Range(0, 5) > 1)
            {
                pole.TimeCounter = 0;
                pole.SwitchOrbs();
            }

//            GameManager.GetInstance().ReversePoleAnimations();
        }
    }

    public IEnumerator AfterGoal()
    {
        yield return new WaitForSecondsRealtime(1.2f);
        //Moving the ball to center
        TouchHandler.GetInstance().Ball.transform.position=new Vector3(0,0);
        TouchHandler.GetInstance().Ball.GetComponent<Rigidbody2D>().Sleep();
        //Turning off the colliders on the Goal line
        GoalPostTop.gameObject.transform.GetChild(0).GetComponent<BoxCollider2D>()
            .enabled = false;
       GoalPostBottom.gameObject.transform.GetChild(0).GetComponent<BoxCollider2D>()
            .enabled = false;
       TouchHandler.GetInstance().Ball.GetComponent<BallScript>().GoalCounted = false;
       TouchHandler.GetInstance().Ball.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
       
       TouchHandler.GetInstance().CanTouch = true;

    }
    
    public void PauseGame(bool pause)
    {

        Time.timeScale = pause == true ? 0 : 1;
    }
    public void NewGame()
    {
        SceneManager.LoadScene("GamePlay");
    }
}