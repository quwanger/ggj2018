using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscalatorSpawn
{
    private Vector3 _spawnPosition;
    public Vector3 SpawnPosition { get { return _spawnPosition; } }

    private int _mallFloor;
    public int MallFloor { get { return _mallFloor; } }

    private bool _isTaken = false;
    public bool IsTaken { get { return _isTaken; } set { _isTaken = value; } }

    public EscalatorSpawn(Vector3 spawnPosition, int mallLevel)
    {
        _spawnPosition = spawnPosition;
        _mallFloor = mallLevel;
    }
}

public class Escalator : MonoBehaviour
{
    // Because I'm lazy, let's just assume:
    //  0 = Up
    //  1 = Down
    [SerializeField]
    private Sprite[] _escalatorSprites = new Sprite[2];

    [SerializeField]
    private SpriteRenderer _escalatorSpriteRenderer;

    public enum EscalatorDirection
    {
        Up = 0,
        Down = 1
    }

    private EscalatorDirection _escalatorDirection;

    public void Init(EscalatorDirection escalatorDirection)
    {
        _escalatorDirection = escalatorDirection;
        _escalatorSpriteRenderer.sprite = _escalatorSprites[(int)escalatorDirection];
    }
}
