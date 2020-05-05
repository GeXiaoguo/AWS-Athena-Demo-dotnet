drop table demodb.rates_raw
CREATE EXTERNAL TABLE IF NOT EXISTS demodb.rates_raw (
  `rate_id` string,
  `bid` decimal(11,5),
  `mid` decimal(11,5),
  `ask` decimal(11,5),
  `description` string)
ROW FORMAT DELIMITED 
  FIELDS TERMINATED BY ',' 
STORED AS INPUTFORMAT 
  'org.apache.hadoop.mapred.TextInputFormat' 
OUTPUTFORMAT 
  'org.apache.hadoop.hive.ql.io.HiveIgnoreKeyTextOutputFormat'
LOCATION 's3://xge-athena-demo/rates_raw/'
TBLPROPERTIES ('has_encrypted_data'='false');


select * from demodb.rates_raw

drop table demodb.rates_processed

CREATE EXTERNAL TABLE IF NOT EXISTS demodb.rates_processed (
  `rate_date` date,
  `rate_id` string,
  `bid` decimal(11,5),
  `mid` decimal(11,5),
  `ask` decimal(11,5),
  `description` string 
)
ROW FORMAT SERDE 'org.apache.hadoop.hive.serde2.lazy.LazySimpleSerDe'
WITH SERDEPROPERTIES (
  'serialization.format' = ',',
  'field.delim' = ','
) LOCATION 's3://xge-athena-demo/rates_processed/'
TBLPROPERTIES ('skip.header.line.count'='1');

select * from demodb.rates_processed
where rate_date > cast('2020-4-1' as date)

select *
from demodb.mapping
join demodb.rates_processed rate on
mapping.rate_id = rate.rate_id
where rate_date > cast('2020-4-1' as date)
and rate_code like '%AUD%'
