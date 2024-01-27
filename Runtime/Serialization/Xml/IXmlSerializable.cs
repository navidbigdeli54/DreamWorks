/**Copyright 2016 - 2023, Dream Machine Game Studio. All Right Reserved.*/

using System.Xml.Linq;

namespace DreamMachineGameStudio.Dreamworks.Serialization.Xml
{
	public interface IXmlSerializable
	{
		int Version { get; }

		XDocument ToXml();
	}
}
