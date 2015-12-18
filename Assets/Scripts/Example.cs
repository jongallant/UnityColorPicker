//Example usage of the color picker
using UnityEngine;

public class Example : MonoBehaviour {

    public ColorPickerSimple ColorPicker;

    ColorSwatch Selected;
    ColorSwatch Complimentary;
    ColorSwatch Triad1;
    ColorSwatch Triad2;

    void Start () {
        Selected = transform.Find("Selected").GetComponent<ColorSwatch>();
        Complimentary = transform.Find("Complimentary").GetComponent<ColorSwatch>();
        Triad1 = transform.Find("Triad1").GetComponent<ColorSwatch>();
        Triad2 = transform.Find("Triad2").GetComponent<ColorSwatch>();
    }
	
	void Update () {

        Selected.Color = ColorPicker.Color;

        HSBColor hColor = new HSBColor(ColorPicker.Color);       

        if (hColor.h <= 0.5f)
            Complimentary.Color = new HSBColor(hColor.h + 0.5f, hColor.s, hColor.b).ToColor();
        else
            Complimentary.Color = new HSBColor(hColor.h - 0.5f, hColor.s, hColor.b).ToColor();

        if (hColor.h <= 0.33f)
        {
            Triad1.Color = new HSBColor(hColor.h + 0.33f, hColor.s, hColor.b).ToColor();
            Triad2.Color = new HSBColor(hColor.h + 0.66f, hColor.s, hColor.b).ToColor();
        }
        else if (hColor.h <= 0.66f)
        {
            Triad1.Color = new HSBColor(hColor.h - 0.33f, hColor.s, hColor.b).ToColor();
            Triad2.Color = new HSBColor(hColor.h + 0.33f, hColor.s, hColor.b).ToColor();
        }
        else
        {
            Triad1.Color = new HSBColor(hColor.h - 0.33f, hColor.s, hColor.b).ToColor();
            Triad2.Color = new HSBColor(hColor.h - 0.66f, hColor.s, hColor.b).ToColor();
        }
    }
}
