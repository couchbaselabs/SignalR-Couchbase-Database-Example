#region [ License information          ]

/* ************************************************************
 *
 *    @author Couchbase <info@couchbase.com>
 *    @copyright 2021 Couchbase, Inc.
 *
 *    Licensed under the Apache License, Version 2.0 (the "License");
 *    you may not use this file except in compliance with the License.
 *    You may obtain a copy of the License at
 *
 *        http://www.apache.org/licenses/LICENSE-2.0
 *
 *    Unless required by applicable law or agreed to in writing, software
 *    distributed under the License is distributed on an "AS IS" BASIS,
 *    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *    See the License for the specific language governing permissions and
 *    limitations under the License.
 *
 * ************************************************************/

#endregion

using SignalR.Tester.Core;
using SignalR.Tester.Core.Agents;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;
using System.Threading.Tasks;

namespace SignalR.Couchbase.LoadTester
{

    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Export(typeof(IAgent))]
    [ExportMetadata("AgentName", "Test Agent")]
    [ExportMetadata("AgentDescription", "Test Agent")]
    public class TestAgent : AgentBase
    {
        [ImportingConstructor]
        public TestAgent(Recomposable<ConnectionArgument> arguments) : base(arguments)
        {
        }

        List<string> RandomUsers;

        protected override Tuple<string, Func<string, Task<object[]>>> MethodToInvokeOnAgentStarted()
        {
            return null;
        }

        protected override List<Tuple<string, Func<string, Task<object[]>>>> PreRegisterMethods()
        {
            return null;
        }

        private Task<object[]> Send(string data)
        {
            //pick up a random user from list
            Random rnd = new Random();
            var randomPosition = rnd.Next(0, RandomUsers.Count - 1);
            var randomUser = RandomUsers[randomPosition];
            return Task.FromResult(new object[] { randomUser, RandomString(10, true) });
        }

        private string RandomString(int size, bool lowerCase)
        {
            var builder = new StringBuilder();
            var random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }
    }
}
