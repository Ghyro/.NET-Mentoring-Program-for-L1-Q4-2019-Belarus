using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using System.Text;
using Potestas.Interfaces;

namespace Potestas.Processors.Serializers
{
    public class JsonSerializeProcessor<T> : SerializeProcessor<T> where T: IEnergyObservation
    {
        private readonly DataContractJsonSerializer _jsonSerializer;

        public JsonSerializeProcessor()
        {
            _jsonSerializer = new DataContractJsonSerializer(typeof(T));
        }

        public override void OnNext(T value)
        {
            using (Stream)
            {
                if (Stream.CanWrite)
                    _jsonSerializer.WriteObject(Stream, value);
            }
        }

        public override string Description => "Json serialize processor";
    }
}
