using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SpriteScript : MonoBehaviour
{
    // Start is called before the first frame update
    public void LoadSprite()
    {
        Debug.Log(ExternalLoader.getPath() +"\\Sprites\\player.png");
        Texture2D SpriteTexture = ExternalLoader.LoadTexture(ExternalLoader.getPath()+"\\Sprites\\player.png");
        SpriteTexture.filterMode = FilterMode.Point;
        UnityEngine.Sprite newsprite = UnityEngine.Sprite.Create(SpriteTexture, new Rect(0,0, SpriteTexture.width, SpriteTexture.height),  new Vector2(0.5f, 0.5f), 100f, 0 , SpriteMeshType.Tight);
        this.GetComponent<SpriteRenderer>().sprite = newsprite;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
