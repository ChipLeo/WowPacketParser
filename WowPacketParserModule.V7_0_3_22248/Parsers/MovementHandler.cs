﻿using System;
using System.Collections.Generic;
using System.Linq;
using WowPacketParser.DBC;
using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;
using WowPacketParser.Proto;
using WowPacketParserModule.V7_0_3_22248.Enums;
using CoreParsers = WowPacketParser.Parsing.Parsers;
using MovementFlag = WowPacketParser.Enums.v4.MovementFlag;
using MovementFlag2 = WowPacketParser.Enums.v7.MovementFlag2;
using SplineFacingType = WowPacketParserModule.V6_0_2_19033.Enums.SplineFacingType;
using SplineFlag = WowPacketParserModule.V7_0_3_22248.Enums.SplineFlag;

namespace WowPacketParserModule.V7_0_3_22248.Parsers
{
    public static class MovementHandler
    {
        public static void ReadFallData(Packet packet, params object[] idx)
        {
            packet.ReadUInt32("FallTime", idx);
            packet.ReadSingle("JumpVelocity", idx);

            packet.ResetBitReader();

            var bit20 = packet.ReadBit("HasFallDirection", idx);
            if (bit20)
            {
                packet.ReadVector2("Direction", idx);
                packet.ReadSingle("HorizontalSpeed", idx);
            }
        }

        public static MovementInfo ReadMovementStats(Packet packet, params object[] idx)
        {
            MovementInfo info = new();
            info.MoverGuid = packet.ReadPackedGuid128("MoverGUID", idx);

            if (ClientVersion.AddedInVersion(ClientBranch.Retail, ClientVersionBuild.V9_2_0_42423) ||
                ClientVersion.AddedInVersion(ClientBranch.Classic, ClientVersionBuild.V1_14_1_40666) ||
                ClientVersion.AddedInVersion(ClientBranch.TBC, ClientVersionBuild.V2_5_3_41812) ||
                ClientVersion.AddedInVersion(ClientBranch.WotLK, ClientVersionBuild.V3_4_0_45166))
            {
                info.Flags = (uint)packet.ReadUInt32E<MovementFlag>("MovementFlags", idx);
                info.Flags2 = (uint)packet.ReadUInt32E<MovementFlag2>("MovementFlags2", idx);
                info.Flags3 = (uint)packet.ReadUInt32E<MovementFlag3>("MovementFlags3", idx);
            }

            packet.ReadInt32("MoveTime", idx);
            var position = packet.ReadVector4("Position", idx);
            info.Position = new Vector3 { X = position.X, Y = position.Y, Z = position.Z };
            info.Orientation = position.O;

            packet.ReadSingle("Pitch", idx);
            packet.ReadSingle("SplineElevation", idx);

            var int152 = packet.ReadInt32("RemoveForcesCount", idx);
            packet.ReadInt32("MoveIndex", idx);

            for (var i = 0; i < int152; i++)
                packet.ReadPackedGuid128("RemoveForcesIDs", idx, i);

            packet.ResetBitReader();

            if (ClientVersion.RemovedInVersion(ClientBranch.Retail, ClientVersionBuild.V9_2_0_42423) ||
                ClientVersion.RemovedInVersion(ClientBranch.Classic, ClientVersionBuild.V1_14_1_40666) ||
                ClientVersion.RemovedInVersion(ClientBranch.TBC, ClientVersionBuild.V2_5_3_41812) ||
                ClientVersion.RemovedInVersion(ClientBranch.WotLK, ClientVersionBuild.V3_4_0_45166))
            {
                info.Flags = (uint)packet.ReadBitsE<MovementFlag>("MovementFlags", 30, idx);
                info.Flags2 = (uint)packet.ReadBitsE<MovementFlag2>("MovementFlags2", 18, idx);
            }

            var hasTransport = packet.ReadBit("HasTransportData", idx);
            var hasFall = packet.ReadBit("HasFallData", idx);
            packet.ReadBit("HasSpline", idx);
            packet.ReadBit("HeightChangeFailed", idx);
            packet.ReadBit("RemoteTimeValid", idx);
            var hasInertia = (ClientVersion.AddedInVersion(ClientBranch.Retail, ClientVersionBuild.V9_2_0_42423) ||
                              ClientVersion.AddedInVersion(ClientBranch.Classic, ClientVersionBuild.V1_14_1_40666) ||
                              ClientVersion.AddedInVersion(ClientBranch.TBC, ClientVersionBuild.V2_5_3_41812) ||
                              ClientVersion.AddedInVersion(ClientBranch.WotLK, ClientVersionBuild.V3_4_0_45166)) &&
                              packet.ReadBit("HasInertia", idx);

            if (hasTransport)
                ReadTransportData(packet, idx, "TransportData");

            if (hasInertia)
            {
                packet.ReadPackedGuid128("GUID", idx, "Inertia");
                packet.ReadVector3("Force", idx, "Inertia");
                packet.ReadUInt32("Lifetime", idx, "Inertia");
            }

            if (hasFall)
                ReadFallData(packet, idx, "FallData");

            return info;
        }

