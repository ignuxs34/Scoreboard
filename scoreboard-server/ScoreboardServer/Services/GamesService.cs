﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScoreboardServer.Models;
using ScoreboardServer.Repositories;

namespace ScoreboardServer.Services
{
    public class GamesService : IGamesService
    {
        private readonly IGamesRepository _repository;

        public GamesService(IGamesRepository repository)
        {
            _repository = repository;
        }

        public async Task<Game> GetGameById(int id, string userId)
        {
            var game = await _repository.GetById(id);
            if (game == null)
            {
                return null;
            }

            if (game.ApplicationUserId == userId || game.Public == true)
            {
                return game;
            }

            return null;
        }

        public async Task<ICollection<Game>> GetAllGames(int offset, int limit, string userId = null)
        {
            var games = await _repository.GetAll(offset, limit, userId);
            return games;
        }

        public async Task<int> Create(Game team)
        {
            var newId = await _repository.Create(team);
            return newId;
        }

        public async Task<bool> Update(int id, Game updatedGame, string userId)
        {
            var existringGame = await _repository.GetById(id);
            if (existringGame == null)
            {
                return false;
            }
            await _repository.Update(existringGame, updatedGame);
            return true;
        }

        public async Task<bool> Delete(int id, string userId)
        {
            var existringGame = await GetGameById(id, userId);
            if (existringGame == null)
            {
                return false;
            }
            await _repository.Delete(existringGame);
            return true;
        }

        public async Task<int> GetSize(string userId = null)
        {
            var size = await _repository.GetSize(userId);
            return size;
        }

        public async Task<ICollection<Game>> GetGamesByTeamId(int teamId)
        {
            var teamGames = await _repository.GetAllByTeamId(teamId);
            return teamGames;
        }
    }
}
