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

using NewLib.Data.Mongodb.Bson.ObjectModel;
using NewLib.Data.Mongodb.Core.Core.Connections;
using NewLib.Data.Mongodb.Core.Core.Misc;
using NewLib.Data.Mongodb.Core.Core.WireProtocol.Messages;
using NewLib.Data.Mongodb.Core.Core.WireProtocol.Messages.Encoders;

namespace NewLib.Data.Mongodb.Core.Core.WireProtocol
{
    internal class DeleteWireProtocol : WriteWireProtocolBase
    {
        // fields
        private readonly bool _isMulti;
        private readonly BsonDocument _query;

        // constructors
        public DeleteWireProtocol(
            CollectionNamespace collectionNamespace,
            BsonDocument query,
            bool isMulti,
            MessageEncoderSettings messageEncoderSettings,
            WriteConcern writeConcern)
            : base(collectionNamespace, messageEncoderSettings, writeConcern)
        {
            _query = Ensure.IsNotNull(query, nameof(query));
            _isMulti = isMulti;
        }

        // methods
        protected override RequestMessage CreateWriteMessage(IConnection connection)
        {
            return new DeleteMessage(
                RequestMessage.GetNextRequestId(),
                CollectionNamespace,
                _query,
                _isMulti);
        }
    }
}