        public static MovementInfo ReadMovementAck(Packet packet, params object[] idx)
        {
            var stats = ReadMovementStats(packet, idx);
            packet.ReadInt32("AckIndex", idx);
            return stats;
        }

        public static void ReadMovementForce(Packet packet, params object[] idx)
        {
            packet.ReadPackedGuid128("ID", idx);
            packet.ReadVector3("Origin", idx);
            packet.ReadVector3("Direction", idx);
            //packet.ReadVector3("TransportPosition", idx);
            packet.ReadInt32("TransportID", idx);
            packet.ReadSingle("Magnitude", idx);

            packet.ResetBitReader();

            packet.ReadBits("Type", 2, idx);
        }

        public static void ReadMovementMonsterSpline(Packet packet, Vector3 pos, params object[] indexes)
        {
            PacketMonsterMove monsterMove = packet.Holder.MonsterMove;
            monsterMove.Id = packet.ReadUInt32("Id", indexes);
            monsterMove.Destination = packet.ReadVector3("Destination", indexes);

            packet.ResetBitReader();

            packet.ReadBit("CrzTeleport", indexes);
            packet.ReadBits("StopDistanceTolerance", 3, indexes);

            ReadMovementSpline(packet, pos, indexes, "MovementSpline");
        }

        public static void ReadMonsterSplineFilter(Packet packet, params object[] indexes)
        {
            var count = packet.ReadUInt32("MonsterSplineFilterKey", indexes);
            packet.ReadSingle("BaseSpeed", indexes);
            packet.ReadUInt16("StartOffset", indexes);
            packet.ReadSingle("DistToPrevFilterKey", indexes);
            packet.ReadUInt16("AddedToStart", indexes);

            for (int i = 0; i < count; i++)
            {
                packet.ReadInt16("IDx", indexes, i);
                packet.ReadUInt16("Speed", indexes, i);
            }

            packet.ResetBitReader();
            packet.ReadBits("FilterFlags", 2, indexes);
        }

        public static void ReadMonsterSplineSpellEffectExtraData(Packet packet, params object[] indexes)
        {
            packet.ReadPackedGuid128("TargetGUID", indexes);
            packet.ReadUInt32("SpellVisualID", indexes);
            packet.ReadUInt32("ProgressCurveID", indexes);
            packet.ReadUInt32("ParabolicCurveID", indexes);
        }

