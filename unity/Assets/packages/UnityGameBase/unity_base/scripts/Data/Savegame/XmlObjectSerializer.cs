using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace UGB.Savegame
{
	public class XmlObjectSerializer
	{
		public static string ObjectToString(Object obj)
		{
			XmlSerializer serializer = new XmlSerializer(obj.GetType());
			StringWriter writer = new StringWriter();
			serializer.Serialize(writer, obj);
			return writer.ToString();
		}
		
		public static T StringToType<T>(string s) where T : class
		{
			XmlSerializer serializer = new XmlSerializer(typeof(T));
			StringReader sr = new StringReader(s);
			XmlReader reader = XmlReader.Create(sr);
			return serializer.Deserialize(reader) as T;
		}
	}
}