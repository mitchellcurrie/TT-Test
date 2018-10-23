using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// Maintains a single score counter
/// updates a GUIText component that must be attached to the same GameObject
public class Score : MonoBehaviour 
{
	Text textComponent;
	
	public static int Total = 0;
	
	void Start () 
	{
		textComponent = GetComponent<Text>() as Text;
	}
	
	void Update () 
	{
		textComponent.text = Score.Total.ToString();
	}
}
