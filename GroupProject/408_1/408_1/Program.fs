open System
open System.ServiceModel
open Microsoft.FSharp.Linq

open FSharp.Data
open FSharp.Data.JsonExtensions

type stockQuote = JsonProvider<"Quote.json">

let doc = JsonValue.Load("http://query.yahooapis.com/v1/public/yql?q=select%20*%20from%20yahoo.finance.quotes%20where%20symbol%20in%20(%22YHOO%22%2C%22AAPL%22%2C%22GOOG%22%2C%22MSFT%22)%0A%09%09&env=http%3A%2F%2Fdatatables.org%2Falltables.env&format=json");

let docElement = doc.AsArray

let test = doc?query?count
let info = JsonValue.Null
Console.WriteLine(test)
match doc with
|  info ->
    // Print overall information
    let count = info?query?count
    printfn 
      "Showing %d quotes" 
      (count.AsInteger())
    let quotes = info?query?results?quote
    // Print every non-null data point
    for record in quotes do
        printfn " %s %s  %f " (record?Name.AsString())  (record?symbol.AsString())  (record?LastTradePriceOnly.AsFloat())

| _ -> printfn "failed"




System.Console.ReadLine();