(*
Demonstrates DTO validation for a complex domain
like a news feed
*)

#load "result.fsx"

/// A domain for showing news in a newsfeed
module rec NewsFeedDomain =

    // common simple types
    type DocumentId = DocumentId of int
    type ReporterId = ReporterId of string
    type TopicName = TopicName of string

    // --------------------------------
    // News items
    // --------------------------------

    /// A NewsItem is a document with
    /// some additional metadata
    type NewsItem = {
        DocumentId : DocumentId
        ReporterId : ReporterId
        Text: string  // unconstrainted
        Topics: Topic list
        }

    /// A topic is a name and category
    type Topic = {
        Name: TopicName
        Category : Category
        }

    /// A Category is Transient or Archival 
    type Category =
        // not worth archiving
        | Transient  
        // worth archiving
        | Archival of ArchiveInfo

    type ArchiveInfo = {
        Data : string
        }

    // --------------------------------
    // NewsFeed items
    // --------------------------------

    /// A NewsFeedItem is an item
    /// that has been given a priority and other
    /// attributes for a news feed
    type NewsFeedItem = {
        NewsItem: NewsItem
        CurationInfo : CurationInfo
        }

    type CurationInfo = {
        Position: CurationPosition
        IsBreaking : bool
        }

    /// As part of curating a news item,
    /// it can be given a position in the news feed
    /// Constrained to be > 0 
    type CurationPosition = CurationPosition of int

// ===================================
// The DTO types have a parallel structure
// to the Domain types, but use serializer-friendly types
// e.g. primitive types and arrays.
// ===================================

module NewsFeedDtos =
    open System

    type DtoError =
        | BadDocumentId
        | BadReporterId
        | BadTopicArea
        | BadCurationPosition
        | CategoryTagNotRecognized of string
        | UnexpectedNull
        | ArchiveInfoIsRequired

    type TopicNameDto = string

    type ArchiveInfoDto = {
        Data : string
        }

    type CategoryDto = {
        Tag: string  // "Transient" or "Archive"
        // If "Archive", the associated data 
        ArchiveInfo: obj // nullable
        }

    type TopicDto = {
        Name: TopicNameDto
        Category: CategoryDto
        }

    type DocumentIdDto = int
    type ReporterIdDto = string

    type NewsItemDto = {
        DocumentId : DocumentIdDto
        ReporterId : ReporterIdDto
        Text: string
        Topics: TopicDto [] //NOTE use of array rather than list
        }

    type CurationPositionDto = int
    type CurationInfoDto = {
        Position: CurationPositionDto
        IsBreaking : bool
        }


    type NewsFeedItemDto = {
        NewsItem : NewsItemDto
        CurationInfo : CurationInfoDto
        }

    // ===================================
    // Once the DTO types are defined, we
    // can then create "conversion" functions
    // that map from DTO to domain and back
    //
    // It's helpful to put them in a module
    // with the same name as the DTO type
    // ===================================

    module ArchiveInfoDto =
        let toDomain (dto:ArchiveInfoDto) :Validation<NewsFeedDomain.ArchiveInfo,DtoError> =
            if String.IsNullOrWhiteSpace(dto.Data) then
                Error [ArchiveInfoIsRequired]
            else
                Ok {NewsFeedDomain.ArchiveInfo.Data = dto.Data}

    module CategoryDto =
        let toDomain (dto:CategoryDto) :Validation<NewsFeedDomain.Category,DtoError> =
            match dto.Tag with
            | "Transient" ->
                // there is no extra data to parse,
                // so this is always OK
                Ok NewsFeedDomain.Category.Transient
            | "Archival" ->
                if dto.ArchiveInfo = null then
                    Error [UnexpectedNull]
                else
                    // cast from obj to DTO
                    // NOTE: this could throw so wrap in an exception handler if you need to
                    let archiveInfoDto = dto.ArchiveInfo :?> ArchiveInfoDto
                    archiveInfoDto
                    // dto -> ArchiveInfo (or error)
                    |> ArchiveInfoDto.toDomain  
                    // map to Category (or error)
                    |> Validation.map NewsFeedDomain.Category.Archival
            | _ ->
                Error [CategoryTagNotRecognized dto.Tag]


    module DocumentIdDto =
        let toDomain (dto:DocumentIdDto) :Validation<NewsFeedDomain.DocumentId,DtoError> =
            if dto < 1 then
                Error [BadDocumentId]
            else
                Ok (NewsFeedDomain.DocumentId dto)

    module ReporterIdDto =
        let toDomain (dto:ReporterIdDto) :Validation<NewsFeedDomain.ReporterId,DtoError> =
            if String.IsNullOrWhiteSpace(dto) then
                Error [BadReporterId]
            else
                Ok (NewsFeedDomain.ReporterId dto)

    module TopicNameDto =
        let toDomain (dto:TopicNameDto) :Validation<NewsFeedDomain.TopicName,DtoError> =
            if System.String.IsNullOrWhiteSpace(dto) then
                Error [BadTopicArea]
            else
                Ok (NewsFeedDomain.TopicName dto)

    module TopicDto =

        let toDomain (dto:TopicDto) :Validation<NewsFeedDomain.Topic,DtoError> =
            // define a constructor
            let ctor name pa :NewsFeedDomain.Topic = {
                Name = name
                Category = pa
                }

            // create the fields as "value or error"
            let nameOrError = 
                dto.Name
                |> TopicNameDto.toDomain 
            let categoryOrError = 
                dto.Category
                |> CategoryDto.toDomain 

            // combine them
            (Validation.lift2 ctor) nameOrError categoryOrError

    module NewsItemDto =

        let toDomain (dto:NewsItemDto) :Validation<NewsFeedDomain.NewsItem,DtoError> =
            // define a constructor
            let ctor docId reporterId text topics :NewsFeedDomain.NewsItem = {
                DocumentId = docId
                ReporterId = reporterId
                Text = text
                Topics = topics
                }

            // create the fields as "value or error"
            let docIdOrError = 
                dto.DocumentId
                |> DocumentIdDto.toDomain 
            let reporterIdOrError = 
                dto.ReporterId
                |> ReporterIdDto.toDomain 
            let textOrError = 
                Ok dto.Text
            let topicsOrError =
                dto.Topics
                // convert to F# list
                |> List.ofArray
                // convert each item
                |> List.map (TopicDto.toDomain)
                // flip from List<Result<>> to Result<List<>>
                |> Validation.sequence

            // combine them
            (Validation.lift4 ctor) docIdOrError reporterIdOrError textOrError topicsOrError

    module CurationPositionDto =
        let toDomain (dto:CurationPositionDto) :Validation<NewsFeedDomain.CurationPosition,DtoError> =
            if dto < 1 then
                Error [BadCurationPosition]
            else
                Ok (NewsFeedDomain.CurationPosition dto)

    module CurationInfoDto =
        let toDomain (dto:CurationInfoDto) :Validation<NewsFeedDomain.CurationInfo,DtoError> =
            // define a constructor
            let ctor p b :NewsFeedDomain.CurationInfo = {
                Position = p
                IsBreaking = b
                }
            let positionOrError = 
                dto.Position
                |> CurationPositionDto.toDomain 
            let isBreakingOrError = 
                dto.IsBreaking |> Ok
            // combine them
            (Validation.lift2 ctor) positionOrError isBreakingOrError


    module NewsFeedItemDto =
        let toDomain (dto:NewsFeedItemDto) :Validation<NewsFeedDomain.NewsFeedItem,DtoError> =
            // define a constructor
            let ctor a c :NewsFeedDomain.NewsFeedItem = {
                NewsItem = a
                CurationInfo = c
                }
            // create the fields as "value or error"
            let alertableItemOrError = 
                dto.NewsItem
                |> NewsItemDto.toDomain 
            let curationInfoOrError = 
                dto.CurationInfo
                |> CurationInfoDto.toDomain 
            // combine them
            (Validation.lift2 ctor) alertableItemOrError curationInfoOrError

