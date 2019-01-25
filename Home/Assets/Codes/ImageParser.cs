using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageParser : MonoBehaviour
{
    public TextAsset imageLevel;

    public GameObject floorAndWall;

    // Build level from reading pixels
    /*  Black = Wall/Floor
        Red   = Enemy
     */

    public void Start ()
    {
        ParseImage (imageLevel);
    }

    public void ParseImage (TextAsset image)
    {
        Texture2D tex = new Texture2D (250, 250);
        tex.LoadImage (image.bytes);

        for (int x = 0; x < tex.width; x++)
        {
            for (int y = 0; y < tex.height; y++)
            {
                Color pixel = tex.GetPixel (x, y);
                if (pixel == Color.black)
                {
                    Debug.Log ("at :" + x + ":" + y);
                    Instantiate (floorAndWall, new Vector2 (x, y), Quaternion.identity);
                }
            }
        }
    }
}