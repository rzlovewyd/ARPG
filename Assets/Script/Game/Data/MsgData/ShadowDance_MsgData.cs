﻿using UnityEngine;
using System.Collections;

namespace Game.Data.MsgData
{
    public class ShadowDance_MsgData : IBaseMsgData
    {
        public eCmd cmd;

        public Vector3 dir;

        public Vector3 pos;

        public int hitCount;

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
