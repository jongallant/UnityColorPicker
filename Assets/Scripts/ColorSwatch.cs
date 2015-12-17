using UnityEngine;
using System.Collections;

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
