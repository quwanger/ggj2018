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
    //  2 = Shutdown
    [SerializeField]
    private Sprite[] _escalatorSprites = new Sprite[3];

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

    private float _escalatorLife;
    private float _shutdownTime;

    private MapManager _mapManager;
    private bool _isDying = false;
    private bool _isShutdown = false;

    private EscalatorDirectionVertical _escalatorDirectionVertical;
    private EscalatorDirectionHorizontal _escalatorDirectionHorizontal;

    private void Update()
    {
        if (_isDying)
        {
            _escalatorLife -= Time.deltaTime;

            if (_escalatorLife < 0)
            {
                TriggerShutdown();
            }
        }
        else if(_isShutdown)
        {
            _shutdownTime -= Time.deltaTime;

            if (_shutdownTime < 0)
            {
                ReactivateEscalator();
            }
        }
    }

    public void Init(MapManager mapManager, EscalatorDirectionVertical escalatorDirectionV, EscalatorDirectionHorizontal escalatorDirectionH)
    {
        _mapManager = mapManager;
        _escalatorDirectionVertical = escalatorDirectionV;
        _escalatorDirectionHorizontal = escalatorDirectionH;
        _isDying = true;

        _shutdownTime = mapManager.EscalatorShutdownTime;
        _escalatorLife = Random.Range(mapManager.MinEscalatorTime, mapManager.MaxEscalatorTime);

        _escalatorSpriteRenderer.sprite = _escalatorSprites[(int)escalatorDirectionV];
        transform.localScale = new Vector3((float)escalatorDirectionH, 1f, 1f);
    }

    private void TriggerShutdown()
    {
        _isDying = false;
        _isShutdown = true;
        _escalatorSpriteRenderer.sprite = _escalatorSprites[2];
    }

    private void ReactivateEscalator()
    {
        _isShutdown = false;
        _shutdownTime = _mapManager.EscalatorShutdownTime;
        _isDying = true;
        _escalatorLife = Random.Range(_mapManager.MinEscalatorTime, _mapManager.MaxEscalatorTime);
        _escalatorDirectionVertical = _escalatorDirectionVertical == EscalatorDirectionVertical.Down ? EscalatorDirectionVertical.Up : EscalatorDirectionVertical.Down;
        _escalatorSpriteRenderer.sprite = _escalatorSprites[(int)_escalatorDirectionVertical];
    }
}
