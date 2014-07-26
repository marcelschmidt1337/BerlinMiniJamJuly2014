using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UEDS;

/*! \page ueds Unity Editor Defaults System
*  
* The UEDS provides an editor Window, which is accessible through the menu of the Unity Editor "UGB / Preferences". 
* Custom Editor Components can us it as a global settings store which is accessible during editor- and runtime. 
* It is extensible with custom renderers and storage classes. 
*  
* \section ueds-storing-data Storing data
*  
* To use the UEDS in your class you simply add a tag to your class and all fields/properties, 
* which you want to store inside UEDS. 
* 
* \include ueds/ex01.cs
*/

/// <summary>
/// UEDS window.
/// </summary>
public class UEDSWindow : EditorWindow
{
	[MenuItem ("UGB/Preferences")]
    public static void Init () 
	{
        var window = EditorWindow.GetWindow<UEDSWindow>("UEDS",true);
		window.position = new Rect(50,50, kMinWidth + 50 + kListWidth,kMinHeight + 50);
		
    }
	const int kMinWidth = 600;
	const int kMinHeight = 400;
	const int kListWidth = 200;

	List<FieldRendererBase>mFieldRenderers = new List<FieldRendererBase>();
	SettingsProxy mSettings;

	string[] mInstanceNames;

	[System.NonSerialized]
	bool mInitilized = false;

	[System.NonSerialized]
	bool mRenameEnabled = false;

	IEnumerator mInitEnumerator;
	[System.NonSerialized]
	Vector2[] mScrollPositions = new Vector2[3];

	List<FieldRendererBase> GetAllInstancesOfAllSubclassesOf (System.Type pType)
	{
		var outList = new List<FieldRendererBase>();
		foreach(var assembly in System.AppDomain.CurrentDomain.GetAssemblies())
		{
			foreach(var t in assembly.GetTypes())
			{
				if(t.IsSubclassOf(pType))
				{
					outList.Add( System.Activator.CreateInstance(t) as FieldRendererBase );
				}
			}
		}
		return outList;
	}
	
	void Initialize()
	{
		if(mInitilized || mInitEnumerator != null)
			return;

		mInitEnumerator = Settings.Init<SerializedRoot>( );

		if(mInitEnumerator == null)
		{
			Debug.LogError(Settings.kLogPrefix + "Could not init Settings");
			return;
		}
		EditorUpdate.Run(() => {
			if(mInitEnumerator == null)
				return true;
			return !mInitEnumerator.MoveNext();
		},() => {


			mInitilized = true;
			mSettings = new SettingsProxy();
			mFieldRenderers = GetAllInstancesOfAllSubclassesOf(typeof(FieldRendererBase));
			mInitEnumerator = null;
		
		});


		
	}

	IEnumerator mSaveProcess;
	void Save()
	{
		if(mSaveProcess != null)
			return;

		mStatusMessage = "Saving ...";

		mSaveProcess = mSettings.Save( Settings.FileName );
		EditorUpdate.Run( () => { 
			if(mSaveProcess == null)
				return true;
			return !mSaveProcess.MoveNext();
		},() => {
			mSaveProcess = null;
			mStatusMessage = kReadyMessage;
		});

	}

	SettingsContainer mSelectedSettingsContainer;
	SerializedSettingsContainer mSelectedContainer;
	string mSelectedContainerName;

	const string kReadyMessage = "Ready";
	string mStatusMessage = kReadyMessage;
	bool mUpdateContainer;

