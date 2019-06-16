using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Networking.Transport;
public struct GhostDeserializerCollection : IGhostDeserializerCollection
{
#if ENABLE_UNITY_COLLECTIONS_CHECKS
    public string[] CreateSerializerNameList()
    {
        var arr = new string[]
        {
            "AsteroidGhostSerializer",
            "BombGhostSerializer",
            "BulletGhostSerializer",
            "ShipGhostSerializer",

        };
        return arr;
    }

    public int Length => 4;
#endif
    public void Initialize(World world)
    {
        var curAsteroidGhostSpawnSystem = world.GetOrCreateSystem<AsteroidGhostSpawnSystem>();
        m_AsteroidSnapshotDataNewGhostIds = curAsteroidGhostSpawnSystem.NewGhostIds;
        m_AsteroidSnapshotDataNewGhosts = curAsteroidGhostSpawnSystem.NewGhosts;
        curAsteroidGhostSpawnSystem.GhostType = 0;
        var curBombGhostSpawnSystem = world.GetOrCreateSystem<BombGhostSpawnSystem>();
        m_BombSnapshotDataNewGhostIds = curBombGhostSpawnSystem.NewGhostIds;
        m_BombSnapshotDataNewGhosts = curBombGhostSpawnSystem.NewGhosts;
        curBombGhostSpawnSystem.GhostType = 1;
        var curBulletGhostSpawnSystem = world.GetOrCreateSystem<BulletGhostSpawnSystem>();
        m_BulletSnapshotDataNewGhostIds = curBulletGhostSpawnSystem.NewGhostIds;
        m_BulletSnapshotDataNewGhosts = curBulletGhostSpawnSystem.NewGhosts;
        curBulletGhostSpawnSystem.GhostType = 2;
        var curShipGhostSpawnSystem = world.GetOrCreateSystem<ShipGhostSpawnSystem>();
        m_ShipSnapshotDataNewGhostIds = curShipGhostSpawnSystem.NewGhostIds;
        m_ShipSnapshotDataNewGhosts = curShipGhostSpawnSystem.NewGhosts;
        curShipGhostSpawnSystem.GhostType = 3;

    }

    public void BeginDeserialize(JobComponentSystem system)
    {
        m_AsteroidSnapshotDataFromEntity = system.GetBufferFromEntity<AsteroidSnapshotData>();
        m_BombSnapshotDataFromEntity = system.GetBufferFromEntity<BombSnapshotData>();
        m_BulletSnapshotDataFromEntity = system.GetBufferFromEntity<BulletSnapshotData>();
        m_ShipSnapshotDataFromEntity = system.GetBufferFromEntity<ShipSnapshotData>();

    }
    public void Deserialize(int serializer, Entity entity, uint snapshot, uint baseline, uint baseline2, uint baseline3,
        DataStreamReader reader,
        ref DataStreamReader.Context ctx, NetworkCompressionModel compressionModel)
    {
        switch (serializer)
        {
        case 0:
            GhostReceiveSystem<GhostDeserializerCollection>.InvokeDeserialize(m_AsteroidSnapshotDataFromEntity, entity, snapshot, baseline, baseline2,
                baseline3, reader, ref ctx, compressionModel);
            break;
        case 1:
            GhostReceiveSystem<GhostDeserializerCollection>.InvokeDeserialize(m_BombSnapshotDataFromEntity, entity, snapshot, baseline, baseline2,
                baseline3, reader, ref ctx, compressionModel);
            break;
        case 2:
            GhostReceiveSystem<GhostDeserializerCollection>.InvokeDeserialize(m_BulletSnapshotDataFromEntity, entity, snapshot, baseline, baseline2,
                baseline3, reader, ref ctx, compressionModel);
            break;
        case 3:
            GhostReceiveSystem<GhostDeserializerCollection>.InvokeDeserialize(m_ShipSnapshotDataFromEntity, entity, snapshot, baseline, baseline2,
                baseline3, reader, ref ctx, compressionModel);
            break;

        default:
            throw new ArgumentException("Invalid serializer type");
        }
    }
    public void Spawn(int serializer, int ghostId, uint snapshot, DataStreamReader reader,
        ref DataStreamReader.Context ctx, NetworkCompressionModel compressionModel)
    {
        switch (serializer)
        {
            case 0:
                m_AsteroidSnapshotDataNewGhostIds.Add(ghostId);
                m_AsteroidSnapshotDataNewGhosts.Add(GhostReceiveSystem<GhostDeserializerCollection>.InvokeSpawn<AsteroidSnapshotData>(snapshot, reader, ref ctx, compressionModel));
                break;
            case 1:
                m_BombSnapshotDataNewGhostIds.Add(ghostId);
                m_BombSnapshotDataNewGhosts.Add(GhostReceiveSystem<GhostDeserializerCollection>.InvokeSpawn<BombSnapshotData>(snapshot, reader, ref ctx, compressionModel));
                break;
            case 2:
                m_BulletSnapshotDataNewGhostIds.Add(ghostId);
                m_BulletSnapshotDataNewGhosts.Add(GhostReceiveSystem<GhostDeserializerCollection>.InvokeSpawn<BulletSnapshotData>(snapshot, reader, ref ctx, compressionModel));
                break;
            case 3:
                m_ShipSnapshotDataNewGhostIds.Add(ghostId);
                m_ShipSnapshotDataNewGhosts.Add(GhostReceiveSystem<GhostDeserializerCollection>.InvokeSpawn<ShipSnapshotData>(snapshot, reader, ref ctx, compressionModel));
                break;

            default:
                throw new ArgumentException("Invalid serializer type");
        }
    }

    private BufferFromEntity<AsteroidSnapshotData> m_AsteroidSnapshotDataFromEntity;
    private NativeList<int> m_AsteroidSnapshotDataNewGhostIds;
    private NativeList<AsteroidSnapshotData> m_AsteroidSnapshotDataNewGhosts;
    private BufferFromEntity<BombSnapshotData> m_BombSnapshotDataFromEntity;
    private NativeList<int> m_BombSnapshotDataNewGhostIds;
    private NativeList<BombSnapshotData> m_BombSnapshotDataNewGhosts;
    private BufferFromEntity<BulletSnapshotData> m_BulletSnapshotDataFromEntity;
    private NativeList<int> m_BulletSnapshotDataNewGhostIds;
    private NativeList<BulletSnapshotData> m_BulletSnapshotDataNewGhosts;
    private BufferFromEntity<ShipSnapshotData> m_ShipSnapshotDataFromEntity;
    private NativeList<int> m_ShipSnapshotDataNewGhostIds;
    private NativeList<ShipSnapshotData> m_ShipSnapshotDataNewGhosts;

}
public class MultiplayerSampleGhostReceiveSystem : GhostReceiveSystem<GhostDeserializerCollection>
{
}
