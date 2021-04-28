﻿using CorruptOSBot.Shared;
using CorruptOSBot.TheHunt;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorruptOSBot.Modules
{
    public class HuntModule : ModuleBase<SocketCommandContext>
    {
        [Command("huntscore")]
        [Summary("!huntscore tional) - Retrieves your teams current score")]
        public async Task SayHuntScoreAsync()
        {
            if (ToggleStateManager.GetToggleState("huntscore", Context.User) && PermissionManager.HasSpecificRole(Context.User, "Staff"))
            {
                var team = HuntManager.GetTeam(Context.User);
                if (team != null)
                {
                    await ReplyAsync(team.CalculatePointsForTeam().ToString());
                }
                else
                {
                    await ReplyAsync("Not part of a team");
                }


                // do stuff
                await ReplyAsync("Should contain the score");
            }
        }


        [Command("addhuntitem")]
        [Summary("!addhuntitem {teamname}-{bossname}-{itemname} - adds the item to the team as a drop manually")]
        public async Task SayAddHuntItemAsync([Remainder]string commandstring)
        {
            if (ToggleStateManager.GetToggleState("addhuntitem", Context.User) && PermissionManager.HasSpecificRole(Context.User, "Staff"))
            {
                var stringSplitted = commandstring.Split('/');
                if (stringSplitted.Length == 3)
                {
                    var teamName = stringSplitted[0];
                    var bossName = stringSplitted[1];
                    var itemName = stringSplitted[2];

                    var imageAttachement = Context.Message.Attachments.FirstOrDefault();

                    // try to score the team
                    Team team = HuntManager.Teams.First(x => x.TeamName == teamName);
                    BossEnum boss = HuntManager.DetermineBoss(bossName);
                    string item = HuntManager.DetermineItem(itemName, boss);
                    byte[] image = HuntManager.GetImageData(imageAttachement);

                    bool validArgs = true;

                    if (validArgs && team == null)
                    {
                        await ReplyAsync(string.Format("Unable to find this team {0}", teamName));
                        validArgs = false;
                    }
                    if (validArgs && boss == BossEnum.Undefined)
                    {
                        await ReplyAsync(string.Format("Unable to find this boss {0}", boss));
                        validArgs = false;
                    }
                    if (validArgs && string.IsNullOrWhiteSpace(item))
                    {
                        await ReplyAsync(string.Format("Unable to find this item {0}", item));
                        validArgs = false;
                    }
                    if (validArgs && image.Length <= 0)
                    {
                        await ReplyAsync(string.Format("Unable to find image", item));
                        validArgs = false;
                    }

                    if (validArgs)
                    {
                        HuntManager.AddDrop(team, boss, item, image);
                    }

                }
            }
            
            // do stuff
        }
    }
}