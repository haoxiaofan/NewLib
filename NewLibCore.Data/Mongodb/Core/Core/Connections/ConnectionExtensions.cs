/* Copyright 2013-2015 MongoDB Inc.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

using System.Threading;
using System.Threading.Tasks;
using NewLib.Data.Mongodb.Core.Core.Misc;
using NewLib.Data.Mongodb.Core.Core.WireProtocol.Messages;
using NewLib.Data.Mongodb.Core.Core.WireProtocol.Messages.Encoders;

namespace NewLib.Data.Mongodb.Core.Core.Connections
{
    /// <summary>
    /// Represents internal IConnection extension methods (used to easily access the IConnectionInternal methods).
    /// </summary>
    internal static class ConnectionExtensions
    {
        // static methods
        public static void SendMessage(this IConnection connection, RequestMessage message, MessageEncoderSettings messageEncoderSettings, CancellationToken cancellationToken)
        {
            Ensure.IsNotNull(connection, nameof(connection));
            connection.SendMessages(new[] { message }, messageEncoderSettings, cancellationToken);
        }

        public static Task SendMessageAsync(this IConnection connection, RequestMessage message, MessageEncoderSettings messageEncoderSettings, CancellationToken cancellationToken)
        {
            Ensure.IsNotNull(connection, nameof(connection));
            return connection.SendMessagesAsync(new[] { message }, messageEncoderSettings, cancellationToken);
        }
    }
}
