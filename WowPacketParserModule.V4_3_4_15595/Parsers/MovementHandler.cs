﻿using System.Linq;
using WowPacketParser.DBC;
using WowPacketParser.Enums;
using WowPacketParser.Enums.Version;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;
using WowPacketParser.Proto;
using WowPacketParser.Store;
using WowPacketParser.Store.Objects;
using CoreParsers = WowPacketParser.Parsing.Parsers;
using MovementFlag = WowPacketParser.Enums.v4.MovementFlag;
using MovementFlagExtra = WowPacketParser.Enums.v4.MovementFlag2;
using SetCollisionHeightReason = WowPacketParserModule.V4_3_4_15595.Enums.SetCollisionHeightReason;
using SplineFlag = WowPacketParserModule.V4_3_4_15595.Enums.SplineFlag;

namespace WowPacketParserModule.V4_3_4_15595.Parsers
{
    public static class MovementHandler
    {
        [Parser(Opcode.SMSG_ON_MONSTER_MOVE)]
        [Parser(Opcode.SMSG_MONSTER_MOVE_TRANSPORT)]
        public static void HandleMonsterMove(Packet packet)
        {
            var monsterMove = packet.Holder.MonsterMove = new();
            var guid = packet.ReadPackedGuid("MoverGUID");
            monsterMove.Mover = guid;

            if (guid.GetObjectType() == ObjectType.Unit && Storage.Objects != null && Storage.Objects.ContainsKey(guid))
            {
                var obj = Storage.Objects[guid].Item1 as Unit;
                if (obj.UpdateFields != null)
                    if ((obj.UnitData.Flags & (uint)UnitFlags.IsInCombat) == 0) // movement could be because of aggro so ignore that
                        obj.Movement.HasWpsOrRandMov = true;
            }

            if (packet.Opcode == Opcodes.GetOpcode(Opcode.SMSG_MONSTER_MOVE_TRANSPORT, Direction.ServerToClient))
            {
                monsterMove.TransportGuid = packet.ReadPackedGuid("TransportGUID");
                monsterMove.VehicleSeat = packet.ReadSByte("VehicleSeat");
            }

            packet.ReadSByte("VehicleExitVoluntary");
            var pos = packet.ReadVector3("Position");
            monsterMove.Position = pos;

            ReadMovementMonsterSpline(packet, pos, "MovementMonsterSpline");
        }

        public static void ReadMovementMonsterSpline(Packet packet, Vector3 pos, params object[] indexes)
        {
            packet.Holder.MonsterMove.Id = (uint)packet.ReadInt32("Id", indexes);
            ReadMovementSpline(packet, pos, indexes, "MovementSpline");
        }

        public static void ReadMovementSpline(Packet packet, Vector3 pos, params object[] indexes)
        {
            var monsterMove = packet.Holder.MonsterMove;
            var type = packet.ReadSByteE<SplineType>("Face", indexes);

            switch (type)
            {
                case SplineType.FacingSpot:
                    monsterMove.LookPosition = packet.ReadVector3("FaceSpot", indexes);
                    break;
                case SplineType.FacingTarget:
                    monsterMove.LookTarget = new() { Target = packet.ReadGuid("FacingGUID", indexes) };
                    break;
                case SplineType.FacingAngle:
                    monsterMove.LookOrientation = packet.ReadSingle("FaceDirection", indexes);
                    break;
                case SplineType.Stop:
                    return;
            }

            var flags = packet.ReadInt32E<SplineFlag>("Flags", indexes);
            monsterMove.Flags = flags.ToUniversal();

            if (flags.HasAnyFlag(SplineFlag.Animation))
            {
                packet.ReadSByteE<MovementAnimationState>("AnimTier", indexes);
                packet.ReadInt32("TierTransStartTime", indexes); // Async-time in ms
            }

            monsterMove.MoveTime = (uint)packet.ReadInt32("MoveTime", indexes);

            if (flags.HasAnyFlag(SplineFlag.Parabolic))
            {
                var jump = monsterMove.Jump = new();
                jump.Gravity = packet.ReadSingle("JumpGravity", indexes);
                jump.Duration = (uint)packet.ReadInt32("SpecialTime", indexes);
            }

            var pointsCount = packet.ReadInt32("PointsCount", indexes);

            if (flags.HasAnyFlag(SplineFlag.UncompressedPath))
            {
                for (var i = 0; i < pointsCount; i++)
                    monsterMove.Points.Add(packet.ReadVector3("Waypoints", indexes, i));
            }
            else
            {
                var endpos = packet.ReadVector3("Points", indexes);

                if (pointsCount > 1)
                {
                    var mid = (pos + endpos) * 0.5f;

                    for (var i = 0; i < pointsCount - 1; ++i)
                    {
                        var vec = mid - packet.ReadPackedVector3();

                        monsterMove.PackedPoints.Add(packet.AddValue("Waypoints", vec, indexes, i));
                    }
                }
            }
        }

        [HasSniffData]
        [Parser(Opcode.SMSG_NEW_WORLD)]
        public static void HandleNewWorld434(Packet packet)
        {
            packet.ReadSingle("X");
            packet.ReadSingle("Orientation");
            packet.ReadSingle("Y");
            CoreParsers.MovementHandler.CurrentMapId = (uint)packet.ReadInt32<MapId>("MapID");
            packet.ReadSingle("Z"); // seriously...
            packet.AddSniffData(StoreNameType.Map, (int)CoreParsers.MovementHandler.CurrentMapId, "NEW_WORLD");
        }

        [Parser(Opcode.MSG_MOVE_TELEPORT_ACK)]
        public static void HandleMoveTeleportAck434(Packet packet)
        {
            packet.ReadInt32("AckIndex");
            packet.ReadInt32("MoveTime");
            var guid = packet.StartBitStream(5, 0, 1, 6, 3, 7, 2, 4);
            packet.ParseBitStream(guid, 4, 2, 7, 6, 5, 1, 3, 0);
            packet.WriteGuid("MoverGUID", guid);
        }

        [Parser(Opcode.MSG_MOVE_HEARTBEAT)]
        public static void HandleMoveHeartbeat434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();

            pos.Z = packet.ReadSingle();
            pos.X = packet.ReadSingle();
            pos.Y = packet.ReadSingle();
            var hasPitch = !packet.ReadBit("Has pitch");
            var hasTime = !packet.ReadBit("Has timestamp");
            var hasFallData = packet.ReadBit("Has fall data");
            var hasMovementFlags2 = !packet.ReadBit();
            var hasTrans = packet.ReadBit("Has transport");
            guid[7] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            var hasO = !packet.ReadBit();
            guid[5] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            packet.ReadBit("Has Spline");
            packet.ReadBit();
            guid[6] = packet.ReadBit();
            var hasMovementFlags = !packet.ReadBit();

            if (hasTrans)
            {
                hasTransTime3 = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
            }

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            if (hasMovementFlags)
                packet.ReadBitsE<WowPacketParser.Enums.v4.MovementFlag>("Movement Flags", 30);

            if (hasMovementFlags2)
                packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            packet.ParseBitStream(guid, 3, 6, 1, 7, 2, 5, 0, 4);

            if (hasTrans)
            {
                var tpos = new Vector4();
                tpos.Z = packet.ReadSingle();
                packet.ReadSByte("Transport Seat");
                tpos.O = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 4);
                tpos.Y = packet.ReadSingle();
                packet.ReadUInt32("Transport Time");
                tpos.X = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 5);
                packet.ReadXORByte(transportGuid, 1);
                packet.ReadXORByte(transportGuid, 3);
                packet.ReadXORByte(transportGuid, 7);

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                packet.ReadXORByte(transportGuid, 2);
                packet.ReadXORByte(transportGuid, 0);
                packet.ReadXORByte(transportGuid, 6);

