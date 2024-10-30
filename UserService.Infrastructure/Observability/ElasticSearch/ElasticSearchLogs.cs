using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.Interfaces;
using UserService.Domain.Entities;
using UserService.Domain.Entities.Abstract;

namespace UserService.Infrastructure.Observability.ElasticSearch
{
    internal class ElasticSearchLogs : ILogService
    {
        protected readonly IElasticClient _elasticClient;
        public ElasticSearchLogs(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public async Task Log<TLog>(TLog log) where TLog : PersistentLog
        {
            var response = await _elasticClient.IndexDocumentAsync(log);
            if (!response.IsValid)
            {
                // TODO: Create an error handler
            }
        }
    }
}
