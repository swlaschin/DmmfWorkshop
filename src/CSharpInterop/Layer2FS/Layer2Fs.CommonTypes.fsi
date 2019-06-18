module Layer2Fs.CommonTypes

type String50
type EmailAddress

val createString50 : s:string -> String50 option
val createEmailAddress : s:string -> EmailAddress option
