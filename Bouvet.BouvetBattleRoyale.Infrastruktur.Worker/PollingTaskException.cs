namespace Bouvet.BouvetBattleRoyale.Infrastruktur.Worker
{
    using System;
    using System.Runtime.Serialization;

    namespace Brisebois.WindowsAzure
    {
        [Serializable]
        public class PollingTaskException : Exception
        {
            public PollingTaskException()
            {
            }

            public PollingTaskException(string message)
                : base(message)
            {
            }
            public PollingTaskException(string message, Exception innerException) :
                base(message, innerException)
            {         
            }
            protected PollingTaskException(SerializationInfo info,
                                           StreamingContext context)
                : base(info, context)
            {              
            }
        }
    }
}