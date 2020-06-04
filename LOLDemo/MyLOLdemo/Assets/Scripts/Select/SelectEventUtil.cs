using Protocol.Dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void DelSelected();
public delegate void DelRefreshUI(SelectRoomDTO selectRoom);
public delegate void DelSelectChampion(int id);
public class SelectEventUtil
{
    public static DelSelected selected;
    public static DelRefreshUI refreshUI;
    public static DelSelectChampion selectCham;
}
