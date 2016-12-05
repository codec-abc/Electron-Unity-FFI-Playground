using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.Linq;

public class TextureReader : MonoBehaviour 
{
	[SerializeField]
	private RenderTexture RenderTexture = null;

	[DllImport("__Internal")]
	private static extern void SendRGBTexture(float[] array, int size, int width, int height);

	[DllImport("CPPEvalClientDll")]
	private static extern int analyze_digits(float[] array, float[] result);


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

			var textureSize = 28;

			TextureScale.Bilinear (tex, textureSize, textureSize);


			#if !UNITY_EDITOR
			var bytes = tex.GetRawTextureData();
			var floats = bytes.Select (x => (float)x).ToArray();

			SendRGBTexture(floats, floats.Length, textureSize, textureSize);
			#else

			var bytes = tex.GetRawTextureData();
			var rgbTextureAsFloatArray = bytes.Select (x => (float)x).ToArray();
			var floats = new float[28 * 28];

			for (int i = 0; i < 28; i++)
			{
				for (var j = 0; j < 28; j++)
				{ 
					var baseIndex = i * 28 * 3 + j * 3;
					var r = rgbTextureAsFloatArray[baseIndex];
					var b = rgbTextureAsFloatArray[baseIndex + 1];
					var g = rgbTextureAsFloatArray[baseIndex + 2];
					floats[i * 28 + j] = (r + g + b ) /3.0f;
					floats[i * 28 + j] = 255.0f - floats[i * 28 + j];
				}
			}


			var result = new float[10];
			var floats_ = new float[28 * 28];
			for (int i = 0; i < 28; i++)
			{
				for (var j = 0; j < 28; j++)
				{ 
					var beginningLine = i * 28;
					var newIndex = beginningLine + 27 - j;
					floats_[newIndex] = floats[i * 28 + j];
//					floats_[newIndex] = floats_[newIndex] * 2;
//					if (floats_[newIndex] > 255.0f)
//					{
//						floats_[newIndex] = 255.0f;
//					}
				}
			}



			var returned_code = analyze_digits(floats_, result);

			float max_proba = -999;
			var max_proba_digit = -1;
			var msg = "";
			var total = 0;
			for (int i = 0; i < 28; i++)
			{
				var msg_ = "";
				for (var j = 0; j < 28; j++)
				{
					//msg_ += ((int)floats_[total]).ToString("D3") + " ";
					msg_ += ((int)floats_[total]) + " ";
					total++;
				}
				msg = msg + msg_ + "\r\n";
			}
			Debug.Log(msg);



//			var currentDigit = 0;
//			foreach(var proba in result)
//			{
//				Debug.Log("value for " + currentDigit + " is " + proba);
//				currentDigit++;
//			}

			var msg2 = "";
			for (var i = 0; i < 10; i++)
			{
				var proba = result[i];

				if (proba > max_proba)
				{
					max_proba = proba;
					max_proba_digit = i;
				}

				msg2 += proba + "\r\n";
			}

			Debug.Log(msg2);

			Debug.Log("digit is " + max_proba_digit + " with proba " + result[max_proba_digit]);

			#endif
		}
	}
}
