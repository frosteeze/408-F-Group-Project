(function()
{
 var Global=this,Runtime=this.IntelliFactory.Runtime,List,Html,Client,Attr,Tags,Client_Server,Client1,EventsPervasives,Concurrency,Remoting,AjaxRemotingProvider;
 Runtime.Define(Global,{
  Client_Server:{
   Client:{
    Main:function()
    {
     var x,input,x1,label,x2,label3,x3,label4,arg10,arg101,x4,arg00,arg102,x5,arg001;
     x=List.ofArray([Attr.Attr().NewAttr("value","")]);
     input=Tags.Tags().NewTag("input",x);
     x1=List.ofArray([Tags.Tags().text("")]);
     label=Tags.Tags().NewTag("div",x1);
     x2=List.ofArray([Tags.Tags().text("http://query.yahooapis.com/v1/public/yql?q=select%20*%20from%20yahoo.finance.quotes%20where%20symbol%20in%20(%22YHOO%22%2C%22AAPL%22%2C%22GOOG%22%2C%22MSFT%22)%0A%09%09&env=http%3A%2F%2Fdatatables.org%2Falltables.env&format=json")]);
     label3=Tags.Tags().NewTag("div",x2);
     x3=List.ofArray([Tags.Tags().text("")]);
     label4=Tags.Tags().NewTag("div",x3);
     arg101=List.ofArray([Tags.Tags().text("Click")]);
     x4=Tags.Tags().NewTag("button",arg101);
     arg00=function()
     {
      return function()
      {
       return Client1.Start(input.get_Value(),function(out)
       {
        return label.set_Text(out);
       });
      };
     };
     EventsPervasives.Events().OnClick(arg00,x4);
     arg102=List.ofArray([Tags.Tags().text("\"Parse Json\"")]);
     x5=Tags.Tags().NewTag("button",arg102);
     arg001=function()
     {
      return function()
      {
       return Client1.auto(input.get_Value(),function(out)
       {
        return label4.set_Text(out);
       });
      };
     };
     EventsPervasives.Events().OnClick(arg001,x5);
     arg10=List.ofArray([label3,input,label,x4,x5,label4]);
     return Tags.Tags().NewTag("div",arg10);
    },
    Start:function(input,k)
    {
     return Concurrency.Start(Concurrency.Delay(function()
     {
      return Concurrency.Bind(AjaxRemotingProvider.Async("Client_Server:1",[input]),function(_arg1)
      {
       return Concurrency.Return(k(_arg1));
      });
     }),{
      $:0
     });
    },
    auto:function(input,k)
    {
     return Concurrency.Start(Concurrency.Delay(function()
     {
      return Concurrency.Bind(AjaxRemotingProvider.Async("Client_Server:2",[input]),function(_arg1)
      {
       return Concurrency.Return(k(_arg1));
      });
     }),{
      $:0
     });
    }
   },
   Controls:{
    EntryPoint:Runtime.Class({
     get_Body:function()
     {
      return Client1.Main();
     }
    })
   }
  }
 });
 Runtime.OnInit(function()
 {
  List=Runtime.Safe(Global.WebSharper.List);
  Html=Runtime.Safe(Global.WebSharper.Html);
  Client=Runtime.Safe(Html.Client);
  Attr=Runtime.Safe(Client.Attr);
  Tags=Runtime.Safe(Client.Tags);
  Client_Server=Runtime.Safe(Global.Client_Server);
  Client1=Runtime.Safe(Client_Server.Client);
  EventsPervasives=Runtime.Safe(Client.EventsPervasives);
  Concurrency=Runtime.Safe(Global.WebSharper.Concurrency);
  Remoting=Runtime.Safe(Global.WebSharper.Remoting);
  return AjaxRemotingProvider=Runtime.Safe(Remoting.AjaxRemotingProvider);
 });
 Runtime.OnLoad(function()
 {
  return;
 });
}());
