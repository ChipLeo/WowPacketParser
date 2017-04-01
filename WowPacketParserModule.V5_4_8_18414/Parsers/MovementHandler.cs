using System;
using WowPacketParser.Enums;
using WowPacketParser.Enums.Version;
using WowPacketParserModule.V5_4_8_18414.Misc;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;
using WowPacketParser.Store;
using CoreParsers = WowPacketParser.Parsing.Parsers;
using Guid = WowPacketParser.Misc.WowGuid;
using UpdateFields = WowPacketParser.Enums.Version.UpdateFields;
using MovementFlag = WowPacketParserModule.V5_4_8_18414.Enums.MovementFlag;
using MovementFlagExtra = WowPacketParserModule.V5_4_8_18414.Enums.MovementFlagExtra;

namespace WowPacketParserModule.V5_4_8_18414.Parsers
{
    public static class MovementHandler
    {
        public static PlayerMovementInfo info = new PlayerMovementInfo();

        public static void ReadPlayerMovementInfo(Packet packet, params MovementStatusElements[] movementStatusElements)
        {
            var guid = new byte[8];
            var transportGUID = new byte[8];

            var pos = new Vector4();
            var transportPos = new Vector4();

            bool hasMovementFlags = false;
            bool hasMovementFlags2 = false;
            bool hasTimestamp = false;
            bool hasOrientation = false;
            bool hasTransportData = false;
            bool hasTransportTime2 = false;
            bool hasTransportTime3 = false;
            bool hasPitch = false;
            bool hasFallData = false;
            bool hasFallDirection = false;
            bool hasSplineElevation = false;
            bool hasUnkTime = false;
            bool hasMountDisplayId = false;

            uint count = 0;

            foreach (var movementInfo in movementStatusElements)
            {
                switch (movementInfo)
                {
                    case MovementStatusElements.MSEHasGuidByte0:
                        guid[0] = packet.ReadBit();
                        break;
                    case MovementStatusElements.MSEHasGuidByte1:
                        guid[1] = packet.ReadBit();
                        break;
                    case MovementStatusElements.MSEHasGuidByte2:
                        guid[2] = packet.ReadBit();
                        break;
                    case MovementStatusElements.MSEHasGuidByte3:
                        guid[3] = packet.ReadBit();
                        break;
                    case MovementStatusElements.MSEHasGuidByte4:
                        guid[4] = packet.ReadBit();
                        break;
                    case MovementStatusElements.MSEHasGuidByte5:
                        guid[5] = packet.ReadBit();
                        break;
                    case MovementStatusElements.MSEHasGuidByte6:
                        guid[6] = packet.ReadBit();
                        break;
                    case MovementStatusElements.MSEHasGuidByte7:
                        guid[7] = packet.ReadBit();
                        break;
                    case MovementStatusElements.MSEHasTransportGuidByte0:
                        if (hasTransportData)
                            transportGUID[0] = packet.ReadBit();
                        break;
                    case MovementStatusElements.MSEHasTransportGuidByte1:
                        if (hasTransportData)
                            transportGUID[1] = packet.ReadBit();
                        break;
                    case MovementStatusElements.MSEHasTransportGuidByte2:
                        if (hasTransportData)
                            transportGUID[2] = packet.ReadBit();
                        break;
                    case MovementStatusElements.MSEHasTransportGuidByte3:
                        if (hasTransportData)
                            transportGUID[3] = packet.ReadBit();
                        break;
                    case MovementStatusElements.MSEHasTransportGuidByte4:
                        if (hasTransportData)
                            transportGUID[4] = packet.ReadBit();
                        break;
                    case MovementStatusElements.MSEHasTransportGuidByte5:
                        if (hasTransportData)
                            transportGUID[5] = packet.ReadBit();
                        break;
                    case MovementStatusElements.MSEHasTransportGuidByte6:
                        if (hasTransportData)
                            transportGUID[6] = packet.ReadBit();
                        break;
                    case MovementStatusElements.MSEHasTransportGuidByte7:
                        if (hasTransportData)
                            transportGUID[7] = packet.ReadBit();
                        break;
                    case MovementStatusElements.MSEGuidByte0:
                        packet.ReadXORByte(guid, 0);
                        break;
                    case MovementStatusElements.MSEGuidByte1:
                        packet.ReadXORByte(guid, 1);
                        break;
                    case MovementStatusElements.MSEGuidByte2:
                        packet.ReadXORByte(guid, 2);
                        break;
                    case MovementStatusElements.MSEGuidByte3:
                        packet.ReadXORByte(guid, 3);
                        break;
                    case MovementStatusElements.MSEGuidByte4:
                        packet.ReadXORByte(guid, 4);
                        break;
                    case MovementStatusElements.MSEGuidByte5:
                        packet.ReadXORByte(guid, 5);
                        break;
                    case MovementStatusElements.MSEGuidByte6:
                        packet.ReadXORByte(guid, 6);
                        break;
                    case MovementStatusElements.MSEGuidByte7:
                        packet.ReadXORByte(guid, 7);
                        break;
                    case MovementStatusElements.MSETransportGuidByte0:
                        if (hasTransportData)
                            packet.ReadXORByte(transportGUID, 0);
                        break;
                    case MovementStatusElements.MSETransportGuidByte1:
                        if (hasTransportData)
                            packet.ReadXORByte(transportGUID, 1);
                        break;
                    case MovementStatusElements.MSETransportGuidByte2:
                        if (hasTransportData)
                            packet.ReadXORByte(transportGUID, 2);
                        break;
                    case MovementStatusElements.MSETransportGuidByte3:
                        if (hasTransportData)
                            packet.ReadXORByte(transportGUID, 3);
                        break;
                    case MovementStatusElements.MSETransportGuidByte4:
                        if (hasTransportData)
                            packet.ReadXORByte(transportGUID, 4);
                        break;
                    case MovementStatusElements.MSETransportGuidByte5:
                        if (hasTransportData)
                            packet.ReadXORByte(transportGUID, 5);
                        break;
                    case MovementStatusElements.MSETransportGuidByte6:
                        if (hasTransportData)
                            packet.ReadXORByte(transportGUID, 6);
                        break;
                    case MovementStatusElements.MSETransportGuidByte7:
                        if (hasTransportData)
                            packet.ReadXORByte(transportGUID, 7);
                        break;
                    case MovementStatusElements.MSEHasMovementFlags:
                        hasMovementFlags = !packet.ReadBit("!hasMovementFlags");
                        break;
                    case MovementStatusElements.MSEHasMovementFlags2:
                        hasMovementFlags2 = !packet.ReadBit("!hasMovementFlags2");
                        break;
                    case MovementStatusElements.MSEHasTimestamp:
                        hasTimestamp = !packet.ReadBit("!hasTimestamp");
                        break;
                    case MovementStatusElements.MSEHasOrientation:
                        hasOrientation = !packet.ReadBit("!hasOrientation");
                        break;
                    case MovementStatusElements.MSEHasTransportData:
                        hasTransportData = packet.ReadBit("hasTransportData");
                        break;
                    case MovementStatusElements.MSEHasTransportTime2:
                        if (hasTransportData)
                            hasTransportTime2 = packet.ReadBit("hasTransportTime2");
                        break;
                    case MovementStatusElements.MSEHasTransportTime3:
                        if (hasTransportData)
                            hasTransportTime3 = packet.ReadBit("hasTransportTime3");
                        break;
                    case MovementStatusElements.MSEHasPitch:
                        hasPitch = !packet.ReadBit("!hasPitch");
                        break;
                    case MovementStatusElements.MSEHasFallData:
                        hasFallData = packet.ReadBit("hasFallData");
                        break;
                    case MovementStatusElements.MSEHasFallDirection:
                        if (hasFallData)
                            hasFallDirection = packet.ReadBit("hasFallDirection");
                        break;
                    case MovementStatusElements.MSEHasSplineElevation:
                        hasSplineElevation = !packet.ReadBit("!hasSplineElevation");
                        break;
                    case MovementStatusElements.MSEHasSpline:
                        packet.ReadBit("hasSpline");
                        break;
                    case MovementStatusElements.MSEHasMountDisplayId:
                        hasMountDisplayId = !packet.ReadBit("!hasMountDisplayId");
                        break;
                    case MovementStatusElements.MSEMountDisplayIdWithCheck: // Fallback here
                        if (!hasMountDisplayId)
                            break;
                        packet.ReadInt32("Mount Display ID");
                        break;
                    case MovementStatusElements.MSEMountDisplayIdWithoutCheck:
                        packet.ReadInt32("Mount Display ID");
                        break;
                    case MovementStatusElements.MSECounterCount:
                        count = packet.ReadBits("counter", 22);
                        break;
                    case MovementStatusElements.MSECount:
                        packet.ReadInt32("MCounter");
                        break;
                    case MovementStatusElements.MSECounter:
                        for (var i = 0; i < count; i++)
                            packet.ReadInt32("Unk Int", i);
                        break;
                    case MovementStatusElements.MSEMovementFlags:
                        if (hasMovementFlags)
                            packet.ReadBitsE<MovementFlag>("Movement Flags", 30);
                        break;
                    case MovementStatusElements.MSEMovementFlags2:
                        if (hasMovementFlags2)
                            packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 13);
                        break;
                    case MovementStatusElements.MSETimestamp:
                        if (hasTimestamp)
                            packet.ReadInt32("Timestamp");
                        break;
                    case MovementStatusElements.MSEPositionX:
                        pos.X = packet.ReadSingle();
                        break;
                    case MovementStatusElements.MSEPositionY:
                        pos.Y = packet.ReadSingle();
                        break;
                    case MovementStatusElements.MSEPositionZ:
                        pos.Z = packet.ReadSingle();
                        break;
                    case MovementStatusElements.MSEOrientation:
                        if (hasOrientation)
                            pos.O = packet.ReadSingle();
                        break;
                    case MovementStatusElements.MSETransportPositionX:
                        if (hasTransportData)
                            transportPos.X = packet.ReadSingle();
                        break;
                    case MovementStatusElements.MSETransportPositionY:
                        if (hasTransportData)
                            transportPos.Y = packet.ReadSingle();
                        break;
                    case MovementStatusElements.MSETransportPositionZ:
                        if (hasTransportData)
                            transportPos.Z = packet.ReadSingle();
                        break;
                    case MovementStatusElements.MSETransportOrientation:
                        if (hasTransportData)
                            transportPos.O = packet.ReadSingle();
                        break;
                    case MovementStatusElements.MSETransportSeat:
                        if (hasTransportData)
                            packet.ReadByte("Seat");
                        break;
                    case MovementStatusElements.MSETransportTime:
                        if (hasTransportData)
                            packet.ReadInt32("Transport Time");
                        break;
                    case MovementStatusElements.MSETransportTime2:
                        if (hasTransportData && hasTransportTime2)
                            packet.ReadInt32("Transport Time 2");
                        break;
                    case MovementStatusElements.MSETransportTime3:
                        if (hasTransportData && hasTransportTime3)
                            packet.ReadInt32("Transport Time 3");
                        break;
                    case MovementStatusElements.MSEPitch:
                        if (hasPitch)
                            packet.ReadSingle("Pitch");
                        break;
                    case MovementStatusElements.MSEFallTime:
                        if (hasFallData)
                            packet.ReadInt32("Fall time");
                        break;
                    case MovementStatusElements.MSEFallVerticalSpeed:
                        if (hasFallData)
                            packet.ReadSingle("Vertical Speed");
                        break;
                    case MovementStatusElements.MSEFallCosAngle:
                        if (hasFallData && hasFallDirection)
                            packet.ReadSingle("Fall Angle");
                        break;
                    case MovementStatusElements.MSEFallSinAngle:
                        if (hasFallData && hasFallDirection)
                            packet.ReadSingle("Fall Sin");
                        break;
                    case MovementStatusElements.MSEFallHorizontalSpeed:
                        if (hasFallData && hasFallDirection)
                            packet.ReadSingle("Horizontal Speed");
                        break;
                    case MovementStatusElements.MSESplineElevation:
                        if (hasSplineElevation)
                            packet.ReadSingle("Spline elevation");
                        break;
                    case MovementStatusElements.MSEHasUnkTime:
                        hasUnkTime = !packet.ReadBit("!hasUnkTime");
                        break;
                    case MovementStatusElements.MSEUnkTime:
                        if (hasUnkTime)
                            packet.ReadInt32("Unk Time");
                        break;
                    case MovementStatusElements.MSEZeroBit:
                    case MovementStatusElements.MSEOneBit:
                        packet.ReadBit();
                        break;
                    case MovementStatusElements.MSEbit148:
                        packet.ReadBit("bit148");
                        break;
                    case MovementStatusElements.MSEbit149:
                        packet.ReadBit("bit149");
                        break;
                    case MovementStatusElements.MSEbit172:
                        packet.ReadBit("bit172");
                        break;
                    case MovementStatusElements.MSEExtra2Bits:
                        packet.ReadBits("2bits", 2);
                        break;
                    case MovementStatusElements.MSEExtraInt32:
                        packet.ReadInt32("Int32");
                        break;
                    case MovementStatusElements.MSEExtraFloat:
                        packet.ReadSingle("Single");
                        break;
                    default:
                        break;
                }
            }

            if (hasTransportData)
            {
                packet.WriteGuid("Transport Guid", transportGUID);
                packet.WriteLine("Transport Position {0}", transportPos);
            }

