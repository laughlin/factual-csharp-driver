factual-csharp-driver
======================

Officially supported .NET driver for [Factual's public API](http://developer.factual.com).

[![NuGet Status](http://nugetstatus.com/factualdriver.png)](http://nugetstatus.com/packages/factualdriver)

# Installation

## Nuget

In the package manager console window: <tt>Install-Package FactualDriver</tt>, or right click on your project, click
manage packages, search for factual and install. Project uses nuget automatic package restore feature, please keep in mind
if you don't have automatic restore enabled you would need to enable it in. In Visual Studio, enable "Allow NuGet to download
missing packages during build". This setting lives under Options -> Package Manager -> General.
![Enable package restore](https://f.cloud.github.com/assets/2625292/1150750/41a9f6ee-1eef-11e3-82d7-f27dc42c5e94.png)

## Non Nuget

You can either clone the github [repository](https://github.com/Factual/factual-csharp-driver), or [download as a zip or tar] (https://github.com/Factual/factual-csharp-driver/downloads) and then build.

# Basic Design

The driver allows you to create an authenticated handle to Factual. With a Factual handle, you can send queries and get results back.

Queries are created using the Query class, which provides a fluent interface to constructing your queries.

Results are returned as the JSON returned by Factual. 

# Setup
First obtain free developer keys from factual.com

    // Create an authenticated handle to Factual
    Factual factual = new Factual(MY_KEY, MY_SECRET);
    
If you don't have a Factual API account yet, [it's free and easy to get one](https://www.factual.com/api-keys/request).   

# Request Timeout
You can optionally set a client-side request timeout for requests sent to Factual. Default values are 100000 and 300000 respectively. For example:

	// Set the request timeouts to 2.5 seconds (values must be integers larger than 0 expressed in milliseconds):
	factual.ConnectionTimeout = 2500;
	factual.ReadTimeout = 2500;

	// Set the request timeouts back to default 100000 and 300000 respectively
	factual.ConnectionTimeout = null;
	factual.ReadTimeout = null;

# Factual API URL Override
You can optionally override the default Factual API URL. Default value is http://api.v3.factual.com. For example:

	// Set the base URL to http://fakeurl.factual.com
	factual.FactualApiUrlOverride = "http://fakeurl.factual.com";

	// Set the base URL back to default http://api.v3.factual.com
	factual.FactualApiUrlOverride = null;

# Optional Tests Setup

If you are going to clone or download the repository you will have access to integration tests which can also
be used as documentation. To setup your tests you would need to add your factual key and factual secret key to the FactualDriver.Tests\app.config.	
	
# Simple Query Example

    // 3 random records from Factual's Places table:
    factual.Fetch("places", new Query().Limit(3))
	
# Full Text Search

    // Entities that match a full text search for Sushi in Santa Monica:
    factual.Fetch("places", new Query().Search("Sushi Santa Monica"));

# Get Row Example

    // Entity from Factual's Places table with ID 03c26917-5d66-4de9-96bc-b13066173c65:
    factual.GetRow("places", "03c26917-5d66-4de9-96bc-b13066173c65";

# Geo Filters

You can query Factual for entities located within a geographic area. For example:

    // Build a Query that finds entities located within 5000 meters of a latitude, longitude
    new Query().WithIn(new Circle(34.06018, -118.41835, 5000));

# Results sorting

You can have Factual sort your query results for you, on a field by field basis. Simple example:

    // Build a Query to find 10 random entities and sort them by name, ascending:
    new Query().Limit(10).SortAsc("name");
    
You can specify more than one sort, and the results will be sorted with the first sort as primary, the second sort or secondary, and so on:

    // Build a Query to find 20 random entities, sorted ascending primarily by region, then by locality, then by name:
    var q = new Query()
      .Limit(20)
      .SortAsc("region")
      .SortAsc("locality")
      .SortDesc("name");

# Limit and Offset

You can use limit and offset to support basic results paging. For example:

    // Build a Query with offset of 150, limiting the page size to 10:
    new Query().Limit(10).Offset(150);
	
# Field Selection

By default your queries will return all fields in the table. You can use the only modifier to specify the exact set of fields returned. For example:

    // Build a Query that only gets the name, tel, and category fields:
    new Query().Only("name", "tel", "category");
    
# All Top Level Query Parameters

<table>
  <tr>
    <th>Parameter</th>
    <th>Description</th>
    <th>Example</th>
  </tr>
  <tr>
    <td>filters</td>
    <td>Restrict the data returned to conform to specific conditions.</td>
    <td>q.Field("name").BeginsWith("Starbucks")</td>
  </tr>
  <tr>
    <td>include count</td>
    <td>Include a count of the total number of rows in the dataset that conform to the request based on included filters. Requesting the row count will increase the time required to return a response. The default behavior is to NOT include a row count. When the row count is requested, the Response object will contain a valid total row count via <tt>.getTotalRowCount()</tt>.</td>
    <td><tt>q.IncludeRowCount()</tt></td>
  </tr>
  <tr>
    <td>geo</td>
    <td>Restrict data to be returned to be within a geographical range based.</td>
    <td>(See the section on Geo Filters)</td>
  </tr>
  <tr>
    <td>limit</td>
    <td>Maximum number of rows to return. Default is 20. The system maximum is 50. For higher limits please contact Factual, however consider requesting a download of the data if your use case is requesting more data in a single query than is required to fulfill a single end-user's request.</td>
    <td><tt>q.Limit(10)</tt></td>
  </tr>
  <tr>
    <td>search</td>
    <td>Full text search query string.</td>
    <td>
      Find "sushi":<br><tt>q.Search("sushi")</tt><p>
      Find "sushi" or "sashimi":<br><tt>q.Search("sushi, sashimi")</tt><p>
      Find "sushi" and "santa" and "monica":<br><tt>q.Search("sushi santa monica")</tt>
    </td>
  </tr>
  <tr>
    <td>offset</td>
    <td>Number of rows to skip before returning a page of data. Maximum value is 500 minus any value provided under limit. Default is 0.</td>
    <td><tt>q.Offset(150)</tt></td>
  </tr>
  <tr>
    <td>only</td>
    <td>What fields to include in the query results.  Note that the order of fields will not necessarily be preserved in the resulting JSON response due to the nature of JSON hashes.</td>
    <td><tt>q.Only("name", "tel", "category")</tt></td>
  </tr>
  <tr>
    <td>sort</td>
    <td>The field (or secondary fields) to sort data on, as well as the direction of sort.  Supports $distance as a sort option if a geo-filter is specified.  Supports $relevance as a sort option if a full text search is specified either using the q parameter or using the $search operator in the filter parameter.  By default, any query with a full text search will be sorted by relevance.  Any query with a geo filter will be sorted by distance from the reference point.  If both a geo filter and full text search are present, the default will be relevance followed by distance.</td>
    <td><tt>q.SortAsc("name").SortDesc("$distance")</tt></td>
  </tr>
</table>  

# Row Filters

The driver supports various row filter logic. Examples:

    // Build a query to find places whose name field starts with "Starbucks"
    new Query().Field("name").BeginsWith("Starbucks");

    // Build a query to find places with a blank telephone number
    new Query().Field("tel").Blank();

## Supported row filter logic

<table>
  <tr>
    <th>Predicate</th>
    <th>Description</th>
    <th>Example</th>
  </tr>
  <tr>
    <td>equal</td>
    <td>equal to</td>
    <td><tt>q.Field("region").Equal("CA")</tt></td>
  </tr>
  <tr>
    <td>notEqual</td>
    <td>not equal to</td>
    <td><tt>q.Field("region").NotEqual("CA")</tt></td>
  </tr>
  <tr>
    <td>search</td>
    <td>full text search</td>
    <td><tt>q.Field("name").Search("fried chicken")</tt></td>
  </tr>
  <tr>
    <td>in</td>
    <td>equals any of</td>
    <td><tt>q.Field("region").In("MA", "VT", "NH", "RI", "CT")</tt></td>
  </tr>
  <tr>
    <td>notIn</td>
    <td>does not equal any of</td>
    <td><tt>q.Field("locality").NotIn("Los Angeles")</tt></td>
  </tr>
  <tr>
    <td>beginsWith</td>
    <td>begins with</td>
    <td><tt>q.Field("name").BeginsWith("b")</tt></td>
  </tr>
  <tr>
    <td>notBeginsWith</td>
    <td>does not begin with</td>
    <td><tt>q.Field("name").NotBeginsWith("star")</tt></td>
  </tr>
  <tr>
    <td>beginsWithAny</td>
    <td>begins with any of</td>
    <td><tt>q.Field("name").BeginsWithAny("star", "coffee", "tull")</tt></td>
  </tr>
  <tr>
    <td>notBeginsWithAny</td>
    <td>does not begin with any of</td>
    <td><tt>q.Field("name").NotBeginsWithAny("star", "coffee", "tull")</tt></td>
  </tr>
  <tr>
    <td>blank</td>
    <td>is blank or null</td>
    <td><tt>q.Field("tel").Blank()</tt></td>
  </tr>
  <tr>
    <td>notBlank</td>
    <td>is not blank or null</td>
    <td><tt>q.Field("tel").NotBlank()</tt></td>
  </tr>
  <tr>
    <td>greaterThan</td>
    <td>greater than</td>
    <td><tt>q.Field("rating").GreaterThan(7.5)</tt></td>
  </tr>
  <tr>
    <td>greaterThanOrEqual</td>
    <td>greater than or equal to</td>
    <td><tt>q.Field("rating").GreaterThanOrEqual(7.5)</tt></td>
  </tr>
  <tr>
    <td>lessThan</td>
    <td>less than</td>
    <td><tt>q.Field("rating").LessThan(7.5)</tt></td>
  </tr>
  <tr>
    <td>lessThanOrEqual</td>
    <td>less than or equal to</td>
    <td><tt>q.Field("rating").LessThanOrEqual(7.5)</tt></td>
  </tr>
  <tr>
    <td>includes</td>
    <td>includes</td>
    <td><tt>q.Field("category_ids").Includes(10)</tt></td>
  </tr>
  <tr>
    <td>includesAny</td>
    <td>includes any</td>
    <td><tt>q.Field("category_ids").IncludesAny(10, 100)</tt></td>
  </tr>
</table>

## AND

Queries support logical AND'ing your row filters. For example:

    // Build a query to find entities where the name begins with "Coffee" AND the telephone is blank:
    Query q = new Query();
    q.And(
      q.Field("name").BeginsWith("Coffee"),
      q.Field("tel").Blank()
    );
    
Note that all row filters set at the top level of the Query are implicitly AND'ed together, so you could also do this:

    new Query()
      .Field("name").BeginsWith("Coffee")
      .Field("tel").Blank();

## OR

Queries support logical OR'ing your row filters. For example:

    // Build a query to find entities where the name begins with "Coffee" OR the telephone is blank:
    Query q = new Query();
    q.Or(
        q.Field("name").BeginsWith("Coffee"),
        q.Field("tel").Blank());
	  
## Combined ANDs and ORs

You can nest AND and OR logic to whatever level of complexity you need. For example:

    // Build a query to find entities where:
    // (name begins with "Starbucks") OR (name begins with "Coffee")
    // OR
    // (name full text search matches on "tea" AND tel is not blank)
    Query q = new Query();
    q.Or(
        q.Or(
            q.Field("name").BeginsWith("Starbucks"),
            q.Field("name").BeginsWith("Coffee")
        ),
        q.And(
            q.Field("name").Search("tea"),
            q.Field("tel").NotBlank()
        )
    );


# Crosswalk

The driver fully supports Factual's Crosswalk feature, which lets you "crosswalk" the web and relate entities between Factual's data and that of other web authorities.

(See [the Crosswalk API](http://developer.factual.com/display/docs/Places+API+-+Crosswalk) for more background.)

Crosswalk requests are treated as any other table read, as seen in the example below.  All query-related features apply.

## Simple Crosswalk Example

    // Get all Crosswalk data for a specific Places entity, using its Factual ID:
    var response = factual.Fetch("crosswalk", new Query().Field("factual_id").Equal("97598010-433f-4946-8fd5-4a6dd1639d77")); 
          
# Finding a Match

Use the common query structure to add known attributes to the query:

    //Build the query
    MatchQuery matchQuery = new MatchQuery()
		.Add("name", "McDonalds")
        .Add("address", "10451 Santa Monica Blvd")
		.Add("region", "CA")
        .Add("postcode", "90025");

And then see if we found a match:	
	
    String id = Factual.Match("places", matchQuery); 
    // id = null means no match, id = some factual id means there is a match	

          
# Resolve

The driver fully supports Factual's Resolve feature, which lets you start with incomplete data you may have for an entity, and get potential entity matches back from Factual.

Each result record will include a confidence score (<tt>"similarity"</tt>), and a flag indicating whether Factual decided the entity is the correct resolved match with a high degree of accuracy (<tt>"resolved"</tt>).

For any Resolve query, there will be 0 or 1 entities returned with <tt>"resolved"=true</tt>. If there was a full match, it is guaranteed to be the first record in the JSON response.

(See [the Resolve Blog](http://blog.factual.com/factual-resolve) for more background.)

## Simple Resolve Example

The <tt>resolve</tt> method gives you the one full match if there is one, or null:

    // Get the entity that is a full match, or null:
    var response = Factual.Fetch("places", new ResolveQuery()
    .Add("name", "Buena Vista")
    .Add("latitude", 34.06)
    .Add("longitude", -118.40));          
          
# World Geographies

Driver fully supports Factual's World Geographies. For a complete documentation please refer to http://developer.factual.com/display/docs/World+Geographies.

## World Geographies example

            //Arrange
            var query = new Query();
                query.And
                (
                    query.Field("name").Equal("philadelphia"),
                    query.Field("country").Equal("us"),
                    query.Field("placetype").Equal("locality")
                );
            //Act
            var response = Factual.Fetch("world-geographies", query);

# Raw GET

A Raw GET request can be used to make just about any kind of query against Factual, including features we've yet to design.
	public string RawQuery(string path, Dictionary<string, object> queryParameters)

<p>You can run a GET request against the specified endpoint path, using the given parameters and your OAuth credentials. Returns the raw response body returned by Factual. The necessary URL base will be automatically prepended to path. If you need to change it, e.g. to make requests against a development instance of the Factual service, please set FactualApiUrlOverride property.</p>

## Example Raw GET Queries

	//GET only the name and category fields from places table, including the row count in the response: 
	//http://api.v3.factual.com/t/places?select=name,category&include_count=True

		string result = Factual.RawQuery("t/places", new Dictionary<string, object>
			{
				{"select", "name,category"},
				{"include_count", true}
			});
		dynamic json = JsonConvert.DeserializeObject(result);

	//GET first 5 restaurants in the Food & Beverage category:
	//http://api.v3.factual.com/t/restaurants?filters={"category":"Food+&+Beverage"}&limit=5

		string result = Factual.RawQuery("t/restaurants", new Dictionary<string, object>
			{
				{
					"filters", new Dictionary<string, object>
					{
						{
							"category", "Food & Beverage"
						}
					}
				},
				{
					"limit", 5
				}
			});
		dynamic json = JsonConvert.DeserializeObject(result);

Note that the above examples demonstrate the ability to construct read queries using the raw read feature.  However, in practice, the recommendation is to always use the convenience classes for features which are supported.

# Raw GET (Self Encoded URL)

A Raw GET Encoded URL request can be used to make just about any kind of query against Factual, including features we've yet to design.
	public string RawQuery(string path, string queryParameters)

<p>You can run a GET request against the specified endpoint path, using the given parameters and your OAuth credentials. Returns the raw response body returned by Factual. The necessary URL base will be automatically prepended to path. If you need to change it, e.g. to make requests against a development instance of the Factual service, please set FactualApiUrlOverride property. Developer is entirly responsible for correct query formatting and URL encoding.</p>

## Example Raw GET Encoded Queries

	//GET only the name and category fields from places table, including the row count in the response: 
	//http://api.v3.factual.com/t/places?select=name,category&include_count=True

		string result = Factual.RawQuery("t/places", "select=name,category&include_count=True");
		dynamic json = JsonConvert.DeserializeObject(result);

# Raw POST

A Raw POST request can be used to make just about any kind of query against Factual, including features we've yet to design.
	 public string RequestPost(string path, Dictionary<string, object> queryParameters, Dictionary<string, object> postData)

<p>You can run a POST request against the specified endpoint path, using the given parameters and your OAuth credentials. Returns the raw response body returned by Factual. The necessary URL base will be automatically prepended to path. If you need to change it, e.g. to make requests against a development instance of the Factual service, please set FactualApiUrlOverride property.</p>

## Example Raw POST Queries

	//GET only the name and category fields from places table, including the row count in the response: 
	//http://api.v3.factual.com/t/us-sandbox/submit?values={"name":"Factual+North","address":"1+North+Pole","latitude":90,"longitude":0}&user=test_driver_user

		string result = Factual.RequestPost("/t/us-sandbox/submit", new Dictionary<string, object>
			{
				{
					"values", JsonConvert.SerializeObject(new Dictionary<string, object>
					{
						{
							"name", "Factual North"
						},
						{
							"address", "1 North Pole"
						},
						{
							"latitude", 90
						},
						{
							"longitude", 0
						}
					})
				},
				{
					"user", "test_driver_user"
				}
			}, new Dictionary<string, object>());
		dynamic json = JsonConvert.DeserializeObject(result);

# Raw POST (Self Encoded URL)

A Raw POST request can be used to make just about any kind of query against Factual, including features we've yet to design.
	 public string RequestPost(string path, string queryParameters, string postData)

<p>You can run a POST request against the specified endpoint path, using the given parameters and your OAuth credentials. Returns the raw response body returned by Factual. The necessary URL base will be automatically prepended to path. If you need to change it, e.g. to make requests against a development instance of the Factual service, please set FactualApiUrlOverride property. Developer is entirly responsible for correct query formatting and URL encoding.</p>

## Example Raw POST Queries

	//GET only the name and category fields from places table, including the row count in the response: 
	//http://api.v3.factual.com/t/us-sandbox/submit?values={"name":"Factual North","address":"1 North Pole","latitude":90,"longitude":0}&user=test_driver_user

		string result = Factual.RequestPost("/t/us-sandbox/submit", "values={"name":"Factual North","address":"1 North Pole","latitude":90,"longitude":0}&user=test_driver_user", "");
		dynamic json = JsonConvert.DeserializeObject(result);

# Debug Mode

To see a full trace of debug information for a request and response, turn debug mode on.  There are two ways to do so:<p>
Use the <tt>Factual</tt> constructor to enable debug on a new instance:

	Factual factual = new Factual(key, secret, true);

or modify an existing instance to toggle debug mode on and off for individual requests:
	
	factual.Debug = true;
	factual.Fetch(...);
	factual.Debug = false;
	
Debug information will be printed to output window, with detailed request and response information.

# Facets

The driver fully supports Factual's Facets feature, which lets you return row counts for Factual tables, grouped by facets of data.  For example, you may want to query all businesses within 1 mile of a location and for a count of those businesses by category.


## Simple Facets Example

    // Returns a count of Starbucks by country
    var response = factual.Fetch("global", new FacetQuery("country").Search("starbucks"));

Not all fields are configured to return facet counts. To determine what fields you can return facets for, use the schema call.  The faceted attribute of the schema will let you know.

## All Top Level Facets Parameters

<table>
  <tr>
    <th>Parameter</th>
    <th>Description</th>
    <th>Example</th>
  </tr>
  <tr>
    <td>select</td>
    <td>The fields for which facets should be generated. The response will not be ordered identically to this list, nor will it reflect any nested relationships between fields.</td>
    <td><tt>new Facet("region", "locality");</tt></td>
  </tr>
  <tr>
    <td>min count</td>
    <td>For each facet value count, the minimum count it must show in order to be returned in the response. Must be zero or greater. The default is 1.</td>
    <td><tt>f.MinCountPerFacetValue(2)</tt></td>
  </tr>
  <tr>
    <td>limit</td>
    <td>The maximum number of unique facet values that can be returned for a single field. Range is 1-250. The default is 25.</td>
    <td><tt>f.MaxValuesPerFacet(10)</tt></td>
  </tr>
  <tr>
    <td>filters</td>
    <td>Restrict the data returned to conform to specific conditions.</td>
    <td><tt>f.Field("name").BeginsWith("Starbucks")</tt></td>
  </tr>
  <tr>
    <td>include count</td>
    <td>Include a count of the total number of rows in the dataset that conform to the request based on included filters. Requesting the row count will increase the time required to return a response. The default behavior is to NOT include a row count. When the row count is requested, the Response object will contain a valid total row count via <tt>.GetTotalRowCount()</tt>.</td>
    <td><tt>f.IncludeRowCount()</tt></td>
  </tr>
  <tr>
    <td>geo</td>
    <td>Restrict data to be returned to be within a geographical range.</td>
    <td>(See the section on Geo Filters)</td>
  </tr>
  <tr>
    <td>search</td>
    <td>Full text search query string.</td>
    <td>
      Find "sushi":<br><tt>f.Search("sushi")</tt><p>
      Find "sushi" or "sashimi":<br><tt>f.Search("sushi, sashimi")</tt><p>
      Find "sushi" and "santa" and "monica":<br><tt>f.Search("sushi santa monica")</tt>
    </td>
  </tr>
</table>  



# Multi

The driver fully supports Factual's Multi feature, which enables making multiple requests on the same connection.
Queue responses using <tt>QueueFetch</tt>, and send all queued reads using <tt>SendQueueRequests</tt>.  The <tt>SendQueueRequests</tt> method requests all reads queued since the last <tt>SendQueueRequests</tt>.  The responses from the multi request are returned in a list, corresponding to the same order in which they were queued.

## Simple Multi Example

	// Fetch a multi response
	factual.QueueFetch("places", new Query().Field("region").Equal("CA"));
	factual.QueueFetch("places", new Query().Limit(1)); 
	var multiResponse = factual.SendQueueRequests();

### Setting custom query keys
By default driver prepends query keys with a "q" and then the number of the query. You can also specify your own multi key so that response data objects will have queries returned with you own prepended key.
To do that you would need to set Factual.MultiQuery.Key property before calling QueueFetch methods.

        //Arrange
        Factual.MultiQuery.Key = "test";
        Factual.QueueFetch("places", new Query().Field("region").Equal("CA"));
        Factual.QueueFetch("places", new Query().Limit(1));


# Diffs

The driver supports Factual's Diffs feature, which enables Factual data update downloads.

## Simple Diffs Example

The <tt>Fetch</tt> method gives the diff data:

	// Request all diffs from the US Places dataset that were generated in a for window of just over 24 minutes, on Fri, 07 Dec 2012 13:41:03 -0800
	DiffsQuery diffs = new DiffsQuery()
		.After(1354916463822)
		.Before(1354917903834);
    var response = Factual.Fetch("places-us", diffs);
	

# Geopulse

The driver fully supports Factual's <a href="http://developer.factual.com/display/docs/Places+API+-+Geopulse">Geopulse</a> feature, which provides point-based access to geographic attributes: you provide a long/lat coordinate pair, we provide everything we can know about that geography. 

## Simple Geopulse Example

The <tt>geopulse</tt> method fetches results based on the given point:

	var respponse = factual.Geopulse(new Geopulse(new Point(latitude, longitude))
												.Only("income", "housing"));


## All Top Level Geopulse Parameters

<table>
  <tr>
    <th>Parameter</th>
    <th>Description</th>
    <th>Example</th>
  </tr>
  <tr>
    <td>geo</td>
    <td>A geographic point around which information is retrieved.</td>
    <td><tt>new Point(latitude, longitude)</tt></td>
  </tr>
  <tr>
    <td>select</td>
    <td>What fields to include in the query results. Note that the order of fields will not necessarily be preserved in the resulting JSON response due to the nature of JSON hashes.</td>
    <td><tt>geopulse.Only("commercial_density", "commercial_profile")</tt></td>
  </tr>
</table>	


# Reverse Geocoder

The driver fully supports Factual's <a href="http://developer.factual.com/display/docs/Places+API+-+Reverse+Geocoder">Reverse Geocoder</a> feature, which returns the nearest valid address given a longitude and latitude. 

## Simple Reverse Geocoder Example
	
The <tt>ReverseGeocode</tt> method fetches results based on the given point:

	var response = factual.ReverseGeocode(new Point(latitude, longitude));	

## All Top Level Reverse Geocoder Parameters

<table>
  <tr>
    <th>Parameter</th>
    <th>Description</th>
    <th>Example</th>
  </tr>
  <tr>
    <td>geo</td>
    <td>A valid geographic point for which the closest address is retrieved.</td>
    <td><tt>new Point(latitude, longitude)</tt></td>
  </tr>
</table>
	

#Submit

NOTICE: At the current time, this API call is ONLY compatible with places-v3. Please see the [the migration page](http://developer.factual.com/display/docs/Places+API+-+v3+Migration) for more details.
---

## Introduction

Submit allows you to add a record to Factual, or update an existing record.  To delete a record, see [Flag](https://github.com/Factual/factual-java-driver/wiki/Flag).

Factual does not currently support deleting entity attributes.

## Syntax

Strictly speaking, our systems do an 'UPSERT' -- we determine if the entity already exists, and return the Factual ID as part of the result object.  You can determine whether the entity you submitted is new
However, it's always a good idea to obtain the Factual ID from a Submit Result, and store it against the submitted entity. See below for specific examples.

In a few cases (such as if the Factual ID you submitted has been deprecated), we may return a Factual ID different from the one you submitted.  It is good practice to check.

The only difference between updating an extant record and adding a new one is the inclusion of the Factual ID.

## All Top Level Submit Parameters

<table>
  <tr>
    <th>Parameter</th>
    <th>Description</th>
    <th>Example</th>
  </tr>
  <tr>
    <td>values</td>
    <td>A JSON hash field of names and values to be added to a Factual table</td>
    <td>Update a value:<p><tt>s.setValue("longitude", 100)</tt><p>Make a value blank:<p><tt>s.removeValue("longitude")</tt></td>
  </tr>
  <tr>
    <td>user</td>
    <td>An arbitrary token representing the user submitting the data.</td>
    <td><tt>new Metadata().User("my_username")</tt></td>
  </tr>
  <tr>
    <td>comment</td>
    <td>Any english text comment that may help explain your corrections.</td>
    <td><tt>metadata.Comment("my comment")</tt></td>
  </tr>
  <tr>
    <td>reference</td>
    <td>A reference to a URL, title, person, etc. that is the source of this data.</td>
    <td><tt>metadata.Reference("http://...")</tt></td>
  </tr>
</table>

## Examples

<b><ex>Add data to Factual's us-sandbox table:</ex></b><br>
```csharp
// Entity data
Submit values = new Submit();
values.AddValue("name", "Starbucks");
values.AddValue("address", "123 Testing Blvd.");
values.AddValue("locality", "Los Angeles");
values.AddValue("region", "CA");
values.AddValue("postcode", "90049");

// An end user id is required
Metadata metadata = new Metadata().User("some_user_id");

// Run the Submit
var response = factual.Submit("us-sandbox", values, metadata);
```

<b><ex>Determine whether Factual considered your Submit to be a new entity:</ex></b><br>
```csharp
dynamic json = JsonConvert.DeserializeObject(response);
Assert.IsTrue((bool)json.response.new_entity);
```
<b><ex>Correct the latitude and longitude of a specific entity in Factual's us-sandbox table:</ex></b><br>
```csharp
Submit submit = new Submit()
  .AddValue("latitude", -79.431708)
  .AddValue("longitude", 43.641605);
factual.Submit("us-sandbox",
               "f33527e0-a8b4-4808-a820-2686f18cb00c",
               submit,
               new Metadata().user("some_user_id"));
```

<b><ex>Correct the business name of a specific entity in Factual's us-sandbox table:</ex></b><br>
```csharp
Submit submit = new Submit()
  .setValue("name", "The New & Improved Tyler's Austin");
factual.Submit("us-sandbox",
               "f33527e0-a8b4-4808-a820-2686f18cb00c",
               submit,
               new Metadata().user("some_user_id"));
```

<b><ex>Add a neighborhood to a specific entity in Factual's us-sandbox table:</ex></b><br>
```csharp
Submit submit = new Submit()
  .AddValue("neighborhood", "Downtown");
factual.Submit("us-sandbox",
               "f33527e0-a8b4-4808-a820-2686f18cb00c",
               submit,
               new Metadata().user("some_user_id"));
```

#Clear

NOTICE: At the current time, this API call is ONLY compatible with places-v3. Please see the [the migration page](http://developer.factual.com/display/docs/Places+API+-+v3+Migration) for more details.
---

## Introduction

Clear allows you to clear one or more attributes from a Factual record.

## Syntax

## All Top Level Submit Parameters

<table>
  <tr>
    <th>Parameter</th>
    <th>Description</th>
	<th>Required</th>
    <th>Example</th>
  </tr>
  <tr>
    <td>user</td>
    <td>An arbitrary token representing the end user who is submitting the data.</td>
	<td>Yes</td>
    <td><tt>new Metadata().User("my_username")</tt></td>
  </tr>
  <tr>
    <td>fields</td>
    <td>The attribute fields to be cleared.</td>
	<td>Yes</td>
    <td><tt>Clear.AddField("longitude")</tt></td>
  </tr>
  <tr>
    <td>comment</td>
    <td>Any english text comment that may help explain the submit.</td>
	<td>No</td>
    <td><tt>Metadata.Comment("my comment")</tt></td>
  </tr>
  <tr>
    <td>reference</td>
    <td>A reference to a URL, title, person, etc. that is the source of the submitted data.</td>
	<td>No</td>
    <td><tt>Metadata.Reference("http://...")</tt></td>
  </tr>
</table>

## Examples

<b><ex>Clear the value of name, address, locality, and region in an existing entity:</ex></b><br>
```csharp
String factualId = "1d93c1ed-8cf3-4d58-94e0-05bbcd827cba";
Clear clear = new Clear();
clear.AddField("name");
clear.AddField("address");
clear.AddField("locality");
clear.AddField("region");
var response = Factual.Clear("us-sandbox", factualId, clear, new Metadata().User("test_driver_user"));
```

<b><ex>Overloaded: Clear the value of name, address, locality, and region in an existing entity:</ex></b><br>
```csharp
String factualId = "1d93c1ed-8cf3-4d58-94e0-05bbcd827cba";
Clear clear = new Clear("name", "address", "locality", "region");
var response = Factual.Clear("us-sandbox", factualId, clear, new Metadata().User("test_driver_user"));
```

#Flag
NOTICE: At the current time, this API call is ONLY compatible with places-v3. Please see the [the migration page](http://developer.factual.com/display/docs/Places+API+-+v3+Migration) for more details.
---

## Introduction

The Flag feature provides developers and editorial teams the ability to 'flag' problematic entities in tables for Factual editorial review. Use this feature to request an entity be deleted, flag an entity as a dupe or spam, note it does not exist, or just ask the Factual editors to check it out.

## Syntax

Calls to Flag require an indication of the problem type, the end user who is reporting the problem, and the Factual ID of the entity being flagged.

## All Top Level Flag Parameters

<table>
  <tr>
    <th>Parameter</th>
    <th>Description</th>
    <th>Example</th>
  </tr>
  <tr>
    <td>problem</td>
    <td>One of: duplicate, inaccurate, inappropriate, nonexistent, spam, or other.</td>
    <td><tt>factual.FlagDuplicate(table, factualId, metadata)</tt>
      <p><tt>factual.FlagInaccurate(table, factualId, metadata)</tt>      
      <p><tt>factual.FlagInappropriate(table, factualId, metadata)</tt>
      <p><tt>factual.FlagNonExistent(table, factualId, metadata)</tt>
      <p><tt>factual.FlagSpam(table, factualId, metadata)</tt>
      <p><tt>factual.FlagClosed(table, factualId, metadata)</tt>
      <p><tt>factual.FlagOther(table, factualId, metadata)</tt>
      </td>
  </tr>
  <tr>
    <td>user</td>
    <td>An arbitrary token representing the user flagging the data.</td>
    <td><tt>Metadata metadata = new Metadata().User("my_username")</tt></td>
  </tr>
  <tr>
    <td>comment</td>
    <td>Any english text comment that may help explain your corrections.</td>
    <td><tt>metadata.Comment("my comment")</tt></td>
  </tr>
  <tr>
    <td>reference</td>
    <td>A reference to a URL, title, person, etc. that is the source of this data.</td>
    <td><tt>metadata.Reference("http://www.example.com")</tt></td>
  </tr>
</table>


## Examples

<b><ex>Flag a row as spam:</ex></b><br>
```csharp
factual.FlagSpam("us-sandbox",
                 "e16ef265-b9be-437f-b7e2-ded852e3920e",
                 new Metadata().User("some_user_id"));
```

<b><ex>Flag a row as innacurate, and also include a comment and a reference:</ex></b><br>
```java
Metadata metadata = new Metadata()
  .Comment("Recently revised by IAAC")
  .Reference("http://www.example.com");
factual.FlagInaccurate("us-sandbox",
                 "e16ef265-b9be-437f-b7e2-ded852e3920e",
                 metadata);
```


# Exception Handling

If Factual's API indicates an error, a <tt>FactualApiException</tt> unchecked Exception will be thrown. It will contain details about the request you sent and the error that Factual returned.

Here is an example of catching a <tt>FactualApiException</tt> and inspecting it:

    Factual badness = new Factual("BAD_KEY", "BAD_SECRET");
    try
    {
      badness.Fetch("places", new Query().Field("country").Equal(true));
    } 
    catch (FactualApiException ex) 
    {
      Console.WriteLine("Requested URL: " + ex.Url);
      Console.WriteLine("Error Status Code: " + ex.StatusCode);
      Console.WriteLine("Error Response Message: " + ex.Response);
    }
    
# Thread Safety

This driver is thread safe.

# Driver Usage in VB.NET

This driver may be used in a VB.NET project as follows:</br>
	1. Add FactualDriver.dll using Package Manger Console (PM> Install-Package FactualDriver);</br>
	2. Add 'Imports FactualDriver' statement;</br>
	3. Create an instance of Factual object with oAuthKey and oAuthSecret.

## Example

	Imports FactualDriver 
	Module Module1 
		Sub Main() 
			Dim oFactualDriver As New Factual("oAuthKey", "oAuthSecret") 
			Dim result As New String(oFactualDriver.FetchRow("places", "03c26917-5d66-4de9-96bc-b13066173c65")) 
			Console.WriteLine(result) 
		End Sub 
	End Module 

# More Examples

For more code examples:

* [ASP.NET Web API integration example](https://github.com/Factual/factual-csharp-driver/wiki/ASP.NET-Web-API-with-Factual-Driver-Example)

* See the integration tests in <tt>FactualDriver.Tests/FactualIntegrationTests.cs</tt>

# Where to Get Help

If you think you've identified a specific bug in this driver, please file an issue in the github repo. Please be as specific as you can, including:

  * What you did to surface the bug
  * What you expected to happen
  * What actually happened
  * Detailed stack trace and/or line numbers

If you are having any other kind of issue, such as unexpected data or strange behaviour from Factual's API (or you're just not sure WHAT'S going on), please contact us through [GetSatisfaction](http://support.factual.com/factual).

# Changelog

09-30-13 Removed support for Monetize
