using System.Collections.Generic;
using System.Text;
using System.IO;

namespace CSVDemo
{
    public static class CSVFileHelper
    {
        public static void Save<T>(string fileName, IEnumerable<T> tableContent, IEnumerable<string> properties) 
        {
            var builder = new StringBuilder();
            var type = typeof(T);
            using(var streamWriter = File.CreateText(fileName))
            {
                foreach (var prop in properties)
                {
                    builder.AppendFormat("{0},", prop);
                }
                builder.Remove(builder.Length - 1, 1);
                builder.Append("\n");
                streamWriter.Write(builder.ToString());
                builder.Clear();
                
                foreach (var entity in tableContent)
                {
                    foreach(var prop in properties)
                    {
                        var propInfo = type.GetProperty(prop);
                        var propValue= propInfo.GetValue(entity);
                        var strFlag = propInfo.PropertyType.Name == "String";

                        if (strFlag)
                        {
                            builder.Append("\"");
                            var propStr = propValue as string;
                            if (propStr != null)
                            {
                                var propValueBuilder = new StringBuilder();
                                for (int i = 0; i < propStr.Length; i++)
                                {
                                    if (propStr[i] == '\"')
                                    {
                                        propValueBuilder.Append("\"");
                                    }
                                    propValueBuilder.Append(propStr[i]);
                                }
                                builder.Append(propValueBuilder);
                            }
                            builder.Append("\",");
                        }
                        else
                        {
                            builder.AppendFormat("{0},", propValue);
                        }
                    }
                    builder.Remove(builder.Length - 1, 1);
                    builder.Append("\n");
                    streamWriter.Write(builder.ToString());
                    builder.Clear();
                }
            }
        }
    }
}
