using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaskedDeformableRegistrationApp.WSIExtraction
{
    class ReadWriteStack
    {
        public static StackList GetStacksFromJson(string jsonFilePath)
        {
            try
            {
                string content = File.ReadAllText(jsonFilePath);
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore

                };
                return JsonConvert.DeserializeObject<StackList>(content);
            }
            catch (Exception)
            {
                // json not found or wrong format
                return null;
            }
        }
    }
}