        public static void ReadMovementSpline(Packet packet, Vector3 pos, params object[] indexes)
        {
            PacketMonsterMove monsterMove = packet.Holder.MonsterMove;
            SplineJump jump = monsterMove.Jump = new();
            monsterMove.Flags = packet.ReadInt32E<SplineFlag>("Flags", indexes).ToUniversal();
            packet.ReadByte("AnimTier", indexes);
            packet.ReadUInt32("TierTransStartTime", indexes);
            monsterMove.ElapsedTime = packet.ReadInt32("Elapsed", indexes);
            monsterMove.MoveTime = packet.ReadUInt32("MoveTime", indexes);
            jump.Gravity = packet.ReadSingle("JumpGravity", indexes);
            jump.Duration = packet.ReadUInt32("SpecialTime", indexes);

            packet.ReadByte("Mode", indexes);
            packet.ReadByte("VehicleExitVoluntary", indexes);

            monsterMove.TransportGuid = packet.ReadPackedGuid128("TransportGUID", indexes);
            monsterMove.VehicleSeat = packet.ReadSByte("VehicleSeat", indexes);

            packet.ResetBitReader();

            var type = packet.ReadBitsE<SplineFacingType>("Face", 2, indexes);
            var pointsCount = packet.ReadBits("PointsCount", 16, indexes);
            var packedDeltasCount = packet.ReadBits("PackedDeltasCount", 16, indexes);
            var hasSplineFilter = packet.ReadBit("HasSplineFilter", indexes);
            var hasSpellEffectExtraData = packet.ReadBit("HasSpellEffectExtraData", indexes);

            if (hasSplineFilter)
                ReadMonsterSplineFilter(packet, indexes, "MonsterSplineFilter");

            switch (type)
            {
                case SplineFacingType.Spot:
                    monsterMove.LookPosition = packet.ReadVector3("FaceSpot", indexes);
                    break;
                case SplineFacingType.Target:
                    SplineLookTarget lookTarget = monsterMove.LookTarget = new();
                    lookTarget.Orientation = packet.ReadSingle("FaceDirection", indexes);
                    lookTarget.Target = packet.ReadPackedGuid128("FacingGUID", indexes);
                    break;
                case SplineFacingType.Angle:
                    monsterMove.LookOrientation = packet.ReadSingle("FaceDirection", indexes);
                    break;
            }

            var endpos = new Vector3();
            double distance = 0.0f;
            if (pointsCount > 0)
            {
                var prevpos = pos;
                for (var i = 0; i < pointsCount; ++i)
                {
                    var spot = packet.ReadVector3("Points", indexes, i);
                    monsterMove.Points.Add(spot);
                    distance += Vector3.GetDistance(prevpos, spot);
                    prevpos = spot;

                    // client always taking first point
                    if (i == 0)
                        endpos = spot;
                }
            }

            if (packedDeltasCount > 0)
            {
                // Calculate mid pos
                var mid = (pos + endpos) * 0.5f;

                // ignore distance set by Points array if packed deltas are used
                distance = 0;

                var prevpos = pos;
                for (var i = 0; i < packedDeltasCount; ++i)
                {
                    var vec = mid - packet.ReadPackedVector3();
                    packet.AddValue("WayPoints", vec, indexes, i);
                    monsterMove.PackedPoints.Add(vec);
                    distance += Vector3.GetDistance(prevpos, vec);
                    prevpos = vec;
                }
                distance += Vector3.GetDistance(prevpos, endpos);
            }

            if (hasSpellEffectExtraData)
                ReadMonsterSplineSpellEffectExtraData(packet, "MonsterSplineSpellEffectExtra");

            if (endpos.X != 0 && endpos.Y != 0 && endpos.Z != 0)
            {
                packet.AddValue("Computed Distance", distance, indexes);
                packet.AddValue("Computed Speed", (distance / monsterMove.MoveTime) * 1000, indexes);
            }
        }
        public static void ReadTransportData(Packet packet, params object[] idx)
        {
            packet.ReadPackedGuid128("TransportGuid", idx);
            packet.ReadVector4("TransportPosition", idx);
            packet.ReadByte("TransportSeat", idx);
            packet.ReadInt32("TransportMoveTime", idx);

            packet.ResetBitReader();

            var hasPrevMoveTime = packet.ReadBit("HasPrevMoveTime", idx);
            var hasVehicleRecID = packet.ReadBit("HasVehicleRecID", idx);

            if (hasPrevMoveTime)
                packet.ReadUInt32("PrevMoveTime", idx);

            if (hasVehicleRecID)
                packet.ReadUInt32("VehicleRecID", idx);
        }

        [Parser(Opcode.CMSG_MOVE_APPLY_MOVEMENT_FORCE_ACK)]
        public static void HandleMoveApplyMovementForceAck(Packet packet)
        {
            ReadMovementAck(packet);
            ReadMovementForce(packet, "MovementForce");
        }

        [Parser(Opcode.SMSG_ON_MONSTER_MOVE)]
        public static void HandleOnMonsterMove(Packet packet)
        {
            PacketMonsterMove monsterMove = packet.Holder.MonsterMove = new();
            monsterMove.Mover = packet.ReadPackedGuid128("MoverGUID");
            Vector3 pos = monsterMove.Position =  packet.ReadVector3("Position");

            ReadMovementMonsterSpline(packet, pos, "MovementMonsterSpline");
        }