	void OnGUI()
	{
		Initialize();

		if(!mInitilized)
			mStatusMessage = "Initializing";
		else
			mStatusMessage = "Ready";
		//	EditorUtility.DisplayProgressBar("Global Editor Settings", "initializing", 0.5f);
		 

		mScrollPositions[0] = GUILayout.BeginScrollView(mScrollPositions[0]);
		
		GUILayout.BeginVertical();
		GUILayout.BeginHorizontal();
		
		//
		// Render Container List
		//
		
		GUILayout.BeginVertical( UEDSStyles.list, GUILayout.Width(kListWidth), 
			GUILayout.MinHeight(kMinHeight), GUILayout.MaxHeight(4000));

		GUILayout.BeginHorizontal(EditorStyles.toolbar);
		bool hActive = true;
		if(GUILayout.Button(UEDSStyles.openFileContent,EditorStyles.toolbarButton))
		{
			var filePath = EditorUtility.OpenFilePanel("Select Settings XML-File","","xml");
			if(filePath != null && filePath.Length > 0)
			{
				Settings.FileName = filePath;

				mInitilized = false;
				hActive = false;
			}
		}

		if(GUILayout.Button(UEDSStyles.saveFileContent,EditorStyles.toolbarButton))
		{
			Save ();
			hActive = false;
		}
		GUILayout.FlexibleSpace();

		if(hActive)
			GUILayout.EndHorizontal();



		mScrollPositions[1] = GUILayout.BeginScrollView(mScrollPositions[1]);
		
		bool odd = true;
		if(mSettings != null)
		{
			foreach(var container in mSettings.settingContainers)
			{
				RenderSelectableContainer(container,odd);
				odd = !odd;
			}
		}
		GUILayout.EndScrollView();
		GUILayout.EndVertical();
		
		UEDSStyles.VerticalLine();


		//
		// Draw Selected Container Inspector
		//



		mScrollPositions[2] = GUILayout.BeginScrollView(mScrollPositions[2],GUILayout.MinWidth(kMinWidth), GUILayout.MaxWidth(4000));
		RenderSelectedContainer();
		GUILayout.EndScrollView();



		GUILayout.EndHorizontal();

		//
		// Draw status bar
		//
		UEDSStyles.HorizontalLine();
		
		RenderStatusBar();
		
		GUILayout.EndVertical();
		
		GUILayout.EndScrollView();


		if(mUpdateContainer)
		{
			mUpdateContainer = false;
			mSelectedContainer = Settings.GetContainer( mSelectedSettingsContainer.mType, mSelectedContainerName);
			if(mSelectedContainer == null)
				mSelectedContainer = mSettings.AddInstance( mSelectedSettingsContainer.mType, Settings.kDefaultInstanceName );
			
			Debug.Log( mSelectedContainer.mConcerningType.ToString() + " > " + mSelectedContainer.mInstanceName);

			SettingsContainer container = mSettings.GetContainerForType(mSelectedContainer.mConcerningType);
			mSelectedContainer.UpdateSettings(container.mSettings);
			mInstanceNames = mSettings.GetInstanceNames(mSelectedSettingsContainer.mType);
		}

	}
	
	
	
	void RenderSelectableContainer(SettingsContainer pContainer, bool pOdd)
	{
		bool selected = false;

		if(mSelectedContainer != null && pContainer.mType == mSelectedContainer.mConcerningType)
		{
			selected = GUILayout.Button(pContainer.DisplayName,UEDSStyles.selectedListEntry, GUILayout.MaxWidth(kListWidth));
		}else
		{
			selected = GUILayout.Button(pContainer.DisplayName,(pOdd)? UEDSStyles.selectableListEntryOdd : UEDSStyles.selectableListEntry, GUILayout.MaxWidth(kListWidth));
		}
		
		if(pContainer.DisplayGizmo != null && pContainer.DisplayGizmo.image != null)
		{
			Rect r = GUILayoutUtility.GetLastRect();
			float height = pContainer.DisplayGizmo.image.height / (float)pContainer.DisplayGizmo.image.width * 16;
			GUI.DrawTexture(new Rect( r.x +(r.width - 26), r.y + (r.height - height) / 2, 16,height),pContainer.DisplayGizmo.image);
		}
		if(selected)
		{
			mUpdateContainer = true;
			mSelectedSettingsContainer = pContainer;
			mSelectedContainerName = Settings.kDefaultInstanceName;
		}

	}
	
	
	
