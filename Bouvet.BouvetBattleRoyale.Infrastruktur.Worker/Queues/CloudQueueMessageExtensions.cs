namespace Bouvet.BouvetBattleRoyale.Infrastruktur.Worker.Queues
{
    using System;
    using System.Reflection;
    using System.Text;

    using Microsoft.WindowsAzure.Storage.Queue;

    using Newtonsoft.Json;

    public static class CloudQueueMessageExtensions
    {
        public static CloudQueueMessage Serialize(Object o)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(o.GetType().FullName);
            stringBuilder.Append(':');
            stringBuilder.Append(JsonConvert.SerializeObject(o));
            return new CloudQueueMessage(stringBuilder.ToString());
        }

        public static string GetMessageTypeName(this CloudQueueMessage m)
        {
            EnsureTypeInMessage(m);

            int indexOf = m.AsString.IndexOf(':');

            string fullName = m.AsString.Substring(0, indexOf);

            string className = fullName.Substring(fullName.LastIndexOf(".") + 1, fullName.Length - fullName.LastIndexOf(".") - 1);

            return className;
        }

        private static void EnsureTypeInMessage<T>(this CloudQueueMessage m)
        {
            int indexOf = m.AsString.IndexOf(':');

            if (indexOf <= 0)
                throw new Exception(string.Format("Cannot deserialize into object of type {0}",
                    typeof(T).FullName));
        }

        private static void EnsureTypeInMessage(this CloudQueueMessage m)
        {
            int indexOf = m.AsString.IndexOf(':');

            if (indexOf <= 0)
                throw new Exception(string.Format("Cannot deserialize object. Missing type"));
        }
        public static T Deserialize<T>(this CloudQueueMessage m)
        {
            EnsureTypeInMessage<T>(m);
            
            int indexOf = m.AsString.IndexOf(':');

            string typeName = m.AsString.Substring(0, indexOf);
            string json = m.AsString.Substring(indexOf + 1);

            EnsureTypeInMessageIsExpected<T>(typeName);

            return JsonConvert.DeserializeObject<T>(json);
        }

        private static void EnsureTypeInMessageIsExpected<T>(string typeName)
        {
            if (typeName != typeof(T).FullName)
            {
                throw new Exception(string.Format("Cannot deserialize object of type {0} into one of type {1}",
                    typeName,
                    typeof(T).FullName));
            }
        }
    }
}