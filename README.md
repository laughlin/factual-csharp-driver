# About
This is the Factual supported C# driver for [Factual's public API](http://developer.factual.com).

# Install
The easiest way to install the Factual C# driver is by using [NuGet](https://www.nuget.org/). You can either use the Nuget command line utility or the Nuget Visual Studio plugin to install the Factual C# driver. If you are using the command line enter the following command: <tt>Install-Package FactualDriver</tt>. If you are using the Visual Studio plugin, right click on your project, select manage dependencies, search for <tt>FactualDriver</tt> and install it. 

# Get Started
Create an authenticated handle to Factual

```csharp
Factual factual = new Factual("MY_KEY", "MY_SECRET");
```

If you don't have a Factual API key yet, [it's free and easy to get one](https://www.factual.com/api-keys/request).

## Read
Use the read API to query data in Factual tables with any combination of full-text search, parametric filtering, and geo-location filtering.

Full documentation: http://developer.factual.com/api-docs/#Read

Related place-specific documentation:
* Categories: http://developer.factual.com/working-with-categories/
* Placerank, Sorting: http://developer.factual.com/search-placerank-and-boost/

##### Full-Text Search
```csharp
factual.Fetch("places", new Query().SearchExact("Raffi's Kebab"));
```

##### Row Filters
```csharp
// http://developer.factual.com/working-with-categories/
factual.Fetch("places",  new Query().Field("category_ids").Includes("347"));

// search restaurants or bars
factual.Fetch("places", new Query().Field("category_ids").IncludesAny("312", "347"));

// search for Starbucks in Los Angeles
factual.Fetch("places", new Query().SearchExact("Starbucks").Field("locality").Equal("Los Angeles"));

// search for starbucks in Los Angeles or Santa Monica
Query q = new Query().SearchExact("Starbucks");
q.Or(q.Field("locality").Equal("Los Angeles"), q.Field("locality").Equal("Santa Monica"));
factual.Fetch("places", q);
```

##### Pagging
```csharp
// search for starbucks in Los Angeles or Santa Monica (second page of results):
factual.Fetch("places", 
  new Query()
    .SearchExact("Starbucks")
    .Field("locality")
    .Equal("Los Angeles")
    .Offset(20)
    .Limit(20));
```

##### Geo Filters
```csharp
//  coffee near the Factual office
factual.Fetch("places", new Query().Search("coffee").WithIn(new Circle(34.05853, -118.416582, 1000)));
```

##### Existence Threshold
```csharp
// Existence threshold:
//  prefer precision over recall:
factual.Fetch("places", new Query().Threshold("confident"));
```

##### Query by Factual ID
```csharp
// Get a row by factual id:
factual.FetchRow("places", "03c26917-5d66-4de9-96bc-b13066173c65");
```

## Facets
Use the facets call to get summarized counts, grouped by specified fields.

Full documentation: http://developer.factual.com/api-docs/#Facets
```csharp
// Returns a count of Starbucks by country
factual.Fetch("global", new FacetQuery("country").Search("starbucks"));
```

## Resolve
Use resolve to generate a confidence-based match to an existing set of place attributes.

Full documentation: http://developer.factual.com/api-docs/#Resolve
```csharp
// resovle from name and geolocation
factual.Fetch("places", new ResolveQuery()
  .Add("name", "Buena Vista")
  .Add("latitude", 34.06)
  .Add("longitude", -118.40));
```

## Match
Match is similar to resolve, but returns only the Factual ID and is intended for high volume mapping.

Full documentation: http://developer.factual.com/api-docs/#Match
```csharp
factual.Match("places", new MatchQuery()
  .Add("name", "McDonalds")
  .Add("address", "10451 Santa Monica Blvd")
  .Add("region", "CA")
  .Add("postcode", "90025")
  .Add("country", "us"));
```

## Crosswalk
Crosswalk contains third party mappings between entities.

Full documentation: http://developer.factual.com/places-crosswalk/

```csharp
// Get all the crosswalk data for a specific Places entity using its Factual ID:
factual.Fetch("crosswalk", new Query().Field("factual_id").Equal("0e32a24f-5070-4997-a665-7213c0099833"));
```

## World Geographies
World Geographies contains administrative geographies (states, counties, countries), natural geographies (rivers, oceans, continents), and assorted geographic miscallaney.  This resource is intended to complement the Global Places and add utility to any geo-related content.

```csharp
// find California, USA

var query = new Query();
query.And(
 query.Field("name").Equal("philadelphia"),
 query.Field("country").Equal("us"),
 query.Field("placetype").Equal("locality"));
 
var response = factual.Fetch("world-geographies", query);
```

## Submit
Submit new data, or update existing data. Submit behaves as an "upsert", meaning that Factual will attempt to match the provided data against any existing places first. Note: you should ALWAYS store the *commit ID* returned from the response for any future support requests.

Full documentation: http://developer.factual.com/api-docs/#Submit

Place-specific Write API documentation: http://developer.factual.com/write-api/

##### Add data to Factual's us-sandbox table:
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

##### Add a neighborhood to a specific entity in Factual's us-sandbox table:
```csharp
Submit submit = new Submit()
  .AddValue("neighborhood", "Downtown");
  
factual.Submit("us-sandbox",
  "f33527e0-a8b4-4808-a820-2686f18cb00c",
  submit,
  new Metadata().user("some_user_id"));
```


## Flag
Use the flag API to flag problems in existing data.

Full documentation: http://developer.factual.com/api-docs/#Flag

Flag a place that is a duplicate of another.

```csharp
Metadata metadata = new Metadata().User("some_user_id");
String factualId = "9d676355-6c74-4cf6-8c4a-03fdaaa2d66a";
String preferredId = "e16ef265-b9be-437f-b7e2-ded852e3920e"
factual.FlagDuplicate("us-sandbox", factualId, preferredId, metadata);
```

Flag a place that is closed.
```charp
Metadata metadata = new Metadata().Comment("was shut down yesterday").User("some user id");
factual.FlagClosed("us-sandbox", "4e4a14fe-988c-4f03-a8e7-0efc806d0a7f", metadata);
```

## Clear
The clear API is used to signal that an existing attribute's value should be reset.

Full documentation: http://developer.factual.com/api-docs/#Clear
```csharp
String factualId = "1d93c1ed-8cf3-4d58-94e0-05bbcd827cba";
Clear clear = new Clear("name", "address", "locality", "region");
var response = Factual.Clear("us-sandbox", factualId, clear, new Metadata().User("test_driver_user"));
```

## Boost
The boost API is used to signal rows that should appear higher in search results.

Full documentation: http://developer.factual.com/api-docs/#Boost
```javascript
// create a full-text search
Query query = new Query().Search("Sushi Santa Monica");
// create a metadata object
Metadata metadata = new Metadata().User("test_driver_user");
// fetch query data
factual.Fetch("places-us", query);
// in the event that user "clicks" on one of the results fetched
factual.Boost("places-us", factualIdOfRowUserClickedOn, query, metadata);
```

## Multi
Make up to three simultaneous requests over a single HTTP connection. Note: while the requests are performed in parallel, the final response is not returned until all contained requests are complete. As such, you shouldn't use multi if you want non-blocking behavior. Also note that a contained response may include an API error message, if appropriate.

Full documentation: http://developer.factual.com/api-docs/#Multi

```csharp
// Fetch a multi response
factual.QueueFetch("places", new Query().Field("region").Equal("CA"));
factual.QueueFetch("places", new Query().Limit(1)); 
var multiResponse = factual.SendQueueRequests();
```


## Error Handling
If Factual's API indicates an error, a FactualApiException exception will be thrown. It will contain details about the request you sent and the error that Factual returned.

Here is an example of catching a FactualApiException and inspecting it:

```csharp
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
```

## Debug Mode
To see a full trace of debug information for a request and response, turn debug mode on. There are two ways to do so:

Use the Factual constructor to enable debug on a new instance:

```csharp
Factual factual = new Factual(key, secret, true);
```

or modify an existing instance to toggle debug mode on and off for individual requests:

```csharp
factual.debug(true);
factual.fetch(…);
factual.debug(false);
```

Debug information will be printed to standard out, with detailed request and response information, including headers.



## Custom timeouts
You can set the request timeout (in milliseconds):
```csharp
// Set the request timeouts to 2.5 seconds (values must be integers larger than 0 expressed in milliseconds):
factual.ConnectionTimeout = 2500;
factual.ReadTimeout = 2500;

// Set the request timeouts back to default 100000 and 300000 respectively
factual.ConnectionTimeout = null;
factual.ReadTimeout = null;
```
You will get [Error: socket hang up] for custom timeout errors.


# Where to Get Help
If you think you've identified a specific bug in this driver, please file an issue in the github repo. Please be as specific as you can, including:

  * What you did to surface the bug
  * What you expected to happen
  * What actually happened
  * Detailed stack trace and/or line numbers

If you are having any other kind of issue, such as unexpected data or strange behaviour from Factual's API (or you're just not sure what's going on), please contact us through the [Factual support site](http://support.factual.com/factual).
