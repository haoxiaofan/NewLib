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
using NewLibCore.Data.Mongodb.Bson.ObjectModel;
using NewLibCore.Data.Mongodb.Bson.Serialization.Serializers;
using NewLibCore.Data.Mongodb.Core.Core.Bindings;
using NewLibCore.Data.Mongodb.Core.Core.Misc;
using NewLibCore.Data.Mongodb.Core.Core.WireProtocol.Messages.Encoders;

namespace NewLibCore.Data.Mongodb.Core.Core.Operations
{
    /// <summary>
    /// Represents a drop index operation.
    /// </summary>
    public class DropIndexOperation : IWriteOperation<BsonDocument>
    {
        // fields
        private readonly CollectionNamespace _collectionNamespace;
        private readonly string _indexName;
        private readonly MessageEncoderSettings _messageEncoderSettings;

        // constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DropIndexOperation"/> class.
        /// </summary>
        /// <param name="collectionNamespace">The collection namespace.</param>
        /// <param name="keys">The keys.</param>
        /// <param name="messageEncoderSettings">The message encoder settings.</param>
        public DropIndexOperation(
            CollectionNamespace collectionNamespace,
            BsonDocument keys,
            MessageEncoderSettings messageEncoderSettings)
            : this(collectionNamespace, IndexNameHelper.GetIndexName(keys), messageEncoderSettings)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DropIndexOperation"/> class.
        /// </summary>
        /// <param name="collectionNamespace">The collection namespace.</param>
        /// <param name="indexName">The name of the index.</param>
        /// <param name="messageEncoderSettings">The message encoder settings.</param>
        public DropIndexOperation(
            CollectionNamespace collectionNamespace,
            string indexName,
            MessageEncoderSettings messageEncoderSettings)
        {
            _collectionNamespace = Ensure.IsNotNull(collectionNamespace, nameof(collectionNamespace));
            _indexName = Ensure.IsNotNullOrEmpty(indexName, nameof(indexName));
            _messageEncoderSettings = Ensure.IsNotNull(messageEncoderSettings, nameof(messageEncoderSettings));
        }

        // properties
        /// <summary>
        /// Gets the collection namespace.
        /// </summary>
        /// <value>
        /// The collection namespace.
        /// </value>
        public CollectionNamespace CollectionNamespace
        {
            get { return _collectionNamespace; }
        }

        /// <summary>
        /// Gets the name of the index.
        /// </summary>
        /// <value>
        /// The name of the index.
        /// </value>
        public string IndexName
        {
            get { return _indexName; }
        }

        /// <summary>
        /// Gets the message encoder settings.
        /// </summary>
        /// <value>
        /// The message encoder settings.
        /// </value>
        public MessageEncoderSettings MessageEncoderSettings
        {
            get { return _messageEncoderSettings; }
        }

        // methods
        internal BsonDocument CreateCommand()
        {
            return new BsonDocument
            {
                { "dropIndexes", _collectionNamespace.CollectionName },
                { "index", _indexName }
            };
        }

        private WriteCommandOperation<BsonDocument> CreateOperation()
        {
            var command = CreateCommand();
            return new WriteCommandOperation<BsonDocument>(_collectionNamespace.DatabaseNamespace, command, BsonDocumentSerializer.Instance, _messageEncoderSettings);
        }

        /// <inheritdoc/>
        public BsonDocument Execute(IWriteBinding binding, CancellationToken cancellationToken)
        {
            Ensure.IsNotNull(binding, nameof(binding));
            try
            {
                var operation = CreateOperation();
                return operation.Execute(binding, cancellationToken);
            }
            catch (MongoCommandException ex)
            {
                if (ShouldIgnoreException(ex))
                {
                    return ex.Result;
                }
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<BsonDocument> ExecuteAsync(IWriteBinding binding, CancellationToken cancellationToken)
        {
            Ensure.IsNotNull(binding, nameof(binding));
            try
            {
                var operation = CreateOperation();
                return await operation.ExecuteAsync(binding, cancellationToken).ConfigureAwait(false);
            }
            catch (MongoCommandException ex)
            {
                if (ShouldIgnoreException(ex))
                {
                    return ex.Result;
                }
                throw;
            }
        }

        private bool ShouldIgnoreException(MongoCommandException ex)
        {
            return ex.ErrorMessage != null && ex.ErrorMessage.Contains("ns not found");
        }
    }
}
