using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIHandler : MonoBehaviour
{
    public static UIHandler Instance { get; set; }
    public GameObject GoalPanel;
    private Animator _goalPanelAnimation,_gameOverPanelAnimator;
    public GameObject GameOverPanel;
    public static UIHandler GetInstance()
    {
        return Instance == null ? FindObjectOfType<UIHandler>() : Instance;
    }
    // Start is called before the first frame update
    void Start()
    {
        _goalPanelAnimation = GoalPanel.GetComponent<Animator>();
        _gameOverPanelAnimator = GameOverPanel.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    
    public void Goaled(bool notOwnGoal)
    {
        _goalPanelAnimation.Play(notOwnGoal ? "GoaledShow" : "OwnGoaledShow");
        
        
    }

    public IEnumerator GameOver(bool open)
    {
        yield return new WaitForSecondsRealtime(1.2f);
        GameManager.GetInstance().PauseGame(open);
        _gameOverPanelAnimator.Play(open==true? "PanelOpenClose":"PanelOpenClose 0");
        Debug.Log("gameOver");
    }

    
}
