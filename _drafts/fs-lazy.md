```fsharp
 
// p.82
 
let x = lazy (5 + 4)
 
printfn "%A" x
 
let y = x.Force()
 
let mutable workNumCount = 0
let workNum () =
   printfn "Starting Num %i" workNumCount
   workNumCount <- workNumCount + 1
 
let numVal = lazy (workNum())
let numVals =
   [
       numVal
       numVal
       numVal
       numVal
       numVal
       numVal
       numVal
       numVal
   ]
   |> List.map(fun numVal -> numVal.Force())
 
numVal.Force()
 
 
 
 
 
 
open System
open System.IO
open System.Net
 
let uri = @"http://dash/"
 
let mutable count = 1
 
let fetch (uri: string) =
   printfn "Request #%i @ %A" count DateTime.UtcNow
   count <- count + 1
 
   let response =    
       let mutable req = WebRequest.CreateHttp(uri);
       req.Method <- "GET"
       req.ContentLength <- 0L
       req.ContentType <- "application/json"
       req.UseDefaultCredentials <- true;
       req.UserAgent <- "GCS API";
       req.AuthenticationLevel <- System.Net.Security.AuthenticationLevel.MutualAuthRequested;
       req.Credentials <- System.Net.CredentialCache.DefaultNetworkCredentials;
       req.GetResponse()
   let reader = new StreamReader(response.GetResponseStream())
   reader.ReadToEnd()
 
let response = lazy (fetch uri)
 
printfn "%A" response
 
 
let calls =
   [
       response
       response
       response
       response
       response
   ]
   |> List.map(
       fun response ->
           response.Force()
   )
  
printfn "%A" calls
```