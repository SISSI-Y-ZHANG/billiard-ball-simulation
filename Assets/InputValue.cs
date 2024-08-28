using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputValue: MonoBehaviour
{
    // the comments are the bounds for values
    public int _regularBallQuantity = 201; // [0, 500]
    public bool _randomizeBallSize = true;
    public float _velocityConstant = 1f; // (0, 1.5]
    public float _diameterConstant = 1f; // (0, 1.5]

    public int _specialBallQuantity = 4; // [0, 50]
    public string _forceType = "Repel"; // None, Repel, Attract
    public float _forceConstant = 60f; // (0, 200]
    public float _forceRadius = 20f; // (0, 100]
    public double _movingIncrement = 0.05; // (0, 0.15]
    public double _angleIncrement = 0.02; // (0, 0.15]
    public bool _colorSpecialBall = true;
}
