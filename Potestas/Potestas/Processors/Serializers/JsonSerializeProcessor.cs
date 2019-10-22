using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Potestas.Interfaces;

namespace Potestas.Processors.Serializers
{
    public class JsonSerializeProcessor<T> : SerializeProcessor<T> where T: IEnergyObservation
    {
        public override string Description => "JsonSerializeProcessor";

        public override void OnNext(T value)
        {
            base.OnNext(value);

            using (var reader = new StreamReader(Stream))
            using (var writer = new StreamWriter(Stream))
            {
                string content;
                if (Stream.Length > 0)
                {
                    content = ReadAllStream(reader).Result;

                    List<T> items;

                    if (content[0] == '[')
                    {
                        items = JsonConvert.DeserializeObject<List<T>>(content);
                        items.Add(value);
                    }
                    else
                    {
                        var item = JsonConvert.DeserializeObject<T>(content);
                        items = new List<T> {item, value};
                    }

                    content = JsonConvert.SerializeObject(items, Formatting.Indented);
                }
                else
                {
                    content = JsonConvert.SerializeObject(value, Formatting.Indented);
                }

                WriteToStream(writer, content).Wait();
            }
        }
    }
}
