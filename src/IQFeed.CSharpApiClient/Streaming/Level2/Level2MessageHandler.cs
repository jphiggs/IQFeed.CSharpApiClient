﻿using System;
using System.Text;
using IQFeed.CSharpApiClient.Streaming.Common.Messages;
using IQFeed.CSharpApiClient.Streaming.Level2.Messages;

namespace IQFeed.CSharpApiClient.Streaming.Level2
{
    public class Level2MessageHandler : ILevel2Event
    {
        public event Action<UpdateSummaryMessage> Summary;
        public event Action<NameLevelQueryResponseMessage> QueryResponse;
        public event Action<SymbolNotFoundMessage> SymbolNotFound;
        public event Action<SystemMessage> System;
        public event Action<ErrorMessage> Error;
        public event Action<TimestampMessage> Timestamp;
        public event Action<UpdateSummaryMessage> Update;

        public void ProcessMessages(byte[] messageBytes, int count)
        {
            string[] messages = Encoding.ASCII.GetString(messageBytes, 0, count - 1).Split(IQFeedDefault.ProtocolLineFeedCharacter);

            for (int i = 0; i < messages.Length; i++)
            {
                var message = messages[i];
                switch (messages[i][0])
                {
                    case 'Z': // A summary message
                        ProcessSummaryMessage(message);
                        break;
                    case '2': // An update message
                        ProcessUpdateMessage(message);
                        break;
                    case 'T': // A timestamp message
                        ProcessTimestampMessage(message);
                        break;
                    case 'M': // A Market Maker name OR order book level query response message.
                        ProcessNameLevelQueryResponseMessage(message);
                        break;
                    case 'S': // A system message
                        ProcessSystemMessage(message);
                        break;
                    case 'n': // Symbol not found message
                        ProcessSymbolNotFoundMessage(message);
                        break;
                    case 'E': // An error message
                        ProcessErrorMessage(message);
                        break;
                    case 'O': // A depeprecated message included only for backwards compatability
                        break;
                    default:
                        throw new Exception("Unknown type of level 2 message received.");
                }
            }
        }

        private void ProcessSummaryMessage(string msg)
        {
            var updateSummaryMessage = UpdateSummaryMessage.Parse(msg);
            Summary?.Invoke(updateSummaryMessage);
        }

        private void ProcessUpdateMessage(string msg)
        {
            var updateSummaryMessage = UpdateSummaryMessage.Parse(msg);
            Update?.Invoke(updateSummaryMessage);
        }

        private void ProcessTimestampMessage(string msg)
        {
            var timestampMessage = TimestampMessage.Parse(msg);
            Timestamp?.Invoke(timestampMessage);
        }

        private void ProcessNameLevelQueryResponseMessage(string msg)
        {
            var nameLevelQueryResponseMessage = NameLevelQueryResponseMessage.Parse(msg);
            QueryResponse?.Invoke(nameLevelQueryResponseMessage);
        }

        private void ProcessSystemMessage(string msg)
        {
            var systemMessage = SystemMessage.Parse(msg);
            System?.Invoke(systemMessage);
        }

        private void ProcessSymbolNotFoundMessage(string msg)
        {
            var symbolNotFoundMessage = SymbolNotFoundMessage.Parse(msg);
            SymbolNotFound?.Invoke(symbolNotFoundMessage);
        }

        private void ProcessErrorMessage(string msg)
        {
            var errorMessage = ErrorMessage.Parse(msg);
            Error?.Invoke(errorMessage);
        }
    }
}