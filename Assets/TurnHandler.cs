using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnHandler : MonoBehaviour
{
    public PlayerScript PlayerTop, PlayerBottom,PlayerInTurn,PlayerNotInTurn;
    private Animator _ballAnimator;
    public bool CanChangeTurn = true;
    
    public static TurnHandler Instance { get; set; }

    public static TurnHandler GetInstance()
    {
        return Instance == null ? FindObjectOfType<TurnHandler>() : Instance;
    }


    private void Start()
    {
        PlayerTop = GameManager.GetInstance().Player1;
        PlayerBottom = GameManager.GetInstance().Player2;
        
        _ballAnimator = TouchHandler.GetInstance().Ball.GetComponent<Animator>();
        PlayerInTurn = Random.Range(0, 2) == 0 ? PlayerTop:PlayerBottom;
        ChangeTurn();
    }


    public void ChangeTurn()
    {
        
        if (!CanChangeTurn)
        {
            return;
        }
        CanChangeTurn = false;
        

        Debug.Log("Turn Changed");
        if (PlayerInTurn==PlayerTop)
        {
            PlayerTop.isInTurn = false;
            PlayerNotInTurn = PlayerTop;

            PlayerInTurn = PlayerBottom;
            PlayerBottom.isInTurn = true;
        }
        else
        {
            PlayerBottom.isInTurn = false;
            PlayerNotInTurn = PlayerBottom;

            PlayerInTurn = PlayerTop;
            PlayerTop.isInTurn = true;
        }
        TouchHandler.GetInstance().HasReleasedShot = false;
        TouchHandler.GetInstance().CanTouch = true;
        Glow(PlayerInTurn,true);
        Glow(PlayerNotInTurn,false);
        
    }

    public void Glow(PlayerScript player, bool glow)
    {
        TouchHandler.GetInstance().Ball.transform.GetChild(0).gameObject.SetActive(true);
        TouchHandler.GetInstance().Ball.transform.GetChild(0).GetComponent<SpriteRenderer>().color =
            PlayerInTurn.PlayerColor;
        if (glow)
        {
            _ballAnimator.Play("BallSelect");
        }

        PlayerInTurn.MyPost.GetComponent<SpriteRenderer>().color = GameManager.GetInstance().InactiveColor;
        PlayerInTurn.MyPost.transform.GetChild(0).GetComponent<SpriteRenderer>().color = GameManager.GetInstance().InactiveColor;
        PlayerNotInTurn.MyPost.GetComponent<SpriteRenderer>().color = GameManager.GetInstance().ActiveColor;
        PlayerNotInTurn.MyPost.transform.GetChild(0).GetComponent<SpriteRenderer>().color = GameManager.GetInstance().ActiveColor;


       
        
        
        
    }
}
