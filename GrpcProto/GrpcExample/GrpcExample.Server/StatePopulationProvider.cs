using System.Collections.Generic;

namespace GrpcExample.Server
{
    public class StatePopulationProvider : IStatePopulationProvider
    {
        private readonly IDictionary<string, long> _states = new Dictionary<string, long> {
            { "NJ", 10000 },
            { "NY", 20000 },
            { "MD", 30000 },
            { "KY", 40000 },
        };

        public long Get(string state)
        {
            if (_states.ContainsKey(state))
            {
                return _states[state];
            }
            return 0;
        }
    }
}
