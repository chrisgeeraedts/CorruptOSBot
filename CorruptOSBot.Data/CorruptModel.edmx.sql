
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 09/16/2021 21:51:01
-- Generated from EDMX file: I:\Projects\CorruptOSBot\CorruptOSBot.Data\CorruptModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [CorruptOSBot];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_Activity]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PlayerPets] DROP CONSTRAINT [FK_Activity];
GO
IF OBJECT_ID(N'[dbo].[FK_BingoBossId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BingoCardSlot] DROP CONSTRAINT [FK_BingoBossId];
GO
IF OBJECT_ID(N'[dbo].[FK_BingoCardId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BingoCardSlot] DROP CONSTRAINT [FK_BingoCardId];
GO
IF OBJECT_ID(N'[dbo].[FK_BingoEventId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BingoCard] DROP CONSTRAINT [FK_BingoEventId];
GO
IF OBJECT_ID(N'[dbo].[FK_BingoTeamCardBingoCardId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BingoTeamCard] DROP CONSTRAINT [FK_BingoTeamCardBingoCardId];
GO
IF OBJECT_ID(N'[dbo].[FK_BingoTeamCardBingoTeamId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BingoTeamCard] DROP CONSTRAINT [FK_BingoTeamCardBingoTeamId];
GO
IF OBJECT_ID(N'[dbo].[FK_BingoTeamCardSlotBingoCardSlotId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BingoTeamCardSlot] DROP CONSTRAINT [FK_BingoTeamCardSlotBingoCardSlotId];
GO
IF OBJECT_ID(N'[dbo].[FK_BingoTeamCardSlotBingoTeamCardId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BingoTeamCardSlot] DROP CONSTRAINT [FK_BingoTeamCardSlotBingoTeamCardId];
GO
IF OBJECT_ID(N'[dbo].[FK_BingoTeamMemberBingoTeamId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BingoTeamMember] DROP CONSTRAINT [FK_BingoTeamMemberBingoTeamId];
GO
IF OBJECT_ID(N'[dbo].[FK_BingoTeamMemberDiscordUserId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BingoTeamMember] DROP CONSTRAINT [FK_BingoTeamMemberDiscordUserId];
GO
IF OBJECT_ID(N'[dbo].[FK_Boss]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PlayerPets] DROP CONSTRAINT [FK_Boss];
GO
IF OBJECT_ID(N'[dbo].[FK_BossId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[hunt_bossdrops] DROP CONSTRAINT [FK_BossId];
GO
IF OBJECT_ID(N'[dbo].[FK_DiscordUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RunescapeAccounts] DROP CONSTRAINT [FK_DiscordUser];
GO
IF OBJECT_ID(N'[dbo].[FK_DiscordUserId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[hunt_team_members] DROP CONSTRAINT [FK_DiscordUserId];
GO
IF OBJECT_ID(N'[dbo].[FK_DiscordUserPoints]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PointMutations] DROP CONSTRAINT [FK_DiscordUserPoints];
GO
IF OBJECT_ID(N'[dbo].[FK_DropBossDropId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[hunt_team_drops] DROP CONSTRAINT [FK_DropBossDropId];
GO
IF OBJECT_ID(N'[dbo].[FK_DropTeamId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[hunt_team_drops] DROP CONSTRAINT [FK_DropTeamId];
GO
IF OBJECT_ID(N'[dbo].[FK_PointStoreUserPoints]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PointMutations] DROP CONSTRAINT [FK_PointStoreUserPoints];
GO
IF OBJECT_ID(N'[dbo].[FK_RunescapeAccount]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PlayerPets] DROP CONSTRAINT [FK_RunescapeAccount];
GO
IF OBJECT_ID(N'[dbo].[FK_Skill]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PlayerPets] DROP CONSTRAINT [FK_Skill];
GO
IF OBJECT_ID(N'[dbo].[FK_TeamId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[hunt_team_members] DROP CONSTRAINT [FK_TeamId];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Activities]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Activities];
GO
IF OBJECT_ID(N'[dbo].[BingoCard]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BingoCard];
GO
IF OBJECT_ID(N'[dbo].[BingoCardSlot]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BingoCardSlot];
GO
IF OBJECT_ID(N'[dbo].[BingoEvent]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BingoEvent];
GO
IF OBJECT_ID(N'[dbo].[BingoTeam]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BingoTeam];
GO
IF OBJECT_ID(N'[dbo].[BingoTeamCard]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BingoTeamCard];
GO
IF OBJECT_ID(N'[dbo].[BingoTeamCardSlot]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BingoTeamCardSlot];
GO
IF OBJECT_ID(N'[dbo].[BingoTeamMember]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BingoTeamMember];
GO
IF OBJECT_ID(N'[dbo].[Bosses]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Bosses];
GO
IF OBJECT_ID(N'[dbo].[BotConfiguration]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BotConfiguration];
GO
IF OBJECT_ID(N'[dbo].[Calendar]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Calendar];
GO
IF OBJECT_ID(N'[dbo].[Channels]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Channels];
GO
IF OBJECT_ID(N'[dbo].[ChatLog]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ChatLog];
GO
IF OBJECT_ID(N'[dbo].[DiscordUsers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DiscordUsers];
GO
IF OBJECT_ID(N'[dbo].[ErrorLog]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ErrorLog];
GO
IF OBJECT_ID(N'[dbo].[hunt_bossdrops]', 'U') IS NOT NULL
    DROP TABLE [dbo].[hunt_bossdrops];
GO
IF OBJECT_ID(N'[dbo].[hunt_bosses]', 'U') IS NOT NULL
    DROP TABLE [dbo].[hunt_bosses];
GO
IF OBJECT_ID(N'[dbo].[hunt_team_drops]', 'U') IS NOT NULL
    DROP TABLE [dbo].[hunt_team_drops];
GO
IF OBJECT_ID(N'[dbo].[hunt_team_members]', 'U') IS NOT NULL
    DROP TABLE [dbo].[hunt_team_members];
GO
IF OBJECT_ID(N'[dbo].[hunt_teams]', 'U') IS NOT NULL
    DROP TABLE [dbo].[hunt_teams];
GO
IF OBJECT_ID(N'[dbo].[PlayerPets]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PlayerPets];
GO
IF OBJECT_ID(N'[dbo].[PointMutations]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PointMutations];
GO
IF OBJECT_ID(N'[dbo].[PointStore]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PointStore];
GO
IF OBJECT_ID(N'[dbo].[Roles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Roles];
GO
IF OBJECT_ID(N'[dbo].[RunescapeAccounts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RunescapeAccounts];
GO
IF OBJECT_ID(N'[dbo].[Skills]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Skills];
GO
IF OBJECT_ID(N'[dbo].[Toggles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Toggles];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Activities'
CREATE TABLE [dbo].[Activities] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Emoji] nvarchar(max)  NULL
);
GO

-- Creating table 'Bosses'
CREATE TABLE [dbo].[Bosses] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Bossname] nvarchar(max)  NOT NULL,
    [EmojiName] nvarchar(max)  NOT NULL,
    [BossImage] nvarchar(max)  NULL,
    [BossCommand] nvarchar(50)  NULL
);
GO

-- Creating table 'BotConfigurations'
CREATE TABLE [dbo].[BotConfigurations] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [PropertyName] nvarchar(max)  NOT NULL,
    [PropertyValue] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Calendars'
CREATE TABLE [dbo].[Calendars] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Title] nvarchar(50)  NOT NULL,
    [StartsAt] datetime  NOT NULL,
    [EndsAt] datetime  NOT NULL,
    [Description] nvarchar(max)  NULL
);
GO

-- Creating table 'Channels'
CREATE TABLE [dbo].[Channels] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50)  NOT NULL,
    [DiscordChannelId] bigint  NOT NULL
);
GO

-- Creating table 'ChatLogs'
CREATE TABLE [dbo].[ChatLogs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Message] varchar(max)  NOT NULL,
    [Author] varchar(max)  NOT NULL,
    [Datetime] datetime  NOT NULL,
    [Severity] nvarchar(max)  NOT NULL,
    [Channel] varchar(max)  NULL,
    [PostId] bigint  NULL,
    [ChannelId] bigint  NULL,
    [AuthorId] bigint  NULL
);
GO

-- Creating table 'DiscordUsers'
CREATE TABLE [dbo].[DiscordUsers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Username] nvarchar(max)  NULL,
    [DiscordId] bigint  NULL,
    [OriginallyJoinedAt] datetime  NULL,
    [BlacklistedForPromotion] bit  NOT NULL,
    [LeavingDate] datetime  NULL,
    [CorruptPoints] int  NOT NULL
);
GO

-- Creating table 'ErrorLogs'
CREATE TABLE [dbo].[ErrorLogs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Message] nvarchar(max)  NULL,
    [Severity] nvarchar(max)  NULL,
    [Datetime] datetime  NULL
);
GO

-- Creating table 'hunt_bossdrops'
CREATE TABLE [dbo].[hunt_bossdrops] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ItemName] nvarchar(max)  NOT NULL,
    [BasePointValue] int  NOT NULL,
    [BossId] int  NOT NULL
);
GO

-- Creating table 'hunt_bosses'
CREATE TABLE [dbo].[hunt_bosses] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [AllUniquesValue] int  NOT NULL
);
GO

-- Creating table 'hunt_team_drops'
CREATE TABLE [dbo].[hunt_team_drops] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [TeamId] int  NOT NULL,
    [DropId] int  NOT NULL,
    [PointValue] int  NULL,
    [DropDate] datetime  NULL,
    [DropImage] nvarchar(max)  NULL
);
GO

-- Creating table 'hunt_team_members'
CREATE TABLE [dbo].[hunt_team_members] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [TeamId] int  NOT NULL,
    [DiscordUserId] int  NOT NULL
);
GO

-- Creating table 'hunt_teams'
CREATE TABLE [dbo].[hunt_teams] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [TeamName] nvarchar(max)  NULL
);
GO

-- Creating table 'PlayerPets'
CREATE TABLE [dbo].[PlayerPets] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [DropDate] datetime  NULL,
    [BossId] int  NULL,
    [SkillId] int  NULL,
    [ActivityId] int  NULL,
    [RunescapeAccountId] int  NULL,
    [LevelOrKcGotten] int  NULL
);
GO

-- Creating table 'PointMutations'
CREATE TABLE [dbo].[PointMutations] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [PointChange] int  NOT NULL,
    [DateTime] datetime  NOT NULL,
    [TargetPlayerId] int  NOT NULL,
    [PointStoreItemId] int  NULL
);
GO

-- Creating table 'PointStores'
CREATE TABLE [dbo].[PointStores] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [StoreItemName] nvarchar(50)  NOT NULL,
    [StoreItemValue] int  NOT NULL,
    [StoreItemDescription] nvarchar(max)  NULL,
    [StoreItemImage] nvarchar(max)  NULL,
    [StoreItemCommand] nvarchar(50)  NULL
);
GO

-- Creating table 'RunescapeAccounts'
CREATE TABLE [dbo].[RunescapeAccounts] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [rsn] varchar(max)  NOT NULL,
    [wom_id] int  NULL,
    [DiscordUserId] int  NULL,
    [account_type] nvarchar(max)  NULL
);
GO

-- Creating table 'Skills'
CREATE TABLE [dbo].[Skills] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Emoji] nvarchar(max)  NULL,
    [Image] nvarchar(max)  NULL
);
GO

-- Creating table 'Toggles'
CREATE TABLE [dbo].[Toggles] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Functionality] nvarchar(50)  NOT NULL,
    [Toggled] bit  NOT NULL,
    [Type] nvarchar(50)  NULL
);
GO

-- Creating table 'Roles'
CREATE TABLE [dbo].[Roles] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50)  NOT NULL,
    [DiscordRoleId] bigint  NOT NULL,
    [DaysToReach] int  NOT NULL,
    [CanUpgradeTo] bit  NOT NULL,
    [IconUrl] nvarchar(max)  NULL,
    [IsStaff] bit  NOT NULL,
    [EmojiId] bigint  NULL
);
GO

-- Creating table 'BingoCards'
CREATE TABLE [dbo].[BingoCards] (
    [Id] int  NOT NULL,
    [BingoEventId] int  NOT NULL
);
GO

-- Creating table 'BingoCardSlots'
CREATE TABLE [dbo].[BingoCardSlots] (
    [Id] int  NOT NULL,
    [BingoCardId] int  NOT NULL,
    [BossId] int  NOT NULL,
    [ItemName] nvarchar(max)  NULL,
    [ItemImage] varchar(max)  NULL,
    [SlotLetter] nvarchar(50)  NULL
);
GO

-- Creating table 'BingoEvents'
CREATE TABLE [dbo].[BingoEvents] (
    [Id] int  NOT NULL,
    [WomId] int  NULL,
    [SourceChannelId] bigint  NULL,
    [TargetChannelId] bigint  NULL,
    [EmojiYesId] bigint  NULL,
    [EmojiNoId] nchar(10)  NULL,
    [Active] bit  NOT NULL
);
GO

-- Creating table 'BingoTeams'
CREATE TABLE [dbo].[BingoTeams] (
    [Id] int  NOT NULL,
    [TeamName] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'BingoTeamCards'
CREATE TABLE [dbo].[BingoTeamCards] (
    [Id] int  NOT NULL,
    [BingoTeamId] int  NOT NULL,
    [BingoCardId] int  NOT NULL
);
GO

-- Creating table 'BingoTeamCardSlots'
CREATE TABLE [dbo].[BingoTeamCardSlots] (
    [Id] int  NOT NULL,
    [BingoTeamCardId] int  NOT NULL,
    [BingoCardSlotId] int  NOT NULL,
    [DateApproved] nchar(10)  NULL,
    [DateDenied] nchar(10)  NULL
);
GO

-- Creating table 'BingoTeamMembers'
CREATE TABLE [dbo].[BingoTeamMembers] (
    [Id] int  NOT NULL,
    [BingoTeamId] int  NOT NULL,
    [DiscordUserId] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Activities'
ALTER TABLE [dbo].[Activities]
ADD CONSTRAINT [PK_Activities]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Bosses'
ALTER TABLE [dbo].[Bosses]
ADD CONSTRAINT [PK_Bosses]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'BotConfigurations'
ALTER TABLE [dbo].[BotConfigurations]
ADD CONSTRAINT [PK_BotConfigurations]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Calendars'
ALTER TABLE [dbo].[Calendars]
ADD CONSTRAINT [PK_Calendars]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Channels'
ALTER TABLE [dbo].[Channels]
ADD CONSTRAINT [PK_Channels]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ChatLogs'
ALTER TABLE [dbo].[ChatLogs]
ADD CONSTRAINT [PK_ChatLogs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DiscordUsers'
ALTER TABLE [dbo].[DiscordUsers]
ADD CONSTRAINT [PK_DiscordUsers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ErrorLogs'
ALTER TABLE [dbo].[ErrorLogs]
ADD CONSTRAINT [PK_ErrorLogs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'hunt_bossdrops'
ALTER TABLE [dbo].[hunt_bossdrops]
ADD CONSTRAINT [PK_hunt_bossdrops]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'hunt_bosses'
ALTER TABLE [dbo].[hunt_bosses]
ADD CONSTRAINT [PK_hunt_bosses]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'hunt_team_drops'
ALTER TABLE [dbo].[hunt_team_drops]
ADD CONSTRAINT [PK_hunt_team_drops]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'hunt_team_members'
ALTER TABLE [dbo].[hunt_team_members]
ADD CONSTRAINT [PK_hunt_team_members]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'hunt_teams'
ALTER TABLE [dbo].[hunt_teams]
ADD CONSTRAINT [PK_hunt_teams]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PlayerPets'
ALTER TABLE [dbo].[PlayerPets]
ADD CONSTRAINT [PK_PlayerPets]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PointMutations'
ALTER TABLE [dbo].[PointMutations]
ADD CONSTRAINT [PK_PointMutations]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PointStores'
ALTER TABLE [dbo].[PointStores]
ADD CONSTRAINT [PK_PointStores]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'RunescapeAccounts'
ALTER TABLE [dbo].[RunescapeAccounts]
ADD CONSTRAINT [PK_RunescapeAccounts]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Skills'
ALTER TABLE [dbo].[Skills]
ADD CONSTRAINT [PK_Skills]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Toggles'
ALTER TABLE [dbo].[Toggles]
ADD CONSTRAINT [PK_Toggles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Roles'
ALTER TABLE [dbo].[Roles]
ADD CONSTRAINT [PK_Roles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'BingoCards'
ALTER TABLE [dbo].[BingoCards]
ADD CONSTRAINT [PK_BingoCards]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'BingoCardSlots'
ALTER TABLE [dbo].[BingoCardSlots]
ADD CONSTRAINT [PK_BingoCardSlots]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'BingoEvents'
ALTER TABLE [dbo].[BingoEvents]
ADD CONSTRAINT [PK_BingoEvents]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'BingoTeams'
ALTER TABLE [dbo].[BingoTeams]
ADD CONSTRAINT [PK_BingoTeams]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'BingoTeamCards'
ALTER TABLE [dbo].[BingoTeamCards]
ADD CONSTRAINT [PK_BingoTeamCards]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'BingoTeamCardSlots'
ALTER TABLE [dbo].[BingoTeamCardSlots]
ADD CONSTRAINT [PK_BingoTeamCardSlots]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'BingoTeamMembers'
ALTER TABLE [dbo].[BingoTeamMembers]
ADD CONSTRAINT [PK_BingoTeamMembers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [ActivityId] in table 'PlayerPets'
ALTER TABLE [dbo].[PlayerPets]
ADD CONSTRAINT [FK_Activity]
    FOREIGN KEY ([ActivityId])
    REFERENCES [dbo].[Activities]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Activity'
CREATE INDEX [IX_FK_Activity]
ON [dbo].[PlayerPets]
    ([ActivityId]);
GO

-- Creating foreign key on [BossId] in table 'PlayerPets'
ALTER TABLE [dbo].[PlayerPets]
ADD CONSTRAINT [FK_Boss]
    FOREIGN KEY ([BossId])
    REFERENCES [dbo].[Bosses]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Boss'
CREATE INDEX [IX_FK_Boss]
ON [dbo].[PlayerPets]
    ([BossId]);
GO

-- Creating foreign key on [DiscordUserId] in table 'RunescapeAccounts'
ALTER TABLE [dbo].[RunescapeAccounts]
ADD CONSTRAINT [FK_DiscordUser]
    FOREIGN KEY ([DiscordUserId])
    REFERENCES [dbo].[DiscordUsers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DiscordUser'
CREATE INDEX [IX_FK_DiscordUser]
ON [dbo].[RunescapeAccounts]
    ([DiscordUserId]);
GO

-- Creating foreign key on [DiscordUserId] in table 'hunt_team_members'
ALTER TABLE [dbo].[hunt_team_members]
ADD CONSTRAINT [FK_DiscordUserId]
    FOREIGN KEY ([DiscordUserId])
    REFERENCES [dbo].[DiscordUsers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DiscordUserId'
CREATE INDEX [IX_FK_DiscordUserId]
ON [dbo].[hunt_team_members]
    ([DiscordUserId]);
GO

-- Creating foreign key on [TargetPlayerId] in table 'PointMutations'
ALTER TABLE [dbo].[PointMutations]
ADD CONSTRAINT [FK_DiscordUserPoints]
    FOREIGN KEY ([TargetPlayerId])
    REFERENCES [dbo].[DiscordUsers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DiscordUserPoints'
CREATE INDEX [IX_FK_DiscordUserPoints]
ON [dbo].[PointMutations]
    ([TargetPlayerId]);
GO

-- Creating foreign key on [BossId] in table 'hunt_bossdrops'
ALTER TABLE [dbo].[hunt_bossdrops]
ADD CONSTRAINT [FK_BossId]
    FOREIGN KEY ([BossId])
    REFERENCES [dbo].[hunt_bosses]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BossId'
CREATE INDEX [IX_FK_BossId]
ON [dbo].[hunt_bossdrops]
    ([BossId]);
GO

-- Creating foreign key on [DropId] in table 'hunt_team_drops'
ALTER TABLE [dbo].[hunt_team_drops]
ADD CONSTRAINT [FK_DropBossDropId]
    FOREIGN KEY ([DropId])
    REFERENCES [dbo].[hunt_bossdrops]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DropBossDropId'
CREATE INDEX [IX_FK_DropBossDropId]
ON [dbo].[hunt_team_drops]
    ([DropId]);
GO

-- Creating foreign key on [TeamId] in table 'hunt_team_drops'
ALTER TABLE [dbo].[hunt_team_drops]
ADD CONSTRAINT [FK_DropTeamId]
    FOREIGN KEY ([TeamId])
    REFERENCES [dbo].[hunt_teams]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DropTeamId'
CREATE INDEX [IX_FK_DropTeamId]
ON [dbo].[hunt_team_drops]
    ([TeamId]);
GO

-- Creating foreign key on [TeamId] in table 'hunt_team_members'
ALTER TABLE [dbo].[hunt_team_members]
ADD CONSTRAINT [FK_TeamId]
    FOREIGN KEY ([TeamId])
    REFERENCES [dbo].[hunt_teams]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TeamId'
CREATE INDEX [IX_FK_TeamId]
ON [dbo].[hunt_team_members]
    ([TeamId]);
GO

-- Creating foreign key on [RunescapeAccountId] in table 'PlayerPets'
ALTER TABLE [dbo].[PlayerPets]
ADD CONSTRAINT [FK_RunescapeAccount]
    FOREIGN KEY ([RunescapeAccountId])
    REFERENCES [dbo].[RunescapeAccounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RunescapeAccount'
CREATE INDEX [IX_FK_RunescapeAccount]
ON [dbo].[PlayerPets]
    ([RunescapeAccountId]);
GO

-- Creating foreign key on [SkillId] in table 'PlayerPets'
ALTER TABLE [dbo].[PlayerPets]
ADD CONSTRAINT [FK_Skill]
    FOREIGN KEY ([SkillId])
    REFERENCES [dbo].[Skills]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Skill'
CREATE INDEX [IX_FK_Skill]
ON [dbo].[PlayerPets]
    ([SkillId]);
GO

-- Creating foreign key on [PointStoreItemId] in table 'PointMutations'
ALTER TABLE [dbo].[PointMutations]
ADD CONSTRAINT [FK_PointStoreUserPoints]
    FOREIGN KEY ([PointStoreItemId])
    REFERENCES [dbo].[PointStores]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PointStoreUserPoints'
CREATE INDEX [IX_FK_PointStoreUserPoints]
ON [dbo].[PointMutations]
    ([PointStoreItemId]);
GO

-- Creating foreign key on [BingoCardId] in table 'BingoCardSlots'
ALTER TABLE [dbo].[BingoCardSlots]
ADD CONSTRAINT [FK_BingoCardId]
    FOREIGN KEY ([BingoCardId])
    REFERENCES [dbo].[BingoCards]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BingoCardId'
CREATE INDEX [IX_FK_BingoCardId]
ON [dbo].[BingoCardSlots]
    ([BingoCardId]);
GO

-- Creating foreign key on [BingoEventId] in table 'BingoCards'
ALTER TABLE [dbo].[BingoCards]
ADD CONSTRAINT [FK_BingoEventId]
    FOREIGN KEY ([BingoEventId])
    REFERENCES [dbo].[BingoEvents]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BingoEventId'
CREATE INDEX [IX_FK_BingoEventId]
ON [dbo].[BingoCards]
    ([BingoEventId]);
GO

-- Creating foreign key on [BingoCardId] in table 'BingoTeamCards'
ALTER TABLE [dbo].[BingoTeamCards]
ADD CONSTRAINT [FK_BingoTeamCardBingoCardId]
    FOREIGN KEY ([BingoCardId])
    REFERENCES [dbo].[BingoCards]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BingoTeamCardBingoCardId'
CREATE INDEX [IX_FK_BingoTeamCardBingoCardId]
ON [dbo].[BingoTeamCards]
    ([BingoCardId]);
GO

-- Creating foreign key on [BossId] in table 'BingoCardSlots'
ALTER TABLE [dbo].[BingoCardSlots]
ADD CONSTRAINT [FK_BingoBossId]
    FOREIGN KEY ([BossId])
    REFERENCES [dbo].[Bosses]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BingoBossId'
CREATE INDEX [IX_FK_BingoBossId]
ON [dbo].[BingoCardSlots]
    ([BossId]);
GO

-- Creating foreign key on [BingoCardSlotId] in table 'BingoTeamCardSlots'
ALTER TABLE [dbo].[BingoTeamCardSlots]
ADD CONSTRAINT [FK_BingoTeamCardSlotBingoCardSlotId]
    FOREIGN KEY ([BingoCardSlotId])
    REFERENCES [dbo].[BingoCardSlots]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BingoTeamCardSlotBingoCardSlotId'
CREATE INDEX [IX_FK_BingoTeamCardSlotBingoCardSlotId]
ON [dbo].[BingoTeamCardSlots]
    ([BingoCardSlotId]);
GO

-- Creating foreign key on [BingoTeamId] in table 'BingoTeamCards'
ALTER TABLE [dbo].[BingoTeamCards]
ADD CONSTRAINT [FK_BingoTeamCardBingoTeamId]
    FOREIGN KEY ([BingoTeamId])
    REFERENCES [dbo].[BingoTeams]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BingoTeamCardBingoTeamId'
CREATE INDEX [IX_FK_BingoTeamCardBingoTeamId]
ON [dbo].[BingoTeamCards]
    ([BingoTeamId]);
GO

-- Creating foreign key on [BingoTeamId] in table 'BingoTeamMembers'
ALTER TABLE [dbo].[BingoTeamMembers]
ADD CONSTRAINT [FK_BingoTeamMemberBingoTeamId]
    FOREIGN KEY ([BingoTeamId])
    REFERENCES [dbo].[BingoTeams]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BingoTeamMemberBingoTeamId'
CREATE INDEX [IX_FK_BingoTeamMemberBingoTeamId]
ON [dbo].[BingoTeamMembers]
    ([BingoTeamId]);
GO

-- Creating foreign key on [BingoTeamCardId] in table 'BingoTeamCardSlots'
ALTER TABLE [dbo].[BingoTeamCardSlots]
ADD CONSTRAINT [FK_BingoTeamCardSlotBingoTeamCardId]
    FOREIGN KEY ([BingoTeamCardId])
    REFERENCES [dbo].[BingoTeamCards]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BingoTeamCardSlotBingoTeamCardId'
CREATE INDEX [IX_FK_BingoTeamCardSlotBingoTeamCardId]
ON [dbo].[BingoTeamCardSlots]
    ([BingoTeamCardId]);
GO

-- Creating foreign key on [DiscordUserId] in table 'BingoTeamMembers'
ALTER TABLE [dbo].[BingoTeamMembers]
ADD CONSTRAINT [FK_BingoTeamMemberDiscordUserId]
    FOREIGN KEY ([DiscordUserId])
    REFERENCES [dbo].[DiscordUsers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BingoTeamMemberDiscordUserId'
CREATE INDEX [IX_FK_BingoTeamMemberDiscordUserId]
ON [dbo].[BingoTeamMembers]
    ([DiscordUserId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------