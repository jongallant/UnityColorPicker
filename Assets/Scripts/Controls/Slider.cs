using UnityEngine;

public class Slider : BaseControl
{
    public delegate void SubmitAction();
    public event SubmitAction OnSubmit;

    public float Value;
    
    float MinValue = 0;
    float MaxValue = 1;

    private bool PreviousPressed;
    private float Size;
    private GameObject Background;
    private GameObject Knob;
    private float MaxExtent;
    private BoxCollider2D BoxCollider;
    private float Min, Max;
    private float NormalizedValue;

    void Awake () {
        FindGameObjects();
        Refresh();
    }

    private void FindGameObjects()
    {
        Background = transform.Find("Background").gameObject;
        Knob = transform.Find("Knob").gameObject;
        BoxCollider = GetComponent<BoxCollider2D>();
    }

    void Update () {      
                          
        if (Pressed)
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos -= Offset;
            SetKnobPosition(pos.y);
            Submit();
        }
        else if (PreviousPressed && !Pressed)
        {
            Submit();
        }

        PreviousPressed = Pressed;
	}

    public override void Submit()
    {
        if (OnSubmit != null)
            OnSubmit();
    }

    public override void Refresh()
    {
        float extent = Background.GetComponent<SpriteRenderer>().bounds.max.y - Background.GetComponent<SpriteRenderer>().bounds.min.y;
        MaxExtent = extent / 2f;

        Size = Background.GetComponent<SpriteRenderer>().sprite.texture.height;

        Min = Background.GetComponent<SpriteRenderer>().bounds.min.y;
        Max = Background.GetComponent<SpriteRenderer>().bounds.max.y;
        
        Offset = transform.position;

        NormalizedValue = 0.5f;

        float knobValue = (NormalizedValue - 0.5f) * Size * 2f;
        knobValue = Mathf.Clamp(knobValue, -MaxExtent, MaxExtent);

        if (Knob.transform.localPosition.y != knobValue)
        {
            Knob.transform.localPosition = new Vector2(0, knobValue);
        }

        Value = NormalizedValue * (MaxValue - MinValue);
        BoxCollider.offset = Knob.transform.localPosition;
    }

    private void SetKnobPosition(float position)
    {
        Knob.transform.position = new Vector2(transform.position.x, Mathf.Clamp(position, -MaxExtent, MaxExtent) + Offset.y);
        
        NormalizedValue = (Knob.transform.position.y - Offset.y + MaxExtent) / MaxExtent / 2f;
        Value = NormalizedValue * (MaxValue - MinValue);

        BoxCollider.offset = Knob.transform.localPosition;
    }
    
    protected float CalculatePixelUnits(Sprite sprite)
    {
        return sprite.rect.width / sprite.bounds.size.x;
    }
}
