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
    public class ElasticSearchAuthLogs : IAuthenticationLogs
    {
        private readonly IElasticClient _elasticClient;

        public ElasticSearchAuthLogs(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public async Task LogFailedLoginAsync(FailedLoginAttemptLog failedLoginLog)
        {
            var response = await _elasticClient.IndexDocumentAsync(failedLoginLog);
            if (!response.IsValid)
            {
                // TODO: Create an error handler
            }
            //TODO: Add method to verify if this ip is trying to access multiple accounts
        }
    }
}
