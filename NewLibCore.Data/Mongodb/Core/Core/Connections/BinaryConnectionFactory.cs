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

using System.Net;
using NewLib.Data.Mongodb.Core.Core.Configuration;
using NewLib.Data.Mongodb.Core.Core.Events;
using NewLib.Data.Mongodb.Core.Core.Misc;
using NewLib.Data.Mongodb.Core.Core.Servers;

namespace NewLib.Data.Mongodb.Core.Core.Connections
{
    /// <summary>
    /// Represents a factory of BinaryConnections.
    /// </summary>
    internal class BinaryConnectionFactory : IConnectionFactory
    {
        #region static
        // static fields
        private static readonly IConnectionInitializer __connectionInitializer;

        // static constructor
        static BinaryConnectionFactory()
        {
            __connectionInitializer = new ConnectionInitializer();
        }
        #endregion

        // fields
        private readonly IEventSubscriber _eventSubscriber;
        private readonly ConnectionSettings _settings;
        private readonly IStreamFactory _streamFactory;

        // constructors
        public BinaryConnectionFactory(ConnectionSettings settings, IStreamFactory streamFactory, IEventSubscriber eventSubscriber)
        {
            _settings = Ensure.IsNotNull(settings, nameof(settings));
            _streamFactory = Ensure.IsNotNull(streamFactory, nameof(streamFactory));
            _eventSubscriber = Ensure.IsNotNull(eventSubscriber, nameof(eventSubscriber));
        }

        // methods
        public IConnection CreateConnection(ServerId serverId, EndPoint endPoint)
        {
            Ensure.IsNotNull(serverId, nameof(serverId));
            Ensure.IsNotNull(endPoint, nameof(endPoint));
            return new BinaryConnection(serverId, endPoint, _settings, _streamFactory, __connectionInitializer, _eventSubscriber);
        }
    }
}
