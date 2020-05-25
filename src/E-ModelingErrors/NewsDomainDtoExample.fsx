(*
Demonstrates DTO validation for a complex domain
like a news feed
*)

#load "result.fsx"

module Domain =
    type DocumentId = DocumentId of int
    type EmailAddress = EmailAddress of string
    type TopicName = TopicName of string

    type CategoryBInfo = {
        Data : string
        }

    /// News Items have a category
    type Category =
        | A
        | B of CategoryBInfo

    /// News Items have a topic
    type Topic = {
        Name: TopicName
        Category : Category
        }

    /// A NewsItem is a document with
    /// some additional metadata
    type NewsItem = {
        DocumentId : DocumentId
        Text: string
        Topics: Topic list
        }

    /// As part of curating a news item,
    /// it can be given a position in the news feed
    type CurationPosition = CurationPosition of int

    type CurationInfo = {
        Position: CurationPosition
        IsBreaking : bool
        }

    /// A NewsFeedItem is an item
    /// that has been given a priority and other
    /// attributes for a news feed
    type NewsFeedItem = {
        NewsItem: NewsItem
        CurationInfo : CurationInfo
        }

// ===================================
// The DTO types have a parallel structure
// to the Domain types, but use serializer-friendly types
// e.g. primitive types and arrays.
// ===================================

module Dto =
    open System

    type DtoError =
        | BadDocumentId
        | BadTopicArea
        | BadCurationPosition
        | CategoryTagNotRecognized of string
        | UnexpectedNull
        | CategoryBDataIsRequired

    type TopicNameDto = string

    type CategoryBInfoDto = {
        Data : string
        }

    type CategoryDto = {
        Tag: string
        // DataForChoiceA: string // nullable
        DataForChoiceB: obj // nullable
        // DataForChoiceC:
        }

    type TopicDto = {
        Name: TopicNameDto
        Category: CategoryDto
        }

    type DocumentIdDto = int

    type NewsItemDto = {
        DocumentId : DocumentIdDto
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

    module CategoryBInfoDto =
        let toDomain (dto:CategoryBInfoDto) :Validation<Domain.CategoryBInfo,DtoError> =
            if String.IsNullOrWhiteSpace(dto.Data) then
                Error [CategoryBDataIsRequired]
            else
                Ok {Domain.CategoryBInfo.Data = dto.Data}

    module CategoryDto =
        let toDomain (dto:CategoryDto) :Validation<Domain.Category,DtoError> =
            match dto.Tag with
            | "A" ->
                Ok Domain.Category.A
            | "B" ->
                if dto.DataForChoiceB = null then
                    Error [UnexpectedNull]
                else
                    // cast from obj to DTO
                    // NOTE: this could throw so wrap in an exception handler if you need to
                    let dataForChoiceB = dto.DataForChoiceB :?> CategoryBInfoDto
                    // field to domain or error
                    let practiceAreaOrError = CategoryBInfoDto.toDomain dataForChoiceB
                    // construct DTO
                    let practiceArea = (Validation.map Domain.Category.B) practiceAreaOrError
                    practiceArea
            | _ ->
                Error [CategoryTagNotRecognized dto.Tag]


    module DocumentIdDto =
        let toDomain (dto:DocumentIdDto) :Validation<Domain.DocumentId,DtoError> =
            if dto < 1 then
                Error [BadDocumentId]
            else
                Ok (Domain.DocumentId dto)

    module TopicNameDto =
        let toDomain (dto:TopicNameDto) :Validation<Domain.TopicName,DtoError> =
            if System.String.IsNullOrWhiteSpace(dto) then
                Error [BadTopicArea]
            else
                Ok (Domain.TopicName dto)

    module TopicDto =

        let toDomain (dto:TopicDto) :Validation<Domain.Topic,DtoError> =
            // define a constructor
            let ctor name pa :Domain.Topic = {
                Name = name
                Category = pa
                }

            // create the fields as "value or error"
            let nameOrError = TopicNameDto.toDomain dto.Name
            let practiceAreaOrError = CategoryDto.toDomain dto.Category

            // combine them
            (Validation.lift2 ctor) nameOrError practiceAreaOrError

    module NewsItemDto =

        let toDomain (dto:NewsItemDto) :Validation<Domain.NewsItem,DtoError> =
            // define a constructor
            let ctor docId text topics :Domain.NewsItem = {
                DocumentId = docId
                Text = text
                Topics = topics
                }

            // create the fields as "value or error"
            let docIdOrError = DocumentIdDto.toDomain dto.DocumentId
            let textOrError = Ok dto.Text
            let topicsOrError =
                dto.Topics
                // convert to F# list
                |> List.ofArray
                // convert each item
                |> List.map (TopicDto.toDomain)
                // flip from List<Result<>> to Result<List<>>
                |> Result.sequence

            // combine them
            (Validation.lift3 ctor) docIdOrError textOrError topicsOrError

    module CurationPositionDto =
        let toDomain (dto:CurationPositionDto) :Validation<Domain.CurationPosition,DtoError> =
            if dto < 1 then
                Error [BadCurationPosition]
            else
                Ok (Domain.CurationPosition dto)

    module CurationInfoDto =
        let toDomain (dto:CurationInfoDto) :Validation<Domain.CurationInfo,DtoError> =
            // define a constructor
            let ctor p b :Domain.CurationInfo = {
                Position = p
                IsBreaking = b
                }
            let positionOrError = CurationPositionDto.toDomain dto.Position
            let isBreakingOrError = dto.IsBreaking |> Ok
            // combine them
            (Validation.lift2 ctor) positionOrError isBreakingOrError


    module NewsFeedItemDto =
        let toDomain (dto:NewsFeedItemDto) :Validation<Domain.NewsFeedItem,DtoError> =
            // define a constructor
            let ctor a c :Domain.NewsFeedItem = {
                NewsItem = a
                CurationInfo = c
                }
            // create the fields as "value or error"
            let alertableItemOrError = NewsItemDto.toDomain dto.NewsItem
            let curationInfoOrError = CurationInfoDto.toDomain dto.CurationInfo
            // combine them
            (Validation.lift2 ctor) alertableItemOrError curationInfoOrError

module Example =
    open Dto

    let goodNewsItem:Dto.NewsFeedItemDto = {
        NewsItem =
            {
            DocumentId = 1
            Text = ""
            Topics =
                [|
                {Name="TopicA"; Category={Tag="A"; DataForChoiceB= null}}
                {Name="TopicB"; Category={Tag="B"; DataForChoiceB= box { CategoryBInfoDto.Data="test"}}}
                |]
            }
        CurationInfo =
            {
            Position = 1
            IsBreaking = false
            }
        }

(*
Dto.NewsFeedItemDto.toDomain Example.goodNewsItem
*)