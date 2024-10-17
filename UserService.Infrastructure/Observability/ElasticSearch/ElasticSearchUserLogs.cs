using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.Interfaces;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Observability.ElasticSearch
{
    internal class ElasticSearchUserLogs : IObservationService
    {
        private readonly IElasticClient _elasticClient;

        public ElasticSearchUserLogs(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public async Task LogObservationAsync(Observation observation)
        {
            var response = await _elasticClient.IndexDocumentAsync(observation);
            if (!response.IsValid)
            {
                // TODO: Create an error handler
            }
        }
    }
}