	void RenderSelectedContainer()
	{
		if(!mInitilized || mSelectedContainer == null)
			return;
		SettingsContainer container = mSettings.GetContainerForType(mSelectedContainer.mConcerningType);

		if(container != null)
		{

			//
			// Draw Toolbar
			//
			GUILayout.BeginVertical();
			
			GUILayout.BeginHorizontal(EditorStyles.toolbar);
			GUILayout.Space(10);
			if(GUILayout.Button(UEDSStyles.addInstanceContent,EditorStyles.toolbarButton))
			{
				mSettings.AddInstance(mSelectedContainer.mConcerningType, mInstanceNames.Length.ToString());
				mUpdateContainer = true;
			}
			if(GUILayout.Button(UEDSStyles.dupInstanceContent,EditorStyles.toolbarButton))
			{
				SerializedSettingsContainer c = mSettings.DupInstance(mSelectedContainer);
				mSelectedContainerName = c.mInstanceName;
				mUpdateContainer = true;
			}
			if(GUILayout.Button(UEDSStyles.delInstanceContent,EditorStyles.toolbarButton))
			{
				mSettings.DelInstance(mSelectedContainer);
				mUpdateContainer = true;
			}
			GUILayout.FlexibleSpace();

			int i = System.Array.IndexOf( mInstanceNames, mSelectedContainer.mInstanceName );
			int selectedIndex = EditorGUILayout.Popup(i, mInstanceNames,EditorStyles.toolbarPopup);

			if(selectedIndex != i)
			{
				mSelectedContainerName = mInstanceNames[selectedIndex] ;
				mUpdateContainer = true;
			}

			GUILayout.Space(10);
			GUILayout.EndHorizontal();

			//
			// rename instance
			//

			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			
			GUILayout.BeginVertical();
			GUILayout.Space(4);
			var newName = GUILayout.TextField( mSelectedContainerName, GUILayout.Width(120) );
			GUILayout.EndVertical();
			if(mRenameEnabled)
			{
				mSelectedContainerName = newName;
			}
			GUI.enabled = mSelectedContainer.mInstanceName != Settings.kDefaultInstanceName;
			bool renameToggle = GUILayout.Toggle(mRenameEnabled, UEDSStyles.renameInstanceContent,"miniButton");

			if(mRenameEnabled != renameToggle)
			{
				mRenameEnabled = renameToggle;
				//
				// cannot rename default instance
				//
				if(mSelectedContainerName == Settings.kDefaultInstanceName)
					mRenameEnabled = false;
				if(!mRenameEnabled && mSelectedContainerName != mSelectedContainer.mInstanceName)
				{
					//
					// prevent renaming to be the same as default instance
					//
					if(mSelectedContainerName == Settings.kDefaultInstanceName)
						mSelectedContainerName = mSelectedContainer.mInstanceName;

					mSelectedContainer.mInstanceName = mSelectedContainerName;
					mUpdateContainer = true;
				}
			}
			GUI.enabled = true;
			GUILayout.EndHorizontal();

			//
			// Draw Info
			//

			GUILayout.BeginVertical(UEDSStyles.detailsGroup);
			
			GUILayout.BeginHorizontal();
			GUILayout.Label(container.DisplayGizmo, UEDSStyles.detailsGizmo);
			GUILayout.Label(container.DisplayName, UEDSStyles.detailsTitle);
			GUILayout.EndHorizontal();




			GUILayout.Label(container.DisplayDescription, UEDSStyles.detailsDescription);

			UEDSStyles.HorizontalSeparator();


			//
			// Draw Settings
			//

			foreach(var c in mSelectedContainer.mSettings)
			{
				
				var renderer = RenderSetting(c);
				renderer.PreEditorGUI(c);
				EditorGUILayout.BeginVertical();
				EditorGUILayout.BeginHorizontal();

				renderer.OnEditorGUI(c);

				if(renderer != null)
				{
					renderer.RenderDescriptionIcon();
				}
				EditorGUILayout.EndHorizontal();

				if(renderer != null)
					renderer.RenderDescription(c);

				EditorGUILayout.EndVertical();
				renderer.PostEditorGUI(c);

			}


			GUILayout.EndVertical();

			GUILayout.EndVertical();

		}else
		{
			GUILayout.Label("No Container Selected",UEDSStyles.bigHint);
		}

	
	}

	
	void RenderStatusBar()
	{
		GUILayout.BeginHorizontal();
		GUILayout.Label(mStatusMessage, UEDSStyles.statusMessage);
		GUILayout.FlexibleSpace();

		if(Settings.FileName != null)
			GUILayout.Label(Settings.FileName, UEDSStyles.statusMessage);

		GUILayout.EndHorizontal();
	}



	FieldRendererBase RenderSetting(Setting pSetting)
	{
		foreach(var renderer in mFieldRenderers)
		{
			if(renderer.HandlesType(pSetting.mValueType))
			{
				return renderer;
			}
		}
		
		return null;
	}



}

