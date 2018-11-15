/* Copyright 2015 MongoDB Inc.
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

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using NewLibCore.Data.Mongodb.Bson.Serialization;
using NewLibCore.Data.Mongodb.Core;
using NewLibCore.Data.Mongodb.Core.Core.Misc;
using NewLibCore.Data.Mongodb.Driver.Linq.Processors;
using NewLibCore.Data.Mongodb.Driver.Linq.Processors.Pipeline;
using NewLibCore.Data.Mongodb.Driver.Linq.Translators;
using NewLibCore.Data.Mongodb.Driver.Support;

namespace NewLibCore.Data.Mongodb.Driver.Linq
{
    internal sealed class MongoQueryProviderImpl<TDocument> : IMongoQueryProvider
    {
        private readonly IMongoCollection<TDocument> _collection;
        private readonly AggregateOptions _options;

        public MongoQueryProviderImpl(IMongoCollection<TDocument> collection, AggregateOptions options)
        {
            _collection = Ensure.IsNotNull(collection, nameof(collection));
            _options = Ensure.IsNotNull(options, nameof(options));
        }

        public CollectionNamespace CollectionNamespace => _collection.CollectionNamespace;

        public IBsonSerializer CollectionDocumentSerializer => _collection.DocumentSerializer;

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new MongoQueryableImpl<TDocument, TElement>(this, expression);
        }

        public IQueryable CreateQuery(Expression expression)
        {
            Ensure.IsNotNull(expression, nameof(expression));

            var elementType = expression.Type.GetSequenceElementType();

            try
            {
                return (IQueryable)Activator.CreateInstance(
                    typeof(MongoQueryableImpl<,>).MakeGenericType(typeof(TDocument), elementType),
                    new object[] { this, expression });
            }
            catch (TargetInvocationException tie)
            {
                throw tie.InnerException;
            }
        }

        public TResult Execute<TResult>(Expression expression)
        {
            var result = Execute(expression);
            return (TResult)result;
        }

        public object Execute(Expression expression)
        {
            var executionPlan = ExecutionPlanBuilder.BuildPlan(
                Expression.Constant(this),
                Translate(expression));

            var lambda = Expression.Lambda(executionPlan);
            try
            {
                return lambda.Compile().DynamicInvoke(null);
            }
            catch (TargetInvocationException tie)
            {
                throw tie.InnerException;
            }
        }

        public Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken = default(CancellationToken))
        {
            var executionPlan = ExecutionPlanBuilder.BuildAsyncPlan(
                Expression.Constant(this),
                Translate(expression),
                Expression.Constant(cancellationToken));

            var lambda = Expression.Lambda(executionPlan);
            return (Task<TResult>)lambda.Compile().DynamicInvoke(null);
        }

        public QueryableExecutionModel GetExecutionModel(Expression expression)
        {
            return Translate(expression).Model;
        }

        internal object ExecuteModel(QueryableExecutionModel model)
        {
            return model.Execute(_collection, _options);
        }

        private Task ExecuteModelAsync(QueryableExecutionModel model, CancellationToken cancellationToken)
        {
            return model.ExecuteAsync(_collection, _options, cancellationToken);
        }

        private Expression Prepare(Expression expression)
        {
            expression = PartialEvaluator.Evaluate(expression);
            expression = Transformer.Transform(expression);
            expression = PipelineBinder.Bind(expression, _collection.Settings.SerializerRegistry);

            return expression;
        }

        private QueryableTranslation Translate(Expression expression)
        {
            var pipelineExpression = Prepare(expression);
            return QueryableTranslator.Translate(pipelineExpression, _collection.Settings.SerializerRegistry);
        }
    }
}