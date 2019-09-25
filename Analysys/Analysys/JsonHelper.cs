using Jayrock.Json.Conversion;
using System;

namespace Analysys
{
    public class JsonHelper
    {
        public static string Serialize(object obj)
        {
            try
            {
                return JsonConvert.ExportToString(obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
