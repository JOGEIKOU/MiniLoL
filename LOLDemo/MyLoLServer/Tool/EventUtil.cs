using Protocol.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// キャンピオン選び事件をクリエイト
/// </summary>
/// <param name="teamRed"></param>
/// <param name="teamBlue"></param>
public delegate void CreateSelectChampion(List<int> teamRed,List<int> teamBlue);
/// <summary>
/// キャンピオン選び事件を潰す　ルームを閉じる
/// </summary>
/// <param name="roomId"></param>
public delegate void DestorySelectChampion(int roomId);

/// <summary>
/// 戦闘モジュールのイベントクリエイト
/// </summary>
/// <param name="teamRed"></param>
/// <param name="teamBlue"></param>
public delegate void CreateFightRoom(SelectModel[] teamRed, SelectModel[] teamBlue);

/// <summary>
/// 戦闘モジュールのルームを潰す
/// </summary>
/// <param name="roomId"></param>
public delegate void DestoryFightRoom(int roomId);

namespace MyLoLServer.Tool
{
    public class EventUtil
    {
        public static CreateSelectChampion createSelect;

        public static DestorySelectChampion destorySelect;

        public static CreateFightRoom createFight;

        public static DestoryFightRoom destoryFight;
    }
}
