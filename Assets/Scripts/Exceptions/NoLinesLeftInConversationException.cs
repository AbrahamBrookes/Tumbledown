using System;

namespace Tumbledown.Exceptions
{
    public class NoLinesLeftInConversationException : Exception
    {
		public NoLinesLeftInConversationException() : base() { }
		public NoLinesLeftInConversationException(string message) : base(message) { }
		public NoLinesLeftInConversationException(string message, Exception inner) : base(message, inner) { }
    }
}
