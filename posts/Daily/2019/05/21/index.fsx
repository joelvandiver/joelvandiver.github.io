(**
# Microservices - Agents
05-21-2019

In planning to move to microservices, I see the potential for using F# MailboxProcessors for communication between services.  This post serves as an exploration.

## MailboxProcessor
*)

let inbox = 
    new MailboxProcessor<_>(fun incoming -> 
        let rec wait value = 
            async { printfn "current = %d, waiting for the next value..." value
                    let! msg = incoming.Receive()
                    return! wait (value + msg)}
        wait 0)

(**
> Output:
```fsharp
val inbox : MailboxProcessor<int>
```
*)

inbox.Start()
inbox.Post(1)
inbox.Post(4)
inbox.Post(2)
inbox.Post(1)

(**
> Output:
```fsharp
current = 0, waiting for the next value...
current = 1, waiting for the next value...
current = 5, waiting for the next value...
current = 7, waiting for the next value...
current = 8, waiting for the next value...
```
*)




(**
*For futher reading:*

1. [Messages and Agents - fsharpforfunandprofit.com](https://fsharpforfunandprofit.com/posts/concurrency-actor-model/)
2. [Control.MailboxProcessor - MSDN](https://msdn.microsoft.com/en-us/visualfsharpdocs/conceptual/control.mailboxprocessor%5B%27msg%5D-class-%5Bfsharp%5D?f=255&MSPPError=-2147217396)

*)


