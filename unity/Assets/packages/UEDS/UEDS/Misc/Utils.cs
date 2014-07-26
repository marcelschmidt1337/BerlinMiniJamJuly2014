using System;
using UnityEngine;

namespace UEDS
{
	public class Utils
	{
		private Utils ()
		{
		}
		
		
		public static Texture2D whiteTexture
		{
			get {
				if(mWhiteTexture == null)
				{
					mWhiteTexture = new Texture2D(4,4);
					for(int x = 0;x < 4;x++)
						for(int y = 0;y < 4;y++)
							mWhiteTexture.SetPixel(x,y,Color.white);
					mWhiteTexture.Apply();
					mWhiteTexture.Compress(true);
				}
				return mWhiteTexture;
			}
		}
		private static Texture2D mWhiteTexture;
	
		public static Texture2D blackTexture
		{
			get {
				if(mWhiteTexture == null)
				{
					mBlackTexture = new Texture2D(4,4);
					for(int x = 0;x < 4;x++)
						for(int y = 0;y < 4;y++)
							mBlackTexture.SetPixel(x,y,Color.black);
					mBlackTexture.Apply();
					mBlackTexture.Compress(true);
				}
				return mBlackTexture;
			}
		}
		private static Texture2D mBlackTexture;
		
		
	}
}

