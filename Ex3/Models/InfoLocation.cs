using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace Ex3.Models
{
    public class InfoLocation
    {
        public double Lat { get; set; }
      
        public double Lon { get; set; }

        public void ToXml(XmlWriter writer)
        {
            writer.WriteStartElement("InfoLocation");
            writer.WriteElementString("Lon", this.Lon.ToString());
            writer.WriteElementString("Lat", this.Lat.ToString());
            writer.WriteEndElement();
        }
    }
}
