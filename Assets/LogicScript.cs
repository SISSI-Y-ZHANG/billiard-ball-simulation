using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum ForceType
{
    None, 
    Repel,
    Attract
}

public class LogicScript : MonoBehaviour
{
    [SerializeField] GameObject spawnerObject;
    BallSpawnScript spawner;

    // -------------------------------------- GUI -------------------------------------- //

    [SerializeField] GameObject pauseMenu;
    bool paused = false;
    public bool Paused 
    {
        get { return paused; }
    }

    [SerializeField] TextMeshProUGUI quantityText;

    // -------------------------------------- balls -------------------------------------- //

    [SerializeField] int regularBallQuantity = 200; // [0, 500]
    public int RegularBallQuantity
    {
        get { return regularBallQuantity; }
        //set { ballQuantity = value; }
    }

    [SerializeField] bool randomizeBallSize = true;
    public bool RandomizeBallSize
    {
        get { return randomizeBallSize;}
    }

    [SerializeField] float velocityConstant = 1; // (0, 1.5]
    public float VelocityConstant
    {
        get { return velocityConstant; }
    }

    [SerializeField] float diameterConstant = 1; // (0, 1.5]
    public float DiameterConstant
    {
        get { return diameterConstant; }
    }
    
    // -------------------------------------- special balls -------------------------------------- //
    [SerializeField] int specialBallQuantity = 3; // [0, 50]
    public int SpecialBallQuantity
    {
        get { return specialBallQuantity; }
    }

    [SerializeField] ForceType forceType;

    int forcePosOrNeg; // {0, 1, -1}
    public int ForcePosOrNeg
    {
        get { return forcePosOrNeg;}
    }

    [SerializeField] float forceConstant = 60; // (0, 200]
    public float ForceConstant
    {
        get { return forceConstant; }
    }

    [SerializeField] float forceRadius = 20; // (0, 100]
    public float ForceRadius
    {
        get { return forceRadius; }
    }

    [SerializeField] float movingIncrement = 0.05f; // (0, 0.15]
    public float MovingIncrement
    {
        get { return movingIncrement; }
    }

    [SerializeField] double angleIncrement = 0.02; // (0, 0.15]
    public double AngleIncrement
    {
        get { return angleIncrement; }
    }

    [SerializeField] bool colorSpecialBall = true;
    public bool ColorSpecialBall
    {
        get { return colorSpecialBall; }
    }

    [SerializeField] Color specialBallColor = new Color(0.34f, 0.79f, 0.76f, 1);
    public Color SpecialBallColor
    {
        get { return specialBallColor; }
    }

    [SerializeField] bool labelSpecialBall = false;
    public bool LabelSpecialBall
    {
        get { return labelSpecialBall; }
    }

    // -------------------------------------- momentum -------------------------------------- //

    double initTotalMomentum = 0;
    public double InitTotalMomentum 
    {
        get { return initTotalMomentum; } 
        set { initTotalMomentum = value; }
    }

    double lastTotalMomentum = 0;
    public double LastTotalMomentum 
    {
        get { return lastTotalMomentum; } 
        set { lastTotalMomentum = value; }
    }

    double curTotalMomentum = 0;
    public double CurTotalMomentum 
    {
        get { return curTotalMomentum; } 
        set { curTotalMomentum = value; }
    }

    // --------------------------------------------------------------------------------------------- //
    // -------------------------------------- default methods -------------------------------------- //
    // --------------------------------------------------------------------------------------------- //

    // Start is called before the first frame update
    void Start()
    {   
        spawner = spawnerObject.GetComponent<BallSpawnScript>();
        spawner.SetLogic();
        ResetGame();
    }

    // Update is called once per frame
    void Update()
    {
        ManageGame();
        lastTotalMomentum = curTotalMomentum;
        curTotalMomentum = 0f;
        //Debug.Log("------------------------------ reset cur total ------------------------------");
    }

    // -------------------------------------------------------------------------------------------- //
    // -------------------------------------- custom methods -------------------------------------- //
    // -------------------------------------------------------------------------------------------- //

    void ValidateInput()
    {
        SetForceTypeValue();

        if (regularBallQuantity < 0)
        {
            regularBallQuantity = 0;
        }
        else if (regularBallQuantity > 500)
        {
            regularBallQuantity = 500;
        }

        if (velocityConstant <= 0)
        {
            velocityConstant = 0.001f;
        }
        else if (velocityConstant > 1.5f)
        {
            velocityConstant = 1.5f;
        }

        if (diameterConstant <= 0)
        {
            diameterConstant = 0.001f;
        }
        else if (diameterConstant > 1.5f)
        {
            diameterConstant = 1.5f;
        }

        if (specialBallQuantity < 0)
        {
            specialBallQuantity = 0;
        }
        else if (specialBallQuantity > 50)
        {
            specialBallQuantity = 50;
        }

        if (forceConstant <= 0)
        {
            forceConstant = 0.001f;
        }
        else if (forceConstant > 200)
        {
            forceConstant = 200;
        }

        if (forceRadius <= 0)
        {
            forceRadius = 0.001f;
        }
        else if (forceRadius > 100)
        {
            forceRadius = 100;
        }

        if (movingIncrement <= 0)
        {
            movingIncrement = 0.0001f;
        }
        else if (movingIncrement > 0.1f)
        {
            movingIncrement = 0.15f;
        }

        if (angleIncrement <= 0)
        {
            angleIncrement = 0.0001f;
        }
        else if (angleIncrement > 0.1f)
        {
            angleIncrement = 0.15f;
        }
    }

    void SetForceTypeValue()
    {
        if (forceType == ForceType.None)
        {
            forcePosOrNeg = 0;
        }
        else if (forceType == ForceType.Repel)
        {
            forcePosOrNeg = 1;
        }
        else if (forceType == ForceType.Attract)
        {
            forcePosOrNeg = -1;
        }
    }

    // -------------------------------------- GUI -------------------------------------- //

    public void UpdateQuantity(int quantity)
    {
        quantityText.text = quantity.ToString();
    }

    void ManageGame()
    {
        if (paused)
        {
            if (Input.GetKeyDown(KeyCode.Space) == true)
            {
                ResumeGame();
            }

            if (Input.GetKeyDown(KeyCode.Return) == true)
            {
                ResetGame();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space) == true)
            {
                PauseGame();
            }
        }
    }

    void PauseGame()
    {
        paused = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    void ResumeGame()
    {
        paused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    void ResetGame()
    {
        spawner.DeleteAllBall();
        ValidateInput();
        spawner.SpawnNewBall();

        ResumeGame();
    }
}