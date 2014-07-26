using UnityEngine;
using System.Collections;
using System.Text;
using System;

public class GameLicense : GameComponent
{
	const string kLizLastChck = "OptDeviceLoc";
	
	const string kLizLastRspn = "OptJumpLimit";
	LicenseInformation mLicenseInfo;

	public LicenseInformation currentLicense
	{
		get {
			if(mLicenseInfo == null)
			{
				
				LoadLicenseInformation();
				if(currentLicense.mTargetStore == ETargetStore.GooglePlayStore)
					isGenuine = !paidLicense;
				else
					isGenuine = true;

			}
			return mLicenseInfo;
		}
	}
	
	public bool paidLicense
	{
		get;
		private set;
	}
	void LoadLicenseInformation()
	{
		// TODO: Implement licence handling
		mLicenseInfo = LicenseInformation.Load();
		paidLicense = true;
		isGenuine = true;
	}
	public bool isGenuine
	{
		get;
		private set;
	}
	
	

}

