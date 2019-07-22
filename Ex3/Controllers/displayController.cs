using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using Ex3.Models;
using Exercise3.Models;

namespace Exercise3.Controllers
{
    public class displayController : Controller
    {
        [HttpGet]
        public ActionResult display(string nameFile, int time)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"\" + nameFile + ".txt";
            string[] files = System.IO.File.ReadAllLines(path);
            ViewBag.time = time;
            Array.Resize(ref files, files.Length + 1);
            files[files.Length - 1] = "End";

            string line = files[0];
            string[] words = line.Split(',');
            files = files.Skip(1).ToArray();
            ViewBag.lon = Convert.ToDouble(words[0]);
            ViewBag.lat = Convert.ToDouble(words[1]);

            Session["arrayfile"] = files;
            return View();
        }
        [HttpPost]

        public string GetFileLocation()
        {

            string[] files = (string[])Session["arrayfile"];
            string line = files[0];
            string[] words = line.Split(',');
            files = files.Skip(1).ToArray();
            Session["arrayfile"] = files;
            InfoLocation infoLocation = new InfoLocation();
            infoLocation.Lon = 200;
            infoLocation.Lat = 200;
            if ((words[0] != "End"))
            {
                infoLocation.Lon = Convert.ToDouble(words[0]);
                infoLocation.Lat = Convert.ToDouble(words[1]);
            }
            return ToXml(infoLocation);
        }
        // GET: save
        [HttpGet]
        public ActionResult save(string ip1, string ip2, string ip3, string ip4, int port, int time, int duration, string nameFile)
        {
            string conIp = ip1 + "." + ip2 + "." + ip3 + "." + ip4;

            Info info = new Info(conIp, port);
            InfoLocation infoLocation = info.processInfo();

            ViewBag.lon = infoLocation.Lon;
            ViewBag.lat = infoLocation.Lat;
            ViewBag.time = time;
            ViewBag.duration = duration;
            @Session["namefile"] = nameFile;
            info.DeletFile(AppDomain.CurrentDomain.BaseDirectory + @"\" + @Session["namefile"] + ".txt");
            return View();
        }

        [HttpPost]
        public string SaveLocation()
        {

            string filePath = AppDomain.CurrentDomain.BaseDirectory + @"\" + @Session["namefile"] + ".txt";
            var emp = Info.Instance.processInfoSave(filePath);

            return ToXml(emp);
        }
        public void EndSave()
        {
            string filePath = AppDomain.CurrentDomain.BaseDirectory + @"\" + @Session["namefile"] + ".txt";
            using (StreamWriter streamWriter = System.IO.File.AppendText(filePath))
            {
                streamWriter.WriteLine("End");
            }

        }


        private string ToXml(InfoLocation infoLocation)
        {
            //Initiate XML stuff
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            XmlWriter writer = XmlWriter.Create(sb, settings);

            writer.WriteStartDocument();
            writer.WriteStartElement("InfoLocations");

            infoLocation.ToXml(writer);

            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            return sb.ToString();
        }
        // GET: Products
        [HttpGet]
        public ActionResult first(string ip1, string ip2, string ip3, string ip4, int port)
        {
            string conIp = ip1 + "." + ip2 + "." + ip3 + "." + ip4;
            Info info = new Info(conIp, port);
            InfoLocation infoLocation= info.processInfo();
            ViewBag.lon = infoLocation.Lon;
            ViewBag.lat = infoLocation.Lat;

            return View();
        }
        [HttpGet]
        public ActionResult secand(string ip1, string ip2, string ip3, string ip4, int port,int time)
        {
            string conIp = ip1 + "." + ip2 + "." + ip3 + "." + ip4;
            
            Info info = new Info(conIp, port);
            InfoLocation infoLocation = info.processInfo();
            ViewBag.lon = infoLocation.Lon;
            ViewBag.lat = infoLocation.Lat;
            ViewBag.time = time;
            return View();
        }

        [HttpPost]
        public string GetLocation()
        {
            var emp = Info.Instance.processInfo();
            return ToXml(emp);
        }

        
    }
}