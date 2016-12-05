using UnityEngine;
using System.Collections;

public class TextureCleaner : MonoBehaviour 
{
	[SerializeField]
	private GameObject BrushContainer = null;

	[SerializeField]
	private GameObject BlankReset = null;

	[SerializeField]
	private TexturePainter Painter;

	private int frameCount = 0;



	// Use this for initialization
	void Start ()
	{
		this.frameCount = 30;
		this.BlankReset.gameObject.transform.position = new Vector3 (77.1f, 0, 32.5f);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (this.frameCount > 0) 
		{
			this.frameCount--;

			if (this.frameCount == 1) 
			{
				if (this.Painter != null) 
				{
					this.Painter.SaveTexture ();
				}
			}

			if (this.frameCount == 0) 
			{
				if (this.BlankReset != null) 
				{
					this.BlankReset.gameObject.transform.position = new Vector3 (77.1f, 0, 36.5f);
				}
			}
		}
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

		if (this.BlankReset != null) 
		{
			this.frameCount = 3;
			this.BlankReset.gameObject.transform.position = new Vector3 (77.1f, 0, 32.5f);
		}
	}
}
