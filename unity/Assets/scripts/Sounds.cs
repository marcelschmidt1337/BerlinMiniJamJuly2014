using UnityEngine;
using System.Collections;
using UGB.audio;

public class Sounds : GameComponent
{
	MultiChannelController controller;
	public int channelCount = 4;
	
	public AudioClip music;
	public AudioClip[] sfx;

	ChannelInfo lastChannel;

	void Start()
	{
		controller = gameObject.AddComponent<MultiChannelController>();
		controller.Init(channelCount);
	}

	public void PlayMusic()
	{
		if(lastChannel != null)
			controller.Stop(lastChannel,false);
		lastChannel = controller.Play(music, true);
	}

	public void StopCurrentChannel(bool pImmediately)
	{
		if(lastChannel != null)
			controller.Stop(lastChannel,pImmediately);
		lastChannel = null;
	}

	public void PlaySfx(int pIndex)
	{
		controller.PlaySoundEffect(sfx[pIndex], 1.0f);
	}
}

