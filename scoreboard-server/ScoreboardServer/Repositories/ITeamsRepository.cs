﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScoreboardServer.Models;

namespace ScoreboardServer.Repositories
{
    public interface ITeamsRepository
    {
        Task<Team> GetById(int id);
        Task<ICollection<Team>> GetAll(int offset, int limit, string userId);
        Task<int> GetSize(string userId);
        Task<int> Create(Team newTeam);
        Task Update(Team existingTeam, Team updatedTeam);
        Task Delete(Team deletedTeam);
    }
}
