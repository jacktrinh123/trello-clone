﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todoist_red_gate.Models;

namespace todoist_red_gate.Services
{
    public interface ICardService
    {
        Task<Card> GetCardAsync(string id);
        Task<Card> CreateCardAsync(Card task, string listId);
        Task<Card> UpdateCardAsync(Card task, string idCard);
        Task DeleteCardAsync(string id);
        Task<Models.Action> GetActionOnCard(string cardId);
        Task<Models.Board> GetBoardCardIsOn(string cardId);
        Task<List<Models.CheckItem>> GetCheckItemsOnTheCard(string cardId);
        Task<List<Models.Checklist>> GetCheckListsOnTheCard(string cardId);
        Task<List<Member>> GetMembersOfCards(string cardId);

        //Attachments
        Task<List<Attachment>> GetAttachmentsOnACard(string cardId);
        Task<Attachment> GetAnAttachment(string cardId, string attachmentId);
        Task<Attachment> CreateAttachment(string cardId, AttachmentCreateRequest request);
        Task DeleteAttachment(string cardId, string attachmentId);

    }
}