        [Parser(Opcode.CMSG_MOVE_CHANGE_TRANSPORT)]
        [Parser(Opcode.CMSG_MOVE_DISMISS_VEHICLE)]
        [Parser(Opcode.CMSG_MOVE_FALL_LAND)]
        [Parser(Opcode.CMSG_MOVE_FALL_RESET)]
        [Parser(Opcode.CMSG_MOVE_HEARTBEAT)]
        [Parser(Opcode.CMSG_MOVE_JUMP)]
        [Parser(Opcode.CMSG_MOVE_REMOVE_MOVEMENT_FORCES)]
        [Parser(Opcode.CMSG_MOVE_SET_FACING)]
        [Parser(Opcode.CMSG_MOVE_SET_FACING_HEARTBEAT)]
        [Parser(Opcode.CMSG_MOVE_SET_FLY)]
        [Parser(Opcode.CMSG_MOVE_SET_PITCH)]
        [Parser(Opcode.CMSG_MOVE_SET_RUN_MODE)]
        [Parser(Opcode.CMSG_MOVE_SET_WALK_MODE)]
        [Parser(Opcode.CMSG_MOVE_START_ASCEND)]
        [Parser(Opcode.CMSG_MOVE_START_BACKWARD)]
        [Parser(Opcode.CMSG_MOVE_START_DESCEND)]
        [Parser(Opcode.CMSG_MOVE_START_FORWARD)]
        [Parser(Opcode.CMSG_MOVE_START_PITCH_DOWN)]
        [Parser(Opcode.CMSG_MOVE_START_PITCH_UP)]
        [Parser(Opcode.CMSG_MOVE_START_SWIM)]
        [Parser(Opcode.CMSG_MOVE_START_TURN_LEFT)]
        [Parser(Opcode.CMSG_MOVE_START_TURN_RIGHT)]
        [Parser(Opcode.CMSG_MOVE_START_STRAFE_LEFT)]
        [Parser(Opcode.CMSG_MOVE_START_STRAFE_RIGHT)]
        [Parser(Opcode.CMSG_MOVE_STOP)]
        [Parser(Opcode.CMSG_MOVE_STOP_ASCEND)]
        [Parser(Opcode.CMSG_MOVE_STOP_PITCH)]
        [Parser(Opcode.CMSG_MOVE_STOP_STRAFE)]
        [Parser(Opcode.CMSG_MOVE_STOP_SWIM)]
        [Parser(Opcode.CMSG_MOVE_STOP_TURN)]
        [Parser(Opcode.CMSG_MOVE_DOUBLE_JUMP)]
        public static void HandleClientPlayerMove(Packet packet)
        {
            var stats = ReadMovementStats(packet, "MovementStats");
            packet.Holder.ClientMove = new() { Position = stats.PositionAsVector4, Mover = stats.MoverGuid };
        }

        [Parser(Opcode.SMSG_MOVE_UPDATE_KNOCK_BACK)]
        [Parser(Opcode.SMSG_MOVE_UPDATE)]
        [Parser(Opcode.CMSG_MOVE_DOUBLE_JUMP)]
        [Parser(Opcode.CMSG_UNK_3A3F)]
        public static void HandlePlayerMove(Packet packet)
        {
            ReadMovementStats(packet, "MovementStats");
        }

        [Parser(Opcode.CMSG_MOVE_GRAVITY_DISABLE_ACK)]
        [Parser(Opcode.CMSG_MOVE_GRAVITY_ENABLE_ACK)]
        [Parser(Opcode.CMSG_MOVE_HOVER_ACK)]
        [Parser(Opcode.CMSG_MOVE_KNOCK_BACK_ACK, ClientVersionBuild.V7_0_3_22248, ClientVersionBuild.V7_2_0_23826)]
        [Parser(Opcode.CMSG_MOVE_WATER_WALK_ACK)]
        [Parser(Opcode.CMSG_MOVE_FORCE_ROOT_ACK)]
        [Parser(Opcode.CMSG_MOVE_FORCE_UNROOT_ACK)]
        [Parser(Opcode.CMSG_MOVE_SET_CAN_FLY_ACK)]
        [Parser(Opcode.CMSG_MOVE_ENABLE_SWIM_TO_FLY_TRANS_ACK)]
        [Parser(Opcode.CMSG_MOVE_FEATHER_FALL_ACK)]
        [Parser(Opcode.CMSG_MOVE_SET_CAN_TURN_WHILE_FALLING_ACK)]
        [Parser(Opcode.CMSG_MOVE_ENABLE_DOUBLE_JUMP_ACK)]
        [Parser(Opcode.CMSG_MOVE_SET_IGNORE_MOVEMENT_FORCES_ACK)]
        public static void HandleMovementAck(Packet packet)
        {
            var stats = V6_0_2_19033.Parsers.MovementHandler.ReadMovementAck(packet, "MovementAck");
            packet.Holder.ClientMove = new() { Mover = stats.MoverGuid, Position = stats.PositionAsVector4 };
        }

