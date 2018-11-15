﻿/* Copyright 2015 MongoDB Inc.
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

using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NewLibCore.Data.Mongodb.Driver.Linq.Expressions;
using NewLibCore.Data.Mongodb.Driver.Linq.Expressions.ResultOperators;

namespace NewLibCore.Data.Mongodb.Driver.Linq.Processors.EmbeddedPipeline.MethodCallBinders
{
    internal sealed class ContainsBinder : IMethodCallBinder<EmbeddedPipelineBindingContext>
    {
        public static string[] SupportedMethodNames
        {
            get { return new[] { "Contains" }; }
        }

        public static bool IsSupported(MethodCallExpression node)
        {
            if (node.Method.DeclaringType == typeof(string))
            {
                return false;
            }
            return
                (node.Object != null && node.Arguments.Count == 1) ||
                (node.Object == null && node.Arguments.Count == 2);
        }

        public Expression Bind(PipelineExpression pipeline, EmbeddedPipelineBindingContext bindingContext, MethodCallExpression node, IEnumerable<Expression> arguments)
        {
            var value = bindingContext.Bind(arguments.Single());

            return new PipelineExpression(
                pipeline.Source,
                pipeline.Projector,
                new ContainsResultOperator(value));
        }
    }
}
