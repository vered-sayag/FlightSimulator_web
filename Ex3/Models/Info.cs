using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using Ex3.Models;

namespace Exercise3.Models
{
    public class Info 
    {
        private static Info s_instace = null;

        public static Info Instance
        {
            get
            {
                if (s_instace == null)
                {
                    s_instace = new Info("127.0.0.1",5400);
                }
                return s_instace;
            }
        }

        TcpClient client;
        private IPEndPoint ep;
        private readonly object locker;
        private NetworkStream stream;
        private BinaryReader reader;


        public Info(string conIp, int port)
        {
            if (s_instace != null)
            {
                s_instace.client.Close();
            }
            IPAddress ip = IPAddress.Parse(conIp);
            locker = new object();
            client = new TcpClient();
            ep = new IPEndPoint(ip, port);

            while (!client.Connected)
            {
                try
            {
                
                    client.Connect(ep);
            }
            catch (Exception e) { }
            }

            s_instace = this;

        }

        public double Write(string command)
        {
            double output= 0;
            stream = client.GetStream();
            byte[] send = Encoding.ASCII.GetBytes(command.ToString());
            stream.Write(send, 0, send.Length);

            reader = new BinaryReader(client.GetStream());
            string input = ""; // input will be stored here
            char s;
            bool seeTag = false;
            while ((s = reader.ReadChar()) != '\n'|| input=="")
            {
                if (seeTag && s != '\'')
                {
                    input += s;
                }
                if (seeTag && s == '\'')
                {
                    break;
                }
                if (s == '\'')
                {
                    seeTag = !seeTag;
                   
                }
            }// read untill \n
            try
            {
                output = Convert.ToDouble(input);
            }catch (Exception e) { }
            
            return output;
        }

        public InfoLocation processInfo()
        {
            InfoLocation infoLocation = new InfoLocation();
            infoLocation.Lon = Write("get /position/longitude-deg\r\n");
            infoLocation.Lat = Write("get /position/latitude-deg\r\n");
            return infoLocation;
            
        }

        public InfoLocation processInfoSave(string fileName)
        {
            InfoLocation infoLocation = new InfoLocation();
            
            infoLocation.Lon = Write("get /position/longitude-deg\r\n");
            infoLocation.Lat = Write("get /position/latitude-deg\r\n");
            double Rudder = Write("get /controls/flight/rudder\r\n");
            double Throttle = Write("get /controls/engines/current-engine/throttle\r\n");
            double Heading = Write("get /instrumentation/heading-indicator/indicated-heading-deg\r\n");
            using (StreamWriter streamWriter = System.IO.File.AppendText(fileName))
            {
                streamWriter.WriteLine(Convert.ToString(infoLocation.Lon) + ','
                    + Convert.ToString(infoLocation.Lat) + ',' + Convert.ToString(Heading)
                    + ',' + Convert.ToString(Rudder) + ',' + Convert.ToString(Throttle));
            }
            
            return infoLocation;
        }

        public void DeletFile(string filePath)
        {
            File.Delete(filePath);
        }


        public void Close()
        {
            client.Close();
        }
    }
}