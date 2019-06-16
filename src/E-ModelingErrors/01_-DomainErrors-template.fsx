// =============================
// Examples of how to structure the code
// =============================

// =============================
// Define simple types first
// =============================

type CustomerId =  CustomerId of string


// =============================
// Then define the input and output data
// =============================

type InputData = {
  // record example
  Id: CustomerId
}

type OutputEvent =
  // choice example
  | NothingHappened
  | Updated of CustomerId

// =============================
// Then define the errors
// =============================

type MyError =
  | BadThing
  | AnotherBadThing

// =============================
// Then define the workflows last, using Result as the output
// =============================

type MyWorkflow = InputData -> Result<OutputEvent,MyError>
