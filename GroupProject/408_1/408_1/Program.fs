open System
open System.ServiceModel
open Microsoft.FSharp.Linq
open FSharp.Data
open FSharp.Data.JsonExtensions

//Loads the json file from the url
let doc = JsonValue.Load("http://query.yahooapis.com/v1/public/yql?q=select%20*%20from%20yahoo.finance.quotes%20where%20symbol%20in%20(%22YHOO%22%2C%22AAPL%22%2C%22GOOG%22%2C%22MSFT%22)%0A%09%09&env=http%3A%2F%2Fdatatables.org%2Falltables.env&format=json");
//Test to access element
//let test = doc?query?count
//Console.WriteLine(test)

let info = JsonValue.Null
(*Match allows us to draw out elements from 
  the doc element through the JsonValue object 
  structure. As we only need a single JsonValue 
  we simply declare a null JsonValue and use that
*)
match doc with
|  info ->
    // Access count
    (*The count field is in the query JSON object which 
    is within  the outermost JSON object represented by 
    info *) 
    let count = info?query?count
    printfn 
      "Showing %d quotes" 
      (count.AsInteger())
    let quotes = info?query?results?quote
    //We itterate for all records (in this case quotes) that is stored in the quotes field
    //NOTE: This will not work if we only query for a single quote, we should instead just
    //manually retrieve the values like how the value of quotes is set
    for record in quotes do
        printfn " %s %s  %f " (record?Name.AsString())  (record?symbol.AsString())  (record?LastTradePriceOnly.AsFloat())

| _ -> printfn "failed"


//To keep window open
System.Console.ReadLine();