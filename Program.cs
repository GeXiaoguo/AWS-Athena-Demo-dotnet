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
            var client = new AmazonAthenaClient("AKIASR7SM37VRBTWH7OS", "xxx-xxx-xxxxxxxxxx", RegionEndpoint.APSoutheast2);
            var queryRequest = new StartQueryExecutionRequest
            {
                QueryString = "select * from cartoorates.lon_eod",
                ResultConfiguration = new ResultConfiguration
                {
                    OutputLocation = "s3://athena-query-result-cartoo/"
                }
            };
            var result = await client.QueryAsyncLight(queryRequest, 1);

            var rows = result.ResultSet.Rows;
        }
    }
}
