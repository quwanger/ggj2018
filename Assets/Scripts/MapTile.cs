using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTile : MonoBehaviour {

    private Vector3 _tilePosition;
    public Vector3 TilePosition { get { return _tilePosition; }}

    private MapManager.TileSlideDirection _slideDirection;
    public MapManager.TileSlideDirection SlideDirection { get { return _slideDirection; }}

    [SerializeField]
    private TextMesh _textPosition;
    [SerializeField]
    private TextMesh _textSlideDirection;

    public void Init(Vector3 tilePosition, MapManager.TileSlideDirection slideDirection)
    {
        _tilePosition = tilePosition;
        _slideDirection = slideDirection;

        _textPosition.text = "(" + _tilePosition.x.ToString() + ", " + _tilePosition.y + ")";
        _textSlideDirection.text = slideDirection.ToString();
    }
}
