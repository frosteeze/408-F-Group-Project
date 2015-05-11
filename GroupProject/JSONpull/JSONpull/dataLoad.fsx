#r "../packages/FSharp.Data.2.2.1/lib/net40/FSharp.Data.dll"

open FSharp.Data

module Quote = 
    type stockQuote = JsonProvider<"Quote.json">
    let doc = JsonValue.Load("http://query.yahooapis.com/v1/public/yql?q=select%20*%20from%20yahoo.finance.quotes%20where%20symbol%20in%20(%22YHOO%22)%0A%09%09&env=http%3A%2F%2Fdatatables.org%2Falltables.env&format=json");
    let simple = stockQuote.Parse(doc.ToString())
    let info = JsonValue.Null;
    match simple with
    |  info ->
        // Print overall information
        let count = (int)info.Query.Count
        printfn 
          "Showing %d quotes" 
          (count)
        let quoteInfo = info.Query.Results.Quote
        // Print every non-null data point
        printfn " %s %s  %f " (quoteInfo.Name)  (quoteInfo.Symbol)  (quoteInfo.LastTradePriceOnly)
