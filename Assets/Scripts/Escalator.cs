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

public class EscalatorRide
{
    private Vector3 _startingPosition;
    public Vector3 StartinPosition { get { return _startingPosition; } }

    private Vector3 _targetPosition;
    public Vector3 TargetPosition { get { return _targetPosition; } }

    private float _rideSpeed;
    public float RideSpeed { get { return _rideSpeed; } }

    public EscalatorRide(Vector3 start, Vector3 target, float speed)
    {
        _startingPosition = start;
        _targetPosition = target;
        _rideSpeed = speed;
    }
}


public class Escalator : MonoBehaviour
{
    [SerializeField]
    private float _properDirectionEscalatorSpeed;
    public float ProperDirectionEscalatorSpeed { get { return _properDirectionEscalatorSpeed; } }

    [SerializeField]
    private float _wrongDirectionEscalatorSpeed;
    public float WrongDirectionEscalatorSpeed { get { return _wrongDirectionEscalatorSpeed; } }

    // Because I'm lazy, let's just assume:
    //  0 = Up
    //  1 = Down
    //  2 = Shutdown
    [SerializeField]
    private Sprite[] _escalatorSprites = new Sprite[3];

    [SerializeField]
    private SpriteRenderer _escalatorSpriteRenderer;

    [SerializeField]
    private Transform _targetTop;
    public Transform TargetTop { get { return _targetTop; } }

    [SerializeField]
    private Transform _targetBottom;
    public Transform TargetBottom { get { return _targetBottom; } }

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

    private int _fromFloor;
    public int FromFloor { get { return _fromFloor; } }
    private int _toFloor;
    public int ToFloor { get { return _toFloor; } }

    private MapManager _mapManager;
    private bool _isDying = false;
    private bool _isShutdown = false;
    public bool IsShutdown { get { return _isShutdown; } }

    private bool _escalatorInUse = false;
    public bool EscalatorInUse { get { return _escalatorInUse; } set { _escalatorInUse = value; } }

    private EscalatorDirectionVertical _escalatorDirectionVertical;
    public EscalatorDirectionVertical EscDirectionVertical { get { return _escalatorDirectionVertical; } }
    private EscalatorDirectionHorizontal _escalatorDirectionHorizontal;
    public EscalatorDirectionHorizontal EscDirectionHorizontal { get { return _escalatorDirectionHorizontal; } }

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

    public void Init(MapManager mapManager, EscalatorDirectionVertical escalatorDirectionV, EscalatorDirectionHorizontal escalatorDirectionH, int startingFloor)
    {
        _mapManager = mapManager;
        _escalatorDirectionVertical = escalatorDirectionV;
        _escalatorDirectionHorizontal = escalatorDirectionH;
        _isDying = true;

        _fromFloor = startingFloor;
        _toFloor = startingFloor + 1;

        _shutdownTime = mapManager.EscalatorShutdownTime;
        _escalatorLife = Random.Range(mapManager.MinEscalatorTime, mapManager.MaxEscalatorTime);

        _escalatorSpriteRenderer.sprite = _escalatorSprites[(int)escalatorDirectionV];
        transform.localScale = new Vector3((float)escalatorDirectionH, 1f, 1f);
    }

    private void TriggerShutdown()
    {
        if (!_escalatorInUse)
        {
            _isDying = false;
            _isShutdown = true;
            _escalatorSpriteRenderer.sprite = _escalatorSprites[2];
        }
        else
        {
            _escalatorLife = Random.Range(_mapManager.MinEscalatorTime, _mapManager.MaxEscalatorTime);
        }
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