        [Parser(Opcode.CMSG_MOVE_KNOCK_BACK_ACK, ClientVersionBuild.V7_2_0_23826)]
        public static void HandleMoveKnockBackAck(Packet packet)
        {
            var stats = V6_0_2_19033.Parsers.MovementHandler.ReadMovementAck(packet, "MovementAck");
            packet.Holder.ClientMove = new() { Mover = stats.MoverGuid, Position = stats.PositionAsVector4 };

            packet.ResetBitReader();

            var hasSpeeds = packet.ReadBit("HasSpeeds");
            if (hasSpeeds)
            {
                packet.ReadSingle("HorzSpeed");
                packet.ReadSingle("VertSpeed");
            }
        }

        [Parser(Opcode.CMSG_MOVE_FORCE_FLIGHT_BACK_SPEED_CHANGE_ACK)]
        [Parser(Opcode.CMSG_MOVE_FORCE_FLIGHT_SPEED_CHANGE_ACK)]
        [Parser(Opcode.CMSG_MOVE_FORCE_PITCH_RATE_CHANGE_ACK)]
        [Parser(Opcode.CMSG_MOVE_FORCE_RUN_BACK_SPEED_CHANGE_ACK)]
        [Parser(Opcode.CMSG_MOVE_FORCE_RUN_SPEED_CHANGE_ACK)]
        [Parser(Opcode.CMSG_MOVE_FORCE_SWIM_BACK_SPEED_CHANGE_ACK)]
        [Parser(Opcode.CMSG_MOVE_FORCE_SWIM_SPEED_CHANGE_ACK)]
        [Parser(Opcode.CMSG_MOVE_FORCE_TURN_RATE_CHANGE_ACK)]
        [Parser(Opcode.CMSG_MOVE_FORCE_WALK_SPEED_CHANGE_ACK)]
        public static void HandleMovementSpeedAck(Packet packet)
        {
            var stats = V6_0_2_19033.Parsers.MovementHandler.ReadMovementAck(packet, "MovementAck");
            packet.Holder.ClientMove = new() { Mover = stats.MoverGuid, Position = stats.PositionAsVector4 };
            packet.ReadSingle("Speed");
        }

        [Parser(Opcode.SMSG_MOVE_UPDATE_WALK_SPEED)]
        [Parser(Opcode.SMSG_MOVE_UPDATE_RUN_SPEED)]
        [Parser(Opcode.SMSG_MOVE_UPDATE_RUN_BACK_SPEED)]
        [Parser(Opcode.SMSG_MOVE_UPDATE_SWIM_SPEED)]
        [Parser(Opcode.SMSG_MOVE_UPDATE_SWIM_BACK_SPEED)]
        [Parser(Opcode.SMSG_MOVE_UPDATE_TURN_RATE)]
        [Parser(Opcode.SMSG_MOVE_UPDATE_FLIGHT_SPEED)]
        [Parser(Opcode.SMSG_MOVE_UPDATE_FLIGHT_BACK_SPEED)]
        [Parser(Opcode.SMSG_MOVE_UPDATE_PITCH_RATE)]
        public static void HandleMovementUpdateSpeed(Packet packet)
        {
            ReadMovementStats(packet, "MovementStats");
            packet.ReadSingle("Speed");
        }

        [Parser(Opcode.CMSG_MOVE_REMOVE_MOVEMENT_FORCE_ACK)]
        public static void HandleMoveRemoveMovementForceAck(Packet packet)
        {
            ReadMovementAck(packet);
            packet.ReadPackedGuid128("TriggerGUID");
        }

        [Parser(Opcode.CMSG_MOVE_SPLINE_DONE)]
        public static void HandleMoveSplineDone(Packet packet)
        {
            ReadMovementStats(packet, "MovementStats");
            packet.ReadInt32("SplineID");
        }

        [Parser(Opcode.SMSG_MOVE_UPDATE_COLLISION_HEIGHT)]
        public static void HandleMoveUpdateCollisionHeight434(Packet packet)
        {
            ReadMovementStats(packet, "MovementStats");
            packet.ReadSingle("Height");
            packet.ReadSingle("Scale");
        }

