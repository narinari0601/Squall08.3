using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeTiling : MonoBehaviour
{
    public MeshRenderer wallMeshRenderer;

    public SpriteRenderer treeSpriteRenderer;

    public SpriteRenderer mapTreeSpriteRenderer;

    public SpriteRenderer groundSprite;

    public GameObject _gameObject;

    private float transformX;
    private float transformY;

    private float objectScaleX;
    private float objectScaleY;


    private const float spriteSize = 0.64f;
    private const float groundSize = 5.0f;


    // Start is called before the first frame update
    void Start()
    {
        //wallMeshRenderer.enabled = false;
        transformX = _gameObject.transform.localScale.x/2;
        transformY = _gameObject.transform.localScale.y/2;

        treeSpriteRenderer.size = new Vector2(transformX * spriteSize, transformY * spriteSize);
        mapTreeSpriteRenderer.size = new Vector2(transformX * spriteSize, transformY * spriteSize);
        if(groundSprite != null)
        {
            groundSprite.size = new Vector2(transformX * groundSize, transformY *groundSize);
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
