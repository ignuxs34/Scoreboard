﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScoreboardServer.Models;

namespace ScoreboardServer.Repositories
{
    public interface IGamesRepository
    {
        Task<Game> GetById(int id);
        Task<ICollection<Game>> GetAll(int offset, int limit, string userId = null);
        Task<int> Create(Game newGame);
        Task Update(Game existingGame, Game updatedGame);
        Task Delete(Game deletedGame);
        Task<int> GetSize(string userId = null);
        Task<ICollection<Game>> GetAllByTeamId(int teamId);
    }
}
