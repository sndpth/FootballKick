using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Timers;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class TouchHandler : MonoBehaviour
{
    public Vector3 Direction;
    public bool MaxForceApplied;

    private Vector3 _startPos,
        _endPos,
        _touchPosWorld; // for determining where in the world the user touches

    public Rigidbody2D RigidBodyBall;

    public bool ReadyToAddForce;

    public float NavCircleSizeLimiter, SpeedForNavigation, Thrust;
    public bool CanTouch, BallTouched, HasReleasedShot;

    public GameObject Ball, NavCircle;
    public float MaxDirection, MinDirection;
    public float ScaleOfNavCircle;


    public static TouchHandler Instance { get; set; }

    public static TouchHandler GetInstance()
    {
        return Instance == null ? FindObjectOfType<TouchHandler>() : Instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        CanTouch = true;
        RigidBodyBall = Ball.GetComponent<Rigidbody2D>();
    }


    private void FixedUpdate()
    {
        if (ReadyToAddForce)
        {
            HasReleasedShot = true;

            RigidBodyBall.AddForce(Direction * Thrust);
            ReadyToAddForce = false;
            Direction = Vector3.zero;
            Debug.Log("And the ball was moved");
            
        }
    }


    private void Update()
    {
        if (!CanTouch)
        {
            return;
        }


        if (Input.GetMouseButton(0))
        {
            IsTouched();
        }

        if (BallTouched)
        {
            StillInTouch();
        }

        if (Input.GetMouseButtonUp(0) && BallTouched) // Drag finish
        {
            TouchFinish();
        }
    }


    // This Method contains the things to be done when the touch is on hold and ball is being navigated

    private void StillInTouch()
    {
        _touchPosWorld = _startPos;
        NavCircle.transform.localPosition = Ball.transform.localPosition;

        RigidBodyBall.WakeUp();

        _endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float xScale = Mathf.Abs(_endPos.x - _startPos.x) / 4;
        float yScale = Mathf.Abs(_endPos.y - _startPos.y) / 4;
        float scaleToSend = 0;
        scaleToSend = xScale > yScale ? xScale : yScale;

        if ((scaleToSend * 2) < NavCircleSizeLimiter
        ) //Resize the NavCircle if the size is under the limit (scaletosend *2 because we send 2 times of the scaletosend below)
        {
            NavCircle.transform.localScale =
                new Vector2(scaleToSend, scaleToSend) * 2; //here we send twice of scale to send . as said above
        }
        else // else keep the certain max size constant
        {
            NavCircle.transform.localScale = new Vector2(NavCircleSizeLimiter, NavCircleSizeLimiter);
            MaxForceApplied = true;
            scaleToSend = NavCircleSizeLimiter;
        }

        ScaleOfNavCircle = scaleToSend;
        Direction = _startPos - _endPos;

        float angle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg;

        Quaternion rotation = Quaternion.Euler(angle * Vector3.forward);
        rotation = rotation.normalized;
       
        NavCircle.transform.rotation = rotation;
        Ball.gameObject.transform.rotation = rotation;
        Direction = Direction.normalized;
    }


    // This method is called when the touch is released by user
    private void TouchFinish()
    {
        float elapsedTime = 0, totalTime = 2f;

        BallTouched = false;

        NavCircle.transform.localScale = Vector3.zero;


        float absY = Mathf.Abs(Direction.y), absX = Mathf.Abs(Direction.x);
        Debug.Log(absX + " is Xvalue & " + absY + "is Yvalue");
        if (absY > MinDirection || absX > MinDirection)
        {
            CanTouch = false;

            if (absX > MaxDirection)
            {
                Direction.x = Direction.x > 0 ? MaxDirection : -MaxDirection;
            }

            if (absY > MaxDirection)
            {
                Direction.y = Direction.y > 0 ? MaxDirection : -MaxDirection;
            }


            ReadyToAddForce = true;
        }

        Debug.Log("Direction x is" + Mathf.Abs(Direction.x) + "and Direction y is" + Mathf.Abs(Direction.y));
    }


    public void IsTouched()
    {
        MaxForceApplied = false;


        NavCircle.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        Ball.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

        _touchPosWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition); // maps touch poisition to screen pos

        RaycastHit2D
            hitMe = Physics2D.Raycast(_touchPosWorld,
                Vector2.zero); // hit raycast directly on the point where user touches.

//        if (hitMe.collider != null) //if collider hits sometihing
//        {
//            if (hitMe.collider.CompareTag("ball"))
//            {
        BallTouched = true;
        _startPos = Ball.transform.position;
        RigidBodyBall.drag = .5f;
        RigidBodyBall.angularDrag = .5f;
//                Ball.transform.GetChild(0).gameObject.SetActive(false);
        Debug.Log("Ball is Touched");
//            }
//        }
    }

    public void GoTowards(Vector2 direction, float force)
    {
        if (Mathf.Abs((_startPos - Ball.transform.position).x) > 2 &&
            Mathf.Abs((_startPos - Ball.transform.position).y) > 2)
        {
            GameManager.GetInstance().EndedText.text = "fulfil distance " + (_startPos - _endPos);

            BallScript.GetInstance().BallRigidbody.AddForce(direction);
        }
        else
        {
            GameManager.GetInstance().EndedText.text = "NO " + _startPos + "" + _endPos;
        }

        Debug.Log("Move RigidBody");
    }
}