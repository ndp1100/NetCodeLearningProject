using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Networking.Transport;
public struct GhostSerializerCollection : IGhostSerializerCollection
{
    public int FindSerializer(EntityArchetype arch)
    {
        if (m_AsteroidGhostSerializer.CanSerialize(arch))
            return 0;
        if (m_BombGhostSerializer.CanSerialize(arch))
            return 1;
        if (m_BulletGhostSerializer.CanSerialize(arch))
            return 2;
        if (m_ShipGhostSerializer.CanSerialize(arch))
            return 3;

        throw new ArgumentException("Invalid serializer type");
    }

    public void BeginSerialize(ComponentSystemBase system)
    {
        m_AsteroidGhostSerializer.BeginSerialize(system);
        m_BombGhostSerializer.BeginSerialize(system);
        m_BulletGhostSerializer.BeginSerialize(system);
        m_ShipGhostSerializer.BeginSerialize(system);

    }

    public int CalculateImportance(int serializer, ArchetypeChunk chunk)
    {
        switch (serializer)
        {
            case 0:
                return m_AsteroidGhostSerializer.CalculateImportance(chunk);
            case 1:
                return m_BombGhostSerializer.CalculateImportance(chunk);
            case 2:
                return m_BulletGhostSerializer.CalculateImportance(chunk);
            case 3:
                return m_ShipGhostSerializer.CalculateImportance(chunk);

        }

        throw new ArgumentException("Invalid serializer type");
    }

    public bool WantsPredictionDelta(int serializer)
    {
        switch (serializer)
        {
            case 0:
                return m_AsteroidGhostSerializer.WantsPredictionDelta;
            case 1:
                return m_BombGhostSerializer.WantsPredictionDelta;
            case 2:
                return m_BulletGhostSerializer.WantsPredictionDelta;
            case 3:
                return m_ShipGhostSerializer.WantsPredictionDelta;

        }

        throw new ArgumentException("Invalid serializer type");
    }

    public int GetSnapshotSize(int serializer)
    {
        switch (serializer)
        {
            case 0:
                return m_AsteroidGhostSerializer.SnapshotSize;
            case 1:
                return m_BombGhostSerializer.SnapshotSize;
            case 2:
                return m_BulletGhostSerializer.SnapshotSize;
            case 3:
                return m_ShipGhostSerializer.SnapshotSize;

        }

        throw new ArgumentException("Invalid serializer type");
    }

    public unsafe int Serialize(int serializer, ArchetypeChunk chunk, int startIndex, uint currentTick,
        Entity* currentSnapshotEntity, void* currentSnapshotData,
        GhostSystemStateComponent* ghosts, NativeArray<Entity> ghostEntities,
        NativeArray<int> baselinePerEntity, NativeList<SnapshotBaseline> availableBaselines,
        DataStreamWriter dataStream, NetworkCompressionModel compressionModel)
    {
        switch (serializer)
        {
            case 0:
            {
                return GhostSendSystem<GhostSerializerCollection>.InvokeSerialize(m_AsteroidGhostSerializer, serializer,
                    chunk, startIndex, currentTick, currentSnapshotEntity, (AsteroidSnapshotData*)currentSnapshotData,
                    ghosts, ghostEntities, baselinePerEntity, availableBaselines,
                    dataStream, compressionModel);
            }
            case 1:
            {
                return GhostSendSystem<GhostSerializerCollection>.InvokeSerialize(m_BombGhostSerializer, serializer,
                    chunk, startIndex, currentTick, currentSnapshotEntity, (BombSnapshotData*)currentSnapshotData,
                    ghosts, ghostEntities, baselinePerEntity, availableBaselines,
                    dataStream, compressionModel);
            }
            case 2:
            {
                return GhostSendSystem<GhostSerializerCollection>.InvokeSerialize(m_BulletGhostSerializer, serializer,
                    chunk, startIndex, currentTick, currentSnapshotEntity, (BulletSnapshotData*)currentSnapshotData,
                    ghosts, ghostEntities, baselinePerEntity, availableBaselines,
                    dataStream, compressionModel);
            }
            case 3:
            {
                return GhostSendSystem<GhostSerializerCollection>.InvokeSerialize(m_ShipGhostSerializer, serializer,
                    chunk, startIndex, currentTick, currentSnapshotEntity, (ShipSnapshotData*)currentSnapshotData,
                    ghosts, ghostEntities, baselinePerEntity, availableBaselines,
                    dataStream, compressionModel);
            }

            default:
                throw new ArgumentException("Invalid serializer type");
        }
    }
    private AsteroidGhostSerializer m_AsteroidGhostSerializer;
    private BombGhostSerializer m_BombGhostSerializer;
    private BulletGhostSerializer m_BulletGhostSerializer;
    private ShipGhostSerializer m_ShipGhostSerializer;

}

public class MultiplayerSampleGhostSendSystem : GhostSendSystem<GhostSerializerCollection>
{
}