        [Parser(Opcode.CMSG_MOVE_SET_COLLISION_HEIGHT_ACK)]
        public static void HandleMoveSetCollisionHeightAck(Packet packet)
        {
            var stats = V6_0_2_19033.Parsers.MovementHandler.ReadMovementAck(packet, "MovementAck");
            packet.Holder.ClientMove = new() { Mover = stats.MoverGuid, Position = stats.PositionAsVector4 };
            packet.ReadSingle("Height");
            packet.ReadInt32("MountDisplayID");

            packet.ResetBitReader();

            packet.ReadBits("Reason", 2);
        }

        [Parser(Opcode.CMSG_UPDATE_MISSILE_TRAJECTORY)]
        public static void HandleUpdateMissileTrajectory(Packet packet)
        {
            packet.ReadPackedGuid128("GUID");
            packet.ReadInt16("unk1");
            packet.ReadInt32<SpellId>("Spell ID");
            packet.ReadSingle("Elevation");
            packet.ReadSingle("Missile speed");
            packet.ReadVector3("Current Position");
            packet.ReadVector3("Targeted Position");

            packet.ResetBitReader();
            var hasMovementStats = packet.ReadBit("HasMovementStats");

            packet.ResetBitReader();
            if (hasMovementStats)
                ReadMovementStats(packet, "Stats");
        }

        [Parser(Opcode.SMSG_MOVE_UPDATE_APPLY_MOVEMENT_FORCE)]
        public static void HandleMoveUpdateApplyMovementForce(Packet packet)
        {
            ReadMovementStats(packet);
            ReadMovementForce(packet, "MovementForce");
        }

        [Parser(Opcode.SMSG_MOVE_UPDATE_TELEPORT, ClientVersionBuild.V7_0_3_22248, ClientVersionBuild.V7_2_0_23826)]
        public static void HandleMoveUpdateTeleport(Packet packet)
        {
            Substructures.MovementHandler.ReadMovementStats(packet, "MovementStats");

            var int32 = packet.ReadInt32("MovementForcesCount");
            for (int i = 0; i < int32; i++)
                ReadMovementForce(packet, i, "MovementForce");

            packet.ResetBitReader();

            var hasWalkSpeed = packet.ReadBit("HasWalkSpeed");
            var hasRunSpeed = packet.ReadBit("HasRunSpeed");
            var hasRunBackSpeed = packet.ReadBit("HasRunBackSpeed");
            var hasSwimSpeed = packet.ReadBit("HasSwimSpeed");
            var hasSwimBackSpeed = packet.ReadBit("HasSwimBackSpeed");
            var hasFlightSpeed = packet.ReadBit("HasFlightSpeed");
            var hasFlightBackSpeed = packet.ReadBit("HasFlightBackSpeed");
            var hasTurnRate = packet.ReadBit("HasTurnRate");
            var hasPitchRate = packet.ReadBit("HasPitchRate");

            if (hasWalkSpeed)
                packet.ReadSingle("WalkSpeed");

            if (hasRunSpeed)
                packet.ReadSingle("RunSpeed");

            if (hasRunBackSpeed)
                packet.ReadSingle("RunBackSpeed");

            if (hasSwimSpeed)
                packet.ReadSingle("SwimSpeed");

            if (hasSwimBackSpeed)
                packet.ReadSingle("SwimBackSpeed");

            if (hasFlightSpeed)
                packet.ReadSingle("FlightSpeed");

            if (hasFlightBackSpeed)
                packet.ReadSingle("FlightBackSpeed");

            if (hasTurnRate)
                packet.ReadSingle("TurnRate");

            if (hasPitchRate)
                packet.ReadSingle("PitchRate");
        }

