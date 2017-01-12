using UnityEngine;
using System.Collections;
using Game.Manager;
using Game.BaseClass;
using Game.Data.MsgData;

namespace Game.Helper
{
    public class SendObjMsgHelper 
    {
        public static void SendMsg_Idle(string receiver, string sender, Vector3 dir, Vector3 pos)
        {
            Idle_MsgData data = new Idle_MsgData
            { 
                cmd = eCmd.Idle,
                dir = dir,
                pos = pos,
                sender = sender,
                receiver = receiver
            };
            SendObjMsg(eCmd.Idle, data);
        }

        public static void SendMsg_Rush(string receiver, string sender, Vector3 dir, Vector3 pos)
        {
            Rush_MsgData data = new Rush_MsgData
            { 
                cmd = eCmd.Rush,
                dir = dir,
                pos = pos,
                sender = sender,
                receiver = receiver
            };
            SendObjMsg(eCmd.Rush, data);
        }

        public static void SendMsg_Move(string receiver, string sender, Vector3 dir, Vector3 pos)
        {
            Move_MsgData data = new Move_MsgData
            { 
                cmd = eCmd.Move,
                dir = dir,
                pos = pos,
                sender = sender,
                receiver = receiver
            };
            SendObjMsg(eCmd.Move, data);
        }

        public static void SendMsg_MoveTo(string receiver, string sender, Vector3 pos, Vector3 targetPos)
        {
            MoveTo_MsgData data = new MoveTo_MsgData
            { 
                cmd = eCmd.MoveTo,
                pos = pos,
                targetPos = targetPos,
                sender = sender,
                receiver = receiver
            };
            SendObjMsg(eCmd.MoveTo, data);
        }



        public static void SendMsg_Attack(string receiver, string sender, Vector3 dir, Vector3 pos, int attackIdx)
        {
            Attack_MsgData data = new Attack_MsgData
            { 
                cmd = eCmd.Attack,
                dir = dir,
                pos = pos,
                sender = sender,
                receiver = receiver,
                attackIdx = attackIdx,
            };
            SendObjMsg(eCmd.Attack, data);
        }

        private static bool SendObjMsg(eCmd cmd, IBaseMsgData data)
        {
            GameObject gobj = ObjectManager.FindObject(data.GetReceiver());
            if (gobj != null)
            {
                BaseObject component = gobj.GetComponent<BaseObject>();
                if (component != null)
                {
                    component.OnObjMsg(cmd, data);
                    return true;
                }
                else
                {
                    Debug.LogError("Don't find BaseObject Component on " +data.GetReceiver());
                }
            }
            return false;
        }
    }
}
