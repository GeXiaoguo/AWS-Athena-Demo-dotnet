﻿using System;
using System.Threading.Tasks;
using Amazon.Athena;
using Amazon.Athena.Model;

namespace AthenaDemo
{
    public static partial class AthenaClientLight
    {
        public static async Task<GetQueryResultsResponse> QueryAsyncLight(this AmazonAthenaClient client, StartQueryExecutionRequest request, int timeoutSeconds)
        {
            var executionResult = await client.StartQueryExecutionAsync(request);

            var queryExecutionRequest = new GetQueryExecutionRequest
            {
                QueryExecutionId = executionResult.QueryExecutionId
            };

            return await Task.Run<GetQueryResultsResponse>(async () =>
              {
                  var start = DateTime.Now;
                  while ((DateTime.Now - start).Seconds < timeoutSeconds)
                  {
                      await Task.Delay(1000);
                      var response = await client.GetQueryExecutionAsync(queryExecutionRequest);
                      switch (response.QueryExecution.Status.State)
                      {
                          case var queued when queued == QueryExecutionState.QUEUED:
                          case var running when running == QueryExecutionState.RUNNING:
                              continue;
                          case var cancelled when cancelled == QueryExecutionState.CANCELLED:
                              {
                                  throw new AthenaQueryExecutionException($"The query({executionResult.QueryExecutionId}) has been calceled", response);
                              }
                          case var failed when failed == QueryExecutionState.FAILED:
                              {
                                  throw new AthenaQueryExecutionException($"The query({executionResult.QueryExecutionId}) failed", response);
                              }
                          case var secceeded when secceeded == QueryExecutionState.SUCCEEDED:
                              {
                                  var resultRequest = new GetQueryResultsRequest
                                  {
                                      QueryExecutionId = executionResult.QueryExecutionId
                                  };

                                  var result = await client.GetQueryResultsAsync(resultRequest);
                                  return result;
                              }
                          default:
                              throw new AthenaQueryExecutionException($"Unrecognized query({response.QueryExecution.QueryExecutionId}) State", response);
                      }
                  }
                  throw new AthenaQueryExecutionException($"query({executionResult.QueryExecutionId}) Timeout", null);
              });
        }
    }
}
