﻿using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;
using MovementFlag = WowPacketParserModule.V7_2_0_23758.Enums.MovementFlag;

namespace WowPacketParserModule.V7_2_0_23758.Parsers
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

        public static void ReadMovementStats(Packet packet, params object[] idx)
        {
            packet.ReadPackedGuid128("MoverGUID", idx);

            packet.ReadUInt32("MoveIndex", idx);
            packet.ReadVector4("Position", idx);

            packet.ReadSingle("Pitch", idx);
            packet.ReadSingle("StepUpStartElevation", idx);

            var int152 = packet.ReadInt32("RemoveForcesCount", idx);
            packet.ReadInt32("MoveTime", idx);

            for (var i = 0; i < int152; i++)
                packet.ReadPackedGuid128("RemoveForcesIDs", idx, i);

            packet.ResetBitReader();

            packet.ReadBitsE<MovementFlag>("MovementFlags", 30, idx);
            packet.ReadBitsE<MovementFlagExtra>("ExtraMovementFlags", 18, idx);

            var hasTransport = packet.ReadBit("HasTransportData", idx);
            var hasFall = packet.ReadBit("HasFallData", idx);
            packet.ReadBit("HasSpline", idx);
            packet.ReadBit("HeightChangeFailed", idx);
            packet.ReadBit("RemoteTimeValid", idx);

            if (hasTransport)
                ReadTransportData(packet, idx, "TransportData");

            if (hasFall)
                ReadFallData(packet, idx, "FallData");
        }

        public static void ReadMovementAck(Packet packet, params object[] idx)
        {
            ReadMovementStats(packet, idx);
            packet.ReadInt32("AckIndex", idx);
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
            packet.ReadUInt32("Id", indexes);
            packet.ReadVector3("Destination", indexes);

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
            packet.ReadInt32E<SplineFlag434>("Flags", indexes);
            packet.ReadByte("AnimTier", indexes);
            packet.ReadUInt32("TierTransStartTime", indexes);
            packet.ReadInt32("Elapsed", indexes);
            packet.ReadUInt32("MoveTime", indexes);
            packet.ReadSingle("JumpGravity", indexes);
            packet.ReadUInt32("SpecialTime", indexes);

            packet.ReadByte("Mode", indexes);
            packet.ReadByte("VehicleExitVoluntary", indexes);

            packet.ReadPackedGuid128("TransportGUID", indexes);
            packet.ReadSByte("VehicleSeat", indexes);

            packet.ResetBitReader();

            var type = packet.ReadBits("Face", 2, indexes);
            var pointsCount = packet.ReadBits("PointsCount", 16, indexes);
            var packedDeltasCount = packet.ReadBits("PackedDeltasCount", 16, indexes);
            var hasSplineFilter = packet.ReadBit("HasSplineFilter", indexes);
            var hasSpellEffectExtraData = packet.ReadBit("HasSpellEffectExtraData", indexes);

            if (hasSplineFilter)
                ReadMonsterSplineFilter(packet, indexes, "MonsterSplineFilter");

            switch (type)
            {
                case 1:
                    packet.ReadVector3("FaceSpot", indexes);
                    break;
                case 2:
                    packet.ReadSingle("FaceDirection", indexes);
                    packet.ReadPackedGuid128("FacingGUID", indexes);
                    break;
                case 3:
                    packet.ReadSingle("FaceDirection", indexes);
                    break;
            }

            Vector3 endpos = new Vector3();
            for (int i = 0; i < pointsCount; i++)
            {
                var spot = packet.ReadVector3();

                // client always taking first point
                if (i == 0)
                    endpos = spot;

                packet.AddValue("Points", spot, indexes, i);
            }

            var waypoints = new Vector3[packedDeltasCount];
            for (int i = 0; i < packedDeltasCount; i++)
            {
                var packedDeltas = packet.ReadPackedVector3();
                waypoints[i].X = packedDeltas.X;
                waypoints[i].Y = packedDeltas.Y;
                waypoints[i].Z = packedDeltas.Z;
            }

            if (hasSpellEffectExtraData)
                ReadMonsterSplineSpellEffectExtraData(packet, "MonsterSplineSpellEffectExtra");

            // Calculate mid pos
            var mid = new Vector3
            {
                X = (pos.X + endpos.X) * 0.5f,
                Y = (pos.Y + endpos.Y) * 0.5f,
                Z = (pos.Z + endpos.Z) * 0.5f
            };

            for (var i = 0; i < packedDeltasCount; ++i)
            {
                var vec = new Vector3
                {
                    X = mid.X - waypoints[i].X,
                    Y = mid.Y - waypoints[i].Y,
                    Z = mid.Z - waypoints[i].Z
                };
                packet.AddValue("WayPoints", vec, indexes, i);
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

        [Parser(Opcode.CMSG_DISCARDED_TIME_SYNC_ACKS)]
        public static void HandleDiscardedTimeSyncAcks(Packet packet)
        {
            packet.ReadUInt32("MaxSequenceIndex");
        }

        [Parser(Opcode.CMSG_MOVE_APPLY_MOVEMENT_FORCE_ACK)]
        public static void HandleMoveApplyMovementForceAck(Packet packet)
        {
            ReadMovementAck(packet);
            ReadMovementForce(packet, "MovementForce");
        }

        [Parser(Opcode.CMSG_MOVE_TIME_SKIPPED)]
        public static void HandleMoveTimeSkipped(Packet packet)
        {
            packet.ReadPackedGuid128("GUID");
            packet.ReadInt32("Time");
        }

        [Parser(Opcode.CMSG_SET_ACTIVE_MOVER)]
        public static void HandleSetActiveMover(Packet packet)
        {
            packet.ReadPackedGuid128("ActiveMover");
        }

        [Parser(Opcode.SMSG_ON_MONSTER_MOVE)]
        public static void HandleOnMonsterMove(Packet packet)
        {
            packet.ReadPackedGuid128("MoverGUID");
            var pos = packet.ReadVector3("Position");

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
        [Parser(Opcode.SMSG_MOVE_UPDATE_KNOCK_BACK)]
        [Parser(Opcode.SMSG_MOVE_UPDATE)]
        [Parser(Opcode.CMSG_MOVE_DOUBLE_JUMP)]
        public static void HandlePlayerMove(Packet packet)
        {
            ReadMovementStats(packet, "MovementStats");
        }

        [Parser(Opcode.CMSG_MOVE_GRAVITY_DISABLE_ACK)]
        [Parser(Opcode.CMSG_MOVE_GRAVITY_ENABLE_ACK)]
        [Parser(Opcode.CMSG_MOVE_HOVER_ACK)]
        [Parser(Opcode.CMSG_MOVE_KNOCK_BACK_ACK)]
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
            ReadMovementAck(packet, "MovementAck");
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
            ReadMovementAck(packet, "MovementAck");
            packet.ReadSingle("Speed");
        }

        [Parser(Opcode.SMSG_MOVE_SET_ACTIVE_MOVER)]
        [Parser(Opcode.SMSG_MOVE_SPLINE_ROOT)]
        [Parser(Opcode.SMSG_MOVE_SPLINE_UNROOT)]
        [Parser(Opcode.SMSG_MOVE_SPLINE_DISABLE_GRAVITY)]
        [Parser(Opcode.SMSG_MOVE_SPLINE_ENABLE_GRAVITY)]
        [Parser(Opcode.SMSG_MOVE_SPLINE_DISABLE_COLLISION)]
        [Parser(Opcode.SMSG_MOVE_SPLINE_ENABLE_COLLISION)]
        [Parser(Opcode.SMSG_MOVE_SPLINE_SET_FEATHER_FALL)]
        [Parser(Opcode.SMSG_MOVE_SPLINE_SET_NORMAL_FALL)]
        [Parser(Opcode.SMSG_MOVE_SPLINE_SET_HOVER)]
        [Parser(Opcode.SMSG_MOVE_SPLINE_UNSET_HOVER)]
        [Parser(Opcode.SMSG_MOVE_SPLINE_SET_WATER_WALK)]
        [Parser(Opcode.SMSG_MOVE_SPLINE_START_SWIM)]
        [Parser(Opcode.SMSG_MOVE_SPLINE_STOP_SWIM)]
        [Parser(Opcode.SMSG_MOVE_SPLINE_SET_RUN_MODE)]
        [Parser(Opcode.SMSG_MOVE_SPLINE_SET_WALK_MODE)]
        [Parser(Opcode.SMSG_MOVE_SPLINE_SET_FLYING)]
        [Parser(Opcode.SMSG_MOVE_SPLINE_UNSET_FLYING)]
        [Parser(Opcode.SMSG_MOVE_SPLINE_SET_LAND_WALK)]
        public static void HandleSplineMoveGravityDisable(Packet packet)
        {
            packet.ReadPackedGuid128("MoverGUID");
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

        [Parser(Opcode.CMSG_MOVE_SPLINE_DONE)]
        public static void HandleMoveSplineDone(Packet packet)
        {
            ReadMovementStats(packet, "MovementStats");
            packet.ReadInt32("SplineID");
        }

        [Parser(Opcode.SMSG_FLIGHT_SPLINE_SYNC)]
        public static void HandleFlightSplineSync(Packet packet)
        {
            packet.ReadPackedGuid128("Guid");
            packet.ReadSingle("SplineDist");
        }

        [Parser(Opcode.SMSG_LOGIN_VERIFY_WORLD)]
        public static void HandleLoginVerifyWorld(Packet packet)
        {
            WowPacketParser.Parsing.Parsers.MovementHandler.CurrentMapId = (uint)packet.ReadInt32<MapId>("Map");
            packet.ReadVector4("Position");
            packet.ReadUInt32("Reason");
        }

        [Parser(Opcode.SMSG_LOGIN_SET_TIME_SPEED)]
        public static void HandleLoginSetTimeSpeed(Packet packet)
        {
            packet.ReadPackedTime("ServerTime");
            packet.ReadPackedTime("GameTime");
            packet.ReadSingle("NewSpeed");
            packet.ReadInt32("ServerTimeHolidayOffset");
            packet.ReadInt32("GameTimeHolidayOffset");
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
            ReadMovementAck(packet, "MovementAck");
            packet.ReadSingle("Height");
            packet.ReadInt32("MountDisplayID");

            packet.ResetBitReader();

            packet.ReadBits("Reason", 2);
        }

        [Parser(Opcode.SMSG_MOVE_DISABLE_GRAVITY)]
        [Parser(Opcode.SMSG_MOVE_DISABLE_TRANSITION_BETWEEN_SWIM_AND_FLY)]
        [Parser(Opcode.SMSG_MOVE_ENABLE_GRAVITY)]
        [Parser(Opcode.SMSG_MOVE_ENABLE_TRANSITION_BETWEEN_SWIM_AND_FLY)]
        [Parser(Opcode.SMSG_MOVE_SET_LAND_WALK)]
        [Parser(Opcode.SMSG_MOVE_ROOT)]
        [Parser(Opcode.SMSG_MOVE_SET_CAN_FLY)]
        [Parser(Opcode.SMSG_MOVE_SET_CAN_TRANSITION_BETWEEN_SWIM_AND_FLY)]
        [Parser(Opcode.SMSG_MOVE_SET_HOVERING)]
        [Parser(Opcode.SMSG_MOVE_UNSET_CAN_FLY)]
        [Parser(Opcode.SMSG_MOVE_UNSET_CAN_TRANSITION_BETWEEN_SWIM_AND_FLY)]
        [Parser(Opcode.SMSG_MOVE_UNSET_HOVERING)]
        [Parser(Opcode.SMSG_MOVE_UNROOT)]
        [Parser(Opcode.SMSG_MOVE_SET_WATER_WALK)]
        [Parser(Opcode.SMSG_MOVE_SET_FEATHER_FALL)]
        [Parser(Opcode.SMSG_MOVE_SET_NORMAL_FALL)]
        [Parser(Opcode.SMSG_MOVE_SET_IGNORE_MOVEMENT_FORCES)]
        [Parser(Opcode.SMSG_MOVE_UNSET_IGNORE_MOVEMENT_FORCES)]
        public static void HandleMovementIndex(Packet packet)
        {
            packet.ReadPackedGuid128("MoverGUID");
            packet.ReadInt32("SequenceIndex");
        }

        [Parser(Opcode.SMSG_MOVE_UPDATE_TELEPORT)]
        public static void HandleMoveUpdateTeleport(Packet packet)
        {
            ReadMovementStats(packet, "MovementStats");

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

        [Parser(Opcode.SMSG_MOVE_TELEPORT)]
        public static void HandleMoveTeleport(Packet packet)
        {
            packet.ReadPackedGuid128("MoverGUID");
            packet.ReadInt32("SequenceIndex");
            packet.ReadVector3("Position");
            packet.ReadSingle("Facing");
            packet.ReadByte("PreloadWorld");

            var hasVehicleTeleport = packet.ReadBit("HasVehicleTeleport");
            var hasTransport = packet.ReadBit("HasTransport");

            // VehicleTeleport
            if (hasVehicleTeleport)
            {
                packet.ReadByte("VehicleSeatIndex");
                packet.ReadBit("VehicleExitVoluntary");
                packet.ReadBit("VehicleExitTeleport");
            }

            if (hasTransport)
                packet.ReadPackedGuid128("TransportGUID");
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
            packet.ReadSingle("Scale");
            packet.ReadSingle("Height");
            packet.ReadUInt32("MountDisplayID");
            packet.ReadInt32("ScaleDuration");

            packet.ResetBitReader();

            packet.ReadBits("Reason", 2);
        }

        [Parser(Opcode.SMSG_PHASE_SHIFT_CHANGE)]
        public static void HandlePhaseShift(Packet packet)
        {
            packet.ReadPackedGuid128("Client");

            // PhaseShiftData
            packet.ReadInt32("PhaseShiftFlags");
            var count = packet.ReadInt32("PhaseShiftCount");
            packet.ReadPackedGuid128("PersonalGUID");
            for (var i = 0; i < count; ++i)
            {
                packet.ReadInt16("PhaseFlags", i);
                packet.ReadInt16("Id", i);
            }

            var preloadMapIDCount = packet.ReadInt32("PreloadMapIDsCount") / 2;
            for (var i = 0; i < preloadMapIDCount; ++i)
                packet.ReadInt16<MapId>("PreloadMapID", i);

            var uiWorldMapAreaIDSwapsCount = packet.ReadInt32("UiWorldMapAreaIDSwap") / 2;
            for (var i = 0; i < uiWorldMapAreaIDSwapsCount; ++i)
                packet.ReadInt16<MapId>("UiWorldMapAreaIDSwaps", i);

            var visibleMapIDsCount = packet.ReadInt32("VisibleMapIDsCount") / 2;
            for (var i = 0; i < visibleMapIDsCount; ++i)
                packet.ReadInt16<MapId>("VisibleMapID", i);
        }

        [Parser(Opcode.SMSG_TRANSFER_ABORTED)]
        public static void HandleTransferAborted(Packet packet)
        {
            packet.ReadInt32<MapId>("MapID");
            packet.ReadByte("Arg");
            packet.ReadInt32("unk");
            packet.ResetBitReader();
            packet.ReadBitsE<TransferAbortReason>("TransfertAbort", 5);
        }

        [Parser(Opcode.SMSG_VIGNETTE_UPDATE)]
        public static void HandleVignetteUpdate(Packet packet)
        {
            packet.ReadBit("ForceUpdate");

            // VignetteInstanceIDList
            var int1 = packet.ReadInt32("RemovedCount");
            for (var i = 0; i < int1; ++i)
                packet.ReadPackedGuid128("IDs", i);

            // Added
            var int2 = packet.ReadInt32("AddedCount");
            for (var i = 0; i < int2; ++i)
                packet.ReadPackedGuid128("IDs", i);

            // VignetteClientData
            var int3 = packet.ReadInt32("VignetteClientDataCount");
            for (var i = 0; i < int3; ++i)
            {
                packet.ReadVector3("Position", i);
                packet.ReadPackedGuid128("ObjGUID", i);
                packet.ReadInt32("VignetteID", i);
                packet.ReadInt32("Unk", i);
            }

            // Updated
            var int4 = packet.ReadInt32("UpdatedCount");
            for (var i = 0; i < int4; ++i)
                packet.ReadPackedGuid128("IDs", i);

            // VignetteClientData
            var int5 = packet.ReadInt32("VignetteClientDataCount");
            for (var i = 0; i < int5; ++i)
            {
                packet.ReadVector3("Position", i);
                packet.ReadPackedGuid128("ObjGUID", i);
                packet.ReadInt32("VignetteID", i);
                packet.ReadInt32("Unk", i);
            }
        }
    }
}
