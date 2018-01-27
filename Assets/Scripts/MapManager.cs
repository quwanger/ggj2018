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

    private static MapManager instance;

    private MapManager() { }

    public static MapManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new MapManager();
            }
            return instance;
        }
    }

    private const float SCREEN_WIDTH = 1920f;
    private const float SCREEN_HEIGHT = 1080f;
    private const float TILE_WIDTH = 480f;
    private const float TILE_HEIGHT = 270f;
    private const float MAP_WIDTH_TILE_COUNT = 3f;
    private const float MAP_HEIGHT_TILE_COUNT = 4f;
    private const int WIDTH_STARTING_POSITION = 1;
    private const int HEIGHT_STARTING_POSITION = 0;

    private const float SPRITE_HEIGHT_ORTHO_SIZE = 5f;  //this value is the orthographic size of the camera... don't ask
    private const float SPRITE_WIDTH_ORTHO_SIZE = SPRITE_HEIGHT_ORTHO_SIZE * (SCREEN_WIDTH / SCREEN_HEIGHT);

    [SerializeField]
    private Transform _mapParent;

    [SerializeField]
    private MapTile _baseMapTile;
    public MapTile BaseMapTile { get { return _baseMapTile; } }

    private MapTile[] _currentMapTiles = new MapTile[12];

    private void Awake()
    {
        InitializeMap();
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
            }
        }
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

    public void ReplaceStore(MapTile mapTile)
    {
        StartCoroutine("AnimateStoreBackIn", mapTile);
    }

    IEnumerator AnimateStoreBackIn(MapTile mapTile)
    {
        yield return new WaitForSeconds(0.3f);
        mapTile.TriggerAnimation(mapTile.SlideDirection, true);
        mapTile.Init(this, mapTile.TilePosition, mapTile.SlideDirection);
    }
}
