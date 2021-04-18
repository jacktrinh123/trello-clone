﻿using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using todoist_red_gate.Models;

namespace todoist_red_gate.Services.TrelloServices.TrelloServices
{
    public class CardService : ICardService
    {
        private const string BaseUrl = "https://api.trello.com/1/";
        private readonly HttpClient _client;
        private IConfiguration _iConfig;
        private string AppKey;
        private string Token;
        public CardService(HttpClient client)
        {
            _client = client;
            AppKey = "07e57a8c0ff7205b8202479a1d9ed50d";
            Token = "16a827c827226d35375b00936d65bea64d6c964f8e2e638f87fb9b27143eae7d";
        }
        public async Task<Card> CreateCardAsync(Card task, string idList)
        {

            var content = JsonConvert.SerializeObject(task);
            string url = BaseUrl + "cards?key=" + AppKey + "&token=" + Token + "&idList=" + idList;
            var httlResponse = await _client.PostAsync(url, new StringContent(content));
            if (!httlResponse.IsSuccessStatusCode)
            {
                throw new Exception("Can not add card");
            }
            var createdTask = JsonConvert.DeserializeObject<Card>(await httlResponse.Content.ReadAsStringAsync());
            return createdTask;
        }
        public async Task DeleteCardAsync(string id)
        {
            string url = BaseUrl + "cards/";
            var httpResponse = await _client.DeleteAsync($"{url}{id}");
            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("Cannot delete the todo item");
            }
        }
        public async Task<List<Card>> GetAllCardsOfListAsync(string listId)
        {
            string url = BaseUrl + "lists/" + listId + "/cards";
            var httpResponse = await _client.GetAsync(url);

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("Cannot retrieve tasks");
            }

            var content = await httpResponse.Content.ReadAsStringAsync();
            var tasks = JsonConvert.DeserializeObject<List<Card>>(content);

            return tasks;

        }
        public async Task<Card> GetCardAsync(string id)
        {
            string url = BaseUrl + "cards/" + id + "?key=" + AppKey + "&token=" + Token;
            var httpResponse = await _client.GetAsync(url);

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("Cannot retrieve tasks");
            }

            var content = await httpResponse.Content.ReadAsStringAsync();
            var cardItem = JsonConvert.DeserializeObject<Card>(content);

            return cardItem;
        }
        public async Task<Card> UpdateCardAsync(Card task, string idCard)
        {
            var url = BaseUrl + "cards/" + idCard + "?key=" + AppKey + "&token=" + Token;
            var content = JsonConvert.SerializeObject(task);
            var httpResponse = await _client.PutAsync(url, new StringContent(content));

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("Cannot update card");
            }

            var updatedTask = JsonConvert.DeserializeObject<Card>(await httpResponse.Content.ReadAsStringAsync());
            return updatedTask;
        }
    }
}