        [Parser(Opcode.SMSG_MOVE_UPDATE_TELEPORT, ClientVersionBuild.V7_2_0_23826)]
        public static void HandleMoveUpdateTeleport720(Packet packet)
        {
            Substructures.MovementHandler.ReadMovementStats(packet, "MovementStats");

            var movementForcesCount = packet.ReadUInt32("MovementForcesCount");

            packet.ResetBitReader();

            var hasWalkSpeed = packet.ReadBit("HasWalkSpeed");
            var hasRunSpeed = packet.ReadBit("HasRunSpeed");
            var hasRunBackSpeed = packet.ReadBit("HasRunBackSpeed");
            var hasSwimSpeed = packet.ReadBit("HasSwimSpeed");
            var hasSwimBackSpeed = packet.ReadBit("HasSwimBackSpeed");
            var hasFlightSpeed = packet.ReadBit("HasFlightSpeed");
            var hasFlightBackSpeed = packet.ReadBit("HasFlightBackSpeed");
            var hasTurnRate = packet.ReadBit("HasTurnRate");
            var hasPitchRate = packet.ReadBit("HasPitchRate");

            for (int i = 0; i < movementForcesCount; i++)
                ReadMovementForce(packet, i, "MovementForce");

            if (hasWalkSpeed)
                packet.ReadSingle("WalkSpeed");

            if (hasRunSpeed)
                packet.ReadSingle("RunSpeed");

            if (hasRunBackSpeed)
                packet.ReadSingle("RunBackSpeed");

            if (hasSwimSpeed)
                packet.ReadSingle("SwimSpeed");

            if (hasSwimBackSpeed)
                packet.ReadSingle("SwimBackSpeed");

            if (hasFlightSpeed)
                packet.ReadSingle("FlightSpeed");

            if (hasFlightBackSpeed)
                packet.ReadSingle("FlightBackSpeed");

            if (hasTurnRate)
                packet.ReadSingle("TurnRate");

            if (hasPitchRate)
                packet.ReadSingle("PitchRate");
        }

        [Parser(Opcode.SMSG_MOVE_TELEPORT)]
        public static void HandleMoveTeleport(Packet packet)
        {
            packet.ReadPackedGuid128("MoverGUID");
            packet.ReadInt32("SequenceIndex");
            packet.ReadVector3("Position");
            packet.ReadSingle("Facing");
            packet.ReadByte("PreloadWorld");

            packet.ResetBitReader();

            var hasVehicleTeleport = false;
            var hasTransport = false;

            if (ClientVersion.AddedInVersion(ClientVersionBuild.V7_3_5_25848))
            {
                hasTransport = packet.ReadBit("HasTransport");
                hasVehicleTeleport = packet.ReadBit("HasVehicleTeleport");
            }
            else
            {
                hasVehicleTeleport = packet.ReadBit("HasVehicleTeleport");
                hasTransport = packet.ReadBit("HasTransport");
            }

            // VehicleTeleport
            if (hasVehicleTeleport)
            {
                packet.ReadByte("VehicleSeatIndex");
                packet.ResetBitReader();
                packet.ReadBit("VehicleExitVoluntary");
                packet.ReadBit("VehicleExitTeleport");
            }

            if (hasTransport)
                packet.ReadPackedGuid128("TransportGUID");
        }

        [Parser(Opcode.SMSG_MOVE_UPDATE_REMOVE_MOVEMENT_FORCE)]
        public static void HandleMoveUpdateRemoveMovementForce(Packet packet)
        {
            ReadMovementStats(packet);
            packet.ReadPackedGuid128("TriggerGUID");
        }

        [HasSniffData]
        [Parser(Opcode.SMSG_NEW_WORLD)]
        public static void HandleNewWorld(Packet packet)
        {
            WowPacketParser.Parsing.Parsers.MovementHandler.CurrentMapId = (uint)packet.ReadInt32<MapId>("Map");
            packet.ReadVector4("Position");
            packet.ReadUInt32("Reason");
            packet.ReadVector3("MovementOffset");

            packet.AddSniffData(StoreNameType.Map, (int)WowPacketParser.Parsing.Parsers.MovementHandler.CurrentMapId, "NEW_WORLD");
        }

        [Parser(Opcode.SMSG_MOVE_ENABLE_DOUBLE_JUMP)]
        public static void HandleMoveEnableDoubleJump(Packet packet)
        {
            packet.ReadPackedGuid128("MoverGUID");
            packet.ReadUInt32("SequenceId");
        }

        [Parser(Opcode.SMSG_MOVE_SET_FLIGHT_BACK_SPEED)]
        [Parser(Opcode.SMSG_MOVE_SET_SWIM_BACK_SPEED)]
        public static void HandleMoveSetSwimBackSpeed(Packet packet)
        {
            packet.ReadPackedGuid128("MoverGUID");
            packet.ReadUInt32("SequenceIndex");
            packet.ReadSingle("Speed");
        }

        [Parser(Opcode.SMSG_MOVE_SET_COLLISION_HEIGHT, ClientVersionBuild.V7_1_0_22900)]
        public static void HandleSetCollisionHeight(Packet packet)
        {
            packet.ReadPackedGuid128("MoverGUID");
            packet.ReadInt32("SequenceIndex");
            packet.ReadSingle("Height");
            packet.ReadSingle("Scale");
            packet.ReadUInt32("MountDisplayID");
            packet.ReadInt32("ScaleDuration");

            packet.ResetBitReader();

            packet.ReadBits("Reason", 2);
        }

