factual-csharp-driver
======================

Officially supported .NET driver for [Factual's public API](http://developer.factual.com).

# Installation

## Nuget

In the package manager console window: <tt>Install-Package FactualDriver</tt>, or right click on your package and click
manage packages, search for factual and install.

## Non Nuget

Either clone the github [repo](https://github.com/Factual/factual-csharp-driver), or [download as a zip or tar] (https://github.com/Factual/factual-csharp-driver/downloads)

# Basic Design

The driver allows you to create an authenticated handle to Factual. With a Factual handle, you can send queries and get results back.

Queries are created using the Query class, which provides a fluent interface to constructing your queries.

Results are returned as the JSON returned by Factual. 

# Setup
First obtain free developer keys from factual.com

    // Create an authenticated handle to Factual
    Factual factual = new Factual(MY_KEY, MY_SECRET);
    
# Optional tests setup

If you are going to clone or download the repository you will have access to integration tests which can also
help during development. To setup your tests: add your factual key and factual secret key in the FactualDriver.Tests\app.config.	
	
# Simple Query Example

    // 3 random records from Factual's Places table:
    factual.Fetch("places", new Query().limit(3))
	
# Full Text Search

    // Entities that match a full text search for Sushi in Santa Monica:
    factual.Fetch("places", new Query().Search("Sushi Santa Monica"));

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

(See [the Crosswalk Blog](http://blog.factual.com/crosswalk-api) for more background.)

## Simple Crosswalk Example

    // Get all Crosswalk data for a specific Places entity, using its Factual ID:
    factual.Fetch("places",
      new CrosswalkQuery().FactualId("97598010-433f-4946-8fd5-4a6dd1639d77"));

## Crosswalk Filter Parameters

<table>
  <tr>
    <th>Filter</th>
    <th>Description</th>
    <th>Example</th>
  </tr>
  <tr>
    <td>factualId</td>
    <td>A Factual ID for an entity in the Factual places database</td>
    <td><tt>q.FactualId("97598010-433f-4946-8fd5-4a6dd1639d77")</tt></td>
  </tr>
  <tr>
    <td>limit</td>
    <td>A Factual ID for an entity in the Factual places database</td>
    <td><tt>q.Limit(100)</tt></td>
  </tr>
  <tr>
    <td>namespace</td>
    <td>The namespace to search for a third party ID within. A list of <b>currently supported</b> services is <a href="http://developer.factual.com/display/docs/Places+API+-+Supported+Crosswalk+Services">here</a>.</td>
    <td><tt>q.Namespace("foursquare")</tt></td>
  </tr>
  <tr>
    <td>namespaceId</td>
    <td>The id used by a third party to identify a place.</td>
    <td><tt>q.NamespaceId("443338")</tt></td>
  </tr>
  <tr>
    <td>only</td>
    <td>A Factual ID for an entity in the Factual places database</td>
    <td><tt>q.Only("foursquare", "yelp")</tt></td>
  </tr>
</table>

NOTE: although these parameters are individually optional, at least one of the following parameter combinations is required:

* factualId
* namespace and namespaceId

## More Crosswalk Examples

    // Get Loopt's Crosswalk data for a specific Places entity, using its Factual ID:
    var jsonResponse = factual.Fetch("places",
        new CrosswalkQuery()
          .FactualId("97598010-433f-4946-8fd5-4a6dd1639d77")
          .Only("loopt"));

    // Get all Crosswalk data for a specific Places entity, using its Foursquare ID
    var jsonResponse = factual.Fetch("places",
        new CrosswalkQuery()
          .Namespace("foursquare")
          .NamespaceId("4ae4df6df964a520019f21e3"));
          

# Raw Read

Factual may occasionally release a new API which is not immediately supported by the Java driver.  To test queries against these APIs, we recommend using the raw read feature.  The recommendation is to only construct a raw read query if the feature is not yet supported using other convenience methods.

<p>You can perform any GET request using the <tt>factual.RawQuery(table,parameters)</tt> method. Add parameters to your request by building a json string, and the request will be made using your OAuth token.  The driver will URL-encode the parameter values.

## Example Raw Read Queries

Fetch only names that begins with "Star" and include count: 
    
    var rawParameters = "filters={\"name\":{\"$bw\":\"Star\"}}&include_count=true";
    string result = Factual.RawQuery("t/global", rawParameters);
    dynamic json = JsonConvert.DeserializeObject(result);
    
    
Note that the above examples demonstrate the ability to construct read queries using the raw read feature.  However, in practice, the recommendation is to always use the convenience classes for features which are supported.  In the above cases, a Query object should be used instead.


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

# Geopulse

The driver fully supports Factual's <a href="http://developer.factual.com/display/docs/Places+API+-+Geopulse">Geopulse</a> feature, which provides point-based access to geographic attributes: you provide a long/lat coordinate pair, we provide everything we can know about that geography. 

## Simple Geopulse Example

The <tt>geopulse</tt> method fetches results based on the given point:

	var respponse = factual.Geopulse(new Geopulse(new Point(latitude, longitude))
												.Only("commercial_density", "commercial_profile"));


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
    
    
# More Examples

For more code examples:

* See the integration tests in <tt>FactualDriver.Tests/FactualIntegrationTests.cs</tt>

# Where to Get Help

If you think you've identified a specific bug in this driver, please file an issue in the github repo. Please be as specific as you can, including:

  * What you did to surface the bug
  * What you expected to happen
  * What actually happened
  * Detailed stack trace and/or line numbers

If you are having any other kind of issue, such as unexpected data or strange behaviour from Factual's API (or you're just not sure WHAT'S going on), please contact us through [GetSatisfaction](http://support.factual.com/factual).
