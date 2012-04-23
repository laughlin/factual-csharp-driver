using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace FactualDriver.Utils
{
    public class ParameterCollectionJsonConverter : JsonConverter
    {
        private readonly Type[] parameterTypes;
        private readonly Dictionary<string, object> parameterInstances;

        public ParameterCollectionJsonConverter(params Type[] parameterTypes)
        {
            this.parameterTypes = parameterTypes;
            this.parameterInstances = new Dictionary<string, object>(parameterTypes.Length);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.IsAssignableFrom(typeof (ParameterCollection));
        }

        public override object ReadJson(JsonReader reader, Type objectType)
        {
            reader.Read(); // read past start object token

            for (int i = 0; i < parameterTypes.Length; i++)
            {
                string parameterName = reader.Value as string;

                this.parameterInstances.Add(parameterName, new JsonSerializer().Deserialize(reader, parameterTypes[i]));
                reader.Read(); // read past end object token        }

                reader.Read(); // read past end object token

                return new ParameterCollection(parameterInstances);
            }
        }

        public static ParameterCollection Deserialize(TextReader jsonTextReader, params Type[] types)
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Converters.Add(new ParameterCollectionJsonConverter(types));

            JsonReader reader = new JsonReader(jsonTextReader);
            return serializer.Deserialize(reader, typeof (ParameterCollection)) as ParameterCollection;
        }
    }

}
