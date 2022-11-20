using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Forms;

namespace AutomaticWebBrowser.Conveters
{
    public class JsonStringArrayEnumArrayConverter : JsonConverter<Keys[]>
    {
        public override Keys[] Read (ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            List<Keys> keys = new List<Keys> ();
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                while (reader.Read ())
                {
                    switch (reader.TokenType)
                    {
                        case JsonTokenType.String:
                            {
                                if (Enum.TryParse (reader.GetString (), out Keys resutl))
                                {
                                    keys.Add (resutl);
                                }
                                else
                                {
                                    keys.Add (Keys.None);
                                }
                            }
                            break;
                        case JsonTokenType.EndArray:
                            return keys.ToArray ();
                    }
                }
            }

            return null;
        }

        public override void Write (Utf8JsonWriter writer, Keys[] value, JsonSerializerOptions options)
        {
            throw new NotImplementedException ();
        }
    }
}
