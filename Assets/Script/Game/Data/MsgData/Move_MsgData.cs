﻿using UnityEngine;

namespace Game.Data.MsgData
{
    public struct Move_MsgData : IBaseMsgData 
    {
        public eCmd cmd;

        public Vector3 dir;

        public Vector3 pos;

        public string sender;

        public string receiver;

        #region IBaseMsgData implementation
        public eCmd GetCmd()
        {
            return cmd;
        }
        public string GetReceiver()
        {
            return receiver;
        }
        #endregion
    }
}