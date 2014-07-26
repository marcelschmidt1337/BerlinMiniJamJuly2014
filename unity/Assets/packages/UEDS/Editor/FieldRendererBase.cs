using System;
using UEDS;
using UnityEditor;
using UnityEngine;

public abstract class FieldRendererBase
{
	static GUISkin mDirtySkin;
	static Texture2D mHelpIcon;
	static GUIContent mHelpContent;
	System.Type mHandledType;

	public FieldRendererBase (System.Type pHandlingType)
	{
		mHandledType = pHandlingType;
	}

	public bool HandlesType(System.Type pType)
	{
		return mHandledType.IsAssignableFrom(pType);
	}

	protected bool mShowDescription = false;

	public void RenderDescriptionIcon()
	{
		if(mHelpContent == null)
		{
			mHelpContent = new GUIContent();
			if(mHelpIcon == null)
			{
				mHelpIcon = (Texture2D)AssetDatabase.LoadAssetAtPath(UEDSStyles.kImagePath + "help.png", typeof(Texture2D));
			}
			if(mHelpIcon == null)
				Debug.LogError("help icon not found");
			
			mHelpContent.image = mHelpIcon;
			mHelpContent.tooltip = "Click to see more information. ";
		}
		
		if( GUILayout.Button(mHelpContent,UEDSStyles.settingDescription))
		{
			mShowDescription = !mShowDescription;
		}
	}
	public virtual void RenderDescription(UEDS.Setting pSetting)
	{

		if(!mShowDescription)
			return;

		GUILayout.BeginVertical(UEDSStyles.infoGroup);

		if(pSetting.Description != null)
			EditorGUILayout.HelpBox(pSetting.Description,MessageType.Info);

		GUILayout.Label("Property Name: " + pSetting.mSettingName,EditorStyles.whiteMiniLabel);
		GUILayout.Label("Property Type: " + pSetting.mType.ToString(),EditorStyles.whiteMiniLabel);
		GUILayout.Label("Property ValueType: " + pSetting.mValueType.ToString(),EditorStyles.whiteMiniLabel);
		GUILayout.Label("Property Default Value: " + pSetting.GetDefault(pSetting.mValueType).ToString(),EditorStyles.whiteMiniLabel);

		GUILayout.EndVertical();


	}
	bool renderedDirty;
	public virtual void PreEditorGUI(UEDS.Setting pSetting)
	{
		renderedDirty = pSetting.isDirty;

		if(renderedDirty)
			GUILayout.BeginHorizontal("TE NodeBoxSelected");
		else
			GUILayout.BeginHorizontal("TE NodeBox");
	}


	public virtual void PostEditorGUI(UEDS.Setting pSetting)
	{
		GUILayout.EndHorizontal();


	}

	public abstract void OnEditorGUI(UEDS.Setting pSetting);

}


