// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 4.0.30319.1
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------
using System;
namespace UGB.Savegame.test
{
	public class SavegameTestType : SavegameBase
	{
		public int TestPropertyA 	= 12;
		public float TestPropertyB 	= 23.7f;
		public string StringProperty = "TestCase";
		public string Playername = "Playername";

		public SavegameTestType()
		{
		}

		public override void Deserialize (string pSavegame)
		{
			SavegameTestType result = XmlObjectSerializer.StringToType<SavegameTestType>(pSavegame);
			TestPropertyA 	= result.TestPropertyA;
			TestPropertyB 	= result.TestPropertyB;
			StringProperty 	= result.StringProperty;
			Playername 		= result.Playername;
		}
		
		public override string Serialize ()
		{
			return XmlObjectSerializer.ObjectToString(this);
		}

	}
}