        [Parser(Opcode.SMSG_TRANSFER_PENDING)]
        public static void HandleTransferPending(Packet packet)
        {
            packet.ReadInt32<MapId>("MapID");
            if (ClientVersion.AddedInVersion(ClientVersionBuild.V7_3_0_24920))
                packet.ReadVector3("OldMapPosition");

            packet.ResetBitReader();

            var hasShipTransferPending = packet.ReadBit();
            var hasTransferSpell = packet.ReadBit();

            if (hasShipTransferPending)
            {
                packet.ReadUInt32<GOId>("ID");
                packet.ReadInt32<MapId>("OriginMapID");
            }

            if (hasTransferSpell)
                packet.ReadUInt32<SpellId>("TransferSpellID");
        }

        [Parser(Opcode.CMSG_MOVE_SET_VEHICLE_REC_ID_ACK)]
        public static void HandleMoveSetVehicleRecIdAck(Packet packet)
        {
            var stats = V6_0_2_19033.Parsers.MovementHandler.ReadMovementAck(packet);
            packet.Holder.ClientMove = new() { Mover = stats.MoverGuid, Position = stats.PositionAsVector4 };
            packet.ReadInt32("VehicleRecID");
        }

        [Parser(Opcode.SMSG_PHASE_SHIFT_CHANGE)]
        public static void HandlePhaseShift(Packet packet)
        {
            var phaseShift = packet.Holder.PhaseShift = new PacketPhaseShift();
            CoreParsers.MovementHandler.ClearPhases();

            phaseShift.Client = packet.ReadPackedGuid128("Client");

            // PhaseShiftData
            packet.ReadInt32("PhaseShiftFlags");
            var count = packet.ReadInt32("PhaseShiftCount");
            phaseShift.PersonalGuid = packet.ReadPackedGuid128("PersonalGUID");
            for (var i = 0; i < count; ++i)
            {
                var flags = packet.ReadUInt16E<PhaseFlags>("PhaseFlags", i);
                var id = packet.ReadUInt16();
                phaseShift.Phases.Add(id);

                if (Settings.UseDBC && DBC.Phase.ContainsKey(id))
                {
                    packet.WriteLine($"[{i}] ID: {id} ({(DBCPhaseFlags)DBC.Phase[id].Flags})");
                }
                else
                    packet.AddValue("ID", id, i);

                CoreParsers.MovementHandler.ActivePhases.Add(id, true);
            }

            if (DBC.Phases.Any())
            {
                foreach (var phaseGroup in DBC.GetPhaseGroups(CoreParsers.MovementHandler.ActivePhases.Keys))
                    packet.WriteLine($"PhaseGroup: { phaseGroup } Phases: { string.Join(" - ", DBC.Phases[phaseGroup]) }");
            }

            var visibleMapIDsCount = packet.ReadInt32("VisibleMapIDsCount") / 2;
            for (var i = 0; i < visibleMapIDsCount; ++i)
                phaseShift.VisibleMaps.Add((uint)packet.ReadInt16<MapId>("VisibleMapID", i));

            var preloadMapIDCount = packet.ReadInt32("PreloadMapIDsCount") / 2;
            for (var i = 0; i < preloadMapIDCount; ++i)
                phaseShift.PreloadMaps.Add((uint)packet.ReadInt16<MapId>("PreloadMapID", i));

            var uiWorldMapAreaIDSwapsCount = packet.ReadInt32("UiWorldMapAreaIDSwap") / 2;
            for (var i = 0; i < uiWorldMapAreaIDSwapsCount; ++i)
                phaseShift.UiMapPhase.Add((uint)packet.ReadInt16("UiWorldMapAreaIDSwaps", i));

            CoreParsers.MovementHandler.WritePhaseChanges(packet);
        }

        [Parser(Opcode.SMSG_TRANSFER_ABORTED)]
        public static void HandleTransferAborted(Packet packet)
        {
            packet.ReadInt32<MapId>("MapID");
            packet.ReadByte("Arg");
            packet.ReadInt32("MapDifficultyXConditionID");
            if (ClientVersion.AddedInVersion(ClientVersionBuild.V9_1_0_39185))
                packet.ReadBitsE<TransferAbortReason>("TransfertAbort", 6);
            else
                packet.ReadBitsE<TransferAbortReason>("TransfertAbort", 5);
        }
    }
}
