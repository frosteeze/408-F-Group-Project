

open FSharp.Data

module Quote = 
    let test = 1
    type stockQuote = JsonProvider<"Quote.json">
//    let doc = JsonValue.Load("http://query.yahooapis.com/v1/public/yql?q=select%20*%20from%20yahoo.finance.quotes%20where%20symbol%20in%20(%22YHOO%22%2C%22AAPL%22%2C%22GOOG%22%2C%22MSFT%22)%0A%09%09&env=http%3A%2F%2Fdatatables.org%2Falltables.env&format=json");
    let response = Http.Request("http://query.yahooapis.com/v1/public/yql?q=select%20*%20from%20yahoo.finance.quotes%20where%20symbol%20in%20(%22YHOO%22%2C%22AAPL%22%2C%22GOOG%22%2C%22MSFT%22)%0A%09%09&env=http%3A%2F%2Fdatatables.org%2Falltables.env&format=json");
//    let simple = stockQuote.Parse(response.Body.ToString())
    let info = JsonValue.Null;
//    match Quote.simple with
//    |  info ->
//        // Print overall information
//        let count = (int)info?query?count
//        printfn 
//          "Showing %d quotes" 
//          (count)
//        let quotes = info?query?results?quote
//        // Print every non-null data point
//        for record in quotes do
//            printfn " %s %s  %f " (record?Name)  (record?symbol)  (record?LastTradePriceOnly)