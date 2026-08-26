using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [Serializable]
    public enum Adjacency
    {
        none, up, down, left, right
    }
    [Serializable]
    public struct RoomAdjacencyPair
    {
        public Adjacency adjacency;
        public RoomManager room;
    }

    [SerializeField]
    private RoomData data;

    [field: SerializeField]
    public List<RoomAdjacencyPair> adjacentRooms { get; private set; }

    public void AddAdjacentRoom(Adjacency adjacency, RoomManager room) {
        adjacentRooms.Add(new() { adjacency = adjacency, room = room });
    }
}
