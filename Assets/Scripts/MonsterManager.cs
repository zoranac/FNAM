using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : Singleton<MonsterManager>
{
    public List<Monster> Monsters = new List<Monster>();


    public void HideAllMonsters()
    {
        foreach (var item in Monsters)
        {
            item.HideImages();
        }
    }

    private void Update()
    {
        if (TimeController.Instance.Day == 1)
        {
            ViewershipManager.Instance.lossRate = .035f;

            if (TimeController.Instance.Hour <= 6)
            {
                Monsters[0].MoveChance = 0;
                Monsters[1].MoveChance = 0; 
            }
            else if (TimeController.Instance.Hour <= 6)
            {
                Monsters[0].MoveChance = 1;
                Monsters[1].MoveChance = 0;
            }
            else if (TimeController.Instance.Hour <= 7)
            {
                Monsters[0].MoveChance = 2;
                Monsters[1].MoveChance = 0;
            }
            else if (TimeController.Instance.Hour <= 8)
            {
                Monsters[0].MoveChance = 3;
                Monsters[1].MoveChance = 0;
            }
            else if (TimeController.Instance.Hour <= 9)
            {
                Monsters[0].MoveChance = 4;
                Monsters[1].MoveChance = 0;
            }
            else if (TimeController.Instance.Hour <= 11)
            {
                Monsters[0].MoveChance = 6;
                Monsters[1].MoveChance = 0;
            }
        }

        if (TimeController.Instance.Day == 2)
        {
            ViewershipManager.Instance.lossRate = .0325f;
            if (TimeController.Instance.Hour <= 6)
            {
                Monsters[0].MoveChance = 6;
                Monsters[1].MoveChance = 0;
            }
            else if (TimeController.Instance.Hour <= 11)
            {
                Monsters[0].MoveChance = 1;
                Monsters[1].MoveChance = 3;
            }
        }

        if (TimeController.Instance.Day == 3)
        {
            ViewershipManager.Instance.lossRate = .03f;

            if (TimeController.Instance.Hour <= 9)
            {
                Monsters[0].MoveChance = 4;
                Monsters[1].MoveChance = 3;
            }
            else if (TimeController.Instance.Hour <= 11)
            {
                Monsters[0].MoveChance = 6;
                Monsters[1].MoveChance = 4;
            }
        }

        if (TimeController.Instance.Day == 4)
        {
            ViewershipManager.Instance.lossRate = .0275f;

            if (TimeController.Instance.Hour <= 6)
            {
                Monsters[0].MoveChance = 8;
                Monsters[1].MoveChance = 3;
            }
            if (TimeController.Instance.Hour <= 10)
            {
                Monsters[0].MoveChance = 4;
                Monsters[1].MoveChance = 6;
            }
            else if (TimeController.Instance.Hour <= 11)
            {
                Monsters[0].MoveChance = 10;
                Monsters[1].MoveChance = 6;
            }
        }

        if (TimeController.Instance.Day == 5)
        {
            ViewershipManager.Instance.lossRate = .025f;

            if (TimeController.Instance.Hour <= 6)
            {
                Monsters[0].MoveChance = 10;
                Monsters[1].MoveChance = 3;
            }
            if (TimeController.Instance.Hour <= 9)
            {
                Monsters[0].MoveChance = 12;
                Monsters[1].MoveChance = 10;
            }
            else if (TimeController.Instance.Hour <= 11)
            {
                Monsters[0].MoveChance = 15;
                Monsters[1].MoveChance = 5;
            }
        }
        if (TimeController.Instance.Day >= 6)
        {
            ViewershipManager.Instance.lossRate = .04f - (.001f * SettingsController.Instance.Difficulty);

            Monsters[0].MoveChance = SettingsController.Instance.Difficulty;
            Monsters[1].MoveChance = SettingsController.Instance.Difficulty;
        }
    }
}
