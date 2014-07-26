using System;
using UnityEngine;
using System.Reflection;
using UnityEditor;


namespace UGBSetup
{
	internal class LinkGameLogicToGameRoot : UGBSetupStep
	{
		public LinkGameLogicToGameRoot ()
		{
		}
		public override string GetName ()
		{
			return "Link GameLogic to GameRoot";
		}

		public override System.Collections.IEnumerator Run ()
		{
			yield return 0;
			Game instance = GameObject.FindObjectOfType<Game>();

			TextAsset ta = (TextAsset)AssetDatabase.LoadAssetAtPath( CreateGameLogicClass.LogicClassFile(), typeof(TextAsset) );
			instance.SetLogicImplementation(ta);
			EditorUtility.SetDirty(instance);
			yield return 0;
		}
	}
}

