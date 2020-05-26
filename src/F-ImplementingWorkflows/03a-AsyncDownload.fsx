(*
Example of async code in action
*)


open System.Net
open System

let fetchUrl url =
    let req = WebRequest.Create(Uri(url))
    use resp = req.GetResponse()
    use stream = resp.GetResponseStream()
    use reader = new IO.StreamReader(stream)
    let html = reader.ReadToEnd()
    printfn "finished downloading %s" url

// a list of sites to fetch
let sites = ["http://www.bing.com";
             "http://www.google.com";
             "http://www.microsoft.com";
             "http://www.amazon.com";
             "http://www.yahoo.com"]

#time                     // turn interactive timer on
sites                     // start with the list of sites
|> List.map fetchUrl      // loop through each site and download
#time                     // turn timer off


(*
Make a note of the time taken, and let's if we can improve on it!

Obviously the example above is inefficient – only one web site at a time is visited. The program would be faster if we could visit them all at the same time.

So how would we convert this to a concurrent algorithm? The logic would be something like:

* Create a task for each web page we are downloading, and then for each task,
  the download logic would be something like:
  * Start downloading a page from a website. While that is going on, pause and let other tasks have a turn.
  * When the download is finished, wake up and continue on with the task

Finally, start all the tasks up and let them go at it!

*)



// Fetch the contents of a web page asynchronously
let fetchUrlAsync url =
    async {
        let req = WebRequest.Create(Uri(url))
        use! resp = req.AsyncGetResponse()  // new keyword "use!"
        use stream = resp.GetResponseStream()
        use reader = new IO.StreamReader(stream)
        let html = reader.ReadToEnd()
        printfn "finished downloading %s" url
        }

(*
Note that the new code looks almost exactly the same as the original. There are only a few minor changes.

The change from "use resp =" to "use! resp =" is exactly the change that
we talked about above – while the async operation is going on, let other tasks have a turn.

We also used the method AsyncGetResponse.
This returns an async workflow that we can nest inside the main workflow.

In addition the whole set of steps is contained in the "async {...}" wrapper
which turns it into a block that can be run asynchronously.


And here is a timed download using the async version.

*)

#time                      // turn interactive timer on
sites
|> List.map fetchUrlAsync  // make a list of async tasks
|> Async.Parallel          // set up the tasks to run in parallel
|> Async.RunSynchronously  // start them off
#time                      // turn timer off

(*
The way this works is:

fetchUrlAsync is applied to each site using "map".
It does *not* immediately start the download, but returns an async workflow for running later.

To set up all the tasks to run at the same time we use the Async.Parallel function

Finally we call Async.RunSynchronously to start all the tasks, and wait for them all to stop.

*)