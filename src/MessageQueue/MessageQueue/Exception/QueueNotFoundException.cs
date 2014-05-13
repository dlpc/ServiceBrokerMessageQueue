using System;
using System.Runtime.Serialization;

namespace MessageQueue.Exception
{
    [Serializable]
    public class QueueNotFoundException : System.Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public QueueNotFoundException()
        {
        }

        public QueueNotFoundException(string message)
            : base(message)
        {
        }

        public QueueNotFoundException(string message, System.Exception inner)
            : base(message, inner)
        {
        }

        protected QueueNotFoundException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}