using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapManager : MonoBehaviour {

    public enum TileSlideDirection
    {
        None = 0,
        Left = 1,
        Right = 2,
        Up = 3,
        Down = 4
    }

    private const float SCREEN_WIDTH = 1920f;
    private const float SCREEN_HEIGHT = 1080f;
    private const float TILE_WIDTH = 480f;
    private const float TILE_HEIGHT = 270f;
    private const float MAP_WIDTH_TILE_COUNT = 4f;
    private const float MAP_HEIGHT_TILE_COUNT = 4f;
    private const int WIDTH_STARTING_POSITION = 0;
    private const int HEIGHT_STARTING_POSITION = 0;
    private const float ESCALATOR_VERTICAL_OFFSET = 0.2f;
    private const int NUMBER_OF_ENTRANCES = 3;

    private const float SPRITE_HEIGHT_ORTHO_SIZE = 5f;  //this value is the orthographic size of the camera... don't ask
    private const float SPRITE_WIDTH_ORTHO_SIZE = SPRITE_HEIGHT_ORTHO_SIZE * (SCREEN_WIDTH / SCREEN_HEIGHT);


    [SerializeField]
    private bool _enableTileReplace = true;
    public bool EnableTileReplace { get { return _enableTileReplace; } }

    [SerializeField]
    private int _maxStoresInLiquidation;
    public int MaxStoresInLiquidation { get { return _maxStoresInLiquidation; } }

    [SerializeField]
    private float _minExpireTime;
    public float MinExpireTime { get { return _minExpireTime; } }
    [SerializeField]
    private float _maxExpireTime;
    public float MaxExpireTime { get { return _maxExpireTime; } }

    [SerializeField]
    private float _minLiquidationTime;
    public float MinLiquidationTime { get { return _minLiquidationTime; } }
    [SerializeField]
    private float _maxLiquidationTime;
    public float MaxLiquidationTime { get { return _maxLiquidationTime; } }

    [SerializeField]
    private float _minEscalatorTime;
    public float MinEscalatorTime { get { return _minEscalatorTime; } }
    [SerializeField]
    private float _maxEscalatorTime;
    public float MaxEscalatorTime { get { return _maxEscalatorTime; } }
    [SerializeField]
    private float _escalatorShutdownTime;
    public float EscalatorShutdownTime { get { return _escalatorShutdownTime; } }

    [SerializeField]
    private Transform _mapParent;

    [SerializeField]
    private Transform _escalatorParent;

    [SerializeField]
    private MapTile _baseMapTile;
    [SerializeField]
    private Escalator _escalator;

    [SerializeField]
    private List<Sprite> _storefronts = new List<Sprite>();

    [SerializeField]
    private List<SpawnerController> _entrances = new List<SpawnerController>();

    private List<Sprite> _availableStorefronts = new List<Sprite>();
    private List<Sprite> _storefrontsInUse = new List<Sprite>();

    private List<MapTile> _storesInLiquidation = new List<MapTile>();
    public List<MapTile> StoresInLiquidation { get { return _storesInLiquidation; } }

    public Transform[] _currentMapTiles = new Transform[16];
    private List<Escalator> _currentEscalators = new List<Escalator>();

    private List<EscalatorSpawn> _escalatorSpawns = new List<EscalatorSpawn>();

    private Escalator[,] _escalatorSlots;

    private void Awake()
    {
        InitStorefronts();
        InitializeMap();
        InitializeEntrances();
        InitializeEscalators();
    }

    public Vector2 GetRandomStorefrontPosition()
    {
        int randomTile = Random.Range(0, _currentMapTiles.Length);
        Vector2 randomStorePosition = new Vector2(_currentMapTiles[randomTile].position.x, _currentMapTiles[randomTile].position.y);
        return randomStorePosition;
    }

    private void InitStorefronts()
    {
        foreach(Sprite store in _storefronts)
        {
            _availableStorefronts.Add(store);
        }
    }

    private void InitializeMap()
    {
        for(int i = WIDTH_STARTING_POSITION; i < MAP_WIDTH_TILE_COUNT + WIDTH_STARTING_POSITION; i++)
        {
            for(int j = HEIGHT_STARTING_POSITION; j < MAP_HEIGHT_TILE_COUNT + HEIGHT_STARTING_POSITION; j++)
            {
                float posX = Mathf.Lerp(-SPRITE_WIDTH_ORTHO_SIZE, SPRITE_WIDTH_ORTHO_SIZE, Mathf.InverseLerp(0, SCREEN_WIDTH, (i * TILE_WIDTH) + (TILE_WIDTH / 2f)));

                float posY = Mathf.Lerp(-SPRITE_HEIGHT_ORTHO_SIZE, SPRITE_HEIGHT_ORTHO_SIZE, Mathf.InverseLerp(0, SCREEN_HEIGHT, (j * TILE_HEIGHT) + (TILE_HEIGHT / 2f)));

                Vector3 tilePosition = new Vector3(posX, posY, 2);
                MapTile tempMapTile = Instantiate(_baseMapTile, tilePosition, Quaternion.identity, _mapParent);
                tempMapTile.Init(this, tilePosition, GetTileSlideDirection(i, j), HandleStorefrontInit());
                _currentMapTiles[(i * (int)MAP_HEIGHT_TILE_COUNT) +  j] = tempMapTile.transform;

                if(i < MAP_WIDTH_TILE_COUNT - 1 && j < MAP_HEIGHT_TILE_COUNT - 1)
                {
                    float escPosX = Mathf.Lerp(-SPRITE_WIDTH_ORTHO_SIZE, SPRITE_WIDTH_ORTHO_SIZE, Mathf.InverseLerp(0, SCREEN_WIDTH, (i * TILE_WIDTH) + (TILE_WIDTH)));

                    float escPosY = Mathf.Lerp(-SPRITE_HEIGHT_ORTHO_SIZE, SPRITE_HEIGHT_ORTHO_SIZE, Mathf.InverseLerp(0, SCREEN_HEIGHT, (j * TILE_HEIGHT) + (TILE_HEIGHT / 2f)));
                    escPosY += ESCALATOR_VERTICAL_OFFSET;

                    Vector3 tempEscalatorSpawn = new Vector3(escPosX, escPosY, 0f);
                    _escalatorSpawns.Add(new EscalatorSpawn(tempEscalatorSpawn, j));
                }

            }
        }
    }

    private Sprite HandleStorefrontInit(Sprite currentStorefront = null)
    {
        Sprite storefront = GetUnusedStorefront();
        _storefrontsInUse.Add(storefront);
        _availableStorefronts.Remove(storefront);
        if (currentStorefront)
        {
            _availableStorefronts.Add(currentStorefront);
        }

        return storefront;
    }

    private Sprite GetUnusedStorefront()
    {
        return _availableStorefronts[Random.Range(0, _availableStorefronts.Count)];
    }

    private void InitializeEntrances()
    {
        //turn all off to start...
        foreach(SpawnerController entrance in _entrances)
        {
            entrance.gameObject.SetActive(false);
        }

        for(int i = 0; i < NUMBER_OF_ENTRANCES; i++)
        {
            int entranceToTurnOn = Random.Range(0, _entrances.Count);
            if (_entrances[entranceToTurnOn].gameObject.activeSelf)
            {
                i--;
            }
            else
            {
                _entrances[entranceToTurnOn].gameObject.SetActive(true);
            }
        }
    }

    public List<NpcExit> GetActiveExits()
    {
        List<NpcExit> exits = new List<NpcExit>();

        foreach (SpawnerController entrance in _entrances)
        {
            if(entrance.gameObject.activeSelf)
            {
                exits.Add(entrance.GetComponentInChildren<NpcExit>());
            }
        }

        return exits;
    }

    private void InitializeEscalators()
    {
        _escalatorSlots = new Escalator[(int)MAP_WIDTH_TILE_COUNT - 1, (int)MAP_HEIGHT_TILE_COUNT - 1];

        for (int m = 0; m < MAP_WIDTH_TILE_COUNT-1; m++)
        {
            for (int n = 0; n < MAP_HEIGHT_TILE_COUNT-1; n++)
            {
                _escalatorSlots[m, n] = null;
            }
        }

        for (int i = 0; i < MAP_HEIGHT_TILE_COUNT - 1; i++)
        {
            List<EscalatorSpawn> escalatorsOnFloor = GetEscalatorsForFloor(i);

            if(escalatorsOnFloor.Count > 0)
            {
                SpawnEscalatorsForFloor(escalatorsOnFloor, i);
            }
            else
            {
                Debug.LogError("MapManager InitializeEscalators: No EscalatorSpawns on floor " + i.ToString() + ".");
            }
        }
    }

    private void SpawnEscalatorsForFloor(List<EscalatorSpawn> escalatorSpawns, int floor)
    {
        int escalatorsSpawnedOnFloor = 0;

        for(int i = 0; i < escalatorSpawns.Count; i++)
        {
            if (TrySpawnEscalator(escalatorSpawns[i], escalatorSpawns.Count, i, floor))
            {
                escalatorsSpawnedOnFloor++;
            }
        }

        //if none spawn, just keep trying until one does
        while(escalatorsSpawnedOnFloor < 1)
        {
            int escNum = Random.Range(0, escalatorSpawns.Count);
            if (TrySpawnEscalator(escalatorSpawns[escNum], escalatorSpawns.Count, escNum, floor))
            {
                escalatorsSpawnedOnFloor++;
            }
        }
    }

    private bool CheckBelow(int slot, int floor, Escalator.EscalatorDirectionHorizontal dirH)
    {
        if(floor > 0)
        {
            if(_escalatorSlots[slot, floor - 1] != null)
            {
                if(_escalatorSlots[slot, floor - 1].EscDirectionHorizontal == dirH)
                {
                    return true;
                }

                return false;
            }
        }

        return true;
    }

    private bool TrySpawnEscalator(EscalatorSpawn escalatorSpawn, int escalatorSpawnsOnFloor, int slot, int floor)
    {
        bool spawnEscalator = Random.Range(0, escalatorSpawnsOnFloor) > (escalatorSpawnsOnFloor - 2) && !escalatorSpawn.IsTaken;

        if (spawnEscalator)
        {
            Escalator.EscalatorDirectionHorizontal dirH = Random.Range(0, 2) > 0 ? Escalator.EscalatorDirectionHorizontal.Right : Escalator.EscalatorDirectionHorizontal.Left;
            spawnEscalator = CheckBelow(slot, floor, dirH);
            if (spawnEscalator)
            {
                Escalator tempEscalator = Instantiate(_escalator, escalatorSpawn.SpawnPosition, Quaternion.identity, _escalatorParent);
                tempEscalator.transform.parent.SetAsFirstSibling();
                escalatorSpawn.IsTaken = true;
                _currentEscalators.Add(tempEscalator);
                tempEscalator.Init(this, Random.Range(0, 2) > 0 ? Escalator.EscalatorDirectionVertical.Up : Escalator.EscalatorDirectionVertical.Down, dirH, floor);
                _escalatorSlots[slot, floor] = tempEscalator;
            }
        }

        return spawnEscalator;
    }

    public Vector2 GetEscalatorFromFloorToFloor(int from, int to, bool down)
    {
        foreach (Escalator escalator in _currentEscalators)
        {
            if(escalator.FromFloor == from && escalator.ToFloor == to)
            {
                Vector2 target = new Vector2(down ? escalator.TargetTop.position.x : escalator.TargetBottom.position.x, down ? escalator.TargetTop.position.y : escalator.TargetBottom.position.y);
                return target;
            }
        }

        return Vector2.zero;
    }

    private List<EscalatorSpawn> GetEscalatorsForFloor(int floor)
    {
        List<EscalatorSpawn> escalatorsOnFloor = new List<EscalatorSpawn>();

        foreach(EscalatorSpawn escSpawn in _escalatorSpawns)
        {
            if (escSpawn.MallFloor == floor)
            {
                escalatorsOnFloor.Add(escSpawn);
            }
        }

        return escalatorsOnFloor;
    }
 
    private TileSlideDirection GetTileSlideDirection(int x, int y)
    {
        if(x == WIDTH_STARTING_POSITION)
        {
            return TileSlideDirection.Left;
        }
        else if(x == (MAP_WIDTH_TILE_COUNT + WIDTH_STARTING_POSITION) - 1)
        {
            return TileSlideDirection.Right;
        }
        else if(y == HEIGHT_STARTING_POSITION)
        {
            return TileSlideDirection.Down;
        }
        else if(y == (MAP_HEIGHT_TILE_COUNT + HEIGHT_STARTING_POSITION) - 1)
        {
            return TileSlideDirection.Up;
        }

        return TileSlideDirection.None;
    }

    public void TriggerLiquidation(MapTile store)
    {
        _storesInLiquidation.Add(store);
    }
    public void TriggerReplaceStore(MapTile mapTile)
    {
        _storesInLiquidation.Remove(mapTile);
        StartCoroutine("ReplaceStore", mapTile);
    }

    IEnumerator ReplaceStore(MapTile mapTile)
    {
        yield return new WaitForSeconds(0.15f);
        StartCoroutine("AnimateStoreBackIn", mapTile);
    }

    IEnumerator AnimateStoreBackIn(MapTile mapTile)
    {
        yield return new WaitForSeconds(0.3f);
        mapTile.TriggerAnimation(mapTile.SlideDirection, true);
        mapTile.Init(this, mapTile.TilePosition, mapTile.SlideDirection, HandleStorefrontInit(mapTile.CurrentSprite));
    }
}
