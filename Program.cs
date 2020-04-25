using Amazon;
using Amazon.Athena;
using Amazon.Athena.Model;
using System.Threading.Tasks;

namespace AthenaDemo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var client = new AmazonAthenaClient("access-key-id", "access-key", RegionEndpoint.APSoutheast2);
            var queryRequest = new StartQueryExecutionRequest
            {
                QueryString = "select * from athena-database-name.table-name",
                ResultConfiguration = new ResultConfiguration
                {
                    OutputLocation = "s3://s3-bucket-name-for-query-result/"
                }
            };
            var result = await client.QueryAsyncLight(queryRequest, 5);

            var rows = result.ResultSet.Rows;
        }
    }
}
