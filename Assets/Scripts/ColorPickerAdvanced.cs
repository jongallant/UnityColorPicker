using UnityEngine;
using System.Collections.Generic;

public class ColorPickerAdvanced : MonoBehaviour {

    Color[] Data;
    ColorSwatch Selected;

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
       
    Dictionary<HSBColor, TextureData> TextureCache = new Dictionary<HSBColor, TextureData>();
    Texture2D TextureBuffer;

    RaycastHit2D[] HitsBuffer = new RaycastHit2D[1];
    Camera Camera;

    int TextureSize = 256;

    void Start () {

        Camera = Camera.main;
        ControlManager = new ControlManager();

        Selected = transform.Find("SelectedColor").GetComponent<ColorSwatch>();

        TextureBuffer = new Texture2D(TextureSize, TextureSize, TextureFormat.ARGB32, false);
        TextureBuffer.filterMode = FilterMode.Point;

        ColorPicker = transform.Find("ColorPicker").gameObject;
        SpriteRenderer = ColorPicker.GetComponent<SpriteRenderer>();

        SpectrumSlider = transform.Find("Spectrum").GetComponent<Slider>();
        SpectrumSlider.OnSubmit += SpectrumSlider_OnSubmit;

        Selector = transform.Find("Selector").gameObject;
        Collider = ColorPicker.GetComponent<BoxCollider2D>();

        CreateHSBTexture(SliderValueToHSBColor(true));
    }

    //Return color based on slider position
    private HSBColor SliderValueToHSBColor(bool init = false)
    {        
        if (init)
        {
            return new HSBColor(SpectrumSlider.Value, 1, 1, 1);
        }
        else
        {
            Color sample = SampleSelector();
            return new HSBColor(SpectrumSlider.Value, sample.g, sample.b, sample.a);
        }        
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
            Vector3 screenPos = Camera.ScreenToWorldPoint(Input.mousePosition);
            screenPos = new Vector2(screenPos.x, screenPos.y);
            
            //check if we clicked this picker control
            int hitCount = Physics2D.RaycastNonAlloc(screenPos, Vector2.zero, HitsBuffer, 0.01f);

            for (int i = 0; i < hitCount; i++)
            {
                //Did we click the colorpicker?
                if (HitsBuffer[i].collider == Collider)
                {
                    //move selector
                    Selector.transform.position = screenPos;

                    //get color data
                    screenPos.x = screenPos.x - ColorPicker.transform.position.x;
                    screenPos.y = ColorPicker.transform.position.y - screenPos.y;

                    int x = (int)(screenPos.x * TextureSize / transform.localScale.x);
                    int y = TextureSize - (int)(screenPos.y * TextureSize / transform.localScale.y);

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

        // Remove this to reduce garbage.
        Hex = ColorToHex(Color);

        HSBColor hsbColor = new HSBColor(color);
        H = hsbColor.h;
        S = hsbColor.s;
        B = hsbColor.b;

        Selected.Color = Color;
    }

    private void UpdateColor(HSBColor color)
    {
        Color = color.ToColor();
        UpdateColor(Color);
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
        // Creates a lot of garbage -- do not call unless you need to pull this Hex value for saving, etc.       
        string hex = "#" + color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
        return hex;
    }

    //Generates a 256x256 texture with all variations for the selected HUE
    void CreateHSBTexture(HSBColor color)
    {
        //Already cached this texture so let's re=use it
        if (TextureCache.ContainsKey(color))
        {
            TextureData data = TextureCache[color];

            SpriteRenderer.sprite = data.Sprite;
            Data = data.ColorData;
        }
        else
        {
            //create this texture.
            Color[] textureData = new Color[TextureSize * TextureSize];
            HSBColor temp = new HSBColor(color.ToColor());
            temp.s = 0;
            temp.b = 1;

            for (int x = 0; x < TextureSize; x++)
            {
                for (int y = 0; y < TextureSize; y++)
                {
                    temp.s = Mathf.Clamp(x / (float)(TextureSize - 1), 0, 1);
                    temp.b = Mathf.Clamp(y / (float)(TextureSize - 1), 0, 1);
                    textureData[x + y * TextureSize] = temp.ToColor();
                }
            }

            TextureBuffer.SetPixels(textureData);
            TextureBuffer.Apply();

            SpriteRenderer.sprite = Sprite.Create(TextureBuffer, new Rect(0, 0, TextureSize, TextureSize), new Vector2(0f, 1f), TextureSize);
            Data = textureData;

            //store in cache
            TextureCache.Add(color, new TextureData(Data, SpriteRenderer.sprite));
        }
    }
}

public struct TextureData
{
    public Color[] ColorData;
    public Sprite Sprite;

    public TextureData(Color[] colorData, Sprite sprite)
    {
        ColorData = colorData;
        Sprite = sprite;
    }
}