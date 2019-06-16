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
// Then define the workflows last
// =============================

type MyWorkflow = InputData -> OutputEvent
