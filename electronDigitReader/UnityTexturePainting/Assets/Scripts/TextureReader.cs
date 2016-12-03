using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.Linq;

public class TextureReader : MonoBehaviour 
{
	[SerializeField]
	private RenderTexture RenderTexture = null;

//	[DllImport("__Internal")]
//	private static extern void Hello();
//
//	[DllImport("__Internal")]
//	private static extern void HelloString(string str);
//
//	[DllImport("__Internal")]
//	private static extern void PrintFloatArray(float[] array, int size);

	[DllImport("__Internal")]
	private static extern void GetSomeFloatArray(float[] array, int size, int width, int height);


	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void ReadTexture()
	{
		if (this.RenderTexture != null) 
		{
			RenderTexture.active = this.RenderTexture;
			Texture2D tex = new Texture2D(RenderTexture.active.width, RenderTexture.active.height, TextureFormat.RGB24, false);
			tex.ReadPixels(new Rect(0, 0, RenderTexture.active.width, RenderTexture.active.height), 0, 0);
			tex.anisoLevel = 0;
			tex.Apply();

			var bytes = tex.GetRawTextureData();
			var floats = bytes.Select (x => (float)x).ToArray();

//			HelloString ("salut");
			GetSomeFloatArray (floats, floats.Length, RenderTexture.active.width, RenderTexture.active.height);



 //	        const string Path = @"C:\Users\codec\Desktop";
//			var bytes2 = tex.EncodeToPNG();
//			System.IO.File.WriteAllBytes(System.IO.Path.Combine(Path, "myTexture.png"), bytes2);
//			System.IO.File.WriteAllBytes(System.IO.Path.Combine(Path, "lol.png"), bytes);
		}
	}
}
