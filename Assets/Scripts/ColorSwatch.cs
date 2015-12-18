//Simple class to display a color
using UnityEngine;

public class ColorSwatch : MonoBehaviour {

	SpriteRenderer Sprite;
	public Color Color { 
		get { return Sprite.color; } 
		set { Sprite.color = value; }
	}

	void Start () {
		Sprite = GetComponent<SpriteRenderer> ();
	}

	void Update () {
	
	}
}
