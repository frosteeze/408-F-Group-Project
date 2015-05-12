namespace Client_Server

open WebSharper
open System.IO
open System
open System.Net
open FSharp.Data

module Remoting =

    [<Remote>]
    let Process input =
        async {
            return "You said: " + input
        }

    [<Remote>]
    let Process_quote input  = 
        async {
            let req = WebRequest.Create(Uri(input)) 
            use resp = req.GetResponse() 
            use stream = resp.GetResponseStream() 
            use reader = new IO.StreamReader(stream) 
            let html = reader.ReadToEnd()
            return html
        }

    type stockQuote = JsonProvider<"Quote.json">
    [<Remote>]
    let Process_quote_FData input  = 
        async {
            //let doc = stockQuote.Load(string(input));
            let response = Http.Request(string(input));
            let simple = stockQuote.Parse(response.Body.ToString())
            return " " + input + " " + simple.Query.Count.ToString()
           // let info = JsonValue.Null;
           // return simple.Query.Count.ToString()
            //return doc.Query.Results.Quote.Name.ToString()
//            match doc with
//            |  info ->
//                // Print overall informations
//                let quote = info.Query.Results.Quote
//                // Print every non-null data point
//                return  (quote.Name) + " " + (quote.Symbol) + " " + (quote.LastTradePriceOnly).ToString() 
        }     

