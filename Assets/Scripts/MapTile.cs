using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTile : MonoBehaviour
{
    private Animator _animator;

    private MapManager _mapManager;
    public MapManager MapManager { get { return _mapManager; } }

    private Vector3 _tilePosition;
    public Vector3 TilePosition { get { return _tilePosition; }}

    private MapManager.TileSlideDirection _slideDirection;
    public MapManager.TileSlideDirection SlideDirection { get { return _slideDirection; }}   

    private float _tileLifeCounter;
    private bool _isDying = false;

    private float _liquidationDuration;

    private bool _inLiquidation = false;
    public bool InLiquidation { get { return _inLiquidation; } }

    [SerializeField]
    private string _storeName;
    public string StoreName { get { return _storeName; } set { _storeName = value; } }

    [SerializeField]
    private SpriteRenderer _storefrontSpriteRenderer;

    [SerializeField]
    private GameObject _announcementGameObject;
    [SerializeField]
    private SpriteRenderer _announcementSpriteRenderer;

    [SerializeField]
    private TextMesh _textName;
    [SerializeField]
    private TextMesh _textPosition;
    [SerializeField]
    private TextMesh _textSlideDirection;

    [SerializeField]
    private List<Sprite> _announcements = new List<Sprite>();

    public void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Update()
    {
        if (_slideDirection != MapManager.TileSlideDirection.None && _mapManager.EnableTileReplace)
        {
            if (_isDying)
            {
                _tileLifeCounter -= Time.deltaTime;

                if (_tileLifeCounter < 0)
                {
                    TriggerLiquidation();
                }
            }
            else if(_inLiquidation)
            {
                _liquidationDuration -= Time.deltaTime;

                if (_liquidationDuration < 0)
                {
                    TriggerStoreClosing();
                }
            }
        }
    }

    public void Init(MapManager mapManager, Vector3 tilePosition, MapManager.TileSlideDirection slideDirection, Sprite storeSprite)
    {
        _mapManager = mapManager;
        _tilePosition = tilePosition;
        _slideDirection = slideDirection;
        _storefrontSpriteRenderer.sprite = storeSprite;

        _announcementGameObject.SetActive(false);

        _tileLifeCounter = Random.Range(mapManager.MinExpireTime, mapManager.MaxExpireTime);
        _isDying = true;

        _liquidationDuration = Random.Range(mapManager.MinLiquidationTime, mapManager.MaxLiquidationTime);

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

    private void TriggerLiquidation()
    {
        _isDying = false;
        _inLiquidation = true;
        _announcementGameObject.SetActive(true);
        _announcementSpriteRenderer.sprite = _announcements[Random.Range(0, _announcements.Count)];
    }

    private void TriggerStoreClosing()
    {
        _inLiquidation = false;
        TriggerAnimation(_slideDirection, false);
        _mapManager.TriggerReplaceStore(this);
    }
}
