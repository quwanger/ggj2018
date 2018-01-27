using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTile : MonoBehaviour {

    private const float MIN_LIFESPAN = 1f;
    private const float MAX_LIFESPAN = 5f;

    private Animator _animator;

    private MapManager _mapManager;
    public MapManager MapManager { get { return _mapManager; } }

    private Vector3 _tilePosition;
    public Vector3 TilePosition { get { return _tilePosition; }}

    private MapManager.TileSlideDirection _slideDirection;
    public MapManager.TileSlideDirection SlideDirection { get { return _slideDirection; }}   

    private float _tileLifeCounter;
    private bool _isDying = false;

    [SerializeField]
    private string _storeName;
    public string StoreName { get { return _storeName; } set { _storeName = value; } }

    [SerializeField]
    private TextMesh _textName;
    [SerializeField]
    private TextMesh _textPosition;
    [SerializeField]
    private TextMesh _textSlideDirection;

    public void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Update()
    {
        if (_isDying && _slideDirection != MapManager.TileSlideDirection.None && _mapManager.EnableTileReplace)
        {
            _tileLifeCounter -= Time.deltaTime;

            if (_tileLifeCounter < 0)
            {
                TriggerExtinction();
            }
        }
    }

    public void Init(MapManager mapManager, Vector3 tilePosition, MapManager.TileSlideDirection slideDirection)
    {
        _mapManager = mapManager;
        _tilePosition = tilePosition;
        _slideDirection = slideDirection;

        _tileLifeCounter = Random.Range(MIN_LIFESPAN, MAX_LIFESPAN);
        _isDying = true;

        //TODO: set text (this will be removed)
        _textName.text = _storeName;
        _textPosition.text = "(" + _tilePosition.x.ToString() + ", " + _tilePosition.y + ")";
        _textSlideDirection.text = slideDirection.ToString();
    }

    public void TriggerAnimation(MapManager.TileSlideDirection slideDirection, bool animateIn)
    {
        switch(slideDirection)
        {
            case MapManager.TileSlideDirection.Down:
                _animator.SetTrigger(animateIn ? "SlideInBottom" : "SlideOutBottom");
                break;
            case MapManager.TileSlideDirection.Left:
                _animator.SetTrigger(animateIn ? "SlideInLeft" : "SlideOutLeft");
                break;
            case MapManager.TileSlideDirection.Right:
                _animator.SetTrigger(animateIn ? "SlideInRight" : "SlideOutRight");
                break;
            case MapManager.TileSlideDirection.Up:
                _animator.SetTrigger(animateIn ? "SlideInTop" : "SlideOutTop");
                break;
            default:
                break;
        }
    }

    private void TriggerExtinction()
    {
        _isDying = false;
        TriggerAnimation(_slideDirection, false);
        _mapManager.TriggerReplaceStore(this);
    }
}
