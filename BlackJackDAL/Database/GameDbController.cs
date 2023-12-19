using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UtilityLib.Game;
using UtilityLib.Data_Management;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using System.Security.Cryptography.Xml;

namespace BlackJackDAL.Database
{
    public class GameDbController
    {
        GameDbContext db;
        public GameDbController() 
        {
            db = new GameDbContext();
        }

        public void AddPlayer(Player user)
        {
            if (!UserRegistered(user.Username))
            {
                db.Players.Add(user);
                db.SaveChanges();
                MessageBox.Show(user.Username + " added to database");
            }
        }
        public void UpdateDatabase(ListManager<Player> players) 
        {
            foreach (Player player in players.List)
            {
                Player playerToUpdate = db.Players.SingleOrDefault(p => p.Username == player.Username);
                PlayerStats statsToUpdate = db.Stats.SingleOrDefault(s => s.PlayerStatsId == playerToUpdate.PlayerId);
                
                if (statsToUpdate != null)
                {
                    statsToUpdate.Losses = player.Stats.Losses;
                    statsToUpdate.Wins = player.Stats.Wins;
                    db.SaveChanges();
                }
            }
        }

        public void UpdatePlayer(Player player)
        {
            Player playerToUpdate = db.Players.SingleOrDefault(p => p.Username == player.Username);

            if (playerToUpdate != null)
            {
                playerToUpdate.BettingData = player.BettingData;
                playerToUpdate.Coins = player.Coins;
                db.SaveChanges();
            }
        }

        public void DeleteAllContext() { }

        public bool UserRegistered(string user) 
        {
            Player content = db.Players.SingleOrDefault(u => u.Username == user);

            if (content != null)
            {
                // User was found in the database
                return true;
            }
            else
            {
                // User was not found in the database
                return false;
            }
        }

        public PlayerStats GetPlayerStats(Player player)
        {
            Player playerData = db.Players.SingleOrDefault(u => u.Username == player.Username);
            PlayerStats statsFromDb = db.Stats.SingleOrDefault(s => s.PlayerID == playerData.PlayerId);

            PlayerStats playerStats = null;

            if (statsFromDb != null)
            {
                playerStats = new PlayerStats
                {
                    Wins = statsFromDb.Wins,
                    Losses = statsFromDb.Losses
                };
            }
 
            return playerStats;
        }

        public bool DeletePlayer(string player)
        {
            Player playerData = db.Players.SingleOrDefault(u => u.Username == player);

            if (playerData != null)
            {
                db.Players.Remove(playerData);
                db.SaveChanges();
                return true;
            }
            else { return false; }

        }

        public double GetPlayerCoins(Player player)
        {
            Player playerData = db.Players.SingleOrDefault(u => u.Username == player.Username);
            if (playerData != null)
            {
                return playerData.Coins;
            }
            else
            {
                return 0;
            }
        }

        public Player GetPlayerData(Player player)
        {
            Player playerData = db.Players.SingleOrDefault(u => u.Username == player.Username);
            if (playerData != null)
            {
                return playerData;
            }
            else
            {
                return null;
            }
        }
    }
}
