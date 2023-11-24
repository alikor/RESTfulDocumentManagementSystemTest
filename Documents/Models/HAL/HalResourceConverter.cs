using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Documents.Models.HAL
{
    public class HalResourceConverter<T> :  JsonConverter<HalResource<T>> where T : new()
    {

        public override void Write(Utf8JsonWriter writer, HalResource<T> value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            // Serialize and write the Data property
            if (value.Data != null)
            {
                var dataJson = JsonSerializer.Serialize(value.Data, options);
                using (JsonDocument doc = JsonDocument.Parse(dataJson))
                {
                    foreach (var element in doc.RootElement.EnumerateObject())
                    {
                        element.WriteTo(writer);
                    }
                }
            }

            if (value.Links != null)
            {
                writer.WritePropertyName("_links");
                JsonSerializer.Serialize(writer, value.Links, options);
            }

            if (value.ShouldSerializeEmbedded())
            {
                writer.WritePropertyName("_embedded");
                JsonSerializer.Serialize(writer, value.Embedded, options);
            }

            if (value.ShouldSerializeTemplates())
            {
                writer.WritePropertyName("_templates");
                JsonSerializer.Serialize(writer, value.Templates, options);
            }

            writer.WriteEndObject();
        }

        public override HalResource<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {

            T data = new T();
            HalResource<T> value = new HalResource<T>(data);

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return value;
                }

                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    string propertyName = reader.GetString().Replace("_", "");
                    propertyName = Char.ToUpper(propertyName[0]) + propertyName.Substring(1);
                    reader.Read();
                    
                    // Try to get the property on T
                    PropertyInfo property = typeof(HalResource<T>).GetProperty(propertyName);

                    if (property != null)
                    {
                        object propertyValue = JsonSerializer.Deserialize(ref reader, property.PropertyType, options);
                        property.SetValue(value, propertyValue);
                        continue;
                    }

                    PropertyInfo dataProperty = typeof(T).GetProperty(propertyName);

                    if (dataProperty != null)
                    {
                        object propertyValue = JsonSerializer.Deserialize(ref reader, dataProperty.PropertyType, options);
                        dataProperty.SetValue(data, propertyValue);
                    }
                    else
                    {
                        reader.Skip();
                    }
                }
            }

            throw new JsonException();
        }

    }
}