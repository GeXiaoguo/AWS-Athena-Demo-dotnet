using Amazon;
using Amazon.Athena;
using Amazon.Athena.Model;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AthenaDemo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var client = new AmazonAthenaClient("AKIASR7SM37VRJWO6HO5", "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx", RegionEndpoint.APSoutheast2);

            var queryRequest = new StartQueryExecutionRequest
            {
                QueryString = "select * from demodb.rates_raw",
                ResultConfiguration = new ResultConfiguration
                {
                    OutputLocation = "s3://result-folder/"
                }
            };

            var result = await client.QueryAsyncLight(queryRequest, 5);

            var rows = result.ResultSet.Rows;
            foreach (var row in rows)
            {
                Console.WriteLine(string.Join(",", row.Data.Select(x => x.VarCharValue)));
            }
        }
    }
}
