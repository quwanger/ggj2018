using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointParticle : MonoBehaviour {

    public SpriteRenderer spriteRenderer;

	public void Init(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
        Destroy(this.gameObject, 1.0f);
    }
}
