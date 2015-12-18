using UnityEngine;

public class ColorPickerSimple : MonoBehaviour {

    Color[] Data;
    SpriteRenderer SpriteRenderer;

    GameObject ColorPicker;
    GameObject Selector;
    BoxCollider2D Collider;

    public int Width { get { return SpriteRenderer.sprite.texture.width; } }
    public int Height { get { return SpriteRenderer.sprite.texture.height; } }

    public Color Color;

    void Awake () {

        ColorPicker = transform.Find("ColorPicker").gameObject;
        SpriteRenderer = ColorPicker.GetComponent<SpriteRenderer> ();		
        Selector = transform.Find("Selector").gameObject;
        Collider = ColorPicker.GetComponent<BoxCollider2D>();

        Data = SpriteRenderer.sprite.texture.GetPixels();

        Color = Color.white;
    }

  
    void Update () {

		if (Input.GetMouseButton (0)) {

			Vector3 screenPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);    
			screenPos = new Vector2(screenPos.x, screenPos.y);

            //check if we clicked this picker control
            RaycastHit2D[] ray = Physics2D.RaycastAll(screenPos, Vector2.zero, 0.01f);
            for (int i = 0; i < ray.Length; i++)
            {
                if (ray[i].collider == Collider)
                {                    
                    //move selector
                    Selector.transform.position = screenPos;

                    //get color data
                    screenPos -= ColorPicker.transform.position;
                    int x = (int)(screenPos.x * Width);
                    int y = (int)(screenPos.y * Height) + Height;

                    if (x > 0 && x < Width && y > 0 && y < Height)
                    {
                        Color = Data[y * Width + x];
                    }                   
                    break;
                }
            }
          
		}       
	}

}
