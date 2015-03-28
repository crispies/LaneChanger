using UnityEngine;

namespace LaneChanger
{
    public class LaneChangerSegment
    {
        public ushort[] permittedConnections;
        public ushort numPermittedConnections;

        public LaneChangerSegment()
        {
            permittedConnections = new ushort[14];
            numPermittedConnections = 0;
        }

        public bool PermittedConnectionTo(ushort segmentId)
        {
            bool ret = false;
            for (int i = 0; i < numPermittedConnections; i++)
            {
                if (segmentId == permittedConnections[i])
                {
                    ret = true;
                    break;
                }
            }

            return ret;
        }

        public void AddPermittedConnection(ushort segmentId)
        {
            ushort offset = numPermittedConnections++;
            permittedConnections[offset] = segmentId;
        }

        public void RemovePermittedConnection(ushort segmentId)
        {
            ushort lastSeg = permittedConnections[numPermittedConnections - 1];
            if (lastSeg == segmentId)
                numPermittedConnections--;
            else
            {
                for (int i = 0; i < numPermittedConnections; i++)
                {
                    if (permittedConnections[i] == segmentId)
                    {
                        permittedConnections[i] = lastSeg;
                        numPermittedConnections--;
                        break;
                    }
                }
            }
        }
    }
}