                packet.WriteGuid("Transport Guid", transportGuid);
                packet.AddValue("Transport Position", tpos);
            }

            if (hasO)
                pos.O = packet.ReadSingle();

            if (hasFallData)
            {
                packet.ReadSingle("Vertical Speed");
                packet.ReadUInt32("Fall time");
                if (hasFallDirection)
                {
                    packet.ReadSingle("Horizontal Speed");
                    packet.ReadSingle("Fall Sin");
                    packet.ReadSingle("Fall Cos");
                }
            }

            if (hasPitch)
                packet.ReadSingle("Pitch");
            if (hasSplineElev)
                packet.ReadSingle("Spline elevation");
            if (hasTime)
                packet.ReadUInt32("Timestamp");

            packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
        }

        [Parser(Opcode.MSG_MOVE_SET_PITCH)]
        public static void HandleMoveSetPitch434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();

            pos.X = packet.ReadSingle();
            pos.Z = packet.ReadSingle();
            pos.Y = packet.ReadSingle();
            var hasFallData = packet.ReadBit("Has fall data");
            var hasMovementFlags = !packet.ReadBit();
            guid[1] = packet.ReadBit();
            var hasO = !packet.ReadBit();
            guid[7] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            var hasMovementFlags2 = !packet.ReadBit();
            var hasTrans = packet.ReadBit("Has transport");
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            guid[6] = packet.ReadBit();
            var hasPitch = !packet.ReadBit("Has pitch");
            guid[4] = packet.ReadBit();
            packet.ReadBit("Has Spline");
            guid[2] = packet.ReadBit();
            packet.ReadBit();
            var hasTime = !packet.ReadBit("Has timestamp");
            guid[0] = packet.ReadBit();
            guid[5] = packet.ReadBit();

            if (hasTrans)
            {
                transportGuid[3] = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
            }

            if (hasMovementFlags2)
                packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            if (hasMovementFlags)
                packet.ReadBitsE<WowPacketParser.Enums.v4.MovementFlag>("Movement Flags", 30);

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            packet.ParseBitStream(guid, 3, 7, 1, 6, 0, 5, 2, 4);

            if (hasTrans)
            {
                var tpos = new Vector4();

                packet.ReadXORByte(transportGuid, 4);
                packet.ReadXORByte(transportGuid, 3);
                packet.ReadXORByte(transportGuid, 6);
                tpos.O = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 1);

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                packet.ReadUInt32("Transport Time");
                packet.ReadXORByte(transportGuid, 2);
                tpos.Z = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 0);

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                tpos.Y = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 5);
                packet.ReadSByte("Transport Seat");
                packet.ReadXORByte(transportGuid, 7);
                tpos.X = packet.ReadSingle();

                packet.WriteGuid("Transport Guid", transportGuid);
                packet.AddValue("Transport Position", tpos);
            }

            if (hasFallData)
            {
                packet.ReadSingle("Vertical Speed");
                packet.ReadUInt32("Fall time");

                if (hasFallDirection)
                {
                    packet.ReadSingle("Horizontal Speed");
                    packet.ReadSingle("Fall Cos");
                    packet.ReadSingle("Fall Sin");
                }
            }

            if (hasPitch)
                packet.ReadSingle("Pitch");
            if (hasSplineElev)
                packet.ReadSingle("Spline elevation");
            if (hasTime)
                packet.ReadUInt32("Timestamp");
            if (hasO)
                pos.O = packet.ReadSingle();

            packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
        }

        [Parser(Opcode.MSG_MOVE_SET_FACING)]
        public static void HandleMoveSetFacing434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();

            pos.X = packet.ReadSingle();
            pos.Y = packet.ReadSingle();
            pos.Z = packet.ReadSingle();
            guid[6] = packet.ReadBit();
            var hasTrans = packet.ReadBit("Has transport");
            guid[4] = packet.ReadBit();
            packet.ReadBit("Has Spline");
            guid[0] = packet.ReadBit();
            var hasO = !packet.ReadBit();
            var hasTime = !packet.ReadBit("Has timestamp");
            var hasPitch = !packet.ReadBit("Has pitch");
            var hasMovementFlags2 = !packet.ReadBit();
            guid[5] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            var hasFallData = packet.ReadBit("Has fall data");
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            var hasMovementFlags = !packet.ReadBit();
            guid[3] = packet.ReadBit();
            packet.ReadBit();
            guid[1] = packet.ReadBit();

            if (hasTrans)
            {
                hasTransTime3 = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
            }

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            if (hasMovementFlags2)
                packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            if (hasMovementFlags)
                packet.ReadBitsE<WowPacketParser.Enums.v4.MovementFlag>("Movement Flags", 30);

            packet.ParseBitStream(guid, 6, 7, 2, 0, 4, 1, 5, 3);

            if (hasTrans)
            {
                var tpos = new Vector4();
                packet.ReadXORByte(transportGuid, 0);
                tpos.X = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 1);

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                packet.ReadXORByte(transportGuid, 4);
                tpos.Z = packet.ReadSingle();
                tpos.O = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 2);

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                packet.ReadXORByte(transportGuid, 3);
                packet.ReadSByte("Transport Seat");
                tpos.Y = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 5);
                packet.ReadXORByte(transportGuid, 6);
                packet.ReadUInt32("Transport Time");
                packet.ReadXORByte(transportGuid, 7);

                packet.WriteGuid("Transport Guid", transportGuid);
                packet.AddValue("Transport Position", tpos);
            }

            if (hasFallData)
            {
                if (hasFallDirection)
                {
                    packet.ReadSingle("Horizontal Speed");
                    packet.ReadSingle("Fall Sin");
                    packet.ReadSingle("Fall Cos");
                }

                packet.ReadUInt32("Fall time");
                packet.ReadSingle("Vertical Speed");
            }

            if (hasSplineElev)
                packet.ReadSingle("Spline elevation");
            if (hasO)
                pos.O = packet.ReadSingle();
            if (hasTime)
                packet.ReadUInt32("Timestamp");
            if (hasPitch)
                packet.ReadSingle("Pitch");

            packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
        }

        [Parser(Opcode.MSG_MOVE_TELEPORT)]
        public static void HandleMoveTeleport434(Packet packet)
        {
            var guid = new byte[8];
            var transGuid = new byte[8];
            var pos = new Vector4();

            guid[6] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            var hasVehicle = packet.ReadBit("HasVehicle");
            if (hasVehicle)
            {
                packet.ReadBit("VehicleExitVoluntary");
                packet.ReadBit("VehicleExitTeleport");
            }

            var onTransport = packet.ReadBit("HasTransport");
            guid[1] = packet.ReadBit();
            if (onTransport)
                transGuid = packet.StartBitStream(1, 3, 2, 5, 0, 7, 6, 4);

            guid[4] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            if (onTransport)
            {
                packet.ParseBitStream(transGuid, 5, 6, 1, 7, 0, 2, 4, 3);
                packet.WriteGuid("TransportGUID", transGuid);
            }

            packet.ReadUInt32("SequenceIndex");
            packet.ReadXORByte(guid, 1);
            packet.ReadXORByte(guid, 2);
            packet.ReadXORByte(guid, 3);
            packet.ReadXORByte(guid, 5);
            pos.X = packet.ReadSingle();
            packet.ReadXORByte(guid, 4);
            pos.O = packet.ReadSingle();
            packet.ReadXORByte(guid, 7);
            pos.Z = packet.ReadSingle();
            if (hasVehicle)
                packet.ReadUInt32("VehicleSeatIndex");

            packet.ReadXORByte(guid, 0);
            packet.ReadXORByte(guid, 6);
            pos.Y = packet.ReadSingle();

            packet.AddValue("Pos", pos);
            packet.WriteGuid("MoverGUID", guid);
        }

        [Parser(Opcode.MSG_MOVE_STOP)]
        public static void HandleMoveStop434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();

            pos.X = packet.ReadSingle();
            pos.Y = packet.ReadSingle();
            pos.Z = packet.ReadSingle();
            guid[3] = packet.ReadBit();
            guid[6] = packet.ReadBit();
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            packet.ReadBit("Has Spline");
            var hasO = !packet.ReadBit();
            guid[7] = packet.ReadBit();
            var hasMovementFlags = !packet.ReadBit();
            guid[5] = packet.ReadBit();
            var hasFallData = packet.ReadBit("Has fall data");
            var hasMovementFlags2 = !packet.ReadBit();
            var hasTrans = packet.ReadBit("Has transport");
            var hasTime = !packet.ReadBit("Has timestamp");
            guid[4] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            packet.ReadBit();
            guid[2] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            var hasPitch = !packet.ReadBit("Has pitch");

            if (hasTrans)
            {
                transportGuid[7] = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
            }

            if (hasMovementFlags)
                packet.ReadBitsE<WowPacketParser.Enums.v4.MovementFlag>("Movement Flags", 30);

            if (hasMovementFlags2)
                packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            packet.ParseBitStream(guid, 6, 3, 0, 4, 2, 1, 5, 7);

            if (hasTrans)
            {
                var tpos = new Vector4();
                packet.ReadXORByte(transportGuid, 4);
                packet.ReadXORByte(transportGuid, 7);
                packet.ReadUInt32("Transport Time");
                packet.ReadSByte("Transport Seat");
                tpos.Z = packet.ReadSingle();

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                packet.ReadXORByte(transportGuid, 2);
                packet.ReadXORByte(transportGuid, 0);
                tpos.Y = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 1);
                packet.ReadXORByte(transportGuid, 3);

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                tpos.X = packet.ReadSingle();
                tpos.O = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 5);
                packet.ReadXORByte(transportGuid, 6);

                packet.WriteGuid("Transport Guid", transportGuid);
                packet.AddValue("Transport Position", tpos);
            }

            if (hasTime)
                packet.ReadUInt32("Timestamp");
            if (hasO)
                pos.O = packet.ReadSingle();
            if (hasPitch)
                packet.ReadSingle("Pitch");
            if (hasSplineElev)
                packet.ReadSingle("Spline elevation");

            if (hasFallData)
            {
                if (hasFallDirection)
                {
                    packet.ReadSingle("Fall Cos");
                    packet.ReadSingle("Fall Sin");
                    packet.ReadSingle("Horizontal Speed");
                }

                packet.ReadSingle("Vertical Speed");
                packet.ReadUInt32("Fall time");
            }

            packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
        }

        [Parser(Opcode.CMSG_MOVE_CHANGE_TRANSPORT)]
        public static void HandleMoveChngTransport434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();

            pos.Y = packet.ReadSingle();
            pos.X = packet.ReadSingle();
            pos.Z = packet.ReadSingle();
            guid[4] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            var hasTrans = packet.ReadBit("Has transport");
            packet.ReadBit("Has Spline");
            var hasO = !packet.ReadBit();
            guid[6] = packet.ReadBit();
            var hasTime = !packet.ReadBit("Has timestamp");
            guid[5] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            var hasPitch = !packet.ReadBit("Has pitch");
            packet.ReadBit();
            guid[3] = packet.ReadBit();
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            guid[1] = packet.ReadBit();
            var hasFallData = packet.ReadBit("Has fall data");
            var hasMovementFlags = !packet.ReadBit();
            var hasMovementFlags2 = !packet.ReadBit();

            if (hasTrans)
            {
                transportGuid[3] = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
            }

            if (hasMovementFlags)
                packet.ReadBitsE<WowPacketParser.Enums.v4.MovementFlag>("Movement Flags", 30);

            if (hasMovementFlags2)
                packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            packet.ParseBitStream(guid, 7, 5, 1, 2, 6, 4, 0, 3);

            if (hasTrans)
            {
                var tpos = new Vector4();

                tpos.Y = packet.ReadSingle();
                packet.ReadSByte("Transport Seat");
                packet.ReadXORByte(transportGuid, 1);
                packet.ReadXORByte(transportGuid, 0);
                packet.ReadXORByte(transportGuid, 2);
                packet.ReadXORByte(transportGuid, 3);
                packet.ReadXORByte(transportGuid, 6);
                tpos.Z = packet.ReadSingle();
                tpos.X = packet.ReadSingle();

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                packet.ReadUInt32("Transport Time");
                packet.ReadXORByte(transportGuid, 7);
                packet.ReadXORByte(transportGuid, 5);
                packet.ReadXORByte(transportGuid, 4);
                tpos.O = packet.ReadSingle();

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                packet.WriteGuid("Transport Guid", transportGuid);
                packet.AddValue("Transport Position", tpos);
            }

            if (hasFallData)
            {
                packet.ReadUInt32("Fall time");

                if (hasFallDirection)
                {
                    packet.ReadSingle("Horizontal Speed");
                    packet.ReadSingle("Fall Cos");
                    packet.ReadSingle("Fall Sin");
                }

                packet.ReadSingle("Vertical Speed");
            }

            if (hasO)
                pos.O = packet.ReadSingle();
            if (hasPitch)
                packet.ReadSingle("Pitch");
            if (hasSplineElev)
                packet.ReadSingle("Spline elevation");
            if (hasTime)
                packet.ReadUInt32("Timestamp");

            packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
        }

        [Parser(Opcode.MSG_MOVE_START_ASCEND)]
        public static void HandleMoveStartAscend434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();

            pos.X = packet.ReadSingle();
            pos.Y = packet.ReadSingle();
            pos.Z = packet.ReadSingle();
            guid[0] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            packet.ReadBit();
            guid[5] = packet.ReadBit();
            var hasTrans = packet.ReadBit("Has transport");
            guid[2] = packet.ReadBit();
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            var hasFallData = packet.ReadBit("Has fall data");
            packet.ReadBit("Has Spline");
            guid[3] = packet.ReadBit();
            var hasMovementFlags2 = !packet.ReadBit();
            guid[6] = packet.ReadBit();
            var hasMovementFlags = !packet.ReadBit();
            var hasPitch = !packet.ReadBit("Has pitch");
            var hasTime = !packet.ReadBit("Has timestamp");
            var hasO = !packet.ReadBit();
            guid[4] = packet.ReadBit();

            if (hasMovementFlags)
                packet.ReadBitsE<WowPacketParser.Enums.v4.MovementFlag>("Movement Flags", 30);

            if (hasTrans)
            {
                transportGuid[0] = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
            }

            if (hasMovementFlags2)
                packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            packet.ParseBitStream(guid, 6, 3, 1, 4, 2, 0, 5, 7);

            if (hasTrans)
            {
                var tpos = new Vector4();

                packet.ReadXORByte(transportGuid, 3);
                packet.ReadXORByte(transportGuid, 5);
                packet.ReadXORByte(transportGuid, 4);
                packet.ReadSByte("Transport Seat");
                packet.ReadXORByte(transportGuid, 2);
                tpos.O = packet.ReadSingle();
                tpos.Y = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 7);

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                tpos.Z = packet.ReadSingle();

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                packet.ReadXORByte(transportGuid, 6);
                packet.ReadUInt32("Transport Time");
                packet.ReadXORByte(transportGuid, 0);
                packet.ReadXORByte(transportGuid, 1);
                tpos.X = packet.ReadSingle();

                packet.WriteGuid("Transport Guid", transportGuid);
                packet.AddValue("Transport Position", tpos);
            }

            if (hasFallData)
            {
                if (hasFallDirection)
                {
                    packet.ReadSingle("Fall Cos");
                    packet.ReadSingle("Fall Sin");
                    packet.ReadSingle("Horizontal Speed");
                }

                packet.ReadUInt32("Fall time");
                packet.ReadSingle("Vertical Speed");
            }

            if (hasO)
                pos.O = packet.ReadSingle();
            if (hasPitch)
                packet.ReadSingle("Pitch");
            if (hasTime)
                packet.ReadUInt32("Timestamp");
            if (hasSplineElev)
                packet.ReadSingle("Spline elevation");

            packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
        }

        [Parser(Opcode.MSG_MOVE_START_DESCEND)]
        public static void HandleMoveStartDescend434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();

            pos.Y = packet.ReadSingle();
            pos.Z = packet.ReadSingle();
            pos.X = packet.ReadSingle();
            guid[0] = packet.ReadBit();
            var hasPitch = !packet.ReadBit("Has pitch");
            var hasFallData = packet.ReadBit("Has fall data");
            guid[4] = packet.ReadBit();
            var hasO = !packet.ReadBit();
            var hasTime = !packet.ReadBit("Has timestamp");
            var hasMovementFlags2 = !packet.ReadBit();
            var hasMovementFlags = !packet.ReadBit();
            guid[6] = packet.ReadBit();
            packet.ReadBit();
            guid[1] = packet.ReadBit();
            var hasTrans = packet.ReadBit("Has transport");
            packet.ReadBit("Has Spline");
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            guid[5] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            guid[2] = packet.ReadBit();

            if (hasTrans)
            {
                transportGuid[0] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
            }

            if (hasMovementFlags2)
                packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            if (hasMovementFlags)
                packet.ReadBitsE<WowPacketParser.Enums.v4.MovementFlag>("Movement Flags", 30);

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            packet.ParseBitStream(guid, 2, 7, 6, 0, 1, 5, 4, 3);

            if (hasPitch)
                packet.ReadSingle("Pitch");

            if (hasTrans)
            {
                var tpos = new Vector4();

                tpos.Z = packet.ReadSingle();
                packet.ReadSByte("Transport Seat");

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                packet.ReadXORByte(transportGuid, 6);

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                packet.ReadXORByte(transportGuid, 1);
                packet.ReadUInt32("Transport Time");
                packet.ReadXORByte(transportGuid, 4);
                tpos.Y = packet.ReadSingle();
                tpos.X = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 7);
                packet.ReadXORByte(transportGuid, 3);
                packet.ReadXORByte(transportGuid, 5);
                packet.ReadXORByte(transportGuid, 2);
                tpos.O = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 0);

                packet.WriteGuid("Transport Guid", transportGuid);
                packet.AddValue("Transport Position", tpos);
            }

            if (hasFallData)
            {
                packet.ReadUInt32("Fall time");

                if (hasFallDirection)
                {
                    packet.ReadSingle("Fall Sin");
                    packet.ReadSingle("Fall Cos");
                    packet.ReadSingle("Horizontal Speed");
                }

                packet.ReadSingle("Vertical Speed");
            }

            if (hasTime)
                packet.ReadUInt32("Timestamp");
            if (hasO)
                pos.O = packet.ReadSingle();
            if (hasSplineElev)
                packet.ReadSingle("Spline elevation");

            packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
        }

        [Parser(Opcode.MSG_MOVE_STOP_ASCEND)]
        public static void HandleMoveStopAscend434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();

            pos.Z = packet.ReadSingle();
            pos.Y = packet.ReadSingle();
            pos.X = packet.ReadSingle();
            guid[3] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            var hasFallData = packet.ReadBit("Has fall data");
            guid[7] = packet.ReadBit();
            var hasO = !packet.ReadBit();
            var hasPitch = !packet.ReadBit("Has pitch");
            packet.ReadBit("Has Spline");
            packet.ReadBit();
            guid[1] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            var hasMovementFlags = !packet.ReadBit();
            guid[0] = packet.ReadBit();
            guid[6] = packet.ReadBit();
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            var hasTrans = packet.ReadBit("Has transport");
            var hasMovementFlags2 = !packet.ReadBit();
            var hasTime = !packet.ReadBit("Has timestamp");
            guid[5] = packet.ReadBit();

            if (hasTrans)
            {
                hasTransTime2 = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
                transportGuid = packet.StartBitStream(1, 3, 2, 5, 7, 4, 6, 0);
            }

            if (hasMovementFlags2)
                packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            if (hasMovementFlags)
                packet.ReadBitsE<WowPacketParser.Enums.v4.MovementFlag>("Movement Flags", 30);

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            packet.ParseBitStream(guid, 7, 5, 4, 3, 2, 1, 0, 6);

            if (hasTrans)
            {
                var tpos = new Vector4();

                packet.ReadXORByte(transportGuid, 5);
                tpos.Y = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 7);
                packet.ReadXORByte(transportGuid, 2);
                packet.ReadXORByte(transportGuid, 6);

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                tpos.O = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 3);
                packet.ReadXORByte(transportGuid, 4);
                packet.ReadSByte("Transport Seat");
                packet.ReadXORByte(transportGuid, 1);
                packet.ReadUInt32("Transport Time");
                packet.ReadXORByte(transportGuid, 0);
                tpos.X = packet.ReadSingle();
                tpos.Z = packet.ReadSingle();

                packet.WriteGuid("Transport Guid", transportGuid);
                packet.AddValue("Transport Position", tpos);
            }

            if (hasFallData)
            {
                packet.ReadUInt32("Fall time");
                packet.ReadSingle("Vertical Speed");

                if (hasFallDirection)
                {
                    packet.ReadSingle("Fall Cos");
                    packet.ReadSingle("Fall Sin");
                    packet.ReadSingle("Horizontal Speed");
                }
            }

            if (hasPitch)
                packet.ReadSingle("Pitch");
            if (hasSplineElev)
                packet.ReadSingle("Spline elevation");
            if (hasTime)
                packet.ReadUInt32("Timestamp");
            if (hasO)
                pos.O = packet.ReadSingle();

            packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
        }

        [Parser(Opcode.MSG_MOVE_START_PITCH_DOWN)]
        public static void HandleMoveStartPitchDown434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();

            pos.X = packet.ReadSingle();
            pos.Z = packet.ReadSingle();
            pos.Y = packet.ReadSingle();
            var hasMovementFlags = !packet.ReadBit();
            guid[7] = packet.ReadBit();
            guid[6] = packet.ReadBit();
            var hasPitch = !packet.ReadBit("Has pitch");
            packet.ReadBit();
            guid[1] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            var hasTime = !packet.ReadBit("Has timestamp");
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            var hasMovementFlags2 = !packet.ReadBit();
            var hasTrans = packet.ReadBit("Has transport");
            guid[0] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            packet.ReadBit("Has Spline");
            var hasO = !packet.ReadBit();
            var hasFallData = packet.ReadBit("Has fall data");
            guid[2] = packet.ReadBit();

            if (hasTrans)
            {
                transportGuid[1] = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
            }

            if (hasMovementFlags2)
                packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            if (hasMovementFlags)
                packet.ReadBitsE<WowPacketParser.Enums.v4.MovementFlag>("Movement Flags", 30);

            packet.ParseBitStream(guid, 3, 7, 0, 5, 2, 6, 4, 1);

            if (hasTrans)
            {
                var tpos = new Vector4();

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                packet.ReadXORByte(transportGuid, 3);

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                packet.ReadXORByte(transportGuid, 5);
                packet.ReadXORByte(transportGuid, 1);
                tpos.Y = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 6);
                packet.ReadXORByte(transportGuid, 4);
                packet.ReadUInt32("Transport Time");
                tpos.Z = packet.ReadSingle();
                tpos.X = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 0);
                packet.ReadSByte("Transport Seat");
                packet.ReadXORByte(transportGuid, 2);
                packet.ReadXORByte(transportGuid, 7);
                tpos.O = packet.ReadSingle();

                packet.WriteGuid("Transport Guid", transportGuid);
                packet.AddValue("Transport Position", tpos);
            }

            if (hasFallData)
            {
                if (hasFallDirection)
                {
                    packet.ReadSingle("Fall Sin");
                    packet.ReadSingle("Horizontal Speed");
                    packet.ReadSingle("Fall Cos");
                }

                packet.ReadSingle("Vertical Speed");
                packet.ReadUInt32("Fall time");
            }

            if (hasPitch)
                packet.ReadSingle("Pitch");
            if (hasO)
                pos.O = packet.ReadSingle();
            if (hasSplineElev)
                packet.ReadSingle("Spline elevation");
            if (hasTime)
                packet.ReadUInt32("Timestamp");

            packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
        }

        [Parser(Opcode.MSG_MOVE_START_PITCH_UP)]
        public static void HandleMoveStartPitchUp434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();

            pos.Z = packet.ReadSingle();
            pos.Y = packet.ReadSingle();
            pos.X = packet.ReadSingle();
            guid[4] = packet.ReadBit();
            var hasMovementFlags = !packet.ReadBit();
            var hasMovementFlags2 = !packet.ReadBit();
            packet.ReadBit("Has Spline");
            guid[2] = packet.ReadBit();
            guid[6] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            packet.ReadBit();
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            var hasFallData = packet.ReadBit("Has fall data");
            guid[0] = packet.ReadBit();
            var hasTrans = packet.ReadBit("Has transport");
            guid[1] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            var hasPitch = !packet.ReadBit("Has pitch");
            var hasTime = !packet.ReadBit("Has timestamp");
            var hasO = !packet.ReadBit();
            guid[7] = packet.ReadBit();

            if (hasMovementFlags2)
                packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            if (hasTrans)
            {
                transportGuid[7] = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
            }

            if (hasMovementFlags)
                packet.ReadBitsE<WowPacketParser.Enums.v4.MovementFlag>("Movement Flags", 30);

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            packet.ParseBitStream(guid, 0, 3, 4, 6, 7, 1, 5, 2);

            if (hasTrans)
            {
                var tpos = new Vector4();

                tpos.X = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 1);
                packet.ReadXORByte(transportGuid, 3);
                packet.ReadXORByte(transportGuid, 2);
                packet.ReadXORByte(transportGuid, 0);
                packet.ReadUInt32("Transport Time");
                tpos.Z = packet.ReadSingle();
                tpos.O = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 7);

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                packet.ReadSByte("Transport Seat");
                packet.ReadXORByte(transportGuid, 6);
                packet.ReadXORByte(transportGuid, 5);

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                tpos.Y = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 4);

                packet.WriteGuid("Transport Guid", transportGuid);
                packet.AddValue("Transport Position", tpos);
            }

            if (hasFallData)
            {
                packet.ReadUInt32("Fall time");

                if (hasFallDirection)
                {
                    packet.ReadSingle("Fall Cos");
                    packet.ReadSingle("Fall Sin");
                    packet.ReadSingle("Horizontal Speed");
                }

                packet.ReadSingle("Vertical Speed");
            }

            if (hasO)
                pos.O = packet.ReadSingle();
            if (hasSplineElev)
                packet.ReadSingle("Spline elevation");
            if (hasPitch)
                packet.ReadSingle("Pitch");
            if (hasTime)
                packet.ReadUInt32("Timestamp");

            packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
        }

        [Parser(Opcode.MSG_MOVE_STOP_PITCH)]
        public static void HandleMoveStopPitch434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();

            pos.Y = packet.ReadSingle();
            pos.X = packet.ReadSingle();
            pos.Z = packet.ReadSingle();
            guid[0] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            var hasFallData = packet.ReadBit("Has fall data");
            guid[2] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            var hasPitch = !packet.ReadBit("Has pitch");
            packet.ReadBit("Has Spline");
            var hasO = !packet.ReadBit();
            packet.ReadBit();
            var hasMovementFlags2 = !packet.ReadBit();
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            guid[6] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            var hasTime = !packet.ReadBit("Has timestamp");
            var hasTrans = packet.ReadBit("Has transport");
            var hasMovementFlags = !packet.ReadBit();

            if (hasTrans)
            {
                transportGuid[7] = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
            }

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            if (hasMovementFlags2)
                packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            if (hasMovementFlags)
                packet.ReadBitsE<WowPacketParser.Enums.v4.MovementFlag>("Movement Flags", 30);

            packet.ParseBitStream(guid, 1, 7, 0, 6, 4, 3, 5, 2);

            if (hasTime)
                packet.ReadUInt32("Timestamp");

            if (hasFallData)
            {
                if (hasFallDirection)
                {
                    packet.ReadSingle("Horizontal Speed");
                    packet.ReadSingle("Fall Sin");
                    packet.ReadSingle("Fall Cos");
                }

                packet.ReadSingle("Vertical Speed");
                packet.ReadUInt32("Fall time");
            }

            if (hasTrans)
            {
                var tpos = new Vector4();

                packet.ReadXORByte(transportGuid, 5);
                packet.ReadXORByte(transportGuid, 2);

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                tpos.X = packet.ReadSingle();
                tpos.O = packet.ReadSingle();

                packet.ReadXORByte(transportGuid, 6);
                packet.ReadXORByte(transportGuid, 3);
                packet.ReadXORByte(transportGuid, 1);
                tpos.Z = packet.ReadSingle();

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                packet.ReadXORByte(transportGuid, 0);
                packet.ReadXORByte(transportGuid, 4);
                packet.ReadUInt32("Transport Time");
                tpos.Y = packet.ReadSingle();
                packet.ReadSByte("Transport Seat");
                packet.ReadXORByte(transportGuid, 7);

                packet.WriteGuid("Transport Guid", transportGuid);
                packet.AddValue("Transport Position", tpos);
            }

            if (hasSplineElev)
                packet.ReadSingle("Spline elevation");
            if (hasO)
                pos.O = packet.ReadSingle();
            if (hasPitch)
                packet.ReadSingle("Pitch");

            packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
        }

        [Parser(Opcode.MSG_MOVE_START_SWIM)]
        public static void HandleMoveStartSwim434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();

            pos.Z = packet.ReadSingle();
            pos.X = packet.ReadSingle();
            pos.Y = packet.ReadSingle();
            guid[3] = packet.ReadBit();
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            guid[4] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            packet.ReadBit();
            var hasPitch = !packet.ReadBit("Has pitch");
            guid[0] = packet.ReadBit();
            var hasO = !packet.ReadBit();
            var hasMovementFlags = !packet.ReadBit();
            var hasFallData = packet.ReadBit("Has fall data");
            guid[5] = packet.ReadBit();
            var hasTrans = packet.ReadBit("Has transport");
            var hasMovementFlags2 = !packet.ReadBit();
            var hasTime = !packet.ReadBit("Has timestamp");
            guid[6] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            packet.ReadBit("Has Spline");
            guid[2] = packet.ReadBit();

            if (hasTrans)
            {
                transportGuid[2] = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
            }

            if (hasMovementFlags)
                packet.ReadBitsE<WowPacketParser.Enums.v4.MovementFlag>("Movement Flags", 30);

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            if (hasMovementFlags2)
                packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            packet.ParseBitStream(guid, 0, 2, 1, 5, 4, 6, 3, 7);

            if (hasPitch)
                packet.ReadSingle("Pitch");

            if (hasTrans)
            {
                var tpos = new Vector4();

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                packet.ReadXORByte(transportGuid, 2);
                tpos.Y = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 3);
                packet.ReadUInt32("Transport Time");
                tpos.X = packet.ReadSingle();
                tpos.O = packet.ReadSingle();

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                packet.ReadXORByte(transportGuid, 6);
                packet.ReadXORByte(transportGuid, 5);
                packet.ReadXORByte(transportGuid, 4);
                tpos.Z = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 1);
                packet.ReadXORByte(transportGuid, 7);
                packet.ReadXORByte(transportGuid, 0);
                packet.ReadSByte("Transport Seat");

                packet.WriteGuid("Transport Guid", transportGuid);
                packet.AddValue("Transport Position", tpos);
            }

            if (hasFallData)
            {
                if (hasFallDirection)
                {
                    packet.ReadSingle("Fall Cos");
                    packet.ReadSingle("Horizontal Speed");
                    packet.ReadSingle("Fall Sin");
                }

                packet.ReadSingle("Vertical Speed");
                packet.ReadUInt32("Fall time");
            }

            if (hasO)
                pos.O = packet.ReadSingle();
            if (hasTime)
                packet.ReadUInt32("Timestamp");
            if (hasSplineElev)
                packet.ReadSingle("Spline elevation");

            packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
        }

        [Parser(Opcode.MSG_MOVE_STOP_SWIM)]
        public static void HandleMoveStopSwim434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();

            pos.X = packet.ReadSingle();
            pos.Y = packet.ReadSingle();
            pos.Z = packet.ReadSingle();
            packet.ReadBit("Has Spline");
            var hasPitch = !packet.ReadBit("Has pitch");
            guid[5] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            var hasMovementFlags = !packet.ReadBit();
            guid[4] = packet.ReadBit();
            var hasMovementFlags2 = !packet.ReadBit();
            guid[2] = packet.ReadBit();
            guid[6] = packet.ReadBit();
            var hasTrans = packet.ReadBit("Has transport");
            var hasO = !packet.ReadBit();
            packet.ReadBit();
            var hasTime = !packet.ReadBit("Has timestamp");
            guid[1] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            var hasFallData = packet.ReadBit("Has fall data");

            if (hasTrans)
            {
                hasTransTime3 = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
            }

            if (hasMovementFlags)
                packet.ReadBitsE<WowPacketParser.Enums.v4.MovementFlag>("Movement Flags", 30);

            if (hasMovementFlags2)
                packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            packet.ParseBitStream(guid, 0, 4, 3, 6, 7, 1, 5, 2);

            if (hasTrans)
            {
                var tpos = new Vector4();

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                packet.ReadXORByte(transportGuid, 4);
                packet.ReadUInt32("Transport Time");
                tpos.Z = packet.ReadSingle();
                packet.ReadSByte("Transport Seat");
                packet.ReadXORByte(transportGuid, 3);
                packet.ReadXORByte(transportGuid, 2);
                tpos.Y = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 7);
                packet.ReadXORByte(transportGuid, 1);
                packet.ReadXORByte(transportGuid, 6);
                packet.ReadXORByte(transportGuid, 5);

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                tpos.X = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 0);
                tpos.O = packet.ReadSingle();

                packet.WriteGuid("Transport Guid", transportGuid);
                packet.AddValue("Transport Position", tpos);
            }

            if (hasFallData)
            {
                packet.ReadSingle("Vertical Speed");

                if (hasFallDirection)
                {
                    packet.ReadSingle("Fall Cos");
                    packet.ReadSingle("Horizontal Speed");
                    packet.ReadSingle("Fall Sin");
                }

                packet.ReadUInt32("Fall time");
            }

            if (hasTime)
                packet.ReadUInt32("Timestamp");
            if (hasPitch)
                packet.ReadSingle("Pitch");
            if (hasO)
                pos.O = packet.ReadSingle();
            if (hasSplineElev)
                packet.ReadSingle("Spline elevation");

            packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
        }

        [Parser(Opcode.SMSG_MOVE_SPLINE_SET_RUN_SPEED)]
        public static void HandleSplineSetRunSpeed434(Packet packet)
        {
            var guid = packet.StartBitStream(4, 0, 5, 7, 6, 3, 1, 2);
            packet.ParseBitStream(guid, 0, 7, 6, 5, 3, 4);
            packet.ReadSingle("Speed");
            packet.ParseBitStream(guid, 2, 1);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.MSG_MOVE_FALL_LAND)]
        public static void HandleMoveFallLand434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();

            pos.X = packet.ReadSingle();
            pos.Y = packet.ReadSingle();
            pos.Z = packet.ReadSingle();
            var hasTrans = packet.ReadBit("Has transport");
            guid[7] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            var hasMovementFlags2 = !packet.ReadBit();
            guid[3] = packet.ReadBit();
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            var hasO = !packet.ReadBit();
            guid[6] = packet.ReadBit();
            var hasTime = !packet.ReadBit("Has timestamp");
            packet.ReadBit();
            var hasPitch = !packet.ReadBit("Has pitch");
            guid[4] = packet.ReadBit();
            packet.ReadBit("Has Spline");
            guid[5] = packet.ReadBit();
            var hasMovementFlags = !packet.ReadBit();
            var hasFallData = packet.ReadBit("Has fall data");
            guid[0] = packet.ReadBit();
            guid[2] = packet.ReadBit();

            if (hasTrans)
            {
                transportGuid[0] = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
            }

            if (hasMovementFlags2)
                packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            if (hasMovementFlags)
                packet.ReadBitsE<WowPacketParser.Enums.v4.MovementFlag>("Movement Flags", 30);

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            packet.ParseBitStream(guid, 1, 7, 4, 3, 6, 0, 2, 5);

            if (hasTrans)
            {
                var tpos = new Vector4();
                packet.ReadUInt32("Transport Time");
                tpos.Z = packet.ReadSingle();
                tpos.Y = packet.ReadSingle();
                tpos.X = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 5);

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                packet.ReadXORByte(transportGuid, 1);
                packet.ReadSByte("Transport Seat");

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                packet.ReadXORByte(transportGuid, 4);
                tpos.O = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 0);
                packet.ReadXORByte(transportGuid, 7);
                packet.ReadXORByte(transportGuid, 3);
                packet.ReadXORByte(transportGuid, 6);
                packet.ReadXORByte(transportGuid, 2);

                packet.WriteGuid("Transport Guid", transportGuid);
                packet.AddValue("Transport Position", tpos);
            }

            if (hasFallData)
            {
                packet.ReadSingle("Vertical Speed");
                packet.ReadUInt32("Fall time");

                if (hasFallDirection)
                {
                    packet.ReadSingle("Fall Cos");
                    packet.ReadSingle("Horizontal Speed");
                    packet.ReadSingle("Fall Sin");
                }
            }

            if (hasSplineElev)
                packet.ReadSingle("Spline elevation");
            if (hasTime)
                packet.ReadUInt32("Timestamp");
            if (hasPitch)
                packet.ReadSingle("Pitch");
            if (hasO)
                pos.O = packet.ReadSingle();

            packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
        }

        [Parser(Opcode.MSG_MOVE_JUMP)]
        public static void HandleMoveJump434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();

            pos.Y = packet.ReadSingle();
            pos.X = packet.ReadSingle();
            pos.Z = packet.ReadSingle();
            var hasTime = !packet.ReadBit("Has timestamp");
            guid[5] = packet.ReadBit();
            var hasMovementFlags = !packet.ReadBit();
            guid[4] = packet.ReadBit();
            guid[6] = packet.ReadBit();
            var hasMovementFlags2 = !packet.ReadBit();
            guid[0] = packet.ReadBit();
            var hasTrans = packet.ReadBit("Has transport");
            guid[3] = packet.ReadBit();
            var hasPitch = !packet.ReadBit("Has pitch");
            guid[7] = packet.ReadBit();
            var hasFallData = packet.ReadBit("Has fall data");
            packet.ReadBit("Has Spline");
            var hasO = !packet.ReadBit();
            guid[1] = packet.ReadBit();
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            packet.ReadBit();
            guid[2] = packet.ReadBit();

            if (hasTrans)
            {
                transportGuid[3] = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
            }

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            if (hasMovementFlags)
                packet.ReadBitsE<WowPacketParser.Enums.v4.MovementFlag>("Movement Flags", 30);

            if (hasMovementFlags2)
                packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            packet.ParseBitStream(guid, 6, 5, 4, 0, 2, 3, 7, 1);

            if (hasTrans)
            {
                var tpos = new Vector4();
                packet.ReadXORByte(transportGuid, 1);
                packet.ReadXORByte(transportGuid, 7);
                tpos.X = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 4);

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                packet.ReadXORByte(transportGuid, 0);
                tpos.O = packet.ReadSingle();

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                packet.ReadUInt32("Transport Time");

                packet.ReadXORByte(transportGuid, 6);
                tpos.Z = packet.ReadSingle();
                packet.ReadSByte("Transport Seat");
                tpos.Y = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 3);
                packet.ReadXORByte(transportGuid, 2);
                packet.ReadXORByte(transportGuid, 5);

                packet.WriteGuid("Transport Guid", transportGuid);
                packet.AddValue("Transport Position", tpos);
            }

            if (hasPitch)
                packet.ReadSingle("Pitch");
            if (hasTime)
                packet.ReadUInt32("Timestamp");

            if (hasFallData)
            {
                if (hasFallDirection)
                {
                    packet.ReadSingle("Fall Cos");
                    packet.ReadSingle("Fall Sin");
                    packet.ReadSingle("Horizontal Speed");
                }

                packet.ReadSingle("Vertical Speed");
                packet.ReadUInt32("Fall time");
            }

            if (hasSplineElev)
                packet.ReadSingle("Spline elevation");
            if (hasO)
                pos.O = packet.ReadSingle();

            packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
        }

        [Parser(Opcode.MSG_MOVE_START_STRAFE_LEFT)]
        public static void HandleMoveStartStrafeLeft434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();

            pos.Z = packet.ReadSingle();
            pos.X = packet.ReadSingle();
            pos.Y = packet.ReadSingle();
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            guid[5] = packet.ReadBit();
            var hasPitch = !packet.ReadBit("Has pitch");
            guid[6] = packet.ReadBit();
            var hasTime = !packet.ReadBit("Has timestamp");
            guid[1] = packet.ReadBit();
            packet.ReadBit();
            guid[4] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            var hasFallData = packet.ReadBit("Has fall data");
            var hasO = !packet.ReadBit();
            guid[3] = packet.ReadBit();
            var hasMovementFlags2 = !packet.ReadBit();
            guid[7] = packet.ReadBit();
            packet.ReadBit("Has Spline");
            var hasMovementFlags = !packet.ReadBit();
            var hasTrans = packet.ReadBit("Has transport");

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            if (hasTrans)
            {
                hasTransTime2 = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
            }

            if (hasMovementFlags)
                packet.ReadBitsE<WowPacketParser.Enums.v4.MovementFlag>("Movement Flags", 30);

            if (hasMovementFlags2)
                packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            packet.ParseBitStream(guid, 2, 6, 3, 1, 0, 7, 4, 5);

            if (hasFallData)
            {
                if (hasFallDirection)
                {
                    packet.ReadSingle("Fall Sin");
                    packet.ReadSingle("Horizontal Speed");
                    packet.ReadSingle("Fall Cos");
                }

                packet.ReadUInt32("Fall time");
                packet.ReadSingle("Vertical Speed");
            }

            if (hasTrans)
            {
                var tpos = new Vector4();
                packet.ReadSByte("Transport Seat");
                packet.ReadXORByte(transportGuid, 2);

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                packet.ReadXORByte(transportGuid, 3);
                tpos.Z = packet.ReadSingle();

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                packet.ReadXORByte(transportGuid, 0);
                packet.ReadXORByte(transportGuid, 7);
                tpos.Y = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 5);
                packet.ReadXORByte(transportGuid, 1);
                tpos.O = packet.ReadSingle();
                packet.ReadUInt32("Transport Time");
                packet.ReadXORByte(transportGuid, 6);
                packet.ReadXORByte(transportGuid, 4);
                tpos.X = packet.ReadSingle();

                packet.WriteGuid("Transport Guid", transportGuid);
                packet.AddValue("Transport Position", tpos);
            }

            if (hasTime)
                packet.ReadUInt32("Timestamp");
            if (hasO)
                pos.O = packet.ReadSingle();
            if (hasPitch)
                packet.ReadSingle("Pitch");
            if (hasSplineElev)
                packet.ReadSingle("Spline elevation");

            packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
        }

        [Parser(Opcode.MSG_MOVE_START_STRAFE_RIGHT)]
        public static void HandleMoveStartStrafeRight434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();

            pos.Y = packet.ReadSingle();
            pos.X = packet.ReadSingle();
            pos.Z = packet.ReadSingle();
            var hasPitch = !packet.ReadBit("Has pitch");
            guid[1] = packet.ReadBit();
            var hasO = !packet.ReadBit();
            guid[4] = packet.ReadBit();
            packet.ReadBit("Has Spline");
            packet.ReadBit();
            guid[5] = packet.ReadBit();
            var hasFallData = packet.ReadBit("Has fall data");
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            var hasTime = !packet.ReadBit("Has timestamp");
            var hasMovementFlags = !packet.ReadBit();
            guid[2] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            guid[6] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            var hasMovementFlags2 = !packet.ReadBit();
            var hasTrans = packet.ReadBit("Has transport");
            guid[0] = packet.ReadBit();

            if (hasTrans)
            {
                transportGuid[7] = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
            }

            if (hasMovementFlags2)
                packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            if (hasMovementFlags)
                packet.ReadBitsE<WowPacketParser.Enums.v4.MovementFlag>("Movement Flags", 30);

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            packet.ParseBitStream(guid, 7, 5, 3, 1, 2, 4, 6, 0);

            if (hasTrans)
            {
                var tpos = new Vector4();
                packet.ReadXORByte(transportGuid, 5);
                packet.ReadXORByte(transportGuid, 1);
                packet.ReadXORByte(transportGuid, 6);
                tpos.Y = packet.ReadSingle();
                tpos.O = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 0);
                packet.ReadXORByte(transportGuid, 2);
                packet.ReadSByte("Transport Seat");
                tpos.X = packet.ReadSingle();

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                packet.ReadUInt32("Transport Time");
                packet.ReadXORByte(transportGuid, 4);
                packet.ReadXORByte(transportGuid, 7);

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                tpos.Z = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 3);

                packet.WriteGuid("Transport Guid", transportGuid);
                packet.AddValue("Transport Position", tpos);
            }

            if (hasPitch)
                packet.ReadSingle("Pitch");
            if (hasO)
                pos.O = packet.ReadSingle();

            if (hasFallData)
            {
                if (hasFallDirection)
                {
                    packet.ReadSingle("Fall Cos");
                    packet.ReadSingle("Fall Sin");
                    packet.ReadSingle("Horizontal Speed");
                }

                packet.ReadUInt32("Fall time");
                packet.ReadSingle("Vertical Speed");
            }

            if (hasTime)
                packet.ReadUInt32("Timestamp");
            if (hasSplineElev)
                packet.ReadSingle("Spline elevation");

            packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
        }

        [Parser(Opcode.MSG_MOVE_STOP_STRAFE)]
        public static void HandleMoveStopStrafe434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();

            pos.Y = packet.ReadSingle();
            pos.Z = packet.ReadSingle();
            pos.X = packet.ReadSingle();
            var hasPitch = !packet.ReadBit("Has pitch");
            var hasTime = !packet.ReadBit("Has timestamp");
            guid[2] = packet.ReadBit();
            var hasFallData = packet.ReadBit("Has fall data");
            guid[7] = packet.ReadBit();
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            guid[3] = packet.ReadBit();
            var hasO = !packet.ReadBit();
            var hasMovementFlags2 = !packet.ReadBit();
            var hasTrans = packet.ReadBit("Has transport");
            var hasMovementFlags = !packet.ReadBit();
            packet.ReadBit("Has Spline");
            guid[0] = packet.ReadBit();
            packet.ReadBit();
            guid[6] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            guid[4] = packet.ReadBit();

            if (hasTrans)
            {
                transportGuid[7] = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
            }

            if (hasMovementFlags)
                packet.ReadBitsE<WowPacketParser.Enums.v4.MovementFlag>("Movement Flags", 30);

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            if (hasMovementFlags2)
                packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            packet.ParseBitStream(guid, 2, 7, 3, 4, 5, 6, 1, 0);

            if (hasTrans)
            {
                var tpos = new Vector4();
                packet.ReadSByte("Transport Seat");
                packet.ReadXORByte(transportGuid, 6);
                tpos.Z = packet.ReadSingle();

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                packet.ReadXORByte(transportGuid, 1);
                packet.ReadXORByte(transportGuid, 3);
                packet.ReadXORByte(transportGuid, 2);
                packet.ReadXORByte(transportGuid, 4);
                packet.ReadXORByte(transportGuid, 5);
                packet.ReadUInt32("Transport Time");
                tpos.O = packet.ReadSingle();
                tpos.X = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 0);
                tpos.Y = packet.ReadSingle();

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                packet.ReadXORByte(transportGuid, 7);

                packet.WriteGuid("Transport Guid", transportGuid);
                packet.AddValue("Transport Position", tpos);
            }

            if (hasFallData)
            {
                if (hasFallDirection)
                {
                    packet.ReadSingle("Fall Cos");
                    packet.ReadSingle("Horizontal Speed");
                    packet.ReadSingle("Fall Sin");
                }

                packet.ReadUInt32("Fall time");
                packet.ReadSingle("Vertical Speed");
            }

            if (hasSplineElev)
                packet.ReadSingle("Spline elevation");
            if (hasO)
                pos.O = packet.ReadSingle();
            if (hasPitch)
                packet.ReadSingle("Pitch");
            if (hasTime)
                packet.ReadUInt32("Timestamp");

            packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
        }

        [Parser(Opcode.MSG_MOVE_START_BACKWARD)]
        public static void HandleMoveStartBackward434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();

            pos.X = packet.ReadSingle();
            pos.Z = packet.ReadSingle();
            pos.Y = packet.ReadSingle();
            var hasTrans = packet.ReadBit("Has transport");
            guid[3] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            var hasTime = !packet.ReadBit("Has timestamp");
            guid[7] = packet.ReadBit();
            var hasPitch = !packet.ReadBit("Has pitch");
            packet.ReadBit();
            var hasMovementFlags = !packet.ReadBit();
            var hasO = !packet.ReadBit();
            packet.ReadBit("Has Spline");
            var hasMovementFlags2 = !packet.ReadBit();
            var hasFallData = packet.ReadBit("Has fall data");
            guid[5] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            guid[6] = packet.ReadBit();
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");

            if (hasTrans)
            {
                transportGuid[3] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
            }

            if (hasMovementFlags)
                packet.ReadBitsE<WowPacketParser.Enums.v4.MovementFlag>("Movement Flags", 30);

            if (hasMovementFlags2)
                packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            packet.ParseBitStream(guid, 6, 7, 4, 1, 5, 0, 2, 3);

            if (hasTrans)
            {
                var tpos = new Vector4();
                tpos.Z = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 2);

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                packet.ReadXORByte(transportGuid, 0);
                packet.ReadXORByte(transportGuid, 5);
                tpos.Y = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 1);
                tpos.X = packet.ReadSingle();

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                packet.ReadXORByte(transportGuid, 4);
                tpos.O = packet.ReadSingle();
                packet.ReadSByte("Transport Seat");
                packet.ReadXORByte(transportGuid, 7);
                packet.ReadUInt32("Transport Time");
                packet.ReadXORByte(transportGuid, 6);
                packet.ReadXORByte(transportGuid, 3);

                packet.WriteGuid("Transport Guid", transportGuid);
                packet.AddValue("Transport Position", tpos);
            }

            if (hasPitch)
                packet.ReadSingle("Pitch");
            if (hasTime)
                packet.ReadUInt32("Timestamp");

            if (hasFallData)
            {
                if (hasFallDirection)
                {
                    packet.ReadSingle("Horizontal Speed");
                    packet.ReadSingle("Fall Sin");
                    packet.ReadSingle("Fall Cos");
                }

                packet.ReadSingle("Vertical Speed");
                packet.ReadUInt32("Fall time");
            }

            if (hasO)
                pos.O = packet.ReadSingle();
            if (hasSplineElev)
                packet.ReadSingle("Spline elevation");

            packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
        }

        [Parser(Opcode.MSG_MOVE_START_TURN_LEFT)]
        public static void HandleMoveStartTurnLeft434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();

            pos.Y = packet.ReadSingle();
            pos.X = packet.ReadSingle();
            pos.Z = packet.ReadSingle();
            packet.ReadBit();
            guid[1] = packet.ReadBit();
            var hasO = !packet.ReadBit();
            packet.ReadBit("Has Spline");
            var hasMovementFlags = !packet.ReadBit();
            guid[4] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            var hasMovementFlags2 = !packet.ReadBit();
            guid[5] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            var hasTrans = packet.ReadBit("Has transport");
            guid[6] = packet.ReadBit();
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            guid[0] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            var hasPitch = !packet.ReadBit("Has pitch");
            var hasTime = !packet.ReadBit("Has timestamp");
            var hasFallData = packet.ReadBit("Has fall data");

            if (hasMovementFlags2)
                packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            if (hasTrans)
            {
                transportGuid[3] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
            }

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            if (hasMovementFlags)
                packet.ReadBitsE<WowPacketParser.Enums.v4.MovementFlag>("Movement Flags", 30);

            packet.ParseBitStream(guid, 0, 4, 7, 5, 6, 3, 2, 1);

            if (hasFallData)
            {
                if (hasFallDirection)
                {
                    packet.ReadSingle("Fall Sin");
                    packet.ReadSingle("Fall Cos");
                    packet.ReadSingle("Horizontal Speed");
                }

                packet.ReadSingle("Vertical Speed");
                packet.ReadUInt32("Fall time");
            }

            if (hasTrans)
            {
                var tpos = new Vector4();
                packet.ReadXORByte(transportGuid, 0);
                tpos.X = packet.ReadSingle();
                packet.ReadUInt32("Transport Time");
                packet.ReadSByte("Transport Seat");
                tpos.Z = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 4);
                tpos.O = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 2);
                packet.ReadXORByte(transportGuid, 6);
                packet.ReadXORByte(transportGuid, 1);
                packet.ReadXORByte(transportGuid, 3);
                tpos.Y = packet.ReadSingle();

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                packet.ReadXORByte(transportGuid, 5);
                packet.ReadXORByte(transportGuid, 7);

                packet.WriteGuid("Transport Guid", transportGuid);
                packet.AddValue("Transport Position", tpos);
            }

            if (hasTime)
                packet.ReadUInt32("Timestamp");
            if (hasPitch)
                packet.ReadSingle("Pitch");
            if (hasO)
                pos.O = packet.ReadSingle();
            if (hasSplineElev)
                packet.ReadSingle("Spline elevation");

            packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
        }

        [Parser(Opcode.MSG_MOVE_START_TURN_RIGHT)]
        public static void HandleMoveStartTurnRight434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();

            pos.X = packet.ReadSingle();
            pos.Z = packet.ReadSingle();
            pos.Y = packet.ReadSingle();
            guid[3] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            var hasMovementFlags = !packet.ReadBit();
            packet.ReadBit("Has Spline");
            guid[0] = packet.ReadBit();
            var hasO = !packet.ReadBit();
            var hasTrans = packet.ReadBit("Has transport");
            guid[7] = packet.ReadBit();
            packet.ReadBit();
            var hasMovementFlags2 = !packet.ReadBit();
            guid[1] = packet.ReadBit();
            var hasTime = !packet.ReadBit("Has timestamp");
            guid[6] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            var hasPitch = !packet.ReadBit("Has pitch");
            var hasFallData = packet.ReadBit("Has fall data");

            if (hasTrans)
            {
                transportGuid[1] = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
            }

            if (hasMovementFlags2)
                packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            if (hasMovementFlags)
                packet.ReadBitsE<WowPacketParser.Enums.v4.MovementFlag>("Movement Flags", 30);

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            packet.ParseBitStream(guid, 5, 0, 7, 3, 2, 1, 4, 6);

            if (hasTrans)
            {
                var tpos = new Vector4();
                tpos.Y = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 0);
                packet.ReadXORByte(transportGuid, 4);
                packet.ReadXORByte(transportGuid, 1);
                packet.ReadXORByte(transportGuid, 6);
                packet.ReadXORByte(transportGuid, 2);
                packet.ReadSByte("Transport Seat");
                tpos.O = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 5);

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                tpos.Z = packet.ReadSingle();
                tpos.X = packet.ReadSingle();
                packet.ReadUInt32("Transport Time");
                packet.ReadXORByte(transportGuid, 7);
                packet.ReadXORByte(transportGuid, 3);

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                packet.WriteGuid("Transport Guid", transportGuid);
                packet.AddValue("Transport Position", tpos);
            }

            if (hasFallData)
            {
                if (hasFallDirection)
                {
                    packet.ReadSingle("Horizontal Speed");
                    packet.ReadSingle("Fall Sin");
                    packet.ReadSingle("Fall Cos");
                }

                packet.ReadUInt32("Fall time");
                packet.ReadSingle("Vertical Speed");
            }

            if (hasPitch)
                packet.ReadSingle("Pitch");
            if (hasO)
                pos.O = packet.ReadSingle();
            if (hasSplineElev)
                packet.ReadSingle("Spline elevation");
            if (hasTime)
                packet.ReadUInt32("Timestamp");

            packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
        }

        [Parser(Opcode.MSG_MOVE_STOP_TURN)]
        public static void HandleMoveStopTurn434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();

            pos.X = packet.ReadSingle();
            pos.Z = packet.ReadSingle();
            pos.Y = packet.ReadSingle();
            guid[5] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            var hasFallData = packet.ReadBit("Has fall data");
            packet.ReadBit();
            guid[1] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            packet.ReadBit("Has Spline");
            var hasMovementFlags = !packet.ReadBit();
            guid[2] = packet.ReadBit();
            guid[6] = packet.ReadBit();
            var hasPitch = !packet.ReadBit("Has pitch");
            var hasTrans = packet.ReadBit("Has transport");
            guid[3] = packet.ReadBit();
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            var hasTime = !packet.ReadBit("Has timestamp");
            var hasMovementFlags2 = !packet.ReadBit();
            var hasO = !packet.ReadBit();
            guid[7] = packet.ReadBit();

            if (hasTrans)
            {
                transportGuid[2] = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
            }

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            if (hasMovementFlags)
                packet.ReadBitsE<WowPacketParser.Enums.v4.MovementFlag>("Movement Flags", 30);

            if (hasMovementFlags2)
                packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            packet.ParseBitStream(guid, 3, 2, 6, 4, 0, 7, 1, 5);

            if (hasSplineElev)
                packet.ReadSingle("Spline elevation");

            if (hasTrans)
            {
                var tpos = new Vector4();
                tpos.X = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 5);
                packet.ReadSByte("Transport Seat");
                packet.ReadXORByte(transportGuid, 2);
                packet.ReadXORByte(transportGuid, 3);
                tpos.O = packet.ReadSingle();

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                packet.ReadXORByte(transportGuid, 7);
                packet.ReadXORByte(transportGuid, 1);
                packet.ReadXORByte(transportGuid, 0);
                packet.ReadXORByte(transportGuid, 4);
                tpos.Y = packet.ReadSingle();
                tpos.Z = packet.ReadSingle();

                packet.ReadUInt32("Transport Time");
                packet.ReadXORByte(transportGuid, 6);

                packet.WriteGuid("Transport Guid", transportGuid);
                packet.AddValue("Transport Position", tpos);
            }

            if (hasFallData)
            {
                packet.ReadUInt32("Fall time");
                if (hasFallDirection)
                {
                    packet.ReadSingle("Horizontal Speed");
                    packet.ReadSingle("Fall Cos");
                    packet.ReadSingle("Fall Sin");
                }

                packet.ReadSingle("Vertical Speed");
            }

            if (hasTime)
                packet.ReadUInt32("Timestamp");
            if (hasPitch)
                packet.ReadSingle("Pitch");
            if (hasO)
                pos.O = packet.ReadSingle();

            packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
        }

        [Parser(Opcode.CMSG_SET_ACTIVE_MOVER)]
        public static void HandleSetActiveMover434(Packet packet)
        {
            var guid = packet.StartBitStream(7, 2, 1, 0, 4, 5, 6, 3);
            packet.ParseBitStream(guid, 3, 2, 4, 0, 5, 1, 6, 7);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_PHASE_SHIFT_CHANGE)]
        public static void HandlePhaseShift434(Packet packet)
        {
            var phaseShift = packet.Holder.PhaseShift = new PacketPhaseShift();
            CoreParsers.MovementHandler.ClearPhases();

            var guid = packet.StartBitStream(2, 3, 1, 6, 4, 5, 0, 7);

            packet.ReadXORByte(guid, 7);
            packet.ReadXORByte(guid, 4);

            var uiWorldMapAreaIDSwapsCount = packet.ReadUInt32("UiWorldMapAreaIDSwap") / 2;
            for (var i = 0; i < uiWorldMapAreaIDSwapsCount; ++i)
                phaseShift.UiMapPhase.Add((uint)packet.ReadInt16("UiWorldMapAreaIDSwaps", i));

            packet.ReadXORByte(guid, 1);

            packet.ReadUInt32("PhaseShiftFlags");

            packet.ReadXORByte(guid, 2);
            packet.ReadXORByte(guid, 6);

            var preloadMapIDCount = packet.ReadUInt32("PreloadMapIDsCount") / 2;
            for (var i = 0; i < preloadMapIDCount; ++i)
                phaseShift.PreloadMaps.Add((uint)packet.ReadInt16<MapId>("PreloadMapID", i));

            var count = packet.ReadUInt32("PhaseShiftCount") / 2;
            for (var i = 0; i < count; ++i)
            {
                var id = packet.ReadUInt16("Id", i);
                phaseShift.Phases.Add(id);
                CoreParsers.MovementHandler.ActivePhases.Add(id, true);
            }

            if (DBC.Phases.Any())
            {
                foreach (var phaseGroup in DBC.GetPhaseGroups(CoreParsers.MovementHandler.ActivePhases.Keys))
                    packet.WriteLine($"PhaseGroup: { phaseGroup } Phases: { string.Join(" - ", DBC.Phases[phaseGroup]) }");
            }

            packet.ReadXORByte(guid, 3);
            packet.ReadXORByte(guid, 0);

            var visibleMapIDsCount = packet.ReadUInt32("VisibleMapIDsCount") / 2;
            for (var i = 0; i < visibleMapIDsCount; ++i)
                phaseShift.VisibleMaps.Add((uint)packet.ReadInt16<MapId>("VisibleMapID", i));

            packet.ReadXORByte(guid, 5);
            phaseShift.Client = packet.WriteGuid("Client", guid);

            CoreParsers.MovementHandler.WritePhaseChanges(packet);
        }

        [Parser(Opcode.SMSG_TRANSFER_PENDING)]
        public static void HandleTransferPending434(Packet packet)
        {
            // s_customLoadScreenSpellID
            var customLoadScreenSpell = packet.ReadBit("HasTransferSpellID");
            var hasTransport = packet.ReadBit("HasShip");
            if (hasTransport)
            {
                packet.ReadInt32<MapId>("ShipOriginMapID");
                packet.ReadInt32("ShipID");
            }

            if (customLoadScreenSpell)
                packet.ReadUInt32<SpellId>("TransferSpellID");

            packet.ReadInt32<MapId>("MapID");
        }

        [Parser(Opcode.CMSG_MOVE_TIME_SKIPPED)]
        public static void HandleMoveTimeSkipped434(Packet packet)
        {
            packet.ReadUInt32("Time");
            var guid = packet.StartBitStream(5, 1, 3, 7, 6, 0, 4, 2);
            packet.ParseBitStream(guid, 7, 1, 2, 4, 3, 6, 0, 5);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SPLINE_SET_FLIGHT_SPEED)]
        public static void HandleSplineSetFlightSpeed434(Packet packet)
        {
            var guid = packet.StartBitStream(7, 4, 0, 1, 3, 6, 5, 2);
            packet.ParseBitStream(guid, 0, 5, 4, 7, 3, 2, 1, 6);
            packet.ReadSingle("Speed");
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SPLINE_SET_SWIM_SPEED)]
        public static void HandleSplineSetSwimSpeed434(Packet packet)
        {
            var guid = packet.StartBitStream(4, 2, 5, 0, 7, 6, 3, 1);
            packet.ReadXORByte(guid, 5);
            packet.ReadXORByte(guid, 6);
            packet.ReadXORByte(guid, 1);
            packet.ReadXORByte(guid, 0);
            packet.ReadXORByte(guid, 2);
            packet.ReadXORByte(guid, 4);
            packet.ReadSingle("Speed");
            packet.ReadXORByte(guid, 7);
            packet.ReadXORByte(guid, 3);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SPLINE_SET_WALK_BACK_SPEED)]
        public static void HandleSplineSetWalkSpeed434(Packet packet)
        {
            var guid = packet.StartBitStream(0, 6, 7, 3, 5, 1, 2, 4);
            packet.ReadXORByte(guid, 0);
            packet.ReadXORByte(guid, 4);
            packet.ReadXORByte(guid, 7);
            packet.ReadXORByte(guid, 1);
            packet.ReadXORByte(guid, 5);
            packet.ReadXORByte(guid, 3);
            packet.ReadSingle("Speed");
            packet.ReadXORByte(guid, 6);
            packet.ReadXORByte(guid, 2);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SPLINE_SET_RUN_BACK_SPEED)]
        public static void HandleSplineSetRunBackSpeed434(Packet packet)
        {
            var guid = packet.StartBitStream(1, 2, 6, 0, 3, 7, 5, 4);
            packet.ReadXORByte(guid, 1);
            packet.ReadSingle("Speed");
            packet.ReadXORByte(guid, 2);
            packet.ReadXORByte(guid, 4);
            packet.ReadXORByte(guid, 0);
            packet.ReadXORByte(guid, 3);
            packet.ReadXORByte(guid, 6);
            packet.ReadXORByte(guid, 5);
            packet.ReadXORByte(guid, 7);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.MSG_MOVE_START_FORWARD)]
        public static void HandleMoveStartForward434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();

            pos.Y = packet.ReadSingle();
            pos.Z = packet.ReadSingle();
            pos.X = packet.ReadSingle();
            guid[5] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            packet.ReadBit();
            var hasMovementFlags = !packet.ReadBit();
            guid[7] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            var hasO = !packet.ReadBit();
            guid[6] = packet.ReadBit();
            packet.ReadBit("Has Spline");
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            guid[4] = packet.ReadBit();
            var hasTrans = packet.ReadBit("Has transport");
            var hasTime = !packet.ReadBit("Has timestamp");
            var hasPitch = !packet.ReadBit("Has pitch");
            var hasMovementFlags2 = !packet.ReadBit();
            var hasFallData = packet.ReadBit("Has fall data");

            if (hasMovementFlags)
                packet.ReadBitsE<WowPacketParser.Enums.v4.MovementFlag>("Movement Flags", 30);

            if (hasTrans)
            {
                transportGuid = packet.StartBitStream(3, 4, 6, 2, 5, 0, 7, 1);
                hasTransTime3 = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
            }

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            if (hasMovementFlags2)
                packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            packet.ParseBitStream(guid, 2, 4, 6, 1, 7, 3, 5, 0);

            if (hasFallData)
            {
                packet.ReadSingle("Vertical Speed");
                if (hasFallDirection)
                {
                    packet.ReadSingle("Horizontal Speed");
                    packet.ReadSingle("Fall Cos");
                    packet.ReadSingle("Fall Sin");
                }

                packet.ReadUInt32("Fall time");
            }

            if (hasTrans)
            {
                var tpos = new Vector4();
                packet.ReadXORByte(transportGuid, 3);
                tpos.Y = packet.ReadSingle();
                tpos.Z = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 1);
                packet.ReadXORByte(transportGuid, 4);
                packet.ReadXORByte(transportGuid, 7);
                tpos.O = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 2);
                tpos.X = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 5);
                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                packet.ReadUInt32("Transport Time");
                packet.ReadXORByte(transportGuid, 6);
                packet.ReadXORByte(transportGuid, 0);
                packet.ReadSByte("Transport Seat");
                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                packet.WriteGuid("Transport Guid", transportGuid);
                packet.AddValue("Transport Position", tpos);
            }

            if (hasSplineElev)
                packet.ReadSingle("Spline elevation");
            if (hasPitch)
                packet.ReadSingle("Pitch");
            if (hasO)
                pos.O = packet.ReadSingle();
            if (hasTime)
                packet.ReadUInt32("Timestamp");

            packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
        }

        [Parser(Opcode.SMSG_MOVE_UPDATE)]
        public static void HandlePlayerMove434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasFallDirection = false;
            var pos = new Vector4();

            var hasFallData = packet.ReadBit();
            guid[3] = packet.ReadBit();
            guid[6] = packet.ReadBit();
            var hasMovementFlagsExtra = !packet.ReadBit();
            packet.ReadBit("Has spline data");
            var hasTime = !packet.ReadBit();
            guid[0] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            if (hasMovementFlagsExtra)
                packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            guid[7] = packet.ReadBit();
            var hasMovementFlags = !packet.ReadBit();
            var hasO = !packet.ReadBit();
            guid[2] = packet.ReadBit();
            var hasSplineElevation = !packet.ReadBit();
            packet.ReadBit();
            guid[4] = packet.ReadBit();
            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            guid[5] = packet.ReadBit();
            var hasTransport = packet.ReadBit();
            if (hasMovementFlags)
                packet.ReadBitsE<WowPacketParser.Enums.v4.MovementFlag>("Movement flags", 30);

            var hasTransportTime3 = false;
            var hasTransportTime2 = false;
            if (hasTransport)
            {
                transportGuid[3] = packet.ReadBit();
                hasTransportTime3 = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                hasTransportTime2 = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
            }

            var hasPitch = !packet.ReadBit();
            packet.ReadXORByte(guid, 5);
            if (hasFallData)
            {
                if (hasFallDirection)
                {
                    packet.ReadSingle("Horizontal speed");
                    packet.ReadSingle("Fall Cos");
                    packet.ReadSingle("Fall Sin");
                }

                packet.ReadSingle("Vertical speed");
                packet.ReadUInt32("Fall time");
            }

            if (hasSplineElevation)
                packet.ReadSingle("Spline elevation");

            packet.ReadXORByte(guid, 7);
            pos.Y = packet.ReadSingle();
            packet.ReadXORByte(guid, 3);
            if (hasTransport)
            {
                var tpos = new Vector4();
                if (hasTransportTime3)
                    packet.ReadUInt32("Transport Time 3");

                packet.ReadXORByte(transportGuid, 6);
                packet.ReadSByte("Transport Seat");
                packet.ReadXORByte(transportGuid, 5);
                tpos.X = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 1);
                tpos.O = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 2);
                if (hasTransportTime2)
                    packet.ReadUInt32("Transport Time 2");

                packet.ReadXORByte(transportGuid, 0);
                tpos.Z = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 7);
                packet.ReadXORByte(transportGuid, 4);
                packet.ReadXORByte(transportGuid, 3);
                tpos.Y = packet.ReadSingle();
                packet.ReadUInt32("Transport Time");

                packet.WriteGuid("Transport Guid", transportGuid);
                packet.AddValue("Transport Position", tpos);
            }

            packet.ReadXORByte(guid, 4);
            pos.X = packet.ReadSingle();
            packet.ReadXORByte(guid, 6);
            pos.Z = packet.ReadSingle();
            if (hasTime)
                packet.ReadUInt32("Timestamp");

            packet.ReadXORByte(guid, 2);
            if (hasPitch)
                packet.ReadSingle("Pitch");

            packet.ReadXORByte(guid, 0);
            if (hasO)
                pos.O = packet.ReadSingle();

            packet.ReadXORByte(guid, 1);
            packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
        }

        [Parser(Opcode.SMSG_MOVE_SET_COLLISION_HEIGHT)]
        public static void HandleSetCollisionHeight434(Packet packet)
        {
            packet.ReadBitsE<SetCollisionHeightReason>("Reason", 2);
            var guid = packet.StartBitStream(6, 1, 4, 7, 5, 2, 0, 3);

            packet.ReadXORByte(guid, 6);
            packet.ReadXORByte(guid, 0);
            packet.ReadXORByte(guid, 4);
            packet.ReadXORByte(guid, 3);
            packet.ReadXORByte(guid, 5);
            packet.ReadUInt32("SequenceIndex");
            packet.ReadXORByte(guid, 1);
            packet.ReadXORByte(guid, 2);
            packet.ReadXORByte(guid, 7);
            packet.ReadSingle("Height");
            packet.WriteGuid("MoverGUID", guid);
        }

        [Parser(Opcode.MSG_MOVE_SET_RUN_MODE)]
        public static void HandleMoveSetRunMode434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();

            pos.Y = packet.ReadSingle();
            pos.X = packet.ReadSingle();
            pos.Z = packet.ReadSingle();
            var hasTime = !packet.ReadBit("Has timestamp");
            var hasMovementFlags2 = !packet.ReadBit();
            guid[1] = packet.ReadBit();
            packet.ReadBit("Has Spline");
            var hasMovementFlags = !packet.ReadBit();
            guid[7] = packet.ReadBit();
            var hasTrans = packet.ReadBit("Has transport");
            packet.ReadBit();
            guid[0] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            guid[5] = packet.ReadBit();
            var hasPitch = !packet.ReadBit("Has pitch");
            guid[6] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            var hasFallData = packet.ReadBit("Has fall data");
            var hasO = !packet.ReadBit();
            guid[2] = packet.ReadBit();

            if (hasTrans)
            {
                transportGuid[6] = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
            }

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            if (hasMovementFlags2)
                packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            if (hasMovementFlags)
                packet.ReadBitsE<WowPacketParser.Enums.v4.MovementFlag>("Movement Flags", 30);

            packet.ParseBitStream(guid, 3, 6, 0, 7, 4, 1, 5, 2);

            if (hasPitch)
                packet.ReadSingle("Pitch");

            if (hasTrans)
            {
                var tpos = new Vector4();

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                packet.ReadXORByte(transportGuid, 3);
                tpos.X = packet.ReadSingle();
                packet.ReadSByte("Transport Seat");
                packet.ReadXORByte(transportGuid, 5);
                packet.ReadXORByte(transportGuid, 1);
                tpos.Z = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 2);
                packet.ReadXORByte(transportGuid, 7);
                tpos.O = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 4);
                packet.ReadUInt32("Transport Time");

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                packet.ReadXORByte(transportGuid, 0);
                tpos.Y = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 6);

                packet.WriteGuid("Transport Guid", transportGuid);
                packet.AddValue("Transport Position", tpos);
            }

            if (hasFallData)
            {
                if (hasFallDirection)
                {
                    packet.ReadSingle("Fall Sin");
                    packet.ReadSingle("Horizontal Speed");
                    packet.ReadSingle("Fall Cos");
                }

                packet.ReadUInt32("Fall time");
                packet.ReadSingle("Vertical Speed");
            }

            if (hasSplineElev)
                packet.ReadSingle("Spline elevation");
            if (hasTime)
                packet.ReadUInt32("Timestamp");
            if (hasO)
                pos.O = packet.ReadSingle();

            packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
        }

        [Parser(Opcode.MSG_MOVE_SET_WALK_MODE)]
        public static void HandleMoveSetWalkMode434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();

            pos.Y = packet.ReadSingle();
            pos.X = packet.ReadSingle();
            pos.Z = packet.ReadSingle();
            guid[6] = packet.ReadBit();
            packet.ReadBit("Has Spline");
            var hasTime = !packet.ReadBit("Has timestamp");
            guid[0] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            var hasMovementFlags = !packet.ReadBit();
            var hasPitch = !packet.ReadBit("Has pitch");
            guid[7] = packet.ReadBit();
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            guid[4] = packet.ReadBit();
            var hasO = !packet.ReadBit();
            var hasTrans = packet.ReadBit("Has transport");
            guid[2] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            packet.ReadBit();
            var hasMovementFlags2 = !packet.ReadBit();
            var hasFallData = packet.ReadBit("Has fall data");

            if (hasTrans)
            {
                transportGuid[2] = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
            }

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            if (hasMovementFlags)
                packet.ReadBitsE<WowPacketParser.Enums.v4.MovementFlag>("Movement Flags", 30);

            if (hasMovementFlags2)
                packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            packet.ParseBitStream(guid, 5, 6, 4, 7, 3, 0, 2, 1);

            if (hasTrans)
            {
                var tpos = new Vector4();

                packet.ReadXORByte(transportGuid, 2);
                packet.ReadXORByte(transportGuid, 5);
                packet.ReadSByte("Transport Seat");
                tpos.Z = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 3);
                packet.ReadXORByte(transportGuid, 6);
                packet.ReadXORByte(transportGuid, 0);
                packet.ReadUInt32("Transport Time");
                packet.ReadXORByte(transportGuid, 4);

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                tpos.O = packet.ReadSingle();
                tpos.X = packet.ReadSingle();

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                packet.ReadXORByte(transportGuid, 7);
                tpos.Y = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 1);

                packet.WriteGuid("Transport Guid", transportGuid);
                packet.AddValue("Transport Position", tpos);
            }

            if (hasFallData)
            {
                if (hasFallDirection)
                {
                    packet.ReadSingle("Fall Cos");
                    packet.ReadSingle("Horizontal Speed");
                    packet.ReadSingle("Fall Sin");
                }

                packet.ReadSingle("Vertical Speed");
                packet.ReadUInt32("Fall time");
            }

            if (hasSplineElev)
                packet.ReadSingle("Spline elevation");
            if (hasPitch)
                packet.ReadSingle("Pitch");
            if (hasTime)
                packet.ReadUInt32("Timestamp");
            if (hasO)
                pos.O = packet.ReadSingle();

            packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
        }

        [Parser(Opcode.CMSG_MOVE_SET_CAN_FLY)]
        public static void HandleMoveSetCanFly434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();

            pos.Z = packet.ReadSingle();
            pos.X = packet.ReadSingle();
            pos.Y = packet.ReadSingle();
            var hasTrans = packet.ReadBit("Has transport");
            guid[1] = packet.ReadBit();
            guid[6] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            var hasFallData = packet.ReadBit("Has fall data");
            guid[0] = packet.ReadBit();
            var hasMovementFlags = !packet.ReadBit();
            guid[3] = packet.ReadBit();
            var hasMovementFlags2 = !packet.ReadBit();
            guid[5] = packet.ReadBit();
            var hasTime = !packet.ReadBit("Has timestamp");
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            packet.ReadBit("Has Spline");
            var hasO = !packet.ReadBit();
            guid[7] = packet.ReadBit();
            packet.ReadBit();
            var hasPitch = !packet.ReadBit("Has pitch");

            if (hasTrans)
            {
                transportGuid[3] = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
            }

            if (hasMovementFlags2)
                packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            if (hasMovementFlags)
                packet.ReadBitsE<WowPacketParser.Enums.v4.MovementFlag>("Movement Flags", 30);

            packet.ParseBitStream(guid, 2, 0, 4, 7, 5, 1, 3, 6);

            if (hasTrans)
            {
                var tpos = new Vector4();

                tpos.Z = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 3);
                packet.ReadXORByte(transportGuid, 5);
                packet.ReadXORByte(transportGuid, 4);
                packet.ReadSByte("Transport Seat");
                tpos.X = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 2);

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                tpos.Y = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 1);
                packet.ReadUInt32("Transport Time");
                packet.ReadXORByte(transportGuid, 7);
                packet.ReadXORByte(transportGuid, 0);
                tpos.O = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 6);

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                packet.WriteGuid("Transport Guid", transportGuid);
                packet.AddValue("Transport Position", tpos);
            }

            if (hasSplineElev)
                packet.ReadSingle("Spline elevation");

            if (hasFallData)
            {
                if (hasFallDirection)
                {
                    packet.ReadSingle("Fall Cos");
                    packet.ReadSingle("Fall Sin");
                    packet.ReadSingle("Horizontal Speed");
                }

                packet.ReadSingle("Vertical Speed");
                packet.ReadUInt32("Fall time");
            }

            if (hasPitch)
                packet.ReadSingle("Pitch");
            if (hasTime)
                packet.ReadUInt32("Timestamp");
            if (hasO)
                pos.O = packet.ReadSingle();

            packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
        }

        [Parser(Opcode.CMSG_DISMISS_CONTROLLED_VEHICLE)]
        public static void HandleDismissControlledVehicle434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();

            pos.Y = packet.ReadSingle();
            pos.Z = packet.ReadSingle();
            pos.X = packet.ReadSingle();
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            var hasFallData = packet.ReadBit("Has fall data");
            var hasTrans = packet.ReadBit("Has transport");
            guid[4] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            var hasMovementFlags2 = !packet.ReadBit();
            guid[2] = packet.ReadBit();
            var hasO = !packet.ReadBit();
            guid[0] = packet.ReadBit();
            guid[6] = packet.ReadBit();
            var hasPitch = !packet.ReadBit("Has pitch");
            var hasTime = !packet.ReadBit("Has timestamp");
            guid[1] = packet.ReadBit();
            packet.ReadBit();
            packet.ReadBit("Has Spline");
            guid[3] = packet.ReadBit();
            var hasMovementFlags = !packet.ReadBit();
            guid[5] = packet.ReadBit();

            if (hasTrans)
            {
                transportGuid[5] = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
            }

            if (hasMovementFlags2)
                packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            if (hasMovementFlags)
                packet.ReadBitsE<WowPacketParser.Enums.v4.MovementFlag>("Movement Flags", 30);

            packet.ParseBitStream(guid, 6, 3, 1, 5, 2, 4, 7, 0);

            if (hasTime)
                packet.ReadUInt32("Timestamp");

            if (hasTrans)
            {
                var tpos = new Vector4();

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                packet.ReadXORByte(transportGuid, 4);
                packet.ReadXORByte(transportGuid, 7);

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                packet.ReadXORByte(transportGuid, 6);
                packet.ReadUInt32("Transport Time");
                tpos.Z = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 0);
                tpos.X = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 3);
                packet.ReadSByte("Transport Seat");
                tpos.Y = packet.ReadSingle();
                tpos.O = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 1);
                packet.ReadXORByte(transportGuid, 5);
                packet.ReadXORByte(transportGuid, 2);

                packet.WriteGuid("Transport Guid", transportGuid);
                packet.AddValue("Transport Position", tpos);
            }

            if (hasFallData)
            {
                packet.ReadUInt32("Fall time");

                if (hasFallDirection)
                {
                    packet.ReadSingle("Horizontal Speed");
                    packet.ReadSingle("Fall Sin");
                    packet.ReadSingle("Fall Cos");
                }

                packet.ReadSingle("Vertical Speed");
            }

            if (hasO)
                pos.O = packet.ReadSingle();
            if (hasSplineElev)
                packet.ReadSingle("Spline elevation");
            if (hasPitch)
                packet.ReadSingle("Pitch");

            packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
        }

        [Parser(Opcode.CMSG_MOVE_SPLINE_DONE)]
        public static void HandleMoveSplineDone434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();

            packet.ReadInt32("Unk Int32 1"); // ##
            pos.Y = packet.ReadSingle();
            pos.X = packet.ReadSingle();
            pos.Z = packet.ReadSingle();
            guid[6] = packet.ReadBit();
            var hasO = !packet.ReadBit();
            var hasFallData = packet.ReadBit("Has fall data");
            var hasTime = !packet.ReadBit("Has timestamp");
            guid[2] = packet.ReadBit();
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            guid[4] = packet.ReadBit();
            var hasTrans = packet.ReadBit("Has transport");
            guid[3] = packet.ReadBit();
            var hasMovementFlags = !packet.ReadBit();
            guid[0] = packet.ReadBit();
            packet.ReadBit();
            guid[1] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            var hasPitch = !packet.ReadBit("Has pitch");
            packet.ReadBit("Has Spline");
            var hasMovementFlags2 = !packet.ReadBit();
            guid[7] = packet.ReadBit();

            if (hasTrans)
            {
                transportGuid[1] = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
            }

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            if (hasMovementFlags2)
                packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            if (hasMovementFlags)
                packet.ReadBitsE<WowPacketParser.Enums.v4.MovementFlag>("Movement Flags", 30);

            packet.ParseBitStream(guid, 7, 4, 5, 6, 0, 1, 2, 3);

            if (hasFallData)
            {
                packet.ReadSingle("Vertical Speed");

                if (hasFallDirection)
                {
                    packet.ReadSingle("Fall Sin");
                    packet.ReadSingle("Fall Cos");
                    packet.ReadSingle("Horizontal Speed");
                }

                packet.ReadUInt32("Fall time");
            }

            if (hasPitch)
                packet.ReadSingle("Pitch");
            if (hasO)
                pos.O = packet.ReadSingle();

            if (hasTrans)
            {
                var tpos = new Vector4();

                packet.ReadXORByte(transportGuid, 1);

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                packet.ReadXORByte(transportGuid, 7);
                packet.ReadUInt32("Transport Time");

                tpos.Y = packet.ReadSingle();
                tpos.X = packet.ReadSingle();
                tpos.Z = packet.ReadSingle();
                packet.ReadSByte("Transport Seat");
                tpos.O = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 0);

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                packet.ReadXORByte(transportGuid, 2);
                packet.ReadXORByte(transportGuid, 3);
                packet.ReadXORByte(transportGuid, 5);
                packet.ReadXORByte(transportGuid, 6);
                packet.ReadXORByte(transportGuid, 4);

                packet.WriteGuid("Transport Guid", transportGuid);
                packet.AddValue("Transport Position", tpos);
            }

            if (hasTime)
                packet.ReadUInt32("Timestamp");
            if (hasSplineElev)
                packet.ReadSingle("Spline elevation");

            packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
        }

        [Parser(Opcode.CMSG_MOVE_SET_CAN_TRANSITION_BETWEEN_SWIM_AND_FLY_ACK)]
        public static void HandleMoveSetCanTransitionBetweenSwimAndFlyAck434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();

            pos.Z = packet.ReadSingle();
            pos.Y = packet.ReadSingle();
            packet.ReadInt32("Unk Int32 1"); // ##
            pos.X = packet.ReadSingle();
            guid[3] = packet.ReadBit();
            var hasO = !packet.ReadBit();
            var hasTrans = packet.ReadBit("Has transport");
            var hasMovementFlags = !packet.ReadBit();
            var hasMovementFlags2 = !packet.ReadBit();
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            var hasTime = !packet.ReadBit("Has timestamp");
            packet.ReadBit("Has Spline");
            packet.ReadBit();
            var hasFallData = packet.ReadBit("Has fall data");
            guid[5] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            guid[6] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            var hasPitch = !packet.ReadBit("Has pitch");
            guid[7] = packet.ReadBit();

            if (hasTrans)
            {
                transportGuid[4] = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
            }

            if (hasMovementFlags2)
                packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            if (hasMovementFlags)
                packet.ReadBitsE<WowPacketParser.Enums.v4.MovementFlag>("Movement Flags", 30);

            packet.ParseBitStream(guid, 3, 2, 0, 4, 1, 5, 7, 6);

            if (hasFallData)
            {
                packet.ReadUInt32("Fall time");

                if (hasFallDirection)
                {
                    packet.ReadSingle("Horizontal Speed");
                    packet.ReadSingle("Fall Sin");
                    packet.ReadSingle("Fall Cos");
                }

                packet.ReadSingle("Vertical Speed");
            }

            if (hasTrans)
            {
                var tpos = new Vector4();

                tpos.Y = packet.ReadSingle();
                tpos.Z = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 5);
                packet.ReadXORByte(transportGuid, 1);
                packet.ReadXORByte(transportGuid, 4);
                packet.ReadUInt32("Transport Time");
                packet.ReadSByte("Transport Seat");
                packet.ReadXORByte(transportGuid, 3);
                packet.ReadXORByte(transportGuid, 0);
                packet.ReadXORByte(transportGuid, 2);
                packet.ReadXORByte(transportGuid, 7);
                tpos.X = packet.ReadSingle();

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                packet.ReadXORByte(transportGuid, 6);
                tpos.O = packet.ReadSingle();

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                packet.WriteGuid("Transport Guid", transportGuid);
                packet.AddValue("Transport Position", tpos);
            }

            if (hasPitch)
                packet.ReadSingle("Pitch");
            if (hasO)
                pos.O = packet.ReadSingle();
            if (hasTime)
                packet.ReadUInt32("Timestamp");
            if (hasSplineElev)
                packet.ReadSingle("Spline elevation");

            packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
        }

        [Parser(Opcode.SMSG_MOVE_UPDATE_SWIM_SPEED)]
        public static void HandleMoveUpdateSwimSpeed434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasFallDirection = false;
            var pos = new Vector4();

            var hasMovementFlags = !packet.ReadBit();
            guid[2] = packet.ReadBit();
            packet.ReadBit("Has spline data");
            var hasTime = !packet.ReadBit();

            if (hasMovementFlags)
                packet.ReadBitsE<WowPacketParser.Enums.v4.MovementFlag>("Movement flags", 30);

            var hasFallData = packet.ReadBit();
            guid[3] = packet.ReadBit();
            var hasTransport = packet.ReadBit();
            packet.ReadBit();
            guid[7] = packet.ReadBit();

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            var hasTransportTime2 = false;
            var hasTransportTime3 = false;
            if (hasTransport)
            {
                transportGuid[4] = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
                hasTransportTime2 = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                hasTransportTime3 = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
            }

            var hasMovementFlagsExtra = !packet.ReadBit();
            guid[6] = packet.ReadBit();
            var hasSplineElevation = !packet.ReadBit();
            guid[0] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            var hasO = !packet.ReadBit();
            var hasPitch = !packet.ReadBit();

            if (hasMovementFlagsExtra)
                packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            guid[1] = packet.ReadBit();

            if (hasTransport)
            {
                var tpos = new Vector4();

                packet.ReadXORByte(transportGuid, 2);
                tpos.X = packet.ReadSingle();

                if (hasTransportTime3)
                    packet.ReadUInt32("Transport Time 3");

                if (hasTransportTime2)
                    packet.ReadUInt32("Transport Time 2");

                packet.ReadXORByte(transportGuid, 6);
                tpos.O = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 0);
                packet.ReadXORByte(transportGuid, 4);
                tpos.Y = packet.ReadSingle();
                packet.ReadUInt32("Transport Time");
                packet.ReadXORByte(transportGuid, 1);
                packet.ReadXORByte(transportGuid, 7);
                tpos.Z = packet.ReadSingle();
                packet.ReadSByte("Transport Seat");
                packet.ReadXORByte(transportGuid, 3);
                packet.ReadXORByte(transportGuid, 5);

                packet.WriteGuid("Transport Guid", transportGuid);
                packet.AddValue("Transport Position", tpos);
            }

            pos.X = packet.ReadSingle();

            if (hasFallData)
            {
                if (hasFallDirection)
                {
                    packet.ReadSingle("Fall Cos");
                    packet.ReadSingle("Horizontal speed");
                    packet.ReadSingle("Fall Sin");
                }

                packet.ReadUInt32("Fall time");
                packet.ReadSingle("Vertical speed");
            }

            packet.ReadXORByte(guid, 7);

            if (hasTime)
                packet.ReadUInt32("Timestamp");

            pos.Y = packet.ReadSingle();

            if (hasSplineElevation)
                packet.ReadSingle("Spline elevation");

            pos.Z = packet.ReadSingle();
            packet.ReadSingle("Speed");
            packet.ReadXORByte(guid, 4);

            if (hasO)
                pos.O = packet.ReadSingle();

            packet.ReadXORByte(guid, 2);
            packet.ReadXORByte(guid, 3);
            packet.ReadXORByte(guid, 0);
            packet.ReadXORByte(guid, 6);
            packet.ReadXORByte(guid, 1);
            packet.ReadXORByte(guid, 5);

            if (hasPitch)
                packet.ReadSingle("Pitch");

            packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
        }

        [Parser(Opcode.SMSG_MOVE_UPDATE_RUN_SPEED)]
        public static void HandleMoveUpdateRunSpeed434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasFallDirection = false;
            var pos = new Vector4();

            pos.Z = packet.ReadSingle();
            pos.X = packet.ReadSingle();
            pos.Y = packet.ReadSingle();
            packet.ReadSingle("Speed");
            guid[6] = packet.ReadBit();
            var hasMovementFlagsExtra = !packet.ReadBit();
            var hasPitch = !packet.ReadBit();
            guid[2] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            var hasSplineElevation = !packet.ReadBit();
            packet.ReadBit("Has spline data");
            var hasMovementFlags = !packet.ReadBit();
            var hasTime = !packet.ReadBit();
            guid[1] = packet.ReadBit();

            if (hasMovementFlagsExtra)
                packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            guid[3] = packet.ReadBit();

            if (hasMovementFlags)
                packet.ReadBitsE<WowPacketParser.Enums.v4.MovementFlag>("Movement flags", 30);

            guid[7] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            var hasO = !packet.ReadBit();
            var hasTransport = packet.ReadBit();

            var hasTransportTime2 = false;
            var hasTransportTime3 = false;
            if (hasTransport)
            {
                transportGuid[5] = packet.ReadBit();
                hasTransportTime2 = packet.ReadBit();
                hasTransportTime3 = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
            }

            var hasFallData = packet.ReadBit();

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            guid[4] = packet.ReadBit();
            packet.ReadBit();

            if (hasTransport)
            {
                var tpos = new Vector4();

                packet.ReadXORByte(transportGuid, 4);
                packet.ReadXORByte(transportGuid, 5);
                tpos.X = packet.ReadSingle();
                tpos.O = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 1);
                packet.ReadXORByte(transportGuid, 0);
                packet.ReadXORByte(transportGuid, 6);
                packet.ReadUInt32("Transport Time");
                packet.ReadXORByte(transportGuid, 7);
                packet.ReadSByte("Transport Seat");

                if (hasTransportTime2)
                    packet.ReadUInt32("Transport Time 2");

                tpos.Y = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 3);
                packet.ReadXORByte(transportGuid, 2);

                if (hasTransportTime3)
                    packet.ReadUInt32("Transport Time 3");

                tpos.Z = packet.ReadSingle();

                packet.WriteGuid("Transport Guid", transportGuid);
                packet.AddValue("Transport Position", tpos);
            }

            if (hasTime)
                packet.ReadUInt32("Timestamp");

            if (hasFallData)
            {
                if (hasFallDirection)
                {
                    packet.ReadSingle("Fall Sin");
                    packet.ReadSingle("Horizontal speed");
                    packet.ReadSingle("Fall Cos");
                }

                packet.ReadSingle("Vertical speed");
                packet.ReadUInt32("Fall time");
            }

            if (hasPitch)
                packet.ReadSingle("Pitch");

            packet.ReadXORByte(guid, 6);

            if (hasSplineElevation)
                packet.ReadSingle("Spline elevation");

            packet.ReadXORByte(guid, 5);
            packet.ReadXORByte(guid, 7);
            packet.ReadXORByte(guid, 4);

            if (hasO)
                pos.O = packet.ReadSingle();

            packet.ReadXORByte(guid, 0);
            packet.ReadXORByte(guid, 3);
            packet.ReadXORByte(guid, 2);
            packet.ReadXORByte(guid, 1);


            packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
        }

        [Parser(Opcode.SMSG_MOVE_UPDATE_FLIGHT_SPEED)]
        public static void HandleMoveUpdateFlightSpeed434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasFallDirection = false;
            var pos = new Vector4();

            pos.Y = packet.ReadSingle();
            packet.ReadSingle("Speed");
            pos.X = packet.ReadSingle();
            pos.Z = packet.ReadSingle();
            var hasMovementFlags = !packet.ReadBit();
            guid[4] = packet.ReadBit();
            packet.ReadBit();
            guid[5] = packet.ReadBit();
            guid[1] = packet.ReadBit();

            if (hasMovementFlags)
                packet.ReadBitsE<WowPacketParser.Enums.v4.MovementFlag>("Movement flags", 30);

            var hasFallData = packet.ReadBit();
            guid[2] = packet.ReadBit();
            var hasO = !packet.ReadBit();
            var hasPitch = !packet.ReadBit();
            var hasSplineElevation = !packet.ReadBit();
            guid[7] = packet.ReadBit();
            var hasTime = !packet.ReadBit();
            var hasTransport = packet.ReadBit();

            var hasTransportTime2 = false;
            var hasTransportTime3 = false;
            if (hasTransport)
            {
                transportGuid[3] = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                hasTransportTime3 = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                hasTransportTime2 = packet.ReadBit();
            }

            guid[6] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            var hasMovementFlagsExtra = !packet.ReadBit();
            packet.ReadBit("Has spline data");

            if (hasMovementFlagsExtra)
                packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            guid[0] = packet.ReadBit();

            if (hasSplineElevation)
                packet.ReadSingle("Spline elevation");

            if (hasPitch)
                packet.ReadSingle("Pitch");

            packet.ReadXORByte(guid, 0);
            packet.ReadXORByte(guid, 5);

            if (hasTransport)
            {
                var tpos = new Vector4();

                tpos.O = packet.ReadSingle();
                packet.ReadUInt32("Transport Time");
                packet.ReadXORByte(transportGuid, 5);
                packet.ReadXORByte(transportGuid, 4);
                packet.ReadXORByte(transportGuid, 6);
                tpos.Z = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 7);
                packet.ReadXORByte(transportGuid, 0);
                packet.ReadXORByte(transportGuid, 2);
                packet.ReadXORByte(transportGuid, 3);
                tpos.Y = packet.ReadSingle();

                if (hasTransportTime3)
                    packet.ReadUInt32("Transport Time 3");

                if (hasTransportTime2)
                    packet.ReadUInt32("Transport Time 2");

                packet.ReadXORByte(transportGuid, 1);
                packet.ReadSByte("Transport Seat");
                tpos.X = packet.ReadSingle();

                packet.WriteGuid("Transport Guid", transportGuid);
                packet.AddValue("Transport Position", tpos);
            }

            if (hasFallData)
            {
                if (hasFallDirection)
                {
                    packet.ReadSingle("Fall Cos");
                    packet.ReadSingle("Horizontal speed");
                    packet.ReadSingle("Fall Sin");
                }

                packet.ReadSingle("Vertical speed");
                packet.ReadUInt32("Fall time");
            }

            packet.ReadXORByte(guid, 1);
            packet.ReadXORByte(guid, 6);

            if (hasTime)
                packet.ReadUInt32("Timestamp");

            packet.ReadXORByte(guid, 7);
            packet.ReadXORByte(guid, 2);
            packet.ReadXORByte(guid, 4);

            if (hasO)
                pos.O = packet.ReadSingle();

            packet.ReadXORByte(guid, 3);

            packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
        }

        [Parser(Opcode.SMSG_MOVE_UPDATE_COLLISION_HEIGHT)]
        public static void HandleMoveUpdateCollisionHeight434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasFallDirection = false;
            var pos = new Vector4();

            pos.Z = packet.ReadSingle();
            packet.ReadSingle("Height");
            pos.X = packet.ReadSingle();
            pos.Y = packet.ReadSingle();
            guid[6] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            var hasSplineElevation = !packet.ReadBit();
            var hasTransport = packet.ReadBit();

            var hasTransportTime2 = false;
            var hasTransportTime3 = false;
            if (hasTransport)
            {
                transportGuid[6] = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                hasTransportTime3 = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                hasTransportTime2 = packet.ReadBit();
            }

            guid[3] = packet.ReadBit();
            var hasO = !packet.ReadBit();
            guid[4] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            var hasTime = !packet.ReadBit();
            var hasFallData = packet.ReadBit();
            guid[0] = packet.ReadBit();
            var hasPitch = !packet.ReadBit();
            packet.ReadBit(); // not sure (offset 157);
            guid[2] = packet.ReadBit();
            var hasMovementFlags = !packet.ReadBit();

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            guid[1] = packet.ReadBit();
            var hasMovementFlagsExtra = !packet.ReadBit();
            packet.ReadBit("Has spline data"); // not sure (offset 156)

            if (hasMovementFlagsExtra)
                packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            if (hasMovementFlags)
                packet.ReadBitsE<WowPacketParser.Enums.v4.MovementFlag>("Movement flags", 30);

            packet.ReadXORByte(guid, 3);

            if (hasTransport)
            {
                var tpos = new Vector4();

                packet.ReadXORByte(transportGuid, 7);
                packet.ReadUInt32("Transport Time");
                packet.ReadXORByte(transportGuid, 4);

                if (hasTransportTime3)
                    packet.ReadUInt32("Transport Time 3");

                if (hasTransportTime2)
                    packet.ReadUInt32("Transport Time 2");

                packet.ReadXORByte(transportGuid, 5);
                tpos.O = packet.ReadSingle();
                tpos.X = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 6);
                packet.ReadXORByte(transportGuid, 0);
                tpos.Y = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 2);
                tpos.Z = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 3);
                packet.ReadXORByte(transportGuid, 1);
                packet.ReadSByte("Transport Seat");

                packet.WriteGuid("Transport Guid", transportGuid);
                packet.AddValue("Transport Position", tpos);
            }

            if (hasPitch)
                packet.ReadSingle("Pitch");

            packet.ReadXORByte(guid, 6);

            if (hasFallData)
            {
                if (hasFallDirection)
                {
                    packet.ReadSingle("Fall Cos");
                    packet.ReadSingle("Fall Sin");
                    packet.ReadSingle("Horizontal speed");
                }

                packet.ReadSingle("Vertical speed");
                packet.ReadUInt32("Fall time");
            }

            packet.ReadXORByte(guid, 7);

            if (hasO)
                pos.O = packet.ReadSingle();

            packet.ReadXORByte(guid, 0);
            packet.ReadXORByte(guid, 5);
            packet.ReadXORByte(guid, 4);

            if (hasSplineElevation)
                packet.ReadSingle("Spline elevation");

            if (hasTime)
                packet.ReadUInt32("Timestamp");

            packet.ReadXORByte(guid, 2);
            packet.ReadXORByte(guid, 1);

            packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
        }

        [Parser(Opcode.SMSG_MOVE_UPDATE_TELEPORT)]
        public static void HandleMoveUpdateTeleport434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasFallDirection = false;
            var pos = new Vector4();

            pos.Z = packet.ReadSingle();
            pos.Y = packet.ReadSingle();
            pos.X = packet.ReadSingle();
            var hasO = !packet.ReadBit();
            packet.ReadBit("Has spline data");
            var hasMovementFlags = !packet.ReadBit();
            guid[2] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            guid[6] = packet.ReadBit();
            var hasFallData = packet.ReadBit();
            guid[0] = packet.ReadBit();
            var hasTransport = packet.ReadBit();
            guid[5] = packet.ReadBit();

            var hasTransportTime2 = false;
            var hasTransportTime3 = false;
            if (hasTransport)
            {
                transportGuid[1] = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                hasTransportTime2 = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                hasTransportTime3 = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
            }

            packet.ReadBit();
            guid[7] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            var hasPitch = !packet.ReadBit();
            var hasMovementFlagsExtra = !packet.ReadBit();
            var hasTime = !packet.ReadBit();

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            if (hasMovementFlagsExtra)
                packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            var hasSplineElevation = !packet.ReadBit();

            if (hasMovementFlags)
                packet.ReadBitsE<WowPacketParser.Enums.v4.MovementFlag>("Movement flags", 30);

            guid[1] = packet.ReadBit();
            packet.ReadXORByte(guid, 7);

            if (hasTransport)
            {
                var tpos = new Vector4();

                packet.ReadXORByte(transportGuid, 3);
                packet.ReadXORByte(transportGuid, 4);
                tpos.O = packet.ReadSingle();

                if (hasTransportTime3)
                    packet.ReadUInt32("Transport Time 3");

                packet.ReadXORByte(transportGuid, 1);

                if (hasTransportTime2)
                    packet.ReadUInt32("Transport Time 2");

                tpos.Z = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 7);
                packet.ReadXORByte(transportGuid, 0);
                packet.ReadXORByte(transportGuid, 6);
                packet.ReadXORByte(transportGuid, 5);
                packet.ReadXORByte(transportGuid, 2);
                packet.ReadSByte("Transport Seat");
                packet.ReadUInt32("Transport Time");
                tpos.Y = packet.ReadSingle();
                tpos.X = packet.ReadSingle();

                packet.WriteGuid("Transport Guid", transportGuid);
                packet.AddValue("Transport Position", tpos);
            }

            packet.ReadXORByte(guid, 6);

            if (hasPitch)
                packet.ReadSingle("Pitch");

            if (hasSplineElevation)
                packet.ReadSingle("Spline elevation");

            if (hasO)
                pos.O = packet.ReadSingle();

            packet.ReadXORByte(guid, 2);
            packet.ReadXORByte(guid, 3);
            packet.ReadXORByte(guid, 1);

            if (hasFallData)
            {
                packet.ReadUInt32("Fall time");

                if (hasFallDirection)
                {
                    packet.ReadSingle("Horizontal speed");
                    packet.ReadSingle("Fall Sin");
                    packet.ReadSingle("Fall Cos");
                }

                packet.ReadSingle("Vertical speed");
            }

            packet.ReadXORByte(guid, 5);
            packet.ReadXORByte(guid, 4);

            if (hasTime)
                packet.ReadUInt32("Timestamp");

            packet.ReadXORByte(guid, 0);

            packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
        }

        [Parser(Opcode.SMSG_MOVE_SPLINE_SET_SWIM_BACK_SPEED)]
        public static void HandleSplineSetSwimBackSpeed434(Packet packet)
        {
            var guid = packet.StartBitStream(0, 1, 3, 6, 4, 5, 7, 2);
            packet.ReadXORByte(guid, 5);
            packet.ReadXORByte(guid, 3);
            packet.ReadXORByte(guid, 1);
            packet.ReadXORByte(guid, 0);
            packet.ReadXORByte(guid, 7);
            packet.ReadXORByte(guid, 6);
            packet.ReadSingle("Speed");
            packet.ReadXORByte(guid, 4);
            packet.ReadXORByte(guid, 2);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SPLINE_SET_FLIGHT_BACK_SPEED)]
        public static void HandleSplineSetFlightBackSpeed434(Packet packet)
        {
            var guid = packet.StartBitStream(2, 1, 6, 5, 0, 3, 4, 7);
            packet.ReadXORByte(guid, 5);
            packet.ReadSingle("Speed");
            packet.ReadXORByte(guid, 6);
            packet.ReadXORByte(guid, 1);
            packet.ReadXORByte(guid, 0);
            packet.ReadXORByte(guid, 2);
            packet.ReadXORByte(guid, 3);
            packet.ReadXORByte(guid, 7);
            packet.ReadXORByte(guid, 4);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SPLINE_SET_TURN_RATE)]
        public static void HandleSplineSetTurnRate434(Packet packet)
        {
            var guid = packet.StartBitStream(2, 4, 6, 1, 3, 5, 7, 0);
            packet.ReadSingle("Rate");
            packet.ParseBitStream(guid, 1, 5, 3, 2, 7, 4, 6, 0);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SPLINE_SET_PITCH_RATE)]
        public static void HandleSplineSetPitchRate434(Packet packet)
        {
            var guid = packet.StartBitStream(3, 5, 6, 1, 0, 4, 7, 2);
            packet.ReadXORByte(guid, 1);
            packet.ReadXORByte(guid, 5);
            packet.ReadXORByte(guid, 7);
            packet.ReadXORByte(guid, 0);
            packet.ReadXORByte(guid, 6);
            packet.ReadXORByte(guid, 3);
            packet.ReadXORByte(guid, 2);
            packet.ReadSingle("Rate");
            packet.ReadXORByte(guid, 4);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SPLINE_ROOT)]
        public static void HandleSplineMoveRoot434(Packet packet)
        {
            var guid = packet.StartBitStream(5, 4, 6, 1, 3, 7, 2, 0);
            packet.ParseBitStream(guid, 2, 1, 7, 3, 5, 0, 6, 4);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SPLINE_UNROOT)]
        public static void HandleSplineMoveUnroot434(Packet packet)
        {
            var guid = packet.StartBitStream(0, 1, 6, 5, 3, 2, 7, 4);
            packet.ParseBitStream(guid, 6, 3, 1, 5, 2, 0, 7, 4);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SPLINE_ENABLE_GRAVITY)]
        public static void HandleSplineMoveGravityEnable434(Packet packet)
        {
            var guid = packet.StartBitStream(5, 4, 7, 1, 3, 6, 2, 0);
            packet.ParseBitStream(guid, 7, 3, 4, 2, 1, 6, 0, 5);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SPLINE_DISABLE_GRAVITY)]
        public static void HandleSplineMoveGravityDisable434(Packet packet)
        {
            var guid = packet.StartBitStream(7, 3, 4, 2, 5, 1, 0, 6);
            packet.ParseBitStream(guid, 7, 1, 3, 4, 6, 2, 5, 0);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SPLINE_ENABLE_COLLISION)]
        public static void HandleSplineMoveCollisionEnable434(Packet packet)
        {
            var guid = packet.StartBitStream(3, 4, 7, 6, 1, 0, 2, 5);
            packet.ParseBitStream(guid, 1, 3, 7, 2, 0, 6, 4, 5);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SPLINE_DISABLE_COLLISION)]
        public static void HandleSplineMoveCollisionDisable434(Packet packet)
        {
            var guid = packet.StartBitStream(3, 7, 1, 0, 4, 2, 6, 5);
            packet.ParseBitStream(guid, 3, 5, 6, 7, 2, 1, 0, 4);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SPLINE_SET_NORMAL_FALL)]
        public static void HandleSplineMoveNormalFall434(Packet packet)
        {
            var guid = packet.StartBitStream(3, 5, 1, 0, 7, 6, 2, 4);
            packet.ParseBitStream(guid, 7, 6, 2, 0, 5, 4, 3, 1);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SPLINE_SET_RUN_MODE)]
        public static void HandleSplineSetRunMode434(Packet packet)
        {
            var guid = packet.StartBitStream(5, 6, 3, 7, 2, 0, 4, 1);
            packet.ParseBitStream(guid, 7, 0, 4, 6, 5, 1, 2, 3);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SPLINE_SET_WALK_MODE)]
        public static void HandleSplineSetWalkMode434(Packet packet)
        {
            var guid = packet.StartBitStream(7, 6, 5, 1, 3, 4, 2, 0);
            packet.ParseBitStream(guid, 4, 2, 1, 6, 5, 0, 7, 3);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SPLINE_SET_HOVER)]
        public static void HandleSplineSetHover434(Packet packet)
        {
            var guid = packet.StartBitStream(3, 7, 0, 1, 4, 6, 2, 5);
            packet.ParseBitStream(guid, 2, 4, 3, 1, 7, 0, 5, 6);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SPLINE_UNSET_HOVER)]
        public static void HandleSplineUnsetHover434(Packet packet)
        {
            var guid = packet.StartBitStream(6, 7, 4, 0, 3, 1, 5, 2);
            packet.ParseBitStream(guid, 4, 5, 3, 0, 2, 7, 6, 1);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SPLINE_SET_WATER_WALK)]
        public static void HandleSplineMoveWaterWalk434(Packet packet)
        {
            var guid = packet.StartBitStream(6, 1, 4, 2, 3, 7, 5, 0);
            packet.ParseBitStream(guid, 0, 6, 3, 7, 4, 2, 5, 1);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SPLINE_START_SWIM)]
        public static void HandleSplineMoveStartSwim434(Packet packet)
        {
            var guid = packet.StartBitStream(1, 6, 0, 7, 3, 5, 2, 4);
            packet.ParseBitStream(guid, 3, 7, 2, 5, 6, 4, 1, 0);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SPLINE_STOP_SWIM)]
        public static void HandleSplineMoveStopSwim434(Packet packet)
        {
            var guid = packet.StartBitStream(4, 1, 5, 3, 0, 7, 2, 6);
            packet.ParseBitStream(guid, 6, 0, 7, 2, 3, 1, 5, 4);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SPLINE_SET_FLYING)]
        public static void HandleSplineMoveSetFlying434(Packet packet)
        {
            var guid = packet.StartBitStream(0, 4, 1, 6, 7, 2, 3, 5);
            packet.ParseBitStream(guid, 7, 0, 5, 6, 4, 1, 3, 2);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SPLINE_UNSET_FLYING)]
        public static void HandleSplineMoveUnsetFlying434(Packet packet)
        {
            var guid = packet.StartBitStream(5, 0, 4, 7, 2, 3, 1, 6);
            packet.ParseBitStream(guid, 7, 2, 3, 4, 5, 1, 6, 0);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SET_RUN_SPEED)]
        public static void HandleMoveSetRunSpeed434(Packet packet)
        {
            var guid = packet.StartBitStream(6, 1, 5, 2, 7, 0, 3, 4);
            packet.ReadXORByte(guid, 5);
            packet.ReadXORByte(guid, 3);
            packet.ReadXORByte(guid, 1);
            packet.ReadXORByte(guid, 4);
            packet.ReadInt32("Unk Int32");
            packet.ReadSingle("Speed");
            packet.ReadXORByte(guid, 6);
            packet.ReadXORByte(guid, 0);
            packet.ReadXORByte(guid, 7);
            packet.ReadXORByte(guid, 2);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_ROOT)]
        public static void HandleMoveRoot434(Packet packet)
        {
            var guid = packet.StartBitStream(2, 7, 6, 0, 5, 4, 1, 3);
            packet.ReadXORByte(guid, 1);
            packet.ReadXORByte(guid, 0);
            packet.ReadXORByte(guid, 2);
            packet.ReadXORByte(guid, 5);
            packet.ReadInt32("Unk Int32");
            packet.ReadXORByte(guid, 3);
            packet.ReadXORByte(guid, 4);
            packet.ReadXORByte(guid, 7);
            packet.ReadXORByte(guid, 6);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_UNROOT)]
        public static void HandleMoveUnroot434(Packet packet)
        {
            var guid = packet.StartBitStream(0, 1, 3, 7, 5, 2, 4, 6);
            packet.ReadXORByte(guid, 3);
            packet.ReadXORByte(guid, 6);
            packet.ReadXORByte(guid, 1);
            packet.ReadInt32("Unk Int32");
            packet.ReadXORByte(guid, 2);
            packet.ReadXORByte(guid, 0);
            packet.ReadXORByte(guid, 7);
            packet.ReadXORByte(guid, 4);
            packet.ReadXORByte(guid, 5);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.CMSG_MOVE_FORCE_RUN_SPEED_CHANGE_ACK)]
        public static void HandleMoveForceRunSpeedChangeAck434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();

            packet.ReadInt32("Unk Int32 1"); // ##
            pos.X = packet.ReadSingle();
            packet.ReadSingle("Speed");
            pos.Z = packet.ReadSingle();
            pos.Y = packet.ReadSingle();
            guid[2] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            var hasO = !packet.ReadBit();
            var hasFallData = packet.ReadBit("Has fall data");
            guid[0] = packet.ReadBit();
            packet.ReadBit("Has Spline");
            var hasTrans = packet.ReadBit("Has transport");
            var hasTime = !packet.ReadBit("Has timestamp");
            var hasMovementFlags2 = !packet.ReadBit();
            guid[6] = packet.ReadBit();
            packet.ReadBit();
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            var hasPitch = !packet.ReadBit("Has pitch");
            guid[5] = packet.ReadBit();
            var hasMovementFlags = !packet.ReadBit();
            guid[3] = packet.ReadBit();

            if (hasTrans)
            {
                hasTransTime3 = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
            }

            if (hasMovementFlags)
                packet.ReadBitsE<WowPacketParser.Enums.v4.MovementFlag>("Movement Flags", 30);

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            if (hasMovementFlags2)
                packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            packet.ParseBitStream(guid, 6, 4, 1, 3, 5, 2, 7, 0);

            if (hasTrans)
            {
                var tpos = new Vector4();

                tpos.Z = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 6);
                packet.ReadXORByte(transportGuid, 1);
                tpos.Y = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 0);
                packet.ReadXORByte(transportGuid, 5);

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                tpos.X = packet.ReadSingle();
                packet.ReadUInt32("Transport Time");
                packet.ReadXORByte(transportGuid, 7);
                tpos.O = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 3);

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                packet.ReadXORByte(transportGuid, 2);
                packet.ReadSByte("Transport Seat");
                packet.ReadXORByte(transportGuid, 4);

                packet.WriteGuid("Transport Guid", transportGuid);
                packet.AddValue("Transport Position", tpos);
            }

            if (hasFallData)
            {
                packet.ReadSingle("Vertical Speed");

                if (hasFallDirection)
                {
                    packet.ReadSingle("Horizontal Speed");
                    packet.ReadSingle("Fall Cos");
                    packet.ReadSingle("Fall Sin");
                }

                packet.ReadUInt32("Fall time");
            }

            if (hasSplineElev)
                packet.ReadSingle("Spline elevation");
            if (hasPitch)
                packet.ReadSingle("Pitch");
            if (hasTime)
                packet.ReadUInt32("Timestamp");
            if (hasO)
                pos.O = packet.ReadSingle();

            packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
        }

        [Parser(Opcode.CMSG_MOVE_SET_COLLISION_HEIGHT_ACK)]
        public static void HandleMoveSetCollisionHeightAck434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();

            packet.ReadSingle("Collision height");
            pos.X = packet.ReadSingle();
            pos.Z = packet.ReadSingle();
            packet.ReadInt32("Unk Int32 1"); // ##
            pos.Y = packet.ReadSingle();
            guid[6] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            packet.ReadBits("Unk bits", 2); // ##
            var hasPitch = !packet.ReadBit("Has pitch");
            guid[5] = packet.ReadBit();
            packet.ReadBit();
            guid[2] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            var hasFallData = packet.ReadBit("Has fall data");
            guid[3] = packet.ReadBit();
            packet.ReadBit("Has Spline");
            guid[7] = packet.ReadBit();
            var hasMovementFlags = !packet.ReadBit();
            var hasTrans = packet.ReadBit("Has transport");
            var hasTime = !packet.ReadBit("Has timestamp");
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            var hasMovementFlags2 = !packet.ReadBit();
            var hasO = !packet.ReadBit();
            guid[0] = packet.ReadBit();

            if (hasTrans)
            {
                transportGuid[4] = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
            }

            if (hasMovementFlags2)
                packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            if (hasMovementFlags)
                packet.ReadBitsE<WowPacketParser.Enums.v4.MovementFlag>("Movement Flags", 30);

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            packet.ParseBitStream(guid, 0, 3, 1, 5, 7, 6, 2, 4);

            if (hasTrans)
            {
                var tpos = new Vector4();

                tpos.X = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 4);

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                packet.ReadXORByte(transportGuid, 0);
                tpos.O = packet.ReadSingle();
                tpos.Y = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 7);
                packet.ReadSByte("Transport Seat");
                packet.ReadXORByte(transportGuid, 5);
                packet.ReadXORByte(transportGuid, 2);
                packet.ReadUInt32("Transport Time");
                packet.ReadXORByte(transportGuid, 6);
                packet.ReadXORByte(transportGuid, 3);
                packet.ReadXORByte(transportGuid, 1);

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                tpos.Z = packet.ReadSingle();

                packet.WriteGuid("Transport Guid", transportGuid);
                packet.AddValue("Transport Position", tpos);
            }

            if (hasFallData)
            {
                packet.ReadSingle("Vertical Speed");
                packet.ReadUInt32("Fall time");

                if (hasFallDirection)
                {
                    packet.ReadSingle("Fall Sin");
                    packet.ReadSingle("Fall Cos");
                    packet.ReadSingle("Horizontal Speed");
                }
            }

            if (hasTime)
                packet.ReadUInt32("Timestamp");
            if (hasSplineElev)
                packet.ReadSingle("Spline elevation");
            if (hasO)
                pos.O = packet.ReadSingle();
            if (hasPitch)
                packet.ReadSingle("Pitch");

            packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
        }

        [Parser(Opcode.CMSG_MOVE_FORCE_FLIGHT_SPEED_CHANGE_ACK)]
        public static void HandleMoveForceFlightSpeedChangeAck434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();

            packet.ReadInt32("Unk Int32 1"); // ##
            pos.Z = packet.ReadSingle();
            pos.X = packet.ReadSingle();
            pos.Y = packet.ReadSingle();
            packet.ReadSingle("Speed");
            guid[4] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            packet.ReadBit();
            var hasMovementFlags = !packet.ReadBit();
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            guid[3] = packet.ReadBit();
            var hasMovementFlags2 = !packet.ReadBit();
            guid[7] = packet.ReadBit();
            var hasTrans = packet.ReadBit("Has transport");
            guid[0] = packet.ReadBit();
            var hasFallData = packet.ReadBit("Has fall data");
            var hasTime = !packet.ReadBit("Has timestamp");
            var hasPitch = !packet.ReadBit("Has pitch");
            packet.ReadBit("Has Spline");
            var hasO = !packet.ReadBit();
            guid[6] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            guid[1] = packet.ReadBit();

            if (hasTrans)
            {
                transportGuid[6] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
            }

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            if (hasMovementFlags2)
                packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            if (hasMovementFlags)
                packet.ReadBitsE<WowPacketParser.Enums.v4.MovementFlag>("Movement Flags", 30);

            packet.ParseBitStream(guid, 5, 6, 1, 7, 3, 0, 2, 4);

            if (hasTrans)
            {
                var tpos = new Vector4();

                packet.ReadXORByte(transportGuid, 6);
                packet.ReadXORByte(transportGuid, 0);
                packet.ReadSByte("Transport Seat");

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                tpos.O = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 1);
                packet.ReadXORByte(transportGuid, 5);

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                packet.ReadXORByte(transportGuid, 2);
                tpos.Z = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 7);
                packet.ReadXORByte(transportGuid, 3);
                packet.ReadXORByte(transportGuid, 4);
                tpos.X = packet.ReadSingle();
                packet.ReadUInt32("Transport Time");
                tpos.Y = packet.ReadSingle();

                packet.WriteGuid("Transport Guid", transportGuid);
                packet.AddValue("Transport Position", tpos);
            }

            if (hasTime)
                packet.ReadUInt32("Timestamp");
            if (hasSplineElev)
                packet.ReadSingle("Spline elevation");

            if (hasFallData)
            {
                if (hasFallDirection)
                {
                    packet.ReadSingle("Fall Cos");
                    packet.ReadSingle("Fall Sin");
                    packet.ReadSingle("Horizontal Speed");
                }

                packet.ReadUInt32("Fall time");
                packet.ReadSingle("Vertical Speed");
            }

            if (hasPitch)
                packet.ReadSingle("Pitch");
            if (hasO)
                pos.O = packet.ReadSingle();

            packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
        }

        [Parser(Opcode.CMSG_MOVE_SET_CAN_FLY_ACK)]
        public static void HandleMoveSetCanFlyAck434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();

            pos.Y = packet.ReadSingle();
            packet.ReadInt32("Unk Int32 1"); // ##
            pos.X = packet.ReadSingle();
            pos.Z = packet.ReadSingle();
            guid[3] = packet.ReadBit();
            var hasTime = !packet.ReadBit("Has timestamp");
            guid[4] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            var hasO = !packet.ReadBit();
            var hasFallData = packet.ReadBit("Has fall data");
            guid[2] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            var hasMovementFlags2 = !packet.ReadBit();
            packet.ReadBit();
            guid[7] = packet.ReadBit();
            packet.ReadBit("Has Spline");
            guid[6] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            var hasMovementFlags = !packet.ReadBit();
            var hasTrans = packet.ReadBit("Has transport");
            var hasPitch = !packet.ReadBit("Has pitch");

            if (hasTrans)
            {
                transportGuid[3] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
            }

            if (hasMovementFlags2)
                packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            if (hasMovementFlags)
                packet.ReadBitsE<WowPacketParser.Enums.v4.MovementFlag>("Movement Flags", 30);

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            packet.ParseBitStream(guid, 1, 0, 2, 3, 7, 6, 4, 5);

            if (hasTrans)
            {
                var tpos = new Vector4();

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                packet.ReadXORByte(transportGuid, 6);
                packet.ReadUInt32("Transport Time");

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                packet.ReadXORByte(transportGuid, 7);
                tpos.Z = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 3);
                tpos.Y = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 5);
                tpos.X = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 2);
                tpos.O = packet.ReadSingle();
                packet.ReadSByte("Transport Seat");
                packet.ReadXORByte(transportGuid, 1);
                packet.ReadXORByte(transportGuid, 0);
                packet.ReadXORByte(transportGuid, 4);

                packet.WriteGuid("Transport Guid", transportGuid);
                packet.AddValue("Transport Position", tpos);
            }

            if (hasFallData)
            {
                packet.ReadUInt32("Fall time");

                if (hasFallDirection)
                {
                    packet.ReadSingle("Fall Cos");
                    packet.ReadSingle("Horizontal Speed");
                    packet.ReadSingle("Fall Sin");
                }

                packet.ReadSingle("Vertical Speed");
            }

            if (hasPitch)
                packet.ReadSingle("Pitch");
            if (hasO)
                pos.O = packet.ReadSingle();
            if (hasTime)
                packet.ReadUInt32("Timestamp");
            if (hasSplineElev)
                packet.ReadSingle("Spline elevation");

            packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
        }

        [Parser(Opcode.CMSG_MOVE_FORCE_SWIM_SPEED_CHANGE_ACK)]
        public static void HandleMoveForceSwimSpeedChangeAck434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();

            pos.X = packet.ReadSingle();
            packet.ReadInt32("Unk Int32 1"); // ##
            packet.ReadSingle("Speed");
            pos.Y = packet.ReadSingle();
            pos.Z = packet.ReadSingle();
            guid[4] = packet.ReadBit();
            var hasO = !packet.ReadBit();
            var hasPitch = !packet.ReadBit("Has pitch");
            var hasTrans = packet.ReadBit("Has transport");
            var hasMovementFlags = !packet.ReadBit();
            var hasMovementFlags2 = !packet.ReadBit();
            guid[1] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            var hasFallData = packet.ReadBit("Has fall data");
            guid[2] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            var hasTime = !packet.ReadBit("Has timestamp");
            packet.ReadBit();
            guid[5] = packet.ReadBit();
            guid[6] = packet.ReadBit();
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            packet.ReadBit("Has Spline");
            guid[0] = packet.ReadBit();

            if (hasTrans)
            {
                transportGuid[1] = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
            }

            if (hasMovementFlags2)
                packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            if (hasMovementFlags)
                packet.ReadBitsE<WowPacketParser.Enums.v4.MovementFlag>("Movement Flags", 30);

            packet.ParseBitStream(guid, 2, 0, 6, 5, 1, 3, 4, 7);

            if (hasTrans)
            {
                var tpos = new Vector4();

                packet.ReadXORByte(transportGuid, 0);

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                tpos.X = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 7);
                packet.ReadSByte("Transport Seat");
                tpos.Y = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 5);
                tpos.Z = packet.ReadSingle();
                tpos.O = packet.ReadSingle();
                packet.ReadUInt32("Transport Time");
                packet.ReadXORByte(transportGuid, 2);
                packet.ReadXORByte(transportGuid, 6);
                packet.ReadXORByte(transportGuid, 3);
                packet.ReadXORByte(transportGuid, 1);
                packet.ReadXORByte(transportGuid, 4);

                packet.WriteGuid("Transport Guid", transportGuid);
                packet.AddValue("Transport Position", tpos);
            }

            if (hasFallData)
            {
                packet.ReadUInt32("Fall time");

                if (hasFallDirection)
                {
                    packet.ReadSingle("Fall Cos");
                    packet.ReadSingle("Fall Sin");
                    packet.ReadSingle("Horizontal Speed");
                }

                packet.ReadSingle("Vertical Speed");
            }

            if (hasO)
                pos.O = packet.ReadSingle();
            if (hasPitch)
                packet.ReadSingle("Pitch");
            if (hasSplineElev)
                packet.ReadSingle("Spline elevation");
            if (hasTime)
                packet.ReadUInt32("Timestamp");

            packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
        }

        [Parser(Opcode.CMSG_MOVE_FORCE_WALK_SPEED_CHANGE_ACK)]
        public static void HandleMoveForceWalkSpeedChangeAck434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();

            pos.Z = packet.ReadSingle();
            pos.Y = packet.ReadSingle();
            packet.ReadSingle("Speed");
            pos.X = packet.ReadSingle();
            packet.ReadInt32("Unk Int32 1"); // ##
            guid[0] = packet.ReadBit();
            var hasMovementFlags2 = !packet.ReadBit();
            var hasTime = !packet.ReadBit("Has timestamp");
            guid[4] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            var hasMovementFlags = !packet.ReadBit();
            var hasFallData = packet.ReadBit("Has fall data");
            var hasO = !packet.ReadBit();
            packet.ReadBit("Has Spline");
            packet.ReadBit();
            guid[3] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            var hasTrans = packet.ReadBit("Has transport");
            guid[7] = packet.ReadBit();
            guid[6] = packet.ReadBit();
            var hasPitch = !packet.ReadBit("Has pitch");

            if (hasTrans)
            {
                hasTransTime2 = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
            }

            if (hasMovementFlags)
                packet.ReadBitsE<WowPacketParser.Enums.v4.MovementFlag>("Movement Flags", 30);

            if (hasMovementFlags2)
                packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            packet.ParseBitStream(guid, 5, 6, 7, 2, 1, 3, 4, 0);

            if (hasFallData)
            {
                packet.ReadSingle("Vertical Speed");

                if (hasFallDirection)
                {
                    packet.ReadSingle("Fall Sin");
                    packet.ReadSingle("Horizontal Speed");
                    packet.ReadSingle("Fall Cos");
                }

                packet.ReadUInt32("Fall time");
            }

            if (hasTrans)
            {
                var tpos = new Vector4();

                tpos.Z = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 7);
                packet.ReadXORByte(transportGuid, 5);
                tpos.X = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 0);
                packet.ReadUInt32("Transport Time");

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                tpos.Y = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 2);
                packet.ReadXORByte(transportGuid, 4);
                packet.ReadXORByte(transportGuid, 1);
                packet.ReadXORByte(transportGuid, 3);
                tpos.O = packet.ReadSingle();
                packet.ReadSByte("Transport Seat");
                packet.ReadXORByte(transportGuid, 6);

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                packet.WriteGuid("Transport Guid", transportGuid);
                packet.AddValue("Transport Position", tpos);
            }

            if (hasSplineElev)
                packet.ReadSingle("Spline elevation");
            if (hasTime)
                packet.ReadUInt32("Timestamp");
            if (hasO)
                pos.O = packet.ReadSingle();
            if (hasPitch)
                packet.ReadSingle("Pitch");

            packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
        }

        [Parser(Opcode.CMSG_MOVE_FORCE_RUN_BACK_SPEED_CHANGE_ACK)]
        public static void HandleMoveForceRunBackSpeedChangeAck434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();

            packet.ReadSingle("Speed");
            packet.ReadInt32("Unk Int32 1"); // ##
            pos.X = packet.ReadSingle();
            pos.Z = packet.ReadSingle();
            pos.Y = packet.ReadSingle();
            guid[2] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            var hasTime = !packet.ReadBit("Has timestamp");
            var hasTrans = packet.ReadBit("Has transport");
            guid[1] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            var hasO = !packet.ReadBit();
            var hasFallData = packet.ReadBit("Has fall data");
            var hasMovementFlags2 = !packet.ReadBit();
            var hasMovementFlags = !packet.ReadBit();
            guid[7] = packet.ReadBit();
            packet.ReadBit("Has Spline");
            var hasPitch = !packet.ReadBit("Has pitch");
            guid[6] = packet.ReadBit();
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            packet.ReadBit();

            if (hasTrans)
            {
                transportGuid[3] = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
            }

            if (hasMovementFlags)
                packet.ReadBitsE<WowPacketParser.Enums.v4.MovementFlag>("Movement Flags", 30);

            if (hasMovementFlags2)
                packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            packet.ParseBitStream(guid, 0, 7, 2, 4, 3, 6, 5, 1);

            if (hasFallData)
            {
                packet.ReadUInt32("Fall time");

                if (hasFallDirection)
                {
                    packet.ReadSingle("Horizontal Speed");
                    packet.ReadSingle("Fall Cos");
                    packet.ReadSingle("Fall Sin");
                }

                packet.ReadSingle("Vertical Speed");
            }

            if (hasTrans)
            {
                var tpos = new Vector4();

                packet.ReadXORByte(transportGuid, 5);
                packet.ReadSByte("Transport Seat");
                packet.ReadXORByte(transportGuid, 6);
                packet.ReadXORByte(transportGuid, 1);
                packet.ReadXORByte(transportGuid, 0);
                packet.ReadXORByte(transportGuid, 2);
                tpos.Y = packet.ReadSingle();

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                tpos.X = packet.ReadSingle();
                tpos.O = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 3);
                packet.ReadUInt32("Transport Time");

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                packet.ReadXORByte(transportGuid, 4);
                tpos.Z = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 7);

                packet.WriteGuid("Transport Guid", transportGuid);
                packet.AddValue("Transport Position", tpos);
            }

            if (hasTime)
                packet.ReadUInt32("Timestamp");
            if (hasSplineElev)
                packet.ReadSingle("Spline elevation");
            if (hasO)
                pos.O = packet.ReadSingle();
            if (hasPitch)
                packet.ReadSingle("Pitch");

            packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
        }

        [Parser(Opcode.SMSG_MOVE_UPDATE_RUN_BACK_SPEED)]
        public static void HandleMoveUpdateRunBackSpeed434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasFallDirection = false;
            var pos = new Vector4();

            guid[1] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            var hasSplineElevation = !packet.ReadBit();
            guid[4] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            var hasFallData = packet.ReadBit();
            guid[6] = packet.ReadBit();
            var hasTime = !packet.ReadBit();
            guid[0] = packet.ReadBit();
            packet.ReadBit();
            var hasMovementFlags = !packet.ReadBit();
            var hasPitch = !packet.ReadBit();
            packet.ReadBit("Has spline data");
            guid[5] = packet.ReadBit();
            var hasMovementFlagsExtra = !packet.ReadBit();

            if (hasMovementFlagsExtra)
                packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            var hasO = !packet.ReadBit();

            if (hasMovementFlags)
                packet.ReadBitsE<WowPacketParser.Enums.v4.MovementFlag>("Movement flags", 30);

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            var hasTransport = packet.ReadBit();

            var hasTransportTime2 = false;
            var hasTransportTime3 = false;
            if (hasTransport)
            {
                transportGuid[5] = packet.ReadBit();
                hasTransportTime2 = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                hasTransportTime3 = packet.ReadBit();
            }

            guid[7] = packet.ReadBit();

            if (hasTransport)
            {
                var tpos = new Vector4();

                tpos.X = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 2);
                packet.ReadXORByte(transportGuid, 5);
                packet.ReadXORByte(transportGuid, 4);
                packet.ReadXORByte(transportGuid, 6);

                if (hasTransportTime2)
                    packet.ReadUInt32("Transport Time 2");

                packet.ReadXORByte(transportGuid, 0);
                packet.ReadXORByte(transportGuid, 3);
                tpos.Y = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 7);

                if (hasTransportTime3)
                    packet.ReadUInt32("Transport Time 3");

                tpos.Z = packet.ReadSingle();
                packet.ReadUInt32("Transport Time");
                packet.ReadSByte("Transport Seat");
                packet.ReadXORByte(transportGuid, 1);
                tpos.O = packet.ReadSingle();

                packet.WriteGuid("Transport Guid", transportGuid);
                packet.AddValue("Transport Position", tpos);
            }

            packet.ReadXORByte(guid, 4);

            if (hasFallData)
            {
                packet.ReadUInt32("Fall time");

                if (hasFallDirection)
                {
                    packet.ReadSingle("Horizontal speed");
                    packet.ReadSingle("Fall Cos");
                    packet.ReadSingle("Fall Sin");
                }

                packet.ReadSingle("Vertical speed");
            }

            if (hasTime)
                packet.ReadUInt32("Timestamp");

            if (hasSplineElevation)
                packet.ReadSingle("Spline elevation");

            packet.ReadXORByte(guid, 1);

            if (hasO)
                pos.O = packet.ReadSingle();

            packet.ReadXORByte(guid, 0);
            packet.ReadXORByte(guid, 5);
            packet.ReadXORByte(guid, 3);
            pos.X = packet.ReadSingle();
            pos.Y = packet.ReadSingle();

            if (hasPitch)
                packet.ReadSingle("Pitch");

            packet.ReadXORByte(guid, 7);
            packet.ReadSingle("Speed");
            packet.ReadXORByte(guid, 2);
            packet.ReadXORByte(guid, 6);
            pos.Z = packet.ReadSingle();

            packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
        }

        [Parser(Opcode.SMSG_MOVE_UPDATE_WALK_SPEED)]
        public static void HandleMoveUpdateWalkSpeed434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasFallDirection = false;
            var pos = new Vector4();

            var hasPitch = !packet.ReadBit();
            var hasO = !packet.ReadBit();
            packet.ReadBit();
            guid[3] = packet.ReadBit();
            var hasSplineElevation = !packet.ReadBit();
            guid[2] = packet.ReadBit();
            var hasTransport = packet.ReadBit();

            var hasTransportTime2 = false;
            var hasTransportTime3 = false;
            if (hasTransport)
            {
                transportGuid[6] = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                hasTransportTime2 = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                hasTransportTime3 = packet.ReadBit();
            }

            guid[7] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            var hasFallData = packet.ReadBit();
            guid[0] = packet.ReadBit();
            var hasMovementFlagsExtra = !packet.ReadBit();
            var hasTime = !packet.ReadBit();
            var hasMovementFlags = !packet.ReadBit();
            guid[6] = packet.ReadBit();

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            if (hasMovementFlagsExtra)
                packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            if (hasMovementFlags)
                packet.ReadBitsE<WowPacketParser.Enums.v4.MovementFlag>("Movement flags", 30);

            packet.ReadBit("Has spline data");
            guid[4] = packet.ReadBit();

            if (hasPitch)
                packet.ReadSingle("Pitch");

            if (hasTransport)
            {
                var tpos = new Vector4();

                packet.ReadXORByte(transportGuid, 6);
                packet.ReadXORByte(transportGuid, 0);
                packet.ReadXORByte(transportGuid, 4);
                packet.ReadXORByte(transportGuid, 2);
                tpos.X = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 7);
                packet.ReadUInt32("Transport Time");

                if (hasTransportTime2)
                    packet.ReadUInt32("Transport Time 2");

                tpos.Z = packet.ReadSingle();
                packet.ReadSByte("Transport Seat");
                packet.ReadXORByte(transportGuid, 5);

                if (hasTransportTime3)
                    packet.ReadUInt32("Transport Time 3");

                tpos.O = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 1);
                tpos.Y = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 3);

                packet.WriteGuid("Transport Guid", transportGuid);
                packet.AddValue("Transport Position", tpos);
            }

            if (hasSplineElevation)
                packet.ReadSingle("Spline elevation");

            if (hasFallData)
            {
                packet.ReadSingle("Vertical speed");

                if (hasFallDirection)
                {
                    packet.ReadSingle("Horizontal speed");
                    packet.ReadSingle("Fall Cos");
                    packet.ReadSingle("Fall Sin");
                }

                packet.ReadUInt32("Fall time");
            }

            packet.ReadXORByte(guid, 1);
            packet.ReadXORByte(guid, 4);
            packet.ReadXORByte(guid, 2);
            packet.ReadXORByte(guid, 6);
            packet.ReadXORByte(guid, 7);

            if (hasTime)
                packet.ReadUInt32("Timestamp");

            if (hasO)
                pos.O = packet.ReadSingle();

            pos.Y = packet.ReadSingle();
            packet.ReadXORByte(guid, 0);
            pos.Z = packet.ReadSingle();
            pos.X = packet.ReadSingle();
            packet.ReadXORByte(guid, 3);
            packet.ReadXORByte(guid, 5);
            packet.ReadSingle("Speed");

            packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
        }

        [Parser(Opcode.CMSG_FORCE_MOVE_ROOT_ACK)]
        public static void HandleForceMoveRootAck434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();

            pos.Y = packet.ReadSingle();
            pos.Z = packet.ReadSingle();
            packet.ReadInt32("Unk Int32 1"); // ##
            pos.X = packet.ReadSingle();
            guid[3] = packet.ReadBit();
            var hasTime = !packet.ReadBit("Has timestamp");
            var hasTrans = packet.ReadBit("Has transport");
            var hasPitch = !packet.ReadBit("Has pitch");
            guid[2] = packet.ReadBit();
            var hasO = !packet.ReadBit();
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            var hasFallData = packet.ReadBit("Has fall data");
            packet.ReadBit("Has Spline");
            guid[4] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            guid[6] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            var hasMovementFlags2 = !packet.ReadBit();
            var hasMovementFlags = !packet.ReadBit();
            packet.ReadBit();
            guid[5] = packet.ReadBit();

            if (hasTrans)
            {
                transportGuid[6] = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
            }

            if (hasMovementFlags2)
                packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            if (hasMovementFlags)
                packet.ReadBitsE<WowPacketParser.Enums.v4.MovementFlag>("Movement Flags", 30);

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            packet.ParseBitStream(guid, 5, 3, 1, 7, 4, 0, 6, 2);

            if (hasTrans)
            {
                var tpos = new Vector4();

                packet.ReadXORByte(transportGuid, 5);
                packet.ReadXORByte(transportGuid, 4);
                tpos.O = packet.ReadSingle();
                tpos.Z = packet.ReadSingle();

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                packet.ReadXORByte(transportGuid, 7);
                packet.ReadXORByte(transportGuid, 2);
                packet.ReadXORByte(transportGuid, 0);
                packet.ReadUInt32("Transport Time");
                packet.ReadXORByte(transportGuid, 6);
                packet.ReadXORByte(transportGuid, 3);
                tpos.X = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 1);
                tpos.Y = packet.ReadSingle();
                packet.ReadSByte("Transport Seat");

                packet.WriteGuid("Transport Guid", transportGuid);
                packet.AddValue("Transport Position", tpos);
            }

            if (hasTime)
                packet.ReadUInt32("Timestamp");

            if (hasFallData)
            {
                packet.ReadSingle("Vertical Speed");

                if (hasFallDirection)
                {
                    packet.ReadSingle("Horizontal Speed");
                    packet.ReadSingle("Fall Sin");
                    packet.ReadSingle("Fall Cos");
                }

                packet.ReadUInt32("Fall time");
            }

            if (hasO)
                pos.O = packet.ReadSingle();
            if (hasSplineElev)
                packet.ReadSingle("Spline elevation");
            if (hasPitch)
                packet.ReadSingle("Pitch");

            packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
        }

        [Parser(Opcode.CMSG_FORCE_MOVE_UNROOT_ACK)]
        public static void HandleForceMoveUnrootAck434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();

            packet.ReadInt32("Unk Int32 1"); // ##
            pos.Z = packet.ReadSingle();
            pos.Y = packet.ReadSingle();
            pos.X = packet.ReadSingle();
            guid[7] = packet.ReadBit();
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            packet.ReadBit("Has Spline");
            guid[5] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            packet.ReadBit();
            var hasTime = !packet.ReadBit("Has timestamp");
            var hasMovementFlags = !packet.ReadBit();
            var hasO = !packet.ReadBit();
            var hasPitch = !packet.ReadBit("Has pitch");
            guid[3] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            var hasTrans = packet.ReadBit("Has transport");
            var hasMovementFlags2 = !packet.ReadBit();
            guid[2] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            var hasFallData = packet.ReadBit("Has fall data");
            guid[6] = packet.ReadBit();

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            if (hasTrans)
            {
                transportGuid[0] = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
            }

            if (hasMovementFlags2)
                packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            if (hasMovementFlags)
                packet.ReadBitsE<WowPacketParser.Enums.v4.MovementFlag>("Movement Flags", 30);

            packet.ParseBitStream(guid, 7, 1, 0, 6, 2, 4, 5, 3);

            if (hasTrans)
            {
                var tpos = new Vector4();

                packet.ReadXORByte(transportGuid, 6);
                tpos.Z = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 2);
                packet.ReadXORByte(transportGuid, 3);
                tpos.Y = packet.ReadSingle();

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                packet.ReadXORByte(transportGuid, 5);
                packet.ReadSByte("Transport Seat");
                packet.ReadXORByte(transportGuid, 0);
                packet.ReadUInt32("Transport Time");

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                packet.ReadXORByte(transportGuid, 4);
                tpos.O = packet.ReadSingle();
                tpos.X = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 1);
                packet.ReadXORByte(transportGuid, 7);

                packet.WriteGuid("Transport Guid", transportGuid);
                packet.AddValue("Transport Position", tpos);
            }

            if (hasFallData)
            {
                if (hasFallDirection)
                {
                    packet.ReadSingle("Fall Cos");
                    packet.ReadSingle("Horizontal Speed");
                    packet.ReadSingle("Fall Sin");
                }

                packet.ReadSingle("Vertical Speed");
                packet.ReadUInt32("Fall time");
            }

            if (hasTime)
                packet.ReadUInt32("Timestamp");
            if (hasSplineElev)
                packet.ReadSingle("Spline elevation");
            if (hasO)
                pos.O = packet.ReadSingle();
            if (hasPitch)
                packet.ReadSingle("Pitch");

            packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
        }

        [Parser(Opcode.CMSG_MOVE_FALL_RESET)]
        public static void HandleMoveFallReset434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();

            pos.Z = packet.ReadSingle();
            pos.X = packet.ReadSingle();
            pos.Y = packet.ReadSingle();
            guid[1] = packet.ReadBit();
            var hasTime = !packet.ReadBit("Has timestamp");
            var hasMovementFlags = !packet.ReadBit();
            packet.ReadBit();
            guid[6] = packet.ReadBit();
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            var hasPitch = !packet.ReadBit("Has pitch");
            guid[3] = packet.ReadBit();
            var hasTrans = packet.ReadBit("Has transport");
            guid[2] = packet.ReadBit();
            var hasMovementFlags2 = !packet.ReadBit();
            var hasO = !packet.ReadBit();
            guid[4] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            packet.ReadBit("Has Spline");
            guid[7] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            var hasFallData = packet.ReadBit("Has fall data");

            if (hasTrans)
            {
                transportGuid[0] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
            }

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            if (hasMovementFlags2)
                packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            if (hasMovementFlags)
                packet.ReadBitsE<WowPacketParser.Enums.v4.MovementFlag>("Movement Flags", 30);

            packet.ParseBitStream(guid, 4, 0, 1, 7, 5, 2, 3, 6);

            if (hasTrans)
            {
                var tpos = new Vector4();

                tpos.X = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 7);
                packet.ReadXORByte(transportGuid, 2);
                packet.ReadXORByte(transportGuid, 3);
                packet.ReadXORByte(transportGuid, 5);
                tpos.Y = packet.ReadSingle();
                tpos.Z = packet.ReadSingle();
                packet.ReadSByte("Transport Seat");

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                packet.ReadXORByte(transportGuid, 6);

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                packet.ReadUInt32("Transport Time");
                packet.ReadXORByte(transportGuid, 0);
                tpos.O = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 4);
                packet.ReadXORByte(transportGuid, 1);

                packet.WriteGuid("Transport Guid", transportGuid);
                packet.AddValue("Transport Position", tpos);
            }

            if (hasFallData)
            {
                packet.ReadSingle("Vertical Speed");

                if (hasFallDirection)
                {
                    packet.ReadSingle("Fall Cos");
                    packet.ReadSingle("Fall Sin");
                    packet.ReadSingle("Horizontal Speed");
                }

                packet.ReadUInt32("Fall time");
            }

            if (hasO)
                pos.O = packet.ReadSingle();
            if (hasTime)
                packet.ReadUInt32("Timestamp");
            if (hasSplineElev)
                packet.ReadSingle("Spline elevation");
            if (hasPitch)
                packet.ReadSingle("Pitch");

            packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
        }

        [Parser(Opcode.CMSG_MOVE_FEATHER_FALL_ACK)]
        public static void HandleMoveFeatherFallAck434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();

            pos.Z = packet.ReadSingle();
            packet.ReadInt32("Unk Int32 1"); // ##
            pos.Y = packet.ReadSingle();
            pos.X = packet.ReadSingle();
            packet.ReadBit();
            guid[3] = packet.ReadBit();
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            guid[1] = packet.ReadBit();
            var hasPitch = !packet.ReadBit("Has pitch");
            guid[2] = packet.ReadBit();
            packet.ReadBit("Has Spline");
            var hasTime = !packet.ReadBit("Has timestamp");
            guid[5] = packet.ReadBit();
            var hasFallData = packet.ReadBit("Has fall data");
            var hasO = !packet.ReadBit();
            var hasTrans = packet.ReadBit("Has transport");
            guid[7] = packet.ReadBit();
            var hasMovementFlags2 = !packet.ReadBit();
            guid[6] = packet.ReadBit();
            var hasMovementFlags = !packet.ReadBit();
            guid[0] = packet.ReadBit();
            guid[4] = packet.ReadBit();

            if (hasTrans)
            {
                transportGuid[6] = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
            }

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            if (hasMovementFlags2)
                packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            if (hasMovementFlags)
                packet.ReadBitsE<WowPacketParser.Enums.v4.MovementFlag>("Movement Flags", 30);

            packet.ParseBitStream(guid, 6, 1, 7, 0, 5, 4, 3, 2);

            if (hasTrans)
            {
                var tpos = new Vector4();

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                tpos.O = packet.ReadSingle();
                tpos.Z = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 7);
                packet.ReadXORByte(transportGuid, 5);
                packet.ReadXORByte(transportGuid, 2);
                tpos.X = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 1);
                packet.ReadXORByte(transportGuid, 6);

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                packet.ReadUInt32("Transport Time");
                tpos.Y = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 4);
                packet.ReadXORByte(transportGuid, 0);
                packet.ReadSByte("Transport Seat");
                packet.ReadXORByte(transportGuid, 3);

                packet.WriteGuid("Transport Guid", transportGuid);
                packet.AddValue("Transport Position", tpos);
            }

            if (hasTime)
                packet.ReadUInt32("Timestamp");
            if (hasSplineElev)
                packet.ReadSingle("Spline elevation");
            if (hasO)
                pos.O = packet.ReadSingle();

            if (hasFallData)
            {
                packet.ReadSingle("Vertical Speed");

                if (hasFallDirection)
                {
                    packet.ReadSingle("Fall Cos");
                    packet.ReadSingle("Horizontal Speed");
                    packet.ReadSingle("Fall Sin");
                }

                packet.ReadUInt32("Fall time");
            }

            if (hasPitch)
                packet.ReadSingle("Pitch");

            packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
        }

        [Parser(Opcode.CMSG_MOVE_GRAVITY_DISABLE_ACK)]
        public static void HandleMoveGravityDisableAck434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();

            pos.Z = packet.ReadSingle();
            pos.Y = packet.ReadSingle();
            packet.ReadInt32("Unk Int32 1"); // ##
            pos.X = packet.ReadSingle();
            guid[3] = packet.ReadBit();
            var hasTrans = packet.ReadBit("Has transport");
            guid[4] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            var hasMovementFlags = !packet.ReadBit();
            var hasO = !packet.ReadBit();
            packet.ReadBit("Has Spline");
            guid[6] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            var hasTime = !packet.ReadBit("Has timestamp");
            var hasPitch = !packet.ReadBit("Has pitch");
            var hasMovementFlags2 = !packet.ReadBit();
            packet.ReadBit();
            guid[2] = packet.ReadBit();
            var hasFallData = packet.ReadBit("Has fall data");
            guid[1] = packet.ReadBit();

            if (hasTrans)
            {
                transportGuid[2] = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
            }

            if (hasMovementFlags)
                packet.ReadBitsE<WowPacketParser.Enums.v4.MovementFlag>("Movement Flags", 30);

            if (hasMovementFlags2)
                packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            packet.ParseBitStream(guid, 0, 2, 1, 3, 5, 7, 4, 6);

            if (hasTrans)
            {
                var tpos = new Vector4();

                tpos.Z = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 2);
                packet.ReadXORByte(transportGuid, 5);
                packet.ReadXORByte(transportGuid, 3);
                packet.ReadXORByte(transportGuid, 4);
                tpos.Y = packet.ReadSingle();
                packet.ReadUInt32("Transport Time");
                packet.ReadSByte("Transport Seat");
                packet.ReadXORByte(transportGuid, 6);
                packet.ReadXORByte(transportGuid, 1);
                packet.ReadXORByte(transportGuid, 0);
                tpos.O = packet.ReadSingle();

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                tpos.X = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 7);

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                packet.WriteGuid("Transport Guid", transportGuid);
                packet.AddValue("Transport Position", tpos);
            }

            if (hasFallData)
            {
                if (hasFallDirection)
                {
                    packet.ReadSingle("Fall Cos");
                    packet.ReadSingle("Fall Sin");
                    packet.ReadSingle("Horizontal Speed");
                }

                packet.ReadUInt32("Fall time");
                packet.ReadSingle("Vertical Speed");
            }

            if (hasO)
                pos.O = packet.ReadSingle();
            if (hasSplineElev)
                packet.ReadSingle("Spline elevation");
            if (hasTime)
                packet.ReadUInt32("Timestamp");
            if (hasPitch)
                packet.ReadSingle("Pitch");

            packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
        }

        [Parser(Opcode.CMSG_MOVE_GRAVITY_ENABLE_ACK)]
        public static void HandleMoveGravityEnableAck434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();

            pos.Z = packet.ReadSingle();
            pos.X = packet.ReadSingle();
            packet.ReadInt32("Unk Int32 1"); // ##
            pos.Y = packet.ReadSingle();
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            var hasMovementFlags2 = !packet.ReadBit();
            guid[6] = packet.ReadBit();
            var hasO = !packet.ReadBit();
            packet.ReadBit();
            guid[5] = packet.ReadBit();
            packet.ReadBit("Has Spline");
            guid[3] = packet.ReadBit();
            var hasTrans = packet.ReadBit("Has transport");
            var hasPitch = !packet.ReadBit("Has pitch");
            guid[1] = packet.ReadBit();
            var hasTime = !packet.ReadBit("Has timestamp");
            guid[2] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            var hasMovementFlags = !packet.ReadBit();
            var hasFallData = packet.ReadBit("Has fall data");
            guid[0] = packet.ReadBit();

            if (hasTrans)
            {
                hasTransTime3 = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
            }

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            if (hasMovementFlags)
                packet.ReadBitsE<WowPacketParser.Enums.v4.MovementFlag>("Movement Flags", 30);

            if (hasMovementFlags2)
                packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            packet.ParseBitStream(guid, 5, 4, 1, 7, 0, 2, 3, 6);

            if (hasFallData)
            {
                packet.ReadUInt32("Fall time");

                if (hasFallDirection)
                {
                    packet.ReadSingle("Horizontal Speed");
                    packet.ReadSingle("Fall Sin");
                    packet.ReadSingle("Fall Cos");
                }

                packet.ReadSingle("Vertical Speed");
            }

            if (hasTrans)
            {
                var tpos = new Vector4();

                packet.ReadXORByte(transportGuid, 1);
                tpos.X = packet.ReadSingle();

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                tpos.O = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 2);
                packet.ReadXORByte(transportGuid, 0);
                packet.ReadXORByte(transportGuid, 7);
                packet.ReadXORByte(transportGuid, 6);
                packet.ReadSByte("Transport Seat");
                packet.ReadXORByte(transportGuid, 3);
                packet.ReadXORByte(transportGuid, 4);
                tpos.Z = packet.ReadSingle();

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                packet.ReadXORByte(transportGuid, 5);
                packet.ReadUInt32("Transport Time");
                tpos.Y = packet.ReadSingle();

                packet.WriteGuid("Transport Guid", transportGuid);
                packet.AddValue("Transport Position", tpos);
            }

            if (hasSplineElev)
                packet.ReadSingle("Spline elevation");
            if (hasO)
                pos.O = packet.ReadSingle();
            if (hasPitch)
                packet.ReadSingle("Pitch");
            if (hasTime)
                packet.ReadUInt32("Timestamp");

            packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
        }

        [Parser(Opcode.CMSG_MOVE_HOVER_ACK)]
        public static void HandleMoveHoverAck434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();

            packet.ReadInt32("Unk Int32 1"); // ##
            pos.Z = packet.ReadSingle();
            pos.Y = packet.ReadSingle();
            pos.X = packet.ReadSingle();
            guid[4] = packet.ReadBit();
            var hasTrans = packet.ReadBit("Has transport");
            guid[2] = packet.ReadBit();
            var hasTime = !packet.ReadBit("Has timestamp");
            packet.ReadBit("Has Spline");
            var hasMovementFlags = !packet.ReadBit();
            guid[1] = packet.ReadBit();
            var hasPitch = !packet.ReadBit("Has pitch");
            guid[0] = packet.ReadBit();
            guid[6] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            packet.ReadBit();
            var hasFallData = packet.ReadBit("Has fall data");
            var hasMovementFlags2 = !packet.ReadBit();
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            var hasO = !packet.ReadBit();
            guid[3] = packet.ReadBit();

            if (hasTrans)
            {
                transportGuid[4] = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
            }

            if (hasMovementFlags)
                packet.ReadBitsE<WowPacketParser.Enums.v4.MovementFlag>("Movement Flags", 30);

            if (hasMovementFlags2)
                packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            packet.ParseBitStream(guid, 1, 4, 7, 2, 5, 6, 3, 0);

            if (hasTime)
                packet.ReadUInt32("Timestamp");

            if (hasFallData)
            {
                if (hasFallDirection)
                {
                    packet.ReadSingle("Fall Cos");
                    packet.ReadSingle("Fall Sin");
                    packet.ReadSingle("Horizontal Speed");
                }

                packet.ReadUInt32("Fall time");
                packet.ReadSingle("Vertical Speed");
            }

            if (hasO)
                pos.O = packet.ReadSingle();

            if (hasTrans)
            {
                var tpos = new Vector4();

                packet.ReadXORByte(transportGuid, 1);

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                packet.ReadXORByte(transportGuid, 2);
                packet.ReadXORByte(transportGuid, 7);
                packet.ReadXORByte(transportGuid, 3);
                packet.ReadXORByte(transportGuid, 4);
                packet.ReadSByte("Transport Seat");
                tpos.X = packet.ReadSingle();
                tpos.Z = packet.ReadSingle();
                tpos.O = packet.ReadSingle();
                packet.ReadUInt32("Transport Time");
                packet.ReadXORByte(transportGuid, 0);
                tpos.Y = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 5);

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                packet.ReadXORByte(transportGuid, 6);

                packet.WriteGuid("Transport Guid", transportGuid);
                packet.AddValue("Transport Position", tpos);
            }

            if (hasSplineElev)
                packet.ReadSingle("Spline elevation");
            if (hasPitch)
                packet.ReadSingle("Pitch");

            packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
        }

        [Parser(Opcode.CMSG_MOVE_KNOCK_BACK_ACK)]
        public static void HandleMoveKnockBackAck434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();

            pos.Y = packet.ReadSingle();
            pos.Z = packet.ReadSingle();
            packet.ReadInt32("Unk Int32 1"); // ##
            pos.X = packet.ReadSingle();
            guid[6] = packet.ReadBit();
            var hasO = !packet.ReadBit();
            var hasPitch = !packet.ReadBit("Has pitch");
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            guid[3] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            packet.ReadBit("Has Spline");
            guid[7] = packet.ReadBit();
            packet.ReadBit();
            var hasMovementFlags2 = !packet.ReadBit();
            var hasTime = !packet.ReadBit("Has timestamp");
            guid[0] = packet.ReadBit();
            var hasMovementFlags = !packet.ReadBit();
            var hasTrans = packet.ReadBit("Has transport");
            guid[5] = packet.ReadBit();
            var hasFallData = packet.ReadBit("Has fall data");

            if (hasMovementFlags)
                packet.ReadBitsE<WowPacketParser.Enums.v4.MovementFlag>("Movement Flags", 30);

            if (hasTrans)
            {
                transportGuid[4] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
            }

            if (hasMovementFlags2)
                packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            packet.ParseBitStream(guid, 4, 5, 1, 6, 0, 3, 2, 7);

            if (hasFallData)
            {
                if (hasFallDirection)
                {
                    packet.ReadSingle("Fall Cos");
                    packet.ReadSingle("Horizontal Speed");
                    packet.ReadSingle("Fall Sin");
                }

                packet.ReadUInt32("Fall time");
                packet.ReadSingle("Vertical Speed");
            }

            if (hasTime)
                packet.ReadUInt32("Timestamp");

            if (hasTrans)
            {
                var tpos = new Vector4();

                tpos.Y = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 4);

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                packet.ReadXORByte(transportGuid, 7);
                tpos.O = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 6);
                packet.ReadUInt32("Transport Time");
                packet.ReadXORByte(transportGuid, 3);
                packet.ReadXORByte(transportGuid, 1);

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                packet.ReadXORByte(transportGuid, 2);
                tpos.Z = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 0);
                packet.ReadXORByte(transportGuid, 5);
                tpos.X = packet.ReadSingle();
                packet.ReadSByte("Transport Seat");

                packet.WriteGuid("Transport Guid", transportGuid);
                packet.AddValue("Transport Position", tpos);
            }

            if (hasPitch)
                packet.ReadSingle("Pitch");
            if (hasSplineElev)
                packet.ReadSingle("Spline elevation");
            if (hasO)
                pos.O = packet.ReadSingle();

            packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
        }

        [Parser(Opcode.CMSG_MOVE_NOT_ACTIVE_MOVER)]
        public static void HandleMoveNotActiveMover434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();

            pos.Z = packet.ReadSingle();
            pos.X = packet.ReadSingle();
            pos.Y = packet.ReadSingle();
            var hasMovementFlags2 = !packet.ReadBit();
            var hasTrans = packet.ReadBit("Has transport");
            guid[6] = packet.ReadBit();
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            guid[3] = packet.ReadBit();
            packet.ReadBit();
            var hasTime = !packet.ReadBit("Has timestamp");
            guid[0] = packet.ReadBit();
            var hasO = !packet.ReadBit();
            guid[5] = packet.ReadBit();
            var hasPitch = !packet.ReadBit("Has pitch");
            guid[1] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            packet.ReadBit("Has Spline");
            guid[2] = packet.ReadBit();
            var hasFallData = packet.ReadBit("Has fall data");
            var hasMovementFlags = !packet.ReadBit();

            if (hasTrans)
            {
                transportGuid[4] = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
            }

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            if (hasMovementFlags)
                packet.ReadBitsE<WowPacketParser.Enums.v4.MovementFlag>("Movement Flags", 30);

            if (hasMovementFlags2)
                packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            packet.ParseBitStream(guid, 1, 0, 4, 2, 7, 5, 6, 3);

            if (hasFallData)
            {
                packet.ReadSingle("Vertical Speed");

                if (hasFallDirection)
                {
                    packet.ReadSingle("Fall Cos");
                    packet.ReadSingle("Fall Sin");
                    packet.ReadSingle("Horizontal Speed");
                }

                packet.ReadUInt32("Fall time");
            }

            if (hasTrans)
            {
                var tpos = new Vector4();

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                packet.ReadXORByte(transportGuid, 1);

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                tpos.O = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 0);
                packet.ReadSByte("Transport Seat");
                packet.ReadXORByte(transportGuid, 4);
                packet.ReadXORByte(transportGuid, 6);
                packet.ReadXORByte(transportGuid, 3);
                packet.ReadXORByte(transportGuid, 5);
                tpos.Y = packet.ReadSingle();
                tpos.X = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 2);
                tpos.Z = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 7);
                packet.ReadUInt32("Transport Time");

                packet.WriteGuid("Transport Guid", transportGuid);
                packet.AddValue("Transport Position", tpos);
            }

            if (hasTime)
                packet.ReadUInt32("Timestamp");
            if (hasSplineElev)
                packet.ReadSingle("Spline elevation");
            if (hasPitch)
                packet.ReadSingle("Pitch");
            if (hasO)
                pos.O = packet.ReadSingle();

            packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
        }

        [Parser(Opcode.CMSG_MOVE_WATER_WALK_ACK)]
        public static void HandleMoveWaterWalkAck434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();

            pos.Y = packet.ReadSingle();
            pos.Z = packet.ReadSingle();
            packet.ReadInt32("Unk Int32 1"); // ##
            pos.X = packet.ReadSingle();
            var hasTime = !packet.ReadBit("Has timestamp");
            var hasPitch = !packet.ReadBit("Has pitch");
            guid[5] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            var hasO = !packet.ReadBit();
            guid[1] = packet.ReadBit();
            var hasMovementFlags2 = !packet.ReadBit();
            guid[2] = packet.ReadBit();
            var hasMovementFlags = !packet.ReadBit();
            guid[3] = packet.ReadBit();
            var hasTrans = packet.ReadBit("Has transport");
            guid[6] = packet.ReadBit();
            var hasFallData = packet.ReadBit("Has fall data");
            guid[4] = packet.ReadBit();
            packet.ReadBit();
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            packet.ReadBit("Has Spline");

            if (hasTrans)
            {
                transportGuid[0] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
            }

            if (hasMovementFlags)
                packet.ReadBitsE<WowPacketParser.Enums.v4.MovementFlag>("Movement Flags", 30);

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            if (hasMovementFlags2)
                packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            packet.ParseBitStream(guid, 2, 7, 3, 5, 6, 0, 4, 1);

            if (hasTrans)
            {
                var tpos = new Vector4();

                tpos.X = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 1);

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                packet.ReadXORByte(transportGuid, 0);
                tpos.O = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 7);
                tpos.Y = packet.ReadSingle();

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                packet.ReadUInt32("Transport Time");
                packet.ReadXORByte(transportGuid, 5);
                packet.ReadSByte("Transport Seat");
                tpos.Z = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 3);
                packet.ReadXORByte(transportGuid, 2);
                packet.ReadXORByte(transportGuid, 6);
                packet.ReadXORByte(transportGuid, 4);

                packet.WriteGuid("Transport Guid", transportGuid);
                packet.AddValue("Transport Position", tpos);
            }

            if (hasSplineElev)
                packet.ReadSingle("Spline elevation");

            if (hasFallData)
            {
                if (hasFallDirection)
                {
                    packet.ReadSingle("Fall Cos");
                    packet.ReadSingle("Horizontal Speed");
                    packet.ReadSingle("Fall Sin");
                }

                packet.ReadSingle("Vertical Speed");
                packet.ReadUInt32("Fall time");
            }

            if (hasO)
                pos.O = packet.ReadSingle();
            if (hasTime)
                packet.ReadUInt32("Timestamp");
            if (hasPitch)
                packet.ReadSingle("Pitch");

            packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
        }

        [Parser(Opcode.SMSG_MOVE_DISABLE_GRAVITY)]
        public static void HandleMoveGravityDisable434(Packet packet)
        {
            var guid = packet.StartBitStream(0, 1, 5, 7, 6, 4, 3, 2);
            packet.ReadXORByte(guid, 7);
            packet.ReadXORByte(guid, 2);
            packet.ReadXORByte(guid, 0);
            packet.ReadInt32("Unk Int32"); // ##
            packet.ReadXORByte(guid, 5);
            packet.ReadXORByte(guid, 1);
            packet.ReadXORByte(guid, 3);
            packet.ReadXORByte(guid, 4);
            packet.ReadXORByte(guid, 6);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_ENABLE_GRAVITY)]
        public static void HandleMoveGravityEnable434(Packet packet)
        {
            var guid = packet.StartBitStream(1, 4, 7, 5, 2, 0, 3, 6);
            packet.ReadXORByte(guid, 3);
            packet.ReadInt32("Unk Int32"); // ##
            packet.ReadXORByte(guid, 7);
            packet.ReadXORByte(guid, 6);
            packet.ReadXORByte(guid, 4);
            packet.ReadXORByte(guid, 0);
            packet.ReadXORByte(guid, 1);
            packet.ReadXORByte(guid, 5);
            packet.ReadXORByte(guid, 2);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SET_NORMAL_FALL)]
        public static void HandleMoveSetNormalFall(Packet packet)
        {
            packet.ReadInt32("Unk Int32"); // ##
            var guid = packet.StartBitStream(3, 0, 1, 5, 7, 4, 6, 2);
            packet.ParseBitStream(guid, 2, 7, 1, 4, 5, 0, 3, 6);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SET_ACTIVE_MOVER)]
        public static void HandleMoveSetActiveMover434(Packet packet)
        {
            var guid = packet.StartBitStream(5, 7, 3, 6, 0, 4, 1, 2);
            packet.ParseBitStream(guid, 6, 2, 3, 0, 5, 7, 1, 4);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SET_COMPOUND_STATE)]
        public static void HandleMoveSetCompoundState434(Packet packet)
        {
            var guid = packet.StartBitStream(5, 0, 4, 1, 7, 6, 2, 3);

            var count = packet.ReadBits("StateChangeCount", 23);
            var HasVehicleRecID = new byte[count];
            var HasSpeed = new byte[count];
            var HasKnockBackInfo = new byte[count];
            var HasCollisionHeightInfo = new byte[count];

            for (int i = 0; i < count; ++i)
            {
                HasVehicleRecID[i] = packet.ReadBit("HasVehicleRecID", i);
                HasSpeed[i] = packet.ReadBit("HasSpeed", i);
                HasKnockBackInfo[i] = packet.ReadBit("HasKnockBackInfo", i);
                HasCollisionHeightInfo[i] = packet.ReadBit("HasCollisionHeightInfo", i);
                if (HasCollisionHeightInfo[i] != 0)
                    packet.ReadBits("CollisionHeightInfoReason", 2, i);
            }

            for (int i = 0; i < count; ++i)
            {
                if (HasCollisionHeightInfo[i] != 0)
                    packet.ReadSingle("CollisionHeightInfoHeight", i);

                if (HasKnockBackInfo[i] != 0)
                {
                    packet.ReadSingle("HorizontalSpeed", i);
                    packet.ReadVector2("Direction", i);
                    packet.ReadSingle("InitVerticalSpeed", i);
                }

                if (HasVehicleRecID[i] != 0)
                    packet.ReadInt32("VehicleRecID", i);

                packet.ReadInt32("SequenceIndex", i);

                if (HasSpeed[i] != 0)
                    packet.ReadSingle("Speed", i);

                var opcode = packet.ReadInt16();
                var opcodeName = Opcodes.GetOpcodeName(opcode, packet.Direction);
                packet.AddValue("Opcode", $"{ opcodeName } (0x{ opcode.ToString("X4") })", i);

            }

            packet.ParseBitStream(guid, 2, 1, 4, 5, 6, 7, 0, 3);
            packet.WriteGuid("MoverGUID", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SET_FLIGHT_SPEED)]
        public static void HandleMoveSetFlightSpeed434(Packet packet)
        {
            var guid = packet.StartBitStream(0, 5, 1, 6, 3, 2, 7, 4);
            packet.ReadXORByte(guid, 0);
            packet.ReadXORByte(guid, 1);
            packet.ReadXORByte(guid, 7);
            packet.ReadXORByte(guid, 5);
            packet.ReadSingle("Speed");
            packet.ReadInt32("Unk Int32"); // ##
            packet.ReadXORByte(guid, 2);
            packet.ReadXORByte(guid, 6);
            packet.ReadXORByte(guid, 3);
            packet.ReadXORByte(guid, 4);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SET_FLIGHT_BACK_SPEED)]
        public static void HandleMoveSetFlightBackSpeed434(Packet packet)
        {
            var guid = packet.StartBitStream(1, 2, 6, 4, 7, 3, 0, 5);
            packet.ReadXORByte(guid, 3);
            packet.ReadInt32("Unk Int32"); // ##
            packet.ReadXORByte(guid, 6);
            packet.ReadSingle("Speed");
            packet.ReadXORByte(guid, 1);
            packet.ReadXORByte(guid, 2);
            packet.ReadXORByte(guid, 4);
            packet.ReadXORByte(guid, 0);
            packet.ReadXORByte(guid, 5);
            packet.ReadXORByte(guid, 7);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SET_RUN_BACK_SPEED)]
        public static void HandleMoveSetRunBackSpeed434(Packet packet)
        {
            var guid = packet.StartBitStream(0, 6, 2, 1, 3, 5, 4, 7);
            packet.ReadXORByte(guid, 5);
            packet.ReadInt32("Unk Int32"); // ##
            packet.ReadSingle("Speed");
            packet.ReadXORByte(guid, 0);
            packet.ReadXORByte(guid, 4);
            packet.ReadXORByte(guid, 7);
            packet.ReadXORByte(guid, 3);
            packet.ReadXORByte(guid, 1);
            packet.ReadXORByte(guid, 2);
            packet.ReadXORByte(guid, 6);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SET_SWIM_SPEED)]
        public static void HandleMoveSetSwimSpeed434(Packet packet)
        {
            var guid = packet.StartBitStream(5, 4, 7, 3, 2, 0, 1, 6);
            packet.ReadXORByte(guid, 0);
            packet.ReadInt32("Unk Int32"); // ##
            packet.ReadXORByte(guid, 6);
            packet.ReadXORByte(guid, 3);
            packet.ReadXORByte(guid, 5);
            packet.ReadXORByte(guid, 2);
            packet.ReadSingle("Speed");
            packet.ReadXORByte(guid, 1);
            packet.ReadXORByte(guid, 7);
            packet.ReadXORByte(guid, 4);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SET_SWIM_BACK_SPEED)]
        public static void HandleMoveSetSwimBackSpeed434(Packet packet)
        {
            var guid = packet.StartBitStream(4, 2, 3, 6, 5, 1, 0, 7);
            packet.ReadInt32("Unk Int32"); // ##
            packet.ReadXORByte(guid, 0);
            packet.ReadXORByte(guid, 3);
            packet.ReadXORByte(guid, 4);
            packet.ReadXORByte(guid, 6);
            packet.ReadXORByte(guid, 5);
            packet.ReadXORByte(guid, 1);
            packet.ReadSingle("Speed");
            packet.ReadXORByte(guid, 7);
            packet.ReadXORByte(guid, 2);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SET_WALK_SPEED)]
        public static void HandleMoveSetWalkSpeed434(Packet packet)
        {
            var guid = packet.StartBitStream(0, 4, 5, 2, 3, 1, 6, 7);
            packet.ReadXORByte(guid, 6);
            packet.ReadXORByte(guid, 1);
            packet.ReadXORByte(guid, 5);
            packet.ReadSingle("Speed");
            packet.ReadXORByte(guid, 2);
            packet.ReadInt32("Unk Int32"); // ##
            packet.ReadXORByte(guid, 4);
            packet.ReadXORByte(guid, 0);
            packet.ReadXORByte(guid, 7);
            packet.ReadXORByte(guid, 3);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SET_TURN_RATE)]
        public static void HandleMoveSetTurnRate434(Packet packet)
        {
            var guid = packet.StartBitStream(7, 2, 1, 0, 4, 5, 6, 3);
            packet.ReadXORByte(guid, 5);
            packet.ReadXORByte(guid, 7);
            packet.ReadXORByte(guid, 2);
            packet.ReadSingle("Rate");
            packet.ReadXORByte(guid, 3);
            packet.ReadXORByte(guid, 1);
            packet.ReadXORByte(guid, 0);
            packet.ReadInt32("Unk Int32"); // ##
            packet.ReadXORByte(guid, 6);
            packet.ReadXORByte(guid, 4);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SET_PITCH_RATE)]
        public static void HandleMoveSetPitchRate434(Packet packet)
        {
            var guid = packet.StartBitStream(1, 2, 6, 7, 0, 3, 5, 4);
            packet.ReadSingle("Rate");
            packet.ReadXORByte(guid, 6);
            packet.ReadXORByte(guid, 4);
            packet.ReadXORByte(guid, 0);
            packet.ReadInt32("Unk Int32"); // ##
            packet.ReadXORByte(guid, 1);
            packet.ReadXORByte(guid, 2);
            packet.ReadXORByte(guid, 7);
            packet.ReadXORByte(guid, 3);
            packet.ReadXORByte(guid, 5);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SPLINE_SET_FEATHER_FALL)]
        public static void HandleSplineMoveSetFeatherFall434(Packet packet)
        {
            var guid = packet.StartBitStream(3, 2, 7, 5, 4, 6, 1, 0);
            packet.ParseBitStream(guid, 1, 4, 7, 6, 2, 0, 5, 3);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SPLINE_SET_LAND_WALK)]
        public static void HandleSplineMoveSetLandWalk434(Packet packet)
        {
            var guid = packet.StartBitStream(5, 0, 4, 6, 7, 2, 3, 1);
            packet.ParseBitStream(guid, 5, 7, 3, 4, 1, 2, 0, 6);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SPLINE_SET_ANIM)]
        public static void HandleSplineMoveSetAnim(Packet packet)
        {
            packet.ReadPackedGuid("Guid");
            packet.ReadUInt32E<MovementAnimationState>("Animation");
        }

        [Parser(Opcode.SMSG_MOVE_UPDATE_KNOCK_BACK)]
        public static void HandleMoveUpdateKnockBack434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasFallDirection = false;
            var pos = new Vector4();

            packet.ReadBit();
            guid[4] = packet.ReadBit();
            var hasMovementFlags = !packet.ReadBit();
            var hasPitch = !packet.ReadBit();
            var hasTime = !packet.ReadBit();
            guid[1] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            packet.ReadBit("Has spline data");
            var hasTransport = packet.ReadBit();

            var hasTransportTime2 = false;
            var hasTransportTime3 = false;
            if (hasTransport)
            {
                transportGuid[7] = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                hasTransportTime2 = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                hasTransportTime3 = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
            }

            guid[5] = packet.ReadBit();
            var hasSplineElevation = !packet.ReadBit();
            var hasMovementFlagsExtra = !packet.ReadBit();
            guid[6] = packet.ReadBit();

            if (hasMovementFlags)
                packet.ReadBitsE<MovementFlag>("Movement flags", 30);

            var hasFallData = packet.ReadBit();

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            var hasO = !packet.ReadBit();

            if (hasMovementFlagsExtra)
                packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            if (hasO)
                pos.O = packet.ReadSingle();

            if (hasFallData)
            {
                if (hasFallDirection)
                {
                    packet.ReadSingle("Fall Cos");
                    packet.ReadSingle("Horizontal speed");
                    packet.ReadSingle("Fall Sin");
                }

                packet.ReadUInt32("Fall time");
                packet.ReadSingle("Vertical speed");
            }

            if (hasSplineElevation)
                packet.ReadSingle("Spline elevation");

            packet.ReadXORByte(guid, 3);

            if (hasTransport)
            {
                var tpos = new Vector4();

                packet.ReadXORByte(transportGuid, 5);

                if (hasTransportTime3)
                    packet.ReadUInt32("Transport Time 3");

                packet.ReadXORByte(transportGuid, 7);
                packet.ReadSByte("Transport Seat");
                packet.ReadXORByte(transportGuid, 3);
                packet.ReadXORByte(transportGuid, 6);
                tpos.Z = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 1);
                tpos.Y = packet.ReadSingle();
                tpos.X = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 2);
                packet.ReadXORByte(transportGuid, 0);
                tpos.O = packet.ReadSingle();
                packet.ReadUInt32("Transport Time");
                packet.ReadXORByte(transportGuid, 4);

                if (hasTransportTime2)
                    packet.ReadUInt32("Transport Time 2");

                packet.WriteGuid("Transport Guid", transportGuid);
                packet.AddValue("Transport Position", tpos);
            }

            if (hasPitch)
                packet.ReadSingle("Pitch");

            pos.Z = packet.ReadSingle();

            if (hasTime)
                packet.ReadUInt32("Timestamp");

            pos.X = packet.ReadSingle();
            packet.ReadXORByte(guid, 4);
            packet.ReadXORByte(guid, 6);
            packet.ReadXORByte(guid, 7);
            packet.ReadXORByte(guid, 2);
            packet.ReadXORByte(guid, 1);

            pos.Y = packet.ReadSingle();
            packet.ReadXORByte(guid, 0);
            packet.ReadXORByte(guid, 5);

            packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
        }

        [Parser(Opcode.SMSG_MOVE_SET_WATER_WALK)]
        public static void HandleMoveSetWaterWalk(Packet packet)
        {
            var guid = packet.StartBitStream(6, 1, 4, 2, 3, 7, 5, 0);
            packet.ParseBitStream(guid, 0, 6, 3, 7, 4, 2, 5, 1);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SET_LAND_WALK)]
        public static void HandleMoveSetLandWalk(Packet packet)
        {
            var guid = packet.StartBitStream(5, 0, 4, 6, 7, 2, 3, 1);
            packet.ParseBitStream(guid, 5, 7, 3, 4, 1, 2, 0, 6);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SET_HOVERING)]
        public static void HandleMoveSetHover(Packet packet)
        {
            var guid = packet.StartBitStream(3, 7, 0, 1, 4, 6, 2, 5);
            packet.ParseBitStream(guid, 2, 4, 3, 1, 7, 0, 5, 6);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_UNSET_HOVERING)]
        public static void HandleMoveUnsetHover(Packet packet)
        {
            var guid = packet.StartBitStream(6, 7, 4, 0, 3, 1, 5, 2);
            packet.ParseBitStream(guid, 4, 5, 3, 0, 2, 7, 6, 1);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SET_FEATHER_FALL)]
        public static void HandleMoveSetFeatherFall(Packet packet)
        {
            var guid = packet.StartBitStream(3, 1, 7, 0, 4, 2, 5, 6);
            packet.ReadXORByte(guid, 5);
            packet.ReadXORByte(guid, 7);
            packet.ReadXORByte(guid, 2);
            packet.ReadInt32("Movement Counter");
            packet.ReadXORByte(guid, 0);
            packet.ReadXORByte(guid, 3);
            packet.ReadXORByte(guid, 4);
            packet.ReadXORByte(guid, 1);
            packet.ReadXORByte(guid, 6);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SET_CAN_FLY)]
        public static void HandleMoveSetCanFly(Packet packet)
        {
            var guid = packet.StartBitStream(1, 6, 5, 0, 7, 4, 2, 3);
            packet.ReadXORByte(guid, 6);
            packet.ReadXORByte(guid, 3);
            packet.ReadInt32("Movement Counter");
            packet.ReadXORByte(guid, 2);
            packet.ReadXORByte(guid, 1);
            packet.ReadXORByte(guid, 4);
            packet.ReadXORByte(guid, 7);
            packet.ReadXORByte(guid, 0);
            packet.ReadXORByte(guid, 5);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_UNSET_CAN_FLY)]
        public static void HandleMoveUnsetCanFly(Packet packet)
        {
            var guid = packet.StartBitStream(1, 4, 2, 5, 0, 3, 6, 7);
            packet.ReadXORByte(guid, 4);
            packet.ReadXORByte(guid, 6);
            packet.ReadInt32("Movement Counter");
            packet.ReadXORByte(guid, 1);
            packet.ReadXORByte(guid, 0);
            packet.ReadXORByte(guid, 2);
            packet.ReadXORByte(guid, 3);
            packet.ReadXORByte(guid, 5);
            packet.ReadXORByte(guid, 7);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_KNOCK_BACK)]
        public static void HandleMoveKnockBack(Packet packet)
        {
            var guid = packet.StartBitStream(0, 3, 6, 7, 2, 5, 1, 4);
            packet.ReadXORByte(guid, 1);
            packet.ReadSingle("DirectionY");
            packet.ReadInt32("SequenceIndex");
            packet.ReadXORByte(guid, 6);
            packet.ReadXORByte(guid, 7);
            packet.ReadSingle("HorzSpeed");
            packet.ReadXORByte(guid, 4);
            packet.ReadXORByte(guid, 5);
            packet.ReadXORByte(guid, 3);
            packet.ReadSingle("VertSpeed");
            packet.ReadSingle("DirectionX");
            packet.ReadXORByte(guid, 2);
            packet.ReadXORByte(guid, 0);
            packet.WriteGuid("MoverGUID", guid);
        }

        private static UniversalSplineFlag ToUniversal(this SplineFlag flags)
        {
            UniversalSplineFlag universal = UniversalSplineFlag.SplineFlagNone;
            if (flags.HasFlag(SplineFlag.AnimTierSwim))
                universal |= UniversalSplineFlag.AnimTierSwim;
            if (flags.HasFlag(SplineFlag.AnimTierHover))
                universal |= UniversalSplineFlag.AnimTierHover;
            if (flags.HasFlag(SplineFlag.AnimTierSubmerged))
                universal |= UniversalSplineFlag.AnimTierSubmerged;
            if (flags.HasFlag(SplineFlag.FallingSlow))
                universal |= UniversalSplineFlag.FallingSlow;
            if (flags.HasFlag(SplineFlag.Done))
                universal |= UniversalSplineFlag.Done;
            if (flags.HasFlag(SplineFlag.Falling))
                universal |= UniversalSplineFlag.Falling;
            if (flags.HasFlag(SplineFlag.No_Spline))
                universal |= UniversalSplineFlag.NoSpline;
            if (flags.HasFlag(SplineFlag.Flying))
                universal |= UniversalSplineFlag.Flying;
            if (flags.HasFlag(SplineFlag.OrientationFixed))
                universal |= UniversalSplineFlag.OrientationFixed;
            if (flags.HasFlag(SplineFlag.Catmullrom))
                universal |= UniversalSplineFlag.Catmullrom;
            if (flags.HasFlag(SplineFlag.Cyclic))
                universal |= UniversalSplineFlag.Cyclic;
            if (flags.HasFlag(SplineFlag.Enter_Cycle))
                universal |= UniversalSplineFlag.EnterCycle;
            if (flags.HasFlag(SplineFlag.Frozen))
                universal |= UniversalSplineFlag.Frozen;
            if (flags.HasFlag(SplineFlag.TransportEnter))
                universal |= UniversalSplineFlag.TransportEnter;
            if (flags.HasFlag(SplineFlag.TransportExit))
                universal |= UniversalSplineFlag.TransportExit;
            if (flags.HasFlag(SplineFlag.Backward))
                universal |= UniversalSplineFlag.OrientationInversed;
            if (flags.HasFlag(SplineFlag.SmoothGroundPath))
                universal |= UniversalSplineFlag.SmoothGroundPath;
            if (flags.HasFlag(SplineFlag.CanSwim))
                universal |= UniversalSplineFlag.CanSwim;
            if (flags.HasFlag(SplineFlag.UncompressedPath))
                universal |= UniversalSplineFlag.UncompressedPath;
            if (flags.HasFlag(SplineFlag.Animation))
                universal |= UniversalSplineFlag.Animation;
            if (flags.HasFlag(SplineFlag.Parabolic))
                universal |= UniversalSplineFlag.Parabolic;
            if (flags.HasFlag(SplineFlag.Final_Point))
                universal |= UniversalSplineFlag.FinalPoint;
            if (flags.HasFlag(SplineFlag.Final_Target))
                universal |= UniversalSplineFlag.FinalTarget;
            if (flags.HasFlag(SplineFlag.Final_Angle))
                universal |= UniversalSplineFlag.FinalAngle;
            return universal;
        }
    }
}
