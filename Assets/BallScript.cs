using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

public class BallScript : MonoBehaviour
{
    public bool IsInTouch, FirstTouch;
    public bool IsBottomTurn = true;

    public Rigidbody2D BallRigidbody;
    public float MySpeed;
    public bool CanTouch;
    public bool GoalCounted;


    public static BallScript Instance { get; set; }

    public static BallScript GetInstance()
    {
        return Instance == null ? FindObjectOfType<BallScript>() : Instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        BallRigidbody = GetComponent<Rigidbody2D>();
        GetComponent<CircleCollider2D>().tag = "ball";
    }

    // Update is called once per frame


    private void FixedUpdate()
    {
        if (BallRigidbody.velocity.magnitude > 0)
        {
            BallRigidbody.velocity = 0.99f * BallRigidbody.velocity;
            BallRigidbody.angularVelocity = 0.99f * BallRigidbody.angularVelocity;
        }
    }

    private void Update()
    {
        MySpeed = TouchHandler.GetInstance().RigidBodyBall.velocity.magnitude;


        if (MySpeed < 0.2f)
        {
            BallRigidbody.Sleep();
            if (TouchHandler.GetInstance().HasReleasedShot)
            {
                TurnHandler.GetInstance().CanChangeTurn = true;
                TurnHandler.GetInstance().ChangeTurn();
            }
        }


//        if (gameObject.GetComponent<CircleCollider2D>().CompareTag("hands"))
//        {
//            BallRigidbody.AddTorque(Pole.GetInstance().Arms.transform.rotation.y);
//        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        var force = BallRigidbody.velocity * 8;
        var wallforce = 50f;
        if (other.gameObject.CompareTag("hands") || other.gameObject.CompareTag("wall"))
        {
            var dir = other.contacts[0].point - (Vector2) transform.position;
            dir = -dir.normalized;
            GetComponent<Rigidbody2D>().WakeUp();
            GetComponent<Rigidbody2D>().AddForce(dir * force);
            Debug.Log("touched " + other.gameObject.tag + " with force " + dir);
        }
        else if (other.gameObject.CompareTag("insidePost"))
        {
            if (GoalCounted) //so that the goal is not counted on every collision
            {
                return;
            }

            TouchHandler.GetInstance().HasReleasedShot = false;
            TurnHandler.GetInstance().PlayerInTurn.CountOfGoal(other.gameObject);

            GoalCounted = true;
            TurnHandler.GetInstance().CanChangeTurn = true;
            TurnHandler.GetInstance().ChangeTurn();
        }
    }
}