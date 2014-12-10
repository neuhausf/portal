namespace Portal.Business
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml;
    using System.Xml.Serialization;

    using Hafas;

    public class SbbService
    {
        public async Task<Station> GetFirstStationWithName(string name)
        {
            var request = CreateBaseRequest();

            request.LocValReq.Add(new LocValReq { id = "searchStation", sMode = "1", ReqLoc = new ReqLoc { match = name, type = type2.ALLTYPE } });

            var resp = await DoRequest<ResC>(request);

            var localValRes = resp.LocValRes.FirstOrDefault(x => x.id == "searchStation");
            if (localValRes != null)
            {
                return localValRes.Station;
            }

            return null;
        }
        public async Task<bool> HasDelay(Connection connection)
        {
            foreach (RtState state in connection.RtStateList)
            {
                if (state.value == value.HAS_DELAYINFO)
                {
                    // TODO Parse entries in I.
                    return connection.IList.I.Any();
                }
            }

            return false;
        }
        public async Task<ResC> GetConnections(string from, string to, DateTime dateAndTime)
        {
            var fromStation = await this.GetFirstStationWithName(from);
            var toStation = await this.GetFirstStationWithName(to);
            if (fromStation == null || toStation == null)
            {
                throw new Exception("From or To Station not found!");
            }

            return await this.GetConnections(fromStation, toStation, dateAndTime);
        }

        public async Task<ResC> GetConnections(Station from, Station to, DateTime dateAndTime)
        {
            var request = CreateBaseRequest();
            request.ConReq.Start.Station = from;
            request.ConReq.Start.Prod.prod = "1111111111000000";
            request.ConReq.Dest.Station = to;
            request.ConReq.ReqT.a = a2._0;
            request.ConReq.ReqT.date = dateAndTime.ToString("yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
            request.ConReq.ReqT.time = dateAndTime.ToString("hh:mm", System.Globalization.CultureInfo.InvariantCulture);
            request.ConReq.RFlags.b = "0";
            request.ConReq.RFlags.f = "4";
            request.ConReq.RFlags.sMode = sMode.N;
            var resp = await DoRequest<ResC>(request);
            return resp;
        }

        private static ReqC CreateBaseRequest()
        {
            var request = new ReqC();
            request.lang = lang.DE;
            request.prod = "iPhone3.1";
            request.ver = "2.3";
            request.accessId = "YJpyuPISerpXNNRTo50fNMP0yVu7L6IMuOaBgS0Xz89l3f6I3WhAjnto4kS9oz1";
            return request;
        }

        private static async Task<T> DoRequest<T>(object request)
        {
            var postData = GetRequestAsString(request);

            var req = new HttpClient();
            var message = new HttpRequestMessage(HttpMethod.Post, "http://xmlfahrplan.sbb.ch/bin/extxml.exe/");
            message.Headers.Add("User-Agent", "SBBMobile/4.11.0.1 CFNetwork/711.1.12 Darwin/14.0.0");
            message.Content = new StringContent(postData);
            message.Content.Headers.ContentType = new MediaTypeHeaderValue("application/xml");

            var response = await req.SendAsync(message);
            var content = await response.Content.ReadAsStreamAsync();

            var serializer = new XmlSerializer(typeof(ResC));
            var resp = (T)serializer.Deserialize(content);
            return resp;
        }

        private static string GetRequestAsString(object request)
        {
            var encoding = Encoding.GetEncoding("ISO-8859-1");
            var xmlWriterSettings = new XmlWriterSettings { Indent = false, OmitXmlDeclaration = false, Encoding = encoding };

            string postData;
            using (var memoryStream = new MemoryStream())
            using (XmlWriter xmlWriter = XmlWriter.Create(memoryStream, xmlWriterSettings))
            {
                var x = new XmlSerializer(request.GetType());
                x.Serialize(xmlWriter, request);

                // we just output back to the console for this demo.
                memoryStream.Position = 0; // rewind the stream before reading back.

                using (var sr = new StreamReader(memoryStream))
                {
                    postData = sr.ReadToEnd();
                    Debug.WriteLine(postData);
                } // note memory stream disposed by StreamReaders Dispose()
            }

            return postData;
        }

    }
}