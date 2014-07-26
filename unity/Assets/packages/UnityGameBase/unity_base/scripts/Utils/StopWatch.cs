using System;

/// <summary>
/// A simple stop watch implementation with a pause function. 
/// </summary>
public class StopWatch
{
	long mStartTime;
	long mElapsed = 0;
	bool mRunning = false;
	
	public bool isRunning { get { return mRunning;}}
	
	public StopWatch ()
	{
		Start();
	}
	
	public long Measure()
	{
		if(mRunning)
		{
			Pause();
			Start();
		}
		return mElapsed;
	}
	public void Start()
	{
		mStartTime = DateTime.UtcNow.Ticks;
		mRunning = true;
	}
	public void Pause()
	{
		if(mRunning)
			mElapsed += DateTime.UtcNow.Ticks - mStartTime;
		mRunning = false;
	}
	public void Stop()
	{
		mElapsed = 0;
		mRunning = false;
	}
	
}

