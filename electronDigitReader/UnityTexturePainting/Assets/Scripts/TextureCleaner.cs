using UnityEngine;
using System.Collections;

public class TextureCleaner : MonoBehaviour 
{
	[SerializeField]
	private GameObject BrushContainer = null;


	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void ResetTexture()
	{
		if (this.BrushContainer != null) 
		{
			var parent = this.BrushContainer.transform;

			foreach(Transform child in parent) 
			{
				Destroy(child.gameObject);
			}
		}
	}
}
