using UnityEngine;

public class ColorPickerAdvanced : MonoBehaviour {

    Color[] Data;

    public float H, S, B;
    public Color Color;
    public string Hex;

    public int Width { get { return SpriteRenderer.sprite.texture.width; } }
    public int Height { get { return SpriteRenderer.sprite.texture.height; } }

    ControlManager ControlManager;
    Slider SpectrumSlider;
    
    SpriteRenderer SpriteRenderer;

    GameObject ColorPicker;
    GameObject Selector;
    BoxCollider2D Collider;

    void Start () {

        ControlManager = new ControlManager();

        ColorPicker = transform.Find("ColorPicker").gameObject;
        SpriteRenderer = ColorPicker.GetComponent<SpriteRenderer>();

        SpectrumSlider = transform.Find("Spectrum").GetComponent<Slider>();
        SpectrumSlider.OnSubmit += SpectrumSlider_OnSubmit;

        Selector = transform.Find("Selector").gameObject;
        Collider = ColorPicker.GetComponent<BoxCollider2D>();

        CreateHSBTexture(SliderValueToHSBColor());
    }

    //Return color based on slider position
    private HSBColor SliderValueToHSBColor()
    {
        return new HSBColor(SpectrumSlider.Value, 1, 1, 1);
    }

    private void SpectrumSlider_OnSubmit()
    {
        HSBColor color = SliderValueToHSBColor();
        CreateHSBTexture(color);

        color.s = S;
        color.b = B;

        UpdateColor(color);
    }

    void Update()
    {
        ControlManager.Update();
        
        if (Input.GetMouseButton(0))
        {
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
                        UpdateColor(Data[y * Width + x]);             
                    }
                    break;
                }
            }
        }
    }

    private void UpdateColor(Color color)
    {
        Color = color;
        Hex = ColorToHex(Color);

        HSBColor hsbColor = new HSBColor(color);
        H = hsbColor.h;
        S = hsbColor.s;
        B = hsbColor.b;
    }

    private void UpdateColor(HSBColor color)
    {
        Color = color.ToColor();
        Hex = ColorToHex(Color);

        H = color.h;
        S = color.s;
        B = color.b;
    }

    //Returns color selector is currently resting on
    private Color SampleSelector()
    {
        Vector2 screenPos = Selector.transform.position - ColorPicker.transform.position;
        int x = (int)(screenPos.x * Width);
        int y = (int)(screenPos.y * Height) + Height;

        if (x > 0 && x < Width && y > 0 && y < Height)
        {
            return Data[y * Width + x];
        }
        return Color.white;
    }

    //Converts a color to a hex string
    string ColorToHex(Color32 color)
    {
        string hex = "#" + color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
        return hex;
    }

    //Generates a 256x256 texture with all variations for the selected HUE
    void CreateHSBTexture(HSBColor color)
    {
        int size = 256;
        Texture2D texture = new Texture2D(size, size, TextureFormat.ARGB32, false);
        texture.filterMode = FilterMode.Point;

        Color[] textureData = new Color[size * size];
        color.s = 0;
        color.b = 1;

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                color.s = Mathf.Clamp(x / (float)(size - 1), 0, 1);
                color.b = Mathf.Clamp(y / (float)(size - 1), 0, 1);
                textureData[x + y * size] = color.ToColor();
            }
        }

        texture.SetPixels(textureData);
        texture.Apply();

        SpriteRenderer.sprite = Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(0f, 1f), 233);
        Data = textureData;
    }
}
