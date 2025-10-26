using System;
using UnityEngine;

public class PlayerBaseInfo
{
    private string _playerName;
    private int _playerLevel;
    private int _playerExp;

    //private int _playerDollar;      // free currency
    //private int _playerGoldbar;     // premium currency

    private int _playerEnergy;
    private int _playerMaxEnergy;

    public event Action _playerLevelUpdate;
    public event Action _playerExpUpdate;

    //public event Action _playerDollarUpdate;
    //public event Action _playerGoldbarUpdate;

    public event Action _playerEnergyUpdate;
}
