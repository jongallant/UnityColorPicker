using UnityEngine;

public class ColorPicker : MonoBehaviour {
            
    Color[] Data;
    SpriteRenderer Sprite;

    public int Width { get { return Sprite.sprite.texture.width; } }
    public int Height { get { return Sprite.sprite.texture.height; } }

    public Color Color;

    void Awake () {
		Sprite = GetComponent<SpriteRenderer> ();
		Data = Sprite.sprite.texture.GetPixels ();
	}
	
	void Update () {

		if (Input.GetMouseButton (0)) {

			Vector3 screenPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);    
			screenPos = new Vector2(screenPos.x, screenPos.y);

			screenPos -= transform.position;

			int x = (int)(screenPos.x * Width);
			int y = (int)(screenPos.y * Height) + Height;

			if (x > 0 && x < Width && y > 0 && y < Height)
			{
				Color = Data[y * Sprite.sprite.texture.width + x];
			}
		}       
	}


}
