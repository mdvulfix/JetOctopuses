﻿using System;

namespace APP.Player
{
    public enum PlayerAction
    {
        None,
        Login,
        MenuMain,
        MenuOptions,
        MenuPlay,
        MenuExit,
        LevelPlay,
        LevelPause,
        LevelExit,
        ResultExit
    }
    
    public abstract class PlayerModel<TPlayer>
    {


        public abstract void Move();
        public abstract void Eat();

    }

    public class PlayerDefault: PlayerModel<PlayerDefault>, IConfigurable
    {
        private int m_Health;
        private int m_Energy;
        private int m_Score;

        private int m_Speed;

        public void Configure(IConfig config)
        {

        }

        public override void Move() { }
        public override void Eat() { }
    }


}