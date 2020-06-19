﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyLoLServer.Tool;
using NetFramework;
using NetFramework.auto;
using Protocol;
using Protocol.Constants;
using Protocol.Dto;
using Protocol.Dto.FightDTO;
using static Protocol.Constants.BuildData;

namespace MyLoLServer.Logic.Fight
{
    public class FightRoom : AbsMulitHandler, HandlerInterface
    {
        public Dictionary<int, AbsFightModel> teamRed = new Dictionary<int, AbsFightModel>();
        public Dictionary<int, AbsFightModel> teamBlue = new Dictionary<int, AbsFightModel>();

        //private ConcurrentInt enterCount;

        private List<int> off = new List<int>();
        private List<int> enterList = new List<int>();

        private int ChamCount;

        public void Init(SelectModel[] teamRed,SelectModel[] teamBlue)
        {
            ChamCount = teamRed.Length + teamBlue.Length;
            //enterCount = new ConcurrentInt(teamRed.Length + teamBlue.Length);
            this.teamRed.Clear();
            this.teamBlue.Clear();

            off.Clear();
            //チャンピオンデータ初期化
            foreach (var item in teamRed)
            {
                this.teamRed.Add(item.userId, Create(item,1));
            }
            foreach (var item in teamBlue)
            {
                this.teamBlue.Add(item.userId, Create(item,2));
            }
            //レッドチームの建築物id -1~-10
            for(int i = -1; i>=-3;i--)
            {
                this.teamRed.Add(i, CreateBuild(i, Math.Abs(i),1));
            }
            //ブルーチームの建築物id -11~-20
            for(int i = -11; i>=-13; i--)
            {
                this.teamBlue.Add(i, CreateBuild(i, Math.Abs(i) - 10,2));
            }
            enterList.Clear();
        }

        private BuildModel CreateBuild(int id , int code,int team)
        {
            BuildDataModel data = BuildData.buildMap[code];
            BuildModel model = new BuildModel(id, code, data.hp, data.hp, data.atk, data.def, data.reborn, data.rebornTime, data.initiative, data.infrared, data.name);
            model.type = ModelType.BUILD;
            model.team = team;
            return model;
        }

        private FightPlayerModel Create(SelectModel model,int team)
        {
            FightPlayerModel player = new FightPlayerModel();
            //召喚使いデータ初期化
            player.id = model.userId;
            player.code = model.Champion;
            player.type = ModelType.HUMAN;
            player.name = GetUser(model.userId).name;
            player.exp = 0;
            player.level = 1;
            player.free = 1;
            player.gold = 500;
            player.team = team;
            //チャンピオンデータ初期化
            ChampionDataModel data = ChampionData.chamMap[model.Champion];
            player.hp = data.hpBase;
            player.maxHp = data.hpBase;
            player.normalAtk = data.natkBase;
            player.normalDef = data.ndefBase;
            player.magicAtk = data.matkBase;
            player.magicDef = data.mdefBase;
            player.speed = data.mSpeed;
            player.atkSpeed = data.atkSpeed;
            player.visionRange = data.visionRange;
            player.skills = InitSkill(data.skills);
            player.itemequs = new int[6];
            return player;
        }

        /// <summary>
        /// Array to int  (skill)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private FightSkill[] InitSkill(int[] value)
        {
            FightSkill[] skills = new FightSkill[value.Length];

            for(int i = 0;i<value.Length;i++)
            {
                int skillCode = value[i];
                SkillDataModel data = SkillData.skillMap[skillCode];
                SkillLevelData levelData = data.levels[0];
                FightSkill skill = new FightSkill(skillCode, 0, levelData.Level, levelData.Time, data.name, levelData.Range, data.info, data.target, data.type);
                skills[i] = skill;
            }
            return skills;
        }

        public void ClientClose(UserToken token, string error)
        {
            Leave(token);
            int userId = GetUserId(token);
            if(teamRed.ContainsKey(userId)||teamBlue.ContainsKey(userId))
            {
                if(!off.Contains(userId))
                {
                    off.Add(userId);
                }
            }

            if(off.Count == ChamCount)
            {
                EventUtil.destoryFight(Area);
            }
        }

        public void MessageReceive(UserToken token, SokectModel message)
        {
            switch(message.command)
            {
                case FightProtocol.ENTER_CREQ:
                    Enter(token);
                    break;
                case FightProtocol.MOVE_CREQ:
                    Move(token, message.GetMessage<MoveDTO>());
                    break; 
            }
        }

        private void EnterBattle(UserToken token)
        {
            int userId = GetUserId(token);
            if (IsEntered(token)) return;
            base.Enter(token);
            if(!enterList.Contains(userId))
            {
                enterList.Add(userId);
            }
            //全ての人もう準備できた、ルームメッセージを送信
            if(enterList.Count== ChamCount)
            {
                FightRoomModel room = new FightRoomModel();
                room.teamRed = teamRed.Values.ToArray();
                room.teamBlue = teamBlue.Values.ToArray();
                Brocast(FightProtocol.START_BRO, room);
            }
        }

        private void Move(UserToken token, MoveDTO value)
        {
            int userId = GetUserId(token);
            value.userId = userId;
            Brocast(FightProtocol.MOVE_BRO, value);
        }

        public override byte Type
        {
            get
            {
                return GameProtocol.TYPE_FIGHT; 
            }
        }

    }
}
