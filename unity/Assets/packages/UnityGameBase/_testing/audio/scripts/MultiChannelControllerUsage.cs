using UnityEngine;
using System.Collections;
namespace UGB.audio.Test
{
	public class MultiChannelControllerUsage : MonoBehaviour
	{
		MultiChannelController controller;
		string channelCount = "3";

		public AudioClip[] clips;
		ChannelInfo lastChannel;
		// Use this for initialization
		void Start ()
		{
			controller = gameObject.AddComponent<MultiChannelController>();
			controller.Init(3);
		}
	
		// Update is called once per frame
		void Update ()
		{
			if(Input.GetKeyDown(KeyCode.A))
				controller.PlaySoundEffect(clips[2],1);
			if(Input.GetKeyDown(KeyCode.D))
				controller.PlaySoundEffect(clips[3],1);

		}

		void PlayAtmo(int idx)
		{
			if(lastChannel != null)
				controller.Stop(lastChannel,false);
			lastChannel = controller.Play(clips[idx], true);
		}

		void StopCurrentChannel(bool pImmediately)
		{
			if(lastChannel != null)
				controller.Stop(lastChannel,pImmediately);
			lastChannel = null;
		}



		void OnGUI()
		{

			GUILayout.BeginArea(new Rect(50,50,Screen.width-50,500));


			GUILayout.BeginVertical();
			GUILayout.BeginHorizontal();
			channelCount = GUILayout.TextField( channelCount, GUILayout.Width(100));
			if(GUILayout.Button("Init"))
			{
				int cCount = System.Convert.ToInt32(channelCount);
				channelCount = cCount.ToString();
				controller.Init(cCount);
			}

			GUILayout.EndHorizontal();
			GUILayout.Space(20);
			GUILayout.TextArea("To play sound FX hit A or D");
			GUILayout.Space(10);
			GUILayout.BeginHorizontal();
			if(GUILayout.Button("Play Atmo1"))
			{
				PlayAtmo(0);

			}
			if(GUILayout.Button("Play Atmo2"))
			{
				PlayAtmo(1);
			}
			if(GUILayout.Button("Stop Current"))
			{
				StopCurrentChannel(false);
			}
			
			if(GUILayout.Button("Stop (no fade)"))
			{
				StopCurrentChannel(true);
			}


			controller.mute = GUILayout.Toggle(controller.mute, "mute");
			controller.fadeDuration = GUILayout.HorizontalSlider(controller.fadeDuration, 0, 2);
			GUILayout.EndHorizontal();

			int i = 0;
			foreach(var ch in controller.Channels)
			{
				GUILayout.BeginHorizontal();

				GUILayout.Label(i.ToString(), GUILayout.Width(30));
				GUILayout.Label(ch.state.ToString(),GUILayout.Width(100) );
				GUILayout.HorizontalSlider(ch.actualVolume,0,1, GUILayout.Width(100));
				GUILayout.HorizontalSlider(ch.volume,0,1, GUILayout.Width(100));
				if(ch.clip != null)
					GUILayout.Label(ch.clip.name);

				GUILayout.EndHorizontal();
			}

			GUILayout.EndVertical();
			GUILayout.EndArea();
		}
	}
}

