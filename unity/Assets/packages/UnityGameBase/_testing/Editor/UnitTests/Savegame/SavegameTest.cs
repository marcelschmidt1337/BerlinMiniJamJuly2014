using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace UGB.Savegame.test
{
	[TestFixture]
	public class SavegameTest
	{
		SavegameController<SavegameTestType> mSavegameInstance;
		List<Metadata> mData;

		[SetUp]
		public void Setup()
		{
			mData = new List<Metadata>();

			XMLProvider p = new XMLProvider();
			mSavegameInstance = new SavegameController<SavegameTestType>(p);
			mSavegameInstance.OnMetadataList += HandleOnMetadataList;
		}

		void HandleOnMetadataList (List<Metadata> l)
		{
			mData = l;
		}

		[Test]
		public void CheckInitialization()
		{
			Assert.IsNotNull(mSavegameInstance);
			Assert.IsTrue(mSavegameInstance.GetType() == typeof(SavegameController<SavegameTestType>));
		}

		[Test]
		public void CreateSavegame()
		{
			mSavegameInstance.OnLoadComplete += (SavegameTestType s) => {
				Assert.IsTrue(s.Metadata.Id == 2);
				Assert.IsTrue(s.TestPropertyA == 12);
			};


			mSavegameInstance.List();
			mSavegameInstance.OnMetadataList += (List<UGB.Savegame.Metadata> l) => {
				mSavegameInstance.Load(2);
				mSavegameInstance.Remove(2);
			};




		}

		[Test]
		public void CreateMultipleSavegames()
		{
			mSavegameInstance.List();
			mSavegameInstance.OnMetadataList += (List<UGB.Savegame.Metadata> l) => {

				mSavegameInstance.Load(1);
				mSavegameInstance.Load(4);
				mSavegameInstance.Load(13);

				mSavegameInstance.List();
				Assert.IsTrue(mData.Count == 3);

				mSavegameInstance.Remove(1);
				mSavegameInstance.Remove(4);
				Assert.IsTrue(mData.Count == 1);
				mSavegameInstance.Remove(13);
			};
		}

		[TearDown]
		public void Clear()
		{
			mData.Clear();
			mSavegameInstance.OnMetadataList -= HandleOnMetadataList;
		}
	}
}
