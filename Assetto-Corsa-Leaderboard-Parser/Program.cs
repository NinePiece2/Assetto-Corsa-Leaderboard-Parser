using Assetto_Corsa_Leaderboard_Parser.Data.Tables;
using HtmlAgilityPack;
using System;
using Assetto_Corsa_Leaderboard_Parser.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Assetto_Corsa_Leaderboard_Parser
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            while (true)
            {
                try
                {
                    Console.WriteLine("Leaderboard Parser Starting");
                    List<Leaderboard> list = ParseHtml("http://192.168.8.22:8000").GetAwaiter().GetResult();
                    StoreLeaderboardData(list).GetAwaiter().GetResult();
                    Console.WriteLine("Leaderboard Parser Stopped");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                await Task.Delay(5000);
            }
        }

        public static async Task<List<Leaderboard>> ParseHtml(string url)
        {
            var rows = new List<Leaderboard>();

            // Load the HTML document
            var web = new HtmlWeb();
            var document = web.Load(url);

            // Select the table rows
            var tableRows = document.DocumentNode.SelectNodes("//table[@class='table table-vcenter card-table']/tbody/tr");

            if (tableRows != null)
            {
                foreach (var row in tableRows)
                {
                    var cells = row.SelectNodes("td");

                    if (cells != null && cells.Count == 7)
                    {
                        // Parse the fields
                        var date = DateTime.Parse(cells[1].InnerText.Trim());
                        var name = cells[2].InnerText.Trim();
                        var car = cells[3].InnerText.Trim();
                        var durationString = cells[4].InnerText.Trim();
                        var scorePerMinute = int.Parse(cells[5].InnerText.Replace(",", "").Trim());
                        var score = int.Parse(cells[6].InnerText.Replace(",", "").Trim());
                        TimeSpan duration;

                        TimeSpan.TryParseExact(durationString, @"h\:mm\:ss\.f", null, out duration);

                        rows.Add(new Leaderboard
                        {
                            Date = date,
                            Name = name,
                            Car = car,
                            Duration = duration,
                            ScorePerMinute = scorePerMinute,
                            Score = score
                        });
                    }
                }
            }

            return rows;
        }

        public static async Task StoreLeaderboardData(List<Leaderboard> leaderboard)
        {
            using (AssettoServerDbContext dbContext = new AssettoServerDbContext())
            {
                foreach (var entry in leaderboard)
                {
                    // Check if the entry already exists in the database
                    bool exists = await dbContext.Leaderboard.AnyAsync(lb =>
                        lb.Date == entry.Date &&
                        lb.Name == entry.Name &&
                        lb.Car == entry.Car &&
                        lb.Duration == entry.Duration &&
                        lb.Score == entry.Score);

                    // Add the entry only if it doesn't already exist
                    if (!exists)
                    {
                        await dbContext.Leaderboard.AddAsync(entry);
                    }
                }

                // Save changes to the database
                await dbContext.SaveChangesAsync();
            }
        }

    }
}
