type Input = {
   Name : string
   Email : string
}

let checkNameNotBlank input =
  if input.Name = "" then
     Error "Name must not be blank"
  else Ok input

let checkName50 input =
  if input.Name.Length > 50 then
     Error "Name must not be longer than 50 chars"
  else Ok input

let checkEmailNotBlank input =
  if input.Email = "" then
     Error "Email must not be blank"
  else Ok input

let validateInput input =
    input
    |> ??
    |> ??
    |> ??

// add some more validations if you like

// -------------------------------
// test data
// -------------------------------


let goodInput = {Name="Scott";Email="x@example.com"}
validateInput goodInput

let blankName = {Name="";Email="x@example.com"}
validateInput blankName

let blankEmail = {Name="Scott";Email=""}
validateInput blankEmail