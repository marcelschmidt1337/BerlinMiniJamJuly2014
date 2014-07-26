using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace UGB.audio
{
	public class MultiChannelController : GameComponent
	{
		List<Channel>mChannels = new List<Channel>();
		bool mMute;
		float mFadeDuration = 0.5f;

		public void Init(int pChannelCount)
		{
			while(mChannels.Count < pChannelCount)
			{
				var ch = new Channel(this);
				mChannels.Add(ch);

			}

			while(mChannels.Count > pChannelCount)
			{
				mChannels[0].Dispose();
				mChannels.RemoveAt(0);
			}

			UpdateFadeDuration();
			UpdateMuteState();
		}

		public Channel currentChannel = null;


		public float fadeDuration
		{
			get
			{
				return mFadeDuration;
			}
			set
			{
				mFadeDuration = value;
			}
		}

		public bool mute
		{
			get
			{
				return mMute;
			}
			set
			{
				if(mMute != value)
				{
					mMute = value;
					UpdateMuteState();
				}
			}
		}

		public void Stop(ChannelInfo pChannel, bool pImmediately)
		{
			pChannel.channel.Stop(pImmediately);
		}

		public virtual ChannelInfo Play(AudioClip pClip, bool pLoop)
		{
			var channel = GetFreeChannel();
			channel.clip = pClip;
			channel.loop = pLoop;
			channel.fadeDuration = fadeDuration;
			channel.Play();
			ChannelInfo ci = new ChannelInfo();
			ci.channel = channel;
			return ci;
		}

		public virtual void PlaySoundEffect(AudioClip pClip)
		{
			var channel = GetFreeChannel();
			channel.clip = pClip;
			channel.loop = false;
			channel.fadeDuration = 0;
			channel.Play();

		}

		public IEnumerable<Channel> Channels
		{
			get
			{
				foreach(var ch in mChannels)
				{
					yield return ch;
				}
			}
		}

		public void Update()
		{
			foreach(var channel in mChannels)
			{
				channel.Update();
			}
		}

		/// <summary>
		/// returns a channel, that is currently stopped. if none found, returns the channel with minimal volume. 
		/// </summary>
		/// <returns>The free channel.</returns>
		Channel GetFreeChannel()
		{
			Channel chnl = null;
			float minVal = float.MaxValue;

			foreach(var channel in mChannels)
			{
				if(channel.state == Channel.eChannelState.stopped)
				{
					chnl = channel;
					break;
				}
				
				if(channel.actualVolume < minVal)
				{
					minVal = channel.actualVolume;
					chnl = channel;
				}
			}
			return chnl;
		}
		
		void UpdateMuteState()
		{
			foreach(var channel in mChannels)
			{
				channel.mute = mute;
			}
		}
		void UpdateFadeDuration()
		{
			foreach(var channel in mChannels)
			{
				channel.fadeDuration = mFadeDuration;
			}
		}
	}
}