// ==============================================
// Example of this domain and validation in action
// ==============================================


module NewsFeedExample =

    open NewsFeedDtos

    let goodNewsItem : NewsFeedDtos.NewsFeedItemDto = {
        NewsItem =
            {
            DocumentId = 1
            ReporterId = "Scott"
            Text = ""
            Topics =
                [|
                {Name="TopicA"; Category={Tag="Transient"; ArchiveInfo= null}}
                {Name="TopicB"; Category={Tag="Archival"; ArchiveInfo= box { ArchiveInfoDto.Data="some data"}}}
                |]
            }
        CurationInfo =
            {
            Position = 1
            IsBreaking = false
            }
        }

    let badNewsItem1 = 
        let badCurationInfo = 
            { goodNewsItem.CurationInfo with Position = -1 }
        { goodNewsItem with CurationInfo = badCurationInfo }

    let badNewsItem2 = 
        let badTopics = 
            [|
            {Name="TopicA"; Category={Tag="BadTag"; ArchiveInfo= null}}
            {Name="TopicB"; Category={Tag="Archival"; ArchiveInfo= null}}
            |]
        { goodNewsItem with NewsItem = {goodNewsItem.NewsItem with Topics = badTopics}}

    let badNewsItem3 = 
        let badTopics = 
            [|
            {Name="TopicB"; Category={Tag="Archival"; ArchiveInfo= null}}
            |]
        { goodNewsItem with NewsItem = {goodNewsItem.NewsItem with Topics = badTopics}}

/// TEST
NewsFeedExample.goodNewsItem 
|> NewsFeedDtos.NewsFeedItemDto.toDomain 

NewsFeedExample.badNewsItem1
|> NewsFeedDtos.NewsFeedItemDto.toDomain 

NewsFeedExample.badNewsItem2
|> NewsFeedDtos.NewsFeedItemDto.toDomain 

NewsFeedExample.badNewsItem3
|> NewsFeedDtos.NewsFeedItemDto.toDomain 
