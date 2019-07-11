(function()
{
 "use strict";
 var Global,ClientServerTable,Client,IntelliFactory,Runtime,WebSharper,UI,Doc,AttrProxy,Var$1,Submitter,View,Remoting,AjaxRemotingProvider,Concurrency;
 Global=self;
 ClientServerTable=Global.ClientServerTable=Global.ClientServerTable||{};
 Client=ClientServerTable.Client=ClientServerTable.Client||{};
 IntelliFactory=Global.IntelliFactory;
 Runtime=IntelliFactory&&IntelliFactory.Runtime;
 WebSharper=Global.WebSharper;
 UI=WebSharper&&WebSharper.UI;
 Doc=UI&&UI.Doc;
 AttrProxy=UI&&UI.AttrProxy;
 Var$1=UI&&UI.Var$1;
 Submitter=UI&&UI.Submitter;
 View=UI&&UI.View;
 Remoting=WebSharper&&WebSharper.Remoting;
 AjaxRemotingProvider=Remoting&&Remoting.AjaxRemotingProvider;
 Concurrency=WebSharper&&WebSharper.Concurrency;
 Client.Inspector$31$22=Runtime.Curried3(function($1,$2,$3)
 {
  Global.$("#demo-table").data("table").toggleInspector();
 });
 Client.Inspector=function()
 {
  return Doc.Element("a",[AttrProxy.Create("class","button ml-1"),AttrProxy.HandlerImpl("click",function()
  {
   return function()
   {
    Global.$("#demo-table").data("table").toggleInspector();
   };
  })],[Doc.Element("span",[AttrProxy.Create("class","mif-cog")],[])]);
 };
 Client.Main=function()
 {
  var rvInput,submit,vReversed;
  rvInput=Var$1.Create$1("");
  submit=Submitter.CreateOption(rvInput.get_View());
  vReversed=View.MapAsync(function(a)
  {
   var b;
   return a!=null&&a.$==1?(new AjaxRemotingProvider.New()).Async("ClientServerTable:ClientServerTable.Server.DoSomething:-1840423385",[a.$0]):(b=null,Concurrency.Delay(function()
   {
    return Concurrency.Return("");
   }));
  },submit.view);
  return Doc.Element("div",[],[Doc.Input([],rvInput),Doc.Button("Send",[],function()
  {
   submit.Trigger();
  }),Doc.Element("hr",[],[]),Doc.Element("h4",[AttrProxy.Create("class","text-muted")],[Doc.TextNode("The server responded:")]),Doc.Element("div",[AttrProxy.Create("class","jumbotron")],[Doc.Element("h1",[],[Doc.TextView(vReversed)])])]);
 };
}());