            if (pos.X != 0 || pos.Y != 0 || pos.Z != 0)
                packet.WriteLine("Position: {0}", pos);

            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.CMSG_CHANGE_SEATS_ON_CONTROLLED_VEHICLE)]
        public static void HandleChangeSeatsOnControlledVehicle(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();
            var guid184 = new byte[8];

            packet.ReadByte("unk176"); // 176
            pos.Y = packet.ReadSingle(); // 40
            pos.Z = packet.ReadSingle(); // 44
            pos.X = packet.ReadSingle(); // 36
            guid184[2] = packet.ReadBit(); // 186
            guid184[1] = packet.ReadBit(); // 185
            var hasTime = !packet.ReadBit("Has no timestamp"); // 32
            guid[6] = packet.ReadBit(); // 22
            guid[0] = packet.ReadBit(); // 16
            guid[2] = packet.ReadBit(); // 18
            guid184[7] = packet.ReadBit(); // 191
            guid[7] = packet.ReadBit(); // 23
            guid[1] = packet.ReadBit(); // 17
            guid184[5] = packet.ReadBit(); // 189
            guid184[3] = packet.ReadBit(); // 187
            var hasTrans = packet.ReadBit("Has transport"); // 104
            var hasMovementFlags2 = !packet.ReadBit("has no MovementFlags2"); // 28
            guid184[6] = packet.ReadBit(); // 190
            guid184[4] = packet.ReadBit(); // 188
            guid[4] = packet.ReadBit(); // 20
            var Count = packet.ReadBits("Counter", 22); // 152
            guid184[0] = packet.ReadBit(); // 184
            var hasO = !packet.ReadBit("Has no Orient"); // 48
            guid[5] = packet.ReadBit(); // 21
            packet.ReadBit("bit 172"); // 172
            packet.ReadBit("bit 148"); // 148
            var hasSplineElev = !packet.ReadBit("Has no Spline Elevation"); // 144
            var hasFallData = packet.ReadBit("Has fall data"); // 140
            var hasMovementFlags = !packet.ReadBit("has no MovementFlags"); // 24
            var hasSpline = !packet.ReadBit("has no Spline"); // 168
            packet.ReadBit("bit 149"); // 149
            guid[3] = packet.ReadBit(); // 19
            var hasPitch = !packet.ReadBit("Has no pitch"); // 112

            if (hasTrans) // 104
            {
                transportGuid[6] = packet.ReadBit(); // 62
                transportGuid[0] = packet.ReadBit(); // 56
                transportGuid[3] = packet.ReadBit(); // 59
                transportGuid[7] = packet.ReadBit(); // 63
                hasTransTime2 = packet.ReadBit("hasTransTime2"); // 92
                hasTransTime3 = packet.ReadBit("hasTransTime3"); // 100
                transportGuid[4] = packet.ReadBit(); // 60
                transportGuid[1] = packet.ReadBit(); // 57
                transportGuid[2] = packet.ReadBit(); // 58
                transportGuid[5] = packet.ReadBit(); // 61
            }

            if (hasMovementFlags) // 24
                packet.ReadBitsE<MovementFlag>("Movement Flags", 30); // 24

            if (hasMovementFlags2) // 28
                packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 13); // 28

            if (hasFallData) // 140
                hasFallDirection = packet.ReadBit("Has Fall Direction"); // 136

            packet.ResetBitReader();
            packet.ParseBitStream(guid, 6);
            packet.ParseBitStream(guid184, 5);
            packet.ParseBitStream(guid, 2);

            for (var cnt = 0; cnt < Count; cnt++)
                packet.ReadInt32("Dword 156", cnt); // 156

            packet.ParseBitStream(guid, 5);
            packet.ParseBitStream(guid184, 4, 7);
            packet.ParseBitStream(guid, 3, 7);
            packet.ParseBitStream(guid184, 1, 3, 2);
            packet.ParseBitStream(guid, 4, 1);
            packet.ParseBitStream(guid184, 6, 0);
            packet.ParseBitStream(guid, 0);

            if (hasSpline) // 168
                packet.ReadInt32("Spline"); // 168

            if (hasFallData) // 140
            {
                packet.ReadSingle("Vertical Speed"); // 120
                if (hasFallDirection) // 136
                {
                    packet.ReadSingle("Fall Sin"); // 124
                    packet.ReadSingle("Fall Cos"); // 128
                    packet.ReadSingle("Horizontal Speed"); // 132
                }
                packet.ReadUInt32("Fall time"); // 116
            }

            if (hasTrans) // 104
            {
                var tpos = new Vector4();
                packet.ReadUInt32("Transport Time"); // 84
                packet.ReadXORByte(transportGuid, 6); // 62
                tpos.Z = packet.ReadSingle(); // 72
                packet.ReadXORByte(transportGuid, 4); // 60
                packet.ReadXORByte(transportGuid, 7); // 63
                tpos.O = packet.ReadSingle(); // 76
                packet.ReadXORByte(transportGuid, 5); // 61
                packet.ReadSByte("Transport Seat"); // 80
                tpos.X = packet.ReadSingle(); // 64
                if (hasTransTime3) // 100
                    packet.ReadUInt32("Transport Time 3"); // 96
                packet.ReadXORByte(transportGuid, 1); // 57
                packet.ReadXORByte(transportGuid, 2); // 58
                packet.ReadXORByte(transportGuid, 0); // 56
                if (hasTransTime2) // 92
                    packet.ReadUInt32("Transport Time 2"); // 88
                tpos.Y = packet.ReadSingle(); // 68
                packet.ReadXORByte(transportGuid, 3); // 59
                packet.WriteGuid("Transport Guid", transportGuid);
                packet.WriteLine("Transport Position: {0}", tpos);
            }

            if (hasSplineElev)
                packet.ReadSingle("Spline elevation"); // 144

            if (hasO)
                pos.O = packet.ReadSingle(); // 48

            if (hasPitch)
                packet.ReadSingle("Pitch"); // 112

            if (hasTime)
                packet.ReadUInt32("Timestamp"); // 32

            packet.WriteGuid("Guid", guid);
            packet.WriteGuid("Guid184", guid184);
            packet.WriteLine("Position: {0}", pos);
        }

        [Parser(Opcode.CMSG_DISCARDED_TIME_SYNC_ACKS)]
        public static void HandleDiscardedTimeSyncAcks(Packet packet)
        {
            var hasData = !packet.ReadBit("!hasSequenceIndex");
            packet.ResetBitReader();
            if (hasData)
                packet.ReadInt32("MaxSequenceIndex");
        }

        [Parser(Opcode.CMSG_FORCE_MOVE_ROOT_ACK)]
        public static void HandleForceMoveRootAck(Packet packet)
        {
            ReadPlayerMovementInfo(packet, info.MovementForceMoveRootAck);
        }

        [Parser(Opcode.CMSG_FORCE_MOVE_UNROOT_ACK)]
        public static void HandleForceMoveUnRootAck(Packet packet)
        {
            ReadPlayerMovementInfo(packet, info.MovementForceMoveUnrootAck);
        }

        [Parser(Opcode.CMSG_MOVE_APPLY_MOVEMENT_FORCE_ACK)]
        public static void HandleMoveApplyMovementForceAck(Packet packet)
        {
            ReadPlayerMovementInfo(packet, info.MovementApplyMovementForceAck);
        }

        [Parser(Opcode.CMSG_MOVE_FEATHER_FALL_ACK)]
        public static void HandleMoveFeatherFallAck(Packet packet)
        {
            ReadPlayerMovementInfo(packet, info.MovementFeatherFallAck);
        }

        [Parser(Opcode.CMSG_MOVE_FORCE_FLIGHT_SPEED_CHANGE_ACK)]
        public static void HandleMoveForceFlightSpeedChangeAck(Packet packet)
        {
            ReadPlayerMovementInfo(packet, info.MovementForceFlightSpeedChangeAck);
        }

        [Parser(Opcode.CMSG_MOVE_FORCE_RUN_BACK_SPEED_CHANGE_ACK)]
        public static void HandleMoveForceRunBackSpeedChangeAck(Packet packet)
        {
            ReadPlayerMovementInfo(packet, info.MovementForceRunBackSpeedChangeAck);
        }

        [Parser(Opcode.CMSG_MOVE_FORCE_RUN_SPEED_CHANGE_ACK)]
        public static void HandleMoveForceRunSpeedChangeAck(Packet packet)
        {
            ReadPlayerMovementInfo(packet, info.MovementForceRunSpeedChangeAck);
        }

        [Parser(Opcode.CMSG_MOVE_FORCE_SWIM_SPEED_CHANGE_ACK)]
        public static void HandleMoveForceSwimSpeedChangeAck(Packet packet)
        {
            ReadPlayerMovementInfo(packet, info.MovementForceSwimSpeedChangeAck);
        }

        [Parser(Opcode.CMSG_MOVE_FORCE_WALK_SPEED_CHANGE_ACK)]
        public static void HandleMoveForceWalkSpeedChangeAck(Packet packet)
        {
            ReadPlayerMovementInfo(packet, info.MovementForceWalkSpeedChangeAck);
        }

        [Parser(Opcode.CMSG_MOVE_GRAVITY_DISABLE_ACK)]
        public static void HandleMoveGravityDisableAck(Packet packet)
        {
            ReadPlayerMovementInfo(packet, info.MovementGravityDisableAck);
        }

        [Parser(Opcode.CMSG_MOVE_GRAVITY_ENABLE_ACK)]
        public static void HandleMoveGravityEnableAck(Packet packet)
        {
            ReadPlayerMovementInfo(packet, info.MovementGravityEnableAck);
        }

        [Parser(Opcode.CMSG_MOVE_REMOVE_MOVEMENT_FORCE_ACK)]
        public static void HandleMoveRemoveMovementForceAck(Packet packet)
        {
            ReadPlayerMovementInfo(packet, info.MovementRemoveMovementForceAck);
        }

        [Parser(Opcode.CMSG_MOVE_SET_COLLISION_HEIGHT_ACK)]
        public static void HandleMoveSetCollisionHeightAck(Packet packet)
        {
            ReadPlayerMovementInfo(packet, info.MovementSetCollisionHeightAck);
        }

        [Parser(Opcode.CMSG_MOVE_SET_CAN_FLY_ACK)]
        public static void HandleMoveSetCanFlyAck(Packet packet)
        {
            ReadPlayerMovementInfo(packet, info.MovementSetCanFlyAck);
        }

        [Parser(Opcode.CMSG_MOVE_SET_CAN_TRANSITION_BETWEEN_SWIM_AND_FLY_ACK)]
        public static void HandleMoveSetCanTransitionBetweenSwimAndFlyAck(Packet packet)
        {
            ReadPlayerMovementInfo(packet, info.MovementSetCanTransitionBetweenSwimAndFlyAck);
        }

        [Parser(Opcode.CMSG_MOVE_SPLINE_DONE)]
        public static void HandleMoveSplineDone(Packet packet)
        {
            ReadPlayerMovementInfo(packet, info.MovementSplineDone);
        }

        [Parser(Opcode.CMSG_MOVE_TELEPORT_ACK)]
        public static void HandleMoveTeleportAck(Packet packet)
        {
            packet.ReadInt32("Time");
            packet.ReadInt32("Flags");

            var guid = packet.StartBitStream(0, 7, 3, 5, 4, 6, 1, 2);
            packet.ParseBitStream(guid, 4, 1, 6, 7, 0, 2, 5, 3);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.CMSG_MOVE_TIME_SKIPPED)]
        public static void HandleMoveTimeSkipped(Packet packet)
        {
            packet.ReadInt32("Time"); // 24
            var guid = packet.StartBitStream(5, 0, 7, 4, 1, 2, 6, 3);
            packet.ResetBitReader();
            packet.ParseBitStream(guid, 7, 2, 0, 6, 1, 5, 3, 4);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.CMSG_SET_ACTIVE_MOVER)]
        public static void HandleSetAsctiveMover(Packet packet)
        {
            packet.ReadBit("unk");
            var guid = packet.StartBitStream(3, 0, 2, 1, 5, 4, 7, 6);
            packet.ResetBitReader();
            packet.ParseBitStream(guid, 3, 4, 5, 2, 7, 0, 1, 6);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.CMSG_UNK_00D9)]
        public static void HandleCUnk00D9(Packet packet)
        {
            // ������� ��� ������� ����� ����������� (� ����� �� ������ �����)
            ReadPlayerMovementInfo(packet, info.CUnk00D9);
        }

        [Parser(Opcode.CMSG_MOVE_KNOCK_BACK_ACK)]
        public static void HandleMoveKnockBackAck(Packet packet)
        {
            ReadPlayerMovementInfo(packet, info.MovementKnockBackAck);
        }

        [Parser(Opcode.CMSG_MOVE_SET_FLY)]
        public static void HandleMoveSetFly(Packet packet)
        {
            ReadPlayerMovementInfo(packet, info.MovementSetFly); // 679E4F
        }

        [Parser(Opcode.CMSG_MOVE_CHANGE_TRANSPORT)]
        public static void HandleMoveChngTransport(Packet packet)
        {
            ReadPlayerMovementInfo(packet, info.MovementChngTransport);
        }

        [Parser(Opcode.CMSG_UNK_09FA)]
        public static void HandleCUnk09FA(Packet packet)
        {
            ReadPlayerMovementInfo(packet, info.CUnk09FA);
        }

        [Parser(Opcode.CMSG_UNK_185B)]
        public static void HandleCUnk185B(Packet packet)
        {
            ReadPlayerMovementInfo(packet, info.CUnk185B);
        }

        [Parser(Opcode.CMSG_MOVE_FALL_LAND)]
        public static void HandleMoveFallLand(Packet packet)
        {
            ReadPlayerMovementInfo(packet, info.MovementFallLand);
        }

        [Parser(Opcode.SMSG_MOVE_APPLY_MOVEMENT_FORCE)]
        public static void HandleMoveApplyMovementForce(Packet packet)
        {
            var guid48 = new byte[8];
            var direction = new Vector3();
            guid48[2] = packet.ReadBit();
            guid48[3] = packet.ReadBit();
            packet.ReadBits("Type", 2); // 40
            guid48[7] = packet.ReadBit();
            guid48[5] = packet.ReadBit();
            guid48[0] = packet.ReadBit();
            guid48[1] = packet.ReadBit();
            guid48[6] = packet.ReadBit();
            guid48[4] = packet.ReadBit();

            packet.ParseBitStream(guid48, 6);
            direction.Y = packet.ReadSingle(); // 24
            packet.ParseBitStream(guid48, 4);
            direction.Z = packet.ReadSingle(); // 28
            packet.ReadInt32("MCounter"); // 44
            packet.ReadInt32("Mount Display ID"); // 16
            packet.ParseBitStream(guid48, 5);
            packet.ReadSingle("Magnitude"); // 36
            packet.ParseBitStream(guid48, 0, 7, 1, 3, 2);
            packet.ReadInt32("unk32"); // 32
            direction.X = packet.ReadSingle(); // 20

            packet.AddValue("Direction", direction);
            packet.WriteGuid("Guid", guid48);
        }

        [Parser(Opcode.SMSG_MOVE_FEATHER_FALL)]
        public static void HandleMoveFeatherFall(Packet packet)
        {
            ReadPlayerMovementInfo(packet, info.MovementFeatherFall);
        }

        [Parser(Opcode.SMSG_MOVE_GRAVITY_DISABLE)]
        public static void HandleMoveGravityDisable(Packet packet)
        {
            ReadPlayerMovementInfo(packet, info.MovementGravityDisable);
        }

        [Parser(Opcode.SMSG_MOVE_GRAVITY_ENABLE)]
        public static void HandleMoveGravityEnable(Packet packet)
        {
            ReadPlayerMovementInfo(packet, info.MovementGravityEnable);
        }

        [Parser(Opcode.SMSG_MOVE_LAND_WALK)]
        public static void HandleMoveLandWalk(Packet packet)
        {
            ReadPlayerMovementInfo(packet, info.MovementLandWalk);
        }

        [Parser(Opcode.SMSG_MOVE_NORMAL_FALL)]
        public static void HandleMoveNormalFall(Packet packet)
        {
            ReadPlayerMovementInfo(packet, info.MovementNormalFall);
        }

        [Parser(Opcode.SMSG_MOVE_REMOVE_MOVEMENT_FORCE)]
        public static void HandleMoveRemoveMovementForce(Packet packet)
        {
            ReadPlayerMovementInfo(packet, info.MovementRemoveMovementForce);
        }

        [Parser(Opcode.SMSG_MOVE_ROOT)]
        public static void HandleMoveRoot(Packet packet)
        {
            ReadPlayerMovementInfo(packet, info.MovementRoot);
        }

        [Parser(Opcode.SMSG_MOVE_SET_CAN_FLY)]
        public static void HandleMoveSetCanFly(Packet packet)
        {
            ReadPlayerMovementInfo(packet, info.MovementSetCanFly);
        }

        [Parser(Opcode.SMSG_MOVE_SET_CAN_TRANSITION_BETWEEN_SWIM_AND_FLY)]
        public static void HandleMoveSetCanTransitionBetweenSwimAndFly(Packet packet)
        {
            ReadPlayerMovementInfo(packet, info.MovementSetCanTransitionBetweenSwimAndFly);
        }

        [Parser(Opcode.SMSG_MOVE_UNROOT)]
        public static void HandleMoveUnRoot(Packet packet)
        {
            ReadPlayerMovementInfo(packet, info.MovementUnroot);
        }

        [Parser(Opcode.SMSG_MOVE_UNSET_CAN_FLY)]
        public static void HandleMoveUnSetCanFly(Packet packet)
        {
            ReadPlayerMovementInfo(packet, info.MovementUnSetCanFly);
        }

        [Parser(Opcode.SMSG_MOVE_UNSET_CAN_TRANSITION_BETWEEN_SWIM_AND_FLY)]
        public static void HandleMoveUnSetCanTransitionBetweenSwimAndFly(Packet packet)
        {
            ReadPlayerMovementInfo(packet, info.MovementUnSetCanTransitionBetweenSwimAndFly);
        }

        [Parser(Opcode.SMSG_MOVE_UPDATE_COLLISION_HEIGHT)]
        public static void HandleMoveUpdateCollisionHeight(Packet packet)
        {
            ReadPlayerMovementInfo(packet, info.MovementUpdateCollisionHeight);
        }

        [Parser(Opcode.SMSG_MOVE_UPDATE_FLIGHT_BACK_SPEED)] // C976E6
        public static void HandleMoveUpdateFlightBackSpeed(Packet packet)
        {
        }

        [Parser(Opcode.SMSG_MOVE_UPDATE_FLIGHT_SPEED)]
        public static void HandleMoveUpdateFlightSpeed(Packet packet)
        {
            ReadPlayerMovementInfo(packet, info.MovementUpdateFlightSpeed);
        }

        [Parser(Opcode.SMSG_MOVE_UPDATE_KNOCK_BACK)]
        public static void HandleMoveUpdateKnockBack(Packet packet)
        {
            ReadPlayerMovementInfo(packet, info.MovementUpdateKnockBack);
        }

        [Parser(Opcode.SMSG_MOVE_UPDATE_PITCH_RATE)] // C9F510
        public static void HandleMoveUpdatePitchRate(Packet packet)
        {
        }

        [Parser(Opcode.SMSG_MOVE_UPDATE_RUN_BACK_SPEED)]
        public static void HandleMoveUpdateRunBackSpeed(Packet packet)
        {
            ReadPlayerMovementInfo(packet, info.MovementUpdateRunBackSpeed);
        }

        [Parser(Opcode.SMSG_MOVE_UPDATE_RUN_SPEED)]
        public static void HandleMoveUpdateRunSpeed(Packet packet)
        {
            ReadPlayerMovementInfo(packet, info.MovementUpdateRunSpeed);
        }

        [Parser(Opcode.SMSG_MOVE_UPDATE_SWIM_BACK_SPEED)] // C9A10C
        public static void HandleMoveUpdateSwimBackSpeed(Packet packet)
        {
        }

        [Parser(Opcode.SMSG_MOVE_UPDATE_SWIM_SPEED)]
        public static void HandleMoveUpdateSwimSpeed(Packet packet)
        {
            ReadPlayerMovementInfo(packet, info.MovementUpdateSwimSpeed);
        }

        [Parser(Opcode.SMSG_MOVE_UPDATE_TELEPORT)] // CA3771
        public static void HandleMoveUpdateTeleport(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasFallDirection = false;
            var pos = new Vector4();

            var hasTransport = packet.ReadBit("hasTransport"); // 144
            var hasSpline = !packet.ReadBit("!hasSpline"); // 72
            var hasFallData = packet.ReadBit(); // 180
            packet.ReadBit("unk189"); // 189
            var unk98h = !packet.ReadBit("!unk 98h"); // 98h

            var hasTransportTime2 = false;
            var hasTransportTime3 = false;
            if (hasTransport)
            {
                transportGuid[7] = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
                transportGuid[0] = packet.ReadBit(); // 96
                transportGuid[2] = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                hasTransportTime3 = packet.ReadBit("hasTransportT3"); // 140
                transportGuid[5] = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                hasTransportTime2 = packet.ReadBit("hasTransportT2"); // 132
                transportGuid[1] = packet.ReadBit();
            }

            guid[0] = packet.ReadBit();
            var hasSwimSpeed = packet.ReadBit("hasSwimSpeed"); // 44
            packet.ReadBit("unk188"); // 188
            var hasMovementFlags = !packet.ReadBit("!hasMovementFlags"); // 64
            guid[1] = packet.ReadBit();
            var unk228 = packet.ReadBit("unk228"); // 228
            var hasRunBackSpeed = packet.ReadBit("hasRunBackSpeed"); // 20
            var unk52 = packet.ReadBit("unk52"); // 52

            var count240 = packet.ReadBits("count240", 19);

            var hasTime = !packet.ReadBit("!hasTime"); // 208
            guid[6] = packet.ReadBit();
            var hasO = !packet.ReadBit("!hasO"); // 58h
            var unk28 = packet.ReadBit("unk28"); // 28
            if (hasFallData) // 180
                hasFallDirection = packet.ReadBit(); // 176
            var hasFlightSpeed = packet.ReadBit("hasFlightSpeed"); // 220
            var unk260 = packet.ReadBit("unk260"); // 260
            guid[3] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            var hasRunSpeed = packet.ReadBit("hasRunSpeed"); // 36
            var unkb8h = !packet.ReadBit("!unk b8h"); // b8h
            packet.ReadBit("unk212"); // 212
            guid[5] = packet.ReadBit();
            for (var i = 0; i < count240; i++)
                packet.ReadBits("unk268", 2, i); // 268
            var unk236 = packet.ReadBit("unk236"); // 236
            guid[2] = packet.ReadBit();
            var count192 = packet.ReadBits("count 192", 22); // 192
            var hasMovementFlagsExtra = !packet.ReadBit(); // 68
            if (hasMovementFlags) // 64
                packet.ReadBitsE<MovementFlag>("Movement flags", 30);
            guid[7] = packet.ReadBit();
            if (hasMovementFlagsExtra) // 68
                packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 13);
            if (hasFlightSpeed)
                packet.ReadSingle("FlightSpeed"); // d8h

            if (hasTransport) // 144
            {
                var tpos = new Vector4();

                packet.ReadXORByte(transportGuid, 3);
                packet.ReadXORByte(transportGuid, 5);
                packet.ReadUInt32("Transport Time"); // 124
                tpos.Z = packet.ReadSingle(); // 70h
                tpos.O = packet.ReadSingle(); // 74h
                packet.ReadXORByte(transportGuid, 4);
                packet.ReadXORByte(transportGuid, 1);
                packet.ReadXORByte(transportGuid, 7);
                tpos.Y = packet.ReadSingle(); // 6ch

                if (hasTransportTime2) // 132
                    packet.ReadUInt32("Transport Time 2"); // 128

                packet.ReadXORByte(transportGuid, 0);
                packet.ReadXORByte(transportGuid, 2);

                if (hasTransportTime3) // 140
                    packet.ReadUInt32("Transport Time 3"); // 136

                packet.ReadSByte("Transport Seat"); // 120
                tpos.X = packet.ReadSingle(); // 68h
                packet.ReadXORByte(transportGuid, 6);

                packet.WriteGuid("Transport Guid", transportGuid);
                packet.AddValue("Transport Position", tpos);
            }
            packet.ReadXORByte(guid, 3);

            if (hasFallData) // 180
            {
                if (hasFallDirection) // 176
                {
                    packet.ReadSingle("Fall Sin"); // a8h
                    packet.ReadSingle("Horizontal speed"); // ach
                    packet.ReadSingle("Fall Cos"); // a4h
                }
                packet.ReadUInt32("Fall time"); // 156
                packet.ReadSingle("Vertical speed"); // a0h
            }
            packet.ReadXORByte(guid, 4);

            for (var i = 0; i < count240; i++)
            {
                packet.ReadInt32("unk260", i); // 260
                packet.ReadInt32("unk244", i); // 244
                packet.ReadSingle("unk248", i); // 248
                packet.ReadSingle("unk252", i); // 252
                packet.ReadSingle("unk256", i); // 256
                packet.ReadSingle("unk264", i); // 264
            }

            if (unk260)
                packet.ReadSingle("unk100h"); // 100h

            pos.Y = packet.ReadSingle(); // 50h

            if (unkb8h)
                packet.ReadSingle("unkb8h"); // b8h

            if (unk236)
                packet.ReadSingle("unke8h"); // e8h

            if (unk98h)
                packet.ReadSingle("unk98h"); // 98h

            if (hasRunBackSpeed)
                packet.ReadSingle("RunBackSpeed"); // 10h

            packet.ReadXORByte(guid, 2);

            packet.ReadXORByte(guid, 7);

            if (hasSwimSpeed)
                packet.ReadSingle("SwimSpeed"); // 28h

            pos.X = packet.ReadSingle(); // 4ch

            if (hasRunSpeed)
                packet.ReadSingle("RunSpeed"); // 20h

            if (unk28)
                packet.ReadSingle("unk18h"); // 18h

            for (var i = 0; i < count192; i++)
                packet.ReadInt32("unk196", i); // 196

            pos.Z = packet.ReadSingle(); // 54h

            if (hasSpline)
                packet.ReadInt32("Spline ID"); // 72

            if (hasTime)
                packet.ReadInt32("TimeStamp"); // 208

            packet.ReadXORByte(guid, 6);

            if (unk52)
                packet.ReadSingle("unk30h");

            packet.ReadXORByte(guid, 5);
            packet.ReadXORByte(guid, 1);

            if (hasO)
                pos.O = packet.ReadSingle(); // 58h

            packet.ReadXORByte(guid, 0);

            if (unk228)
                packet.ReadSingle("unke0h"); // e0h

            packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
        }

        [Parser(Opcode.SMSG_MOVE_UPDATE_TURN_RATE)] // CA2E66
        public static void HandleMoveUpdateTurnRate(Packet packet)
        {
        }

        [Parser(Opcode.SMSG_MOVE_UPDATE_WALK_SPEED)]
        public static void HandleMoveUpdateWalkSpeed(Packet packet)
        {
            ReadPlayerMovementInfo(packet, info.MovementUpdateWalkSpeed);
        }

        [Parser(Opcode.SMSG_MOVE_UPDATE)]
        public static void HandleMoveUpdate(Packet packet)
        {
            ReadPlayerMovementInfo(packet, info.PlayerMove);
        }

        [Parser(Opcode.SMSG_SPLINE_MOVE_COLLISION_DISABLE)]
        public static void HandleSplineMoveCollisionDisable(Packet packet)
        {
            ReadPlayerMovementInfo(packet, info.MovementSplineCollisionDisable);
        }

        [Parser(Opcode.SMSG_SPLINE_MOVE_COLLISION_ENABLE)]
        public static void HandleSplineMoveCollisionEnable(Packet packet)
        {
            ReadPlayerMovementInfo(packet, info.MovementSplineCollisionEnable);
        }

        [Parser(Opcode.SMSG_SPLINE_MOVE_GRAVITY_DISABLE)]
        public static void HandleSplineMoveGravityDisable(Packet packet)
        {
            ReadPlayerMovementInfo(packet, info.MovementSplineGravityDisable);
        }

        [Parser(Opcode.SMSG_SPLINE_MOVE_GRAVITY_ENABLE)]
        public static void HandleSplineMoveGravityEnable(Packet packet)
        {
            ReadPlayerMovementInfo(packet, info.MovementSplineGravityEnable);
        }

        [Parser(Opcode.SMSG_SPLINE_MOVE_ROOT)]
        public static void HandleSplineMoveRoot(Packet packet)
        {
            ReadPlayerMovementInfo(packet, info.MovementSplineRoot);
        }

        [Parser(Opcode.SMSG_SPLINE_MOVE_SET_FLYING)]
        public static void HandleSplineMoveSetFlying(Packet packet)
        {
            ReadPlayerMovementInfo(packet, info.MovementSplineSetFlying);
        }

        [Parser(Opcode.SMSG_SPLINE_MOVE_START_SWIM)]
        public static void HandleSplineMoveStartswim(Packet packet)
        {
            ReadPlayerMovementInfo(packet, info.MovementSplineStartSwim);
        }

        [Parser(Opcode.SMSG_SPLINE_MOVE_UNROOT)]
        public static void HandleSplineMoveUnroot(Packet packet)
        {
            ReadPlayerMovementInfo(packet, info.MovementSplineUnRoot);
        }

        [Parser(Opcode.CMSG_MOVE_HEARTBEAT)]
        public static void HandleMoveHeartbeat(Packet packet)
        {
            ReadPlayerMovementInfo(packet, info.MovementHeartBeat);
        }

        [Parser(Opcode.CMSG_MOVE_JUMP)]
        public static void HandleMoveJump(Packet packet)
        {
            ReadPlayerMovementInfo(packet, info.MovementJump);
        }

        [Parser(Opcode.CMSG_MOVE_SET_FACING)]
        public static void HandleMoveSetFacing(Packet packet)
        {
            ReadPlayerMovementInfo(packet, info.MovementSetFacing);
        }

        [Parser(Opcode.CMSG_MOVE_SET_PITCH)]
        public static void HandleMoveSetPitch(Packet packet)
        {
            ReadPlayerMovementInfo(packet, info.MovementSetPitch);
        }

        [Parser(Opcode.CMSG_MOVE_SET_RUN_MODE)]
        public static void HandleMoveSetRunMode(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();

            pos.Z = packet.ReadSingle(); // 44
            pos.Y = packet.ReadSingle(); // 40
            pos.X = packet.ReadSingle(); // 36
            var hasPitch = !packet.ReadBit("Has no pitch"); // 112
            var hasSplineElev = !packet.ReadBit("Has no Spline Elevation"); // 144
            var Count = packet.ReadBits("Counter", 22); // 152
            var hasO = !packet.ReadBit("Has no Orient"); // 48
            guid[1] = packet.ReadBit(); // 17
            guid[4] = packet.ReadBit(); // 20
            guid[0] = packet.ReadBit(); // 16
            guid[3] = packet.ReadBit(); // 19
            var hasSpline = !packet.ReadBit("has no Spline"); // 168
            guid[6] = packet.ReadBit(); // 22
            var hasTime = !packet.ReadBit("Has no timestamp"); // 32
            guid[2] = packet.ReadBit(); // 18
            guid[5] = packet.ReadBit(); // 21
            var hasMovementFlags2 = !packet.ReadBit("has no MovementFlags2"); // 28
            var hasFallData = packet.ReadBit("Has fall data"); // 140
            guid[7] = packet.ReadBit(); // 23
            packet.ReadBit("bit 148"); // 148
            packet.ReadBit("bit 172"); // 172
            packet.ReadBit("bit 149"); // 149
            var hasMovementFlags = !packet.ReadBit("has no MovementFlags"); // 24
            var hasTrans = packet.ReadBit("Has transport"); // 104

            if (hasTrans) // 104
            {
                transportGuid[7] = packet.ReadBit(); // 63
                transportGuid[4] = packet.ReadBit(); // 60
                transportGuid[5] = packet.ReadBit(); // 61
                hasTransTime3 = packet.ReadBit("hasTransTime3"); // 100
                transportGuid[3] = packet.ReadBit(); // 59
                hasTransTime2 = packet.ReadBit("hasTransTime2"); // 92
                transportGuid[6] = packet.ReadBit(); // 62
                transportGuid[1] = packet.ReadBit(); // 57
                transportGuid[2] = packet.ReadBit(); // 58
                transportGuid[0] = packet.ReadBit(); // 56
            }

            if (hasFallData) // 140
                hasFallDirection = packet.ReadBit("Has Fall Direction"); // 136

            if (hasMovementFlags) // 24
                packet.ReadBitsE<MovementFlag>("Movement Flags", 30); // 24

            if (hasMovementFlags2) // 28
                packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 13); // 28

            packet.ResetBitReader();
            packet.ParseBitStream(guid, 5, 6, 3, 7, 1, 0);

            if (Count > 0) // 152
                for (var cnt = 0; cnt < Count; cnt++)
                {
                    packet.ReadInt32("Dword 156", cnt); // 156
                }

            packet.ParseBitStream(guid, 4, 2);

            if (hasTrans) // 104
            {
                var tpos = new Vector4();
                tpos.Z = packet.ReadSingle(); // 72
                packet.ReadXORByte(transportGuid, 3); // 59
                if (hasTransTime3) // 100
                    packet.ReadUInt32("Transport Time 3"); // 96
                packet.ReadXORByte(transportGuid, 0); // 56
                packet.ReadXORByte(transportGuid, 1); // 57
                packet.ReadXORByte(transportGuid, 7); // 63
                tpos.O = packet.ReadSingle(); // 76
                packet.ReadSByte("Transport Seat"); // 80
                tpos.X = packet.ReadSingle(); // 64
                packet.ReadUInt32("Transport Time"); // 84
                if (hasTransTime2) // 92
                    packet.ReadUInt32("Transport Time 2"); // 88
                packet.ReadXORByte(transportGuid, 4); // 60
                packet.ReadXORByte(transportGuid, 6); // 62
                packet.ReadXORByte(transportGuid, 2); // 58
                packet.ReadXORByte(transportGuid, 5); // 61
                tpos.Y = packet.ReadSingle(); // 68
                packet.WriteGuid("Transport Guid", transportGuid);
                packet.WriteLine("Transport Position: {0}", tpos);
            }

            if (hasFallData) // 140
            {
                if (hasFallDirection) // 136
                {
                    packet.ReadSingle("Fall Sin"); // 124
                    packet.ReadSingle("Horizontal Speed"); // 132
                    packet.ReadSingle("Fall Cos"); // 128
                }
                packet.ReadUInt32("Fall time"); // 116
                packet.ReadSingle("Vertical Speed"); // 120
            }

            if (hasPitch)
                packet.ReadSingle("Pitch"); // 112

            if (hasO)
                pos.O = packet.ReadSingle(); // 48

            if (hasSplineElev)
                packet.ReadSingle("Spline elevation"); // 144

            if (hasTime)
                packet.ReadUInt32("Timestamp"); // 32

            if (hasSpline) // 168
                packet.ReadInt32("Spline"); // 168

            packet.WriteGuid("Guid", guid);
            packet.WriteLine("Position: {0}", pos);
        }

        [Parser(Opcode.CMSG_MOVE_SET_WALK_MODE)]
        public static void HandleMoveSetWalkMode(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();

            pos.Y = packet.ReadSingle(); // 40
            pos.X = packet.ReadSingle(); // 36
            pos.Z = packet.ReadSingle(); // 44
            var hasSplineElev = !packet.ReadBit("Has no Spline Elevation"); // 144
            packet.ReadBit("bit 172"); // 172
            var Count = packet.ReadBits("Counter", 22); // 152
            var hasMovementFlags = !packet.ReadBit("has no MovementFlags"); // 24
            guid[2] = packet.ReadBit(); // 18
            guid[4] = packet.ReadBit(); // 20
            guid[5] = packet.ReadBit(); // 21
            guid[1] = packet.ReadBit(); // 17
            guid[0] = packet.ReadBit(); // 16
            var hasFallData = packet.ReadBit("Has fall data"); // 140
            var hasMovementFlags2 = !packet.ReadBit("has no MovementFlags2"); // 28
            var hasPitch = !packet.ReadBit("Has no pitch"); // 112
            var hasTime = !packet.ReadBit("Has no timestamp"); // 32
            packet.ReadBit("bit 148"); // 148
            guid[7] = packet.ReadBit(); // 23
            guid[3] = packet.ReadBit(); // 19
            var hasSpline = !packet.ReadBit("has no Spline"); // 168
            guid[6] = packet.ReadBit(); // 22
            packet.ReadBit("bit 149"); // 149
            var hasO = !packet.ReadBit("Has no Orient"); // 48
            var hasTrans = packet.ReadBit("Has transport"); // 104

            if (hasTrans) // 104
            {
                transportGuid[6] = packet.ReadBit(); // 62
                transportGuid[0] = packet.ReadBit(); // 56
                hasTransTime3 = packet.ReadBit("hasTransTime3"); // 100
                transportGuid[4] = packet.ReadBit(); // 60
                transportGuid[5] = packet.ReadBit(); // 61
                transportGuid[1] = packet.ReadBit(); // 57
                transportGuid[7] = packet.ReadBit(); // 63
                transportGuid[3] = packet.ReadBit(); // 59
                hasTransTime2 = packet.ReadBit("hasTransTime2"); // 92
                transportGuid[2] = packet.ReadBit(); // 58
            }

            if (hasMovementFlags2) // 28
                packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 13); // 28

            if (hasMovementFlags) // 24
                packet.ReadBitsE<MovementFlag>("Movement Flags", 30); // 24

            if (hasFallData) // 140
                hasFallDirection = packet.ReadBit("Has Fall Direction"); // 136

            packet.ResetBitReader();
            packet.ParseBitStream(guid, 4, 3, 7, 2, 1, 0, 6);

            if (Count > 0) // 152
                for (var cnt = 0; cnt < Count; cnt++)
                {
                    packet.ReadInt32("Dword 156", cnt); // 156
                }

            packet.ParseBitStream(guid, 5);

            if (hasTrans) // 104
            {
                var tpos = new Vector4();
                tpos.X = packet.ReadSingle(); // 64
                packet.ReadXORByte(transportGuid, 6); // 62
                tpos.Z = packet.ReadSingle(); // 72
                packet.ReadXORByte(transportGuid, 0); // 56
                packet.ReadXORByte(transportGuid, 1); // 57
                packet.ReadXORByte(transportGuid, 4); // 60
                packet.ReadSByte("Transport Seat"); // 80
                packet.ReadXORByte(transportGuid, 7); // 63
                tpos.Y = packet.ReadSingle(); // 68
                packet.ReadXORByte(transportGuid, 5); // 61
                packet.ReadUInt32("Transport Time"); // 84
                packet.ReadXORByte(transportGuid, 2); // 58
                tpos.O = packet.ReadSingle(); // 76
                packet.ReadXORByte(transportGuid, 3); // 59
                if (hasTransTime2) // 92
                    packet.ReadUInt32("Transport Time 2"); // 88
                if (hasTransTime3) // 100
                    packet.ReadUInt32("Transport Time 3"); // 96
                packet.WriteGuid("Transport Guid", transportGuid);
                packet.WriteLine("Transport Position: {0}", tpos);
            }

            if (hasFallData) // 140
            {
                if (hasFallDirection) // 136
                {
                    packet.ReadSingle("Horizontal Speed"); // 132
                    packet.ReadSingle("Fall Sin"); // 124
                    packet.ReadSingle("Fall Cos"); // 128
                }
                packet.ReadSingle("Vertical Speed"); // 120
                packet.ReadUInt32("Fall time"); // 116
            }

            if (hasO)
                pos.O = packet.ReadSingle(); // 48

            if (hasPitch)
                packet.ReadSingle("Pitch"); // 112

            if (hasTime)
                packet.ReadUInt32("Timestamp"); // 32

            if (hasSplineElev)
                packet.ReadSingle("Spline elevation"); // 144

            if (hasSpline) // 168
                packet.ReadInt32("Spline"); // 168

            packet.WriteGuid("Guid", guid);
            packet.WriteLine("Position: {0}", pos);
        }

        [Parser(Opcode.CMSG_MOVE_START_ASCEND)]
        public static void HandleMoveStartAscend(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();

            pos.Y = packet.ReadSingle(); // 40
            pos.X = packet.ReadSingle(); // 36
            pos.Z = packet.ReadSingle(); // 44
            var hasO = !packet.ReadBit("Has no Orient"); // 48
            guid[3] = packet.ReadBit(); // 19
            var hasTrans = packet.ReadBit("Has transport"); // 104
            var hasMovementFlags = !packet.ReadBit("has no MovementFlags"); // 24
            packet.ReadBit("bit 172"); // 172
            guid[0] = packet.ReadBit(); // 16
            guid[4] = packet.ReadBit(); // 20
            var hasTime = !packet.ReadBit("Has no timestamp"); // 32
            guid[7] = packet.ReadBit(); // 23
            packet.ReadBit("bit 149"); // 149
            var hasPitch = !packet.ReadBit("Has no pitch"); // 112
            guid[5] = packet.ReadBit(); // 21
            var hasMovementFlags2 = !packet.ReadBit("has no MovementFlags2"); // 28
            packet.ReadBit("bit 148"); // 148
            guid[6] = packet.ReadBit(); // 22
            guid[2] = packet.ReadBit(); // 18
            var hasSpline = !packet.ReadBit("has no Spline"); // 168
            var Count = packet.ReadBits("Counter", 22); // 152
            guid[1] = packet.ReadBit(); // 17
            var hasSplineElev = !packet.ReadBit("Has no Spline Elevation"); // 144
            var hasFallData = packet.ReadBit("Has fall data"); // 140

            if (hasTrans) // 104
            {
                transportGuid[4] = packet.ReadBit(); // 60
                transportGuid[0] = packet.ReadBit(); // 56
                transportGuid[3] = packet.ReadBit(); // 59
                transportGuid[5] = packet.ReadBit(); // 61
                hasTransTime2 = packet.ReadBit("hasTransTime2"); // 92
                transportGuid[1] = packet.ReadBit(); // 57
                hasTransTime3 = packet.ReadBit("hasTransTime3"); // 100
                transportGuid[6] = packet.ReadBit(); // 62
                transportGuid[2] = packet.ReadBit(); // 58
                transportGuid[7] = packet.ReadBit(); // 63
            }

            if (hasFallData) // 140
                hasFallDirection = packet.ReadBit("Has Fall Direction"); // 136

            if (hasMovementFlags2) // 28
                packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 13); // 28

            if (hasMovementFlags) // 24
                packet.ReadBitsE<MovementFlag>("Movement Flags", 30); // 24

            packet.ResetBitReader();
            packet.ParseBitStream(guid, 2, 5);

            if (Count > 0) // 152
                for (var cnt = 0; cnt < Count; cnt++)
                {
                    packet.ReadInt32("Dword 156", cnt); // 156
                }

            packet.ParseBitStream(guid, 1, 0, 4, 7, 6, 3);

            if (hasO)
                pos.O = packet.ReadSingle(); // 48

            if (hasTime)
                packet.ReadUInt32("Timestamp"); // 32

            if (hasTrans) // 104
            {
                var tpos = new Vector4();
                packet.ReadXORByte(transportGuid, 3); // 59
                packet.ReadUInt32("Transport Time"); // 84
                tpos.Y = packet.ReadSingle(); // 68
                tpos.O = packet.ReadSingle(); // 76
                packet.ReadXORByte(transportGuid, 6); // 62
                if (hasTransTime3) // 100
                    packet.ReadUInt32("Transport Time 3"); // 96
                tpos.X = packet.ReadSingle(); // 64
                packet.ReadXORByte(transportGuid, 2); // 58
                if (hasTransTime2) // 92
                    packet.ReadUInt32("Transport Time 2"); // 88
                packet.ReadXORByte(transportGuid, 1); // 57
                packet.ReadXORByte(transportGuid, 7); // 63
                tpos.Z = packet.ReadSingle(); // 72
                packet.ReadSByte("Transport Seat"); // 80
                packet.ReadXORByte(transportGuid, 0); // 56
                packet.ReadXORByte(transportGuid, 4); // 60
                packet.ReadXORByte(transportGuid, 5); // 61
                packet.WriteGuid("Transport Guid", transportGuid);
                packet.WriteLine("Transport Position: {0}", tpos);
            }

            if (hasSplineElev)
                packet.ReadSingle("Spline elevation"); // 144

            if (hasFallData) // 140
            {
                packet.ReadSingle("Vertical Speed"); // 120
                if (hasFallDirection) // 136
                {
                    packet.ReadSingle("Fall Cos"); // 128
                    packet.ReadSingle("Fall Sin"); // 124
                    packet.ReadSingle("Horizontal Speed"); // 132
                }
                packet.ReadUInt32("Fall time"); // 116
            }

            if (hasPitch)
                packet.ReadSingle("Pitch"); // 112

            if (hasSpline) // 168
                packet.ReadInt32("Spline"); // 168

            packet.WriteGuid("Guid", guid);
            packet.WriteLine("Position: {0}", pos);
        }

        [Parser(Opcode.CMSG_MOVE_START_BACKWARD)]
        public static void HandleMoveStartBackWard(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();

            pos.Y = packet.ReadSingle(); // 40
            pos.Z = packet.ReadSingle(); // 44
            pos.X = packet.ReadSingle(); // 36
            var hasTime = !packet.ReadBit("Has no timestamp"); // 32
            var hasO = !packet.ReadBit("Has no Orient"); // 48
            guid[7] = packet.ReadBit(); // 23
            guid[2] = packet.ReadBit(); // 18
            var Count = packet.ReadBits("Counter", 22); // 152
            var hasFallData = packet.ReadBit("Has fall data"); // 140
            packet.ReadBit("bit 172"); // 172
            guid[5] = packet.ReadBit(); // 21
            guid[3] = packet.ReadBit(); // 19
            guid[6] = packet.ReadBit(); // 22
            var hasSplineElev = !packet.ReadBit("Has no Spline Elevation"); // 144
            guid[4] = packet.ReadBit(); // 20
            var hasTrans = packet.ReadBit("Has transport"); // 104
            guid[0] = packet.ReadBit(); // 16
            var hasMovementFlags = !packet.ReadBit("has no MovementFlags"); // 24
            var hasPitch = !packet.ReadBit("Has no pitch"); // 112
            var hasSpline = !packet.ReadBit("has no Spline"); // 168
            var hasMovementFlags2 = !packet.ReadBit("has no MovementFlags2"); // 28
            packet.ReadBit("bit 148"); // 148
            guid[1] = packet.ReadBit(); // 17
            packet.ReadBit("bit 149"); // 149

            if (hasTrans) // 104
            {
                transportGuid[1] = packet.ReadBit(); // 57
                hasTransTime2 = packet.ReadBit("hasTransTime2"); // 92
                transportGuid[0] = packet.ReadBit(); // 56
                transportGuid[7] = packet.ReadBit(); // 63
                hasTransTime3 = packet.ReadBit("hasTransTime3"); // 100
                transportGuid[3] = packet.ReadBit(); // 59
                transportGuid[5] = packet.ReadBit(); // 61
                transportGuid[6] = packet.ReadBit(); // 62
                transportGuid[2] = packet.ReadBit(); // 58
                transportGuid[4] = packet.ReadBit(); // 60
            }

            if (hasMovementFlags2) // 28
                packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 13); // 28

            if (hasMovementFlags) // 24
                packet.ReadBitsE<MovementFlag>("Movement Flags", 30); // 24

            if (hasFallData) // 140
                hasFallDirection = packet.ReadBit("Has Fall Direction"); // 136

            packet.ResetBitReader();
            if (Count > 0) // 152
                for (var cnt = 0; cnt < Count; cnt++)
                {
                    packet.ReadInt32("Dword 156", cnt); // 156
                }

            packet.ParseBitStream(guid, 1, 3, 5, 2, 0, 4, 7, 6);

            if (hasSpline) // 168
                packet.ReadInt32("Spline"); // 168

            if (hasTrans) // 104
            {
                var tpos = new Vector4();
                packet.ReadUInt32("Transport Time"); // 84
                packet.ReadXORByte(transportGuid, 4); // 60
                packet.ReadXORByte(transportGuid, 1); // 57
                packet.ReadXORByte(transportGuid, 5); // 61
                packet.ReadXORByte(transportGuid, 3); // 59
                packet.ReadXORByte(transportGuid, 6); // 62
                packet.ReadSByte("Transport Seat"); // 80
                tpos.O = packet.ReadSingle(); // 76
                tpos.X = packet.ReadSingle(); // 64
                packet.ReadXORByte(transportGuid, 0); // 56
                tpos.Y = packet.ReadSingle(); // 68
                if (hasTransTime3) // 100
                    packet.ReadUInt32("Transport Time 3"); // 96
                packet.ReadXORByte(transportGuid, 7); // 63
                if (hasTransTime2) // 92
                    packet.ReadUInt32("Transport Time 2"); // 88
                tpos.Z = packet.ReadSingle(); // 72
                packet.ReadXORByte(transportGuid, 2); // 58
                packet.WriteGuid("Transport Guid", transportGuid);
                packet.WriteLine("Transport Position: {0}", tpos);
            }

            if (hasO)
                pos.O = packet.ReadSingle(); // 48

            if (hasFallData) // 140
            {
                packet.ReadUInt32("Fall time"); // 116
                if (hasFallDirection) // 136
                {
                    packet.ReadSingle("Fall Cos"); // 128
                    packet.ReadSingle("Fall Sin"); // 124
                    packet.ReadSingle("Horizontal Speed"); // 132
                }
                packet.ReadSingle("Vertical Speed"); // 120
            }

            if (hasPitch)
                packet.ReadSingle("Pitch"); // 112

            if (hasTime)
                packet.ReadUInt32("Timestamp"); // 32

            if (hasSplineElev)
                packet.ReadSingle("Spline elevation"); // 144

            packet.WriteGuid("Guid", guid);
            packet.WriteLine("Position: {0}", pos);
        }

        [Parser(Opcode.CMSG_MOVE_START_DESCEND)]
        public static void HandleMoveStartDescend(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();

            pos.X = packet.ReadSingle(); // 36
            pos.Y = packet.ReadSingle(); // 40
            pos.Z = packet.ReadSingle(); // 44
            var hasFallData = packet.ReadBit("Has fall data"); // 140
            var hasMovementFlags = !packet.ReadBit("has no MovementFlags"); // 24
            guid[7] = packet.ReadBit(); // 23
            guid[0] = packet.ReadBit(); // 16
            guid[4] = packet.ReadBit(); // 20
            var hasMovementFlags2 = !packet.ReadBit("has no MovementFlags2"); // 28
            var hasPitch = !packet.ReadBit("Has no pitch"); // 112
            guid[6] = packet.ReadBit(); // 22
            guid[2] = packet.ReadBit(); // 18
            packet.ReadBit("bit 148"); // 148
            var hasSpline = !packet.ReadBit("has no Spline"); // 168
            var Count = packet.ReadBits("Counter", 22); // 152
            var hasTrans = packet.ReadBit("Has transport"); // 104
            var hasO = !packet.ReadBit("Has no Orient"); // 48
            guid[1] = packet.ReadBit(); // 17
            packet.ReadBit("bit 149"); // 149
            packet.ReadBit("bit 172"); // 172
            guid[3] = packet.ReadBit(); // 19
            guid[5] = packet.ReadBit(); // 21
            var hasSplineElev = !packet.ReadBit("Has no Spline Elevation"); // 144
            var hasTime = !packet.ReadBit("Has no timestamp"); // 32

            if (hasTrans) // 104
            {
                transportGuid[0] = packet.ReadBit(); // 56
                hasTransTime3 = packet.ReadBit("hasTransTime3"); // 100
                transportGuid[7] = packet.ReadBit(); // 63
                hasTransTime2 = packet.ReadBit("hasTransTime2"); // 92
                transportGuid[1] = packet.ReadBit(); // 57
                transportGuid[4] = packet.ReadBit(); // 60
                transportGuid[5] = packet.ReadBit(); // 61
                transportGuid[3] = packet.ReadBit(); // 59
                transportGuid[6] = packet.ReadBit(); // 62
                transportGuid[2] = packet.ReadBit(); // 58
            }

            if (hasMovementFlags2) // 28
                packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 13); // 28

            if (hasMovementFlags) // 24
                packet.ReadBitsE<MovementFlag>("Movement Flags", 30); // 24

            if (hasFallData) // 140
                hasFallDirection = packet.ReadBit("Has Fall Direction"); // 136

            packet.ResetBitReader();
            packet.ParseBitStream(guid, 4, 7, 1, 3);

            if (Count > 0) // 152
                for (var cnt = 0; cnt < Count; cnt++)
                {
                    packet.ReadInt32("Dword 156", cnt); // 156
                }

            packet.ParseBitStream(guid, 2, 6, 0, 5);

            if (hasTrans) // 104
            {
                var tpos = new Vector4();
                tpos.X = packet.ReadSingle(); // 64
                packet.ReadXORByte(transportGuid, 0); // 56
                packet.ReadXORByte(transportGuid, 3); // 59
                packet.ReadXORByte(transportGuid, 7); // 63
                packet.ReadSByte("Transport Seat"); // 80
                packet.ReadXORByte(transportGuid, 5); // 61
                packet.ReadXORByte(transportGuid, 1); // 57
                tpos.Y = packet.ReadSingle(); // 68
                if (hasTransTime3) // 100
                    packet.ReadUInt32("Transport Time 3"); // 96
                packet.ReadUInt32("Transport Time"); // 84
                packet.ReadXORByte(transportGuid, 4); // 60
                if (hasTransTime2) // 92
                    packet.ReadUInt32("Transport Time 2"); // 88
                tpos.O = packet.ReadSingle(); // 76
                tpos.Z = packet.ReadSingle(); // 72
                packet.ReadXORByte(transportGuid, 2); // 58
                packet.ReadXORByte(transportGuid, 6); // 62
                packet.WriteGuid("Transport Guid", transportGuid);
                packet.WriteLine("Transport Position: {0}", tpos);
            }

            if (hasFallData) // 140
            {
                packet.ReadUInt32("Fall time"); // 116
                if (hasFallDirection) // 136
                {
                    packet.ReadSingle("Fall Sin"); // 124
                    packet.ReadSingle("Horizontal Speed"); // 132
                    packet.ReadSingle("Fall Cos"); // 128
                }
                packet.ReadSingle("Vertical Speed"); // 120
            }

            if (hasPitch)
                packet.ReadSingle("Pitch"); // 112

            if (hasSpline) // 168
                packet.ReadInt32("Spline"); // 168

            if (hasSplineElev)
                packet.ReadSingle("Spline elevation"); // 144

            if (hasO)
                pos.O = packet.ReadSingle(); // 48

            if (hasTime)
                packet.ReadUInt32("Timestamp"); // 32

            packet.WriteGuid("Guid", guid);
            packet.WriteLine("Position: {0}", pos);
        }

        [Parser(Opcode.CMSG_MOVE_START_FORWARD)]
        public static void HandleMoveStartForward(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();

            pos.Z = packet.ReadSingle(); // 44
            pos.X = packet.ReadSingle(); // 36
            pos.Y = packet.ReadSingle(); // 40
            var hasMovementFlags2 = !packet.ReadBit("has no MovementFlags2"); // 28
            packet.ReadBit("bit 149"); // 149
            var hasSpline = !packet.ReadBit("has no Spline"); // 168
            packet.ReadBit("bit 148"); // 148
            guid[0] = packet.ReadBit(); // 16
            var hasO = !packet.ReadBit("Has no Orient"); // 48
            var hasFallData = packet.ReadBit("Has fall data"); // 140
            var Count = packet.ReadBits("Counter", 22); // 152
            guid[4] = packet.ReadBit(); // 20
            guid[1] = packet.ReadBit(); // 17
            var hasTime = !packet.ReadBit("Has no timestamp"); // 32
            guid[7] = packet.ReadBit(); // 23
            var hasPitch = !packet.ReadBit("Has no pitch"); // 112
            var hasTrans = packet.ReadBit("Has transport"); // 104
            guid[5] = packet.ReadBit(); // 21
            var hasMovementFlags = !packet.ReadBit("has no MovementFlags"); // 24
            guid[3] = packet.ReadBit(); // 19
            var hasSplineElev = !packet.ReadBit("Has no Spline Elevation"); // 144
            guid[2] = packet.ReadBit(); // 18
            guid[6] = packet.ReadBit(); // 22
            packet.ReadBit("bit 172"); // 172

            if (hasTrans) // 104
            {
                transportGuid[1] = packet.ReadBit(); // 57
                hasTransTime3 = packet.ReadBit("hasTransTime3"); // 100
                transportGuid[3] = packet.ReadBit(); // 59
                transportGuid[4] = packet.ReadBit(); // 60
                transportGuid[2] = packet.ReadBit(); // 58
                transportGuid[5] = packet.ReadBit(); // 61
                transportGuid[0] = packet.ReadBit(); // 56
                transportGuid[7] = packet.ReadBit(); // 63
                transportGuid[6] = packet.ReadBit(); // 62
                hasTransTime2 = packet.ReadBit("hasTransTime2"); // 92
            }

            if (hasFallData) // 140
                hasFallDirection = packet.ReadBit("Has Fall Direction"); // 136

            if (hasMovementFlags2) // 28
                packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 13); // 28

            if (hasMovementFlags) // 24
                packet.ReadBitsE<MovementFlag>("Movement Flags", 30); // 24

            packet.ResetBitReader();
            packet.ParseBitStream(guid, 1, 6, 7);

            if (Count > 0) // 152
                for (var cnt = 0; cnt < Count; cnt++)
                {
                    packet.ReadInt32("Dword 156", cnt); // 156
                }

            packet.ParseBitStream(guid, 5, 0, 3, 2, 4);

            if (hasTrans) // 104
            {
                var tpos = new Vector4();
                packet.ReadXORByte(transportGuid, 3); // 59
                packet.ReadXORByte(transportGuid, 1); // 57
                packet.ReadXORByte(transportGuid, 6); // 62
                tpos.Z = packet.ReadSingle(); // 72
                packet.ReadXORByte(transportGuid, 4); // 60
                if (hasTransTime3) // 100
                    packet.ReadUInt32("Transport Time 3"); // 96
                packet.ReadSByte("Transport Seat"); // 80
                packet.ReadXORByte(transportGuid, 7); // 63
                tpos.O = packet.ReadSingle(); // 76
                if (hasTransTime2) // 92
                    packet.ReadUInt32("Transport Time 2"); // 88
                packet.ReadXORByte(transportGuid, 5); // 61
                packet.ReadXORByte(transportGuid, 2); // 58
                tpos.X = packet.ReadSingle(); // 64
                packet.ReadXORByte(transportGuid, 0); // 56
                tpos.Y = packet.ReadSingle(); // 68
                packet.ReadUInt32("Transport Time"); // 84
                packet.WriteGuid("Transport Guid", transportGuid);
                packet.WriteLine("Transport Position: {0}", tpos);
            }

            if (hasFallData) // 140
            {
                if (hasFallDirection) // 136
                {
                    packet.ReadSingle("Fall Sin"); // 124
                    packet.ReadSingle("Fall Cos"); // 128
                    packet.ReadSingle("Horizontal Speed"); // 132
                }
                packet.ReadUInt32("Fall time"); // 116
                packet.ReadSingle("Vertical Speed"); // 120
            }

            if (hasTime)
                packet.ReadUInt32("Timestamp"); // 32

            if (hasPitch)
                packet.ReadSingle("Pitch"); // 112

            if (hasSplineElev)
                packet.ReadSingle("Spline elevation"); // 144

            if (hasO)
                pos.O = packet.ReadSingle(); // 48

            if (hasSpline) // 168
                packet.ReadInt32("Spline"); // 168

            packet.WriteGuid("Guid", guid);
            packet.WriteLine("Position: {0}", pos);
        }

        [Parser(Opcode.CMSG_MOVE_START_PITCH_DOWN)]
        public static void HandleMoveStartPitchDown(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();

            pos.Z = packet.ReadSingle(); // 44
            pos.Y = packet.ReadSingle(); // 40
            pos.X = packet.ReadSingle(); // 36
            guid[2] = packet.ReadBit(); // 18
            guid[7] = packet.ReadBit(); // 23
            guid[3] = packet.ReadBit(); // 19
            var hasO = !packet.ReadBit("Has no Orient"); // 48
            guid[5] = packet.ReadBit(); // 21
            packet.ReadBit("bit 172"); // 172
            var hasSpline = !packet.ReadBit("has no Spline"); // 168
            packet.ReadBit("bit 148"); // 148
            var hasMovementFlags = !packet.ReadBit("has no MovementFlags"); // 24
            guid[4] = packet.ReadBit(); // 20
            guid[1] = packet.ReadBit(); // 17
            var hasSplineElev = !packet.ReadBit("Has no Spline Elevation"); // 144
            var hasFallData = packet.ReadBit("Has fall data"); // 140
            packet.ReadBit("bit 149"); // 149
            var hasTrans = packet.ReadBit("Has transport"); // 104
            var hasTime = !packet.ReadBit("Has no timestamp"); // 32
            var hasMovementFlags2 = !packet.ReadBit("has no MovementFlags2"); // 28
            var Count = packet.ReadBits("Counter", 22); // 152
            guid[6] = packet.ReadBit(); // 22
            guid[0] = packet.ReadBit(); // 16
            var hasPitch = !packet.ReadBit("Has no pitch"); // 112

            if (hasTrans) // 104
            {
                hasTransTime2 = packet.ReadBit("hasTransTime2"); // 92
                transportGuid[2] = packet.ReadBit(); // 58
                transportGuid[6] = packet.ReadBit(); // 62
                transportGuid[3] = packet.ReadBit(); // 59
                transportGuid[7] = packet.ReadBit(); // 63
                transportGuid[5] = packet.ReadBit(); // 61
                transportGuid[4] = packet.ReadBit(); // 60
                hasTransTime3 = packet.ReadBit("hasTransTime3"); // 100
                transportGuid[1] = packet.ReadBit(); // 57
                transportGuid[0] = packet.ReadBit(); // 56
            }

            if (hasMovementFlags) // 24
                packet.ReadBitsE<MovementFlag>("Movement Flags", 30); // 24

            if (hasMovementFlags2) // 28
                packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 13); // 28

            if (hasFallData) // 140
                hasFallDirection = packet.ReadBit("Has Fall Direction"); // 136

            packet.ResetBitReader();
            packet.ParseBitStream(guid, 6, 3, 5, 0, 4);

            if (Count > 0) // 152
                for (var cnt = 0; cnt < Count; cnt++)
                {
                    packet.ReadInt32("Dword 156", cnt); // 156
                }

            packet.ParseBitStream(guid, 7, 2, 1);

            if (hasFallData) // 140
            {
                if (hasFallDirection) // 136
                {
                    packet.ReadSingle("Horizontal Speed"); // 132
                    packet.ReadSingle("Fall Sin"); // 124
                    packet.ReadSingle("Fall Cos"); // 128
                }
                packet.ReadUInt32("Fall time"); // 116
                packet.ReadSingle("Vertical Speed"); // 120
            }

            if (hasTrans) // 104
            {
                var tpos = new Vector4();
                tpos.O = packet.ReadSingle(); // 76
                packet.ReadXORByte(transportGuid, 4); // 60
                packet.ReadXORByte(transportGuid, 7); // 63
                tpos.Z = packet.ReadSingle(); // 72
                packet.ReadXORByte(transportGuid, 0); // 56
                packet.ReadUInt32("Transport Time"); // 84
                packet.ReadXORByte(transportGuid, 6); // 62
                packet.ReadXORByte(transportGuid, 1); // 57
                packet.ReadSByte("Transport Seat"); // 80
                packet.ReadXORByte(transportGuid, 2); // 58
                packet.ReadXORByte(transportGuid, 5); // 61
                tpos.X = packet.ReadSingle(); // 64
                if (hasTransTime3) // 100
                    packet.ReadUInt32("Transport Time 3"); // 96
                tpos.Y = packet.ReadSingle(); // 68
                packet.ReadXORByte(transportGuid, 3); // 59
                if (hasTransTime2) // 92
                    packet.ReadUInt32("Transport Time 2"); // 88
                packet.WriteGuid("Transport Guid", transportGuid);
                packet.WriteLine("Transport Position: {0}", tpos);
            }

            if (hasO)
                pos.O = packet.ReadSingle(); // 48

            if (hasTime)
                packet.ReadUInt32("Timestamp"); // 32

            if (hasSplineElev)
                packet.ReadSingle("Spline elevation"); // 144

            if (hasSpline) // 168
                packet.ReadInt32("Spline"); // 168

            if (hasPitch)
                packet.ReadSingle("Pitch"); // 112

            packet.WriteGuid("Guid", guid);
            packet.WriteLine("Position: {0}", pos);
        }

        [Parser(Opcode.CMSG_MOVE_START_PITCH_UP)]
        public static void HandleMoveStartPitchUp(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();

            pos.Y = packet.ReadSingle(); // 40
            pos.Z = packet.ReadSingle(); // 44
            pos.X = packet.ReadSingle(); // 36
            guid[0] = packet.ReadBit(); // 16
            var hasPitch = !packet.ReadBit("Has no pitch"); // 112
            guid[3] = packet.ReadBit(); // 19
            packet.ReadBit("bit 148"); // 148
            var hasO = !packet.ReadBit("Has no Orient"); // 48
            guid[5] = packet.ReadBit(); // 21
            packet.ReadBit("bit 149"); // 149
            guid[2] = packet.ReadBit(); // 18
            guid[7] = packet.ReadBit(); // 23
            guid[1] = packet.ReadBit(); // 17
            var hasFallData = packet.ReadBit("Has fall data"); // 140
            var hasMovementFlags2 = !packet.ReadBit("has no MovementFlags2"); // 28
            var hasTrans = packet.ReadBit("Has transport"); // 104
            var hasSpline = !packet.ReadBit("has no Spline"); // 168
            var hasMovementFlags = !packet.ReadBit("has no MovementFlags"); // 24
            guid[6] = packet.ReadBit(); // 22
            var Count = packet.ReadBits("Counter", 22); // 152
            var hasTime = !packet.ReadBit("Has no timestamp"); // 32
            guid[4] = packet.ReadBit(); // 20
            packet.ReadBit("bit 172"); // 172
            var hasSplineElev = !packet.ReadBit("Has no Spline Elevation"); // 144

            if (hasTrans) // 104
            {
                transportGuid[5] = packet.ReadBit(); // 61
                transportGuid[1] = packet.ReadBit(); // 57
                hasTransTime2 = packet.ReadBit("hasTransTime2"); // 92
                transportGuid[4] = packet.ReadBit(); // 60
                transportGuid[2] = packet.ReadBit(); // 58
                transportGuid[6] = packet.ReadBit(); // 62
                transportGuid[0] = packet.ReadBit(); // 56
                hasTransTime3 = packet.ReadBit("hasTransTime3"); // 100
                transportGuid[7] = packet.ReadBit(); // 63
                transportGuid[3] = packet.ReadBit(); // 59
            }

            if (hasMovementFlags2) // 28
                packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 13); // 28

            if (hasMovementFlags) // 24
                packet.ReadBitsE<MovementFlag>("Movement Flags", 30); // 24

            if (hasFallData) // 140
                hasFallDirection = packet.ReadBit("Has Fall Direction"); // 136

            packet.ResetBitReader();
            packet.ParseBitStream(guid, 6);

            if (Count > 0) // 152
                for (var cnt = 0; cnt < Count; cnt++)
                {
                    packet.ReadInt32("Dword 156", cnt); // 156
                }

            packet.ParseBitStream(guid, 0, 5, 7, 1, 3, 4, 2);

            if (hasTrans) // 104
            {
                var tpos = new Vector4();
                packet.ReadSByte("Transport Seat"); // 80
                packet.ReadXORByte(transportGuid, 3); // 59
                if (hasTransTime3) // 100
                    packet.ReadUInt32("Transport Time 3"); // 96
                packet.ReadXORByte(transportGuid, 2); // 58
                packet.ReadXORByte(transportGuid, 7); // 63
                packet.ReadXORByte(transportGuid, 0); // 56
                tpos.Y = packet.ReadSingle(); // 68
                packet.ReadXORByte(transportGuid, 4); // 60
                packet.ReadXORByte(transportGuid, 5); // 61
                if (hasTransTime2) // 92
                    packet.ReadUInt32("Transport Time 2"); // 88
                packet.ReadXORByte(transportGuid, 1); // 57
                tpos.X = packet.ReadSingle(); // 64
                tpos.O = packet.ReadSingle(); // 76
                packet.ReadXORByte(transportGuid, 6); // 62
                packet.ReadUInt32("Transport Time"); // 84
                tpos.Z = packet.ReadSingle(); // 72
                packet.WriteGuid("Transport Guid", transportGuid);
                packet.WriteLine("Transport Position: {0}", tpos);
            }

            if (hasPitch)
                packet.ReadSingle("Pitch"); // 112

            if (hasO)
                pos.O = packet.ReadSingle(); // 48

            if (hasSplineElev)
                packet.ReadSingle("Spline elevation"); // 144

            if (hasFallData) // 140
            {
                packet.ReadSingle("Vertical Speed"); // 120
                if (hasFallDirection) // 136
                {
                    packet.ReadSingle("Horizontal Speed"); // 132
                    packet.ReadSingle("Fall Cos"); // 128
                    packet.ReadSingle("Fall Sin"); // 124
                }
                packet.ReadUInt32("Fall time"); // 116
            }

            if (hasSpline) // 168
                packet.ReadInt32("Spline"); // 168

            if (hasTime)
                packet.ReadUInt32("Timestamp"); // 32

            packet.WriteGuid("Guid", guid);
            packet.WriteLine("Position: {0}", pos);
        }

        [Parser(Opcode.CMSG_MOVE_START_STRAFE_LEFT)]
        public static void HandleMoveStartStrafeLeft(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();

            pos.Y = packet.ReadSingle(); // 40
            pos.Z = packet.ReadSingle(); // 44
            pos.X = packet.ReadSingle(); // 36
            guid[0] = packet.ReadBit(); // 16
            var hasTime = !packet.ReadBit("Has no timestamp"); // 32
            guid[3] = packet.ReadBit(); // 19
            var hasMovementFlags2 = !packet.ReadBit("has no MovementFlags2"); // 28
            var hasPitch = !packet.ReadBit("Has no pitch"); // 112
            packet.ReadBit("bit 148"); // 148
            guid[2] = packet.ReadBit(); // 18
            packet.ReadBit("bit 149"); // 149
            var hasTrans = packet.ReadBit("Has transport"); // 104
            var hasFallData = packet.ReadBit("Has fall data"); // 140
            guid[5] = packet.ReadBit(); // 21
            var Count = packet.ReadBits("Counter", 22); // 152
            packet.ReadBit("bit 172"); // 172
            guid[4] = packet.ReadBit(); // 20
            var hasO = !packet.ReadBit("Has no Orient"); // 48
            var hasSplineElev = !packet.ReadBit("Has no Spline Elevation"); // 144
            guid[7] = packet.ReadBit(); // 23
            var hasSpline = !packet.ReadBit("has no Spline"); // 168
            guid[1] = packet.ReadBit(); // 17
            guid[6] = packet.ReadBit(); // 22
            var hasMovementFlags = !packet.ReadBit("has no MovementFlags"); // 24

            if (hasTrans) // 104
            {
                transportGuid[0] = packet.ReadBit(); // 56
                transportGuid[2] = packet.ReadBit(); // 58
                transportGuid[1] = packet.ReadBit(); // 57
                transportGuid[6] = packet.ReadBit(); // 62
                transportGuid[7] = packet.ReadBit(); // 63
                transportGuid[3] = packet.ReadBit(); // 59
                transportGuid[5] = packet.ReadBit(); // 61
                hasTransTime3 = packet.ReadBit("hasTransTime3"); // 100
                hasTransTime2 = packet.ReadBit("hasTransTime2"); // 92
                transportGuid[4] = packet.ReadBit(); // 60
            }

            if (hasMovementFlags) // 24
                packet.ReadBitsE<MovementFlag>("Movement Flags", 30); // 24

            if (hasFallData) // 140
                hasFallDirection = packet.ReadBit("Has Fall Direction"); // 136

            if (hasMovementFlags2) // 28
                packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 13); // 28

            packet.ResetBitReader();
            packet.ParseBitStream(guid, 0, 2);

            if (Count > 0) // 152
                for (var cnt = 0; cnt < Count; cnt++)
                {
                    packet.ReadInt32("Dword 156", cnt); // 156
                }

            packet.ParseBitStream(guid, 3, 5, 1, 7, 4, 6);

            if (hasTrans) // 104
            {
                var tpos = new Vector4();
                packet.ReadXORByte(transportGuid, 2); // 58
                tpos.Z = packet.ReadSingle(); // 72
                if (hasTransTime3) // 100
                    packet.ReadUInt32("Transport Time 3"); // 96
                packet.ReadXORByte(transportGuid, 6); // 62
                packet.ReadXORByte(transportGuid, 3); // 59
                tpos.O = packet.ReadSingle(); // 76
                packet.ReadXORByte(transportGuid, 5); // 61
                if (hasTransTime2) // 92
                    packet.ReadUInt32("Transport Time 2"); // 88
                packet.ReadXORByte(transportGuid, 1); // 57
                tpos.Y = packet.ReadSingle(); // 68
                packet.ReadXORByte(transportGuid, 4); // 60
                packet.ReadUInt32("Transport Time"); // 84
                packet.ReadSByte("Transport Seat"); // 80
                tpos.X = packet.ReadSingle(); // 64
                packet.ReadXORByte(transportGuid, 0); // 56
                packet.ReadXORByte(transportGuid, 7); // 63
                packet.WriteGuid("Transport Guid", transportGuid);
                packet.WriteLine("Transport Position: {0}", tpos);
            }

            if (hasPitch)
                packet.ReadSingle("Pitch"); // 112

            if (hasTime)
                packet.ReadUInt32("Timestamp"); // 32

            if (hasFallData) // 140
            {
                packet.ReadUInt32("Fall time"); // 116
                if (hasFallDirection) // 136
                {
                    packet.ReadSingle("Fall Sin"); // 124
                    packet.ReadSingle("Horizontal Speed"); // 132
                    packet.ReadSingle("Fall Cos"); // 128
                }
                packet.ReadSingle("Vertical Speed"); // 120
            }

            if (hasSpline) // 168
                packet.ReadInt32("Spline"); // 168

            if (hasSplineElev)
                packet.ReadSingle("Spline elevation"); // 144

            if (hasO)
                pos.O = packet.ReadSingle(); // 48

            packet.WriteGuid("Guid", guid);
            packet.WriteLine("Position: {0}", pos);
        }

        [Parser(Opcode.CMSG_MOVE_START_STRAFE_RIGHT)]
        public static void HandleMoveStartStrafeRight(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();

            pos.Y = packet.ReadSingle(); // 40
            pos.X = packet.ReadSingle(); // 36
            pos.Z = packet.ReadSingle(); // 44
            guid[0] = packet.ReadBit(); // 16
            var hasFallData = packet.ReadBit("Has fall data"); // 140
            var Count = packet.ReadBits("Counter", 22); // 152
            guid[7] = packet.ReadBit(); // 23
            guid[6] = packet.ReadBit(); // 22
            guid[4] = packet.ReadBit(); // 20
            var hasMovementFlags = !packet.ReadBit("has no MovementFlags"); // 24
            guid[5] = packet.ReadBit(); // 21
            var hasSplineElev = !packet.ReadBit("Has no Spline Elevation"); // 144
            guid[3] = packet.ReadBit(); // 19
            packet.ReadBit("bit 149"); // 149
            var hasTrans = packet.ReadBit("Has transport"); // 104
            var hasSpline = !packet.ReadBit("has no Spline"); // 168
            guid[1] = packet.ReadBit(); // 17
            packet.ReadBit("bit 172"); // 172
            guid[2] = packet.ReadBit(); // 18
            var hasPitch = !packet.ReadBit("Has no pitch"); // 112
            var hasMovementFlags2 = !packet.ReadBit("has no MovementFlags2"); // 28
            var hasO = !packet.ReadBit("Has no Orient"); // 48
            packet.ReadBit("bit 148"); // 148
            var hasTime = !packet.ReadBit("Has no timestamp"); // 32

            if (hasFallData) // 140
                hasFallDirection = packet.ReadBit("Has Fall Direction"); // 136

            if (hasTrans) // 104
            {
                transportGuid[1] = packet.ReadBit(); // 57
                transportGuid[6] = packet.ReadBit(); // 62
                transportGuid[3] = packet.ReadBit(); // 59
                transportGuid[5] = packet.ReadBit(); // 61
                transportGuid[2] = packet.ReadBit(); // 58
                transportGuid[0] = packet.ReadBit(); // 56
                transportGuid[4] = packet.ReadBit(); // 60
                hasTransTime3 = packet.ReadBit("hasTransTime3"); // 100
                transportGuid[7] = packet.ReadBit(); // 63
                hasTransTime2 = packet.ReadBit("hasTransTime2"); // 92
            }

            if (hasMovementFlags) // 24
                packet.ReadBitsE<MovementFlag>("Movement Flags", 30); // 24

            if (hasMovementFlags2) // 28
                packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 13); // 28

            packet.ResetBitReader();
            packet.ParseBitStream(guid, 6, 7, 0, 4, 1);

            if (Count > 0) // 152
                for (var cnt = 0; cnt < Count; cnt++)
                {
                    packet.ReadInt32("Dword 156", cnt); // 156
                }

            packet.ParseBitStream(guid, 2, 3, 5);

            if (hasPitch)
                packet.ReadSingle("Pitch"); // 112

            if (hasTrans) // 104
            {
                var tpos = new Vector4();
                packet.ReadXORByte(transportGuid, 1); // 57
                packet.ReadSByte("Transport Seat"); // 80
                packet.ReadXORByte(transportGuid, 3); // 59
                if (hasTransTime2) // 92
                    packet.ReadUInt32("Transport Time 2"); // 88
                packet.ReadXORByte(transportGuid, 7); // 63
                if (hasTransTime3) // 100
                    packet.ReadUInt32("Transport Time 3"); // 96
                packet.ReadXORByte(transportGuid, 5); // 61
                packet.ReadXORByte(transportGuid, 6); // 62
                packet.ReadXORByte(transportGuid, 2); // 58
                packet.ReadXORByte(transportGuid, 0); // 56
                packet.ReadUInt32("Transport Time"); // 84
                tpos.O = packet.ReadSingle(); // 76
                tpos.Y = packet.ReadSingle(); // 68
                tpos.Z = packet.ReadSingle(); // 72
                packet.ReadXORByte(transportGuid, 4); // 60
                tpos.X = packet.ReadSingle(); // 64
                packet.WriteGuid("Transport Guid", transportGuid);
                packet.WriteLine("Transport Position: {0}", tpos);
            }

            if (hasTime)
                packet.ReadUInt32("Timestamp"); // 32

            if (hasFallData) // 140
            {
                packet.ReadSingle("Vertical Speed"); // 120
                if (hasFallDirection) // 136
                {
                    packet.ReadSingle("Fall Sin"); // 124
                    packet.ReadSingle("Horizontal Speed"); // 132
                    packet.ReadSingle("Fall Cos"); // 128
                }
                packet.ReadUInt32("Fall time"); // 116
            }

            if (hasO)
                pos.O = packet.ReadSingle(); // 48

            if (hasSpline) // 168
                packet.ReadInt32("Spline"); // 168

            if (hasSplineElev)
                packet.ReadSingle("Spline elevation"); // 144

            packet.WriteGuid("Guid", guid);
            packet.WriteLine("Position: {0}", pos);
        }

        [Parser(Opcode.CMSG_MOVE_START_SWIM)]
        public static void HandleMoveStartSwim(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();

            pos.X = packet.ReadSingle(); // 36
            pos.Y = packet.ReadSingle(); // 40
            pos.Z = packet.ReadSingle(); // 44
            var hasSplineElev = !packet.ReadBit("Has no Spline Elevation"); // 144
            var hasTrans = packet.ReadBit("Has transport"); // 104
            guid[2] = packet.ReadBit(); // 18
            packet.ReadBit("bit 172"); // 172
            var hasMovementFlags2 = !packet.ReadBit("has no MovementFlags2"); // 28
            var Count = packet.ReadBits("Counter", 22); // 152
            var hasPitch = !packet.ReadBit("Has no pitch"); // 112
            var hasTime = !packet.ReadBit("Has no timestamp"); // 32
            guid[3] = packet.ReadBit(); // 19
            packet.ReadBit("bit 149"); // 149
            guid[6] = packet.ReadBit(); // 22
            guid[1] = packet.ReadBit(); // 17
            var hasFallData = packet.ReadBit("Has fall data"); // 140
            var hasSpline = !packet.ReadBit("has no Spline"); // 168
            packet.ReadBit("bit 148"); // 148
            guid[7] = packet.ReadBit(); // 23
            guid[0] = packet.ReadBit(); // 16
            var hasO = !packet.ReadBit("Has no Orient"); // 48
            guid[5] = packet.ReadBit(); // 21
            var hasMovementFlags = !packet.ReadBit("has no MovementFlags"); // 24
            guid[4] = packet.ReadBit(); // 20

            if (hasTrans) // 104
            {
                hasTransTime2 = packet.ReadBit("hasTransTime2"); // 92
                transportGuid[2] = packet.ReadBit(); // 58
                transportGuid[7] = packet.ReadBit(); // 63
                transportGuid[6] = packet.ReadBit(); // 62
                transportGuid[5] = packet.ReadBit(); // 61
                transportGuid[1] = packet.ReadBit(); // 57
                transportGuid[4] = packet.ReadBit(); // 60
                hasTransTime3 = packet.ReadBit("hasTransTime3"); // 100
                transportGuid[0] = packet.ReadBit(); // 56
                transportGuid[3] = packet.ReadBit(); // 59
            }

            if (hasMovementFlags) // 24
                packet.ReadBitsE<MovementFlag>("Movement Flags", 30); // 24

            if (hasFallData) // 140
                hasFallDirection = packet.ReadBit("Has Fall Direction"); // 136

            if (hasMovementFlags2) // 28
                packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 13); // 28

            packet.ResetBitReader();

            packet.ParseBitStream(guid, 5, 0);

            for (var cnt = 0; cnt < Count; cnt++) // 152
                packet.ReadInt32("Dword 156", cnt); // 156

            packet.ParseBitStream(guid, 7, 3, 4, 1, 6, 2);

            if (hasTrans) // 104
            {
                var tpos = new Vector4();
                packet.ReadXORByte(transportGuid, 3); // 59
                tpos.X = packet.ReadSingle(); // 64
                packet.ReadXORByte(transportGuid, 7); // 63
                packet.ReadXORByte(transportGuid, 0); // 56
                tpos.Y = packet.ReadSingle(); // 68
                packet.ReadSByte("Transport Seat"); // 80
                if (hasTransTime2) // 92
                    packet.ReadUInt32("Transport Time 2"); // 88
                packet.ReadXORByte(transportGuid, 6); // 62
                packet.ReadXORByte(transportGuid, 4); // 60
                packet.ReadXORByte(transportGuid, 1); // 57
                packet.ReadXORByte(transportGuid, 2); // 58
                tpos.Z = packet.ReadSingle(); // 72
                packet.ReadUInt32("Transport Time"); // 84
                tpos.O = packet.ReadSingle(); // 76
                if (hasTransTime3) // 100
                    packet.ReadUInt32("Transport Time 3"); // 96
                packet.ReadXORByte(transportGuid, 5); // 61
                packet.WriteGuid("Transport Guid", transportGuid);
                packet.WriteLine("Transport Position: {0}", tpos);
            }

            if (hasFallData) // 140
            {
                if (hasFallDirection) // 136
                {
                    packet.ReadSingle("Fall Sin"); // 124
                    packet.ReadSingle("Fall Cos"); // 128
                    packet.ReadSingle("Horizontal Speed"); // 132
                }
                packet.ReadUInt32("Fall time"); // 116
                packet.ReadSingle("Vertical Speed"); // 120
            }

            if (hasTime)
                packet.ReadUInt32("Timestamp"); // 32

            if (hasSplineElev)
                packet.ReadSingle("Spline elevation"); // 144

            if (hasO)
                pos.O = packet.ReadSingle(); // 48

            if (hasSpline) // 168
                packet.ReadInt32("Spline"); // 168

            if (hasPitch)
                packet.ReadSingle("Pitch"); // 112

            packet.WriteGuid("Guid", guid);
            packet.WriteLine("Position: {0}", pos);
        }

        [Parser(Opcode.CMSG_MOVE_START_TURN_LEFT)]
        public static void HandleMoveStartTurnLeft(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();

            pos.Z = packet.ReadSingle(); // 44
            pos.X = packet.ReadSingle(); // 36
            pos.Y = packet.ReadSingle(); // 40
            var hasO = !packet.ReadBit("Has no Orient"); // 48
            guid[4] = packet.ReadBit(); // 20
            guid[5] = packet.ReadBit(); // 21
            packet.ReadBit("bit 148"); // 148
            var hasTime = !packet.ReadBit("Has no timestamp"); // 32
            packet.ReadBit("bit 172"); // 172
            packet.ReadBit("bit 149"); // 149
            var hasSpline = !packet.ReadBit("has no Spline"); // 168
            guid[3] = packet.ReadBit(); // 19
            guid[1] = packet.ReadBit(); // 17
            var hasMovementFlags2 = !packet.ReadBit("has no MovementFlags2"); // 28
            var hasMovementFlags = !packet.ReadBit("has no MovementFlags"); // 24
            guid[0] = packet.ReadBit(); // 16
            guid[2] = packet.ReadBit(); // 18
            var Count = packet.ReadBits("Counter", 22); // 152
            var hasTrans = packet.ReadBit("Has transport"); // 104
            guid[7] = packet.ReadBit(); // 23
            var hasPitch = !packet.ReadBit("Has no pitch"); // 112
            var hasSplineElev = !packet.ReadBit("Has no Spline Elevation"); // 144
            var hasFallData = packet.ReadBit("Has fall data"); // 140
            guid[6] = packet.ReadBit(); // 22

            if (hasTrans) // 104
            {
                hasTransTime3 = packet.ReadBit("hasTransTime3"); // 100
                transportGuid[5] = packet.ReadBit(); // 61
                transportGuid[6] = packet.ReadBit(); // 62
                transportGuid[2] = packet.ReadBit(); // 58
                transportGuid[3] = packet.ReadBit(); // 59
                transportGuid[4] = packet.ReadBit(); // 60
                transportGuid[7] = packet.ReadBit(); // 63
                hasTransTime2 = packet.ReadBit("hasTransTime2"); // 92
                transportGuid[0] = packet.ReadBit(); // 56
                transportGuid[1] = packet.ReadBit(); // 57
            }

            if (hasMovementFlags) // 24
                packet.ReadBitsE<MovementFlag>("Movement Flags", 30); // 24

            if (hasMovementFlags2) // 28
                packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 13); // 28

            if (hasFallData) // 140
                hasFallDirection = packet.ReadBit("Has Fall Direction"); // 136

            packet.ResetBitReader();
            packet.ParseBitStream(guid, 7, 3, 6, 4, 1);

            if (Count > 0) // 152
                for (var cnt = 0; cnt < Count; cnt++)
                {
                    packet.ReadInt32("Dword 156", cnt); // 156
                }

            packet.ParseBitStream(guid, 5, 0, 2);

            if (hasFallData) // 140
            {
                packet.ReadUInt32("Fall time"); // 116
                if (hasFallDirection) // 136
                {
                    packet.ReadSingle("Horizontal Speed"); // 132
                    packet.ReadSingle("Fall Sin"); // 124
                    packet.ReadSingle("Fall Cos"); // 128
                }
                packet.ReadSingle("Vertical Speed"); // 120
            }

            if (hasPitch)
                packet.ReadSingle("Pitch"); // 112

            if (hasTrans) // 104
            {
                var tpos = new Vector4();
                tpos.Y = packet.ReadSingle(); // 68
                packet.ReadXORByte(transportGuid, 3); // 59
                tpos.X = packet.ReadSingle(); // 64
                tpos.O = packet.ReadSingle(); // 76
                packet.ReadXORByte(transportGuid, 5); // 61
                if (hasTransTime2) // 92
                    packet.ReadUInt32("Transport Time 2"); // 88
                tpos.Z = packet.ReadSingle(); // 72
                packet.ReadXORByte(transportGuid, 2); // 58
                packet.ReadXORByte(transportGuid, 1); // 57
                packet.ReadXORByte(transportGuid, 7); // 63
                packet.ReadXORByte(transportGuid, 4); // 60
                packet.ReadXORByte(transportGuid, 0); // 56
                if (hasTransTime3) // 100
                    packet.ReadUInt32("Transport Time 3"); // 96
                packet.ReadSByte("Transport Seat"); // 80
                packet.ReadXORByte(transportGuid, 6); // 62
                packet.ReadUInt32("Transport Time"); // 84
                packet.WriteGuid("Transport Guid", transportGuid);
                packet.WriteLine("Transport Position: {0}", tpos);
            }

            if (hasO)
                pos.O = packet.ReadSingle(); // 48

            if (hasSplineElev)
                packet.ReadSingle("Spline elevation"); // 144

            if (hasSpline) // 168
                packet.ReadInt32("Spline"); // 168

            if (hasTime)
                packet.ReadUInt32("Timestamp"); // 32

            packet.WriteGuid("Guid", guid);
            packet.WriteLine("Position: {0}", pos);
        }

        [Parser(Opcode.CMSG_MOVE_START_TURN_RIGHT)]
        public static void HandleMoveStartTurnRight(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();

            pos.X = packet.ReadSingle(); // 36
            pos.Z = packet.ReadSingle(); // 44
            pos.Y = packet.ReadSingle(); // 40
            packet.ReadBit("bit 148"); // 148
            packet.ReadBit("bit 172"); // 172
            guid[1] = packet.ReadBit(); // 17
            guid[0] = packet.ReadBit(); // 16
            var hasMovementFlags = !packet.ReadBit("has no MovementFlags"); // 24
            var hasFallData = packet.ReadBit("Has fall data"); // 140
            var hasPitch = !packet.ReadBit("Has no pitch"); // 112
            var hasSpline = !packet.ReadBit("has no Spline"); // 168
            var Count = packet.ReadBits("Counter", 22); // 152
            var hasSplineElev = !packet.ReadBit("Has no Spline Elevation"); // 144
            var hasMovementFlags2 = !packet.ReadBit("has no MovementFlags2"); // 28
            var hasO = !packet.ReadBit("Has no Orient"); // 48
            guid[2] = packet.ReadBit(); // 18
            var hasTime = !packet.ReadBit("Has no timestamp"); // 32
            guid[4] = packet.ReadBit(); // 20
            guid[6] = packet.ReadBit(); // 22
            guid[5] = packet.ReadBit(); // 21
            guid[3] = packet.ReadBit(); // 19
            packet.ReadBit("bit 149"); // 149
            var hasTrans = packet.ReadBit("Has transport"); // 104
            guid[7] = packet.ReadBit(); // 23

            if (hasTrans) // 104
            {
                transportGuid[2] = packet.ReadBit(); // 58
                hasTransTime2 = packet.ReadBit("hasTransTime2"); // 92
                transportGuid[6] = packet.ReadBit(); // 62
                transportGuid[5] = packet.ReadBit(); // 61
                transportGuid[3] = packet.ReadBit(); // 59
                transportGuid[7] = packet.ReadBit(); // 63
                transportGuid[4] = packet.ReadBit(); // 60
                hasTransTime3 = packet.ReadBit("hasTransTime3"); // 100
                transportGuid[0] = packet.ReadBit(); // 56
                transportGuid[1] = packet.ReadBit(); // 57
            }

            if (hasMovementFlags) // 24
                packet.ReadBitsE<MovementFlag>("Movement Flags", 30); // 24

            if (hasMovementFlags2) // 28
                packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 13); // 28

            if (hasFallData) // 140
                hasFallDirection = packet.ReadBit("Has Fall Direction"); // 136

            packet.ResetBitReader();

            packet.ParseBitStream(guid, 5, 1, 3, 0, 4, 2, 6);

            for (var cnt = 0; cnt < Count; cnt++) // 152
                packet.ReadInt32("Dword 156", cnt); // 156

            packet.ParseBitStream(guid, 7);

            if (hasFallData) // 140
            {
                if (hasFallDirection) // 136
                {
                    packet.ReadSingle("Fall Cos"); // 128
                    packet.ReadSingle("Horizontal Speed"); // 132
                    packet.ReadSingle("Fall Sin"); // 124
                }
                packet.ReadSingle("Vertical Speed"); // 120
                packet.ReadUInt32("Fall time"); // 116
            }

            if (hasPitch)
                packet.ReadSingle("Pitch"); // 112

            if (hasTrans) // 104
            {
                var tpos = new Vector4();
                if (hasTransTime3) // 100
                    packet.ReadUInt32("Transport Time 3"); // 96
                packet.ReadXORByte(transportGuid, 3); // 59
                if (hasTransTime2) // 92
                    packet.ReadUInt32("Transport Time 2"); // 88
                packet.ReadXORByte(transportGuid, 7); // 63
                packet.ReadXORByte(transportGuid, 1); // 57
                tpos.X = packet.ReadSingle(); // 64
                packet.ReadSByte("Transport Seat"); // 80
                packet.ReadXORByte(transportGuid, 5); // 61
                packet.ReadXORByte(transportGuid, 4); // 60
                packet.ReadXORByte(transportGuid, 2); // 58
                packet.ReadXORByte(transportGuid, 0); // 56
                tpos.Z = packet.ReadSingle(); // 72
                packet.ReadUInt32("Transport Time"); // 84
                tpos.Y = packet.ReadSingle(); // 68
                packet.ReadXORByte(transportGuid, 6); // 62
                tpos.O = packet.ReadSingle(); // 76
                packet.WriteGuid("Transport Guid", transportGuid);
                packet.WriteLine("Transport Position: {0}", tpos);
            }

            if (hasO)
                pos.O = packet.ReadSingle(); // 48

            if (hasTime)
                packet.ReadUInt32("Timestamp"); // 32

            if (hasSplineElev)
                packet.ReadSingle("Spline elevation"); // 144

            if (hasSpline) // 168
                packet.ReadInt32("Spline"); // 168

            packet.WriteGuid("Guid", guid);
            packet.WriteLine("Position: {0}", pos);
        }

        [Parser(Opcode.CMSG_MOVE_STOP)]
        public static void HandleMoveStop(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();

            pos.X = packet.ReadSingle(); // 36
            pos.Y = packet.ReadSingle(); // 40
            pos.Z = packet.ReadSingle(); // 44
            guid[5] = packet.ReadBit(); // 21
            guid[2] = packet.ReadBit(); // 18
            var hasFallData = packet.ReadBit("Has fall data"); // 140
            guid[0] = packet.ReadBit(); // 16
            packet.ReadBit("bit 172"); // 172
            packet.ReadBit("bit 148"); // 148
            var hasSpline = !packet.ReadBit("has no Spline"); // 168
            guid[1] = packet.ReadBit(); // 17
            var Count = packet.ReadBits("Counter", 22); // 152
            var hasPitch = !packet.ReadBit("Has no pitch"); // 112
            guid[3] = packet.ReadBit(); // 19
            guid[4] = packet.ReadBit(); // 20
            var hasTrans = packet.ReadBit("Has transport"); // 104
            packet.ReadBit("bit 149"); // 149
            guid[6] = packet.ReadBit(); // 22
            var hasMovementFlags = !packet.ReadBit("has no MovementFlags"); // 24
            var hasTime = !packet.ReadBit("Has no timestamp"); // 32
            var hasMovementFlags2 = !packet.ReadBit("has no MovementFlags2"); // 28
            var hasO = !packet.ReadBit("Has no Orient"); // 48
            var hasSplineElev = !packet.ReadBit("Has no Spline Elevation"); // 144
            guid[7] = packet.ReadBit(); // 23

            if (hasTrans) // 104
            {
                hasTransTime2 = packet.ReadBit("hasTransTime2"); // 92
                transportGuid[7] = packet.ReadBit(); // 63
                transportGuid[4] = packet.ReadBit(); // 60
                transportGuid[1] = packet.ReadBit(); // 57
                transportGuid[0] = packet.ReadBit(); // 56
                transportGuid[5] = packet.ReadBit(); // 61
                transportGuid[2] = packet.ReadBit(); // 58
                transportGuid[3] = packet.ReadBit(); // 59
                hasTransTime3 = packet.ReadBit("hasTransTime3"); // 100
                transportGuid[6] = packet.ReadBit(); // 62
            }

            if (hasFallData) // 140
                hasFallDirection = packet.ReadBit("Has Fall Direction"); // 136

            if (hasMovementFlags2) // 28
                packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 13); // 28

            if (hasMovementFlags) // 24
                packet.ReadBitsE<MovementFlag>("Movement Flags", 30); // 24

            packet.ResetBitReader();

            packet.ParseBitStream(guid, 0, 3);

            if (Count > 0) // 152
                for (var cnt = 0; cnt < Count; cnt++)
                {
                    packet.ReadInt32("Dword 156", cnt); // 156
                }

            packet.ParseBitStream(guid, 6, 1, 4, 2, 5, 7);

            if (hasO)
                pos.O = packet.ReadSingle(); // 48

            if (hasFallData) // 140
            {
                packet.ReadSingle("Vertical Speed"); // 120
                if (hasFallDirection) // 136
                {
                    packet.ReadSingle("Horizontal Speed"); // 132
                    packet.ReadSingle("Fall Cos"); // 128
                    packet.ReadSingle("Fall Sin"); // 124
                }
                packet.ReadUInt32("Fall time"); // 116
            }

            if (hasSplineElev)
                packet.ReadSingle("Spline elevation"); // 144

            if (hasTrans) // 104
            {
                var tpos = new Vector4();
                tpos.X = packet.ReadSingle(); // 64
                packet.ReadUInt32("Transport Time"); // 84
                packet.ReadXORByte(transportGuid, 3); // 59
                tpos.O = packet.ReadSingle(); // 76
                tpos.Y = packet.ReadSingle(); // 68
                packet.ReadXORByte(transportGuid, 2); // 58
                packet.ReadXORByte(transportGuid, 6); // 62
                packet.ReadXORByte(transportGuid, 7); // 63
                packet.ReadXORByte(transportGuid, 1); // 57
                packet.ReadXORByte(transportGuid, 4); // 60
                if (hasTransTime3) // 100
                    packet.ReadUInt32("Transport Time 3"); // 96
                packet.ReadXORByte(transportGuid, 0); // 56
                packet.ReadSByte("Transport Seat"); // 80
                tpos.Z = packet.ReadSingle(); // 72
                packet.ReadXORByte(transportGuid, 5); // 61
                if (hasTransTime2) // 92
                    packet.ReadUInt32("Transport Time 2"); // 88
                packet.WriteGuid("Transport Guid", transportGuid);
                packet.WriteLine("Transport Position: {0}", tpos);
            }

            if (hasSpline) // 168
                packet.ReadInt32("Spline"); // 168

            if (hasPitch)
                packet.ReadSingle("Pitch"); // 112

            if (hasTime)
                packet.ReadUInt32("Timestamp"); // 32

            packet.WriteGuid("Guid", guid);
            packet.WriteLine("Position: {0}", pos);
        }

        [Parser(Opcode.CMSG_MOVE_STOP_ASCEND)]
        public static void HandleMoveStopAscend(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();

            pos.Z = packet.ReadSingle(); // 44
            pos.X = packet.ReadSingle(); // 36
            pos.Y = packet.ReadSingle(); // 40
            var hasO = !packet.ReadBit("Has no Orient"); // 48
            guid[0] = packet.ReadBit(); // 16
            guid[3] = packet.ReadBit(); // 19
            guid[7] = packet.ReadBit(); // 23
            guid[2] = packet.ReadBit(); // 18
            guid[6] = packet.ReadBit(); // 22
            var hasMovementFlags2 = !packet.ReadBit("has no MovementFlags2"); // 28
            var hasTime = !packet.ReadBit("Has no timestamp"); // 32
            var hasSpline = !packet.ReadBit("has no Spline"); // 168
            var hasTrans = packet.ReadBit("Has transport"); // 104
            var hasPitch = !packet.ReadBit("Has no pitch"); // 112
            packet.ReadBit("bit 148"); // 148
            packet.ReadBit("bit 172"); // 172
            guid[4] = packet.ReadBit(); // 20
            packet.ReadBit("bit 149"); // 149
            guid[5] = packet.ReadBit(); // 21
            var Count = packet.ReadBits("Counter", 22); // 152
            var hasFallData = packet.ReadBit("Has fall data"); // 140
            var hasMovementFlags = !packet.ReadBit("has no MovementFlags"); // 24
            guid[1] = packet.ReadBit(); // 17
            var hasSplineElev = !packet.ReadBit("Has no Spline Elevation"); // 144

            if (hasTrans) // 104
            {
                hasTransTime2 = packet.ReadBit("hasTransTime2"); // 92
                transportGuid[0] = packet.ReadBit(); // 56
                transportGuid[5] = packet.ReadBit(); // 61
                transportGuid[4] = packet.ReadBit(); // 60
                transportGuid[6] = packet.ReadBit(); // 62
                transportGuid[2] = packet.ReadBit(); // 58
                transportGuid[1] = packet.ReadBit(); // 57
                hasTransTime3 = packet.ReadBit("hasTransTime3"); // 100
                transportGuid[3] = packet.ReadBit(); // 59
                transportGuid[7] = packet.ReadBit(); // 63
            }

            if (hasMovementFlags2) // 28
                packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 13); // 28

            if (hasMovementFlags) // 24
                packet.ReadBitsE<MovementFlag>("Movement Flags", 30); // 24

            if (hasFallData) // 140
                hasFallDirection = packet.ReadBit("Has Fall Direction"); // 136

            packet.ResetBitReader();

            packet.ParseBitStream(guid, 0);

            for (var cnt = 0; cnt < Count; cnt++) // 152
                packet.ReadInt32("Dword 156", cnt); // 156

            packet.ParseBitStream(guid, 4, 5, 1, 7, 6, 3, 2);

            if (hasTrans) // 104
            {
                var tpos = new Vector4();
                packet.ReadXORByte(transportGuid, 5); // 61
                tpos.Y = packet.ReadSingle(); // 68
                packet.ReadXORByte(transportGuid, 4); // 60
                packet.ReadXORByte(transportGuid, 7); // 63
                packet.ReadXORByte(transportGuid, 1); // 57
                packet.ReadXORByte(transportGuid, 3); // 59
                if (hasTransTime2) // 92
                    packet.ReadUInt32("Transport Time 2"); // 88
                tpos.X = packet.ReadSingle(); // 64
                tpos.O = packet.ReadSingle(); // 76
                packet.ReadXORByte(transportGuid, 0); // 56
                packet.ReadXORByte(transportGuid, 2); // 58
                tpos.Z = packet.ReadSingle(); // 72
                if (hasTransTime3) // 100
                    packet.ReadUInt32("Transport Time 3"); // 96
                packet.ReadUInt32("Transport Time"); // 84
                packet.ReadSByte("Transport Seat"); // 80
                packet.ReadXORByte(transportGuid, 6); // 62
                packet.WriteGuid("Transport Guid", transportGuid);
                packet.WriteLine("Transport Position: {0}", tpos);
            }

            if (hasFallData) // 140
            {
                if (hasFallDirection) // 136
                {
                    packet.ReadSingle("Fall Sin"); // 124
                    packet.ReadSingle("Horizontal Speed"); // 132
                    packet.ReadSingle("Fall Cos"); // 128
                }
                packet.ReadUInt32("Fall time"); // 116
                packet.ReadSingle("Vertical Speed"); // 120
            }

            if (hasTime)
                packet.ReadUInt32("Timestamp"); // 32

            if (hasSplineElev)
                packet.ReadSingle("Spline elevation"); // 144

            if (hasPitch)
                packet.ReadSingle("Pitch"); // 112

            if (hasSpline) // 168
                packet.ReadInt32("Spline"); // 168

            if (hasO)
                pos.O = packet.ReadSingle(); // 48

            packet.WriteGuid("Guid", guid);
            packet.WriteLine("Position: {0}", pos);
        }

        [Parser(Opcode.CMSG_MOVE_STOP_PITCH)]
        public static void HandleMoveStopPitch(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();

            pos.Y = packet.ReadSingle(); // 40
            pos.X = packet.ReadSingle(); // 36
            pos.Z = packet.ReadSingle(); // 44
            var hasFallData = packet.ReadBit("Has fall data"); // 140
            var hasSplineElev = !packet.ReadBit("Has no Spline Elevation"); // 144
            guid[0] = packet.ReadBit(); // 16
            var hasTime = !packet.ReadBit("Has no timestamp"); // 32
            packet.ReadBit("bit 148"); // 148
            guid[2] = packet.ReadBit(); // 18
            var Count = packet.ReadBits("Counter", 22); // 152
            var hasO = !packet.ReadBit("Has no Orient"); // 48
            var hasMovementFlags = !packet.ReadBit("has no MovementFlags"); // 24
            var hasPitch = !packet.ReadBit("Has no pitch"); // 112
            guid[3] = packet.ReadBit(); // 19
            guid[7] = packet.ReadBit(); // 23
            guid[5] = packet.ReadBit(); // 21
            var hasMovementFlags2 = !packet.ReadBit("has no MovementFlags2"); // 28
            packet.ReadBit("bit 172"); // 172
            var hasSpline = !packet.ReadBit("has no Spline"); // 168
            var hasTrans = packet.ReadBit("Has transport"); // 104
            guid[6] = packet.ReadBit(); // 22
            guid[4] = packet.ReadBit(); // 20
            packet.ReadBit("bit 149"); // 149
            guid[1] = packet.ReadBit(); // 17

            if (hasTrans) // 104
            {
                transportGuid[4] = packet.ReadBit(); // 60
                transportGuid[5] = packet.ReadBit(); // 61
                hasTransTime3 = packet.ReadBit("hasTransTime3"); // 100
                transportGuid[6] = packet.ReadBit(); // 62
                transportGuid[3] = packet.ReadBit(); // 59
                transportGuid[0] = packet.ReadBit(); // 56
                transportGuid[7] = packet.ReadBit(); // 63
                transportGuid[1] = packet.ReadBit(); // 57
                hasTransTime2 = packet.ReadBit("hasTransTime2"); // 92
                transportGuid[2] = packet.ReadBit(); // 58
            }

            if (hasMovementFlags2) // 28
                packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 13); // 28

            if (hasMovementFlags) // 24
                packet.ReadBitsE<MovementFlag>("Movement Flags", 30); // 24

            if (hasFallData) // 140
                hasFallDirection = packet.ReadBit("Has Fall Direction"); // 136

            packet.ResetBitReader();

            packet.ParseBitStream(guid, 0, 6, 7, 1);

            for (var cnt = 0; cnt < Count; cnt++) // 152
                packet.ReadInt32("Dword 156", cnt); // 156

            packet.ParseBitStream(guid, 5, 3, 4, 2);

            if (hasFallData) // 140
            {
                if (hasFallDirection) // 136
                {
                    packet.ReadSingle("Fall Cos"); // 128
                    packet.ReadSingle("Horizontal Speed"); // 132
                    packet.ReadSingle("Fall Sin"); // 124
                }
                packet.ReadUInt32("Fall time"); // 116
                packet.ReadSingle("Vertical Speed"); // 120
            }

            if (hasTrans) // 104
            {
                var tpos = new Vector4();
                packet.ReadXORByte(transportGuid, 1); // 57
                tpos.X = packet.ReadSingle(); // 64
                tpos.Z = packet.ReadSingle(); // 72
                tpos.Y = packet.ReadSingle(); // 68
                packet.ReadXORByte(transportGuid, 5); // 61
                packet.ReadXORByte(transportGuid, 0); // 56
                if (hasTransTime2) // 92
                    packet.ReadUInt32("Transport Time 2"); // 88
                packet.ReadXORByte(transportGuid, 6); // 62
                packet.ReadXORByte(transportGuid, 2); // 58
                packet.ReadXORByte(transportGuid, 3); // 59
                packet.ReadSByte("Transport Seat"); // 80
                if (hasTransTime3) // 100
                    packet.ReadUInt32("Transport Time 3"); // 96
                packet.ReadUInt32("Transport Time"); // 84
                packet.ReadXORByte(transportGuid, 4); // 60
                packet.ReadXORByte(transportGuid, 7); // 63
                tpos.O = packet.ReadSingle(); // 76
                packet.WriteGuid("Transport Guid", transportGuid);
                packet.WriteLine("Transport Position: {0}", tpos);
            }

            if (hasPitch)
                packet.ReadSingle("Pitch"); // 112

            if (hasTime)
                packet.ReadUInt32("Timestamp"); // 32

            if (hasSplineElev)
                packet.ReadSingle("Spline elevation"); // 144

            if (hasO)
                pos.O = packet.ReadSingle(); // 48

            if (hasSpline) // 168
                packet.ReadInt32("Spline"); // 168

            packet.WriteGuid("Guid", guid);
            packet.WriteLine("Position: {0}", pos);
        }

        [Parser(Opcode.CMSG_MOVE_STOP_STRAFE)]
        public static void HandleMoveStopStrafe(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();

            pos.Z = packet.ReadSingle(); // 44
            pos.X = packet.ReadSingle(); // 36
            pos.Y = packet.ReadSingle(); // 40
            var hasFallData = packet.ReadBit("Has fall data"); // 140
            var hasO = !packet.ReadBit("Has no Orient"); // 48
            var hasSplineElev = !packet.ReadBit("Has no Spline Elevation"); // 144
            var hasTime = !packet.ReadBit("Has no timestamp"); // 32
            var hasMovementFlags = !packet.ReadBit("has no MovementFlags"); // 24
            var hasSpline = !packet.ReadBit("has no Spline"); // 168
            guid[6] = packet.ReadBit(); // 22
            var hasTrans = packet.ReadBit("Has transport"); // 104
            packet.ReadBit("bit 172"); // 172
            var hasMovementFlags2 = !packet.ReadBit("has no MovementFlags2"); // 28
            guid[4] = packet.ReadBit(); // 20
            var hasPitch = !packet.ReadBit("Has no pitch"); // 112
            guid[5] = packet.ReadBit(); // 21
            guid[3] = packet.ReadBit(); // 19
            guid[2] = packet.ReadBit(); // 18
            var Count = packet.ReadBits("Counter", 22); // 152
            packet.ReadBit("bit 149"); // 149
            guid[7] = packet.ReadBit(); // 23
            guid[0] = packet.ReadBit(); // 16
            packet.ReadBit("bit 148"); // 148
            guid[1] = packet.ReadBit(); // 17

            if (hasTrans) // 104
            {
                transportGuid[7] = packet.ReadBit(); // 63
                hasTransTime3 = packet.ReadBit("hasTransTime3"); // 100
                transportGuid[3] = packet.ReadBit(); // 59
                transportGuid[1] = packet.ReadBit(); // 57
                transportGuid[6] = packet.ReadBit(); // 62
                hasTransTime2 = packet.ReadBit("hasTransTime2"); // 92
                transportGuid[2] = packet.ReadBit(); // 58
                transportGuid[5] = packet.ReadBit(); // 61
                transportGuid[4] = packet.ReadBit(); // 60
                transportGuid[0] = packet.ReadBit(); // 56
            }

            if (hasMovementFlags2) // 28
                packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 13); // 28

            if (hasFallData) // 140
                hasFallDirection = packet.ReadBit("Has Fall Direction"); // 136

            if (hasMovementFlags) // 24
                packet.ReadBitsE<MovementFlag>("Movement Flags", 30); // 24

            packet.ResetBitReader();

            packet.ParseBitStream(guid, 5, 3);

            if (Count > 0) // 152
                for (var cnt = 0; cnt < Count; cnt++)
                {
                    packet.ReadInt32("Dword 156", cnt); // 156
                }

            packet.ParseBitStream(guid, 2, 0, 1, 6, 4, 7);

            if (hasTrans) // 104
            {
                var tpos = new Vector4();
                packet.ReadXORByte(transportGuid, 0); // 56
                if (hasTransTime3) // 100
                    packet.ReadUInt32("Transport Time 3"); // 96
                packet.ReadXORByte(transportGuid, 1); // 57
                packet.ReadXORByte(transportGuid, 6); // 62
                packet.ReadUInt32("Transport Time"); // 84
                tpos.Y = packet.ReadSingle(); // 68
                tpos.Z = packet.ReadSingle(); // 72
                packet.ReadXORByte(transportGuid, 4); // 60
                if (hasTransTime2) // 92
                    packet.ReadUInt32("Transport Time 2"); // 88
                packet.ReadXORByte(transportGuid, 3); // 59
                packet.ReadSByte("Transport Seat"); // 80
                tpos.X = packet.ReadSingle(); // 64
                packet.ReadXORByte(transportGuid, 2); // 58
                packet.ReadXORByte(transportGuid, 7); // 63
                packet.ReadXORByte(transportGuid, 5); // 61
                tpos.O = packet.ReadSingle(); // 76
                packet.WriteGuid("Transport Guid", transportGuid);
                packet.WriteLine("Transport Position: {0}", tpos);
            }

            if (hasO)
                pos.O = packet.ReadSingle(); // 48

            if (hasSplineElev)
                packet.ReadSingle("Spline elevation"); // 144

            if (hasTime)
                packet.ReadUInt32("Timestamp"); // 32

            if (hasFallData) // 140
            {
                if (hasFallDirection) // 136
                {
                    packet.ReadSingle("Fall Sin"); // 124
                    packet.ReadSingle("Fall Cos"); // 128
                    packet.ReadSingle("Horizontal Speed"); // 132
                }
                packet.ReadUInt32("Fall time"); // 116
                packet.ReadSingle("Vertical Speed"); // 120
            }

            if (hasPitch)
                packet.ReadSingle("Pitch"); // 112

            if (hasSpline) // 168
                packet.ReadInt32("Spline"); // 168

            packet.WriteGuid("Guid", guid);
            packet.WriteLine("Position: {0}", pos);
        }

        [Parser(Opcode.CMSG_MOVE_STOP_TURN)]
        public static void HandleMoveStopTurn(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();

            pos.X = packet.ReadSingle(); // 36
            pos.Z = packet.ReadSingle(); // 44
            pos.Y = packet.ReadSingle(); // 40
            var hasTrans = packet.ReadBit("Has transport"); // 104
            var Count = packet.ReadBits("Counter", 22); // 152
            packet.ReadBit("bit 149"); // 149
            guid[4] = packet.ReadBit(); // 20
            guid[5] = packet.ReadBit(); // 21
            var hasSpline = !packet.ReadBit("has no Spline"); // 168
            guid[3] = packet.ReadBit(); // 19
            packet.ReadBit("bit 172"); // 172
            var hasFallData = packet.ReadBit("Has fall data"); // 140
            guid[0] = packet.ReadBit(); // 16
            guid[1] = packet.ReadBit(); // 17
            var hasPitch = !packet.ReadBit("Has no pitch"); // 112
            guid[6] = packet.ReadBit(); // 22
            var hasMovementFlags = !packet.ReadBit("has no MovementFlags"); // 24
            guid[2] = packet.ReadBit(); // 18
            packet.ReadBit("bit 148"); // 148
            var hasMovementFlags2 = !packet.ReadBit("has no MovementFlags2"); // 28
            var hasSplineElev = !packet.ReadBit("Has no Spline Elevation"); // 144
            var hasO = !packet.ReadBit("Has no Orient"); // 48
            guid[7] = packet.ReadBit(); // 23
            var hasTime = !packet.ReadBit("Has no timestamp"); // 32

            if (hasMovementFlags2)
                packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 13); // 28

            if (hasTrans)
            {
                transportGuid[1] = packet.ReadBit(); // 57
                hasTransTime3 = packet.ReadBit("hasTransTime3"); // 100
                hasTransTime2 = packet.ReadBit("hasTransTime2"); // 92
                transportGuid[3] = packet.ReadBit(); // 59
                transportGuid[6] = packet.ReadBit(); // 62
                transportGuid[2] = packet.ReadBit(); // 58
                transportGuid[0] = packet.ReadBit(); // 56
                transportGuid[5] = packet.ReadBit(); // 61
                transportGuid[7] = packet.ReadBit(); // 63
                transportGuid[4] = packet.ReadBit(); // 60
            }

            if (hasMovementFlags)
                packet.ReadBitsE<MovementFlag>("Movement Flags", 30); // 24

            if (hasFallData)
                hasFallDirection = packet.ReadBit("Has Fall Direction"); // 136

            packet.ResetBitReader();
            packet.ParseBitStream(guid, 2, 3, 6);

            if (Count > 0)
                for (var cnt = 0; cnt < Count; cnt++)
                {
                    packet.ReadInt32("Dword 156", cnt); // 156
                }

            packet.ParseBitStream(guid, 0, 5, 4, 7, 1);

            if (hasTrans)
            {
                var tpos = new Vector4();
                packet.ReadUInt32("Transport Time"); // 84
                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3"); // 96
                packet.ReadSByte("Transport Seat"); // 80
                tpos.Y = packet.ReadSingle(); // 68
                tpos.X = packet.ReadSingle(); // 64
                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2"); // 88
                packet.ReadXORByte(transportGuid, 4); // 60
                packet.ReadXORByte(transportGuid, 3); // 59
                tpos.O = packet.ReadSingle(); // 76
                packet.ReadXORByte(transportGuid, 0); // 56
                tpos.Z = packet.ReadSingle(); // 72
                packet.ReadXORByte(transportGuid, 6); // 62
                packet.ReadXORByte(transportGuid, 7); // 63
                packet.ReadXORByte(transportGuid, 5); // 61
                packet.ReadXORByte(transportGuid, 1); // 57
                packet.ReadXORByte(transportGuid, 2); // 58
                packet.WriteGuid("Transport Guid", transportGuid);
                packet.WriteLine("Transport Position: {0}", tpos);
            }

            if (hasO)
                pos.O = packet.ReadSingle(); // 48

            if (hasTime)
                packet.ReadUInt32("Timestamp"); // 32

            if (hasFallData)
            {
                if (hasFallDirection)
                {
                    packet.ReadSingle("Fall Sin"); // 128
                    packet.ReadSingle("Fall Cos"); // 124
                    packet.ReadSingle("Horizontal Speed"); // 132
                }
                packet.ReadSingle("Vertical Speed"); // 120
                packet.ReadUInt32("Fall time"); // 116
            }

            if (hasSpline)
                packet.ReadInt32("Spline"); // 168

            if (hasSplineElev)
                packet.ReadSingle("Spline elevation"); // 144

            if (hasPitch)
                packet.ReadSingle("Pitch"); // 112

            packet.WriteGuid("Guid", guid);
            packet.WriteLine("Position: {0}", pos);
        }

        [Parser(Opcode.CMSG_MOVE_STOP_SWIM)]
        public static void HandleMoveStopSwim(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();

            pos.X = packet.ReadSingle(); // 36
            pos.Z = packet.ReadSingle(); // 44
            pos.Y = packet.ReadSingle(); // 40
            var hasPitch = !packet.ReadBit("Has no pitch"); // 112
            guid[6] = packet.ReadBit(); // 22
            var hasO = !packet.ReadBit("Has no Orient"); // 48
            packet.ReadBit("bit 172"); // 172
            packet.ReadBit("bit 149"); // 149
            packet.ReadBit("bit 148"); // 148
            var hasSpline = !packet.ReadBit("has no Spline"); // 168
            guid[4] = packet.ReadBit(); // 20
            var hasMovementFlags2 = !packet.ReadBit("has no MovementFlags2"); // 28
            var hasTrans = packet.ReadBit("Has transport"); // 104
            var hasFallData = packet.ReadBit("Has fall data"); // 140
            guid[1] = packet.ReadBit(); // 17
            var Count = packet.ReadBits("Counter", 22); // 152
            var hasMovementFlags = !packet.ReadBit("has no MovementFlags"); // 24
            guid[7] = packet.ReadBit(); // 23
            guid[0] = packet.ReadBit(); // 16
            var hasTime = !packet.ReadBit("Has no timestamp"); // 32
            guid[3] = packet.ReadBit(); // 19
            guid[5] = packet.ReadBit(); // 21
            var hasSplineElev = !packet.ReadBit("Has no Spline Elevation"); // 144
            guid[2] = packet.ReadBit(); // 18

            if (hasTrans) // 104
            {
                transportGuid[1] = packet.ReadBit(); // 57
                hasTransTime2 = packet.ReadBit("hasTransTime2"); // 92
                transportGuid[5] = packet.ReadBit(); // 61
                transportGuid[2] = packet.ReadBit(); // 58
                transportGuid[4] = packet.ReadBit(); // 60
                transportGuid[6] = packet.ReadBit(); // 62
                transportGuid[7] = packet.ReadBit(); // 63
                transportGuid[3] = packet.ReadBit(); // 59
                hasTransTime3 = packet.ReadBit("hasTransTime3"); // 100
                transportGuid[0] = packet.ReadBit(); // 56
            }

            if (hasFallData) // 140
                hasFallDirection = packet.ReadBit("Has Fall Direction"); // 136

            if (hasMovementFlags) // 24
                packet.ReadBitsE<MovementFlag>("Movement Flags", 30); // 24

            if (hasMovementFlags2) // 28
                packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 13); // 28

            packet.ResetBitReader();

            packet.ParseBitStream(guid, 7, 6, 1, 5, 4, 3);

            for (var cnt = 0; cnt < Count; cnt++) // 152
                packet.ReadInt32("Dword 156", cnt); // 156

            packet.ParseBitStream(guid, 0, 2);

            if (hasTrans) // 104
            {
                var tpos = new Vector4();
                packet.ReadUInt32("Transport Time"); // 84
                packet.ReadXORByte(transportGuid, 1); // 57
                packet.ReadXORByte(transportGuid, 2); // 58
                tpos.Z = packet.ReadSingle(); // 72
                tpos.X = packet.ReadSingle(); // 64
                tpos.Y = packet.ReadSingle(); // 68
                packet.ReadXORByte(transportGuid, 0); // 56
                packet.ReadXORByte(transportGuid, 6); // 62
                packet.ReadXORByte(transportGuid, 3); // 59
                packet.ReadSByte("Transport Seat"); // 80
                tpos.O = packet.ReadSingle(); // 76
                packet.ReadXORByte(transportGuid, 7); // 63
                packet.ReadXORByte(transportGuid, 4); // 60
                packet.ReadXORByte(transportGuid, 5); // 61
                if (hasTransTime2) // 92
                    packet.ReadUInt32("Transport Time 2"); // 88
                if (hasTransTime3) // 100
                    packet.ReadUInt32("Transport Time 3"); // 96
                packet.WriteGuid("Transport Guid", transportGuid);
                packet.WriteLine("Transport Position: {0}", tpos);
            }

            if (hasFallData) // 140
            {
                if (hasFallDirection) // 136
                {
                    packet.ReadSingle("Fall Sin"); // 124
                    packet.ReadSingle("Horizontal Speed"); // 132
                    packet.ReadSingle("Fall Cos"); // 128
                }
                packet.ReadSingle("Vertical Speed"); // 120
                packet.ReadUInt32("Fall time"); // 116
            }

            if (hasSplineElev)
                packet.ReadSingle("Spline elevation"); // 144

            if (hasPitch)
                packet.ReadSingle("Pitch"); // 112

            if (hasTime)
                packet.ReadUInt32("Timestamp"); // 32

            if (hasSpline) // 168
                packet.ReadInt32("Spline"); // 168

            if (hasO)
                pos.O = packet.ReadSingle(); // 48

            packet.WriteGuid("Guid", guid);
            packet.WriteLine("Position: {0}", pos);
        }

        [Parser(Opcode.SMSG_ADJUST_SPLINE_DURATION)]
        public static void HandleAdjustSplineDuration(Packet packet)
        {
            ReadPlayerMovementInfo(packet, info.SAdjustSplineDuration);
        }

        [Parser(Opcode.SMSG_FLIGHT_SPLINE_SYNC)]
        public static void HandleFlightSplineSync(Packet packet)
        {
            var guid = new byte[8];

            packet.StartBitStream(guid, 6, 4, 2, 7, 1, 3, 0, 5);
            packet.ParseBitStream(guid, 2, 7, 5, 1, 4, 6, 0, 3);
            packet.ReadSingle("Duration modifier");

            packet.WriteGuid("Guid2", guid);
        }

        [Parser(Opcode.CMSG_MOVE_WATER_WALK_ACK)]
        public static void HandleWaterWalkAck(Packet packet)
        {
            ReadPlayerMovementInfo(packet, info.MovementWaterWalkAck);
        }

        [Parser(Opcode.SMSG_BIND_POINT_UPDATE)]
        public static void HandleBindPointUpdate(Packet packet)
        {
            packet.ReadVector3("Position");
            packet.ReadInt32<ZoneId>("Zone Id");
            packet.ReadInt32<MapId>("Map Id");
        }

        [Parser(Opcode.SMSG_CONTROL_UPDATE)]
        public static void HandleControlUpdate(Packet packet)
        {
            var guid = new byte[8];
            guid[2] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            packet.ReadBit("Allow Move");
            guid[0] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            guid[6] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            packet.ParseBitStream(guid, 1, 5, 7, 4, 2, 6, 3, 0);

            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_ON_MONSTER_MOVE)]
        public static void HandleOnMonsterMove(Packet packet)
        {
            var guid3 = new byte[8];
            var target = new byte[8];
            var pos = new Vector3();

            pos.Z = packet.ReadSingle(); // 24
            pos.X = packet.ReadSingle(); // 16
            packet.ReadInt32("Spline ID");
            pos.Y = packet.ReadSingle(); //20
            packet.WriteLine("Pos: {0}", pos);
            var transportPos = new Vector3
            {
                Y = packet.ReadSingle(), // 48
                Z = packet.ReadSingle(), // 52
                X = packet.ReadSingle(), // 44
            };
            packet.WriteLine("transportPos: {0}", transportPos);
            var hasAngle = !packet.ReadBit("!hasAngle");
            guid3[0] = packet.ReadBit();
            var splineType = packet.ReadBitsE<SplineType>("Spline Type", 3);
            if (splineType == SplineType.FacingTarget)
            {
                target = packet.StartBitStream(6, 4, 3, 0, 5, 7, 1, 2); // 184
            }
            var unk76 = !packet.ReadBit("!unk76");
            var unk69 = !packet.ReadBit("!unk69");
            var unk120 = -packet.ReadBit("unk120");

            var uncompressedSplineCount = packet.ReadBits("uncompressedSplineCount", 20);
            var hasSplineFlags = !packet.ReadBit("!hasSplineFlags");
            guid3[3] = packet.ReadBit();
            var unk108 = !packet.ReadBit("!unk108");
            var unk88 = !packet.ReadBit("!unk88");
            var unk109 = !packet.ReadBit("!unk109");
            var hasDutation = !packet.ReadBit("!hasduration");
            guid3[7] = packet.ReadBit();
            guid3[4] = packet.ReadBit();
            var unk72 = !packet.ReadBit("!unk72");
            guid3[5] = packet.ReadBit();
            var compressedSplineCount = packet.ReadBits("compressedSplineCount", 22);
            guid3[6] = packet.ReadBit();
            var unk112 = packet.ReadBit("!unk112") ? 0u : 1u;
            var guid2 = packet.StartBitStream(7, 1, 3, 0, 6, 4, 5, 2); // 112
            var unk176 = packet.ReadBit("unk176");
            var unk78 = 0u;
            var count140 = 0u;
            if (unk176)
            {
                unk78 = packet.ReadBits("unk78*2", 2);
                count140 = packet.ReadBits("count140", 22);
            }
            var unk56 = packet.ReadBit("unk56");
            guid3[2] = packet.ReadBit();
            guid3[1] = packet.ReadBit();
            for (var i = 0; i < compressedSplineCount; i++)
            {
                var vec = packet.ReadPackedVector3();
                vec.X += pos.X;
                vec.Y += pos.Y;
                vec.Z += pos.Z;
                packet.WriteLine("[{0}] Waypoint: {1}", i, vec); // not completed
            }
            packet.ParseBitStream(guid3, 1);
            packet.ParseBitStream(guid2, 6, 4, 1, 7, 0, 3, 5, 2);
            packet.WriteGuid("Guid2", guid2);

            for (var i = 0; i < uncompressedSplineCount; i++)
            {
                var point = new Vector3
                {
                    Y = packet.ReadSingle(), // 100
                    X = packet.ReadSingle(), // 96
                    Z = packet.ReadSingle(), // 104
                };
                packet.WriteLine("[{0}] Point: {1}", i, point);
            }

            if (unk72)
                packet.ReadInt32("unk72");

            if (splineType == SplineType.FacingTarget)
            {
                packet.ParseBitStream(target, 5, 7, 0, 4, 3, 2, 6, 1);
                packet.WriteGuid("Target", target);
            }

            packet.ParseBitStream(guid3, 5);

            if (hasAngle)
                packet.ReadSingle("Angle");

            if (unk176)
            {
                for (var i = 0; i < count140; i++)
                {
                    packet.ReadInt16("unk146", i);
                    packet.ReadInt16("unk144", i);
                }
                packet.ReadSingle("unka8h*4");
                packet.ReadInt16("unk82*2");
                packet.ReadInt16("unk86*2");
                packet.ReadSingle("unka0h*4");
            }

            if (unk76)
                packet.ReadInt32("unk76");


            if (splineType == SplineType.FacingAngle)
                packet.ReadSingle("Facing Angle");

            packet.ParseBitStream(guid3, 3);

            if (hasSplineFlags)
                packet.ReadUInt32E<SplineFlag>("Spline Flags");

            if (unk69)
                packet.ReadByte("unk69");

            packet.ParseBitStream(guid3, 6);

            if (unk109)
                packet.ReadByte("unk109");

            if (splineType == SplineType.FacingSpot)
            {
                var facingSpot = new Vector3
                {
                    X = packet.ReadSingle(),
                    Y = packet.ReadSingle(),
                    Z = packet.ReadSingle(),
                };
                packet.WriteLine("Facing spot: {0}", facingSpot);
            }

            packet.ParseBitStream(guid3, 0);

            if (unk120 != -1)
                packet.ReadByte("unk120");

            if (unk108)
                packet.ReadByte("unk108");

            packet.ParseBitStream(guid3, 7, 2);

            if (unk88)
                packet.ReadInt32("unk88");

            packet.ParseBitStream(guid3, 4);

            if (hasDutation)
                packet.ReadInt32("Spline Duration");

            packet.WriteGuid("Unit", guid3);

            var guidUnit = new WowGuid64(BitConverter.ToUInt64(guid3, 0));

            if (!Storage.Objects.IsEmpty() && Storage.Objects.ContainsKey(guidUnit))
            {
                var obj = Storage.Objects[guidUnit].Item1;
                UpdateField uf;
                if (obj.UpdateFields != null && obj.UpdateFields.TryGetValue(UpdateFields.GetUpdateField(UnitField.UNIT_FIELD_FLAGS), out uf))
                    if ((uf.UInt32Value & (uint)UnitFlags.IsInCombat) == 0) // movement could be because of aggro so ignore that
                        obj.Movement.HasWpsOrRandMov = true;
            }
        }

        [Parser(Opcode.SMSG_MOVE_KNOCK_BACK)]
        public static void HandleMoveKnockBack(Packet packet)
        {
            packet.ReadSingle("Horizontal Speed"); // 40
            packet.ReadSingle("Fall Cos"); // 32
            packet.ReadSingle("Vertical Speed"); // 24
            packet.ReadInt32("MCounter");
            packet.ReadSingle("Fall Sin"); // 28

            var guid = packet.StartBitStream(2, 0, 7, 1, 4, 6, 5, 3);
            packet.ParseBitStream(guid, 6, 0, 7, 5, 4, 3, 1, 2);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SET_ACTIVE_MOVER)]
        public static void HandleMoveSetActiveMover(Packet packet)
        {
            var guid = packet.StartBitStream(5, 1, 4, 2, 3, 7, 0, 6);
            packet.ParseBitStream(guid, 4, 6, 2, 0, 3, 7, 5, 1);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SET_COLLISION_HEIGHT)]
        public static void HandleMoveSetCollisionHeight(Packet packet)
        {
            var guid = new byte[8];

            guid[7] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            guid[1] = packet.ReadBit();

            var hasMountDisplayId = !packet.ReadBit("!hasMountDisplayId");

            guid[3] = packet.ReadBit();

            var unk16 = packet.ReadBits("unk16", 2);

            guid[2] = packet.ReadBit();
            guid[6] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            guid[4] = packet.ReadBit();

            packet.ReadSingle("Speed");
            if (hasMountDisplayId)
                packet.ReadInt32("Mount Display Id");

            packet.ParseBitStream(guid, 3, 2, 5, 6);

            packet.ReadInt32("MCounter");
            packet.ReadSingle("unk32");
            packet.ParseBitStream(guid, 7, 1, 4, 0);

            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SET_COMPOUND_STATE)]
        public static void HandleMoveSetCompoundState(Packet packet)
        {
            var guid = new byte[8];
            guid[1] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            var count = packet.ReadBits("count", 21);
            var unk92 = new bool[count];
            var unk112 = new bool[count];
            var unk120 = new bool[count];
            var unk136 = new bool[count];
            var unk168 = new bool[count];
            for (var i = 0; i < count; i++)
            {
                unk112[i] = packet.ReadBit("unk112", i); // 112
                unk136[i] = packet.ReadBit("unk136", i); // 136
                unk120[i] = packet.ReadBit("unk120", i); // 120
                if (unk136[i])
                    packet.ReadBits("unk132", 2, i); // 132
                unk92[i] = packet.ReadBit("unk92", i); // 92
                unk168[i] = packet.ReadBit("unk168", i); // 168
                if (unk168[i])
                    packet.ReadBits("unk164", 2, i);
            }
            guid[0] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            guid[6] = packet.ReadBit();

            for (var i = 0; i < count; i++)
            {
                if (unk168[i])
                {
                    packet.ReadSingle("unk160", i); // 160
                    packet.ReadSingle("unk144", i); // 144
                    packet.ReadSingle("unk148", i); // 148
                    packet.ReadInt32("unk156", i); // 156
                    packet.ReadInt32("unk140", i); // 140
                    packet.ReadSingle("unk152", i); // 152
                }
                if (unk112[i])
                {
                    packet.ReadSingle("unk108", i); // 108
                    packet.ReadSingle("unk104", i); // 104
                    packet.ReadSingle("unk100", i); // 100
                    packet.ReadSingle("unk96", i); // 96
                }
                if (unk136[i])
                {
                    packet.ReadSingle("unk128", i); // 128
                    packet.ReadSingle("unk124", i); // 124
                }
                packet.ReadInt32("unk84", i); // 84
                if (unk120[i])
                    packet.ReadInt32("unk116", i); // 116
                packet.ReadInt16("unk80", i); // 80
                if (unk92[i])
                    packet.ReadSingle("unk88", i); // 88
            }

            packet.ParseBitStream(guid, 1, 5, 4, 6, 7, 0, 2, 3);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SET_FLIGHT_SPEED)] // sub_C8A820
        public static void HandleMoveSetFlightSpeed(Packet packet)
        {
            packet.ReadSingle("Speed");
            packet.ReadInt32("MCounter");
            var guid = packet.StartBitStream(6, 5, 0, 4, 1, 7, 3, 2);
            packet.ParseBitStream(guid, 0, 7, 4, 5, 6, 2, 3, 1);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SET_HOVERING)]
        public static void HandleMoveSetHover(Packet packet)
        {
            ReadPlayerMovementInfo(packet, info.MovementSetHover);
        }

        [Parser(Opcode.SMSG_MOVE_SET_IGNORE_MOVEMENT_FORCES)]
        public static void HandleMoveSsetIgnoreMovementForces(Packet packet)
        {
            ReadPlayerMovementInfo(packet, info.MovementSetIgnoreMovementForces);
        }

        [Parser(Opcode.SMSG_MOVE_SET_RUN_SPEED)]
        public static void HandleMoveSetRunSpeed(Packet packet)
        {
            var guid = packet.StartBitStream(1, 7, 4, 2, 5, 3, 6, 0);
            packet.ReadXORByte(guid, 1);
            packet.ReadInt32("MCounter");
            packet.ParseBitStream(guid, 7, 3, 0);
            packet.ReadSingle("Speed");
            packet.ParseBitStream(guid, 2, 4, 6, 5);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SET_RUN_BACK_SPEED)] // sub_C8977A
        public static void HandleMoveSetRunBackSpeed(Packet packet)
        {
            var guid = packet.StartBitStream(7, 1, 0, 2, 4, 3, 6, 5);
            packet.ReadInt32("MCounter");
            packet.ParseBitStream(guid, 0, 3, 7, 5, 2, 4, 1);
            packet.ReadSingle("Speed");
            packet.ReadXORByte(guid, 6);
            packet.WriteGuid("Guid", guid);
        }


        [Parser(Opcode.SMSG_MOVE_SET_SWIM_SPEED)]
        public static void HandleMoveSetswimSpeed(Packet packet)
        {
            var guid = packet.StartBitStream(5, 0, 6, 3, 7, 2, 4, 1);
            packet.ReadInt32("MCounter"); // 28
            packet.ParseBitStream(guid, 1, 3);
            packet.ReadSingle("Speed"); // 24
            packet.ParseBitStream(guid, 6, 7, 0, 5, 2, 4);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SET_WALK_SPEED)] // sub_C8F849
        public static void HandleMoveSetWalkSpeed(Packet packet)
        {
            var guid = packet.StartBitStream(6, 7, 3, 1, 2, 0, 4, 5);
            packet.ParseBitStream(guid, 5, 6);
            packet.ReadInt32("MCounter");
            packet.ReadXORByte(guid, 4);
            packet.ReadSingle("Speed");
            packet.ParseBitStream(guid, 2, 3, 0, 1, 7);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SKIP_TIME)]
        public static void HandleMoveSkipTime(Packet packet)
        {
            ReadPlayerMovementInfo(packet, info.MovementSkipTime);
        }

        [Parser(Opcode.SMSG_MOVE_TELEPORT)] // C90474
        public static void HandleMoveTeleport(Packet packet)
        {
            var guid = new byte[8];
            var guid2 = new byte[8];
            var pos = new Vector4();
            guid[0] = packet.ReadBit();
            guid[6] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            var unk56 = packet.ReadBit("unk56");
            guid[4] = packet.ReadBit();

            if (unk56)
                guid2 = packet.StartBitStream(1, 3, 6, 4, 5, 2, 0, 7);

            guid[3] = packet.ReadBit();
            guid[1] = packet.ReadBit();

            var unk27 = packet.ReadBit("unk27");

            if (unk27)
            {
                packet.ReadBit("unk25");
                packet.ReadBit("unk26");

                packet.ReadByte("unk24");
            }

            if (unk56)
            {
                packet.ParseBitStream(guid2, 4, 3, 7, 1, 6, 0, 2, 5);
                packet.WriteGuid("Guid2", guid2);
            }

            packet.ParseBitStream(guid, 4, 7);
            pos.Z = packet.ReadSingle();
            pos.Y = packet.ReadSingle();
            packet.ParseBitStream(guid, 2, 3, 5);
            pos.X = packet.ReadSingle();
            packet.ReadInt32("Count");
            packet.ParseBitStream(guid, 0, 6, 1);
            pos.O = packet.ReadSingle();
            packet.WriteLine("Pos: {0}", pos);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_UNSET_IGNORE_MOVEMENT_FORCES)]
        public static void HandleMoveUnsetIgnoreMovementForces(Packet packet)
        {
            ReadPlayerMovementInfo(packet, info.MovementUnsetIgnoreMovementForces);
        }

        [Parser(Opcode.SMSG_MOVE_UPDATE_APPLY_MOVEMENT_FORCE)]
        public static void HandleMoveUpdateApplyMovementForce(Packet packet)
        {
            ReadPlayerMovementInfo(packet, info.MovementUpdateApplyMovementForce);
        }

        [Parser(Opcode.SMSG_MOVE_UPDATE_REMOVE_MOVEMENT_FORCE)]
        public static void HandleMoveUpdateRemoveMovementForce(Packet packet)
        {
            ReadPlayerMovementInfo(packet, info.MovementUpdateRemoveMovementForce);
        }

        [Parser(Opcode.SMSG_MOVE_UNSET_HOVERING)]
        public static void HandleMoveUnSetHover(Packet packet)
        {
            ReadPlayerMovementInfo(packet, info.MovementUnSetHover);
        }

        [Parser(Opcode.SMSG_MOVE_WATER_WALK)]
        public static void HandleMoveWaterWalk(Packet packet)
        {
            var guid = packet.StartBitStream(2, 0, 4, 5, 3, 7, 1, 6);
            packet.ParseBitStream(guid, 4, 7, 0, 1, 6, 2, 3, 5);
            packet.ReadInt32("MCounter");
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOUNT_SPECIAL_ANIM)]
        public static void HandleMountSpecialAnim(Packet packet)
        {
            var guid = packet.StartBitStream(5, 7, 0, 3, 2, 1, 4, 6);
            packet.ParseBitStream(guid, 7, 2, 0, 4, 5, 6, 1, 3);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_PHASE_SHIFT_CHANGE)]
        public static void HandleSetPhaseShiftChange(Packet packet)
        {
            CoreParsers.MovementHandler.ActivePhases.Clear();

            var guid = packet.StartBitStream(0, 3, 1, 4, 6, 2, 7, 5);
            packet.ParseBitStream(guid, 4, 3, 2);

            var count = packet.ReadUInt32() / 2;
            packet.AddValue("Phases count", count);
            for (var i = 0; i < count; ++i)
                packet.ReadInt16("Phase id", i); // if + Phase.dbc, if - duno atm

            packet.ParseBitStream(guid, 0, 6);

            count = packet.ReadUInt32() / 2;
            packet.AddValue("Inactive Terrain swap count", count);
            for (var i = 0; i < count; ++i)
                packet.ReadInt16<MapId>("Inactive Terrain swap", i); // Map.dbc, all possible terrainswaps

            packet.ParseBitStream(guid, 1, 7);

            count = packet.ReadUInt32() / 2;
            packet.AddValue("WorldMapArea swap count", count);
            for (var i = 0; i < count; ++i)
                packet.ReadUInt16("WorldMapArea swap", i); // WorldMapArea.dbc

            count = packet.ReadUInt32() / 2;
            packet.AddValue("Active Terrain swap count", count);
            for (var i = 0; i < count; ++i)
                packet.ReadInt16<MapId>("Active Terrain swap", i); // Map.dbc, all active terrainswaps

            packet.ParseBitStream(guid, 5);
            packet.WriteGuid("GUID", guid);

            packet.ReadUInt32("Flags");
        }

        [Parser(Opcode.SMSG_SET_PLAY_HOVER_ANIM)]
        public static void HandleSetPlayHoverAnim(Packet packet)
        {
            var guid = new byte[8];
            guid[3] = packet.ReadBit();
            guid[6] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            packet.ReadBit("unk24"); // 24
            guid[2] = packet.ReadBit();
            guid[7] = packet.ReadBit();

            packet.ParseBitStream(guid, 5, 1, 6, 2, 3, 0, 4, 7);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_SPLINE_MOVE_SET_FEATHER_FALL)]
        public static void HandleSplineMoveSetFeatherFall(Packet packet)
        {
            ReadPlayerMovementInfo(packet, info.SMovementSplineSetFeatherFall);
        }

        [Parser(Opcode.SMSG_SPLINE_MOVE_SET_FLIGHT_BACK_SPEED)]
        public static void HandleSplineMoveSetFlightBackSpeed(Packet packet)
        {
            ReadPlayerMovementInfo(packet, info.SMovementSplineSetFlightBackSpeed);
        }

        [Parser(Opcode.SMSG_SPLINE_MOVE_SET_FLIGHT_SPEED)]
        public static void HandleSplineMoveSetFlightSpeed(Packet packet)
        {
            packet.ReadSingle("Speed"); // 24
            var guid = packet.StartBitStream(1, 4, 7, 3, 2, 6, 5, 0);
            packet.ParseBitStream(guid, 5, 1, 0, 6, 2, 4, 7, 3);

            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_SPLINE_MOVE_SET_HOVER)]
        public static void HandleSplineMoveSetHover(Packet packet)
        {
            ReadPlayerMovementInfo(packet, info.SMovementSplineSetHover);
        }

        [Parser(Opcode.SMSG_SPLINE_MOVE_SET_LAND_WALK)]
        public static void HandleSplineMoveSetLandWalk(Packet packet)
        {
            ReadPlayerMovementInfo(packet, info.SMovementSplineSetLandWalk);
        }

        [Parser(Opcode.SMSG_SPLINE_MOVE_SET_NORMAL_FALL)]
        public static void HandleSplineMoveSetNormalFall(Packet packet)
        {
            ReadPlayerMovementInfo(packet, info.SMovementSplineSetNormalFall);
        }

        [Parser(Opcode.SMSG_SPLINE_MOVE_SET_PITCH_RATE)]
        public static void HandleSplineMoveSetPitchRate(Packet packet)
        {
            ReadPlayerMovementInfo(packet, info.SMovementSplineSetPitchRate);
        }

        [Parser(Opcode.SMSG_SPLINE_MOVE_SET_RUN_BACK_SPEED)]
        public static void HandleSplineMoveSetRunBackSpeed(Packet packet)
        {
            var guid = packet.StartBitStream(7, 4, 0, 3, 2, 5, 6, 1);
            packet.ParseBitStream(guid, 6, 4, 1, 5, 2, 3, 7);
            packet.ReadSingle("Speed"); // 24
            packet.ParseBitStream(guid, 0);

            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_SPLINE_MOVE_SET_RUN_MODE)]
        public static void HandleSplineMoveSetRunMode(Packet packet)
        {
            ReadPlayerMovementInfo(packet, info.SMovementSplineSetRunMode);
        }

        [Parser(Opcode.SMSG_SPLINE_MOVE_SET_RUN_SPEED)]
        public static void HandleSplineMoveSetRunSpeed(Packet packet)
        {
            ReadPlayerMovementInfo(packet, info.SMovementSplineSetRunSpeed);
        }

        [Parser(Opcode.SMSG_SPLINE_MOVE_SET_SWIM_BACK_SPEED)]
        public static void HandleSplineMoveSetSwimBackSpeed(Packet packet)
        {
            ReadPlayerMovementInfo(packet, info.MovementSplineSetSwimBackSpeed);
        }

        [Parser(Opcode.SMSG_SPLINE_MOVE_SET_SWIM_SPEED)]
        public static void HandleSplineMoveSetSwimSpeed(Packet packet)
        {
            var guid = packet.StartBitStream(5, 6, 7, 3, 4, 2, 1, 0);
            packet.ParseBitStream(guid, 4, 1, 6, 7, 3);
            packet.ReadSingle("Speed"); // 24
            packet.ParseBitStream(guid, 5, 0, 2);

            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_SPLINE_MOVE_SET_TURN_RATE)]
        public static void HandleSplineMoveSetTurnRate(Packet packet)
        {
            ReadPlayerMovementInfo(packet, info.MovementSplineSetTurnRate);
        }

        [Parser(Opcode.SMSG_SPLINE_MOVE_SET_WALK_MODE)]
        public static void HandleSplineMoveSetWalkMode(Packet packet)
        {
            ReadPlayerMovementInfo(packet, info.MovementSplineSetWalkMode);
        }

        [Parser(Opcode.SMSG_SPLINE_MOVE_SET_WALK_SPEED)]
        public static void HandleSplineMoveSetWalkSpeed(Packet packet)
        {
            var guid = packet.StartBitStream(4, 1, 7, 6, 3, 2, 5, 0);
            packet.ParseBitStream(guid, 2, 3, 1, 0, 6, 5, 4, 7);
            packet.WriteGuid("Guid", guid);
            packet.ReadSingle("Speed");
        }

        [Parser(Opcode.SMSG_SPLINE_MOVE_SET_WATER_WALK)]
        public static void HandleSplineMoveSetWaterWalk(Packet packet)
        {
            ReadPlayerMovementInfo(packet, info.MovementSplineSetWaterWalk);
        }

        [Parser(Opcode.SMSG_SPLINE_MOVE_STOP_SWIM)]
        public static void HandleSplineMoveStopSwim(Packet packet)
        {
            ReadPlayerMovementInfo(packet, info.MovementSplineStopSwim);
        }

        [Parser(Opcode.SMSG_SPLINE_MOVE_UNSET_FLYING)]
        public static void HandleSplineMoveUnsetFlying(Packet packet)
        {
            ReadPlayerMovementInfo(packet, info.MovementSplineUnsetFlying);
        }

        [Parser(Opcode.SMSG_SPLINE_MOVE_UNSET_HOVER)]
        public static void HandleSplineMoveUnsetHover(Packet packet)
        {
            ReadPlayerMovementInfo(packet, info.MovementSplineUnsetHover);
        }

        [Parser(Opcode.SMSG_TRANSFER_ABORTED)]
        public static void HandleTransferAborted(Packet packet)
        {
            var unk16 = !packet.ReadBit("!unk16"); // 16
            packet.ReadBitsE<TransferAbortReason>("TransfertAbort", 5);
            if (unk16)
                packet.ReadByte("Arg");
            packet.ReadInt32<MapId>("MapID");
        }
    }
}
