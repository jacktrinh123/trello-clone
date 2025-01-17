﻿using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using todoist_red_gate.Controllers;
using todoist_red_gate.Models;
using todoist_red_gate.Services.TrelloServices;

namespace todoist_red_gate.Services
{
    public class ListService : IListService
    {
        private const string BaseUrl = "https://api.trello.com/1";
        private readonly HttpClient _client;
        private readonly string AppKey;
        private readonly IConfiguration _config;
        public ListService(HttpClient client, IConfiguration config)
        {
            _client = client;
            _config = config;
            AppKey = _config.GetValue<string>("Trello:ConsumerKey");
        }


        public async Task<List> CreateListAsync(string listName, string idBoard, string Token)
        {
            string url = BaseUrl + "/lists?key=" + AppKey + "&token=" + Token + "&name=" + listName + "&idBoard=" + idBoard;
            var httlResponse = await _client.PostAsync(url, null);
            if (!httlResponse.IsSuccessStatusCode)
            {
                throw new Exception("Can not add todo task");
            }
            var createdTask = JsonConvert.DeserializeObject<Models.List>(await httlResponse.Content.ReadAsStringAsync());
            return createdTask;
        }

        public async Task ArchiveAsync(string id, string Token)
        {
            string url = BaseUrl + "/lists/" + id+"/closed?key=" + AppKey + "&token=" + Token + "&value=true";
            var httpResponse = await _client.PutAsync(url, null);
            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("Cannot archive");
            }
        }

        public async Task<List<Card>> GetCardsInAList(string idList, string Token)
        {
            string url = BaseUrl + "/lists/" + idList + "/cards?key=" + AppKey + "&token=" + Token;
            var httpResponse = await _client.GetAsync(url);
            if(!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("Cannot retrieve task");
            }
            var content = await httpResponse.Content.ReadAsStringAsync();
            var res = JsonConvert.DeserializeObject<List<Models.Card>>(content);
            return res;
        }

        public async Task<List> GetListAsync(string idList, string Token)
        {
            string url = BaseUrl + "/lists/" + idList + "?key=" + AppKey + "&token=" + Token;
            var httpResponse = await _client.GetAsync(url);

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("Cannot retrieve tasks");
            }

            var content = await httpResponse.Content.ReadAsStringAsync();
            var listItem = JsonConvert.DeserializeObject<List>(content);
            return listItem;
        }


        public async Task<List> UpdateListAsync(List task, string idList, string Token)
        {
            var url = BaseUrl + "/lists/" + idList + "?key=" + AppKey + "&token=" + Token;
            var content = JsonConvert.SerializeObject(task);
            var stringContent = new StringContent(content, UnicodeEncoding.UTF8, "application/json");
            //_client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer");
            var httpResponse = await _client.PutAsync(url, stringContent);

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("Cannot update list");
            }

            var updatedTask = JsonConvert.DeserializeObject<Models.List>(await httpResponse.Content.ReadAsStringAsync());
            return updatedTask;
        }

        public async Task<Board> GetBoardAListIsOn(string listId, string Token)
        {
            string url = BaseUrl + "/lists/" + listId + "/board?key=" + AppKey + "&token=" + Token;
            var httpRespone = await _client.GetAsync(url);
            if(!httpRespone.IsSuccessStatusCode)
            {
                throw new Exception("Cannot retrieve task");
            }
            var content = await httpRespone.Content.ReadAsStringAsync();
            var board = JsonConvert.DeserializeObject<Board>(content);
            return board;
        }
    }
}
