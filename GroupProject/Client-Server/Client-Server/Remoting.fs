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

    [<Remote>]
    let Process_quote_FData input  = 
        async {

            let req = WebRequest.Create(Uri(input)) 
            use resp = req.GetResponse() 
            use stream = resp.GetResponseStream() 
            use reader = new IO.StreamReader(stream) 
            let html = reader.ReadToEnd()
            let htmlArr = html.ToCharArray()
            let mutable result = ""
            let symbol =  Array.sub htmlArr 99 4
            let name = Array.sub htmlArr 1551 11
            let price = Array.sub htmlArr 1188 5

            let arSymbol = Array.ofSeq symbol 
            let strSymbol = System.String arSymbol
            let arName = Array.ofSeq name 
            let strName = System.String arName
            let arPrice = Array.ofSeq price 
            let strPrice = System.String arPrice
            return strSymbol + " "+ strName + " " + strPrice
           // let doc = JsonValue.Load(string(input));
            //let response = Http.Request(string(input));
            //Code breaks here as the type stockQuote is not set
            //let simple = .Parse(response.Body.ToString())
            //return " " + input + " " + simple.Query.Count.ToString()
            //let info = JsonValue.Null;
            //return doc.ToString()         // return simple.Query.Count.ToString()
            //return doc.Query.Results.Quote.Name.ToString()
//            match doc with
//            |  info ->
//                // Print overall informations
//                let quote = info?Query?Results?Quote
//                // Print every non-null data point
//                return  (quote.Name) + " " + (quote.Symbol) + " " + (quote.LastTradePriceOnly).ToString() 
            }     

   (* [<Remote>]
    let Process_quote input  = 
        async {
            let req = WebRequest.Create(Uri(input)) 
            use resp = req.GetResponse() 
            use stream = resp.GetResponseStream() 
            use reader = new IO.StreamReader(stream) 
            let html = reader.ReadToEnd()
            let htmlArr = html.ToCharArray()
//            let mutable inStr = false
//            let mutable inSymbol = false
//            let mutable inPrice = false
//            let mutable inName = false
            let mutable name = ""
            let mutable symbol = ""
            let mutable price = ""
            let mutable tempStr = string ""
//            for i = 0 to htmlArr.Length do
//
//                let cl = htmlArr.GetValue(i)
//                    //We are at the start ofs a string
//                if cl.ToString() = "\"" then 
//                if(inStr.Equals(true)) then
//                    inStr <- false
//                //We are still in a string
//                else if inStr then
//                    tempStr <- tempStr + cl.ToString()
//                //We are in a string but at the end
//                else if inStr &&  cl.ToString().Equals("\"") then
//                    //Note that all values are in quotes except when they are null
//                    //If our temp string is called symbol then our next string will be the symbol
//                    if tempStr.Equals("symbol") then
//                        inSymbol <- true
//                    //If we end and we are in the symbol, we have the symbol
//                    else if inSymbol then
//                        symbol <- tempStr 
//                        inSymbol <-false
//                    //If our temp string is called name then our next string will hold the name
//                    else if tempStr.Equals("Name") then
//                        inName <- true
//                    //If we end and are in name we have the name
//                    else if inName then
//                        name <- tempStr
//                        inName <- false
//                    //If our temp string is called lasttradeprice then our next string will hold the name
//                    else if tempStr.Equals("LastTradePriceOnly") then
//                        inPrice <- true
//                    //If we end and are in lasttradeprice we have the latest price of the stock
//                    else if inName then
//                        price <- tempStr
//                        inPrice <- false
//            //let result = name + " " + symbol + " " + price
            return ""
            //return html
        }*)
