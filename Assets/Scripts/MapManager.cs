using System.Collections;
using System.Collections.Generic;
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

    private const float SPRITE_HEIGHT_ORTHO_SIZE = 5f;  //this value is the orthographic size of the camera... don't ask
    private const float SPRITE_WIDTH_ORTHO_SIZE = SPRITE_HEIGHT_ORTHO_SIZE * (SCREEN_WIDTH / SCREEN_HEIGHT);


    [SerializeField]
    private bool _enableTileReplace = true;
    public bool EnableTileReplace { get { return _enableTileReplace; } }

    [SerializeField]
    private Transform _mapParent;

    [SerializeField]
    private Transform _escalatorParent;

    [SerializeField]
    private MapTile _baseMapTile;
    [SerializeField]
    private Escalator _escalator;

    private MapTile[] _currentMapTiles = new MapTile[12];
    private List<Escalator> _currentEscalators = new List<Escalator>();

    private List<EscalatorSpawn> _escalatorSpawns = new List<EscalatorSpawn>();

    private void Awake()
    {
        InitializeMap();
        InitializeEscalators();
    }

    private void InitializeMap()
    {
        for(int i = WIDTH_STARTING_POSITION; i < MAP_WIDTH_TILE_COUNT + WIDTH_STARTING_POSITION; i++)
        {
            for(int j = HEIGHT_STARTING_POSITION; j < MAP_HEIGHT_TILE_COUNT + HEIGHT_STARTING_POSITION; j++)
            {
                float posX = Mathf.Lerp(-SPRITE_WIDTH_ORTHO_SIZE, SPRITE_WIDTH_ORTHO_SIZE, Mathf.InverseLerp(0, SCREEN_WIDTH, (i * TILE_WIDTH) + (TILE_WIDTH / 2f)));

                float posY = Mathf.Lerp(-SPRITE_HEIGHT_ORTHO_SIZE, SPRITE_HEIGHT_ORTHO_SIZE, Mathf.InverseLerp(0, SCREEN_HEIGHT, (j * TILE_HEIGHT) + (TILE_HEIGHT / 2f)));

                Vector3 tilePosition = new Vector3(posX, posY, 0);
                MapTile tempMapTile = Instantiate(_baseMapTile, tilePosition, Quaternion.identity, _mapParent);
                tempMapTile.Init(this, tilePosition, GetTileSlideDirection(i, j));
                _currentMapTiles[(i- WIDTH_STARTING_POSITION) * (j - HEIGHT_STARTING_POSITION)] = tempMapTile;

                if(i < MAP_WIDTH_TILE_COUNT - 1 && j < MAP_HEIGHT_TILE_COUNT - 1)
                {
                    float escPosX = Mathf.Lerp(-SPRITE_WIDTH_ORTHO_SIZE, SPRITE_WIDTH_ORTHO_SIZE, Mathf.InverseLerp(0, SCREEN_WIDTH, (i * TILE_WIDTH) + (TILE_WIDTH)));

                    float escPosY = Mathf.Lerp(-SPRITE_HEIGHT_ORTHO_SIZE, SPRITE_HEIGHT_ORTHO_SIZE, Mathf.InverseLerp(0, SCREEN_HEIGHT, j * TILE_HEIGHT));

                    Vector3 tempEscalatorSpawn = new Vector3(escPosX, escPosY, 0f);
                    _escalatorSpawns.Add(new EscalatorSpawn(tempEscalatorSpawn, j));
                }

            }
        }
    }

    private void InitializeEscalators()
    {
        for (int i = 0; i < MAP_HEIGHT_TILE_COUNT - 1; i++)
        {
            List<EscalatorSpawn> escalatorsOnFloor = GetEscalatorsForFloor(i);

            if(escalatorsOnFloor.Count > 0)
            {
                SpawnEscalatorsForFloor(escalatorsOnFloor);
            }
            else
            {
                Debug.LogError("MapManager InitializeEscalators: No EscalatorSpawns on floor " + i.ToString() + ".");
            }
        }
    }

    private void SpawnEscalatorsForFloor(List<EscalatorSpawn> escalatorSpawns)
    {
        int escalatorsSpawnedOnFloor = 0;

        foreach (EscalatorSpawn escalatorSpawn in escalatorSpawns)
        {
            if (TrySpawnEscalator(escalatorSpawn, escalatorSpawns.Count))
            {
                escalatorsSpawnedOnFloor++;
            }
        }

        while(escalatorsSpawnedOnFloor < 1)
        {
            TrySpawnEscalator(escalatorSpawns[Random.Range(0, escalatorSpawns.Count)], escalatorSpawns.Count);
        }
    }

    private bool TrySpawnEscalator(EscalatorSpawn escalatorSpawn, int escalatorSpawnsOnFloor)
    {
        bool spawnEscalator = Random.Range(0, escalatorSpawnsOnFloor) > (escalatorSpawnsOnFloor - 2) && !escalatorSpawn.IsTaken;

        if (spawnEscalator)
        {
            Escalator tempEscalator = Instantiate(_escalator, escalatorSpawn.SpawnPosition, Quaternion.identity, _escalatorParent);
            escalatorSpawn.IsTaken = true;
            _currentEscalators.Add(tempEscalator);
            tempEscalator.Init(Random.Range(0, 2) > 0 ? Escalator.EscalatorDirection.Up : Escalator.EscalatorDirection.Down);
        }

        return spawnEscalator;
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

    public void TriggerReplaceStore(MapTile mapTile)
    {
        StartCoroutine("ReplaceStore", mapTile);
    }

    IEnumerator ReplaceStore(MapTile mapTile)
    {
        yield return new WaitForSeconds(0.15f);
        mapTile.StoreName = "Store " + Random.Range(1, 100).ToString();
        StartCoroutine("AnimateStoreBackIn", mapTile);
    }

    IEnumerator AnimateStoreBackIn(MapTile mapTile)
    {
        yield return new WaitForSeconds(0.3f);
        mapTile.TriggerAnimation(mapTile.SlideDirection, true);
        mapTile.Init(this, mapTile.TilePosition, mapTile.SlideDirection);
    }
}
