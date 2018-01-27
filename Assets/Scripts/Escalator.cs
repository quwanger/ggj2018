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

    public enum EscalatorDirectionVertical
    {
        Up = 0,
        Down = 1
    }

    public enum EscalatorDirectionHorizontal
    {
        Left = -1,
        Right = 1
    }


    private EscalatorDirectionVertical _escalatorDirectionVertical;
    private EscalatorDirectionHorizontal _escalatorDirectionHorizontal;

    public void Init(EscalatorDirectionVertical escalatorDirectionV, EscalatorDirectionHorizontal escalatorDirectionH)
    {
        _escalatorDirectionVertical = escalatorDirectionV;
        _escalatorDirectionHorizontal = escalatorDirectionH;

        _escalatorSpriteRenderer.sprite = _escalatorSprites[(int)escalatorDirectionV];
        transform.localScale = new Vector3((float)escalatorDirectionH, 1f, 1f);
    }
}
