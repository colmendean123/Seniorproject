using System.Collections;
using System.Collections.Generic;



namespace RPGExceptions
{
    [System.Serializable]
    public class ObjectVariableException : System.Exception
    {
        public ObjectVariableException() : base() { }
        public ObjectVariableException(string message) : base(message) { }
        public ObjectVariableException(string message, System.Exception inner) : base(message, inner) { }

        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client.
        protected ObjectVariableException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}