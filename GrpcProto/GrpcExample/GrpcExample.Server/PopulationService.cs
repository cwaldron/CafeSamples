using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using GrpcPopulation;

namespace GrpcExample.Server
{
    public class PopulationService : PopulationProvider.PopulationProviderBase
    {
        private readonly IStatePopulationProvider _statePopulationProvider;

        public PopulationService(IStatePopulationProvider statePopulationProvider)
        {
            this._statePopulationProvider = statePopulationProvider;
        }

        public override async Task<PopulationResponse> GetPopulation(IAsyncStreamReader<PopulationRequest> requestStream, ServerCallContext context)
        {
            var statePopulations = new List<long>();
            while (await requestStream.MoveNext())
            {
                var populationRequest = requestStream.Current;
                statePopulations.Add(_statePopulationProvider.Get(populationRequest.State));
            }

            return new PopulationResponse { Count = statePopulations.Sum() };
        }
    }
}
