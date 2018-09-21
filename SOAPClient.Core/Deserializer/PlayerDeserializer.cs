using SOAPClient.Entities;
using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

namespace SOAPClient.Core.Serializer
{
    public static class PlayerSerializer
    {
        private static string WSCall(string sTeamName)
        {
            var uri = "https://footballpool.dataaccess.eu/info.wso";
            var xml = TeamPlayers(sTeamName, true);
            var response = Core.WSCall.Client(xml, uri);
            return response.ToString();
        }

        public static List<Player> GetPlayers(string sTeamName) 
            => DeserializeResponse(WSCall(sTeamName));
        
        private static List<Player> DeserializeResponse(string xml)
        {

            XmlDocument xDoc = new XmlDocument();

            XmlNamespaceManager ns = new XmlNamespaceManager(xDoc.NameTable);
            ns.AddNamespace("m", "https://footballpool.dataaccess.eu");

            // Environment.CurrentDirectory.Split(new string[] { "\\bin\\Debug" }, StringSplitOptions.None)[0];
            xDoc.LoadXml(xml);

            XmlNodeList details = xDoc.GetElementsByTagName("m:tPlayer");
            List<Player> Players = new List<Player>();

            foreach (XmlNode node in details)
            {
                Player player = new Player()
                {
                    Id = Convert.ToInt32(node.SelectSingleNode("m:iId", ns).InnerText),
                    Name = node.SelectSingleNode("m:sName", ns).InnerText,
                    FullName = node.SelectSingleNode("m:sFullName", ns).InnerText,
                    FirstName = node.SelectSingleNode("m:sFirstName", ns).InnerText,
                    LastName = node.SelectSingleNode("m:sLastName", ns).InnerText
                };

                Players.Add(player);
            }

            return Players;

        }

        private static XDocument TeamPlayers(string sTeamName, bool bSelected)
        {
            XNamespace ns = "http://schemas.xmlsoap.org/soap/envelope/";
            XNamespace foot = "https://footballpool.dataaccess.eu";

            XDocument soapRequest = new XDocument(
               new XDeclaration("1.0", "UTF-8", "no"),
               new XElement(ns + "Envelope",
                   new XAttribute(XNamespace.Xmlns + "foot", foot),
                   new XAttribute(XNamespace.Xmlns + "soap", ns),
                    new XElement(ns + "Header"),
                   new XElement(ns + "Body",
                       new XElement(foot + "TeamPlayers",
                           new XElement(foot + "sTeamName", sTeamName),
                           new XElement(foot + "bSelected", bSelected.ToString().ToLower())
                       )
                   )
               ));

            return soapRequest;
        }

    }
}
