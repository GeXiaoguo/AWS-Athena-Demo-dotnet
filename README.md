
# AWS Athena Demo in C#

Assuming you already setup AWS Athena and able to query data from sql-workbench. The next step is to query data programmatically. This is a simple demo of how to query AWS Athena data with C#.

`QueryAsyncLight` is an extension function that helps with making the querying code simpler. Have a look at AthenaClientLight.cs if you want to look under the hood.
                
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


