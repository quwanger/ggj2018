using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTile : MonoBehaviour
{
    private Animator _animator;
    private AudioManager audioManager;

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
    public Sprite CurrentSprite { get { return _storefrontSpriteRenderer.sprite; } }

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

    public GameObject brickPS;
    public void Awake()
    {
        _animator = GetComponent<Animator>();
        brickPS.SetActive(false);
        audioManager = FindObjectOfType<AudioManager>();
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
                _announcementGameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(-3.0f, 3.0f)));
                if(_liquidationDuration <= 1.0f) {
                    if(brickPS.active == false)
                    {
                        audioManager.PlaySound("drilling");
                    }
                    brickPS.SetActive(true);
                    transform.rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(-3.0f, 3.0f)));
                }
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

        ResetLifeSpan();
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

        audioManager.PlaySound("swoosh");
    }

    private void ResetLifeSpan()
    {
        _tileLifeCounter = Random.Range(_mapManager.MinExpireTime, _mapManager.MaxExpireTime);
        brickPS.SetActive(false);
        transform.rotation = Quaternion.identity;
    }

    private void TriggerLiquidation()
    {
        if (_mapManager.StoresInLiquidation.Count < _mapManager.MaxStoresInLiquidation)
        {
            int saleStrength = Random.Range(1, 11);
            GameManager.Instance.NPCManager.SendNPCsToSale(saleStrength, transform);
            _liquidationDuration = Random.Range(_mapManager.MinLiquidationTime, _mapManager.MaxLiquidationTime);
            _mapManager.TriggerLiquidation(this);
            _isDying = false;
            _inLiquidation = true;
            _announcementGameObject.SetActive(true);
            _announcementSpriteRenderer.sprite = _announcements[Random.Range(0, _announcements.Count)];

            string word = _announcementSpriteRenderer.sprite.name;
            switch (word)
            {
                case "message_everything":
                    audioManager.PlaySound("everything");
                    break;

                case "message_blackfriday":
                    audioManager.PlaySound("black friday");
                    break;

                case "message_clearance":
                    audioManager.PlaySound("clearance");
                    break;

                case "message_endofseason":
                    audioManager.PlaySound("end of season");
                    break;

                case "message_firesale":
                    audioManager.PlaySound("fire sale");
                    break;

                case "message_liquidation":
                    audioManager.PlaySound("liquidation");
                    break;

                case "message_obama":
                    audioManager.PlaySound("obama");
                    break;
            }
            
        }
        else
        {
            ResetLifeSpan();
        }
    }

    private void TriggerStoreClosing()
    {
        _inLiquidation = false;
        TriggerAnimation(_slideDirection, false);
        _mapManager.TriggerReplaceStore(this);
    }
}
