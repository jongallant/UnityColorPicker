using UnityEngine;

public class ColorPickerSimple : MonoBehaviour
{    
    Color[] Data;
    bool Selected;

    SpriteRenderer SpriteRenderer;
    GameObject ColorPicker;
    GameObject Selector;
    BoxCollider2D Collider;

    public int Width { get { return SpriteRenderer.sprite.texture.width; } }
    public int Height { get { return SpriteRenderer.sprite.texture.height; } }

    public Color Color;
    Camera Camera;

    RaycastHit2D[] HitsBuffer = new RaycastHit2D[1];

    void Awake () {

        Camera = Camera.main;

        ColorPicker = transform.Find("ColorPicker").gameObject;
        SpriteRenderer = ColorPicker.GetComponent<SpriteRenderer> ();		
        Selector = transform.Find("Selector").gameObject;
        Collider = ColorPicker.GetComponent<BoxCollider2D>();

        Data = SpriteRenderer.sprite.texture.GetPixels();

        Color = Color.white;
    }

  
    void Update () {

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 screenPos = Camera.ScreenToWorldPoint(Input.mousePosition);
            int hitCount = Physics2D.RaycastNonAlloc(screenPos, Vector2.zero, HitsBuffer, 0.01f);

            for (int i = 0; i < hitCount; i++)
            {
                if (HitsBuffer[i].collider == Collider)
                {
                    Selected = true;
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Selected = false;
        }

        if (Selected && Input.GetMouseButton(0))
        {
            Vector3 screenPos = Camera.ScreenToWorldPoint(Input.mousePosition);
            screenPos.z = 0;

            screenPos.x = Mathf.Clamp(screenPos.x, transform.position.x, transform.position.x + transform.localScale.x);
            screenPos.y = Mathf.Clamp(screenPos.y, transform.position.y - transform.localScale.y, transform.position.y);

            Selector.transform.position = screenPos;

            //get color data
            screenPos.x = screenPos.x - ColorPicker.transform.position.x;
            screenPos.y = ColorPicker.transform.position.y - screenPos.y;

            int x = (int)(screenPos.x * Width / transform.localScale.x);
            int y = Height - (int)(screenPos.y * Height / transform.localScale.y);

            if (x == Width)
                x -= 1;
            if (y == Height)
                y -= 1;

            if (x >= 0 && x < Width && y >= 0 && y < Height)
            {
                Color = Data[y * Width + x];
            }
        }     
	}

}
