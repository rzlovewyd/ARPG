using UnityEngine;
using System.Collections;

namespace Game.Data.MsgData
{
    public class FatalCircle_MsgData : IBaseMsgData
    {
        public eCmd cmd;

        public Vector3 dir;

        public Vector3 pos;

        public float range;

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
